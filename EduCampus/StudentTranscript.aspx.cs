using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class StudentTranscript : System.Web.UI.Page
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
                LoadStudents();
                LoadSemester();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AcademicResults.aspx");
        }

        private void LoadStudents()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT DISTINCT
                        s.StudentID, 
                        u.FullName, 
                        s.StudentID + ' ' + u.FullName AS StudentDisplay

                    FROM CourseMarks cm
                    
                    INNER JOIN EnrollmentDetails ed 
                        ON cm.DetailID = ed.DetailID

                    INNER JOIN EnrollmentMaster em 
                        ON ed.EnrolmentID = em.EnrolmentID

                    INNER JOIN Students s 
                        ON em.StudentID = s.StudentID

                    INNER JOIN Users u 
                        ON s.UserID = u.UserID

                    ORDER BY u.FullName";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlStudent.DataSource = dt;
                ddlStudent.DataTextField = "StudentDisplay";
                ddlStudent.DataValueField = "StudentID";
                ddlStudent.DataBind();

                ddlStudent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Student --", ""));
            }
        }

        private void LoadSemester()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT DISTINCT Semester
                    FROM EnrollmentMaster
                    ORDER BY Semester";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlSemester.DataSource = dt;
                ddlSemester.DataTextField = "Semester";
                ddlSemester.DataValueField = "Semester";
                ddlSemester.DataBind();

                ddlSemester.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Semester --", ""));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Ensure required filters are selected before executing search
            if (ddlStudent.SelectedValue == "" || ddlSemester.SelectedValue == "")
                return;

            gvGradeReport.DataSource = null;
            gvGradeReport.DataBind();

            // Reset UI
            pnlReport.Visible = false;

            LoadStudentInfo();
            LoadEachCourseResult();
            CalculateGPA();
            CalculateCGPA();

            lblDate.Text = DateTime.Now.ToString("dd MMMM yyyy");

            pnlReport.Visible = true;
        }

        private void LoadStudentInfo()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        u.FullName, 
                        s.StudentID, 
                        p.ProgrammeName,
                        em.Session

                    FROM Students s

                    INNER JOIN Users u 
                        ON s.UserID = u.UserID

                    INNER JOIN Programmes p 
                        ON s.ProgrammeID = p.ProgrammeID

                    INNER JOIN EnrollmentMaster em
                        ON s.StudentID = em.StudentID

                    WHERE s.StudentID = @StudentID
                    AND em.Semester = @Semester";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", ddlStudent.SelectedValue);
                cmd.Parameters.AddWithValue("@Semester", ddlSemester.SelectedValue);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblStudentName.Text = dr["FullName"].ToString();
                    lblStudentID.Text = dr["StudentID"].ToString();
                    lblProgramme.Text = dr["ProgrammeName"].ToString();
                    lblSession.Text = dr["Session"].ToString();
                    lblSemester.Text = ddlSemester.SelectedValue;
                }
            }
        }

        private void LoadEachCourseResult()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Get student's course results for selected semester
                string query = @"
                    SELECT 
                        c.CourseCode, 
                        c.CourseName,
                        c.CreditHours,
                        cm.FinalMark, 
                        cm.FinalGrade, 
                        cm.GradePoint,
                        (c.CreditHours * cm.GradePoint) AS CreditPoint

                    FROM CourseMarks cm

                    INNER JOIN EnrollmentDetails ed 
                        ON cm.DetailID = ed.DetailID

                    INNER JOIN EnrollmentMaster em 
                        ON ed.EnrolmentID = em.EnrolmentID

                    INNER JOIN CourseOfferings co 
                        ON ed.OfferingID = co.OfferingID

                    INNER JOIN Courses c 
                        ON co.CourseID = c.CourseID

                    WHERE em.StudentID = @StudentID
                    AND em.Semester = @Semester";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", ddlStudent.SelectedValue);
                cmd.Parameters.AddWithValue("@Semester", ddlSemester.SelectedValue);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display course results in GridView
                gvGradeReport.DataSource = dt;
                gvGradeReport.DataBind();

                // Save data for GPA calculation and PDF export
                ViewState["Grades"] = dt;
            }
        }

        private (double creditPoint, double creditHours) CalculateCredits(DataTable dt)
        {
            double totalCreditPoint = 0;
            double totalCreditHours = 0;

            // Calculate total credit points and credit hours
            foreach (DataRow row in dt.Rows)
            {
                double gradePoint = Convert.ToDouble(row["GradePoint"]);
                double creditHours = Convert.ToDouble(row["CreditHours"]);

                totalCreditPoint += gradePoint * creditHours;
                totalCreditHours += creditHours;
            }

            return (totalCreditPoint, totalCreditHours);
        }

        private void CalculateGPA()
        {
            // Get current semester results
            DataTable dt = ViewState["Grades"] as DataTable;

            if (dt == null || dt.Rows.Count == 0)
            {
                lblGPA.Text = "0.00";
                return;
            }

            // Calculate GPA = Total Credit Point / Total Credit Hours
            var result = CalculateCredits(dt);

            double gpa;

            if (result.creditHours == 0)
            {
                gpa = 0;
            }
            else
            {
                gpa = result.creditPoint / result.creditHours;
            }

            lblGPA.Text = gpa.ToString("0.00");
        }

        private void CalculateCGPA()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Get all previous and current semester results for CGPA calculation
                string query = @"
                    SELECT 
                        c.CreditHours,
                        cm.GradePoint

                    FROM CourseMarks cm

                    INNER JOIN EnrollmentDetails ed 
                        ON cm.DetailID = ed.DetailID

                    INNER JOIN EnrollmentMaster em 
                        ON ed.EnrolmentID = em.EnrolmentID

                    INNER JOIN CourseOfferings co 
                        ON ed.OfferingID = co.OfferingID

                    INNER JOIN Courses c 
                        ON co.CourseID = c.CourseID

                    WHERE em.StudentID = @StudentID
                    AND em.Semester <= @Semester";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", ddlStudent.SelectedValue);
                cmd.Parameters.AddWithValue("@Semester", ddlSemester.SelectedValue);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Calculate CGPA = Total Credit Point / Total Credit Hours
                var result = CalculateCredits(dt);

                double cgpa;

                if (result.creditHours == 0)
                {
                    cgpa = 0;
                }
                else
                {
                    cgpa = result.creditPoint / result.creditHours;
                }

                lblCGPA.Text = cgpa.ToString("0.00");
            }
        }

        // Download grade report as PDF
        protected void btnPDF_Click(object sender, EventArgs e)
        {
            // Stop PDF generation if no results data is available
            DataTable dt = ViewState["Grades"] as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return;

            // Create PDF document
            Document pdfDoc = new Document(PageSize.A4);
            MemoryStream ms = new MemoryStream();

            PdfWriter.GetInstance(pdfDoc, ms);
            pdfDoc.Open();

            pdfDoc.Add(new Paragraph("Student Grade Report"));
            pdfDoc.Add(new Paragraph(" "));

            // Student information
            pdfDoc.Add(new Paragraph("Student Name: " + lblStudentName.Text));
            pdfDoc.Add(new Paragraph("Student ID: " + lblStudentID.Text));
            pdfDoc.Add(new Paragraph("Programme: " + lblProgramme.Text));
            pdfDoc.Add(new Paragraph("Session: " + lblSession.Text));
            pdfDoc.Add(new Paragraph("Semester: " + lblSemester.Text));
            pdfDoc.Add(new Paragraph("Date: " + lblDate.Text));
            pdfDoc.Add(new Paragraph(" "));

            // Create PDF table
            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.WidthPercentage = 100;

            // Set the columns width
            float[] widths = {1.5f, 2.5f, 1.5f, 1.5f, 1f, 1.5f, 1f};

            table.SetWidths(widths);

            // Customize column headers
            foreach (DataColumn col in dt.Columns)
            {
                string header = col.ColumnName;

                if (header == "CourseCode")
                    header = "Course Code";

                else if (header == "CourseName")
                    header = "Course Name";

                else if (header == "CreditHours")
                    header = "Credit Hours";

                else if (header == "FinalMark")
                    header = "Final Mark";

                else if (header == "FinalGrade")
                    header = "Grade";

                else if (header == "GradePoint")
                    header = "Grade Point";

                else if (header == "CreditPoint")
                    header = "Credit Point";

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

            // Add GPA & CGPA
            pdfDoc.Add(new Paragraph(" "));
            pdfDoc.Add(new Paragraph("GPA: " + lblGPA.Text));
            pdfDoc.Add(new Paragraph("CGPA: " + lblCGPA.Text));

            pdfDoc.Close();

            // Send PDF file to browser
            Response.ContentType = "application/pdf";

            Response.AddHeader("content-disposition", "attachment;filename=StudentTranscript.pdf");

            Response.BinaryWrite(ms.ToArray());

            Response.End();
        }
    }
}