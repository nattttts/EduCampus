using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

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

        private void LoadLecturers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
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

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtLecturerEmail.Text == "" || txtLecturerPass.Text == "" || txtDept.Text == "")
            {
                lblMessage.CssClass = "text-danger";
                lblMessage.Text = "Please fill in all fields!";
                return;
            }

            string hashedPassword = HashPassword(txtLecturerPass.Text);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    string userQuery = @"
                        INSERT INTO Users (FullName, Email, PasswordHash, Role) 
                        VALUES (@name, @email, @password, 'Lecturer');

                        SELECT SCOPE_IDENTITY();";


                    conn.Open();
                    SqlCommand userCmd = new SqlCommand(userQuery, conn);

                    userCmd.Parameters.AddWithValue("@name", txtName.Text);
                    userCmd.Parameters.AddWithValue("@email", txtLecturerEmail.Text);
                    userCmd.Parameters.AddWithValue("@password", hashedPassword);

                    int userID = Convert.ToInt32(userCmd.ExecuteScalar());

                    string lecturerQuery = @"
                        INSERT INTO Lecturers (Department, UserID)
                        VALUES (@dept, @userID)";

                    SqlCommand lecturerCmd = new SqlCommand(lecturerQuery, conn);
                    
                    lecturerCmd.Parameters.AddWithValue("@dept", txtDept.Text);
                    lecturerCmd.Parameters.AddWithValue("@userID", userID);

                    lecturerCmd.ExecuteNonQuery();

                    
                    //userCmd.ExecuteNonQuery();

                    lblMessage.CssClass = "text-success";
                    lblMessage.Text = "Lecturer registered successfully!";

                    txtName.Text = "";
                    txtLecturerEmail.Text = "";
                    txtLecturerPass.Text = "";
                    txtDept.Text = "";

                    LoadLecturers(); // refresh table
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchDept.Text = "";
            LoadLecturers();
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("Login.aspx"); // go back to login
        }
    }
}