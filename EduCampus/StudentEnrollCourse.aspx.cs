using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class StudentEnrollCourse : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
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
                LoadCourses();
                LoadMyCourses();
            }
        }

        // LOAD ALL AVAILABLE COURSES
        void LoadCourses()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT 
                        o.OfferingID,
                        c.CourseCode,
                        c.CourseName,
                        c.CreditHours
                    FROM CourseOfferings o
                    INNER JOIN Courses c ON o.CourseID = c.CourseID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCourses.DataSource = dt;
                gvCourses.DataBind();
            }
        }

        // LOAD STUDENT ENROLLED COURSES
        void LoadMyCourses()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT 
                        e.EnrolmentID,
                        c.CourseCode,
                        c.CourseName,
                        e.Status
                    FROM EnrollmentMaster e
                    INNER JOIN CourseOfferings o ON e.OfferingID = o.OfferingID
                    INNER JOIN Courses c ON o.CourseID = c.CourseID
                    WHERE e.StudentID = (
                        SELECT StudentID FROM Students
                        WHERE UserID = (
                            SELECT UserId FROM Users WHERE Email = @Email
                        )
                    )";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"]);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvEnrollment.DataSource = dt;
                gvEnrollment.DataBind();
            }
        }

        // ENROLL COURSE
        protected void btnEnroll_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int offeringID = Convert.ToInt32(btn.CommandArgument);

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // GET STUDENT ID
                string studentQuery = @"
                    SELECT StudentID FROM Students
                    WHERE UserID = (
                        SELECT UserId FROM Users WHERE Email = @Email
                    )";

                SqlCommand studentCmd = new SqlCommand(studentQuery, con);
                studentCmd.Parameters.AddWithValue("@Email", Session["Email"]);

                string studentID = studentCmd.ExecuteScalar().ToString();

                // INSERT ENROLMENT
                string insertQuery = @"
                    INSERT INTO EnrollmentMaster
                    (DateEnrolled, Status, StudentID, OfferingID)
                    VALUES
                    (GETDATE(), 'Pending', @StudentID, @OfferingID)";

                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@StudentID", studentID);
                cmd.Parameters.AddWithValue("@OfferingID", offeringID);

                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = "Course Enrolled Successfully!";
            LoadMyCourses();
        }

        // DROP COURSE
        protected void btnDrop_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int enrolmentID = Convert.ToInt32(btn.CommandArgument);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM EnrollmentMaster WHERE EnrolmentID=@ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", enrolmentID);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = "Course Dropped Successfully!";
            LoadMyCourses();
        }
    }
}