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
    });
    //查询条件
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
    var setPageControlVal = function () {
        //$('.notKeyVal').each(function (index) {
        //    var id = $(this).attr("id");
        //    var descId = "ASNCondition_" + id;
        //    if ($(this).attr("type") === "checkbox") {
        //        var isChecked = document.getElementById(id).checked;
        //        if (isChecked) {
        //            $('#' + descId).val("1");
        //        } else {
        //            $('#' + descId).val("0");
        //        }
        //    } else {
        //        $('#' + descId).val($(this).val());
        //    }
        //});

        $('.DropDownList').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id.substr(0, id.length - 4) + "ID";
            $('#' + descId).val($(this).val());
        });

        $(".calendarRange").each(function (index) {
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
    };
    $('#searchButton').click(function () {
        setPageControlVal();
    });

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
function PrintBarCode(ID) {
    window.location.href = "/WMS/ReceiptManagement/PrintBarCode?IDS="+ID;
}
function BatchPrintBarCode() {
    var str = '';
    $('.checkForSelect').each(function (index) {
        if ($(this).attr('checked') === 'checked') {
            str += "'" + $(this).attr('data-id') + "'" + ",";
        }
    });
    if (str == "") {
        showMsg("请选择打印条码", "4000");
        return;
    }
    window.location.href = "/WMS/ReceiptManagement/PrintBarCode?IDS=" + str.substring(0, str.length - 1);
}