/* ============================================================================
 * VoiceAssistant.js  (v2.0)
 * AI Voice Command System for the MLM cPanel (ASP.NET WebForms, Stisla template)
 *
 * Loaded by SitePage.master only for logged-in users. Config comes from the
 * global window.VoiceAICfg = { endpoint, home, logout, lang }.
 *
 * Design: one IIFE, internal modules so concerns stay separated
 * (Speech / Nav / Grid / Forms / Buttons / Fuzzy / Intent / Router / UI).
 * No external library required; uses jQuery & DataTables only if already present.
 * All user/voice text is treated as data -> inserted with textContent only.
 * ========================================================================== */
(function () {
    "use strict";

    if (window.__VoiceAILoaded) return;      // guard against double-injection
    window.__VoiceAILoaded = true;

    var Cfg = window.VoiceAICfg || {};
    var LANG = Cfg.lang || "hi-IN";
    var ENDPOINT = Cfg.endpoint || "VoiceAI.ashx";

    /* ----------------------------------------------------------- Fuzzy module */
    var Fuzzy = {
        norm: function (s) {
            return (s || "").toString().toLowerCase()
                .replace(/\s+/g, " ")
                .replace(/[^\u0900-\u097Fa-z0-9 ]/g, "")  // keep Devanagari + latin + digits
                .trim();
        },
        lev: function (a, b) {
            a = a || ""; b = b || "";
            var m = a.length, n = b.length;
            if (!m) return n; if (!n) return m;
            var prev = [], cur = [], i, j;
            for (j = 0; j <= n; j++) prev[j] = j;
            for (i = 1; i <= m; i++) {
                cur[0] = i;
                for (j = 1; j <= n; j++) {
                    var cost = a.charAt(i - 1) === b.charAt(j - 1) ? 0 : 1;
                    cur[j] = Math.min(prev[j] + 1, cur[j - 1] + 1, prev[j - 1] + cost);
                }
                prev = cur.slice();
            }
            return prev[n];
        },
        // 0..1 similarity; rewards substring containment.
        score: function (query, candidate) {
            var q = this.norm(query), c = this.norm(candidate);
            if (!q || !c) return 0;
            if (q === c) return 1;
            if (c.indexOf(q) >= 0 || q.indexOf(c) >= 0) return 0.9;
            var d = this.lev(q, c);
            var maxLen = Math.max(q.length, c.length);
            return maxLen ? (1 - d / maxLen) : 0;
        },
        bestMatch: function (query, items, getText, threshold) {
            threshold = threshold == null ? 0.55 : threshold;
            var best = null, bestScore = 0;
            for (var i = 0; i < items.length; i++) {
                var text = getText ? getText(items[i]) : items[i];
                var s = this.score(query, text);
                if (s > bestScore) { bestScore = s; best = items[i]; }
            }
            return bestScore >= threshold ? best : null;
        }
    };

    /* ------------------------------------------------------------- UI module */
    var UI = {
        fab: null, panel: null, statusEl: null, transcriptEl: null,
        build: function () {
            var style = document.createElement("style");
            style.textContent =
                ".vai-fab{position:fixed;right:22px;bottom:22px;width:60px;height:60px;border-radius:50%;" +
                "background:linear-gradient(135deg,#6f42c1,#4d7cfe);color:#fff;border:none;cursor:pointer;" +
                "box-shadow:0 6px 20px rgba(77,124,254,.45);z-index:99999;display:flex;align-items:center;" +
                "justify-content:center;transition:transform .15s ease}" +
                ".vai-fab:hover{transform:scale(1.06)}" +
                ".vai-fab.listening{animation:vaiPulse 1.2s infinite}" +
                ".vai-fab svg{width:26px;height:26px;fill:#fff}" +
                "@keyframes vaiPulse{0%{box-shadow:0 0 0 0 rgba(111,66,193,.55)}" +
                "70%{box-shadow:0 0 0 18px rgba(111,66,193,0)}100%{box-shadow:0 0 0 0 rgba(111,66,193,0)}}" +
                ".vai-panel{position:fixed;right:22px;bottom:92px;width:300px;max-width:86vw;background:#fff;" +
                "border-radius:14px;box-shadow:0 10px 40px rgba(0,0,0,.18);z-index:99999;padding:14px 16px;" +
                "font-family:inherit;display:none}" +
                ".vai-panel.show{display:block}" +
                ".vai-title{font-weight:600;color:#34395e;font-size:14px;margin-bottom:6px;display:flex;" +
                "align-items:center;gap:6px}" +
                ".vai-status{font-size:12px;color:#6c757d;min-height:16px}" +
                ".vai-transcript{font-size:13px;color:#191d21;margin-top:8px;min-height:18px;word-break:break-word}" +
                ".vai-hint{font-size:11px;color:#98a6ad;margin-top:8px;line-height:1.5}" +
                ".vai-inputrow{display:flex;gap:6px;margin-top:10px}" +
                ".vai-input{flex:1;min-width:0;border:1px solid #e3e6f0;border-radius:8px;padding:6px 9px;" +
                "font-size:13px;color:#191d21;outline:none}" +
                ".vai-input:focus{border-color:#6f42c1;box-shadow:0 0 0 2px rgba(111,66,193,.15)}" +
                ".vai-send{border:none;border-radius:8px;padding:6px 12px;font-size:13px;font-weight:600;" +
                "color:#fff;background:linear-gradient(135deg,#6f42c1,#4d7cfe);cursor:pointer}" +
                ".vai-send:hover{opacity:.92}";
            document.head.appendChild(style);

            var fab = document.createElement("button");
            fab.className = "vai-fab";
            fab.type = "button";
            fab.title = "Voice Assistant";
            fab.setAttribute("aria-label", "Voice Assistant");
            fab.innerHTML = '<svg viewBox="0 0 24 24"><path d="M12 14a3 3 0 0 0 3-3V5a3 3 0 0 0-6 0v6a3 3 0 0 0 3 3zm5-3a5 5 0 0 1-10 0H5a7 7 0 0 0 6 6.92V21h2v-3.08A7 7 0 0 0 19 11h-2z"/></svg>';

            var panel = document.createElement("div");
            panel.className = "vai-panel";
            var title = document.createElement("div");
            title.className = "vai-title";
            title.textContent = "🎤 Voice Assistant";
            var status = document.createElement("div");
            status.className = "vai-status";
            status.textContent = "Mic dabaiye aur command boliye.";
            var transcript = document.createElement("div");
            transcript.className = "vai-transcript";
            var hint = document.createElement("div");
            hint.className = "vai-hint";
            hint.textContent = 'Jaise: "Wallet kholo", "Search Raj", "Print karo", "Save karo".';

            // Typed-command fallback: test full command routing without a microphone.
            var inputRow = document.createElement("div");
            inputRow.className = "vai-inputrow";
            var input = document.createElement("input");
            input.type = "text";
            input.className = "vai-input";
            input.setAttribute("placeholder", "Ya yahan command type karein…");
            var send = document.createElement("button");
            send.type = "button";
            send.className = "vai-send";
            send.textContent = "Go";
            inputRow.appendChild(input);
            inputRow.appendChild(send);

            function submitTyped() {
                var v = (input.value || "").trim();
                if (!v) return;
                UI.setTranscript("⌨ " + v);
                input.value = "";
                Router.handle(v);                 // same path voice uses
            }
            send.addEventListener("click", submitTyped);
            input.addEventListener("keydown", function (e) {
                if (e.key === "Enter" || e.keyCode === 13) { e.preventDefault(); submitTyped(); }
            });

            panel.appendChild(title); panel.appendChild(status);
            panel.appendChild(transcript);
            panel.appendChild(inputRow);
            panel.appendChild(hint);

            document.body.appendChild(panel);
            document.body.appendChild(fab);

            this.fab = fab; this.panel = panel; this.inputEl = input;
            this.statusEl = status; this.transcriptEl = transcript;

            fab.addEventListener("click", function () { Speech.toggle(); });
        },
        setStatus: function (txt) { if (this.statusEl) this.statusEl.textContent = txt; this.show(); },
        setTranscript: function (txt) { if (this.transcriptEl) this.transcriptEl.textContent = txt; },
        listening: function (on) {
            if (!this.fab) return;
            if (on) this.fab.classList.add("listening"); else this.fab.classList.remove("listening");
        },
        show: function () { if (this.panel) this.panel.classList.add("show"); this._auto(); },
        _auto: function () {
            var self = this;
            clearTimeout(this._t);
            this._t = setTimeout(function () {
                var typing = self.inputEl && document.activeElement === self.inputEl;
                if (self.panel && !self.fab.classList.contains("listening") && !typing) {
                    self.panel.classList.remove("show");
                }
            }, 6000);
        }
    };

    /* --------------------------------------------------------- Speech module */
    var Speech = {
        recog: null, active: false, micGranted: false,
        synth: window.speechSynthesis || null,

        // Friendly, actionable messages for each SpeechRecognition error code.
        explainError: function (code) {
            switch (code) {
                case "not-allowed":
                case "service-not-allowed":
                    if (!Speech.secure()) {
                        return "Mic block hai: site HTTP par hai. SpeechRecognition sirf HTTPS ya localhost par chalta hai. SSL lagayein ya localhost se test karein.";
                    }
                    return "Mic permission denied. Address bar ke 🔒 icon par click → Microphone → Allow → page reload karein.";
                case "no-speech":
                    return "Kuch suna nahi. Mic ke paas saaf boliye.";
                case "audio-capture":
                    return "Microphone nahi mila. Device connect/enable karein.";
                case "network":
                    return "Network error. Speech service tak nahi pahunch paaya.";
                case "aborted":
                    return "Mic dabaiye aur command boliye.";
                default:
                    return "Mic error: " + (code || "unknown");
            }
        },

        // SpeechRecognition + getUserMedia dono ke liye secure context zaroori.
        secure: function () {
            if (typeof window.isSecureContext === "boolean") return window.isSecureContext;
            var h = location.hostname;
            return location.protocol === "https:" || h === "localhost" || h === "127.0.0.1";
        },

        // Explicitly surface the permission prompt; resolves on grant, rejects with a code.
        ensureMic: function () {
            if (this.micGranted) return Promise.resolve();
            if (!this.secure()) return Promise.reject("not-allowed");
            if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
                return Promise.resolve(); // older browser: let recognition try directly
            }
            var self = this;
            return navigator.mediaDevices.getUserMedia({ audio: true }).then(function (stream) {
                self.micGranted = true;
                // Release the track immediately; recognition opens its own.
                try { stream.getTracks().forEach(function (t) { t.stop(); }); } catch (x) { }
            }).catch(function (err) {
                var name = err && err.name ? err.name : "";
                if (name === "NotAllowedError" || name === "SecurityError") throw "not-allowed";
                if (name === "NotFoundError" || name === "DevicesNotFoundError") throw "audio-capture";
                throw "not-allowed";
            });
        },

        init: function () {
            var SR = window.SpeechRecognition || window.webkitSpeechRecognition;
            if (!SR) { this.recog = null; return false; }
            var r = new SR();
            r.lang = LANG;
            r.interimResults = false;
            r.maxAlternatives = 1;
            r.continuous = false;
            var self = this;
            r.onstart = function () { self.active = true; UI.listening(true); UI.setStatus("Sun raha hoon…"); };
            r.onend = function () { self.active = false; UI.listening(false); };
            r.onerror = function (e) {
                self.active = false; UI.listening(false);
                UI.setStatus(self.explainError(e && e.error));
            };
            r.onresult = function (e) {
                var text = (e.results[0][0].transcript || "").trim();
                UI.setTranscript("“" + text + "”");
                Router.handle(text);
            };
            this.recog = r;
            return true;
        },

        toggle: function () {
            if (!this.recog && !this.init()) {
                UI.setStatus("Is browser mein speech support nahi hai. Chrome/Edge use karein.");
                return;
            }
            if (this.active) { try { this.recog.stop(); } catch (x) { } return; }

            if (!this.secure()) {
                UI.setStatus(this.explainError("not-allowed"));
                return;
            }

            var self = this;
            UI.setStatus("Mic permission check…");
            this.ensureMic().then(function () {
                try { self.recog.lang = LANG; self.recog.start(); }
                catch (x) { /* already started */ }
            }).catch(function (code) {
                UI.setStatus(self.explainError(code));
            });
        },
        say: function (text) {
            if (!text) return;
            try {
                if (this.synth) {
                    this.synth.cancel();
                    var u = new SpeechSynthesisUtterance(text);
                    u.lang = LANG;
                    this.synth.speak(u);
                }
            } catch (x) { }
        }
    };

    /* ------------------------------------------------------------ Nav module */
    var Nav = {
        // Build {name, href} from the actual rendered sidebar (dynamic per company).
        items: function () {
            var out = [], seen = {};
            var anchors = document.querySelectorAll(".sidebar-menu a[href], .sidebar-menu a[runat]");
            for (var i = 0; i < anchors.length; i++) {
                var a = anchors[i];
                var name = (a.textContent || "").replace(/\s+/g, " ").trim();
                var href = a.getAttribute("href") || "";
                if (!name) continue;
                if (!href || href === "#") continue;
                var key = name.toLowerCase();
                if (seen[key]) continue;
                seen[key] = true;
                out.push({ name: name, href: href });
            }
            return out;
        },
        names: function () {
            return this.items().map(function (x) { return x.name; });
        },
        // True if target matches a sidebar menu item with decent confidence.
        has: function (target) {
            var list = this.items();
            return !!Fuzzy.bestMatch(target, list, function (x) { return x.name; }, 0.5);
        },
        go: function (target) {
            var list = this.items();
            var match = Fuzzy.bestMatch(target, list, function (x) { return x.name; }, 0.5);
            if (match) {
                UI.setStatus("Khol raha hoon: " + match.name);
                Speech.say(match.name + " khol raha hoon");
                window.location.href = match.href;
                return true;
            }
            // direct hardcoded fallbacks from config
            var t = Fuzzy.norm(target);
            if (Cfg.home && (t.indexOf("dashboard") >= 0 || t.indexOf("home") >= 0 || t.indexOf("होम") >= 0)) {
                window.location.href = Cfg.home; return true;
            }
            if (Cfg.logout && (t.indexOf("logout") >= 0 || t.indexOf("log out") >= 0 || t.indexOf("लॉगआउट") >= 0)) {
                window.location.href = Cfg.logout; return true;
            }
            UI.setStatus('Menu mein "' + target + '" nahi mila.');
            Speech.say("Ye menu nahi mila");
            return false;
        }
    };

    /* ----------------------------------------------------------- Grid module */
    var Grid = {
        search: function (query) {
            // 1) DataTables (project uses it on most report grids)
            try {
                if (window.jQuery && window.jQuery.fn && window.jQuery.fn.dataTable) {
                    var $ = window.jQuery, did = false;
                    $(".dataTable, table").each(function () {
                        if ($.fn.dataTable.isDataTable(this)) {
                            $(this).DataTable().search(query).draw();
                            did = true;
                        }
                    });
                    if (did) { UI.setStatus('Grid filter: "' + query + '"'); Speech.say(query + " filter kiya"); return true; }
                }
            } catch (x) { }

            // 2) Plain table fallback: hide rows that don't contain the query
            var tables = document.querySelectorAll("table");
            var q = query.toLowerCase(), matched = false;
            for (var t = 0; t < tables.length; t++) {
                var rows = tables[t].tBodies.length ? tables[t].tBodies[0].rows : tables[t].rows;
                for (var r = 0; r < rows.length; r++) {
                    var show = rows[r].textContent.toLowerCase().indexOf(q) >= 0;
                    rows[r].style.display = show ? "" : "none";
                    if (show) matched = true;
                }
            }
            UI.setStatus(matched ? ('Filter: "' + query + '"') : ('Kuch nahi mila: "' + query + '"'));
            Speech.say(matched ? (query + " filter kiya") : "Kuch nahi mila");
            return matched;
        }
    };

    /* ---------------------------------------------------------- Forms module */
    var Forms = {
        labelFor: function (el) {
            var txt = "";
            if (el.id) {
                var lbl = document.querySelector('label[for="' + el.id + '"]');
                if (lbl) txt += " " + lbl.textContent;
            }
            txt += " " + (el.getAttribute("placeholder") || "");
            txt += " " + (el.getAttribute("name") || "");
            txt += " " + (el.id || "");
            txt += " " + (el.getAttribute("aria-label") || "");
            return txt;
        },
        fields: function () {
            return Array.prototype.slice.call(
                document.querySelectorAll("input:not([type=hidden]):not([type=submit]):not([type=button]), textarea, select")
            ).filter(function (e) {
                if (e.offsetParent === null) return false;          // visible only
                if (e.disabled || e.readOnly) return false;          // editable only
                return true;
            });
        },
        fill: function (target, value) {
            var list = this.fields(), self = this;
            var match = Fuzzy.bestMatch(target, list, function (e) { return self.labelFor(e); }, 0.45);
            if (!match) { UI.setStatus('Field "' + target + '" nahi mila.'); return false; }

            var tag = match.tagName.toLowerCase();
            var type = (match.getAttribute("type") || "").toLowerCase();

            if (tag === "select") {
                var opts = match.options, picked = -1, bestS = 0;
                for (var i = 0; i < opts.length; i++) {
                    var s = Fuzzy.score(value, opts[i].text);
                    if (s > bestS) { bestS = s; picked = i; }
                }
                if (picked >= 0 && bestS >= 0.5) {
                    match.selectedIndex = picked;
                    this._fire(match, "change");
                }
            } else if (type === "checkbox") {
                var on = /^(yes|true|on|haan|हाँ|check|select)/i.test(value);
                match.checked = on; this._fire(match, "change");
            } else if (type === "radio") {
                var group = document.getElementsByName(match.name), bestR = null, bestRS = 0;
                for (var j = 0; j < group.length; j++) {
                    var s2 = Fuzzy.score(value, this.labelFor(group[j]));
                    if (s2 > bestRS) { bestRS = s2; bestR = group[j]; }
                }
                if (bestR) { bestR.checked = true; this._fire(bestR, "change"); }
            } else {
                match.value = value;                 // textContent-safe (value, not innerHTML)
                this._fire(match, "input"); this._fire(match, "change");
            }
            try { match.focus(); } catch (x) { }
            UI.setStatus("Bhar diya: " + value);
            Speech.say("Bhar diya");
            return true;
        },
        _fire: function (el, type) {
            var ev;
            try { ev = new Event(type, { bubbles: true }); }
            catch (x) { ev = document.createEvent("Event"); ev.initEvent(type, true, true); }
            el.dispatchEvent(ev);
        }
    };

    /* -------------------------------------------------------- Buttons module */
    var Buttons = {
        synonyms: {
            save: ["save", "सेव", "सहेज", "जमा"],
            search: ["search", "खोज", "ढूंढ", "find"],
            reset: ["reset", "रीसेट", "साफ", "clear"],
            cancel: ["cancel", "रद्द", "कैंसिल"],
            update: ["update", "अपडेट"],
            "delete": ["delete", "डिलीट", "हटा", "remove"],
            approve: ["approve", "अप्रूव", "स्वीकार", "मंजूर"],
            reject: ["reject", "रिजेक्ट", "अस्वीकार"],
            print: ["print", "प्रिंट", "छाप"],
            "export": ["export", "excel", "pdf", "डाउनलोड"],
            submit: ["submit", "generate", "invoice", "बनाओ"]
        },
        clickables: function () {
            return Array.prototype.slice.call(
                document.querySelectorAll(
                    "button, input[type=submit], input[type=button], a.btn, a[onclick], .btn"
                )
            ).filter(function (e) { return e.offsetParent !== null; });
        },
        textOf: function (el) {
            return ((el.value || "") + " " + (el.textContent || "") + " " +
                (el.title || "") + " " + (el.id || "")).trim();
        },
        click: function (target) {
            var keyset = null, t = Fuzzy.norm(target);
            for (var key in this.synonyms) {
                if (!this.synonyms.hasOwnProperty(key)) continue;
                var arr = this.synonyms[key];
                for (var k = 0; k < arr.length; k++) {
                    if (t.indexOf(Fuzzy.norm(arr[k])) >= 0 || Fuzzy.norm(arr[k]).indexOf(t) >= 0) {
                        keyset = [key].concat(arr); break;
                    }
                }
                if (keyset) break;
            }
            var searchTerms = keyset || [target];
            var list = this.clickables(), best = null, bestScore = 0, self = this;
            for (var i = 0; i < list.length; i++) {
                var label = this.textOf(list[i]);
                for (var s = 0; s < searchTerms.length; s++) {
                    var sc = Fuzzy.score(searchTerms[s], label);
                    if (sc > bestScore) { bestScore = sc; best = list[i]; }
                }
            }
            if (best && bestScore >= 0.5) {
                UI.setStatus("Click: " + (self.textOf(best).trim().substring(0, 24)));
                Speech.say(target + " kar raha hoon");
                best.click();
                return true;
            }
            UI.setStatus('Button "' + target + '" nahi mila.');
            return false;
        }
    };

    /* ---------------------------------------------------------- Intent module */
    var Intent = {
        // Ask the server (Mistral) to parse; returns a Promise of an intent object.
        remote: function (text) {
            var payload = JSON.stringify({ text: text, menu: Nav.names(), page: location.pathname.split("/").pop() });
            return fetch(ENDPOINT, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: payload,
                credentials: "same-origin"
            }).then(function (r) { return r.json(); });
        }
    };

    /* ---------------------------------------------------------- Router module */
    var Router = {
        // Open / navigation verbs incl. common typos (kolo, jao). Used by both
        // the local parser and the dispatch guard.
        navVerbs: /(kholo|kolo|khol|kkholo|open|jaao|jao|chalo|chalu|dikhao|dikha|show|go to|goto|खोलो|खोल|दिखाओ|दिखा|जाओ|पर जाओ|चलो)/i,

        looksLikeNav: function (text) {
            return this.navVerbs.test(Fuzzy.norm(text));
        },
        stripNavVerbs: function (text) {
            return (text || "")
                .replace(/(kholo|kolo|khol|kkholo|open|jaao|jao|chalo|chalu|dikhao|dikha|show|go to|goto|page|खोलो|खोल|दिखाओ|दिखा|पर जाओ|जाओ|चलो|पेज)/gi, " ")
                .replace(/\s+/g, " ").trim();
        },

        // Parse "<field> mein <value> [likho/daalo/bharo]" style commands offline.
        fillVerbs: "likho|likh do|likhdo|likhiye|daalo|daal do|dalo|daaliye|bharo|bhar do|bhardo|type karo|set karo|enter karo|fill karo",
        fillFromText: function (text) {
            // Pattern: <field> (mein|me|में) <value> [optional fill verb]
            var re = new RegExp("^(.+?)\\s+(?:mein|me|में|may)\\s+(.+?)(?:\\s+(?:" + this.fillVerbs + "))?$", "i");
            var m = text.match(re);
            if (m && m[1] && m[2]) {
                var field = m[1].trim();
                var value = m[2].trim();
                if (field && value) return { field: field, value: value };
            }
            // Pattern: <field> <value> <fill verb>   (verb mandatory, no connector)
            var re2 = new RegExp("^(.+?)\\s+(.+)\\s+(?:" + this.fillVerbs + ")$", "i");
            var m2 = text.match(re2);
            if (m2 && m2[1] && m2[2]) {
                return { field: m2[1].trim(), value: m2[2].trim() };
            }
            return null;
        },

        handle: function (text) {
            if (!text) return;
            // Local quick wins first (instant, offline) for common verbs.
            if (this.tryLocal(text)) return;

            UI.setStatus("Samajh raha hoon…");
            Intent.remote(text).then(function (intent) {
                Router.dispatch(intent || {}, text);
            }).catch(function () {
                // Even if server fails, attempt a navigation guess locally.
                if (!Router.tryLocal(text, true)) {
                    UI.setStatus("Samajh nahi paaya. Dobara boliye.");
                    Speech.say("Samajh nahi paaya");
                }
            });
        },
        tryLocal: function (text, force) {
            var t = Fuzzy.norm(text);

            // 1) explicit grid search
            var m = text.match(/(?:search|find|खोजो|खोज|ढूंढो)\s+(.+)/i);
            if (m && m[1]) return Grid.search(m[1].trim());

            // 2) offline form fill ("<field> mein <value> likho") — skip if it's
            //    actually a navigation command (those carry an open-verb).
            if (!this.looksLikeNav(text)) {
                var f = this.fillFromText(text);
                if (f && Forms.fill(f.field, f.value)) return true;
            }

            // 3) navigation verbs -> match against sidebar
            if (this.looksLikeNav(text) || force) {
                var cleaned = this.stripNavVerbs(text);
                if (Nav.go(cleaned || text)) return true;
            }

            // 4) single button verbs
            var btnWords = ["save", "search", "reset", "cancel", "update", "delete", "approve",
                "reject", "print", "export", "submit", "सेव", "खोज", "रीसेट", "प्रिंट", "अप्रूव", "रिजेक्ट"];
            for (var i = 0; i < btnWords.length; i++) {
                if (t === Fuzzy.norm(btnWords[i]) || t.indexOf(Fuzzy.norm(btnWords[i])) >= 0) {
                    if (t.split(" ").length <= 2) return Buttons.click(text);
                }
            }
            return false;
        },
        dispatch: function (intent, original) {
            var action = (intent.action || "none").toLowerCase();

            // Guard: the LLM sometimes labels an "open <menu>" command as click
            // or none. If the user used an open-verb and the target is a real
            // menu item, force navigation — menu items are never buttons.
            if (action === "click" || action === "none") {
                var candidate = intent.target || original;
                var stripped = Router.stripNavVerbs(original);
                if (Router.looksLikeNav(original) && (Nav.has(candidate) || Nav.has(stripped))) {
                    action = "navigate";
                    intent.target = Nav.has(candidate) ? candidate : stripped;
                }
            }

            switch (action) {
                case "navigate":
                    Nav.go(intent.target || original); break;
                case "search":
                case "filter":
                    Grid.search(intent.query || intent.target || original); break;
                case "fill":
                    Forms.fill(intent.target || "", intent.value || ""); break;
                case "click":
                    Buttons.click(intent.target || original); break;
                default:
                    // last resort: try a navigation guess
                    if (!Nav.go(intent.target || original)) {
                        UI.setStatus(intent.say || "Samajh nahi paaya.");
                        Speech.say(intent.say || "Samajh nahi paaya");
                    }
                    return;
            }
            if (intent.say && action !== "navigate") UI.setStatus(intent.say);
        }
    };

    /* --------------------------------------------------------------- Bootstrap */
    function boot() { UI.build(); }
    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", boot);
    } else {
        boot();
    }
})();