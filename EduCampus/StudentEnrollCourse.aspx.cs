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
            if (Session["Email"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["Role"] == null || Session["Role"].ToString() != "Student")
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
                    ON o.CourseID = c.CourseID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();

                da.Fill(dt);

                gvCourses.DataSource = dt;
                gvCourses.DataBind();
            }
        }

        // LOAD ENROLLED COURSES
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

                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    // GET STUDENT ID
                    string studentQuery = @"
                    SELECT StudentID
                    FROM Students
                    WHERE UserID =
                    (
                        SELECT UserID
                        FROM Users
                        WHERE Email = @Email
                    )";

                    SqlCommand studentCmd =
                        new SqlCommand(studentQuery, con, trans);

                    studentCmd.Parameters.AddWithValue(
                        "@Email",
                        Session["Email"].ToString());

                    object result = studentCmd.ExecuteScalar();

                    if (result == null)
                    {
                        lblMessage.Text = "Student record not found.";
                        return;
                    }

                    string studentID = result.ToString();

                    // CHECK DUPLICATE ENROLLMENT
                    string checkQuery = @"
                    SELECT COUNT(*)
                    FROM EnrollmentMaster em
                    INNER JOIN EnrollmentDetails ed
                        ON em.EnrolmentID = ed.EnrolmentID
                    WHERE em.StudentID = @StudentID
                    AND ed.OfferingID = @OfferingID";

                    SqlCommand checkCmd =
                        new SqlCommand(checkQuery, con, trans);

                    checkCmd.Parameters.AddWithValue(
                        "@StudentID",
                        studentID);

                    checkCmd.Parameters.AddWithValue(
                        "@OfferingID",
                        offeringID);

                    int count =
                        Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        lblMessage.Text =
                            "You have already enrolled in this course.";
                        return;
                    }

                    // INSERT ENROLLMENT MASTER
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

                    SqlCommand masterCmd =
                        new SqlCommand(insertMaster, con, trans);

                    masterCmd.Parameters.AddWithValue(
                        "@StudentID",
                        studentID);

                    int enrolmentID =
                        Convert.ToInt32(masterCmd.ExecuteScalar());

                    // INSERT ENROLLMENT DETAIL
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

                    SqlCommand detailCmd =
                        new SqlCommand(insertDetail, con, trans);

                    detailCmd.Parameters.AddWithValue(
                        "@EnrolmentID",
                        enrolmentID);

                    detailCmd.Parameters.AddWithValue(
                        "@OfferingID",
                        offeringID);

                    detailCmd.ExecuteNonQuery();

                    trans.Commit();

                    lblMessage.Text =
                        "Course enrolled successfully.";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    lblMessage.Text =
                        "Error: " + ex.Message;
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
                    // DELETE CHILD RECORDS FIRST
                    string deleteDetails = @"
                    DELETE FROM EnrollmentDetails
                    WHERE EnrolmentID = @ID";

                    SqlCommand cmd1 =
                        new SqlCommand(deleteDetails, con, trans);

                    cmd1.Parameters.AddWithValue(
                        "@ID",
                        enrolmentID);

                    cmd1.ExecuteNonQuery();

                    // DELETE RESULTS IF EXIST
                    string deleteResults = @"
                    DELETE FROM Results
                    WHERE EnrolmentID = @ID";

                    SqlCommand cmd2 =
                        new SqlCommand(deleteResults, con, trans);

                    cmd2.Parameters.AddWithValue(
                        "@ID",
                        enrolmentID);

                    cmd2.ExecuteNonQuery();

                    // DELETE MASTER RECORD
                    string deleteMaster = @"
                    DELETE FROM EnrollmentMaster
                    WHERE EnrolmentID = @ID";

                    SqlCommand cmd3 =
                        new SqlCommand(deleteMaster, con, trans);

                    cmd3.Parameters.AddWithValue(
                        "@ID",
                        enrolmentID);

                    cmd3.ExecuteNonQuery();

                    trans.Commit();

                    lblMessage.Text =
                        "Course dropped successfully.";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    lblMessage.Text =
                        "Error: " + ex.Message;
                }
            }

            LoadMyCourses();
        }
    }
}