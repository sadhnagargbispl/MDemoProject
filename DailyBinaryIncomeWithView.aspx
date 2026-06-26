<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="DailyBinaryIncomeWithView.aspx.cs" Inherits="DailyBinaryIncomeWithView" %>

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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Daily Payout Detail</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div id="DivSideA" runat="server" class="col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" CssClass="table datatable"
                                            AllowPaging="true" PageSize="10" OnPageIndexChanging="gv_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText=" S.No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Payout Date">
                                                    <ItemTemplate>
                                                        <%#Eval("Session")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Matching Income">
                                                    <ItemTemplate>
                                                        <%# Eval("BinaryIncome") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Direct Referral Income">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0);"
                                                            onclick='openIncomeModal("ViewRefIncome.aspx", "REFLEVEL", <%# Eval("SessId") %>, "Direct Referral Income");'
                                                            style="color: Blue; cursor: pointer;">
                                                            <%# Eval("SpillIncome") %>
</a>
                                                        <%--<a href="javascript:void(0);"
                                                            onclick='openIncomeModal("ViewRefIncome.aspx", "REFLEVEL", <%# Eval("SessId") %>);'
                                                            style="color: Blue; cursor: pointer;">
                                                            <%# Eval("SpillIncome") %>
                                                        </a>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sponsor Matching Income">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0);"
                                                            onclick='openIncomeModal("ViewLevelIncome.aspx", "LEVEL", <%# Eval("SessId") %>, "Sponsor Matching Income");'
                                                            style="color: Blue; cursor: pointer;">
                                                            <%# Eval("LevelIncome") %>
</a>
                                                        <%-- <a href="javascript:void(0);"
                                                            onclick='openIncomeModal("ViewLevelIncome.aspx", "LEVEL", <%# Eval("SessId") %>);'
                                                            style="color: Blue; cursor: pointer;">
                                                            <%# Eval("LevelIncome") %>
                                                        </a>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sponsor Level Income">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0);"
                                                            onclick='openIncomeModal("ViewRefIncome.aspx", "REF", <%# Eval("SessId") %>, "Sponsor Level Income");'
                                                            style="color: Blue; cursor: pointer;">
                                                            <%# Eval("MagicBinary") %>
</a>
                                                        <%--  <a href="javascript:void(0);"
                                                            onclick='openIncomeModal("ViewRefIncome.aspx", "REF", <%# Eval("SessId") %>);'
                                                            style="color: Blue; cursor: pointer;">
                                                            <%# Eval("MagicBinary") %>
                                                        </a>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reward Income">
                                                    <ItemTemplate>
                                                        <%# Eval("RewardInc") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Gross Income">
                                                    <ItemTemplate>
                                                        <%# Eval("NetIncome") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TDS Amount">
                                                    <ItemTemplate>
                                                        <%# Eval("TdsAmount") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Admin Charge">
                                                    <ItemTemplate>
                                                        <%# Eval("AdminCharge") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Deduction">
                                                    <ItemTemplate>
                                                        <%# Eval("TotalDeduction") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Net Income">
                                                    <ItemTemplate>
                                                        <%# Eval("NetIncome") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <div class="modal fade" id="incomeModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title" id="incomeModalTitle">Referral Income</h5>
                    <button type="button"
                        class="btn-close"
                        data-bs-dismiss="modal"
                        onclick="location.reload();">
                    </button>
                </div>

                <div class="modal-body p-0">
                    <iframe id="incomeFrame"
                        style="width: 100%; height: 400px; border: none;"></iframe>
                </div>

            </div>
        </div>
    </div>
    <script>
        function openIncomeModal(page, reftype, sessId, title) {

            const frame = document.getElementById("incomeFrame");
            const modalEl = document.getElementById("incomeModal");
            const modalTitle = document.getElementById("incomeModalTitle");

            if (!frame || !modalEl) {
                console.error("Modal or iframe not found");
                return;
            }

            // ✅ Modal title change dynamically
            if (modalTitle) {
                modalTitle.innerText = title;
            }

            const url =
                page +
                "?pagetype=" + encodeURIComponent(reftype) +
                "&SessId=" + encodeURIComponent(sessId);

            frame.src = "about:blank";
            frame.src = url;

            const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
            modal.show();
        }
        //function openIncomeModal(page, reftype, sessId) {
        //    const frame = document.getElementById("incomeFrame");
        //    const modalEl = document.getElementById("incomeModal");
        //    if (!frame || !modalEl) {
        //        console.error("Modal or iframe not found");
        //        return;
        //    }
        //    const url =
        //        page +
        //        "?pagetype=" + encodeURIComponent(reftype) +
        //        "&SessId=" + encodeURIComponent(sessId);

        //    frame.src = "about:blank";
        //    frame.src = url;

        //    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        //    modal.show();
        //}
    </script>
    <%--    <div class="modal fade" id="incomeModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Income Details</h5>
                    <button type="button"
                        class="btn-close"
                        data-bs-dismiss="modal"
                        onclick="location.reload();">
                    </button>
                </div>

                <div class="modal-body p-0">
                    <iframe id="incomeFrame"
                        style="width: 100%; height: 400px; border: none;"></iframe>
                </div>

            </div>
        </div>
    </div>

    <script>
        function openIncomeModal(page, reftype, sessId) {

            const frame = document.getElementById("incomeFrame");
            const modalEl = document.getElementById("incomeModal");

            if (!frame || !modalEl) {
                console.error("Modal or iframe not found");
                return;
            }

            const url =
                page +
                "?pagetype=" + encodeURIComponent(reftype) +
                "&SessId=" + encodeURIComponent(sessId);

            frame.src = "about:blank";
            frame.src = url;

            const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
            modal.show();
        }
    </script>--%>
</asp:Content>

