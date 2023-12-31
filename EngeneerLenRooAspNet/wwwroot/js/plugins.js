/*!
 * current-device v0.7.2 - https://github.com/matthewhudson/current-device
 * MIT Licensed
 */
!(function (n, o) {
  "object" == typeof exports && "object" == typeof module
    ? (module.exports = o())
    : "function" == typeof define && define.amd
    ? define([], o)
    : "object" == typeof exports
    ? (exports.device = o())
    : (n.device = o());
})(this, function () {
  return (function (n) {
    function o(t) {
      if (e[t]) return e[t].exports;
      var i = (e[t] = { i: t, l: !1, exports: {} });
      return n[t].call(i.exports, i, i.exports, o), (i.l = !0), i.exports;
    }
    var e = {};
    return (
      (o.m = n),
      (o.c = e),
      (o.d = function (n, e, t) {
        o.o(n, e) ||
          Object.defineProperty(n, e, {
            configurable: !1,
            enumerable: !0,
            get: t,
          });
      }),
      (o.n = function (n) {
        var e =
          n && n.__esModule
            ? function () {
                return n.default;
              }
            : function () {
                return n;
              };
        return o.d(e, "a", e), e;
      }),
      (o.o = function (n, o) {
        return Object.prototype.hasOwnProperty.call(n, o);
      }),
      (o.p = ""),
      o((o.s = 0))
    );
  })([
    function (n, o, e) {
      n.exports = e(1);
    },
    function (n, o, e) {
      "use strict";
      function t(n) {
        return -1 !== m.indexOf(n);
      }
      function i(n) {
        return w.className.match(new RegExp(n, "i"));
      }
      function r(n) {
        var o = null;
        i(n) ||
          ((o = w.className.replace(/^\s+|\s+$/g, "")),
          (w.className = o + " " + n));
      }
      function a(n) {
        i(n) && (w.className = w.className.replace(" " + n, ""));
      }
      function d() {
        b.landscape()
          ? (a("portrait"), r("landscape"), c("landscape"))
          : (a("landscape"), r("portrait"), c("portrait")),
          l();
      }
      function c(n) {
        for (var o in p) p[o](n);
      }
      function u(n) {
        for (var o = 0; o < n.length; o++) if (b[n[o]]()) return n[o];
        return "unknown";
      }
      function l() {
        b.orientation = u(["portrait", "landscape"]);
      }
      Object.defineProperty(o, "__esModule", { value: !0 });
      var s =
          "function" == typeof Symbol && "symbol" == typeof Symbol.iterator
            ? function (n) {
                return typeof n;
              }
            : function (n) {
                return n &&
                  "function" == typeof Symbol &&
                  n.constructor === Symbol &&
                  n !== Symbol.prototype
                  ? "symbol"
                  : typeof n;
              },
        f = window.device,
        b = {},
        p = [];
      window.device = b;
      var w = window.document.documentElement,
        m = window.navigator.userAgent.toLowerCase(),
        v = [
          "googletv",
          "viera",
          "smarttv",
          "internet.tv",
          "netcast",
          "nettv",
          "appletv",
          "boxee",
          "kylo",
          "roku",
          "dlnadoc",
          "roku",
          "pov_tv",
          "hbbtv",
          "ce-html",
        ];
      (b.macos = function () {
        return t("mac");
      }),
        (b.ios = function () {
          return b.iphone() || b.ipod() || b.ipad();
        }),
        (b.iphone = function () {
          return !b.windows() && t("iphone");
        }),
        (b.ipod = function () {
          return t("ipod");
        }),
        (b.ipad = function () {
          return t("ipad");
        }),
        (b.android = function () {
          return !b.windows() && t("android");
        }),
        (b.androidPhone = function () {
          return b.android() && t("mobile");
        }),
        (b.androidTablet = function () {
          return b.android() && !t("mobile");
        }),
        (b.blackberry = function () {
          return t("blackberry") || t("bb10") || t("rim");
        }),
        (b.blackberryPhone = function () {
          return b.blackberry() && !t("tablet");
        }),
        (b.blackberryTablet = function () {
          return b.blackberry() && t("tablet");
        }),
        (b.windows = function () {
          return t("windows");
        }),
        (b.windowsPhone = function () {
          return b.windows() && t("phone");
        }),
        (b.windowsTablet = function () {
          return b.windows() && t("touch") && !b.windowsPhone();
        }),
        (b.fxos = function () {
          return (t("(mobile") || t("(tablet")) && t(" rv:");
        }),
        (b.fxosPhone = function () {
          return b.fxos() && t("mobile");
        }),
        (b.fxosTablet = function () {
          return b.fxos() && t("tablet");
        }),
        (b.meego = function () {
          return t("meego");
        }),
        (b.cordova = function () {
          return window.cordova && "file:" === location.protocol;
        }),
        (b.nodeWebkit = function () {
          return "object" === s(window.process);
        }),
        (b.mobile = function () {
          return (
            b.androidPhone() ||
            b.iphone() ||
            b.ipod() ||
            b.windowsPhone() ||
            b.blackberryPhone() ||
            b.fxosPhone() ||
            b.meego()
          );
        }),
        (b.tablet = function () {
          return (
            b.ipad() ||
            b.androidTablet() ||
            b.blackberryTablet() ||
            b.windowsTablet() ||
            b.fxosTablet()
          );
        }),
        (b.desktop = function () {
          return !b.tablet() && !b.mobile();
        }),
        (b.television = function () {
          for (var n = 0; n < v.length; ) {
            if (t(v[n])) return !0;
            n++;
          }
          return !1;
        }),
        (b.portrait = function () {
          return window.innerHeight / window.innerWidth > 1;
        }),
        (b.landscape = function () {
          return window.innerHeight / window.innerWidth < 1;
        }),
        (b.noConflict = function () {
          return (window.device = f), this;
        }),
        b.ios()
          ? b.ipad()
            ? r("ios ipad tablet")
            : b.iphone()
            ? r("ios iphone mobile")
            : b.ipod() && r("ios ipod mobile")
          : b.macos()
          ? r("macos desktop")
          : b.android()
          ? r(b.androidTablet() ? "android tablet" : "android mobile")
          : b.blackberry()
          ? r(b.blackberryTablet() ? "blackberry tablet" : "blackberry mobile")
          : b.windows()
          ? r(
              b.windowsTablet()
                ? "windows tablet"
                : b.windowsPhone()
                ? "windows mobile"
                : "windows desktop"
            )
          : b.fxos()
          ? r(b.fxosTablet() ? "fxos tablet" : "fxos mobile")
          : b.meego()
          ? r("meego mobile")
          : b.nodeWebkit()
          ? r("node-webkit")
          : b.television()
          ? r("television")
          : b.desktop() && r("desktop"),
        b.cordova() && r("cordova"),
        (b.onChangeOrientation = function (n) {
          "function" == typeof n && p.push(n);
        });
      var y = "resize";
      Object.prototype.hasOwnProperty.call(window, "onorientationchange") &&
        (y = "onorientationchange"),
        window.addEventListener
          ? window.addEventListener(y, d, !1)
          : window.attachEvent
          ? window.attachEvent(y, d)
          : (window[y] = d),
        d(),
        (b.type = u(["mobile", "tablet", "desktop"])),
        (b.os = u([
          "ios",
          "iphone",
          "ipad",
          "ipod",
          "android",
          "blackberry",
          "windows",
          "fxos",
          "meego",
          "television",
        ])),
        l(),
        (o.default = b);
    },
  ]);
});

/**
 *  Ajax Autocomplete for jQuery, version %version%
 *  (c) 2017 Tomas Kirda
 *
 *  Ajax Autocomplete for jQuery is freely distributable under the terms of an MIT-style license.
 *  For details, see the web site: https://github.com/devbridge/jQuery-Autocomplete
 */

/*jslint  browser: true, white: true, single: true, this: true, multivar: true */
/*global define, window, document, jQuery, exports, require */

// Expose plugin as an AMD module if AMD loader is present:
(function (factory) {
  "use strict";
  if (typeof define === "function" && define.amd) {
    // AMD. Register as an anonymous module.
    define(["jquery"], factory);
  } else if (typeof exports === "object" && typeof require === "function") {
    // Browserify
    factory(require("jquery"));
  } else {
    // Browser globals
    factory(jQuery);
  }
})(function ($) {
  "use strict";

  var utils = (function () {
      return {
        escapeRegExChars: function (value) {
          return value.replace(/[|\\{}()[\]^$+*?.]/g, "\\$&");
        },
        createNode: function (containerClass) {
          var div = document.createElement("div");
          div.className = containerClass;
          div.style.position = "absolute";
          div.style.display = "none";
          return div;
        },
      };
    })(),
    keys = {
      ESC: 27,
      TAB: 9,
      RETURN: 13,
      LEFT: 37,
      UP: 38,
      RIGHT: 39,
      DOWN: 40,
    },
    noop = $.noop;

  function Autocomplete(el, options) {
    var that = this;

    // Shared variables:
    that.element = el;
    that.el = $(el);
    that.suggestions = [];
    that.badQueries = [];
    that.selectedIndex = -1;
    that.currentValue = that.element.value;
    that.timeoutId = null;
    that.cachedResponse = {};
    that.onChangeTimeout = null;
    that.onChange = null;
    that.isLocal = false;
    that.suggestionsContainer = null;
    that.noSuggestionsContainer = null;
    that.options = $.extend({}, Autocomplete.defaults, options);
    that.classes = {
      selected: "b-autocomplete__suggestion--selected",
      suggestion: "b-autocomplete__suggestion",
    };
    that.hint = null;
    that.hintValue = "";
    that.selection = null;

    // Initialize and set options:
    that.initialize();
    that.setOptions(options);
  }

  Autocomplete.utils = utils;

  $.Autocomplete = Autocomplete;

  Autocomplete.defaults = {
    ajaxSettings: {},
    autoSelectFirst: false,
    appendTo: "body",
    serviceUrl: null,
    lookup: null,
    onSelect: null,
    width: "auto",
    minChars: 1,
    maxHeight: 300,
    deferRequestBy: 0,
    params: {},
    formatResult: _formatResult,
    formatGroup: _formatGroup,
    delimiter: null,
    zIndex: 9999,
    type: "GET",
    noCache: false,
    onSearchStart: noop,
    onSearchComplete: noop,
    onSearchError: noop,
    preserveInput: false,
    containerClass: "b-autocomplete",
    tabDisabled: false,
    dataType: "text",
    currentRequest: null,
    triggerSelectOnValidInput: true,
    preventBadQueries: true,
    lookupFilter: _lookupFilter,
    paramName: "query",
    transformResult: _transformResult,
    showNoSuggestionNotice: false,
    noSuggestionNotice: "Результаты отсутствуют",
    orientation: "bottom",
    forceFixPosition: false,
  };

  function _lookupFilter(suggestion, originalQuery, queryLowerCase) {
    return suggestion.value.toLowerCase().indexOf(queryLowerCase) !== -1;
  }

  function _transformResult(response) {
    return typeof response === "string" ? $.parseJSON(response) : response;
  }

  function _formatResult(suggestion, currentValue) {
    // Do not replace anything if the current value is empty
    if (!currentValue) {
      return suggestion.value;
    }

    var pattern = "(" + utils.escapeRegExChars(currentValue) + ")";

    return suggestion.value
      .replace(new RegExp(pattern, "gi"), "<strong>$1</strong>")
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;")
      .replace(/"/g, "&quot;")
      .replace(/&lt;(\/?strong)&gt;/g, "<$1>");
  }

  function _formatGroup(suggestion, category) {
    return '<div class="b-autocomplete__group">' + category + "</div>";
  }

  Autocomplete.prototype = {
    initialize: function () {
      var that = this,
        suggestionSelector = "." + that.classes.suggestion,
        selected = that.classes.selected,
        options = that.options,
        container;

      // Remove autocomplete attribute to prevent native suggestions:
      that.element.setAttribute("autocomplete", "off");

      // html() deals with many types: htmlString or Element or Array or jQuery
      that.noSuggestionsContainer = $(
        '<div class="b-autocomplete--no-suggestion"></div>'
      )
        .html(this.options.noSuggestionNotice)
        .get(0);

      that.suggestionsContainer = Autocomplete.utils.createNode(
        options.containerClass
      );

      container = $(that.suggestionsContainer);

      container.appendTo(options.appendTo || "body");

      // Only set width if it was provided:
      if (options.width !== "auto") {
        container.css("width", options.width);
      }

      // Listen for mouse over event on suggestions list:
      container.on("mouseover.autocomplete", suggestionSelector, function () {
        that.activate($(this).data("index"));
      });

      // Deselect active element when mouse leaves suggestions container:
      container.on("mouseout.autocomplete", function () {
        that.selectedIndex = -1;
        container.children("." + selected).removeClass(selected);
      });

      // Listen for click event on suggestions list:
      container.on("click.autocomplete", suggestionSelector, function () {
        that.select($(this).data("index"));
      });

      container.on("click.autocomplete", function () {
        clearTimeout(that.blurTimeoutId);
      });

      that.fixPositionCapture = function () {
        if (that.visible) {
          that.fixPosition();
        }
      };

      $(window).on("resize.autocomplete", that.fixPositionCapture);

      that.el.on("keydown.autocomplete", function (e) {
        that.onKeyPress(e);
      });
      that.el.on("keyup.autocomplete", function (e) {
        that.onKeyUp(e);
      });
      that.el.on("blur.autocomplete", function () {
        that.onBlur();
      });
      that.el.on("focus.autocomplete", function () {
        that.onFocus();
      });
      that.el.on("change.autocomplete", function (e) {
        that.onKeyUp(e);
      });
      that.el.on("input.autocomplete", function (e) {
        that.onKeyUp(e);
      });
    },

    onFocus: function () {
      var that = this;

      that.fixPosition();

      if (that.el.val().length >= that.options.minChars) {
        that.onValueChange();
      }
    },

    onBlur: function () {
      var that = this;

      // If user clicked on a suggestion, hide() will
      // be canceled, otherwise close suggestions
      that.blurTimeoutId = setTimeout(function () {
        that.hide();
      }, 200);
    },

    abortAjax: function () {
      var that = this;
      if (that.currentRequest) {
        that.currentRequest.abort();
        that.currentRequest = null;
      }
    },

    setOptions: function (suppliedOptions) {
      var that = this,
        options = $.extend({}, that.options, suppliedOptions);

      that.isLocal = Array.isArray(options.lookup);

      if (that.isLocal) {
        options.lookup = that.verifySuggestionsFormat(options.lookup);
      }

      options.orientation = that.validateOrientation(
        options.orientation,
        "bottom"
      );

      // Adjust height, width and z-index:
      $(that.suggestionsContainer).css({
        "max-height": options.maxHeight + "px",
        width: options.width + "px",
        "z-index": options.zIndex,
      });

      this.options = options;
    },

    clearCache: function () {
      this.cachedResponse = {};
      this.badQueries = [];
    },

    clear: function () {
      this.clearCache();
      this.currentValue = "";
      this.suggestions = [];
    },

    disable: function () {
      var that = this;
      that.disabled = true;
      clearTimeout(that.onChangeTimeout);
      that.abortAjax();
    },

    enable: function () {
      this.disabled = false;
    },

    fixPosition: function () {
      // Use only when container has already its content

      var that = this,
        $container = $(that.suggestionsContainer),
        containerParent = $container.parent().get(0);
      // Fix position automatically when appended to body.
      // In other cases force parameter must be given.
      if (containerParent !== document.body && !that.options.forceFixPosition) {
        return;
      }

      // Choose orientation
      var orientation = that.options.orientation,
        containerHeight = $container.outerHeight(),
        height = that.el.outerHeight(),
        offset = that.el.offset(),
        styles = { top: offset.top, left: offset.left };

      if (orientation === "auto") {
        var viewPortHeight = $(window).height(),
          scrollTop = $(window).scrollTop(),
          topOverflow = -scrollTop + offset.top - containerHeight,
          bottomOverflow =
            scrollTop +
            viewPortHeight -
            (offset.top + height + containerHeight);

        orientation =
          Math.max(topOverflow, bottomOverflow) === topOverflow
            ? "top"
            : "bottom";
      }

      if (orientation === "top") {
        styles.top += -containerHeight;
      } else {
        styles.top += height;
      }

      // If container is not positioned to body,
      // correct its position using offset parent offset
      if (containerParent !== document.body) {
        var opacity = $container.css("opacity"),
          parentOffsetDiff;

        if (!that.visible) {
          $container.css("opacity", 0).show();
        }

        parentOffsetDiff = $container.offsetParent().offset();
        styles.top -= parentOffsetDiff.top;
        styles.top += containerParent.scrollTop;
        styles.left -= parentOffsetDiff.left;

        if (!that.visible) {
          $container.css("opacity", opacity).hide();
        }
      }

      if (that.options.width === "auto") {
        styles.width = that.el.outerWidth() + "px";
      }

      $container.css(styles);
    },

    isCursorAtEnd: function () {
      var that = this,
        valLength = that.el.val().length,
        selectionStart = that.element.selectionStart,
        range;

      if (typeof selectionStart === "number") {
        return selectionStart === valLength;
      }
      if (document.selection) {
        range = document.selection.createRange();
        range.moveStart("character", -valLength);
        return valLength === range.text.length;
      }
      return true;
    },

    onKeyPress: function (e) {
      var that = this;

      // If suggestions are hidden and user presses arrow down, display suggestions:
      if (
        !that.disabled &&
        !that.visible &&
        e.which === keys.DOWN &&
        that.currentValue
      ) {
        that.suggest();
        return;
      }

      if (that.disabled || !that.visible) {
        return;
      }

      switch (e.which) {
        case keys.ESC:
          that.el.val(that.currentValue);
          that.hide();
          break;
        case keys.RIGHT:
          if (that.hint && that.options.onHint && that.isCursorAtEnd()) {
            that.selectHint();
            break;
          }
          return;
        case keys.TAB:
          if (that.hint && that.options.onHint) {
            that.selectHint();
            return;
          }
          if (that.selectedIndex === -1) {
            that.hide();
            return;
          }
          that.select(that.selectedIndex);
          if (that.options.tabDisabled === false) {
            return;
          }
          break;
        case keys.RETURN:
          if (that.selectedIndex === -1) {
            that.hide();
            return;
          }
          that.select(that.selectedIndex);
          break;
        case keys.UP:
          that.moveUp();
          break;
        case keys.DOWN:
          that.moveDown();
          break;
        default:
          return;
      }

      // Cancel event if function did not return:
      e.stopImmediatePropagation();
      e.preventDefault();
    },

    onKeyUp: function (e) {
      var that = this;

      if (that.disabled) {
        return;
      }

      switch (e.which) {
        case keys.UP:
        case keys.DOWN:
          return;
      }

      clearTimeout(that.onChangeTimeout);

      if (that.currentValue !== that.el.val()) {
        that.findBestHint();
        if (that.options.deferRequestBy > 0) {
          // Defer lookup in case when value changes very quickly:
          that.onChangeTimeout = setTimeout(function () {
            that.onValueChange();
          }, that.options.deferRequestBy);
        } else {
          that.onValueChange();
        }
      }
    },

    onValueChange: function () {
      if (this.ignoreValueChange) {
        this.ignoreValueChange = false;
        return;
      }

      var that = this,
        options = that.options,
        value = that.el.val(),
        query = that.getQuery(value);

      if (that.selection && that.currentValue !== query) {
        that.selection = null;
        (options.onInvalidateSelection || $.noop).call(that.element);
      }

      clearTimeout(that.onChangeTimeout);
      that.currentValue = value;
      that.selectedIndex = -1;

      // Check existing suggestion for the match before proceeding:
      if (options.triggerSelectOnValidInput && that.isExactMatch(query)) {
        that.select(0);
        return;
      }

      if (query.length < options.minChars) {
        that.hide();
      } else {
        that.getSuggestions(query);
      }
    },

    isExactMatch: function (query) {
      var suggestions = this.suggestions;

      return (
        suggestions.length === 1 &&
        suggestions[0].value.toLowerCase() === query.toLowerCase()
      );
    },

    getQuery: function (value) {
      var delimiter = this.options.delimiter,
        parts;

      if (!delimiter) {
        return value;
      }
      parts = value.split(delimiter);
      return $.trim(parts[parts.length - 1]);
    },

    getSuggestionsLocal: function (query) {
      var that = this,
        options = that.options,
        queryLowerCase = query.toLowerCase(),
        filter = options.lookupFilter,
        limit = parseInt(options.lookupLimit, 10),
        data;

      data = {
        suggestions: $.grep(options.lookup, function (suggestion) {
          return filter(suggestion, query, queryLowerCase);
        }),
      };

      if (limit && data.suggestions.length > limit) {
        data.suggestions = data.suggestions.slice(0, limit);
      }

      return data;
    },

    getSuggestions: function (q) {
      var response,
        that = this,
        options = that.options,
        serviceUrl = options.serviceUrl,
        params,
        cacheKey,
        ajaxSettings;

      options.params[options.paramName] = q;

      if (options.onSearchStart.call(that.element, options.params) === false) {
        return;
      }

      params = options.ignoreParams ? null : options.params;

      if ($.isFunction(options.lookup)) {
        options.lookup(q, function (data) {
          that.suggestions = data.suggestions;
          that.suggest();
          options.onSearchComplete.call(that.element, q, data.suggestions);
        });
        return;
      }

      if (that.isLocal) {
        response = that.getSuggestionsLocal(q);
      } else {
        if ($.isFunction(serviceUrl)) {
          serviceUrl = serviceUrl.call(that.element, q);
        }
        cacheKey = serviceUrl + "?" + $.param(params || {});
        response = that.cachedResponse[cacheKey];
      }

      if (response && Array.isArray(response.suggestions)) {
        that.suggestions = response.suggestions;
        that.suggest();
        options.onSearchComplete.call(that.element, q, response.suggestions);
      } else if (!that.isBadQuery(q)) {
        that.abortAjax();

        ajaxSettings = {
          url: serviceUrl,
          data: params,
          type: options.type,
          dataType: options.dataType,
        };

        $.extend(ajaxSettings, options.ajaxSettings);

        that.currentRequest = $.ajax(ajaxSettings)
          .done(function (data) {
            var result;
            that.currentRequest = null;
            result = options.transformResult(data, q);
            that.processResponse(result, q, cacheKey);
            options.onSearchComplete.call(that.element, q, result.suggestions);
          })
          .fail(function (jqXHR, textStatus, errorThrown) {
            options.onSearchError.call(
              that.element,
              q,
              jqXHR,
              textStatus,
              errorThrown
            );
          });
      } else {
        options.onSearchComplete.call(that.element, q, []);
      }
    },

    isBadQuery: function (q) {
      if (!this.options.preventBadQueries) {
        return false;
      }

      var badQueries = this.badQueries,
        i = badQueries.length;

      while (i--) {
        if (q.indexOf(badQueries[i]) === 0) {
          return true;
        }
      }

      return false;
    },

    hide: function () {
      var that = this,
        container = $(that.suggestionsContainer);

      if ($.isFunction(that.options.onHide) && that.visible) {
        that.options.onHide.call(that.element, container);
      }

      that.visible = false;
      that.selectedIndex = -1;
      clearTimeout(that.onChangeTimeout);
      $(that.suggestionsContainer).hide();
      that.signalHint(null);
    },

    suggest: function () {
      if (!this.suggestions.length) {
        if (this.options.showNoSuggestionNotice) {
          this.noSuggestions();
        } else {
          this.hide();
        }
        return;
      }

      var that = this,
        options = that.options,
        groupBy = options.groupBy,
        formatResult = options.formatResult,
        value = that.getQuery(that.currentValue),
        className = that.classes.suggestion,
        classSelected = that.classes.selected,
        container = $(that.suggestionsContainer),
        noSuggestionsContainer = $(that.noSuggestionsContainer),
        beforeRender = options.beforeRender,
        html = "",
        category,
        formatGroup = function (suggestion, index) {
          var currentCategory = suggestion.data[groupBy];

          if (category === currentCategory) {
            return "";
          }

          category = currentCategory;

          return options.formatGroup(suggestion, category);
        };

      if (options.triggerSelectOnValidInput && that.isExactMatch(value)) {
        that.select(0);
        return;
      }

      // Build suggestions inner HTML:
      $.each(that.suggestions, function (i, suggestion) {
        if (groupBy) {
          html += formatGroup(suggestion, value, i);
        }

        html +=
          '<div class="' +
          className +
          '" data-index="' +
          i +
          '">' +
          formatResult(suggestion, value, i) +
          "</div>";
      });

      this.adjustContainerWidth();

      noSuggestionsContainer.detach();
      container.html(html);

      if ($.isFunction(beforeRender)) {
        beforeRender.call(that.element, container, that.suggestions);
      }

      that.fixPosition();
      container.show();

      // Select first value by default:
      if (options.autoSelectFirst) {
        that.selectedIndex = 0;
        container.scrollTop(0);
        container
          .children("." + className)
          .first()
          .addClass(classSelected);
      }

      that.visible = true;
      that.findBestHint();
    },

    noSuggestions: function () {
      var that = this,
        beforeRender = that.options.beforeRender,
        container = $(that.suggestionsContainer),
        noSuggestionsContainer = $(that.noSuggestionsContainer);

      this.adjustContainerWidth();

      // Some explicit steps. Be careful here as it easy to get
      // noSuggestionsContainer removed from DOM if not detached properly.
      noSuggestionsContainer.detach();

      // clean suggestions if any
      container.empty();
      container.append(noSuggestionsContainer);

      if ($.isFunction(beforeRender)) {
        beforeRender.call(that.element, container, that.suggestions);
      }

      that.fixPosition();

      container.show();
      that.visible = true;
    },

    adjustContainerWidth: function () {
      var that = this,
        options = that.options,
        width,
        container = $(that.suggestionsContainer);

      // If width is auto, adjust width before displaying suggestions,
      // because if instance was created before input had width, it will be zero.
      // Also it adjusts if input width has changed.
      if (options.width === "auto") {
        width = that.el.outerWidth();
        container.css("width", width > 0 ? width : 300);
      } else if (options.width === "flex") {
        // Trust the source! Unset the width property so it will be the max length
        // the containing elements.
        container.css("width", "");
      }
    },

    findBestHint: function () {
      var that = this,
        value = that.el.val().toLowerCase(),
        bestMatch = null;

      if (!value) {
        return;
      }

      $.each(that.suggestions, function (i, suggestion) {
        var foundMatch = suggestion.value.toLowerCase().indexOf(value) === 0;
        if (foundMatch) {
          bestMatch = suggestion;
        }
        return !foundMatch;
      });

      that.signalHint(bestMatch);
    },

    signalHint: function (suggestion) {
      var hintValue = "",
        that = this;
      if (suggestion) {
        hintValue =
          that.currentValue + suggestion.value.substr(that.currentValue.length);
      }
      if (that.hintValue !== hintValue) {
        that.hintValue = hintValue;
        that.hint = suggestion;
        (this.options.onHint || $.noop)(hintValue);
      }
    },

    verifySuggestionsFormat: function (suggestions) {
      // If suggestions is string array, convert them to supported format:
      if (suggestions.length && typeof suggestions[0] === "string") {
        return $.map(suggestions, function (value) {
          return { value: value, data: null };
        });
      }

      return suggestions;
    },

    validateOrientation: function (orientation, fallback) {
      orientation = $.trim(orientation || "").toLowerCase();

      if ($.inArray(orientation, ["auto", "bottom", "top"]) === -1) {
        orientation = fallback;
      }

      return orientation;
    },

    processResponse: function (result, originalQuery, cacheKey) {
      var that = this,
        options = that.options;

      result.suggestions = that.verifySuggestionsFormat(result.suggestions);

      // Cache results if cache is not disabled:
      if (!options.noCache) {
        that.cachedResponse[cacheKey] = result;
        if (options.preventBadQueries && !result.suggestions.length) {
          that.badQueries.push(originalQuery);
        }
      }

      // Return if originalQuery is not matching current query:
      if (originalQuery !== that.getQuery(that.currentValue)) {
        return;
      }

      that.suggestions = result.suggestions;
      that.suggest();
    },

    activate: function (index) {
      var that = this,
        activeItem,
        selected = that.classes.selected,
        container = $(that.suggestionsContainer),
        children = container.find("." + that.classes.suggestion);

      container.find("." + selected).removeClass(selected);

      that.selectedIndex = index;

      if (that.selectedIndex !== -1 && children.length > that.selectedIndex) {
        activeItem = children.get(that.selectedIndex);
        $(activeItem).addClass(selected);
        return activeItem;
      }

      return null;
    },

    selectHint: function () {
      var that = this,
        i = $.inArray(that.hint, that.suggestions);

      that.select(i);
    },

    select: function (i) {
      var that = this;
      that.hide();
      that.onSelect(i);
    },

    moveUp: function () {
      var that = this;

      if (that.selectedIndex === -1) {
        return;
      }

      if (that.selectedIndex === 0) {
        $(that.suggestionsContainer)
          .children("." + that.classes.suggestion)
          .first()
          .removeClass(that.classes.selected);
        that.selectedIndex = -1;
        that.ignoreValueChange = false;
        that.el.val(that.currentValue);
        that.findBestHint();
        return;
      }

      that.adjustScroll(that.selectedIndex - 1);
    },

    moveDown: function () {
      var that = this;

      if (that.selectedIndex === that.suggestions.length - 1) {
        return;
      }

      that.adjustScroll(that.selectedIndex + 1);
    },

    adjustScroll: function (index) {
      var that = this,
        activeItem = that.activate(index);

      if (!activeItem) {
        return;
      }

      var offsetTop,
        upperBound,
        lowerBound,
        heightDelta = $(activeItem).outerHeight();

      offsetTop = activeItem.offsetTop;
      upperBound = $(that.suggestionsContainer).scrollTop();
      lowerBound = upperBound + that.options.maxHeight - heightDelta;

      if (offsetTop < upperBound) {
        $(that.suggestionsContainer).scrollTop(offsetTop);
      } else if (offsetTop > lowerBound) {
        $(that.suggestionsContainer).scrollTop(
          offsetTop - that.options.maxHeight + heightDelta
        );
      }

      if (!that.options.preserveInput) {
        // During onBlur event, browser will trigger "change" event,
        // because value has changed, to avoid side effect ignore,
        // that event, so that correct suggestion can be selected
        // when clicking on suggestion with a mouse
        that.ignoreValueChange = true;
        that.el.val(that.getValue(that.suggestions[index].value));
      }

      that.signalHint(null);
    },

    onSelect: function (index) {
      var that = this,
        onSelectCallback = that.options.onSelect,
        suggestion = that.suggestions[index];

      that.currentValue = that.getValue(suggestion.value);

      if (that.currentValue !== that.el.val() && !that.options.preserveInput) {
        that.el.val(that.currentValue);
      }

      that.signalHint(null);
      that.suggestions = [];
      that.selection = suggestion;

      if ($.isFunction(onSelectCallback)) {
        onSelectCallback.call(that.element, suggestion);
      }
    },

    getValue: function (value) {
      var that = this,
        delimiter = that.options.delimiter,
        currentValue,
        parts;

      if (!delimiter) {
        return value;
      }

      currentValue = that.currentValue;
      parts = currentValue.split(delimiter);

      if (parts.length === 1) {
        return value;
      }

      return (
        currentValue.substr(
          0,
          currentValue.length - parts[parts.length - 1].length
        ) + value
      );
    },

    dispose: function () {
      var that = this;
      that.el.off(".b-autocomplete").removeData("b-autocomplete");
      $(window).off("resize.autocomplete", that.fixPositionCapture);
      $(that.suggestionsContainer).remove();
    },
  };

  // Create chainable jQuery plugin:
  $.fn.devbridgeAutocomplete = function (options, args) {
    var dataKey = "autocomplete";
    // If function invoked without argument return
    // instance of the first matched element:
    if (!arguments.length) {
      return this.first().data(dataKey);
    }

    return this.each(function () {
      var inputElement = $(this),
        instance = inputElement.data(dataKey);

      if (typeof options === "string") {
        if (instance && typeof instance[options] === "function") {
          instance[options](args);
        }
      } else {
        // If instance already exists, destroy it:
        if (instance && instance.dispose) {
          instance.dispose();
        }
        instance = new Autocomplete(this, options);
        inputElement.data(dataKey, instance);
      }
    });
  };

  // Don't overwrite if it already exists
  if (!$.fn.autocomplete) {
    $.fn.autocomplete = $.fn.devbridgeAutocomplete;
  }
});

/*!
 * JavaScript Cookie v2.2.0
 * https://github.com/js-cookie/js-cookie
 *
 * Copyright 2006, 2015 Klaus Hartl & Fagner Brack
 * Released under the MIT license
 */
(function (factory) {
  var registeredInModuleLoader = false;
  if (typeof define === "function" && define.amd) {
    define(factory);
    registeredInModuleLoader = true;
  }
  if (typeof exports === "object") {
    module.exports = factory();
    registeredInModuleLoader = true;
  }
  if (!registeredInModuleLoader) {
    var OldCookies = window.Cookies;
    var api = (window.Cookies = factory());
    api.noConflict = function () {
      window.Cookies = OldCookies;
      return api;
    };
  }
})(function () {
  function extend() {
    var i = 0;
    var result = {};
    for (; i < arguments.length; i++) {
      var attributes = arguments[i];
      for (var key in attributes) {
        result[key] = attributes[key];
      }
    }
    return result;
  }

  function init(converter) {
    function api(key, value, attributes) {
      var result;
      if (typeof document === "undefined") {
        return;
      }

      // Write

      if (arguments.length > 1) {
        attributes = extend(
          {
            path: "/",
          },
          api.defaults,
          attributes
        );

        if (typeof attributes.expires === "number") {
          var expires = new Date();
          expires.setMilliseconds(
            expires.getMilliseconds() + attributes.expires * 864e5
          );
          attributes.expires = expires;
        }

        // We're using "expires" because "max-age" is not supported by IE
        attributes.expires = attributes.expires
          ? attributes.expires.toUTCString()
          : "";

        try {
          result = JSON.stringify(value);
          if (/^[\{\[]/.test(result)) {
            value = result;
          }
        } catch (e) {}

        if (!converter.write) {
          value = encodeURIComponent(String(value)).replace(
            /%(23|24|26|2B|3A|3C|3E|3D|2F|3F|40|5B|5D|5E|60|7B|7D|7C)/g,
            decodeURIComponent
          );
        } else {
          value = converter.write(value, key);
        }

        key = encodeURIComponent(String(key));
        key = key.replace(/%(23|24|26|2B|5E|60|7C)/g, decodeURIComponent);
        key = key.replace(/[\(\)]/g, escape);

        var stringifiedAttributes = "";

        for (var attributeName in attributes) {
          if (!attributes[attributeName]) {
            continue;
          }
          stringifiedAttributes += "; " + attributeName;
          if (attributes[attributeName] === true) {
            continue;
          }
          stringifiedAttributes += "=" + attributes[attributeName];
        }
        return (document.cookie = key + "=" + value + stringifiedAttributes);
      }

      // Read

      if (!key) {
        result = {};
      }

      // To prevent the for loop in the first place assign an empty array
      // in case there are no cookies at all. Also prevents odd result when
      // calling "get()"
      var cookies = document.cookie ? document.cookie.split("; ") : [];
      var rdecode = /(%[0-9A-Z]{2})+/g;
      var i = 0;

      for (; i < cookies.length; i++) {
        var parts = cookies[i].split("=");
        var cookie = parts.slice(1).join("=");

        if (!this.json && cookie.charAt(0) === '"') {
          cookie = cookie.slice(1, -1);
        }

        try {
          var name = parts[0].replace(rdecode, decodeURIComponent);
          cookie = converter.read
            ? converter.read(cookie, name)
            : converter(cookie, name) ||
              cookie.replace(rdecode, decodeURIComponent);

          if (this.json) {
            try {
              cookie = JSON.parse(cookie);
            } catch (e) {}
          }

          if (key === name) {
            result = cookie;
            break;
          }

          if (!key) {
            result[name] = cookie;
          }
        } catch (e) {}
      }

      return result;
    }

    api.set = api;
    api.get = function (key) {
      return api.call(api, key);
    };
    api.getJSON = function () {
      return api.apply(
        {
          json: true,
        },
        [].slice.call(arguments)
      );
    };
    api.defaults = {};

    api.remove = function (key, attributes) {
      api(
        key,
        "",
        extend(attributes, {
          expires: -1,
        })
      );
    };

    api.withConverter = init;

    return api;
  }

  return init(function () {});
});

/**
 * Owl Carousel v2.2.1
 * Copyright 2013-2017 David Deutsch
 * Licensed under  ()
 */
!(function (a, b, c, d) {
  function e(b, c) {
    (this.settings = null),
      (this.options = a.extend({}, e.Defaults, c)),
      (this.$element = a(b)),
      (this._handlers = {}),
      (this._plugins = {}),
      (this._supress = {}),
      (this._current = null),
      (this._speed = null),
      (this._coordinates = []),
      (this._breakpoint = null),
      (this._width = null),
      (this._items = []),
      (this._clones = []),
      (this._mergers = []),
      (this._widths = []),
      (this._invalidated = {}),
      (this._pipe = []),
      (this._drag = {
        time: null,
        target: null,
        pointer: null,
        stage: { start: null, current: null },
        direction: null,
      }),
      (this._states = {
        current: {},
        tags: {
          initializing: ["busy"],
          animating: ["busy"],
          dragging: ["interacting"],
        },
      }),
      a.each(
        ["onResize", "onThrottledResize"],
        a.proxy(function (b, c) {
          this._handlers[c] = a.proxy(this[c], this);
        }, this)
      ),
      a.each(
        e.Plugins,
        a.proxy(function (a, b) {
          this._plugins[a.charAt(0).toLowerCase() + a.slice(1)] = new b(this);
        }, this)
      ),
      a.each(
        e.Workers,
        a.proxy(function (b, c) {
          this._pipe.push({ filter: c.filter, run: a.proxy(c.run, this) });
        }, this)
      ),
      this.setup(),
      this.initialize();
  }
  (e.Defaults = {
    items: 3,
    loop: !1,
    center: !1,
    rewind: !1,
    mouseDrag: !0,
    touchDrag: !0,
    pullDrag: !0,
    freeDrag: !1,
    margin: 0,
    stagePadding: 0,
    merge: !1,
    mergeFit: !0,
    autoWidth: !1,
    startPosition: 0,
    rtl: !1,
    smartSpeed: 250,
    fluidSpeed: !1,
    dragEndSpeed: !1,
    responsive: {},
    responsiveRefreshRate: 200,
    responsiveBaseElement: b,
    fallbackEasing: "swing",
    info: !1,
    nestedItemSelector: !1,
    itemElement: "div",
    stageElement: "div",
    refreshClass: "owl-refresh",
    loadedClass: "owl-loaded",
    loadingClass: "owl-loading",
    rtlClass: "owl-rtl",
    responsiveClass: "owl-responsive",
    dragClass: "owl-drag",
    itemClass: "owl-item",
    stageClass: "owl-stage",
    stageOuterClass: "owl-stage-outer",
    grabClass: "owl-grab",
  }),
    (e.Width = { Default: "default", Inner: "inner", Outer: "outer" }),
    (e.Type = { Event: "event", State: "state" }),
    (e.Plugins = {}),
    (e.Workers = [
      {
        filter: ["width", "settings"],
        run: function () {
          this._width = this.$element.width();
        },
      },
      {
        filter: ["width", "items", "settings"],
        run: function (a) {
          a.current = this._items && this._items[this.relative(this._current)];
        },
      },
      {
        filter: ["items", "settings"],
        run: function () {
          this.$stage.children(".cloned").remove();
        },
      },
      {
        filter: ["width", "items", "settings"],
        run: function (a) {
          var b = this.settings.margin || "",
            c = !this.settings.autoWidth,
            d = this.settings.rtl,
            e = {
              width: "auto",
              "margin-left": d ? b : "",
              "margin-right": d ? "" : b,
            };
          !c && this.$stage.children().css(e), (a.css = e);
        },
      },
      {
        filter: ["width", "items", "settings"],
        run: function (a) {
          var b =
              (this.width() / this.settings.items).toFixed(3) -
              this.settings.margin,
            c = null,
            d = this._items.length,
            e = !this.settings.autoWidth,
            f = [];
          for (a.items = { merge: !1, width: b }; d--; )
            (c = this._mergers[d]),
              (c =
                (this.settings.mergeFit && Math.min(c, this.settings.items)) ||
                c),
              (a.items.merge = c > 1 || a.items.merge),
              (f[d] = e ? b * c : this._items[d].width());
          this._widths = f;
        },
      },
      {
        filter: ["items", "settings"],
        run: function () {
          var b = [],
            c = this._items,
            d = this.settings,
            e = Math.max(2 * d.items, 4),
            f = 2 * Math.ceil(c.length / 2),
            g = d.loop && c.length ? (d.rewind ? e : Math.max(e, f)) : 0,
            h = "",
            i = "";
          for (g /= 2; g--; )
            b.push(this.normalize(b.length / 2, !0)),
              (h += c[b[b.length - 1]][0].outerHTML),
              b.push(this.normalize(c.length - 1 - (b.length - 1) / 2, !0)),
              (i = c[b[b.length - 1]][0].outerHTML + i);
          (this._clones = b),
            a(h).addClass("cloned").appendTo(this.$stage),
            a(i).addClass("cloned").prependTo(this.$stage);
        },
      },
      {
        filter: ["width", "items", "settings"],
        run: function () {
          for (
            var a = this.settings.rtl ? 1 : -1,
              b = this._clones.length + this._items.length,
              c = -1,
              d = 0,
              e = 0,
              f = [];
            ++c < b;

          )
            (d = f[c - 1] || 0),
              (e = this._widths[this.relative(c)] + this.settings.margin),
              f.push(d + e * a);
          this._coordinates = f;
        },
      },
      {
        filter: ["width", "items", "settings"],
        run: function () {
          var a = this.settings.stagePadding,
            b = this._coordinates,
            c = {
              width: Math.ceil(Math.abs(b[b.length - 1])) + 2 * a,
              "padding-left": a || "",
              "padding-right": a || "",
            };
          this.$stage.css(c);
        },
      },
      {
        filter: ["width", "items", "settings"],
        run: function (a) {
          var b = this._coordinates.length,
            c = !this.settings.autoWidth,
            d = this.$stage.children();
          if (c && a.items.merge)
            for (; b--; )
              (a.css.width = this._widths[this.relative(b)]),
                d.eq(b).css(a.css);
          else c && ((a.css.width = a.items.width), d.css(a.css));
        },
      },
      {
        filter: ["items"],
        run: function () {
          this._coordinates.length < 1 && this.$stage.removeAttr("style");
        },
      },
      {
        filter: ["width", "items", "settings"],
        run: function (a) {
          (a.current = a.current ? this.$stage.children().index(a.current) : 0),
            (a.current = Math.max(
              this.minimum(),
              Math.min(this.maximum(), a.current)
            )),
            this.reset(a.current);
        },
      },
      {
        filter: ["position"],
        run: function () {
          this.animate(this.coordinates(this._current));
        },
      },
      {
        filter: ["width", "position", "items", "settings"],
        run: function () {
          var a,
            b,
            c,
            d,
            e = this.settings.rtl ? 1 : -1,
            f = 2 * this.settings.stagePadding,
            g = this.coordinates(this.current()) + f,
            h = g + this.width() * e,
            i = [];
          for (c = 0, d = this._coordinates.length; c < d; c++)
            (a = this._coordinates[c - 1] || 0),
              (b = Math.abs(this._coordinates[c]) + f * e),
              ((this.op(a, "<=", g) && this.op(a, ">", h)) ||
                (this.op(b, "<", g) && this.op(b, ">", h))) &&
                i.push(c);
          this.$stage.children(".active").removeClass("active"),
            this.$stage
              .children(":eq(" + i.join("), :eq(") + ")")
              .addClass("active"),
            this.settings.center &&
              (this.$stage.children(".center").removeClass("center"),
              this.$stage.children().eq(this.current()).addClass("center"));
        },
      },
    ]),
    (e.prototype.initialize = function () {
      if (
        (this.enter("initializing"),
        this.trigger("initialize"),
        this.$element.toggleClass(this.settings.rtlClass, this.settings.rtl),
        this.settings.autoWidth && !this.is("pre-loading"))
      ) {
        var b, c, e;
        (b = this.$element.find("img")),
          (c = this.settings.nestedItemSelector
            ? "." + this.settings.nestedItemSelector
            : d),
          (e = this.$element.children(c).width()),
          b.length && e <= 0 && this.preloadAutoWidthImages(b);
      }
      this.$element.addClass(this.options.loadingClass),
        (this.$stage = a(
          "<" +
            this.settings.stageElement +
            ' class="' +
            this.settings.stageClass +
            '"/>'
        ).wrap('<div class="' + this.settings.stageOuterClass + '"/>')),
        this.$element.append(this.$stage.parent()),
        this.replace(this.$element.children().not(this.$stage.parent())),
        this.$element.is(":visible")
          ? this.refresh()
          : this.invalidate("width"),
        this.$element
          .removeClass(this.options.loadingClass)
          .addClass(this.options.loadedClass),
        this.registerEventHandlers(),
        this.leave("initializing"),
        this.trigger("initialized");
    }),
    (e.prototype.setup = function () {
      var b = this.viewport(),
        c = this.options.responsive,
        d = -1,
        e = null;
      c
        ? (a.each(c, function (a) {
            a <= b && a > d && (d = Number(a));
          }),
          (e = a.extend({}, this.options, c[d])),
          "function" == typeof e.stagePadding &&
            (e.stagePadding = e.stagePadding()),
          delete e.responsive,
          e.responsiveClass &&
            this.$element.attr(
              "class",
              this.$element
                .attr("class")
                .replace(
                  new RegExp(
                    "(" + this.options.responsiveClass + "-)\\S+\\s",
                    "g"
                  ),
                  "$1" + d
                )
            ))
        : (e = a.extend({}, this.options)),
        this.trigger("change", { property: { name: "settings", value: e } }),
        (this._breakpoint = d),
        (this.settings = e),
        this.invalidate("settings"),
        this.trigger("changed", {
          property: { name: "settings", value: this.settings },
        });
    }),
    (e.prototype.optionsLogic = function () {
      this.settings.autoWidth &&
        ((this.settings.stagePadding = !1), (this.settings.merge = !1));
    }),
    (e.prototype.prepare = function (b) {
      var c = this.trigger("prepare", { content: b });
      return (
        c.data ||
          (c.data = a("<" + this.settings.itemElement + "/>")
            .addClass(this.options.itemClass)
            .append(b)),
        this.trigger("prepared", { content: c.data }),
        c.data
      );
    }),
    (e.prototype.update = function () {
      for (
        var b = 0,
          c = this._pipe.length,
          d = a.proxy(function (a) {
            return this[a];
          }, this._invalidated),
          e = {};
        b < c;

      )
        (this._invalidated.all || a.grep(this._pipe[b].filter, d).length > 0) &&
          this._pipe[b].run(e),
          b++;
      (this._invalidated = {}), !this.is("valid") && this.enter("valid");
    }),
    (e.prototype.width = function (a) {
      switch ((a = a || e.Width.Default)) {
        case e.Width.Inner:
        case e.Width.Outer:
          return this._width;
        default:
          return (
            this._width - 2 * this.settings.stagePadding + this.settings.margin
          );
      }
    }),
    (e.prototype.refresh = function () {
      this.enter("refreshing"),
        this.trigger("refresh"),
        this.setup(),
        this.optionsLogic(),
        this.$element.addClass(this.options.refreshClass),
        this.update(),
        this.$element.removeClass(this.options.refreshClass),
        this.leave("refreshing"),
        this.trigger("refreshed");
    }),
    (e.prototype.onThrottledResize = function () {
      b.clearTimeout(this.resizeTimer),
        (this.resizeTimer = b.setTimeout(
          this._handlers.onResize,
          this.settings.responsiveRefreshRate
        ));
    }),
    (e.prototype.onResize = function () {
      return (
        !!this._items.length &&
        this._width !== this.$element.width() &&
        !!this.$element.is(":visible") &&
        (this.enter("resizing"),
        this.trigger("resize").isDefaultPrevented()
          ? (this.leave("resizing"), !1)
          : (this.invalidate("width"),
            this.refresh(),
            this.leave("resizing"),
            void this.trigger("resized")))
      );
    }),
    (e.prototype.registerEventHandlers = function () {
      a.support.transition &&
        this.$stage.on(
          a.support.transition.end + ".owl.core",
          a.proxy(this.onTransitionEnd, this)
        ),
        this.settings.responsive !== !1 &&
          this.on(b, "resize", this._handlers.onThrottledResize),
        this.settings.mouseDrag &&
          (this.$element.addClass(this.options.dragClass),
          this.$stage.on("mousedown.owl.core", a.proxy(this.onDragStart, this)),
          this.$stage.on(
            "dragstart.owl.core selectstart.owl.core",
            function () {
              return !1;
            }
          )),
        this.settings.touchDrag &&
          (this.$stage.on(
            "touchstart.owl.core",
            a.proxy(this.onDragStart, this)
          ),
          this.$stage.on(
            "touchcancel.owl.core",
            a.proxy(this.onDragEnd, this)
          ));
    }),
    (e.prototype.onDragStart = function (b) {
      var d = null;
      3 !== b.which &&
        (a.support.transform
          ? ((d = this.$stage
              .css("transform")
              .replace(/.*\(|\)| /g, "")
              .split(",")),
            (d = {
              x: d[16 === d.length ? 12 : 4],
              y: d[16 === d.length ? 13 : 5],
            }))
          : ((d = this.$stage.position()),
            (d = {
              x: this.settings.rtl
                ? d.left +
                  this.$stage.width() -
                  this.width() +
                  this.settings.margin
                : d.left,
              y: d.top,
            })),
        this.is("animating") &&
          (a.support.transform ? this.animate(d.x) : this.$stage.stop(),
          this.invalidate("position")),
        this.$element.toggleClass(
          this.options.grabClass,
          "mousedown" === b.type
        ),
        this.speed(0),
        (this._drag.time = new Date().getTime()),
        (this._drag.target = a(b.target)),
        (this._drag.stage.start = d),
        (this._drag.stage.current = d),
        (this._drag.pointer = this.pointer(b)),
        a(c).on(
          "mouseup.owl.core touchend.owl.core",
          a.proxy(this.onDragEnd, this)
        ),
        a(c).one(
          "mousemove.owl.core touchmove.owl.core",
          a.proxy(function (b) {
            var d = this.difference(this._drag.pointer, this.pointer(b));
            a(c).on(
              "mousemove.owl.core touchmove.owl.core",
              a.proxy(this.onDragMove, this)
            ),
              (Math.abs(d.x) < Math.abs(d.y) && this.is("valid")) ||
                (b.preventDefault(),
                this.enter("dragging"),
                this.trigger("drag"));
          }, this)
        ));
    }),
    (e.prototype.onDragMove = function (a) {
      var b = null,
        c = null,
        d = null,
        e = this.difference(this._drag.pointer, this.pointer(a)),
        f = this.difference(this._drag.stage.start, e);
      this.is("dragging") &&
        (a.preventDefault(),
        this.settings.loop
          ? ((b = this.coordinates(this.minimum())),
            (c = this.coordinates(this.maximum() + 1) - b),
            (f.x = ((((f.x - b) % c) + c) % c) + b))
          : ((b = this.settings.rtl
              ? this.coordinates(this.maximum())
              : this.coordinates(this.minimum())),
            (c = this.settings.rtl
              ? this.coordinates(this.minimum())
              : this.coordinates(this.maximum())),
            (d = this.settings.pullDrag ? (-1 * e.x) / 5 : 0),
            (f.x = Math.max(Math.min(f.x, b + d), c + d))),
        (this._drag.stage.current = f),
        this.animate(f.x));
    }),
    (e.prototype.onDragEnd = function (b) {
      var d = this.difference(this._drag.pointer, this.pointer(b)),
        e = this._drag.stage.current,
        f = (d.x > 0) ^ this.settings.rtl ? "left" : "right";
      a(c).off(".owl.core"),
        this.$element.removeClass(this.options.grabClass),
        ((0 !== d.x && this.is("dragging")) || !this.is("valid")) &&
          (this.speed(this.settings.dragEndSpeed || this.settings.smartSpeed),
          this.current(this.closest(e.x, 0 !== d.x ? f : this._drag.direction)),
          this.invalidate("position"),
          this.update(),
          (this._drag.direction = f),
          (Math.abs(d.x) > 3 || new Date().getTime() - this._drag.time > 300) &&
            this._drag.target.one("click.owl.core", function () {
              return !1;
            })),
        this.is("dragging") &&
          (this.leave("dragging"), this.trigger("dragged"));
    }),
    (e.prototype.closest = function (b, c) {
      var d = -1,
        e = 30,
        f = this.width(),
        g = this.coordinates();
      return (
        this.settings.freeDrag ||
          a.each(
            g,
            a.proxy(function (a, h) {
              return (
                "left" === c && b > h - e && b < h + e
                  ? (d = a)
                  : "right" === c && b > h - f - e && b < h - f + e
                  ? (d = a + 1)
                  : this.op(b, "<", h) &&
                    this.op(b, ">", g[a + 1] || h - f) &&
                    (d = "left" === c ? a + 1 : a),
                d === -1
              );
            }, this)
          ),
        this.settings.loop ||
          (this.op(b, ">", g[this.minimum()])
            ? (d = b = this.minimum())
            : this.op(b, "<", g[this.maximum()]) && (d = b = this.maximum())),
        d
      );
    }),
    (e.prototype.animate = function (b) {
      var c = this.speed() > 0;
      this.is("animating") && this.onTransitionEnd(),
        c && (this.enter("animating"), this.trigger("translate")),
        a.support.transform3d && a.support.transition
          ? this.$stage.css({
              transform: "translate3d(" + b + "px,0px,0px)",
              transition: this.speed() / 1e3 + "s",
            })
          : c
          ? this.$stage.animate(
              { left: b + "px" },
              this.speed(),
              this.settings.fallbackEasing,
              a.proxy(this.onTransitionEnd, this)
            )
          : this.$stage.css({ left: b + "px" });
    }),
    (e.prototype.is = function (a) {
      return this._states.current[a] && this._states.current[a] > 0;
    }),
    (e.prototype.current = function (a) {
      if (a === d) return this._current;
      if (0 === this._items.length) return d;
      if (((a = this.normalize(a)), this._current !== a)) {
        var b = this.trigger("change", {
          property: { name: "position", value: a },
        });
        b.data !== d && (a = this.normalize(b.data)),
          (this._current = a),
          this.invalidate("position"),
          this.trigger("changed", {
            property: { name: "position", value: this._current },
          });
      }
      return this._current;
    }),
    (e.prototype.invalidate = function (b) {
      return (
        "string" === a.type(b) &&
          ((this._invalidated[b] = !0),
          this.is("valid") && this.leave("valid")),
        a.map(this._invalidated, function (a, b) {
          return b;
        })
      );
    }),
    (e.prototype.reset = function (a) {
      (a = this.normalize(a)),
        a !== d &&
          ((this._speed = 0),
          (this._current = a),
          this.suppress(["translate", "translated"]),
          this.animate(this.coordinates(a)),
          this.release(["translate", "translated"]));
    }),
    (e.prototype.normalize = function (a, b) {
      var c = this._items.length,
        e = b ? 0 : this._clones.length;
      return (
        !this.isNumeric(a) || c < 1
          ? (a = d)
          : (a < 0 || a >= c + e) &&
            (a = ((((a - e / 2) % c) + c) % c) + e / 2),
        a
      );
    }),
    (e.prototype.relative = function (a) {
      return (a -= this._clones.length / 2), this.normalize(a, !0);
    }),
    (e.prototype.maximum = function (a) {
      var b,
        c,
        d,
        e = this.settings,
        f = this._coordinates.length;
      if (e.loop) f = this._clones.length / 2 + this._items.length - 1;
      else if (e.autoWidth || e.merge) {
        for (
          b = this._items.length,
            c = this._items[--b].width(),
            d = this.$element.width();
          b-- &&
          ((c += this._items[b].width() + this.settings.margin), !(c > d));

        );
        f = b + 1;
      } else
        f = e.center ? this._items.length - 1 : this._items.length - e.items;
      return a && (f -= this._clones.length / 2), Math.max(f, 0);
    }),
    (e.prototype.minimum = function (a) {
      return a ? 0 : this._clones.length / 2;
    }),
    (e.prototype.items = function (a) {
      return a === d
        ? this._items.slice()
        : ((a = this.normalize(a, !0)), this._items[a]);
    }),
    (e.prototype.mergers = function (a) {
      return a === d
        ? this._mergers.slice()
        : ((a = this.normalize(a, !0)), this._mergers[a]);
    }),
    (e.prototype.clones = function (b) {
      var c = this._clones.length / 2,
        e = c + this._items.length,
        f = function (a) {
          return a % 2 === 0 ? e + a / 2 : c - (a + 1) / 2;
        };
      return b === d
        ? a.map(this._clones, function (a, b) {
            return f(b);
          })
        : a.map(this._clones, function (a, c) {
            return a === b ? f(c) : null;
          });
    }),
    (e.prototype.speed = function (a) {
      return a !== d && (this._speed = a), this._speed;
    }),
    (e.prototype.coordinates = function (b) {
      var c,
        e = 1,
        f = b - 1;
      return b === d
        ? a.map(
            this._coordinates,
            a.proxy(function (a, b) {
              return this.coordinates(b);
            }, this)
          )
        : (this.settings.center
            ? (this.settings.rtl && ((e = -1), (f = b + 1)),
              (c = this._coordinates[b]),
              (c += ((this.width() - c + (this._coordinates[f] || 0)) / 2) * e))
            : (c = this._coordinates[f] || 0),
          (c = Math.ceil(c)));
    }),
    (e.prototype.duration = function (a, b, c) {
      return 0 === c
        ? 0
        : Math.min(Math.max(Math.abs(b - a), 1), 6) *
            Math.abs(c || this.settings.smartSpeed);
    }),
    (e.prototype.to = function (a, b) {
      var c = this.current(),
        d = null,
        e = a - this.relative(c),
        f = (e > 0) - (e < 0),
        g = this._items.length,
        h = this.minimum(),
        i = this.maximum();
      this.settings.loop
        ? (!this.settings.rewind && Math.abs(e) > g / 2 && (e += f * -1 * g),
          (a = c + e),
          (d = ((((a - h) % g) + g) % g) + h),
          d !== a &&
            d - e <= i &&
            d - e > 0 &&
            ((c = d - e), (a = d), this.reset(c)))
        : this.settings.rewind
        ? ((i += 1), (a = ((a % i) + i) % i))
        : (a = Math.max(h, Math.min(i, a))),
        this.speed(this.duration(c, a, b)),
        this.current(a),
        this.$element.is(":visible") && this.update();
    }),
    (e.prototype.next = function (a) {
      (a = a || !1), this.to(this.relative(this.current()) + 1, a);
    }),
    (e.prototype.prev = function (a) {
      (a = a || !1), this.to(this.relative(this.current()) - 1, a);
    }),
    (e.prototype.onTransitionEnd = function (a) {
      if (
        a !== d &&
        (a.stopPropagation(),
        (a.target || a.srcElement || a.originalTarget) !== this.$stage.get(0))
      )
        return !1;
      this.leave("animating"), this.trigger("translated");
    }),
    (e.prototype.viewport = function () {
      var d;
      return (
        this.options.responsiveBaseElement !== b
          ? (d = a(this.options.responsiveBaseElement).width())
          : b.innerWidth
          ? (d = b.innerWidth)
          : c.documentElement && c.documentElement.clientWidth
          ? (d = c.documentElement.clientWidth)
          : console.warn("Can not detect viewport width."),
        d
      );
    }),
    (e.prototype.replace = function (b) {
      this.$stage.empty(),
        (this._items = []),
        b && (b = b instanceof jQuery ? b : a(b)),
        this.settings.nestedItemSelector &&
          (b = b.find("." + this.settings.nestedItemSelector)),
        b
          .filter(function () {
            return 1 === this.nodeType;
          })
          .each(
            a.proxy(function (a, b) {
              (b = this.prepare(b)),
                this.$stage.append(b),
                this._items.push(b),
                this._mergers.push(
                  1 *
                    b
                      .find("[data-merge]")
                      .addBack("[data-merge]")
                      .attr("data-merge") || 1
                );
            }, this)
          ),
        this.reset(
          this.isNumeric(this.settings.startPosition)
            ? this.settings.startPosition
            : 0
        ),
        this.invalidate("items");
    }),
    (e.prototype.add = function (b, c) {
      var e = this.relative(this._current);
      (c = c === d ? this._items.length : this.normalize(c, !0)),
        (b = b instanceof jQuery ? b : a(b)),
        this.trigger("add", { content: b, position: c }),
        (b = this.prepare(b)),
        0 === this._items.length || c === this._items.length
          ? (0 === this._items.length && this.$stage.append(b),
            0 !== this._items.length && this._items[c - 1].after(b),
            this._items.push(b),
            this._mergers.push(
              1 *
                b
                  .find("[data-merge]")
                  .addBack("[data-merge]")
                  .attr("data-merge") || 1
            ))
          : (this._items[c].before(b),
            this._items.splice(c, 0, b),
            this._mergers.splice(
              c,
              0,
              1 *
                b
                  .find("[data-merge]")
                  .addBack("[data-merge]")
                  .attr("data-merge") || 1
            )),
        this._items[e] && this.reset(this._items[e].index()),
        this.invalidate("items"),
        this.trigger("added", { content: b, position: c });
    }),
    (e.prototype.remove = function (a) {
      (a = this.normalize(a, !0)),
        a !== d &&
          (this.trigger("remove", { content: this._items[a], position: a }),
          this._items[a].remove(),
          this._items.splice(a, 1),
          this._mergers.splice(a, 1),
          this.invalidate("items"),
          this.trigger("removed", { content: null, position: a }));
    }),
    (e.prototype.preloadAutoWidthImages = function (b) {
      b.each(
        a.proxy(function (b, c) {
          this.enter("pre-loading"),
            (c = a(c)),
            a(new Image())
              .one(
                "load",
                a.proxy(function (a) {
                  c.attr("src", a.target.src),
                    c.css("opacity", 1),
                    this.leave("pre-loading"),
                    !this.is("pre-loading") &&
                      !this.is("initializing") &&
                      this.refresh();
                }, this)
              )
              .attr(
                "src",
                c.attr("src") || c.attr("data-src") || c.attr("data-src-retina")
              );
        }, this)
      );
    }),
    (e.prototype.destroy = function () {
      this.$element.off(".owl.core"),
        this.$stage.off(".owl.core"),
        a(c).off(".owl.core"),
        this.settings.responsive !== !1 &&
          (b.clearTimeout(this.resizeTimer),
          this.off(b, "resize", this._handlers.onThrottledResize));
      for (var d in this._plugins) this._plugins[d].destroy();
      this.$stage.children(".cloned").remove(),
        this.$stage.unwrap(),
        this.$stage.children().contents().unwrap(),
        this.$stage.children().unwrap(),
        this.$element
          .removeClass(this.options.refreshClass)
          .removeClass(this.options.loadingClass)
          .removeClass(this.options.loadedClass)
          .removeClass(this.options.rtlClass)
          .removeClass(this.options.dragClass)
          .removeClass(this.options.grabClass)
          .attr(
            "class",
            this.$element
              .attr("class")
              .replace(
                new RegExp(this.options.responsiveClass + "-\\S+\\s", "g"),
                ""
              )
          )
          .removeData("owl.carousel");
    }),
    (e.prototype.op = function (a, b, c) {
      var d = this.settings.rtl;
      switch (b) {
        case "<":
          return d ? a > c : a < c;
        case ">":
          return d ? a < c : a > c;
        case ">=":
          return d ? a <= c : a >= c;
        case "<=":
          return d ? a >= c : a <= c;
      }
    }),
    (e.prototype.on = function (a, b, c, d) {
      a.addEventListener
        ? a.addEventListener(b, c, d)
        : a.attachEvent && a.attachEvent("on" + b, c);
    }),
    (e.prototype.off = function (a, b, c, d) {
      a.removeEventListener
        ? a.removeEventListener(b, c, d)
        : a.detachEvent && a.detachEvent("on" + b, c);
    }),
    (e.prototype.trigger = function (b, c, d, f, g) {
      var h = { item: { count: this._items.length, index: this.current() } },
        i = a.camelCase(
          a
            .grep(["on", b, d], function (a) {
              return a;
            })
            .join("-")
            .toLowerCase()
        ),
        j = a.Event(
          [b, "owl", d || "carousel"].join(".").toLowerCase(),
          a.extend({ relatedTarget: this }, h, c)
        );
      return (
        this._supress[b] ||
          (a.each(this._plugins, function (a, b) {
            b.onTrigger && b.onTrigger(j);
          }),
          this.register({ type: e.Type.Event, name: b }),
          this.$element.trigger(j),
          this.settings &&
            "function" == typeof this.settings[i] &&
            this.settings[i].call(this, j)),
        j
      );
    }),
    (e.prototype.enter = function (b) {
      a.each(
        [b].concat(this._states.tags[b] || []),
        a.proxy(function (a, b) {
          this._states.current[b] === d && (this._states.current[b] = 0),
            this._states.current[b]++;
        }, this)
      );
    }),
    (e.prototype.leave = function (b) {
      a.each(
        [b].concat(this._states.tags[b] || []),
        a.proxy(function (a, b) {
          this._states.current[b]--;
        }, this)
      );
    }),
    (e.prototype.register = function (b) {
      if (b.type === e.Type.Event) {
        if (
          (a.event.special[b.name] || (a.event.special[b.name] = {}),
          !a.event.special[b.name].owl)
        ) {
          var c = a.event.special[b.name]._default;
          (a.event.special[b.name]._default = function (a) {
            return !c ||
              !c.apply ||
              (a.namespace && a.namespace.indexOf("owl") !== -1)
              ? a.namespace && a.namespace.indexOf("owl") > -1
              : c.apply(this, arguments);
          }),
            (a.event.special[b.name].owl = !0);
        }
      } else
        b.type === e.Type.State &&
          (this._states.tags[b.name]
            ? (this._states.tags[b.name] = this._states.tags[b.name].concat(
                b.tags
              ))
            : (this._states.tags[b.name] = b.tags),
          (this._states.tags[b.name] = a.grep(
            this._states.tags[b.name],
            a.proxy(function (c, d) {
              return a.inArray(c, this._states.tags[b.name]) === d;
            }, this)
          )));
    }),
    (e.prototype.suppress = function (b) {
      a.each(
        b,
        a.proxy(function (a, b) {
          this._supress[b] = !0;
        }, this)
      );
    }),
    (e.prototype.release = function (b) {
      a.each(
        b,
        a.proxy(function (a, b) {
          delete this._supress[b];
        }, this)
      );
    }),
    (e.prototype.pointer = function (a) {
      var c = { x: null, y: null };
      return (
        (a = a.originalEvent || a || b.event),
        (a =
          a.touches && a.touches.length
            ? a.touches[0]
            : a.changedTouches && a.changedTouches.length
            ? a.changedTouches[0]
            : a),
        a.pageX
          ? ((c.x = a.pageX), (c.y = a.pageY))
          : ((c.x = a.clientX), (c.y = a.clientY)),
        c
      );
    }),
    (e.prototype.isNumeric = function (a) {
      return !isNaN(parseFloat(a));
    }),
    (e.prototype.difference = function (a, b) {
      return { x: a.x - b.x, y: a.y - b.y };
    }),
    (a.fn.owlCarousel = function (b) {
      var c = Array.prototype.slice.call(arguments, 1);
      return this.each(function () {
        var d = a(this),
          f = d.data("owl.carousel");
        f ||
          ((f = new e(this, "object" == typeof b && b)),
          d.data("owl.carousel", f),
          a.each(
            [
              "next",
              "prev",
              "to",
              "destroy",
              "refresh",
              "replace",
              "add",
              "remove",
            ],
            function (b, c) {
              f.register({ type: e.Type.Event, name: c }),
                f.$element.on(
                  c + ".owl.carousel.core",
                  a.proxy(function (a) {
                    a.namespace &&
                      a.relatedTarget !== this &&
                      (this.suppress([c]),
                      f[c].apply(this, [].slice.call(arguments, 1)),
                      this.release([c]));
                  }, f)
                );
            }
          )),
          "string" == typeof b && "_" !== b.charAt(0) && f[b].apply(f, c);
      });
    }),
    (a.fn.owlCarousel.Constructor = e);
})(window.Zepto || window.jQuery, window, document),
  (function (a, b, c, d) {
    var e = function (b) {
      (this._core = b),
        (this._interval = null),
        (this._visible = null),
        (this._handlers = {
          "initialized.owl.carousel": a.proxy(function (a) {
            a.namespace && this._core.settings.autoRefresh && this.watch();
          }, this),
        }),
        (this._core.options = a.extend({}, e.Defaults, this._core.options)),
        this._core.$element.on(this._handlers);
    };
    (e.Defaults = { autoRefresh: !0, autoRefreshInterval: 500 }),
      (e.prototype.watch = function () {
        this._interval ||
          ((this._visible = this._core.$element.is(":visible")),
          (this._interval = b.setInterval(
            a.proxy(this.refresh, this),
            this._core.settings.autoRefreshInterval
          )));
      }),
      (e.prototype.refresh = function () {
        this._core.$element.is(":visible") !== this._visible &&
          ((this._visible = !this._visible),
          this._core.$element.toggleClass("owl-hidden", !this._visible),
          this._visible &&
            this._core.invalidate("width") &&
            this._core.refresh());
      }),
      (e.prototype.destroy = function () {
        var a, c;
        b.clearInterval(this._interval);
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (c in Object.getOwnPropertyNames(this))
          "function" != typeof this[c] && (this[c] = null);
      }),
      (a.fn.owlCarousel.Constructor.Plugins.AutoRefresh = e);
  })(window.Zepto || window.jQuery, window, document),
  (function (a, b, c, d) {
    var e = function (b) {
      (this._core = b),
        (this._loaded = []),
        (this._handlers = {
          "initialized.owl.carousel change.owl.carousel resized.owl.carousel":
            a.proxy(function (b) {
              if (
                b.namespace &&
                this._core.settings &&
                this._core.settings.lazyLoad &&
                ((b.property && "position" == b.property.name) ||
                  "initialized" == b.type)
              )
                for (
                  var c = this._core.settings,
                    e = (c.center && Math.ceil(c.items / 2)) || c.items,
                    f = (c.center && e * -1) || 0,
                    g =
                      (b.property && b.property.value !== d
                        ? b.property.value
                        : this._core.current()) + f,
                    h = this._core.clones().length,
                    i = a.proxy(function (a, b) {
                      this.load(b);
                    }, this);
                  f++ < e;

                )
                  this.load(h / 2 + this._core.relative(g)),
                    h && a.each(this._core.clones(this._core.relative(g)), i),
                    g++;
            }, this),
        }),
        (this._core.options = a.extend({}, e.Defaults, this._core.options)),
        this._core.$element.on(this._handlers);
    };
    (e.Defaults = { lazyLoad: !1 }),
      (e.prototype.load = function (c) {
        var d = this._core.$stage.children().eq(c),
          e = d && d.find(".owl-lazy");
        !e ||
          a.inArray(d.get(0), this._loaded) > -1 ||
          (e.each(
            a.proxy(function (c, d) {
              var e,
                f = a(d),
                g =
                  (b.devicePixelRatio > 1 && f.attr("data-src-retina")) ||
                  f.attr("data-src");
              this._core.trigger("load", { element: f, url: g }, "lazy"),
                f.is("img")
                  ? f
                      .one(
                        "load.owl.lazy",
                        a.proxy(function () {
                          f.css("opacity", 1),
                            this._core.trigger(
                              "loaded",
                              { element: f, url: g },
                              "lazy"
                            );
                        }, this)
                      )
                      .attr("src", g)
                  : ((e = new Image()),
                    (e.onload = a.proxy(function () {
                      f.css({
                        "background-image": 'url("' + g + '")',
                        opacity: "1",
                      }),
                        this._core.trigger(
                          "loaded",
                          { element: f, url: g },
                          "lazy"
                        );
                    }, this)),
                    (e.src = g));
            }, this)
          ),
          this._loaded.push(d.get(0)));
      }),
      (e.prototype.destroy = function () {
        var a, b;
        for (a in this.handlers) this._core.$element.off(a, this.handlers[a]);
        for (b in Object.getOwnPropertyNames(this))
          "function" != typeof this[b] && (this[b] = null);
      }),
      (a.fn.owlCarousel.Constructor.Plugins.Lazy = e);
  })(window.Zepto || window.jQuery, window, document),
  (function (a, b, c, d) {
    var e = function (b) {
      (this._core = b),
        (this._handlers = {
          "initialized.owl.carousel refreshed.owl.carousel": a.proxy(function (
            a
          ) {
            a.namespace && this._core.settings.autoHeight && this.update();
          },
          this),
          "changed.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              this._core.settings.autoHeight &&
              "position" == a.property.name &&
              this.update();
          }, this),
          "loaded.owl.lazy": a.proxy(function (a) {
            a.namespace &&
              this._core.settings.autoHeight &&
              a.element.closest("." + this._core.settings.itemClass).index() ===
                this._core.current() &&
              this.update();
          }, this),
        }),
        (this._core.options = a.extend({}, e.Defaults, this._core.options)),
        this._core.$element.on(this._handlers);
    };
    (e.Defaults = { autoHeight: !1, autoHeightClass: "owl-height" }),
      (e.prototype.update = function () {
        var b = this._core._current,
          c = b + this._core.settings.items,
          d = this._core.$stage.children().toArray().slice(b, c),
          e = [],
          f = 0;
        a.each(d, function (b, c) {
          e.push(a(c).height());
        }),
          (f = Math.max.apply(null, e)),
          this._core.$stage
            .parent()
            .height(f)
            .addClass(this._core.settings.autoHeightClass);
      }),
      (e.prototype.destroy = function () {
        var a, b;
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (b in Object.getOwnPropertyNames(this))
          "function" != typeof this[b] && (this[b] = null);
      }),
      (a.fn.owlCarousel.Constructor.Plugins.AutoHeight = e);
  })(window.Zepto || window.jQuery, window, document),
  (function (a, b, c, d) {
    var e = function (b) {
      (this._core = b),
        (this._videos = {}),
        (this._playing = null),
        (this._handlers = {
          "initialized.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              this._core.register({
                type: "state",
                name: "playing",
                tags: ["interacting"],
              });
          }, this),
          "resize.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              this._core.settings.video &&
              this.isInFullScreen() &&
              a.preventDefault();
          }, this),
          "refreshed.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              this._core.is("resizing") &&
              this._core.$stage.find(".cloned .owl-video-frame").remove();
          }, this),
          "changed.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              "position" === a.property.name &&
              this._playing &&
              this.stop();
          }, this),
          "prepared.owl.carousel": a.proxy(function (b) {
            if (b.namespace) {
              var c = a(b.content).find(".owl-video");
              c.length &&
                (c.css("display", "none"), this.fetch(c, a(b.content)));
            }
          }, this),
        }),
        (this._core.options = a.extend({}, e.Defaults, this._core.options)),
        this._core.$element.on(this._handlers),
        this._core.$element.on(
          "click.owl.video",
          ".owl-video-play-icon",
          a.proxy(function (a) {
            this.play(a);
          }, this)
        );
    };
    (e.Defaults = { video: !1, videoHeight: !1, videoWidth: !1 }),
      (e.prototype.fetch = function (a, b) {
        var c = (function () {
            return a.attr("data-vimeo-id")
              ? "vimeo"
              : a.attr("data-vzaar-id")
              ? "vzaar"
              : "youtube";
          })(),
          d =
            a.attr("data-vimeo-id") ||
            a.attr("data-youtube-id") ||
            a.attr("data-vzaar-id"),
          e = a.attr("data-width") || this._core.settings.videoWidth,
          f = a.attr("data-height") || this._core.settings.videoHeight,
          g = a.attr("href");
        if (!g) throw new Error("Missing video URL.");
        if (
          ((d = g.match(
            /(http:|https:|)\/\/(player.|www.|app.)?(vimeo\.com|youtu(be\.com|\.be|be\.googleapis\.com)|vzaar\.com)\/(video\/|videos\/|embed\/|channels\/.+\/|groups\/.+\/|watch\?v=|v\/)?([A-Za-z0-9._%-]*)(\&\S+)?/
          )),
          d[3].indexOf("youtu") > -1)
        )
          c = "youtube";
        else if (d[3].indexOf("vimeo") > -1) c = "vimeo";
        else {
          if (!(d[3].indexOf("vzaar") > -1))
            throw new Error("Video URL not supported.");
          c = "vzaar";
        }
        (d = d[6]),
          (this._videos[g] = { type: c, id: d, width: e, height: f }),
          b.attr("data-video", g),
          this.thumbnail(a, this._videos[g]);
      }),
      (e.prototype.thumbnail = function (b, c) {
        var d,
          e,
          f,
          g =
            c.width && c.height
              ? 'style="width:' + c.width + "px;height:" + c.height + 'px;"'
              : "",
          h = b.find("img"),
          i = "src",
          j = "",
          k = this._core.settings,
          l = function (a) {
            (e = '<div class="owl-video-play-icon"></div>'),
              (d = k.lazyLoad
                ? '<div class="owl-video-tn ' +
                  j +
                  '" ' +
                  i +
                  '="' +
                  a +
                  '"></div>'
                : '<div class="owl-video-tn" style="opacity:1;background-image:url(' +
                  a +
                  ')"></div>'),
              b.after(d),
              b.after(e);
          };
        if (
          (b.wrap('<div class="owl-video-wrapper"' + g + "></div>"),
          this._core.settings.lazyLoad && ((i = "data-src"), (j = "owl-lazy")),
          h.length)
        )
          return l(h.attr(i)), h.remove(), !1;
        "youtube" === c.type
          ? ((f = "//img.youtube.com/vi/" + c.id + "/hqdefault.jpg"), l(f))
          : "vimeo" === c.type
          ? a.ajax({
              type: "GET",
              url: "//vimeo.com/api/v2/video/" + c.id + ".json",
              jsonp: "callback",
              dataType: "jsonp",
              success: function (a) {
                (f = a[0].thumbnail_large), l(f);
              },
            })
          : "vzaar" === c.type &&
            a.ajax({
              type: "GET",
              url: "//vzaar.com/api/videos/" + c.id + ".json",
              jsonp: "callback",
              dataType: "jsonp",
              success: function (a) {
                (f = a.framegrab_url), l(f);
              },
            });
      }),
      (e.prototype.stop = function () {
        this._core.trigger("stop", null, "video"),
          this._playing.find(".owl-video-frame").remove(),
          this._playing.removeClass("owl-video-playing"),
          (this._playing = null),
          this._core.leave("playing"),
          this._core.trigger("stopped", null, "video");
      }),
      (e.prototype.play = function (b) {
        var c,
          d = a(b.target),
          e = d.closest("." + this._core.settings.itemClass),
          f = this._videos[e.attr("data-video")],
          g = f.width || "100%",
          h = f.height || this._core.$stage.height();
        this._playing ||
          (this._core.enter("playing"),
          this._core.trigger("play", null, "video"),
          (e = this._core.items(this._core.relative(e.index()))),
          this._core.reset(e.index()),
          "youtube" === f.type
            ? (c =
                '<iframe width="' +
                g +
                '" height="' +
                h +
                '" src="//www.youtube.com/embed/' +
                f.id +
                "?autoplay=1&rel=0&v=" +
                f.id +
                '" frameborder="0" allowfullscreen></iframe>')
            : "vimeo" === f.type
            ? (c =
                '<iframe src="//player.vimeo.com/video/' +
                f.id +
                '?autoplay=1" width="' +
                g +
                '" height="' +
                h +
                '" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>')
            : "vzaar" === f.type &&
              (c =
                '<iframe frameborder="0"height="' +
                h +
                '"width="' +
                g +
                '" allowfullscreen mozallowfullscreen webkitAllowFullScreen src="//view.vzaar.com/' +
                f.id +
                '/player?autoplay=true"></iframe>'),
          a('<div class="owl-video-frame">' + c + "</div>").insertAfter(
            e.find(".owl-video")
          ),
          (this._playing = e.addClass("owl-video-playing")));
      }),
      (e.prototype.isInFullScreen = function () {
        var b =
          c.fullscreenElement ||
          c.mozFullScreenElement ||
          c.webkitFullscreenElement;
        return b && a(b).parent().hasClass("owl-video-frame");
      }),
      (e.prototype.destroy = function () {
        var a, b;
        this._core.$element.off("click.owl.video");
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (b in Object.getOwnPropertyNames(this))
          "function" != typeof this[b] && (this[b] = null);
      }),
      (a.fn.owlCarousel.Constructor.Plugins.Video = e);
  })(window.Zepto || window.jQuery, window, document),
  (function (a, b, c, d) {
    var e = function (b) {
      (this.core = b),
        (this.core.options = a.extend({}, e.Defaults, this.core.options)),
        (this.swapping = !0),
        (this.previous = d),
        (this.next = d),
        (this.handlers = {
          "change.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              "position" == a.property.name &&
              ((this.previous = this.core.current()),
              (this.next = a.property.value));
          }, this),
          "drag.owl.carousel dragged.owl.carousel translated.owl.carousel":
            a.proxy(function (a) {
              a.namespace && (this.swapping = "translated" == a.type);
            }, this),
          "translate.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              this.swapping &&
              (this.core.options.animateOut || this.core.options.animateIn) &&
              this.swap();
          }, this),
        }),
        this.core.$element.on(this.handlers);
    };
    (e.Defaults = { animateOut: !1, animateIn: !1 }),
      (e.prototype.swap = function () {
        if (
          1 === this.core.settings.items &&
          a.support.animation &&
          a.support.transition
        ) {
          this.core.speed(0);
          var b,
            c = a.proxy(this.clear, this),
            d = this.core.$stage.children().eq(this.previous),
            e = this.core.$stage.children().eq(this.next),
            f = this.core.settings.animateIn,
            g = this.core.settings.animateOut;
          this.core.current() !== this.previous &&
            (g &&
              ((b =
                this.core.coordinates(this.previous) -
                this.core.coordinates(this.next)),
              d
                .one(a.support.animation.end, c)
                .css({ left: b + "px" })
                .addClass("animated owl-animated-out")
                .addClass(g)),
            f &&
              e
                .one(a.support.animation.end, c)
                .addClass("animated owl-animated-in")
                .addClass(f));
        }
      }),
      (e.prototype.clear = function (b) {
        a(b.target)
          .css({ left: "" })
          .removeClass("animated owl-animated-out owl-animated-in")
          .removeClass(this.core.settings.animateIn)
          .removeClass(this.core.settings.animateOut),
          this.core.onTransitionEnd();
      }),
      (e.prototype.destroy = function () {
        var a, b;
        for (a in this.handlers) this.core.$element.off(a, this.handlers[a]);
        for (b in Object.getOwnPropertyNames(this))
          "function" != typeof this[b] && (this[b] = null);
      }),
      (a.fn.owlCarousel.Constructor.Plugins.Animate = e);
  })(window.Zepto || window.jQuery, window, document),
  (function (a, b, c, d) {
    var e = function (b) {
      (this._core = b),
        (this._timeout = null),
        (this._paused = !1),
        (this._handlers = {
          "changed.owl.carousel": a.proxy(function (a) {
            a.namespace && "settings" === a.property.name
              ? this._core.settings.autoplay
                ? this.play()
                : this.stop()
              : a.namespace &&
                "position" === a.property.name &&
                this._core.settings.autoplay &&
                this._setAutoPlayInterval();
          }, this),
          "initialized.owl.carousel": a.proxy(function (a) {
            a.namespace && this._core.settings.autoplay && this.play();
          }, this),
          "play.owl.autoplay": a.proxy(function (a, b, c) {
            a.namespace && this.play(b, c);
          }, this),
          "stop.owl.autoplay": a.proxy(function (a) {
            a.namespace && this.stop();
          }, this),
          "mouseover.owl.autoplay": a.proxy(function () {
            this._core.settings.autoplayHoverPause &&
              this._core.is("rotating") &&
              this.pause();
          }, this),
          "mouseleave.owl.autoplay": a.proxy(function () {
            this._core.settings.autoplayHoverPause &&
              this._core.is("rotating") &&
              this.play();
          }, this),
          "touchstart.owl.core": a.proxy(function () {
            this._core.settings.autoplayHoverPause &&
              this._core.is("rotating") &&
              this.pause();
          }, this),
          "touchend.owl.core": a.proxy(function () {
            this._core.settings.autoplayHoverPause && this.play();
          }, this),
        }),
        this._core.$element.on(this._handlers),
        (this._core.options = a.extend({}, e.Defaults, this._core.options));
    };
    (e.Defaults = {
      autoplay: !1,
      autoplayTimeout: 5e3,
      autoplayHoverPause: !1,
      autoplaySpeed: !1,
    }),
      (e.prototype.play = function (a, b) {
        (this._paused = !1),
          this._core.is("rotating") ||
            (this._core.enter("rotating"), this._setAutoPlayInterval());
      }),
      (e.prototype._getNextTimeout = function (d, e) {
        return (
          this._timeout && b.clearTimeout(this._timeout),
          b.setTimeout(
            a.proxy(function () {
              this._paused ||
                this._core.is("busy") ||
                this._core.is("interacting") ||
                c.hidden ||
                this._core.next(e || this._core.settings.autoplaySpeed);
            }, this),
            d || this._core.settings.autoplayTimeout
          )
        );
      }),
      (e.prototype._setAutoPlayInterval = function () {
        this._timeout = this._getNextTimeout();
      }),
      (e.prototype.stop = function () {
        this._core.is("rotating") &&
          (b.clearTimeout(this._timeout), this._core.leave("rotating"));
      }),
      (e.prototype.pause = function () {
        this._core.is("rotating") && (this._paused = !0);
      }),
      (e.prototype.destroy = function () {
        var a, b;
        this.stop();
        for (a in this._handlers) this._core.$element.off(a, this._handlers[a]);
        for (b in Object.getOwnPropertyNames(this))
          "function" != typeof this[b] && (this[b] = null);
      }),
      (a.fn.owlCarousel.Constructor.Plugins.autoplay = e);
  })(window.Zepto || window.jQuery, window, document),
  (function (a, b, c, d) {
    "use strict";
    var e = function (b) {
      (this._core = b),
        (this._initialized = !1),
        (this._pages = []),
        (this._controls = {}),
        (this._templates = []),
        (this.$element = this._core.$element),
        (this._overrides = {
          next: this._core.next,
          prev: this._core.prev,
          to: this._core.to,
        }),
        (this._handlers = {
          "prepared.owl.carousel": a.proxy(function (b) {
            b.namespace &&
              this._core.settings.dotsData &&
              this._templates.push(
                '<div class="' +
                  this._core.settings.dotClass +
                  '">' +
                  a(b.content)
                    .find("[data-dot]")
                    .addBack("[data-dot]")
                    .attr("data-dot") +
                  "</div>"
              );
          }, this),
          "added.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              this._core.settings.dotsData &&
              this._templates.splice(a.position, 0, this._templates.pop());
          }, this),
          "remove.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              this._core.settings.dotsData &&
              this._templates.splice(a.position, 1);
          }, this),
          "changed.owl.carousel": a.proxy(function (a) {
            a.namespace && "position" == a.property.name && this.draw();
          }, this),
          "initialized.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              !this._initialized &&
              (this._core.trigger("initialize", null, "navigation"),
              this.initialize(),
              this.update(),
              this.draw(),
              (this._initialized = !0),
              this._core.trigger("initialized", null, "navigation"));
          }, this),
          "refreshed.owl.carousel": a.proxy(function (a) {
            a.namespace &&
              this._initialized &&
              (this._core.trigger("refresh", null, "navigation"),
              this.update(),
              this.draw(),
              this._core.trigger("refreshed", null, "navigation"));
          }, this),
        }),
        (this._core.options = a.extend({}, e.Defaults, this._core.options)),
        this.$element.on(this._handlers);
    };
    (e.Defaults = {
      nav: !1,
      navText: ["prev", "next"],
      navSpeed: !1,
      navElement: "div",
      navContainer: !1,
      navContainerClass: "owl-nav",
      navClass: ["owl-prev", "owl-next"],
      slideBy: 1,
      dotClass: "owl-dot",
      dotsClass: "owl-dots",
      dots: !0,
      dotsEach: !1,
      dotsData: !1,
      dotsSpeed: !1,
      dotsContainer: !1,
    }),
      (e.prototype.initialize = function () {
        var b,
          c = this._core.settings;
        (this._controls.$relative = (
          c.navContainer
            ? a(c.navContainer)
            : a("<div>").addClass(c.navContainerClass).appendTo(this.$element)
        ).addClass("disabled")),
          (this._controls.$previous = a("<" + c.navElement + ">")
            .addClass(c.navClass[0])
            .html(c.navText[0])
            .prependTo(this._controls.$relative)
            .on(
              "click",
              a.proxy(function (a) {
                this.prev(c.navSpeed);
              }, this)
            )),
          (this._controls.$next = a("<" + c.navElement + ">")
            .addClass(c.navClass[1])
            .html(c.navText[1])
            .appendTo(this._controls.$relative)
            .on(
              "click",
              a.proxy(function (a) {
                this.next(c.navSpeed);
              }, this)
            )),
          c.dotsData ||
            (this._templates = [
              a("<div>")
                .addClass(c.dotClass)
                .append(a("<span>"))
                .prop("outerHTML"),
            ]),
          (this._controls.$absolute = (
            c.dotsContainer
              ? a(c.dotsContainer)
              : a("<div>").addClass(c.dotsClass).appendTo(this.$element)
          ).addClass("disabled")),
          this._controls.$absolute.on(
            "click",
            "div",
            a.proxy(function (b) {
              var d = a(b.target).parent().is(this._controls.$absolute)
                ? a(b.target).index()
                : a(b.target).parent().index();
              b.preventDefault(), this.to(d, c.dotsSpeed);
            }, this)
          );
        for (b in this._overrides) this._core[b] = a.proxy(this[b], this);
      }),
      (e.prototype.destroy = function () {
        var a, b, c, d;
        for (a in this._handlers) this.$element.off(a, this._handlers[a]);
        for (b in this._controls) this._controls[b].remove();
        for (d in this.overides) this._core[d] = this._overrides[d];
        for (c in Object.getOwnPropertyNames(this))
          "function" != typeof this[c] && (this[c] = null);
      }),
      (e.prototype.update = function () {
        var a,
          b,
          c,
          d = this._core.clones().length / 2,
          e = d + this._core.items().length,
          f = this._core.maximum(!0),
          g = this._core.settings,
          h = g.center || g.autoWidth || g.dotsData ? 1 : g.dotsEach || g.items;
        if (
          ("page" !== g.slideBy && (g.slideBy = Math.min(g.slideBy, g.items)),
          g.dots || "page" == g.slideBy)
        )
          for (this._pages = [], a = d, b = 0, c = 0; a < e; a++) {
            if (b >= h || 0 === b) {
              if (
                (this._pages.push({
                  start: Math.min(f, a - d),
                  end: a - d + h - 1,
                }),
                Math.min(f, a - d) === f)
              )
                break;
              (b = 0), ++c;
            }
            b += this._core.mergers(this._core.relative(a));
          }
      }),
      (e.prototype.draw = function () {
        var b,
          c = this._core.settings,
          d = this._core.items().length <= c.items,
          e = this._core.relative(this._core.current()),
          f = c.loop || c.rewind;
        this._controls.$relative.toggleClass("disabled", !c.nav || d),
          c.nav &&
            (this._controls.$previous.toggleClass(
              "disabled",
              !f && e <= this._core.minimum(!0)
            ),
            this._controls.$next.toggleClass(
              "disabled",
              !f && e >= this._core.maximum(!0)
            )),
          this._controls.$absolute.toggleClass("disabled", !c.dots || d),
          c.dots &&
            ((b =
              this._pages.length - this._controls.$absolute.children().length),
            c.dotsData && 0 !== b
              ? this._controls.$absolute.html(this._templates.join(""))
              : b > 0
              ? this._controls.$absolute.append(
                  new Array(b + 1).join(this._templates[0])
                )
              : b < 0 && this._controls.$absolute.children().slice(b).remove(),
            this._controls.$absolute.find(".active").removeClass("active"),
            this._controls.$absolute
              .children()
              .eq(a.inArray(this.current(), this._pages))
              .addClass("active"));
      }),
      (e.prototype.onTrigger = function (b) {
        var c = this._core.settings;
        b.page = {
          index: a.inArray(this.current(), this._pages),
          count: this._pages.length,
          size:
            c &&
            (c.center || c.autoWidth || c.dotsData ? 1 : c.dotsEach || c.items),
        };
      }),
      (e.prototype.current = function () {
        var b = this._core.relative(this._core.current());
        return a
          .grep(
            this._pages,
            a.proxy(function (a, c) {
              return a.start <= b && a.end >= b;
            }, this)
          )
          .pop();
      }),
      (e.prototype.getPosition = function (b) {
        var c,
          d,
          e = this._core.settings;
        return (
          "page" == e.slideBy
            ? ((c = a.inArray(this.current(), this._pages)),
              (d = this._pages.length),
              b ? ++c : --c,
              (c = this._pages[((c % d) + d) % d].start))
            : ((c = this._core.relative(this._core.current())),
              (d = this._core.items().length),
              b ? (c += e.slideBy) : (c -= e.slideBy)),
          c
        );
      }),
      (e.prototype.next = function (b) {
        a.proxy(this._overrides.to, this._core)(this.getPosition(!0), b);
      }),
      (e.prototype.prev = function (b) {
        a.proxy(this._overrides.to, this._core)(this.getPosition(!1), b);
      }),
      (e.prototype.to = function (b, c, d) {
        var e;
        !d && this._pages.length
          ? ((e = this._pages.length),
            a.proxy(this._overrides.to, this._core)(
              this._pages[((b % e) + e) % e].start,
              c
            ))
          : a.proxy(this._overrides.to, this._core)(b, c);
      }),
      (a.fn.owlCarousel.Constructor.Plugins.Navigation = e);
  })(window.Zepto || window.jQuery, window, document),
  (function (a, b, c, d) {
    "use strict";
    var e = function (c) {
      (this._core = c),
        (this._hashes = {}),
        (this.$element = this._core.$element),
        (this._handlers = {
          "initialized.owl.carousel": a.proxy(function (c) {
            c.namespace &&
              "URLHash" === this._core.settings.startPosition &&
              a(b).trigger("hashchange.owl.navigation");
          }, this),
          "prepared.owl.carousel": a.proxy(function (b) {
            if (b.namespace) {
              var c = a(b.content)
                .find("[data-hash]")
                .addBack("[data-hash]")
                .attr("data-hash");
              if (!c) return;
              this._hashes[c] = b.content;
            }
          }, this),
          "changed.owl.carousel": a.proxy(function (c) {
            if (c.namespace && "position" === c.property.name) {
              var d = this._core.items(
                  this._core.relative(this._core.current())
                ),
                e = a
                  .map(this._hashes, function (a, b) {
                    return a === d ? b : null;
                  })
                  .join();
              if (!e || b.location.hash.slice(1) === e) return;
              b.location.hash = e;
            }
          }, this),
        }),
        (this._core.options = a.extend({}, e.Defaults, this._core.options)),
        this.$element.on(this._handlers),
        a(b).on(
          "hashchange.owl.navigation",
          a.proxy(function (a) {
            var c = b.location.hash.substring(1),
              e = this._core.$stage.children(),
              f = this._hashes[c] && e.index(this._hashes[c]);
            f !== d &&
              f !== this._core.current() &&
              this._core.to(this._core.relative(f), !1, !0);
          }, this)
        );
    };
    (e.Defaults = { URLhashListener: !1 }),
      (e.prototype.destroy = function () {
        var c, d;
        a(b).off("hashchange.owl.navigation");
        for (c in this._handlers) this._core.$element.off(c, this._handlers[c]);
        for (d in Object.getOwnPropertyNames(this))
          "function" != typeof this[d] && (this[d] = null);
      }),
      (a.fn.owlCarousel.Constructor.Plugins.Hash = e);
  })(window.Zepto || window.jQuery, window, document),
  (function (a, b, c, d) {
    function e(b, c) {
      var e = !1,
        f = b.charAt(0).toUpperCase() + b.slice(1);
      return (
        a.each((b + " " + h.join(f + " ") + f).split(" "), function (a, b) {
          if (g[b] !== d) return (e = !c || b), !1;
        }),
        e
      );
    }
    function f(a) {
      return e(a, !0);
    }
    var g = a("<support>").get(0).style,
      h = "Webkit Moz O ms".split(" "),
      i = {
        transition: {
          end: {
            WebkitTransition: "webkitTransitionEnd",
            MozTransition: "transitionend",
            OTransition: "oTransitionEnd",
            transition: "transitionend",
          },
        },
        animation: {
          end: {
            WebkitAnimation: "webkitAnimationEnd",
            MozAnimation: "animationend",
            OAnimation: "oAnimationEnd",
            animation: "animationend",
          },
        },
      },
      j = {
        csstransforms: function () {
          return !!e("transform");
        },
        csstransforms3d: function () {
          return !!e("perspective");
        },
        csstransitions: function () {
          return !!e("transition");
        },
        cssanimations: function () {
          return !!e("animation");
        },
      };
    j.csstransitions() &&
      ((a.support.transition = new String(f("transition"))),
      (a.support.transition.end = i.transition.end[a.support.transition])),
      j.cssanimations() &&
        ((a.support.animation = new String(f("animation"))),
        (a.support.animation.end = i.animation.end[a.support.animation])),
      j.csstransforms() &&
        ((a.support.transform = new String(f("transform"))),
        (a.support.transform3d = j.csstransforms3d()));
  })(window.Zepto || window.jQuery, window, document);

/*! Swipebox v1.4.4 | Constantin Saguin csag.co | MIT License | github.com/brutaldesign/swipebox */
!(function (a, b, c, d) {
  (c.swipebox = function (e, f) {
    var g,
      h,
      i = {
        useCSS: !0,
        useSVG: !0,
        initialIndexOnArray: 0,
        removeBarsOnMobile: !0,
        hideCloseButtonOnMobile: !1,
        hideBarsDelay: 3e3,
        videoMaxWidth: 1140,
        vimeoColor: "cccccc",
        beforeOpen: null,
        afterOpen: null,
        afterClose: null,
        afterMedia: null,
        nextSlide: null,
        prevSlide: null,
        loopAtEnd: !1,
        autoplayVideos: !1,
        queryStringData: {},
        toggleClassOnLoad: "",
        selector: null,
      },
      j = this,
      k = [],
      l = navigator.userAgent.match(
        /(iPad)|(iPhone)|(iPod)|(Android)|(PlayBook)|(BB10)|(BlackBerry)|(Opera Mini)|(IEMobile)|(webOS)|(MeeGo)/i
      ),
      m =
        null !== l ||
        b.createTouch !== d ||
        "ontouchstart" in a ||
        "onmsgesturechange" in a ||
        navigator.msMaxTouchPoints,
      n =
        !!b.createElementNS &&
        !!b.createElementNS("http://www.w3.org/2000/svg", "svg").createSVGRect,
      o = a.innerWidth ? a.innerWidth : c(a).width(),
      p = a.innerHeight ? a.innerHeight : c(a).height(),
      q = 0;
    (j.settings = {}),
      (c.swipebox.close = function () {
        g.closeSlide();
      }),
      (c.swipebox.extend = function () {
        return g;
      }),
      (j.init = function () {
        (j.settings = c.extend({}, i, f)),
          c.isArray(e)
            ? ((k = e),
              (g.target = c(a)),
              g.init(j.settings.initialIndexOnArray))
            : c(e).on("click", j.settings.selector, function (a) {
                if ("slide current" === a.target.parentNode.className)
                  return !1;
                g.destroy(),
                  (h =
                    null === j.settings.selector
                      ? c(e)
                      : c(e).find(j.settings.selector)),
                  (k = []);
                var b, d, f;
                f || ((d = "data-rel"), (f = c(this).attr(d))),
                  f || ((d = "rel"), (f = c(this).attr(d))),
                  f &&
                    "" !== f &&
                    "nofollow" !== f &&
                    (h = h.filter("[" + d + '="' + f + '"]')),
                  h.each(function () {
                    var a = null,
                      b = null;
                    c(this).attr("title") && (a = c(this).attr("title")),
                      c(this).attr("href") && (b = c(this).attr("href")),
                      k.push({ href: b, title: a });
                  }),
                  (b = h.index(c(this))),
                  a.preventDefault(),
                  a.stopPropagation(),
                  (g.target = c(a.target)),
                  g.init(b);
              });
      }),
      (g = {
        init: function (a) {
          j.settings.beforeOpen && j.settings.beforeOpen(),
            this.target.trigger("swipebox-start"),
            (c.swipebox.isOpen = !0),
            this.build(),
            this.openSlide(a),
            this.openMedia(a),
            this.preloadMedia(a + 1),
            this.preloadMedia(a - 1),
            j.settings.afterOpen && j.settings.afterOpen(a);
        },
        build: function () {
          var a,
            b = this;
          c("body").append(
            '<div id="swipebox-overlay">\t\t\t\t\t<div id="swipebox-container">\t\t\t\t\t\t<div id="swipebox-slider"></div>\t\t\t\t\t\t<div id="swipebox-top-bar">\t\t\t\t\t\t\t<div id="swipebox-title"></div>\t\t\t\t\t\t</div>\t\t\t\t\t\t<div id="swipebox-bottom-bar">\t\t\t\t\t\t\t<div id="swipebox-arrows">\t\t\t\t\t\t\t\t<a id="swipebox-prev"></a>\t\t\t\t\t\t\t\t<a id="swipebox-next"></a>\t\t\t\t\t\t\t</div>\t\t\t\t\t\t</div>\t\t\t\t\t\t<a id="swipebox-close"></a>\t\t\t\t\t</div>\t\t\t</div>'
          ),
            n &&
              !0 === j.settings.useSVG &&
              ((a = c("#swipebox-close").css("background-image")),
              (a = a.replace("png", "svg")),
              c("#swipebox-prev, #swipebox-next, #swipebox-close").css({
                "background-image": a,
              })),
            l &&
              j.settings.removeBarsOnMobile &&
              c("#swipebox-bottom-bar, #swipebox-top-bar").remove(),
            c.each(k, function () {
              c("#swipebox-slider").append('<div class="slide"></div>');
            }),
            b.setDim(),
            b.actions(),
            m && b.gesture(),
            b.keyboard(),
            b.animBars(),
            b.resize();
        },
        setDim: function () {
          var b,
            d,
            e = {};
          "onorientationchange" in a
            ? a.addEventListener(
                "orientationchange",
                function () {
                  0 === a.orientation
                    ? ((b = o), (d = p))
                    : (90 !== a.orientation && -90 !== a.orientation) ||
                      ((b = p), (d = o));
                },
                !1
              )
            : ((b = a.innerWidth ? a.innerWidth : c(a).width()),
              (d = a.innerHeight ? a.innerHeight : c(a).height())),
            (e = { width: b, height: d }),
            c("#swipebox-overlay").css(e);
        },
        resize: function () {
          var b = this;
          c(a)
            .resize(function () {
              b.setDim();
            })
            .resize();
        },
        supportTransition: function () {
          var a,
            c =
              "transition WebkitTransition MozTransition OTransition msTransition KhtmlTransition".split(
                " "
              );
          for (a = 0; a < c.length; a++)
            if (b.createElement("div").style[c[a]] !== d) return c[a];
          return !1;
        },
        doCssTrans: function () {
          if (j.settings.useCSS && this.supportTransition()) return !0;
        },
        gesture: function () {
          var a,
            b,
            d,
            e,
            f,
            g,
            h = this,
            i = !1,
            j = !1,
            l = 10,
            m = 50,
            n = {},
            p = {},
            r = c("#swipebox-top-bar, #swipebox-bottom-bar"),
            s = c("#swipebox-slider");
          r.addClass("visible-bars"),
            h.setTimeout(),
            c("body")
              .bind("touchstart", function (h) {
                return (
                  c(this).addClass("touching"),
                  (a = c("#swipebox-slider .slide").index(
                    c("#swipebox-slider .slide.current")
                  )),
                  (p = h.originalEvent.targetTouches[0]),
                  (n.pageX = h.originalEvent.targetTouches[0].pageX),
                  (n.pageY = h.originalEvent.targetTouches[0].pageY),
                  c("#swipebox-slider").css({
                    "-webkit-transform": "translate3d(" + q + "%, 0, 0)",
                    transform: "translate3d(" + q + "%, 0, 0)",
                  }),
                  c(".touching").bind("touchmove", function (h) {
                    if (
                      (h.preventDefault(),
                      h.stopPropagation(),
                      (p = h.originalEvent.targetTouches[0]),
                      !j &&
                        ((f = d),
                        (d = p.pageY - n.pageY),
                        Math.abs(d) >= m || i))
                    ) {
                      var r = 0.75 - Math.abs(d) / s.height();
                      s.css({ top: d + "px" }), s.css({ opacity: r }), (i = !0);
                    }
                    (e = b),
                      (b = p.pageX - n.pageX),
                      (g = (100 * b) / o),
                      !j &&
                        !i &&
                        Math.abs(b) >= l &&
                        (c("#swipebox-slider").css({
                          "-webkit-transition": "",
                          transition: "",
                        }),
                        (j = !0)),
                      j &&
                        (0 < b
                          ? 0 === a
                            ? c("#swipebox-overlay").addClass("leftSpringTouch")
                            : (c("#swipebox-overlay")
                                .removeClass("leftSpringTouch")
                                .removeClass("rightSpringTouch"),
                              c("#swipebox-slider").css({
                                "-webkit-transform":
                                  "translate3d(" + (q + g) + "%, 0, 0)",
                                transform:
                                  "translate3d(" + (q + g) + "%, 0, 0)",
                              }))
                          : 0 > b &&
                            (k.length === a + 1
                              ? c("#swipebox-overlay").addClass(
                                  "rightSpringTouch"
                                )
                              : (c("#swipebox-overlay")
                                  .removeClass("leftSpringTouch")
                                  .removeClass("rightSpringTouch"),
                                c("#swipebox-slider").css({
                                  "-webkit-transform":
                                    "translate3d(" + (q + g) + "%, 0, 0)",
                                  transform:
                                    "translate3d(" + (q + g) + "%, 0, 0)",
                                }))));
                  }),
                  !1
                );
              })
              .bind("touchend", function (a) {
                if (
                  (a.preventDefault(),
                  a.stopPropagation(),
                  c("#swipebox-slider").css({
                    "-webkit-transition": "-webkit-transform 0.4s ease",
                    transition: "transform 0.4s ease",
                  }),
                  (d = p.pageY - n.pageY),
                  (b = p.pageX - n.pageX),
                  (g = (100 * b) / o),
                  i)
                )
                  if (
                    ((i = !1),
                    Math.abs(d) >= 2 * m && Math.abs(d) > Math.abs(f))
                  ) {
                    var k = d > 0 ? s.height() : -s.height();
                    s.animate({ top: k + "px", opacity: 0 }, 300, function () {
                      h.closeSlide();
                    });
                  } else s.animate({ top: 0, opacity: 1 }, 300);
                else
                  j
                    ? ((j = !1),
                      b >= l && b >= e
                        ? h.getPrev()
                        : b <= -l && b <= e && h.getNext())
                    : r.hasClass("visible-bars")
                    ? (h.clearTimeout(), h.hideBars())
                    : (h.showBars(), h.setTimeout());
                c("#swipebox-slider").css({
                  "-webkit-transform": "translate3d(" + q + "%, 0, 0)",
                  transform: "translate3d(" + q + "%, 0, 0)",
                }),
                  c("#swipebox-overlay")
                    .removeClass("leftSpringTouch")
                    .removeClass("rightSpringTouch"),
                  c(".touching").off("touchmove").removeClass("touching");
              });
        },
        setTimeout: function () {
          if (j.settings.hideBarsDelay > 0) {
            var b = this;
            b.clearTimeout(),
              (b.timeout = a.setTimeout(function () {
                b.hideBars();
              }, j.settings.hideBarsDelay));
          }
        },
        clearTimeout: function () {
          a.clearTimeout(this.timeout), (this.timeout = null);
        },
        showBars: function () {
          var a = c("#swipebox-top-bar, #swipebox-bottom-bar");
          this.doCssTrans()
            ? a.addClass("visible-bars")
            : (c("#swipebox-top-bar").animate({ top: 0 }, 500),
              c("#swipebox-bottom-bar").animate({ bottom: 0 }, 500),
              setTimeout(function () {
                a.addClass("visible-bars");
              }, 1e3));
        },
        hideBars: function () {
          var a = c("#swipebox-top-bar, #swipebox-bottom-bar");
          this.doCssTrans()
            ? a.removeClass("visible-bars")
            : (c("#swipebox-top-bar").animate({ top: "-50px" }, 500),
              c("#swipebox-bottom-bar").animate({ bottom: "-50px" }, 500),
              setTimeout(function () {
                a.removeClass("visible-bars");
              }, 1e3));
        },
        animBars: function () {
          var a = this,
            b = c("#swipebox-top-bar, #swipebox-bottom-bar");
          b.addClass("visible-bars"),
            a.setTimeout(),
            c("#swipebox-slider").click(function () {
              b.hasClass("visible-bars") || (a.showBars(), a.setTimeout());
            }),
            c("#swipebox-bottom-bar").hover(
              function () {
                a.showBars(), b.addClass("visible-bars"), a.clearTimeout();
              },
              function () {
                j.settings.hideBarsDelay > 0 &&
                  (b.removeClass("visible-bars"), a.setTimeout());
              }
            );
        },
        keyboard: function () {
          var b = this;
          c(a).bind("keyup", function (a) {
            a.preventDefault(),
              a.stopPropagation(),
              37 === a.keyCode
                ? b.getPrev()
                : 39 === a.keyCode
                ? b.getNext()
                : 27 === a.keyCode && b.closeSlide();
          });
        },
        actions: function () {
          var a = this,
            b = "touchend click";
          k.length < 2
            ? (c("#swipebox-bottom-bar").hide(),
              d === k[1] && c("#swipebox-top-bar").hide())
            : (c("#swipebox-prev").bind(b, function (b) {
                b.preventDefault(),
                  b.stopPropagation(),
                  a.getPrev(),
                  a.setTimeout();
              }),
              c("#swipebox-next").bind(b, function (b) {
                b.preventDefault(),
                  b.stopPropagation(),
                  a.getNext(),
                  a.setTimeout();
              })),
            c("#swipebox-close").bind(b, function () {
              a.closeSlide();
            });
        },
        setSlide: function (a, b) {
          b = b || !1;
          var d = c("#swipebox-slider");
          (q = 100 * -a),
            this.doCssTrans()
              ? d.css({
                  "-webkit-transform": "translate3d(" + 100 * -a + "%, 0, 0)",
                  transform: "translate3d(" + 100 * -a + "%, 0, 0)",
                })
              : d.animate({ left: 100 * -a + "%" }),
            c("#swipebox-slider .slide").removeClass("current"),
            c("#swipebox-slider .slide").eq(a).addClass("current"),
            this.setTitle(a),
            b && d.fadeIn(),
            c("#swipebox-prev, #swipebox-next").removeClass("disabled"),
            0 === a
              ? c("#swipebox-prev").addClass("disabled")
              : a === k.length - 1 &&
                !0 !== j.settings.loopAtEnd &&
                c("#swipebox-next").addClass("disabled");
        },
        openSlide: function (b) {
          c("html").addClass("swipebox-html"),
            m
              ? (c("html").addClass("swipebox-touch"),
                j.settings.hideCloseButtonOnMobile &&
                  c("html").addClass("swipebox-no-close-button"))
              : c("html").addClass("swipebox-no-touch"),
            c(a).trigger("resize"),
            this.setSlide(b, !0);
        },
        preloadMedia: function (a) {
          var b = this,
            c = null;
          k[a] !== d && (c = k[a].href),
            b.isVideo(c)
              ? b.openMedia(a)
              : setTimeout(function () {
                  b.openMedia(a);
                }, 1e3);
        },
        openMedia: function (a) {
          var b,
            e,
            f = this;
          if ((k[a] !== d && (b = k[a].href), a < 0 || a >= k.length))
            return !1;
          (e = c("#swipebox-slider .slide").eq(a)),
            f.isVideo(b)
              ? (e.html(f.getVideo(b)),
                j.settings.afterMedia && j.settings.afterMedia(a))
              : (e.addClass("slide-loading"),
                f.loadMedia(b, function () {
                  e.removeClass("slide-loading"),
                    e.html(this),
                    j.settings.afterMedia && j.settings.afterMedia(a);
                }));
        },
        setTitle: function (a) {
          var b = null;
          c("#swipebox-title").empty(),
            k[a] !== d && (b = k[a].title),
            b
              ? (c("#swipebox-top-bar").show(), c("#swipebox-title").append(b))
              : c("#swipebox-top-bar").hide();
        },
        isVideo: function (a) {
          if (a) {
            if (
              a.match(
                /(youtube\.com|youtube-nocookie\.com)\/watch\?v=([a-zA-Z0-9\-_]+)/
              ) ||
              a.match(/vimeo\.com\/([0-9]*)/) ||
              a.match(/youtu\.be\/([a-zA-Z0-9\-_]+)/)
            )
              return !0;
            if (a.toLowerCase().indexOf("swipeboxvideo=1") >= 0) return !0;
          }
        },
        parseUri: function (a, d) {
          var e = b.createElement("a"),
            f = {};
          return (
            (e.href = decodeURIComponent(a)),
            e.search &&
              (f = JSON.parse(
                '{"' +
                  e.search
                    .toLowerCase()
                    .replace("?", "")
                    .replace(/&/g, '","')
                    .replace(/=/g, '":"') +
                  '"}'
              )),
            c.isPlainObject(d) &&
              (f = c.extend(f, d, j.settings.queryStringData)),
            c
              .map(f, function (a, b) {
                if (a && a > "")
                  return encodeURIComponent(b) + "=" + encodeURIComponent(a);
              })
              .join("&")
          );
        },
        getVideo: function (a) {
          var b = "",
            c = a.match(
              /((?:www\.)?youtube\.com|(?:www\.)?youtube-nocookie\.com)\/watch\?v=([a-zA-Z0-9\-_]+)/
            ),
            d = a.match(/(?:www\.)?youtu\.be\/([a-zA-Z0-9\-_]+)/),
            e = a.match(/(?:www\.)?vimeo\.com\/([0-9]*)/),
            f = "";
          return (
            c || d
              ? (d && (c = d),
                (f = g.parseUri(a, {
                  autoplay: j.settings.autoplayVideos ? "1" : "0",
                  v: "",
                })),
                (b =
                  '<iframe width="560" height="315" src="//' +
                  c[1] +
                  "/embed/" +
                  c[2] +
                  "?" +
                  f +
                  '" frameborder="0" allowfullscreen></iframe>'))
              : e
              ? ((f = g.parseUri(a, {
                  autoplay: j.settings.autoplayVideos ? "1" : "0",
                  byline: "0",
                  portrait: "0",
                  color: j.settings.vimeoColor,
                })),
                (b =
                  '<iframe width="560" height="315"  src="//player.vimeo.com/video/' +
                  e[1] +
                  "?" +
                  f +
                  '" frameborder="0" webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe>'))
              : (b =
                  '<iframe width="560" height="315" src="' +
                  a +
                  '" frameborder="0" allowfullscreen></iframe>'),
            '<div class="swipebox-video-container" style="max-width:' +
              j.settings.videoMaxWidth +
              'px"><div class="swipebox-video">' +
              b +
              "</div></div>"
          );
        },
        loadMedia: function (a, b) {
          if (0 === a.trim().indexOf("#"))
            b.call(
              c("<div>", { class: "swipebox-inline-container" }).append(
                c(a).clone().toggleClass(j.settings.toggleClassOnLoad)
              )
            );
          else if (!this.isVideo(a)) {
            var d = c("<img>").on("load", function () {
              b.call(d);
            });
            d.attr("src", a);
          }
        },
        getNext: function () {
          var a,
            b = this,
            d = c("#swipebox-slider .slide").index(
              c("#swipebox-slider .slide.current")
            );
          d + 1 < k.length
            ? ((a = c("#swipebox-slider .slide")
                .eq(d)
                .contents()
                .find("iframe")
                .attr("src")),
              c("#swipebox-slider .slide")
                .eq(d)
                .contents()
                .find("iframe")
                .attr("src", a),
              d++,
              b.setSlide(d),
              b.preloadMedia(d + 1),
              j.settings.nextSlide && j.settings.nextSlide(d))
            : !0 === j.settings.loopAtEnd
            ? ((a = c("#swipebox-slider .slide")
                .eq(d)
                .contents()
                .find("iframe")
                .attr("src")),
              c("#swipebox-slider .slide")
                .eq(d)
                .contents()
                .find("iframe")
                .attr("src", a),
              (d = 0),
              b.preloadMedia(d),
              b.setSlide(d),
              b.preloadMedia(d + 1),
              j.settings.nextSlide && j.settings.nextSlide(d))
            : (c("#swipebox-overlay").addClass("rightSpring"),
              setTimeout(function () {
                c("#swipebox-overlay").removeClass("rightSpring");
              }, 500));
        },
        getPrev: function () {
          var a,
            b = c("#swipebox-slider .slide").index(
              c("#swipebox-slider .slide.current")
            );
          b > 0
            ? ((a = c("#swipebox-slider .slide")
                .eq(b)
                .contents()
                .find("iframe")
                .attr("src")),
              c("#swipebox-slider .slide")
                .eq(b)
                .contents()
                .find("iframe")
                .attr("src", a),
              b--,
              this.setSlide(b),
              this.preloadMedia(b - 1),
              j.settings.prevSlide && j.settings.prevSlide(b))
            : (c("#swipebox-overlay").addClass("leftSpring"),
              setTimeout(function () {
                c("#swipebox-overlay").removeClass("leftSpring");
              }, 500));
        },
        nextSlide: function (a) {},
        prevSlide: function (a) {},
        closeSlide: function () {
          c("html").removeClass("swipebox-html"),
            c("html").removeClass("swipebox-touch"),
            c(a).trigger("resize"),
            this.destroy();
        },
        destroy: function () {
          c(a).unbind("keyup"),
            c("body").unbind("touchstart"),
            c("body").unbind("touchmove"),
            c("body").unbind("touchend"),
            c("#swipebox-slider").unbind(),
            c("#swipebox-overlay").remove(),
            c.isArray(e) || e.removeData("_swipebox"),
            this.target && this.target.trigger("swipebox-destroy"),
            (c.swipebox.isOpen = !1),
            j.settings.afterClose && j.settings.afterClose();
        },
      }),
      j.init();
  }),
    (c.fn.swipebox = function (a) {
      if (!c.data(this, "_swipebox")) {
        var b = new c.swipebox(this, a);
        this.data("_swipebox", b);
      }
      return this.data("_swipebox");
    });
})(window, document, jQuery);

/* ============================================================
 * flatui-radiocheck v0.1.0
 * ============================================================ */

+(function (global, $) {
  "use strict";

  var Radiocheck = function (element, options) {
    this.init("radiocheck", element, options);
  };

  Radiocheck.DEFAULTS = {
    checkboxClass: "b-checkbox--custom",
    radioClass: "b-radio--custom",
    checkboxTemplate:
      '<span class="b-icons"><span class="b-icon--unchecked"></span><span class="b-icon--checked"></span></span>',
    radioTemplate:
      '<span class="b-icons"><span class="b-icon--unchecked"></span><span class="b-icon--checked"></span></span>',
  };

  Radiocheck.prototype.init = function (type, element, options) {
    this.$element = $(element);
    this.options = $.extend(
      {},
      Radiocheck.DEFAULTS,
      this.$element.data(),
      options
    );
    if (this.$element.attr("type") == "checkbox") {
      this.$element.addClass(this.options.checkboxClass);
      this.$element.after(this.options.checkboxTemplate);
    } else if (this.$element.attr("type") == "radio") {
      this.$element.addClass(this.options.radioClass);
      this.$element.after(this.options.radioTemplate);
    }
  };

  (Radiocheck.prototype.check = function () {
    this.$element.prop("checked", true);
    this.$element.trigger("change.radiocheck").trigger("checked.radiocheck");
  }),
    (Radiocheck.prototype.uncheck = function () {
      this.$element.prop("checked", false);
      this.$element
        .trigger("change.radiocheck")
        .trigger("unchecked.radiocheck");
    }),
    (Radiocheck.prototype.toggle = function () {
      this.$element.prop("checked", function (i, value) {
        return !value;
      });
      this.$element.trigger("change.radiocheck").trigger("toggled.radiocheck");
    }),
    (Radiocheck.prototype.indeterminate = function () {
      this.$element.prop("indeterminate", true);
      this.$element
        .trigger("change.radiocheck")
        .trigger("indeterminated.radiocheck");
    }),
    (Radiocheck.prototype.determinate = function () {
      this.$element.prop("indeterminate", false);
      this.$element
        .trigger("change.radiocheck")
        .trigger("determinated.radiocheck");
    }),
    (Radiocheck.prototype.disable = function () {
      this.$element.prop("disabled", true);
      this.$element.trigger("change.radiocheck").trigger("disabled.radiocheck");
    }),
    (Radiocheck.prototype.enable = function () {
      this.$element.prop("disabled", false);
      this.$element.trigger("change.radiocheck").trigger("enabled.radiocheck");
    }),
    (Radiocheck.prototype.destroy = function () {
      this.$element
        .removeData()
        .removeClass(this.options.checkboxClass + " " + this.options.radioClass)
        .next(".b-icons")
        .remove();
      this.$element.trigger("destroyed.radiocheck");
    });

  // RADIOCHECK PLUGIN DEFINITION
  // ============================

  function Plugin(option) {
    return this.each(function () {
      var $this = $(this);
      var data = $this.data("radiocheck");
      var options = typeof option == "object" && option;

      if (!data && option == "destroy") {
        return;
      }
      if (!data) {
        $this.data("radiocheck", (data = new Radiocheck(this, options)));
      }
      if (typeof option == "string") {
        data[option]();
      }

      // Adding 'nohover' class for mobile devices

      var mobile = /mobile|tablet|phone|ip(ad|od)|android|silk|webos/i.test(
        global.navigator.userAgent
      );

      if (mobile === true) {
        $this.parent().hover(
          function () {
            $this.addClass("b-nohover");
          },
          function () {
            $this.removeClass("b-nohover");
          }
        );
      }
    });
  }

  var old = $.fn.radiocheck;

  $.fn.radiocheck = Plugin;
  $.fn.radiocheck.Constructor = Radiocheck;

  // RADIOCHECK NO CONFLICT
  // ======================

  $.fn.radiocheck.noConflict = function () {
    $.fn.radiocheck = old;
    return this;
  };
})(this, jQuery);

/*
    jQuery Masked Input Plugin
    Copyright (c) 2007 - 2015 Josh Bush (digitalbush.com)
    Licensed under the MIT license (http://digitalbush.com/projects/masked-input-plugin/#license)
    Version: 1.4.1
*/
!(function (a) {
  "function" == typeof define && define.amd
    ? define(["jquery"], a)
    : a("object" == typeof exports ? require("jquery") : jQuery);
})(function (a) {
  var b,
    c = navigator.userAgent,
    d = /iphone/i.test(c),
    e = /chrome/i.test(c),
    f = /android/i.test(c);
  (a.mask = {
    definitions: { 9: "[0-9]", a: "[A-Za-z]", "*": "[A-Za-z0-9]" },
    autoclear: !0,
    dataName: "rawMaskFn",
    placeholder: "_",
  }),
    a.fn.extend({
      caret: function (a, b) {
        var c;
        if (0 !== this.length && !this.is(":hidden"))
          return "number" == typeof a
            ? ((b = "number" == typeof b ? b : a),
              this.each(function () {
                this.setSelectionRange
                  ? this.setSelectionRange(a, b)
                  : this.createTextRange &&
                    ((c = this.createTextRange()),
                    c.collapse(!0),
                    c.moveEnd("character", b),
                    c.moveStart("character", a),
                    c.select());
              }))
            : (this[0].setSelectionRange
                ? ((a = this[0].selectionStart), (b = this[0].selectionEnd))
                : document.selection &&
                  document.selection.createRange &&
                  ((c = document.selection.createRange()),
                  (a = 0 - c.duplicate().moveStart("character", -1e5)),
                  (b = a + c.text.length)),
              { begin: a, end: b });
      },
      unmask: function () {
        return this.trigger("unmask");
      },
      mask: function (c, g) {
        var h, i, j, k, l, m, n, o;
        if (!c && this.length > 0) {
          h = a(this[0]);
          var p = h.data(a.mask.dataName);
          return p ? p() : void 0;
        }
        return (
          (g = a.extend(
            {
              autoclear: a.mask.autoclear,
              placeholder: a.mask.placeholder,
              completed: null,
            },
            g
          )),
          (i = a.mask.definitions),
          (j = []),
          (k = n = c.length),
          (l = null),
          a.each(c.split(""), function (a, b) {
            "?" == b
              ? (n--, (k = a))
              : i[b]
              ? (j.push(new RegExp(i[b])),
                null === l && (l = j.length - 1),
                k > a && (m = j.length - 1))
              : j.push(null);
          }),
          this.trigger("unmask").each(function () {
            function h() {
              if (g.completed) {
                for (var a = l; m >= a; a++) if (j[a] && C[a] === p(a)) return;
                g.completed.call(B);
              }
            }
            function p(a) {
              return g.placeholder.charAt(a < g.placeholder.length ? a : 0);
            }
            function q(a) {
              for (; ++a < n && !j[a]; );
              return a;
            }
            function r(a) {
              for (; --a >= 0 && !j[a]; );
              return a;
            }
            function s(a, b) {
              var c, d;
              if (!(0 > a)) {
                for (c = a, d = q(b); n > c; c++)
                  if (j[c]) {
                    if (!(n > d && j[c].test(C[d]))) break;
                    (C[c] = C[d]), (C[d] = p(d)), (d = q(d));
                  }
                z(), B.caret(Math.max(l, a));
              }
            }
            function t(a) {
              var b, c, d, e;
              for (b = a, c = p(a); n > b; b++)
                if (j[b]) {
                  if (
                    ((d = q(b)),
                    (e = C[b]),
                    (C[b] = c),
                    !(n > d && j[d].test(e)))
                  )
                    break;
                  c = e;
                }
            }
            function u() {
              var a = B.val(),
                b = B.caret();
              if (o && o.length && o.length > a.length) {
                for (A(!0); b.begin > 0 && !j[b.begin - 1]; ) b.begin--;
                if (0 === b.begin)
                  for (; b.begin < l && !j[b.begin]; ) b.begin++;
                B.caret(b.begin, b.begin);
              } else {
                for (A(!0); b.begin < n && !j[b.begin]; ) b.begin++;
                B.caret(b.begin, b.begin);
              }
              h();
            }
            function v() {
              A(), B.val() != E && B.change();
            }
            function w(a) {
              if (!B.prop("readonly")) {
                var b,
                  c,
                  e,
                  f = a.which || a.keyCode;
                (o = B.val()),
                  8 === f || 46 === f || (d && 127 === f)
                    ? ((b = B.caret()),
                      (c = b.begin),
                      (e = b.end),
                      e - c === 0 &&
                        ((c = 46 !== f ? r(c) : (e = q(c - 1))),
                        (e = 46 === f ? q(e) : e)),
                      y(c, e),
                      s(c, e - 1),
                      a.preventDefault())
                    : 13 === f
                    ? v.call(this, a)
                    : 27 === f &&
                      (B.val(E), B.caret(0, A()), a.preventDefault());
              }
            }
            function x(b) {
              if (!B.prop("readonly")) {
                var c,
                  d,
                  e,
                  g = b.which || b.keyCode,
                  i = B.caret();
                if (
                  !(b.ctrlKey || b.altKey || b.metaKey || 32 > g) &&
                  g &&
                  13 !== g
                ) {
                  if (
                    (i.end - i.begin !== 0 &&
                      (y(i.begin, i.end), s(i.begin, i.end - 1)),
                    (c = q(i.begin - 1)),
                    n > c && ((d = String.fromCharCode(g)), j[c].test(d)))
                  ) {
                    if ((t(c), (C[c] = d), z(), (e = q(c)), f)) {
                      var k = function () {
                        a.proxy(a.fn.caret, B, e)();
                      };
                      setTimeout(k, 0);
                    } else B.caret(e);
                    i.begin <= m && h();
                  }
                  b.preventDefault();
                }
              }
            }
            function y(a, b) {
              var c;
              for (c = a; b > c && n > c; c++) j[c] && (C[c] = p(c));
            }
            function z() {
              B.val(C.join(""));
            }
            function A(a) {
              var b,
                c,
                d,
                e = B.val(),
                f = -1;
              for (b = 0, d = 0; n > b; b++)
                if (j[b]) {
                  for (C[b] = p(b); d++ < e.length; )
                    if (((c = e.charAt(d - 1)), j[b].test(c))) {
                      (C[b] = c), (f = b);
                      break;
                    }
                  if (d > e.length) {
                    y(b + 1, n);
                    break;
                  }
                } else C[b] === e.charAt(d) && d++, k > b && (f = b);
              return (
                a
                  ? z()
                  : k > f + 1
                  ? g.autoclear || C.join("") === D
                    ? (B.val() && B.val(""), y(0, n))
                    : z()
                  : (z(), B.val(B.val().substring(0, f + 1))),
                k ? b : l
              );
            }
            var B = a(this),
              C = a.map(c.split(""), function (a, b) {
                return "?" != a ? (i[a] ? p(b) : a) : void 0;
              }),
              D = C.join(""),
              E = B.val();
            B.data(a.mask.dataName, function () {
              return a
                .map(C, function (a, b) {
                  return j[b] && a != p(b) ? a : null;
                })
                .join("");
            }),
              B.one("unmask", function () {
                B.off(".mask").removeData(a.mask.dataName);
              })
                .on("focus.mask", function () {
                  if (!B.prop("readonly")) {
                    clearTimeout(b);
                    var a;
                    (E = B.val()),
                      (a = A()),
                      (b = setTimeout(function () {
                        B.get(0) === document.activeElement &&
                          (z(),
                          a == c.replace("?", "").length
                            ? B.caret(0, a)
                            : B.caret(a));
                      }, 10));
                  }
                })
                .on("blur.mask", v)
                .on("keydown.mask", w)
                .on("keypress.mask", x)
                .on("input.mask paste.mask", function () {
                  B.prop("readonly") ||
                    setTimeout(function () {
                      var a = A(!0);
                      B.caret(a), h();
                    }, 0);
                }),
              e && f && B.off("input.mask").on("input.mask", u),
              A();
          })
        );
      },
    });
});

/*! lozad.js - v1.16.0 - 2020-09-06
 * https://github.com/ApoorvSaxena/lozad.js
 * Copyright (c) 2020 Apoorv Saxena; Licensed MIT */
!(function (t, e) {
  "object" == typeof exports && "undefined" != typeof module
    ? (module.exports = e())
    : "function" == typeof define && define.amd
    ? define(e)
    : (t.lozad = e());
})(this, function () {
  "use strict";
  /**
   * Detect IE browser
   * @const {boolean}
   * @private
   */ var g = "undefined" != typeof document && document.documentMode,
    f = {
      rootMargin: "0px",
      threshold: 0,
      load: function (t) {
        if ("picture" === t.nodeName.toLowerCase()) {
          var e = t.querySelector("img"),
            r = !1;
          null === e && ((e = document.createElement("img")), (r = !0)),
            g &&
              t.getAttribute("data-iesrc") &&
              (e.src = t.getAttribute("data-iesrc")),
            t.getAttribute("data-alt") && (e.alt = t.getAttribute("data-alt")),
            r && t.append(e);
        }
        if (
          "video" === t.nodeName.toLowerCase() &&
          !t.getAttribute("data-src") &&
          t.children
        ) {
          for (var a = t.children, o = void 0, i = 0; i <= a.length - 1; i++)
            (o = a[i].getAttribute("data-src")) && (a[i].src = o);
          t.load();
        }
        t.getAttribute("data-poster") &&
          (t.poster = t.getAttribute("data-poster")),
          t.getAttribute("data-src") && (t.src = t.getAttribute("data-src")),
          t.getAttribute("data-srcset") &&
            t.setAttribute("srcset", t.getAttribute("data-srcset"));
        var n = ",";
        if (
          (t.getAttribute("data-background-delimiter") &&
            (n = t.getAttribute("data-background-delimiter")),
          t.getAttribute("data-background-image"))
        )
          t.style.backgroundImage =
            "url('" +
            t.getAttribute("data-background-image").split(n).join("'),url('") +
            "')";
        else if (t.getAttribute("data-background-image-set")) {
          var d = t.getAttribute("data-background-image-set").split(n),
            u = d[0].substr(0, d[0].indexOf(" ")) || d[0]; // Substring before ... 1x
          (u = -1 === u.indexOf("url(") ? "url(" + u + ")" : u),
            1 === d.length
              ? (t.style.backgroundImage = u)
              : t.setAttribute(
                  "style",
                  (t.getAttribute("style") || "") +
                    "background-image: " +
                    u +
                    "; background-image: -webkit-image-set(" +
                    d +
                    "); background-image: image-set(" +
                    d +
                    ")"
                );
        }
        t.getAttribute("data-toggle-class") &&
          t.classList.toggle(t.getAttribute("data-toggle-class"));
      },
      loaded: function () {},
    };
  function A(t) {
    t.setAttribute("data-loaded", !0);
  }
  var m = function (t) {
      return "true" === t.getAttribute("data-loaded");
    },
    v = function (t) {
      var e =
        1 < arguments.length && void 0 !== arguments[1]
          ? arguments[1]
          : document;
      return t instanceof Element
        ? [t]
        : t instanceof NodeList
        ? t
        : e.querySelectorAll(t);
    };
  return function () {
    var r,
      a,
      o =
        0 < arguments.length && void 0 !== arguments[0]
          ? arguments[0]
          : ".lozad",
      t = 1 < arguments.length && void 0 !== arguments[1] ? arguments[1] : {},
      e = Object.assign({}, f, t),
      i = e.root,
      n = e.rootMargin,
      d = e.threshold,
      u = e.load,
      g = e.loaded,
      s = void 0;
    "undefined" != typeof window &&
      window.IntersectionObserver &&
      (s = new IntersectionObserver(
        ((r = u),
        (a = g),
        function (t, e) {
          t.forEach(function (t) {
            (0 < t.intersectionRatio || t.isIntersecting) &&
              (e.unobserve(t.target),
              m(t.target) || (r(t.target), A(t.target), a(t.target)));
          });
        }),
        { root: i, rootMargin: n, threshold: d }
      ));
    for (var c, l = v(o, i), b = 0; b < l.length; b++)
      (c = l[b]).getAttribute("data-placeholder-background") &&
        (c.style.background = c.getAttribute("data-placeholder-background"));
    return {
      observe: function () {
        for (var t = v(o, i), e = 0; e < t.length; e++)
          m(t[e]) || (s ? s.observe(t[e]) : (u(t[e]), A(t[e]), g(t[e])));
      },
      triggerLoad: function (t) {
        m(t) || (u(t), A(t), g(t));
      },
      observer: s,
    };
  };
});
