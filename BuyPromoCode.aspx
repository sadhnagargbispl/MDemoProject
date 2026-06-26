<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="BuyPromoCode.aspx.cs" Inherits="BuyPromoCode" %>

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

            .buttoncss {
                background-color: #F68D1F !important;
                border-color: #F68D1F !important;
            }
        </style>

        <section class="section">
            <ul class="breadcrumb breadcrumb-style ">
            </ul>
            <div class="row">
                <div class="col-12 col-sm-12 col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">Buy PromoCode</h5>
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
                                            Available Balance <span class="red">*</span>
                                            <asp:TextBox ID="TxtCredit" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        </div>
                                        <div class="form-group" style="display: none;">
                                            Select <span class="red">*</span>
                                            <asp:DropDownList CssClass="form-control" ID="DDlCode" runat="server" TabIndex="7">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group ">
                                            Amount <span class="red">*</span>
                                            <asp:TextBox ID="TxtAmount" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                                        </div>
                                        <div class="form-group ">
                                            Qty <span class="red">*</span>
                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                            <asp:TextBox ID="Txtqty" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                runat="server" TabIndex="3" ValidationGroup="eInformation" OnTextChanged="Txtqty_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <div class="clr">
                                                <asp:Label ID="errMsg" runat="server" CssClass="error" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group ">
                                            Final Amount <span class="red">*</span>
                                            <asp:TextBox ID="TxtFinalAmount" runat="server" TabIndex="8" CssClass="form-control" Enabled="False"></asp:TextBox>
                                        </div>
                                        <div class="clr">
                                            <asp:Label ID="Label1" runat="server" CssClass="error" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="CmdSave" runat="server" Text="Buy PromoCode" CssClass="btn btn-primary buttoncss text-white w-100"
                                                TabIndex="27" ValidationGroup="eInformation" OnClick="CmdSave_Click" />
                                            <asp:Label ID="Label3" runat="server" CssClass="error" ForeColor="Red" Font-Bold="true" Font-Size="Small">Minimum purchase requirement: 6 coupons.</asp:Label>
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

