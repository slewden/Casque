﻿
/*!
 * angular-translate - v2.1.0 - 2014-04-02
 * http://github.com/PascalPrecht/angular-translate
 * Copyright (c) 2014 ; Licensed MIT
 */
angular.module("pascalprecht.translate", ["ng"]).run(["$translate", function (a) {
  var b = a.storageKey(), c = a.storage();
  c ? c.get(b) ? a.use(c.get(b)) : angular.isString(a.preferredLanguage()) ? a.use(a.preferredLanguage()) : c.set(b, a.use()) : angular.isString(a.preferredLanguage()) && a.use(a.preferredLanguage())
}]), angular.module("pascalprecht.translate").provider("$translate", ["$STORAGE_KEY", function (a) {
  var b, c, d, e, f, g, h, i, j, k, l, m, n, o, p = {}, q = [], r = a, s = [], t = !1, u = "translate-cloak", v = !1, w = ".", x = function () {
    var a = window.navigator;
    return (a.language || a.browserLanguage || a.systemLanguage || a.userLanguage || "").split("-").join("_")
  }, y = function (a) {
    for (var b = [], d = angular.lowercase(a), e = 0, f = q.length; f > e; e++)
      b.push(angular.lowercase(q[e]));
    if (b.indexOf(d) > -1) return d;
    if (c) {
      var g;
      for (var h in c)
        if (c.hasOwnProperty(h) && angular.lowercase(h) === angular.lowercase(a) && (g = c[h], b.indexOf(angular.lowercase(g)) > -1)) return g
    }
    var i = a.split("_");
    return i.length > 1 && b.indexOf(angular.lowercase(i[0])) > -1 ? i[0] : void 0
  }, z = function (a, b) {
    if (!a && !b) return p; if (a && !b) {
      if (angular.isString(a)) return p[a]
    } else
      angular.isObject(p[a]) || (p[a] = {}), angular.extend(p[a], A(b));
    return this
  }; this.translations = z, this.cloakClassName = function (a) {
    return a ? (u = a, this) : u
  }; var A = function (a, b, c, d) {
    var e, f, g, h;
    b || (b = []), c || (c = {});
    for (e in a)
      a.hasOwnProperty(e) && (h = a[e], angular.isObject(h) ? A(h, b.concat(e), c, e) : (f = b.length ? "" + b.join(w) + w + e : e, b.length && e === d && (g = "" + b.join(w), c[g] = "@:" + f), c[f] = h));
    return c
  }; this.addInterpolation = function (a) {
    return s.push(a), this
  }, this.useMessageFormatInterpolation = function () { return this.useInterpolation("$translateMessageFormatInterpolation") }, this.useInterpolation = function (a) { return k = a, this }, this.useSanitizeValueStrategy = function (a) { return t = a, this }, this.preferredLanguage = function (a) { return a ? (b = a, this) : b }, this.translationNotFoundIndicator = function (a) { return this.translationNotFoundIndicatorLeft(a), this.translationNotFoundIndicatorRight(a), this }, this.translationNotFoundIndicatorLeft = function (a) { return a ? (n = a, this) : n }, this.translationNotFoundIndicatorRight = function (a) { return a ? (o = a, this) : o }, this.fallbackLanguage = function (a) { return B(a), this }; var B = function (a) { return a ? (angular.isString(a) ? (e = !0, d = [a]) : angular.isArray(a) && (e = !1, d = a), angular.isString(b) && d.push(b), this) : e ? d[0] : d }; this.use = function (a) { if (a) { if (!p[a] && !l) throw new Error("$translateProvider couldn't find translationTable for langKey: '" + a + "'"); return f = a, this } return f }; var C = function (a) { return a ? (r = a, void 0) : i ? i + r : r }; this.storageKey = C, this.useUrlLoader = function (a) { return this.useLoader("$translateUrlLoader", { url: a }) }, this.useStaticFilesLoader = function (a) { return this.useLoader("$translateStaticFilesLoader", a) }, this.useLoader = function (a, b) { return l = a, m = b || {}, this }, this.useLocalStorage = function () { return this.useStorage("$translateLocalStorage") }, this.useCookieStorage = function () { return this.useStorage("$translateCookieStorage") }, this.useStorage = function (a) { return h = a, this }, this.storagePrefix = function (a) { return a ? (i = a, this) : a }, this.useMissingTranslationHandlerLog = function () { return this.useMissingTranslationHandler("$translateMissingTranslationHandlerLog") }, this.useMissingTranslationHandler = function (a) { return j = a, this }, this.usePostCompiling = function (a) { return v = !!a, this }, this.determinePreferredLanguage = function (a) { var c = a && angular.isFunction(a) ? a() : x(); return q.length ? (b = y(c), void 0) : (b = c, this) }, this.registerAvailableLanguageKeys = function (a, b) { return a ? (q = a, b && (c = b), this) : q }, this.$get = ["$log", "$injector", "$rootScope", "$q", function (a, c, i, q) { var w, x, y, D = c.get(k || "$translateDefaultInterpolation"), E = !1, F = {}, G = {}, H = function (a, c, e) { if (angular.isArray(a)) { var g = function (a) { for (var b = {}, d = [], f = function (a) { var d = q.defer(), f = function (c) { b[a] = c, d.resolve([a, c]) }; return H(a, c, e).then(f, f), d.promise }, g = 0, h = a.length; h > g; g++) d.push(f(a[g])); return q.all(d).then(function () { return b }) }; return g(a) } var i = q.defer(); a = a.trim(); var j = function () { var a = b ? G[b] : G[f]; if (x = 0, h && !a) { var c = w.get(r); if (a = G[c], d && d.length) { var e = I(d, c); x = e > -1 ? e += 1 : 0, d.push(b) } } return a }(); return j ? j.then(function () { T(a, c, e).then(i.resolve, i.reject) }, i.reject) : T(a, c, e).then(i.resolve, i.reject), i.promise }, I = function (a, b) { for (var c = 0, d = a.length; d > c; c++) if (a[c] === b) return c; return -1 }, J = function (a) { return n && (a = [n, a].join(" ")), o && (a = [a, o].join(" ")), a }, K = function (a) { f = a, i.$emit("$translateChangeSuccess"), h && w.set(H.storageKey(), f), D.setLocale(f), angular.forEach(F, function (a, b) { F[b].setLocale(f) }), i.$emit("$translateChangeEnd") }, L = function (a) { if (!a) throw "No language key specified for loading."; var b = q.defer(); return i.$emit("$translateLoadingStart"), E = !0, c.get(l)(angular.extend(m, { key: a })).then(function (c) { var d = {}; i.$emit("$translateLoadingSuccess"), angular.isArray(c) ? angular.forEach(c, function (a) { angular.extend(d, A(a)) }) : angular.extend(d, A(c)), E = !1, b.resolve({ key: a, table: d }), i.$emit("$translateLoadingEnd") }, function (a) { i.$emit("$translateLoadingError"), b.reject(a), i.$emit("$translateLoadingEnd") }), b.promise }; if (h && (w = c.get(h), !w.get || !w.set)) throw new Error("Couldn't use storage '" + h + "', missing get() or set() method!"); angular.isFunction(D.useSanitizeValueStrategy) && D.useSanitizeValueStrategy(t), s.length && angular.forEach(s, function (a) { var d = c.get(a); d.setLocale(b || f), angular.isFunction(d.useSanitizeValueStrategy) && d.useSanitizeValueStrategy(t), F[d.getInterpolationIdentifier()] = d }); var M = function (a) { var b = q.defer(); return p.hasOwnProperty(a) ? (b.resolve(p[a]), b.promise) : (G[a].then(function (a) { z(a.key, a.table), b.resolve(a.table) }, b.reject), b.promise) }, N = function (a, b, c, d) { var e = q.defer(); return M(a).then(function (g) { g.hasOwnProperty(b) ? (d.setLocale(a), e.resolve(d.interpolate(g[b], c)), d.setLocale(f)) : e.reject() }, e.reject), e.promise }, O = function (a, b, c, d) { var e, g = p[a]; return g.hasOwnProperty(b) && (d.setLocale(a), e = d.interpolate(g[b], c), d.setLocale(f)), e }, P = function (a, b, c, e) { var f = q.defer(); if (a < d.length) { var g = d[a]; N(g, b, c, e).then(function (a) { f.resolve(a) }, function () { var d = P(a + 1, b, c, e); f.resolve(d) }) } else f.resolve(b); return f.promise }, Q = function (a, b, c, e) { var f; if (a < d.length) { var g = d[a]; f = O(g, b, c, e), f || (f = Q(a + 1, b, c, e)) } return f }, R = function (a, b, c) { return P(y > 0 ? y : x, a, b, c) }, S = function (a, b, c) { return Q(y > 0 ? y : x, a, b, c) }, T = function (a, b, e) { var g = q.defer(), h = f ? p[f] : p, i = e ? F[e] : D; if (h && h.hasOwnProperty(a)) { var k = h[a]; "@:" === k.substr(0, 2) ? H(k.substr(2), b, e).then(g.resolve, g.reject) : g.resolve(i.interpolate(k, b)) } else j && !E && c.get(j)(a, f), f && d && d.length ? R(a, b, i).then(function (a) { g.resolve(a) }, function (a) { g.reject(J(a)) }) : g.reject(J(a)); return g.promise }, U = function (a, b, e) { var g, h = f ? p[f] : p, i = e ? F[e] : D; if (h && h.hasOwnProperty(a)) { var k = h[a]; g = "@:" === k.substr(0, 2) ? U(k.substr(2), b, e) : i.interpolate(k, b) } else j && !E && c.get(j)(a, f), f && d && d.length ? (x = 0, g = S(a, b, i)) : g = J(a); return g }; if (H.preferredLanguage = function () { return b }, H.cloakClassName = function () { return u }, H.fallbackLanguage = function (a) { if (void 0 !== a && null !== a) { if (B(a), l && d && d.length) for (var b = 0, c = d.length; c > b; b++) G[d[b]] || (G[d[b]] = L(d[b])); H.use(H.use()) } return e ? d[0] : d }, H.useFallbackLanguage = function (a) { if (void 0 !== a && null !== a) if (a) { var b = I(d, a); b > -1 && (y = b) } else y = 0 }, H.proposedLanguage = function () { return g }, H.storage = function () { return w }, H.use = function (a) { if (!a) return f; var b = q.defer(); return i.$emit("$translateChangeStart"), !p[a] && l ? (g = a, G[a] = L(a).then(function (c) { z(c.key, c.table), b.resolve(c.key), g === a && (K(c.key), g = void 0) }, function (a) { g = void 0, i.$emit("$translateChangeError"), b.reject(a), i.$emit("$translateChangeEnd") })) : (b.resolve(a), K(a)), b.promise }, H.storageKey = function () { return C() }, H.isPostCompilingEnabled = function () { return v }, H.refresh = function (a) { function b() { e.resolve(), i.$emit("$translateRefreshEnd") } function c() { e.reject(), i.$emit("$translateRefreshEnd") } if (!l) throw new Error("Couldn't refresh translation table, no loader registered!"); var e = q.defer(); if (i.$emit("$translateRefreshStart"), a) p[a] ? L(a).then(function (c) { z(c.key, c.table), a === f && K(f), b() }, c) : c(); else { var g = []; if (d && d.length) for (var h = 0, j = d.length; j > h; h++) g.push(L(d[h])); f && g.push(L(f)), q.all(g).then(function (a) { angular.forEach(a, function (a) { p[a.key] && delete p[a.key], z(a.key, a.table) }), f && K(f), b() }) } return e.promise }, H.instant = function (a, e, g) { if (angular.isArray(a)) { for (var h = {}, i = 0, k = a.length; k > i; i++) h[a[i]] = H.instant(a[i], e, g); return h } if ("undefined" == typeof a || "" === a) return a; a = a.trim(); var l, m = []; b && m.push(b), f && m.push(f), d && d.length && (m = m.concat(d)); for (var n = 0, o = m.length; o > n; n++) { var q = m[n]; if (p[q] && "undefined" != typeof p[q][a] && (l = U(a, e, g)), "undefined" != typeof l) break } return l || "" === l || (l = a, j && !E && c.get(j)(a, f)), l }, l && (angular.equals(p, {}) && H.use(H.use()), d && d.length)) for (var V = 0, W = d.length; W > V; V++) G[d[V]] = L(d[V]); return H }]
}]),

angular.module("pascalprecht.translate").factory("$translateDefaultInterpolation", ["$interpolate", function (a) {
  var b, c = {}, d = "default", e = null, f = { escaped: function (a) { var b = {}; for (var c in a) a.hasOwnProperty(c) && (b[c] = angular.element("<div></div>").text(a[c]).html()); return b } }, g = function (a) { var b; return b = angular.isFunction(f[e]) ? f[e](a) : a }; return c.setLocale = function (a) { b = a }, c.getInterpolationIdentifier = function () { return d }, c.useSanitizeValueStrategy = function (a) { return e = a, this }, c.interpolate = function (b, c) { return e && (c = g(c)), a(b)(c) }, c
}]), angular.module("pascalprecht.translate").constant("$STORAGE_KEY", "NG_TRANSLATE_LANG_KEY"), angular.module("pascalprecht.translate").directive("translate", ["$translate", "$q", "$interpolate", "$compile", "$parse", "$rootScope", function (a, b, c, d, e, f) { return { restrict: "AE", scope: !0, compile: function (b, g) { var h = g.translateValues ? g.translateValues : void 0, i = g.translateInterpolation ? g.translateInterpolation : void 0, j = b[0].outerHTML.match(/translate-value-+/i); return function (b, k, l) { if (b.interpolateParams = {}, l.$observe("translate", function (a) { b.translationId = angular.equals(a, "") || !angular.isDefined(a) ? c(k.text().replace(/^\s+|\s+$/g, ""))(b.$parent) : a }), l.$observe("translateDefault", function (a) { b.defaultText = a }), h && l.$observe("translateValues", function (a) { a && b.$parent.$watch(function () { angular.extend(b.interpolateParams, e(a)(b.$parent)) }) }), j) { var m = function (a) { l.$observe(a, function (c) { b.interpolateParams[angular.lowercase(a.substr(14, 1)) + a.substr(15)] = c }) }; for (var n in l) l.hasOwnProperty(n) && "translateValue" === n.substr(0, 14) && "translateValues" !== n && m(n) } var o = function (b, c, e) { e || "undefined" == typeof c.defaultText || (b = c.defaultText), k.html(b); var f = a.isPostCompilingEnabled(), h = "undefined" != typeof g.translateCompile, i = h && "false" !== g.translateCompile; (f && !h || i) && d(k.contents())(c) }, p = function () { return h || j ? function () { var c = function () { b.translationId && b.interpolateParams && a(b.translationId, b.interpolateParams, i).then(function (a) { o(a, b, !0) }, function (a) { o(a, b, !1) }) }; b.$watch("interpolateParams", c, !0), b.$watch("translationId", c) } : function () { var c = b.$watch("translationId", function (d) { b.translationId && d && a(d, {}, i).then(function (a) { o(a, b, !0), c() }, function (a) { o(a, b, !1), c() }) }, !0) } }(), q = f.$on("$translateChangeSuccess", p); p(), b.$on("$destroy", q) } } } }]), angular.module("pascalprecht.translate").directive("translateCloak", ["$rootScope", "$translate", function (a, b) { return { compile: function (c) { a.$on("$translateLoadingSuccess", function () { c.removeClass(b.cloakClassName()) }), c.addClass(b.cloakClassName()) } } }]), angular.module("pascalprecht.translate").filter("translate", ["$parse", "$translate", function (a, b) { return function (c, d, e) { return angular.isObject(d) || (d = a(d)()), b.instant(c, d, e) } }]);


/*****************************
* Trad framework angular
*****************************/
"use strict";
angular.module("ngLocale", [], ["$provide", function (e) {
  var r = {
    ZERO: "zero", ONE: "un", TWO: "deux", FEW: "un peu", MANY: "quelques", OTHER: "autre"
  };
  e.value("$locale", {
    DATETIME_FORMATS: {
      AMPMS: ["AM", "PM"],
      DAY: ["Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"],
      MONTH: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"],
      SHORTDAY: ["Dim", "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam"],
      SHORTMONTH: ["Janv", "Févr", "Mars", "Avr", "Mai", "Juin", "Juil", "Août", "Sept", "Oct", "Nov", "Déc"],
      fullDate: "EEEE d MMMM y",
      longDate: "d MMMM y",
      medium: "d MMM y HH:mm:ss",
      mediumDate: "d MMM y",
      mediumTime: "HH:mm:ss",
      short: "dd/MM/yyyy HH:mm",
      shortDate: "dd/MM/yyyy",
      shortTime: "HH:mm"
    },
    NUMBER_FORMATS: {
      CURRENCY_SYM: "€",
      DECIMAL_SEP: ",",
      GROUP_SEP: " ",
      PATTERNS: [
        { gSize: 3, lgSize: 3, macFrac: 0, maxFrac: 3, minFrac: 0, minInt: 1, negPre: "-", negSuf: "", posPre: "", posSuf: "" },
        { gSize: 3, lgSize: 3, macFrac: 0, maxFrac: 2, minFrac: 2, minInt: 1, negPre: "-", negSuf: " ¤", posPre: "", posSuf: " ¤" }
      ]
    }, id: "fr-fr",
    pluralCat: function (e) {
      return e >= 0 && 2 >= e && 2 != e ? r.ONE : r.OTHER
    }
  })
}]);
