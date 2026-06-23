<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="LecturerDashboard.aspx.cs"
    Inherits="EduCampus.Dashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Lecturer Dashboard</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />

    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
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
                            <a class="nav-link" href="Dashboard.aspx">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="CourseMaterial.aspx">Course Materials</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="AttendanceManagement.aspx">Attendance</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="LecturerMarks.aspx">Marks</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link active" href="LecturerAnnouncements.aspx">Announcements</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="LecturerProfile.aspx">Profile</a>
                        </li>
                    </ul>

                    <!-- Logout button -->
                    <asp:Button ID="Button1" runat="server"
                        Text="Logout"
                        CssClass="btn btn-danger"
                        OnClick="btnLogout_Click" />
                </div>
            </div>
        </nav>

        <style>
            body {
                background-color: #A4D8FF;
                font-family: Arial, sans-serif;
                margin: 0;
            }

            .container {
                width: 95%;
                max-width: 1200px;
                margin: 30px auto;
                text-align: center;
                background: white;
                padding: 25px;
                border-radius: 10px;
            }

            .section {
                margin-top: 40px;
                text-align: center;
            }

            h2, h3 {
                text-align: center;
                margin-bottom: 20px;
            }

            .grid {
                margin: 0 auto;
                width: auto;
            }

            table {
                margin: 0 auto;
            }

            select {
                padding: 8px;
                min-width: 450px;
            }

            .chart-container {
                width: 700px;
                max-width: 100%;
                height: 400px;
                margin: 20px auto 0 auto;
            }

            .message {
                text-align: center;
                font-weight: bold;
                margin-top: 15px;
            }
        </style>

        <div class="container">
            <h2>Lecturer Dashboard</h2>

            <div class="section">
                <h3>Assigned Courses</h3>
                <asp:GridView ID="gvAssignedCourses"
                    runat="server"
                    CssClass="grid">
                </asp:GridView>
            </div>

            <div class="section">
                <h3>Select Course</h3>

                <asp:DropDownList
                    ID="ddlCourse"
                    runat="server"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                </asp:DropDownList>

                <br /><br />

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    CssClass="message">
                </asp:Label>
            </div>

            <div class="section">
                <h3>Poor Attendance Students</h3>

                <asp:GridView
                    ID="gvPoorAttendance"
                    runat="server"
                    CssClass="grid">
                </asp:GridView>
            </div>

            <div class="section">
                <h3>Student Grade Distribution</h3>

                <div class="chart-container">
                    <canvas id="gradeChart"></canvas>
                </div>
            </div>
        </div>

        <script>
            const gradeLabels = <%= GradeLabelsJson %>;
            const gradeData = <%= GradeDataJson %>;

            const ctx = document.getElementById('gradeChart');

            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: gradeLabels,
                    datasets: [{
                        label: 'Number of Students',
                        data: gradeData,
                        backgroundColor: 'rgba(54, 162, 235, 0.6)',
                        borderColor: 'rgba(54, 162, 235, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: true
                        }
                    },
                    scales: {
                        x: {
                            title: {
                                display: true,
                                text: 'Grade'
                            }
                        },
                        y: {
                            beginAtZero: true,
                            ticks: {
                                precision: 0
                            },
                            title: {
                                display: true,
                                text: 'Number of Students'
                            }
                        }
                    }
                }
            });
        </script>
    </form>
</body>
</html>
