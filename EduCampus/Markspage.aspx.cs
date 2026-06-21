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

        private int GetLecturerID()
        {
            if (Session["LecturerID"] != null)
                return Convert.ToInt32(Session["LecturerID"]);

            if (Session["UserId"] == null)
                Response.Redirect("Login.aspx");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT LecturerID
                      FROM Lecturers
                      WHERE UserID = @UserID", conn);

                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserId"]));

                object result = cmd.ExecuteScalar();

                if (result == null)
                    Response.Redirect("Login.aspx");

                Session["LecturerID"] = Convert.ToInt32(result);
                return Convert.ToInt32(result);
            }
        }

        private void LoadSession()
        {
            int lecturerID = GetLecturerID();

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

            int lecturerID = GetLecturerID();

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

        private (string grade, decimal gradePoint) CalculateGrade(decimal mark)
        {
            if (mark >= 90) return ("A+", 4.00m);
            if (mark >= 80) return ("A", 4.00m);
            if (mark >= 75) return ("A-", 3.67m);
            if (mark >= 70) return ("B+", 3.33m);
            if (mark >= 65) return ("B", 3.00m);
            if (mark >= 60) return ("B-", 2.67m);
            if (mark >= 55) return ("C+", 2.33m);
            if (mark >= 50) return ("C", 2.00m);
            if (mark >= 45) return ("C-", 1.50m);
            if (mark >= 40) return ("D", 1.00m);

            return ("F", 0.00m);
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
                    var result = CalculateGrade(finalMark);

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
                    cmd.Parameters.AddWithValue("@Grade", result.grade);
                    cmd.Parameters.AddWithValue("@GradePoint", result.gradePoint);

                    cmd.ExecuteNonQuery();
                }
            }

            LoadStudents();
            lblMessage.Text = "Marks saved successfully.";
        }
    }
}
