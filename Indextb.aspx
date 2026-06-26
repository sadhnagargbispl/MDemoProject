<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="Indextb.aspx.cs" Inherits="Indextb" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .upload-box {
            width: 100%;
            height: 180px;
            border: 2px dashed #bbb;
            border-radius: 10px;
            text-align: center;
            padding-top: 35px;
            cursor: pointer;
            background-color: #fafafa;
        }

            .upload-box:hover {
                background-color: #f0f0f0;
            }
        /* Only Desktop (>= 992px) */
        @media (min-width: 992px) {
            .ref-card,
            .news-card,
            .ref-card .card-body,
            .news-card .card-body {
                height: 200px !important; /* fix height */
                overflow: hidden !important; /* extra content hide */
            }

            /* rows ko bhi 50px me adjust karna */
            .ref-row {
                height: 50px !important;
                align-items: center;
            }
        }

        /* Mobile ke liye kuch change nahi — already sahi hai */
    </style>
    <script type="text/javascript">
        function copyLinkClient() {

            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_lblLink");
            if (document.body.createTextRange) {
                range = document.body.createTextRange();
                range.moveToElementText(element);
                range.select();
            } else if (window.getSelection) {
                selection = window.getSelection();
                range = document.createRange();
                range.selectNodeContents(element);
                selection.removeAllRanges();
                selection.addRange(range);
            }

            try {
                document.execCommand('copy');
                alert('link copied');
            }
            catch (err) {
                alert('unable to copy link');
            }
            return false;
        }
        function copyLinkClientgreen() {

            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_lblLinkgreen");
            if (document.body.createTextRange) {
                range = document.body.createTextRange();
                range.moveToElementText(element);
                range.select();
            } else if (window.getSelection) {
                selection = window.getSelection();
                range = document.createRange();
                range.selectNodeContents(element);
                selection.removeAllRanges();
                selection.addRange(range);
            }

            try {
                document.execCommand('copy');
                alert('link copied');
            }
            catch (err) {
                alert('unable to copy link');
            }
            return false;
        }
    </script>
    <style>
        .badge-verified {
            background: #28a745;
            color: #fff;
        }

        .badge-pending {
            background: linear-gradient(135deg, #ff2d55, #ff5f7e);
            color: #fff;
        }

        .badge-notupload {
            background: #ff4d2d;
            color: #fff;
        }

        .badge-rejected {
            background: #6c757d;
            color: #fff;
        }

        /* Mobile text adjust */
        @media (max-width: 576px) {
            .kyc-title {
                font-size: 12px;
            }
        }
    </style>
    <script type="text/javascript">
        function copyLinkClient1() {
            debugger;
            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_lbllink1");
            if (document.body.createTextRange) {
                range = document.body.createTextRange();
                range.moveToElementText(element);
                range.select();
            } else if (window.getSelection) {
                selection = window.getSelection();
                range = document.createRange();
                range.selectNodeContents(element);
                selection.removeAllRanges();
                selection.addRange(range);
            }

            try {
                document.execCommand('copy');
                alert('link copied');
            }
            catch (err) {
                alert('unable to copy link');
            }
            return false;
        }
        function copyLinkClientUpdate() {
            debugger;
            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_lblLinkupdate");
            if (document.body.createTextRange) {
                range = document.body.createTextRange();
                range.moveToElementText(element);
                range.select();
            } else if (window.getSelection) {
                selection = window.getSelection();
                range = document.createRange();
                range.selectNodeContents(element);
                selection.removeAllRanges();
                selection.addRange(range);
            }

            try {
                document.execCommand('copy');
                alert('link copied');
            }
            catch (err) {
                alert('unable to copy link');
            }
            return false;
        }
    </script>
    <div class="main-content">


        <!-- Welcome + Profile Card -->
        <div class="row mb-4">
            <div runat="server" id="divProfile">
                <div class="card member-card">
                    <div class="card-body">

                        <!-- PROFILE IMAGE -->
                        <%--   <div class="profile-wrap">
                       
                            <asp:Image ID="Image2" runat="server" CssClass="profile-img" AlternateText="" />
                 
                        </div>--%>
                        <!-- MAIN PROFILE PHOTO -->
                        <div class="profile-wrap">
                            <asp:Image ID="Image2" runat="server" CssClass="profile-img"
                                Style="width: 130px; height: 130px; border-radius: 50%; cursor: pointer;"
                                onclick="openPhotoModal()" />
                        </div>

                        <!-- MODAL -->
                        <div class="modal fade" id="profilePhotoModal" tabindex="-1">
                            <div class="modal-dialog modal-lg">
                                <div class="modal-content">

                                    <div class="modal-header bg-light">
                                        <h4 class="modal-title">Upload Profile Photo</h4>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                    </div>

                                    <div class="modal-body">

                                        <div class="row">

                                            <!-- LEFT PREVIEW -->
                                            <div class="col-md-4 text-center">
                                                <img id="photoPreview" runat="server"
                                                    src="https://www.iconpacks.net/icons/2/free-user-icon-3296-thumb.png"
                                                    style="width: 150px; height: 150px; border-radius: 10px; border: 1px solid #ccc;" />
                                            </div>

                                            <!-- RIGHT UPLOAD BOX -->
                                            <div class="col-md-8">
                                                <label class="form-label fw-bold">Upload new photo</label>

                                                <div class="upload-box" onclick="openFilePicker()">
                                                    <i class="fa fa-image" style="font-size: 40px; color: #999;"></i>
                                                    <p><b>Upload</b> or drop your file here</p>
                                                </div>

                                                <asp:FileUpload ID="FileUpload1" runat="server" accept="image/*"
                                                    Style="display: none;" onchange="previewPhoto(this)" />

                                                <div class="mt-3" style="font-size: 13px; color: #555;">
                                                    Accepted file types: .jpg, .jpeg, .png  
                            <br />
                                                    Best result: 200x200 pixel
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="modal-footer">
                                        <button class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                        <asp:Button ID="btnSave" runat="server" Text="Save"
                                            CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                    </div>

                                </div>
                            </div>
                        </div>

                        <!-- DETAILS -->
                        <div class="member-text">

                            <h5 class="member-title mb-1">
                                <span class="label">Member Name :</span>
                                <span class="value">
                                    <asp:Label ID="LblMemName" runat="server"></asp:Label>
                                </span>
                            </h5>

                            <p class="member-sub mb-0">
                                <span><strong>Member ID :</strong>
                                    <span class="value">
                                        <asp:Label ID="LblMemId" runat="server"></asp:Label></span>
                                </span>
                            </p>
                            <%-- <span class="dot">•</span>--%>
                            <p class="member-sub mb-0">
                                <span><strong>Rank : 
                                </strong>
                                    <span class="rank-badge">
                                        <asp:Label ID="lblRankTitle" runat="server"></asp:Label>
                                    </span>
                                </span>
                            </p>

                        </div>

                    </div>
                </div>
            </div>
            <div class="col-lg-6" id="Div1" runat="server" visible="false">
                <div class="card member-card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-link me-2"></i>My Referral Link</h5>
                    </div>

                    <div class="card-body">

                        <!-- responsive row -->
                        <div class="ref-row" id="refRow">

                            <!-- LABEL -->
                            <div class="ref-label" id="Div2" runat="server">
                                <% 
                                    Response.Write("Referral Link");
                                %>
                            </div>

                            <!-- LINK -->
                            <div class="ref-link">
                                <a id="aRfLinkupdate" runat="server" target="_blank">
                                    <asp:Label ID="lblLinkupdate" runat="server" CssClass="d-inline-block" />
                                </a>
                            </div>

                            <!-- ACTION -->
                            <div class="ref-action">
                                <button type="button" class="copy-btn" onclick="copyLinkClientUpdate()" id="btnCopyTop">
                                    <i class="fa fa-copy me-1"></i>Copy
                                </button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <% if (Session["Compid"] != null && Session["Compid"].ToString() == "1091")
                { %>

            <div class="col-md-12" id="kycstatus" runat="server" visible="false">
                <div class="row tab-box" style="margin: 17px 0;">
                    <div class="col-lg-6 col-sm-12 tab-bg">
                        <span class="tab-lable">Kyc Status</span>
                    </div>
                    <div class="col-lg-6 col-sm-12" style="font-weight: bold;">
                        <span class="tab-field" id="status" runat="server"></span>
                    </div>
                </div>
            </div>

            <% } %>
        </div>

        <div class="row ">
            <div class="col-xl-3 col-lg-6">
                <div class="card l-bg-style1">
                    <div class="card-statistic-3">
                        <div class="card-icon card-icon-large"><i class="fa fa-award"></i></div>
                        <div class="card-content">
                            <h4 class="card-title">Total Income</h4>

                            <!-- INCOME LABEL -->
                            <span></span>
                            <div class="progress1 mt-1 mb-1" data-height="8">
                                <%-- <div class="progress-bar l-bg-green" role="progressbar" data-width="25%" aria-valuenow="25"
        aria-valuemin="0" aria-valuemax="100">
    </div>--%>
                            </div>
                            <p class="mb-0 text-sm">
                                <span class="mr-2">
                                    <asp:Label ID="LblMyIncome" runat="server" CssClass="error" /></span>
                                <span class="text-nowrap"></span>
                            </p>
                            <!-- PROGRESS BAR SECTION -->
                            <%--  <div class="progress mt-2 mb-2" style="height: 8px;">
                                <div id="incomeProgressBar" runat="server"
                                    class="progress-bar l-bg-purple"
                                    role="progressbar"
                                    style="width: 0%;"
                                    aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                </div>
                            </div>

                            <p class="mb-0 text-sm">
                                <span id="incomePercent" runat="server" class="mr-2 text-success">
                                    <i class="fa fa-arrow-up"></i>0%
                                </span>
                            </p>--%>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-xl-3 col-lg-6">
                <div class="card l-bg-style2">
                    <div class="card-statistic-3">
                        <div class="card-icon card-icon-large"><i class="fa fa-briefcase"></i></div>
                        <div class="card-content">
                            <h4 class="card-title">My Directs</h4>
                            <span></span>
                            <div class="progress1 mt-1 mb-1" data-height="8">
                                <%-- <div class="progress-bar l-bg-green" role="progressbar" data-width="25%" aria-valuenow="25"
        aria-valuemin="0" aria-valuemax="100">
    </div>--%>
                            </div>
                            <p class="mb-0 text-sm">
                                <span class="mr-2">
                                    <asp:Label ID="LblMyDirects" runat="server" CssClass="error" /></span>
                                <span class="text-nowrap"></span>
                            </p>
                            <%-- <div class="progress mt-1 mb-1" data-height="8">
                                <div class="progress-bar l-bg-orange" role="progressbar" data-width="25%" aria-valuenow="25"
                                    aria-valuemin="0" aria-valuemax="100">
                                </div>
                            </div>
                            <p class="mb-0 text-sm">
                                <span class="mr-2"><i class="fa fa-arrow-up"></i>10%</span>
                                <span class="text-nowrap"></span>
                            </p>--%>
                        </div>
                    </div>
                </div>
            </div>


            <div class="col-xl-3 col-lg-6">
                <div class="card l-bg-style3">
                    <div class="card-statistic-3">
                        <div class="card-icon card-icon-large"><i class="fa fa-globe"></i></div>
                        <div class="card-content">
                            <h4 class="card-title">My Team</h4>
                            <span></span>
                            <div class="progress1 mt-1 mb-1" data-height="8">
                                <%-- <div class="progress-bar l-bg-green" role="progressbar" data-width="25%" aria-valuenow="25"
        aria-valuemin="0" aria-valuemax="100">
    </div>--%>
                            </div>
                            <p class="mb-0 text-sm">
                                <span class="mr-2">
                                    <asp:Label ID="LblMyTeam" runat="server" CssClass="error" /></span>
                                <span class="text-nowrap"></span>
                            </p>
                            <%--   <div class="progress mt-1 mb-1" data-height="8">
                                <div class="progress-bar l-bg-cyan" role="progressbar" data-width="25%" aria-valuenow="25"
                                    aria-valuemin="0" aria-valuemax="100">
                                </div>
                            </div>
                            <p class="mb-0 text-sm">
                                <span class="mr-2"><i class="fa fa-arrow-up"></i>10%</span>
                                <span class="text-nowrap"></span>
                            </p>--%>
                        </div>
                    </div>
                </div>
            </div>


            <div class="col-xl-3 col-lg-6" id="divTotalBusiness" runat="server">
                <div class="card l-bg-style4">
                    <div class="card-statistic-3">
                        <div class="card-icon card-icon-large"><i class="fa fa-money-bill-alt"></i></div>
                        <div class="card-content">
                            <h4 class="card-title">Total Business</h4>
                            <span></span>
                            <div class="progress1 mt-1 mb-1" data-height="8">
                                <%-- <div class="progress-bar l-bg-green" role="progressbar" data-width="25%" aria-valuenow="25"
                                    aria-valuemin="0" aria-valuemax="100">
                                </div>--%>
                            </div>
                            <p class="mb-0 text-sm">
                                <span class="mr-2">
                                    <asp:Label ID="LblTotalBusiness" runat="server" CssClass="error" /></span>
                                <span class="text-nowrap"></span>
                            </p>
                        </div>
                    </div>
                </div>
            </div>







        </div>

        <!-- Income Summary (repMyIncome rendered as table) -->


        <div class="row g-4">
            <div class="col-lg-6" id="DivIncomeSummary" runat="server">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Income Summary</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-hover table-bordered mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th>Income Type</th>
                                        <th class="text-end">Amount</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="repMyIncome" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><strong><%# Eval("Name") %></strong></td>
                                                <td class="text-end fw-bold "><%# Eval("Amount") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Wallet Summary -->
            <div class="col-lg-6" id="DivWalletSummary" runat="server">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-wallet me-2"></i>Wallet Summary</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-hover mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th>Wallet</th>
                                        <th>Credit</th>
                                        <th>Debit</th>
                                        <th>Balance</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="gvBalance" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><strong><%# Eval("WalletName") %></strong></td>
                                                <td><%# Eval("Credit") %></td>
                                                <td><%# Eval("Debit") %></td>
                                                <td class="fw-bold"><%# Eval("Balnace") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <style>
                    .status-count-box {
                        border-radius: 12px;
                        padding: 16px 20px;
                        text-align: center;
                    }

                    .status-count-label {
                        display: inline-flex;
                        align-items: center;
                        justify-content: center;
                        gap: 6px;
                        font-size: 12px;
                        font-weight: 600;
                        letter-spacing: 0.3px;
                        margin-bottom: 6px;
                    }

                        .status-count-label i {
                            font-size: 14px;
                        }

                    .status-count-num {
                        font-size: 13px;
                        font-weight: 700;
                        margin: 0;
                        line-height: 1;
                    }

                    .sc-approved {
                        background: #EAF3DE;
                    }

                        .sc-approved .status-count-label {
                            color: #27500A;
                        }

                        .sc-approved .status-count-num {
                            color: #173404;
                        }

                    .sc-pending {
                        background: #FAEEDA;
                    }

                        .sc-pending .status-count-label {
                            color: #633806;
                        }

                        .sc-pending .status-count-num {
                            color: #412402;
                        }

                    .sc-rejected {
                        background: #FCEBEB;
                    }

                        .sc-rejected .status-count-label {
                            color: #791F1F;
                        }

                        .sc-rejected .status-count-num {
                            color: #501313;
                        }
                </style>

                <div class="card kyc-card p-3" runat="server" visible="false" id="divwithdrawal">
                    <h5 class="mb-3">Withdrawal Status</h5>
                    <div class="row text-center">
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                                <div class="col-md-4 col-6 kyc-box mb-2">
                                    <%# Eval("StatusHtml") %>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>

            <!-- Referral Link Card -->
            <div class="col-lg-8" id="Div13" runat="server" visible="false">
                <div class="card ref-card h-100">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-link me-2"></i>My Referral Link</h5>
                    </div>

                    <div class="card-body">

                        <!-- responsive row -->
                        <div class="ref-row" id="refRow">

                            <!-- LABEL -->
                            <div class="ref-label" id="tdReferalHead" runat="server">
                                <% 
                                    string cid = Session["Compid"]?.ToString();
                                    if (cid == "1101" || cid == "1106" || cid == "1109")
                                    { Response.Write("Referral Link"); }
                                    else { Response.Write("LEFT"); }
                                %>
                            </div>

                            <!-- LINK -->
                            <div class="ref-link">
                                <a id="aRfLink" runat="server" target="_blank">
                                    <asp:Label ID="lblLink" runat="server" CssClass="d-inline-block" />
                                </a>
                            </div>

                            <!-- ACTION -->
                            <div class="ref-action">
                                <button type="button" class="copy-btn" onclick="copyLinkClient()" id="btnCopyTop">
                                    <i class="fa fa-copy me-1"></i>Copy
                                </button>
                            </div>
                        </div>

                        <!-- Optional: second row (RIGHT/EVEN) - kept responsive -->
                        <div class="ref-row" style="margin-top: 12px;" id="trReferalHead" runat="server">
                            <div class="ref-label">
                                <% 
                                    string cid = Session["Compid"].ToString();
                                    if (cid == "1101")
                                    { /* blank as original */ }
                                    else { Response.Write("RIGHT"); }
                                %>
                            </div>

                            <div class="ref-link">
                                <% if (!(cid == "1066" || cid == "1067" || cid == "1072" || cid == "1073" ||
                                                                                                                                                                                                                       cid == "1077" || cid == "1078" || cid == "1079" || cid == "1093" || cid == "1101"))
                                    { %>
                                <a id="aRfLink1" runat="server" target="_blank">
                                    <asp:Label ID="lbllink1" runat="server" Style="color: Blue;" />
                                </a>
                                <% } %>
                            </div>

                            <div class="ref-action">
                                <% if (!(cid == "1066" || cid == "1067" || cid == "1072" || cid == "1073" ||
                                                                                                                                                                                                                       cid == "1077" || cid == "1078" || cid == "1079" || cid == "1093" || cid == "1101"))
                                    { %>
                                <button type="button" class="copy-btn" onclick="copyLinkClient1()" id="btnCopy2">
                                    <i class="fa fa-copy me-1"></i>Copy
                                </button>
                                <% } %>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div class="col-lg-8" id="Div4" runat="server" visible="false">
                <div class="card ref-card h-100">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-link me-2"></i>My Referral Link</h5>
                    </div>

                    <div class="card-body">

                        <!-- responsive row -->
                        <div class="ref-row" id="refRow">

                            <!-- LABEL -->
                            <div class="ref-label" id="Div5" runat="server">
                                <% 
                                    Response.Write("Referral Link"); 
                                %>
                            </div>

                            <!-- LINK -->
                            <div class="ref-link">
                                <a id="aRfLinkgreen" runat="server" target="_blank">
                                    <asp:Label ID="lblLinkgreen" runat="server" CssClass="d-inline-block" />
                                </a>
                            </div>

                            <!-- ACTION -->
                            <div class="ref-action">
                                <button type="button" class="copy-btn" onclick="copyLinkClientgreen()" id="btnCopyTopgreen">
                                    <i class="fa fa-copy me-1"></i>Copy
                                </button>
                            </div>
                        </div>

                        <!-- Optional: second row (RIGHT/EVEN) - kept responsive -->
                        

                    </div>
                </div>
            </div>
            <div class="col-lg-6" id="DivMyKYCStatus" runat="server" visible="false">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-bullhorn me-2"></i>My KYC Status</h5>
                    </div>
                    <%--<div class="card-header p-1">
                        <h4 class="card-title float-left">My KYC Status</h4>
                    </div>--%>
                    <div class="card-content collapse show">
                        <div class="card-footer text-center p-1">
                            <div class="row">
                                <asp:Repeater ID="RptKYCStatus" runat="server">
                                    <ItemTemplate>
                                        <div class="col-md-4 kyc-box text-center">
                                            <p class="kyc-title mb-1"><%# Eval("DocName") %></p>
                                            <div>
                                                <%# Eval("StatusHtml") %>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </div>
                            <hr>
                        </div>
                    </div>
                </div>
            </div>
            <!-- News Card -->
            <div runat="server" id="DivNews">
                <div class="card news-card h-100">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-bullhorn me-2"></i>Latest Updates</h5>
                    </div>
                    <div class="card-body news-marquee p-0">
                        <div class="p-3">
                            <asp:Repeater ID="RptNews" runat="server">
                                <ItemTemplate>
                                    <div class="border-bottom pb-3 mb-3">
                                        <p class="mb-1 small"><%# Eval("NewsDetail") %></p>
                                        <small class="text-muted"><i class="fa fa-calendar"></i><%# Eval("NewsDate") %></small>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>



            <!-- Team Summary -->
            <div class="col-lg-6" id="Div15" runat="server">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-users-cog me-2"></i>My Team Summary</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table id="table" class="table table-hover table-mc-light-blue table-bordered">
                                <tbody>
                                    <% if (Session["CompID"] != null && (Session["CompID"].ToString() == "1106"))
                                        { %>
                                    <tr class="infoclr">
                                        <td></td>
                                        <th style="text-align: right">Direct</th>
                                        <th style="text-align: right">Indirect</th>
                                        <th style="text-align: right">Total</th>
                                    </tr>
                                    <% }
                                        else
                                        { %>
                                    <tr class="infoclr">
                                        <td></td>
                                        <th style="text-align: right">Left</th>
                                        <th style="text-align: right">Right</th>
                                        <th style="text-align: right">Total</th>
                                    </tr>
                                    <% } %>

                                    <!-- Today's / Current Week Registration -->
                                    <tr>
                                        <th>
                                            <% if (
                                                                                                                                                                                              Session["CompID"] != null &&
                                                                                                                                                                                              (Session["CompID"].ToString() == "1007" ||
                                                                                                                                                                                               Session["CompID"].ToString() == "1038" ||
                                                                                                                                                                                               Session["CompID"].ToString() == "1030" ||
                                                                                                                                                                                               Session["CompID"].ToString() == "1074")
                                                                                                                                                                                           )
                                                { %>
                        Current Week
                <% }
                    else
                    { %>
                        Today's
                <% } %>
                Registration
                                        </th>

                                        <td id="TodayLeftRegister" runat="server" style="text-align: right">0</td>
                                        <td id="TodayRightRegister" runat="server" style="text-align: right">0</td>
                                        <td id="TotalTodayRegister" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <!-- Total Registration -->
                                    <tr class="backclr">
                                        <th>Total Registration</th>
                                        <td id="TotalLeftJoin" runat="server" style="text-align: right">0</td>
                                        <td id="TotalRightJoin" runat="server" style="text-align: right">0</td>
                                        <td id="TotalJoin" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <!-- Activation (Today / Week, Plan A) -->
                                    <tr class="backclr">
                                        <th>
                                            <% if (
                                                                                                                                                                                              Session["CompID"] != null &&
                                                                                                                                                                                              (Session["CompID"].ToString() == "1007" ||
                                                                                                                                                                                               Session["CompID"].ToString() == "1038" ||
                                                                                                                                                                                               Session["CompID"].ToString() == "1030" ||
                                                                                                                                                                                               Session["CompID"].ToString() == "1074")
                                                                                                                                                                                           )
                                                { %>
                    Current Week
                <% }
                    else
                    { %>
                    Today's
                <% } %>
                Activation
                <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1038")
                    { %>
                    In Plan A
                <% } %>
                                        </th>

                                        <td id="TodayLeftActive" runat="server" style="text-align: right">0</td>
                                        <td id="TodayRightActive" runat="server" style="text-align: right">0</td>
                                        <td id="TodayTotalActive" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <!-- Total Activation -->
                                    <tr>
                                        <th>Total Activation
                <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1038")
                    { %>
                    In Plan A
                <% } %>
                                        </th>
                                        <td id="TotalLeftActivation" runat="server" style="text-align: right">0</td>
                                        <td id="TotalRightActivation" runat="server" style="text-align: right">0</td>
                                        <td id="TotalActivation" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <!-- Plan B Current -->
                                    <tr class="backclr" id="trplanBCurrentactive" runat="server" visible="false">
                                        <th>
                                            <% if (
                                                                                                                                                                                               Session["CompID"] != null &&
                                                                                                                                                                                               (Session["CompID"].ToString() == "1007" ||
                                                                                                                                                                                                Session["CompID"].ToString() == "1038" ||
                                                                                                                                                                                                Session["CompID"].ToString() == "1030" ||
                                                                                                                                                                                                Session["CompID"].ToString() == "1074")
                                                                                                                                                                                            )
                                                { %>
                    Current Week
                <% }
                    else
                    { %>
                    Today's
                <% } %>
                Activation
                <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1038")
                    { %>
                    In Plan B
                <% } %>
                                        </th>

                                        <td id="Tdplanbcurrleftactive" runat="server" style="text-align: right">0</td>
                                        <td id="Tdplanbcurrrightactive" runat="server" style="text-align: right">0</td>
                                        <td id="Tdplanbcurractive" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <!-- Plan B Total -->
                                    <tr id="trtotalplanbactive" runat="server" visible="false">
                                        <th>Total Activation
                <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1038")
                    { %>
                    In Plan B
                <% } %>
                                        </th>

                                        <td id="Tdplanbleftactive" runat="server" style="text-align: right">0</td>
                                        <td id="Tdplanbrightactive" runat="server" style="text-align: right">0</td>
                                        <td id="Tdplanbtotalactive" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <!-- Total Direct Active -->
                                    <tr class="backclr" id="trdirectactive" runat="server">
                                        <th>Total Direct Active</th>

                                        <td id="DirectActive" runat="server" style="text-align: right">0</td>
                                        <td id="IndirectActive" runat="server" style="text-align: right">0</td>
                                        <td id="TotalDirectActive" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <!-- Current Week / Today's BV / Unit -->
                                    <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1105" || Session["CompID"].ToString() == "1107" || Session["CompID"].ToString() == "1109" || Session["CompID"].ToString() == "1110")
                                        {   %>
                                    <% } %>
                                    <% else
                                        {  %>
                                    <tr id="trcWeek" runat="server">
                                        <th>
                                            <% if (Session["CompID"] != null &&
                                                                                                                                                                                               (Session["CompID"].ToString() == "1007" ||
                                                                                                                                                                                                Session["CompID"].ToString() == "1038")
                                                                                                                                                                                            )
                                                { %>
                    Current Week Joining
                <% }
                    else if (Session["CompID"] != null &&
                               Session["CompID"].ToString() == "1030")
                    { %>
                    Current Week
                <% }
                    else if (Session["CompID"] != null &&
                               (Session["CompID"].ToString() == "1074" || Session["CompID"].ToString() == "1100")
                     )
                    { %>
                    Today's <%= Session["ColName1"] %>
                                            <% }
                                                else
                                                { %>
                    Today's <%= Session["ColName1"] %>
                                            <% } %>
                                        </th>

                                        <td id="TdTodayLeftUnit" runat="server" style="text-align: right">0</td>
                                        <td id="TdTodayRightUnit" runat="server" style="text-align: right">0</td>
                                        <td id="TdTodayTotalUnit" runat="server" style="text-align: right">0</td>
                                    </tr>
                                    <% } %>
                                    <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1107" || Session["CompID"].ToString() == "1109" || Session["CompID"].ToString() == "1110")
                                        {   %>
                                    <% } %>
                                    <% else
                                        {  %>
                                    <!-- Current Week Repurchase -->
                                    <tr runat="server" id="trRepurcurrSessPVBV" visible="false">
                                        <th>
                                            <% if (
                                                                                                                                                                                               Session["CompID"] != null &&
                                                                                                                                                                                               (Session["CompID"].ToString() == "1007" ||
                                                                                                                                                                                                Session["CompID"].ToString() == "1038" ||
                                                                                                                                                                                                Session["CompID"].ToString() == "1030")
                                                                                                                                                                                            )
                                                { %>
                    Current Week Repurchase
                <% }
                    else
                    { %>
                    Today's
                <% } %>
                                            <%= Session["ColName1"] %>
                                        </th>

                                        <td id="TdTodayLeftUnitR" runat="server" style="text-align: right">0</td>
                                        <td id="TdTodayRightUnitR" runat="server" style="text-align: right">0</td>
                                        <td id="TdTodayTotalUnitR" runat="server" style="text-align: right">0</td>
                                    </tr>
                                    <%} %>

                                    <!-- Total BV / Total Units -->
                                    <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1105" || Session["CompID"].ToString() == "1107" || Session["CompID"].ToString() == "1109" || Session["CompID"].ToString() == "1110")
                                        {   %>
                                    <% } %>
                                    <% else
                                        {  %>

                                    <tr id="trToalBv" runat="server">
                                        <th>
                                            <% if (Session["CompID"] != null &&
                                                                                                                                                                                               (Session["CompID"].ToString() == "1074" || Session["CompID"].ToString() == "1100"))
                                                { %>
                    Total <%= Session["ColName1"] %>
                                            <% }
                                                else
                                                { %>
                    Total <%= Session["ColName1"] %>
                                            <% } %>
                                        </th>

                                        <td id="TotalLeftUnit" runat="server" style="text-align: right">0</td>
                                        <td id="TotalRightUnit" runat="server" style="text-align: right">0</td>
                                        <td id="TotalUnit" runat="server" style="text-align: right">0</td>
                                    </tr>
                                    <% } %>
                                    <!-- Total Direct BV / Unit -->
                                    <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1105" || Session["CompID"].ToString() == "1107" || Session["CompID"].ToString() == "1109" || Session["CompID"].ToString() == "1110")
                                        {   %>
                                    <% } %>
                                    <% else
                                        {  %>
                                    <tr class="backclr" id="trTotaldirect" runat="server">
                                        <th>
                                            <% if (Session["CompID"] != null &&
                                                                                                                                                                                               (Session["CompID"].ToString() == "1074" || Session["CompID"].ToString() == "1100"))
                                                { %>
                    Total Direct BV
                <% }
                    else
                    { %>
                    Total Direct Total <%= Session["ColName1"] %>
                                            <% } %>
                                        </th>

                                        <td id="Directunit" runat="server" style="text-align: right">0</td>
                                        <td id="indirectunit" runat="server" style="text-align: right">0</td>
                                        <td id="totalDirectunit" runat="server" style="text-align: right">0</td>
                                    </tr>
                                    <% } %>

                                    <!-- Today's SB -->
                                    <tr visible="false" id="trtodaysb" runat="server" style="text-align: right">
                                        <th>Today's SB</th>
                                        <td id="todayleftsb" runat="server" style="text-align: right"></td>
                                        <td id="todayrightsb" runat="server" style="text-align: right"></td>
                                        <td id="todaytotalsb" runat="server" style="text-align: right"></td>
                                    </tr>

                                    <!-- Total SB -->
                                    <tr visible="false" class="backclr" id="trtotalsb" runat="server">
                                        <th>Total SB</th>
                                        <td id="Totalleftsb" runat="server" style="text-align: right"></td>
                                        <td id="totalrightsb" runat="server" style="text-align: right"></td>
                                        <td id="totalsb" runat="server" style="text-align: right"></td>
                                    </tr>

                                    <!-- Total Direct SB -->
                                    <tr visible="false" id="trdirectsb" runat="server">
                                        <th>Total Direct SB</th>
                                        <td id="TdLeftdirectsb" runat="server" style="text-align: right"></td>
                                        <td id="Tdrightdirectsb" runat="server" style="text-align: right"></td>
                                        <td id="Tdtotaldirectsb" runat="server" style="text-align: right"></td>
                                    </tr>

                                    <!-- Current Month Activation -->
                                    <tr style="display: none;">
                                        <th>Current <%= Session["ColName3"] %> Activation
                                        </th>
                                        <td id="TdLeftCMonth" runat="server" style="text-align: right">0</td>
                                        <td id="TdRightCMonth" runat="server" style="text-align: right">0</td>
                                        <td id="TdTotalCMonth" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <!-- Current Session BV / PV -->
                                    <tr runat="server" id="trcurrSessPVBV" visible="false">
                                        <th>Current <%= Session["ColName3"] %> <%= Session["ColName1"] %>
                                        </th>
                                        <td id="CLeftCoin" runat="server" style="text-align: right">0</td>
                                        <td id="CRightCoin" runat="server" style="text-align: right">0</td>
                                        <td id="CTotalCoin" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <!-- Carry Forward -->
                                    <tr runat="server" id="trCFBVPV" visible="false">
                                        <th>
                                            <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1074")
                                                { %>
                    Carry Forward BV
                <% }
                    else
                    { %>
                    Carry Forward <%= Session["ColName1"] %>
                                            <% } %>
                                        </th>

                                        <td id="LegXBvCF" runat="server" style="text-align: right">0</td>
                                        <td id="LegYBvCF" runat="server" style="text-align: right">0</td>
                                        <td id="TotalBVCF" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <!-- Royalty & Repurchase BV for 1010 / 1074 -->
                                    <% if (Session["CompID"] != null &&
                                                                                                                                                                                                   (Session["CompID"].ToString() == "1010" || Session["CompID"].ToString() == "1074"))
                                        { %>

                                    <tr runat="server" id="RoyaltyBV">
                                        <th>
                                            <% if (Session["CompID"].ToString() == "1010")
                                                { %>
                        Royalty
                    <% }
                        else
                        { %>
                        Current Month BV
                    <% } %>
                                        </th>

                                        <td id="LeftRoyaltyBV" runat="server" style="text-align: right">0</td>
                                        <td id="RightRoyaltyBV" runat="server" style="text-align: right">0</td>
                                        <td id="TotalRoyaltyBV" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <tr runat="server" id="RepurchinBV">
                                        <th>
                                            <% if (Session["CompID"].ToString() == "1010")
                                                { %>
                        Repurchase BV
                    <% }
                        else
                        { %>
                        Total BV
                    <% } %>
                                        </th>

                                        <td id="LeftRepurchinBV" runat="server" style="text-align: right">0</td>
                                        <td id="RightRepurchinBV" runat="server" style="text-align: right">0</td>
                                        <td id="TotalRepurchinBV" runat="server" style="text-align: right">0</td>
                                    </tr>

                                    <% } %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

            </div>
            <div runat="server" id="divRbv" visible="false">
                <div class="profile-bar-simple red-border clearfix">
                    <h6>MY TEAM RBV BUSINESS SUMMARY</h6>
                </div>

                <div class="col-md-12">
                    <table id="table5" class="table table-hover table-mc-light-blue table-bordered">
                        <thead>
                            <tr class="tablerowcolor"></tr>
                        </thead>
                        <tbody>

                            <tr>
                                <td></td>

                                <th style="text-align: right">
                                    <% 
                                        if (Session["CompID"] != null && Session["CompID"].ToString() == "1006")
                                        {
                                    %>
            Direct
        <% 
            }
            else
            {
        %>
            Left
        <% 
            }
        %>
                                </th>

                                <th style="text-align: right">
                                    <% 
                                        if (Session["CompID"] != null && Session["CompID"].ToString() == "1006")
                                        {
                                    %>
            InDirect
        <% 
            }
            else
            {
        %>
            Right
        <% 
            }
        %>
                                </th>

                                <th style="text-align: right">Total</th>
                            </tr>

                            <tr>
                                <th>Current Month RBV</th>
                                <td id="CurrentMonthLeftRBV" runat="server" style="text-align: right">0</td>
                                <td id="CurrentMonthRightRbv" runat="server" style="text-align: right">0</td>
                                <td id="TotalCurrentMonthRbv" runat="server" style="text-align: right">0</td>
                            </tr>

                            <tr>
                                <th>Till Prev. Month RBV</th>
                                <td id="PerviousMonthLeftRBV" runat="server" style="text-align: right">0</td>
                                <td id="PerviousMonthRightRbv" runat="server" style="text-align: right">0</td>
                                <td id="TotalPerviousMonthRbv" runat="server" style="text-align: right">0</td>
                            </tr>

                            <tr>
                                <th>Total Accumulative RBV</th>
                                <td id="TAmmLeftRBV" runat="server" style="text-align: right">0</td>
                                <td id="TAmRightRbv" runat="server" style="text-align: right">0</td>
                                <td id="TotalAmmRbv" runat="server" style="text-align: right">0</td>
                            </tr>

                        </tbody>
                    </table>
                </div>

                <div class="news-separator"></div>
            </div>
            <div class="clearfix gen-profile-box" id="Divsingle" runat="server">
                <div class="profile-features-1">

                    <div class="profile-bar-simple red-border clearfix">
                        <h6>Single Leg Income</h6>
                    </div>

                    <div id="Div6" class="col-md-12" style="font-size: 9px">
                        <div class="table-responsive">
                            <table id="Table1" class="table datatable">
                                <thead>
                                    <tr class="infoclr">
                                        <th>SNo</th>
                                        <th>Level</th>
                                        <th>Commission</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:Repeater ID="RptSingle" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRowNumber"
                                                        Text='<%# (Container.ItemIndex + 1).ToString() %>'
                                                        runat="server" />
                                                </td>
                                                <td><%# Eval("Mlevel") %></td>
                                                <td><%# Eval("Commission") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>

                            </table>
                        </div>
                    </div>

                    <p class="btn btn-default pull-right">View More</p>

                    <div class="news-separator"></div>

                </div>
            </div>
            <!-- My Directs + Rewards -->
            <div class="col-lg-6" id="dvVPRequest" runat="server">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-user-friends me-2"></i>My Direct Members</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-hover table-mc-light-blue table-bordered">
                                <thead>
                                    <tr>
                                        <th>SNo
                                        </th>
                                        <th>ID No
                                        </th>
                                        <th>Member Name
                                        </th>
                                        <th>Activation Date
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="RptDirects" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                </td>
                                                <td>
                                                    <%#Eval("IDNo")%>
                                                </td>
                                                <td>
                                                    <%#Eval("MemName")%>
                                                </td>
                                                <td>
                                                    <%#Eval("DOA")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6" id="DivEV" runat="server" visible="false">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-user-friends me-2"></i>Direct Business Reward</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-hover table-mc-light-blue table-bordered">
                                <thead>
                                    <tr>
                                        <th>SNo
                                        </th>
                                        <th>Required Direct Business
                                        </th>
                                        <th>Completed Direct Business
                                        </th>
                                        <th>Achievers
                                        </th>
                                        <th>Status
                                        </th>
                                        <th>No. Of Achiever
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="RptDirectBusinessReward" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                </td>
                                                <td>
                                                    <%#Eval("RequiredDirectBusiness")%>
                                                </td>
                                                <td>
                                                    <%#Eval("CompletedDirectBusiness")%>
                                                </td>
                                                <td>
                                                    <%#Eval("Achievers")%>
                                                </td>
                                                <td>
                                                    <%#Eval("Status")%>
                                                </td>
                                                <td>
                                                    <%#Eval("NoOfAchiever")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="clearfix gen-profile-box" id="divAppAmount" visible="false" runat="server">
                <div class="profile-features-1">
                    <div class="profile-bar-simple red-border clearfix">
                        <h6>
                            <asp:Label ID="lblApp" runat="server" Text="Label"></asp:Label>
                        </h6>
                    </div>

                    <div id="Div7" class="col-md-12" style="font-size: 9px">
                        <div class="table-responsive">
                            <table id="Table2" class="table datatable">
                                <thead>
                                    <tr class="infoclr">
                                        <th>ID Type</th>
                                        <th>Amount</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="RepAppAmount" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("Name") %></td>
                                                <td><%# Eval("Amount") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>

                    <div class="news-separator"></div>
                </div>
            </div>


            <!-- Surksha Fund -->
            <div class="clearfix gen-profile-box" id="divsurkshafund" visible="false" runat="server">
                <div class="profile-features-1">
                    <div class="profile-bar-simple red-border clearfix">
                        <h6>
                            <asp:Label ID="lblname" runat="server" Text="Family Surksha Fund"></asp:Label>
                        </h6>
                    </div>

                    <div id="Div19" class="col-md-12" style="font-size: 9px">
                        <div class="table-responsive" style="text-align: center; font-weight: bold;">
                            <asp:Label ID="lblstatus" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="news-separator"></div>
                </div>
            </div>


            <!-- Re-entry -->
            <div class="clearfix gen-profile-box" id="divreentry" visible="false" runat="server">
                <div class="profile-features-1">
                    <div class="profile-bar-simple red-border clearfix">
                        <h6>
                            <asp:Label ID="lbl" runat="server" Text="Rentry Total Count "></asp:Label>
                        </h6>
                    </div>

                    <div id="Div23" class="col-md-12" style="font-size: 9px">
                        <div class="table-responsive" style="text-align: center; font-weight: bold;">
                            <asp:Label ID="lblreentry" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="news-separator"></div>
                </div>
            </div>


            <!-- Direct Income -->
            <div class="col-md-12" id="DivDirectIncome" runat="server" visible="false">
                <div class="row">
                    <div class="col-lg-12 col-sm-12">
                        <div>
                            <div class="row">
                                <div class="clearfix gen-profile-box" style="min-height: 100px;">
                                    <div class="profile-bar-simple blue-border">
                                        <h6>My Direct Income</h6>
                                    </div>

                                    <div class="profile-features-1">
                                        <asp:Repeater ID="Mydirectincome" runat="server">
                                            <ItemTemplate>
                                                <div class="col-md-12">
                                                    <div class="row tab-box" style="margin: 17px 0;">
                                                        <div class="col-lg-6 col-sm-12 tab-bg">
                                                            <span class="tab-lable">My Direct Income</span>
                                                        </div>
                                                        <div class="col-lg-6 col-sm-12">
                                                            <span class="tab-field" id="directincome" runat="server">
                                                                <%# Eval("DirectIncome") %>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <!-- Pair Cut -->
            <div class="col-md-12" id="divPairCut" runat="server" visible="false">
                <div class="row">
                    <div class="col-lg-12 col-sm-12">
                        <div>
                            <div class="row">
                                <div class="clearfix gen-profile-box" style="min-height: 100px;">
                                    <div class="profile-bar-simple blue-border">
                                        <h6>Status Of Participation in Plan "B"</h6>
                                    </div>

                                    <div class="profile-features-1">
                                        <asp:Repeater ID="repPaircut" runat="server">
                                            <ItemTemplate>
                                                <div class="col-md-12">
                                                    <h4><%# Eval("IDStatus") %></h4>
                                                    <br />

                                                    <button type="button" id="b1" runat="server" class='<%# Eval("b1") %>'>Point1</button>
                                                    <button type="button" id="b2" runat="server" class='<%# Eval("b2") %>'>Point2</button>
                                                    <button type="button" id="b3" runat="server" class='<%# Eval("b3") %>'>Point3</button>
                                                    <button type="button" id="b4" runat="server" class='<%# Eval("b4") %>'>Point4</button>
                                                    <button type="button" id="b5" runat="server" class='<%# Eval("b5") %>'>Point5</button>

                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <% 
                if (Session["compid"] != null &&
                    (Session["compid"].ToString() == "1010" ||
                     Session["compid"].ToString() == "1091"))
                {
            %>

            <div class="clearfix gen-profile-box" id="Div20" runat="server" visible="false">
                <div class="profile-features-1">

                    <div class="profile-bar-simple red-border clearfix">
                        <h6>
                            <asp:Label ID="Bonzaname" runat="server" />
                        </h6>
                    </div>

                    <div id="Div21" class="col-md-12" style="font-size: 9px">
                        <div class="table-responsive">
                            <asp:GridView
                                ID="gvbonanza"
                                Width="100%"
                                runat="server"
                                AllowPaging="false"
                                GridLines="Both"
                                CssClass="table table-bordered"
                                HeaderStyle-CssClass="bg-primary"
                                ShowHeader="true"
                                AutoGenerateColumns="false"
                                ForeColor="Black">

                                <Columns>
                                    <asp:BoundField HeaderText="Rank" DataField="Rank" />
                                    <asp:BoundField HeaderText="Reward" DataField="Reward" />
                                    <asp:BoundField HeaderText="Req.Matching BV" DataField="MatchingBV" />
                                    <asp:BoundField HeaderText="LeftBV" DataField="LeftBV" />
                                    <asp:BoundField HeaderText="RightBV" DataField="RightBV" />
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                </Columns>

                            </asp:GridView>
                        </div>
                    </div>

                    <div class="profile-bar-simple red-border clearfix">
                        <h6>
                            <asp:Label ID="LblBonanzaDirect" runat="server" />
                        </h6>
                    </div>

                    <div id="Div22" class="col-md-12" style="font-size: 9px">
                        <div class="table-responsive">
                            <asp:GridView
                                ID="gvDirectBonanza"
                                Width="100%"
                                runat="server"
                                AllowPaging="false"
                                GridLines="Both"
                                CssClass="table table-bordered"
                                HeaderStyle-CssClass="bg-primary"
                                ShowHeader="true"
                                AutoGenerateColumns="false"
                                ForeColor="Black">

                                <Columns>
                                    <asp:BoundField HeaderText="Req.Direct BV" DataField="MatchingBV" />
                                    <asp:BoundField HeaderText="Direct BV" DataField="DirectBV" />
                                    <asp:BoundField HeaderText="Reward" DataField="Reward" />
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                </Columns>

                            </asp:GridView>
                        </div>
                    </div>

                    <div class="news-separator"></div>

                </div>
            </div>

            <% } %>
            <div class="row">
                <div class="col-md-12">
                    <div class="clearfix gen-profile-box" id="Div3" runat="server">
                        <div class="profile-features-1">
                            <div class="col-md-12">
                                <div id="Div8">
                                    <div class="profile-bar-simple red-border clearfix">
                                        <h6>Ganesh Chaturthi And VijyaDashmi Bonanza
                                        </h6>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="profile-bar-simple red-border clearfix">
                                                    <h6>Direct Bonanza
                                                    </h6>
                                                </div>
                                                <div class="col-md-4">
                                                    <table id="table4" class="table table-hover table-mc-light-blue table-bordered">
                                                        <thead>
                                                            <th style="text-align: center !important">Bonanza
                                                            </th>
                                                            <th style="text-align: right !important">Require BV
                                                            </th>
                                                            <th style="text-align: right !important">Pending BV
                                                            </th>
                                                            <th style="text-align: right !important">Status
                                                            </th>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="RpDirectBonanza" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td style="text-align: center !important">
                                                                            <%#Eval("Reward")%>
                                                                        </td>
                                                                        <td style="text-align: right !important">
                                                                            <%#Eval("ReqBV")%>
                                                                        </td>
                                                                        <td style="text-align: right !important">
                                                                            <%#Eval("PendingBV")%>
                                                                        </td>
                                                                        <td style="text-align: right !important">
                                                                            <%#Eval("AchStatus")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="profile-bar-simple red-border clearfix">
                                                    <h6>Self Bonanza
                                                    </h6>
                                                </div>
                                                <table id="table8" class="table table-hover table-mc-light-blue table-bordered">
                                                    <thead>
                                                        <th style="text-align: center !important">Bonanza
                                                        </th>
                                                        <th style="text-align: right !important">Require BV
                                                        </th>
                                                        <th style="text-align: right !important">Pending BV
                                                        </th>
                                                        <th style="text-align: right !important">Status
                                                        </th>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="RptSelf" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="text-align: center !important">
                                                                        <%#Eval("Reward")%>
                                                                    </td>
                                                                    <td style="text-align: right !important">
                                                                        <%#Eval("ReqBV")%>
                                                                    </td>
                                                                    <td style="text-align: right !important">
                                                                        <%#Eval("PendingBV")%>
                                                                    </td>
                                                                    <td style="text-align: right !important">
                                                                        <%#Eval("AchStatus")%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="profile-bar-simple red-border clearfix">
                                                    <h6>Matching Bonanza
                                                    </h6>
                                                </div>
                                                <table id="table9" class="table table-hover table-mc-light-blue table-bordered">
                                                    <thead>
                                                        <th style="text-align: center !important">Bonanza
                                                        </th>
                                                        <th style="text-align: right !important">Require BV
                                                        </th>
                                                        <th style="text-align: right !important">Pending BV
                                                        </th>
                                                        <th style="text-align: right !important">Status
                                                        </th>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="RptMatching" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="text-align: center !important">
                                                                        <%#Eval("Reward")%>
                                                                    </td>
                                                                    <td style="text-align: right !important">
                                                                        <%#Eval("ReqBV")%>
                                                                    </td>
                                                                    <td style="text-align: right !important">
                                                                        <%#Eval("PendingBV")%>
                                                                    </td>
                                                                    <td style="text-align: right !important">
                                                                        <%#Eval("AchStatus")%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="DivPurchaseVolume" runat="server" visible="false">
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">Personally Purchase Volume</h5>
                        </div>

                        <div class="card-content collapse show">
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-hover table-bordered mb-0">
                                        <thead class="table-light">
                                            <tr>
                                                <th>PPV Detail</th>
                                                <th class="text-end"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td><strong>Current Month</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblCurrentMonthPPV" runat="server" class="fw-bold" />
                                                    PPV</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Last Month</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblLastMonthPPV" runat="server" class="fw-bold" />
                                                    PPV</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Total</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblTotalMonthPPV" runat="server" class="fw-bold" />
                                                    PPV</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">Wholesale Business
Volumes</h5>
                        </div>

                        <div class="card-content collapse show">
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-hover table-bordered mb-0">
                                        <thead class="table-light">
                                            <tr>
                                                <th>WBV Detail</th>
                                                <th class="text-end"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td><strong>Current Month</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblCurrentMonthWBV" runat="server" class="fw-bold" />
                                                    WBV</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Last Month</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblLastMonthWBV" runat="server" class="fw-bold" />
                                                    WBV</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Total</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblTotalMonthWBV" runat="server" class="fw-bold" />
                                                    WBV</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">Royalty Business
1 To 5 Levels
Volume</h5>
                        </div>

                        <div class="card-content collapse show">
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-hover table-bordered mb-0">
                                        <thead class="table-light">
                                            <tr>
                                                <th>RBV Detail</th>
                                                <th class="text-end"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td><strong>Current Month</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblCurrentMonthRBV" runat="server" class="fw-bold" />
                                                    RBV</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Last Month</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblLastMonthRBV" runat="server" class="fw-bold" />
                                                    RBV</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Total</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblTotalMonthRBV" runat="server" class="fw-bold" />
                                                    RBV</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">Production Bonus 6 Level & Above Volume</h5>
                        </div>

                        <div class="card-content collapse show">
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-hover table-bordered mb-0">
                                        <thead class="table-light">
                                            <tr>
                                                <th>PBV Detail</th>
                                                <th class="text-end"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td><strong>Current Month</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblCurrentMonthPBV" runat="server" class="fw-bold" />
                                                    PBV</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Last Month</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblLastMonthPBV" runat="server" class="fw-bold" />
                                                    PBV</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Total</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblTotalMonthPBV" runat="server" class="fw-bold" />
                                                    PBV</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
                <div class="col-md-4">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">Total Volumes PPV & WBV</h5>
                        </div>

                        <div class="card-content collapse show">
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-hover table-bordered mb-0">
                                        <thead class="table-light">
                                            <tr>
                                                <th>PPV Detail</th>
                                                <th class="text-end"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td><strong>Current Month</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblCuunerentPPVWBV" runat="server" class="fw-bold" />
                                                    PPV</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Last Month</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblLastPPVWBV" runat="server" class="fw-bold" />
                                                    PPV</td>
                                            </tr>
                                            <tr>
                                                <td><strong>Total</strong></td>
                                                <td class="text-end fw-bold ">
                                                    <asp:Label ID="LblTotalPPVWBV" runat="server" class="fw-bold" />
                                                    PPV</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>
    <asp:Repeater ID="RptPopup" runat="server">
        <ItemTemplate>
            <%#Eval("DivStart")%>
        </ItemTemplate>
    </asp:Repeater>


    <script>
        function openPhotoModal() {
            var modal = new bootstrap.Modal(document.getElementById('profilePhotoModal'));
            modal.show();
        }

        // open file chooser
        function openFilePicker() {
            document.getElementById('<%= FileUpload1.ClientID %>').click();
        }

        // instant preview after selecting image
        function previewPhoto(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    // show preview inside modal
                    document.getElementById("ContentPlaceHolder1_photoPreview").src = e.target.result;
                };

                reader.readAsDataURL(input.files[0]);
            }
        }


    </script>

</asp:Content>


