using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace lecturer
{
    public partial class Attendance : System.Web.UI.Page
    {
        string connStr =
            ConfigurationManager.ConnectionStrings["EduCampusDB"]
            .ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSession();
            }
        }

        private void LoadSession()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT DISTINCT Session
                      FROM CourseOfferings
                      ORDER BY Session",
                    conn);

                ddlSession.DataSource = cmd.ExecuteReader();
                ddlSession.DataTextField = "Session";
                ddlSession.DataValueField = "Session";
                ddlSession.DataBind();

                ddlSession.Items.Insert(0, new ListItem("--Select Session--", ""));
            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"
                    SELECT DISTINCT C.CourseCode
                    FROM Courses C
                    INNER JOIN CourseOfferings CO
                        ON C.CourseID = CO.CourseID
                    WHERE CO.Session = @Session",
                    conn);

                cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);

                ddlCourse.DataSource = cmd.ExecuteReader();
                ddlCourse.DataTextField = "CourseCode";
                ddlCourse.DataValueField = "CourseCode";
                ddlCourse.DataBind();

                ddlCourse.Items.Insert(0, new ListItem("--Select Course--", ""));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadAttendance();
        }

        private void LoadAttendance()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"
                    SELECT
                        A.AttendanceID,
                        S.StudentID,
                        U.FullName AS StudentName,
                        A.AttendanceDate,
                        A.Status,
                        A.Remarks
                    FROM Attendance A
                    INNER JOIN EnrollmentDetails ED
                        ON A.DetailID = ED.DetailID
                    INNER JOIN EnrollmentMaster EM
                        ON ED.EnrolmentID = EM.EnrolmentID
                    INNER JOIN Students S
                        ON EM.StudentID = S.StudentID
                    INNER JOIN Users U
                        ON S.UserID = U.UserId
                    INNER JOIN CourseOfferings CO
                        ON ED.OfferingID = CO.OfferingID
                    INNER JOIN Courses C
                        ON CO.CourseID = C.CourseID
                    WHERE CO.Session = @Session
                    AND C.CourseCode = @Course
                    AND A.AttendanceDate = @AttendanceDate
                    ORDER BY S.StudentID",
                    conn);

                cmd.Parameters.AddWithValue("@Session", ddlSession.SelectedValue);
                cmd.Parameters.AddWithValue("@Course", ddlCourse.SelectedValue);
                cmd.Parameters.AddWithValue("@AttendanceDate", Convert.ToDateTime(txtAttendanceDate.Text));

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
                    {
                        ddlStatus.SelectedValue = dt.Rows[i]["Status"].ToString();
                    }
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvAttendance.Rows)
            {
                DropDownList ddlStatus =
                    (DropDownList)row.FindControl("ddlStatus");

                TextBox txtRemarks =
                    (TextBox)row.FindControl("txtRemarks");

                if (ddlStatus != null)
                    ddlStatus.Enabled = true;

                if (txtRemarks != null)
                    txtRemarks.Enabled = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                foreach (GridViewRow row in gvAttendance.Rows)
                {
                    int attendanceID = Convert.ToInt32(row.Cells[0].Text);

                    DropDownList ddlStatus =
                        (DropDownList)row.FindControl("ddlStatus");

                    TextBox txtRemarks =
                        (TextBox)row.FindControl("txtRemarks");

                    SqlCommand cmd = new SqlCommand(
                        @"UPDATE Attendance
                          SET Status = @Status,
                              Remarks = @Remarks
                          WHERE AttendanceID = @AttendanceID",
                        conn);

                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
                    cmd.Parameters.AddWithValue("@AttendanceID", attendanceID);

                    cmd.ExecuteNonQuery();
                }
            }

            LoadAttendance();
        }
    }
}