using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;


namespace EduCampus
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Check empty fields
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblMessage.Text = "Please enter email and password.";
                return;
            }

            string email = txtEmail.Text.Trim();
            string hashedPassword = HashPassword(txtPassword.Text.Trim());

            // Get connection string from Web.config
            string connectionString =
                ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

            string query = @"
                SELECT Role
                FROM Users
                WHERE Email = @Email
                AND PasswordHash = @PasswordHash";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string role = result.ToString();

                    Session["Email"] = email;
                    Session["Role"] = role;

                    // Redirect based on role
                    if (role == "Admin")
                    {
                        Response.Redirect("AdminDashboard.aspx");
                    }
                    else if (role == "Lecturer")
                    {
                        Response.Redirect("LecturerAnnouncements.aspx");
                    }
                    else if (role == "Student")
                    {
                        Response.Redirect("StudentDashboard.aspx");
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid email or password.";
                }
            }
        }
    }
}