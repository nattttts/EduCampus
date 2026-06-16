using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class Results : System.Web.UI.Page
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
                ViewState["Semester"] = "Semester 1";

                SetActiveSemesterButton("Semester 1");

                LoadResults("Semester 1");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentDashboard.aspx");
        }

        protected void btnSem_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            string semester = btn.CommandArgument;

            ViewState["Semester"] = semester;

            SetActiveSemesterButton(semester);

            LoadResults(semester);
        }

        void LoadResults(string semester)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT 
                    c.CourseCode,
                    c.CourseName,
                    cm.AssignmentMark,
                    cm.MidTestMark,
                    cm.FinalMark,
                    cm.FinalGrade
                FROM CourseMarks cm
                INNER JOIN EnrollmentDetails ed ON cm.DetailID = ed.DetailID
                INNER JOIN EnrollmentMaster em ON ed.EnrolmentID = em.EnrolmentID
                INNER JOIN CourseOfferings co ON ed.OfferingID = co.OfferingID
                INNER JOIN Courses c ON co.CourseID = c.CourseID
                WHERE em.StudentID = (
                    SELECT StudentID FROM Students
                    WHERE UserID = (
                        SELECT UserID FROM Users WHERE Email = @Email
                    )
                )
                AND em.Semester = @Semester";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"]);
                cmd.Parameters.AddWithValue("@Semester", semester);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvResults.DataSource = dt;
                gvResults.DataBind();
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        private void SetActiveSemesterButton(string semester)
        {
            btnSem1.CssClass = "btn btn-secondary mx-2";
            btnSem2.CssClass = "btn btn-secondary mx-2";

            if (semester == "Semester 1")
            {
                btnSem1.CssClass = "btn btn-primary mx-2";
            }
            else if (semester == "Semester 2")
            {
                btnSem2.CssClass = "btn btn-primary mx-2";
            }
        }
    }
}