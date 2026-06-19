using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class AdminViewEnrollment : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Protect page (must login first)
            if (Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("AccessDenied.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadSession();
            }
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("Login.aspx"); // go back to login
        }

        // Load session
        private void LoadSession()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Get session from EnrollmentMaster
                string query = @"SELECT DISTINCT Session
                                 FROM EnrollmentMaster
                                 ORDER BY Session";

                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                ddlSession.DataSource = dt;
                ddlSession.DataTextField = "Session";
                ddlSession.DataValueField = "Session";
                ddlSession.DataBind();

                ddlSession.Items.Insert(0, new ListItem("-- Select Session --", ""));
            }
        }

        // Load enrollment master
        private void LoadEnrollment()
        {
            lblMessage.Text = "";

            // If no session is selected
            if (ddlSession.SelectedValue == "")
            {
                gvEnrollment.Visible = false;
                lblMessage.Text = "Please select a session.";
                return;
            }

            gvEnrollment.Visible = true;

            using (SqlConnection conn = new SqlConnection(connStr)) 
            {
                conn.Open();

                // Get enrollment records based on selected session
                string query = @"
                    SELECT 
                        em.EnrolmentID, 
                        em.StudentID, 
                        u.FullName, 
                        em.Session, 
                        em.Semester, 
                        em.Status
                    FROM EnrollmentMaster em
                    INNER JOIN Students s 
                        ON em.StudentID = s.StudentID
                    INNER JOIN Users u
                        ON s.UserID = u.UserID
                    WHERE em.Session = @Session 
                    AND (@Status = '' OR em.Status = @Status)";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Pass selected session and status to SQL query
                cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);
                cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                // Fill data table with query result
                sda.Fill(dt);

                // Bind data to GrindView
                gvEnrollment.DataSource = dt;
                gvEnrollment.DataBind();
            }
        }

        // Load course details
        protected void gvEnrollment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Only run for data rows (not header)
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get current EnrolmentID from main GridView
                int enrolmentID = Convert.ToInt32(gvEnrollment.DataKeys[e.Row.RowIndex].Value);

                GridView gvCourses = (GridView)e.Row.FindControl("gvCourses");

                SqlConnection conn = new SqlConnection(connStr);

                // Get all course for this enrollment
                string query = @"
                    SELECT 
                        c.CourseCode, 
                        c.CourseName, 
                        c.CreditHours
                    FROM EnrollmentDetails ed
                    INNER JOIN CourseOfferings co
                        ON ed.OfferingID = co.OfferingID
                    INNER JOIN Courses c
                        ON co.CourseID = c.CourseID
                    WHERE ed.EnrolmentID = @EnrolmentID";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@EnrolmentID", enrolmentID);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                // Bind data to GrindView
                gvCourses.DataSource = dt;
                gvCourses.DataBind();
            }
        }

        // Session changed
        protected void ddlSession_SelectedIndexChanged(object senser, EventArgs e)
        {
            LoadEnrollment();
        }

        // Status changed
        protected void ddlStatus_SelectedIndexChanged(object senser, EventArgs e)
        {
            LoadEnrollment();
        }

        // Approve enrollment
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            // Get clicked button
            Button btn = (Button)sender;

            // Get enrollment ID from button
            int enrolmentID = Convert.ToInt32(btn.CommandArgument);

            SqlConnection conn = new SqlConnection(connStr);

            // Update enrollment status to Approved
            string query = @"UPDATE EnrollmentMaster
                             SET Status='Approved'
                             WHERE EnrolmentID=@EnrolmentID";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@EnrolmentID", enrolmentID);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            LoadEnrollment();
        }

        // Reject enrollment
        protected void btnReject_Click(object sender, EventArgs e)
        {
            // Get clicked button
            Button btn = (Button)sender;

            // Get enrollment ID from button
            int enrolmentID = Convert.ToInt32(btn.CommandArgument);

            SqlConnection conn = new SqlConnection(connStr);

            // Update enrollment status to Rejected
            string query = @"UPDATE EnrollmentMaster
                             SET Status='Rejected'
                             WHERE EnrolmentID=@EnrolmentID";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@EnrolmentID", enrolmentID);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            LoadEnrollment();
        }

        protected void btnStatistics_Click(object sender, EventArgs e)
        {
            Response.Redirect("EnrollmentStatistics.aspx");
        }
    }
}