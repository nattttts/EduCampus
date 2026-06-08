using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class RegisterStudent : System.Web.UI.Page
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
                LoadProgrammes();
                LoadStudents();
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

        // Loads all registered students into the GridView
        private void LoadStudents()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Join Students, Users, and Programmes tables to retrieve students' details
                string query = @"
                    SELECT
                        s.StudentID,
                        u.FullName,
                        u.Email,
                        p.ProgrammeID,
                        p.ProgrammeName
                    FROM Students s
                    INNER JOIN Users u
                        ON s.UserID = u.UserID
                    INNER JOIN Programmes p
                        ON s.ProgrammeID = p.ProgrammeID";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
        }

        // Loads all programmes into the dropdown list for selection during student registration
        private void LoadProgrammes()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT ProgrammeID, ProgrammeName
                    FROM Programmes";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlProgramme.DataSource = dt;
                ddlProgramme.DataTextField = "ProgrammeName";
                ddlProgramme.DataValueField = "ProgrammeID";
                ddlProgramme.DataBind();
                ddlProgramme.Items.Insert(0, new ListItem("-- Select Programme --", ""));
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Validate that all required fields have been filled in
            if (txtStudentID.Text == "" || txtName.Text == "" || txtStudentEmail.Text == "" || txtStudentPass.Text == "" || ddlProgramme.SelectedValue == "")
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please fill in all fields!";
                return;
            }

            // Hash password before saving it to the database
            string hashedPassword = HashPassword(txtStudentPass.Text);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    // Check whether the email already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @email";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@email", txtStudentEmail.Text);

                    conn.Open();

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        lblMessage.CssClass = "text-danger";
                        lblMessage.Text = "Email already exists. Please use another email.";
                        return;
                    }

                    // Check whether the student ID already exists
                    string studentId = txtStudentID.Text.Trim().ToUpper();

                    if (studentId.Length > 10)
                    {
                        lblMessage.CssClass = "text-danger";
                        lblMessage.Text = "Student ID cannot exceed 10 characters.";
                        return;
                    }

                    string checkStudentQuery = @"
                        SELECT COUNT(*) 
                        FROM Students 
                        WHERE StudentID = @studentId";

                    SqlCommand checkStudentCmd = new SqlCommand(checkStudentQuery, conn);

                    checkStudentCmd.Parameters.AddWithValue("@studentId", studentId);

                    int studentCount = Convert.ToInt32(checkStudentCmd.ExecuteScalar());

                    if (studentCount > 0)
                    {
                        lblMessage.CssClass = "text-danger";
                        lblMessage.Text = "Student ID already exists.";
                        return;
                    }

                    // Insert student login information into Users table
                    string userQuery = @"
                        INSERT INTO Users
                        (FullName, Email, PasswordHash, Role)
                        VALUES
                        (@name, @email, @password, 'Student');

                        SELECT SCOPE_IDENTITY();";

                    SqlCommand userCmd = new SqlCommand(userQuery, conn);

                    userCmd.Parameters.AddWithValue("@name", txtName.Text);
                    userCmd.Parameters.AddWithValue("@email", txtStudentEmail.Text);
                    userCmd.Parameters.AddWithValue("@password", hashedPassword);

                    // Retrieve the UserID generated by SQL Server
                    int userID = Convert.ToInt32(userCmd.ExecuteScalar());

                    // Insert student information into Students table
                    string studentQuery = @"
                        INSERT INTO Students
                        (StudentID, UserID, ProgrammeID)
                        VALUES
                        (@studentId, @userID, @programmeId)";

                    SqlCommand studentCmd = new SqlCommand(studentQuery, conn);

                    studentCmd.Parameters.AddWithValue("@studentId", txtStudentID.Text);
                    studentCmd.Parameters.AddWithValue("@userID", userID);
                    studentCmd.Parameters.AddWithValue("@programmeId", ddlProgramme.SelectedValue);

                    studentCmd.ExecuteNonQuery();

                    lblMessage.CssClass = "text-success";
                    lblMessage.Text = "Student registered successfully!";

                    // Clear form fields after successful registration
                    txtStudentID.Text = "";
                    txtName.Text = "";
                    txtStudentEmail.Text = "";
                    txtStudentPass.Text = "";
                    ddlProgramme.SelectedIndex = 0;

                    // Refresh student list
                    LoadStudents();
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
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Search students by programme
                string query = @"
                    SELECT
                        s.StudentID,
                        u.FullName,
                        u.Email,
                        p.ProgrammeID,
                        p.ProgrammeName
                    FROM Students s
                    INNER JOIN Users u
                        ON s.UserID = u.UserID
                    INNER JOIN Programmes p
                        ON s.ProgrammeID = p.ProgrammeID
                    WHERE p.ProgrammeName LIKE @programme";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@programme", "%" + txtSearchProgramme.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchProgramme.Text = "";
            LoadStudents();
        }

        // Edit mode
        protected void gvStudent_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            LoadStudents();
        }

        protected void gvStudent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Only run for rows in edit mode
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                // Find the dropdown inside the edit template
                DropDownList ddlProgramme = (DropDownList)e.Row.FindControl("ddlEditProgramme");

                if (ddlProgramme != null)
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        // Load all programmes into dropdown
                        string query = @"
                            SELECT ProgrammeID, ProgrammeName
                            FROM Programmes";

                        SqlDataAdapter da = new SqlDataAdapter(query, conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddlProgramme.DataSource = dt;
                        ddlProgramme.DataTextField = "ProgrammeName";
                        ddlProgramme.DataValueField = "ProgrammeID";
                        ddlProgramme.DataBind();
                    }

                    // Get current programme of this student row
                    string currentProgrammeId =
                        System.Web.UI.DataBinder.Eval(e.Row.DataItem, "ProgrammeID").ToString();

                    // Set selected value in dropdown
                    ddlProgramme.SelectedValue = currentProgrammeId;
                }
            }
        }

        // Update student information
        protected void gvStudent_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string studentId = gvStudents.DataKeys[e.RowIndex].Value.ToString();

            string fullName = ((TextBox)gvStudents.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            string email = ((TextBox)gvStudents.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            
            DropDownList ddlProgramme = (DropDownList)gvStudents.Rows[e.RowIndex].FindControl("ddlEditProgramme");
            int programmeId = Convert.ToInt32(ddlProgramme.SelectedValue);

            // Validate that all fields have been filled in
            if (fullName.Trim() == "" || email.Trim() == "")
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please fill in all fields.";

                gvStudents.EditIndex = -1;
                LoadStudents();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Retrieve the UserID associated with the StudentID
                string getUserQuery = @"
                    SELECT UserID
                    FROM Students 
                    WHERE StudentID = @studentId";

                SqlCommand getUserCmd = new SqlCommand(getUserQuery, conn);

                getUserCmd.Parameters.AddWithValue("@studentId", studentId);

                int userId = Convert.ToInt32(getUserCmd.ExecuteScalar());

                // Check if the new email already exists for another user
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

                // Update student programme in Students table
                string updateStudentQuery = @"
                    UPDATE Students
                    SET ProgrammeID = @programmeId
                    WHERE StudentID = @studentId";

                SqlCommand updateStudentCmd = new SqlCommand(updateStudentQuery, conn);

                updateStudentCmd.Parameters.AddWithValue("@programmeId", programmeId);
                updateStudentCmd.Parameters.AddWithValue("@studentId", studentId);

                updateStudentCmd.ExecuteNonQuery();
            }

            // Exit edit mode and refresh the student list
            gvStudents.EditIndex = -1;
            LoadStudents();

            lblMessage.CssClass = "text-success";
            lblMessage.Text = "Student updated successfully!";
        }

        // Cancel
        protected void gvStudent_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            LoadStudents();
        }

        // Delete student
        protected void gvStudent_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                GridViewRow row =
                    (GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;

                int index = row.RowIndex;

                string studentId =
                    gvStudents.DataKeys[index].Value.ToString();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Check if student has enrolments
                    string checkQuery = @"
                        SELECT COUNT(*)
                        FROM EnrollmentMaster
                        WHERE StudentID = @id";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@id", studentId);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        lblMessage.CssClass = "text-danger";
                        lblMessage.Text =
                            $"Cannot delete student: student is enrolled.";

                        return;
                    }

                    // Get UserID from Students table
                    string getUserQuery = @"
                        SELECT UserID
                        FROM Students
                        WHERE StudentID = @id";

                    SqlCommand getUserCmd = new SqlCommand(getUserQuery, conn);
                    getUserCmd.Parameters.AddWithValue("@id", studentId);

                    int userId = Convert.ToInt32(getUserCmd.ExecuteScalar());

                    // Delete Student record
                    string deleteStudent = @"
                        DELETE FROM Students
                        WHERE StudentID = @id";

                    SqlCommand delStudentCmd = new SqlCommand(deleteStudent, conn);
                    delStudentCmd.Parameters.AddWithValue("@id", studentId);
                    delStudentCmd.ExecuteNonQuery();

                    // Delete User account
                    string deleteUser = @"
                        DELETE FROM Users
                        WHERE UserID = @uid";

                    SqlCommand delUserCmd = new SqlCommand(deleteUser, conn);
                    delUserCmd.Parameters.AddWithValue("@uid", userId);
                    delUserCmd.ExecuteNonQuery();
                }

                LoadStudents();

                lblMessage.CssClass = "text-success";
                lblMessage.Text = "Student deleted successfully!";
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