using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class Announcements : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                LoadAnnouncements();
            }
        }

        // Load all announcements that are not tied to a specific offering
        private void LoadAnnouncements()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT AnnouncementID, Title, Message, PostedDateTime
                    FROM Announcements
                    WHERE OfferingID IS NULL
                    ORDER BY PostedDateTime DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAnnouncements.DataSource = dt;
                gvAnnouncements.DataBind();
            }
        }

        private void SearchAnnouncements()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Search announcements by title (case-insensitive)
                string query = @"
                    SELECT AnnouncementID, Title, Message, PostedDateTime
                    FROM Announcements
                    WHERE OfferingID IS NULL
                    AND Title LIKE @title
                    ORDER BY PostedDateTime DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", "%" + txtSearchTitle.Text.Trim() + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAnnouncements.DataSource = dt;
                gvAnnouncements.DataBind();
            }
        }

        // Handle posting a new announcement
        protected void btnPostAnnouncement_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() == "" || txtMessage.Text.Trim() == "")
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please fill in all fields!";
                return;
            }

            // Insert new announcement into the database
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    INSERT INTO Announcements (Title, Message, OfferingID)
                    VALUES (@title, @message, NULL)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                cmd.Parameters.AddWithValue("@message", txtMessage.Text.Trim());

                conn.Open();
                cmd.ExecuteNonQuery();

                lblMessage.CssClass = "text-success";
                lblMessage.Text = "Announcement posted successfully!";

                txtTitle.Text = "";
                txtMessage.Text = "";

                LoadAnnouncements();
            }
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
                    UPDATE Announcements
                    SET Title = @title,
                        Message = @message
                    WHERE AnnouncementID = @id
                    AND OfferingID IS NULL";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters.AddWithValue("@id", announcementId);

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

                // Delete announcement from the database
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"
                        DELETE FROM Announcements
                        WHERE AnnouncementID = @id
                        AND OfferingID IS NULL";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);

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