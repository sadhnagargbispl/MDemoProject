<%@ WebHandler Language="C#" Class="VoiceAI" %>

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Script.Serialization;

/// <summary>
/// Voice command parser for cpanel.BasicmlmCSharp (Mistral AI).
/// Receives { text, page, menus[] } from VoiceAssistant.js and returns ONE
/// strict intent JSON. The Mistral API key never leaves the server.
///
/// IMPORTANT: cpanel's sidebar menu is DB-driven, so the client sends the
/// LIVE menu item labels in "menus". For navigate, the model must return the
/// EXACT matching menu label in "page" - the front-end then opens that menu
/// item's real href. No hard-coded page/file map (that was the earlier bug).
/// </summary>
public class VoiceAI : IHttpHandler, IRequiresSessionState
{
    private const string MistralUrl = "https://api.mistral.ai/v1/chat/completions";

    // Base instructions; the live menu list is appended per-request.
    private const string SystemPromptBase = @"You are the voice command parser for an MLM member cPanel (Hindi+English users).
The user speaks in Hinglish (Hindi+English mix), Devanagari Hindi, or pure English.
Convert their spoken command into ONE strict JSON object. Output ONLY JSON, no markdown, no extra text.

Schema:
{
  ""intent"": ""navigate"" | ""filter"" | ""fill"" | ""guidedfill"" | ""unknown"",
  ""page"": ""<for navigate: the EXACT menu label from AVAILABLE MENU ITEMS that best matches; else empty>"",
  ""filters"": { ""query"": ""<free text to match table rows>"" },
  ""fields"": { ""<fieldName>"": ""<value>"" },
  ""speak"": ""<very short confirmation, max 8 words>"",
  ""speakLang"": ""hi"" | ""en""
}

LANGUAGE RULE (important):
- Devanagari Hindi, romanized Hindi/Hinglish (profile kholo), aur pure English (open profile) -
  teeno ko EQUALLY samjho. Script matter nahi karti.
- ""speakLang"" = user jis language/feel me bola usi me reply: mostly Hindi/Hinglish -> ""hi""
  aur ""speak"" Hinglish me. Pure English -> ""en"" aur ""speak"" English me.
- ""speak"" hamesha short. filters.query / fields values translate MAT karo (naam/mobile/amount as-is).

NAVIGATION (very important):
- For navigate, ALWAYS pick the closest item from the AVAILABLE MENU ITEMS list below and put
  its EXACT text (as listed) in ""page"". Do NOT invent file names or keys.
- Examples: ""profile kholo""/""open profile"" -> closest is likely ""Edit Profile"" or ""Profile"".
  ""edit profile"" -> ""Edit Profile"". ""new registration""/""naya registration"" -> ""New Registration"".
  ""login password change""/""password badlo"" -> ""Change Login Password"".
  ""transaction password"" -> ""Change Trans. Password"". ""kyc"" -> ""Upload KYC"".
  ""my team""/""meri team"" -> ""My Team"". ""wallet"" -> ""Wallet"". ""home""/""ghar"" -> ""Home"".
  ""logout""/""sign out"" -> ""Logout"" (or the closest logout item).
- If nothing in the list matches, intent=unknown with a short polite speak.

OTHER INTENTS:
- ""Rakesh dikhao"", ""filter 9001234567"" on a report/grid -> intent=filter, filters.query=<text>.
- Form bharo with values -> intent=fill, fields={...} (only fields the user said).
- ""step by step bharo"", ""ek ek karke poochho"", ""guided fill"" -> intent=guidedfill.
- Command clear nahi -> intent=unknown with a short polite speak in user's language.";

    private static readonly bool _tlsReady = EnsureTls();
    private static bool EnsureTls()
    {
        try { ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; }
        catch { }
        return true;
    }

    private const string Fallback =
        "{\"intent\":\"unknown\",\"speak\":\"Maaf kijiye, samajh nahi aaya\",\"speakLang\":\"hi\"}";

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.ContentEncoding = Encoding.UTF8;
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        string outJson;
        try
        {
            string body;
            using (var sr = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                body = sr.ReadToEnd();

            var ser = new JavaScriptSerializer();
            var input = string.IsNullOrWhiteSpace(body)
                ? new Dictionary<string, object>()
                : ser.Deserialize<Dictionary<string, object>>(body);

            string text  = GetStr(input, "text");
            string page  = GetStr(input, "page");
            string menus = JoinArr(input, "menus");

            if (string.IsNullOrEmpty(text))
            {
                context.Response.Write("{\"intent\":\"unknown\",\"speak\":\"Kuch sunai nahi diya\",\"speakLang\":\"hi\"}");
                return;
            }

            string key = ConfigurationManager.AppSettings["MistralApiKey"];
            if (string.IsNullOrWhiteSpace(key) || key == "PUT_KEY_HERE" || key == "YAHAN_APNI_MISTRAL_KEY_DAALEIN")
            {
                context.Response.Write("{\"intent\":\"unknown\",\"speak\":\"AI key configure nahi hai\",\"speakLang\":\"hi\"}");
                return;
            }
            key = key.Trim().Trim('"', '\'');
            if (key.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                key = key.Substring(7).Trim();

            string model = ConfigurationManager.AppSettings["MistralModel"];
            if (string.IsNullOrWhiteSpace(model)) model = "mistral-small-latest";

            string systemPrompt = SystemPromptBase +
                "\n\nAVAILABLE MENU ITEMS (match navigate target to one of these, return its EXACT text):\n" +
                (string.IsNullOrEmpty(menus) ? "(none provided)" : menus);

            string userMsg = "Current page file: " + page + "\nUser said: " + text;

            outJson = CallMistral(key, model, systemPrompt, userMsg, ser);
        }
        catch (Exception)
        {
            outJson = Fallback;
        }

        context.Response.Write(outJson);
    }

    private string CallMistral(string key, string model, string systemPrompt, string userMsg, JavaScriptSerializer ser)
    {
        var payload = new Dictionary<string, object>();
        payload.Add("model", model);
        payload.Add("temperature", 0.1);

        var responseFormat = new Dictionary<string, object>();
        responseFormat.Add("type", "json_object");
        payload.Add("response_format", responseFormat);

        var sysMsg = new Dictionary<string, object>();
        sysMsg.Add("role", "system");
        sysMsg.Add("content", systemPrompt);

        var usrMsg = new Dictionary<string, object>();
        usrMsg.Add("role", "user");
        usrMsg.Add("content", userMsg);

        payload.Add("messages", new object[] { sysMsg, usrMsg });

        byte[] bytes = Encoding.UTF8.GetBytes(ser.Serialize(payload));

        var req = (HttpWebRequest)WebRequest.Create(MistralUrl);
        req.Method = "POST";
        req.ContentType = "application/json";
        req.Accept = "application/json";
        req.Headers["Authorization"] = "Bearer " + key;
        req.Timeout = 20000;
        req.ReadWriteTimeout = 20000;
        req.ContentLength = bytes.Length;

        try
        {
            using (var rs = req.GetRequestStream())
                rs.Write(bytes, 0, bytes.Length);

            string respText;
            using (var resp = (HttpWebResponse)req.GetResponse())
            using (var rdr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                respText = rdr.ReadToEnd();

            string content = ExtractContent(respText, ser);
            if (string.IsNullOrWhiteSpace(content)) return Fallback;

            content = StripFences(content).Trim();
            if (!content.StartsWith("{")) return Fallback;
            return content;
        }
        catch (WebException)
        {
            return Fallback;
        }
    }

    private static string ExtractContent(string respText, JavaScriptSerializer ser)
    {
        try
        {
            var root = ser.Deserialize<Dictionary<string, object>>(respText);
            object choicesObj;
            if (!root.TryGetValue("choices", out choicesObj)) return null;
            var choices = choicesObj as object[];
            if (choices == null || choices.Length == 0) return null;

            var choice0 = choices[0] as Dictionary<string, object>;
            if (choice0 == null) return null;

            object msgObj;
            if (!choice0.TryGetValue("message", out msgObj)) return null;
            var msg = msgObj as Dictionary<string, object>;
            if (msg == null) return null;

            object contentObj;
            if (!msg.TryGetValue("content", out contentObj)) return null;
            return contentObj as string;
        }
        catch { return null; }
    }

    private static string StripFences(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        s = s.Trim();
        if (s.StartsWith("```"))
        {
            int nl = s.IndexOf('\n');
            if (nl >= 0) s = s.Substring(nl + 1);
            int fence = s.LastIndexOf("```", StringComparison.Ordinal);
            if (fence >= 0) s = s.Substring(0, fence);
        }
        return s.Trim();
    }

    private static string GetStr(Dictionary<string, object> d, string k)
    {
        object v;
        if (d != null && d.TryGetValue(k, out v) && v != null) return v.ToString().Trim();
        return "";
    }

    // Join a JSON string-array (sent as object[]) into a comma list for the prompt.
    private static string JoinArr(Dictionary<string, object> d, string k)
    {
        object v;
        if (d == null || !d.TryGetValue(k, out v) || v == null) return "";
        var arr = v as object[];
        if (arr == null) return "";
        var parts = new List<string>();
        foreach (var o in arr)
        {
            string s = (o ?? "").ToString().Trim();
            if (s.Length > 0) parts.Add(s);
        }
        return string.Join(", ", parts);
    }

    public bool IsReusable { get { return false; } }
}
