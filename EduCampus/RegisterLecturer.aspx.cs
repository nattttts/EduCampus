using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class RegisterLecturer : System.Web.UI.Page
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
                LoadLecturers();
            }
        }

        // Hashes the password using SHA-256 before storing it in the database
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        // Loads all registered lecturers into the GridView
        private void LoadLecturers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Join Lecturers and Users tables to retrieve lecturer details
                string query = @"
                    SELECT 
                        l.LecturerID, 
                        u.FullName, 
                        u.Email, 
                        l.Department  
                    FROM Lecturers l  
                    INNER JOIN Users u 
                        ON l.UserID = u.UserID";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvLecturers.DataSource = dt;
                gvLecturers.DataBind();
            }
        }

        private void SearchLecturers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Search lecturers by department
                string query = @"
                    SELECT
                        l.LecturerID,
                        u.FullName,
                        u.Email,
                        l.Department
                    FROM Lecturers l
                    INNER JOIN Users u
                        ON l.UserID = u.UserID
                    WHERE l.Department LIKE @dept";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dept", "%" + txtSearchDept.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvLecturers.DataSource = dt;
                gvLecturers.DataBind();
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Validate that all required fields have been filled in
            if (txtName.Text == "" || txtLecturerEmail.Text == "" || txtLecturerPass.Text == "" || txtDept.Text == "")
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please fill in all fields!";
                return;
            }

            // Hash password before saving it to the database
            string hashedPassword = HashPassword(txtLecturerPass.Text);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    // Check whether the email already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @email";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@email", txtLecturerEmail.Text);

                    conn.Open();

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        lblMessage.CssClass = "text-danger";
                        lblMessage.Text = "Email already exists. Please use another email.";
                        return;
                    }

                    // Insert lecturer login information into Users table
                    string userQuery = @"
                        INSERT INTO Users (FullName, Email, PasswordHash, Role) 
                        VALUES (@name, @email, @password, 'Lecturer');

                        SELECT SCOPE_IDENTITY();";

                    SqlCommand userCmd = new SqlCommand(userQuery, conn);

                    userCmd.Parameters.AddWithValue("@name", txtName.Text);
                    userCmd.Parameters.AddWithValue("@email", txtLecturerEmail.Text);
                    userCmd.Parameters.AddWithValue("@password", hashedPassword);

                    // Retrieve the UserID generated by SQL Server
                    int userID = Convert.ToInt32(userCmd.ExecuteScalar());

                    // Insert lecturer department into Lecturers table
                    string lecturerQuery = @"
                        INSERT INTO Lecturers (Department, UserID)
                        VALUES (@dept, @userID)";

                    SqlCommand lecturerCmd = new SqlCommand(lecturerQuery, conn);
                    
                    lecturerCmd.Parameters.AddWithValue("@dept", txtDept.Text);
                    lecturerCmd.Parameters.AddWithValue("@userID", userID);

                    lecturerCmd.ExecuteNonQuery();

                    lblMessage.CssClass = "text-success";
                    lblMessage.Text = "Lecturer registered successfully!";

                    // Clear form fields after successful registration
                    txtName.Text = "";
                    txtLecturerEmail.Text = "";
                    txtLecturerPass.Text = "";
                    txtDept.Text = "";

                    // Refresh lecturer list
                    LoadLecturers(); 
                }
                catch (Exception ex)
                {
                    lblMessage.CssClass = "text-danger";
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchLecturers();
        }

        private void RefreshLecturerGrid()
        {
            if (txtSearchDept.Text.Trim() == "")
                LoadLecturers();
            else
                SearchLecturers();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchDept.Text = "";
            RefreshLecturerGrid();
        }

        // Edit mode
        protected void gvLecturer_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvLecturers.EditIndex = e.NewEditIndex;
            RefreshLecturerGrid();
        }

        // Update lecturer information
        protected void gvLecturer_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int lecturerId = Convert.ToInt32(gvLecturers.DataKeys[e.RowIndex].Value);

            string fullName = ((TextBox)gvLecturers.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            string email = ((TextBox)gvLecturers.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            string department = ((TextBox)gvLecturers.Rows[e.RowIndex].Cells[3].Controls[0]).Text;

            // Validate that all fields have been filled in
            if (fullName.Trim() == "" || email.Trim() == "" || department.Trim() == "")
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please fill in all fields.";

                gvLecturers.EditIndex = -1;
                RefreshLecturerGrid();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                
                // Retrieve the UserID associated with the LecturerID
                string getUserQuery = "SELECT UserID FROM Lecturers WHERE LecturerID = @lecturerId";

                SqlCommand getUserCmd = new SqlCommand(getUserQuery, conn);

                getUserCmd.Parameters.AddWithValue("@lecturerId", lecturerId);

                int userId = Convert.ToInt32(getUserCmd.ExecuteScalar());

                string checkEmailQuery = @"
                    SELECT COUNT(*)
                    FROM Users
                    WHERE Email = @Email
                    AND UserID <> @UserID";

                SqlCommand checkCmd = new SqlCommand(checkEmailQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);
                checkCmd.Parameters.AddWithValue("@UserID", userId);

                int emailExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (emailExists > 0)
                {
                    lblMessage.CssClass = "text-danger";
                    lblMessage.Text = "Email already exists. Please use another email.";
                    return; // Stop update here
                }

                string updateUserQuery = @"
                    UPDATE Users
                    SET FullName = @name,
                        Email = @email
                    WHERE UserID = @userId";

                SqlCommand updateUserCmd = new SqlCommand(updateUserQuery, conn);

                updateUserCmd.Parameters.AddWithValue("@name", fullName);
                updateUserCmd.Parameters.AddWithValue("@email", email);
                updateUserCmd.Parameters.AddWithValue("@userId", userId);

                updateUserCmd.ExecuteNonQuery();

                // Update lecturer department in Lecturers table
                string updateLecturerQuery = @"
                    UPDATE Lecturers
                    SET Department = @department
                    WHERE LecturerID = @lecturerId";

                SqlCommand updateLecturerCmd = new SqlCommand(updateLecturerQuery, conn);

                updateLecturerCmd.Parameters.AddWithValue("@department", department);
                updateLecturerCmd.Parameters.AddWithValue("@lecturerId", lecturerId);

                updateLecturerCmd.ExecuteNonQuery();
            }

            // Exit edit mode and refresh the lecturer list
            gvLecturers.EditIndex = -1;
            RefreshLecturerGrid();

            lblMessage.CssClass = "text-success";
            lblMessage.Text = "Lecturer updated successfully!";
        }

        // Cancel
        protected void gvLecturer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvLecturers.EditIndex = -1;
            RefreshLecturerGrid();
        }

        // Delete lecturer
        protected void gvLecturer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                GridViewRow row = (GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;

                int index = row.RowIndex;
                int lecturerId = Convert.ToInt32(gvLecturers.DataKeys[index].Value);

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Check if lecturer is assigned to any course offerings
                    string checkQuery = @"
                        SELECT COUNT(*)
                        FROM CourseOfferings
                        WHERE LecturerID = @id";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@id", lecturerId);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    // If lecturer is assigned to courses, prevent deletion and show error message
                    if (count > 0)
                    {
                        lblMessage.CssClass = "text-danger";
                        lblMessage.Text = $"Cannot delete lecturer: lecturer is assigned to {count} course offerings.";
                        return;
                    }

                    // Get UserID 
                    string getUserQuery = "SELECT UserID FROM Lecturers WHERE LecturerID = @id";
                    SqlCommand getUserCmd = new SqlCommand(getUserQuery, conn);
                    getUserCmd.Parameters.AddWithValue("@id", lecturerId);

                    int userId = Convert.ToInt32(getUserCmd.ExecuteScalar());

                    // Delete lecturer 
                    string deleteLecturer = "DELETE FROM Lecturers WHERE LecturerID = @id";
                    SqlCommand delLecturerCmd = new SqlCommand(deleteLecturer, conn);
                    delLecturerCmd.Parameters.AddWithValue("@id", lecturerId);
                    delLecturerCmd.ExecuteNonQuery();

                    // Delete user account
                    string deleteUser = "DELETE FROM Users WHERE UserID = @uid";
                    SqlCommand delUserCmd = new SqlCommand(deleteUser, conn);
                    delUserCmd.Parameters.AddWithValue("@uid", userId);
                    delUserCmd.ExecuteNonQuery();
                }

                RefreshLecturerGrid();

                lblMessage.CssClass = "text-success";
                lblMessage.Text = "Lecturer deleted successfully!";
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