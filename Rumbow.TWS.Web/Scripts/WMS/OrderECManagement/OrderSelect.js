﻿$(document).ready(function () {
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
    //打印
    //$(function ($) {
    //    $('body').click(function () {
    //        ShowsOut()
    //    });
    //});
    if ($("#resultTable tbody")[0].rows.length < 1) {
        $("#resultTable").removeAttr("style");
        $("#tables").removeAttr("style");
        $("#OperateTh").removeAttr("style");
    }

    $('#printButton').live('click', function () {
        layer.confirm('<font size="4">确认是否批量打印拣货单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {

            layer.close(index);
            var id = TableToJson();
            // var isPrint = PrintStatus();
            if (id == "") {
                showMsg("请选择需要打印的拣货单！", 4000);
                return false;
            }
            else {
                if ($("#ProjectName").val() == "EWE") {
                    window.location.href = '/WMS/OrderECManagement/PrintOrder_EWE?id=' + id + "&Flag=3";
                }
                else if ($("#ProjectName").val() == "NIKE") {
                    window.location.href = '/WMS/OrderECManagement/PrintOrderNike?id=' + id + "&Flag=1";
                }
                else if ($("#ProjectName").val() == "延锋百利得") {
                    window.location.href = '/WMS/OrderECManagement/PrintOrderYFBLD?id=' + id + "&Flag=1";
                }
                else if ($("#ProjectName").val() == "Akzo") {
                    window.location.href = '/WMS/OrderECManagement/PrintOrderAkzo?id=' + id + "&Flag=1";
                }
                else if ($("#ProjectName").val() == "YXDR") {
                    window.location.href = '/WMS/OrderECManagement/PrintOrderYXDR?id=' + id + "&Flag=1";
                }
                else {
                    window.location.href = '/WMS/OrderECManagement/PrintOrderNike?id=' + id + "&Flag=1";
                }
                // });
                //  }  
            }
        });

    });
    //HideTr();
    $('#exportButton').live('click', function () {
        var a = TableToJson();
        if (a == "") {
            return true;
        }
        else {

            window.location.href = "/WMS/OrderECManagement/ExportOrder?IDs=" + a + "&CustomerID=" + $("#SearchCondition_CustomerID").val();
            //window.location.href = "ExportOrder?IDs=" + a + "&CustomerID=" + $("#SearchCondition_CustomerID").val();
            //$.ajax({
            //    url: "/WMS/OrderECManagement/ExportOrder",
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
            //    url: "/WMS/OrderECManagement/ExportOrder",
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

    $("#SearchCondition_Province").live("keydown", function () {
        $('#SearchCondition_Province').autocomplete({
            source: function (request, response) {

                $.ajax({
                    url: "/WMS/OrderECManagement/GetProvince",
                    type: "POST",
                    dataType: "json",
                    data: {
                        name: request.term,
                        type: "keydown"
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Text, value: item.Text, data: item }
                        }));
                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });
            },
        });
    }).trigger('keydown');
    $("#SearchCondition_City").live("keydown", function () {
        $('#SearchCondition_City').autocomplete({
            source: function (request, response) {

                $.ajax({
                    url: "/WMS/OrderECManagement/GetCity",
                    type: "POST",
                    dataType: "json",
                    data: {
                        province: $("#SearchCondition_Province").val(),
                        name: request.term,
                        type: "keydown"
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Text, value: item.Text, data: item }
                        }));
                    }
                });
            },
        });
    }).trigger('keydown');
    $("#SearchCondition_District").live("keydown", function () {
        $('#SearchCondition_District').autocomplete({
            source: function (request, response) {

                $.ajax({
                    url: "/WMS/OrderECManagement/GetDistrict",
                    type: "POST",
                    dataType: "json",
                    data: {
                        city: $("#SearchCondition_City").val(),
                        name: request.term,
                        type: "keydown"
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Text, value: item.Text, data: item }
                        }));
                    }
                });
            },
        });
    }).trigger('keydown');

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

    });

});


function LTrim(str) {
    var i;
    for (i = 0; i < str.length; i++) {
        if (str.charAt(i) != " " && str.charAt(i) != " ") break;
    }
    str = str.substring(i, str.length);
    return str;
}
function RTrim(str) {
    var i;
    for (i = str.length - 1; i >= 0; i--) {
        if (str.charAt(i) != " " && str.charAt(i) != " ") break;
    }
    str = str.substring(0, i + 1);
    return str;
}
function Trim(str) {
    return LTrim(RTrim(str));
}

String.prototype.Trim = function () { return Trim(this); }
String.prototype.LTrim = function () { return LTrim(this); }
String.prototype.RTrim = function () { return RTrim(this); }


$('select[id=SearchCondition_CustomerID]').live('change', function () {
    window.location.href = "/WMS/OrderECManagement/Index/?customerID=" + $(this).val();
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


function PrintStatus() {
    var a = false;

    var checkBoxs = $("#resultTable tbody input[type='checkbox']");

    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {


            $(".printstatus").each(function (c, d) {
                if (i == c) {
                    if (Trim(d.innerText) == "已打印") {
                        a = true;

                    }
                }
            });
        }
    }



    return a;
}


function PickOrConfirm(ID, type, obj) {
    var txt = "";
    if (type == "Pick") {
        txt = "确认是否拣货？";

    }
    if (type == "Confirm") {
        txt = "确认是否复检？";

    }
    layer.confirm('<font size="4">' + txt + '</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Post",
            url: "/WMS/OrderECManagement/PickOrConfirm",
            data: {
                "ID": ID,
                "type": type
            },
            async: "false",
            success: function (data) {
                var a = parseInt(obj.parentNode.parentNode.parentNode.cells.length);
                if (data == "") {
                    if (type == "Pick") {
                        //location.href = "/WMS/OrderECManagement/Index"
                        showMsg("拣货完成", 4000);
                        obj.parentNode.parentNode.parentNode.cells[7].innerHTML = '已拣货'

                        obj.parentNode.parentNode.parentNode.cells[a - 1].children.StatusbackCode.value = 3
                    }
                    else if (type == "Confirm") {
                        //location.href = "/WMS/OrderECManagement/Index"
                        showMsg("复检完成", 4000);
                        obj.parentNode.parentNode.parentNode.cells[7].innerHTML = '已复检'
                        obj.parentNode.parentNode.parentNode.cells[a - 1].children.StatusbackCode.value = 4
                    }
                }
                else {
                    showMsg("操作失败：" + data, 4000);
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });

}
function Package(ID) {
    if ($("#ProjectName").val() == "NIKE") {
        location.href = "/WMS/OrderECManagement/NikePackage/?ID=" + ID
    }
    else if ($("#ProjectName").val() == "YXDR") {
        location.href = "/WMS/OrderECManagement/YXDRPackage/?ID=" + ID
    }
    else {
        location.href = "/WMS/OrderECManagement/NikePackage/?ID=" + ID
    }
}
function BarCodePackage(ID) {
    location.href = "/WMS/OrderECManagement/BarCodePackage/?ID=" + ID
}
function statusBack(ID, obj) {
    $("#StatusbackID").val(ID);

    $('#backStatusid').children("span").children().unwrap();
    var a = parseInt(obj.parentNode.parentNode.parentNode.cells.length);
    var Status = obj.parentNode.parentNode.parentNode.cells[a - 1].children.StatusbackCode.value;
    openPopup('pop99', true, 350, 300, null, 'statusBackDiv');
    $("#popupLayer_pop99")[0].style.top = "200px";
    for (var i = 0; i < $('#backStatusid').children().length; i++) {
        var a = $('#backStatusid').children()[i].value;
        if (a >= Status) {
            $('#backStatusid').children('option[value=' + a + ']').wrap('<span>').hide();
        }

    }
}


$("#statusBackReturn").live('click', function () {
    closePopup();
});
$("#intelligentDispatchRT").live('click', function () {
    closePopup();
});
$("#statusBackOK").live('click', function () {
    if ($('#backStatusid').val() == "") {
        //alert("请选择要回退的状态");
        showMsg("请选择要回退的状态", 4000);
        return;
    }
    $.ajax({
        type: "Post",
        url: "/WMS/OrderECManagement/OrderBackStatus",
        data: {
            "ID": $('#StatusbackID').val(),
            "ToStatus": $('#backStatusid').val(),
            "type": 0
        },
        async: "false",
        success: function (data) {
            if (data == "") {
                //showMsg("回退成功！", 4000);
                location.href = "/WMS/OrderECManagement/Index"
            }
            else {
                if (data == "请输入用户名密码") {
                    //弹出用户名密码
                    closePopup();
                    openPopup('pop990', true, 350, 300, null, 'showUserNameAndPwd');
                    $("#popupLayer_pop990")[0].style.top = "200px";
                }
                else {
                    showMsg("回退失败！" + data, 4000);
                }
            }
        },
        error: function (msg) {
            alert(msg.val);
        }

    });


});

$("#statusBackReturnUser").live('click', function () {
    closePopup();
});

$("#statusBackOKUser").live('click', function () {
    if ($('#backStatusid').val() == "") {
        showMsg("请选择要回退的状态", 4000);
        return;
    }
    if ($('#UserName').val() == "") {
        showMsg("请输入用户名", 4000);
        return;
    }
    if ($('#Pwd').val() == "") {
        showMsg("请输入密码", 4000);
        return;
    }

    $.ajax({
        type: "Post",
        url: "/WMS/OrderECManagement/OrderBackStatus",
        data: {
            "ID": $('#StatusbackID').val(),
            "ToStatus": $('#backStatusid').val(),
            "type": 1,
            "username": $('#UserName').val(),
            "password": $('#Pwd').val()
        },
        async: "false",
        success: function (data) {
            if (data == "") {
                //showMsg("回退成功！", 4000);
                location.href = "/WMS/OrderECManagement/Index"
            }
            else {
                showMsg("回退失败！" + data, 4000);
            }
        },
        error: function (msg) {
            alert(msg.val);
        }

    });
});


$("#intelligentDispatchOK").live('click', function () {
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    var sql = "";
    showMsg("已自动过滤掉非待处理状态的订单！", 4000);
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked && $(checkBoxs[i]).data().status == 1) {
            sql += checkBoxs[i].name.toString() + ",";
        }
    }
    if (sql.length == 0) {
        showMsg("请选择订单", 4000);
        return false;
    }

    if ($('#WorkStation').val() == "") {
        //alert("请选择要回退的状态");
        showMsg("请选择操作台", 4000);
        return;
    }
    $.ajax({
        type: "post",
        url: "/WMS/OrderECManagement/AddInstructions",
        data: {
            "ids": sql,
            "WorkStation": $('#WorkStation').val(),
            "WarehouseQueue": $('#WarehouseQueue').val(),
            "Priority": $('#Priority').val()
        },
        //async: "false",
        success: function (data) {
            if (data.Code == "1") {
                closePopup();
                showMsg("发送成功！", 4000);
                //location.href = "/WMS/OrderECManagement/Index"
            }
            else {
                showMsg("发送失败！", 4000);
            }
        },
        error: function (msg) {
            alert(msg.val);
        }

    });


});
$("#pickButton").live('click', function () {
    layer.confirm('<font size="4">确认是否批量拣货？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);

        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                if (checkBoxs[i].dataset.status < '3') {
                    sql += checkBoxs[i].name.toString() + ",";

                }
            }
        }
        if (sql.length > 0) {
            $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
        }
        else {
            showMsg("请勾选出库单！", 4000);
            return;
        }
        $.ajax({
            type: "Post",
            url: "/WMS/OrderECManagement/PickOrConfirm",
            data: {
                "ID": $('#StatusbackID').val(),
                "type": "Pick"
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    location.href = "/WMS/OrderECManagement/Index"
                    //showMsg("拣货完成", 4000);
                    //obj.parentNode.parentNode.cells[6].innerHTML = '已拣货'

                }
                else {
                    showMsg("操作失败：" + data, 4000);
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });

});

$("#repickButton").live('click', function () {
    layer.confirm('<font size="4">确认是否批量复检？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);

        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                if (checkBoxs[i].dataset.status < '4') {
                    sql += checkBoxs[i].name.toString() + ",";
                }
            }
        }
        if (sql.length > 0) {
            $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
        }
        else {
            showMsg("请勾选出库单！", 4000);
            return;
        }
        $.ajax({
            type: "Post",
            url: "/WMS/OrderECManagement/PickOrConfirm",
            data: {
                "ID": $('#StatusbackID').val(),
                "type": "Confirm"
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    location.href = "/WMS/OrderECManagement/Index"
                    //showMsg("拣货完成", 4000);
                    //obj.parentNode.parentNode.cells[6].innerHTML = '已复检'
                }
                else {
                    showMsg("操作失败：" + data, 4000);
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });

});
$("#deliverButton").live('click', function () {
    layer.confirm('<font size="4">确认是否批量交接？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                if (checkBoxs[i].dataset.status < '8') {
                    sql += checkBoxs[i].name.toString() + ",";
                }
            }
        }
        if (sql.length > 0) {
            $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
        }
        else {
            showMsg("请勾选出库单！", 4000);
            return;
        }
        $.ajax({
            type: "Post",
            url: "/WMS/OrderECManagement/Handover",
            data: {
                "ID": $('#StatusbackID').val()
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    //showMsg("交接完成！", 4000);
                    location.href = "/WMS/OrderECManagement/Index"
                }
                else {
                    showMsg("交接失败：" + data, 4000);
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });

});
$("#ExportAllBoxlist").live('click', function () {
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    var sql = "";
    var type = "1";
    var OrderID = "0";
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            sql += checkBoxs[i].name.toString() + ",";
        }
    }
    if (sql.length > 0) {
        if ($("#ProjectName").val().toString().indexOf("NIKE") > -1) {
            $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
            window.location.href = "/WMS/NikeOSRBJPrint/ExportBoxDetailsPL?id=" + $("#StatusbackID").val() + "&type=" + type + "&OrderID=" + OrderID;
        }
        else if ($("#ProjectName").val().toString().indexOf("YXDR") > -1) {
            //$("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
            //window.location.href = "/WMS/YXDRPackagePrint/PrintAllPod?ids=" + $("#StatusbackID").val();
        }
        else {
            $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
            window.location.href = "/WMS/NikeOSRBJPrint/ExportBoxDetailsPL?id=" + $("#StatusbackID").val() + "&type=" + type + "&OrderID=" + OrderID;
        }
    }
    else {
        showMsg("请勾选要导出的出库单！", 4000);
        return;
    }
});
$("#printAllpod").live('click', function () {
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    var sql = "";
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            sql += checkBoxs[i].name.toString() + ",";
        }
    }
    if (sql.length > 0) {
        if ($("#ProjectName").val().toString().indexOf("NIKE") > -1) {
            $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
            window.location.href = "/WMS/OrderECManagement/PrintAllPod?ids=" + $("#StatusbackID").val();
        }
        else if ($("#ProjectName").val().toString().indexOf("YXDR") > -1) {
            $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
            window.location.href = "/WMS/YXDRPackagePrint/PrintAllPod?ids=" + $("#StatusbackID").val();
        }
        else {
            $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
            window.location.href = "/WMS/OrderECManagement/PrintAllPod?ids=" + $("#StatusbackID").val();
        }
    }
    else {
        showMsg("请勾选要打印的出库单！", 4000);
        return;
    }
});
$("#btnBatchPrintOrder").live('click', function () {
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    var sql = "";
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            sql += checkBoxs[i].name.toString() + ",";
        }
    }
    if (sql.length > 0) {
        $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
        if ($("#ProjectName").val() == "延锋百利得") {
            location.href = "/WMS/OrderECManagement/BatchPrintOrderYFBLD?id=" + $("#StatusbackID").val();
        }
    }
    else {
        showMsg("请勾选出库单！", 4000);
        return;
    }
});
$("#outButton").live('click', function () {
    layer.confirm('<font size="4">确认是否批量出库？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                if (checkBoxs[i].dataset.status < '9') {
                    sql += checkBoxs[i].name.toString() + ",";
                }
            }
        }
        if (sql.length > 0) {
            $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
        }
        else {
            showMsg("请勾选出库单！", 4000);
            return;
        }
        $.ajax({
            type: "Post",
            url: "/WMS/OrderECManagement/Outs",
            data: {
                "ID": $('#StatusbackID').val()
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    //showMsg("出库完成！", 4000);
                    location.href = "/WMS/OrderECManagement/Index"
                }
                else {
                    showMsg("出库失败：" + data, 4000);
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });

});
$("#ChangeExpressButton").live('click', function () {
    layer.confirm('<font size="4">确认是否转换快递？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {

        layer.close(index);
        if ($("#SearchCondition_ExpressCompany").val() == "") {
            showMsg("请先选择快递公司！", 4000);
            return;
        }
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                if (checkBoxs[i].dataset.status < '9') {
                    sql += checkBoxs[i].name.toString() + ",";
                }
            }
        }
        if (sql.length > 0) {
            $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
        }
        else {
            showMsg("请勾选出库单！", 4000);
            return;
        }
        $.ajax({
            type: "Post",
            url: "/WMS/OrderECManagement/ChangeExpress",
            data: {
                "ID": $('#StatusbackID').val(),
                "ExpressCompany": $("#SearchCondition_ExpressCompany").val()
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    showMsg("转换成功！", 4000);
                    location.href = "/WMS/OrderECManagement/Index"
                }
                else {
                    showMsg("出库失败：" + data, 4000);
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });

});
//$("#unionButton").live('click', function () {
//    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
//    var sql = "";
//    for (var i = 0; i < checkBoxs.length; i++) {
//        if (checkBoxs[i].checked) {
//            sql += checkBoxs[i].name.toString() + ",";
//        }
//    }
//    if (sql.length > 0) {
//        $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
//    }
//    else {
//        showMsg("请勾选出库单！", 4000);
//        return;
//    }
//    $.ajax({
//        type: "Post",
//        url: "/WMS/OrderECManagement/UnionOrder",
//        data: {
//            "ID": $('#StatusbackID').val()
//        },
//        async: "false",
//        success: function (data) {
//            if (data == "") {
//                //showMsg("出库完成！", 4000);
//                location.href = "/WMS/OrderECManagement/Index"
//            }
//            else {
//                showMsg("合并失败：" + data, 4000);
//            }
//        },
//        error: function (msg) {
//            alert(msg.val);
//        }

//    });
//});

$("#intelligentDispatch").live('click', function () {



    //$("#statusBack").popover('destroy');
    //$("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
    openPopup("", true, 350, 300, null, 'intelligentDispatchPanel', true);

    //}
    //else {
    //    showMsg("请勾选出库单！", 4000);

    //}

});

$("#backButton").live('click', function () {

    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    var sql = "";
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            sql += checkBoxs[i].name.toString() + ",";

        }
    }
    if (sql.length > 0) {

        $("#statusBack").popover('destroy');
        $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
        openPopup("pop11", true, 350, 300, null, 'statusBackDiv', true);

    }
    else {
        showMsg("请勾选出库单！", 4000);

    }

});


$("#resultTable tbody input[type='checkbox']").live('click', function () {
    RefreshIDs();
});

var RefreshIDs = function () {
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    var length = checkBoxs.length;
    var checked = 0;
    checkBoxs.each(function () {
        if ($(this).attr("checked") === "checked") {
            checked++;
        }
    });

    if (checked == checkBoxs.length) {
        $('#selectAll').attr("checked", "checked");
    } else {
        $('#selectAll').removeAttr("checked");
    }
}

$("#addButton").live('click', function () {
    openPopup("", true, 1000, 600, '/WMS/OrderECManagement/PreOrderQuery/?CustomerID=' + $("#SearchCondition_CustomerID option:selected").val(), null, function (PreID) {
        location.href = "/WMS/PreOrder/ManualAllocation/?ID=" + PreID

    });
});


$("#moreCondition").live('click', function () {
    var rows = $("#conditionTable")[0].rows.length;
    for (var i = 4; i < rows; i++) {
        if ($("#conditionTable")[0].rows[i].style.display == "none") {
            $("#conditionTable")[0].rows[i].style.display = ""
            $("#moreCondition")[0].innerText = "收缩︽"
        }
        else {
            $("#conditionTable")[0].rows[i].style.display = "none";
            $("#moreCondition")[0].innerText = "展开︾"
        }

    }
});


function HideTr() {
    var rows = $("#conditionTable")[0].rows.length;
    for (var i = 4; i < rows; i++) {
        $("#conditionTable")[0].rows[i].style.display = "none";
    }
}

function Handover(ID, obj) {
    layer.confirm('<font size="4">确认是否交接？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Post",
            url: "/WMS/OrderECManagement/Handover",
            data: {
                "ID": ID
            },
            async: "false",
            success: function (data) {
                var a = parseInt(obj.parentNode.parentNode.parentNode.cells.length);
                if (data == "") {
                    showMsg("交接完成！", 4000);
                    obj.parentNode.parentNode.parentNode.cells[7].innerHTML = '已交接'
                    obj.parentNode.parentNode.parentNode.cells[a - 1].children.StatusbackCode.value = 8
                }
                else {
                    showMsg("交接失败：" + data, 4000);
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });

}

function Outs(ID, obj) {
    layer.confirm('<font size="4">确认是否出库？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Post",
            url: "/WMS/OrderECManagement/Outs",
            data: {
                "ID": ID
            },
            async: "false",
            success: function (data) {
                var a = parseInt(obj.parentNode.parentNode.parentNode.cells.length);
                if (data == "") {
                    showMsg("出库完成！", 4000);
                    obj.parentNode.parentNode.parentNode.cells[7].innerHTML = '已出库'
                    obj.parentNode.parentNode.parentNode.cells[a - 1].children.StatusbackCode.value = 9
                }
                else {
                    showMsg("出库失败：" + data, 4000);
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });

}

function print(id) {
    layer.confirm('<font size="4">确认是否打印拣货单？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        if ($("#ProjectName").val() == "EWE") {
            window.location.href = '/WMS/OrderECManagement/PrintOrder_EWE?id=' + id + "&Flag=3";
        }
        else if ($("#ProjectName").val() == "NIKE") {
            window.location.href = '/WMS/OrderECManagement/PrintOrderNike?id=' + id + "&Flag=1";
        }
        else if ($("#ProjectName").val() == "延锋百利得") {
            window.location.href = '/WMS/OrderECManagement/PrintOrderYFBLD?id=' + id + "&Flag=1";
        }
        else if ($("#ProjectName").val() == "Akzo") {
            window.location.href = '/WMS/OrderECManagement/PrintOrderAkzo?id=' + id + "&Flag=1";
        }
        else if ($("#ProjectName").val() == "YXDR") {
            window.location.href = '/WMS/OrderECManagement/PrintOrderYXDR?id=' + id + "&Flag=1";
        }
        else {
            window.location.href = '/WMS/OrderECManagement/PrintOrderNike?id=' + id + "&Flag=1";
        }
    });

}
//包装导入 
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
    var ss = $('#StorerID option:selected').text();
    var ff = $('#StorerID').val();
    WebPortal.MessageMask.Show("导入中...");
    $.ajaxFileUpload({
        url: "/WMS/OrderECManagement/ImputEcecl",
        secureuri: false,
        fileElementId: 'importExcel',
        type: "POST",
        dataType: "json",
        data: {
            CustomerName: $('#StorerID option:selected').text(),
            CustomerID: $('#StorerID').val()
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
    return false;
};
////下载模板
//$("#portButtonTemplet").click(function () {
//    demo();
//    //demo('/WMS/PreOrder/ReportExportExecl?/CustomerID');

//    //window.location.href = "http://www.runbow.com.cn:8080/Picture/DemoExcel/包装信息导入模板.xlsx";
//})
//function demo(url) {
//    // $.send('/WMS/Product/demoExecl');
//    // 绑定导出按钮

//    var ID = $('select[id=StorerID]')[0].value;
//    var url = '/WMS/PreOrder/ReportExportExecl';
//    var form = $("<form>");
//    form.attr('style', 'display:none');
//    form.attr('target', '');
//    form.attr('method', 'post}');
//    form.attr('action', url);//'/WMS/PreOrder/ReportExportExecl'
//    var input1 = $('<input>');
//    input1.attr('type', 'hidden');
//    input1.attr('name', 'demo');
//    input1.attr('value', 'Export');
//    var input2 = $('<input>');
//    input2.attr('type', 'hidden');
//    input2.attr('name', 'fileId');
//    input2.attr('value', "fileId");
//    var input3 = $('<input id="ID" name="ID" type="hidden" value="' + ID + '" />');
//    $('body').append(form);
//    form.append(input1);
//    form.append(input2);
//    form.append(input3);

//    form.submit();
//    form.remove();
//}
//function ShowsIn(ID, obj) {
//    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
//        $(".ddiv:not(obj)").animate({
//            width: "hide",
//            width: "600%",
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
//        width: "600%",
//        paddingRight: "hide",
//        paddingLeft: "hide",
//        marginRight: "hide",
//        marginLeft: "hide"

//    }, 100);
//    //$("#operateTD" + ID)[0].style.display = "";
//}

$("#btnSaveDetail").live("click", function () {
    //var Sku = $(this).children()[1].innerText;
    //var GoodsName = $(this).children()[2].innerText;
    //var UPC = $(this).children()[3].innerText;
    //if (Sku != "SKU") {
    //    closePopup(Sku, GoodsName, UPC);
    //}
    var id_select = TableToJson();
    if (id_select != "") {
        var CustomerID = $("#SearchCondition_CustomerID  option:selected").val();
        var CustomerName = $("#SearchCondition_CustomerID  option:selected").text();
        var WarehouseID = $("#SearchCondition_Warehouse  option:selected").val();
        var WarehouseName = $("#SearchCondition_Warehouse  option:selected").text();
        //先验证一下是否还允许组波（是否超过波次大小）
        var IsCan = false;
        $.ajax({
            type: "Post",
            url: "/WMS/OrderECManagement/VerifyWaveSize",
            data: {
                "HeaderID": $("#PrintID").val(), "IDs": id_select
            },
            async: "false",
            success: function (res) {
                var data = JSON.parse(res);
                if (data.code == 0) {
                    closePopup(id_select, CustomerID, CustomerName, WarehouseID, WarehouseName);
                } else {
                    layer.confirm('' + data.msg + '', {
                        btn: ['确定'] //按钮
                    }, function (index) {
                        IsCan = false;
                        layer.close(index);
                    });
                }
            },
            error: function (msg) {
                alert(msg.val);
            }
        });
       

       
    }
});