<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminAttendance.aspx.cs" Inherits="EduCampus.AdminAttendance" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>View Attendance</title>

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
                              <a class="nav-link" href="AdminDashboard.aspx">Dashboard</a>
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
                             <a class="nav-link active" href="AdminAttendance.aspx">Attendance</a>
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

        <!-- Content -->
        <div class="container mt-4">

            <h3 class="text-center mb-2">Student Attendance</h3>

            <!-- Filter section: Session, Course, and Date selection -->
            <div class="w-75 mx-auto">

                <!-- Session dropdown -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">Session</label>

                    <asp:DropDownList ID="ddlRecordSession"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlRecordSession_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>


                <!-- Course dropdown -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">Course</label>

                    <asp:DropDownList ID="ddlRecordCourse"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlRecordCourse_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <!-- Date filter -->
                <div class="mb-4">
                    <label class="form-label fw-semibold">Attendance Date</label>

                    <asp:TextBox ID="txtAttendanceDate" 
                        runat="server" 
                        CssClass="form-control"
                        TextMode="Date">
                    </asp:TextBox>

                </div>

                <!-- Search Button -->
                <div class="text-center">
                    <asp:Button ID="btnSearch"
                        runat="server"
                        Text="Search"
                        CssClass="btn btn-primary"
                        OnClick="btnSearch_Click" />
                </div>

            </div>

            <!-- Search result section (hidden until user clicks Search) -->
            <asp:Panel ID="pnlSearchResult" runat="server" Visible="false">

                <!-- Attendance Summary -->
                <h5 class="mt-4 mb-3">Attendance Summary</h5>

                <div class="row mb-4 mx-10">

                    <!-- Card 1 -->
                    <div class="col-md-3">
                        <div class="card text-center border-primary">
                            <div class="card-body">

                                <h5>Total Records</h5>
                                <asp:Label ID="lblTotalRecords"
                                    runat="server"
                                    Font-Size="25px"
                                    Font-Bold="True">
                                </asp:Label>

                            </div>
                        </div>
                    </div>

                    <!-- Card 2 -->
                    <div class="col-md-3">
                        <div class="card text-center border-success">
                            <div class="card-body">

                                <h5 class="text-success">Present</h5>
                                <asp:Label ID="lblPresent"
                                    runat="server"
                                    Font-Size="25px"
                                    Font-Bold="True">
                                </asp:Label>

                            </div>
                        </div>
                    </div>

                    <!-- Card 3 -->
                    <div class="col-md-3">
                        <div class="card text-center border-danger">
                            <div class="card-body">

                                <h5 class="text-danger">Absent</h5>
                                <asp:Label ID="lblAbsent"
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

                                <h5>Attendance Rate</h5>
                                <asp:Label ID="lblRate"
                                    runat="server"
                                    Font-Size="25px"
                                    Font-Bold="True">
                                </asp:Label>

                            </div>
                        </div>
                    </div>

                </div>

                <!-- Attendance Records -->
                <h5 class="mt-5 mb-3">Attendance Records</h5>

                <!-- Attendance Records Grid -->
                <asp:GridView ID="gvAttendance"
                    runat="server"
                    AutoGenerateColumns="False"
                    EmptyDataText="No attendance records found"
                    CssClass="table table-bordered table-striped">

                    <Columns>

                        <asp:BoundField DataField="FullName" HeaderText="Student Name" />
                        <asp:BoundField DataField="StudentID" HeaderText="Student ID" />
                        <asp:BoundField DataField="AttendanceDate" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                        
                        <asp:TemplateField HeaderText="Status">

                            <ItemTemplate>
                                <span class='<%# Eval("Status").ToString() == "Present" ? "present" : "absent" %>'>
                                    <%# Eval("Status") %>
                                </span>
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />

                    </Columns>

                </asp:GridView>

                <!-- Download PDF button-->
                <div class="text-center mt-4">
                    <asp:Button ID="btnPDF" runat="server"
                        Text="Download PDF"
                        CssClass="btn btn-danger me-4"
                        OnClick="btnPDF_Click" />
                </div>

                <!-- Poor Attendance Students -->
                <h5 class="mt-4 mb-3">Poor Attendance Students</h5>

                <!-- Poor Attendance Students Grid -->
                <asp:GridView ID="gvPoorAttendance"
                    runat="server"
                    AutoGenerateColumns="False"
                    EmptyDataText="No poor attendance students found"
                    CssClass="table table-bordered table-striped">

                    <Columns>

                        <asp:BoundField DataField="FullName" HeaderText="Student Name" />
                        <asp:BoundField DataField="StudentID" HeaderText="Student ID" />
                        <asp:BoundField DataField="AbsentCount" HeaderText="Absent Count" />

                    </Columns>

                 </asp:GridView>

            </asp:Panel>

        </div>

    </form>
</body>
</html>
