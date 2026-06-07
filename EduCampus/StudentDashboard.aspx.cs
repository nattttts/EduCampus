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
            // CHECK LOGIN
            if (Session["Role"].ToString() != "Student")
            {
                Response.Redirect("AccessDenied.aspx");
                return;
            }

            if (Session["Email"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                lblStudentName.Text = Session["Email"].ToString();
                LoadDashboard();
            }
        }

        // LOAD DASHBOARD
        void LoadDashboard()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // TOTAL COURSES
                string totalQuery = @"
                    SELECT COUNT(*)
                    FROM EnrollmentMaster
                    WHERE StudentID = (
                        SELECT StudentID FROM Students
                        WHERE UserID = (
                            SELECT UserId FROM Users WHERE Email = @Email
                        )
                    )";

                SqlCommand totalCmd = new SqlCommand(totalQuery, con);
                totalCmd.Parameters.AddWithValue("@Email", Session["Email"]);

                lblTotalCourses.Text = totalCmd.ExecuteScalar().ToString();

                // APPROVED COURSES
                string approvedQuery = @"
                    SELECT COUNT(*)
                    FROM EnrollmentMaster
                    WHERE Status = 'Approved'
                    AND StudentID = (
                        SELECT StudentID FROM Students
                        WHERE UserID = (
                            SELECT UserId FROM Users WHERE Email = @Email
                        )
                    )";

                SqlCommand approvedCmd = new SqlCommand(approvedQuery, con);
                approvedCmd.Parameters.AddWithValue("@Email", Session["Email"]);

                lblApproved.Text = approvedCmd.ExecuteScalar().ToString();

                // PENDING COURSES
                string pendingQuery = @"
                    SELECT COUNT(*)
                    FROM EnrollmentMaster
                    WHERE Status = 'Pending'
                    AND StudentID = (
                        SELECT StudentID FROM Students
                        WHERE UserID = (
                            SELECT UserId FROM Users WHERE Email = @Email
                        )
                    )";

                SqlCommand pendingCmd = new SqlCommand(pendingQuery, con);
                pendingCmd.Parameters.AddWithValue("@Email", Session["Email"]);

                lblPending.Text = pendingCmd.ExecuteScalar().ToString();
            }
        }

        // LOGOUT
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }
    }
}