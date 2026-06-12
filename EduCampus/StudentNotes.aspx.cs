using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace StudentManagementSystem
{
    public partial class StudentCourseNotes : System.Web.UI.Page
    {
        string cs = ConfigurationManager
            .ConnectionStrings["CollegeDB"]
            .ConnectionString;

        int courseID;

        protected void Page_Load(object sender, EventArgs e)
        {
            // CHECK LOGIN

            if (Session["StudentEmail"] == null)
            {
                Response.Redirect("StudentLogin.aspx");
            }

            courseID =
                Convert.ToInt32(Request.QueryString["CourseID"]);

            if (!IsPostBack)
            {
                LoadCourseName();
                LoadNotes();
            }
        }

        // LOAD COURSE NAME

        void LoadCourseName()
        {
            SqlConnection con =
                new SqlConnection(cs);

            string query =
                "SELECT CourseName FROM Courses WHERE CourseID=@CourseID";

            SqlCommand cmd =
                new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@CourseID",
                courseID);

            con.Open();

            lblCourseName.Text =
                cmd.ExecuteScalar().ToString();

            con.Close();
        }

        // LOAD NOTES

        void LoadNotes()
        {
            SqlConnection con =
                new SqlConnection(cs);

            string query = @"

            SELECT
            WeekNo,
            FileName,
            FilePath,
            UploadDate

            FROM Notes

            WHERE CourseID=@CourseID

            ORDER BY WeekNo";

            SqlCommand cmd =
                new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@CourseID",
                courseID);

            SqlDataAdapter sda =
                new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            sda.Fill(dt);

            gvNotes.DataSource = dt;

            gvNotes.DataBind();
        }
    }
}