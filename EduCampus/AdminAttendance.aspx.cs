using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class AdminAttendance : System.Web.UI.Page
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

                ddlRecordCourse.Items.Clear();
                ddlRecordCourse.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Course --", ""));
            }
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("Login.aspx"); // go back to login
        }

        // Load session in the dropdown
        private void LoadSession()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Load available sessions for filtering from attendance
                string query = @"SELECT DISTINCT co.Session
                                 FROM Attendance a

                                 INNER JOIN EnrollmentDetails ed 
                                    ON a.DetailID = ed.DetailID
                                
                                 INNER JOIN CourseOfferings co 
                                    ON ed.OfferingID = co.OfferingID
                                 
                                 ORDER BY co.Session";

                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                ddlRecordSession.DataSource = dt;
                ddlRecordSession.DataTextField = "Session";
                ddlRecordSession.DataValueField = "Session";
                ddlRecordSession.DataBind();

                // Add default selection option
                ddlRecordSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Session --", ""));
            }
        }

        // Load course in the dropdown
        private void LoadCourses()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Get courses based on selected session
                string query = @"
                    SELECT DISTINCT c.CourseID, c.CourseCode + ' ' + c.CourseName AS CourseDisplay
                    FROM Attendance a
                    
                    INNER JOIN EnrollmentDetails ed
                        ON a.DetailID = ed.DetailID
               
                    INNER JOIN CourseOfferings co 
                        ON ed.OfferingID = co.OfferingID
    
                    INNER JOIN Courses c
                        ON co.CourseID = c.CourseID

                    WHERE co.Session = @Session
                    ORDER BY CourseDisplay";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Session", ddlRecordSession.SelectedValue);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                ddlRecordCourse.DataSource = dt;
                ddlRecordCourse.DataTextField = "CourseDisplay";
                ddlRecordCourse.DataValueField = "CourseID";
                ddlRecordCourse.DataBind();

                // Add default selection option
                ddlRecordCourse.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Course --", ""));
            }
        }

        protected void ddlRecordSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hide results when filter changes
            pnlSearchResult.Visible = false;

            // If no session is selected, reset course dropdown and stop loading
            if (ddlRecordSession.SelectedValue == "")
            {
                ddlRecordCourse.Items.Clear();
                ddlRecordCourse.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Course --", ""));
                return;
            }

            LoadCourses();
        }

        protected void ddlRecordCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hide results when filter changes
            pnlSearchResult.Visible = false;
        }

        // Search attendance records
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Ensure required filters are selected before executing search
            if (ddlRecordSession.SelectedValue == "")
                return;

            if (ddlRecordCourse.SelectedValue == "")
                return;

            LoadAttendance();

            // Show result panel
            pnlSearchResult.Visible = true;
        }

        // Load attendance data and summary
        private void LoadAttendance()
        {
            LoadAttendanceRecords();

            LoadAttendanceSummary();

            LoadPoorAttendance();
        }

        // Get attendance summary
        private void LoadAttendanceSummary()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT COUNT(*) AS Total,
                    
                    ISNULL(SUM(CASE WHEN a.Status = 'Present' THEN 1 ELSE 0 END), 0) AS Present, 
                    ISNULL(SUM(CASE WHEN a.Status = 'Absent' THEN 1 ELSE 0 END), 0) AS Absent

                    FROM Attendance a

                    INNER JOIN EnrollmentDetails ed 
                        ON a.DetailID = ed.DetailID

                    INNER JOIN CourseOfferings co 
                        ON ed.OfferingID = co.OfferingID

                    WHERE co.Session = @Session
                    AND co.CourseID = @CourseID";

                // Add date query if user selects a date
                if (!string.IsNullOrEmpty(txtAttendanceDate.Text))
                {
                    query += " AND a.AttendanceDate=@Date";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Session", ddlRecordSession.SelectedValue);
                cmd.Parameters.AddWithValue("@CourseID", ddlRecordCourse.SelectedValue);

                // Add date parameter if date is selected
                if (!string.IsNullOrEmpty(txtAttendanceDate.Text))
                {
                    cmd.Parameters.AddWithValue("@Date", txtAttendanceDate.Text);
                }

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                // Populate attendance summary values to UI
                if (dt.Rows.Count > 0)
                {
                    int total = Convert.ToInt32(dt.Rows[0]["Total"]);
                    int present = Convert.ToInt32(dt.Rows[0]["Present"]);
                    int absent = Convert.ToInt32(dt.Rows[0]["Absent"]);

                    lblTotalRecords.Text = total.ToString();
                    lblPresent.Text = present.ToString();
                    lblAbsent.Text = absent.ToString();

                    // Calculate attendance rate
                    double rate;

                    if (total == 0)
                    {
                        rate = 0;
                    }
                    else
                    {
                        rate = (present * 100.0) / total;
                    }

                    lblRate.Text = rate.ToString("0.0") + "%";
                }
            }
        }

        // Get attendance records
        private void LoadAttendanceRecords()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT
                        u.FullName,
                        em.StudentID,
                        a.AttendanceDate,
                        a.Status,
                        a.Remarks

                     FROM Attendance a

                     INNER JOIN EnrollmentDetails ed
                         ON a.DetailID = ed.DetailID

                     INNER JOIN EnrollmentMaster em
                         ON ed.EnrolmentID = em.EnrolmentID

                     INNER JOIN Students s
                         ON em.StudentID = s.StudentID

                     INNER JOIN Users u
                         ON s.UserID = u.UserID

                     INNER JOIN CourseOfferings co
                         ON ed.OfferingID = co.OfferingID

                     WHERE co.Session = @Session
                     AND co.CourseID = @CourseID";

                // Add date query if user selects a date
                if (!string.IsNullOrEmpty(txtAttendanceDate.Text))
                {
                    query += " AND a.AttendanceDate=@Date";
                }

                query += " ORDER BY a.AttendanceDate, u.FullName";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Session", ddlRecordSession.SelectedValue);
                cmd.Parameters.AddWithValue("@CourseID", ddlRecordCourse.SelectedValue);

                // Add date parameter if date is selected
                if (!string.IsNullOrEmpty(txtAttendanceDate.Text))
                {
                    cmd.Parameters.AddWithValue("@Date", txtAttendanceDate.Text);
                }

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                sda.Fill(dt);

                gvAttendance.DataSource = dt;
                gvAttendance.DataBind();

                // Store data for PDF download
                ViewState["AttendanceData"] = dt;
            }
        }

        // Download attendance report as PDF
        protected void btnPDF_Click(object sender, EventArgs e)
        {
            // Stop PDF generation if no attendance data is available
            DataTable dt = ViewState["AttendanceData"] as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return;

            // Create PDF document
            Document pdfDoc = new Document(PageSize.A4);
            MemoryStream ms = new MemoryStream();

            PdfWriter.GetInstance(pdfDoc, ms);
            pdfDoc.Open();

            pdfDoc.Add(new Paragraph("Attendance Report"));
            pdfDoc.Add(new Paragraph("Generated On: " + DateTime.Now.ToString()));
            pdfDoc.Add(new Paragraph(" "));

            // Add selected session and course filter to the report
            pdfDoc.Add(new Paragraph("Session: " + ddlRecordSession.SelectedItem.Text));
            pdfDoc.Add(new Paragraph("Course: " + ddlRecordCourse.SelectedItem.Text));

            // Display date filter if selected
            if (!string.IsNullOrEmpty(txtAttendanceDate.Text))
            {
                pdfDoc.Add(new Paragraph("Attendance Date: " + txtAttendanceDate.Text));
            }

            pdfDoc.Add(new Paragraph(" "));

            pdfDoc.Add(new Paragraph("Attendance Summary"));
            pdfDoc.Add(new Paragraph("Total Records: " + lblTotalRecords.Text));
            pdfDoc.Add(new Paragraph("Present: " + lblPresent.Text));
            pdfDoc.Add(new Paragraph("Absent: " + lblAbsent.Text));
            pdfDoc.Add(new Paragraph("Attendance Rate: " + lblRate.Text));
            pdfDoc.Add(new Paragraph(" "));

            // Create PDF table
            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.WidthPercentage = 100;

            // Customize column headers
            foreach (DataColumn col in dt.Columns)
            {
                string header = col.ColumnName;

                if (header == "FullName")
                    header = "Student Name";

                else if (header == "StudentID")
                    header = "Student ID";

                else if (header == "AttendanceDate")
                    header = "Attendance Date";

                table.AddCell(new Phrase(header));
            }

            // Add attendance records to PDF
            foreach (DataRow row in dt.Rows)
            {
                foreach (var cell in row.ItemArray)
                {
                    if (cell == DBNull.Value)
                    {
                        table.AddCell("-");
                    }

                    else if (cell is DateTime)
                    {
                        table.AddCell(Convert.ToDateTime(cell).ToString("dd/MM/yyyy"));
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

            Response.AddHeader("content-disposition", "attachment;filename=AttendanceReport.pdf");

            Response.BinaryWrite(ms.ToArray());

            Response.End();
        }

        // Load students with poor attendance
        private void LoadPoorAttendance()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Get students with 3 or more absences
                string query = @"
                    SELECT
                        u.FullName,
                        em.StudentID,
                        COUNT(*) AS AbsentCount

                     FROM Attendance a

                     INNER JOIN EnrollmentDetails ed
                         ON a.DetailID = ed.DetailID

                     INNER JOIN EnrollmentMaster em
                         ON ed.EnrolmentID = em.EnrolmentID

                     INNER JOIN Students s
                         ON em.StudentID = s.StudentID

                     INNER JOIN Users u
                         ON s.UserID = u.UserID

                     INNER JOIN CourseOfferings co
                         ON ed.OfferingID = co.OfferingID

                     WHERE co.Session = @Session
                     AND co.CourseID = @CourseID
                     AND a.Status = 'Absent'
                
                     GROUP BY
                            u.FullName,
                            em.StudentID
                 
                     HAVING COUNT(*) >= 3
                     ORDER BY AbsentCount DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Session",
                    ddlRecordSession.SelectedValue);

                cmd.Parameters.AddWithValue("@CourseID",
                    ddlRecordCourse.SelectedValue);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sda.Fill(dt);

                gvPoorAttendance.DataSource = dt;
                gvPoorAttendance.DataBind();
            }
        }

    }
}