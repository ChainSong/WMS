$(document).ready(function () {
    $(".Ooperation").live("mouseover", function () {
        $(this).prev()[0].style.display = "none";
    });
    $(".Ooperation").live("mouseenter", function () {
        $(this).prev()[0].style.display = "";
    });
    $(".Adiv").live("mouseleave", function () {
        $(this)[0].style.display = "none";
    });
    $("tr").live("mouseenter", function () {
        $(".Adiv").each(function (a, b) {
            $(b)[0].style.display = "none";
        })
    });
    var selecs = $('select[id=SearchCondition_WarehouseID]').val(); //获取改变的选项值
    $('select[id=SearchCondition_WarehouseID]').live('change', function ()
    {
        var AreaLists = $("#AreaLists");
        var Area = document.getElementById("SearchCondition_Area");
        Area.length = 0;
        var opt = new Option("==请选择==", "==请选择==");
        Area.options.add(opt);
        for (var i = 0; i < AreaLists[0].length; i++)
        {
            if (AreaLists[0][i].value == $("#SearchCondition_WarehouseID").val())
            {
                var opt = new Option(AreaLists[0][i].text, AreaLists[0][i].text);
                Area.options.add(opt);
            }
        }
    });
    $(function ($) {
        $('body').click(function () {
            ShowsOut()
        });
    });
    $('#searchButton').click(function () {
        setPageControlVal();
    });
    var setPageControlVal = function () {
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
    $("#AddButton").live('click', function () {
        var pamstr = "";
        if ($("#SearchCondition_CustomerID").val() != '') {
            pamstr += "?customerID=" + $("#SearchCondition_CustomerID").val();
        }
        if ($("#SearchCondition_WarehouseID").val() != '') {
            if (pamstr != "") {
                pamstr += "&&warehouseID=" + $("#SearchCondition_WarehouseID").val();
            }
            else {
                pamstr += "?warehouseID=" + $("#SearchCondition_WarehouseID").val();
            }
        }
        location.href = "/WMS/SettlementManagement/PaySettlementDetail" + pamstr;
    });
    ////客户  仓库联动
    $('select[id=SearchCondition_CustomerID]').live('change', function () {
        if ($(this).val().length > 0) {
            var selec = $(this).val(); //获取改变的选项值
            if (selec != "") {
            $.ajax({
                type: "POST",
                url: "/WMS/SettlementManagement/ChangeCustomer",
                data: {
                    "str": selec == null ? 0 : selec,
                },
                async: "false",
                success: function (data) {

                    var js = JSON.parse(data);
                        if (js.length != 0) {
                    document.all['SearchCondition_WarehouseID'].length = 0;
                    for (var i = 0; i < js.length; i++) {
                        document.all['SearchCondition_WarehouseID'].options.add(new Option(js[i]["WarehouseName"], js[i]["ID"]));
                    }
                    var selecs = $('select[id=SearchCondition_WarehouseID]').val(); //获取改变的选项值
                    $.ajax({
                        type: "POST",
                        url: "/WMS/SettlementManagement/ChangeWarehouse",
                        data: {
                            "str": selecs == null ? 0 : selecs,
                        },
                        async: "false",
                        success: function (datas) {

                            var js = JSON.parse(datas);
                                    if (js.length != 0) {
                            document.all['SearchCondition_Area'].length = 0;
                            document.all['SearchCondition_Area'].options.add(new Option("全部", 0));
                                        for (var i = js.length - 1; i > 0; i--) {
                                document.all['SearchCondition_Area'].options.add(new Option(js[i]["AreaName"], js[i]["ID"]));
                            }
                            var areaval = $('#HidArea').val();
                            if (areaval != '')
                                $('#HidArea option[value="' + areaval + '"]').attr('selected', true);
                                    }
                        },
                        error: function (msg) {
                            alert(msg.val);
                        }
                    });
                        }
                },
                error: function (msg) {
                    alert(msg.val);
                }
            });
            }
        }
        else {
            document.all['SearchCondition_AreaID'].length = 0;
            document.all['SearchCondition_AreaID'].options.add(new Option("==请选择==", "==请选择=="));
        }
    });
    //全选
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }
    });
    $('#SummaryExportSettlement').click(function () {
        SummaryExportSettlement();
    });
});
//取选中数据
function TableToJson() {
    var a = "";
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            a += $('#' + checkBoxs[i].id).data('settlementnumber').toString() + ",";
            //a += $('#' + checkBoxs[i].id).attr('data-id').toString() + ",";
        }
    }
    a = a.substring(0, a.length - 1);
    return a;
}
//编辑
function edit(SettlementNumber) {
    layer.confirm('<font size="4">是否编辑？</font>【' + SettlementNumber + "】", {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        var CustomerID = $('select[id=SearchCondition_CustomerID]').val();
        var WarehouseID = $('select[id=SearchCondition_WarehouseID]').val();
        location.href = "/WMS/SettlementManagement/SettlementEdit/?ViewType=1&&SettlementNumber=" + SettlementNumber
    });
}
//查看
function Look(SettlementNumber) {
    var CustomerID = $('select[id=SearchCondition_CustomerID]').val();
    var WarehouseID = $('select[id=SearchCondition_WarehouseID]').val();
    //var list = JSON.parse('@Html.Raw(ViewBag.CustomerList)');
    //var CustomerName = list.selec
    location.href = "/WMS/SettlementManagement/SettlementLook/?SettlementNumber=" + SettlementNumber + '&&CustomerID=' + CustomerID + '&&WarehouseID=' + WarehouseID;
}
//确认
function Done(SettlementNumber) {
    //var SettlementNumber = $('#SearchCondition_SettlementNumber').val();
    layer.confirm('<font size="4">是否确认？</font>【' + SettlementNumber + "】", {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Post",
            url: "/WMS/SettlementManagement/GetSettlementDone",
            data: {
                "SettlementNumber": SettlementNumber
            },
            async: "false",
            success: function (datasss) {
                layer.confirm('<font size="4">' + datasss + "</font>", {
                    btn: ['确定'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                    //shade: [0.8, '#393D49'],
                    title: ['提示', 'font-size:18px;']
                    //按钮
                });
            }, error: function (msg) {
                layer.confirm('<font size="4">' + msg + "</font>", {
                    btn: ['确定'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                    //shade: [0.8, '#393D49'],
                    title: ['提示', 'font-size:18px;']
                    //按钮
                });
            }
        });
    });
}
//删除
function Delete(SettlementNumber) {
    layer.confirm('<font size="4">是否删除？</font>【' + SettlementNumber + "】", {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        var CustomerID = $('select[id=SearchCondition_CustomerID]').val();
        var WarehouseID = $('select[id=SearchCondition_WarehouseID]').val();
        $.ajax({
            type: "Post",
            url: "/WMS/SettlementManagement/SettlementDelete",
            data: {
                "customerID": CustomerID,
                "warehouseID": WarehouseID,
                "SettlementNumber": SettlementNumber
            },
            async: "false",
            success: function (datasss) {
                layer.confirm('<font size="4">' + datasss + "</font>", {
                    btn: ['确定'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                    //shade: [0.8, '#393D49'],
                    title: ['提示', 'font-size:18px;']
                    //按钮
                }, function () {
                    window.location.reload();
                });
            }, error: function (msg) {
                layer.confirm('<font size="4">' + msg + "</font>", {
                    btn: ['确定'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                    //shade: [0.8, '#393D49'],
                    title: ['提示', 'font-size:18px;']
                    //按钮
                }, function () {
                    window.location.reload();
                });
            }
        });
    });
   
}
//打印
function Print(SettlementNumber) {
    layer.confirm('<font size="4">是否打印？</font>【' + SettlementNumber + "】", {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        window.location = "/WMS/SettlementManagement/PrintSettlement?Settlementnumber=" + SettlementNumber;
    });
}
//导出
function SummaryExportSettlement() {
    var SettlementNumberList = TableToJson();
    if (SettlementNumberList == '' || SettlementNumberList == null) {
        layer.confirm('<font size="4">请至少勾选一单？</font>', {
            btn: ['确定'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        });
    }
    else
    {
        layer.confirm('<font size="4">是否批量导出？</font>', {
            btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
                //取所有勾选的结算单
                var CustomerID = $('select[id=SearchCondition_CustomerID]').val();
                var WarehouseID = $('select[id=SearchCondition_WarehouseID]').val();
                layer.close(index);
                window.location = "/WMS/SettlementManagement/SummaryExportSettlement?SettlementNumberList=" + SettlementNumberList + '&&CustomerID=' + CustomerID + '&&WarehouseID=' + WarehouseID;
            });
    }
}
//导出
function ExportSettlement(SettlementNumber) {
    layer.confirm('<font size="4">是否导出？</font>【' + SettlementNumber + '】', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        var CustomerID = $('select[id=SearchCondition_CustomerID]').val();
        var WarehouseID = $('select[id=SearchCondition_WarehouseID]').val();
        layer.close(index);
        window.location = "/WMS/SettlementManagement/ExportSettlement?SettlementNumber=" + SettlementNumber + '&&CustomerID=' + CustomerID + '&&WarehouseID=' + WarehouseID;
    });
}
function ShowsIn(ID, obj) {
    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
        $(".ddiv:not(obj)").animate({
            width: "hide",
            width: "320%",
            paddingRight: "hide",
            paddingLeft: "hide",
            marginRight: "hide",
            marginLeft: "hide"
        });
        $("#operateTD" + ID).animate({
            width: "show",
            width: "360%",
            paddingRight: "show",
            paddingLeft: "show",
            marginRight: "show",
            marginLeft: "show"
        });
    }
    //$("#operateTD" + ID)[0].style.display = "";
}
function ShowsOut() {
    //$("#operateTD" + ID).fadeOut("slow");
    $(".ddiv").animate({
        width: "hide",
        width: "320%",
        paddingRight: "hide",
        paddingLeft: "hide",
        marginRight: "hide",
        marginLeft: "hide"
    });
    //$("#operateTD" + ID)[0].style.display = "";
}
