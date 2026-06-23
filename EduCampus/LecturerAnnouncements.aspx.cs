using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class LecturerAnnouncements : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["Role"].ToString() != "Lecturer")
            {
                Response.Redirect("AccessDenied.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadOfferings();
                LoadAnnouncements();
            }
        }

        // Load course offerings assigned to the lecturer
        private void LoadOfferings()
        {
            int lecturerId = GetLecturerId();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT co.OfferingID,
                           c.CourseName + ' (' + co.Session + ')' AS DisplayText
                    FROM CourseOfferings co
                    INNER JOIN Courses c ON co.CourseID = c.CourseID
                    WHERE co.LecturerID = @lecturerId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@lecturerId", lecturerId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlOffering.DataSource = dt;
                ddlOffering.DataTextField = "DisplayText";
                ddlOffering.DataValueField = "OfferingID";
                ddlOffering.DataBind();

                ddlOffering.Items.Insert(0, new ListItem("-- Select Course --", ""));

            }
        }

        private void LoadAnnouncements()
        {
            int lecturerId = GetLecturerId();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT
                        a.AnnouncementID,
                        a.Title,
                        a.Message,
                        a.PostedDateTime,
                        c.CourseName + ' (' + co.Session + ')' AS CourseOffering
                    FROM Announcements a
                    INNER JOIN CourseOfferings co
                        ON a.OfferingID = co.OfferingID
                    INNER JOIN Courses c
                        ON co.CourseID = c.CourseID
                    WHERE co.LecturerID = @lecturerId
                    ORDER BY a.PostedDateTime DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@lecturerId", lecturerId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAnnouncements.DataSource = dt;
                gvAnnouncements.DataBind();
            }
        }

        private int GetLecturerId()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT l.LecturerID
                    FROM Lecturers l
                    INNER JOIN Users u ON l.UserID = u.UserID
                    WHERE u.Email = @Email";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void SearchAnnouncements()
        {
            int lecturerId = GetLecturerId();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
            SELECT
                a.AnnouncementID,
                a.Title,
                a.Message,
                a.PostedDateTime,
                c.CourseName + ' (' + co.Session + ')' AS CourseOffering
            FROM Announcements a
            INNER JOIN CourseOfferings co
                ON a.OfferingID = co.OfferingID
            INNER JOIN Courses c
                ON co.CourseID = c.CourseID
            WHERE co.LecturerID = @lecturerId
            AND (
                a.Title LIKE @search
                OR c.CourseName LIKE @search
                OR co.Session LIKE @search
            )
            ORDER BY a.PostedDateTime DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@lecturerId", lecturerId);
                cmd.Parameters.AddWithValue("@search", "%" + txtSearchTitle.Text.Trim() + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAnnouncements.DataSource = dt;
                gvAnnouncements.DataBind();
            }
        }

        protected void btnPostAnnouncement_Click(object sender, EventArgs e)
        {
            if (ddlOffering.SelectedValue == "" || txtTitle.Text.Trim() == "" ||
                txtMessage.Text.Trim() == "")
            {
                lblMessage.Text = "Please fill in all fields!";
                lblMessage.CssClass = "text-danger";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    INSERT INTO Announcements (Title, Message, OfferingID)
                    VALUES (@title, @message, @offeringId)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                cmd.Parameters.AddWithValue("@message", txtMessage.Text.Trim());
                cmd.Parameters.AddWithValue("@offeringId", ddlOffering.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = "Announcement posted successfully!";
            lblMessage.CssClass = "text-success";

            txtTitle.Text = "";
            txtMessage.Text = "";
            ddlOffering.SelectedIndex = 0;

            LoadAnnouncements();
        }

        // Handle searching announcements by title
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchAnnouncements();
        }

        private void RefreshAnnouncementGrid()
        {
            if (txtSearchTitle.Text.Trim() == "")
                LoadAnnouncements();
            else
                SearchAnnouncements();
        }

        // Handle resetting the search
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchTitle.Text = "";
            RefreshAnnouncementGrid();
        }

        // Handle editing an announcement
        protected void gvAnnouncements_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAnnouncements.EditIndex = e.NewEditIndex;
            RefreshAnnouncementGrid();
        }

        // Cancel editing an announcement
        protected void gvAnnouncements_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAnnouncements.EditIndex = -1;
            RefreshAnnouncementGrid();
        }

        // Handle updating an announcement
        protected void gvAnnouncements_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int lecturerId = GetLecturerId();

            int announcementId = Convert.ToInt32(gvAnnouncements.DataKeys[e.RowIndex].Value);

            string title = ((TextBox)gvAnnouncements.Rows[e.RowIndex].FindControl("txtEditTitle")).Text;

            string message = ((TextBox)gvAnnouncements.Rows[e.RowIndex].FindControl("txtEditMessage")).Text;

            if (title.Trim() == "" || message.Trim() == "")
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Fields cannot be empty!";

                gvAnnouncements.EditIndex = -1;
                RefreshAnnouncementGrid();
                return;
            }

            // Update announcement in the database
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    UPDATE a
                    SET a.Title = @title,
                        a.Message = @message
                    FROM Announcements a
                    INNER JOIN CourseOfferings co
                        ON a.OfferingID = co.OfferingID
                    WHERE a.AnnouncementID = @id
                    AND co.LecturerID = @lecturerId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters.AddWithValue("@id", announcementId);
                cmd.Parameters.AddWithValue("@lecturerId", lecturerId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvAnnouncements.EditIndex = -1;
            RefreshAnnouncementGrid();

            lblMessage.CssClass = "text-success";
            lblMessage.Text = "Announcement updated successfully!";
        }

        // Handle deleting an announcement
        protected void gvAnnouncements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                GridViewRow row = (GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
                int index = row.RowIndex;

                int id = Convert.ToInt32(gvAnnouncements.DataKeys[index].Value);
                int lecturerId = GetLecturerId();

                // Delete announcement from the database
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"
                        DELETE a
                        FROM Announcements a
                        INNER JOIN CourseOfferings co
                            ON a.OfferingID = co.OfferingID
                        WHERE a.AnnouncementID = @id
                        AND co.LecturerID = @lecturerId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@lecturerId", lecturerId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                RefreshAnnouncementGrid();

                lblMessage.CssClass = "text-success";
                lblMessage.Text = "Announcement deleted successfully!";
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