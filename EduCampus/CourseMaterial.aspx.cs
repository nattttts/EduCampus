using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace EduCampus

{
    public partial class CourseMaterial : System.Web.UI.Page
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["EduCampusDB"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAssignedCourses();
            }
        }

        private int GetLecturerId()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT l.LecturerID
                    FROM Lecturers l
                    INNER JOIN Users u ON l.UserID = u.UserID
                    WHERE u.Email = @Email";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void LoadAssignedCourses()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"
                    SELECT
                        CO.OfferingID,
                        C.CourseCode + ' - ' + C.CourseName + ' (' + CO.Session + ')' AS CourseDisplay
                    FROM CourseOfferings CO
                    INNER JOIN Courses C
                        ON CO.CourseID = C.CourseID
                    WHERE CO.LecturerID = @LecturerID
                    ORDER BY CO.Session, C.CourseCode
                    ",
                    conn);

                cmd.Parameters.AddWithValue("@LecturerID", GetLecturerId());

                ddlCourse.DataSource = cmd.ExecuteReader();
                ddlCourse.DataTextField = "CourseDisplay";
                ddlCourse.DataValueField = "OfferingID";
                ddlCourse.DataBind();

                ddlCourse.Items.Insert(0, new ListItem("--Select Course--", ""));
            }
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadNotes();
            LoadStudents();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            if (ddlCourse.SelectedValue == "")
            {
                lblMessage.Text = "Please select a course first.";
                return;
            }

            if (!fileUploadNotes.HasFile)
            {
                lblMessage.Text = "Please choose a file to upload.";
                return;
            }

            int offeringID = Convert.ToInt32(ddlCourse.SelectedValue);

            string folderPath = Server.MapPath("~/CourseFiles/");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string originalFileName = Path.GetFileName(fileUploadNotes.FileName);
            string uniqueFileName =
                DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + originalFileName;

            string physicalPath = Path.Combine(folderPath, uniqueFileName);
            string dbFilePath = "~/CourseFiles/" + uniqueFileName;

            fileUploadNotes.SaveAs(physicalPath);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"
                    INSERT INTO Notes
                    (
                        FileName,
                        FilePath,
                        OfferingID
                    )
                    VALUES
                    (
                        @FileName,
                        @FilePath,
                        @OfferingID
                    )
                    ",
                    conn);

                cmd.Parameters.AddWithValue("@FileName", originalFileName);
                cmd.Parameters.AddWithValue("@FilePath", dbFilePath);
                cmd.Parameters.AddWithValue("@OfferingID", offeringID);

                cmd.ExecuteNonQuery();
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "File uploaded successfully.";

            LoadNotes();
        }

        private void LoadNotes()
        {
            if (ddlCourse.SelectedValue == "")
            {
                gvNotes.DataSource = null;
                gvNotes.DataBind();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"
                    SELECT
                        NoteID,
                        FileName,
                        FilePath,
                        UploadDate
                    FROM Notes
                    WHERE OfferingID = @OfferingID
                    ORDER BY UploadDate DESC
                    ",
                    conn);

                cmd.Parameters.AddWithValue(
                    "@OfferingID",
                    ddlCourse.SelectedValue);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                gvNotes.DataSource = dt;
                gvNotes.DataBind();
            }
        }

        protected void gvNotes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteNote")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                int noteID =
                    Convert.ToInt32(gvNotes.DataKeys[rowIndex]["NoteID"]);

                string filePath =
                    gvNotes.DataKeys[rowIndex]["FilePath"].ToString();

                string physicalPath =
                    Server.MapPath(filePath);

                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(
                        @"DELETE FROM Notes
                          WHERE NoteID = @NoteID",
                        conn);

                    cmd.Parameters.AddWithValue("@NoteID", noteID);
                    cmd.ExecuteNonQuery();
                }

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "File deleted successfully.";

                LoadNotes();
            }
        }

        private void LoadStudents()
        {
            if (ddlCourse.SelectedValue == "")
            {
                gvStudents.DataSource = null;
                gvStudents.DataBind();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"
                    SELECT DISTINCT
                        S.StudentID,
                        U.FullName,
                        U.Email
                    FROM EnrollmentDetails ED
                    INNER JOIN EnrollmentMaster EM
                        ON ED.EnrolmentID = EM.EnrolmentID
                    INNER JOIN Students S
                        ON EM.StudentID = S.StudentID
                    INNER JOIN Users U
                        ON S.UserID = U.UserId
                    WHERE ED.OfferingID = @OfferingID
                    ORDER BY S.StudentID
                    ",
                    conn);

                cmd.Parameters.AddWithValue(
                    "@OfferingID",
                    ddlCourse.SelectedValue);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
        }
    }
}