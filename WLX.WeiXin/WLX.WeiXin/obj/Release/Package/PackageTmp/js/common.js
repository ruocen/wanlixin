//正则实现列表Json数据和html模板绑定数据
String.prototype.temp = function (obj) {
    return this.replace(/\$\w+\$/gi, function (matchs) {
        var returns = obj[matchs.replace(/\$/g, "")];
        return (returns + "") == "undefined" ? "" : returns;
    });
};

//var currentUrl ;
//var lastUrl;
$(document).ready(function () {
//    lastUrl = currentUrl;
    //    currentUrl = window.location.href;

    //弹出框空白位置点击关闭弹出框
    $(document).on("tap", function (e) {
        var target = $(e.target);
        if (target.closest(".share_list").length == 0
         && target.closest(".app_page_loading").length == 1) {
            setTimeout('$(".app_page_loading").hide()', 400);
        }
    });
});

function pageBack() {
    window.history.back();
    //window.location = document.referrer;
}

var mpage = 9999, mrows = '' || 50, mcount = 1, isLoading;

function bindOntouchend(callback) {
    $('body').off('touchmove');
    $('body').on('touchmove', function () {
        var bodyHeight = $('body').height(),
            winHeight = $(window).height(),
            scrollTop = $(window).scrollTop();

        if (scrollTop >= (bodyHeight - winHeight - 45) && !isLoading) {
            isLoading = true;
            $("#mini_loading").css("display", "block");
            if (mcount < mpage) {
                mcount++;
            } else {
                $("#mini_loading").css("display", "none");
                $(".app_list_load_last").css("display", "block");
                return;
            }
            //$("#mini_loading").css("display", "block");
            callback && callback.call();
        }
    });
}
//判断滚动条是否拉制底部
function lazyload(selector, callback) {
    $(selector).append('<div>' +
           '<section class="app_list_loading" id="mini_loading" style="display:none;"></section>' +
           '<span class="app_list_load_last" style="display:none;">没有更多数据</span>' +
           '</div>');
    bindOntouchend(callback);
}

function resetLazyload(callback) {
    mcount = 1;
    bindOntouchend(callback);
}

function alertMsg(msg, title) {
    title = (typeof (title) == 'undefined' ? '提示' : title);
    $('').messager(msg, title);
}

//cookie操作方法
function setCookie(c_name, value, expiredays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + expiredays);
    document.cookie = c_name + "=" + escape(value) + ((expiredays == null) ? "" : ";expires=" + exdate.toGMTString());
}
function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=");
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) c_end = document.cookie.length;
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return "";
}


(function ($) {
    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };
    //扩展弹出框方法
    $.fn.messager = function (msg, title) {
        $("body div:first").append('<section class="app_page_loading">' +
            '<div class="share_list">' +
                '<p class="popup_span hint">' + title + '</p>' +
                '<p class="popup_info">' + msg + '</p>' +
                '<div class="btn_wrap">' +
                    '<a class="btn" href="javascript:void(0);" onclick="javascript:$(this).parent().parent().parent().remove();">确定</a>' +
                '</div>' +
            '</div>' +
            '</section>');
    };
})(Zepto);