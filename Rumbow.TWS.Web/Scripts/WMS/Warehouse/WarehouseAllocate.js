//自动检索功能
//$(function () {
   
//    $('#CustomerName').autocomplete({
//        source: function (request, response) {
//            //$.ajax({
//            //    url: "/WMS/Warehouse/GetAllCustomersbyCustomerID",
//            //    type: "POST",
//            //    dataType: "json",
//            //    data: { name: request.term },
//            //    success: function (data) {
//            //            response($.map(data, function (item) {
//            //            return { label: item.Text, value: item.Text, data: item }
//            //        }));
//            //    }
//            //});
//        },
//        select: function (event, ui) {
//            $('#CustomerID').val(ui.item.data.Value);
//            $('#CustomerName').val(ui.item.data.Text);
           
//            //查找当前用户拥有哪些仓库权限
//            var CustomerID = $('#CustomerID').val();
//            if (CustomerID === '') {
//                return;
//            } else {
//                $.send(
//                '/WMS/Warehouse/GetWarehouseAllocate',
//                { CustomerID: CustomerID },
//                   function (response) {
//                       $('.checkForSelect').removeAttr('checked');
//                       for (var i = 0; i < response.length; i++) {
//                           $('.checkForSelect[data-id=' + response[i].WarehouseID + ']').attr('checked', 'checked');
//                       }
//                   },
//                   function () {
//                       Runbow.TWS.Alert("获取仓库客户权限失败！");
//                   });
//            }
//        }
//    }).change(function () {
//        $('.checkForSelect').removeAttr('checked');
//        if ($(this).val() === '') {
//            $('#CustomerID').val('');
             
//        }
//    });
//});
//function keypress() {
//    $('.checkForSelect').removeAttr('checked');
//}

//提交按钮
$(document).ready(function () {
    $('.checkForSelect').click(function () {
        if ($(this).attr("checked") === "checked") {
            $(this).attr('checked', 'checked');
        }
    });

    $('#submitWarehouseUser').click(function () {
        var WarehouseID = '';
        var CustomerID = $('#CustomerID').val();

        if (CustomerID === '') {
            Runbow.TWS.Alert('请选择客户');
            return;
        }

        $('.checkForSelect').each(function (index) {
            if ($(this).attr('checked') === 'checked') {
                WarehouseID = $(this).attr('data-id');

         $.send(
            '/WMS/Warehouse/SetWarehouseAllocate',
            { CustomerID: CustomerID, WarehouseID: WarehouseID },
               function (response) {
                   Runbow.TWS.Alert(response);
               },
               function () {
                   Runbow.TWS.Alert("分配仓库客户权限设置失败！");
               });
            }
        });
        if (WarehouseID === '') {
            Runbow.TWS.Alert('请选择需要设置的仓库');
            return;
        }
    });
});