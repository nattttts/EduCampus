using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Xml.Linq;

namespace EduCampus
{
    public partial class lecturerprofile : System.Web.UI.Page
    {
        string cs = ConfigurationManager
            .ConnectionStrings["EduCampusDB"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email"] == null || Session["Role"] == null)
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
                LoadLecturerProfile();
            }
        }

        private void LoadLecturerProfile()
        {
            string email = Session["Email"].ToString();

            string query = @"
                SELECT
                    L.LecturerID,
                    U.FullName,
                    U.Email,
                    U.Role
                FROM Lecturers L
                INNER JOIN Users U
                    ON L.UserID = U.UserId
                WHERE U.Email = @Email";

            using (SqlConnection conn = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtLecturerID.Text = reader["LecturerID"].ToString();
                    txtName.Text = reader["FullName"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtRole.Text = reader["Role"].ToString();
                }
            }
        }

        private string HashPassword(string password)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] bytes =
                Encoding.UTF8.GetBytes(password);

            byte[] hash =
                sha256.ComputeHash(bytes);

            StringBuilder result =
                new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("x2"));
            }

            return result.ToString();
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string email = Session["Email"].ToString();

            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (oldPassword == "" || newPassword == "" || confirmPassword == "")
            {
                lblMessage.Text = "Please fill in all fields.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                KeepPasswordModalOpen();
                return;
            }

            if (newPassword != confirmPassword)
            {
                lblMessage.Text = "New password and confirm password do not match.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                KeepPasswordModalOpen();
                return;
            }

            if (newPassword.Length < 8)
            {
                lblMessage.Text = "Password must be at least 8 characters.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                KeepPasswordModalOpen();
                return;
            }

            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();

                string selectQuery = @"
                    SELECT PasswordHash
                    FROM Users
                    WHERE Email = @Email";

                SqlCommand selectCmd =
                    new SqlCommand(selectQuery, conn);

                selectCmd.Parameters.AddWithValue("@Email", email);

                object result = selectCmd.ExecuteScalar();

                if (result == null)
                {
                    lblMessage.Text = "User account not found.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    KeepPasswordModalOpen();
                    return;
                }

                string oldHashFromDB = result.ToString();

                string oldHash =
                    HashPassword(oldPassword);

                if (oldHash != oldHashFromDB)
                {
                    lblMessage.Text = "Current password is incorrect.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    KeepPasswordModalOpen();
                    return;
                }

                string newHash =
                    HashPassword(newPassword);

                if (newHash == oldHashFromDB)
                {
                    lblMessage.Text = "New password cannot be the same as old password.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    KeepPasswordModalOpen();
                    return;
                }

                string updateQuery = @"
                    UPDATE Users
                    SET PasswordHash = @PasswordHash
                    WHERE Email = @Email";

                SqlCommand updateCmd =
                    new SqlCommand(updateQuery, conn);

                updateCmd.Parameters.AddWithValue("@PasswordHash", newHash);
                updateCmd.Parameters.AddWithValue("@Email", email);

                updateCmd.ExecuteNonQuery();

                lblMessage.Text = "Password changed successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Green;

                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "CloseModal",
                    "closeChangePasswordModal();",
                    true);

                txtOldPassword.Text = "";
                txtNewPassword.Text = "";
                txtConfirmPassword.Text = "";
            }
        }

        private void KeepPasswordModalOpen()
        {
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "openModal",
                "openChangePasswordModal();",
                true);
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx");
        }
    }
}