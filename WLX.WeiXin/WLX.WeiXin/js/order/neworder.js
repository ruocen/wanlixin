$(function() {
    //地址提交验证相关代码
    $("form .Place_sel input").keyup(function () {
        $(this).removeClass("verify-error");
    });
    $("form .Place_sel select").change(function () {
        $(this).removeClass("verify-error");
    });
    $("form .Place_sel textarea").keyup(function () {
        $(this).removeClass("verify-error");
    });
    $('#chooseWXPay').click(function () {
	    if ('undefined' != typeof(requiredIDCard) && requiredIDCard && $('#idcard').val() == '') { //该条件来源于create.cshtml中关于客户身份的html
		    alertMsg('请填写身份证号');
		    return;
	    }
	    //自助商品禁止改价，目前处理方式
        var b = true;
        var skucode = "";
        var skulist = "";
        $('.Place_sel li').each(function () {
            skucode = $(this).find('#StoreProductSkuCode').val();
            skulist += skucode + "|";
        });

        var codelist = skulist.split('|');
        $(codelist).each(function (index, item) {
            if (item.trim() == "QT9999999999" && codelist.length > 2) {
                b = false;
                return alertMsg("自助下单补差价链接 不能和其他商品同时拍哦!");
            }
        });
        if (!b) {
            return;
        }
        var orderid = $("#orderId").val();
        if (orderid !== undefined && orderid !== "") {
            payorder(orderid);
        } else {
            var data = $('form').serializeArray();
            console.log(data);
            if (!validate()) return;
            //$(this).removeAttr('href');//去掉a标签中的href属性
            //$(this).removeAttr('onclick');//设置按钮不可点击，防止重复提交数据
            $.ajax({
                url: '/order/placeorder',
                type: 'POST',
                data: $.param(data),
                success: function (res) {
                    if (res.status == "true") {
                        $("#orderId").val(res.msg);
                        payorder(res.msg);
                    } else if (res.msg.indexOf("库存不足")) {
                        $("#divupdatephone").show();
                    }
                    else
                    {
                        alert(res.msg);
                    }
                }
            });
        }
    });

    //异步获取支付openid
    //getpayopenid();
});

function selectAddress() {
    
}

wx.config({
    debug: true,// 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
    appId: $("#appId").val(),// 必填，公众号的唯一标识
    timestamp: $("#timestamp").val(),// 必填，生成签名的时间戳
    nonceStr: $("#nonceStr").val(),// 必填，生成签名的随机串
    signature: $("#signature").val(),// 必填，签名，
    jsApiList: [
    'chooseWXPay'
    ]
});
wx.ready(function () {
    
   
});
function validate() {
    var obj = $("form").serializeObject();
    var result = true;
    var resultStr = "";
    if (obj.addressId == undefined || obj.addressId == "") {
        if (obj.ReceiverName == "" || obj.ReceiverName == undefined) {
            $("input[name=ReceiverName]").addClass("verify-error");
            resultStr += "姓名不能为空\n";
            result = false;
        }
        if (obj.Phone == "" || obj.Phone == undefined) {
            $("input[name=Phone]").addClass("verify-error");
            resultStr += "手机号码不能为空\n";
            result = false;
        } else if (!obj.Phone.match(/^(((1[1-9][0-9]{1}))+\d{8})$/)) {
            $("input[name=Phone]").addClass("verify-error");
            resultStr += "手机号码不正确，请正确填写\n";
            result = false;
        }
        if (obj.Province == "" || obj.Province == "省份" || obj.Province == undefined) {
            $("#selProvince").addClass("verify-error");
            resultStr += "请选择您所在的省份\n";
            result = false;
        }
        if (obj.City == "" || obj.City == "城市" || obj.City == undefined) {
            $("#selCity").addClass("verify-error");
            resultStr += "请选择您所在的城市\n";
            result = false;
        }
        if (obj.Street == "" || obj.Street == undefined) {
            $("textarea[name=Street]").addClass("verify-error");
            resultStr += "详细地址不能为空";
            result = false;
        }
        if (!result) {
            alertMsg(resultStr);
        }
    }
    return result;
}
function payorder(orderid) {
    //            if (parseInt(@ViewBag.payInfo.userVersion) < 5) {
    //                alert("抱歉，您的微信版本不支持微信支付。");
    //                return;
    //            }
    
    // 注意：此 Demo 使用 2.7 版本支付接口实现，建议使用此接口时参考微信支付相关最新文档。
    var body = $($(".productName")[0]).html();
    var detail = $("#detail").val();
    var attach = $("#attach").val();
    //var product_id = $("input[name=storeSkuList[0].Key]").val();
    var product_id = 'product_id';
    var goods_tag = $("#goods_tag").val();
    var isTestPayment = $("#isTestPayment").val();
    var orderFee = isTestPayment === 'true' ? 0.01 : $("#totalFee").html();
    var openid = $("#openid").val();
    var orderid = orderid;
    $.ajax({
        url: '/wxpay/index',
        type: 'POST',
        data: {
            timestamp: "",
            body: body,
            detail: "",
            attach: "",
            total_fee: orderFee, //订单总金额，只能为整数
            trade_type: 'JSAPI',
            goods_tag: '', //商品标记，代金券或立减优惠功能的参数
            product_id: "", //trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义。
            orderid: orderid,
            openid: openid
        },
        success: function (res) {
            if (res == "")return;
            if (res.result_code == "SUCCESS") {
                var nonce_str = res.nonce_str;
                var timestamp = res.timestamp;
                var prepay_id = res.prepay_id;
                var paySign = res.sign;
                weixinPay(nonce_str, prepay_id, paySign, timestamp);
            } else {
                alertMsg(res.return_code);
                alertMsg(res.return_msg);
            }
        },
        error: function (res) {
            alertMsg("支付准备失败");
        }
    });
}
function weixinPay(nonce_str, prepay_id, paySign, timestamp) {
    //alert('timestamp: ' + timestamp + ',nonceStr: ' + nonce_str + ',prepay_id:' + prepay_id + ',paySign:' + paySign);
    wx.chooseWXPay({
        timestamp: timestamp,
        nonceStr: nonce_str,
        package: 'prepay_id=' + prepay_id,
        signType: 'MD5',
        paySign: paySign,
        success: function (res) {
            //alertMsg(res.errMsg);
            // 支付成功后的回调函数，详细请参见：http://pay.weixin.qq.com/wiki/doc/api/index.php?chapter=7_7
            if (res.errMsg == "chooseWXPay:ok") { // 使用以上方式判断前端返回,微信团队郑重提示：res.err_msg将在用户支付成功后返回    ok，但并不保证它绝对可靠。
                //TODO：此处为安全期间，应调用商户api查询订单状态。
                //$.ajax({
                //    url: '/wxpay/SuccessResponse',
                //    type: 'POST',
                //    data: {
                //    },
                //    success: function (res) {
                
                //    }
                //});

                //alert("支付成功");
                //setTimeout(function () {
                //    window.location.href = "/"; //这里默认跳转到主页
                //}, 2000);
                window.location = "/customer/orderlist";
            }
        },
        cancel: function (res) {
            if (res.errMsg == "chooseWXPay:cancel") //支付过程中用户取消
            {
                //TODO：支付过程中用户取消的商户处理逻辑。
                //alert("支付过程中用户取消");
                window.location = "/customer/orderlist";
            }
        },
        fail: function (res) {
            //alert(res);
            //alert(res.errMsg);
            if (res.errMsg == "chooseWXPay:fail") //支付失败
            {
                //TODO：支付失败的商户处理逻辑。
                alertMsg("支付失败");
                //$.ajax({
                //    url: '/wxpay/FailResponse',
                //    type: 'POST',
                //    data: {
                //    },
                //    success: function (res) {
                
                //    }
                //});
                window.location = "/customer/orderlist";
            }
        }
    });
}
//异步获取支付openid
function getpayopenid() {
    $.ajax({
        url: '/order/getpayopenid',
        type: 'post',
        data: {},
        success: function (res) {
            if (res.status == "true") {
                    $("#openid").val(data.msg);
                }
        },
        error: function (res) {
           
        }
    });
}