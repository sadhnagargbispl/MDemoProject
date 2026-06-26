using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

public partial class CompanyImages : System.Web.UI.Page
{
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetCompanyImage(string companyId)
    {
        var result = new { imageUrl = (string)null };

        try
        {
            string folder = HttpContext.Current.Server.MapPath("~/CompanyImages/" + companyId + "/");

            if (Directory.Exists(folder))
            {
                var files = Directory
                    .GetFiles(folder)
                    .Where(f =>
                        f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        f.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                if (files.Length > 0)
                {
                    string fileName = Path.GetFileName(files[0]);
                    result = new { imageUrl = "/CompanyImages/" + companyId + "/" + fileName };
                }
            }
        }
        catch (Exception ex)
        {
            // yahan chahe to logging kar sakte ho
        }

        return result;
    }
}
