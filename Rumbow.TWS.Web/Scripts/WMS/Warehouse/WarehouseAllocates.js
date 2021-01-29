//自动检索功能
//$(function () {
//    $('#UserName').autocomplete({
//        source: function (request, response) {
//            $.ajax({
//                url: "/WMS/Warehouse/GetAllUserByUserID",
//                type: "POST",
//                dataType: "json",
//                data: { name: request.term },
//                success: function (data) {
//                    response($.map(data, function (item) {
//                        return { label: item.Text, value: item.Text, data: item }
//                    }));
//                }
//            });
//        },
//        select: function (event, ui) {
//            $('#UserID').val(ui.item.data.Value);
//            $('#UserName').val(ui.item.data.Text);

//            //查找当前用户拥有哪些仓库权限
//            var UserID = $('#UserID').val();
//            if (UserID === '') {
//                return;
//            } else {
//                $.send(
//                '/WMS/Warehouse/GetWarehouseAllocates',
//                { UserID: UserID },
//                   function (response) {
//                       $('.checkForSelect').removeAttr('checked');
//                       for (var i = 0; i < response.length; i++) {
//                           $('.checkForSelect[data-id=' + response[i].WarehouseID + ']').attr('checked', 'checked');
//                       }
//                   },
//                   function () {
//                       Runbow.TWS.Alert("获取用户仓库权限失败！");
//                   });
//            }
//        }
//    }).change(function () {
//        $('.checkForSelect').removeAttr('checked');
//        if ($(this).val() === '') {
//            $('#UserID').val('');

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
        var UserID = $('#UserID').val();

        if (UserID === '') {
            Runbow.TWS.Alert('请选择用户');
            return;
        }

        $('.checkForSelect').each(function (index) {
            if ($(this).attr('checked') === 'checked') {
                WarehouseID = $(this).attr('data-id');
                $.send(
                   '/WMS/Warehouse/SetWarehouseAllocates',
                   { UserID: UserID, WarehouseID: WarehouseID },
                      function (response) {
                          Runbow.TWS.Alert(response);
                      },
                      function () {
                          Runbow.TWS.Alert("分配用户仓库权限分配失败！");
                      });
            }
        });
        if (WarehouseID === '') {
            Runbow.TWS.Alert('请选择需要设置的仓库');
            return;
        }
    });
});