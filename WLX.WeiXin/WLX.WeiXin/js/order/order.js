$(function () {
    //售价合计
    $("#btnsaletotal").click(function () {

        //自助商品禁止改价，目前处理方式
        var skucode = "";
        $('.orderitemlist ul li input').each(function () {
            if ($(this).parent().find('#StoreProductSkuCode').val() == "QT9999999999")
                skucode = "QT9999999999";
        });
        if (skucode.trim() == "QT9999999999") {
            alertMsg("自助下单补差价链接 不能修改价格哦!");
            return false;
        }
        var saletotalprice = $("#saletotalprice").val();
        var LamaTotal = $("#LamaTotal").val();
        if (saletotalprice != "") {
            var regex = /^[0-9]+(.[0-9]{0,1}[1-9]{1})?$/;
            if (!regex.test(saletotalprice) || parseFloat(saletotalprice) <= 0.00 || parseFloat(saletotalprice) > 99999) {
                alertMsg("请输入正确的金额");
                $("#saletotalprice").val("")
                $("#saletotalprice").focus();
                return false;
            }
            if (parseFloat(saletotalprice) < parseFloat(LamaTotal)) {
                $("#divordertotal").show();
                return false;
            }
            var params = { 'do': 1 };
            params.OrderTotal = saletotalprice;
            params.OrderID = $("#OrderID").val();
            $.post("/Sale/updateordertotal", params, function (data) {
                if (data.status == "true") {
                    alertMsg(data.msg);
                    $("#ordertotal").html(saletotalprice);
                    var profitprice = parseFloat(params.OrderTotal) - parseFloat(LamaTotal);
                    $("#orderprofitprice").html(profitprice.toFixed(2));
                }
            }, "json");
        }
        else {
            $("#saletotalprice").focus();
            alertMsg("请输入有效的订单售价");
            return false;
        }
    });

    //订单详情--添加备注
    $("#btn_remark_save").click(function () {
        var remark = $("#remark").val();
        var orderID = $("#OrderID").val();      
        if ($("#remark").val() == "") {
            alertMsg("订单备注不能为空，请重新填写");
            return;
        }
        $.ajax({
            type: 'POST',
            url: '/Sale/UpdateOrderLamaRemark',
            dataType: "json",
            data: $.param({
                remark: remark,
                OrderID: orderID
            }),
            success: function (result) {
                if (result.status == "true") {
                    alertMsg(result.msg);
                }
            }
        });
    });

    $("#OrderAddress li a").click(function () {
       
        var orderid = $(this).attr("orderid");
        var addressid = $(this).attr("addressid");
        
        var isUpdata = IsUpdateOrderStatus(orderid);
        if (isUpdata && orderid != '' && addressid!='')
        {
            window.location.href = "/store/selectaddress?addressId=" + addressid + "&orderStr=" + orderid;
        }
    });

    $("#CustomerAddress li a").click(function () {

        var orderid = $(this).attr("orderid");
        var addressid = $(this).attr("addressid");
        var customerid = $(this).attr("customerid");

        var isUpdata = IsUpdateOrderStatus(orderid);
        if (isUpdata && orderid != '' && addressid != '' && customerid!='') {
            window.location.href = "/store/editcustomeraddress?addressId=" + addressid + "&customerid=" + customerid + "&orderStr=" + orderid;
        }
        if ($("#StatusName") == "待支付") {
            window.location.href = "/store/editcustomeraddress?addressId=" + addressid + "&customerid=" + customerid + "&orderStr=" + orderid;
        }
    });
    $('#modifyIDCard').unbind('click').bind('click', function () {
    	var orderid = $(this).attr("orderid");
    	if (IsUpdateOrderStatus2(orderid)) {
    		$('#IDCardModify').show();
	    }
    });
});

function IsUpdateOrderStatus(obj)
{
    var isUpdata = false;
    if (obj != '') {
        
        $.ajax({
            type: 'POST',
            url: '/Sale/UpdateOrderStatusID',
            data: { OrderID: obj },
            async: false,
            success: function (result) {
                if (result.msg == '0') {
                    isUpdata = true;
                }
                else {
                    $("#divplaceorder").show();
                }
            },
            error: function () {
                alert(result.msg);
            }
        });
    }
    return isUpdata;
}
function IsUpdateOrderStatus2(obj) {
	var isUpdata = false;
	if (obj != '') {

		$.ajax({
			type: 'POST',
			url: '/Sale/UpdateOrderStatusID',
			data: { OrderID: obj },
			async: false,
			success: function (result) {
				if (result.msg == '0') {
					isUpdata = true;
				}
				else {
					$("#divplaceorder2").show();
				}
			},
			error: function () {
				alert(result.msg);
			}
		});
	}
	return isUpdata;
}