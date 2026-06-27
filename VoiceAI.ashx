<%@ WebHandler Language="C#" Class="VoiceAI" %>

/*
 * VoiceAI.ashx
 * ----------------------------------------------------------------------------
 * Server-side proxy for the AI Voice Assistant.
 *
 * Why server-side?
 *   The Mistral API key MUST NOT live in the browser. The browser sends the
 *   recognised speech transcript to this handler; the handler calls Mistral
 *   with the key (read from Web.config <appSettings>) and returns a clean,
 *   strict-JSON "intent" object that VoiceAssistant.js can act on.
 *
 * Dependencies: NONE beyond the default .NET 4.7.2 / System.Web stack.
 *   - HttpWebRequest      -> no NuGet, no extra assembly reference
 *   - JavaScriptSerializer -> System.Web.Extensions (always referenced)
 *
 * Contract
 *   Request  (POST, application/json):
 *     { "text": "<spoken transcript>", "menu": ["Dashboard","Wallet",...],
 *       "page": "AllWalletReport.aspx" }
 *   Response (application/json):
 *     { "action":"navigate|search|fill|click|filter|none",
 *       "target":"...", "query":"...", "value":"...", "say":"...",
 *       "source":"mistral|fallback" }
 * ----------------------------------------------------------------------------
 */

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;

public class VoiceAI : IHttpHandler
{
    private const string MistralUrl = "https://api.mistral.ai/v1/chat/completions";

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        // This endpoint is same-origin only; reflect no wildcard CORS.
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        var serializer = new JavaScriptSerializer();
        string result;

        try
        {
            // ---- 1. Read & validate the incoming request -------------------
            string rawBody;
            using (var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
            {
                rawBody = reader.ReadToEnd();
            }

            var req = string.IsNullOrWhiteSpace(rawBody)
                ? new Dictionary<string, object>()
                : (Dictionary<string, object>)serializer.DeserializeObject(rawBody);

            string userText = SafeString(req, "text");
            // Hard sanitise: voice text is data, never markup/script.
            userText = Sanitize(userText);

            if (string.IsNullOrWhiteSpace(userText))
            {
                context.Response.Write(serializer.Serialize(new
                {
                    action = "none",
                    say = "Maine kuch suna nahi. Dobara boliye.",
                    source = "fallback"
                }));
                return;
            }

            string page = Sanitize(SafeString(req, "page"));
            var menuItems = ExtractList(req, "menu");

            // ---- 2. Try Mistral; fall back to local parser on any failure --
            string apiKey = (ConfigurationManager.AppSettings["MistralApiKey"] ?? "").Trim();
            string model = (ConfigurationManager.AppSettings["MistralModel"] ?? "mistral-small-latest").Trim();

            if (!string.IsNullOrEmpty(apiKey))
            {
                string intentJson = CallMistral(apiKey, model, userText, page, menuItems);
                if (!string.IsNullOrEmpty(intentJson))
                {
                    // Validate it parses; attach source marker.
                    var parsed = serializer.DeserializeObject(intentJson) as Dictionary<string, object>;
                    if (parsed != null && parsed.ContainsKey("action"))
                    {
                        parsed["source"] = "mistral";
                        context.Response.Write(serializer.Serialize(parsed));
                        return;
                    }
                }
            }

            // ---- 3. Local rule-based fallback ------------------------------
            result = serializer.Serialize(LocalParse(userText, menuItems));
        }
        catch (Exception ex)
        {
            result = serializer.Serialize(new
            {
                action = "none",
                say = "Server par dikkat aa gayi. Thodi der baad try karein.",
                source = "error",
                error = ex.Message
            });
        }

        context.Response.Write(result);
    }

    // ------------------------------------------------------------------ Mistral
    private string CallMistral(string apiKey, string model, string userText,
                               string page, List<string> menuItems)
    {
        try
        {
            string menuList = (menuItems != null && menuItems.Count > 0)
                ? string.Join(", ", menuItems)
                : "(unknown)";

            string systemPrompt =
                "You are the intent parser for an MLM cPanel web application. " +
                "The user speaks in Hindi, English or Hinglish and may have typos. " +
                "Convert the user's command into ONE JSON object and reply with JSON ONLY " +
                "(no markdown, no code fences, no explanation). Schema:\n" +
                "{\"action\":\"navigate|search|filter|fill|click|none\"," +
                "\"target\":\"\",\"query\":\"\",\"value\":\"\",\"say\":\"\"}\n" +
                "Rules:\n" +
                "- If the user uses an open/go verb (kholo, kolo, khol, open, jaao, jao, dikhao, " +
                "show, go to, खोलो, दिखाओ, जाओ), the action is ALWAYS 'navigate'. " +
                "Set target to the closest matching menu item from the list below. " +
                "A menu item is NEVER action 'click'.\n" +
                "- action 'click' is ONLY for in-page action buttons: Save, Update, Delete, " +
                "Search, Reset, Cancel, Approve, Reject, Print, Export, Upload, Download, " +
                "Generate Invoice, Submit. Set target to that button label.\n" +
                "- action 'search'/'filter': set query to the text to find in the current grid.\n" +
                "- action 'fill': target = field label, value = value to type.\n" +
                "- Always pick target from the Available menu items when navigating, even if " +
                "the user's word is misspelled (fuzzy match).\n" +
                "- 'say' = a very short confirmation in the user's language.\n" +
                "Available menu items: " + menuList + ".\n" +
                "Current page: " + page + ".";

            var payload = new Dictionary<string, object>
            {
                { "model", model },
                { "temperature", 0 },
                { "messages", new object[]
                    {
                        new Dictionary<string,object>{ {"role","system"}, {"content", systemPrompt} },
                        new Dictionary<string,object>{ {"role","user"}, {"content", userText} }
                    }
                }
            };

            var serializer = new JavaScriptSerializer();
            byte[] body = Encoding.UTF8.GetBytes(serializer.Serialize(payload));

            // TLS 1.2 for older runtimes.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var http = (HttpWebRequest)WebRequest.Create(MistralUrl);
            http.Method = "POST";
            http.ContentType = "application/json";
            http.Accept = "application/json";
            http.Headers["Authorization"] = "Bearer " + apiKey;
            http.Timeout = 15000;
            http.ContentLength = body.Length;

            using (var stream = http.GetRequestStream())
            {
                stream.Write(body, 0, body.Length);
            }

            using (var response = (HttpWebResponse)http.GetResponse())
            using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string respText = sr.ReadToEnd();
                var obj = serializer.DeserializeObject(respText) as Dictionary<string, object>;
                if (obj == null) return null;

                // choices[0].message.content
                var choices = obj.ContainsKey("choices") ? obj["choices"] as object[] : null;
                if (choices == null || choices.Length == 0) return null;

                var first = choices[0] as Dictionary<string, object>;
                var message = first != null && first.ContainsKey("message")
                    ? first["message"] as Dictionary<string, object> : null;
                string content = message != null && message.ContainsKey("content")
                    ? message["content"] as string : null;

                return CleanJson(content);
            }
        }
        catch
        {
            // Any network/parse error -> let caller use local fallback.
            return null;
        }
    }

    // Strip accidental ```json fences / leading prose, keep the JSON object.
    private string CleanJson(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        s = s.Trim();
        s = s.Replace("```json", "").Replace("```", "").Trim();
        int start = s.IndexOf('{');
        int end = s.LastIndexOf('}');
        if (start >= 0 && end > start) return s.Substring(start, end - start + 1);
        return null;
    }

    // --------------------------------------------------- Local rule-based parse
    private object LocalParse(string text, List<string> menuItems)
    {
        string t = text.ToLowerInvariant().Trim();

        // Button / action keywords (Hindi + English).
        string[][] buttonMap = new string[][]
        {
            new[]{ "save", "save|सेव|सहेज|जमा करो" },
            new[]{ "search", "search|खोज|ढूंढ|find" },
            new[]{ "reset", "reset|रीसेट|साफ|clear" },
            new[]{ "print", "print|प्रिंट|छाप" },
            new[]{ "export", "export|excel|pdf|डाउनलोड export" },
            new[]{ "approve", "approve|अप्रूव|स्वीकार|मंजूर" },
            new[]{ "reject", "reject|रिजेक्ट|अस्वीकार|नामंजूर" },
            new[]{ "update", "update|अपडेट" },
            new[]{ "delete", "delete|डिलीट|हटा" }
        };
        foreach (var pair in buttonMap)
        {
            foreach (var kw in pair[1].Split('|'))
            {
                if (t.Contains(kw)) return new { action = "click", target = pair[0], say = pair[0] + " kar raha hoon.", source = "fallback" };
            }
        }

        // Grid search: "search <x>" / "खोजो <x>"
        foreach (var prefix in new[] { "search ", "find ", "खोजो ", "खोज ", "ढूंढो " })
        {
            int idx = t.IndexOf(prefix, StringComparison.Ordinal);
            if (idx >= 0)
            {
                string q = text.Substring(idx + prefix.Length).Trim();
                if (q.Length > 0)
                    return new { action = "search", query = q, say = "\"" + q + "\" search kar raha hoon.", source = "fallback" };
            }
        }

        // Navigation: best fuzzy match against the supplied menu items.
        if (menuItems != null && menuItems.Count > 0)
        {
            string best = null; int bestScore = int.MaxValue;
            foreach (var m in menuItems)
            {
                if (string.IsNullOrWhiteSpace(m)) continue;
                string ml = m.ToLowerInvariant();
                int score;
                if (t.Contains(ml) || ml.Contains(t)) score = 0;
                else score = Levenshtein(t, ml);
                if (score < bestScore) { bestScore = score; best = m; }
            }
            // Accept only reasonably close matches.
            if (best != null && bestScore <= Math.Max(3, best.Length / 2))
                return new { action = "navigate", target = best, say = best + " khol raha hoon.", source = "fallback" };
        }

        return new { action = "none", say = "Samajh nahi paaya. Dobara boliye.", source = "fallback" };
    }

    // ----------------------------------------------------------------- Helpers
    private static int Levenshtein(string a, string b)
    {
        if (string.IsNullOrEmpty(a)) return (b ?? "").Length;
        if (string.IsNullOrEmpty(b)) return a.Length;
        int[,] d = new int[a.Length + 1, b.Length + 1];
        for (int i = 0; i <= a.Length; i++) d[i, 0] = i;
        for (int j = 0; j <= b.Length; j++) d[0, j] = j;
        for (int i = 1; i <= a.Length; i++)
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
            }
        return d[a.Length, b.Length];
    }

    private static string SafeString(Dictionary<string, object> d, string key)
    {
        if (d != null && d.ContainsKey(key) && d[key] != null) return d[key].ToString();
        return "";
    }

    private static List<string> ExtractList(Dictionary<string, object> d, string key)
    {
        var list = new List<string>();
        if (d != null && d.ContainsKey(key) && d[key] is object[])
        {
            foreach (var o in (object[])d[key])
                if (o != null) list.Add(o.ToString());
        }
        return list;
    }

    // Remove anything that could enable XSS/HTML/script injection from voice text.
    private static string Sanitize(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        s = s.Replace("<", " ").Replace(">", " ").Replace("\"", " ").Replace("'", " ");
        if (s.Length > 500) s = s.Substring(0, 500);
        return s.Trim();
    }

    public bool IsReusable { get { return false; } }
}
