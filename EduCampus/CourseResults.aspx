<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseResults.aspx.cs" Inherits="EduCampus.CourseResults" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Course Results</title>

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

        <!-- Back button -->
        <div class="mt-3 px-4">
            <asp:Button ID="btnBack"
                runat="server"
                Text="← Back"
                CssClass="btn btn-secondary"
                OnClick="btnBack_Click" />
        </div>

        <!-- Content -->
        <div class="container mt-4 px-4 px-lg-5">

            <h3 class="text-center mb-2">Course Results</h3>

            <!-- Filter section: Session, Course, and Date selection -->
            <div class="w-75 mx-auto">

                <div class="mb-3">
                    <label class="form-label fw-semibold">Session</label>

                    <!-- Session dropdown -->
                    <asp:DropDownList ID="ddlSession"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <!-- Course dropdown -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">Course</label>
                     
                    <asp:DropDownList ID="ddlCourse"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <!-- Search Button -->
                <div class="text-center">
                    <asp:Button ID="btnSearch"
                        runat="server"
                        Text="Search"
                        CssClass="btn btn-primary mt-2"
                        OnClick="btnSearch_Click" />
                </div>

            </div>
            
            <asp:Panel ID="pnlSearchResult" runat="server" Visible="false">

                <!-- Results Summary -->
                <h5 class="mt-4 mb-3">Results Summary</h5>

                <div class="row mb-4 mx-10">

                    <!-- Average Mark -->
                    <div class="col-md-3">
                        <div class="card text-center border-primary">
                            <div class="card-body">

                                <h5>Average Mark</h5>
                                <asp:Label ID="lblAverageMark"
                                    runat="server"
                                    Font-Size="25px"
                                    Font-Bold="True">
                                </asp:Label>
    
                            </div>
                        </div>
                    </div>

                    <!-- Total Pass -->
                    <div class="col-md-3">
                        <div class="card text-center border-success">
                            <div class="card-body">

                                <h5 class="text-success">Pass</h5>
                                <asp:Label ID="lblPass"
                                    runat="server"
                                    Font-Size="25px"
                                    Font-Bold="True">
                                </asp:Label>

                            </div>
                        </div>
                    </div>

                    <!-- Total Fail -->
                    <div class="col-md-3">
                        <div class="card text-center border-danger">
                            <div class="card-body">

                                <h5 class="text-danger">Fail</h5>
                                <asp:Label ID="lblFail"
                                    runat="server"
                                    Font-Size="25px"
                                    Font-Bold="True">
                                </asp:Label>

                            </div>
                        </div>
                    </div>

                    <!-- Passing Rate -->
                    <div class="col-md-3">
                        <div class="card text-center border-primary">
                            <div class="card-body">

                                <h5>Passing Rate</h5>
                                <asp:Label ID="lblPassingRate"
                                    runat="server"
                                    Font-Size="25px"
                                    Font-Bold="True">
                                </asp:Label>

                            </div>
                        </div>
                    </div>

                </div>
                <!-- Student Results -->
                <h5 class="mt-5 mb-3">Student Results</h5>

                <!-- Student Result Records Grid -->
                <asp:GridView ID="gvResult"
                    runat="server"
                    CssClass="table table-bordered table-striped"
                    AutoGenerateColumns="False"
                    EmptyDataText="No student academic results found">

                    <Columns>

                        <asp:BoundField DataField="Student Name" HeaderText="Student Name" />
                        <asp:BoundField DataField="Student ID" HeaderText="Student ID" />
                        <asp:BoundField DataField="Assignment" HeaderText="Assignment" />
                        <asp:BoundField DataField="Quiz" HeaderText="Quiz" />
                        <asp:BoundField DataField="Mid Test" HeaderText="Mid Test" />
                        <asp:BoundField DataField="Final Exam" HeaderText="Final Exam" />
                        <asp:BoundField DataField="Final Mark" HeaderText="Final Mark" />
                        <asp:BoundField DataField="Grade" HeaderText="Grade" />

                    </Columns>

                </asp:GridView>

                <!-- Download PDF button-->
                <div class="text-center mt-4 mb-4">
                    <asp:Button ID="btnPDF" runat="server"
                        Text="Download PDF"
                        CssClass="btn btn-danger me-4"
                        OnClick="btnPDF_Click" />
                </div>

            </asp:Panel>

        </div>

    </form>
</body>
</html>
