using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EduCampus

{
    public partial class Attendance : System.Web.UI.Page
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtAttendanceDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                LoadSession();
            }
        }

        private int GetLecturerId()
        {
            if (Session["Email"] == null)
            {
                Response.Redirect("Login.aspx");
                return 0;
            }

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
                object result = cmd.ExecuteScalar();

                if (result == null)
                {
                    Response.Redirect("Login.aspx");
                    return 0;
                }

                return Convert.ToInt32(result);
            }
        }

        private void LoadSession()
        {
            int lecturerID = GetLecturerId();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT DISTINCT Session
                      FROM CourseOfferings
                      WHERE LecturerID = @LecturerID
                      ORDER BY Session", conn);

                cmd.Parameters.AddWithValue("@LecturerID", lecturerID);

                ddlSession.DataSource = cmd.ExecuteReader();
                ddlSession.DataTextField = "Session";
                ddlSession.DataValueField = "Session";
                ddlSession.DataBind();
            }

            ddlSession.Items.Insert(0, new ListItem("--Select Session--", ""));
            ddlCourse.Items.Clear();
            ddlCourse.Items.Insert(0, new ListItem("--Select Course--", ""));
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCourse.Items.Clear();
            ddlCourse.Items.Insert(0, new ListItem("--Select Course--", ""));
            gvAttendance.DataSource = null;
            gvAttendance.DataBind();
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            lblMessage.Text = "";

            if (string.IsNullOrEmpty(ddlSession.SelectedValue))
                return;

            int lecturerID = GetLecturerId();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT CO.OfferingID,
                             C.CourseCode + ' - ' + C.CourseName AS CourseDisplay
                      FROM CourseOfferings CO
                      INNER JOIN Courses C ON CO.CourseID = C.CourseID
                      WHERE CO.Session = @Session
                      AND CO.LecturerID = @LecturerID
                      ORDER BY C.CourseCode", conn);

                cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);
                cmd.Parameters.AddWithValue("@LecturerID", lecturerID);

                ddlCourse.DataSource = cmd.ExecuteReader();
                ddlCourse.DataTextField = "CourseDisplay";
                ddlCourse.DataValueField = "OfferingID";
                ddlCourse.DataBind();
            }

            ddlCourse.Items.Insert(0, new ListItem("--Select Course--", ""));
        }

        protected void btnLoadStudents_Click(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private bool IsFilterValid()
        {
            lblMessage.Text = "";

            if (string.IsNullOrEmpty(ddlSession.SelectedValue))
            {
                lblMessage.Text = "Please select a session.";
                return false;
            }

            if (string.IsNullOrEmpty(ddlCourse.SelectedValue))
            {
                lblMessage.Text = "Please select a course.";
                return false;
            }

            DateTime attendanceDate;
            if (!DateTime.TryParse(txtAttendanceDate.Text, out attendanceDate))
            {
                lblMessage.Text = "Please select an attendance date.";
                return false;
            }

            return true;
        }

        private void LoadStudents()
        {
            if (!IsFilterValid())
                return;

            int offeringID = Convert.ToInt32(ddlCourse.SelectedValue);
            DateTime attendanceDate = Convert.ToDateTime(txtAttendanceDate.Text).Date;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT
                          ISNULL(A.AttendanceID, 0) AS AttendanceID,
                          ED.DetailID,
                          S.StudentID,
                          U.FullName AS StudentName,
                          @AttendanceDate AS AttendanceDate,
                          ISNULL(A.Status, 'Present') AS Status,
                          ISNULL(A.Remarks, '') AS Remarks
                      FROM EnrollmentDetails ED
                      INNER JOIN EnrollmentMaster EM ON ED.EnrolmentID = EM.EnrolmentID
                      INNER JOIN Students S ON EM.StudentID = S.StudentID
                      INNER JOIN Users U ON S.UserID = U.UserId
                      LEFT JOIN Attendance A
                          ON A.DetailID = ED.DetailID
                          AND CONVERT(date, A.AttendanceDate) = @AttendanceDate
                      WHERE ED.OfferingID = @OfferingID
                      ORDER BY S.StudentID", conn);

                cmd.Parameters.AddWithValue("@OfferingID", offeringID);
                cmd.Parameters.AddWithValue("@AttendanceDate", attendanceDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAttendance.DataSource = dt;
                gvAttendance.DataBind();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DropDownList ddlStatus =
                        (DropDownList)gvAttendance.Rows[i].FindControl("ddlStatus");

                    if (ddlStatus != null)
                        ddlStatus.SelectedValue = dt.Rows[i]["Status"].ToString();
                }

                btnEdit.Enabled = dt.Rows.Count > 0;
                btnSave.Enabled = false;

                lblMessage.Text = dt.Rows.Count > 0
                    ? "Students loaded. Click Take Attendance to edit status."
                    : "No students found for this course.";
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (gvAttendance.Rows.Count == 0)
            {
                lblMessage.Text = "Please load students first.";
                return;
            }

            foreach (GridViewRow row in gvAttendance.Rows)
            {
                DropDownList ddlStatus = (DropDownList)row.FindControl("ddlStatus");
                TextBox txtRemarks = (TextBox)row.FindControl("txtRemarks");

                if (ddlStatus != null)
                    ddlStatus.Enabled = true;

                if (txtRemarks != null)
                    txtRemarks.Enabled = true;
            }

            btnSave.Enabled = true;
            lblMessage.Text = "You can now take attendance.";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gvAttendance.Rows.Count == 0)
            {
                lblMessage.Text = "Please load students first.";
                return;
            }

            DateTime attendanceDate;
            if (!DateTime.TryParse(txtAttendanceDate.Text, out attendanceDate))
            {
                lblMessage.Text = "Invalid attendance date.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                foreach (GridViewRow row in gvAttendance.Rows)
                {
                    int detailID = Convert.ToInt32(gvAttendance.DataKeys[row.RowIndex]["DetailID"]);

                    SqlCommand cmd = new SqlCommand(
                        @"IF EXISTS (
                      SELECT 1 FROM Attendance
                      WHERE DetailID = @DetailID
                      AND CONVERT(date, AttendanceDate) = @AttendanceDate
                  )
                  BEGIN
                      UPDATE Attendance
                      SET Status = @Status,
                          Remarks = @Remarks
                      WHERE DetailID = @DetailID
                      AND CONVERT(date, AttendanceDate) = @AttendanceDate
                  END
                  ELSE
                  BEGIN
                      INSERT INTO Attendance
                          (DetailID, AttendanceDate, Status, Remarks)
                      VALUES
                          (@DetailID, @AttendanceDate, @Status, @Remarks)
                  END", conn);

                    cmd.Parameters.AddWithValue("@DetailID", detailID);
                    cmd.Parameters.AddWithValue("@AttendanceDate", attendanceDate.Date);
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@Remarks", txtRemarks == null ? "" : txtRemarks.Text.Trim());

                    cmd.ExecuteNonQuery();
                }
            }

            LoadStudents();
            lblMessage.Text = "Attendance saved successfully.";
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

    }
}