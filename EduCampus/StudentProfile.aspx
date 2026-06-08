<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentProfile.aspx.cs" Inherits="EduCampus.StudentProfile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Profile</title>

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="style.css" rel="stylesheet" />
</head>

<body>

<form id="form1" runat="server">


<div class="container mt-5">

    <div class="row justify-content-center">

        <div class="col-md-6">

            <div class="card shadow">

                <div class="card-header bg-dark text-white text-center">
                    <h3>Student Profile</h3>
                </div>

                <div class="card-body">

                    <div class="mb-3">
                        <label class="fw-bold">Student ID</label>
                        <asp:TextBox ID="txtStudentID"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="fw-bold">Name</label>
                        <asp:TextBox ID="txtName"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="fw-bold">Email</label>
                        <asp:TextBox ID="txtEmail"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label class="fw-bold">Programme</label>
                        <asp:TextBox ID="txtProgramme"
                            runat="server"
                            CssClass="form-control"
                            ReadOnly="true">
                        </asp:TextBox>
                    </div>

                    <div class="text-center mt-4">
                        <asp:Button ID="btnBackDashboard"
                            runat="server"
                            Text="Back to Dashboard"
                            CssClass="btn btn-primary"
                            OnClick="btnBackDashboard_Click" />
                    </div>

                </div>

            </div>

        </div>

    </div>

</div>

</form>

</body>
</html>