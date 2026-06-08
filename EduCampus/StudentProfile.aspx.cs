using System;
using System.Configuration;
using System.Data.SqlClient;

namespace EduCampus
{
    public partial class StudentProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Email"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

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

        private void LoadStudentProfile()
        {
            string email = Session["Email"].ToString();

            string connectionString =
                ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtStudentID.Text = reader["StudentID"].ToString();
                    txtName.Text = reader["FullName"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtProgramme.Text = reader["ProgrammeName"].ToString();
                }
            }
        }

    }
}