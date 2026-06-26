<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="KYCUpdate.aspx.cs" Inherits="KYCUpdate" %>

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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>KYC Detail</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <!-- Genex Business -->
                                    <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                                        <div class="profile-bar-simple red-border clearfix">
                                            <h6>
                                                <asp:Label ID="LblKycHead" runat="server" Text="KYC Detail"></asp:Label>
                                            </h6>
                                        </div>
                                        <div class="col-md-12">
                                            Dear
                            <%=Session["MemName"].ToString()%>
                                            <asp:HiddenField ID="hdnSessn" runat="server" />
                                            <asp:HiddenField ID="hdnIsCompany" runat="server" Value="0" />
                                            (<asp:Label ID="lblid" runat="server"></asp:Label>) , Update Your KYC (<asp:Label
                                                ID="LblIdproofText" runat="server"></asp:Label>)
                            <br />
                                        </div>
                                        <div class="col-md-8">
                                            <div class="profile-bar-simple red-border clearfix">
                                                <h6>
                                                    <asp:Label ID="LblAddressHead" runat="server" Text="Address Proof"></asp:Label>
                                   <% if (Session["CompId"] != null && Session["CompId"].ToString() == "1074")
                                       { %>
    (Same as document)
                                                    <% } %>

                                                </h6>
                                            </div>
                                            <div class="form-group">
                                                <label for="pwd">
                                                    Address:</label>
                                                <asp:TextBox ID="txtaddrs" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="pincode">
                                                    Pincode</label>
                                                <asp:TextBox ID="Txtpincode" runat="server" CssClass="form-control validate[required,custom[pincode]]"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="pwd">
                                                    State:</label>

                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                </asp:DropDownList>

                                                <asp:HiddenField ID="StateCode" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <label for="district">
                                                    District</label>
                                                <asp:HiddenField ID="HDistrictCode" runat="server" />
                                                <asp:TextBox ID="Txtdistrict" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="city">
                                                    City</label>
                                                <asp:TextBox ID="Txtcity" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                                <asp:HiddenField ID="HCityCode" runat="server" />
                                            </div>
                                            <div class="form-group " style="display: none;">
                                                <label for="inputdefault">
                                                    Area</label>
                                                <asp:DropDownList ID="DDlVillage" CssClass="form-control" runat="server" ValidationGroup="eInformation"
                                                    autocomplete="off" onchange="FnVillageChange(this.value);">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group" id="divVillage" style="display: none">
                                                <label for="inputdefault">
                                                    Area Name</label>
                                                <asp:TextBox ID="TxtVillage" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="addressproof">
                                                    Address Proof</label>
                                                <asp:DropDownList ID="DDLAddressProof" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>

                                            <%-- Company: har proof type ka alag textbox + file (alag columns). JS/server se sirf selected dikhe. --%>
                                            <div class="form-group comp-bill" id="divBillNo" runat="server" visible="false">
                                                <label>Bill No.</label>
                                                <asp:TextBox ID="txtBillNo" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                            </div>
                                            <div class="form-group comp-bill" id="divBillUpload" runat="server" visible="false">
                                                <label>Bill Upload: <span style="font-size:11px;color:#888;">(jpg/png/pdf)</span></label>
                                                <asp:FileUpload ID="FuBill" runat="server" CssClass="form-control" accept=".jpg,.jpeg,.png,.pdf" onchange="previewFile(this,'Bill')" />
                                                <asp:Label ID="LblBillImg" runat="server" Visible="false"></asp:Label>
                                                <div class="kyc-prev" id="prevBill" style="display:none; margin-top:10px; border:1.5px solid #4CAF50; border-radius:10px; padding:10px 12px; background:#f6fff6; align-items:center; gap:12px;">
                                                    <div style="position:relative; flex-shrink:0; cursor:pointer;" onclick="openBig('Bill')">
                                                        <img id="thBill" src="" alt="thumb" style="width:72px; height:72px; object-fit:cover; border-radius:8px; border:1px solid #c8e6c9; display:block;">
                                                        <span style="position:absolute; bottom:3px; right:3px; background:rgba(0,0,0,0.55); color:#fff; font-size:10px; border-radius:4px; padding:1px 5px;">&#128269; zoom</span>
                                                    </div>
                                                    <div style="flex:1; min-width:0;">
                                                        <span id="nmBill" style="display:block; color:#2e7d32; font-weight:600; font-size:13px; white-space:nowrap; overflow:hidden; text-overflow:ellipsis;"></span>
                                                        <span id="szBill" style="display:block; color:#888; font-size:11px; margin-top:2px;"></span>
                                                        <span style="font-size:11px; color:#4CAF50; margin-top:3px; display:block;">&#10004; Click image to view full size</span>
                                                    </div>
                                                    <button type="button" onclick="openBig('Bill')" style="background:#4CAF50; color:white; border:none; border-radius:6px; padding:6px 14px; font-size:12px; cursor:pointer;">View Details</button>
                                                    <button type="button" onclick="removePrev('Bill','<%=FuBill.ClientID%>')" style="background:#e53935; color:white; border:none; border-radius:50%; width:26px; height:26px; cursor:pointer; font-size:15px;">&#x2715;</button>
                                                </div>
                                            </div>

                                            <div class="form-group comp-gst" id="divGstNo" runat="server" visible="false">
                                                <label>GST No.</label>
                                                <asp:TextBox ID="txtGstNo" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                            </div>
                                            <div class="form-group comp-gst" id="divGstUpload" runat="server" visible="false">
                                                <label>GST Upload: <span style="font-size:11px;color:#888;">(jpg/png/pdf)</span></label>
                                                <asp:FileUpload ID="FuGst" runat="server" CssClass="form-control" accept=".jpg,.jpeg,.png,.pdf" onchange="previewFile(this,'Gst')" />
                                                <asp:Label ID="LblGstImg" runat="server" Visible="false"></asp:Label>
                                                <div class="kyc-prev" id="prevGst" style="display:none; margin-top:10px; border:1.5px solid #4CAF50; border-radius:10px; padding:10px 12px; background:#f6fff6; align-items:center; gap:12px;">
                                                    <div style="position:relative; flex-shrink:0; cursor:pointer;" onclick="openBig('Gst')">
                                                        <img id="thGst" src="" alt="thumb" style="width:72px; height:72px; object-fit:cover; border-radius:8px; border:1px solid #c8e6c9; display:block;">
                                                        <span style="position:absolute; bottom:3px; right:3px; background:rgba(0,0,0,0.55); color:#fff; font-size:10px; border-radius:4px; padding:1px 5px;">&#128269; zoom</span>
                                                    </div>
                                                    <div style="flex:1; min-width:0;">
                                                        <span id="nmGst" style="display:block; color:#2e7d32; font-weight:600; font-size:13px; white-space:nowrap; overflow:hidden; text-overflow:ellipsis;"></span>
                                                        <span id="szGst" style="display:block; color:#888; font-size:11px; margin-top:2px;"></span>
                                                        <span style="font-size:11px; color:#4CAF50; margin-top:3px; display:block;">&#10004; Click image to view full size</span>
                                                    </div>
                                                    <button type="button" onclick="openBig('Gst')" style="background:#4CAF50; color:white; border:none; border-radius:6px; padding:6px 14px; font-size:12px; cursor:pointer;">View Details</button>
                                                    <button type="button" onclick="removePrev('Gst','<%=FuGst.ClientID%>')" style="background:#e53935; color:white; border:none; border-radius:50%; width:26px; height:26px; cursor:pointer; font-size:15px;">&#x2715;</button>
                                                </div>
                                            </div>

                                            <div class="form-group comp-other" id="divOtherNo" runat="server" visible="false">
                                                <label>Other No.</label>
                                                <asp:TextBox ID="txtOtherNo" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                            </div>
                                            <div class="form-group comp-other" id="divOtherUpload" runat="server" visible="false">
                                                <label>Other Upload: <span style="font-size:11px;color:#888;">(jpg/png/pdf)</span></label>
                                                <asp:FileUpload ID="FuOther" runat="server" CssClass="form-control" accept=".jpg,.jpeg,.png,.pdf" onchange="previewFile(this,'Other')" />
                                                <asp:Label ID="LblOtherImg" runat="server" Visible="false"></asp:Label>
                                                <div class="kyc-prev" id="prevOther" style="display:none; margin-top:10px; border:1.5px solid #4CAF50; border-radius:10px; padding:10px 12px; background:#f6fff6; align-items:center; gap:12px;">
                                                    <div style="position:relative; flex-shrink:0; cursor:pointer;" onclick="openBig('Other')">
                                                        <img id="thOther" src="" alt="thumb" style="width:72px; height:72px; object-fit:cover; border-radius:8px; border:1px solid #c8e6c9; display:block;">
                                                        <span style="position:absolute; bottom:3px; right:3px; background:rgba(0,0,0,0.55); color:#fff; font-size:10px; border-radius:4px; padding:1px 5px;">&#128269; zoom</span>
                                                    </div>
                                                    <div style="flex:1; min-width:0;">
                                                        <span id="nmOther" style="display:block; color:#2e7d32; font-weight:600; font-size:13px; white-space:nowrap; overflow:hidden; text-overflow:ellipsis;"></span>
                                                        <span id="szOther" style="display:block; color:#888; font-size:11px; margin-top:2px;"></span>
                                                        <span style="font-size:11px; color:#4CAF50; margin-top:3px; display:block;">&#10004; Click image to view full size</span>
                                                    </div>
                                                    <button type="button" onclick="openBig('Other')" style="background:#4CAF50; color:white; border:none; border-radius:6px; padding:6px 14px; font-size:12px; cursor:pointer;">View Details</button>
                                                    <button type="button" onclick="removePrev('Other','<%=FuOther.ClientID%>')" style="background:#e53935; color:white; border:none; border-radius:50%; width:26px; height:26px; cursor:pointer; font-size:15px;">&#x2715;</button>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label for="idproof">
                                                    <asp:Label ID="LblAddresProof" runat="server"></asp:Label>
                                                </label>
                                                <asp:TextBox ID="TxtIdProofNo" CssClass="form-control validate[required]" runat="server"
                                                    MaxLength="15" AutoPostBack="false" OnTextChanged="TxtIdProofNo_TextChanged"
                                                    onblur="kyupCheckAadhaar(this)"></asp:TextBox>
                                            </div>

                                            <div class="form-group">
                                                <label for="email">
                                                    <asp:Label ID="LblFrontUpload" runat="server" Text="Front Address Proof Upload:"></asp:Label></label>
                                                <asp:FileUpload ID="Fuidentity" runat="server" CssClass="form-control validate[required]" onchange="previewFile(this,'Front')" />
                                                <asp:RequiredFieldValidator ID="rfvImage" runat="server" ErrorMessage="Please Select  Front Address proof.!!"
                                                    Enabled="false" ControlToValidate="Fuidentity" ValidationGroup="eInformation">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblimage" runat="server" Visible="false"></asp:Label>
                                                <div class="kyc-prev" id="prevFront" style="display:none; margin-top:10px; border:1.5px solid #4CAF50; border-radius:10px; padding:10px 12px; background:#f6fff6; align-items:center; gap:12px;">
                                                    <div style="position:relative; flex-shrink:0; cursor:pointer;" onclick="openBig('Front')">
                                                        <img id="thFront" src="" alt="thumb" style="width:72px; height:72px; object-fit:cover; border-radius:8px; border:1px solid #c8e6c9; display:block;">
                                                        <span style="position:absolute; bottom:3px; right:3px; background:rgba(0,0,0,0.55); color:#fff; font-size:10px; border-radius:4px; padding:1px 5px;">&#128269; zoom</span>
                                                    </div>
                                                    <div style="flex:1; min-width:0;">
                                                        <span id="nmFront" style="display:block; color:#2e7d32; font-weight:600; font-size:13px; white-space:nowrap; overflow:hidden; text-overflow:ellipsis;"></span>
                                                        <span id="szFront" style="display:block; color:#888; font-size:11px; margin-top:2px;"></span>
                                                        <span style="font-size:11px; color:#4CAF50; margin-top:3px; display:block;">&#10004; Click image to view full size</span>
                                                    </div>
                                                    <button type="button" onclick="openBig('Front')" style="background:#4CAF50; color:white; border:none; border-radius:6px; padding:6px 14px; font-size:12px; cursor:pointer;">View Details</button>
                                                    <button type="button" onclick="removePrev('Front','<%=Fuidentity.ClientID%>')" style="background:#e53935; color:white; border:none; border-radius:50%; width:26px; height:26px; cursor:pointer; font-size:15px;">&#x2715;</button>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="pwd">
                                                    <asp:Label ID="LblBackUpload" runat="server" Text="Back Address Proof Upload:"></asp:Label></label>
                                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control validate[required]" onchange="previewFile(this,'Back')" />
                                                <asp:RequiredFieldValidator ID="rfvImage1" runat="server" ErrorMessage="Please Select  Back Address proof.!!"
                                                    Enabled="false" ControlToValidate="FileUpload1" ValidationGroup="eInformation">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="LblBackImage" runat="server" Visible="false"></asp:Label>
                                                <div class="kyc-prev" id="prevBack" style="display:none; margin-top:10px; border:1.5px solid #4CAF50; border-radius:10px; padding:10px 12px; background:#f6fff6; align-items:center; gap:12px;">
                                                    <div style="position:relative; flex-shrink:0; cursor:pointer;" onclick="openBig('Back')">
                                                        <img id="thBack" src="" alt="thumb" style="width:72px; height:72px; object-fit:cover; border-radius:8px; border:1px solid #c8e6c9; display:block;">
                                                        <span style="position:absolute; bottom:3px; right:3px; background:rgba(0,0,0,0.55); color:#fff; font-size:10px; border-radius:4px; padding:1px 5px;">&#128269; zoom</span>
                                                    </div>
                                                    <div style="flex:1; min-width:0;">
                                                        <span id="nmBack" style="display:block; color:#2e7d32; font-weight:600; font-size:13px; white-space:nowrap; overflow:hidden; text-overflow:ellipsis;"></span>
                                                        <span id="szBack" style="display:block; color:#888; font-size:11px; margin-top:2px;"></span>
                                                        <span style="font-size:11px; color:#4CAF50; margin-top:3px; display:block;">&#10004; Click image to view full size</span>
                                                    </div>
                                                    <button type="button" onclick="openBig('Back')" style="background:#4CAF50; color:white; border:none; border-radius:6px; padding:6px 14px; font-size:12px; cursor:pointer;">View Details</button>
                                                    <button type="button" onclick="removePrev('Back','<%=FileUpload1.ClientID%>')" style="background:#e53935; color:white; border:none; border-radius:50%; width:26px; height:26px; cursor:pointer; font-size:15px;">&#x2715;</button>
                                                </div>
                                            </div>
                                            <div class="profile-bar-simple red-border clearfix">
                                                <h6>
                                                    <asp:Label ID="LblBankHead" runat="server" Text="Bank Detail"></asp:Label><% if (Session["CompId"].ToString() != null && Session["CompId"].ToString() == "1074")
                                                                   { %>(Cancel cheque)<% } %></h6>
                                            </div>
                                            <div class="form-group" id="Accountype" runat="server" visible="false">
                                                <label for="inputdefault">
                                                    Account Type</label>
                                                <asp:DropDownList ID="DDLAccountType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="CHOOSE ACCOUNT TYPE" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="SAVING ACCOUNT" Value="SAVING ACCOUNT"></asp:ListItem>
                                                    <asp:ListItem Text="CURRENT ACCOUNT" Value="CURRENT ACCOUNT"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    Account No:</label>
                                                <asp:TextBox ID="Txtacno" runat="server" CssClass="form-control validate[required,custom[onlyNumberSp]]"
                                                    MaxLength="15"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    Bank:</label>
                                                <asp:DropDownList ID="cmbbank" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group" id="divBank" runat="server" visible="false">
                                                <label for="inputdefault">
                                                    Bank Name</label>
                                                <asp:TextBox ID="Txtbank" CssClass="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    Branch Name :</label>
                                                <asp:TextBox ID="Txtbranch" runat="server" CssClass="form-control validate[required,custom[onlyLetterNumberChar]]"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    IFSC Code :</label>
                                                <div class="form-group">
                                                    <asp:TextBox ID="Txtcode" runat="server" CssClass="form-control validate[required,custom[ifsccode]]"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    Bank KYC Upload</label>
                                            </div>
                                            <div class="form-group">
                                                <asp:FileUpload ID="BankKYCFileUpload3" runat="server" CssClass="form-control validate[required]" onchange="previewFile(this,'Bank')" />
                                                <asp:Label ID="LblBankImage" runat="server" Visible="false"></asp:Label>
                                                <div class="kyc-prev" id="prevBank" style="display:none; margin-top:10px; border:1.5px solid #4CAF50; border-radius:10px; padding:10px 12px; background:#f6fff6; align-items:center; gap:12px;">
                                                    <div style="position:relative; flex-shrink:0; cursor:pointer;" onclick="openBig('Bank')">
                                                        <img id="thBank" src="" alt="thumb" style="width:72px; height:72px; object-fit:cover; border-radius:8px; border:1px solid #c8e6c9; display:block;">
                                                        <span style="position:absolute; bottom:3px; right:3px; background:rgba(0,0,0,0.55); color:#fff; font-size:10px; border-radius:4px; padding:1px 5px;">&#128269; zoom</span>
                                                    </div>
                                                    <div style="flex:1; min-width:0;">
                                                        <span id="nmBank" style="display:block; color:#2e7d32; font-weight:600; font-size:13px; white-space:nowrap; overflow:hidden; text-overflow:ellipsis;"></span>
                                                        <span id="szBank" style="display:block; color:#888; font-size:11px; margin-top:2px;"></span>
                                                        <span style="font-size:11px; color:#4CAF50; margin-top:3px; display:block;">&#10004; Click image to view full size</span>
                                                    </div>
                                                    <button type="button" onclick="openBig('Bank')" style="background:#4CAF50; color:white; border:none; border-radius:6px; padding:6px 14px; font-size:12px; cursor:pointer;">View Details</button>
                                                    <button type="button" onclick="removePrev('Bank','<%=BankKYCFileUpload3.ClientID%>')" style="background:#e53935; color:white; border:none; border-radius:50%; width:26px; height:26px; cursor:pointer; font-size:15px;">&#x2715;</button>
                                                </div>
                                            </div>
                                            <div class="profile-bar-simple red-border clearfix">
                                                <h6>
                                                    <asp:Label ID="LblPanHead" runat="server" Text="PAN Card Detail"></asp:Label>
                                                </h6>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <div class="form-group">
                                                        <label for="inputdefault">
                                                            Pan Card No. :</label>
                                                        <%--AutoPostBack ="true"--%>
                                                        <asp:TextBox ID="txtpan" runat="server" CssClass="form-control validate[required,custom[panno]]"
                                                            MaxLength="10" style="text-transform:uppercase;"
                                                            AutoPostBack="false" OnTextChanged="txtpan_TextChanged"
                                                            onblur="kyupCheckPan(this)"></asp:TextBox>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtpan" EventName="TextChanged" />
                                                    <%-- <asp:AsyncPostBackTrigger ControlID="BtnIdentity" EventName="Click" />--%>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <div class="form-group">
                                                <label for="inputdefault">
                                                    PanCard Upload :</label>
                                                <asp:FileUpload ID="Pankyc" runat="server" CssClass="form-control validate[required]" onchange="previewFile(this,'Pan')" />
                                                <asp:Label ID="LblPanImage" runat="server" Visible="false"></asp:Label>
                                                <div class="kyc-prev" id="prevPan" style="display:none; margin-top:10px; border:1.5px solid #4CAF50; border-radius:10px; padding:10px 12px; background:#f6fff6; align-items:center; gap:12px;">
                                                    <div style="position:relative; flex-shrink:0; cursor:pointer;" onclick="openBig('Pan')">
                                                        <img id="thPan" src="" alt="thumb" style="width:72px; height:72px; object-fit:cover; border-radius:8px; border:1px solid #c8e6c9; display:block;">
                                                        <span style="position:absolute; bottom:3px; right:3px; background:rgba(0,0,0,0.55); color:#fff; font-size:10px; border-radius:4px; padding:1px 5px;">&#128269; zoom</span>
                                                    </div>
                                                    <div style="flex:1; min-width:0;">
                                                        <span id="nmPan" style="display:block; color:#2e7d32; font-weight:600; font-size:13px; white-space:nowrap; overflow:hidden; text-overflow:ellipsis;"></span>
                                                        <span id="szPan" style="display:block; color:#888; font-size:11px; margin-top:2px;"></span>
                                                        <span style="font-size:11px; color:#4CAF50; margin-top:3px; display:block;">&#10004; Click image to view full size</span>
                                                    </div>
                                                    <button type="button" onclick="openBig('Pan')" style="background:#4CAF50; color:white; border:none; border-radius:6px; padding:6px 14px; font-size:12px; cursor:pointer;">View Details</button>
                                                    <button type="button" onclick="removePrev('Pan','<%=Pankyc.ClientID%>')" style="background:#e53935; color:white; border:none; border-radius:50%; width:26px; height:26px; cursor:pointer; font-size:15px;">&#x2715;</button>
                                                </div>
                                            </div>
                                            <asp:Button ID="BtnIdentity" runat="server" ValidationGroup="eInformation" CssClass="btn btn-primary"
                                                Text="submit" OnClientClick="return SaveButton();" OnClick="BtnIdentity_Click" />
                                        </div>
                                    </div>
                                    <!-- end dashboards/dashboard -->
                                </div>
                                <div class="col-md-4">
                                    <!-- Genex Business -->
                                    <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                                        <div class="profile-bar-simple red-border clearfix">
                                            <h6>Uploaded Images
                                            </h6>
                                        </div>
                                        <div class="col-md-12">
                                            <%-- <div class="col-md-6">
                                <div class="image">--%>

                                            <script src="popupassets/popper.min.js"></script>

                                            <script src="popupassets/lib.js"></script>

                                            <script src="popupassets/jquery.flagstrap.min.js"></script>

                                            <script type="text/javascript" src="popupassets/jquery.themepunch.tools.min.js"></script>

                                            <script type="text/javascript" src="popupassets/jquery.themepunch.revolution.min.js"></script>

                                            <script src="js/functions1.js"></script>

                                            <%-- Company: proof uploaded files (jis type me data ho wahi dikhe) --%>
                                            <div class="col-md-12" id="divBillImg" runat="server" visible="false">
                                                Light Bill<br />
                                                <a id="BillAnchor" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="imgBill" runat="server" Width="150px" Height="150px" />
                                                </a>
                                            </div>
                                            <div class="col-md-12" id="divGstImg" runat="server" visible="false">
                                                GST Certificate<br />
                                                <a id="GstAnchor" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="imgGst" runat="server" Width="150px" Height="150px" />
                                                </a>
                                            </div>
                                            <div class="col-md-12" id="divOtherImg" runat="server" visible="false">
                                                Other Proof<br />
                                                <a id="OtherAnchor" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="imgOther" runat="server" Width="150px" Height="150px" />
                                                </a>
                                            </div>

                                            <div class="col-md-12">
                                                <asp:Label ID="LblImgFront" runat="server" Text="Front Address Proof"></asp:Label><br />
                                                <a id="FrontAddress" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="ShowIdentity" runat="server" Width="150px" Height="150px" />
                                                </a>
                                            </div>

                                            <div class="col-md-12">
                                                <asp:Label ID="LblImgBack" runat="server" Text="Back Address Proof"></asp:Label><br />
                                                <a id="BackAddress" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="showBackImage" runat="server" Width="150px" Height="150px" />
                                                </a>
                                            </div>

                                            <div class="col-md-12">
                                                Bank Address Proof<br />
                                                <a id="BankProof" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="bANKiMAGE" runat="server" Width="150px" Height="150px" />
                                                </a>
                                            </div>

                                            <div class="col-md-12">
                                                Pan Card<br />
                                                <a id="PanCard" runat="server" onclick="return openPopup(this)">
                                                    <asp:Image ID="pANiMAGE" runat="server" Width="150px" Height="150px" />
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
    <!-- KYC upload bada preview (walletrequest style overlay) -->
    <div id="kycModalBg" style="display:none; position:fixed; inset:0; background:rgba(0,0,0,0.72); z-index:99999; align-items:center; justify-content:center; padding:20px;">
        <div style="background:#fff; border-radius:12px; width:100%; max-width:600px; overflow:hidden; border:0.5px solid #ddd;">
            <div style="display:flex; align-items:center; justify-content:space-between; padding:10px 14px; border-bottom:0.5px solid #eee;">
                <span style="font-size:13px; font-weight:600;">Document Preview</span>
                <button type="button" onclick="closeBig()" style="background:none; border:none; cursor:pointer; font-size:18px; color:#888;">&#x2715;</button>
            </div>
            <div style="padding:12px; text-align:center; background:#111;">
                <img id="kycBigImg" src="" alt="Preview" style="max-width:100%; max-height:70vh; border-radius:8px; object-fit:contain; display:block; margin:0 auto;">
            </div>
        </div>
    </div>

    <div class="modal fade" id="imagePreviewModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Image Preview</h5>
                    <button type="button"
                        class="btn-close"
                        data-bs-dismiss="modal">
                    </button>
                </div>

                <div class="modal-body text-center">
                    <img id="modalImage"
                        class="img-fluid"
                        style="max-height: 500px;" />
                </div>

            </div>
        </div>
    </div>
    <script>
        // ===== AJAX (onblur) duplicate check - no postback (KYC.aspx jaisa) =====
        function kyupCheckAadhaar(el) {
            var v = (el.value || '').trim();
            if (v === '') return;
            fetch('KYCUpdate.aspx/VerifyAadhaar', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify({ idProofNo: v })
            })
                .then(function (r) { return r.json(); })
                .then(function (res) {
                    var btn = document.getElementById('<%=BtnIdentity.ClientID%>');
                    if (res.d === false) {
                        alert('Aadhaar Card already registered with another ID.');
                        el.value = ''; el.focus();
                        if (btn) btn.disabled = true;
                    } else {
                        if (btn) btn.disabled = false;
                    }
                })
                .catch(function () { /* network error - ignore */ });
        }
        function kyupCheckPan(el) {
            var v = (el.value || '').trim().toUpperCase();
            if (v === '') return;
            el.value = v;
            fetch('KYCUpdate.aspx/panverifyC', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify({ txtpan: v })
            })
                .then(function (r) { return r.json(); })
                .then(function (res) {
                    var btn = document.getElementById('<%=BtnIdentity.ClientID%>');
                    if (res.d === false) {
                        alert('PAN Card already registered with another ID.');
                        el.value = ''; el.focus();
                        if (btn) btn.disabled = true;
                    } else {
                        if (btn) btn.disabled = false;
                    }
                })
                .catch(function () { /* network error - ignore */ });
        }
    </script>
    <script>

        function SaveButton() {

            if ($("#<%=txtaddrs.ClientID%>").val().trim() == "") {
                alert("Please enter Address");
                $("#<%=txtaddrs.ClientID%>").focus();
                return false;
            }

            if ($("#<%=Txtpincode.ClientID%>").val().trim() == "") {
                alert("Please enter Pincode");
                $("#<%=Txtpincode.ClientID%>").focus();
                return false;
            }


            if ($("#<%=Txtdistrict.ClientID%>").val().trim() == "") {
                alert("Please enter District");
                $("#<%=Txtdistrict.ClientID%>").focus();
                return false;
            }

            if ($("#<%=Txtcity.ClientID%>").val().trim() == "") {
                alert("Please enter City");
                $("#<%=Txtcity.ClientID%>").focus();
                return false;
            }

            if ($("#<%=TxtIdProofNo.ClientID%>").val().trim() == "") {
                alert("Please enter Address Proof Number");
                $("#<%=TxtIdProofNo.ClientID%>").focus();
                return false;
            }

            if ($("#<%=Fuidentity.ClientID%>").val() == "") {
                alert("Please upload Front Address Proof");
                $("#<%=Fuidentity.ClientID%>").focus();
                return false;
            }

            if ($("#<%=FileUpload1.ClientID%>").val() == "") {
                alert("Please upload Back Address Proof");
                $("#<%=FileUpload1.ClientID%>").focus();
                return false;
            }

            if ($("#<%=Txtacno.ClientID%>").val().trim() == "") {
                alert("Please enter Account Number");
                $("#<%=Txtacno.ClientID%>").focus();
                return false;
            }

            if ($("#<%=cmbbank.ClientID%>").val() == "" || $("#<%=cmbbank.ClientID%>").val() == "0") {
                alert("Please select Bank");
                $("#<%=cmbbank.ClientID%>").focus();
                return false;
            }

            if ($("#<%=Txtbranch.ClientID%>").val().trim() == "") {
                alert("Please enter Branch Name");
                $("#<%=Txtbranch.ClientID%>").focus();
                return false;
            }

            if ($("#<%=Txtcode.ClientID%>").val().trim() == "") {
                alert("Please enter IFSC Code");
                $("#<%=Txtcode.ClientID%>").focus();
                return false;
            }

            if ($("#<%=BankKYCFileUpload3.ClientID%>").val() == "") {
                alert("Please upload Bank KYC");
                $("#<%=BankKYCFileUpload3.ClientID%>").focus();
                return false;
            }

            if ($("#<%=txtpan.ClientID%>").val().trim() == "") {
                alert("Please enter PAN Card Number");
                $("#<%=txtpan.ClientID%>").focus();
                return false;
            }

            // PAN format: 5 letters + 4 digits + 1 letter (e.g. ABCDE1234F)
            var panNo = $("#<%=txtpan.ClientID%>").val().trim().toUpperCase();
            if (!/^[A-Z]{5}[0-9]{4}[A-Z]$/.test(panNo)) {
                alert("Invalid PAN Number. Format: ABCDE1234F");
                $("#<%=txtpan.ClientID%>").focus();
                return false;
            }
            $("#<%=txtpan.ClientID%>").val(panNo);

            if ($("#<%=Pankyc.ClientID%>").val() == "") {
                alert("Please upload PAN Card");
                $("#<%=Pankyc.ClientID%>").focus();
                return false;
            }

            return true;

        }

    </script>
    <script>
        // Company: Address Proof dropdown change -> sirf selected type ke fields dikhao + baaki clear
        function onCompProofChange(dd) {
            var t = dd.options[dd.selectedIndex].text.trim().toUpperCase();
            var bill = (t === 'LIGHT BILL'), gst = (t === 'GSTIN'), oth = (!bill && !gst);
            compShow('comp-bill', bill);
            compShow('comp-gst', gst);
            compShow('comp-other', oth);
            // jo type select nahi hai uska No + file + preview blank
            if (!bill) clearComp('Bill', '<%=txtBillNo.ClientID%>', '<%=FuBill.ClientID%>');
            if (!gst) clearComp('Gst', '<%=txtGstNo.ClientID%>', '<%=FuGst.ClientID%>');
            if (!oth) clearComp('Other', '<%=txtOtherNo.ClientID%>', '<%=FuOther.ClientID%>');
        }
        function compShow(cls, vis) {
            var e = document.getElementsByClassName(cls);
            for (var i = 0; i < e.length; i++) { e[i].style.display = vis ? '' : 'none'; }
        }
        // No textbox + file input + preview clear (dropdown switch par)
        function clearComp(key, txtId, fileId) {
            var tx = document.getElementById(txtId); if (tx) tx.value = '';
            removePrev(key, fileId);
        }
        // Generic upload preview (walletrequest jaisa): thumbnail + name + size + zoom
        var PDF_ICON = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0naHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmcnIHZpZXdCb3g9JzAgMCA2NCA2NCc+PHJlY3QgeD0nMTInIHk9JzQnIHdpZHRoPSczNicgaGVpZ2h0PSc1Nicgcng9JzQnIGZpbGw9JyNmZmZmZmYnIHN0cm9rZT0nI2U1MzkzNScgc3Ryb2tlLXdpZHRoPSczJy8+PHBhdGggZD0nTTQwIDQgbDggOCBoLTggeicgZmlsbD0nI2U1MzkzNScvPjxyZWN0IHg9JzgnIHk9JzM0JyB3aWR0aD0nNDAnIGhlaWdodD0nMTYnIHJ4PScyJyBmaWxsPScjZTUzOTM1Jy8+PHRleHQgeD0nMjgnIHk9JzQ2JyBmb250LWZhbWlseT0nQXJpYWwsSGVsdmV0aWNhLHNhbnMtc2VyaWYnIGZvbnQtc2l6ZT0nMTEnIGZvbnQtd2VpZ2h0PSdib2xkJyBmaWxsPScjZmZmZmZmJyB0ZXh0LWFuY2hvcj0nbWlkZGxlJz5QREY8L3RleHQ+PC9zdmc+";
        function previewFile(input, key) {
            var pv = document.getElementById('prev' + key);
            var th = document.getElementById('th' + key);
            var nm = document.getElementById('nm' + key);
            var sz = document.getElementById('sz' + key);
            if (!input.files || !input.files[0]) { if (pv) pv.style.display = 'none'; return; }
            var f = input.files[0];
            var url = URL.createObjectURL(f);
            var isPdf = /\.pdf$/i.test(f.name);
            var kb = f.size / 1024;
            if (th) {
                th.src = isPdf ? PDF_ICON : url;
                th.setAttribute('data-full', isPdf ? '' : url);
                th.setAttribute('data-pdf', isPdf ? url : '');
            }
            if (nm) nm.innerText = f.name;
            if (sz) sz.innerText = kb > 1024 ? (kb / 1024).toFixed(2) + ' MB' : kb.toFixed(1) + ' KB';
            if (pv) pv.style.display = 'flex';
        }
        // Bada preview modal: image zoom, pdf -> nayi tab
        function openBig(key) {
            var th = document.getElementById('th' + key);
            if (!th) return;
            var pdf = th.getAttribute('data-pdf');
            if (pdf) { window.open(pdf, '_blank'); return; }
            var full = th.getAttribute('data-full') || th.src;
            if (!full) return;
            document.getElementById('kycBigImg').src = full;
            document.getElementById('kycModalBg').style.display = 'flex';
        }
        function closeBig() {
            var m = document.getElementById('kycModalBg'); if (m) m.style.display = 'none';
        }
        function removePrev(key, fileId) {
            var fu = document.getElementById(fileId); if (fu) fu.value = '';
            var pv = document.getElementById('prev' + key); if (pv) pv.style.display = 'none';
            var th = document.getElementById('th' + key); if (th) { th.src = ''; th.removeAttribute('data-full'); th.removeAttribute('data-pdf'); }
            var nm = document.getElementById('nm' + key); if (nm) nm.innerText = '';
            var sz = document.getElementById('sz' + key); if (sz) sz.innerText = '';
        }
    </script>
    <script>
        function openPopup(anchor) {
            // anchor ka href lo (PDF ya image)
            var url = anchor.href || (anchor.getAttribute && anchor.getAttribute("href"));
            if (!url) {
                var im = anchor.querySelector("img");
                url = im ? im.src : "";
            }
            if (!url) return false;

            // PDF -> nayi tab me kholo
            if (/\.pdf(\?|$)/i.test(url)) {
                window.open(url, "_blank");
                return false;
            }

            var modalImg = document.getElementById("modalImage");
            modalImg.src = "";
            modalImg.src = url + (url.indexOf("?") > -1 ? "&" : "?") + "t=" + new Date().getTime();

            var modal = new bootstrap.Modal(document.getElementById("imagePreviewModal"));
            modal.show();
            return false; // page reload roke
        }
    </script>

</asp:Content>
