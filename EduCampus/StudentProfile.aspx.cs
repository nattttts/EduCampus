using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace EduCampus
{
    public partial class StudentProfile : System.Web.UI.Page
    {

        string cs = ConfigurationManager
            .ConnectionStrings["EduCampusDB"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            // Check login
            if (Session["Email"] == null || Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Only Student can access
            if (Session["Role"].ToString() != "Student")
            {
                Response.Redirect("AccessDenied.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStudentProfile();
            }

        }

        // LOAD STUDENT PROFILE

        private void LoadStudentProfile()
        {

            string email = Session["Email"].ToString();

            string query = @"
                SELECT 
                    s.StudentID,
                    u.FullName,
                    u.Email,
                    p.ProgrammeName

                FROM Students s

                INNER JOIN Users u
                ON s.UserID = u.UserID

                INNER JOIN Programmes p
                ON s.ProgrammeID = p.ProgrammeID

                WHERE u.Email = @Email";


            using (SqlConnection conn = new SqlConnection(cs))
            {

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue(
                    "@Email",
                    email);



                conn.Open();


                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    txtStudentID.Text =
                        reader["StudentID"].ToString();


                    txtName.Text =
                        reader["FullName"].ToString();


                    txtEmail.Text =
                        reader["Email"].ToString();


                    txtProgramme.Text =
                        reader["ProgrammeName"].ToString();

                }

            }

        }
        // HASH PASSWORD FUNCTION

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

                result.Append(
                    hash[i].ToString("x2"));

            }

            return result.ToString();

        }

        // CHANGE PASSWORD

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string email = Session["Email"].ToString();

            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Check empty fields
            if (oldPassword == "" || newPassword == "" || confirmPassword == "")
            {
                lblMessage.Text = "Please fill in all fields.";
                lblMessage.ForeColor = System.Drawing.Color.Red;

                KeepPasswordModalOpen();

                return;
            }

            // Check new password and confirm password
            if (newPassword != confirmPassword)
            {
                lblMessage.Text =
                "New password and confirm password do not match.";

                lblMessage.ForeColor =
                System.Drawing.Color.Red;

                KeepPasswordModalOpen();

                return;
            }

            // Password length limit
            if (newPassword.Length < 8)
            {
                lblMessage.Text =
                "Password must be at least 8 characters.";

                lblMessage.ForeColor =
                System.Drawing.Color.Red;

                KeepPasswordModalOpen();

                return;
            }

            using (SqlConnection conn = new SqlConnection(cs))
            {

                conn.Open();

                // Get current password
                string selectQuery = @"
                SELECT PasswordHash
                FROM Users
                WHERE Email=@Email";


                SqlCommand selectCmd =
                    new SqlCommand(selectQuery, conn);

                selectCmd.Parameters.AddWithValue("@Email", email);

                string oldHashFromDB =
                    selectCmd.ExecuteScalar().ToString();

                // Check old password correct

                string oldHash =
                    HashPassword(oldPassword);

                if (oldHash != oldHashFromDB)
                {
                    lblMessage.Text =
                    "Current password is incorrect.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;

                    KeepPasswordModalOpen();

                    return;
                }

                // Prevent same password

                string newHash =
                    HashPassword(newPassword);

                if (newHash == oldHashFromDB)
                {
                    lblMessage.Text =
                        "New password cannot be the same as old password.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;

                    KeepPasswordModalOpen();

                    return;
                }

                // Update password

                string updateQuery = @"
                UPDATE Users
                SET PasswordHash=@PasswordHash
                WHERE Email=@Email";

                SqlCommand updateCmd =
                    new SqlCommand(updateQuery, conn);

                updateCmd.Parameters.AddWithValue(
                    "@PasswordHash",
                    newHash);

                updateCmd.Parameters.AddWithValue(
                    "@Email",
                    email);

                updateCmd.ExecuteNonQuery();

                lblMessage.Text =
                "Password changed successfully!";


                lblMessage.ForeColor =
                    System.Drawing.Color.Green;

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
                true
            );
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {

            Session.Clear();

            Session.Abandon();

            Response.Redirect("Login.aspx");

        }


    }
}