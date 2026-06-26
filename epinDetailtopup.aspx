<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="epinDetailtopup.aspx.cs" Inherits="epinDetailtopup" %>


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
                            <h5 class="mb-0">Epin Detail</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="clr">
                                </div>
                                <div class="form-horizontal">
                                    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputdefault">Select Status</label>
                                                <asp:DropDownList ID="rbtnStatus" runat="server" class="form-control" RepeatDirection="Horizontal"
                                                     Font-Bold="True">
                                                    <asp:ListItem>BOTH</asp:ListItem>
                                                    <asp:ListItem>USED</asp:ListItem>
                                                    <asp:ListItem Selected="True">UN-USED</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-4" style="display: none;">
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    Package Wise Detail</label>
                                                <asp:DropDownList ID="CmbKit" class="form-control" runat="server">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-2" style="padding: 1%;">
                                            <div class="form-group">
                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" OnClick="btnSubmit_Click" />
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                        </div>

                                        <div id="DivTopup" runat="server" class="col-md-4 table-responsive" visible="false">
                                            <table align="center" border="1px" class="table table-striped table-bordered">
                                                <tr>
                                                    <td align="center" colspan="2"></td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <strong>IDNo</strong>*
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="TxtIDNo" runat="server" AutoPostBack="true" OnTextChanged="TxtIDNo_TextChanged"></asp:TextBox>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <strong>ScratchNo</strong>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblPinNo" runat="server" Visible="false"></asp:Label>
                                                        <asp:TextBox ID="TxtScratchNo" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <div id="tdleg1" runat="server" visible="false">
                                                    <tr>
                                                        <td align="left">
                                                            <strong>Leg</strong>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="RbtLeg" runat="server">
                                                                <asp:ListItem Text="Group A" Value="1" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Group B" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                </div>
                                                <tr id="trcommand" runat="server">
                                                    <td style="height: 26px"></td>
                                                    <td style="height: 26px" align="left">
                                                        <asp:Button ID="btnTopup" runat="server" Text="Topup" class="btn btn-primary" OnClick="btnTopup_Click" />
                                                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancel_Click" />
                                                        &nbsp;
                                                    </td>
                                                </tr>

                                                <asp:Label ID="Label1" Text="Member Name:" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                                <asp:Label ID="LblSponserN" Text="Sponsor Id:" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblnetamount" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lbllastkitid" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblSponsor" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblformno" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblkitid" runat="server" Visible="false"></asp:Label>

                                                <tr>
                                                    <td colspan="2">
                                                        <div id="divconfirm" runat="server" class="table-responsive">
                                                            <table cellspacing="2" cellpadding="2" width="100%" border="1px" style="background-color: #5AA9CA; color: #fff;">
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <strong>CONFIRM TOPUP</strong>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <strong>IDNo</strong>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TxtIDNo1" runat="server" ReadOnly="True" ForeColor="Black"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <strong>Name</strong>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TxtName" runat="server" ReadOnly="True" ForeColor="Black"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <strong>Topup By</strong>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TxtPackage" runat="server" ReadOnly="True" ForeColor="Black"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="TrDilivery" runat="server" visible="false">
                                                                        <td align="left">
                                                                            <strong>Delivery</strong>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:RadioButtonList ID="RbtDelivery" runat="server" AutoPostBack="True" RepeatColumns="3"
                                                                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                                <asp:ListItem Text="By Courier" Value="C" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Text="By SpeedPost" Value="S"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trDeliveryCenter" runat="server" visible="false">
                                                                        <td align="left">
                                                                            <strong>Delivery Center</strong>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList ID="DDlDeliveryCenter" runat="server" Style="font: 14px 'Open Sans', sans-serif; font-weight: normal; font-style: normal; line-height: 23px; color: #727272; width: 160px;">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trDeliveryAddress" runat="server" visible="false">
                                                                        <td align="left">
                                                                            <strong>Delivery Address</strong>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="TxtDeliveryAddress" runat="server" CssClass="inputbox_long"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="bottom" align="left">&nbsp;
                                                                        </td>
                                                                        <td valign="middle" align="left">
                                                                            <asp:Button ID="BtnConfirm" runat="server" Text="Confirm" class="btn btn-primary" OnClick="BtnConfirm_Click" />
                                                                            &nbsp;<asp:Button ID="Button1" runat="server" Text="Cancel" class="btn btn-primary" OnClick="Button1_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="col-md-4">
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:DataGrid ID="DgPayment" runat="server" CssClass="table table-striped table-bordered"
                                                AutoGenerateColumns="False" AllowPaging="True" PageSize="30" PagerStyle-Mode="NumericPages" OnItemCommand="DgPayment_ItemCommand" OnItemDataBound="DgPayment_ItemDataBound">
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="sno">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="PinNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCardNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CardNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="ScratchNo" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScratchNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ScratchNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="ProductName" HeaderText="Product Name"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="IssuedDate" HeaderText="Issue Date"></asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="epinStatus" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="UsedBy" HeaderText="Used By"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="MemName" HeaderText="Name"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="UsedDate" HeaderText="Used Date"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Status" HeaderText="Use Type"></asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="IsTopup" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="IsTopup" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IsTopup") %>'></asp:Label>
                                                            <asp:Label ID="lblKitID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "KitID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnRegister" runat="server" Text="Join Now" CommandArgument="Join"
                                                                Visible="false" />
                                                            <asp:Button ID="btnTopup" runat="server" Text="Topup" CommandArgument="Topup" Visible="true" class="btn btn-primary" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                            </asp:DataGrid>

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
