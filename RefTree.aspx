<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="Reftree.aspx.cs" Inherits="Reftree" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>My Direct Tree
                </h6>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">

                        <div class="clearfix gen-profile-box" style="min-height: auto;">
                            <div class="profile-bar clearfix" style="background: #fff;">
                                <div class="clearfix">
                                    <br>
                                    <div class="centered">
                                        <div class="clr">
                                            <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                        </div>
                                        <div class="col-md-12">
                                            <table class="table-responsive" style="display: none">
                                                <tr>
                                                    <td width="30%">Downline Member ID
                                                    </td>
                                                    <tr>
                                                        <td width="20%">
                                                            <input class="input-xxlarge" id="DownLineFormNo" style="width: 250px;" name="DownLineFormNo"
                                                                runat="server" type="text" maxlength="15" />
                                                        </td>
                                                        <tr>
                                                            <td width="30%">
                                                                <asp:Button ID="Button1" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="Button1_Click" />
                                                                <asp:Button ID="cmdBack" runat="server" Text="Back" class="btn btn-primary" OnClick="cmdBack_Click" />
                                                            </td>
                                                            <tr>
                                                                <td width="30%">
                                                                    <label id="Label1" class="control-label col-sm-2" runat="server" visible="false">
                                                                        Down Level</label>
                                                                    <input class="input-xxlarge" id="deptlevel" runat="server" visible="false" type="text"
                                                                        maxlength="4" name="deptlevel" />
                                                                </td>
                                                            </tr>
                                            </table>
                                        </div>
                                        <div class="col-sm-12 pull-none" style="position: inherit">
                                            <div class="form-group ">
                                                <iframe name="TreeFrame" frameborder="0" scrolling="auto" src="Referaltree.aspx"
                                                    width="100%" height="430" id="TreeFrame" runat="server"></iframe>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>    
</asp:Content>
