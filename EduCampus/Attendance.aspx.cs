using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EduCampus
{
    public partial class Attendance : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email"] == null || Session["Role"].ToString() != "Student")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCourses();
                LoadAttendanceAll();
            }
        }

        void LoadCourses()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT DISTINCT c.CourseID, c.CourseName
                FROM Attendance a
                INNER JOIN CourseOfferings co ON a.OfferingID = co.OfferingID
                INNER JOIN Courses c ON co.CourseID = c.CourseID
                WHERE a.StudentID = (
                    SELECT StudentID FROM Students
                    WHERE UserID = (
                        SELECT UserID FROM Users WHERE Email = @Email
                    )
                )";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCourse.DataSource = dt;
                ddlCourse.DataTextField = "CourseName";
                ddlCourse.DataValueField = "CourseID";
                ddlCourse.DataBind();

                ddlCourse.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Courses", "0"));
            }
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCourse.SelectedValue == "0")
                LoadAttendanceAll();
            else
                LoadAttendanceByCourse(ddlCourse.SelectedValue);
        }

        void LoadAttendanceAll()
        {
            LoadAttendance(null);
        }

        void LoadAttendanceByCourse(string courseID)
        {
            LoadAttendance(courseID);
        }

        void LoadAttendance(string courseID)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT 
                    c.CourseName,
                    a.AttendanceDate,
                    a.Status,
                    a.Remarks
                FROM Attendance a
                INNER JOIN CourseOfferings co ON a.OfferingID = co.OfferingID
                INNER JOIN Courses c ON co.CourseID = c.CourseID
                WHERE a.StudentID = (
                    SELECT StudentID FROM Students
                    WHERE UserID = (
                        SELECT UserID FROM Users WHERE Email = @Email
                    )
                )";

                if (!string.IsNullOrEmpty(courseID))
                {
                    query += " AND c.CourseID = @CourseID";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"]);

                if (!string.IsNullOrEmpty(courseID))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAttendance.DataSource = dt;
                gvAttendance.DataBind();
            }
        }
        protected void btnBackDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentDashboard.aspx");
        }
    }
}