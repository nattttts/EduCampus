using System;
using System.Configuration;
using System.Data.SqlClient;

namespace EduCampus
{
    public partial class StudentDashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager
            .ConnectionStrings["EduCampusDB"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // CHECK LOGIN FIRST
            if (Session["Email"] == null || Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // CHECK ROLE
            if (Session["Role"].ToString() != "Student")
            {
                Response.Redirect("AccessDenied.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStudentName();
                LoadDashboard();
            }
        }

        // GET STUDENT NAME
        void LoadStudentName()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT FullName
                    FROM Users
                    WHERE Email = @Email";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"]);

                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    lblStudentName.Text = result.ToString();
                }
                else
                {
                    lblStudentName.Text = "Student";
                }
            }
        }

        // LOAD DASHBOARD STATS
        void LoadDashboard()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string studentQuery = @"
                    SELECT StudentID
                    FROM Students
                    WHERE UserID = (
                        SELECT UserID FROM Users WHERE Email = @Email
                    )";

                SqlCommand studentCmd = new SqlCommand(studentQuery, con);
                studentCmd.Parameters.AddWithValue("@Email", Session["Email"]);

                object studentResult = studentCmd.ExecuteScalar();

                if (studentResult == null)
                    return;

                string studentID = studentResult.ToString();

                // TOTAL COURSES
                string totalQuery = @"
                    SELECT COUNT(*)
                    FROM EnrollmentMaster
                    WHERE StudentID = @StudentID";

                SqlCommand totalCmd = new SqlCommand(totalQuery, con);
                totalCmd.Parameters.AddWithValue("@StudentID", studentID);

                lblTotalCourses.Text = totalCmd.ExecuteScalar().ToString();

                // APPROVED
                string approvedQuery = @"
                    SELECT COUNT(*)
                    FROM EnrollmentMaster
                    WHERE StudentID = @StudentID
                    AND Status = 'Approved'";

                SqlCommand approvedCmd = new SqlCommand(approvedQuery, con);
                approvedCmd.Parameters.AddWithValue("@StudentID", studentID);

                lblApproved.Text = approvedCmd.ExecuteScalar().ToString();

                // PENDING
                string pendingQuery = @"
                    SELECT COUNT(*)
                    FROM EnrollmentMaster
                    WHERE StudentID = @StudentID
                    AND Status = 'Pending'";

                SqlCommand pendingCmd = new SqlCommand(pendingQuery, con);
                pendingCmd.Parameters.AddWithValue("@StudentID", studentID);

                lblPending.Text = pendingCmd.ExecuteScalar().ToString();
            }
        }

        // LOGOUT
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}