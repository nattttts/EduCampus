using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        protected string GradeLabelsJson = "[]";
        protected string GradeDataJson = "[]";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadAssignedCourses();
                LoadCourseDropdown();
                ClearGradeChart();
            }
        }

        private int GetLecturerId()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT L.LecturerID
                    FROM Lecturers L
                    INNER JOIN Users U ON L.UserID = U.UserId
                    WHERE U.Email = @Email";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"].ToString());

                con.Open();
                object result = cmd.ExecuteScalar();

                if (result == null)
                {
                    Response.Redirect("Login.aspx");
                    return 0;
                }

                return Convert.ToInt32(result);
            }
        }

        private void LoadAssignedCourses()
        {
            int lecturerId = GetLecturerId();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT 
                        CO.OfferingID,
                        C.CourseCode,
                        C.CourseName,
                        CO.Session
                    FROM CourseOfferings CO
                    INNER JOIN Courses C ON CO.CourseID = C.CourseID
                    WHERE CO.LecturerID = @LecturerID
                    ORDER BY CO.Session, C.CourseCode";

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
            int lecturerId = GetLecturerId();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT 
                        CO.OfferingID,
                        C.CourseCode + ' - ' + C.CourseName + ' (' + CO.Session + ')' AS CourseDisplay
                    FROM CourseOfferings CO
                    INNER JOIN Courses C ON CO.CourseID = C.CourseID
                    WHERE CO.LecturerID = @LecturerID
                    ORDER BY CO.Session, C.CourseCode";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@LecturerID", lecturerId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCourse.DataSource = dt;
                ddlCourse.DataTextField = "CourseDisplay";
                ddlCourse.DataValueField = "OfferingID";
                ddlCourse.DataBind();

                ddlCourse.Items.Insert(0, new ListItem("-- Select Course --", ""));
            }
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            if (string.IsNullOrEmpty(ddlCourse.SelectedValue))
            {
                gvPoorAttendance.DataSource = null;
                gvPoorAttendance.DataBind();
                ClearGradeChart();
                return;
            }

            LoadPoorAttendance();
            LoadGradeChart();
        }

        private void LoadPoorAttendance()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT
                        S.StudentID,
                        U.FullName,
                        COUNT(A.AttendanceID) AS TotalClass,
                        SUM(CASE WHEN A.Status = 'Absent' THEN 1 ELSE 0 END) AS AbsentCount,
                        CAST(
                            (
                                SUM(CASE WHEN A.Status = 'Present' THEN 1 ELSE 0 END) * 100.0
                            ) / NULLIF(COUNT(A.AttendanceID), 0)
                            AS DECIMAL(5,2)
                        ) AS AttendancePercent
                    FROM Attendance A
                    INNER JOIN EnrollmentDetails ED ON A.DetailID = ED.DetailID
                    INNER JOIN EnrollmentMaster EM ON ED.EnrolmentID = EM.EnrolmentID
                    INNER JOIN Students S ON EM.StudentID = S.StudentID
                    INNER JOIN Users U ON S.UserID = U.UserId
                    WHERE ED.OfferingID = @OfferingID
                    GROUP BY S.StudentID, U.FullName
                    HAVING 
                        CAST(
                            (
                                SUM(CASE WHEN A.Status = 'Present' THEN 1 ELSE 0 END) * 100.0
                            ) / NULLIF(COUNT(A.AttendanceID), 0)
                            AS DECIMAL(5,2)
                        ) < 80
                    ORDER BY AttendancePercent ASC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@OfferingID", ddlCourse.SelectedValue);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvPoorAttendance.DataSource = dt;
                gvPoorAttendance.DataBind();

                lblMessage.Text = dt.Rows.Count > 0
                    ? "Poor attendance students loaded."
                    : "No poor attendance students found.";
            }
        }

        private void LoadGradeChart()
        {
            List<string> gradeLabels = new List<string>();
            List<int> gradeData = new List<int>();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT 
                        CM.FinalGrade,
                        COUNT(*) AS TotalStudents
                    FROM CourseMarks CM
                    INNER JOIN EnrollmentDetails ED 
                        ON CM.DetailID = ED.DetailID
                    WHERE ED.OfferingID = @OfferingID
                    GROUP BY CM.FinalGrade
                    ORDER BY CM.FinalGrade";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@OfferingID", ddlCourse.SelectedValue);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    gradeLabels.Add(dr["FinalGrade"].ToString());
                    gradeData.Add(Convert.ToInt32(dr["TotalStudents"]));
                }
            }

            GradeLabelsJson = ToJsonStringArray(gradeLabels);
            GradeDataJson = ToJsonNumberArray(gradeData);
        }

        private void ClearGradeChart()
        {
            GradeLabelsJson = "[]";
            GradeDataJson = "[]";
        }

        private string ToJsonStringArray(List<string> values)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            for (int i = 0; i < values.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }

                sb.Append(""");
                sb.Append(HttpUtility.JavaScriptStringEncode(values[i]));
                sb.Append(""");
            }

            sb.Append("]");
            return sb.ToString();
        }

        private string ToJsonNumberArray(List<int> values)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            for (int i = 0; i < values.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }

                sb.Append(values[i]);
            }

            sb.Append("]");
            return sb.ToString();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}
