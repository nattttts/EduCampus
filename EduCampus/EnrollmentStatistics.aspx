<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnrollmentStatistics.aspx.cs" Inherits="EduCampus.EnrollmentStatistics" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Enrollment Statistics</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <style>
        @media print {
        
            /* Hide buttons when printing PDF */
            .btn{
                display: none;
            }

            /* Set print page size to A4 format */
            @page {
                size: A4;
            }
        }
    </style>

</head>

<body>
    <form id="form1" runat="server">

        <div class="mt-3 px-4">
            <asp:Button ID="btnBack"
                runat="server"
                Text="← Back"
                CssClass="btn btn-secondary"
                OnClick="btnBack_Click" />
        </div>

        <h3 class="text-center mb-4">Enrollment Statistics</h3>

            
        <div class="d-flex justify-content-center align-items-end gap-3 mb-4">
            <!-- Session dropdown -->
            <label class="form-label fw-semibold">Session</label>

            <asp:DropDownList ID="ddlSession"
                runat="server"
                CssClass="form-select"
                Width="250px"
                AutoPostBack="True"
                OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
            </asp:DropDownList>
        </div>

        <!-- Enrollment statistics bar chart -->
        <div style="width: 600px; margin:auto;">
            <div class="card shadow-sm mb-4">
                <div class="card-body m-2">
                    <h5 class="card-title text-center mb-3">Course Enrollment Chart</h5>
                    <canvas id="enrollmentChart"></canvas>
                </div>
            </div>
        </div>

        <!-- Enrollment statistics table -->
        <div style="width: 700px; margin:auto;">

            <div class="card shadow-sm mb-4">
                <div class="card-body">
                    <h5 class="card-title text-center mb-3">Detailed Table</h5>
                    
                    <asp:GridView ID="gvStatistics"
                        runat="server"
                        CssClass="table table-bordered table-striped"
                        AutoGenerateColumns="False"
                        EmptyDataText="No records found. Please select the session.">

                        <Columns>
                            <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                            <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                            <asp:BoundField DataField="TotalStudents" HeaderText="Total Students" />
                        </Columns>

                    </asp:GridView>
                </div>
            </div>

        </div>

        <!-- Download PDF button-->
        <div class="text-center mb-4">
            <asp:Button ID="btnPDF" runat="server"
                Text="Download PDF"
                CssClass="btn btn-danger me-4"
                OnClientClick="window.print(); return false;" />
        </div>

        <!-- Chart.js to display course enrollment statistics -->
       <script>

        new Chart(document.getElementById('enrollmentChart'),
        {
            type: 'bar',

            data:
            {
                labels: [<%= CourseLabels %>],

                datasets: [{

                    label: 'Total Students', // Number of enrolled students

                    data: [<%= StudentCounts %>], // Student count per course

                    backgroundColor: 'rgba(255, 99, 132, 0.7)',

                    borderColor: 'rgba(255, 99, 132, 1)',

                    borderWidth: 2

                }]
            },

            options:
            {
                plugins:
                {
                    legend:
                    {
                        display: false
                    }
                }
            }

        });

       </script>

    </form>
</body>
</html>