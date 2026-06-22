using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class AssignLecturer : System.Web.UI.Page
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
                ddlSession.SelectedIndex = 0;
                LoadLecturers();
                LoadCourses();
                LoadAssignments();
            }
        }

        // Loads all lecturers into the lecturer dropdown list
        private void LoadLecturers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Join Lecturers and Users tables to retrieve lecturer details
                string query = @"
                    SELECT
                        l.LecturerID,
                        u.FullName
                    FROM Lecturers l
                    INNER JOIN Users u
                        ON l.UserID = u.UserID";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlLecturer.DataSource = dt;
                ddlLecturer.DataTextField = "FullName";
                ddlLecturer.DataValueField = "LecturerID";
                ddlLecturer.DataBind();
            }
        }

        // Loads courses that have not yet been assigned in the selected session
        private void LoadCourses()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT
                        CourseID,
                        CourseName + ' (' + CourseCode + ')' AS DisplayName
                    FROM Courses
                    WHERE CourseID NOT IN (
                        SELECT CourseID
                        FROM CourseOfferings
                        WHERE Session = @session
                    )";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);

                da.SelectCommand.Parameters.AddWithValue("@session", ddlSession.SelectedValue);

                DataTable dt = new DataTable();
                da.Fill(dt);

                cblCourses.DataSource = dt;
                cblCourses.DataTextField = "DisplayName";
                cblCourses.DataValueField = "CourseID";
                cblCourses.DataBind();
            }
        }

        // Refresh available courses whenever a different session is selected
        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourses();
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                bool anySelected = false;

                foreach (ListItem item in cblCourses.Items)
                {
                    if (item.Selected)
                    {
                        // Check whether the course has already been assigned
                        // in the selected academic session
                        string checkQuery = @"
                            SELECT COUNT(*)
                            FROM CourseOfferings
                            WHERE CourseID = @cid
                            AND Session = @session";

                        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@cid", item.Value);
                        checkCmd.Parameters.AddWithValue("@session", ddlSession.SelectedValue);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            lblMsg.Text = "One or more selected courses already assigned for this session!";
                            lblMsg.CssClass = "text-danger";
                            continue; // skip this course
                        }


                        anySelected = true;

                        // Create a new course offering by assigning
                        // the selected lecturer to the selected course
                        string query = @"
                            INSERT INTO CourseOfferings
                            (Session, CourseID, LecturerID)
                            VALUES
                            (@session, @cid, @lid)";
                        SqlCommand cmd = new SqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@session", ddlSession.SelectedValue);
                        cmd.Parameters.AddWithValue("@cid", item.Value);
                        cmd.Parameters.AddWithValue("@lid", ddlLecturer.SelectedValue);

                        cmd.ExecuteNonQuery();
                    }
                }

                // Ensure at least one course was selected
                if (!anySelected)
                {
                    lblMsg.Text = "Please select at least one subject!";
                    lblMsg.CssClass = "text-danger";
                    return;
                }

                lblMsg.Text = "Subjects assigned successfully!";
                lblMsg.CssClass = "text-success";

                // Refresh available courses and assignment list
                LoadCourses();
                LoadAssignments();
            }
        }

        // Loads all lecturer-course assignments into the GridView
        private void LoadAssignments()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Retrieve lecturer, session, and course information
                // for all course offerings
                string query = @"
                SELECT
                    co.OfferingID,
                    u.FullName,
                    co.Session,
                    c.CourseName,
                    c.CourseCode
                FROM CourseOfferings co
                INNER JOIN Lecturers l
                    ON co.LecturerID = l.LecturerID
                INNER JOIN Users u
                    ON l.UserID = u.UserID
                INNER JOIN Courses c
                    ON co.CourseID = c.CourseID";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAssign.DataSource = dt;
                gvAssign.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Search assignment by lecturer name/course name/course code
                string query = @"
                    SELECT
                        co.OfferingID,
                        u.FullName,
                        co.Session,
                        c.CourseName,
                        c.CourseCode
                    FROM CourseOfferings co
                    INNER JOIN Lecturers l
                        ON co.LecturerID = l.LecturerID
                    INNER JOIN Users u
                        ON l.UserID = u.UserID
                    INNER JOIN Courses c
                        ON co.CourseID = c.CourseID
                    WHERE u.FullName LIKE @search
                       OR c.CourseName LIKE @search
                       OR c.CourseCode LIKE @search";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + txtSearchAssignment.Text.Trim() + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAssign.DataSource = dt;
                gvAssign.DataBind();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchAssignment.Text = "";
            LoadAssignments();
        }

        protected void gvAssign_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                // Retrieve the selected course offering ID
                int index = Convert.ToInt32(e.CommandArgument);
                int offeringId = Convert.ToInt32(gvAssign.DataKeys[index].Value);

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Prevent deletion if students are already enrolled
                    // in the course offering
                    string checkQuery = @"
                        SELECT COUNT(*)
                        FROM EnrollmentDetails
                        WHERE OfferingID = @id";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@id", offeringId);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        lblMsg.Text = "Cannot delete: students are already enrolled in this course offering.";
                        lblMsg.CssClass = "text-danger";
                        return;
                    }

                    // Delete the course offering if there are no enrolments
                    string query = "DELETE FROM CourseOfferings WHERE OfferingID = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", offeringId);

                    cmd.ExecuteNonQuery();
                }

                lblMsg.Text = "Deleted successfully!";
                lblMsg.CssClass = "text-success";

                // Refresh course list and assignment table
                LoadCourses();
                LoadAssignments();
            }
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("Login.aspx"); // go back to login
        }
    }
}