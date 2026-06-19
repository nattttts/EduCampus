using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class EnrollmentStatistics : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        // Variables are used for Chart.js binding
        public string CourseLabels = "";
        public string StudentCounts = "";

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
                LoadStatistics();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminViewEnrollment.aspx");
        }

        // Load available session list into dropdown
        private void LoadSession()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Get session from EnrollmentMaster
                string query = @"SELECT DISTINCT Session
                                 FROM EnrollmentMaster
                                 ORDER BY Session";

                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                ddlSession.DataSource = dt;
                ddlSession.DataTextField = "Session";
                ddlSession.DataValueField = "Session";
                ddlSession.DataBind();

                ddlSession.Items.Insert(0, new ListItem("-- Select Session --", ""));
            }
        }

        // Load enrollment statistics based on selected session
        private void LoadStatistics()
        {
            if (ddlSession.SelectedValue == "")
            {
                gvStatistics.DataSource = null;
                gvStatistics.DataBind();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Count number of students per course in selected session
                string query = @"
                    SELECT 
                        c.CourseCode,
                        c.CourseName,
                        COUNT(em.StudentID) AS TotalStudents

                    FROM EnrollmentMaster em

                    INNER JOIN EnrollmentDetails ed 
                        ON em.EnrolmentID = ed.EnrolmentID

                    INNER JOIN CourseOfferings co 
                        ON ed.OfferingID = co.OfferingID

                    INNER JOIN Courses c 
                        ON co.CourseID = c.CourseID

                    WHERE em.Session = @Session

                    GROUP BY c.CourseCode, c.CourseName";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                gvStatistics.DataSource = dt;
                gvStatistics.DataBind();

                // Send data to chart renderer
                LoadEnrollmentChart(dt);
            }
        }

        // Convert DataTable into Chart.js compatible format
        private void LoadEnrollmentChart(DataTable dt) 
        {
            List<string> labels = new List<string>();
            List<int> counts = new List<int>();

            foreach (DataRow row in dt.Rows)
            {
                labels.Add("'" + row["CourseCode"].ToString() + "'");
                counts.Add(Convert.ToInt32(row["TotalStudents"]));
            }

            CourseLabels = string.Join(",", labels);
            StudentCounts = string.Join(",", counts);
        }

        // Reload data when session dropdown changes
        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStatistics();
        }
    }
}