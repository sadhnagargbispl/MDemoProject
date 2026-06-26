<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reply.aspx.cs" Inherits="Reply" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Session["Title"].ToString ()%></title>
    <link rel="stylesheet" href="assets/css/app.min.css">
    <link rel="stylesheet" href="assets/css/style.css">
    <link rel="stylesheet" href="assets/css/components.css">
    <link rel="stylesheet" href="assets/bundles/jqvmap/dist/jqvmap.min.css">
    <link rel="stylesheet" href="assets/css/custom.css">
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="row">
                <div class="col-12 col-sm-12 col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Reply</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            Complaint type<span class="red">*</span></label>
                                        <asp:Label class="form-control" ID="LblCType" runat="server"></asp:Label>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            Complaint<span class="red">*</span>
                                        </label>

                                        <asp:TextBox class="form-control" ID="TxtComplaint" ReadOnly="true" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            Previous Reply<span class="red">*</span></label>
                                        <asp:TextBox class="form-control" ID="TxtPreReply" ReadOnly="true" TextMode="MultiLine" runat="server"></asp:TextBox>


                                    </div>

                                </div>

                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </div>


    </form>
</body>
</html>
