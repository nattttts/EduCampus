using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

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
                ddlProgramme.Items.Insert(0, new ListItem("-- Select Programme --", ""));
            }
        }

        // Load course list
        void LoadCourse()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT c.CourseID, c.CourseCode, c.CourseName, c.CreditHours, c.ProgrammeID, p.ProgrammeName
                                 FROM Courses c
                                 INNER JOIN Programmes p 
                                 ON c.ProgrammeID = p.ProgrammeID";

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
                // Validate that all fields have been filled in
                if (txtCode.Text.Trim() == "" || txtName.Text.Trim() == "" || txtCredit.Text.Trim() == "" || ddlProgramme.SelectedValue == "")
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Please fill in all fields.";
                    return;
                }

                // Validate that credit hours is a positive number
                int creditHours;

                if (!int.TryParse(txtCredit.Text.Trim(), out creditHours) || creditHours <= 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Credit hours must be a positive number.";
                    return;
                }

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // Check if any duplicate course code
                    string checkQuery = "SELECT COUNT(*) FROM Courses WHERE CourseCode = @code";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@code", txtCode.Text.Trim().ToUpper());

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "Course code already exists.";
                        return;
                    }

                    // Insert course
                    string query = "INSERT INTO Courses (CourseCode, CourseName, CreditHours, ProgrammeID) " +
                                   "VALUES (@code, @name, @credit, @programmeId)";
                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@code", txtCode.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@credit", creditHours);
                    cmd.Parameters.AddWithValue("@programmeId", ddlProgramme.SelectedValue);

                    cmd.ExecuteNonQuery();
                }

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Course added successfully!";

                // Clear form fields after successful add course
                txtCode.Text = "";
                txtName.Text = "";
                txtCredit.Text = "";
                ddlProgramme.SelectedIndex = 0;

                LoadCourse();
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
            txtCredit.Text = "";
            ddlProgramme.SelectedIndex = 0;
            lblMsg.Text = "";
        }

        // Search
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Search courses by programme
                string query = @"
                    SELECT c.CourseID, c.CourseCode, c.CourseName, c.CreditHours, c.ProgrammeID, p.ProgrammeName
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

        // Reset
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

        protected void gvCourse_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Only run for rows in edit mode
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                // Find the dropdown inside the edit template
                DropDownList ddlProgramme = (DropDownList)e.Row.FindControl("ddlEditProgramme");

                if (ddlProgramme != null)
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        // Load all programmes into dropdown
                        string query = @"SELECT ProgrammeID, ProgrammeName
                                         FROM Programmes";

                        SqlDataAdapter da = new SqlDataAdapter(query, conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddlProgramme.DataSource = dt;
                        ddlProgramme.DataTextField = "ProgrammeName";
                        ddlProgramme.DataValueField = "ProgrammeID";
                        ddlProgramme.DataBind();
                    }

                    // Get current programme of this course row
                    string currentProgrammeId = System.Web.UI.DataBinder.Eval(e.Row.DataItem, "ProgrammeID").ToString();

                    // Set selected value in dropdown
                    ddlProgramme.SelectedValue = currentProgrammeId;
                }
            }
        }

        // Update
        protected void gvCourse_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvCourse.DataKeys[e.RowIndex].Value);

            string code = ((System.Web.UI.WebControls.TextBox)gvCourse.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            string name = ((System.Web.UI.WebControls.TextBox)gvCourse.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            string credit = ((System.Web.UI.WebControls.TextBox)gvCourse.Rows[e.RowIndex].Cells[3].Controls[0]).Text;

            // Get selected programme from edit dropdown
            DropDownList ddlProgramme = (DropDownList)gvCourse.Rows[e.RowIndex].FindControl("ddlEditProgramme");
            
            int programmeId = Convert.ToInt32(ddlProgramme.SelectedValue);

            // Validate that all fields have been filled in
            if (code.Trim() == "" || name.Trim() == "" || credit.Trim() == "")
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please fill in all fields.";

                gvCourse.EditIndex = -1;
                LoadCourse();
                return;
            }

            // Validate that credit hours is a positive number
            int creditHours;

            if (!int.TryParse(credit.Trim(), out creditHours) || creditHours <= 0)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Credit hours must be a positive number.";

                gvCourse.EditIndex = -1;
                LoadCourse();
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Check if any duplicate course code
                string checkQuery = "SELECT COUNT(*) FROM Courses WHERE CourseCode = @code AND CourseID != @id";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con);

                checkCmd.Parameters.AddWithValue("@code", code.Trim().ToUpper());
                checkCmd.Parameters.AddWithValue("@id", id);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Course code already exists.";
 
                    gvCourse.EditIndex = -1;
                    LoadCourse();
                    return;
                }

                // Update course information
                string query = @"UPDATE Courses 
                                 SET CourseCode=@code, CourseName=@name, CreditHours=@credit, ProgrammeID=@programmeId
                                 WHERE CourseID=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@code", code.Trim().ToUpper());
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@credit", creditHours);
                cmd.Parameters.AddWithValue("@programmeId", programmeId);

                cmd.ExecuteNonQuery();
            }

            // Exit edit mode and refresh the course list 
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

        // Delete
        protected void gvCourse_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvCourse.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check if course is assigned to any course offerings
                string checkQuery = "SELECT COUNT(*) FROM CourseOfferings WHERE CourseID=@id";

                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@id", id);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                // If course is assigned to course offerings, prevent deletion and show error message
                if (count > 0)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = $"Cannot delete course: course is assigned to {count} course offerings.";
                    return;
                }

                // Delete only when course no assigned to any course offerings
                string query = "DELETE FROM Courses WHERE CourseID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            // Refresh the course list after successful deletion
            LoadCourse();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Course deleted successfully!";
        }
    }
}