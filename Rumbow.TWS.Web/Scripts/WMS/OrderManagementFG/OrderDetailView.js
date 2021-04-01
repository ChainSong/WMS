$(document).ready(function () {
    HideButton();
    $('#returnButton').live('click', function () {
        history.back();
    });
    $('#printButton').live('click', function () {
        var id = $("#order_ID").val();
        //if (id == "") {
        //    showMsg("请选择需要打印的拣货单！", 4000);
        //    return false;
        //}
        //else {
        //    window.location.href = '/WMS/OrderManagementFG/PrintOrder?id=' + id;
        //}
        layer.confirm('<font size="4">确认是否打印拣货单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            //if ($("#ProjectName").val() == "EWE") {
            //    window.location.href = '/WMS/OrderManagementFG/PrintOrder_EWE?id=' + id + "&Flag=3";
            //}
            //else if ($("#ProjectName").val() == "NIKE") {
            //    window.location.href = '/WMS/OrderManagementFG/PrintOrderNike?id=' + id + "&Flag=1";
            //}
            //else if ($("#ProjectName").val() == "YXDR") {
            //    window.location.href = '/WMS/OrderManagementFG/PrintOrderYXDR?id=' + id + "&Flag=1";
            //}
            //else if ($("#ProjectName").val() == "Mono" || $("#ProjectName").val() == "吉特") {
            //    window.location.href = '/WMS/OrderManagementFG/PrintOrder_JT?id=' + id + "&Flag=1";
            //}
            else {
                window.location.href = '/WMS/OrderManagementFG/PrintOrder_JT?id=' + id + "&Flag=1";
            }
        });
    });


    $('#backButton').live('click', function () {
        var id = $("#order_ID").val();
        var Status = $("#order_Status").val();
        $("#StatusbackID").val(id);

        $('#backStatusid').children("span").children().unwrap();
        openPopup('popp', true, 350, 300, null, 'statusBackDiv');
        $("#popupLayer_popp")[0].style.top = "200px";
        for (var i = 0; i < $('#backStatusid').children().length; i++) {
            var a = $('#backStatusid').children()[i].value;
            if (a >= Status) {
                $('#backStatusid').children('option[value=' + a + ']').wrap('<span>').hide();
            }

        }
    });

    $("#statusBackReturn").live('click', function () {
        closePopup();
    });
    $("#statusBackOK").live('click', function () {
        if ($('#backStatusid').val() == "") {
            //alert("请选择要回退的状态");
            showMsg("请选择要回退的状态", 4000);
            return;
        }
        $.ajax({
            type: "Get",
            url: "/WMS/OrderManagementFG/OrderBackStatus",
            data: {
                "ID": $('#StatusbackID').val(),
                "ToStatus": $('#backStatusid').val()
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    showMsg("回退成功！", 4000);
                    if ($('#backStatusid').val() == "0") {
                        location.href = "/WMS/OrderManagementFG/Index";
                    }
                    else {

                        $("#label_OrderStatus")[0].innerText = $('#backStatusid  option:selected').text();
                        closePopup();
                        var s = $('#backStatusid').val();
                        switch (s) {
                            case "1":
                                $("#pickButton").show();
                                $("#repickButton").show();
                                $("#packageButton").show();
                                $("#deliverButton").show();
                                break;
                            case "3":
                                $("#pickButton").show();
                                $("#repickButton").show();
                                $("#packageButton").show();
                                $("#deliverButton").show();
                                break;
                            case "4":
                                $("#repickButton").show();
                                $("#packageButton").show();
                                $("#deliverButton").show();
                                break;
                            case "6":
                                $("#packageButton").show();
                                $("#deliverButton").show();
                            case "8":
                                $("#deliverButton").show();
                                break;
                        }
                    }

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
});

function Pick() {
    layer.confirm('<font size="4">确认是否拣货</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Post",
            url: "/WMS/OrderManagementFG/PickOrConfirm",
            data: {
                "ID": $("#order_ID").val(),
                "type": "Pick"
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    showMsg("拣货完成", 4000);
                    $("#label_OrderStatus")[0].innerText = '已拣货'
                    $("#order_Status").val(3);
                    //obj.parentNode.parentNode.cells[13].children.StatusbackCode.value = 3
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

function Confirm() {
    layer.confirm('<font size="4">确认是否复检</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Post",
            url: "/WMS/OrderManagementFG/PickOrConfirm",
            data: {
                "ID": $("#order_ID").val(),
                "type": "Confirm"
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    showMsg("复检完成", 4000);
                    $("#label_OrderStatus")[0].innerText = '已复检'
                    $("#order_Status").val(4);
                    $("#pickButton").hide();
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

function Package() {
    if ($("#ProjectName").val() == "NIKE" || $("#ProjectName").val() == "YXDR") {
        location.href = "/WMS/OrderManagementFG/YXDRPackage/?ID=" + +$("#order_ID").val()
    }
    else {
        location.href = "/WMS/OrderManagementFG/Package/?ID=" + +$("#order_ID").val()
    }
}
function Handover() {
    layer.confirm('<font size="4">确认是否交接</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Post",
            url: "/WMS/OrderManagementFG/Handover",
            data: {
                "ID": $("#order_ID").val()
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    showMsg("交接完成！", 4000);
                    $("#label_OrderStatus")[0].innerText = '已交接'
                    $("#order_Status").val(8);
                    $("#pickButton").hide();
                    $("#repickButton").hide();
                    $("#packageButton").hide();
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

function Outs() {
    layer.confirm('<font size="4">确认是否出库</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Post",
            url: "/WMS/OrderManagementFG/Outs",
            data: {
                "ID": $("#order_ID").val()
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    showMsg("出库完成！", 4000);
                    $("#label_OrderStatus")[0].innerText = '已出库'
                    $("#order_Status").val(9);
                    $("#pickButton").hide();
                    $("#repickButton").hide();
                    $("#packageButton").hide();
                    $("#deliverButton").hide();
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

function Returns() {
    //location.href = history.back();//多层级页面跳转，准确跳回所在菜单主页
    var url = $(window.parent.document).find(".active a").attr('href');
    url = url.toString().split(',')[2];
    url = url.substring(1, url.length - 2);
    location.href = url;
}

function HideButton() {
    var s = $('#order_Status').val();
    switch (s) {
        case "4":
            $("#pickButton").hide();
            $("#repickButton").show();
            $("#packageButton").show();
            $("#deliverButton").show();
            break;
        case "6":
            $("#pickButton").hide();
            $("#repickButton").hide();
            $("#packageButton").show();
            $("#deliverButton").show();
            break;
        case "8":
            $("#pickButton").hide();
            $("#repickButton").hide();
            $("#packageButton").hide();
            $("#deliverButton").show();
            break;
        case "9":
            $("#pickButton").hide();
            $("#repickButton").hide();
            $("#packageButton").hide();
            $("#deliverButton").hide();
            $("#outButton").hide();
            $("#backButton").hide();
            break;
    }
}