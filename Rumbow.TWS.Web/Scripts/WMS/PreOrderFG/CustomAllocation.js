$(document).ready(function () {
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

    if ($("#resultTable tbody")[0].rows.length < 1) {
        $("#resultTable").removeAttr("style");
        $("#Newdiv").removeAttr("style");
        $("#OperateTh").removeAttr("style");
    }
    //$(function ($) {
    //    $('body').click(function () {
    //        ShowsOut()
    //    });
    //});

    $('#printButton').live('click', function () {
        layer.confirm('<font size="4">确认是否批量打印拣货单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var id = TableToJson();
            if (id == "") {
                showMsg("请选择需要打印的拣货单！", 4000);
                return false;
            }
            else {
                if ($("#ProjectName").val() == "延锋百利得") {
                    window.location.href = '/WMS/OrderManagement/PrintPreOrderYFBLD?id=' + id + "&Flag=2";
                }
                else {
                    window.location.href = '/WMS/OrderManagement/PrintPreOrder?id=' + id + "&Flag=2";
                }
            }
        });

    });
    function TableToJson() {
        var a = "";

        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                a += checkBoxs[i].dataset.id + ",";
            }
        }

        a = a.substring(0, a.length - 1);


        return a;
    }

    $('select[id=SearchCondition_CustomerID]').live('change', function () {
        var hiddenActionButton = $('#HideActionButton').val();
        var showEditRelated = $('#ShowEditRelated').val();
        window.location.href = "/WMS/PreOrder/Index/?customerID=" + $(this).val() + "&hideActionButton=" + hiddenActionButton + "&showEditRelated=" + showEditRelated;
    });

    $("#AddPreOrder").click(function () {

        var CustomerID = $("#SearchCondition_CustomerID").val() == null ? 0 : $("#SearchCondition_CustomerID").val();
        var WarehouseID = $("#SearchCondition_Warehouse").val() == null ? 0 : $("#SearchCondition_Warehouse").val();
        window.location.href = "/WMS/PreOrder/PreOrderCreateOrEdit/?ViewType=1&customerID=" + $("#SearchCondition_CustomerID").val() + "&WarehouseID=" + WarehouseID;
    });
    $('#SearchCondition_Province').autocomplete({
        source: function (request, response) {
            var a = this;
            $.ajax({
                url: "/WMS/PreOrder/GetCitys",
                type: "POST",
                dataType: "json",
                data: {
                    find: "Province",
                    Province: $('#SearchCondition_Province').val(),
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
    });
    $('#SearchCondition_City').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/WMS/PreOrder/GetCity",
                type: "POST",
                dataType: "json",
                data: {
                    find: "City",
                    Province: $('#SearchCondition_Province').val(),
                    City: $('#SearchCondition_City').val(),
                    //District: $('#SearchCondition_District').val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
    });
    $('#SearchCondition_District').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/WMS/PreOrder/GetCity",
                type: "POST",
                dataType: "json",
                data: {
                    find: "SearchCondition_Province",
                    City: $('#SearchCondition_City').val(),
                    District: $('#SearchCondition_District').val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
    });

    //$("#searchButton").click(function () {
    //window, location.href = "/wms/Preorder/Index";
    //});

    //全选
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            //$(this.dataset.status!==-1){ 
            checkBoxs.each(function (a, h) {
                //$(h)[0].attr("checked", "checked");
                if ($($(h)).attr("data-status") != -1) {
                    //if ($(h)[0].data().status != -1) {
                    $(h)[0].checked = true;
                }
            })
            //checkBoxs.attr("checked", "checked"); 
            //}
        } else {
            checkBoxs.removeAttr("checked");
        }
    });
    $("#Automate").live('click', (function () {
        layer.confirm('<font size="4">确认是否批量自动分配？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var ids = "";
            var CustomerId = "";
            var Warehouse = "";
            var checkBoxs = $("#resultTable tbody input[type='checkbox']");

            for (var i = 0; i < checkBoxs.length; i++) {
                if (checkBoxs[i].checked) {
                    if ($(checkBoxs[i]).parent().parent()[0].children[6].innerText.trim() != "完成" && $(checkBoxs[i]).parent().parent()[0].children[6].innerText.trim() != "取消") {
                        ids += "" + checkBoxs[i].getAttribute("data-id").toString() + ",";
                        CustomerId += "" + checkBoxs[i].getAttribute("data-CustomerID").toString() + ",";
                        Warehouse += "" + checkBoxs[i].getAttribute("data-Warehouse").toString() + ",";
                    }
                }
            }
            if (ids.length > 0) {
                ids = ids.toString().substring(0, ids.toString().length - 1);
                CustomerId = CustomerId.toString().substring(0, CustomerId.toString().length - 1);
                Warehouse = Warehouse.toString().substring(0, Warehouse.toString().length - 1);
            } else {
                showMsg("没有可以分配的订单", 4000);
                return false;
            }
            $.ajax({
                url: "/WMS/PreOrder/AutomaticAllocation",
                type: "POST",
                dataType: "text",
                async: false,
                data: {
                    ids: ids,
                    CustomerId: CustomerId,
                    Warehouse: Warehouse
                },
                success: function (data) {
                    var StrHtml = JSON.parse(data)
                    if (data.Errorcode == 0) {
                        showMsg('操作失败，分配可能没有进行！', 4000);
                    } else {
                        if (StrHtml.data[0].Type == "1") {
                            var html = $("#Evaluation2").render(StrHtml.data);
                            $("#DisInfoBody2")["empty"]();
                            $("#DisInfoBody2").append(html);
                            //openPopup("panel", true, 400, 400, null, 'DisInfo2', true);
                            //$("#popupLayer_panel")[0].style.top = "200px";
                            layer.open({
                                title: '缺货信息',
                                type: 1,
                                skin: 'layui-layer-rim', //加上边框
                                area: ['450px', '400px'], //宽高
                                content: $("#DisInfo2")[0].innerHTML
                            });
                        }
                        else {
                            var html = $("#Evaluation").render(StrHtml.data);
                            $("#DisInfoBody")["empty"]();
                            $("#DisInfoBody").append(html);
                            openPopup("panel", true, 400, 400, null, 'DisInfo', true);
                            $("#popupLayer_panel")[0].style.top = "200px";
                        }
                        for (var i = 0; i < StrHtml.data.length; i++) {
                            for (var j = 0; j < checkBoxs.length; j++) {
                                if (checkBoxs[j].getAttribute("data-id") == StrHtml.data[i].POID) {
                                    $(checkBoxs[j]).parent().parent().children()[4].innerHTML = '<font size="3" color="red">' + status[StrHtml.data[i].Note] + '</font>';
                                    if (StrHtml.data[i].Note == '9') {
                                        $(checkBoxs[j]).parent().parent().children().last()[0].innerHTML = '<label data-name="' + StrHtml.data[i].POID + '" style="cursor: pointer; color:white" class="label label-info">查看出库单</label>';
                                    }
                                }
                            }
                        }
                    }
                    //location.reload();
                },
                error: function (msg) {
                    showMsg("操作失败", "4000");
                }
            });
        });

    }));
    $("#CompleteProOrder").live("click", function () {
        layer.confirm('<font size="4">确认是否批量完成？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var ids = "";
            var checkBoxs = $("#resultTable tbody input[type='checkbox']");

            for (var i = 0; i < checkBoxs.length; i++) {
                if (checkBoxs[i].checked && checkBoxs[i].dataset.status != -1) {
                    ids += "" + checkBoxs[i].dataset.id.toString() + ",";
                }
            }
            if (ids.length > 0) {
                ids = ids.toString().substring(0, ids.toString().length - 1);
            }
            else {
                showMsg("请选择要完成的订单!", 4000);
                return false;
            }
            $.ajax({
                url: "/WMS/PreOrder/OrderFinish",
                type: "POST",
                dataType: "json",
                data: {
                    ids: ids,
                    //CustomerID: self
                },
                success: function (data) {
                    showMsg("操作成功!", "4000");
                    location.href = "/WMS/PreOrder/Index"
                    //location.reload();
                },
                error: function (msg) {
                    showMsg("操作失败", "4000");
                }
            });
        });

    });

    $("#resultTbody>tr").each(function (a, h) {
        //if ($($(h).children("td").children(".checkBoxHead")[0]).children("input")[0].dataset.status == -1) {
        if ($(h).children(".checkBox").children("input")[0].dataset == undefined) {
            if ($($(h).children(".checkBox").children("input")[0]).attr("data-status") == -1) {
                $(h)[0].style.background = "#cbc8c8";
            }
        }
        else {
            if ($(h).children(".checkBox").children("input")[0].dataset.status == -1) {
                $(h)[0].style.background = "#cbc8c8";
                //$(h).children("td:last")[0].hidden = true
            }
        }
    })


    //$(".CheckOutboundOrder").live("click", function () {
    //    $.ajax({
    //        url: "/WMS/PreOrder/CheckOutboundOrder",
    //        type: "POST",
    //        dataType: "json",
    //        data: {
    //            Id: $(this)[0].dataset.id,
    //            //CustomerID: self
    //        },
    //        success: function (data) {
    //            if (data.ErrorCode == 1) {
    //                if (data.OrderInfo.length == 1) {
    //                    window.location = "/WMS/OrderManagement/OrderDetailView/?ID=" + data.OrderInfo[0].ID + "&ViewType=3";
    //                } else {
    //                    var html = $("#CheckOutboundOrderList").render(data.OrderInfo);
    //                    $("#CheckOutboundOrderBody")["empty"]();
    //                    $("#CheckOutboundOrderBody").append(html);
    //                    openPopup("panel", true, 400, 400, null, 'CheckOutboundOrder', true);
    //                    $("#popupLayer_panel")[0].style.top = "200px";
    //                }
    //            } else {
    //                showMsg("没有生成出库单", 4000);
    //            }
    //            //showMsg("操作成功", "4000");
    //            //location.reload();
    //        },
    //        error: function (msg) {
    //            showMsg("操作失败", "4000");
    //        }
    //    });
    //})
    $('.calendarRangeReWrite').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = 'start_';
        if (actualID === 'CreateTime') {
            if (pref === 'start') {
                descID += 'CreateTime';
            }
            else {
                descID = 'end_CreateTime';
            }
        } else {
            if (pref === 'start') {
                descID += 'OrderTime';
            }
            else {
                descID = 'end_OrderTime';
            }
        }

        $(this).val($('#' + descID).val());
    });
    $("#searchButton").click(function () {
        setPageControlVal();
    })
    var setPageControlVal = function () {
        $(".calendarRangeReWrite").each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'SearchCondition_';
            if (actualID === 'CreateTime') {
                if (pref === 'start') {
                    descID += 'CreateTime';
                }
                else {
                    descID += 'EndCreateTime';
                }
            }
            else {
                if (pref === 'start') {
                    descID += 'OrderTime';
                }
                else {
                    descID += 'EndOrderTime';
                }
            }
            $('#' + descID).val($(this).val());
        });
        //$('.calendarRangeReWrite').each(function (index) {
        //    var id = $(this).attr('id');
        //    var pref = id.split('_')[0];
        //    var actualID = id.split('_')[1];
        //    var descID = 'start_';
        //    if (actualID === 'ShelvesTime') {
        //        if (pref === 'start') {
        //            descID += 'CreateStorageTime';
        //        }
        //        else {
        //            descID += 'EndShelvesTime';
        //        }
        //    } else {
        //        if (pref === 'start') {
        //            descID += 'StartStorageTime';
        //        }
        //        else {
        //            descID += 'EndStorageTime';
        //        }
        //    }

        //    $('#' + descID).val($(this).val());
        //});
    }
    $("#portButtonTemplet").click(function () {
        demo();
        //demo('/WMS/PreOrder/ReportExportExecl?/CustomerID');

        //window.location.href = "http://www.runbow.com.cn:8080/Picture/DemoExcel/货品导入模板.xlsx";
    })
    function demo(url) {
        // $.send('/WMS/Product/demoExecl');
        // 绑定导出按钮

        var ID = $('select[id=StorerID]')[0].value;
        var url = '/WMS/PreOrder/ReportExportExecl';
        var form = $("<form>");
        form.attr('style', 'display:none');
        form.attr('target', '');
        form.attr('method', 'post}');
        form.attr('action', url);//'/WMS/PreOrder/ReportExportExecl'
        var input1 = $('<input>');
        input1.attr('type', 'hidden');
        input1.attr('name', 'demo');
        input1.attr('value', 'Export');
        var input2 = $('<input>');
        input2.attr('type', 'hidden');
        input2.attr('name', 'fileId');
        input2.attr('value', "fileId");
        var input3 = $('<input id="ID" name="ID" type="hidden" value="' + ID + '" />');
        $('body').append(form);
        form.append(input1);
        form.append(input2);
        form.append(input3);

        form.submit();
        form.remove();
    }

});

var status = { "1": "新增", "3": "库存不足", "5": "已生成出库单", "9": "完成" };

var fileImportClick = function () {
    if ($('#importExcel').val() === '') {
        //Runbow.TWS.Alert("请选择要导入的Excel");
        showMsg("请选择要导入的Excel", 4000);
        return false;
    }

    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        //Runbow.TWS.Alert("请选择Excel格式的文件");
        showMsg("请选择Excel格式的文件", 4000);
        //$('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    };

    WebPortal.MessageMask.Show("导入中...");
    $.ajaxFileUpload({
        url: "/WMS/PreOrder/ImputEcecl",
        secureuri: false,
        fileElementId: 'importExcel',
        type: "POST",
        dataType: "json",
        data: {
            CustomerName: $('#StorerID option:selected').text(),
            CustomerID: $('#StorerID').val(),
            WarehouseName: $('#warehouseID option:selected').text(),
            WarehouseID: $('#warehouseID').val()
            //$('#StorerID option:selected').text()
        },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            if (data.IsSuccess == true) {
                $('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
            } else {
                $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            }
        },
        error: function (data, status, e) {
            WebPortal.MessageMask.Close();
            $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
        }
    });
};
function WorkersAlloctions(id, self) {
    layer.confirm('<font size="4">确认是否现场分配？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            url: "/WMS/PreOrder/WorkersAlloctions",
            type: "POST",
            dataType: "text",
            data: {
                ids: id,
            },
            success: function (data) {
                var StrHtml = JSON.parse(data)
                if (StrHtml.data == '操作失败') {
                    showMsg('操作失败，分配可能没有进行！', 4000);
                } else {
                    if (StrHtml.data[0].Type == "1") {
                        var html = $("#Evaluation2").render(StrHtml.data);
                        $("#DisInfoBody2")["empty"]();
                        $("#DisInfoBody2").append(html);
                        //openPopup("panel", true, 400, 400, null, 'DisInfo2', true);
                        //$("#popupLayer_panel")[0].style.top = "200px";
                        layer.open({
                            title: '缺货信息',
                            type: 1,
                            skin: 'layui-layer-rim', //加上边框
                            area: ['450px', '400px'], //宽高
                            content: $("#DisInfo2")[0].innerHTML
                        });
                    }
                    else {
                        var html = $("#Evaluation").render(StrHtml.data);
                        $("#DisInfoBody")["empty"]();
                        $("#DisInfoBody").append(html);
                        openPopup("panel", true, 400, 400, null, 'DisInfo', true);
                        $("#popupLayer_panel")[0].style.top = "200px";
                    }
                    //for (var i = 0; i < StrHtml.data.length; i++) {
                    //    for (var j = 0; j < checkBoxs.length; j++) {
                    //        if (checkBoxs[j].dataset.id == StrHtml.data[i].POID) {
                    $(self).parent().parent().parent().children()[6].innerHTML = '<font size="3" color="red">' + status[StrHtml.data[0].Note] + '</font>';
                    if (StrHtml.data[0].Note == '9') {
                        $(self).parent()[0].innerHTML = '<label data-name="' + StrHtml.data[0].POID + '" style="cursor: pointer; color:white" class="label label-info">查看出库单</label>';
                    }
                    //        }
                    //    }
                    //}
                }
                //showMsg(data, "3000");
                //location.reload();
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
            }
        });
    });
}
function Automatedoutbound(id, self) {
    layer.confirm('<font size="4">确认是否自动分配？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            url: "/WMS/PreOrder/AutomaticAllocation",
            type: "POST",
            dataType: "text",
            data: {
                ids: id,
            },
            success: function (data) {
                var StrHtml = JSON.parse(data)
                if (StrHtml.data == '操作失败') {
                    showMsg('操作失败，分配可能没有进行！', 4000);
                } else {
                    if (StrHtml.data[0].Type == "1") {
                        var html = $("#Evaluation2").render(StrHtml.data);
                        $("#DisInfoBody2")["empty"]();
                        $("#DisInfoBody2").append(html);
                        //if ($("#DisInfo2")[0].style.display == 'none') {
                        //    $("#DisInfo2")[0].style.display = 'block';
                        //}
                        //alert($("#DisInfo2")[0].innerHTML);
                        layer.open({
                            title: '缺货信息',
                            type: 1,
                            skin: 'layui-layer-rim', //加上边框
                            area: ['450px', '400px'], //宽高
                            content: $("#DisInfo2")[0].innerHTML
                        });

                        //openPopup("panel", true, 400, 400, null, 'DisInfo2', true);
                        //$("#popupLayer_panel")[0].style.top = "200px";
                    }
                    else {
                        var html = $("#Evaluation").render(StrHtml.data);
                        $("#DisInfoBody")["empty"]();
                        $("#DisInfoBody").append(html);
                        openPopup("panel", true, 400, 400, null, 'DisInfo', true);
                        $("#popupLayer_panel")[0].style.top = "200px";
                    }
                    //for (var i = 0; i < StrHtml.data.length; i++) {
                    //    for (var j = 0; j < checkBoxs.length; j++) {
                    //        if (checkBoxs[j].dataset.id == StrHtml.data[i].POID) {
                    $(self).parent().parent().parent().children(".Status")[0].innerHTML = '<font size="3" color="red">' + status[StrHtml.data[0].Note] + '</font>';
                    if (StrHtml.data[0].Note == '9') {
                        $(self).parent()[0].innerHTML = '<label data-name="' + StrHtml.data[0].POID + '" style="cursor: pointer; color:white" class="label label-info">查看出库单</label>';
                    }
                    //        }
                    //    }
                    //}
                }
                //showMsg(data, "3000");
                //location.reload();
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
            }
        });
    });

}
function Cancel(Id, CustomerID) {
    layer.confirm('<font size="4">确认是否取消？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        if ($("#check" + Id)[0].dataset.status > 3) {
            showMsg("此状态不能取消！", 2500);
            return false;
        }
        $.ajax({
            url: "/WMS/PreOrder/Cancel",
            type: "POST",
            dataType: "text",
            data: {
                ids: Id,
                CustomerID: CustomerID
            },
            success: function (data) {
                showMsg("操作成功", "4000");
                location.href = "/WMS/PreOrder/Index";
                //location.reload();
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
            }
        });
    });

}

function Complete(Id, self) {
    layer.confirm('<font size="4">确认是否完成？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            url: "/WMS/PreOrder/OrderFinish",
            type: "POST",
            dataType: "json",
            data: {
                ids: Id,
                //CustomerID: self
            },
            success: function (data) {
                if (data.Code == 1) {
                    showMsg("操作成功", "4000");
                    location.href = "/WMS/PreOrder/Index";
                    //location.reload();
                } else {
                    showMsg("操作失败", "4000");
                }
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
            }
        });
    });

}

$("#Cancel").live('click', (function () {
    layer.confirm('<font size="4">确认是否批量取消？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        var ids = "";
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                if (checkBoxs[i].dataset.status > 3) {
                    showMsg("有运单已经分配,请重新筛选！", 2500);
                    return false;
                }
                if (checkBoxs[i].dataset.status != -1) {
                    ids += "" + checkBoxs[i].dataset.id.toString() + ",";
                }
            }
        }
        if (ids.length > 0) {
            ids = ids.toString().substring(0, ids.toString().length - 1);

        }
        else {
            showMsg("请选择要取消的订单！", 4000);
            return false;
        }
        $.ajax({
            url: "/WMS/PreOrder/Cancel",
            type: "POST",
            dataType: "text",
            data: {
                ids: ids,
            },
            success: function (data) {
                showMsg("操作成功", "4000");
                location.href = "/WMS/PreOrder/Index";
                //location.reload();
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
            }
        });
        $('#SearchCondition_City').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/WMS/PreOrder/GetCity",
                    type: "POST",
                    dataType: "json",
                    data: {
                        find: "City",
                        Province: $('#SearchCondition_Province').val(),
                        City: $('#SearchCondition_City').val(),
                        //District: $('#SearchCondition_District').val()
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Text, value: item.Text, data: item }
                        }));
                    }
                });
            },
        });
        $('#SearchCondition_District').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/WMS/PreOrder/GetCity",
                    type: "POST",
                    dataType: "json",
                    data: {
                        find: "District",
                        City: $('#SearchCondition_City').val(),
                        District: $('#SearchCondition_District').val()
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Text, value: item.Text, data: item }
                        }));
                    }
                });
            },
        });
    });

}));


function Edits(ID) {
    location.href = "/WMS/PreOrder/PreOrderCreateOrEdit/?ID=" + ID + "&ViewType=2"
}

function ManuAlloctions(ID) {
    location.href = "/WMS/PreOrder/ManualAllocation/?ID=" + ID + "&ShowSubmit=" + $("#ShowSubmit").val()
}
function AssignedAllocation(ID) {
    location.href = "/WMS/PreOrder/AssignedAllocation/?ID=" + ID + "&ShowSubmit=" + $("#ShowSubmit").val()
}
//function ShowsIn(ID, obj) {
//    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
//        $(".ddiv:not(obj)").animate({
//            width: "hide",
//            width: "590%",
//            paddingRight: "hide",
//            paddingLeft: "hide",
//            marginRight: "hide",
//            marginLeft: "hide"

//        }, 100);
//        $("#operateTD" + ID).animate({
//            width: "show",
//            width: "625%",
//            paddingRight: "show",
//            paddingLeft: "show",
//            marginRight: "show",
//            marginLeft: "show"
//        });
//    }
//    //$("#operateTD" + ID)[0].style.display = "";
//}

//function ShowsOut() {
//    //$("#operateTD" + ID).fadeOut("slow");

//    $(".ddiv").animate({
//        width: "hide",
//        width: "590%",
//        paddingRight: "hide",
//        paddingLeft: "hide",
//        marginRight: "hide",
//        marginLeft: "hide"

//    }, 100);
//    //$("#operateTD" + ID)[0].style.display = "";
//}
function print(id) {
    layer.confirm('<font size="4">确认是否打印拣货单？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        if ($("#ProjectName").val() == "延锋百利得") {
            window.location.href = '/WMS/OrderManagement/PrintPreOrderYFBLD?id=' + id + "&Flag=2";
        }
        else {
            window.location.href = '/WMS/OrderManagement/PrintPreOrder?id=' + id + "&Flag=2";
        }
    });

}

function ViewOrderDetail(ID, CustomerID) {
    $.ajax({
        url: "/WMS/PreOrder/CheckOutboundOrder",
        type: "POST",
        dataType: "json",
        data: {
            Id: ID
            //CustomerID: self
        },
        success: function (data) {
            if (data.ErrorCode == 1) {
                if (data.OrderInfo.length == 1) {
                    window.location = "/WMS/OrderManagement/OrderDetailView/?ID=" + data.OrderInfo[0].ID + "&ViewType=3&CustomerID=" + CustomerID;
                } else {
                    var html = $("#CheckOutboundOrderList").render(data.OrderInfo);
                    $("#CheckOutboundOrderBody")["empty"]();
                    $("#CheckOutboundOrderBody").append(html);
                    openPopup("panel", true, 400, 400, null, 'CheckOutboundOrder', true);
                    $("#popupLayer_panel")[0].style.top = "200px";
                }
            } else {
                showMsg("没有生成出库单", 4000);
            }
            //showMsg("操作成功!", "4000");
            //location.reload();
        },
        error: function (msg) {
            showMsg("操作失败", "4000");
        }
    });

}
