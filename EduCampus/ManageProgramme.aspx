<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageProgramme.aspx.cs" Inherits="EduCampus.ManageProgramme" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Programme</title>

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
                            <a class="nav-link active" href="ManageProgramme.aspx">Programme</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">Courses</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="RegisterLecturer.aspx">Register Lecturer</a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="#">Assign Lecturer</a>
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
                        OnClick="btnLogout_Click" />

                </div>

            </div>
        </nav>

        <!-- Manage programme form -->
        <div class="container mt-4">
            <h3 class="text-center">Manage Programme</h3>

            <div class="mb-3">
                <label>Programme Code</label>
                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" placeholder="Enter Programme Code"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label>Programme Name</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter Programme Name"></asp:TextBox>
            </div>

            <!-- Save button -->
            <asp:Button ID="btnSave" runat="server"
                Text="Save Programme"
                CssClass="btn btn-primary"
                OnClick="btnSave_Click" />

            <!-- Clear button -->
            <asp:Button ID="btnClear" runat="server"
                Text="Clear"
                CssClass="btn btn-secondary"
                OnClick="btnClear_Click"/>

            <asp:Label ID="lblMsg" runat="server"></asp:Label>

            <hr />

            <!-- Programme List -->
            <h4>Programme List</h4>

            <asp:GridView ID="gvProgramme" runat="server"
                CssClass="table table-bordered"
                HeaderStyle-CssClass="table-dark"
                AutoGenerateColumns="False"
                DataKeyNames="ProgrammeID"
                OnRowEditing="gvProgramme_RowEditing"
                OnRowUpdating="gvProgramme_RowUpdating"
                OnRowCancelingEdit="gvProgramme_RowCancelingEdit">

                <Columns>

                    <asp:BoundField DataField="ProgrammeID" HeaderText="No." ReadOnly="True" />
                    <asp:BoundField DataField="ProgrammeCode" HeaderText="Programme Code" />
                    <asp:BoundField DataField="ProgrammeName" HeaderText="Programme Name" />

                    <asp:TemplateField HeaderText="Action">

                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server"
                                CommandName="Edit"
                                Text="Edit"
                                CssClass="btn btn-primary btn-sm me-2" />
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
    </form>
</body>
</html>