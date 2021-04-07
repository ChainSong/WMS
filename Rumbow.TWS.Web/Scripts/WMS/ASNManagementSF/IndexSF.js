$(document).ready(function () {
    //if ($('#ASNCondition_CustomerID').val() == "") {
    //    $('#ASNCondition_CustomerID option:first').next().attr("selected", "selected");

    //} else {

    //}

    if (sessionStorage.getItem("WmsUserName") == "SHDZ01") {
        $("#StatusBackButton").show();
    }
    $("#resultTbody>tr").each(function (a, h) {
        if ($(h).children(".checkBox").children("input")[0].dataset == undefined) {
            if ($($(h).children(".checkBox").children("input")[0]).attr("data-status") == -1) {
                $(h)[0].style.background = "#cbc8c8";
            }
        }
        else {
            if ($(h).children(".checkBox").children("input")[0].dataset.status == -1) {
                $(h)[0].style.background = "#cbc8c8";
            }
        }
    })
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
    $(function ($) {
        $('body').click(function () {
            ShowsOut()
        });
    });
    $(".ddiv").mouseover(function () {
        return false;
    })
    $(".ddiv").mouseout(function () {
        return false;
    })
    if ($("#resultTable tbody")[0].rows.length < 1) {
        $("#resultTable").removeAttr("style");
        $("#tables").removeAttr("style");
        $("#OperateTh").removeAttr("style");
    }
    var lens = $("#resultTable tbody tr").length;
    if (lens > 0) {
        for (var i = 0; i < lens; i++) {
            if ($("#resultTable tbody tr")[i].cells[6].innerText.trim().toString() == "取消") {
                $("#resultTable tbody tr")[i].style.backgroundColor = "#cbc8c8"
            }
        }
    }
    $('.DynamicCalendarRange').each(function (index) {
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
        $(this).val($('#' + descID).val());
    });
    var setHiddenValToControl = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "ASNCondition_" + id;
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
    };
    var setPageControlVal = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "ASNCondition_" + id;
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
            var descId = "ASNCondition_" + id.substr(0, id.length - 4) + "ID";
            $('#' + descId).val($(this).val());
        });
        $(".calendarRange").each(function (index) {
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
    };
    //查询
    $('#searchButton').click(function () {
        setPageControlVal();
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
    //客户变更
    $('select[id=ASNCondition_CustomerID]').live('change', function () {
        window.location.href = "/WMS/ASNManagementSF/IndexSF/?customerID=" + $(this).val();
    });
    setHiddenValToControl();
    //导出
    $('#portButton').live('click', function () {
        var a = TableToJson();
        if (a == "") {
            return true;
        }
        else {
            window.location.href = "/WMS/ASNManagementSF/ExportOrder?IDs=" + a + "&CustomerID=" + $("#ASNCondition_CustomerID").val();
            return false;
        }
    });
    //生成上架库位
    $('#createShelfLocation').live('click', function () {
        var a = TableToJson();
        if (a == "") {
            layer.alert("请选择单号！", {
                icon: 2
            });
            return true;
        }
        else {
            if (a.split(',').length > 1) {
                layer.alert("目前暂不支持多单操作!");
                return true;
            }

            layer.confirm('<font size="4">是否生成上架库位？</font>', {
                btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                title: ['提示', 'font-size:18px;']
            }, function (index) {
                layer.close(index);
                index = layer.load(1);
                IDs = a;
                if (IDs != "" && IDs != 0) {
                    $.ajax({
                        type: "POST",
                        url: "/WMS/ASNManagement/CreateShelfLocation",
                        data: {
                            "ID": IDs
                        },
                        async: "false",
                        success: function (data) {
                            layer.close(index);
                            layer.alert("生成成功!");
                        },
                        error: function (msg) {
                            layer.alert("生成失败!");
                        }
                    });
                }
            });
            return false;
        }
    });
    //打印上架箱标贴
    $("#printShelfLocation").live("click", function () {
        layer.confirm('<font size="4">确认是否打印箱标贴？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);

            var id = TableToJson();
            // var isPrint = PrintStatus();
            if (id == "") {
                showMsg("请选择需要打印箱标贴的订单！", 4000);
                return false;
            }
            else {
                window.location.href = '/WMS/ASNManagement/PrintBoxLabel?ids=' + id + "&type=3";

            }
        });

    });

    //var a = TableToJson();
    //if (a == "") {
    //    return true;
    //}
    //else {
    //    layer.confirm('<font size="4">是否生成上架库位？</font>', {
    //        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
    //        title: ['提示', 'font-size:18px;']
    //    },
    //        location.href = '/WMS/ASNManagement/PrintASNNewBoxLabel?ids=' + str.substring(0, str.length - 1) + "&type=2"
    //    );
    //    return false;
    //}



    //订单状态统计
    $('#ReceiptStatusStatis').live('click', function () {
        var href = "/WMS/ASNManagementSF/ReceiptStatusStatis?CustomerID=" + $("#ASNCondition_CustomerID").val() + "";
        layer.open({
            type: 2,
            title: '入库单状态统计',
            shadeClose: true,
            shade: false,
            maxmin: true, //开启最大化最小化按钮
            area: ['900px', '600px'],
            content: href,
            move: '.layui-layer-title',
            moveOut: true
        });
    })
});
function ddd() { }
function TableToJson() {
    var a = "";
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            a += $('#' + checkBoxs[i].id).data('id').toString() + ",";
        }
    }
    a = a.substring(0, a.length - 1);
    return a;
}
//批量导入新增
var fileImportClick = function () {
    if ($('#CustomerIDImport option:selected').val() == "") {
        showMsg("请选择客户", "4000");
        return false;
    }
    if ($('#importExcel').val() === '') {
        showMsg("请选择要导入的Excel", "4000");
        return false;
    }
    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        showMsg("请选择Excel格式的文件", "4000");
        $(this).before(fileImport);
        return false;
    };
    WebPortal.MessageMask.Show("导入中...");
    $.ajaxFileUpload({
        url: '/WMS/ASNManagementSF/ImportASN',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: { customerid: $('#CustomerIDImport option:selected').val() },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            if (data.IsSuccess == true) {
                $('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
            }
            else {
                $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            }
        },
        error: function (data, status, e) {
            $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            WebPortal.MessageMask.Close();
        }
    });
    return false;
};
//编辑操作
function edit(ID) {
    location.href = "/WMS/ASNManagementSF/ASNCreateOrEditSF/?ID=" + ID + "&ViewType=2"
}
//单条转入库单操作
//function Receipt(ID, CustomerID) {
//    location.href = "/WMS/ReceiptManagement/ReceiptCreateSF/?ID=" + ID + "&ViewType=1&PageType=1&CustomerID=" + CustomerID
//}
//单条转入库单操作
function Receipt(ID, CustomerID) {
    $.ajax({
        type: "POST",
        url: "/WMS/ASNManagementSF/TurnASN",
        data: {
            "Id": ID, //Id, int CustomerID
            "CustomerID": CustomerID
        },
        async: "false",
        success: function (data) {
            if (data.Code == 1) {
                showMsg("操作成功", "4000");
            } else {
                showMsg("操作失败", "4000");

            }
            //location.href = "/WMS/ASNManagementSF/Index/"
        },
        error: function (msg) {
            showMsg("操作失败", "4000");
        }
    });
    //location.href = "/WMS/ReceiptManagement/ReceiptCreate/?ID=" + ID + "&ViewType=1&PageType=1&CustomerID=" + CustomerID
}
//单条取消操作
var IDs = '';
function ASNDelete(ID) {
    layer.confirm('<font size="4">是否取消？</font>', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        title: ['提示', 'font-size:18px;']
    }, function (index) {
        layer.close(index);
        IDs = ID;
        if (IDs != "" && IDs != 0) {
            $.ajax({
                type: "POST",
                url: "/WMS/ASNManagementSF/ASNDeleteSF",
                data: {
                    "ID": IDs
                },
                async: "false",
                success: function (data) {
                    showMsg("取消成功", "4000");
                    location.href = "/WMS/ASNManagementSF/IndexSF/"
                },
                error: function (msg) {
                    showMsg("操作失败", "4000");
                }
            });
        }
    });
}

$("#confirmReturn").live('click', function () {
    closePopup();
});
//单条完成操作
function Complet(ID) {
    layer.confirm('<font size="4">是否完成？</font>', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        title: ['提示', 'font-size:18px;']
    }, function (index) {
        layer.close(index);
        if (ID != "" && ID != 0) {
            $.ajax({
                type: "POST",
                url: "/WMS/ASNManagement/Complet",
                data: {
                    "ID": ID
                },
                async: "false",
                success: function (data) {
                    showMsg("完成成功!", "4000");
                    location.href = "/WMS/ASNManagement/IndexSF/"
                },
                error: function (msg) {
                    showMsg("操作失败", "4000");
                }
            });
        }
    });
}
//批量转入库单
function Receipts() {
    layer.confirm('<font size="4">是否批量转入库单？</font>', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        title: ['提示', 'font-size:18px;']
    }, function (index) {
        layer.close(index);
        if ($("#resultTable tbody input[type='checkbox']:checked").size() == 0) {
            showMsg("请选择单据！", "4000");
            return;
        } else {
            var status = '';
            var asnid = '';
            $('.checkForSelect').each(function (index) {
                if ($(this).attr('checked') === 'checked') {
                    status += "'" + $(this).attr('data-name') + "'" + ",";
                    asnid += "'" + $(this).attr('data-id') + "'" + ",";
                }
            })
            asnid = asnid.substring(0, asnid.length - 1).trim();
            status = status.substring(0, status.length - 1).trim();
            var arrys = new Array();
            arrys = status.split(',');
            var count = 0;
            for (var i = 0; i < arrys.length; i++) {
                if (arrys[i] == "'1'" || arrys[i] == "'5'") {
                    count++;
                }
            }
            if (count == 0) {
                showMsg("没有符合要求的单据!", "4000");
                return;
            } else {
                var messages = '';
                if (arrys.length == count) {
                    messages = "您一共选择" + arrys.length.toString() + "条单据您确定执行转入库单操作吗？";
                } else {
                    messages = "您一共选择" + arrys.length.toString() + "条单据,符合要求的有" + count + "条,其它" + (arrys.length - count).toString() + "条因为状态不是新增或者已生成入库单,您确定执行转入库单操作吗？";
                }
                $.ajax({
                    type: "POST",
                    url: "/WMS/ASNManagementSF/InsertIntoReceiptAndReceiptDetails",
                    data: {
                        "ASNIDs": asnid
                    },
                    async: "false",
                    success: function (data) {
                        if (data.IsSuccess) {
                            layer.confirm('批量转入库单成功！', {
                                btn: ['确定'] //按钮
                            }, function () {
                                    location.href = "/WMS/ASNManagementSF/Index/";
                            });
                        } else {
                            showMsg("操作失败", "4000");
                        }
                    },
                    error: function (msg) {
                        showMsg("操作失败", "4000");
                    }
                });
            }
        }
    });
}
//批量取消
function StatusBack() {
    layer.confirm('<font size="4">是否批量取消？</font>', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        title: ['提示', 'font-size:18px;']
    }, function (index) {
        layer.close(index);
        if ($("#resultTable tbody input[type='checkbox']:checked").size() == 0) {
            showMsg("请选择单据！", "4000");
            return;
        } else {
            var status = '';
            var asnid = '';
            $('.checkForSelect').each(function (index) {
                if ($(this).attr('checked') === 'checked') {
                    status += "'" + $(this).attr('data-name') + "'" + ",";
                    asnid += "'" + $(this).attr('data-id') + "'" + ",";
                }
            })
            asnid = asnid.substring(0, asnid.length - 1).trim();
            status = status.substring(0, status.length - 1).trim();
            var arrys = new Array();
            arrys = status.split(',');
            var count = 0;
            for (var i = 0; i < arrys.length; i++) {
                if (arrys[i] == "'1'") {
                    count++;
                }
            }
            if (count == 0) {
                showMsg("没有符合要求的单据!", "4000");
                return;
            } else {
                var messages = '';
                if (arrys.length == count) {
                    messages = "您一共选择" + arrys.length.toString() + "条单据您确定执行取消操作吗？";
                } else {
                    messages = "您一共选择" + arrys.length.toString() + "条单据,符合要求的有" + count + "条,其它" + (arrys.length - count).toString() + "条因为状态不是新增,您确定执行取消操作吗？";
                }
                $.ajax({
                    type: "POST",
                    url: "/WMS/ASNManagementSF/ASNStatusReturn",
                    data: {
                        "asnnumberlist": asnid
                    },
                    async: "false",
                    success: function (data) {
                        showMsg("取消成功!", "4000");
                        location.href = "/WMS/ASNManagement/Index/";
                    },
                    error: function (msg) {
                        showMsg("操作失败", "4000");
                    }
                });
            }
        }
    });
}
//批量完成操作
function CompletALLSelect() {
    layer.confirm('<font size="4">是否批量完成？</font>', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        title: ['提示', 'font-size:18px;']
    }, function (index) {
        layer.close(index);
        if ($("#resultTable tbody input[type='checkbox']:checked").size() == 0) {
            showMsg("请选择单据！", "4000");
            return;
        } else {
            var status = '';
            var asnid = '';
            $('.checkForSelect').each(function (index) {
                if ($(this).attr('checked') === 'checked') {
                    status += "'" + $(this).attr('data-name') + "'" + ",";
                    asnid += "'" + $(this).attr('data-id') + "'" + ",";
                }
            })
            asnid = asnid.substring(0, asnid.length - 1).trim();
            status = status.substring(0, status.length - 1).trim();
            var arrys = new Array();
            arrys = status.split(',');
            var count = 0;
            for (var i = 0; i < arrys.length; i++) {
                if (arrys[i] == "'5'") {
                    count++;
                }
            }
            if (count == 0) {
                showMsg("没有符合要求的单据!", "4000");
                return;
            } else {
                var messages = '';
                if (arrys.length == count) {
                    messages = "您一共选择" + arrys.length.toString() + "条单据您确定执行完成操作吗？";
                } else {
                    messages = "您一共选择" + arrys.length.toString() + "条单据,符合要求的有" + count + "条,其它" + (arrys.length - count).toString() + "条因为状态不是已生成入库单,您确定执行完成操作吗？";
                }
                $.ajax({
                    type: "POST",
                    url: "/WMS/ASNManagementSF/CompletALLSelect",
                    data: {
                        "asnid": asnid
                    },
                    async: "false",
                    success: function (data) {
                        showMsg("成功!", "4000");
                        location.href = "/WMS/ASNManagement/Index/";
                    },
                    error: function (msg) {
                        showMsg("操作失败", "4000");
                    }
                });
            }
        }
    });
}
//新增操作
function createasn() {
    var customerid = $('#ASNCondition_CustomerID').val();
    location.href = "/WMS/ASNManagementSF/ASNCreateOrEditSF/?ID=0" + "&customerID=" + customerid + "" + "&ViewType=1"
}

function ViewReceipt(ID, CustomerID) {
    $.ajax({
        type: "POST",
        url: "/WMS/ASNManagementSF/CountReceiptSF",
        data: {
            "ID": ID
        },
        async: "false",
        success: function (data) {
            if (data.length.toString() == "1") {
                location.href = "/WMS/ReceiptManagementSF/ReceiptCreate/?ID=" + data[0].ID + "&ViewType=3&CustomerID=" + CustomerID
            }
            if (parseInt(data.length.toString()) > 1) {
                $("#ReceiptTableID  tr:not(:first)").html("");
                for (var i = 0; i < data.length; i++) {
                    addNew(data[i].ID, data[i].ReceiptNumber, data[i].Status);
                }
                openPopup('pop123', true, 400, 300, null, 'ReceiptID');
                $("#popupLayer_pop123")[0].style.top = "200px";
            }
        },
        error: function (msg) {
            showMsg("操作失败", "4000");
        }
    });
}

function addNew(td11, td22, td33) {
    var table1 = $('#ReceiptTableID');
    var firstTr = table1.find('tbody>tr:first');
    var row = $("<tr id=Row></tr>");
    var td2 = $("<td></td>");
    var td3 = $("<td></td>");
    if (td33 == 1) {
        td33 = "待上架";
    }
    else if (td33 == 5) {
        td33 = "已上架";
    }
    else if (td33 == 9) {
        td33 = "已入库";
    }
    else { td33 = td33; }
    td2.append($("<a href=" + '/WMS/ReceiptManagementSF/ReceiptCreateSF/?ID=' + td11 + '&ViewType=3>' + td22 + "</a>"));
    td3.append($("<b>" + td33 + "</b>"));
    row.append(td2);
    row.append(td3);
    table1.append(row);
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
        }, 100);
        $("#operateTD" + ID).animate({
            width: "show",
            width: "360%",
            paddingRight: "show",
            paddingLeft: "show",
            marginRight: "show",
            marginLeft: "show"
        });
    }
}

function ShowsOut() {
    $(".ddiv").animate({
        width: "hide",
        width: "320%",
        paddingRight: "hide",
        paddingLeft: "hide",
        marginRight: "hide",
        marginLeft: "hide"
    }, 100);
}
