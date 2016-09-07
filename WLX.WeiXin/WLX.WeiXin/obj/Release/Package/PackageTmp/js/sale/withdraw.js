$(function () {
    //$("#selectAccount").click(function () {
    //    var amount = $("input[name=amount]").val();
    //    window.location = "/sale/selectaccount?amount=" + amount;
    //});
});

function addWithdraw() {
    var amount = $("input[name=amount]").val();
    var regex = /^[0-9]+(.[0-9]{0,1}[0-9]{1})?$/;
    if (!regex.test(amount) || parseFloat(amount) <= 0.00) {
        alertMsg("请输入正确的金额");
        $("input[name=amount]").val("")
        $("input[name=amount]").focus();
        return;
    }
    //var accountBindId = $("input[name=accountBindId]").val();
    $.ajax({
        url: '/sale/addwithdraw/',
        type: 'POST',
        data: {
            amount: amount
            //accountBindId: accountBindId
        },
        success: function (res) {
            if (res == "success") {
                $('#divupdatephone').show();
            } else {
                alert(res);
            }
        }
    });
}