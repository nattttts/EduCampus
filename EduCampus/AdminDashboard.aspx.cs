using System;
using System.Configuration;
using System.Data.SqlClient;

namespace EduCampus
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["EduCampusDB"].ConnectionString;
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
                LoadDashboard();
            }
        }

        // Logout
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();              // clear session
            Response.Redirect("Login.aspx"); // go back to login
        }

        private void LoadDashboard()
        {
            SqlConnection con = new SqlConnection(cs);

            con.Open();

            // Total Programme
            string totalProgrammesQuery = "SELECT COUNT(*) FROM Programmes";
            SqlCommand totalProgrammesCmd = new SqlCommand(totalProgrammesQuery, con);
            lblTotalProgrammes.Text = totalProgrammesCmd.ExecuteScalar().ToString();

            // Total Course
            string totalCoursesQuery = "SELECT COUNT(*) FROM Courses";
            SqlCommand totalCoursesCmd = new SqlCommand(totalCoursesQuery, con);
            lblTotalCourses.Text = totalCoursesCmd.ExecuteScalar().ToString();

            // Total Lecturer
            string totalLecturersQuery = "SELECT COUNT(*) FROM Lecturers";
            SqlCommand totalLecturersCmd = new SqlCommand(totalLecturersQuery, con);
            lblTotalLecturers.Text = totalLecturersCmd.ExecuteScalar().ToString();

            // Total Student
            string totalStudentsQuery = "SELECT COUNT(*) FROM Students";
            SqlCommand totalStudentsCmd = new SqlCommand(totalStudentsQuery, con);
            lblTotalStudents.Text = totalStudentsCmd.ExecuteScalar().ToString();

            // Total Enrollment
            string totalEnrollmentsQuery = "SELECT COUNT(*) FROM EnrollmentMaster";
            SqlCommand totalEnrollmentsCmd = new SqlCommand(totalEnrollmentsQuery, con);
            lblTotalEnrollments.Text = totalEnrollmentsCmd.ExecuteScalar().ToString();

            // Pending
            string pendingQuery = @"SELECT COUNT(*) FROM EnrollmentMaster 
                                    WHERE Status='Pending'";
            SqlCommand pendingCmd = new SqlCommand(pendingQuery, con);
            lblPending.Text = pendingCmd.ExecuteScalar().ToString();

            // Approved
            string approvedQuery = @"SELECT COUNT(*) FROM EnrollmentMaster
                                     WHERE Status='Approved'";
            SqlCommand approvedCmd = new SqlCommand(approvedQuery, con);
            lblApproved.Text = approvedCmd.ExecuteScalar().ToString();

            // Rejected
            string rejectedQuery = @"SELECT COUNT(*) FROM EnrollmentMaster
                                     WHERE Status='Rejected'";
            SqlCommand rejectedCmd = new SqlCommand(rejectedQuery, con);
            lblRejected.Text = rejectedCmd.ExecuteScalar().ToString();

            // Total Course Offerings
            string courseOfferingsQuery = "SELECT COUNT(*) FROM CourseOfferings";
            SqlCommand courseOfferingsCmd = new SqlCommand(courseOfferingsQuery, con);
            lblCourseOfferings.Text = courseOfferingsCmd.ExecuteScalar().ToString();

            // Total Announcements
            string totalAnnouncementsQuery = @"SELECT COUNT(*) FROM Announcements
                                               WHERE OfferingID IS NULL";
            SqlCommand totalAnnouncementsCmd = new SqlCommand(totalAnnouncementsQuery, con);
            lblTotalAnnouncements.Text = totalAnnouncementsCmd.ExecuteScalar().ToString();

            // Total Academic Events
            string totalAcademicEventsQuery = "SELECT COUNT(*) FROM AcademicCalendar";
            SqlCommand totalAcademicEventsCmd = new SqlCommand(totalAcademicEventsQuery, con);
            lblTotalAcademicEvents.Text = totalAcademicEventsCmd.ExecuteScalar().ToString();

            con.Close();
        }
    }
}