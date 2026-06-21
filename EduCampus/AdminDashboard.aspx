<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="EduCampus.AdminDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Admin Dashboard</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    
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
                             <a class="nav-link active" href="AdminDashboard.aspx">Dashboard</a>
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
                            <a class="nav-link" href="AcademicResults.aspx">Results</a>
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
        <div class="container mt-4">
            <h3 class="text-center">Admin Dashboard</h3>
            <h4 class="mb-4">Welcome, Admin</h4>

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

            <!-- Charts Section -->
            <div class="container mt-5 mb-4">

                <div class="row justify-content-center g-4">

                    <!-- Bar Chart -->
                    <div class="col-lg-7 col-md-12 d-flex justify-content-center">

                        <div class="card shadow-sm w-100" style="height: 420px; max-width: 700px;">

                            <div class="card-header text-center">
                                <h5>Students per Programme</h5>
                            </div>

                            <div class="card-body d-flex justify-content-center align-items-center">

                                <canvas id="programmeChart"></canvas>

                            </div>

                        </div>

                    </div>

                    <!-- Pie Chart -->
                    <div class="col-lg-5 col-md-12 d-flex justify-content-center">

                        <div class="card shadow-sm w-100" style="height: 420px; max-width: 400px;">

                            <div class="card-header text-center">
                                <h5>Enrollment Status</h5>
                            </div>

                            <div class="card-body d-flex justify-content-center align-items-center">

                                <canvas id="enrollmentChart"></canvas>

                            </div>

                        </div>

                    </div>

                </div>

            </div>

        </div>
        
    </form>

    <script>

    // Students per Programme
    new Chart(document.getElementById('programmeChart'),
    {

        type: 'bar',

        data: {

            labels: [<%= ProgrammeLabels %>],

            datasets: [{

                label: 'Students',

                data: [<%= ProgrammeCounts %>],

                backgroundColor: 'rgba(255, 99, 132, 0.7)',

                borderColor: 'rgba(255, 99, 132, 1)',

                borderWidth: 1

            }]
        },

        options: {

            responsive: true,

            maintainAspectRatio: false,

            plugins: {

                title: {

                    display: true,

                    text: 'Student Distribution by Programme'

                },

                legend: {

                    display: false

                }

            },

            scales: {

                y: {

                    beginAtZero: true,

                    ticks: {

                        precision: 0

                    }

                }

            }

        }

    });


    // Enrollment Status
    new Chart(document.getElementById('enrollmentChart'),
    {

        type: 'pie',

        data: {

            labels: [<%= EnrollmentLabels %>],

            datasets: [{

                data: [<%= EnrollmentCounts %>],

                backgroundColor: [

                    'rgba(255, 206, 86, 0.7)',   // Pending

                    'rgba(75, 192, 192, 0.7)',   // Approved

                    'rgba(255, 99, 132, 0.7)'    // Rejected

                ],

                borderWidth: 1
            }]
        },

        options: {

            responsive: true,

            maintainAspectRatio: false,

            plugins: {

                title: {

                    display: true,

                    text: 'Enrollment Status Overview'

                },

                legend: {

                    position: 'bottom'

                }

            }

        }

    });

    </script>
</body>
</html>