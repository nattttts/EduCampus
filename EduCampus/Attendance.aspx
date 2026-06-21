<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Attendance.aspx.cs"
    Inherits="lecturer.Attendance" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Attendance</title>

    <style>
        body {
            background-color: #A4D8FF;
            font-family: Arial;
        }

        .container {
            width: 1000px;
            margin: 30px auto;
            background: white;
            padding: 20px;
            border-radius: 10px;
        }

        .filter-row {
            display: flex;
            gap: 15px;
            margin-bottom: 20px;
        }

        .button-row {
            margin-top: 20px;
            display: flex;
            justify-content: space-between;
        }

        .btn {
            width: 100px;
            height: 40px;
        }
    </style>
</head>

<body>

<form id="form1" runat="server">

<div class="container">

    <h2>Attendance Management</h2>

    <div class="filter-row">

        <asp:DropDownList
            ID="ddlSession"
            runat="server"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
        </asp:DropDownList>

        <asp:DropDownList
            ID="ddlCourse"
            runat="server">
        </asp:DropDownList>

        <asp:TextBox
            ID="txtAttendanceDate"
            runat="server"
            TextMode="Date">
        </asp:TextBox>

        <asp:Button
            ID="btnFilter"
            runat="server"
            Text="Filter"
            OnClick="btnFilter_Click" />

    </div>

    <asp:GridView
        ID="gvAttendance"
        runat="server"
        AutoGenerateColumns="False"
        Width="100%"
        BorderWidth="1">

        <Columns>

            <asp:BoundField
                DataField="AttendanceID"
                HeaderText="Attendance ID" />

            <asp:BoundField
                DataField="StudentID"
                HeaderText="Student ID" />

            <asp:BoundField
                DataField="StudentName"
                HeaderText="Student Name" />

            <asp:BoundField
                DataField="AttendanceDate"
                HeaderText="Date"
                DataFormatString="{0:dd/MM/yyyy}" />

            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>

                    <asp:DropDownList
                        ID="ddlStatus"
                        runat="server"
                        Enabled="false">

                        <asp:ListItem>Present</asp:ListItem>
                        <asp:ListItem>Absent</asp:ListItem>
                        <asp:ListItem>Late</asp:ListItem>

                    </asp:DropDownList>

                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Remarks">
                <ItemTemplate>

                    <asp:TextBox
                        ID="txtRemarks"
                        runat="server"
                        Enabled="false"
                        Text='<%# Eval("Remarks") %>'>
                    </asp:TextBox>

                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>

    <div class="button-row">

        <asp:Button
            ID="btnEdit"
            runat="server"
            Text="Edit"
            CssClass="btn"
            OnClick="btnEdit_Click" />

        <asp:Button
            ID="btnSave"
            runat="server"
            Text="Save"
            CssClass="btn"
            OnClick="btnSave_Click" />

    </div>

</div>

</form>

</body>
</html>