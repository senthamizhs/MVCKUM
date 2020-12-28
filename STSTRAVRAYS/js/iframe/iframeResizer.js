!function (e) { "use strict"; function n(n, t, i) { "addEventListener" in e ? n.addEventListener(t, i, !1) : "attachEvent" in e && n.attachEvent("on" + t, i) } function t(n, t, i) { "removeEventListener" in e ? n.removeEventListener(t, i, !1) : "detachEvent" in e && n.detachEvent("on" + t, i) } function i() { var n, t = ["moz", "webkit", "o", "ms"]; for (n = 0; n < t.length && !A; n += 1) A = e[t[n] + "RequestAnimationFrame"]; A || c("setup", "RequestAnimationFrame not supported") } function o(n) { var t = "Host page: " + n; return e.top !== e.self && (t = e.parentIFrame && e.parentIFrame.getId ? e.parentIFrame.getId() + ": " + n : "Nested host page: " + n), t } function r(e) { return W + "[" + o(e) + "]" } function a(e) { return j[e] ? j[e].log : N } function c(e, n) { s("log", e, n, a(e)) } function u(e, n) { s("info", e, n, a(e)) } function f(e, n) { s("warn", e, n, !0) } function s(n, t, i, o) { !0 === o && "object" == typeof e.console && console[n](r(t), i) } function l(i) { function o() { r("Height"), r("Width"), v(function () { y(A), h(L) }, A, "init") } function r(e) { var n = Number(j[L]["max" + e]), t = Number(j[L]["min" + e]), i = e.toLowerCase(), o = Number(A[i]); c(L, "Checking " + i + " is in range " + t + "-" + n), o < t && (o = t, c(L, "Set " + i + " to min value")), o > n && (o = n, c(L, "Set " + i + " to max value")), A[i] = "" + o } function a(e) { return T.substr(T.indexOf(":") + P + e) } function s(e) { c(L, "MessageCallback passed: {iframe: " + A.iframe.id + ", message: " + e + "}"), O("messageCallback", { iframe: A.iframe, message: JSON.parse(e) }), c(L, "--") } function l() { var n = document.body.getBoundingClientRect(), t = A.iframe.getBoundingClientRect(); return JSON.stringify({ iframeHeight: t.height, iframeWidth: t.width, clientHeight: Math.max(document.documentElement.clientHeight, e.innerHeight || 0), clientWidth: Math.max(document.documentElement.clientWidth, e.innerWidth || 0), offsetTop: parseInt(t.top - n.top, 10), offsetLeft: parseInt(t.left - n.left, 10), scrollTop: e.pageYOffset, scrollLeft: e.pageXOffset }) } function F(e, n) { x(function () { w("Send Page Info", "pageInfo:" + l(), e, n) }, 32) } function I() { function i(n, t) { function i() { j[r] ? F(j[r].iframe, r) : o() } ["scroll", "resize"].forEach(function (o) { c(r, n + o + " listener for sendPageInfo"), t(e, o, i) }) } function o() { i("Remove ", t) } var r = L; i("Add ", n), j[r].stopPageInfo = o } function M() { j[L] && j[L].stopPageInfo && (j[L].stopPageInfo(), delete j[L].stopPageInfo) } function z(e) { var n = e.getBoundingClientRect(); return g(L), { x: Math.floor(Number(n.left) + Number(S.x)), y: Math.floor(Number(n.top) + Number(S.y)) } } function E(n) { var t = n ? z(A.iframe) : { x: 0, y: 0 }, i = { x: Number(A.width) + t.x, y: Number(A.height) + t.y }; c(L, "Reposition requested from iFrame (offset x:" + t.x + " y:" + t.y + ")"), e.top !== e.self ? e.parentIFrame ? e.parentIFrame["scrollTo" + (n ? "Offset" : "")](i.x, i.y) : f(L, "Unable to scroll to requested position, window.parentIFrame not found") : (S = i, R(), c(L, "--")) } function R() { !1 !== O("scrollCallback", S) ? h(L) : p() } function C(n) { var t = n.split("#")[1] || "", i = decodeURIComponent(t), o = document.getElementById(i) || document.getElementsByName(i)[0]; o ? function () { var e = z(o); c(L, "Moving to in page link (#" + t + ") at x: " + e.x + " y: " + e.y), S = { x: e.x, y: e.y }, R(), c(L, "--") }() : e.top !== e.self ? e.parentIFrame ? e.parentIFrame.moveToAnchor(t) : c(L, "In page link #" + t + " not found and window.parentIFrame not found") : c(L, "In page link #" + t + " not found") } function O(e, n) { return d(L, e, n) } function N() { j[L].firstRun = !1 } var T = i.data, A = {}, L = null; "[iFrameResizerChild]Ready" === T ? function () { for (var e in j) w("iFrame requested init", k(e), document.getElementById(e), e) }() : W === ("" + T).substr(0, H) && T.substr(H).split(":")[0] in j ? (A = function () { var e = T.substr(H).split(":"); return { iframe: j[e[0]].iframe, id: e[0], height: e[1], width: e[2], type: e[3] } }(), L = B = A.id, !function () { var e = A.type in { true: 1, false: 1, undefined: 1 }; return e && c(L, "Ignoring init message from meta parent page"), e }() && function (e) { var n = !0; return j[e] || (n = !1, f(A.type + " No settings for " + e + ". Message was: " + T)), n }(L) && (c(L, "Received: " + T), function () { var e = !0; return null === A.iframe && (f(L, "IFrame (" + A.id + ") not found"), e = !1), e }() && function () { var e = i.origin, n = j[L].checkOrigin; if (n && "" + e != "null" && !(n.constructor === Array ? function () { var t = 0, i = !1; for (c(L, "Checking connection is from allowed list of origins: " + n) ; t < n.length; t++) if (n[t] === e) { i = !0; break } return i }() : function () { var n = j[L].remoteHost; return c(L, "Checking connection is from: " + n), e === n }())) throw new Error("Unexpected message received from: " + e + " for " + A.iframe.id + ". Message was: " + i.data + ". This error can be disabled by setting the checkOrigin: false option or by providing of array of trusted domains."); return !0 }() && function () { switch (j[L].firstRun && N(), A.type) { case "close": m(A.iframe); break; case "message": s(a(6)); break; case "scrollTo": E(!1); break; case "scrollToOffset": E(!0); break; case "pageInfo": F(j[L].iframe, L), I(); break; case "pageInfoStop": M(); break; case "inPageLink": C(a(9)); break; case "reset": b(A); break; case "init": o(), O("initCallback", A.iframe), O("resizedCallback", A); break; default: o(), O("resizedCallback", A) } }())) : u(L, "Ignored: " + T) } function d(e, n, t) { var i = null, o = null; if (j[e]) { if ("function" != typeof (i = j[e][n])) throw new TypeError(n + " on iFrame[" + e + "] is not a function"); o = i(t) } return o } function m(e) { var n = e.id; c(n, "Removing iFrame: " + n), e.parentNode && e.parentNode.removeChild(e), d(n, "closedCallback", n), c(n, "--"), delete j[n] } function g(n) { null === S && c(n, "Get page position: " + (S = { x: void 0 !== e.pageXOffset ? e.pageXOffset : document.documentElement.scrollLeft, y: void 0 !== e.pageYOffset ? e.pageYOffset : document.documentElement.scrollTop }).x + "," + S.y) } function h(n) { null !== S && (e.scrollTo(S.x, S.y), c(n, "Set page position: " + S.x + "," + S.y), p()) } function p() { S = null } function b(e) { c(e.id, "Size reset requested by " + ("init" === e.type ? "host page" : "iFrame")), g(e.id), v(function () { y(e), w("reset", "reset", e.iframe, e.id) }, e, "reset") } function y(e) { function n(n) { e.iframe.style[n] = e[n] + "px", c(e.id, "IFrame (" + o + ") " + n + " set to " + e[n] + "px") } function t(n) { T || "0" !== e[n] || (T = !0, c(o, "Hidden iFrame detected, creating visibility listener"), I()) } function i(e) { n(e), t(e) } var o = e.iframe.id; j[o] && (j[o].sizeHeight && i("height"), j[o].sizeWidth && i("width")) } function v(e, n, t) { t !== n.type && A ? (c(n.id, "Requesting animation frame"), A(e)) : e() } function w(e, n, t, i) { function o() { var o = j[i].targetOrigin; c(i, "[" + e + "] Sending msg to iframe[" + i + "] (" + n + ") targetOrigin: " + o), t.contentWindow.postMessage(W + n, o) } function r() { f(i, "[" + e + "] IFrame(" + i + ") not found") } i = i || t.id, j[i] && (t && "contentWindow" in t && null !== t.contentWindow ? o() : r()) } function k(e) { return e + ":" + j[e].bodyMarginV1 + ":" + j[e].sizeWidth + ":" + j[e].log + ":" + j[e].interval + ":" + j[e].enablePublicMethods + ":" + j[e].autoResize + ":" + j[e].bodyMargin + ":" + j[e].heightCalculationMethod + ":" + j[e].bodyBackground + ":" + j[e].bodyPadding + ":" + j[e].tolerance + ":" + j[e].inPageLinks + ":" + j[e].resizeFrom + ":" + j[e].widthCalculationMethod } function F(e, t) { function i() { var e = t && t.id || V.id + O++; return null !== document.getElementById(e) && (e += O++), e } function o() { var n = j[s].firstRun, t = j[s].heightCalculationMethod in L; !n && t && b({ iframe: e, height: 0, width: 0, type: "init" }) } function r(e) { if ("object" != typeof e) throw new TypeError("Options is not an object") } function a(e) { for (var n in V) V.hasOwnProperty(n) && (j[s][n] = e.hasOwnProperty(n) ? e[n] : V[n]) } function u(e) { return "" === e || "file://" === e ? "*" : e } var s = function (n) { return B = n, "" === n && (e.id = n = i(), N = (t || {}).log, B = n, c(n, "Added missing iframe ID: " + n + " (" + e.src + ")")), n }(e.id); s in j && "iFrameResizer" in e ? f(s, "Ignored iFrame, already setup.") : (!function (n) { n = n || {}, j[s] = { firstRun: !0, iframe: e, remoteHost: e.src.split("/").slice(0, 3).join("/") }, r(n), a(n), j[s].targetOrigin = !0 === j[s].checkOrigin ? u(j[s].remoteHost) : "*" }(t), c(s, "IFrame scrolling " + (j[s].scrolling ? "enabled" : "disabled") + " for " + s), e.style.overflow = !1 === j[s].scrolling ? "hidden" : "auto", e.scrolling = !1 === j[s].scrolling ? "no" : "yes", function () { function n(n) { 1 / 0 !== j[s][n] && 0 !== j[s][n] && (e.style[n] = j[s][n] + "px", c(s, "Set " + n + " = " + j[s][n] + "px")) } function t(e) { if (j[s]["min" + e] > j[s]["max" + e]) throw new Error("Value for min" + e + " can not be greater than max" + e) } t("Height"), t("Width"), n("maxHeight"), n("minHeight"), n("maxWidth"), n("minWidth") }(), "number" != typeof j[s].bodyMargin && "0" !== j[s].bodyMargin || (j[s].bodyMarginV1 = j[s].bodyMargin, j[s].bodyMargin = j[s].bodyMargin + "px"), function (t) { n(e, "load", function () { w("iFrame.onload", t, e), o() }), w("init", t, e) }(k(s)), Function.prototype.bind && (j[s].iframe.iFrameResizer = { close: m.bind(null, j[s].iframe), resize: w.bind(null, "Window resize", "resize", j[s].iframe), moveToAnchor: function (e) { w("Move to anchor", "moveToAnchor:" + e, j[s].iframe, s) }, sendMessage: function (e) { w("Send Message", "message:" + (e = JSON.stringify(e)), j[s].iframe, s) } })) } function x(e, n) { null === q && (q = setTimeout(function () { q = null, e() }, n)) } function I() { function n() { for (var e in j) !function (e) { function n(n) { return "0px" === j[e].iframe.style[n] } null !== j[e].iframe.offsetParent && (n("height") || n("width")) && w("Visibility change", "resize", j[e].iframe, e) }(e) } function t(e) { c("window", "Mutation observed: " + e[0].target + " " + e[0].type), x(n, 16) } var i = e.MutationObserver || e.WebKitMutationObserver; i && function () { var e = document.querySelector("body"), n = { attributes: !0, attributeOldValue: !1, characterData: !0, characterDataOldValue: !1, childList: !0, subtree: !0 }; new i(t).observe(e, n) }() } function M(e) { c("window", "Trigger event: " + e), x(function () { E("Window " + e, "resize") }, 16) } function z() { "hidden" !== document.visibilityState && (c("document", "Trigger event: Visiblity change"), x(function () { E("Tab Visable", "resize") }, 16)) } function E(e, n) { for (var t in j) (function (e) { return "parent" === j[e].resizeFrom && j[e].autoResize && !j[e].firstRun })(t) && w(e, n, document.getElementById(t), t) } function R() { n(e, "message", l), n(e, "resize", function () { M("resize") }), n(document, "visibilitychange", z), n(document, "-webkit-visibilitychange", z), n(e, "focusin", function () { M("focus") }), n(e, "focus", function () { M("focus") }) } function C() { function e(e, n) { n && (!function () { if (!n.tagName) throw new TypeError("Object is not a valid DOM element"); if ("IFRAME" !== n.tagName.toUpperCase()) throw new TypeError("Expected <IFRAME> tag, found <" + n.tagName + ">") }(), F(n, e), t.push(n)) } function n(e) { e && e.enablePublicMethods && f("enablePublicMethods option has been removed, public methods are now always available in the iFrame") } var t; return i(), R(), function (i, o) { switch (t = [], n(i), typeof o) { case "undefined": case "string": Array.prototype.forEach.call(document.querySelectorAll(o || "iframe"), e.bind(void 0, i)); break; case "object": e(i, o); break; default: throw new TypeError("Unexpected data type (" + typeof o + ")") } return t } } var O = 0, N = !1, T = !1, P = "message".length, W = "[iFrameSizer]", H = W.length, S = null, A = e.requestAnimationFrame, L = { max: 1, scroll: 1, bodyScroll: 1, documentElementScroll: 1 }, j = {}, q = null, B = "Host Page", V = { autoResize: !0, bodyBackground: null, bodyMargin: null, bodyMarginV1: 8, bodyPadding: null, checkOrigin: !1, inPageLinks: !1, enablePublicMethods: !0, heightCalculationMethod: "bodyOffset", id: "iFrameResizer", interval: 32, log: !1, maxHeight: 1 / 0, maxWidth: 1 / 0, minHeight: 0, minWidth: 0, resizeFrom: "parent", scrolling: !1, sizeHeight: !0, sizeWidth: !1, tolerance: 0, widthCalculationMethod: "scroll", closedCallback: function () { }, initCallback: function () { }, messageCallback: function () { f("MessageCallback function not defined") }, resizedCallback: function () { }, scrollCallback: function () { return !0 } }; e.jQuery && function (e) { e.fn ? e.fn.iFrameResize || (e.fn.iFrameResize = function (e) { return this.filter("iframe").each(function (n, t) { F(t, e) }).end() }) : u("", "Unable to bind to jQuery, it is not fully loaded.") }(jQuery), "function" == typeof define && define.amd ? define([], C) : "object" == typeof module && "object" == typeof module.exports ? module.exports = C() : e.iFrameResize = e.iFrameResize || C() }(window || {});