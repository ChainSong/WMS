

$(document).ready(function () {
    if ($('#SearchCondition_CustomerID').val() == "") {
        $('#SearchCondition_CustomerID option:first').next().attr("selected", "selected");

    } else {

    }
    $('#addButton').live('click', function () {
        window.location.href = "/WMS/OrderECManagement/PrintDetail?ViewType=1";
    });
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
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

    });

    $('#printButton').live('click', function () {
        var bl = IsPrint();

        var id = TableToJson();

        var href = "";
        if ($("#ProjectName").val() == "YXDR") {
            href = '/WMS/OrderECManagement/PrintHeaderAndDetailYXDR?id=' + id + "&Flag=4";
        }
        else {
            href = '/WMS/OrderECManagement/PrintHeaderAndDetail?id=' + id + "&Flag=4";
        }

        if (bl) {
            layer.confirm('<font size="4">订单已打印，是否重新打印？</font>', {
                btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                //shade: [0.8, '#393D49'],
                title: ['提示', 'font-size:18px;']
                //按钮
            }, function (index) {
                layer.close(index);
                // var isPrint = PrintStatus();
                if (id == "") {
                    showMsg("请选择需要打印的拣货单！", 4000);
                    return false;
                }
                else {
                    window.location.href = href;
                }
            });
        }
        else {
            //var id = TableToJson();
            // var isPrint = PrintStatus();
            if (id == "") {
                showMsg("请选择需要打印的拣货单！", 4000);
                return false;
            }
            else {
                window.location.href = href;
            }
        }
    });

    //根据波次打印拣货单，为了显示分拣的几号箱子
    $('#printWaveButton').live('click', function () {
        var id = TableToJson();
        var href = "";
        if ($('#ProjectName').val() == '爱库存') {
            href = "/WMS/OrderECManagement/PrintWaveOrderAKC?ids=" + id + "";
        } else {
            href = "/WMS/OrderECManagement/PrintWaveOrderAKC?ids=" + id + "";
        }
        if (id == "") {
            showMsg("请选择需要打印的快递单！", 4000);
            return false;
        }
        else {
            window.location.href = href;
        }
    })

    //打印快递单（根据打印关联表打印）
    $('#printExpresss').live('click', function () {

        var id = TableToJson();
        var href = "";
        if ($("#ProjectName").val() == "爱库存") {
            href = "/WMS/OrderECManagement/PrintExpressAKC?id=" + id + "&type=2";
        }
        else {
            href = "/WMS/OrderECManagement/PrintExpressAKC?id=" + id + "&type=2";
        }
        if (id == "") {
            showMsg("请选择需要打印的快递单！", 4000);
            return false;
        }
        else {
            window.location.href = href;
        }


    });



});

//打印的ID
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



function IsPrint() {

    var bl = false;
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");

    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked && $($("#" + checkBoxs[i].name + " input[type='hidden']")[1]).val() == 1) {
            bl = true;
        }
    }
    return bl;
}