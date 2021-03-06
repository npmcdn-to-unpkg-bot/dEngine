(function() {
    function b(b) {
        this.tokens = [], this.tokens.links = {}, this
            .options = b || l.defaults, this.rules = a
            .normal, this.options.gfm && (this.options.tables ? this.rules = a.tables : this.rules = a.gfm)
    }

    function d(a, b) {
        if (this.options = b || l.defaults, this
            .links = a, this.rules = c.normal, this
            .renderer = this.options.renderer || new e, this.renderer
            .options = this.options, !this.links) throw new Error("Tokens array requires a `links` property.");
        this.options.gfm
            ? this.options.breaks ? this.rules = c.breaks : this.rules = c.gfm
            : this.options.pedantic && (this.rules = c.pedantic)
    }

    function e(a) { this.options = a || {} }

    function f(a) {
        this.tokens = [], this.token = null, this
            .options = a || l.defaults, this.options
            .renderer = this.options.renderer || new e, this
            .renderer = this.options.renderer, this.renderer.options = this.options
    }

    function g(a, b) {
        return a.replace(b ? /&/g : /&(?!#?\w+;)/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#39;")
    }

    function h(a) {
        return a.replace(/&(#(?:\d+)|(?:#x[0-9A-Fa-f]+)|(?:\w+));?/g,
            function(a, b) {
                return b = b.toLowerCase(), "colon" === b
                    ? ":"
                    : "#" === b.charAt(0)
                    ? "x" === b.charAt(1)
                    ? String.fromCharCode(parseInt(b.substring(2), 16))
                    : String.fromCharCode(+b.substring(1))
                    : ""
            })
    }

    function i(a, b) {
        return a = a.source, b = b || "", function c(d, e) {
            return d
                ? (e = e.source || e, e = e.replace(/(^|[^\[])\^/g, "$1"), a = a.replace(d, e), c)
                : new RegExp(a, b)
        }
    }

    function j() {}

    function k(a) {
        for (var c, d, b = 1; b < arguments.length; b++) {
            c = arguments[b];
            for (d in c) Object.prototype.hasOwnProperty.call(c, d) && (a[d] = c[d])
        }
        return a
    }

    function l(a, c, d) {
        if (d || "function" == typeof c) {
            d || (d = c, c = null), c = k({}, l.defaults, c || {});
            var h, i, e = c.highlight, j = 0;
            try {
                h = b.lex(a, c)
            } catch (a) {
                return d(a)
            }
            i = h.length;
            var m = function(a) {
                if (a) return c.highlight = e, d(a);
                var b;
                try {
                    b = f.parse(h, c)
                } catch (b) {
                    a = b
                }
                return c.highlight = e, a ? d(a) : d(null, b)
            };
            if (!e || e.length < 3) return m();
            if (delete c.highlight, !i) return m();
            for (; j < h.length; j++)
                !function(a) {
                    return "code" !== a.type
                        ? --i || m()
                        : e(a.text,
                            a.lang,
                            function(b, c) {
                                return b
                                    ? m(b)
                                    : null == c || c === a.text
                                    ? --i || m()
                                    : (a.text = c, a.escaped = !0, void (--i || m()))
                            })
                }(h[j])
        } else
            try {
                return c && (c = k({}, l.defaults, c)), f.parse(b.lex(a, c), c)
            } catch (a) {
                if (a
                        .message += "\nPlease report this to https://github.com/chjj/marked.", (c || l.defaults).silent
                ) return "<p>An error occured:</p><pre>" + g(a.message + "", !0) + "</pre>";
                throw a
            }
    }

    var a = {
        newline: /^\n+/,
        code: /^( {4}[^\n]+\n*)+/,
        fences: j,
        hr: /^( *[-*_]){3,} *(?:\n+|$)/,
        heading: /^ *(#{1,6}) *([^\n]+?) *#* *(?:\n+|$)/,
        nptable: j,
        lheading: /^([^\n]+)\n *(=|-){2,} *(?:\n+|$)/,
        blockquote: /^( *>[^\n]+(\n(?!def)[^\n]+)*\n*)+/,
        list: /^( *)(bull) [\s\S]+?(?:hr|def|\n{2,}(?! )(?!\1bull )\n*|\s*$)/,
        html: /^ *(?:comment *(?:\n|\s*$)|closed *(?:\n{2,}|\s*$)|closing *(?:\n{2,}|\s*$))/,
        def: /^ *\[([^\]]+)\]: *<?([^\s>]+)>?(?: +["(]([^\n]+)[")])? *(?:\n+|$)/,
        table: j,
        paragraph: /^((?:[^\n]+\n?(?!hr|heading|lheading|blockquote|tag|def))+)\n*/,
        text: /^[^\n]+/
    };
    a.bullet = /(?:[*+-]|\d+\.)/, a
            .item = /^( *)(bull) [^\n]*(?:\n(?!\1bull )[^\n]*)*/, a.item = i(a.item, "gm")(/bull/g, a.bullet)(),
        a.list =
            i(a
                .list)(/bull/g,
                a
                .bullet)("hr", "\\n+(?=\\1?(?:[-*_] *){3,}(?:\\n+|$))")("def", "\\n+(?=" + a.def.source + ")")(),
        a.blockquote = i(a.blockquote)("def", a.def)(), a
            ._tag =
            "(?!(?:a|em|strong|small|s|cite|q|dfn|abbr|data|time|code|var|samp|kbd|sub|sup|i|b|u|mark|ruby|rt|rp|bdi|bdo|span|br|wbr|ins|del|img)\\b)\\w+(?!:/|[^\\w\\s@]*@)\\b", a.html = i(a.html)("comment", /<!--[\s\S]*?-->/)("closed", /<(tag)[\s\S]+?<\/\1>/)("closing", /<tag(?:"[^"]*"|'[^']*'|[^'">])*?>/)(/tag/g, a._tag)(), a.paragraph = i(a.paragraph)("hr", a.hr)("heading", a.heading)("lheading", a.lheading)("blockquote", a.blockquote)("tag", "<" + a._tag)("def", a.def)(), a.normal = k({}, a), a.gfm = k({}, a.normal, { fences: /^ *(`{3,}|~{3,})[ \.]*(\S+)? *\n([\s\S]*?)\s*\1 *(?:\n+|$)/, paragraph: /^/, heading: /^ *(#{1,6}) +([^\n]+?) *#* *(?:\n+|$)/ }), a.gfm.paragraph = i(a.paragraph)("(?!", "(?!" + a.gfm.fences.source.replace("\\1", "\\2") + "|" + a.list.source.replace("\\1", "\\3") + "|")(), a.tables = k({}, a.gfm, { nptable: /^ *(\S.*\|.*)\n *([-:]+ *\|[-| :]*)\n((?:.*\|.*(?:\n|$))*)\n*/, table: /^ *\|(.+)\n *\|( *[-:]+[-| :]*)\n((?: *\|.*(?:\n|$))*)\n*/ }), b.rules = a, b.lex = function(a, c) {
            var d = new b(c);
            return d.lex(a)
        }, b.prototype.lex = function(a) {
            return a = a
                .replace(/\r\n|\r/g, "\n")
                .replace(/\t/g, "    ")
                .replace(/\u00a0/g, " ")
                .replace(/\u2424/g, "\n"), this.token(a, !0)
        }, b.prototype.token = function(b, c, d) {
            for (var e, f, g, h, i, j, k, l, m, b = b.replace(/^ +$/gm, ""); b;)
                if ((g = this.rules.newline.exec(b)) &&
                    (b = b.substring(g[0].length), g[0].length > 1 && this.tokens.push({ type: "space" })), g = this
                    .rules.code.exec(b))
                    b = b.substring(g[0].length), g = g[0].replace(/^ {4}/gm, ""), this.tokens
                        .push({ type: "code", text: this.options.pedantic ? g : g.replace(/\n+$/, "") });
                else if (g = this.rules.fences.exec(b))
                    b = b.substring(g[0].length), this.tokens.push({ type: "code", lang: g[2], text: g[3] || "" });
                else if (g = this.rules.heading.exec(b))
                    b = b.substring(g[0].length), this.tokens.push({ type: "heading", depth: g[1].length, text: g[2] });
                else if (c && (g = this.rules.nptable.exec(b))) {
                    for (b = b.substring(g[0].length), j = {
                            type: "table",
                            header: g[1].replace(/^ *| *\| *$/g, "").split(/ *\| */),
                            align: g[2].replace(/^ *|\| *$/g, "").split(/ *\| */),
                            cells: g[3].replace(/\n$/, "").split("\n")
                        }, l = 0;
                        l < j.align.length;
                        l++)
                        /^ *-+: *$/.test(j.align[l])
                            ? j.align[l] = "right"
                            : /^ *:-+: *$/.test(j.align[l])
                            ? j.align[l] = "center"
                            : /^ *:-+ *$/.test(j.align[l]) ? j.align[l] = "left" : j.align[l] = null;
                    for (l = 0; l < j.cells.length; l++) j.cells[l] = j.cells[l].split(/ *\| */);
                    this.tokens.push(j)
                } else if (g = this.rules.lheading.exec(b))
                    b = b.substring(g[0].length), this.tokens
                        .push({ type: "heading", depth: "=" === g[2] ? 1 : 2, text: g[1] });
                else if (g = this.rules.hr.exec(b)) b = b.substring(g[0].length), this.tokens.push({ type: "hr" });
                else if (g = this.rules.blockquote.exec(b))
                    b = b.substring(g[0].length), this.tokens.push({ type: "blockquote_start" }), g = g[0]
                        .replace(/^ *> ?/gm, ""), this.token(g, c, !0), this.tokens.push({ type: "blockquote_end" });
                else if (g = this.rules.list.exec(b)) {
                    for (b = b.substring(g[0].length), h = g[2], this.tokens
                            .push({ type: "list_start", ordered: h.length > 1 }), g = g[0]
                            .match(this.rules.item), e = !1, m = g.length, l = 0;
                        l < m;
                        l++)
                        j = g[l], k = j.length, j = j
                                .replace(/^ *([*+-]|\d+\.) +/, ""),
                            ~j.indexOf("\n ") &&
                            (k -= j.length, j = this.options.pedantic
                                ? j.replace(/^ {1,4}/gm, "")
                                : j
                                .replace(new RegExp("^ {1," + k + "}", "gm"), "")),
                            this.options.smartLists &&
                                l !== m - 1 &&
                                (i = a.bullet
                                    .exec(g[l + 1])[0], h === i ||
                                    h.length > 1 && i.length > 1 ||
                                    (b = g.slice(l + 1).join("\n") + b, l = m - 1)), f = e || /\n\n(?!\s*$)/.test(j), l !== m - 1 &&
                                (e = "\n" === j.charAt(j.length - 1), f || (f = e)), this.tokens
                                .push({ type: f ? "loose_item_start" : "list_item_start" }), this.token(j, !1, d), this
                                .tokens
                                .push({ type: "list_item_end" });
                    this.tokens.push({ type: "list_end" })
                } else if (g = this.rules.html.exec(b))
                    b = b.substring(g[0].length), this.tokens
                        .push({
                            type: this.options.sanitize ? "paragraph" : "html",
                            pre: !this.options.sanitizer && ("pre" === g[1] || "script" === g[1] || "style" === g[1]),
                            text: g[0]
                        });
                else if (!d && c && (g = this.rules.def.exec(b))
                ) b = b.substring(g[0].length), this.tokens.links[g[1].toLowerCase()] = { href: g[2], title: g[3] };
                else if (c && (g = this.rules.table.exec(b))) {
                    for (b = b.substring(g[0].length), j = {
                            type: "table",
                            header: g[1].replace(/^ *| *\| *$/g, "").split(/ *\| */),
                            align: g[2].replace(/^ *|\| *$/g, "").split(/ *\| */),
                            cells: g[3].replace(/(?: *\| *)?\n$/, "").split("\n")
                        }, l = 0;
                        l < j.align.length;
                        l++)
                        /^ *-+: *$/.test(j.align[l])
                            ? j.align[l] = "right"
                            : /^ *:-+: *$/.test(j.align[l])
                            ? j.align[l] = "center"
                            : /^ *:-+ *$/.test(j.align[l]) ? j.align[l] = "left" : j.align[l] = null;
                    for (l = 0; l < j.cells.length; l++)
                        j.cells[l] = j.cells[l].replace(/^ *\| *| *\| *$/g, "").split(/ *\| */);
                    this.tokens.push(j)
                } else if (c && (g = this.rules.paragraph.exec(b))
                )
                    b = b.substring(g[0].length), this.tokens
                        .push({
                            type: "paragraph",
                            text: "\n" === g[1].charAt(g[1].length - 1)
                                ? g[1].slice(0, -1)
                                : g[1
                                ]
                        });
                else if (g = this.rules.text.exec(b))
                    b = b.substring(g[0].length), this.tokens.push({ type: "text", text: g[0] });
                else if (b) throw new Error("Infinite loop on byte: " + b.charCodeAt(0));
            return this.tokens
        };
    var c = {
        escape: /^\\([\\`*{}\[\]()#+\-.!_>])/,
        autolink: /^<([^ >]+(@|:\/)[^ >]+)>/,
        url: j,
        tag: /^<!--[\s\S]*?-->|^<\/?\w+(?:"[^"]*"|'[^']*'|[^'">])*?>/,
        link: /^!?\[(inside)\]\(href\)/,
        reflink: /^!?\[(inside)\]\s*\[([^\]]*)\]/,
        nolink: /^!?\[((?:\[[^\]]*\]|[^\[\]])*)\]/,
        strong: /^__([\s\S]+?)__(?!_)|^\*\*([\s\S]+?)\*\*(?!\*)/,
        em: /^\b_((?:[^_]|__)+?)_\b|^\*((?:\*\*|[\s\S])+?)\*(?!\*)/,
        code: /^(`+)\s*([\s\S]*?[^`])\s*\1(?!`)/,
        br: /^ {2,}\n(?!\s*$)/,
        del: j,
        text: /^[\s\S]+?(?=[\\<!\[_*`]| {2,}\n|$)/
    };
    c._inside = /(?:\[[^\]]*\]|[^\[\]]|\](?=[^\[]*\]))*/, c
            ._href = /\s*<?([\s\S]*?)>?(?:\s+['"]([\s\S]*?)['"])?\s*/, c.link =
            i(c.link)("inside", c._inside)("href", c._href)(), c
            .reflink = i(c.reflink)("inside", c._inside)(), c
            .normal = k({}, c), c.pedantic = k({},
            c.normal,
            {
                strong: /^__(?=\S)([\s\S]*?\S)__(?!_)|^\*\*(?=\S)([\s\S]*?\S)\*\*(?!\*)/,
                em: /^_(?=\S)([\s\S]*?\S)_(?!_)|^\*(?=\S)([\s\S]*?\S)\*(?!\*)/
            }), c.gfm = k({},
            c.normal,
            {
                escape: i(c.escape)("])", "~|])")(),
                url: /^(https?:\/\/[^\s<]+[^<.,:;"')\]\s])/,
                del: /^~~(?=\S)([\s\S]*?\S)~~/,
                text: i(c.text)("]|", "~]|")("|", "|https?://|")()
            }), c
            .breaks = k({}, c.gfm, { br: i(c.br)("{2,}", "*")(), text: i(c.gfm.text)("{2,}", "*")() }), d.rules = c,
        d.output = function(a, b, c) {
            var e = new d(b, c);
            return e.output(a)
        }, d.prototype.output = function(a) {
            for (var c, d, e, f, b = ""; a;)
                if (f = this.rules.escape.exec(a)) a = a.substring(f[0].length), b += f[1];
                else if (f = this.rules.autolink.exec(a))
                    a = a.substring(f[0].length), "@" === f[2]
                        ? (d = ":" === f[1].charAt(6) ? this.mangle(f[1].substring(7)) : this.mangle(f[1]), e =
                            this.mangle("mailto:") + d)
                        : (d = g(f[1]), e = d), b += this.renderer.link(e, null, d);
                else if (this.inLink || !(f = this.rules.url.exec(a))) {
                    if (f = this.rules.tag.exec(a))
                        !this.inLink && /^<a /i.test(f[0])
                            ? this.inLink = !0
                            : this.inLink && /^<\/a>/i.test(f[0]) && (this.inLink = !1), a = a
                            .substring(f[0].length), b += this.options.sanitize
                            ? this.options.sanitizer ? this.options.sanitizer(f[0]) : g(f[0])
                            : f[0];
                    else if (f = this.rules.link.exec(a))
                        a = a.substring(f[0].length), this.inLink = !0, b += this
                            .outputLink(f, { href: f[2], title: f[3] }), this.inLink = !1;
                    else if ((f = this.rules.reflink.exec(a)) || (f = this.rules.nolink.exec(a))) {
                        if (a = a.substring(f[0].length), c = (f[2] || f[1]).replace(/\s+/g, " "), c = this.links[c
                            .toLowerCase()], !c || !c.href) {
                            b += f[0].charAt(0), a = f[0].substring(1) + a;
                            continue
                        }
                        this.inLink = !0, b += this.outputLink(f, c), this.inLink = !1
                    } else if (f = this.rules.strong.exec(a))
                        a = a.substring(f[0].length), b += this.renderer.strong(this.output(f[2] || f[1]));
                    else if (f = this.rules.em.exec(a))
                        a = a.substring(f[0].length), b += this.renderer.em(this.output(f[2] || f[1]));
                    else if (f = this.rules.code.exec(a))
                        a = a.substring(f[0].length), b += this.renderer.codespan(g(f[2], !0));
                    else if (f = this.rules.br.exec(a)) a = a.substring(f[0].length), b += this.renderer.br();
                    else if (f = this.rules.del.exec(a))
                        a = a.substring(f[0].length), b += this.renderer.del(this.output(f[1]));
                    else if (f = this.rules.text.exec(a))
                        a = a.substring(f[0].length), b += this.renderer.text(g(this.smartypants(f[0])));
                    else if (a) throw new Error("Infinite loop on byte: " + a.charCodeAt(0))
                } else a = a.substring(f[0].length), d = g(f[1]), e = d, b += this.renderer.link(e, null, d);
            return b
        }, d.prototype.outputLink = function(a, b) {
            var c = g(b.href), d = b.title ? g(b.title) : null;
            return "!" !== a[0].charAt(0)
                ? this.renderer.link(c, d, this.output(a[1]))
                : this.renderer.image(c, d, g(a[1]))
        }, d.prototype.smartypants = function(a) {
            return this.options.smartypants
                ? a.replace(/---/g, "�")
                .replace(/--/g, "�")
                .replace(/(^|[-\u2014\/(\[{"\s])'/g, "$1�")
                .replace(/'/g, "�")
                .replace(/(^|[-\u2014\/(\[{\u2018\s])"/g, "$1�")
                .replace(/"/g, "�")
                .replace(/\.{3}/g, "�")
                : a
        }, d.prototype.mangle = function(a) {
            if (!this.options.mangle) return a;
            for (var e, b = "", c = a.length, d = 0;
                d < c;
                d++
            ) e = a.charCodeAt(d), Math.random() > .5 && (e = "x" + e.toString(16)), b += "&#" + e + ";";
            return b
        }, e.prototype.code = function(a, b, c) {
            if (this.options.highlight) {
                var d = this.options.highlight(a, b);
                null != d && d !== a && (c = !0, a = d)
            }
            return b
                ? '<pre><code class="' +
                this.options.langPrefix +
                g(b, !0) +
                '">' +
                (c ? a : g(a, !0)) +
                "\n</code></pre>\n"
                : "<pre><code>" + (c ? a : g(a, !0)) + "\n</code></pre>"
        }, e.prototype
            .blockquote = function(a) { return "<blockquote>\n" + a + "</blockquote>\n" }, e.prototype.html =
            function(a) { return a }, e.prototype.heading = function(a, b, c) {
            return "<h" +
                b +
                ' id="' +
                this.options.headerPrefix +
                c.toLowerCase().replace(/[^\w]+/g, "-") +
                '">' +
                a +
                "</h" +
                b +
                ">\n"
        }, e.prototype
            .hr = function() { return this.options.xhtml ? "<hr/>\n" : "<hr>\n" }, e.prototype.list = function(a, b) {
            var c = b ? "ol" : "ul";
            return "<" + c + ">\n" + a + "</" + c + ">\n"
        }, e.prototype
            .listitem = function(a) { return "<li>" + a + "</li>\n" }, e.prototype.paragraph =
            function(a) { return "<p>" + a + "</p>\n" }, e.prototype
            .table =
            function(a, b) { return "<table>\n<thead>\n" + a + "</thead>\n<tbody>\n" + b + "</tbody>\n</table>\n" },
        e.prototype.tablerow = function(a) { return "<tr>\n" + a + "</tr>\n" }, e.prototype.tablecell = function(a, b) {
            var c = b.header ? "th" : "td",
                d = b.align ? "<" + c + ' style="text-align:' + b.align + '">' : "<" + c + ">";
            return d + a + "</" + c + ">\n"
        }, e.prototype
            .strong = function(a) { return "<strong>" + a + "</strong>" }, e.prototype.em =
            function(a) { return "<em>" + a + "</em>" }, e.prototype
            .codespan = function(a) { return "<code>" + a + "</code>" }, e.prototype.br =
            function() { return this.options.xhtml ? "<br/>" : "<br>" }, e.prototype.del =
            function(a) { return "<del>" + a + "</del>" }, e.prototype.link = function(a, b, c) {
            if (this.options.sanitize) {
                try {
                    var d = decodeURIComponent(h(a)).replace(/[^\w:]/g, "").toLowerCase()
                } catch (a) {
                    return ""
                }
                if (0 === d.indexOf("javascript:") || 0 === d.indexOf("vbscript:")) return ""
            }
            var e = '<a href="' + a + '"';
            return b && (e += ' title="' + b + '"'), e += ">" + c + "</a>"
        }, e.prototype.image = function(a, b, c) {
            var d = '<img src="' + a + '" alt="' + c + '"';
            return b && (d += ' title="' + b + '"'), d += this.options.xhtml ? "/>" : ">"
        }, e.prototype.text = function(a) { return a }, f.parse = function(a, b, c) {
            var d = new f(b, c);
            return d.parse(a)
        }, f.prototype.parse = function(a) {
            this.inline = new d(a.links, this.options, this.renderer), this.tokens = a.reverse();
            for (var b = ""; this.next();) b += this.tok();
            return b
        }, f.prototype
            .next = function() { return this.token = this.tokens.pop() }, f.prototype.peek =
            function() { return this.tokens[this.tokens.length - 1] || 0 }, f.prototype.parseText = function() {
            for (var a = this.token.text; "text" === this.peek().type;) a += "\n" + this.next().text;
            return this.inline.output(a)
        }, f.prototype.tok = function() {
            switch (this.token.type) {
            case "space":
                return "";
            case "hr":
                return this.renderer.hr();
            case "heading":
                return this.renderer.heading(this.inline.output(this.token.text), this.token.depth, this.token.text);
            case "code":
                return this.renderer.code(this.token.text, this.token.lang, this.token.escaped);
            case "table":
                var c, d, e, f, g, a = "", b = "";
                for (e = "", c = 0;
                    c < this.token.header.length;
                    c++
                )
                    f = { header: !0, align: this.token.align[c] }, e += this.renderer
                        .tablecell(this.inline.output(this.token
                            .header[c]),
                        { header: !0, align: this.token.align[c] });
                for (a += this.renderer.tablerow(e), c = 0; c < this.token.cells.length; c++) {
                    for (d = this.token
                            .cells[c], e = "", g = 0;
                        g < d.length;
                        g++)
                        e += this.renderer.tablecell(this.inline.output(d[g]),
                            { header: !1, align: this.token.align[g] });
                    b += this.renderer.tablerow(e)
                }
                return this.renderer.table(a, b);
            case "blockquote_start":
                for (var b = ""; "blockquote_end" !== this.next().type;) b += this.tok();
                return this.renderer.blockquote(b);
            case "list_start":
                for (var b = "", h = this.token.ordered; "list_end" !== this.next().type;) b += this.tok();
                return this.renderer.list(b, h);
            case "list_item_start":
                for (var b = "";
                    "list_item_end" !==
                        this
                        .next()
                        .type;
                ) b += "text" === this.token.type ? this.parseText() : this.tok();
                return this.renderer.listitem(b);
            case "loose_item_start":
                for (var b = ""; "list_item_end" !== this.next().type;) b += this.tok();
                return this.renderer.listitem(b);
            case "html":
                var i = this.token.pre || this.options.pedantic ? this.token.text : this.inline.output(this.token.text);
                return this.renderer.html(i);
            case "paragraph":
                return this.renderer.paragraph(this.inline.output(this.token.text));
            case "text":
                return this.renderer.paragraph(this.parseText())
            }
        }, j.exec = j, l.options = l
            .setOptions = function(a) { return k(l.defaults, a), l }, l.defaults =
        {
            gfm: !0,
            tables: !0,
            breaks: !1,
            pedantic: !1,
            sanitize: !1,
            sanitizer: null,
            mangle: !0,
            smartLists: !1,
            silent: !1,
            highlight: null,
            langPrefix: "lang-",
            smartypants: !1,
            headerPrefix: "",
            renderer: new e,
            xhtml: !1
        }, l.Parser = f, l.parser = f.parse, l.Renderer = e, l
            .Lexer = b, l.lexer = b.lex, l.InlineLexer = d, l
            .inlineLexer = d.output, l.parse = l, "undefined" != typeof module && "object" == typeof exports
            ? module.exports = l
            : "function" == typeof define && define.amd ? define(function() { return l }) : this.marked = l
}).call(function() { return this || ("undefined" != typeof window ? window : global) }());