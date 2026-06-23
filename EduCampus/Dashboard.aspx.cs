using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EduCampus

{
    public partial class Dashboard : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["EduCampusConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LecturerID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadAssignedCourses();
                LoadCourseDropdown();
            }
        }

        private void LoadAssignedCourses()
        {
            int lecturerId = Convert.ToInt32(Session["LecturerID"]);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT 
                        co.OfferingID,
                        c.CourseCode,
                        c.CourseName,
                        s.SessionName
                    FROM CourseOfferings co
                    INNER JOIN Courses c ON co.CourseID = c.CourseID
                    INNER JOIN Sessions s ON co.SessionID = s.SessionID
                    WHERE co.LecturerID = @LecturerID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@LecturerID", lecturerId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                gvAssignedCourses.DataSource = dt;
                gvAssignedCourses.DataBind();
            }
        }

        private void LoadCourseDropdown()
        {
            int lecturerId = Convert.ToInt32(Session["LecturerID"]);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT 
                        co.OfferingID,
                        c.CourseCode + ' - ' + c.CourseName AS CourseDisplay
                    FROM CourseOfferings co
                    INNER JOIN Courses c ON co.CourseID = c.CourseID
                    WHERE co.LecturerID = @LecturerID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@LecturerID", lecturerId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                ddlCourse.DataSource = dt;
                ddlCourse.DataTextField = "CourseDisplay";
                ddlCourse.DataValueField = "OfferingID";
                ddlCourse.DataBind();

                ddlCourse.Items.Insert(0, "-- Select Course --");
            }
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCourse.SelectedIndex > 0)
            {
                lblMessage.Text = "Selected course loaded.";
            }
            else
            {
                lblMessage.Text = "";
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