using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace lecturer
{
    public partial class Marks : System.Web.UI.Page
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
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd =
                    new SqlCommand(
                    @"SELECT DISTINCT Session
              FROM EnrollmentMaster
              ORDER BY Session",
                    conn);

                ddlSession.DataSource =
                    cmd.ExecuteReader();

                ddlSession.DataTextField = "Session";
                ddlSession.DataValueField = "Session";
                ddlSession.DataBind();

                ddlSession.Items.Insert(
                    0,
                    new ListItem("--Select Session--", ""));
            }
        }

        protected void ddlSession_SelectedIndexChanged(
        object sender,
            EventArgs e)
               {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd =
                    new SqlCommand(
                    @"
            SELECT DISTINCT
                C.CourseCode
            FROM Courses C

            INNER JOIN CourseOfferings CO
                ON C.CourseID = CO.CourseID

            WHERE CO.Session = @Session
            ",
                    conn);

                cmd.Parameters.AddWithValue(
                    "@Session",
                    ddlSession.SelectedValue);

                ddlCourse.DataSource =
                    cmd.ExecuteReader();

                ddlCourse.DataTextField = "CourseCode";
                ddlCourse.DataValueField = "CourseCode";
                ddlCourse.DataBind();

                ddlCourse.Items.Insert(
                    0,
                    new ListItem("--Select Course--", ""));
            }
        }

        protected void btnFilter_Click(
            object sender,
            EventArgs e)
        {
            LoadMarks();
        }

        private void LoadMarks()
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd =
                    new SqlCommand(
                    @"
                    SELECT
                        CM.MarkID,
                        S.StudentID,
                        U.FullName AS StudentName,

                        CM.AssignmentMark,
                        CM.QuizMark,
                        CM.MidTestMark,
                        CM.FinalExamMark,

                        CM.FinalMark,
                        CM.FinalGrade,
                        CM.GradePoint

                    FROM CourseMarks CM

                    INNER JOIN EnrollmentDetails ED
                        ON CM.DetailID = ED.DetailID

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

                    ORDER BY S.StudentID
                    ",
                    conn);

                    cmd.Parameters.AddWithValue(
                    "@Session",
                    ddlSession.SelectedValue);

                    cmd.Parameters.AddWithValue(
                    "@Course",
                    ddlCourse.SelectedValue);

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                DataTable dt =
                    new DataTable();

                da.Fill(dt);

                gvMarks.DataSource = dt;
                gvMarks.DataBind();
            }
        }

        protected void btnEdit_Click(
            object sender,
            EventArgs e)
        {
            foreach (GridViewRow row in gvMarks.Rows)
            {
                ((TextBox)row.FindControl("txtAssignment")).Enabled = true;
                ((TextBox)row.FindControl("txtQuiz")).Enabled = true;
                ((TextBox)row.FindControl("txtMidTest")).Enabled = true;
                ((TextBox)row.FindControl("txtFinalExam")).Enabled = true;
            }
        }

        private (string grade, decimal gradePoint)
            CalculateGrade(decimal mark)
        {
            if (mark >= 90)
                return ("A+", 4.00m);

            if (mark >= 80)
                return ("A", 4.00m);

            if (mark >= 75)
                return ("A-", 3.67m);

            if (mark >= 70)
                return ("B+", 3.33m);

            if (mark >= 65)
                return ("B", 3.00m);

            if (mark >= 60)
                return ("B-", 2.67m);

            if (mark >= 55)
                return ("C+", 2.33m);

            if (mark >= 50)
                return ("C", 2.00m);

            if (mark >= 45)
                return ("C-", 1.50m);

            if (mark >= 40)
                return ("D", 1.00m);

            return ("F", 0.00m);
        }

        protected void btnSave_Click(
            object sender,
            EventArgs e)
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                foreach (GridViewRow row in gvMarks.Rows)
                {
                    int markID =
                        Convert.ToInt32(
                        row.Cells[0].Text);

                    decimal assignment =
                        string.IsNullOrEmpty(
                        ((TextBox)row.FindControl("txtAssignment")).Text)
                        ? 0
                        : Convert.ToDecimal(
                        ((TextBox)row.FindControl("txtAssignment")).Text);

                    decimal quiz =
                        string.IsNullOrEmpty(
                        ((TextBox)row.FindControl("txtQuiz")).Text)
                        ? 0
                        : Convert.ToDecimal(
                        ((TextBox)row.FindControl("txtQuiz")).Text);

                    decimal midTest =
                        string.IsNullOrEmpty(
                        ((TextBox)row.FindControl("txtMidTest")).Text)
                        ? 0
                        : Convert.ToDecimal(
                        ((TextBox)row.FindControl("txtMidTest")).Text);

                    decimal finalExam =
                        string.IsNullOrEmpty(
                        ((TextBox)row.FindControl("txtFinalExam")).Text)
                        ? 0
                        : Convert.ToDecimal(
                        ((TextBox)row.FindControl("txtFinalExam")).Text);

                    decimal finalMark =
                        assignment +
                        quiz +
                        midTest +
                        finalExam;

                    var result =
                        CalculateGrade(finalMark);

                    string grade =
                        result.grade;

                    decimal gradePoint =
                        result.gradePoint;

                    SqlCommand cmd =
                        new SqlCommand(
                        @"
                        UPDATE CourseMarks
                        SET AssignmentMark = @Assignment,
                            QuizMark = @Quiz,
                            MidTestMark = @MidTest,
                            FinalExamMark = @FinalExam,
                            FinalMark = @FinalMark,
                            FinalGrade = @Grade,
                            GradePoint = @GradePoint
                        WHERE MarkID = @MarkID
                        ",
                        conn);

                    cmd.Parameters.AddWithValue(
                        "@Assignment",
                        assignment);

                    cmd.Parameters.AddWithValue(
                        "@Quiz",
                        quiz);

                    cmd.Parameters.AddWithValue(
                        "@MidTest",
                        midTest);

                    cmd.Parameters.AddWithValue(
                        "@FinalExam",
                        finalExam);

                    cmd.Parameters.AddWithValue(
                        "@FinalMark",
                        finalMark);

                    cmd.Parameters.AddWithValue(
                        "@Grade",
                        grade);

                    cmd.Parameters.AddWithValue(
                        "@GradePoint",
                        gradePoint);

                    cmd.Parameters.AddWithValue(
                        "@MarkID",
                        markID);

                    cmd.ExecuteNonQuery();
                }
            }

            LoadMarks();
        }
    }
}