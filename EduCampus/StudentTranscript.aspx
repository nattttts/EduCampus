<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentTranscript.aspx.cs" Inherits="EduCampus.StudentTranscript" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Student Transcript</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="style.css" />
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

        <div class="container mt-4">
            <h3 class="text-center mb-4">Student Transcript</h3>

             <!-- Filter section: Student and Semester selection -->
            <div class="w-75 mx-auto">

                <!-- Student dropdown -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">Student</label>

                    <asp:DropDownList ID="ddlStudent"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlStudent_SelectedIndexChanged">
                    </asp:DropDownList>

                </div>

                <!-- Seemester Selection -->
                <div class="mb-3">
                    <label class="form-label fw-semibold">Semester</label>

                    <asp:DropDownList ID="ddlSemester"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="ddlSemester_SelectedIndexChanged">
                    </asp:DropDownList>

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

            <asp:Panel ID="pnlReport" runat="server" Visible="false">

                <h5 class="mt-4 mb-3">Grade Report</h5>

                <!-- Student information -->
                <div class="card mb-3">
                    <div class="card-body m-2">

                        <div class="row mb-2">

                            <div class="col-md-7">
                                <strong>Student Name:</strong>
                                <asp:Label ID="lblStudentName" runat="server"></asp:Label>
                            </div>

                            <div class="col-md-5">
                                <strong>Session:</strong>
                                <asp:Label ID="lblSession" runat="server"></asp:Label>
                            </div>

                        </div>

                        <div class="row mb-2">

                            <div class="col-md-7">
                                <strong>Student ID:</strong>
                                <asp:Label ID="lblStudentID" runat="server"></asp:Label>
                            </div>

                            <div class="col-md-5">
                                <strong>Semester:</strong>
                                <asp:Label ID="lblSemester" runat="server"></asp:Label>
                            </div>

                        </div>

                        <div class="row mb-4">

                            <div class="col-md-7">
                                <strong>Programme:</strong>
                                <asp:Label ID="lblProgramme" runat="server"></asp:Label>
                            </div>

                            <div class="col-md-5">
                                <strong>Date:</strong>
                                <asp:Label ID="lblDate" runat="server"></asp:Label>
                            </div>

                        </div>

                        <!-- Result details table -->
                        <asp:GridView ID="gvGradeReport"
                            runat="server"
                            CssClass="table table-bordered"
                            HeaderStyle-CssClass="table-light"
                            AutoGenerateColumns="False">

                            <Columns>

                                <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                                <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                                <asp:BoundField DataField="CreditHours" HeaderText="Credit Hours" />
                                <asp:BoundField DataField="FinalMark" HeaderText="Final Mark" />
                                <asp:BoundField DataField="FinalGrade" HeaderText="Grade" />
                                <asp:BoundField DataField="GradePoint" HeaderText="Grade Point" />
                                <asp:BoundField DataField="CreditPoint" HeaderText="Credit Point" />

                            </Columns>

                        </asp:GridView>

                        <!-- GPA and CGPA -->
                        <div class="row mt-3">
                            <div class="col-auto">
                                <strong>GPA:</strong>
                                <asp:Label ID="lblGPA" runat="server" Font-Bold="True"></asp:Label>
                            </div>

                            <div class="col-auto ms-4">
                                <strong>CGPA:</strong>
                                <asp:Label ID="lblCGPA" runat="server" Font-Bold="True"></asp:Label>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="text-center mt-4 mb-4">
                    <asp:Button ID="btnPDF"
                        runat="server"
                        Text="Download PDF"
                        CssClass="btn btn-danger"
                        OnClick="btnPDF_Click" />
                </div>

            </asp:Panel>

        </div>
    </form>
</body>
</html>
