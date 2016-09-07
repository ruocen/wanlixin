$(function() {
    $("#nextstep").click(function () {
        commitAccountInfo();
    });
    //界面相关输入校验
    $("input[name=accountname]").keyup(function () {
        $(this).removeClass("verify-error");
    });
    $("input[name=cardno]").keyup(function () {
        $(this).removeClass("verify-error");
    });
});

function commitAccountInfo() {
    if (accountValidation()) {
        $.ajax({
            url: '/sale/commitAccountInfo/',
            type: 'POST',
            data: {
                name: $("input[name=accountname]").val(),
                cardno: $("input[name=cardno]").val()
            },
            success: function(res) {
                if (res == "success") {
                    window.location = "/sale/withdraw";
                }
            }
        });
    }
}

function accountValidation() {
    var name = $("input[name=accountname]").val();
    var cardNo = $("input[name=cardno]").val();
    var result = true;
    var resultstr = "";
    if (name == "") {
        $("input[name=accountname]").addClass("verify-error");
        resultstr += "用户姓名不能为空，请填写完整\n";
        result = false;
    }
    //if (cardNo == "") {
    //    $("input[name=cardno]").addClass("verify-error");
    //    resultstr += "身份证号不能为空，请填写完整\n";
    //    result = false;
    //}else if (cardNo.length != 18) {
    //    $("input[name=cardno]").addClass("verify-error");
    //    resultstr += "身份证号不正确，请填写完整";
    //    result = false;
    //}
    if (!IdentityCodeValid(cardNo)) {
        $("input[name=cardno]").addClass("verify-error");
        resultstr += "请输入正确的身份证号";
        result = false;
    }
    if (!result) {
        alertMsg(resultstr);
    }
    return result;
}