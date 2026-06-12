using System;
using System.Data;
using System.Data.SqlClient;

namespace EduCampus
{
    public partial class StudentNotification : System.Web.UI.Page
    {
        string cs = System.Configuration.ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 🔒 CHECK LOGIN (based on Email since login stores Email)
            if (Session["Email"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadNotifications();
            }
        }

        void LoadNotifications()
        {
            string email = Session["Email"].ToString();
            int userId = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                // 🔎 STEP 1: GET USERID FROM EMAIL
                string getUserIdQuery = @"
                    SELECT UserID 
                    FROM Users 
                    WHERE Email = @Email";

                using (SqlCommand cmdUser = new SqlCommand(getUserIdQuery, con))
                {
                    cmdUser.Parameters.AddWithValue("@Email", email);

                    object result = cmdUser.ExecuteScalar();

                    if (result == null)
                    {
                        Response.Redirect("Login.aspx");
                        return;
                    }

                    userId = Convert.ToInt32(result);
                }

                // 🔔 STEP 2: GET NOTIFICATIONS
                string query = @"
                    SELECT Title, Message, CreatedDateTime
                    FROM Notifications
                    WHERE UserID = @UserID
                    ORDER BY CreatedDateTime DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    gvNotifications.DataSource = dt;
                    gvNotifications.DataBind();
                }
            }
        }

        // 🚪 LOGOUT
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}