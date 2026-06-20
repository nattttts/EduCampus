<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCalendar.aspx.cs" Inherits="EduCampus.ManageCalendar" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Academic Calendar</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
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
                             <a class="nav-link" href="AdminDashboard.aspx">Home</a>
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
                            <a class="nav-link active" href="ManageCalendar.aspx">Calendar</a>
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

        <!-- Manage academic calendar form -->
        <div class="container mt-5 d-flex justify-content-center">
            <div class="card shadow p-4" style="width: 700px;">
                <h3 class="text-center mb-4">📅 Manage Academic Calendar</h3>

                <div class="mb-3">
                    <label class="form-label">Session</label>
                    <asp:DropDownList ID="ddlSession"
                        runat="server"
                        CssClass="form-select">

                        <asp:ListItem Selected="True">Jan2026</asp:ListItem>
                        <asp:ListItem>Apr2026</asp:ListItem>
                        <asp:ListItem>Aug2026</asp:ListItem>

                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <label class="form-label">Start Date</label>
                    <asp:TextBox ID="txtStartDate"
                        runat="server"
                        TextMode="Date"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label">End Date</label>
                    <asp:TextBox ID="txtEndDate"
                        runat="server"
                        TextMode="Date"
                        CssClass="form-control">
                    </asp:TextBox>

                    <small class="form-text"> For single-day events, use the same start and end date.</small>
                </div>

                <div class="mb-3">
                    <label class="form-label">Event</label>
                    <asp:TextBox ID="txtEvent" runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="3"
                        placeholder="Enter academic calendar event" />
                </div>

                <div class="d-flex gap-2">
                    <!-- Save button -->
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save Calendar Event"
                        CssClass="btn btn-primary"
                        OnClick="btnSave_Click" />
              
                    <!-- Clear button -->
                    <asp:Button ID="btnClear" runat="server"
                        Text="Clear"
                        CssClass="btn btn-secondary"
                        OnClick="btnClear_Click"/>
                </div>

                <div class="mt-3 text-center">
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </div>

            </div>
        </div>

        <!-- Academic Calendar List -->
        <div class="container mt-4">
            <div class="card shadow p-4">
                <h4 class="mb-3">📋 Academic Calendar</h4>

                 <!-- Search -->
                <div class="row mb-3">
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
                    DataKeyNames="CalendarID"
                    EmptyDataText="No academic calendar found"
                    OnRowEditing="gvCalendar_RowEditing"
                    OnRowUpdating="gvCalendar_RowUpdating"
                    OnRowCancelingEdit="gvCalendar_RowCancelingEdit"
                    OnRowDeleting="gvCalendar_RowDeleting">

                    <Columns>
                        <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Session" HeaderText="Session" ReadOnly="true"/>
                        
                        <asp:TemplateField HeaderText="Date">

                            <ItemTemplate>
                                <%# FormatDate(Eval("StartDate"), Eval("EndDate")) %>
                            </ItemTemplate>

                            <EditItemTemplate>

                                <asp:TextBox ID="txtEditStartDate"
                                    runat="server"
                                    Text='<%# Bind("StartDate","{0:yyyy-MM-dd}") %>'
                                    TextMode="Date"
                                    CssClass="form-control mb-1">
                                </asp:TextBox>
                                    
                                <asp:TextBox ID="txtEditEndDate"
                                    runat="server"
                                    Text='<%# Bind("EndDate","{0:yyyy-MM-dd}") %>'
                                    TextMode="Date"
                                    CssClass="form-control">
                                </asp:TextBox>

                            </EditItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Event" ItemStyle-Width="600px">
                            
                            <ItemTemplate>
                                <%# Eval("Event") %>
                            </ItemTemplate>

                            <EditItemTemplate>
                                 <asp:TextBox ID="txtEditEvent"
                                     runat="server"
                                     Text='<%# Bind("Event") %>'
                                     TextMode="MultiLine"
                                     Rows="3"
                                     CssClass="form-control">
                                 </asp:TextBox>

                            </EditItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action">

                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CommandName="Edit"
                                    Text="Edit"
                                    CssClass="btn btn-primary btn-sm me-2" />

                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CommandName="Delete"
                                    Text="Delete"
                                    CssClass="btn btn-danger btn-sm"
                                    OnClientClick="return confirm('Are you sure you want to delete this event?');" />
                            </ItemTemplate>

                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" runat="server"
                                    CommandName="Update"
                                    Text="Update"
                                    CssClass="btn btn-success btn-sm me-2" />

                                <asp:LinkButton ID="btnCancel" runat="server"
                                    CommandName="Cancel"
                                    Text="Cancel"
                                    CssClass="btn btn-secondary btn-sm" />
                            </EditItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>
        </div>

    </form>
</body>
</html>
