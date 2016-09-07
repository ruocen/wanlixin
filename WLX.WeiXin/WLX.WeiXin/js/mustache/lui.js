(function () {
    window.TapEvent = 'click';
    var $$ = window.$$ = {};
    var DEBUG = false;

    $$.Template = {};
    // $$.Template.localStorage = localStorage;//本地存储存到本地全局中 

    $$.Template.init = function () {
        if (window.localStorage) {
            if (DEBUG == true) {
                window.localStorage.clear();
            }
            $$.Template.localStorage = localStorage;
        } else {
            $$.Template.localStorage = {};
        }
    }

    $$.Template.load = function (url, callback) {
        if ($$.Template.localStorage[url]) {//取加到全局中的模版
            callback.call(this,$$.Template.localStorage[url]);
        } else {
            $.ajax({
                url: url,
                type:'GET',
                cache: true,
                success: function (data) {
                    $$.Template.localStorage[url] = data;
                    callback.call(this,data);
                },
                error: function (XHR, TS, msg) {
                }
            });
        }
    }

    $$.Template.localrender = function ($elem, seletor, data, hasLogic, callback) {
        var html;
        if (typeof (data) === 'undefined') {
            data = '';
        }
        if (hasLogic) {
            html = Mustache.renderEx($(seletor).html(), data);
        } else {
            html = Mustache.render($(seletor).html(), data);
        }
        if (typeof ($elem) === "string") {

            $($elem).html(html);
        } else {

            $elem.html(html);
        }
        if (callback)
            callback.call(this);
        return html;
    }

    $$.Template.translate = function (seletor, data, hasLogic, callback) {
        var html;
        if (typeof (data) === 'undefined') {
            data = '';
        }
        if (hasLogic) {
            html = Mustache.renderEx($(seletor).html(), data)
        } else {
            html = Mustache.render($(seletor).html(), data);
        }

        if (callback)
            callback.call(this);
        return html;
    }

    $$.Template.render = function ($elem, url, data, hasLogic, callback) {
        var html;
        if (DEBUG == true) {
            $$.Template.init();
        }

        $$.Template.load(url, function () {
            if (typeof (data) === 'undefined') {
                data = '';
            }
            var options = {
                cache: true
            };
            try {
                if (hasLogic) {
                    html = Mustache.renderEx($$.Template.localStorage[url], data, options);
                } else {
                    html = Mustache.render($$.Template.localStorage[url], data, options);
                }
                if (typeof ($elem) == 'string')
                    $($elem).html(html);
                else {
                    $elem.html(html);
                }

                if (callback)
                    callback.call(this, true, null);
            } catch (e) {
                if (callback)
                    callback.call(this, false, e);
            }
        });
    }

    $$.Template.init();
})();

