$(function () {
    $("#submitbtn").click(function () {
        bindAccountSubmit();
    });

    //绑定银行卡 选择是否为默认银行卡
    $(".chikr-news_all .chikr-news-list .btn_select").click(function () {
        if ($(this).hasClass("btn_selected")) {
            $(this).removeClass("btn_selected");
        } else {
            $(this).addClass("btn_selected");
        }
    });

});

function bindAccountSubmit() {
    var isDefault = $(".btn_select").hasClass("btn_selected");
    var bankName = $("select[name=bankname]").text();
    //var bankCardId = $("select[name=bankname]").val();
    //if (bankCardId === undefined) bankCardId = "";
    var code = $("input[name=code]").val();
    var accountBindId = $("input[name=accountBindId]").val();
    var hiddenBankCardID = $("input[name=hiddenBankCardID]").val();
    if (code == "") {
        alert("请输入支付宝账号");
        $("input[name=code]").focus();
        return false;
    }
    $.ajax({
        url: '/sale/EditBankCard/',
        type: 'POST',
        data: {
            IsDefault: isDefault,
            Code: code,
            //BankCardId: bankCardId,
            accountBindId: accountBindId,
            hiddenBankCardID: hiddenBankCardID
        },
        success: function (res) {
            if (res == "success") {
                window.location.href = '/sale/bindlist';
            }
        }
    });
}