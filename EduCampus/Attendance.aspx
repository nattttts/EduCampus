<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance.aspx.cs" Inherits="EduCampus.Attendance" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Attendance</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        .present {
            background-color: #28a745;
            color: white;
            padding: 5px 10px;
            border-radius: 6px;
            display: inline-block;
        }

        .absent {
            background-color: #dc3545;
            color: white;
            padding: 5px 10px;
            border-radius: 6px;
            display: inline-block;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

<div class="container mt-4">

    <!-- FILTER -->
    <div class="d-flex justify-content-start mb-3">

    <asp:Button ID="btnBackDashboard"
        runat="server"
        Text="← Back to Dashboard"
        CssClass="btn btn-secondary"
        OnClick="btnBackDashboard_Click" />

    </div>
    <div class="row mb-3">
        <div class="col-md-4">

            <asp:DropDownList ID="ddlCourse"
                runat="server"
                CssClass="form-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
            </asp:DropDownList>

        </div>
    </div>

    <!-- TABLE -->
    <asp:GridView ID="gvAttendance" runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-bordered">

        <Columns>

            <asp:BoundField DataField="CourseName" HeaderText="Course" />
            <asp:BoundField DataField="AttendanceDate" HeaderText="Date" />

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

</div>

</form>
</body>
</html>