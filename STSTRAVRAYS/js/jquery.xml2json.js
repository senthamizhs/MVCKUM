window.jQuery && function (e) { e.extend({ xml2json: function (t, n) { function r(t, a) { if (!t) return null; var u = "", c = null, i = null; t.nodeType, o(t.localName || t.nodeName), t.text || t.nodeValue; t.childNodes && t.childNodes.length > 0 && e.each(t.childNodes, function (e, t) { var n = t.nodeType, a = o(t.localName || t.nodeName), i = t.text || t.nodeValue || ""; if (8 != n) if (3 != n && 4 != n && a) (c = c || {})[a] ? (c[a].length || (c[a] = l(c[a])), c[a] = l(c[a]), c[a][c[a].length] = r(t, !0), c[a].length = c[a].length) : c[a] = r(t); else { if (i.match(/^\s+$/)) return; u += i.replace(/^\s+/, "").replace(/\s+$/, "") } }), t.attributes && t.attributes.length > 0 && (i = {}, c = c || {}, e.each(t.attributes, function (e, t) { var n = o(t.name), r = t.value; i[n] = r, c[n] ? (c[cnn] = l(c[cnn]), c[n][c[n].length] = r, c[n].length = c[n].length) : c[n] = r })), c && (c = e.extend("" != u ? new String(u) : {}, c || {}), (u = c.text ? ("object" == typeof c.text ? c.text : [c.text || ""]).concat([u]) : u) && (c.text = u), u = ""); var s = c || u; return n && (u && (s = {}), (u = s.text || u || "") && (s.text = u), a || (s = l(s))), s } if (!t) return {}; var o = function (e) { return String(e || "").replace(/-/g, "_") }, l = function (t) { return e.isArray(t) || (t = [t]), t.length = t.length, t }; if ("string" == typeof t && (t = e.text2xml(t)), t.nodeType) { if (3 == t.nodeType || 4 == t.nodeType) return t.nodeValue; var a = 9 == t.nodeType ? t.documentElement : t, u = r(a, !0); return t = null, a = null, u } }, text2xml: function (t) { var n; try { var r = e.support.opacity || e.support.style ? new DOMParser : new ActiveXObject("Microsoft.XMLDOM"); r.async = !1 } catch (e) { throw new Error("XML Parser could not be instantiated") } try { n = e.support.opacity || e.support.style ? r.parseFromString(t, "text/xml") : !!r.loadXML(t) && r } catch (e) { throw new Error("Error parsing XML string") } return n } }) }(jQuery);