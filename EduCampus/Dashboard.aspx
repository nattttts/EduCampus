<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs"
    Inherits="lecturer.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization"
    Namespace="System.Web.UI.DataVisualization.Charting"
    TagPrefix="asp" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Dashboard</title>

    <style>
        body {
            background-color: #A4D8FF;
            font-family: Arial;
        }

        .container {
            width: 1100px;
            margin: 30px auto;
            background: white;
            padding: 25px;
            border-radius: 10px;
        }

        .filter-row {
            display: flex;
            gap: 15px;
            margin-bottom: 25px;
        }

        .section {
            margin-top: 30px;
        }

        .grid {
            width: 100%;
        }

        h2, h3 {
            text-align: center;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

<div class="container">

    <h2>Dashboard</h2>

    <div class="filter-row">

        <asp:DropDownList
            ID="ddlCourse"
            runat="server"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
        </asp:DropDownList>

    </div>

    <div class="section">

        <h3>Poor Attendance Students</h3>

        <asp:GridView
            ID="gvPoorAttendance"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="grid">

            <Columns>

                <asp:BoundField
                    DataField="StudentID"
                    HeaderText="Student ID" />

                <asp:BoundField
                    DataField="StudentName"
                    HeaderText="Student Name" />

                <asp:BoundField
                    DataField="TotalClasses"
                    HeaderText="Total Classes" />

                <asp:BoundField
                    DataField="PresentCount"
                    HeaderText="Present" />

                <asp:BoundField
                    DataField="AttendancePercentage"
                    HeaderText="Attendance %"
                    DataFormatString="{0:N2}%" />

            </Columns>

        </asp:GridView>

    </div>

    <div class="section">

        <h3>Grade Distribution</h3>

        <asp:Chart
            ID="chartGrades"
            runat="server"
            Width="900px"
            Height="400px">

            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                    <AxisX Title="Grade"></AxisX>
                    <AxisY Title="Number of Students"></AxisY>
                </asp:ChartArea>
            </ChartAreas>

            <Series>
                <asp:Series
                    Name="Grades"
                    ChartType="Column"
                    XValueMember="FinalGrade"
                    YValueMembers="StudentCount">
                </asp:Series>
            </Series>

            <Titles>
                <asp:Title Text="Grade Distribution"></asp:Title>
            </Titles>

        </asp:Chart>

    </div>

</div>

</form>
</body>
</html>