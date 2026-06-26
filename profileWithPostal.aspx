<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="profileWithPostal.aspx.cs" Inherits="profileWithPostal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

    </script>
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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Profile</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <%-- <h4>
                                    Your Placement Detail
                                </h4>--%>
                                    <div class="form-group ">
                                        <label class="control-label col-sm-2" style="display: none;">
                                            Sponsor ID<span class="red">*</span></label>
                                        <asp:TextBox ID="txtReferalId" CssClass="form-control" runat="server" AutoPostBack="True"
                                            Enabled="False"></asp:TextBox>
                                    </div>
                                    <div class="form-group " id="DivSponsorName" runat="server" visible="false">
                                        <label class="control-label col-sm-3">
                                            Sponsor Name<span class="red">*</span></label>
                                        <asp:TextBox ID="TxtReferalNm" class="form-control" runat="server" Enabled="False"></asp:TextBox>
                                    </div>
                                    <div class="form-group " id="DivUplinerId" runat="server" visible="false">
                                        <label class="control-label col-sm-3">
                                            Placement ID<span class="red">*</span></label>
                                        <asp:TextBox ID="TxtUplinerid" class="form-control" runat="server" AutoPostBack="True"
                                            Enabled="False"></asp:TextBox>
                                    </div>
                                    <div class="form-group " id="DivUplinerName" runat="server" visible="false">
                                        <label class="control-label col-sm-3">
                                            Placement Name<span class="red">*</span></label>
                                        <asp:TextBox ID="TxtUplinerName" class="form-control" runat="server" Enabled="False"></asp:TextBox>
                                    </div>
                                    <div class="form-group greybt" style="display: none;">
                                        <label class="control-label col-sm-2">
                                            Position<span class="red">*</span></label>
                                        <asp:TextBox ID="lblPosition" class="form-control" runat="server" Enabled="false"></asp:TextBox>
                                    </div>
                                    <h4>Personal Detail
                                    </h4>
                                    <%-- Registration Type Section (Read-Only Display with Radio look) --%>
                                    <%-- Registration Type Section --%>
                                    <div id="Div1" runat="server" visible="false">
                                        <%-- <h4>Registration Detail</h4>--%>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3">Registration As</label>
                                            <asp:TextBox ID="txtRegAs" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group" id="Div2" runat="server" visible="false">
                                            <label class="control-label col-sm-3">Registration Type</label>
                                            <asp:TextBox ID="txtRegType" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div id="DivRegType" runat="server" visible="false">
                                        <%-- <h4>Registration Detail</h4>--%>
                                        <div class="form-group" id="DivRegistration" runat="server" visible="false">
                                            <label class="control-label col-sm-3">Registration As</label>
                                            <asp:RadioButtonList ID="RbCategory" runat="server"
                                                RepeatDirection="Horizontal"
                                                CssClass="form-control"
                                                Enabled="false">
                                                <asp:ListItem Text="Individual" Value="IN"></asp:ListItem>
                                                <asp:ListItem Text="Company" Value="C"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="form-group" id="DivSubCategory" runat="server" visible="false">
                                            <label class="control-label col-sm-3">Registration Type</label>
                                            <asp:RadioButtonList ID="CbSubCategory" runat="server"
                                                RepeatDirection="Horizontal"
                                                Enabled="false">
                                                <asp:ListItem Text="ProprietorShip" Value="SP"></asp:ListItem>
                                                <asp:ListItem Text="Partnership Firm" Value="PF"></asp:ListItem>
                                                <asp:ListItem Text="Private Limited Company" Value="PL"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>

                                    <div class="form-group" id="DivYourName" runat="server">
                                        <label class="control-label col-sm-3">
                                            Your Name<span class="red">*</span></label>
                                        <div class="row">
                                            <div class="col-sm-2">
                                                <asp:DropDownList CssClass="form-control" ID="ddlPreFix" runat="server">
                                                    <asp:ListItem Value="Mr." Text="Mr."></asp:ListItem>
                                                    <asp:ListItem Value="Mrs." Text="Mrs."></asp:ListItem>
                                                    <asp:ListItem Value="Miss" Text="Miss"></asp:ListItem>
                                                    <asp:ListItem Value="M/S." Text="M/S."></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtFrstNm" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- Firm Name (only for RegType = C) --%>
                                    <div class="form-group" id="DivFirmName" runat="server" visible="false">
                                        <label class="control-label col-sm-3">
                                            Firm Name<span class="red">*</span></label>
                                        <asp:TextBox ID="txtFirmName" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:HiddenField ID="hdnSessn" runat="server" />
                                    <%--   <div class="form-group ">
                                        <label class="control-label col-sm-3">
                                            Your Name<span class="red">*</span></label>
                                        
                                        <div class="row">
                                            <div class="col-sm-2">
                                                <asp:DropDownList CssClass="form-control" ID="ddlPreFix" runat="server">
                                                    <asp:ListItem Value="Mr." Text="Mr."></asp:ListItem>
                                                    <asp:ListItem Value="Mrs." Text="Mrs."></asp:ListItem>
                                                    <asp:ListItem Value="Miss" Text="Miss"></asp:ListItem>
                                                    <asp:ListItem Value="M/S." Text="M/S."></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtFrstNm" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                    runat="server" ValidationGroup="eInformation" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%-- Father's Name / Authorized Person Name --%>
                                    <div class="form-group" id="DivFatherName" runat="server">
                                        <label class="control-label col-sm-3" id="lblFatherLabel" runat="server">Father's Name</label>
                                        <div class="row">
                                            <div class="col-sm-2">
                                                <asp:DropDownList CssClass="form-control" ID="CmbType" runat="server">
                                                    <asp:ListItem Value="S/O" Text="S/O"></asp:ListItem>
                                                    <asp:ListItem Value="D/O" Text="D/O"></asp:ListItem>
                                                    <asp:ListItem Value="W/O" Text="W/O"></asp:ListItem>
                                                    <asp:ListItem Value="C/O" Text="C/O"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-10" style="padding-left: 0px;">
                                                <asp:TextBox ID="txtFNm" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- Authorized Person Name (only for RegType = C) --%>
                                    <div class="form-group" id="DivAuthPerson" runat="server" visible="false">
                                        <label class="control-label col-sm-3">
                                            Authorized Person Name<span class="red">*</span></label>
                                        <asp:TextBox ID="txtAuthPerson" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <%--   <div class="form-group ">
                                        <label class="control-label col-sm-3">
                                            Father's Name</label>
                                        <div class="row">
                                            <div class="col-sm-2">
                                                <asp:DropDownList CssClass="form-control" ID="CmbType" runat="server">
                                                    <asp:ListItem Value="S/O" Text="S/O"></asp:ListItem>
                                                    <asp:ListItem Value="D/O" Text="D/O"></asp:ListItem>
                                                    <asp:ListItem Value="W/O" Text="W/O"></asp:ListItem>
                                                    <asp:ListItem Value="C/O" Text="C/O"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-10" style="padding-left: 0px;">
                                                <asp:TextBox ID="txtFNm" runat="server" CssClass="form-control" ValidationGroup="eInformation"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <div class="form-group">
                                        <label class="control-label col-sm-2">
                                            Date of Birth<span class="red">*</span></label>
                                        <asp:TextBox ID="TxtDobDate" CssClass="validate[required] form-control" runat="server"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtDobDate"
                                            Format="dd-MM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <%-- <div class="col-sm-2  p0 pl10">
                                            <asp:DropDownList ID="ddlDOBdt" runat="server" CssClass="form-control" TabIndex="9">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-4  p0 pl10">
                                            <asp:DropDownList ID="ddlDOBmnth" runat="server" CssClass="form-control" TabIndex="10">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-sm-4  p0 pl10">
                                            <asp:DropDownList ID="ddlDOBYr" runat="server" Style="padding-right: 30px;" CssClass="form-control"
                                                TabIndex="11">
                                            </asp:DropDownList>
                                        </div>--%>
                                    </div>
                                    <div class="form-group ">
                                        <label class="control-label col-sm-2">
                                            Mobile No.<span class="red">*</span></label>
                                        <asp:TextBox ID="txtMobileNo" onkeypress="return isNumberKey(event);" CssClass="form-control validate[required,custom[mobile]]"
                                            TabIndex="15" runat="server" ValidationGroup="eInformation" MaxLength="10"></asp:TextBox>
                                    </div>
                                    <div class="form-group ">
                                        <label class="control-label col-sm-2">
                                            Phone No.</label>
                                        <asp:TextBox ID="txtPhNo" onkeypress="return isNumberKey(event);" CssClass="form-control"
                                            TabIndex="16" runat="server" MaxLength="10"></asp:TextBox>
                                    </div>
                                    <div class="form-group " id="Divcardno" runat="server" visible="false">
                                        <label class="control-label col-sm-2">
                                            Card No.</label>
                                        <asp:TextBox ID="txtCardNo" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group greybt ">
                                        <label class="control-label col-sm-2">
                                            E-Mail ID</label>
                                        <asp:TextBox ID="txtEMailId" CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <h4>
                                        <span>Nominee Detail</span>
                                    </h4>
                                    <div class="form-group ">
                                        <label class="control-label col-sm-3">
                                            Nominee Name</label>
                                        <asp:TextBox ID="txtNominee" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                            runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group greybt ">
                                        <label class="control-label col-sm-2">
                                            Relation</label>
                                        <asp:TextBox ID="txtRelation" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                            runat="server"></asp:TextBox>
                                    </div>
                                    <h4>
                                        <span>Current Address</span>
                                    </h4>
                                    <div class="form-group greybt ">
                                        <label class="control-label col-sm-2">
                                            Address <span class="red">*</span></label>

                                        <asp:TextBox ID="TxtAddress" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group greybt ">
                                        <label class="control-label col-sm-2">
                                            Pincode<span class="red">*</span></label>
                                        <asp:TextBox ID="TxtPincode" CssClass="form-control validate[required]" onkeypress="return isNumberKey(event);"
                                            runat="server" MaxLength="6" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="form-group greybt ">
                                        <label class="control-label col-sm-2">
                                            State<span class="red">*</span></label>
                                        <asp:DropDownList ID="ddlSate" runat="server" CssClass="form-control ">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group greybt ">
                                        <label class="control-label col-sm-2">
                                            District<span class="red">*</span></label>

                                        <asp:TextBox ID="TxtDistrict" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group greybt ">
                                        <label class="control-label col-sm-2">
                                            City <span class="red">*</span></label>

                                        <asp:TextBox ID="TxtCity" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                                    </div>

                                    <div runat="server" id="DivPostal">
                                        <h4>
                                            <span>Postal Address</span>
                                        </h4>
                                        <div class="form-group greybt ">
                                            <label class="control-label col-sm-2">
                                                Address <span class="red">*</span></label>

                                            <asp:TextBox ID="TxtPostalAddress" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group greybt ">
                                            <label class="control-label col-sm-2">
                                                Pincode<span class="red">*</span></label>
                                            <asp:TextBox ID="TxtPostPincode" CssClass="form-control " onkeypress="return isNumberKey(event);"
                                                runat="server" MaxLength="6" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="form-group greybt ">
                                            <label class="control-label col-sm-2">
                                                State<span class="red">*</span></label>
                                            <asp:DropDownList ID="ddlPostSate" runat="server" CssClass="form-control ">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group greybt ">
                                            <label class="control-label col-sm-2">
                                                District<span class="red">*</span></label>

                                            <asp:TextBox ID="TxtPostDistrict" CssClass="form-control " runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group greybt ">
                                            <label class="control-label col-sm-2">
                                                City <span class="red">*</span></label>

                                            <asp:TextBox ID="TxtPostCity" CssClass="form-control " runat="server"></asp:TextBox>
                                        </div>
                                    </div>



                                    <div class="form-group ">
                                        <asp:Button ID="CmdSave" runat="server" Text="Update" CssClass="btn btn-primary"
                                            ValidationGroup="eInformation" OnClick="CmdSave_Click" />
                                        &nbsp;<asp:Button ID="CmdCancel" runat="server" Text="Cancel" CssClass="btn btn-primary"
                                            ValidationGroup="Form-Reset" OnClick="CmdCancel_Click" Visible="false" />
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

