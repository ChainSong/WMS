$(document).ready(function () {
    $('#searchButton').click(function () {
        setPageControlVal();
        var customerName = $('#ASNCondition_CustomerID').val();
        var warehouseName = $('#ASNCondition_WarehouseName').val();
        if (customerName == '' || warehouseName == '') {
            showMsg("请选择客户和仓库！", 3000);
            return false;
        }
    })

    var setPageControlVal = function () {

        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');

            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];

            var descID = 'ASNCondition_';
            if (pref === 'start') {
                descID += 'Start' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());

        });
    }
    //悬浮事件
    $(".Ooperation").live("mouseover", function () {
        $(this).prev()[0].style.display = "none";
    });
    $(".Ooperation").live("mouseenter", function () {
        $(this).prev()[0].style.display = "";

    })
    $(".Adiv").live("mouseleave", function () {
        $(this)[0].style.display = "none";
    })
    $("tr").live("mouseenter", function () {
        $(".Adiv").each(function (a, b) {
            $(b)[0].style.display = "none";
        })
    })
})
//菜单切换
function tabSwitch2(_this, content_prefix, active) {
    var tabs = document.getElementsByName(_this.name);
    var number = tabs.length;
    for (var i = 0; i < number; i++) {
        var tab = tabs[i];
        tab.className = "";
        document.getElementById(content_prefix + i).style.display = 'none';
    }
    _this.className = "easytab_active";
    document.getElementById(content_prefix + active).style.display = 'block';
}
//点击查看明细
function SearchDetail(status, obj, type) {
    var customerName = $('#ASNCondition_CustomerID').val();
    var warehouseName = $('#ASNCondition_WarehouseName').val();
    if (customerName == '' || warehouseName == '') {
        showMsg("请选择客户和仓库！", 3000);
        return false;
    }
    var tbList = document.getElementById('tbList');
    tbList.innerHTML = "";
    var div1 = $(obj).parent().parent().parent().parent().parent().parent();
    var tab = document.getElementsByName("easytab")[1];
    //ajax查询到订单明细
    $.ajax({
        type: "Post",
        url: "/WMS/ASNManagementFG/SearchReceiptOrderTotal",
        data: {
            "CustomerID": $('#ASNCondition_CustomerID').val(),
            "Warehouose": $('#ASNCondition_WarehouseName').val(),
            "Status": status,
            "StartTime": $('#ASNCondition_StartCreateTime').val(),
            "EndTime": $('#ASNCondition_EndCreateTime').val(),
            "Type": type
        },
        async: "false",
        success: function (data) {
            if (data.Errorcode == 1) {
                var html = "";
                var OrderInfo = data;
                for (var i = 0; i < OrderInfo.data.length; i++) {
                    var obj = OrderInfo.data[i];
                    html += '<tr>'
                    html += "<td>" + obj.ASNNumber + "</td>";
                    html += "<td>" + obj.ExternReceiptNumber + "</td>";
                    html += "<td>" + obj.str1 + "</td>";
                    html += "<td>" + obj.Int1 + "</td>";
                    html += "<td>" + obj.ASNType + "</td>";
                    html += "<td>" + obj.WarehouseName + "</td>";
                    html += "</tr>"
                }
                $('#tbList').append(html);

            }
            else {
            }
        },
        error: function (msg) {
            showMsg("网络连接失败！", 2000);
        }
    });

    tabSwitch2(tab, "easytab_content_", 1);
}
