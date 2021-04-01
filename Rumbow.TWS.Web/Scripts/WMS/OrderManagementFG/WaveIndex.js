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
    $(".numberCheck").live('keydown', function () {
        replaceNotNumber(this);
    });
    $(".numberCheck").live('keyup', function () {
        replaceNotNumber(this);
    });
    if ($("#resultTable tbody")[0].rows.length < 1) {
        $("#resultTable").removeAttr("style");
        $("#tables").removeAttr("style");
        $("#OperateTh").removeAttr("style");
    }

    $('#printButton').live('click', function () {
        layer.confirm('<font size="4">确认是否打印波次拣货单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var id = TableToJson();
            var WaveCheck = TableWaveCheck();
            if (id == "") {
                showMsg("请选择需要打印的拣货单！", 2000);
                return false;
            }
            if (WaveCheck == "wrong") {
                showMsg("订单波次为空或者不一致,请检查！", 4000);
                return false;
            }
            //var WaveNumber = $("#resultTable tbody input[type='checkbox']:checked")[0].dataset.name;
            window.location.href = '/WMS/OrderManagement/PrintOrder_Wave?id=' + id;

        });

    });
    $('#PrintPickGoodsOrder').live('click', function () {
        layer.confirm('<font size="4">确认是否打印批次拣货单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var id = TableToJson();
            var WaveCheck = TableWaveCheck();
            if (id == "") {
                showMsg("请选择需要打印的拣货单！", 2000);
                return false;
            }
            if (WaveCheck == "wrong") {
                showMsg("订单波次为空或者不一致,请检查！", 4000);
                return false;
            }
            //var WaveNumber = $("#resultTable tbody input[type='checkbox']:checked")[0].dataset.name;
            window.location.href = '/WMS/OrderManagement/PrintPickGoodsOrder?ids=' + id;

        });

    });
    $('#searchButton').click(function () {
        setPageControlVal();
    });
    $('#AddWaveButton').live('click', function () {
        var id = TableToJson();
        if (id == "") {
            showMsg("请选择订单！", 2000);
            return false;
        }
        var myDate = new Date();
        //获取当前年
        var year = myDate.getFullYear();
        //获取当前月
        var month = myDate.getMonth() + 1;
        //获取当前日
        var date = myDate.getDate();
        var h = myDate.getHours();       //获取当前小时数(0-23)
        var m = myDate.getMinutes();     //获取当前分钟数(0-59)
        var s = myDate.getSeconds();

        var now = year.toString() + p(month).toString() + p(date).toString() + p(h).toString() + p(m).toString() + p(s).toString();
        var WaveNumber = "WAVE" + $("#UserInfo").val() + now;
        $("#WaveNumberText").val(WaveNumber);
        layer.open({
            type: 1,
            title: '确认是否使用下面波次号？',
            area: ['400px', '200px'],
            fixed: false, //不固定
            maxmin: false,
            content: $('#WaveNumberDiv')
        });

        //layer.confirm('<font size="4">确认是否对下面订单分配波次？</font>', {
        //    btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //    title: ['提示', 'font-size:18px;']
        //}, function (index) {
        //    layer.close(index);
        //    $.ajax({
        //        url: "/WMS/OrderManagement/AllocatedWave",
        //        type: "POST",
        //        dataType: "json",
        //        data: {
        //            IDS:id
        //        },
        //        success: function (data) {
        //            location.href = "/WMS/OrderManagement/WaveIndex/";
        //        }
        //    });
        //});
    })
    $("#WaveNumberOK").live('click', function () {
        layer.closeAll();
        var id = TableToJson();
        if (id == "") {
            showMsg("请选择订单！", 4000);
            return false;
        }
        $.ajax({
            url: "/WMS/OrderManagement/AllocatedWave",
            type: "POST",
            dataType: "json",
            data: {
                IDS: id,
                WaveNumber: $("#WaveNumberText").val()
            },
            success: function (data) {
                location.href = "/WMS/OrderManagement/WaveIndex/";
            }
        });
    });
    $("#WaveNumberCancel").live('click', function () {
        layer.closeAll();
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

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

    });

});


$('select[id=SearchCondition_CustomerID]').live('change', function () {
    window.location.href = "/WMS/OrderManagement/WaveIndex/?customerID=" + $(this).val();
});




$("#statusBackReturn").live('click', function () {
    closePopup();
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
        showMsg("请勾选出库单！", 2000);

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
function TableWaveCheck() {
    var a = "";
    var checkBoxs = $("#resultTable tbody input[type='checkbox']:checked");
    for (var i = 0; i < checkBoxs.length; i++) {
        if ($(checkBoxs[i]).data().name == "") {
            a = "wrong";
        }
        for (var j = 0; j < checkBoxs.length; j++) {
            if ($(checkBoxs[i]).data().name != $(checkBoxs[j]).data().name) {
                a = "wrong";
            }
        }
    }
    return a;
}
function p(s) {
    return s < 10 ? '0' + s : s;
}
function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    //var re = new RegExp(pattern);
    if (pattern.test(hehe.value)) {

        hehe.value = hehe.value.replace(pattern, "");
    }
}