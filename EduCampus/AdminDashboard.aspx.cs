using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace EduCampus
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;
        
        public string ProgrammeLabels = "";
        public string ProgrammeCounts = "";

        public string EnrollmentLabels = "";
        public string EnrollmentCounts = "";

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
                LoadDashboard();
            }
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("Login.aspx"); // go back to login
        }

        private void LoadDashboard()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Total Programmes
                string totalProgrammesQuery = "SELECT COUNT(*) FROM Programmes";
                lblTotalProgrammes.Text =
                    new SqlCommand(totalProgrammesQuery, conn).ExecuteScalar().ToString();

                // Total Courses
                string totalCoursesQuery = "SELECT COUNT(*) FROM Courses";
                lblTotalCourses.Text =
                    new SqlCommand(totalCoursesQuery, conn).ExecuteScalar().ToString();

                // Total Lecturers
                string totalLecturersQuery = "SELECT COUNT(*) FROM Lecturers";
                lblTotalLecturers.Text =
                    new SqlCommand(totalLecturersQuery, conn).ExecuteScalar().ToString();

                // Total Students
                string totalStudentsQuery = "SELECT COUNT(*) FROM Students";
                lblTotalStudents.Text =
                    new SqlCommand(totalStudentsQuery, conn).ExecuteScalar().ToString();

                // Charts
                LoadProgrammeChart(conn);

                LoadEnrollmentChart(conn);
            }
        }

        private void LoadProgrammeChart(SqlConnection conn)
        {
            string query = @"
                SELECT p.ProgrammeName,
                       COUNT(s.StudentID) AS TotalStudents
                FROM Programmes p
                LEFT JOIN Students s
                ON p.ProgrammeID = s.ProgrammeID
                GROUP BY p.ProgrammeName
                ORDER BY p.ProgrammeName";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader dr = cmd.ExecuteReader();

            List<string> labels = new List<string>();

            List<int> counts = new List<int>();

            while (dr.Read())
            {
                labels.Add("'" + dr["ProgrammeName"].ToString() + "'");

                counts.Add(
                    Convert.ToInt32(dr["TotalStudents"])
                );
            }

            dr.Close();

            ProgrammeLabels = string.Join(",", labels);

            ProgrammeCounts = string.Join(",", counts);
        }

        private void LoadEnrollmentChart(SqlConnection con)
        {
            string query = @"
                SELECT Status,
                       COUNT(*) AS Total
                FROM EnrollmentMaster
                GROUP BY Status";

            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataReader dr = cmd.ExecuteReader();

            List<string> labels = new List<string>();

            List<int> counts = new List<int>();

            while (dr.Read())
            {
                labels.Add("'" + dr["Status"].ToString() + "'");

                counts.Add(
                    Convert.ToInt32(dr["Total"])
                );
            }

            dr.Close();

            EnrollmentLabels = string.Join(",", labels);

            EnrollmentCounts = string.Join(",", counts);
        }
    }
}