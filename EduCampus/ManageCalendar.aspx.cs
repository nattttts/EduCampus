using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class ManageCalendar : System.Web.UI.Page
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
                LoadCalendar();
            }
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("Login.aspx"); // go back to login
        }

        // Load event list
        private void LoadCalendar()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT * 
                    FROM AcademicCalendar
                    ORDER BY StartDate, EndDate";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCalendar.DataSource = dt;
                gvCalendar.DataBind();
            }
        }

        // Format date display for single-day or multi-day events
        protected string FormatDate(object start, object end) 
        {
            DateTime startDate = Convert.ToDateTime(start);
            DateTime endDate = Convert.ToDateTime(end);

            // Same date
            if (startDate == endDate)
                return startDate.ToString("dd MMM yyyy");

            // Same year and month
            else if (startDate.Month == endDate.Month && startDate.Year == endDate.Year)
                return $"{startDate:dd} - {endDate:dd MMM yyyy}";
            
            // Same year but different month 
            else if (startDate.Month != endDate.Month && startDate.Year == endDate.Year)
                return $"{startDate:dd MMM} - {endDate:dd MMM yyyy}";

            // Different year
            else
                return $"{startDate:dd MMM yyyy} - {endDate:dd MMM yyyy}";
        }

        private void SearchCalendar()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Search academic calendar
                string query = @"
                    SELECT *
                    FROM AcademicCalendar
                    WHERE Session LIKE @search
                        OR Event LIKE @search
                    ORDER BY StartDate, EndDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text.Trim() + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCalendar.DataSource = dt;
                gvCalendar.DataBind();
            }
        }

        // Add academic calendar event
        protected void btnSave_Click(object sender, EventArgs e) 
        {
            try
            {
                // Validate that all fields have been filled in
                if (txtStartDate.Text == "" || txtEndDate.Text == "" || txtEvent.Text.Trim() == "")
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Please fill in all fields.";
                    return;
                }

                // Ensure end date is not earlier than start date
                DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
                DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

                if (endDate < startDate)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "End date must be on or after start date.";
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Insert academic calendar event
                    string query = @"INSERT INTO AcademicCalendar (Session, StartDate, EndDate, Event) 
                                     VALUES (@session, @startDate, @endDate, @event)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@session", ddlSession.SelectedValue);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@event", txtEvent.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Academic calendar event added successfully!";

                // Clear form fields after successful add event
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtEvent.Text = "";

                LoadCalendar();
            }
            catch (Exception ex)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Error: " + ex.Message;
            }
        }

        // Clear button
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            txtEvent.Text = "";
            lblMsg.Text = "";
        }

        // Search
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchCalendar();
        }

        private void RefreshCalendarGrid()
        {
            if (txtSearch.Text.Trim() == "")
                LoadCalendar();
            else
                SearchCalendar();
        }

        // Reset
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            RefreshCalendarGrid();
        }

        // Edit mode
        protected void gvCalendar_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvCalendar.EditIndex = e.NewEditIndex;
            RefreshCalendarGrid();
        }

        // Update
        protected void gvCalendar_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvCalendar.DataKeys[e.RowIndex].Value);

            // Get current row in edit mode
            GridViewRow row = gvCalendar.Rows[e.RowIndex];

            // Get input controls from EditItemTemplate
            TextBox txtStartDate = (TextBox)row.FindControl("txtEditStartDate");
            TextBox txtEndDate = (TextBox)row.FindControl("txtEditEndDate");
            TextBox txtEvent = (TextBox)row.FindControl("txtEditEvent");

            // Validate that all fields have been filled in
            if (txtStartDate.Text == "" || txtEndDate.Text == "" || txtEvent.Text.Trim() == "")
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please fill in all fields.";

                gvCalendar.EditIndex = -1;
                RefreshCalendarGrid();
                return;
            }

            // Validate that the event end date must later than start date
            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

            if (endDate < startDate)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "End date cannot be earlier than start date.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Update academic calendar information
                string query = @"UPDATE AcademicCalendar 
                                 SET StartDate=@startDate, EndDate=@endDate, Event=@event 
                                 WHERE CalendarID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@event", txtEvent.Text.Trim());

                cmd.ExecuteNonQuery();
            }

            // Exit edit mode and refresh the academic calendar list
            gvCalendar.EditIndex = -1;
            RefreshCalendarGrid();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Academic calendar event updated successfully!";
        }

        // Cancel
        protected void gvCalendar_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvCalendar.EditIndex = -1;
            RefreshCalendarGrid();
        }

        // Delete
        protected void gvCalendar_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvCalendar.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Delete event
                string query = "DELETE FROM AcademicCalendar WHERE CalendarID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            // Refresh the academic calendar list after successful deletion
            RefreshCalendarGrid();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Academic calendar event deleted successfully!";
        }
    }
}