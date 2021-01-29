$(document).ready(function () {
    
    //客户  仓库联动
    $('select[id=SearchCondition_CustomerID]').live('change', function () {
        window.location.href = "/WMS/SettlementManagement/Settlement/?customerID=" + $(this).val() + "&externNumber=" + $('#SearchCondition_ExternNumber').val();
    });

    //仓库  库区联动
    $('select[id=SearchCondition_WarehouseID]').live('change', function () {
        window.location.href = "/WMS/SettlementManagement/Settlement/?warehouseID=" + $(this).val() + "&customerID=" + $('select[id=SearchCondition_CustomerID]').val() + "&externNumber=" + $('#SearchCondition_ExternNumber').val();
    });

    var setPageControlVal = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id;
            if ($(this).attr("type") === "checkbox") {
                var isChecked = document.getElementById(id).checked;
                if (isChecked) {
                    $('#' + descId).val("1");
        
        } else {
                    $('#' + descId).val("0");
                }
            } else {
                $('#' + descId).val($(this).val());
            }
        });
        $('.DropDownList').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id.substr(0, id.length - 4) + "ID";
            $('#' + descId).val($(this).val());
        });
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += 'Start' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    }

    //提交查询按钮并返回数据
    $("#searchButton").click(function () {
        if ($("#SearchCondition_CustomerID option:selected").val() == "") {
            layer.tips('请选择客户！', '#SearchCondition_CustomerID', {
                tips: [1, '#3595CC'],
                time: 4000
            });
            return false;
        }
        if ($("#SearchCondition_WarehouseID option:selected").val() == "") {
            layer.tips('请选择仓库！', '#SearchCondition_WarehouseID', {
                tips: [1, '#3595CC'],
                time: 4000
            });
            return false;
        }
        setPageControlVal();
    });
    
    //提交保存按钮并返回数据
    $("#SaveButton").click(function () {
        if ($("#SearchCondition_Completeddate").val() == "") {
            layer.tips('请选择日期！', '#SearchCondition_Completeddate', {
                tips: [1, '#3595CC'],
                time: 4000
            });
            return false;
        }
        if ($("#SearchCondition_CustomerID option:selected").val() == "") {
            layer.tips('请选择客户！', '#SearchCondition_CustomerID', {
                tips: [1, '#3595CC'],
                time: 4000
            });
            return false;
        }
        if ($("#SearchCondition_WarehouseID option:selected").val() == "") {
            layer.tips('请选择仓库！', '#SearchCondition_WarehouseID', {
                tips: [1, '#3595CC'],
                time: 4000
            });
            return false;
        }
        var rows = $("#RoleTable")[0].rows.length;
        if (rows == '1') {
            layer.confirm('<font size="4">保存失败，没有结算明细！</font>', {
                btn: ['确定'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                //shade: [0.8, '#393D49'],
                title: ['提示', 'font-size:18px;']
                //按钮
            });
            return false;
        }
    })

});

function Returns() {
    var url = $(window.parent.document).find(".active a").attr('href');
    url = url.toString().split(',')[2];
    url = url.substring(1, url.length - 2);
    location.href = url;
}

function Preview() {
    var index = null;//加载层
    var CustomerID = $('select[id=SearchCondition_CustomerID]').val();
    var WarehouseID = $('select[id=SearchCondition_WarehouseID]').val();
    var StartCompleteDate = $('#SearchCondition_StartCompleteDate').val().toString();
    var EndCompleteDate = $('#SearchCondition_EndCompleteDate').val().toString();
    var href = '/WMS/SettlementManagement/SettlementDetailPreview/?CustomerID='
        + CustomerID + '&WarehouseID=' + WarehouseID + '&StartCompleteDate=' + StartCompleteDate + '&EndCompleteDate=' + EndCompleteDate
    
    layer.open({
        type: 2,
        title: '结算预览',
        shadeClose: true,//点击遮罩区域是否关闭页面
        shade: 0.5,
        maxmin: true, //开启最大化最小化按钮
        area: ['1000px', '400px'],
        content: [href,'no'],
        move: '.layui-layer-title',
        moveOut: true,
        end: function () {
            //$datagrid.bootstrapTable('refresh');//关闭时的动作
        }
    });
    //$.ajax({
    //    type: "Post",
    //    url: "/WMS/SettlementManagement/SettlementDetailPreview", 
    //    data: {
    //        "CustomerID": CustomerID,
    //        "WarehouseID": WarehouseID,
    //        "StartCompleteDate": StartCompleteDate,
    //        "EndCompleteDate": EndCompleteDate
    //    },
    //    beforeSend: function (request) {
    //        index = layer.load();//加载层
    //    },
    //    async: "false", //同步请求
    //    success: function (datasucc) {
    //        layer.close(index);//关闭加载层

    //    }, error: function (msg) {
    //        layer.confirm('<font size="4">' + msg + '</font > ', {
    //            btn: ['确定'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
    //            //shade: [0.8, '#393D49'],
    //            title: ['提示', 'font-size:18px;']
    //            //按钮
    //        });
    //    }
    //});

}