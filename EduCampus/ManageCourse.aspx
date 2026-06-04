<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCourse.aspx.cs" Inherits="EduCampus.ManageCourse" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Courses</title>

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
                             <a class="nav-link active" href="ManageCourse.aspx">Courses</a>
                         </li>
                          <li class="nav-item">
                             <a class="nav-link" href="RegisterLecturer.aspx">Register Lecturer</a>
                         </li>
                          <li class="nav-item">
                             <a class="nav-link" href="AssignLecturer.aspx">Assign Lecturer</a>
                         </li>
                         <li class="nav-item">
                             <a class="nav-link" href="#">Register Student</a>
                         </li>
                         <li class="nav-item">
                             <a class="nav-link" href="#">Enrolment</a>
                         </li>
                         <li class="nav-item">
                             <a class="nav-link" href="#">Announcements</a>
                         </li>
                         <li class="nav-item">
                             <a class="nav-link" href="#">Calendar</a>
                         </li>
                         <li class="nav-item">
                             <a class="nav-link" href="#">Attendance</a>
                         </li>
                         <li class="nav-item">
                             <a class="nav-link" href="#">Report</a>
                         </li>
                     </ul>

                     <!-- Logout button -->
                     <asp:Button ID="btnLogout" runat="server"
                         Text="Logout"
                         CssClass="btn btn-danger"
                         OnClick="btnLogout_Click"/>

                 </div>

             </div>
         </nav>

        <!-- Manage course form -->
        <div class="container mt-5 d-flex justify-content-center">
            <div class="card shadow p-4" style="width: 400px;">
                <h3 class="text-center mb-4">Manage Courses</h3>

                <div class="mb-3">
                    <label class="form-label">Course Code</label>
                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" placeholder="Enter course code"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label">Course Name</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter course name"></asp:TextBox>
                </div>

                <div class="mb-3">
                    <label class="form-label">Credit Hours</label>
                    <asp:TextBox ID="txtCredit" runat="server" CssClass="form-control" placeholder="Enter credit hours"></asp:TextBox>
                </div>

                <!-- Programme Dropdown -->
                 <div class="mb-3">
                    <label>Select Programme</label>
                    <asp:DropDownList ID="ddlProgramme" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>

                <div class="d-flex gap-2">
                    <!-- Save button -->
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save Course"
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

        <!-- Course List -->
        <div class="container mt-4">
            <div class="card shadow p-4">
                <h4 class="mb-3">Course List</h4>

                 <!-- Search -->
                <div class="row mb-3">
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchDept" runat="server"
                            CssClass="form-control"
                            Placeholder="Enter programme name" />
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
                <asp:GridView ID="gvCourse" runat="server"
                    CssClass="table table-bordered table-striped"
                    AutoGenerateColumns="False"
                    DataKeyNames="CourseID"
                    EmptyDataText="No courses found"
                    OnRowEditing="gvCourse_RowEditing"
                    OnRowUpdating="gvCourse_RowUpdating"
                    OnRowCancelingEdit="gvCourse_RowCancelingEdit">
                
                    <Columns>
                        <asp:TemplateField HeaderText="No.">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                        <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                        <asp:BoundField DataField="CreditHours" HeaderText="Credit Hours" />
                        <asp:BoundField DataField="ProgrammeName" HeaderText="Programme" />

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"
                                    CommandName="Edit"
                                    Text="Edit"
                                    CssClass="btn btn-primary btn-sm me-2" />
                            </ItemTemplate>

                            <EditItemTemplate>

                                <div class="d-flex gap-2">
                                    <asp:LinkButton ID="btnUpdate" runat="server"
                                        CommandName="Update"
                                        Text="Update"
                                        CssClass="btn btn-success btn-sm me-2" />

                                    <asp:LinkButton ID="btnCancel" runat="server"
                                        CommandName="Cancel"
                                        Text="Cancel"
                                        CssClass="btn btn-secondary btn-sm" />
                                </div>

                            </EditItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>
        </div>

    </form>
</body>
</html>