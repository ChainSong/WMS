$(document).ready(function () {
    if ($('#Condition_CustomerID').val() == "") {
        $('#Condition_CustomerID option:first').next().attr("selected", "selected");

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
    $('select[id=Condition_CustomerID]').live('change', function () {
        var hiddenActionButton = $('#HideActionButton').val();
        var showEditRelated = $('#ShowEditRelated').val();
        window.location.href = "/WMS/ShelvesManagementFG/Index/?customerID=" + $(this).val() + "&hideActionButton=" + hiddenActionButton + "&showEditRelated=" + showEditRelated;
    });
    $("#portButtonTemplet").click(function () {
        demo();
    })
    //打印
    $("#printButton").live("click", function () {
        layer.confirm('<font size="4">是否打印上架单？</font>', {
            btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var str = '';
            $('.checkForSelect').each(function (index) {
                if ($(this).attr('checked') === 'checked') {
                    str += "'" + $(this).attr('data-id') + "'" + ",";
                }
            })
            if (str.length > 0)
            {
                location.href = '/WMS/ReceiptManagement/PrintShelves?rid=' + str.substring(0, str.length - 1) + "&Flag=2";
            } else
            {
                showMsg("请至少勾选一单！", 4000);
            }
        });
    });
    $("#intelligentDispatch").live('click', function () {
       openPopup("", true, 350, 300, null, 'intelligentDispatchPanel', true);
    });
    $("#intelligentDispatchRT").live('click', function () {
        closePopup();
    });
    $("#intelligentDispatchOK").live('click', function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked && $(checkBoxs[i]).data().status == 5) {
                sql += checkBoxs[i].name.toString() + ",";
            }
        }
        if (sql.length == 0) {
            showMsg("请选择订单", 4000);
            return false;
        }
        if ($('#WorkStation').val() == "") {
            showMsg("请选择操作台", 4000);
            return;
        }
        $.ajax({
            type: "post",
            url: "/WMS/ShelvesManagementFG/AddInstructions",
            data: {
                "ids": sql,
                "WorkStation": $('#WorkStation').val(),
                "WarehouseQueue": $('#WarehouseQueue').val(),
                "Priority": $('#Priority').val()
            },
            success: function (data) {
                if (data.Code == "1") {
                    closePopup();
                    showMsg("发送成功！", 4000);
                }
                else {
                    showMsg("发送失败！" + data.Message, 4000);
                }
            },
            error: function (msg) {
                alert(msg.val);
            }
        });
    });
    $('#ExeclShelvesSingle').live('click', function () {
        var a = TableToJson();
        if (a == "") {
            return true;
        }
        else {
            window.location.href = "/WMS/ShelvesManagementFG/ExportReceiptReceving?IDs=" + a + "&CustomerID=" + $("#SearchCondition_CustomerID").val() + "&type=1";
            return false;
        }
    });
    function demo() {
        var ID = $('select[id=StorerID]')[0].value;
        var form = $("<form>");
        form.attr('style', 'display:none');
        form.attr('target', '');
        form.attr('method', 'post}');
        form.attr('action', '/WMS/ShelvesManagementFG/ExportReceiptReceiving');
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
    $('#searchButton').click(function () {
        setPageControlVal();
    });
    var setPageControlVal = function () {
        $('.calendarRangeReWrite').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'Condition_';
            if (actualID === 'ShelvesTime') {
                if (pref === 'start') {
                    descID += 'StartShelvesTime';
                }
                else {
                    descID += 'EndShelvesTime';
                }
            } else if (actualID === 'CreateTime') {
                if (pref === 'start') {
                    descID += 'StartCreateTime';
                }
                else {
                    descID += 'EndCreateTime';
                }
            }
            else {
                if (pref === 'start') {
                    descID += 'StartStorageTime';
                }
                else {
                    descID += 'EndStorageTime';
                }
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
});
var fileImportClick = function () {
    if ($('#importExcel').val() === '') {
        showMsg("请选择要导入的Excel", 4000)
        return false;
    }
    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        showMsg("请选择Excel格式的文件", 4000)
        $(this).before(fileImport);
        return false;
    };
    WebPortal.MessageMask.Show("导入中...");
    $.ajaxFileUpload({
        url: "/WMS/ShelvesManagementFG/ImputEcecl",
        secureuri: false,
        fileElementId: 'importExcel',
        type: "POST",
        dataType: "json",
        data: {
            CustomerID: $('#StorerID').val(),
            Customer: $('#StorerID option:selected').text()
        },
        success: function (data, status) {
            //sleep(10000);  //睡眠5秒
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
function sleep(numberMillis)
{
    var now = new Date();
    var exitTime = now.getTime() + numberMillis;
    while (true) {
        now = new Date();
        if (now.getTime() > exitTime) return;
    }
}
var status = '';
function statusBack(ReceiptKey, statuss) {
    status = ReceiptKey;
    openPopup("backPop", true, 350, 200, null, 'Abnormals', true);
    $("#popupLayer_backPop")[0].style.top = "200px";
}
$("#OpenStatusBack").live('click', function () {
    var sql = "";
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            sql += "" + checkBoxs[i].id.toString() + ",";
        }
    }
    if (sql.length > 0) {
        sql = sql.toString().substring(0, sql.toString().length - 1);
        openPopup("backpop2", true, 350, 200, null, 'Abnormals', true);
        $("#popupLayer_backpop2")[0].style.top = "200px";
    }
    else {
        showMsg("请至少勾选一单！", 4000);
    }
})
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
$("#statusBackReturn").live('click', function () {
    closePopup();
})
$("#statusBack").live('click', function () {
    if ($('#backStatusid').val() == "") {
        showMsg("请选择要回退的运单！", 4000)
        return;
    }
    var sql = "";
    if (status == '') {
            var checkBoxs = $("#resultTable tbody input[type='checkbox']");
            for (var i = 0; i < checkBoxs.length; i++) {
                if (checkBoxs[i].checked) {
                    sql += "" + checkBoxs[i].id.toString() + ",";
                }
            }
        if (sql.length > 0) {
            sql = sql.toString().substring(0, sql.toString().length - 1);
        }
    } else {
        sql = status;
    }
    if (sql == "") {
        showMsg("请选择要回退的运单！", 4000)
        return;
    }
    $.ajax({
        url: "/WMS/ShelvesManagementFG/BackStatus",
        type: "POST",
        data: {
            "ID": sql,
            "ToStatus": $('#backStatusid').val()
        },
        async: "false",
        success: function (data) {
            closePopup();
            if (data == '') {
                showMsg("回退成功！", 1000)
                setTimeout("window.location.href = '/WMS/ShelvesManagementFG/Index'", 1000);
            } else {
                showMsg("回退失败！" + data, 6000)
            }
            status = '';
        },
        error: function (msg) {
            showMsg("回退失败！", 4000)
        }
    });
});
$("#multiAddInvButton").live('click', function () {
    PLAddInventory();
});

function post(URL, PARAMS) {
    var temp = document.createElement("form");
    temp.action = URL;
    temp.method = "post";
    temp.style.display = "none";
    for (var x in PARAMS) {
        var opt = document.createElement("textarea");
        opt.name = x;
        opt.value = PARAMS[x];
        temp.appendChild(opt);
    }
    document.body.appendChild(temp);
    temp.submit();
    return temp;
}
//获取差异
function CheckReceiving(id, chktype) {
    var resualt = false;
    $.ajax({
        type: "POST",
        url: "/WMS/ShelvesManagementFG/CheckReceiptReceiving",
        data: {
            "id": id,
        },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.Code == 2) {
                showMsg("检查差异失败！", 4000);
            }
            else {
                if (data.data.length <= 0) {
                    resualt = true;
                    if (chktype == 1) {
                    }
                    else {
                        showMsg("检查差异完成，无差异信息！", 4000);
                    }
                }
                else {
                    if (chktype == 1) {
                    }
                    else {
                        var html = $("#CheckDifference").render(data.data);
                        //页面层
                        layer.open({
                            type: 1,
                            skin: 'layui-layer-rim', //加上边框
                            area: ['800px', '800px'], //宽高
                            content: showmsg(html)
                        });
                    }
                }
            }
        },
        error: function (msg) {
            showMsg("检查差异失败！", 4000);
        }
    });
    return resualt;
}
//查看RF上架进展
function CheckNoReceiving(id, chktype) {
    var resualt = false;
    $.ajax({
        type: "POST",
        url: "/WMS/ShelvesManagementFG/CheckRFReceiptReceiving",
        data: {
            "id": id,
        },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.Code == 2) {
                showMsg("检查差异失败！", 4000);
            }
            else {
                if (data.data.length <= 0) {
                    resualt = true;
                    if (chktype == 1) {
                    }
                    else {
                        showMsg("检查差异完成，无差异信息！", 4000);
                    }
                }
                else {
                    if (chktype == 1) {
                    }
                    else {
                        var html = $("#CheckRFDifference").render(data.data);
                        //页面层
                        layer.open({
                            type: 1,
                            skin: 'layui-layer-rim', //加上边框
                            area: ['800px', '600px'], //宽高
                            content: showmsgRF(html)
                        });
                    }
                }
            }
        },
        error: function (msg) {
            showMsg("检查差异失败！", 4000);
        }
    });
    return resualt;
}
//导出
function ExportDiffrent(id, chktype) {
    var resualt = false;
    window.location.href = "/WMS/ShelvesManagementFG/ExportDiffrent?id=" + id;
}
//显示结果
function showmsg(html) {
    return dataTable = " <table>\
        <thead>\
            <tr>\
                <th>SKU</th>\
                <th>订单数量</th>\
                <th>上架数量</th>\
                <th>差异数量</th>\
            </tr>\
        </thead>\
        <tbody id='CheckDifferencePopup'>" + html + "</tbody> </table>"
}
//显示结果
function plshowmsg(html) {
    return dataTable = " <table>\
        <thead>\
            <tr>\
                <th>系统单号</th>\
                <th>结果</th>\
            </tr>\
        </thead>\
        <tbody id='plCheckDifferencePopup'>" + html + "</tbody> </table>"
}
//显示结果
function showmsgRF(html) {
    return dataTable = " <table>\
        <thead>\
            <tr>\
                <th>箱号</th>\
                <th>SKU</th>\
                <th>订单数量</th>\
                <th>上架数量</th>\
                <th>差异数量</th>\
            </tr>\
        </thead>\
        <tbody id='CheckRFDifferencePopup'>" + html + "</tbody> </table>"
}
//关闭结果
function CloseDiv() {
    $("#showdata")[0].style.display = 'none';
    $("#showtable").html("");
}
function AddInventory(id, receiptnumber, oneself) {
    layer.confirm('<font size="4">是否加入库存？</font>', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        title: ['提示', 'font-size:18px;']
    }, function (index) {
        layer.close(index);
        if (CheckReceiving(id, 1)) {
            $.ajax({
                type: "POST",
                url: "/WMS/ShelvesManagementFG/AddInventory",
                data: {
                    "id": id,
                },
                async: "false",
                success: function (data) {
                    if (data == '成功') {
                        showMsg("加入库存成功！", 4000);
                        $(oneself).prev()[0].style.display = "none";
                        $(oneself)[0].style.display = "none";
                        $($(oneself).parent().parent().parent()).children(".Status")[0].innerText = "已入库"
                    } else {
                        showMsg(data, 4000);
                    }
                },
                error: function (msg) {
                    showMsg("加入库存失败，请检查收货是否匹配！", 4000);
                }
            });
        }
        else {
            if ($("#ProjectName").val() == "NIKE") {
                layer.confirm('<font size="4">存在差异，是否加入库存并生成冻结单？</font>', {
                    btn: ['是', '否'], icon: 3, area: ['300px', '200px'], shift: 0, closeBtn: 1,
                    title: ['提示', 'font-size:18px;']
                }, function (index) {
                    layer.close(index);
                    $.ajax({
                        type: "POST",
                        url: "/WMS/ShelvesManagementFG/AddInventoryWithFreeze", //Nike差异入库自动生成冻结单
                        data: {
                            "id": id,
                        },
                        async: "false",
                        success: function (data) {
                            if (data == '成功') {
                                showMsg("加入库存成功！", 4000);
                                $(oneself).prev()[0].style.display = "none";
                                $(oneself)[0].style.display = "none";
                                $($(oneself).parent().parent().parent()).children(".Status")[0].innerText = "已入库"
                            } else {
                                showMsg(data, 4000);
                            }
                        },
                        error: function (msg) {
                            showMsg("加入库存失败，请检查收货是否匹配！", 4000);
                        }
                    });
                });
            }
            else {
                layer.confirm('<font size="4">存在差异，是否加入库存？</font>', {
                    btn: ['是', '否'], icon: 3, area: ['300px', '200px'], shift: 0, closeBtn: 1,
                    title: ['提示', 'font-size:18px;']
                }, function (index) {
                    layer.close(index);
                    $.ajax({
                        type: "POST",
                        url: "/WMS/ShelvesManagementFG/AddInventory",
                        data: {
                            "id": id,
                        },
                        async: "false",
                        success: function (data) {
                            if (data == '成功') {
                                showMsg("加入库存成功！", 4000);
                                $(oneself).prev()[0].style.display = "none";
                                $(oneself)[0].style.display = "none";
                                $($(oneself).parent().parent().parent()).children(".Status")[0].innerText = "已入库"
                            } else {
                                showMsg(data, 4000);
                            }
                        },
                        error: function (msg) {
                            showMsg("加入库存失败，请检查收货是否匹配！", 4000);
                        }
                    });
                });
            }
        }
    });
}
//批量加入库存
function PLAddInventory() {
    layer.confirm('<font size="4">是否批量加入库存？</font>', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        title: ['提示', 'font-size:18px;']
    }, function (index) {
        layer.close(index);

        var id = "";
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var keyArray = [];
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {//
                id = checkBoxs[i].id.toString();
                $.ajax({
                    type: "POST",
                    url: "/WMS/ShelvesManagementFG/CheckReceiptReceiving",
                    data: {
                        "id": id,
                    },
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.Code == 2) {
                            keyArray.push({ id: id, mess: "检查差异失败" });
                        }
                        else {
                            if (data.data.length <= 0) {
                                //keyArray.push({ id: id, mess: "无差异" });
                            }
                            else {
                                keyArray.push({ id: id, mess: "有差异" });
                            }
                        }
                    },
                    error: function (msg) {
                        keyArray.push({ id: id, mess: "检查差异失败:" + msg });
                    }
                });
            }//
        }
        //批量检查差异没问题
        if (keyArray.length == 0) {
            var keyArray2 = [];
            for (var i = 0; i < checkBoxs.length; i++) {
                if (checkBoxs[i].checked) {//
                    id = checkBoxs[i].id.toString();
                    $.ajax({
                        type: "POST",
                        url: "/WMS/ShelvesManagementFG/AddInventory",
                        data: {
                            "id": id,
                        },
                        async: "false",
                        success: function (data) {
                            if (data == '成功') {
                                keyArray2.push({ id: id, mess: "加入库存成功" });
                                $(oneself).prev()[0].style.display = "none";
                                $(oneself)[0].style.display = "none";
                                $($(oneself).parent().parent().parent()).children(".Status")[0].innerText = "已入库"
                            } else {
                                keyArray2.push({ id: id, mess: "加入库存失败：" + data });
                            }
                        },
                        error: function (msg) {
                            keyArray2.push({ id: id, mess: "加入库存失败：" + msg });
                        }
                    });
                }//
            }
            var html = $("#CheckOrderDifference").render(keyArray2);
            //页面层
            layer.open({
                type: 1,
                skin: 'layui-layer-rim', //加上边框
                area: ['800px', '800px'], //宽高
                content: plshowmsg(html)
            });
        } else {
            var html = $("#CheckOrderDifference").render(keyArray);
            //页面层
            layer.open({
                type: 1,
                skin: 'layui-layer-rim', //加上边框
                area: ['800px', '800px'], //宽高
                content: plshowmsg(html)
            });
        }
    });
}
function ShelvesLink(ShelvesLink, Status) {
    window.location.href = "/WMS/ShelvesManagementFG/ReceiptReceivingDetail/?RID=" + ShelvesLink + "&Status=" + Status;
}
function printPick(ID) {
    layer.confirm('<font size="4">是否打印上架单？</font>', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        title: ['提示', 'font-size:18px;']
    }, function (index) {
        layer.close(index);
        location.href = '/WMS/ReceiptManagement/PrintShelves?rid=' + ID + "&Flag=2";
    });
}
