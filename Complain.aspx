<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="Complain.aspx.cs" Inherits="Complain" %>

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
                <%--      <li class="breadcrumb-item">
            <h4 class="page-title m-b-0">Dashboard</h4>
        </li>
        <li class="breadcrumb-item">
            <a href="#"><i data-feather="home"></i></a>
        </li>
        <li class="breadcrumb-item active">Dashboard</li>--%>
            </ul>
            <div class="row">
                <div class="col-12 col-sm-12 col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Raise Ticket</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                                    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    <div id="DivError" runat="server" visible="false">
                                        <span id="spanError" runat="server"></span>
                                    </div>
                                    <h6>
                                        <asp:Label ID="lbludaan" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblother" runat="server" Text="" Visible="false"></asp:Label>
                                        <%--Complaint--%>
                                    </h6>
                                </div>
                                <div class="row" id="divSuccess" runat="server">
                                    <div class="col-sm-12">
                                        <asp:Label ID="LblCompalin" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="Lblgroup" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <asp:HiddenField ID="hdnSessn" runat="server" />
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label for="inputdefault">
                                                <% if (Session["CompID"].ToString() == "1091")
                                                    { %>
    Distributor ID :
                                                <% }
                                                    else
                                                    { %>
    Direct Seller ID :
                                                <% } %> <span class="red">*</span></label>
                                            <asp:TextBox ID="TxtDirectSeller" Enabled="false" runat="server" CssClass="form-control"
                                                PlaceHolder="Direct Seller Id" AutoPostBack="true" OnTextChanged="TxtDirectSeller_TextChanged"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputdefault">
                                                Name<span class="red">*</span></label>
                                            <asp:TextBox ID="TxtName" Enabled="false" runat="server" CssClass="form-control" PlaceHolder="Name"
                                                ValidationGroup="Save"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputdefault">
                                                Mobile No.<span class="red">*</span></label>
                                            <asp:TextBox ID="TxtMobl" Enabled="false" runat="server" CssClass="form-control" PlaceHolder="Mobile No"
                                                ValidationGroup="Save"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputdefault">
                                                Email<span class="red">*</span></label>
                                            <asp:TextBox ID="TxtEmail" Enabled="false" runat="server" CssClass="form-control" PlaceHolder="Email Id"
                                                ValidationGroup="Save"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputdefault">
                                                Nature of Grievance:<span class="red">*</span></label>
                                            <asp:DropDownList ID="CmbCmplntType" runat="server" placeholder="Nature of Grievance "
                                                CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputdefault">
                                                Subject<span class="red">*</span></label>
                                            <asp:TextBox ID="TxtSubject" runat="server" CssClass="form-control validate[required]" PlaceHolder="Subject"
                                                ValidationGroup="Save"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="inputdefault">
                                                Description<span class="red">*</span></label>
                                            <asp:TextBox ID="TxtDesc" runat="server" TextMode="MultiLine" CssClass="form-control validate[required]"
                                                placeholder="Description" ValidationGroup="Save" onkeyup="CountChar();" MaxLength="500"></asp:TextBox>
                                        </div>
                                        <div class="form-group" id="Iddiscountmart" visible="false" runat="server">
                                            <label for="inputdefault">
                                                Complain Proof Upload<span class="red">*</span></label>
                                            <asp:FileUpload ID="Fuidentity" runat="server" CssClass="form-control" />
                                            <asp:Label ID="lblimage" runat="server" Visible="false"></asp:Label>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="BtnSubMit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Save" OnClick="BtnSubMit_Click" />
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
</asp:Content>

