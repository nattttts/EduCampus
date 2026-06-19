using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Drawing;

namespace EduCampus
{
    public partial class StudentEnrollCourse : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        // ================= PAGE LOAD =================
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
                LoadSessions();
                LoadCourses();
                LoadMyCourses();
            }
        }

        // ================= GET STUDENT ID =================
        private string GetStudentID(SqlConnection con, SqlTransaction trans)
        {
            string query = @"
        SELECT StudentID
        FROM Students
        WHERE UserID = (
            SELECT UserID FROM Users WHERE Email = @Email
        )";

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return null;

                return result.ToString();
            }
        }

        // ================= LOAD SESSION =================
        private void LoadSessions()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT DISTINCT Session
                FROM CourseOfferings
                ORDER BY Session";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                ddlSession.Items.Clear();

                while (dr.Read())
                {
                    ddlSession.Items.Add(
                        new ListItem(
                            dr["Session"].ToString(),
                            dr["Session"].ToString()
                        )
                    );
                }
            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourses();
        }

        protected void ddlSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourses();
        }

        // ================= LOAD COURSES =================
        private void LoadCourses()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT
                    c.CourseID,
                    c.CourseCode,
                    c.CourseName,
                    c.CreditHours
                FROM CourseOfferings co
                INNER JOIN Courses c
                    ON co.CourseID = c.CourseID
                WHERE co.Session = @Session

                AND c.CourseID NOT IN
                (
                    SELECT co2.CourseID
                    FROM EnrollmentMaster em
                    INNER JOIN EnrollmentDetails ed
                        ON em.EnrolmentID = ed.EnrolmentID
                    INNER JOIN CourseOfferings co2
                        ON ed.OfferingID = co2.OfferingID
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
                    )
                )";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCourses.DataKeyNames = new string[] { "CourseID" };
                gvCourses.DataSource = dt;
                gvCourses.DataBind();
            }
        }

        // ================= ENROLL =================
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    string studentID = GetStudentID(con, trans);

                    if (string.IsNullOrEmpty(studentID))
                        throw new Exception("Student record not found.");

                    // ================= INSERT MASTER =================
                    string insertMaster = @"
                        INSERT INTO EnrollmentMaster
                        (DateEnrolled, Status, Session, Semester, StudentID)
                        VALUES
                        (GETDATE(), 'Pending', @Session, @Semester, @StudentID);

                        SELECT SCOPE_IDENTITY();";

                    int enrolmentID;

                    using (SqlCommand cmd = new SqlCommand(insertMaster, con, trans))
                    {
                        cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);
                        cmd.Parameters.AddWithValue("@Semester", ddlSemester.SelectedValue);
                        cmd.Parameters.AddWithValue("@StudentID", studentID);

                        object result = cmd.ExecuteScalar();

                        if (result == null)
                            throw new Exception("Failed to create EnrollmentMaster.");

                        enrolmentID = Convert.ToInt32(result);
                    }

                    bool hasSelection = false;

                    // ================= INSERT DETAILS =================
                    foreach (GridViewRow row in gvCourses.Rows)
                    {
                        CheckBox chk = row.FindControl("chkSelect") as CheckBox;

                        if (chk != null && chk.Checked)
                        {
                            hasSelection = true;

                            int courseID = Convert.ToInt32(gvCourses.DataKeys[row.RowIndex].Value);

                            string getOffering = @"
                                SELECT TOP 1 OfferingID
                                FROM CourseOfferings
                                WHERE CourseID = @CourseID
                                AND Session = @Session";

                            object offeringObj;

                            using (SqlCommand offerCmd = new SqlCommand(getOffering, con, trans))
                            {
                                offerCmd.Parameters.AddWithValue("@CourseID", courseID);
                                offerCmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);
                                offerCmd.Parameters.AddWithValue("@Semester", ddlSemester.SelectedValue);

                                offeringObj = offerCmd.ExecuteScalar();
                            }

                            if (offeringObj == null)
                                throw new Exception("Course offering not found for CourseID: " + courseID);

                            int offeringID = Convert.ToInt32(offeringObj);

                            string insertDetail = @"
                                INSERT INTO EnrollmentDetails
                                (EnrolmentID, OfferingID)
                                VALUES (@EnrolmentID, @OfferingID)";

                            using (SqlCommand detailCmd = new SqlCommand(insertDetail, con, trans))
                            {
                                detailCmd.Parameters.AddWithValue("@EnrolmentID", enrolmentID);
                                detailCmd.Parameters.AddWithValue("@OfferingID", offeringID);
                                detailCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    if (!hasSelection)
                        throw new Exception("Please select at least one course.");

                    trans.Commit();

                    lblMessage.ForeColor = Color.Green;
                    lblMessage.Text = "Enrollment successful!";

                    LoadMyCourses();
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = ex.ToString();
                }
            }
        }

        // ================= LOAD MY COURSES =================
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
                    INNER JOIN EnrollmentDetails ed ON em.EnrolmentID = ed.EnrolmentID
                    INNER JOIN CourseOfferings co ON ed.OfferingID = co.OfferingID
                    INNER JOIN Courses c ON co.CourseID = c.CourseID
                    WHERE em.StudentID = (
                        SELECT StudentID 
                        FROM Students 
                        WHERE UserID = (
                            SELECT UserID FROM Users WHERE Email = @Email
                        )
                    )";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvEnrollment.DataSource = dt;
                gvEnrollment.DataBind();
            }
        }

        // ================= DROP COURSE =================
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
                    // Check if attendance exists
                    string checkAttendance = @"
                    SELECT COUNT(*)
                    FROM Attendance a
                    INNER JOIN EnrollmentDetails ed
                        ON a.DetailID = ed.DetailID
                    WHERE ed.EnrolmentID = @ID";

                    SqlCommand checkCmd = new SqlCommand(checkAttendance, con, trans);
                    checkCmd.Parameters.AddWithValue("@ID", enrolmentID);

                    int attendanceCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (attendanceCount > 0)
                    {
                        lblMessage.ForeColor = Color.Red;
                        lblMessage.Text = "This course already has attendance records and cannot be dropped.";
                        trans.Rollback();
                        return;
                    }

                    // Delete CourseMarks first (if any)
                    string deleteMarks = @"
                    DELETE FROM CourseMarks
                    WHERE DetailID IN (
                        SELECT DetailID
                        FROM EnrollmentDetails
                        WHERE EnrolmentID = @ID
                    )";

                    SqlCommand cmdMarks = new SqlCommand(deleteMarks, con, trans);
                    cmdMarks.Parameters.AddWithValue("@ID", enrolmentID);
                    cmdMarks.ExecuteNonQuery();

                    // Delete EnrollmentDetails
                    string deleteDetails = @"
                    DELETE FROM EnrollmentDetails
                    WHERE EnrolmentID = @ID";

                    SqlCommand cmdDetails = new SqlCommand(deleteDetails, con, trans);
                    cmdDetails.Parameters.AddWithValue("@ID", enrolmentID);
                    cmdDetails.ExecuteNonQuery();

                    // Delete EnrollmentMaster
                    string deleteMaster = @"
                    DELETE FROM EnrollmentMaster
                    WHERE EnrolmentID = @ID";

                    SqlCommand cmdMaster = new SqlCommand(deleteMaster, con, trans);
                    cmdMaster.Parameters.AddWithValue("@ID", enrolmentID);
                    cmdMaster.ExecuteNonQuery();

                    trans.Commit();

                    lblMessage.ForeColor = Color.Green;
                    lblMessage.Text = "Course dropped successfully.";

                    LoadMyCourses();
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = ex.Message;
                }
            }
        }

        // ================= LOGOUT =================
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}