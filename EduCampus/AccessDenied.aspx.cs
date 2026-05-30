using System;

namespace EduCampus
{
    public partial class AccessDenied : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string role = Session["Role"].ToString();

            if (role == "Admin")
            {
                Response.Redirect("AdminDashboard.aspx");
            }
            else if (role == "Lecturer")
            {
                Response.Redirect("LecturerDashboard.aspx");
            }
            else if (role == "Student")
            {
                Response.Redirect("StudentDashboard.aspx");
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}