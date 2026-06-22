using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class AcademicResults : System.Web.UI.Page
    { 
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
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("Login.aspx"); // go back to login
        }

        protected void btnCourseResults_Click(object sender, EventArgs e)
        {
            Response.Redirect("CourseResults.aspx");
        }

        protected void btnStudentTranscript_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentTranscript.aspx");
        }
    }
}