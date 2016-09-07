$(function() {
    $("a[name=editBankCard]").click(function(obj) {
        var bindId = $(this).attr("bindid");
        editBind(bindId);
    });

    //左划出现删除按钮，右划隐藏
    $('.bankcard').find('li').each(function () {
        $(this).on('swipeLeft', function () {
            if (!$(this).hasClass('action_del')) {
                $(this).addClass('action_del');
                $(this).find(".chikr-nl-btn .iconfont").hide();
            }
        });
        $(this).on('swipeRight', function () {
            if ($(this).hasClass('action_del')) {
                $(this).removeClass('action_del');
                $(this).find(".chikr-nl-btn .iconfont").show();
            }
        });
    });

});
function editBind(id) {
    window.location = "/sale/BindDetail?accountBindId=" + id;
    
}

function deladdress(obj, id) {
    $.ajax({
        url: '/sale/DeleteAccountBind/' + id,
        type: 'POST',
        success: function (res) {
            if (res == "success") {
                $(obj).parent().remove();
            }
        }
    });
}