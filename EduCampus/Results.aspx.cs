using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace EduCampus
{
    public partial class Results : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ViewState["Semester"] = "Semester 1";

                SetActiveSemesterButton("Semester 1");

                LoadStudentInfo("Semester 1");
                LoadResults("Semester 1");

                CalculateGPA("Semester 1");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentDashboard.aspx");
        }

        protected void btnSem_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            string semester = btn.CommandArgument;

            ViewState["Semester"] = semester;

            SetActiveSemesterButton(semester);

            LoadStudentInfo(semester);
            LoadResults(semester);

            CalculateGPA(semester);
        }

        private void LoadStudentInfo(string semester)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT TOP 1
                u.FullName,
                s.StudentID,
                p.ProgrammeName,
                em.Session,
                em.Semester
                FROM Users u
                INNER JOIN Students s
                    ON u.UserID = s.UserID
                INNER JOIN Programmes p
                    ON s.ProgrammeID = p.ProgrammeID
                INNER JOIN EnrollmentMaster em
                    ON s.StudentID = em.StudentID
                WHERE u.Email = @Email
                AND em.Semester = @Semester";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Email",
                    Session["Email"]);

                cmd.Parameters.AddWithValue("@Semester",
                    semester);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblStudentName.Text =
                        "Name : " + dr["FullName"];

                    lblStudentID.Text =
                        "Student ID : " + dr["StudentID"];

                    lblProgramme.Text =
                        "Programme : " + dr["ProgrammeName"];

                    lblSession.Text =
                        "Session : " + dr["Session"];

                    lblSemester.Text =
                        "Semester : " + dr["Semester"];

                    lblDate.Text =
                        "Date : " + DateTime.Now.ToShortDateString();

                    // GPA and CGPA will be calculated separately
                    lblGPA.Text = "";
                    lblCGPA.Text = "";
                }

                else
                {

                    lblStudentName.Text =
                        "Name : " + Session["StudentName"];

                    lblStudentID.Text =
                        "Student ID : " + Session["StudentID"];

                    lblProgramme.Text =
                        "Programme : " + Session["Programme"];

                    lblSession.Text =
                        "Session : " + Session["Session"];

                    lblSemester.Text =
                        "Semester : " + semester;

                    lblDate.Text =
                        "Date : " + DateTime.Now.ToShortDateString();

                    // No result available
                    lblGPA.Text = "GPA : N/A";

                    lblCGPA.Text = "CGPA : N/A";
                }
            }
        }

        private void CalculateGPA(string semester)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {

                string query = @"

                SELECT 
                    cm.GradePoint,
                    c.CreditHours

                FROM CourseMarks cm

                INNER JOIN EnrollmentDetails ed
                    ON cm.DetailID = ed.DetailID

                INNER JOIN EnrollmentMaster em
                    ON ed.EnrolmentID = em.EnrolmentID

                INNER JOIN CourseOfferings co
                    ON ed.OfferingID = co.OfferingID

                INNER JOIN Courses c
                    ON co.CourseID = c.CourseID

                WHERE em.StudentID =
                (
                    SELECT StudentID
                    FROM Students
                    WHERE UserID =
                    (
                        SELECT UserID
                        FROM Users
                        WHERE Email=@Email
                    )
                )

                AND em.Semester=@Semester";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Email",
                    Session["Email"]);

                cmd.Parameters.AddWithValue("@Semester",
                    semester);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                double totalPoint = 0;
                int totalCredit = 0;

                while (dr.Read())
                {
                    double gradePoint =
                        Convert.ToDouble(dr["GradePoint"]);

                    int credit =
                        Convert.ToInt32(dr["CreditHours"]);

                    totalPoint += gradePoint * credit;

                    totalCredit += credit;

                }

                if (totalCredit > 0)
                {
                    double gpa =
                        totalPoint / totalCredit;

                    lblGPA.Text =
                        "GPA : " + gpa.ToString("0.00");

                }
                else
                {
                    lblGPA.Text =
                        "GPA : N/A";
                }

                dr.Close();

                CalculateCGPA();

            }

        }

        private void CalculateCGPA()
        {

            using (SqlConnection con = new SqlConnection(cs))
            {

                string query = @"

                SELECT 
                    cm.GradePoint,
                    c.CreditHours

                FROM CourseMarks cm

                INNER JOIN EnrollmentDetails ed
                    ON cm.DetailID = ed.DetailID

                INNER JOIN EnrollmentMaster em
                    ON ed.EnrolmentID = em.EnrolmentID

                INNER JOIN CourseOfferings co
                    ON ed.OfferingID = co.OfferingID

                INNER JOIN Courses c
                    ON co.CourseID = c.CourseID

                WHERE em.StudentID =
                (
                    SELECT StudentID
                    FROM Students
                    WHERE UserID =
                    (
                        SELECT UserID
                        FROM Users
                        WHERE Email=@Email
                    )
                )

                ";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Email",
                    Session["Email"]);

                con.Open();

                SqlDataReader dr =
                    cmd.ExecuteReader();

                double totalPoint = 0;

                int totalCredit = 0;

                while (dr.Read())
                {

                    double gradePoint =
                        Convert.ToDouble(dr["GradePoint"]);

                    int credit =
                        Convert.ToInt32(dr["CreditHours"]);

                    totalPoint += gradePoint * credit;


                    totalCredit += credit;

                }

                if (totalCredit > 0)
                {

                    double cgpa =
                        totalPoint / totalCredit;

                    lblCGPA.Text =
                        "CGPA : " + cgpa.ToString("0.00");

                }
                else
                {

                    lblCGPA.Text =
                        "CGPA : N/A";

                }

            }

        }

        void LoadResults(string semester)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                SELECT
                    c.CourseCode,
                    c.CourseName,
                    c.CreditHours,

                    cm.AssignmentMark,
                    cm.QuizMark,
                    cm.MidTestMark,
                    cm.FinalExamMark,

                    cm.FinalMark,
                    cm.FinalGrade,
                    cm.GradePoint

                FROM CourseMarks cm

                INNER JOIN EnrollmentDetails ed
                    ON cm.DetailID = ed.DetailID

                INNER JOIN EnrollmentMaster em
                    ON ed.EnrolmentID = em.EnrolmentID

                INNER JOIN CourseOfferings co
                    ON ed.OfferingID = co.OfferingID

                INNER JOIN Courses c
                    ON co.CourseID = c.CourseID

                WHERE em.StudentID =
                (
                    SELECT StudentID
                    FROM Students
                    WHERE UserID =
                    (
                        SELECT UserID
                        FROM Users
                        WHERE Email = @Email
                    )
                )
                AND em.Semester = @Semester";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Session["Email"]);
                cmd.Parameters.AddWithValue("@Semester", semester);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    pnlResult.Visible = true;
                    pnlNoResult.Visible = false;


                    gvResults.DataSource = dt;
                    gvResults.DataBind();

                }
                else
                {

                    pnlResult.Visible = false;
                    pnlNoResult.Visible = true;


                    gvResults.DataSource = null;
                    gvResults.DataBind();


                }
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        private void SetActiveSemesterButton(string semester)
        {
            btnSem1.CssClass = "btn btn-secondary mx-2";
            btnSem2.CssClass = "btn btn-secondary mx-2";

            if (semester == "Semester 1")
            {
                btnSem1.CssClass = "btn btn-primary mx-2";
            }
            else if (semester == "Semester 2")
            {
                btnSem2.CssClass = "btn btn-primary mx-2";
            }
        }
    }
}