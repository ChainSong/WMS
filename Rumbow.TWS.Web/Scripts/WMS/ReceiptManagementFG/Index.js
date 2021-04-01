var txt = "确认是否打印上架单";
$(document).ready(function () {
    if ($('#SearchCondition_CustomerID').val() == "") {
        $('#SearchCondition_CustomerID option:first').next().attr("selected", "selected");

    } else {

    }
    if (sessionStorage.getItem("WmsUserName") == "SHDZ01") {
        $("#statusBack").show();
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
    if ($("#resultTable tbody")[0].rows.length < 1) {
        $("#resultTable").removeAttr("style");
        $("#tables").removeAttr("style");
        $("#OperateTh").removeAttr("style");
    }
    $("#ReceiptSend").live('click', function () {
        if ($("#resultTable tbody input[type='checkbox']:checked").size() == 0) {//加了一个判断
            showMsg("请选择单据！", "4000");
            return;
        }
        layer.confirm('<font size="4">确认是否批量推送入库单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var jsonStr = TableToJsonJC();
            
            $.ajax({
                url: "/WMS/ReceiptManagementFG/ReceiptSend",
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

    //发送上架任务
    $("#ReceiptTask").live("click", function () {
        layer.confirm('<font size="4">确认是否下发任务？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var id = TableToJson();
            // var isPrint = PrintStatus();
            if (id == "") {
                showMsg("请选择需要下发的订单！", 4000);
                return false;
            }
            else {
                $.ajax({
                    type: "Post",
                    url: "/WMS/ReceiptManagementFG/ReceiptTask",
                    data: {
                        "ID": id
                    },
                    async: "false",
                    success: function (data) {
                        if (data == "") {
                            layer.msg("订单上架任务下发成功");
                        }
                        else {
                            layer.msg("订单上架任务下发失败");
                        }
                    },
                    error: function (msg) {
                        alert(msg.val);
                    }

                });
            }
        });
    });
    $(function ($) {
        $('body').click(function () {
            ShowsOut()
        });
    });
    $("#printButton").live("click", function () {
        layer.confirm('<font size="4">确认是否批量打印上架单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            if ($("#resultTable tbody input[type='checkbox']:checked").size() == 0) {//加了一个判断，当用户没有选择要打印订单时,提示 20170807
                showMsg("请选择单据！", "4000");
                return;
            }
            var str = '';
            $('.checkForSelect').each(function (index) {
                if ($(this).attr('checked') === 'checked') {
                    //str += "'" + $(this).attr('data-id') + "'" + ",";
                    str += "" + $(this).attr('data-id') + "" + ",";
                }
            })
            if ($("#ProjectName").val() == "NIKE") {
                location.href = '/WMS/ReceiptManagementFG/PrintShelvesNike?rid=' + str.substring(0, str.length - 1) + "&Flag=1";
            }
            else if ($("#ProjectName").val() == "YXDR") {
                location.href = '/WMS/ReceiptManagementFG/PrintShelvesYXDR?rid=' + str.substring(0, str.length - 1) + "&Flag=1";
            }
            else if ($("#ProjectName").val() == "延锋百利得") {
                location.href = '/WMS/ReceiptManagementFG/PrintShelvesYFBLD?rid=' + str.substring(0, str.length - 1) + "&Flag=1";
            }
            else if ($("#ProjectName").val() == "Mono" || $("#ProjectName").val() == "吉特") {
                location.href = '/WMS/ReceiptManagementFG/PrintShelves_JT?rid=' + str.substring(0, str.length - 1) + "&Flag=1";
            }
            else {
                location.href = '/WMS/ReceiptManagementFG/PrintShelves_JT?rid=' + str.substring(0, str.length - 1) + "&Flag=1";
            }
        });

        // }
        //$.ajax({
        //    type: 'GET',
        //    url: '/WMS/ReceiptManagementFG/PrintShelves',
        //    data: { id: "@ViewBag.Id" },
        //    cache: false,

        //    success: function (data) {
        //        var a = JSON.parse(data);
        //        var html = $('#Evaluation').render(a);
        //        $('#PrintArea')['empty']();
        //        $('#PrintArea').append(html);
        //    }, error: function (err) {
        //        alert("错误！");
        //    }
        //});
        //$.ajax({
        //    type: "GET",
        //    url: "/WMS/ReceiptManagementFG/PrintShelves",
        //    data: { rid: str },
        //    cache: false,
        //    success: function (data)
        //    {
        //        //if (data != "")
        //        //{
        //        //    location.href = '/WMS/ReceiptManagementFG/PrintShelves'
        //        //}
        //    }, error: function (msg)
        //    {
        //        alert("");
        //    }
        //});
    });


    $('#exportButtom').live('click', function () {
        var a = TableToJson();
        if (a == "") {
            return true;
        }
        else {
            window.location.href = "/WMS/ReceiptManagementFG/ExportOrder?IDs=" + a + "&CustomerID=" + $("#SearchCondition_CustomerID").val();
            //$.ajax({
            //    url: "/WMS/OrderManagement/ExportOrder",
            //    type: "POST",
            //    async: false,
            //    data: {
            //        IDs: a,
            //        CustomerID: $("#CustomerID").val()
            //    },
            //    success: function (data) {
            //        alert(2);
            //    },
            //    error: function (msg) {
            //        alert(msg);
            //    }
            //});
            //$.ajax({
            //    url: "/WMS/OrderManagement/ExportOrder",
            //    type: "POST",
            //    dataType: "json",
            //    data: {
            //        IDs: a,
            //         CustomerID: $("#CustomerID").val()
            //    },
            //    success: function (data) {
            //        response($.map(data, function (item) {
            //            return { label: item.Text, value: item.Text, data: item }
            //        }));
            //    }
            //});
            return false;
        }
    });
    $('#exportButton').live('click', function () {
        var a = TableToJson();
        if (a == "") {
            return true;
        }
        else {
            window.location.href = "/WMS/ReceiptManagementFG/ExportShelves?IDs=" + a + "&CustomerID=" + $("#SearchCondition_CustomerID").val() + "&type=1";
            return false;
        }
    });
    $('#exportShelvesButton').live('click', function () {
        var a = TableToJson();
        if (a == "") {
            return true;
        }
        else {
            window.location.href = "/WMS/ReceiptManagementFG/ExportShelves?IDs=" + a + "&CustomerID=" + $("#SearchCondition_CustomerID").val() + "&type=2";
            return false;
        }
    });

    $('#searchButton').click(function () {
        setPageControlVal();
    });


    $('.DynamicCalendarRange').each(function (index) {
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
        $(this).val($('#' + descID).val());


    });
    var setHiddenValToControl = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id;
            if ($(this).attr("type") === "checkbox") {
                if ($('#' + descId).val() === '1') {
                    $(this).attr('checked', 'checked');
                } else {
                    $(this).removeAttr('checked');
                }

            } else if ($(this).attr("type") === "DropDownList") {
                var desc = $('#' + descId);
                if (desc.val() === '1' || desc.val() === 'Y' || desc.val === 'y' || desc.val() === '是') {
                    $(this).val('1');
                } else {
                    $(this).val('0');
                }

            } else {
                $(this).val($('#' + descId).val());
            }
        });
    }

    $('.DropDownList').each(function (index) {
        var id = $(this).attr("id");
        var descId = "SearchCondition_" + id.substr(0, id.length - 4) + "ID";
        $(this).val($('#' + descId).val());
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

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

    });
    $("#resultTable tbody input[type='checkbox']").live('click', function () {
        RefreshIDs();
    });
    setHiddenValToControl();


    $("#statusBackOK").live('click', function () {
        if ($('#backStatusid').val() == "") {
            //alert("请选择要回退的状态");
            showMsg("请选择要回退的状态", "4000");
            return;
        }

        $.ajax({

            url: "/WMS/ReceiptManagementFG/BackStatus",
            type: "POST",
            dataType: "text",
            data: {
                ID: $('#StatusbackID').val().toString(),
                ToStatus: $('#backStatusid').val().toString(),
            },

            //async: "false",
            success: function (data) {
                if (data == "") {
                    showMsg("回退成功！", "4000");
                    location.href = "/WMS/ReceiptManagementFG/Index"
                    //$('#BackReturn')[0].innerText = "回退成功！"
                    //openPopup("pop33", true, 350, 200, null, 'BackStatusReturnDiv', true)
                }
                else {
                    showMsg("回退失败！失败单号：" + data, "4000");
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });



});



function TableToJson() {
    var a = "";

    var checkBoxs = $("#resultTable tbody input[type='checkbox']");

    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            a += checkBoxs[i].name + ",";
        }
    }

    a = a.substring(0, a.length - 1);


    return a;
}

//if ($(this).attr("checked") == "checked" && $(this).id.toString() != "selectAll") {
//    a = document.getElementsByName($(this).name);
//}

//var table = document.getElementById('resultTable');
//var tr = table.getElementsByTagName('tr');
//for (var i = 1; i < tr.length; i++) {

//    var chks=document.getElementsByTagName('input');

//    for (var i = 0; i < chks.length; i++) {
//    }

//    var id = tr[i].id;



//}

function TableToJsonJC()
{
    var txt = "{\"jCRequestLists\":[";
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            var r = "{";
            r += "\"ID\":\"" + checkBoxs[i].dataset.id + "\",";
            r += "\"CustomerID\":\"" + checkBoxs[i].dataset.customerid + "\",";
            r += "\"WarehouseID\":\"" + checkBoxs[i].dataset.warehouseid + "\","; 
            r += "\"RelateNumber\":\"" + checkBoxs[i].dataset.receiptnumber + "\",";
            r = r.substring(0, r.length - 1)
            r += "},";
            txt += r;
        }
    }
    txt = txt.substring(0, txt.length - 1);
    txt += "]}";
    return txt;
}



function ReceiptDelete(ReceiptNumber) {

    if (window.confirm('您确认取消此入库单？')) {
        if (ReceiptNumber != "") {

            $.ajax({
                type: "POST",
                url: "/WMS/ReceiptManagementFG/ReceiptDelete",
                data: {
                    "ReceiptNumber": ReceiptNumber,
                },
                async: "false",
                success: function (data) {

                    var js = data;
                    location.href = "/WMS/ReceiptManagementFG/Index";
                    //window.location.reload();
                    if (js == "StatusWarning") {
                        alert("当前状态不允许取消，请退回到1状态！");
                    }
                },
                error: function (msg) {
                    alert(msg.val);
                }

            });
        }
    }

}

var fileImportClick = function () {
    if ($('#importExcel').val() === '') {
        //Runbow.TWS.Alert("请选择要导入的Excel");
        showMsg("请选择要导入的Excel", "4000");
        return false;
    }
    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        //Runbow.TWS.Alert("请选择Excel格式的文件");
        showMsg("请选择Excel格式的文件", "4000");
        //$('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    };
    WebPortal.MessageMask.Show("导入中...");

    $.ajaxFileUpload({
        url: '/WMS/ReceiptManagementFG/ImportReceiptUpdate_Batch',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: {},
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            $('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
        },
        error: function (data, status, e) {
            $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            WebPortal.MessageMask.Close();
        }
    });
    return false;
};
$('select[id=SearchCondition_CustomerID]').live('change', function () {
    window.location.href = "/WMS/ReceiptManagementFG/Index/?customerID=" + $(this).val();
});

$("#addButton").live('click', function () {
    var CustomerID = $('#SearchCondition_CustomerID option:selected').val();
    location.href = "/WMS/ReceiptManagementFG/ReceiptCreate/?ViewType=0&CustomerID=" + CustomerID
});

function editButton(ID, CustomerID) {
    location.href = "/WMS/ReceiptManagementFG/ReceiptCreate/?ID=" + ID + "&ViewType=2&PageType=2&CustomerID=" + CustomerID
}

function BackCloseBox(ExternNumber) {
    layer.confirm('<font size="4">确认是否回退装箱数据？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
            layer.close(index);
            $.ajax({
                type: "POST",
                url: "/WMS/ReceiptManagementFG/BackCloseBox",
                data: {
                    "ExternReceiptNumber": ExternNumber
                },
                async: "false",
                success: function (data) {
                    
                    if (data == "") {
                        layer.confirm('回退成功！', {
                            btn: ['确定'] //按钮
                        }, function () {
                            location.href = "/WMS/ReceiptManagementFG/Index";
                        });
                    } else {
                        showMsg("失败：" + data, 3000);
                    }
                },
                error: function (msg) {
                    alert(msg.val);
                }
            });

    })

}

//点击上架按钮
function Shelves(ID, type) {
    $.ajax({
        type: "Post",
        url: "/WMS/ReceiptManagementFG/ValidOrderCancel",
        data: {
            "OrderNumber": ID.toString(),
            "customerID": $('#SearchCondition_CustomerID').val(),
            "warehouse": $('#SearchCondition_WarehouseID').val(),
            "type": 6
        },
        async: "false",
        success: function (data) {
            if (data == "该订单已取消") {
                layer.confirm('订单已取消！', {
                    btn: ['确定'] //按钮
                });
                return;
            }
            else {
                //如果是爱库存那么直接上架加入库存
                if ($("#ProjectName").val() == "爱库存" && type == "快进快出") {
                    layer.confirm('<font size="4">此订单为快进快出订单，点击确定则加入库存？</font>', {
                        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                        //shade: [0.8, '#393D49'],
                        title: ['提示', 'font-size:18px;']
                        //按钮
                    }, function (index) {
                        layer.close(index);
                        //他点了上架，调个api弄他！
                        $.ajax({
                            type: "POST",
                            url: "/WMS/ReceiptManagementFG/AddInventoryAKC",
                            data: {
                                "ID": ID
                            },
                            async: "false",
                            success: function (res) {
                                var data = JSON.parse(res);
                                if (data.code == 0) {
                                    layer.confirm('上架加入库存成功！', {
                                        btn: ['确定'] //按钮
                                    }, function () {
                                        location.href = "/WMS/ReceiptManagementFG/Index";
                                    });
                                } else {
                                    showMsg("失败：" + data.message, 3000);
                                }
                            },
                            error: function (msg) {
                                alert(msg.val);
                            }
                        });
                    });
                } else {
                    location.href = "/WMS/ShelvesManagementFG/ReceiptReceivingDetail/?RID=" + ID
                }
            }
        },
        error: function (msg) {
            layer.confirm('订单取消验证报错，请重试！' + msg.val, {
                btn: ['确定'] //按钮
            });
            return;
        }
    })  ;
    
}

function statusBackClick(ID, Status) {
    $("#StatusbackID").val(ID);
    $('#backStatusid').children("span").children().unwrap();
    openPopup('popp', true, 350, 300, null, 'statusBackDiv');
    $("#popupLayer_popp")[0].style.top = "200px";
    for (var i = 0; i < $('#backStatusid').children().length; i++) {
        var a = $('#backStatusid').children()[i].value;
        if (a >= Status) {
            $('#backStatusid').children('option[value=' + a + ']').wrap('<span>').hide();
        }
    }
    //$('#backStatusid').children('option[value="-1"]').wrap('<span>').hide();
    //$('#backStatusid').find("option:selected").unwrap();    
}

$("#statusBackReturn").live('click', function () {
    closePopup();
});

//更新入库单体积弹框
function UpdateVolume(ID) {   
    if (ID) {
        $("#tdVolume").val('');
        $("#UpdateReceiptID").val(ID);
        openPopup('UpdateVolume', true, 350, 300, null, 'UpdateVolumeDiv');
        $("#popupLayer_UpdateVolume")[0].style.top = "200px";
      
    }
}
//取消更新
$("#UpdateVolumeOKCancel").live('click', function () {
    closePopup();
});

//更新体积
$("#UpdateVolumeOK").live('click', function () {
   
    if ($('#UpdateReceiptID').val() == "") {       
        showMsg("请先选择订单", "4000");
        return;
    }
    if ($("#tdVolume").val() == '') {
        showMsg("请输入总体积", 3000);
        return;
    }
    $.ajax({
        url: "/WMS/ReceiptManagementFG/UpdateReceiptVolume",
        type: "POST",
        dataType: "text",
        data: {
            ID: $('#UpdateReceiptID').val(),
            Volume: $('#tdVolume').val()
        },
        //async: "false",
        success: function (res) {
            var data = JSON.parse(res);
            if (data.code == 0) {
                layer.confirm('体积更新成功！', {
                    btn: ['确定'] //按钮
                }, function () {
                    location.href = "/WMS/ReceiptManagementFG/Index";
                });               
                        
            }
            else {
                showMsg("更新失败：" + data.message, "4000");
            }
        },
        error: function (msg) {
            alert(msg.val);
        }
    });
});

function changeNum(obj) {
    //如果用户第一位输入的是小数点，则重置输入框内容
    if (obj.value != '' && obj.value.substr(0, 1) == '.') {
        obj.value = "";
    }
    obj.value = obj.value.replace(/^0*(0\.|[1-9])/, '$1');//粘贴不生效
    obj.value = obj.value.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符
    obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
    obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');//只能输入两个小数
    if (obj.value.indexOf(".") < 0 && obj.value != "") {//以上已经过滤，此处控制的是如果没有小数点，首位不能为类似于 01、02的金额
        if (obj.value.substr(0, 1) == '0' && obj.value.length == 2) {
            obj.value = obj.value.substr(1, obj.value.length);
        }
    }
}

//function clearNoNum(obj) {
//    obj.value = obj.value.replace(/[^\d.]/g, "");//清除“数字”和“.”以外的字符
//    obj.value = obj.value.replace(/^\./g, "");//验证第一个字符是数字而不是.
//    obj.value = obj.value.replace(/\.{2,}/g, ".");//只保留第一个. 清除多余的.
//    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
//}

$("#statusBack").live('click', function () {
    let isRetrun = true;//是否有已入库状态的订单，已入库状态不允许回退   lrg  
    var checkBoxs = $("#resultTable tbody input[type='checkbox']:checked");
    var sql = "";
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            sql += checkBoxs[i].name.toString() + ",";

        }
        if ($(checkBoxs[i]).attr("data-Status") == "9") {
            isRetrun = false;
        }
    }
    if (!isRetrun) {
        showMsg("勾选的订单中存在已入库的订单，不允许回退，请检查！", 3000);
        return;
    }
    if (sql.length > 0) {

        $("#statusBack").popover('destroy');
        $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
        openPopup("pop11", true, 350, 300, null, 'statusBackDiv', true);
        $("#popupLayer_pop11")[0].style.top = "200px";
    }
    else {
        //Runbow.TWS.Alert("请勾选入库单！");
        //$("#statusBack").popover('show');
        showMsg("请勾选入库单！", 4000);

    }

});

var RefreshIDs = function () {
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    var length = checkBoxs.length;
    //var IDs = [];
    var checked = 0;
    checkBoxs.each(function () {
        if ($(this).attr("checked") === "checked") {
            //var id = { ID: $(this).attr("data-ID") };
            //IDs.push(id);
            checked++;
        }
    });

    if (checked == checkBoxs.length) {
        $('#selectAll').attr("checked", "checked");
    } else {
        $('#selectAll').removeAttr("checked");
    }

    //$('#SelectedIDs').val(JSON.stringify(IDs));
}


function printPick(ID) {
    layer.confirm('<font size="4">确认是否打印上架单？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        if ($("#ProjectName").val() == "NIKE") {
            location.href = '/WMS/ReceiptManagementFG/PrintShelvesNike?rid=' + ID + "&Flag=1";
        }
        else if ($("#ProjectName").val() == "YXDR") {
            location.href = '/WMS/ReceiptManagementFG/PrintShelvesYXDR?rid=' + ID + "&Flag=1";
        }
        else if ($("#ProjectName").val() == "延锋百利得") {
            location.href = '/WMS/ReceiptManagementFG/PrintShelvesYFBLD?rid=' + ID + "&Flag=1";
        }
        else if ($("#ProjectName").val() == "Mono" || $("#ProjectName").val() == "吉特") {
            location.href = '/WMS/ReceiptManagementFG/PrintShelves_JT?rid=' + ID + "&Flag=1";
        }
        else {
            location.href = '/WMS/ReceiptManagementFG/PrintShelves_JT?rid=' + ID + "&Flag=1";
        }
    });
}


function ShowsIn(ID, obj) {
    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
        $(".ddiv:not(obj)").animate({
            width: "hide",
            width: "400%",
            paddingRight: "hide",
            paddingLeft: "hide",
            marginRight: "hide",
            marginLeft: "hide"

        }, 100);
        $("#operateTD" + ID).animate({
            width: "show",
            width: "445%",
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
        width: "400%",
        paddingRight: "hide",
        paddingLeft: "hide",
        marginRight: "hide",
        marginLeft: "hide"

    }, 100);
    //$("#operateTD" + ID)[0].style.display = "";
}

function GenerateBarCode(receiptId) {
    //document.URL = '/WMS/ReceiptManagementFG/GenerateBarCode?ID=' + receiptId + "&ViewType=0";
    window.location.href = '/WMS/ReceiptManagementFG/GenerateBarCode?ID=' + receiptId + "&ViewType=0";
}
function ScanBarCode(receiptId) {
    window.location.href = '/WMS/ReceiptManagementFG/ScanBarCode?ID=' + receiptId + "&ViewType=0";
}