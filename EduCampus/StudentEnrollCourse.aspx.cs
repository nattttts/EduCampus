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
        private int? GetStudentID(SqlConnection con, SqlTransaction trans)
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

                return Convert.ToInt32(result);
            }
        }

        // ================= LOAD SESSION =================
        private void LoadSessions()
        {
            ddlSession.Items.Clear();
            ddlSession.Items.Add(new ListItem("2025", "2025"));
            ddlSession.Items.Add(new ListItem("2026", "2026"));
            ddlSession.Items.Add(new ListItem("2027", "2027"));
            ddlSession.SelectedValue = "2026";
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
                INNER JOIN Courses c ON co.CourseID = c.CourseID
                WHERE co.Session = @Session";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);

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
                    int? studentID = GetStudentID(con, trans);

                    if (studentID == null)
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
                    lblMessage.Text = ex.Message;
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
                    string deleteMaster = @"
                    DELETE FROM EnrollmentMaster
                    WHERE EnrolmentID = @ID";

                    SqlCommand cmd = new SqlCommand(deleteMaster, con, trans);
                    cmd.Parameters.AddWithValue("@ID", enrolmentID);
                    cmd.ExecuteNonQuery();

                    trans.Commit();

                    lblMessage.ForeColor = Color.Green;
                    lblMessage.Text = "Course dropped successfully!";

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