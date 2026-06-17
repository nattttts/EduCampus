<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="EduCampus.AdminDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Admin Dashboard</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
    
    <style>
        .card h5
        {
            color: darkblue;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <!-- Navigation bar -->
        <nav class="navbar navbar-expand-lg bg-white">
            <div class="container-fluid">
                <img src="logo.jpeg" alt="Logo" width="50" height="50" class="me-2">

                <div class="collapse navbar-collapse">
                    <!-- Menu -->
                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                             <a class="nav-link active" href="AdminDashboard.aspx">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="ManageProgramme.aspx">Programme</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="ManageCourse.aspx">Courses</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="RegisterLecturer.aspx">Register Lecturer</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="AssignLecturer.aspx">Assign Lecturer</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="RegisterStudent.aspx">Register Student</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AdminViewEnrollment.aspx">Enrollment</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AdminAnnouncements.aspx">Announcements</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="ManageCalendar.aspx">Calendar</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AdminAttendance.aspx">Attendance</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Report</a>
                        </li>
                    </ul>

                    <!-- Logout button -->
                    <asp:Button ID="btnLogout" runat="server"
                        Text="Logout"
                        CssClass="btn btn-danger"
                        OnClick="btnLogout_Click" />

                </div>

            </div>
        </nav>

        <!-- Dashboard content -->
        <div class="px-4">
            <h3 class="text-center mt-4">Admin Dashboard</h3>
            <h4 class="mb-4">Welcome Administrator</h4>

            <!-- 1st Row Statistics -->
            <div class="row">

                <!-- Card 1 -->
                <div class="col-md-3">
                    <div class="card text-center border-primary">
                        <div class="card-body">

                            <h5>Total Programmes 🎓</h5>
                            <asp:Label ID="lblTotalProgrammes"
                                runat="server"
                                Font-Size="25px"
                                Font-Bold="True">
                            </asp:Label>

                        </div>
                    </div>
                </div>    

                <!-- Card 2 -->
                <div class="col-md-3">
                    <div class="card text-center border-primary">
                        <div class="card-body">

                            <h5>Total Courses 📚</h5>
                            <asp:Label ID="lblTotalCourses"
                                runat="server"
                                Font-Size="25px"
                                Font-Bold="True">
                            </asp:Label>

                        </div>
                    </div>
                </div>

                <!-- Card 3 -->
                <div class="col-md-3">
                    <div class="card text-center border-primary">
                        <div class="card-body">

                            <h5>Total Lecturers 👩‍🏫</h5>
                            <asp:Label ID="lblTotalLecturers"
                                runat="server"
                                Font-Size="25px"
                                Font-Bold="True">
                            </asp:Label>

                        </div>
                    </div>
                </div>

                <!-- Card 4 -->
                 <div class="col-md-3">
                     <div class="card text-center border-primary">
                         <div class="card-body">

                             <h5>Total Students 👩‍🎓</h5>
                             <asp:Label ID="lblTotalStudents"
                                 runat="server"
                                 Font-Size="25px"
                                 Font-Bold="True">
                             </asp:Label>

                         </div>
                     </div>
                 </div>

            </div>

            <!-- 2nd Row Statistics -->
            <div class="row mt-4">

                <!-- Card 5 -->
                <div class="col-md-3">
                    <div class="card text-center border-primary">
                        <div class="card-body">

                            <h5>Total Enrollments 📝</h5>
                            <asp:Label ID="lblTotalEnrollments"
                                runat="server"
                                Font-Size="25px"
                                Font-Bold="True">
                            </asp:Label>

                        </div>
                    </div>
                </div>

                <!-- Card 6 -->
                <div class="col-md-3">
                    <div class="card text-center border-warning">
                        <div class="card-body">

                            <h5 class="text-warning">Pending Enrollments ⌛</h5>
                            <asp:Label ID="lblPending"
                                runat="server"
                                Font-Size="25px"
                                Font-Bold="True">
                            </asp:Label>

                        </div>
                    </div>
                </div>

                <!-- Card 7 -->
                <div class="col-md-3">
                    <div class="card text-center border-success">
                        <div class="card-body">

                            <h5 class="text-success">Approved Enrollments ✅</h5>
                            <asp:Label ID="lblApproved"
                                runat="server"
                                Font-Size="25px"
                                Font-Bold="True">
                            </asp:Label>

                        </div>
                    </div>
                </div>

                <!-- Card 8 -->
                <div class="col-md-3">
                    <div class="card text-center border-danger">
                        <div class="card-body">

                            <h5 class="text-danger">Rejected Enrollments ❌</h5>
                            <asp:Label ID="lblRejected"
                                runat="server"
                                Font-Size="25px"
                                Font-Bold="True">
                            </asp:Label>

                        </div>
                    </div>
                </div>

            </div>

            <!-- 3rd Row Statistics -->
            <div class="row mt-4">

                <!-- Card 9 -->
                <div class="col-md-4">
                    <div class="card text-center border-primary">
                        <div class="card-body">

                            <h5>Total Course Offerings 📖</h5>
                            <asp:Label ID="lblCourseOfferings"
                                runat="server"
                                Font-Size="25px"
                                Font-Bold="True">
                            </asp:Label>

                        </div>
                    </div>
                </div>

                <!-- Card 10 -->
                <div class="col-md-4">
                    <div class="card text-center border-primary">
                        <div class="card-body">

                            <h5>Total Announcements 📢</h5>
                            <asp:Label ID="lblTotalAnnouncements"
                                runat="server"
                                Font-Size="25px"
                                Font-Bold="True">
                            </asp:Label>

                        </div>
                    </div>
                </div>

                <!-- Card 11 -->
                <div class="col-md-4">
                    <div class="card text-center border-primary">
                        <div class="card-body">

                            <h5>Total Academic Events 📅</h5>
                            <asp:Label ID="lblTotalAcademicEvents"
                                runat="server"
                                Font-Size="25px"
                                Font-Bold="True">
                            </asp:Label>

                        </div>
                    </div>
                </div>

            </div>

        </div>
        
    </form>
</body>
</html>