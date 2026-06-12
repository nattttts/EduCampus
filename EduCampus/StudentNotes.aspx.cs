using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EduCampus
{
    public partial class StudentNotes : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        int courseID;
        int studentID;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔒 LOGIN CHECK
            if (Session["Email"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            lblMessage.Text = "";

            // ⚠️ VALIDATE COURSE ID
            if (!int.TryParse(Request.QueryString["CourseID"], out courseID))
            {
                lblMessage.Text = "⚠ Invalid Course ID.";
                gvNotes.DataSource = null;
                gvNotes.DataBind();
                return;
            }

            // 🔎 GET STUDENT ID
            if (!GetStudentID())
            {
                lblMessage.Text = "⚠ Student not found.";
                return;
            }

            // 🔐 CHECK ENROLLMENT
            if (!IsEnrolled())
            {
                lblMessage.Text = "⚠ You are not enrolled in this course.";
                gvNotes.DataSource = null;
                gvNotes.DataBind();
                return;
            }

            if (!IsPostBack)
            {
                LoadCourseName();
                LoadNotes();
            }
        }

        // 🔎 GET STUDENT ID FROM EMAIL
        bool GetStudentID()
        {
            string email = Session["Email"].ToString();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT StudentID FROM Students WHERE Email = @Email";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();

                object result = cmd.ExecuteScalar();

                if (result == null)
                    return false;

                studentID = Convert.ToInt32(result);
                return true;
            }
        }

        // 🔐 CHECK IF STUDENT IS ENROLLED
        bool IsEnrolled()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM Enrollments
                    WHERE StudentID = @StudentID
                    AND CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentID", studentID);
                cmd.Parameters.AddWithValue("@CourseID", courseID);

                con.Open();

                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        // 📘 LOAD COURSE NAME
        void LoadCourseName()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT CourseName FROM Courses WHERE CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CourseID", courseID);

                con.Open();

                object result = cmd.ExecuteScalar();

                lblCourseName.Text = result != null
                    ? result.ToString()
                    : "Course Not Found";
            }
        }

        // 📄 LOAD NOTES
        void LoadNotes()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT WeekNo, FileName, FilePath, UploadDate
                    FROM Notes
                    WHERE CourseID = @CourseID
                    ORDER BY WeekNo";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CourseID", courseID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                gvNotes.DataSource = dt;
                gvNotes.DataBind();

                if (dt.Rows.Count == 0)
                {
                    lblMessage.Text = "ℹ No notes available for this course.";
                }
            }
        }

        // 🚪 LOGOUT
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}