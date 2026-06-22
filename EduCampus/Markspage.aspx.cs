using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace lecturer
{
    public partial class Markspage : System.Web.UI.Page
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSession();
                btnEdit.Enabled = false;
                btnSave.Enabled = false;
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
                    INNER JOIN Users u ON l.UserID = u.UserId
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
            gvMarks.DataSource = null;
            gvMarks.DataBind();
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

            return true;
        }

        private void LoadStudents()
        {
            if (!IsFilterValid())
                return;

            int offeringID = Convert.ToInt32(ddlCourse.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT
                          ISNULL(CM.MarkID, 0) AS MarkID,
                          ED.DetailID,
                          S.StudentID,
                          U.FullName AS StudentName,
                          ISNULL(CM.AssignmentMark, 0) AS AssignmentMark,
                          ISNULL(CM.QuizMark, 0) AS QuizMark,
                          ISNULL(CM.MidTestMark, 0) AS MidTestMark,
                          ISNULL(CM.FinalExamMark, 0) AS FinalExamMark,
                          ISNULL(CM.FinalMark, 0) AS FinalMark,
                          ISNULL(CM.FinalGrade, '') AS FinalGrade,
                          ISNULL(CM.GradePoint, 0) AS GradePoint
                      FROM EnrollmentDetails ED
                      INNER JOIN EnrollmentMaster EM ON ED.EnrolmentID = EM.EnrolmentID
                      INNER JOIN Students S ON EM.StudentID = S.StudentID
                      INNER JOIN Users U ON S.UserID = U.UserId
                      LEFT JOIN CourseMarks CM ON CM.DetailID = ED.DetailID
                      WHERE ED.OfferingID = @OfferingID
                      ORDER BY S.StudentID", conn);

                cmd.Parameters.AddWithValue("@OfferingID", offeringID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMarks.DataSource = dt;
                gvMarks.DataBind();

                btnEdit.Enabled = dt.Rows.Count > 0;
                btnSave.Enabled = false;

                lblMessage.Text = dt.Rows.Count > 0
                    ? "Students loaded. Click Give Marks to enter marks."
                    : "No students found for this course.";
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (gvMarks.Rows.Count == 0)
            {
                lblMessage.Text = "Please load students first.";
                return;
            }

            foreach (GridViewRow row in gvMarks.Rows)
            {
                TextBox txtAssignment = (TextBox)row.FindControl("txtAssignment");
                TextBox txtQuiz = (TextBox)row.FindControl("txtQuiz");
                TextBox txtMidTest = (TextBox)row.FindControl("txtMidTest");
                TextBox txtFinalExam = (TextBox)row.FindControl("txtFinalExam");

                if (txtAssignment != null) txtAssignment.Enabled = true;
                if (txtQuiz != null) txtQuiz.Enabled = true;
                if (txtMidTest != null) txtMidTest.Enabled = true;
                if (txtFinalExam != null) txtFinalExam.Enabled = true;
            }

            btnSave.Enabled = true;
            lblMessage.Text = "You can now give marks.";
        }

        private decimal GetDecimalValue(TextBox textBox)
        {
            decimal value;

            if (textBox == null || string.IsNullOrWhiteSpace(textBox.Text))
                return 0;

            if (!decimal.TryParse(textBox.Text.Trim(), out value))
                return 0;

            return value;
        }

        private void CalculateGrade(decimal mark, out string grade, out decimal gradePoint)
        {
            if (mark >= 90) { grade = "A+"; gradePoint = 4.00m; return; }
            if (mark >= 80) { grade = "A"; gradePoint = 4.00m; return; }
            if (mark >= 75) { grade = "A-"; gradePoint = 3.67m; return; }
            if (mark >= 70) { grade = "B+"; gradePoint = 3.33m; return; }
            if (mark >= 65) { grade = "B"; gradePoint = 3.00m; return; }
            if (mark >= 60) { grade = "B-"; gradePoint = 2.67m; return; }
            if (mark >= 55) { grade = "C+"; gradePoint = 2.33m; return; }
            if (mark >= 50) { grade = "C"; gradePoint = 2.00m; return; }
            if (mark >= 45) { grade = "C-"; gradePoint = 1.50m; return; }
            if (mark >= 40) { grade = "D"; gradePoint = 1.00m; return; }

            grade = "F";
            gradePoint = 0.00m;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (gvMarks.Rows.Count == 0)
            {
                lblMessage.Text = "Please load students first.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                foreach (GridViewRow row in gvMarks.Rows)
                {
                    HiddenField hfDetailID = (HiddenField)row.FindControl("hfDetailID");

                    if (hfDetailID == null || string.IsNullOrEmpty(hfDetailID.Value))
                        continue;

                    int detailID = Convert.ToInt32(hfDetailID.Value);

                    decimal assignment = GetDecimalValue((TextBox)row.FindControl("txtAssignment"));
                    decimal quiz = GetDecimalValue((TextBox)row.FindControl("txtQuiz"));
                    decimal midTest = GetDecimalValue((TextBox)row.FindControl("txtMidTest"));
                    decimal finalExam = GetDecimalValue((TextBox)row.FindControl("txtFinalExam"));

                    decimal finalMark = assignment + quiz + midTest + finalExam;
                    string grade;
                    decimal gradePoint;
                    CalculateGrade(finalMark, out grade, out gradePoint);

                    SqlCommand cmd = new SqlCommand(
                        @"IF EXISTS (SELECT 1 FROM CourseMarks WHERE DetailID = @DetailID)
                          BEGIN
                              UPDATE CourseMarks
                              SET AssignmentMark = @Assignment,
                                  QuizMark = @Quiz,
                                  MidTestMark = @MidTest,
                                  FinalExamMark = @FinalExam,
                                  FinalMark = @FinalMark,
                                  FinalGrade = @Grade,
                                  GradePoint = @GradePoint
                              WHERE DetailID = @DetailID
                          END
                          ELSE
                          BEGIN
                              INSERT INTO CourseMarks
                                  (DetailID, AssignmentMark, QuizMark, MidTestMark, FinalExamMark, FinalMark, FinalGrade, GradePoint)
                              VALUES
                                  (@DetailID, @Assignment, @Quiz, @MidTest, @FinalExam, @FinalMark, @Grade, @GradePoint)
                          END", conn);

                    cmd.Parameters.AddWithValue("@DetailID", detailID);
                    cmd.Parameters.AddWithValue("@Assignment", assignment);
                    cmd.Parameters.AddWithValue("@Quiz", quiz);
                    cmd.Parameters.AddWithValue("@MidTest", midTest);
                    cmd.Parameters.AddWithValue("@FinalExam", finalExam);
                    cmd.Parameters.AddWithValue("@FinalMark", finalMark);
                    cmd.Parameters.AddWithValue("@Grade", grade);
                    cmd.Parameters.AddWithValue("@GradePoint", gradePoint);

                    cmd.ExecuteNonQuery();
                }
            }

            LoadStudents();
            lblMessage.Text = "Marks saved successfully.";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}
