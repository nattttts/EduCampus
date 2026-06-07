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
                    em.EnrolmentID,
                    c.CourseCode,
                    c.CourseName,
                    em.Status
                FROM EnrollmentMaster em
                INNER JOIN EnrollmentDetails ed
                    ON em.EnrolmentID = ed.EnrolmentID
                INNER JOIN CourseOfferings co
                    ON ed.OfferingID = co.OfferingID
                INNER JOIN Courses c
                    ON co.CourseID = c.CourseID
                WHERE em.StudentID =
                (
                    SELECT StudentID
                    FROM Students
                    WHERE UserID =
                    (
                        SELECT UserID
                        FROM Users
                        WHERE Email = @Email
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
                cmd.Parameters.AddWithValue("@CourseOfferingID", offeringID);

                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = "Course Enrolled Successfully!";
            LoadMyCourses();
        }

        // DROP COURSE
        protected void btnDrop_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int enrollmentID = Convert.ToInt32(btn.CommandArgument);

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string query = @"
            DELETE FROM EnrollmentMaster
            WHERE EnrollmentID = @EnrollmentID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentID);

                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = "Course dropped successfully!";
            LoadMyCourses();
        }
    }
}