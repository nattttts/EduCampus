using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace EduCampus
{
    public partial class CourseResults : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Protect page (must login first)
            if (Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("AccessDenied.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadSession();
                pnlSearchResult.Visible = false;

                // Add default selection option
                ddlCourse.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Course --", ""));
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AcademicResults.aspx");
        }

        // Load session in the dropdown
        private void LoadSession()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Load available sessions that have recorded course results
                string query = @"
                    SELECT DISTINCT co.Session
                    FROM CourseMarks cm

                    INNER JOIN EnrollmentDetails ed
                        ON ed.DetailID = cm.DetailID

                    INNER JOIN CourseOfferings co
                        ON ed.OfferingID = co.OfferingID

                    ORDER BY co.Session";

                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                ddlSession.DataSource = dt;
                ddlSession.DataTextField = "Session";
                ddlSession.DataValueField = "Session";
                ddlSession.DataBind();

                // Add default selection option
                ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Session --", ""));
            }
        }

        // Load course in the dropdown
        private void LoadCourses()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Load courses for the selected session
                // Only courses that have existing student marks are displayed
                string query = @"
                    SELECT DISTINCT c.CourseID, c.CourseCode + ' ' + c.CourseName AS CourseDisplay
                    FROM CourseMarks cm

                    INNER JOIN EnrollmentDetails ed
                        ON cm.DetailID = ed.DetailID
                
                    INNER JOIN CourseOfferings co
                        ON ed.OfferingID = co.OfferingID

                    INNER JOIN Courses c
                        ON co.CourseID = c.CourseID

                    WHERE co.Session = @Session
                    ORDER BY CourseDisplay";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                ddlCourse.DataSource = dt;
                ddlCourse.DataTextField = "CourseDisplay";
                ddlCourse.DataValueField = "CourseID";
                ddlCourse.DataBind();

                // Add default selection option
                ddlCourse.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Course --", ""));
            }
        }

        private void LoadResults()
        {
            LoadResultSummary();

            LoadResultsRecords();
        }

        // Get result summary
        private void LoadResultSummary()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT COUNT(*) AS Total,
                           AVG(cm.FinalMark) AS AverageMark,
                           SUM(CASE WHEN cm.FinalGrade != 'F' THEN 1 ELSE 0 END) AS TotalPass,
                           SUM(CASE WHEN cm.FinalGrade = 'F' THEN 1 ELSE 0 END) AS TotalFail

                    FROM CourseMarks cm

                    INNER JOIN EnrollmentDetails ed 
                        ON cm.DetailID = ed.DetailID

                    INNER JOIN CourseOfferings co 
                        ON ed.OfferingID = co.OfferingID

                    WHERE co.Session = @Session
                    AND co.CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);
                cmd.Parameters.AddWithValue("@CourseID", ddlCourse.SelectedValue);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                // Populate result summary values to UI
                if (dt.Rows.Count > 0)
                {
                    int total = Convert.ToInt32(dt.Rows[0]["Total"]);

                    double average = Convert.ToDouble(dt.Rows[0]["AverageMark"]);
                    int pass = Convert.ToInt32(dt.Rows[0]["TotalPass"]);
                    int fail = Convert.ToInt32(dt.Rows[0]["TotalFail"]);

                    lblAverageMark.Text = average.ToString("0.0");
                    lblPass.Text = pass.ToString();
                    lblFail.Text = fail.ToString();

                    // Calculate passing rate
                    double passingRate;

                    if (total == 0)
                    {
                        passingRate = 0;
                    }
                    else
                    {
                        passingRate = (pass * 100.0) / total;
                    }

                    lblPassingRate.Text = passingRate.ToString("0.0") + "%";
                }
            }
        }

        // Get student academic results
        private void LoadResultsRecords()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Get results records based on selected session and course
                string query = @"
                    SELECT 
                        u.FullName,
                        em.StudentID,
                        cm.AssignmentMark, 
                        cm.QuizMark, 
                        cm.MidTestMark, 
                        cm.FinalExamMark, 
                        cm.FinalMark, 
                        cm.FinalGrade

                    FROM CourseMarks cm

                    INNER JOIN EnrollmentDetails ed
                        ON cm.DetailID = ed.DetailID

                    INNER JOIN EnrollmentMaster em
                        ON ed.EnrolmentID = em.EnrolmentID

                    INNER JOIN CourseOfferings co
                        ON ed.OfferingID = co.OfferingID

                    INNER JOIN Students s 
                        ON em.StudentID = s.StudentID

                    INNER JOIN Users u
                        ON s.UserID = u.UserID

                    WHERE co.Session = @Session 
                    AND co.CourseID = @Course";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Pass selected session and course to SQL query
                cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);
                cmd.Parameters.AddWithValue("@Course", ddlCourse.SelectedValue);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                // Fill data table with query result
                sda.Fill(dt);

                // Bind data to GrindView
                gvResult.DataSource = dt;
                gvResult.DataBind();

                // Store data for PDF download
                ViewState["ResultsData"] = dt;
            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hide results when filter changes
            pnlSearchResult.Visible = false;

            // If no session is selected, reset course dropdown and stop loading
            if (ddlSession.SelectedValue == "")
            {
                ddlCourse.Items.Clear();
                ddlCourse.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Course --", ""));
                return;
            }

            LoadCourses();
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hide results when filter changes
            pnlSearchResult.Visible = false;
        }

        // Search student academic results
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Ensure required filters are selected before executing search
            if (ddlSession.SelectedValue == "" || ddlCourse.SelectedValue == "")
                return;

            LoadResults();

            // Show result panel
            pnlSearchResult.Visible = true;
        }

        // Download results report as PDF
        protected void btnPDF_Click(object sender, EventArgs e)
        {
            // Stop PDF generation if no results data is available
            DataTable dt = ViewState["ResultsData"] as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return;

            // Create PDF document
            Document pdfDoc = new Document(PageSize.A4);
            MemoryStream ms = new MemoryStream();

            PdfWriter.GetInstance(pdfDoc, ms);
            pdfDoc.Open();

            pdfDoc.Add(new Paragraph("Academic Results Report"));
            pdfDoc.Add(new Paragraph("Generated On: " + DateTime.Now.ToString()));
            pdfDoc.Add(new Paragraph(" "));

            // Add selected session and course filter to the report
            pdfDoc.Add(new Paragraph("Session: " + ddlSession.SelectedItem.Text));
            pdfDoc.Add(new Paragraph("Course: " + ddlCourse.SelectedItem.Text));
            pdfDoc.Add(new Paragraph(" "));

            pdfDoc.Add(new Paragraph("Results Summary"));
            pdfDoc.Add(new Paragraph("Average Mark: " + lblAverageMark.Text));
            pdfDoc.Add(new Paragraph("Total Pass: " + lblPass.Text));
            pdfDoc.Add(new Paragraph("Total Fail: " + lblFail.Text));
            pdfDoc.Add(new Paragraph("Passing Rate: " + lblPassingRate.Text));
            pdfDoc.Add(new Paragraph(" "));

            // Create PDF table
            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.WidthPercentage = 100;

            // Set the columns width
            float[] widths = { 2f, 1.5f, 1.5f, 1f, 1.5f, 1.5f, 1.5f, 1f };

            table.SetWidths(widths);

            // Customize column headers
            foreach (DataColumn col in dt.Columns)
            {
                string header = col.ColumnName;

                if (header == "FullName")
                    header = "Student Name";

                else if (header == "StudentID")
                    header = "Student ID";

                else if (header == "AssignmentMark")
                    header = "Assignment";

                else if (header == "QuizMark")
                    header = "Quiz";

                else if (header == "MidTestMark")
                    header = "Mid Test";

                else if (header == "FinalExamMark")
                    header = "Final Exam";

                else if (header == "FinalMark")
                    header = "Final Mark";

                else if (header == "FinalGrade")
                    header = "Grade";

                table.AddCell(new Phrase(header));
            }

            // Add result records to PDF
            foreach (DataRow row in dt.Rows)
            {
                foreach (var cell in row.ItemArray)
                {
                    if (cell == DBNull.Value)
                    {
                        table.AddCell("-");
                    }

                    else
                    {
                        table.AddCell(cell.ToString());
                    }
                }
            }

            pdfDoc.Add(table);

            pdfDoc.Close();

            // Send PDF file to browser
            Response.ContentType = "application/pdf";

            Response.AddHeader("content-disposition", "attachment;filename=CourseResults.pdf");

            Response.BinaryWrite(ms.ToArray());

            Response.End();
        }
    }
}