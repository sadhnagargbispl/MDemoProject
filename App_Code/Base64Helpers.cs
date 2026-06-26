using System;
using System.Text;

public static class Base64Helpers
{
    public static string Base64Decode(string base64EncodedData)
    {
        if (string.IsNullOrEmpty(base64EncodedData))
            return string.Empty;

        try
        {
            byte[] bytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (FormatException)
        {
            // invalid base64 string
            return string.Empty;
        }
        catch (Exception)
        {
            // any other error
            return string.Empty;
        }
    }

    public static string Base64Encode(string plainText)
    {
        if (plainText == null)
            return string.Empty;

        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(bytes);
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}
