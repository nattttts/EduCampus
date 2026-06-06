using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class ManageProgramme : System.Web.UI.Page
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
            }
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("Login.aspx"); // go back to login
        }

        // Load programme list
        void LoadProgramme()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Programmes", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProgramme.DataSource = dt;
                gvProgramme.DataBind();
            }
        }

        // Add programme
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCode.Text.Trim() == "" || txtName.Text.Trim() == "")
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Please fill in all fields.";
                    return;
                }

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // Check duplicate programme code
                    string checkQuery = "SELECT COUNT(*) FROM Programmes WHERE ProgrammeCode = @code";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@code", txtCode.Text.Trim());

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "Programme code already exists.";
                        return;
                    }

                    // Insert programme
                    string query = "INSERT INTO Programmes (ProgrammeCode, ProgrammeName) VALUES (@code, @name)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@code", txtCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Programme added successfully!";

                txtCode.Text = "";
                txtName.Text = "";

                LoadProgramme();
            }
            catch (Exception ex)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Error: " + ex.Message;
            }
        }

        // Clear button
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtName.Text = "";
            lblMsg.Text = "";
        }

        // Edit mode
        protected void gvProgramme_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvProgramme.EditIndex = e.NewEditIndex;
            LoadProgramme();
        }

        // Update
        protected void gvProgramme_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvProgramme.DataKeys[e.RowIndex].Value);

            string code = ((System.Web.UI.WebControls.TextBox)gvProgramme.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            string name = ((System.Web.UI.WebControls.TextBox)gvProgramme.Rows[e.RowIndex].Cells[2].Controls[0]).Text;

            if (code.Trim() == "" || name.Trim() == "")
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please fill in all fields.";

                gvProgramme.EditIndex = -1;
                LoadProgramme();
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Check duplicate programme code
                string checkQuery = "SELECT COUNT(*) FROM Programmes WHERE ProgrammeCode = @code AND ProgrammeID != @id";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con);

                checkCmd.Parameters.AddWithValue("@code", code);
                checkCmd.Parameters.AddWithValue("@id", id);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Programme code already exists.";

                    gvProgramme.EditIndex = -1;
                    LoadProgramme();
                    return;
                }

                // Update programme
                string query = "UPDATE Programmes SET ProgrammeCode=@code, ProgrammeName=@name WHERE ProgrammeID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@code", code);
                cmd.Parameters.AddWithValue("@name", name);

                cmd.ExecuteNonQuery();
            }

            gvProgramme.EditIndex = -1;
            LoadProgramme();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Programme updated successfully!";
        }

        // Cancel
        protected void gvProgramme_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvProgramme.EditIndex = -1;
            LoadProgramme();
        }

        // Delete
        protected void gvProgramme_RowDeleting(object sender, GridViewDeleteEventArgs e)
        { 
            int id = Convert.ToInt32(gvProgramme.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check if programme is assigned to any course
                string checkCourseQuery = "SELECT COUNT(*) FROM Courses WHERE ProgrammeID=@id";

                SqlCommand checkCourseCmd = new SqlCommand(checkCourseQuery, conn);
                checkCourseCmd.Parameters.AddWithValue("@id", id);

                int courseCount = Convert.ToInt32(checkCourseCmd.ExecuteScalar());

                // Check if programme is assigned to any student
                string checkStudentQuery = "SELECT COUNT(*) FROM Students WHERE ProgrammeID=@id";

                SqlCommand checkStudentCmd = new SqlCommand(checkStudentQuery, conn);
                checkStudentCmd.Parameters.AddWithValue("@id", id);

                int studentCount = Convert.ToInt32(checkStudentCmd.ExecuteScalar());

                // If programme is assigned to courses or students, prevent deletion and show error message
                if (courseCount > 0 && studentCount > 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = $"Cannot delete programme: programme is assigned to {courseCount} course(s) and {studentCount} student(s).";
                    return;
                }
                else if (courseCount > 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = $"Cannot delete programme: programme is assigned to {courseCount} course(s).";
                    return;
                }
                else if (studentCount > 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = $"Cannot delete programme: programme is assigned to {studentCount} student(s).";
                    return;
                }

                // Delete only when programme no assigned to any course and student
                string query = "DELETE FROM Programmes WHERE ProgrammeID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            LoadProgramme();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Programme deleted successfully!";
        }
    }
}