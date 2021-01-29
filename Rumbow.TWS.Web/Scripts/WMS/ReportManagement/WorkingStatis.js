$(function () {
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
    //全选
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        if ($(this).attr("checked") === "checked") {
            checkBoxs.each(function (a, h) {
                if ($($(h)).attr("data-status") != -1) {
                    $(h)[0].checked = true;
                }
            })
        } else {
            checkBoxs.removeAttr("checked");
        }
    });
    $('#searchButton').click(function () {
        setPageControlVal();

    });
    var setPageControlVal = function () {
        $(".calendarRangeReWrite").each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += 'Begin' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    };

});

var index;

//更新工时
function UpdateWorking(ID, obj) {
    $("#UpdateWorkID").val(ID);
    let updateStatisDate = $(obj).parent().parent().parent().find('[name=StatisD]').text();
    let updatePersonNumber = $(obj).parent().parent().parent().find('[name=PersonNo]').text();
    let updatePWorkHour = $(obj).parent().parent().parent().find('[name=WorkH]').text();
    $("#updateStatisDate").text(updateStatisDate);
    $("#updatePersonNo").val(updatePersonNumber);
    $("#updateWorkH").val(updatePWorkHour);
    index = layer.open({
        type: 1,
        title: '更新工时',
        shadeClose: true,
        shade: false,
        maxmin: false, //开启最大化最小化按钮
        area: ['400px', '300px'],
        content: $("#UpdateDiv"),
        move: '.layui-layer-title',
        moveOut: true
    });


}

//确定更新
$("#UpdateOK").live('click', function () {
    if ($("#updatePersonNo").val() == "") {
        layer.tips('请输入总人数', '#updatePersonNo');
        return;
    }
    if ($("#updateWorkH").val() == "") {
        layer.tips('请输入总工时', '#updateWorkH');
        return;
    }

    //更新
    let request = {
        ID: $("#UpdateWorkID").val(),
        PersonNumber: parseInt($("#updatePersonNo").val()),
        WorkHour: parseInt($("#updateWorkH").val())
    };

    $.ajax({
        url: "/WMS/ReportManagement/UpdateWorkingStatis",
        type: "POST",
        data:
        {
            requestData: JSON.stringify(request)
        },
        async: false,
        success: function (res) {
            if (res.code == 0) {
                layer.alert('更新成功！', {
                    skin: 'layui-layer-lan'
                    , closeBtn: 0
                }, function () {
                    layer.close(index);
                    window.location.href = "/WMS/ReportManagement/WorkingStatis";
                });
            }
            else {
                layer.alert('更新失败：' + res.msg, {
                    skin: 'layui-layer-lan'
                    , closeBtn: 0
                }, function () {

                });
            }
        },
        error: function (err) {
            layer.alert('更新失败，服务器错误', {
                skin: 'layui-layer-lan'
                , closeBtn: 0
            }, function () {

            });
        }
    });




});

//取消更新
$("#UpdateCancel").live('click', function () {
    layer.close(index);
});



//新增按钮
$("#addWorkingStatis").live('click', function () {

    $("#createCustomer").text('');
    $("#createCustomerID").val('');
    $("#createWarehouse").text('');
    $("#createWarehouseID").val('');
    $("#createPersonNo").val('');
    $("#createWorkH").val('');
    $("#createStatisTime").val('');
    //判断客户和仓库是否选择
    let customerID = $('#SearchCondition_CustomerID').val();
    let warehouseID = $('#SearchCondition_WarehouseID').val();

    if (customerID == '' || warehouseID == '') {
        showMsg("请选择客户和仓库！", 3000);
        return false;
    }
    $("#createCustomer").text($("#SearchCondition_CustomerID").find('option:selected').text());
    $("#createCustomerID").val(customerID);
    $("#createWarehouse").text($("#SearchCondition_WarehouseID").find('option:selected').text());
    $("#createWarehouseID").val(warehouseID);
    $("#createPersonNo").val('');
    $("#createWorkH").val('');

    index = layer.open({
        type: 1,
        title: '新增工时',
        shadeClose: true,
        shade: false,
        maxmin: false, //开启最大化最小化按钮
        area: ['400px', '380px'],
        content: $("#CreateDiv"),
        move: '.layui-layer-title',
        moveOut: true
    });

});


$("#CreateOK").live('click', function () {
    if ($("#createCustomerID").val() == "") {
        layer.tips('客户不能为空', '#createCustomer');
        return;
    }
    if ($("#createWarehouseID").val() == "") {
        layer.tips('仓库不能为空', '#createWarehouse');
        return;
    }

    if ($("#createStatisTime").val() == "") {
        layer.tips('请选择日期', '#createStatisTime');
        return;
    }
    if ($("#createPersonNo").val() == "") {
        layer.tips('请输入总人数', '#createPersonNo');
        return;
    }
    if ($("#createWorkH").val() == "") {
        layer.tips('请输入总工时', '#createWorkH');
        return;
    }
    let request = {
        CustomerID: $("#createCustomerID").val(),
        WarehouseID: $("#createWarehouseID").val(),
        CustomerName: $("#createCustomer").text(),
        WarehouseName: $("#createWarehouse").text(),
        StatisDate: $("#createStatisTime").val(),
        PersonNumber: parseInt($("#createPersonNo").val()),
        WorkHour: parseInt($("#createWorkH").val())
    };

    $.ajax({
        url: "/WMS/ReportManagement/CreateWorkingStatis",
        type: "POST",
        data:
        {
            requestData: JSON.stringify(request)
        },
        async: false,
        success: function (res) {
            if (res.code == 0) {
                layer.alert('新增成功！', {
                    skin: 'layui-layer-lan'
                    , closeBtn: 0
                }, function () {
                    layer.close(index);
                    window.location.href = "/WMS/ReportManagement/WorkingStatis";
                });
            }
            else {
                layer.alert('新增失败：' + res.msg, {
                    skin: 'layui-layer-lan'
                    , closeBtn: 0
                }, function () {

                });
            }
        },
        error: function (err) {
            layer.alert('新增失败，服务器错误', {
                skin: 'layui-layer-lan'
                , closeBtn: 0
            }, function () {

            });
        }
    });


});

$("#CreateCancel").live('click', function () {
    layer.close(index);
});
