<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="fasaidetail.aspx.cs" Inherits="fasaidetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <style>
            .red {
                color: red;
                font-size: 1.5em;
                padding-left: 4px;
                font-weight: bold;
            }
        </style>
        <section class="section">
            <ul class="breadcrumb breadcrumb-style ">
            </ul>
            <div class="row">
                <div class="col-12 col-sm-12 col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>FSSAI Detail</h5>
                        </div>
                         <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <!-- Genex Business -->
                                    <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                                        <div class="col-md-8">
                                            <div class="profile-bar-simple red-border clearfix">
                                            </div>
                                            <div class="form-group">
                                                <label for="pwd">
                                                    FSSAI No.:</label>
                                                <asp:TextBox ID="txtpan" runat="server" CssClass="form-control validate[required]" MaxLength="14"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="email">
                                                    Pdf Upload:</label>
                                                <asp:FileUpload ID="Fuidentity" runat="server"
                                                    CssClass="form-control"
                                                    onchange="return validatePdfUpload(this);" />
                                                <asp:Label ID="lblimage" runat="server" Visible="false"></asp:Label>
                                            </div>

                                            <asp:Button ID="BtnIdentity" runat="server" ValidationGroup="eInformation" CssClass="btn btn-primary"
                                                Text="submit" OnClientClick="return Page_ClientValidate('eInformation') && SaveButton();" OnClick="BtnIdentity_Click" />
                                        </div>
                                    </div>
                                    <!-- end dashboards/dashboard -->
                                </div>
                                <div class="col-md-4">
                                    <!-- Genex Business -->
                                    <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                                        <%--    <div class="profile-bar-simple red-border clearfix">
                                            <h6>Uploaded Images
                                            </h6>
                                        </div>--%>
                                        <div class="col-md-12">
                                            <script src="popupassets/popper.min.js"></script>

                                            <script src="popupassets/lib.js"></script>

                                            <script src="popupassets/jquery.flagstrap.min.js"></script>

                                            <script type="text/javascript" src="popupassets/jquery.themepunch.tools.min.js"></script>

                                            <script type="text/javascript" src="popupassets/jquery.themepunch.revolution.min.js"></script>

                                            <script src="js/functions1.js"></script>
                                            <div class="col-md-12" style="display: none">
                                                Front Address Proof<br />
                                                <a id="FrontAddress" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="ShowIdentity" runat="server" Width="150px" Height="150px" />
                                                </a>
                                                <a id="PanCard" runat="server" class="fbox" rel="group">
                                                    <asp:Image ID="Image1" Width="130px" Height="130px" runat="server" Style="margin-left: 30%" />
                                                </a>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div id="DivVerify" runat="server">
                                                <br />
                                                <asp:Label ID="LblVerification" Text="Verification Status :  " Font-Bold="true" runat="server"></asp:Label>
                                                <asp:Label ID="lblverstatus" runat="server"></asp:Label>
                                                <br />
                                                <asp:Label ID="VerifyDate" runat="server" Text="Verify/Reject Date : " Visible="false"
                                                    Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="Lblverdate" runat="server"></asp:Label>
                                                <br />
                                                <asp:Label ID="LblVerfRemark" Text="Reject Remark : " Visible="false" runat="server"
                                                    Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="LblRemark" runat="server"></asp:Label>
                                                <br />
                                                <asp:Label ID="LblVerfReason" Text="Reject Reason : " Visible="false" runat="server"
                                                    Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="LbLrejectRemark" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <script type="text/javascript">
        function validatePdfUpload(input) {

            if (input.files.length === 0)
                return true;

            var file = input.files[0];
            var maxSize = 2 * 1024 * 1024; // 2 MB

            // File type check
            if (file.type !== "application/pdf") {
                alert("Only PDF files are allowed.");
                input.value = "";
                return false;
            }

            // File size check
            if (file.size > maxSize) {
                alert("PDF size must not exceed 2 MB.");
                input.value = "";
                return false;
            }

            return true;
        }
    </script>
</asp:Content>

