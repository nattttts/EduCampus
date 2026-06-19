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
                LoadSemester();
            }
        }

        private void LoadSemester()
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd =
                    new SqlCommand(
                    @"SELECT DISTINCT Semester
                      FROM CourseOfferings",
                    conn);

                ddlSemester.DataSource =
                    cmd.ExecuteReader();

                ddlSemester.DataTextField = "Semester";
                ddlSemester.DataValueField = "Semester";
                ddlSemester.DataBind();

                ddlSemester.Items.Insert(
                    0,
                    new ListItem("--Select Semester--", ""));
            }
        }

        protected void ddlSemester_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd =
                    new SqlCommand(
                    @"SELECT DISTINCT CourseCode
                      FROM CourseOfferings
                      WHERE Semester=@Semester",
                    conn);

                cmd.Parameters.AddWithValue(
                    "@Semester",
                    ddlSemester.SelectedValue);

                ddlCourse.DataSource =
                    cmd.ExecuteReader();

                ddlCourse.DataTextField = "CourseCode";
                ddlCourse.DataValueField = "CourseCode";
                ddlCourse.DataBind();
            }
        }

        protected void ddlCourse_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand cmd =
                    new SqlCommand(
                    @"SELECT DISTINCT ClassName
                      FROM CourseOfferings
                      WHERE Semester=@Semester
                      AND CourseCode=@Course",
                    conn);

                cmd.Parameters.AddWithValue(
                    "@Semester",
                    ddlSemester.SelectedValue);

                cmd.Parameters.AddWithValue(
                    "@Course",
                    ddlCourse.SelectedValue);

                ddlClass.DataSource =
                    cmd.ExecuteReader();

                ddlClass.DataTextField = "ClassName";
                ddlClass.DataValueField = "ClassName";
                ddlClass.DataBind();
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
                        R.ResultID,
                        S.StudentID,
                        S.StudentName,
                        R.Mark,
                        R.Grade
                    FROM Results R

                    INNER JOIN Students S
                        ON R.StudentID = S.StudentID

                    INNER JOIN Enrolments E
                        ON R.EnrolmentID = E.EnrolmentID

                    INNER JOIN CourseOfferings C
                        ON E.OfferingID = C.OfferingID

                    WHERE C.Semester=@Semester
                    AND C.CourseCode=@Course
                    AND C.ClassName=@Class
                    ",
                    conn);

                cmd.Parameters.AddWithValue(
                    "@Semester",
                    ddlSemester.SelectedValue);

                cmd.Parameters.AddWithValue(
                    "@Course",
                    ddlCourse.SelectedValue);

                cmd.Parameters.AddWithValue(
                    "@Class",
                    ddlClass.SelectedValue);

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
                TextBox txtMark =
                    (TextBox)row.FindControl("txtMark");

                txtMark.Enabled = true;
            }
        }

        private string CalculateGrade(decimal mark)
        {
            if (mark >= 80)
                return "A";

            if (mark >= 70)
                return "B";

            if (mark >= 60)
                return "C";

            if (mark >= 50)
                return "D";

            return "F";
        }

        protected void btnSave_Click(
            object sender,
            EventArgs e)
        {
            using (SqlConnection conn =
                new SqlConnection(connStr))
            {
                conn.Open();

                foreach (GridViewRow row
                    in gvMarks.Rows)
                {
                    int resultID =
                        Convert.ToInt32(
                        row.Cells[0].Text);

                    TextBox txtMark =
                        (TextBox)row.FindControl("txtMark");

                    decimal mark =
                        Convert.ToDecimal(
                        txtMark.Text);

                    string grade =
                        CalculateGrade(mark);

                    SqlCommand cmd =
                        new SqlCommand(
                        @"UPDATE Results
                          SET Mark=@Mark,
                              Grade=@Grade
                          WHERE ResultID=@ResultID",
                        conn);

                    cmd.Parameters.AddWithValue(
                        "@Mark",
                        mark);

                    cmd.Parameters.AddWithValue(
                        "@Grade",
                        grade);

                    cmd.Parameters.AddWithValue(
                        "@ResultID",
                        resultID);

                    cmd.ExecuteNonQuery();

                    txtMark.Enabled = false;
                }
            }

            LoadMarks();
        }
    }
}