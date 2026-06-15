using System;
using System.Data;
using System.Data.SqlClient;

namespace EduCampus
{
    public partial class StudentNotification : System.Web.UI.Page
    {
        string cs = System.Configuration.ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔒 CHECK LOGIN
            if (Session["Email"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadNotifications();
                LoadAnnouncements();
            }
        }

        // 🔔 EXISTING: NOTIFICATIONS
        void LoadNotifications()
        {
            string email = Session["Email"].ToString();
            int userId = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // GET USER ID
                string getUserIdQuery = @"
                    SELECT UserID 
                    FROM Users 
                    WHERE Email = @Email";

                using (SqlCommand cmdUser = new SqlCommand(getUserIdQuery, con))
                {
                    cmdUser.Parameters.AddWithValue("@Email", email);

                    object result = cmdUser.ExecuteScalar();

                    if (result == null)
                    {
                        Response.Redirect("Login.aspx");
                        return;
                    }

                    userId = Convert.ToInt32(result);
                }

                // GET NOTIFICATIONS
                string query = @"
                    SELECT Title, Message, CreatedDateTime
                    FROM Notifications
                    WHERE UserID = @UserID
                    ORDER BY CreatedDateTime DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    gvNotifications.DataSource = dt;
                    gvNotifications.DataBind();
                }
            }
        }

        // 📢 NEW: ANNOUNCEMENTS
        void LoadAnnouncements()
        {
            string email = Session["Email"].ToString();
            int userId = 0;
            string studentId = "";

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // GET USER ID
                string getUserIdQuery = @"
                    SELECT UserID 
                    FROM Users 
                    WHERE Email = @Email";

                using (SqlCommand cmdUser = new SqlCommand(getUserIdQuery, con))
                {
                    cmdUser.Parameters.AddWithValue("@Email", email);

                    object result = cmdUser.ExecuteScalar();

                    if (result == null)
                        return;

                    userId = Convert.ToInt32(result);
                }

                // GET STUDENT ID
                string getStudentQuery = @"
                    SELECT StudentID 
                    FROM Students 
                    WHERE UserID = @UserID";

                using (SqlCommand cmdStudent = new SqlCommand(getStudentQuery, con))
                {
                    cmdStudent.Parameters.AddWithValue("@UserID", userId);

                    object result = cmdStudent.ExecuteScalar();

                    if (result == null)
                        return;

                    studentId = result.ToString();
                }

                // GET ANNOUNCEMENTS (BASED ON ENROLLED COURSES)
                string query = @"
                    SELECT DISTINCT a.Title, a.Message, a.PostedDateTime
                    FROM Announcements a
                    INNER JOIN CourseOfferings co ON a.OfferingID = co.OfferingID
                    INNER JOIN EnrollmentDetails ed ON co.OfferingID = ed.OfferingID
                    INNER JOIN EnrollmentMaster em ON ed.EnrolmentID = em.EnrolmentID
                    WHERE em.StudentID = @StudentID
                    ORDER BY a.PostedDateTime DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    gvAnnouncements.DataSource = dt;
                    gvAnnouncements.DataBind();
                }
            }
        }

        // 🚪 LOGOUT (UNCHANGED)
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}