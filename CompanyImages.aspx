
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyImages.aspx.cs" Inherits="CompanyImages" %>

<!DOCTYPE html>
<html>
<head>
    <title>Image Load on Dropdown Change</title>
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
</head>
<body>

<form id="form1" runat="server">   <!-- REQUIRED FOR PAGE METHODS -->

    <select id="ddlCompany">
        <option value="">Select...</option>
        <option value="1001">Company A</option>
        <option value="1002">Company B</option>
        <option value="1003">Company C</option>
    </select>

    <br><br>

    <img id="imgPreview" src="images/placeholder.png"
         style="width: 250px; border: 1px solid #ccc;" />

</form>

<script>
    $("#ddlCompany").change(function () {
        var compId = $(this).val();

        if (compId === "") {
            $("#imgPreview").attr("src", "images/placeholder.png");
            return;
        }

        $.ajax({
            url: "CompanyImages.aspx/GetCompanyImage",   // ✅ yahi sahi hai
            type: "POST",
            data: JSON.stringify({ companyId: compId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                if (res.d && res.d.imageUrl) {
                    $("#imgPreview").attr("src", res.d.imageUrl);
                } else {
                    $("#imgPreview").attr("src", "images/placeholder.png");
                }
            },
            error: function (xhr, status, err) {
                console.log("AJAX Error:", err);
                console.log(xhr.responseText);
            }
        });
    });
</script>


</body>
</html>
