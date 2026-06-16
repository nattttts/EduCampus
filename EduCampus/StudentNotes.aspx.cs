using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EduCampus
{
    public partial class StudentNotes : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadNotes();
            }
        }

        private void LoadNotes()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT
                        c.CourseCode,
                        c.CourseName,
                        n.FileName,
                        n.FilePath,
                        n.UploadDate
                    FROM Notes n
                    INNER JOIN CourseOfferings co
                        ON n.OfferingID = co.OfferingID
                    INNER JOIN Courses c
                        ON co.CourseID = c.CourseID
                    INNER JOIN EnrollmentDetails ed
                        ON co.OfferingID = ed.OfferingID
                    INNER JOIN EnrollmentMaster em
                        ON ed.EnrolmentID = em.EnrolmentID
                    INNER JOIN Students s
                        ON em.StudentID = s.StudentID
                    INNER JOIN Users u
                        ON s.UserID = u.UserID
                    WHERE u.Email = @Email
                    ORDER BY c.CourseCode";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                gvNotes.DataSource = dt;
                gvNotes.DataBind();

                if (dt.Rows.Count == 0)
                {
                    lblMessage.Text = "No notes available for your enrolled courses.";
                }
                else
                {
                    lblMessage.Text = "";
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}