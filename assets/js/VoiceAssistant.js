/* ==========================================================================
 *  VoiceAssistant.js   -   cpanel.BasicmlmCSharp (WebForms)
 * --------------------------------------------------------------------------
 *  Mistral + Web Speech API voice assistant.
 *
 *  Flow:
 *    mic tap -> SpeechRecognition (hi-IN; understands Hinglish/English)
 *            -> scan LIVE sidebar menu (label + real href)
 *            -> POST { text, page:<current aspx>, menus:[labels] } to VoiceAI.ashx
 *            -> receive { intent, page, filters, fields, speak, speakLang }
 *            -> navigate: open the menu item whose label == page (exact/fuzzy)
 *               filter / fill / guidedfill: act on the current page
 *            -> speak the AI's confirmation in its language (hi/en).
 *
 *  cpanel's menu is DB-driven, so navigation is driven entirely by the LIVE
 *  menu the user actually sees - nothing is hard-coded.
 *
 *  Pure vanilla JS - no jQuery. Additive, self-contained.
 * ========================================================================== */
(function () {
    "use strict";

    if (window.__voiceAILoaded) return;
    window.__voiceAILoaded = true;

    var CFG = window.VoiceAICfg || {};
    var ENDPOINT = CFG.endpoint || "VoiceAI.ashx";
    var HOME = CFG.home || "Index.aspx";
    var LOGOUT = CFG.logout || "logout.aspx";
    var LANG = CFG.lang || "hi-IN";

    function currentPageFile() {
        var p = (location.pathname || "").split("/").pop();
        return p || "Index.aspx";
    }

    // ======================================================================
    //  1.  UI
    // ======================================================================
    var fab, pill, isListening = false;

    function injectUI() {
        var css = document.createElement("style");
        css.textContent =
            ".vai-fab{position:fixed;right:22px;bottom:22px;width:60px;height:60px;border-radius:50%;" +
            "background:#6777ef;color:#fff;border:none;box-shadow:0 6px 18px rgba(103,119,239,.45);" +
            "cursor:pointer;z-index:99999;display:flex;align-items:center;justify-content:center;" +
            "transition:transform .15s,background .2s;font-size:24px;}" +
            ".vai-fab:hover{transform:scale(1.08);}" +
            ".vai-fab.listening{background:#fc544b;animation:vaiPulse 1.1s infinite;}" +
            ".vai-fab.guided{background:#47c363;}" +
            "@keyframes vaiPulse{0%{box-shadow:0 0 0 0 rgba(252,84,75,.55);}70%{box-shadow:0 0 0 16px rgba(252,84,75,0);}100%{box-shadow:0 0 0 0 rgba(252,84,75,0);}}" +
            ".vai-pill{position:fixed;right:22px;bottom:92px;max-width:320px;background:#1f2533;color:#fff;" +
            "padding:10px 14px;border-radius:12px;font-size:13px;line-height:1.4;z-index:99999;" +
            "box-shadow:0 6px 18px rgba(0,0,0,.25);opacity:0;transform:translateY(8px);" +
            "transition:opacity .2s,transform .2s;pointer-events:none;}" +
            ".vai-pill.show{opacity:1;transform:translateY(0);}" +
            ".vai-field-hi{outline:2px solid #6777ef !important;outline-offset:1px;transition:outline .2s;}";
        document.head.appendChild(css);

        fab = document.createElement("button");
        fab.type = "button";
        fab.className = "vai-fab";
        fab.title = "Voice Assistant - tap and speak your command";
        fab.innerHTML = "&#127908;";
        fab.addEventListener("click", function (e) { e.preventDefault(); toggleListen(); });
        document.body.appendChild(fab);

        pill = document.createElement("div");
        pill.className = "vai-pill";
        document.body.appendChild(pill);
    }

    var pillTimer = null;
    function showPill(text, keep) {
        if (!pill) return;
        pill.textContent = text;
        pill.classList.add("show");
        if (pillTimer) clearTimeout(pillTimer);
        if (!keep) pillTimer = setTimeout(function () { pill.classList.remove("show"); }, 4500);
    }

    // ======================================================================
    //  2.  Speech synthesis (bilingual)
    // ======================================================================
    var voices = [];
    function loadVoices() { if ("speechSynthesis" in window) voices = window.speechSynthesis.getVoices() || []; }
    if ("speechSynthesis" in window) { loadVoices(); window.speechSynthesis.onvoiceschanged = loadVoices; }
    function voiceFor(lc) {
        var rx = (lc === "en") ? /en[-_](IN|US|GB)/i : /hi[-_]IN/i;
        return voices.filter(function (v) { return rx.test(v.lang); })[0] ||
            voices.filter(function (v) { return new RegExp("^" + (lc || "en"), "i").test(v.lang); })[0] ||
            voices[0] || null;
    }
    function speak(text, speakLang) {
        if (!text) return;
        showPill(text);
        if (!("speechSynthesis" in window)) return;
        var lc = (speakLang === "en") ? "en" : "hi";
        try {
            window.speechSynthesis.cancel();
            var u = new SpeechSynthesisUtterance(text);
            u.lang = (lc === "en") ? "en-IN" : "hi-IN";
            var v = voiceFor(lc); if (v) u.voice = v;
            u.rate = 1; u.pitch = 1;
            window.speechSynthesis.speak(u);
        } catch (e) { }
    }

    // ======================================================================
    //  3.  Speech recognition
    // ======================================================================
    var recog = null;
    function getRecognizer() {
        var SR = window.SpeechRecognition || window.webkitSpeechRecognition;
        if (!SR) return null;
        var r = new SR();
        r.lang = LANG; r.interimResults = false; r.maxAlternatives = 1; r.continuous = false;
        return r;
    }
    var routeResult = null;
    function toggleListen() {
        if (isListening) { stopListen(); return; }
        if (!recog) recog = getRecognizer();
        if (!recog) { speak("Voice input is not supported in this browser. Please use Chrome.", "en"); return; }
        try { window.speechSynthesis && window.speechSynthesis.cancel(); } catch (e) { }
        recog.onstart = function () {
            isListening = true;
            fab.classList.add(guided.active ? "guided" : "listening");
            showPill(guided.active ? ("Listening for: " + guided.label) : "Listening... please speak", true);
        };
        recog.onerror = function (ev) {
            isListening = false; fab.classList.remove("listening", "guided");
            if (ev.error === "no-speech") showPill("I didn't catch that.");
            else if (ev.error === "not-allowed") speak("Please allow microphone access.", "en");
            else showPill("Microphone error: " + ev.error);
        };
        recog.onend = function () { isListening = false; fab.classList.remove("listening", "guided"); };
        recog.onresult = function (ev) {
            var t = "";
            for (var i = 0; i < ev.results.length; i++) t += ev.results[i][0].transcript;
            t = (t || "").trim();
            if (!t) { showPill("I didn't catch that."); return; }
            showPill("\u201C" + t + "\u201D", true);
            if (routeResult) { var fn = routeResult; routeResult = null; fn(t); }
            else handleCommand(t);
        };
        try { recog.start(); } catch (e) { }
    }
    function stopListen() { try { recog && recog.stop(); } catch (e) { } isListening = false; fab.classList.remove("listening", "guided"); }
    function listenOnce(cb) { routeResult = cb; toggleListen(); }

    // ======================================================================
    //  4.  DOM helpers
    // ======================================================================
    function isVisible(el) {
        if (!el) return false;
        if (el.offsetParent === null && el.tagName !== "BODY") {
            var st = window.getComputedStyle(el); if (st.position !== "fixed") return false;
        }
        var s2 = window.getComputedStyle(el);
        return s2.display !== "none" && s2.visibility !== "hidden";
    }
    function prettify(s) {
        if (!s) return "";
        s = s.replace(/^(txt|cmb|ddl|lbl|btn)/i, "");
        s = s.replace(/[_\-]+/g, " ").replace(/([a-z])([A-Z])/g, "$1 $2");
        return s.replace(/\s+/g, " ").trim();
    }
    function cssEscape(s) { return (window.CSS && CSS.escape) ? CSS.escape(s) : s.replace(/([^\w-])/g, "\\$1"); }
    function fieldLabel(el) {
        if (el.placeholder && el.placeholder.trim()) return el.placeholder.trim();
        if (el.id) { var lab = document.querySelector('label[for="' + cssEscape(el.id) + '"]'); if (lab && lab.textContent.trim()) return lab.textContent.trim(); }
        var p = el.closest ? el.closest("label") : null;
        if (p && p.textContent.trim()) return p.textContent.trim();
        if (el.getAttribute("aria-label")) return el.getAttribute("aria-label").trim();
        if (el.name) return prettify(el.name.split("$").pop());
        if (el.id) return prettify(el.id.split("_").pop());
        return "";
    }
    function scanFields() {
        var list = [];
        document.querySelectorAll(
            'input[type=text],input[type=email],input[type=number],input[type=tel],' +
            'input[type=date],input[type=search],input[type=url],input[type=password],' +
            'input:not([type]),textarea,select').forEach(function (el) {
                if (el.disabled || el.readOnly || el.type === "hidden") return;
                if (!isVisible(el)) return;
                var label = fieldLabel(el); if (!label) return;
                list.push({ label: label, el: el });
            });
        return list;
    }
    // LIVE menu - navigable anchors (real href). Template-agnostic:
    //   1) try the usual menu containers
    //   2) if too few found, fall back to EVERY <a> on the page whose href
    //      points to an .aspx page (works regardless of menu CSS structure).
    function collectAnchors(nodeList, list, seen) {
        nodeList.forEach(function (a) {
            var href = a.getAttribute("href") || "";
            var text = (a.textContent || "").replace(/\s+/g, " ").trim();
            if (!text || text.length > 40) return;
            if (!href || href === "#" || href.toLowerCase().indexOf("javascript:") === 0) return;
            var key = text.toLowerCase();
            if (seen[key]) return;
            seen[key] = true;
            list.push({ label: text, href: a.href }); // resolved absolute href
        });
    }
    function scanMenuAnchors() {
        var list = [], seen = {};
        collectAnchors(document.querySelectorAll('.sidebar-menu a, #menu a, nav a, .navbar-nav a, aside a, .menu a, .sidebar a'), list, seen);
        if (list.length < 3) {
            // fallback: any anchor that links to an .aspx page
            var all = [];
            document.querySelectorAll('a[href]').forEach(function (a) {
                if (/\.aspx(\?|#|$)/i.test(a.getAttribute("href") || "")) all.push(a);
            });
            collectAnchors(all, list, seen);
        }
        try { window.__vaiMenus = list.map(function (m) { return m.label; }); } catch (e) { }
        return list;
    }

    // ----- fuzzy -----
    function norm(s) { return (s || "").toLowerCase().replace(/[^a-z0-9\u0900-\u097F ]/g, "").replace(/\s+/g, " ").trim(); }
    function lev(a, b) {
        a = a || ""; b = b || ""; var m = a.length, n = b.length; if (!m) return n; if (!n) return m;
        var prev = [], cur = [], i, j;
        for (j = 0; j <= n; j++) prev[j] = j;
        for (i = 1; i <= m; i++) { cur[0] = i; for (j = 1; j <= n; j++) { var cost = a.charAt(i - 1) === b.charAt(j - 1) ? 0 : 1; cur[j] = Math.min(prev[j] + 1, cur[j - 1] + 1, prev[j - 1] + cost); } for (j = 0; j <= n; j++) prev[j] = cur[j]; }
        return prev[n];
    }
    function bestMatch(wanted, list, minScore) {
        if (!wanted || !list || !list.length) return null;
        var w = norm(wanted), best = null, bestScore = -1;
        list.forEach(function (item) {
            var c = norm(item.label); if (!c) return;
            var score;
            if (c === w) score = 1;
            else if (c.indexOf(w) >= 0 || w.indexOf(c) >= 0) score = 0.85;
            else { score = 1 - lev(w, c) / (Math.max(w.length, c.length) || 1); }
            if (score > bestScore) { bestScore = score; best = item; }
        });
        return bestScore >= (minScore || 0.5) ? best : null;
    }
    function normValue(v) {
        if (v == null) return "";
        var map = { "\u0966": "0", "\u0967": "1", "\u0968": "2", "\u0969": "3", "\u096A": "4", "\u096B": "5", "\u096C": "6", "\u096D": "7", "\u096E": "8", "\u096F": "9" };
        return String(v).replace(/[\u0966-\u096F]/g, function (c) { return map[c]; });
    }
    function setFieldValue(el, value) {
        if (!el) return;
        el.classList.add("vai-field-hi"); setTimeout(function () { el.classList.remove("vai-field-hi"); }, 1500);
        if (el.tagName === "SELECT") {
            var opts = el.options, matched = false;
            for (var i = 0; i < opts.length; i++) { if (norm(opts[i].text) === norm(value) || norm(opts[i].text).indexOf(norm(value)) >= 0) { el.selectedIndex = i; matched = true; break; } }
            if (!matched && value) { var best = -1, bi = -1; for (var j = 0; j < opts.length; j++) { var s = 1 - lev(norm(value), norm(opts[j].text)) / (Math.max(norm(value).length, norm(opts[j].text).length) || 1); if (s > best) { best = s; bi = j; } } if (best >= 0.5 && bi >= 0) el.selectedIndex = bi; }
        } else { try { el.focus(); } catch (e) { } el.value = value; }
        el.dispatchEvent(new Event("input", { bubbles: true }));
        el.dispatchEvent(new Event("change", { bubbles: true }));
        try { el.blur(); } catch (e) { }
    }

    // ======================================================================
    //  5.  Pipeline
    // ======================================================================
    function handleCommand(raw) {
        showPill("Thinking...", true);
        var menus = scanMenuAnchors();
        var payload = { text: raw, page: currentPageFile(), menus: menus.map(function (m) { return m.label; }) };
        try { console.log("[VoiceAI] menu items found:", payload.menus.length, payload.menus); console.log("[VoiceAI] sending:", payload); } catch (e) { }
        var xhr = new XMLHttpRequest();
        xhr.open("POST", ENDPOINT, true);
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.timeout = 25000;
        xhr.onreadystatechange = function () {
            if (xhr.readyState !== 4) return;
            if (xhr.status < 200 || xhr.status >= 300) { speak("Server error. Please try again.", "en"); return; }
            var data;
            try { console.log("[VoiceAI] raw response:", xhr.responseText); data = JSON.parse(xhr.responseText); }
            catch (e) { speak("I couldn't understand the response.", "en"); return; }
            executeIntent(data, menus);
        };
        xhr.ontimeout = function () { speak("The request timed out. Please try again.", "en"); };
        xhr.onerror = function () { speak("Network error.", "en"); };
        xhr.send(JSON.stringify(payload));
    }

    function executeIntent(obj, menus) {
        var intent = (obj && obj.intent) || "unknown";
        var sl = (obj && obj.speakLang) || "hi";
        var say = (obj && obj.speak) || "";

        switch (intent) {
            case "navigate": {
                var target = obj.page || "";
                if (/^\s*(logout|log\s*out|sign\s*out)\s*$/i.test(target)) {
                    var lo = bestMatch(target, menus, 0.5);
                    if (say) speak(say, sl);
                    setTimeout(function () { window.location.href = lo ? lo.href : LOGOUT; }, 600);
                    break;
                }
                var m = bestMatch(target, menus, 0.45);
                if (m && m.href) {
                    if (say) speak(say, sl); else showPill("Opening " + m.label);
                    setTimeout(function () { window.location.href = m.href; }, 600);
                } else {
                    speak(say || ("Menu not found: " + target), sl);
                    showPill("Menu not found: \"" + target + "\" (scanned " + menus.length + " menu items)", true);
                }
                break;
            }

            case "filter": {
                var q = obj.filters && obj.filters.query ? normValue(obj.filters.query) : "";
                if (!q) { speak(say || "What should I filter?", sl); break; }
                var ok = applyFilter(q);
                speak(say || (ok ? ("Filtering " + q) : "No search box on this page."), sl);
                break;
            }

            case "fill": {
                var fields = obj.fields || {}, flds = scanFields(), done = 0, missed = [];
                Object.keys(fields).forEach(function (k) {
                    var f = bestMatch(k, flds, 0.5);
                    if (f) { setFieldValue(f.el, normValue(fields[k])); done++; } else missed.push(k);
                });
                var msg = say || (done ? ("Filled " + done + " field" + (done > 1 ? "s" : "")) : "No matching field found");
                if (!say && missed.length) msg += ". Not found: " + missed.join(", ");
                speak(msg, sl);
                break;
            }

            case "guidedfill":
                startGuidedFill(sl);
                break;

            default:
                speak(say || "Sorry, I didn't understand. Please try again.", sl);
        }
    }

    function applyFilter(query) {
        var flds = scanFields(), box = null;
        flds.forEach(function (f) {
            if (box) return;
            if (/search|filter|find|khoj|naam|name|member|id/i.test(f.label) || f.el.type === "search") box = f.el;
        });
        if (!box && flds.length) box = flds[0].el;
        if (!box) return false;
        setFieldValue(box, query);
        var btn = document.querySelector('input[type=submit][value*="Search" i],input[type=button][value*="Search" i],button[id*="Search" i],a[id*="Search" i]');
        if (btn) setTimeout(function () { btn.click(); }, 300);
        else box.dispatchEvent(new KeyboardEvent("keyup", { bubbles: true, key: "Enter", keyCode: 13 }));
        return true;
    }

    // ----- guided fill -----
    var guided = { active: false, fields: [], idx: 0, label: "", lang: "hi" };
    function startGuidedFill(lang) {
        guided.fields = scanFields();
        if (!guided.fields.length) { speak("There are no fields on this page.", lang); return; }
        guided.active = true; guided.idx = 0; guided.lang = lang || "hi";
        speak(lang === "en" ? "Starting guided fill. Say 'skip' to skip, 'stop' to cancel."
            : "Guided fill shuru. 'skip' bolo chhodne ke liye, 'stop' bolo rokne ke liye.", lang);
        setTimeout(nextGuidedField, 1800);
    }
    function nextGuidedField() {
        if (!guided.active) return;
        if (guided.idx >= guided.fields.length) { finishGuided(); return; }
        var f = guided.fields[guided.idx];
        if (!f.el || f.el.disabled || !isVisible(f.el)) { guided.idx++; return nextGuidedField(); }
        guided.label = f.label;
        speak(guided.lang === "en" ? ("Say value for " + f.label) : (f.label + " boliye"), guided.lang);
        setTimeout(function () {
            listenOnce(function (spoken) {
                var low = spoken.toLowerCase().trim();
                if (/^(stop|cancel|band karo|ruk(o|jao)?)$/.test(low)) { finishGuided(true); return; }
                if (/^(skip|chhodo|chodo|agla|next)$/.test(low)) { guided.idx++; setTimeout(nextGuidedField, 500); return; }
                if (/^(submit|jama karo|save)$/.test(low)) { finishGuided(); trySubmit(); return; }
                setFieldValue(f.el, normValue(spoken));
                guided.idx++; setTimeout(nextGuidedField, 700);
            });
        }, 1400);
    }
    function finishGuided(cancelled) {
        guided.active = false; fab.classList.remove("guided", "listening");
        speak(cancelled ? (guided.lang === "en" ? "Cancelled." : "Cancel kar diya.")
            : (guided.lang === "en" ? "Done. Review and submit." : "Ho gaya. Check karke submit kijiye."), guided.lang);
    }
    function trySubmit() {
        var btn = document.querySelector('input[type=submit].btn-primary,input[type=submit],button.btn-primary,button[type=submit]');
        if (btn) setTimeout(function () { btn.click(); }, 400);
    }

    // ======================================================================
    //  6.  Boot
    // ======================================================================
    function boot() { injectUI(); showPill("Voice Assistant ready. Tap the mic."); }
    if (document.readyState === "loading") document.addEventListener("DOMContentLoaded", boot);
    else boot();

})();