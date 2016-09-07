$(function () {
    LoadAccountInfo();
    LoadSaleAmount();
    //    loadorderlist();
    lazyload(".chikr-wrapper", function () {
        loadorderlist();
    });
});
//本月销售额
function LoadSaleAmount() {
    $.ajax({
        url: "/sale/GetSaleAmountThisMonth",
        type: 'post',
        success: function (data) {
            if (data != undefined) {
                var saleAmount = data;
                $("#saleAmount").html(saleAmount);
            }
        },
        error: function (data) {
            //alert("本月销售额请求错误！");
        }
    });
    
}
//余额
function LoadAccountInfo() {
    $.ajax({
        url: "/sale/GetLamaAccount",
        type: 'post',
        success: function (data) {
            if (data != undefined) {
                var lamaAmount = data.Amount;
                $("#lamaAmount").html(lamaAmount.toFixed(2));
            }
        },
        error: function (data) {
            //alert("余额请求错误！");
        }
    });

}
//订单列表
function loadorderlist() {
    $.ajax({
        url: "/sale/SaleOrdersList",
        data: $.param({
            //OrderID: "",
            pageIndex: mcount,
            pageSize: mrows
        }),
        type: 'post',
        dataType: "html",
        success: function (data) {
            $('#saleorderlist').append(data);
        },
        error: function (data) {
            //alert("订单请求错误！");
        }
    });
}

//订单详情
function GetOrderDeatil(obj) {
    var orderId = $(obj).attr("orderid");
    var addressid = $(obj).attr("addressid");
    if (addressid == null) {
        addressid = "";
    }
    window.location.href = '/Sale/OrderDetail?orderId=' + orderId + '&addressid=' + addressid + '&ti=' + Math.random();
}

//var mpage = 1, mrows = '' || 10, mcount = 0, isLoading;
////判断滚动条是否拉制底部
//function lazyload(selector, callback) {
//    $(selector).append('<div>' +
//           '<section class="app_list_loading" id="mini_loading" style="display:none;"></section>' +
//           '<span class="app_list_load_last" style="display:none;">没有更多商品了。。。</span>' +
//           '</div>');
//    $('body').on('touchend', function () {
//        var bodyHeight = $('body').height(),
//            winHeight = $(window).height(),
//            scrollTop = $(window).scrollTop();
//
//        if (scrollTop >= (bodyHeight - winHeight - 45) && !isLoading) {
//            isLoading = true;
//            $("#mini_loading").css("display", "block");
//            if (mcount < mpage) {
//                mcount++;
//            } else {
//                $("#mini_loading").css("display", "none");
//                $(".app_list_load_last").css("display", "block");
//                return;
//            }
//            $("#mini_loading").css("display", "block");
//            callback && callback.call();
//        }
//    });
//
//}