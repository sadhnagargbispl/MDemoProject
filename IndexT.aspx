<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="IndexT.aspx.cs" Inherits="IndexT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="assets/Hemalika/new_custom.css?v=1.0" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css">
    <script type="text/javascript">
        function copyLinkClientUpdate() {

            var copyText = document.getElementById("ContentPlaceHolder1_hfLinkupdate").value;

            navigator.clipboard.writeText(copyText).then(function () {
                alert('Link copied successfully');
            }).catch(function () {
                alert('Unable to copy link');
            });

            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <section class="section">
            <%--   <ul class="breadcrumb breadcrumb-style ">
                <li class="breadcrumb-item">
                    <h4 class="page-title m-b-0">Dashboard</h4>
                </li>
                <li class="breadcrumb-item">
                    <a href="#">
                        <i data-feather="home"></i>
                    </a>
                </li>
                <li class="breadcrumb-item active">Dashboard</li>
            </ul>--%>



            <div class="row mb-4">


                <div id="#" class="col-lg-4">
                    <div class="card profile-card p-4">

                        <!-- Profile Image -->
                        <div class="text-center">
                            <img src="assets/img/banner/bannerr.jpeg" class="profileimg img-fluid" style="max-width: 100%; height: auto;">
                        </div>

                        <!-- Name & Designation -->
                        <div class="row mt-3 align-items-center">
                            <div class="col-md-6">
                                <p class="mb-1 text-success" style="color: #198754 !important;"><b>Name:</b></p>
                                <h5 class="mb-0">
                                    <asp:Label ID="LblMemName" runat="server"></asp:Label></h5>
                            </div>

                            <div class="col-md-6 text-md-right mt-2 mt-md-0">
                                <p class="mb-1 text-success" style="color: #198754 !important;"><b>Designation : </b></p>
                                <h6 class="mb-0 text-dark" style="color: #433e3e !important;">
                                    <asp:Label ID="lblRankTitle" runat="server"></asp:Label></h6>
                            </div>
                        </div>

                        <hr>
                        <style>
                            .theme-white .text-primary1 {
                                color: #433e3e !important;
                            }
                        </style>
                        <!-- ID + Sponsor -->
                        <div class="row info-box text-center text-md-left">

                            <div class="col-md-6 border-right">
                                <p class="mb-1">
                                    <b>ID No:</b>
                                    <asp:Label ID="LblMemId" runat="server"></asp:Label>
                                </p>
                                <p class="mb-0 text-primary1">
                                    Date Of Joining:
                                    <asp:Label ID="LblJoiningDate" runat="server"></asp:Label>
                                    <%--     Mar 01, 2025--%>
                                </p>
                            </div>

                            <div class="col-md-6 mt-2 mt-md-0">
                                <p class="mb-1 text-primary1"><b>Sponsor</b></p>
                                <p class="mb-0">
                                    Name: <b>
                                        <asp:Label ID="LblSponsorName" runat="server"></asp:Label></b>
                                </p>
                                <p class="mb-0">
                                    ID No:
                                    <asp:Label ID="LblSponsorID" runat="server"></asp:Label>
                                </p>
                            </div>

                        </div>

                        <hr>

                        <!-- Referral Links -->
                        <div class="text-center">
                            <asp:HiddenField ID="hfLinkupdate" runat="server" />
                            <h6 class="mb-3">My Referral Links</h6>

                            <div class="d-flex justify-content-center flex-wrap gap-2">

                                <!-- Copy -->
                                <button type="button" class="btn btn-copy" onclick="copyLinkClientUpdate()">
                                    <i class="fa-solid fa-copy"></i>Copy Links
                                </button>
                                <!-- Facebook -->
                                <a id="aFacebook" runat="server" target="_blank" class="btn btn-fb">
                                    <i class="fa-brands fa-facebook-f"></i>
                                </a>

                                <!-- Twitter -->
                                <a id="aTwitter" runat="server" target="_blank" class="btn btn-twitter">
                                    <i class="fa-brands fa-x-twitter"></i>
                                </a>

                                <!-- WhatsApp -->
                                <a id="aWhatsapp" runat="server" target="_blank" class="btn btn-whatsapp">
                                    <i class="fa-brands fa-whatsapp"></i>
                                </a>

                                <!-- Telegram -->
                                <a id="aTelegram" runat="server" target="_blank" class="btn btn-telegram">
                                    <i class="fa-brands fa-telegram"></i>
                                </a>
                                <!-- Facebook -->

                                <%-- <button class="btn btn-fb"
                                <i class="fa-brands fa-facebook-f"></i>>--%>
                                <%--</button>--%>

                                <!-- Twitter (X) -->

                                <%-- <button class="btn btn-twitter">
                                    <i class="fa-brands fa-x-twitter"></i>
                                </button>--%>

                                <!-- WhatsApp -->

                                <%--  <button class="btn btn-whatsapp">
                                    <i class="fa-brands fa-whatsapp"></i>
                                </button>--%>

                                <!-- Telegram -->

                                <%-- <button class="btn btn-telegram">
                                    <i class="fa-brands fa-telegram"></i>
                                </button>--%>
                            </div>
                        </div>

                    </div>
                </div>




                <div class="col-lg-8">


                    <div class="dashboard-cards">
                        <div class="row gy-0">

                            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
                                <div class="card" style="">
                                    <a href="javascript:void(0);" data-bs-toggle="modal" data-bs-target="#iconModal">
                                        <div class="card-body">
                                            <span class="card-icon text-danger"><i class="fa-solid fa-file"></i></span>
                                            <div>
                                                <p class="card-title">Agreement Form </p>

                                            </div>
                                        </div>
                                    </a>
                                </div>
                                <%-- <div class="card">
                                        <div class="card-body">
                                            <span class="card-icon text-danger"><i class="fa-solid fa-file"></i></span>
                                            <div>
                                                <p class="card-title">Agreement Form </p>
                                                
                                            </div>
                                        </div>

                                    </div>--%>
                            </div>
                            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
                                <a href="ShoppingRedirect.aspx" style="text-decoration: none; color: black;">
                                    <div class="card">

                                        <div class="card-body">
                                            <span class="card-icon text-danger">
                                                <i class="fa-solid fa-cart-shopping"></i>
                                            </span>

                                            <div>
                                                <p class="card-title">Shop</p>
                                                <%-- <p class="card-label">Purchase products & offers</p> --%>
                                            </div>

                                        </div>

                                    </div>
                                </a>
                            </div>

                            <div class="col-xl-6 col-lg-4 col-md-6 col-12">

                                <a href="assets/Hemalika/Hemalika.pdf" target="_blank" style="text-decoration: none; color: black;">
                                    <div class="card">

                                        <div class="card-body">
                                            <span class="card-icon text-warning"><i class="fa-solid fa-briefcase"></i></span>
                                            <div>
                                                <p class="card-title">My Business</p>
                                                <%-- <p class="card-label">Track progress & metrics</p>--%>
                                            </div>
                                        </div>

                                    </div>
                                </a>
                            </div>

                            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
                                <a href="#" runat="server" id="registrationlink" style="text-decoration: none; color: black;">
                                    <div class="card">

                                        <div class="card-body">
                                            <span class="card-icon text-success"><i class="fa-solid fa-user-plus"></i></span>
                                            <div>
                                                <p class="card-title">New Registration</p>
                                                <%--<p class="card-label">Add new member quickly</p>--%>
                                            </div>
                                        </div>

                                    </div>
                                </a>
                            </div>

                            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
                                <a href="#" style="text-decoration: none; color: black;">
                                    <div class="card">

                                        <div class="card-body">
                                            <span class="card-icon text-primary"><i class="fa-solid fa-bullhorn"></i></span>
                                            <div>
                                                <p class="card-title">Events & Promotions</p>
                                                <%-- <p class="card-label">Latest announcements</p>--%>
                                            </div>
                                        </div>

                                    </div>
                                </a>
                            </div>

                            <div class="col-xl-6 col-lg-4 col-md-6 col-12">
                                <a href="#" style="text-decoration: none; color: black;">
                                    <div class="card">
                                        <div class="card-body">
                                            <span class="card-icon text-dark"><i class="fa-solid fa-key"></i></span>
                                            <div>
                                                <p class="card-title">Key To Success</p>
                                                <%--<p class="card-label">Top tips & resources</p>--%>
                                            </div>
                                        </div>

                                    </div>
                                </a>
                            </div>




                        </div>
                    </div>



                </div>

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


            





            <div class="row g-4">

                <!-- Wallet Summary -->

                <!-- Referral Link Card -->

                
                <div class="col-lg-6" id="DivMyKYCStatus" runat="server" visible="false">
                    <div class="card kyc-card p-3">

                        <h5 class="mb-3">My KYC Status</h5>

                        <div class="row text-center">
                            <asp:Repeater ID="RptKYCStatus" runat="server">
                                <ItemTemplate>
                                    <div class="col-md-4 col-6 kyc-box">
                                        <p class="kyc-title"><%# Eval("DocName") %></p>
                                        <%# Eval("StatusHtml") %>
                                        <%--<span class="badge badge-success"><i class="fa fa-check-circle"></i>VERIFIED</span>--%>
                                    </div>
                                    <%--<div class="col-md-4 kyc-box text-center">
             <p class="kyc-title mb-1"><%# Eval("DocName") %></p>
             <div>
                 <%# Eval("StatusHtml") %>
             </div>
         </div>--%>
                                </ItemTemplate>
                            </asp:Repeater>


                            <%--   <div class="col-md-4 col-6 kyc-box">
                                <p class="kyc-title">BANK</p>
                                <span class="badge badge-rejected"><i class="fa fa-times-circle"></i>REJECTED</span>
                            </div>

                            <div class="col-md-4 col-6 kyc-box">
                                <p class="kyc-title">PAN CARD</p>
                                <span class="badge badge-rejected">
                                    <i class="fa fa-times-circle"></i>Rejected
                                </span>
                            </div>

                            <div class="col-md-4 col-6 kyc-box">
                                <p class="kyc-title">GST</p>
                                <span class="badge badge-pending"><i class="fa fa-exclamation-triangle"></i>NOT UPLOADED</span>
                            </div>

                            <div class="col-md-4 col-6 kyc-box">
                                <p class="kyc-title">FSSAI</p>
                                <span class="badge badge-pending">
                                    <i class="fa fa-exclamation-triangle"></i>Pending
                                </span>
                            </div>--%>
                        </div>

                    </div>

                </div>


                <div class="col-12 col-sm-12 col-md-12 col-lg-8 col-xl-6" runat="server" id="DivNews">
                    <div class="card">
                        <div class="card-header">
                            <h4>Latest News </h4>
                        </div>
                        <div class="card-body">
                            <br>

                            <marquee direction="up" scrollamount="5" onmouseover="this.stop();" onmouseout="this.start();">
                                <asp:Repeater ID="RptNews" runat="server">
                                    <ItemTemplate>
                                        <h6 class="mb-0"><%# Eval("NewsDate") %></h6>
                                        <p><%# Eval("NewsDetail") %></p>
                                        <hr>
                                        <%-- <div class="border-bottom pb-3 mb-3">
            <p class="mb-1 small"><%# Eval("NewsDetail") %></p>
            <small class="text-muted"><i class="fa fa-calendar"></i><%# Eval("NewsDate") %></small>
        </div>--%>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--<h6 class="mb-0">News Heading </h6>
                                <p>Sample text here.. Sample text here.. </p>
                                <hr>
                                <h6 class="mb-0">News Heading </h6>
                                <p>Sample text here.. Sample text here.. </p>--%>
                                <%--    <hr>--%>
                            </marquee>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="row" id="DivPurchaseVolume" runat="server" visible="false">
                    <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
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
                    <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
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
                    <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
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
                    <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
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
                    <div class="col-12 col-sm-12 col-md-12 col-lg-4 col-xl-4">
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
        </section>


    </div>
    <div class="modal fade text-left" id="iconModal" tabindex="-1" role="dialog" aria-labelledby="basicModalLabel2" aria-hidden="true" style="display: none;">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <h4 class="modal-title text-white" id="myModalLabel4">Agreement Form</h4>
                </div>
                <div class="modal-body">
                    <div class="text-justify" style="text-align: justify">
                        <p><b>HEMALIKA INDEPENDENT STAR ASSOCIATE AGREEMENT FORM</b></p>

                        <p>
                            <br />
                            I hereby declare that I am resident of India, my age is above 18 years. I have no criminal history. I have read and understood all the terms and conditions given in this agreement form. I confirm that my sponsor has explained me all the details and I have signed/ affixed thumb impression on agreement only after understanding all the terms and conditions. Therefore, I kindly represent contractual offer to Hemalika wellness Private Limited to appoint me as an IA (Independent associate) For all Hemalika products and services. *HEMALIKA WELLNESS PRIVATE LIMITED is a Company incorporated under the Companies Act 2013, having its office PLOT NO.14 sharda road Parasmani colony ,80 fit road Bhilwara Bhilwara 311001.running business in name and style of Hemalika Here in after referred to as "The Company”. The Company is engaged into the business of direct selling on contract basis and other business activities as stated in the Object Clause of Memorandum of Association of the Company. The company is using its website exclusively to display the details of the products and business methods. It also uses verbal publicity to promote its Business. Individuals (Indian Citizen only) interested in becoming the IA of the company, can apply for the same in the prescribed form by filling each and every column mandatory. There is no deposit or any charges for becoming IA of the company. Before filling this form, IA should go through all the terms and conditions herein below thoroughly and if he agrees to the term and conditions, he shall append his signatures in the column provided hereunder as token of acceptance. The following words used in these presents shall have the meaning as defined here under:- *Direct Selling means marketing, distribution and sale of goods or providing of services as a part of network of direct selling other than under a pyramid scheme, Provided that such sale of goods or services occurs otherwise than through a “Permanent retail location” to the consumers, generally in their houses or at their workplace or through explanation and demonstration of such goods and services at a particular place *Direct selling Entity/ Company: Means a Company namely Hemalika wellness private limited, not being engaged in a pyramid scheme, which sells or offers to sell goods or services through a direct seller. Provided that direct selling entity does not include any entity or business notified otherwise by the Government for the said purpose from time to time *Direct seller/IA: Means a person appointed or authorized and registered as Direct seller/ IA by a Hemalika wellness through a legally enforceable written contract with the Hemalika wellness to undertake direct selling business on principal-to-principal basis. *Consumer: Means a person who purchases goods or hires services for consumption and not for commercial purposes. It shall have the same meaning as provided under the Consumer Protection Act 1986. *Goods/products means goods as defined in the Sale of Goods Act, 1930 and “Service” means service as defined in the Consumer Protection Act, 2019 *Unique ID: Means unique identification number issued by the Company to the IA as token of acceptance of his /her application for Direct selling of the goods/services of the company. Sales incentive: Means amount of incentive payable to the IA for effecting sales of goods/products/services as stipulated in the contract between the IA and the direct selling entity. Password: Means unique code allotted to each IA to allow them to log on to the website of the company. Website: Means official website of the company i.e. www.myhemalika.com
                    * THE APPOINTMENT AND UNDERSTANDING: The Company upon Verification and scrutiny may register the applicant as "IA" for selling the goods/services/products of the company. The Company shall be at liberty to accept or reject the application at its discretion. The IA shall enjoy the following privileges: - 1. Incentive for effecting sale of goods/Products of the Company as per Marketing plan. 2.No territorial restriction to sale the goods/products/services. 3.Search and inspect his account on website of the Company through password awarded by the Company. 4. Earnings of the IA shall be in proportion to the volume of performance by the IA either by his personal efforts or through team as stipulated in the Company. An individual, upon appending his signature at the bottom of these presents, shall be deemed to have accepted the terms and conditions stipulated herein. Upon registration after scrutiny of the application, he shall become the IA of the company, Allotment of password and ID shall be construed as registered as IA
                    Once the username & password is received, the IA must immediately change the password within 24 hours and should not be shared to anyone.
                    For Placing order and to check any information he should take proper training from his Sponsor.
                    Payment should not be given for Placing order to sponsor or upline
                    If does so, IA is solely responsible if anything happens.
                    If the product order delivery address is provided of Sponsor or any other person then IA is solely responsible if anything happens.
                    And IA must check the products as soon as received and if any damage or product not in proper condition then it should be informed to company within 4 hours of receiving products. If does so it will be solely IA Responsibility.

                    FSSAI FOOD REGISTRATION IS MANDATORY (IF IA WANTS TO DO BUSINESS) FOR PLACING ORDER. If not applied IA is solely responsible if anything happens.
                    If IA is operating a office/Nutrition Club and selling HEMALIKA WELLNESS products and promoting business plan, he cannot use logo of company in board, or use product picture on board, and logo of company with part time or full-time business opportunity
                    In case, he does so, warning letter will be sent by company to IA.
                    Promotion Material like Boards, flyers etc., should be approved by Company before using.
                    Before After pics of anyone cannot be used without their permission.
                    Any images from Google cannot be used as may get copyright issues.
                    The IA should not give any Medical Claim for HEMALIKA products to anyone
                    If does so, he is solely responsible for any kind of issues.
                    Take proper training of 90 days with Sponsor or upline before involving in any kind of activities.
                    Company is not encouraging anyone to take office or do any kind of investment,
                    It’s the IA sole responsibility if does so.
                    Purpose for running a Nutrition club or office is to promote Healthy lifestyle to the people and can Earn part time/full time income.
                    Does not keep or order Downline product at your office or Nutrition club.
                    Product exchange with Downline is prohibited.
                    Selling of product should not exceed MRP.
                    In Nutrition club charges of the nutrition provided to customers is given by company
                    Shakes should be given by asking the customers for chilled or normal shakes
                    Follow the Disclaimer before giving products
                        </p>
                        <div class="section disclaimer">
                            <h6>Disclaimer</h6>
                            <p>
                                These Product are not intended to cure or diagnose any kind of diseases
                        Pregnant or lactating women or children below 18 years anyone or having health issues should consult their physician before consuming products.
                        Checkup and counselling should not be chargeable.
                        Free sampling of Product is solely choice of club owner.
                        NUTRITION CLUB OWNERS should not take maintenance charges from downline.
                        Its their mutual understanding if does so.
                        NUTRITION CLUB owner cannot Purchase Products of HEMALIKA WELLNESS Private limited on any e-commerce platforms and should be purchased in his id through company website.
                        Expiry Product cannot be used even for sampling
                        IF ANY TRAINING IN NUTRITION CLUB, THEN SHOULD GIVE INVOICE IN THE NAME OF NC
                        Nutrition club owner cannot use the name of the club in the name of HEMALIKA WELLNESS.


                        Any kind of activities done by IA in his Nutrition Club, he is solely responsible for that.

                        If the IA is conducting any events or training in conference halls or banquet halls then he must take GST number if taking ticket charges from distributors or your team members
                        THE COMPANY Is not encouraging their IAs to promote events for making money,
                        The events done by them are for promoting healthy lifestyle and income opportunity

                        The IA must follow the ethics provided by company in case of showing product results income
                        Disclaimer:
                        Achievement requires skills and years of hard work
                        Individual Income may vary from person to person.
                        Then he must provide GST bill in the name of your firm or NC and must take use copyright version for playing songs and music.
                        In case if IA doesn’t follow the rules, he is solely responsible for that.


                        The applicant hereby covenants that as under: * That he is clearly understood the marketing methods/plan, the compensation plan, its limitations and conditions. He agrees that he is not relying upon any misrepresentation or fraudulent inducement or assurance that is not set out in terms and conditions or other officially printed or published materials of the Company. *Relation between the Company and IA shall be governed, in addition to this agreement, by the rules and procedure mentioned in the marketing plan, available on website. The IA further confirms that he read and understood the terms and conditions carefully and agrees to be bound by them. *IA shall act as a freelancer and shall not commit any misfeasance to create any liability/ obligation on the Company. *It is made and understood in very clear terms that IA is not an Agent, Employee, nor an authorized representative of the Company or Its service providers. He is not authorized to receive or accept any amount/payment for and on behalf of the Company and any payment received by him will not be deemed to be received by the Company. The Company reserves its right to withheld /block/suspend the IA, If IA Takes the payment from his IA downlines, IA cross lines and his IA uplines. * IA, hereby declare that all the information furnished by him is true and correct. Company shall be at liberty to take any action against the IA in the event; it is discovered that the IA furnished any wrong/false information to the Company.
                        GENERAL TERMS: The Company may appoint any person for Collection/ Distribution services. IA is required to visit the Company's official website from time to time to get such appointment and avail facilities make payment and collect valid receipt and products from its outlet/collection centre. The IA will be eligible for income, as per the volume of sale of Products/services/Business done by him, subject to the eligibility norms formulated by the Company from time to time. The Company does not guarantee/assure any particular or income to the IA. Track ID has to be quoted by the IA for all his/her transactions and correspondence with the Company. The track ID once chosen cannot be altered at any point of time. No communication will be entertained without unique ID and password. IA shall preserve the ID and password properly as it is "must" for logging on to website.
                        Commission/income to the IA shall be subjected to statutory deductions as applicable. The Company reserves its right to withheld /block/suspend the IA in the event; the IA fails to provide any details as desired by the Company from time to time including but not limited to Pan Card Details.
                        IA undertakes to adhere for policies, procedures, rules and regulations formed by the Company.

                        The IA shall be faithful to the Company and shall uphold the integrity and decorum to the Company and shall maintain good relations with other IA and their clients.
                        Company reserves the rights to modify the terms and conditions, products, plan, business and policies at anytime without notice. Modification shall be published through the official website of the Company and such modifications/amendment shall be applicable and binding upon the IA from the date of such notification.
                        In case of death of IA either his nominee or one of the legal heirs with consent of all the legal heirs may join the Company as IA in place of the deceased provided, he applies in prescribed form and undertakes to abide all the rules and regulations, terms and conditions etc.in the same manner as that of the Original IA.
                        In case of failure to arrival at such consent within six months from the date of death of the IA, no one has the right to claim for the earnings generated on that I'd after six months. For this Period the Company will keep his ID in abeyance.
                        If any IA loses his contractual capacity due to any reason such as lunacy, bankruptcy or sentenced to imprisonment or any other legal embargo is created, his IA shall be continued through the person duly appointed by the competent Court.
                        IA shall have to follow all statutory laws, rules and regulations in operation of their HEMALIKA WELLNESS PRIVATE LIMITED business,
                        IA shall not engage in any deceptive of unlawful trade practice as defined Statue.
                        Some examples of unfair trade methods are: the false representation of a good or service; false free gift or prize offers; non-compliance with manufacturing standards; false advertising; or deceptive pricing or any medical claims.
                        If any provision of these Terms and Conditions is declared invalid or unenforceable, the remaining provisions shall remain in full force and effect. IA shall not manipulate the HEMALIKA WELLNESS PRIVATE LIMITED marketing plan or product's rate, VP, etc., in any way. IA shall not send, transmit or otherwise communicate any messages to anybody on behalf of the Company without any authority from the Company.
                        IA or any other person under him is strictly prohibited to use any promotional Material, other than the developed and authorised from the Company.
                        IA shall not use the HEMALIKA WELLNESS PRIVATE LIMITED trademark, logotype, and design anywhere without the written consent from the Company. Said permission can be withdrawn at any time by the Company
                        All the arrangements, expenses, permission from local authorities, complying with the rules of central and local body is the whole responsibility of IA for meetings and seminars conducted by IA.
                        IA cannot sell Products of Hemalika wellness Private limited on any e-commerce platforms If they do so, then Company can take action against him If Company gets any kind of evidence against IA.
                        Hemalika wellness Products are sold ONLY by its Company Website, it's Sales centre and Independent star associate and not by unauthorised sellers selling on any e-commerce platforms The Buyer or VC has a right to cross check IA identity card of HEMALIKA WELLNESS at the time of purchasing products from him/her HEMALIKA WELLNESS cannot give any assurance of quality of products sold through these E-commerce platforms For any assistance, please reach us on <a href="/cdn-cgi/l/email-protection" class="__cf_email__" data-cfemail="25565055554a575165485c4d404844494c4e440b464a48">[email&#160;protected]</a>
                                PROHIBITIONS:
                        IA shall not be engaged in any activities of Multilevel marketing of other Company/Person. If it is found then such IA shall be terminated.
                        IA is prohibited from listing, marketing, advertising, promoting, discussing, or selling any products, or the opportunity on any website or online forum that offers like auction as a mode of selling.
                        Username & password of an IA should not be shared to anyone except the id holder
                        Placing the order or taking Payment for order by upline should not be done.
                        IA hereby undertakes not to compel or induce or mislead any person with any false statements / promise to purchase products from the Company or to become IA of the Company.
                        In case IA misleads or induced anyone to join under him or takes anyone leads, whether cross line or downline,
                        And if the company finds IA is not following the terms and conditions
                        Then company has a right to block his id temporarily for first three months by sending mail to him
                        Then can do it permanently also
                        And hold his royalty income and bonus if bonus achiever for three months and permanent also

                        The IA must follow the ethics of the company for creating any IA under him
                        IA should also resolve the matter of -wrong id placed under sponsor or downline or cross line at the time of giving presentation to the new member
                        And it's completely and solely new person responsibility if the new person is joining under him or any other person

                        Hemalika wellness management is not responsible or will not get involved for any id which is wrongly placed by sponsor or downline or cross line
                        If any IA gives threatening messages to company through voice or text messages in WhatsApp or mail or any other social media for the wrong id placed or issue with sponsor or downline or any other personal reason
                        Then company has a right to terminate his/her id by sending a warning letter
                        And if this is repeated again then his/her id is permanently blocked and   his/her team transferred to sponsor or upline.
                        And cannot join HEMALIKA WELLNESS again in his/her lifetime.
                        New member has 100% right to choose his sponsor even if invited by anyone in the meeting physical or virtual

                        And in case if any person joins under any other person not the invitee, then its responsibility is totally of his/her sponsor and three uplines to make him agree to join under invitee or whether he is willing to work, Company is not involved in this matter.
                        Sponsor and his uplines and Tab team bonus achievers has right to transfer the id wrongly placed to the invitee if they are willing to do so in their organisation and the sponsor and uplines must be ready to support the IA even if the id is transferred
                        This will be accepted by company only If given in Notary and get it registered in registrar office by government-authorized public official who verifies the identity of the parties, witnesses their signatures, and authenticates the document with their official seal and signature. along with signatures of four level royalty receivers and bonus achievers and IA.
                        If any IA gives threatening messages to company through voice or text messages in WhatsApp or mail or any other social media for the wrong id placed or issue with sponsor or downline or any other personal reason

                        Then company has a right to terminate his/her id by sending a warning letter
                        And if this is repeated again then his/her id is permanently blocked and his/her team transferred to sponsor or upline.
                        And cannot join Hemalika wellness again in lifetime
                        CORPORATE EVENTS
                        If any IA doesn't behave properly in any corporate event or Hemalika distributor events like threatening women's or males too
                        Fighting with each other for any personal reason
                        Or using vulgar language in events or drunked while event is going on
                        All this cannot be entertained by corporate then company will get his /her terminate his/her id by sending a warning letter
                        And if this is repeated again then his/her id is permanently blocked and his/her team transferred to sponsor or upline.
                        Any IA who is joining the corporate events or training physically like Wellness academy, success mantra, spectacular event, leadership training or any upcoming events is coming with his own willingness
                        Note: - if any IA is having any health issues or any severe health disorders or diseases
                        They are solely responsible for joining the corporate events
                        Any IA who is diabetic or having any health issues
                        And should have meal on time then he must bring or arrange something for himself till the lunch or any meal break provided by corporate.
                        Children are not allowed in this corporate events below 17 years and IA is solely responsible for anything unusual happened to children.
                        Event ticket is non refundable
                        Any corporate event is just a distributor run event to provide knowledge and training to the distributors regarding Product and Plan.
                            </p>
                        </div>



                        <div class="section">
                            <h6>ORDER CANCELLATION </h6>
                            <p>
                                Order can be cancelled only if mail received through registered email id with valid reason. And company may confirm the same with the id holder
                        If the IA accepts this
                        Then he cannot receive the income and level qualification cannot be completed and this is sole responsibility of the id holder
                        Note: - Order can be cancelled by IA one or two times in case of emergency
                        And if this exceeds more than thrice then his id will be blocked permanently by company.

                        COOLING OFF PERIOD: Any IA of the Company can cancel participation in the company within 30 days of free registration in Company. IA will receive full refund for any goods or services purchased if any.
                        BUY BACK POLICY: Any IA can return the products 30days from date of invoice from the company RETURN POLICY: any IA of the company can return all the products not used; the product should be in saleable condition.
                        DUTY AND CONFIDENTIALITY: Parties shall keep and maintain secrecy and confidentiality about the information for which they are obliged and expected to keep secret and not to disclose anybody other than the persons to whom is reasonably expected to be disclosed.
                        SPECIAL CONDITIONS: Notwithstanding anything stated or provided herein, the Company shall have all powers and discretion to modify, alter or vary the terms and conditions in any manner it deems fit and shall be communicated through official website or other mode as the Company deems fit and proper. If any IA does not agree to such amendment, he may terminate his agreement within 45 days of such publication by giving a written consent to the Company. Without any objection to such modifications/ alterations, if IA continues his activities, it will be deemed that he had accepted all modifications and amendments in the terms and conditions for future.
                        IA has to work with his Sponsor only and he does not have any right to change the Sponsor or can go for cross sponsoring or create another ID in the name of other family members with some other IA (cross lines), if done so company has a right to shift his ID to his original one with his team and his incomes also

                        Golden supervisor and above ROYALTY & BONUS CONDITIONS -
                        Royalty income & Bonus will be given to those who are attached with the system
                        System means should be in touch with downline, sponsor and 5 level uplines and GET TEAM & above He/she must attend All the trainings both online ZOOM meetings and offline trainings in their sponsor clubs or meetings or any events organised by uplines or sponsor or corporate.
                        Mail from sponsor and GET TEAM & Above must be received in between 1st to 15th of any month then action will be taken in between 20th to 30th of the present month and his income will be stopped next month and the amount is holded in the company only
                        To release royalty also then the mail should be rec'd on the same dates Must attend All training online on zoom or other e- platform daily and monthly meetings offline meetings which is run by the uplines and company corporate events
                        If the distributor is not participating in all this for 3-5 months then the sponsor or his upline can take action to hold his royalty income & above from 1-2 months through mail to company A distributor can do the retails and get wholesale income If the distributor started attending the events and get in touch with his downlines and sponsor and above then he will start getting his royalty income He/she must attend All the trainings both online ZOOM meetings and offline  for 20 days or more and it was monitored by his sponsor or upline and mail to company about this not to stop royalty from now onwards
                        Note:-if id holder placed order in his id before 10th of a month, then his order cannot be cancelled and income will be given to him and can be holded from next month if mail again received by the sponsor or uplines.
                        Previous Royalty income & bonus which is holded can not be released to him as it got rollup to his uplines To get the royalty & bonus you need to follow all this :- If you have some work or an emergency like some health related issues, any uncertain death ,exams ,marriage of himself/herself or any close family members then you need to inform to your sponsor or upline regarding this for a period of 3 months In this case, royalty income & bonus will also be issued. THE REASON FOR DOING THIS IS TO AVOID DUPLICATION OF THE SAME THING - TAKING ROYALTY & ABOVE WITHOUT ATTENDING MEETINGS AND IN TOUCH WITH THEIR TEAM TERMINATION: where a direct seller is found to have made no sales of goods and services for a period of up to two years since the contract was entered into, or since the date of the last sale made by the direct seller. Intimation letter may be given to the IA In Physical or electronic mail. IA can reply to the Notice within 7 days of the receipt of Notice. FORCE MAJEURE: Company shall not be liable for any failure to perform it's obligations where such failure had resulted due to Acts or Nature (including fire, flood, earthquake, storm, hurricane or other natural disaster),war, invasion, act of foreign enemies, hostilities (whether war is declared or not),civil war, rebellion, revolution, insurrection or failure of electricity ,any type of redirection by Govt. (Central or State),Local authorities etc., RESIGNATION *If the IA wants to resign or not willing to work with his existing sponsor He must get the details about resignation or sponsor changing ,mail should be send to company on support Myhemalika.com or contact on 92512-73202 to get the details If the IA submits the mail or give written consent to company ,his request would be accepted and within 45 working days from the date of submission And if the reason is valid from his behalf after verification He has a right to change the Sponsor after the completion of period of inactivity i.e. Two years *If the IA is in INACTIVITY PERIOD, then he doesn't have any right to Attend meetings or any corporate events, Trainings, Nutrition Hub and others. *If the IA or the IA's Spouse wanted to join under another Sponsor as a VC or IA, they must follow the period of inactivity (waiting period) before signing a new agreement. If they restart during this period then they have to restart the new date will be as such the new activity taken place. *IA Can submit as an Independent Star associate or VIP Customers agreement under new sponsor after 2years consecutive days of no activity *If the IA or IA Spouse enrol as an IA under another Sponsor without complying with the period of inactivity, Hemalika wellness may terminate the membership place it under original sponsor or can take any action. *If the IA or IA Spouse is under the period of activity and if he is willing to take products Then he can take from his Sponsor or Uplines with their permission. *If the agreement is terminated, the IA and their spouse can apply for an IA under a different sponsor only after a lapse of the Period of Inactivity (2years) RECOURSE AND LEGAL APPLICABILITY: The terms and conditions stipulated in the forgoing paragraphs shall be governed in accordance with the law in force of India. Disputes, either civil or criminal in natural, shall be subject to the exclusive jurisdiction of the courts in Telangana only and nowhere else. If any dispute or difference arises out if it in relation to these presents, the same shall be referred to sole arbitrator appointed by the Company. IA shall not raise any objection, in case the Arbitrator so appointed in any manner whatsoever. Arbitration shall be conducted as per "Arbitration and conciliation Act,1996" as amended from time to time. venue of such Arbitration shall be in Rajasthan and the language shall be English. HEMALIKA WELLNESS PRIVATE LIMITED liability, whether in contract, tort or otherwise arising out of or in connection with the agreement and or relationship arising there from shall not exceed actual damages or loss assessed by the Arbitrator or any other dispute resolution mechanism adopted by the parties or solemnly affirm and declare as follows: That I have read and understood the terms and conditions of IA of the Company. I have also gone through the Company's official website, printed materials, broachers and convinced about the business and I have applied to appoint me as a IA on my own violation. I declare that I have not been given any assurance or promise or inducement by the Company or its Directors or any of the management or my sponsor in regard to any fixed income incentive, prize or benefits on account of the products purchased by me. I have clearly understood that eligibility of income exclusively depends on my performance in business volumes as from terms and conditions. I further agree that company reserves the right to change the Business plan at any point of time without prior notice. I
                        unique code allotted to each IA to allow them to log on to the website of the company. Website: Means official website of the company i.e. www.myhemalika.com
                        <b>THE APPOINTMENT AND UNDERSTANDING:</b> The Company upon Verification and scrutiny may register the applicant as "IA" for selling the goods/services/products of the company. The Company shall be at liberty to accept or reject the application at its discretion. The IA shall enjoy the following privileges: - 1. Incentive for effecting sale of goods/Products of the Company as per Marketing plan. 2.No territorial restriction to sale the goods/products/services. 3.Search and inspect his account on website of the Company through password awarded by the Company. 4. Earnings of the IA shall be in proportion to the volume of performance by the IA either by his personal efforts or through team as stipulated in the Company. An individual, upon appending his signature at the bottom of these presents, shall be deemed to have accepted the terms and conditions stipulated herein. Upon registration after scrutiny of the application, he shall become the IA of the company, Allotment of password and ID shall be construed as registered as IA
                        Once the username & password is received, the IA must immediately change the password within 24 hours and should not be shared to anyone.
                        For Placing order and to check any information he should take proper training from his Sponsor.
                        Payment should not be given for Placing order to sponsor or upline
                        If does so, IA is solely responsible if anything happens.
                        If the product order delivery address is provided of Sponsor or any other person then IA is solely responsible if anything happens.
                        And IA must check the products as soon as received and if any damage or product not in proper condition then it should be informed to company within 4 hours of receiving products. If does so it will be solely IA Responsibility.

                        FSSAI FOOD REGISTRATION IS MANDATORY (IF IA WANTS TO DO BUSINESS) FOR PLACING ORDER. If not applied IA is solely responsible if anything happens.
                        If IA is operating a office/Nutrition Club and selling HEMALIKA WELLNESS products and promoting business plan, he cannot use logo of company in board, or use product picture on board, and logo of company with part time or full-time business opportunity
                        In case, he does so, warning letter will be sent by company to IA.
                        Promotion Material like Boards, flyers etc., should be approved by Company before using.
                        Before After pics of anyone cannot be used without their permission.
                        Any kind of activities done by IA in his Nutrition Club, he is solely responsible for that.

                        If the IA is conducting any events or training in conference halls or banquet halls then he must take GST number if taking ticket charges from distributors or your team members
                        THE COMPANY Is not encouraging their IAs to promote events for making money,
                        The events done by them are for promoting healthy lifestyle and income opportunity

                        The IA must follow the ethics provided by company in case of showing product results income

                            </p>
                        </div>

                        <div class="section">
                            <h6>Disclaimer:</h6>
                            <p>
                                Achievement requires skills and years of hard work
                        Individual Income may vary from person to person.
                        Then he must provide GST bill in the name of your firm or NC and must take use copyright version for playing songs and music.
                        In case if IA doesn’t follow the rules, he is solely responsible for that.


                        The applicant hereby covenants that as under: * That he is clearly understood the marketing methods/plan, the compensation plan, its limitations and conditions. He agrees that he is not relying upon any misrepresentation or fraudulent inducement or assurance that is not set out in terms and conditions or other officially printed or published materials of the Company. *Relation between the Company and IA shall be governed, in addition to this agreement, by the rules and procedure mentioned in the marketing plan, available on website. The IA further confirms that he read and understood the terms and conditions carefully and agrees to be bound by them. *IA shall act as a freelancer and shall not commit any misfeasance to create any liability/ obligation on the Company. *It is made and understood in very clear terms that IA is not an Agent, Employee, nor an authorized representative of the Company or Its service providers. He is not authorized to receive or accept any amount/payment for and on behalf of the Company and any payment received by him will not be deemed to be received by the Company. The Company reserves its right to withheld /block/suspend the IA, If IA Takes the payment from his IA downlines, IA cross lines and his IA uplines. * IA, hereby declare that all the information furnished by him is true and correct. Company shall be at liberty to take any action against the IA in the event; it is discovered that the IA furnished any wrong/false information to the Company.
                        Some examples of unfair trade methods are: the false representation of a good or service; false free gift or prize offers; non-compliance with manufacturing standards; false advertising; or deceptive pricing or any medical claims.
                        If any provision of these Terms and Conditions is declared invalid or unenforceable, the remaining provisions shall remain in full force and effect. IA shall not manipulate the HEMALIKA WELLNESS PRIVATE LIMITED marketing plan or product's rate, VP, etc., in any way. IA shall not send, transmit or otherwise communicate any messages to anybody on behalf of the Company without any authority from the Company.
                        IA or any other person under him is strictly prohibited to use any promotional Material, other than the developed and authorised from the Company.
                        IA shall not use the HEMALIKA WELLNESS PRIVATE LIMITED trademark, logotype, and design anywhere without the written consent from the Company. Said permission can be withdrawn at any time by the Company
                        All the arrangements, expenses, permission from local authorities, complying with the rules of central and local body is the whole responsibility of IA for meetings and seminars conducted by IA.
                        IA cannot sell Products of Hemalika wellness Private limited on any e-commerce platforms If they do so, then Company can take action against him If Company gets any kind of evidence against IA.
                        Hemalika wellness Products are sold ONLY by its Company Website, it's Sales centre and Independent star associate and not by unauthorised sellers selling on any e-commerce platforms The Buyer or VC has a right to cross check IA identity card of HEMALIKA WELLNESS at the time of purchasing products from him/her HEMALIKA WELLNESS cannot give any assurance of quality of products sold through these E-commerce platforms For any assistance, please reach us on <a href="/cdn-cgi/l/email-protection" class="__cf_email__" data-cfemail="285b5d5858475a5c684551404d454944414349064b4745">[email&#160;protected]</a>
                            </p>
                        </div>



                        <div class="section">
                            <h6>PROHIBITIONS: </h6>
                            <p>
                                IA shall not be engaged in any activities of Multilevel marketing of other Company/Person. If it is found then such IA shall be terminated.
                        IA is prohibited from listing, marketing, advertising, promoting, discussing, or selling any products, or the opportunity on any website or online forum that offers like auction as a mode of selling.
                        Username & password of an IA should not be shared to anyone except the id holder
                        Placing the order or taking Payment for order by upline should not be done.
                        IA hereby undertakes not to compel or induce or mislead any person with any false statements / promise to purchase products from the Company or to become IA of the Company.
                        Sponsor and his uplines and Tab team bonus achievers has right to transfer the id wrongly placed to the invitee if they are willing to do so in their organisation and the sponsor and uplines must be ready to support the IA even if the id is transferred
                        This will be accepted by company only If given in Notary and get it registered in registrar office by government-authorized public official who verifies the identity of the parties, witnesses their signatures, and authenticates the document with their official seal and signature. along with signatures of four level royalty receivers and bonus achievers and IA.
                        If any IA gives threatening messages to company through voice or text messages in WhatsApp or mail or any other social media for the wrong id placed or issue with sponsor or downline or any other personal reason

                        Then company has a right to terminate his/her id by sending a warning letter
                        And if this is repeated again then his/her id is permanently blocked and his/her team transferred to sponsor or upline.
                        And cannot join Hemalika wellness again in lifetime

                            </p>
                        </div>

                        <div class="section">
                            <h6>Golden supervisor and above ROYALTY & BONUS CONDITIONS -</h6>
                            <p>
                                Royalty income & Bonus will be given to those who are attached with the system
                        System means should be in touch with downline, sponsor and 5 level uplines and GET TEAM & above He/she must attend All the trainings both online ZOOM meetings and offline trainings in their sponsor clubs or meetings or any events organised by uplines or sponsor or corporate.
                        Mail from sponsor and GET TEAM & Above must be received in between 1st to 15th of any month then action will be taken in between 20th to 30th of the present month and his income will be stopped next month and the amount is holded in the company only
                        To release royalty also then the mail should be rec'd on the same dates Must attend All training online on zoom or other e- platform daily and monthly meetings offline meetings which is run by the uplines and company corporate events
                        If the distributor is not participating in all this for 3-5 months then the sponsor or his upline can take action to hold his royalty income & above from 1-2 months through mail to company A distributor can do the retails and get wholesale income If the distributor started attending the events and get in touch with his downlines and sponsor and above then he will start getting his royalty income He/she must attend All the trainings both online ZOOM meetings and offline  for 20 days or more and it was monitored by his sponsor or upline and mail to company about this not to stop royalty from now onwards
                        Note:-if id holder placed order in his id before 10th of a month, then his order cannot be cancelled and income will be given to him and can be holded from next month if mail again received by the sponsor or uplines.
                        Previous Royalty income & bonus which is holded can not be released to him as it got rollup to his uplines To get the royalty & bonus you need to follow all this :- If you have some work or an emergency like some health related issues, any uncertain death ,exams ,marriage of himself/herself or any close family members then you need to inform to your sponsor or upline regarding this for a period of 3 months In this case, royalty income & bonus will also be issued. THE REASON FOR DOING THIS IS TO AVOID DUPLICATION OF THE SAME THING - TAKING ROYALTY & ABOVE WITHOUT ATTENDING MEETINGS AND IN TOUCH WITH THEIR TEAM TERMINATION: where a direct seller is found to have made no sales of goods and services for a period of up to two years since the contract was entered into, or since the date of the last sale made by the direct seller. Intimation letter may be given to the IA In Physical or electronic mail. IA can reply to the Notice within 7 days of the receipt of Notice. FORCE MAJEURE: Company shall not be liable for any failure to perform it's obligations where such failure had resulted due to Acts or Nature (including fire, flood, earthquake, storm, hurricane
                        undertake not to misguide or induce dishonestly anybody to join the Company. I hereby agree and adhere to the terms and conditions as stipulated along with the agreement form and as mentioned above to agree to purchase the product as Consumer/ to do the IA activities. I hereby agree to submit all disputes to arbitration as provided in the terms and conditions of the company. I also declare that at present any other IA ship IDENTITY (ID) is not activated in my name. I have read and agreed to the terms and conditions and Privacy policy for HEMALIKA
                            </p>

                        </div>
                        <%--  <div class="col-12 text-center my-3">
                        <a class="btn btn-warning" href="/website-assets/agreement-form.docx" download="agreement-24-sep.docx">Download
                            Agreement</a>
                    </div>--%>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>

