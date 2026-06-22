using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class StudentCalendar : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            // CHECK LOGIN FIRST
            if (Session["Email"] == null || Session["Role"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // CHECK ROLE
            if (Session["Role"].ToString() != "Student")
            {
                Response.Redirect("AccessDenied.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCalendar();
            }
        }

        // LOAD ACADEMIC CALENDAR LIST
        private void LoadCalendar()
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                string query = @"
                    SELECT * 
                    FROM AcademicCalendar
                    ORDER BY StartDate, EndDate";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCalendar.DataSource = dt;
                gvCalendar.DataBind();
            }
        }

        protected string FormatDate(object start, object end)
        {
            DateTime startDate = Convert.ToDateTime(start);
            DateTime endDate = Convert.ToDateTime(end);

            // Same date
            if (startDate == endDate)
                return startDate.ToString("dd MMM yyyy");

            // Same year and month
            else if (startDate.Month == endDate.Month && startDate.Year == endDate.Year)
                return $"{startDate:dd} - {endDate:dd MMM yyyy}";

            // Same year but different month 
            else if (startDate.Month != endDate.Month && startDate.Year == endDate.Year)
                return $"{startDate:dd MMM} - {endDate:dd MMM yyyy}";

            // Different year
            else
                return $"{startDate:dd MMM yyyy} - {endDate:dd MMM yyyy}";
        }

        // SEARCH
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(cs))
            {
                // Search academic calendar
                string query = @"
                    SELECT *
                    FROM AcademicCalendar
                    WHERE Session LIKE @search
                        OR Event LIKE @search
                    ORDER BY StartDate, EndDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text.Trim() + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCalendar.DataSource = dt;
                gvCalendar.DataBind();
            }
        }

        // RESET
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadCalendar();
        }

        // LOGOUT
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}