<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentCalendar.aspx.cs" Inherits="EduCampus.StudentCalendar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Academic Calendar</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
    <link rel="stylesheet" href="student.css" />
</head>

<body>
    <form id="form1" runat="server">
         <!-- Navigation bar -->
         <nav class="navbar navbar-expand-lg bg-white">
             <div class="container-fluid">
                 <img src="logo.jpeg" alt="Logo" width="50" height="50" class="me-2">

                 <div class="collapse navbar-collapse">
                     <ul class="navbar-nav me-auto">

                         <li class="nav-item">
                             <a class="nav-link" href="StudentDashboard.aspx">Dashboard</a>
                         </li>

                         <li class="nav-item">
                             <a class="nav-link" href="StudentEnrollCourse.aspx">Course</a>
                         </li>

                         <li class="nav-item">
                             <a class="nav-link" href="Attendance.aspx">Attendance</a>
                         </li>

                         <li class="nav-item">
                             <a class="nav-link" href="Results.aspx">Results</a>
                         </li>

                         <li class="nav-item">
                             <a class="nav-link" href="StudentNotification.aspx">Notifications</a>
                         </li>


                         <li class="nav-item">
                             <a class="nav-link" href="StudentNotes.aspx">Notes</a>
                         </li>

                         <li class="nav-item">
                             <a class="nav-link active" href="StudentCalendar.aspx">Academic Calendar</a>
                         </li>

                         <li class="nav-item">
                             <a class="nav-link" href="StudentProfile.aspx">Profile</a>
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

        <!-- Academic Calendar List -->
        <div class="container container-box">
            <h2 class="page-title mt-4 mb-4">📅 Academic Calendar</h2>

            <!-- Search -->
            <div class="row mb-4">
                <div class="col-md-4">
                    <asp:TextBox ID="txtSearch" runat="server"
                        CssClass="form-control"
                        Placeholder="Enter session or event" />
                </div>

                <div class="col-md-2">
                    <asp:Button ID="btnSearch" runat="server" Text="Search"
                        CssClass="btn btn-primary w-100"
                        OnClick="btnSearch_Click" />
                </div>

                <div class="col-md-2">
                    <asp:Button ID="btnReset" runat="server" Text="Reset"
                        CssClass="btn btn-secondary w-100"
                        OnClick="btnReset_Click" />
                </div>
            </div>

            <!-- Gridview -->
            <asp:GridView ID="gvCalendar" runat="server"
                CssClass="table table-bordered table-striped"
                AutoGenerateColumns="False"
                EmptyDataText="No academic calendar found">

                <Columns>
                    <asp:TemplateField HeaderText="No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Session" HeaderText="Session" />
                
                    <asp:TemplateField HeaderText="Date">

                        <ItemTemplate>
                            <%# FormatDate(Eval("StartDate"), Eval("EndDate")) %>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:BoundField DataField="Event" HeaderText="Event" ItemStyle-Width="750px" />

                </Columns>

            </asp:GridView>

        </div>

    </form>
</body>
</html>
