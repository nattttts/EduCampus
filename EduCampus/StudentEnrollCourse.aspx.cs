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
            SELECT StudentID 
            FROM Students
            WHERE UserID = (
                SELECT UserId 
                FROM Users 
                WHERE Email = @Email
            )";

                SqlCommand studentCmd = new SqlCommand(studentQuery, con);
                studentCmd.Parameters.AddWithValue("@Email", Session["Email"]);

                object result = studentCmd.ExecuteScalar();

                if (result == null)
                {
                    lblMessage.Text = "Student not found!";
                    return;
                }

                string studentID = result.ToString();

                // INSERT ENROLLMENT (FIXED COLUMN NAME)
                string insertQuery = @"
            INSERT INTO EnrollmentMaster
            (DateEnrolled, Status, StudentID, CourseOfferingID)
            VALUES
            (GETDATE(), 'Pending', @StudentID, @CourseOfferingID)";

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
            int enrolmentID = Convert.ToInt32(btn.CommandArgument);

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    // 1. DELETE CHILD FIRST (EnrollmentDetails)
                    string deleteDetail = @"
                DELETE FROM EnrollmentDetails
                WHERE EnrolmentID = @ID";

                    SqlCommand cmd1 = new SqlCommand(deleteDetail, con, trans);
                    cmd1.Parameters.AddWithValue("@ID", enrolmentID);
                    cmd1.ExecuteNonQuery();

                    // 2. DELETE PARENT (EnrollmentMaster)
                    string deleteMaster = @"
                DELETE FROM EnrollmentMaster
                WHERE EnrolmentID = @ID";

                    SqlCommand cmd2 = new SqlCommand(deleteMaster, con, trans);
                    cmd2.Parameters.AddWithValue("@ID", enrolmentID);
                    cmd2.ExecuteNonQuery();

                    trans.Commit();

                    lblMessage.Text = "Course Dropped Successfully!";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }

            LoadMyCourses();
        }
    }
}