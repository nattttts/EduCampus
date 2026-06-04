using System;
using System.Data;
using System.Data.SqlClient;

namespace EduCampus
{
    public partial class ManageCourse : System.Web.UI.Page
    {
        string connStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EduCampusDB;Integrated Security=True";
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

            if (!IsPostBack)
            {
                LoadProgramme();
                LoadCourse();
            }
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("Login.aspx"); // go back to login
        }

        // Load Programme dropdown
        void LoadProgramme()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Programmes", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlProgramme.DataSource = dt;
                ddlProgramme.DataTextField = "ProgrammeName";
                ddlProgramme.DataValueField = "ProgrammeID";
                ddlProgramme.DataBind();
            }
        }

        // Load course list
        void LoadCourse()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT c.CourseID, c.CourseCode, c.CourseName, c.CreditHours,
                                 p.ProgrammeName
                                 FROM Courses c
                                 INNER JOIN Programmes p ON c.ProgrammeID = p.ProgrammeID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCourse.DataSource = dt;
                gvCourse.DataBind();
            }
        }

        // Add course
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCode.Text == "" || txtName.Text == "" || txtCredit.Text == "")
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Please fill in all fields.";
                    return;
                }

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = "INSERT INTO Courses (CourseCode, CourseName, CreditHours, ProgrammeID) VALUES (@code, @name, @credit, @pid)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@code", txtCode.Text);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@credit", txtCredit.Text);
                    cmd.Parameters.AddWithValue("@pid", ddlProgramme.SelectedValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Course added successfully!";

                txtCode.Text = "";
                txtName.Text = "";
                txtCredit.Text = "";

                LoadCourse();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        // Clear button
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtCredit.Text = "";
            lblMsg.Text = "";
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Search course by programme
                string query = @"
                    SELECT c.CourseID, c.CourseCode, c.CourseName, c.CreditHours, p.ProgrammeName
                    FROM Courses c
                    INNER JOIN Programmes p 
                    ON c.ProgrammeID = p.ProgrammeID
                    WHERE p.ProgrammeName LIKE @dept";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dept", "%" + txtSearchDept.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCourse.DataSource = dt;
                gvCourse.DataBind();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearchDept.Text = "";
            LoadCourse();
        }

        // Edit mode
        protected void gvCourse_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvCourse.EditIndex = e.NewEditIndex;
            LoadCourse();
        }

        // Update
        protected void gvCourse_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvCourse.DataKeys[e.RowIndex].Value);

            string code = ((System.Web.UI.WebControls.TextBox)gvCourse.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            string name = ((System.Web.UI.WebControls.TextBox)gvCourse.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            string credit = ((System.Web.UI.WebControls.TextBox)gvCourse.Rows[e.RowIndex].Cells[3].Controls[0]).Text;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "UPDATE Courses SET CourseCode=@code, CourseName=@name, CreditHours=@credit WHERE CourseID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@credit", credit);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvCourse.EditIndex = -1;
            LoadCourse();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Course updated successfully!";
        }

        // Cancel
        protected void gvCourse_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvCourse.EditIndex = -1;
            LoadCourse();
        }
    }
}