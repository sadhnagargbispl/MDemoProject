<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="NewJoiningFreeUpWithOtp.aspx.cs" Inherits="NewJoiningFreeUpWithOtp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function DivOnOff() {
            if (document.getElementById("<%= chkterms.ClientID %>").checked == true) {
                document.getElementById("DivTerms").style.display = "block";
            } else {
                document.getElementById("DivTerms").style.display = "none";
            }
        }

        function FnBankChange(val) {
            if (val == "97") {
                document.getElementById("divBank").style.display = "block";
            } else {
                document.getElementById("divBank").style.display = "none";
            }
        }

        function FnVillageChange(val) {
            if (val == "381264") {
                document.getElementById("divVillage").style.display = "block";
            } else {
                document.getElementById("divVillage").style.display = "none";
            }
        }

        function FnPostVillageChange(val) {
            var ddlVillage = document.getElementById("<%= DDlPostVillage.ClientID %>");
            var selectedValue = ddlVillage.value;

            document.getElementById("<%= HPostVillage.ClientID %>").value = selectedValue;

            if (val == "381264") {
                document.getElementById("divPostVillage").style.display = "block";
            } else {
                document.getElementById("divPostVillage").style.display = "none";
            }
        }

        function GetSelectedItem() {
            var rb = document.getElementById("<%= RbtMarried.ClientID %>");
            var radio = rb.getElementsByTagName("input");

            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    if (radio[i].value == "Y") {
                        document.getElementById("divMarriageDate").style.display = "block";
                    } else {
                        document.getElementById("divMarriageDate").style.display = "none";
                    }
                }
            }
        }

        <%--function GetRegistrationAs() {
            var rb = document.getElementById("<%= RbCategory.ClientID %>");
            var radio = rb.getElementsByTagName("input");

            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {

                    if (radio[i].value == "IN") {

                        document.getElementById("RegType").style.display = "none";
                        document.getElementById("CompName").style.display = "none";
                        document.getElementById("CompRegistrationNo").style.display = "none";
                        document.getElementById("Div1").style.display = "block";
                        document.getElementById("divFName").style.display = "block";
                        document.getElementById("TrPrtnrCap").style.display = "none";

                        document.getElementById("<%= LblName.ClientID %>").textContent = "";
                        document.getElementById("<%= LblRegistDate.ClientID %>").textContent = "Date Of Birth";

                        var rb2 = document.getElementById("<%= CbSubCategory.ClientID %>");
                        var radio2 = rb2.getElementsByTagName("input");
                        radio2[0].checked = true;

                    } else {

                        document.getElementById("RegType").style.display = "block";
                        document.getElementById("CompName").style.display = "block";
                        document.getElementById("CompRegistrationNo").style.display = "block";
                        document.getElementById("Div1").style.display = "none";
                        document.getElementById("divFName").style.display = "none";
                        document.getElementById("TrPrtnrCap").style.display = "block";

                        document.getElementById("<%= LblName.ClientID %>").textContent = "Properitor ";
                        document.getElementById("<%= LblRegistDate.ClientID %>").textContent = "Date Of Company Registration";

                        var rb2 = document.getElementById("<%= CbSubCategory.ClientID %>");
                        var radio2 = rb2.getElementsByTagName("input");
                        radio2[0].checked = true;
                    }
                }
            }
        }--%>

       <%-- function GetRegistrationType() {

            var rb = document.getElementById("<%= CbSubCategory.ClientID %>");
            var radio = rb.getElementsByTagName("input");

            for (var i = 0; i < radio.length; i++) {

                if (radio[i].checked) {

                    if (radio[i].value == "SP") {

                        document.getElementById("Div1").style.display = "none";
                        document.getElementById("divFName").style.display = "none";
                        document.getElementById("TrPrtnrCap").style.display = "none";

                        document.getElementById("<%= LblName.ClientID %>").textContent = "Properitor ";
                        document.getElementById("<%= LblRegistDate.ClientID %>").textContent = "Date Of Company Registration";

                    } else if (radio[i].value == "PF") {

                        document.getElementById("Div1").style.display = "none";
                        document.getElementById("divFName").style.display = "none";
                        document.getElementById("TrPrtnrCap").style.display = "block";

                        document.getElementById("<%= LblName.ClientID %>").textContent = "Partner ";
                        document.getElementById("<%= LblRegistDate.ClientID %>").textContent = "Date Of Company Registration";

                    } else {

                        document.getElementById("Div1").style.display = "none";
                        document.getElementById("divFName").style.display = "none";
                        document.getElementById("TrPrtnrCap").style.display = "none";

                        document.getElementById("<%= LblName.ClientID %>").textContent = "Owner ";
                        document.getElementById("<%= LblRegistDate.ClientID %>").textContent = "Date Of Company Registration";
                    }
                }
            }
        }--%>

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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>New joining</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">

                                <div class="col-md-6">
                                    <div class="clr">
                                        <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                    </div>

                                    <div class="form-group">
                                        <label for="textInput" class="form-label">
                                            Sponsor ID<span class="red">*</span></label>
                                        <asp:TextBox ID="txtRefralId" CssClass="form-control validate[required,custom[onlyLetterNumber]]"
                                            TabIndex="1" runat="server" AutoPostBack="True" ValidationGroup="eInformation"
                                            autocomplete="off" OnTextChanged="txtRefralId_TextChanged"></asp:TextBox>
                                        <asp:Label ID="lblRefralNm" runat="server" Style="color: #1faf45; font-weight: bolder;"></asp:Label>
                                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </div>

                                    <div class="form-group" id="rwSpnsr" runat="server">
                                        <label for="textInput" class="form-label">
                                            Upliner ID<span class="red">*</span></label>
                                        <asp:TextBox ID="txtUplinerId" CssClass="form-control" TabIndex="2" runat="server" AutoPostBack="True"
                                            ValidationGroup="eSponsor" autocomplete="off" OnTextChanged="txtUplinerId_TextChanged"></asp:TextBox>
                                        <asp:Label ID="lblUplnrNm" runat="server" ForeColor="#D11F7B"></asp:Label>
                                    </div>

                                    <div class="form-group" runat="server" id="DivVVRanta" visible="false">
                                        <label class="form-label">
                                            <asp:Label ID="Label1" runat="server"></asp:Label>
                                            User Name<span class="red">*</span>
                                        </label>
                                        <asp:TextBox
                                            ID="TxtUserName"
                                            runat="server"
                                            CssClass="form-control"
                                            TabIndex="6"
                                            ValidationGroup="eInformation"
                                            AutoPostBack="true"
                                            autocomplete="off"
                                            OnTextChanged="TxtUserName_TextChanged"
                                            onkeypress="return event.keyCode != 32;"
                                            oninput="this.value = this.value.replace(/\s/g,'');">
        </asp:TextBox>
                                        <asp:Label ID="LblUsername" runat="server" ForeColor="#D11F7B"></asp:Label>
                                    </div>

                                    <div class="form-group" runat="server" id="DivLeg1">
                                        <label class="form-label">
                                            Leg<span class="red">*</span></label>
                                        <div>
                                            <asp:RadioButtonList ID="RbtnLegNo" runat="server" TabIndex="3" RepeatDirection="Horizontal" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <h4>Personal Detail</h4>

                                    <div id="dvreg" runat="server" visible="true">
                                        <div class="form-group">
                                            <label class="form-label">Registration As</label>
                                            <asp:RadioButtonList ID="RbCategory" runat="server" RepeatDirection="Horizontal" CssClass="form-control"
                                                TabIndex="4" onchange="handleRegistrationChange();">
                                                <asp:ListItem Text="Individual" Value="IN" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Company" Value="C"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>

                                        <div class="form-group" id="RegType" style="display: none">
                                            <label class="form-label">
                                                <asp:Label ID="LblRegType" Text="Registration Type" runat="server"></asp:Label>
                                            </label>
                                            <asp:RadioButtonList ID="CbSubCategory" runat="server" TabIndex="5" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="ProprietorShip" Value="SP" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Partnership Firm" Value="PF"></asp:ListItem>
                                                <asp:ListItem Text="Private Limited Company" Value="PL"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>

                                    <%-- NAME FIELD --%>
                                    <div class="form-group">
                                        <label class="form-label" id="lblNameLabel">
                                            <asp:Label ID="LblName" runat="server"></asp:Label>
                                            <span id="spnNameText">Name</span><span class="red">*</span>
                                        </label>
                                        <div>
                                            <asp:TextBox ID="txtFrstNm" class="form-control" runat="server" TabIndex="6"
                                                ValidationGroup="eInformation" autocomplete="off"
                                                CssClass="form-control validate[required,custom[onlyLetterNumberChar]]"></asp:TextBox>
                                        </div>
                                        <asp:Label ID="lblmemmsg" runat="server" ForeColor="#D11F7B"></asp:Label>
                                    </div>

                                    <div class="form-group" id="TrPrtnrCap" style="display: none">
                                        <label class="form-label">
                                            <asp:Label ID="LblPartnerName" runat="server" Text="Partner Name Seperated By Comma(,)"></asp:Label>
                                        </label>
                                    </div>

                                    <%-- FATHER / HUSBAND / AUTHORIZED PERSON SECTION --%>
                                    <div class="form-group greybt" id="divFName" runat="server">
                                        <label class="form-label" id="lblFatherLabel">
                                            Father/Husband's Name <span class="red">*</span>
                                        </label>
                                        <div class="row" id="divSoWoRow">
                                            <%-- S/O D/O W/O Dropdown - shown for Individual --%>
                                            <div class="col-md-2" id="divCmbTypeCol">
                                                <asp:DropDownList CssClass="form-control" ID="CmbType" runat="server" TabIndex="7">
                                                    <asp:ListItem Value="S/O" Text="S/O"></asp:ListItem>
                                                    <asp:ListItem Value="D/O" Text="D/O"></asp:ListItem>
                                                    <asp:ListItem Value="W/O" Text="W/O"></asp:ListItem>
                                                    <asp:ListItem Value="C/O" Text="C/O"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-10" style="padding-left: 0px;" id="divFatherNameCol">
                                                <asp:TextBox ID="txtFNm" runat="server" TabIndex="8"
                                                    class="form-control validate[required]" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>

                                        <%-- Authorized Person Name - shown for Company (hidden by default) --%>
                                        <div id="divAuthorizedPerson" style="display: none;">
                                            <asp:TextBox ID="TxtAuthorizedPerson" runat="server" TabIndex="8"
                                                CssClass="form-control" autocomplete="off"
                                                placeholder="Enter Authorized Person Name"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%-- DOB --%>
                                    <div class="form-group greybt" visible="false" runat="server" id="Divdob">
                                        <label class="form-label">
                                            <asp:Label ID="LblRegistDate" runat="server" Text="Date Of Birth"></asp:Label>
                                        </label>
                                        <div class="row">
                                            <div class="col-sm-4 p0 pl10">
                                                <asp:DropDownList ID="ddlDOBdt" runat="server" CssClass="form-control" TabIndex="9" autocomplete="off"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-4 p0 pl10">
                                                <asp:DropDownList ID="ddlDOBmnth" runat="server" CssClass="form-control" TabIndex="10" autocomplete="off"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-4 p0 pl10">
                                                <asp:DropDownList ID="ddlDOBYr" runat="server" Style="padding-right: 30px;" CssClass="form-control" TabIndex="11" autocomplete="off"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group" id="Div1" visible="false" runat="server">
                                        <label class="form-label"><span class="red">*</span></label>
                                        <asp:RadioButtonList ID="RbtMarried" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" TabIndex="12" onchange="return GetSelectedItem()" autocomplete="off">
                                            <asp:ListItem Text="Married" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="UnMarried" Value="N"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <span class="red"></span>
                                    </div>

                                    <div class="form-group greybt" id="divMarriageDate" visible="false" style="display: none;">
                                        <label class="form-label">Marriage Date</label>
                                        <div class="row">
                                            <div class="col-sm-4 p0 pl10">
                                                <asp:DropDownList ID="DDlMDay" runat="server" CssClass="form-control" TabIndex="13"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-4 p0 pl10">
                                                <asp:DropDownList ID="DDLMMonth" runat="server" CssClass="form-control" TabIndex="14"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-4 p0 pl10">
                                                <asp:DropDownList ID="DDLMYear" runat="server" Style="padding-right: 30px;" CssClass="form-control" TabIndex="15"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group" id="CompName" style="display: none">
                                        <label class="form-label">Company Name</label>
                                        <asp:TextBox ID="TxtCompanyName" runat="server" CssClass="form-control" TabIndex="16"></asp:TextBox>
                                    </div>

                                    <div class="form-group" id="CompRegistrationNo" style="display: none">
                                        <label class="form-label">Company Registration No</label>
                                        <asp:TextBox ID="TxtRegistrationNo" runat="server" CssClass="form-control" TabIndex="17"></asp:TextBox>
                                    </div>

                                    <div id="dvpin" runat="server">
                                        <h4>Contact Detail</h4>

                                        <div class="form-group" visible="false" runat="server" id="divAddress">
                                            <label class="form-label">Address</label>
                                            <div>
                                                <asp:TextBox ID="txtAddLn1" TabIndex="19" runat="server" autocomplete="off" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="form-label">State</label>
                                            <div>
                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="form-label">District</label>
                                            <div>
                                                <asp:TextBox ID="ddlDistrict" CssClass="form-control" TabIndex="17" runat="server" autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="HDistrictCode" runat="server" />
                                            </div>
                                        </div>

                                        <div class="form-group" id="divcity" runat="server">
                                            <label class="form-label">City</label>
                                            <div>
                                                <asp:TextBox ID="ddlTehsil" CssClass="form-control" TabIndex="18" runat="server"
                                                    ValidationGroup="eInformation" autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="HCityCode" runat="server" />
                                            </div>
                                        </div>

                                        <div class="form-group" style="display: none">
                                            <label class="form-label">Area</label>
                                            <div>
                                                <asp:DropDownList ID="DDlVillage" CssClass="form-control" TabIndex="19" runat="server"
                                                    ValidationGroup="eInformation" autocomplete="off" onchange="FnVillageChange(this.value);">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group" id="divVillage" style="display: none">
                                            <label class="form-label">Area Name</label>
                                            <div>
                                                <asp:TextBox ID="TxtVillage" CssClass="form-control" TabIndex="20" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="form-label">Pin code<span class="red">*</span></label>
                                            <div>
                                                <asp:TextBox ID="txtPinCode" CssClass="form-control validate[required]"
                                                    onkeypress="return isNumberKey(event);" TabIndex="19" runat="server"
                                                    MaxLength="6" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div id="Dvfld" runat="server" visible="false">
                                            <div class="form-group">
                                                <label class="form-label">Same As Above<span class="red">*</span></label>
                                                <asp:CheckBox ID="ChkSame" runat="server" onclick="return GetSameAsPostal()" TabIndex="21" />
                                            </div>
                                            <h5>Postal Address</h5>
                                            <div class="form-group">
                                                <label class="form-label">Address</label>
                                                <asp:TextBox ID="TxtPostalAddress" CssClass="form-control" TabIndex="22" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label class="form-label">Pin code<span class="red">*</span></label>
                                                <asp:TextBox ID="TxtPostPincode" CssClass="form-control" onkeypress="return isNumberKey(event);"
                                                    TabIndex="23" runat="server" MaxLength="6" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label class="form-label">State</label>
                                                <asp:TextBox ID="TxtpostState" runat="server" CssClass="form-control" TabIndex="24" autocomplete="off" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="HPostStateCode" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label class="form-label">District</label>
                                                <asp:TextBox ID="TxtPostDistrict" CssClass="form-control" TabIndex="25" runat="server" autocomplete="off" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="HPostDistrict" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label class="form-label">City</label>
                                                <asp:TextBox ID="TxtPostCity" CssClass="form-control" TabIndex="26" runat="server"
                                                    ValidationGroup="eInformation" autocomplete="off" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="HPostCity" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label class="form-label">Area</label>
                                                <asp:DropDownList ID="DDlPostVillage" CssClass="form-control" TabIndex="27" runat="server"
                                                    ValidationGroup="eInformation" autocomplete="off" onchange="FnPostVillageChange(this.value);">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="HPostVillage" runat="server" />
                                            </div>
                                            <div class="form-group" id="divPostVillage" style="display: none">
                                                <label class="form-label">Area Name</label>
                                                <asp:TextBox ID="TxtPostVillage" CssClass="form-control" TabIndex="28" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group" id="divMobile" runat="server">
                                        <label class="form-label">Mobile No.<span class="red">*</span></label>
                                        <div>
                                            <asp:TextBox ID="txtMobileNo" onkeypress="return isNumberKey(event);"
                                                CssClass="form-control validate[required,custom[mobile]]"
                                                TabIndex="29" runat="server" MaxLength="10"
                                                ValidationGroup="eInformation" autocomplete="off"></asp:TextBox>
                                            <asp:Label ID="lblMobileNo" runat="server" ForeColor="#D11F7B"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group" visible="false" runat="server">
                                        <label class="form-label">Phone No.</label>
                                        <asp:TextBox ID="txtPhNo" onkeypress="return isNumberKey(event);" CssClass="form-control"
                                            TabIndex="30" runat="server" MaxLength="10" autocomplete="off"></asp:TextBox>
                                    </div>

                                    <div class="form-group greybt" visible="true" runat="server">
                                        <label class="form-label">E-Mail ID. <span class="red">*</span></label>
                                        <div>
                                            <asp:TextBox ID="txtEMailId" CssClass="form-control validate[custom[email]]" TabIndex="31"
                                                runat="server" autocomplete="off" Style="text-transform: none;"></asp:TextBox>
                                        </div>
                                        <asp:Label ID="lblemail" runat="server" ForeColor="#D11F7B"></asp:Label>
                                    </div>

                                    <div class="form-group" runat="server" style="display: none">
                                        <label class="form-label">PAN No Available<span class="red">*</span></label>
                                        <div>
                                            <asp:RadioButtonList ID="RbtPan" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                                RepeatLayout="Table" TabIndex="41">
                                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="N" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <span class="red">
                                                <asp:Label ID="LblPanNoAvail" runat="server" Text="Payout will deduct 20%, If you not enter PAN NO."></asp:Label>
                                            </span>
                                        </div>
                                    </div>

                                    <div class="form-group greybt" runat="server" id="divPan">
                                        <label class="form-label">PAN No.</label>
                                        <div>
                                            <asp:TextBox ID="txtPanNo" CssClass="form-control validate[custom[panno]]" TabIndex="42"
                                                runat="server" autocomplete="off" MaxLength="10" ValidationGroup="eInformation"></asp:TextBox>
                                            <asp:Label ID="lblpan" runat="server" ForeColor="#D11F7B"></asp:Label>
                                        </div>
                                    </div>

                                    <div id="dvname" runat="server" visible="false">
                                        <h4>Nominee Detail</h4>
                                        <div class="form-group">
                                            <label class="form-label">Nominee Name</label>
                                            <asp:TextBox ID="txtNominee" CssClass="form-control" TabIndex="32" runat="server" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="form-group greybt">
                                            <label class="form-label">Relation</label>
                                            <asp:TextBox ID="txtRelation" CssClass="form-control" TabIndex="33" runat="server" autocomplete="off"></asp:TextBox>
                                        </div>

                                        <div id="divBankDetail" runat="server">
                                            <h4>Bank Detail</h4>
                                            <div class="form-group">
                                                <label class="form-label">Account No.</label>
                                                <asp:TextBox ID="TxtAccountNo" onkeypress="return isNumberKey(event);" CssClass="form-control"
                                                    TabIndex="34" runat="server" MaxLength="16" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label class="form-label">Account Type</label>
                                                <asp:DropDownList ID="DDLAccountType" runat="server" CssClass="form-control" TabIndex="21">
                                                    <asp:ListItem Text="CHOOSE ACCOUNT TYPE" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="SAVING ACCOUNT" Value="SAVING ACCOUNT"></asp:ListItem>
                                                    <asp:ListItem Text="CURRENT ACCOUNT" Value="CURRENT ACCOUNT"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group">
                                                <label class="form-label">Bank</label>
                                                <asp:DropDownList ID="CmbBank" runat="server" CssClass="form-control" TabIndex="36"
                                                    onchange="FnBankChange(this.value);" autocomplete="off">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group" id="divBank" style="display: none">
                                                <label class="form-label">Bank Name</label>
                                                <asp:TextBox ID="TxtBank" CssClass="form-control" TabIndex="37" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label class="form-label">Branch Name</label>
                                                <asp:TextBox ID="TxtBranchName" CssClass="form-control" TabIndex="38" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label class="form-label">IFSC Code</label>
                                                <asp:TextBox ID="txtIfsCode" runat="server" CssClass="form-control" TabIndex="39" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="form-group" visible="false">
                                                <asp:TextBox ID="TxtMICR" CssClass="form-control" Visible="false" TabIndex="40" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group greybt" id="divAdhar" runat="server">
                                        <label class="form-label">AADHAR No.<span class="red">*</span></label>
                                        <div class="row">
                                            <div class="col-md-12 p0 pl10">
                                                <asp:TextBox ID="TxtAAdhar1" CssClass="form-control" TabIndex="43" runat="server"
                                                    onkeypress="return isNumberKey(event);" MaxLength="16" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 p0 pl10" style="display: none;">
                                                <asp:TextBox ID="TxtAadhar2" CssClass="form-control" TabIndex="44" runat="server"
                                                    onkeypress="return isNumberKey(event);" MaxLength="4" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 p0 pl10" style="display: none;">
                                                <asp:TextBox ID="TxtAadhar3" CssClass="form-control" TabIndex="45" runat="server"
                                                    onkeypress="return isNumberKey(event);" MaxLength="4" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="divpay" runat="server" visible="false">
                                        <h4>Payment Deposit Detail</h4>
                                        <div class="form-group">
                                            <label class="form-label">Select Paymode</label>
                                            <asp:DropDownList ID="DdlPaymode" runat="server" CssClass="form-control" TabIndex="46" autocomplete="off"></asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label class="form-label">
                                                <asp:Label ID="LblDDNo" runat="server" Text="Draft/CHEQUE No. *"></asp:Label>
                                            </label>
                                            <asp:TextBox ID="TxtDDNo" CssClass="form-control" TabIndex="47" runat="server" MaxLength="15" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label class="form-label">
                                                <asp:Label ID="LblDDDate" runat="server" Text="Draft/CHEQUE Date *"></asp:Label>
                                            </label>
                                            <asp:TextBox ID="TxtDDDate" runat="server" TabIndex="48" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtDDDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        </div>
                                        <div class="form-group">
                                            <label class="form-label">Issued Bank Name</label>
                                            <asp:TextBox ID="TxtIssueBank" CssClass="form-control" TabIndex="49" runat="server" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="form-group greybt">
                                            <label class="form-label">Issued Bank Branch</label>
                                            <asp:TextBox ID="TxtIssueBranch" CssClass="form-control" TabIndex="50" runat="server" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div id="divlogin" runat="server" visible="false">
                                        <h4>Login Information</h4>
                                        <div class="form-group">
                                            <label class="form-label">Password<span class="red">*</span></label>
                                            <div>
                                                <asp:TextBox ID="TxtPasswd" runat="server" CssClass="form-control" TextMode="Password" ClientIDMode="Static"></asp:TextBox>
                                                <i class="fa fa-eye fa-eye field_icon" aria-hidden="true"
                                                    style="transform: translateY(-190%); float: right; padding-right: 10px;" id="toggle_pwd"></i>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="form-label">Confirm Password<span class="red">*</span></label>
                                            <div>
                                                <asp:TextBox ID="TxtConfirmPasswd" runat="server" CssClass="form-control"
                                                    TextMode="Password" ClientIDMode="Static" onkeyup="checkPasswordMatch();">
                </asp:TextBox>
                                                <i class="fa fa-eye fa-eye field_icon" aria-hidden="true"
                                                    style="transform: translateY(-190%); float: right; padding-right: 10px;" id="toggle_pwd1"></i>
                                                <asp:Label ID="lblPassMsg" runat="server" ClientIDMode="Static"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group" visible="false" runat="server">
                                            <label class="form-label">Transaction Password<span class="red">*</span></label>
                                            <asp:TextBox ID="TxtTransactionPassword"
                                                class="validate[required,minSize[5],maxSize[10]] form-control"
                                                TabIndex="52" runat="server" TextMode="Password"
                                                ValidationGroup="eInformation" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>

                                    <asp:Label ID="lblErrEpin" runat="server" CssClass="error"></asp:Label>

                                    <div>
                                        <div class="form-group">
                                            <textarea style="width: 100%; height: 80px; text-align: left;" cols="5" rows="10"
                                                runat="server" visible="false" id="divTermAll" readonly="readonly">
   Terms & Conditions :-
•It is kind advise to you that you promote Business as per actual. Company will not responsible for your miss commitments in the market through any manner. 
•Registration is FREE in our system.
•Company provides you online account as your ID with password. It contains all your legal information , Transaction Balance, Team detail, Bonus details etc.
•Year starts from 1st April every year.
•KYC Documents is mandatory.
•You must sign your application form & submitted in nearest company Branch/office along with one colour passport size photo & copy of self attested ID proof or address proof / PAN No.
                             </textarea>
                                            <textarea style="width: 100%; height: 80px; text-align: left;" cols="5" rows="10"
                                                runat="server" visible="false" id="Textarea1" readonly="readonly">
   Direct Seller Contract Agreement 

This agreement is agreed and accepted electronically & online by and between the executing parties (Hereinafter mentioned and referred to as Direct Seller and the Direct Selling Entity which expressions shall mean and include their respective legal heirs, assigns, successors, administrators, and undertakers).  

Be known that this Contract agreement is executed and entered into in accordance with the provisions of India Contract Act and Consumer Protection (Direct Selling) Rules, 2021 (Hereinafter referred to as the Rules)

Whereas the Direct Seller has voluntarily out of his / her own accord, sweet will and without any coercion whatsoever, mental, or physical, offered to join the Direct Selling Network business of the Direct Selling Entity named M/s DV9 Wellness Pvt. Ltd. also referred as “DV9 Wellness Pvt. Ltd.” (Registered under the Companies Act, 2013) having head office at  ¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬-¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬¬ ¬¬¬¬¬ Office 702/A, 7th Floor, Metro Tower, A.B. Road, Indore (M.P.). 

And whereas the DV9 Wellness Pvt. Ltd. is engaged in “Direct Selling business” which means marketing, distribution and sale of goods or providing of services through a network of Direct Seller as per its prescribed Marketing Plan (Which may be read as part and parcel of this agreement as the same is not being reproduced here for the sake of brevity) not falling under the pyramid or Money circulation scheme.

And whereas the Direct Seller named below along with his / her KYC particulars therein has, after being explained all the provisions of the said DV9 Wellness Pvt. Ltd.  Marketing Plan, product details and the present E-contract Agreement in the vernacular language known to him by Shri ___________________ ID No. _______________, duly ascertained and satisfied by visiting the DV9 Wellness Pvt. Ltd. ’s website www.dv9wellness.com, has voluntarily offered to join the business of the DV9 Wellness Pvt. Ltd.  and resolved to enter into this E-contract agreement, hence this deed.

DEFINITION:  
In this Contract
1.	"Direct Seller(s)" means a person authorized by a Direct Selling entity through a legally enforceable written contract to undertake Direct Selling business on principal-to-principal basis.  
2.	“Direct Selling Entity” means the principal entity which sells or offer to sell goods or services through Direct Seller, but does not include an entity which is engaged in a pyramid scheme or money circulation scheme.
3.	“Network of Sellers” means a network of Direct Seller formed by a Direct Selling entity to sell goods or services for the purpose of receiving consideration solely from such sale.

NOW THEREFORE THIS DEED AGREEMENTS AS UNDER:
1.	The Direct Selling entity hereby agrees that it is fully compliant to the Consumer Protection Act, 2019, Consumer Protection (Direct Selling) Rules, 2021, Legal Metrology Act, 2009, E-commerce Rules, 2020 and all other Rules and laws applicable to an Indian Direct Selling Entity.
2.	The Direct Selling entity assures and the Direct Seller agrees:
a.	That this E-contract agreement has no provision that a Direct Seller will receive remuneration or incentive for the recruitment / enrolment only of new participants.  
b.	That it does not require a participant to purchase goods or services for an amount that exceeds the amount for which such goods or services can be expected to be sold or resold to consumers.
c.	That it does not require a participant to pay any entry / registration fee / subscription fee, cost of sales demonstration equipment and materials or other fees relating to participation in the DV9 Wellness Pvt. Ltd. Direct Selling business.
d.	That it has ascertained from the Marketing Plan provided by the DV9 Wellness Pvt. Ltd.  (The same may be read as part and parcel of this E-contract agreement as the same is not being reproduced here for the sake of brevity), the stipulated amount of any or all types of Incentives, rewards, etc. including financial and non-financial benefits payable to the Direct Seller are calculated only and only on the basis of effective sale, marketing and distribution of products and in no way on the basis of  recruiting / sponsoring / introducing another Direct Seller.
3.	Cooling Off Policy: 
a.	That the DV9 Wellness Pvt. Ltd. allows or provides to the Direct Seller herein a reasonable cooling off period in accordance with clause 3 (b) of the Rules, of the said Rules undertake to provide a newly registered Direct Seller a cooling off period of 7 days effective from the date of signing and execution of the contract agreement by him / her while registering as Direct Seller with us wherein the said Direct Seller can cancel the contract agreement without resulting in any breach of contract or levy of penalty.
b.	That if such Direct Seller receive any form of compensation from the DV9 Wellness Pvt. Ltd. during this Cooling Off period, they are obligated to repay the corresponding amount to the DV9 Wellness Pvt. Ltd. This repayment should be accompanied by a formal repudiation letter. The repayment can be made through methods like cash, cheque, demand draft (DD), NEFT, RTGS, and so on.
c.	That if the DV9 Wellness Pvt. Ltd. collects any fees, which may include training fees, franchise fees, fees for promotional materials, or any other fees, and if the Direct Seller decides to return all goods received at the time of joining, they have the entitlement to receive those fees back from the DV9 Wellness Pvt. Ltd. In this scenario, the DV9 Wellness Pvt. Ltd. bears the responsibility of reimbursing these fees. This reimbursement will be made to the Direct Seller through methods like cash, cheque, demand draft (DD), NEFT, RTGS, or Net Banking. Applicable taxes such as TDS and GST will be adjusted, and the repayment will be accompanied by an appropriate repudiation letter.
4.	Buyback Policy: The DV9 Wellness Pvt. Ltd. provides buyback guarantee to every Direct Seller on the following terms:
a.	If the product is in marketable* condition and is returned within 7 days of receipt of goods accompanied by the original invoice, 100% of the amount as refund will be given.
b.	If the product is in Unmarketable** condition and is returned within 7 days of receipt of goods no refund will be given.
c.	*Marketable* refers to products that are unused, sealed, and undamaged, not expired, not sessional, discontinued, or special promotional product or services.
5.	The DV9 Wellness Pvt. Ltd. commits to offering a warranty for the products it sells. Direct Seller has the option to request an exchange or return of a product within 30 days of purchase if they identify any manufacturing defect or if the purchased product is of sub quality. To initiate an exchange or refund process, the Direct Seller should present the original invoice, along with their identity proof and address proof, to DV9 Wellness Pvt. Ltd. This documentation will be required for verification purposes.
6.	That the Direct Seller herein agrees that the DV9 Wellness Pvt. Ltd. has established a “Grievance Redressal Mechanism” for Direct Seller to redress their grievances and complaints, annexed herewith which may be read as part and parcel of this agreement as the same is not being reproduced here for the sake of brevity.
7.	That the applicant Direct Seller herein agrees that he / she has attained the minimum age of 18 years or 21 years in the Indian state and he / she shall knowingly sponsor any person under the age of 18 years or 21 years in the state.  
8.	The DV9 Wellness Pvt. Ltd. explicitly states that it does not ask, encourage, or seek any potential individuals, known as prospects or future Direct Seller, to invest any money in any way to join its Direct Selling business. However, the Direct Seller is responsible for covering the costs of the products they buy. It is important to note that there is no provision stating that the Direct Seller will earn money from recruiting other participants. Instead, the compensation the Direct Seller receives solely comes from the sales, marketing, and distribution of products. This compensation aligns with the DV9 Wellness Pvt. Ltd. Marketing Plan outlined by the DV9 Wellness Pvt. Ltd. The Direct Seller agrees to abide by the terms of this agreement in its entirety, following the guidelines and principles set by the Direct Selling entity.
9.	That the DV9 Wellness Pvt. Ltd. will provide all support to the Direct Seller in delivery of the products through Franchisee / Pick-up centers / Available Courier / Transport or any other Logistics Service for maintaining effective support system. 
10.	That by accepting the offer of the Direct Seller herein the DV9 Wellness Pvt. Ltd. requires him / her to do and complete the following steps. An Individual / Firm / entity eligible to enter into a contract as per the provisions of the Indian Contract Act, 1872 and wish to become a Direct Seller of the Direct Selling business of the DV9 Wellness Pvt. Ltd. herein, can apply to become a Direct Seller for marketing and selling of DV9 Wellness Pvt. Ltd. ’s product on pan India basis, in prescribed form through online method.
a.	Fill the application form online and upload scanned KYC documents.
b.	Accept the terms and condition of this E-contract agreement by clicking on “I AGREE” button below.
c.	On the completion of the above process, the Direct Seller can take a printout of this agreement.
d.	Upon the execution of this agreement and after the verification of all the KYC documents uploaded through the above process, the applicant shall be accepted as a Direct Seller of the DV9 Wellness Pvt. Ltd. ’s business and a Unique Identification number and password shall be allotted to the applicant, to allow him / her to log on to access his / her own personal account maintained by the Direct Selling Entity on its website.  
e.	That the Direct Seller is required to upload the following self-attested documents within 30 days starting from the date of signing this Agreement. The acceptance of the terms of this agreement is confirmed by clicking the “I AGREE” button at the bottom of this document. 
f.	That the DV9 Wellness Pvt. Ltd. upon scrutiny and verification of the Application and KYC particulars uploaded by the Direct Seller may re-consider its decision and reject the application. The Direct Seller acknowledges and accepts this possibility. The DV9 Wellness Pvt. Ltd. holds the exclusive authority and freedom to decline the issuance of unique ID number, if the KYC and other documents are determined to be unsatisfactory, altered, counterfeit or not in accordance with the Government’s stipulated guidelines for this specific purpose. 
g.	That the KYC shall include but not limited to verified proof of address, proof of identity, and PAN as per the provisions of the Income Tax Act, 1961, as follows, duly issued by the Government of India or a State / UT government.  
i.	Aadhaar Card
ii.	Voter ID Card
iii.	Passport
iv.	Ration card
v.	Any other identity document issued by the State / UT or central government which can be verified online.
vi.	Additional Documents required for Applicant in case of a company or firm:
1.	CIN or Registration Certificate, MOA & AOA, or Partnership Deed, as the case may be;
2.	PAN, GSTIN, FSSAI (wherever applicable)
3.	List of Directors / Partners of the applicant entity
4.	Board Resolution / Authorization in favor of the Director / Partner signing and executing this E-Contract agreement and Application.
11.	The Direct Seller herein declares that he / she / they has / have not been declared a bankrupt by a competent court of law as provided under clause (3) of section 79 of the Insolvency and Bankruptcy Code, 2016 and that he / she is neither of unsound mind nor convicted by any court of law in preceding five years” of the date of joining the Direct Selling entity’s business herein.
12.	The Direct Seller herein agrees that he / she shall take appropriate steps to ensure the protection of all sensitive personal information provided by the consumer with the applicable laws for the time being in force and ensure adequate safeguards to prevent access to, or misuse of, data by unauthorized persons.
13.	The Direct Seller herein agrees that he / she shall not visit a consumer’s premises without identity card and prior appointment or approval.
14.	Scope of the Work:
a.	That the Direct Seller shall market, distribute and sell the products of the DV9 Wellness Pvt. Ltd. using word of mouth publicity, display and demonstration of the products, distribution of pamphlets, and door to door selling to consumers and prospective Direct Seller. 
b.	That the DV9 Wellness Pvt. Ltd. shall be exclusive owner of the name and logo of the DV9 Wellness Pvt. Ltd. The Direct Seller shall not use the trademark, logo type and design anywhere without prior written permission from the DV9 Wellness Pvt. Ltd. This permission, if given, can be withdrawn at any time by the DV9 Wellness Pvt. Ltd. Violations if any, shall be termed as violation of this agreement and may result in termination of this agreement and Direct Seller of the Direct Seller, penal actions under the prevailing Intellectual Property Rights (IPR) laws and Rules at the sole discretion of the entity herein to which the Direct Seller herein agrees.
c.	That the Direct Seller shall not manipulate, alter, amend, add, or delete any provisions of the DV9 Wellness Pvt. Ltd.  herein DV9 Wellness Pvt. Ltd.  Marketing Plan, pricing of products, BV etc., in any way whatsoever and shall not send, transmit, or otherwise communicate any messages to anybody on behalf of the DV9 Wellness Pvt. Ltd. , contrary to DV9 Wellness Pvt. Ltd. ’s policies, principles, instructions and prescriptions without prior written authorization and permission for the same by the DV9 Wellness Pvt. Ltd. .
d.	That the Direct Seller will get specified percentage / points-based (BV Points) Incentives pertaining to the sales for selling the DV9 Wellness Pvt. Ltd. ’s products under this E-contract Agreement.
e.	The DV9 Wellness Pvt. Ltd. commits to providing the Direct Seller with comprehensive instruction books, catalogs, and pamphlets to assist in promoting sales, marketing, and distribution. Additionally, the DV9 Wellness Pvt. Ltd. will ensure that mandatory orientation training is provided to the Direct Seller.
f.	That the DV9 Wellness Pvt. Ltd. shall issue photo identity cards to Direct Seller. This photo identity card shall be returned by the Direct Seller to the DV9 Wellness Pvt. Ltd. at the expiry / termination / revocation of this agreement and / or shall be destroyed but shall not be misused in any way or form whatsoever. The identity card shall contain the Name & Unique ID number (FSSAI Number, if applicable) of the Direct Seller.
g.	The identity card provided by the DV9 Wellness Pvt. Ltd. to the Direct Seller does not establish an employee – employer, service, or salaried relationship between the DV9 Wellness Pvt. Ltd. and the Direct Seller.
h.	The Direct Seller will not be authorized to collect any type of cash / cheque / demand draft in his own name, on behalf of the DV9 Wellness Pvt. Ltd. All cheques / demand drafts etc. should be drawn in the name of the DV9 Wellness Pvt. Ltd. only and the same should be deposited with the DV9 Wellness Pvt. Ltd. ’s office or other offices as may be specified by the DV9 Wellness Pvt. Ltd., within 24 hours of the time of receipt. Direct Seller shall hold the said cash collection / cheque / DD in trust for and on behalf of the DV9 Wellness Pvt. Ltd. Upon failure to deposit the said cash collection / cheque / DD, Direct Seller shall be liable to pay damages / compensation and Mesne-profit, if any. The receipt / invoice issued by the DV9 Wellness Pvt. Ltd. only would be valid documentary evidence in the hand of the consumer. It means Direct Seller would not be authorized to issue any receipt / invoice on behalf of the DV9 Wellness Pvt. Ltd.
i.	That the DV9 Wellness Pvt. Ltd. may open following facilities for sale of its products:
i.	Online Portal / E-commerce
ii.	Stores (Retail Outlets)
iii.	Authorized Sales Point / Pickup Center
j.	That a Direct Seller is not authorized to sell any product of the DV9 Wellness Pvt. Ltd.  herein on e-commerce platform / marketplace, without prior written consent, permission, or authorization of the entity herein the Direct Seller is also prohibited from listing, marketing, advertising, promoting, discussing, or selling any product, or the business opportunity on any website or online forum that offers auction as a mode of Selling. 
k.	That a Direct Seller and their legal representatives, including spouse, son, unmarried daughter, or husband, shall not be permitted to enroll under a different sponsor or misrepresent facts to the company at any time during the validity of this agreement. In the event that a Direct Seller is found engaging in such conduct, the company reserves the right to terminate the Direct Seller’s ID as well as the IDs of the aforementioned legal heirs. The company also reserves the right to initiate any other appropriate legal action, as deemed necessary.
15.	Sales Incentives / Commission Structure or other Benefit: The Direct Seller shall be eligible for the following financial incentives and / or privileges:
a.	Incentives on the sales, marketing, and distribution of products and / or services by the Direct Seller and his / her team or network of Direct Seller, as per the DV9 Wellness Pvt. Ltd. Marketing Plan of the entity herein, annexed herewith but not being reproduced here for the sake of brevity.
b.	Direct Seller has the authorization to market, sell, and distribute products offered by the DV9 Wellness Pvt. Ltd. across all regions of India. There are no territorial restrictions or limits imposed on the sale of these products. 
c.	He / she can always check and inspect his / her account on the DV9 Wellness Pvt. Ltd. ’s website by using his / her Unique ID and Password allotted to him / her by the DV9 Wellness Pvt. Ltd.
d.	That the DV9 Wellness Pvt. Ltd. reserves the right to restrict the list of products for a particular area / region.
e.	Changes in pricing, government regulations, market influences, and other factors might force the DV9 Wellness Pvt. Ltd. to change its Marketing plan. The decision of the DV9 Wellness Pvt. Ltd. regarding these changes will be final and binding. Whenever such changes occur, they will be communicated through notifications posted on the website. These notifications will hold legal significance and will apply to all the Direct Seller. However, if any Direct Seller disagrees with and does not wish to be bound by these changes, they have the option to terminate this agreement within 30 days of such notification. To do so, the Direct Seller must provide a written notice expressing their objections to the DV9 Wellness Pvt. Ltd. If a Direct Seller continues their involvement in the Direct Selling business without submitting objections, it will be presumed that they have accepted all modification and amendments to the terms and conditions for future activities. 
f.	That all payments and transactions shall be valued in India Rupees (INR).
g.	That the DV9 Wellness Pvt. Ltd. does not guarantee / assure / promise or offer any facilitation fees or any amount or quantum of income whatsoever to the Direct Seller on account of becoming a Direct Seller of the DV9 Wellness Pvt. Ltd.
h.	That Sales Incentives to the Direct Seller shall be subject to all statutory deductions as applicable like TDS etc. 
i.	That Sales Incentive accrued and paid to the Direct Seller is inclusive of all taxes.
16.	That the DV9 Wellness Pvt. Ltd. shall provide accurate and complete information to prospective and existing Direct Seller concerning the reasonable amount of earning opportunity and related rights and obligations.
17.	That DV9 Wellness Pvt. Ltd. does not require a Direct Seller to maintain an office or establishment in furtherance of his / her entrepreneurship and if a Direct Seller does so then he / she himself / herself will be responsible to bear such expenses and the DV9 Wellness Pvt. Ltd. will in no way be responsible to refund or reimburse the same.
18.	The Direct Seller agrees that he/she shall exclusively focus on selling the products and services offered by DV9 Wellness Pvt. Ltd. The Direct Seller shall not, during the tenure of this contract, directly or indirectly sell, market, promote, or engage in the sale of any products that are similar or identical to those offered by DV9 Wellness Pvt. Ltd., whether for any other company, brand, or individual. Further, the Direct Seller agrees that he/she shall not promote, represent, associate with, or engage in any manner with any other direct selling entity or direct selling opportunity during the validity of this contract.
19.	That Unique Identification Number will have to be quoted by the Direct Seller in all his / her transactions and correspondence with the DV9 Wellness Pvt. Ltd. The Unique Identification Number once allotted cannot be altered at any point of time. That no communication will be entertained without Unique Identification Number and password. Direct Seller shall preserve the Unique Identification Number and Password properly as it is must for logging on to the website of the entity herein.
20.	That the Direct Seller shall be faithful to the DV9 Wellness Pvt. Ltd. and shall uphold the integrity and decorum to the DV9 Wellness Pvt. Ltd. and shall maintain good relations with another Direct Seller also.
21.	The Direct Seller is required to adhere to the policies, procedures, rules, and regulations established by the DV9 Wellness Pvt. Ltd. Additionally, they must comply with all applicable laws, rules, regulations, directives, and mandates issued by the Government of India, State Governments, Local bodies, Court of Law, and local administrations. Furthermore, the Direct Seller must refrain from engaging in any deceptive or unlawful trade practices, including Mis-Selling or unfair trade practices as outlined in clause 3 (f, g, and i), as defined in the Direct Selling Rules, 2021, and clauses 2(1), (18), (20), (41) to (43), and (47) of the Consumer Protection Act, 2019. In the event that the Direct Seller does engage in such activities, they will bear full responsibility for the consequences and outcomes thereof.   
22.	The Direct Seller has a responsibility to present, display, explain the DV9 Wellness Pvt. Ltd. Marketing Plan to potential prospects exactly as they received it from the DV9 Wellness Pvt. Ltd. If the Direct Selling entity observes that the Direct Seller is functioning in a manner that goes against the stipulated guidelines or authorization of the DV9 Wellness Pvt. Ltd., the entity holds the exclusive authority to either terminate the Direct Seller’s involvement or restrict their participation in the business, regardless of whether a show cause notice is provided or not.
23.	The DV9 Wellness Pvt. Ltd. holds the authority to make changes to the terms & conditions, products, DV9 Wellness Pvt. Ltd. Marketing Plan, and policies, whether with or without prior notice. Such notifications may be communicated through the official website of the Direct Selling Entity. Any modifications or amendments will come into effect and be binding for the Direct Seller starting from the date of the respective notice.
24.	That the Direct Seller is personally liable for delivery of goods to its customers. He is also liable to collect products from where it reaches last by the transporter / courier.
25.	That the Direct Seller is prohibited from mentioning / posting / telecasting any inappropriate or defaming content about the DV9 Wellness Pvt. Ltd., its products, etc. in any social media platforms. If he / she does any act in contravention to this clause, then this contract agreement will be deemed terminated and the DV9 Wellness Pvt. Ltd. reserves rights to initiate appropriate legal action against him / her.   
26.	That only one Direct Seller code shall be issued on one PAN Card.
27.	That the Direct Seller hereby undertakes not to compel or induce or mislead any person with any false statement / promise to purchase products from the DV9 Wellness Pvt. Ltd. or to become Direct Seller.
28.	All statutory changes will be in force with immediate effect or as per the law prescribed.
29.	The Direct Seller agrees and grants authorization to the DV9 Wellness Pvt. Ltd. to generate their sales and purchase records, which will include information about products, prices, taxes, quantities, and other details related to the items they have sold. These records will be created in accordance with the applicable laws and regulations.
30.	The Direct Selling entity bears the responsibility for ensuring the quality of products and services that the Direct Seller sells. Additionally, the DV9 Wellness Pvt. Ltd. is obligated to provide guidance to the Direct Seller to uphold the best practices that safeguard consumer interests. This guidance should be provided within the legal and ethical boundaries. If a Direct Seller chooses to operate outside the established policies and guidance of the DV9 Wellness Pvt. Ltd., they will be held individually accountable for all their actions related to the sales of products and services.
31.	Any notices or communications directed to the Direct Seller registered address, provided E-mail ID and mobile number mentioned in the registration form, whether sent through registered post, courier service, E-mail, or WhatsApp message, will be considered as officially delivered to the intended recipient. However, it is strongly recommended that Direct Seller promptly informs the DV9 Wellness Pvt. Ltd. of any alterations to their address, E-mail ID, or mobile number. Failing to do so will render any claims of non-delivery by the Direct Seller invalid under any circumstances.
32.	The term of this E-contract agreement is at will, subject to earlier termination in accordance with this E-contract agreement or in accordance with law. If this E-Contract Agreement is terminated for any reason whatsoever, the Direct Seller understands that his / her right to sell the products and receiving incentives with respect of his / her activities as a Direct Seller will cease immediately. DV9 Wellness Pvt. Ltd. reserves the right to terminate this E-contract agreement if any condition(s) of this E-Contract Agreement are violated by a Direct Seller. 
33.	Limitation of Action: If a Direct Seller wishes to bring any grievance to the notice of the DV9 Wellness Pvt. Ltd. he can do so as per the “Grievance Redressal Mechanism” annexed to this agreement may be read as part and parcel of this agreement as the same is not being reproduced here for the sake of brevity.    
34.	Indemnification: That the Direct Seller agrees to protect, defend, indemnify, and hold harmless DV9 Wellness Pvt. Ltd. and its employees, officers, directors, agents, or representatives from and against any and all liabilities, damages, fines, penalties, and costs (including legal costs and disbursements) arising from or relating to:
a.	Any breach of any statute, regulation, direction, orders, or standards notified by any governmental body, agency, or regulator applicable to the Direct Seller including payment and deposit of taxes; on account of Income tax, GST, Trade tax, Professional Tax, whenever applicable and shall obtain necessary registrations / licenses whenever applicable and required under law.
b.	Any breach of the terms and conditions of this E-contract agreement by the
Direct Seller,
c.	Any claim of any infringement of any intellectual property right or any other right of any third party or of law by the Direct Seller; or
d.	Against all matters of embezzlement, misappropriation or misapplications of collection / moneys which may from time to time during the continuance of the Agreement come into his / her / its possession / control.
35.	Relationship: The Direct Seller acknowledges that they function as an independently owned business entity. This Agreement does not establish them as an employee, associate, agent, or legal representative of the DV9 Wellness Pvt. Ltd. for any purpose. The Direct Seller has no explicit or implicit authorization or authority to take on obligations on behalf of the DV9 Wellness Pvt. Ltd. or to act in any way that would legally bind the entity. If a Direct Seller breaches this provision in any manner, they will be held accountable for all types of consequences, including financial, statutory, civil, or criminal implications.
36.	Liability: Except for the provisions stated in this Agreement, the DV9 Wellness Pvt. Ltd. holds no liability towards the Direct Seller for terminating this Agreement for any reason. This includes claims for loss or profit or any claims related to expenditures, investments, leases, capital investments, or other commitments undertaken by the other party in connection with the business, which were made based on or due to this Agreement. 
37.	Suspension, Revocation or Termination of this E-contract agreement:
a.	That the DV9 Wellness Pvt. Ltd. reserves the right to suspend the operation of this E-contract agreement, at any time, due to change in its own license conditions or upon directions from the competent government authorities. In such a situation, DV9 Wellness Pvt. Ltd. shall not be responsible for any damage or loss caused or arisen out of aforesaid action.
b.	If the Direct Seller breaches any of the terms outlined in this agreement, which they have previously accepted, the DV9 Wellness Pvt. Ltd. reserves the right to act. Without diminishing other possible remedies, the entity can issue a written notice with a one-month notice period. This notice will request the Direct Seller to provide a written explanation for their actions. If the explanation is not provided or is deemed inadequate based on standard business norms, the DV9 Wellness Pvt. Ltd. holds the authority to suspend, block or terminate the Direct Seller’s participation in the business. Consequently, the Direct Seller’s commissions will be discontinued.
c.	That the Direct Seller may terminate this agreement at any time by giving a written notice of 30 days to the DV9 Wellness Pvt. Ltd. at the head office of the DV9 Wellness Pvt. Ltd.
38.	Actions pursuant to Suspension / Blocking / Termination of this E-contract agreement: That notwithstanding any other rights and remedies provided elsewhere in the agreement, upon termination of this agreement:
a.	The Direct Seller shall not represent the DV9 Wellness Pvt. Ltd. in any of its dealings.
b.	The Direct Seller shall not intentionally or otherwise commit any act(s) as would keep a third party to believe that the DV9 Wellness Pvt. Ltd. is still having Direct Selling agreement with the Direct Seller.
c.	The Direct Seller shall stop using the DV9 Wellness Pvt. Ltd. ’s name, trademark, logo, etc., in any audio or visual form.
d.	All obligations and liabilities of such Direct Seller to the DV9 Wellness Pvt. Ltd. existing on the date having accrued during the validity of this Agreement will have to be fulfilled, met, and satisfied by the Direct Seller in every manner whatsoever. 
39.	Governing Laws and Regulations
a.	That this Agreements shall be governed by the provisions of the Indian Contract Act, 1872, the Consumer Protection Act, 2019, Consumer Protection (Direct Selling} Rules, 2021 or other laws of the land.  
40.	Dispute Settlement: The Direct Seller herein agrees and accepts that the remedial action available to him / her in the event of any interpretation of any question of law, dispute or difference arising under this agreement or in connection there-with (except as to the matters, the decision to which is specifically provided under this agreement), the same shall be as under:
a.	As per the Grievance Redressal Mechanism offered by the DV9 Wellness Pvt. Ltd. herein and forming part of this contract agreement, any dispute or difference arising out of or in connection with this Direct Seller/Direct Seller Agreement shall first be attempted to be resolved through mutual discussions within 30 (thirty) days of such dispute arising. In the event the Direct Seller/Direct Seller is not satisfied with the decision of the DV9 Wellness Pvt. Ltd. or if any issue raised remains unresolved for more than two months, the matter shall be referred to the Grievance Redressal Committee constituted by the DV9 Wellness Pvt. Ltd.
b.	Disputes, if any, remaining unresolved even after reference to the Grievance Redressal Committee, shall be finally resolved through binding arbitration in accordance with the provisions of the Indian Arbitration and Conciliation Act, 1996. The venue of arbitration shall be at Indore (M.P.), and the decision of the Arbitrator shall be final and binding on all parties. Subject to the foregoing, the courts at Indore (M.P.), have exclusive jurisdiction over matters arising out of or in relation to this agreement.

41.	Force- Majeure: That if at any time, during the continuance of this agreement, the performance in whole or in part, by the DV9 Wellness Pvt. Ltd. , of any obligation under this is prevented or delayed, by reason of war, or hostility, acts of the public enemy, civic commotion, sabotage, Act of State or direction from Statutory Authority, explosion, epidemic, quarantine restriction, strikes and lockouts, fire, floods, natural calamities / disaster or any act of God (hereinafter referred to as event), neither party shall, by reason of such event, be entitled to terminate this agreement, nor shall either party have any such claims for damages against the other, in respect of such non-performance or delay in performance. Provided that the services under this agreement shall be resumed as soon as practicable, after such event comes to an end or ceases to exist.
42.	The Direct Seller hereby agrees as under:
a.	That he / she has clearly understood the terms and conditions, as well as the Marketing Plan of the DV9 Wellness Pvt. Ltd., along with it associated its limitations and provisions.   He / she confirms that he / she is not relying upon any representation or promises that are not set out in this E-contract agreement.
b.	That their association with the DV9 Wellness Pvt. Ltd. and all their undertakings as outlined in this agreement shall be regulated, in conjunction with this agreement, by the regulations and processes specified in the DV9 Wellness Pvt. Ltd. Marketing Plan accessible on the company website. The Direct Seller validates that they have either read through these documents or they have been read to them in a language they understand. He / she thereby agrees to be legally bound by the provisions stipulated in this agreement. 
c.	That he / she will function as an independent entity and will refrain from engaging in any actions that could result in misfeasance or malfeasance, causing liabilities or obligations of any kind upon the company.
d.	That all the information provided to the DV9 Wellness Pvt. Ltd. is accurate and truthful. The DV9 Wellness Pvt. Ltd. holds the sole right and freedom to take appropriate action against him / her if it is discovered that the information furnished to the DV9 Wellness Pvt. Ltd. was incorrect or false.
e.	That any violation of the terms and conditions outlined in this agreement can lead to the termination of this agreement, as per the procedures detailed within.
f.	That I am the individual concerned and am fully aware of the facts stated above. I voluntarily agree to be designated as a Direct Seller across India, in accordance with the terms and conditions contained within this agreement.
g.	That I have carefully read and understood the terms and conditions concerning the appointment of a Direct Seller by the company. I have also reviewed the company's official website, printed materials, brochures, and am convinced about the business. I am submitting my application to be appointed as a Direct Seller based on my personal choice.
h.	That I commit to adhering to the policies, procedures, rules, and regulations established by the Company. I confirm that I have read, been explained, and fully comprehended the content of the document outlining the policies and procedures for the appointment of a Direct Seller.

IN TOKEN OF HIS / HER AGREEING TO AND ACCEPTING ALL PROVISIONS OF THIS CONTRACT AGREEMENT SET HEREINABOVE, HE / SHE IS CLICKING ON THE “I AGREE” BUTTON GIVEN HEREIN.


								
I AGREE & ACCEPT



Name: _____________________________      Bank A/C No.: ___________________
S/O Shri.: ____________________________      IFSC Code: ______________________
Resident of: ____________________________________________________________
_________________
Pin Code: _____________
State: ________________________________
Pan No.: __________________________________________
Aadhar No.: _______________________________________
Name of the Bank & Branch: __________________________

AGREE & ACCEPT



NODAL OFFICER
Mr.: 
Address: Office 702/A, 7th Floor, Metro Tower, A.B. Road, Indore (M.P.)
Mob no.: 9307147691
E mail: dv9wellness@gmail.com
                             </textarea>

                                            <textarea style="width: 100%; height: 80px; text-align: left;" cols="5" rows="10"
                                                runat="server" visible="false" id="divTermVisionAllright" readonly="readonly">
   Direct Seller Contract Agreement 
   ... (same as original) ...
            </textarea>
                                        </div>

                                        <div class="form-group">
                                            <asp:CheckBox ID="chkterms" runat="server" onclick="DivOnOff();" TabIndex="53" />
                                            <font face="Verdana" color="#000000" size="1">
                                                <b>I Agree With
                   
                                                    <asp:Literal ID="ltrTerms" runat="server"></asp:Literal>
                                                </b>
                                            </font>
                                        </div>
                                    </div>

                                    <div class="col-12 d-flex gap-2" id="DivTerms" style="display: none">
                                        <asp:Button ID="CmdSave" runat="server" Text="Submit" CssClass="btn btn-primary"
                                            TabIndex="54" OnClick="CmdSave_Click" />
                                        &nbsp;
       
                                        <asp:Button ID="CmdCancel" runat="server" Text="Cancel" CssClass="btn btn-primary"
                                            ValidationGroup="eCancel" TabIndex="55" OnClick="CmdCancel_Click" Visible="false" />
                                    </div>
                                </div>

                            </div>



                        </div>
                    </div>
                </div>
            </div>
        </section>


    </div>


    <%-- ==================== JAVASCRIPT ==================== --%>

    <%-- ==================== JAVASCRIPT ==================== --%>
    <script type="text/javascript">

        // -------------------------------------------------------
        // Main function: Individual vs Company toggle
        // -------------------------------------------------------
        function handleRegistrationChange() {

            // Find selected value from RadioButtonList (rendered as table of radio inputs)
            var radios = document.querySelectorAll("input[name$='RbCategory']");
            var selectedValue = '';
            for (var i = 0; i < radios.length; i++) {
                if (radios[i].checked) {
                    selectedValue = radios[i].value;
                    break;
                }
            }

            // Elements
            var spnNameText = document.getElementById('spnNameText');
            var lblFatherLabel = document.getElementById('lblFatherLabel');
            var divCmbTypeCol = document.getElementById('divCmbTypeCol');
            var divFatherNameCol = document.getElementById('divFatherNameCol');
            var divAuthorizedPerson = document.getElementById('divAuthorizedPerson');
            var RegTypeDiv = document.getElementById('RegType');

            if (selectedValue === 'C') {
                // ---- COMPANY selected ----

                // Change "Name" label to "Firm Name"
                if (spnNameText) spnNameText.innerText = 'Firm Name';

                // Change Father/Husband label to Authorized Person Name
                if (lblFatherLabel)
                    lblFatherLabel.innerHTML = 'Authorized Person Name <span class="red">*</span>';

                // Hide S/O D/O W/O dropdown column and original father textbox
                if (divCmbTypeCol) divCmbTypeCol.style.display = 'none';
                if (divFatherNameCol) divFatherNameCol.style.display = 'none';

                // Show Authorized Person textbox
                if (divAuthorizedPerson) divAuthorizedPerson.style.display = 'block';

                // Show Registration Type
                if (RegTypeDiv) RegTypeDiv.style.display = 'block';

            } else {
                // ---- INDIVIDUAL selected ----

                // Restore "Name" label
                if (spnNameText) spnNameText.innerText = 'Name';

                // Restore Father/Husband label
                if (lblFatherLabel)
                    lblFatherLabel.innerHTML = "Father/Husband's Name <span class='red'>*</span>";

                // Show S/O D/O W/O dropdown and original father name textbox
                if (divCmbTypeCol) divCmbTypeCol.style.display = '';
                if (divFatherNameCol) divFatherNameCol.style.display = '';

                // Hide Authorized Person textbox
                if (divAuthorizedPerson) divAuthorizedPerson.style.display = 'none';

                // Hide Registration Type
                if (RegTypeDiv) RegTypeDiv.style.display = 'none';
            }
        }

        // Run on page load to set correct initial state
        document.addEventListener('DOMContentLoaded', function () {
            handleRegistrationChange();
        });

</script>


    <script>

        function checkPasswordMatch() {

            var pass = document.getElementById("TxtPasswd").value;
            var confirmPass = document.getElementById("TxtConfirmPasswd").value;
            var msg = document.getElementById("lblPassMsg");

            if (confirmPass.length === 0) {
                msg.innerHTML = "";
                return;
            }
            if (pass !== confirmPass) {
                msg.innerHTML = "Password not matched ❌";
                msg.style.color = "red";
                msg.style.fontWeight = "bold";
            }
            else {
                msg.innerHTML = "Password matched ✅";
                msg.style.color = "#1faf45";
                msg.style.fontWeight = "bolder";
            }
            //if (pass !== confirmPass) {
            //    msg.innerHTML = "Password not matched ❌";
            //}
            //else {
            //    msg.innerHTML = "Password matched ✅";
            //}
        }



    </script>
    <script>

        $(document).ready(function () {

            // Toggle Password
            $("#toggle_pwd").click(function () {

                $(this).toggleClass("fa-eye fa-eye-slash");

                var input = $("#TxtPasswd");

                if (input.attr("type") === "password")
                    input.attr("type", "text");
                else
                    input.attr("type", "password");

            });


            // Toggle Confirm Password
            $("#toggle_pwd1").click(function () {

                $(this).toggleClass("fa-eye fa-eye-slash");

                var input = $("#TxtConfirmPasswd");

                if (input.attr("type") === "password")
                    input.attr("type", "text");
                else
                    input.attr("type", "password");

            });

        });

    </script>
</asp:Content>

