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
            if (Session["Email"] == null || Session["Role"] == null || Session["Role"].ToString() != "Student")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCourses();
                LoadAttendance(null);
            }
        }

        // GET STUDENT ID (STRING, NOT INT!)
        private string GetStudentID(SqlConnection con)
        {
            string query = @"
                SELECT StudentID 
                FROM Students 
                WHERE UserID = (SELECT UserID FROM Users WHERE Email = @Email)";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", Session["Email"]);

            object result = cmd.ExecuteScalar();

            return (result == null || result == DBNull.Value) ? "" : result.ToString();
        }

        // LOAD COURSES (via Enrollment flow)
        void LoadCourses()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string studentID = GetStudentID(con);

                string query = @"
                    SELECT DISTINCT c.CourseID, c.CourseName
                    FROM EnrollmentMaster em
                    INNER JOIN EnrollmentDetails ed ON em.EnrolmentID = ed.EnrolmentID
                    INNER JOIN CourseOfferings co ON ed.OfferingID = co.OfferingID
                    INNER JOIN Courses c ON co.CourseID = c.CourseID
                    WHERE em.StudentID = @StudentID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentID", studentID);

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
                LoadAttendance(null);
            else
                LoadAttendance(ddlCourse.SelectedValue);
        }

        // LOAD ATTENDANCE (CORRECT USING DetailID)
        void LoadAttendance(string courseID)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string studentID = GetStudentID(con);

                string query = @"
                    SELECT 
                        c.CourseName,
                        a.AttendanceDate,
                        a.Status,
                        a.Remarks
                    FROM Attendance a
                    INNER JOIN EnrollmentDetails ed ON a.DetailID = ed.DetailID
                    INNER JOIN EnrollmentMaster em ON ed.EnrolmentID = em.EnrolmentID
                    INNER JOIN CourseOfferings co ON ed.OfferingID = co.OfferingID
                    INNER JOIN Courses c ON co.CourseID = c.CourseID
                    WHERE em.StudentID = @StudentID";

                if (!string.IsNullOrEmpty(courseID) && courseID != "0")
                {
                    query += " AND c.CourseID = @CourseID";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentID", studentID);

                if (!string.IsNullOrEmpty(courseID) && courseID != "0")
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
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}