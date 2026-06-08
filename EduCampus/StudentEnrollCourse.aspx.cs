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

        string GetStudentID(SqlConnection con, SqlTransaction trans = null)
        {
            string query = @"
                SELECT StudentID
                FROM Students
                WHERE UserID = (
                    SELECT UserID
                    FROM Users
                    WHERE Email = @Email
                )";

            SqlCommand cmd = new SqlCommand(query, con, trans);
            cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                return result.ToString();
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email"] == null || Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["Role"].ToString() != "Student")
            {
                Response.Redirect("AccessDenied.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCourses();
                LoadMyCourses();
            }
        }

        // LOAD AVAILABLE COURSES
        private void LoadCourses()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT
                    o.OfferingID,
                    c.CourseCode,
                    c.CourseName,
                    c.CreditHours,
                    o.Session
                FROM CourseOfferings o
                INNER JOIN Courses c
                    ON o.CourseID = c.CourseID
                WHERE o.Session IS NOT NULL";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCourses.DataSource = dt;
                gvCourses.DataBind();
            }
        }

        // LOAD MY COURSES
        private void LoadMyCourses()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT
                    em.EnrolmentID,
                    c.CourseCode,
                    c.CourseName,
                    em.Status,
                    em.Session,
                    em.Semester
                FROM EnrollmentMaster em
                INNER JOIN EnrollmentDetails ed
                    ON em.EnrolmentID = ed.EnrolmentID
                INNER JOIN CourseOfferings co
                    ON ed.OfferingID = co.OfferingID
                INNER JOIN Courses c
                    ON co.CourseID = c.CourseID
                WHERE em.StudentID = (
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
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    string studentID = GetStudentID(con, trans);

                    if (studentID == null)
                    {
                        lblMessage.Text = "Student not found.";
                        return;
                    }

                    string checkQuery = @"
                    SELECT COUNT(*)
                    FROM EnrollmentDetails ed
                    INNER JOIN EnrollmentMaster em
                        ON ed.EnrolmentID = em.EnrolmentID
                    WHERE em.StudentID = @StudentID
                    AND ed.OfferingID = @OfferingID";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, con, trans);
                    checkCmd.Parameters.AddWithValue("@StudentID", studentID);
                    checkCmd.Parameters.AddWithValue("@OfferingID", offeringID);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        lblMessage.Text = "Already enrolled in this course.";
                        return;
                    }

                    string insertMaster = @"
                    INSERT INTO EnrollmentMaster
                    (
                        DateEnrolled,
                        Status,
                        Session,
                        Semester,
                        StudentID
                    )
                    VALUES
                    (
                        GETDATE(),
                        'Pending',
                        '2026',
                        'Semester 1',
                        @StudentID
                    );

                    SELECT SCOPE_IDENTITY();";

                    SqlCommand masterCmd = new SqlCommand(insertMaster, con, trans);
                    masterCmd.Parameters.AddWithValue("@StudentID", studentID);

                    int enrolmentID = Convert.ToInt32(masterCmd.ExecuteScalar());

                    string insertDetail = @"
                    INSERT INTO EnrollmentDetails
                    (
                        EnrolmentID,
                        OfferingID
                    )
                    VALUES
                    (
                        @EnrolmentID,
                        @OfferingID
                    )";

                    SqlCommand detailCmd = new SqlCommand(insertDetail, con, trans);
                    detailCmd.Parameters.AddWithValue("@EnrolmentID", enrolmentID);
                    detailCmd.Parameters.AddWithValue("@OfferingID", offeringID);

                    detailCmd.ExecuteNonQuery();

                    trans.Commit();

                    lblMessage.Text = "Enrolled successfully.";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    lblMessage.Text = ex.Message;
                }
            }

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
                    string deleteResults = @"
                    DELETE FROM Results
                    WHERE EnrolmentID = @ID";

                    new SqlCommand(deleteResults, con, trans)
                    {
                        Parameters = { new SqlParameter("@ID", enrolmentID) }
                    }.ExecuteNonQuery();

                    string deleteDetails = @"
                    DELETE FROM EnrollmentDetails
                    WHERE EnrolmentID = @ID";

                    new SqlCommand(deleteDetails, con, trans)
                    {
                        Parameters = { new SqlParameter("@ID", enrolmentID) }
                    }.ExecuteNonQuery();

                    string deleteMaster = @"
                    DELETE FROM EnrollmentMaster
                    WHERE EnrolmentID = @ID";

                    new SqlCommand(deleteMaster, con, trans)
                    {
                        Parameters = { new SqlParameter("@ID", enrolmentID) }
                    }.ExecuteNonQuery();

                    trans.Commit();

                    lblMessage.Text = "Course dropped successfully.";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    lblMessage.Text = ex.Message;
                }
            }

            LoadMyCourses();
        }
    }
}