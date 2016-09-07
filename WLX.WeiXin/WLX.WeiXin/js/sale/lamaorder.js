$(function () {
    loadOrderlist();
    $('body').on('touchend', lazyload);
    //sort
    $("#ordersnav a").click(function () {
        mcount = 1;
        $("#ordersnav a").removeClass("chikr-current");
        $(this).addClass("chikr-current");
        loadOrderlist();
    });

    //search
    $("#search").click(function () {
        mcount = 1;
        loadOrderlist();
    });

});

//var mpage = 1, mrows = '' || 10, mcount = 1, isLoading;

//加载订单列表
function loadOrderlist() {
    var attr;
        $("#ordersnav a").each(function () {
            if ($(this).attr("class") == 'chikr-current') {
                attr = $(this).attr('attr');
            }
        });

    $.ajax({
        url: "/sale/GetOrderList",
        data: $.param({
            OrderStatusID: attr,
            OrderID: $("#keyword").val(),
            pageIndex: mcount,
            pageSize: mrows
        }),
        type: 'post',
        dataType: "html",
        success: function (data) {
            if (mcount == 1) {
                $('.chikr-orders-list').html(data);
            }
            else {
                $('.chikr-orders-list').append(data);
            }
            mcount++;
            $("#mini_loading").css("display", "none");
            isLoading = false;
        },
        error: function (data) {
            //alert("加载订单请求错误！");
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


//判断滚动条是否拉制底部
function lazyload() {
    var bodyHeight = $('body').height(),
        winHeight = $(window).height(),
        scrollTop = $(window).scrollTop();
    var OrderTotalNum = $("#OrderTotalNum").val();
    var num = Math.ceil(OrderTotalNum / 10);
    if (scrollTop >= (bodyHeight - winHeight - 45) && !isLoading && mpage <= num) {
        isLoading = true;
        $("#mini_loading").css("display", "block");
        if (mcount < mpage) {
            mcount++;
        } else {
            return;
        }
        loadOrderlist();
    }

}
