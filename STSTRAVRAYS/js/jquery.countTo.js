!function (t) { t.fn.countTo = function (e) { return e = e || {}, t(this).each(function () { function a(t) { var e = n.formatter.call(l, t, n); f.text(e) } var n = t.extend({}, t.fn.countTo.defaults, { from: t(this).data("from"), to: t(this).data("to"), speed: t(this).data("speed"), refreshInterval: t(this).data("refresh-interval"), decimals: t(this).data("decimals") }, e), o = Math.ceil(n.speed / n.refreshInterval), r = (n.to - n.from) / o, l = this, f = t(this), i = 0, c = n.from, s = f.data("countTo") || {}; f.data("countTo", s), s.interval && clearInterval(s.interval), s.interval = setInterval(function () { i++, a(c += r), "function" == typeof n.onUpdate && n.onUpdate.call(l, c), i >= o && (f.removeData("countTo"), clearInterval(s.interval), c = n.to, "function" == typeof n.onComplete && n.onComplete.call(l, c)) }, n.refreshInterval), a(c) }) }, t.fn.countTo.defaults = { from: 0, to: 0, speed: 1e3, refreshInterval: 100, decimals: 0, formatter: function (t, e) { return t.toFixed(e.decimals) }, onUpdate: null, onComplete: null } }(jQuery);