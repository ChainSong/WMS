$(document).ready(function () {
    if ($('#SearchCondition_CustomerID').val() == "") {
        $('#SearchCondition_CustomerID option:first').next().attr("selected", "selected");

    } else {

    }
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
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

    });
    var selecs = $('select[id=SearchCondition_Warehouse]').val(); //获取改变的选项值
    //if (selecs != "") {
    //$.ajax({
    //    type: "POST",
    //    url: "/WMS/Warehouse/ChangeWarehouse",
    //    data: {
    //        "str": selecs == null ? 0 : selecs,
    //    },
    //    async: "false",
    //    success: function (datas) {
    //        if (datas == '')
    //        {
    //            return;
    //        }
    //        var js = JSON.parse(datas);
    //            if (js.length != 0) {
    //        document.all['SearchCondition_Area'].length = 0;
    //        document.all['SearchCondition_Area'].options.add(new Option("全部", 0));
    //        for (var i = 0; i < js.length; i++) {
    //            document.all['SearchCondition_Area'].options.add(new Option(js[i]["AreaName"], js[i]["ID"]));
    //        }
    //        var areaval = $('#HidArea').val();
    //        if (areaval != '')
    //            $('#HidArea option[value="' + areaval + '"]').attr('selected', true);
    //            }
    //    },
    //    error: function (msg) {
    //        alert(msg.val);
    //    }
    //});
    //}
    $("#CheckSend").live('click', function () {
        if ($("#resultTable tbody input[type='checkbox']:checked").size() == 0) {//加了一个判断，当用户没有选择要打印订单时,提示 20170807
            showMsg("请选择单据！", "4000");
            return;
        }
        layer.confirm('<font size="4">确认是否批量推送盘点单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var jsonStr = TableToJsonJC();

            $.ajax({
                url: "/WMS/Warehouse/CheckSend",
                type: "POST",
                dataType: "json",
                data: {
                    JCRequestList: jsonStr
                },
                success: function (data) {
                    if (data.Result.length != 0) {
                        var table1 = $('#OrderCheck01');
                        $("#OrderCheck01  tr:not(:first)").empty("");
                        for (var i = 0; i < data.Result.length; i++) {
                            var row = $("<tr></tr>");
                            var td1 = $("<td>" + data.Result[i].relatednumber + "</td>");
                            var td2 = $("<td>" + (data.Result[i].flag == "success" ? "成功" : "失败") + "</td>");
                            var td3 = $("<td>" + data.Result[i].message + "</td>");
                            row.append(td1);
                            row.append(td2);
                            row.append(td3);
                            table1.append(row);
                        }
                        layer.open({
                            type: 1,
                            title: '推送结果',
                            skin: 'layui-layer-rim', //加上边框
                            area: ['800px', '600px'], //宽高
                            content: $('#OrderCheck01Div')

                        });
                    }
                    else {
                        showMsg("推送失败！" + data, 3000);
                    }
                }
            });
        });
    });

    $('select[id=SearchCondition_Warehouse]').live('change', function ()
    {
        var AreaLists = $("#AreaLists");
        var Area = document.getElementById("SearchCondition_Area");
        Area.length = 0;
        var opt = new Option("==请选择==", "==请选择==");
        Area.options.add(opt);
        for (var i = 0; i < AreaLists[0].length; i++)
        {
            if (AreaLists[0][i].value == $("#SearchCondition_Warehouse").val())
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
    ////客户  仓库联动
    //$('select[id=SearchCondition_CustomerID]').live('change', function () {
    //    window.location.href = "/WMS/Warehouse/WareHouseCheckDetail/?customerID=" + $(this).val();
    //});
    ////仓库  库区联动
    //$('select[id=SearchCondition_Warehouse]').live('change', function () {
    //    window.location.href = "/WMS/Warehouse/WareHouseCheckDetail/?warehouseID=" + $(this).val();
    //});
    $("#AddButton").live('click', function () {
        var pamstr = "";
        if ($("#SearchCondition_CustomerID").val() != '') {
            pamstr += "?customerID=" + $("#SearchCondition_CustomerID").val();
        }
        if ($("#SearchCondition_Warehouse").val() != '') {
            if (pamstr != "") {
                pamstr += "&&warehouseID=" + $("#SearchCondition_Warehouse").val();
            }
            else {
                pamstr += "?warehouseID=" + $("#SearchCondition_Warehouse").val();
            }
        }
        location.href = "/WMS/Warehouse/WareHouseCheck" + pamstr;
    });
    ////客户  仓库联动
    $('select[id=SearchCondition_CustomerID]').live('change', function () {
        if ($(this).val().length > 0) {
            var selec = $(this).val(); //获取改变的选项值
            if (selec != "") {
            $.ajax({
                type: "POST",
                url: "/WMS/Warehouse/ChangeCustomer",
                data: {
                    "str": selec == null ? 0 : selec,
                },
                async: "false",
                success: function (data) {

                    var js = JSON.parse(data);
                        if (js.length != 0) {
                    document.all['SearchCondition_Warehouse'].length = 0;
                    for (var i = 0; i < js.length; i++) {
                        document.all['SearchCondition_Warehouse'].options.add(new Option(js[i]["WarehouseName"], js[i]["ID"]));
                    }
                    var selecs = $('select[id=SearchCondition_Warehouse]').val(); //获取改变的选项值
                    $.ajax({
                        type: "POST",
                        url: "/WMS/Warehouse/ChangeWarehouse",
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
    ////仓库  库区联动
    //$('select[id=SearchCondition_Warehouse]').live('change', function () {
    //    if ($(this).val().length > 0) {
    //        var selec = $(this).val(); //获取改变的选项值
    //        $.ajax({
    //            type: "POST",
    //            url: "/WMS/Warehouse/ChangeWarehouse",
    //            data: {
    //                "str": selec,
    //            },
    //            async: "false",
    //            success: function (data) {
    //                var js = JSON.parse(data);
    //                document.all['SearchCondition_Area'].length = 0;
    //                document.all['SearchCondition_Area'].options.add(new Option("全部", 0));
    //                for (var i = 0; i < js.length; i++) {
    //                    document.all['SearchCondition_Area'].options.add(new Option(js[i]["AreaName"], js[i]["ID"]));
    //                }
    //            },
    //            error: function (msg) {
    //                alert(msg.val);
    //            }
    //        });
    //    }
    //    else {
    //        document.all['SearchCondition_AreaID'].length = 0;
    //        document.all['SearchCondition_AreaID'].options.add(new Option("==请选择==", "==请选择=="));
    //    }
    //});
});
//编辑
function edit(ID) {
    location.href = "/WMS/Warehouse/WareHouseCheckEdit/?ViewType=1&&CheckNumber=" + ID
}
//查看
function Look(ID) {
    location.href = "/WMS/Warehouse/WareHouseCheckEdit/?ViewType=2&&CheckNumber=" + ID
}
//完成盘点
function Done() {
    var CheckNumber = $('#SearchCondition_CheckNumber').val();
    layer.confirm('<font size="4">确认是否完成盘点单</font>【' + CheckNumber + "】", {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Post",
            url: "/WMS/Warehouse/GetWareHouseCheckDone",
            data: {
                "CheckNumber": CheckNumber
            },
            async: "false",
            success: function (datasss) {
                showMsg(datasss, "2000");
                location.href = "/WMS/Warehouse/WareHouseCheckDetail";
                //location.reload();
            }, error: function (msg) {
                showMsg("查询失败", "2000");
            }
        });
    });
}
//删除
function Delete(CheckNumber) {
    layer.confirm('<font size="4">确认是否删除盘点单</font>【' + CheckNumber + "】", {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Post",
            url: "/WMS/Warehouse/GetWareHouseCheckDelete",
            data: {
                "CheckNumber": CheckNumber
            },
            async: "false",
            success: function (datasss) {
                showMsg(datasss, "2000");
                location.href = "/WMS/Warehouse/WareHouseCheckDetail";
                //location.reload();
            }, error: function (msg) {
                showMsg("查询失败", "2000");
            }
        });
    });
    //if (confirm("是否删除[" + CheckNumber + "]？")) {
    //    $.ajax({
    //        type: "Post",
    //        url: "/WMS/Warehouse/GetWareHouseCheckDelete",
    //        data: {
    //            "CheckNumber": CheckNumber
    //        },
    //        async: "false",
    //        success: function (datasss) {
              
    //                showMsg(datasss, "2000");
    //                location.reload();
    //        }, error: function (msg) {
    //            showMsg("查询失败", "2000");
    //        }
    //    });
    //}
}
//打印
function Print(CheckNumber) {
    window.location = "/WMS/Warehouse/PrintWareHouseCheck?checknumber=" + CheckNumber;
    //window.location.href = "/WMS/Warehouse/PrintWareHouseChecks/?checknumber =121 ";
}
//打印
function ExportCheck(CheckNumber) {
    window.location = "/WMS/Warehouse/ExportCheck?checknumber=" + CheckNumber;
    //window.location.href = "/WMS/Warehouse/PrintWareHouseChecks/?checknumber =121 ";
}
function ExportCheckRF(CheckNumber) {
    window.location = "/WMS/Warehouse/ExportCheckRF?checknumber=" + CheckNumber;
    //window.location.href = "/WMS/Warehouse/PrintWareHouseChecks/?checknumber =121 ";
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

function TableToJsonJC() {
    var txt = "{\"jCRequestLists\":[";
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            var r = "{";
            r += "\"ID\":\"" + checkBoxs[i].dataset.id + "\",";
            r += "\"CustomerID\":\"" + checkBoxs[i].dataset.customerid + "\",";
            r += "\"WarehouseID\":\"" + checkBoxs[i].dataset.warehouseid + "\",";
            r += "\"RelateNumber\":\"" + checkBoxs[i].dataset.checknumber + "\",";
            r = r.substring(0, r.length - 1)
            r += "},";
            txt += r;
        }
    }
    txt = txt.substring(0, txt.length - 1);
    txt += "]}";
    return txt;
}