$(document).ready(function () {
    $('#btnSave').click(function () {

        setPageControlVal();
    });
    $('.calendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = 'SearchCondition_';
        if (pref === 'start') {
            descID += 'Stat' + actualID;
        }
        else {
            descID += 'End' + actualID;
        }
        $(this).val($('#' + descID).val());
    });
    var setPageControlVal = function () {
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += 'Stat' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    }
    $("#birth").click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                sql += "'" + checkBoxs[i].id.toString().substring(5, checkBoxs[i].id.length) + "',";
            }
        }
        if (sql.length > 0) {
            sql.toString().substring(0, sql.toString().length - 1);
        }
        if (sql != "" && sql != "undefined") {
            $.send(
               '/AMS/AMS/GenBoxNumbers',
               { sql: sql },
               function (response) {
                   if (response != null) {
                       for (var i = 0; i < checkBoxs.length; i++) {
                           if (checkBoxs[i].checked) {
                               for (var j = 0; j < response.length; j++) {
                                   var td = checkBoxs[i].id.toString().substring(5, checkBoxs[i].id.length);
                                   if (td == response[j].ID) {
                                       $("#" + td + "").html(response[j].OrderNo);
                                   }
                                
                               }
                           }
                       }
                       Runbow.TWS.Alert("生成装箱单成功");
                   }
                   else {
                       Runbow.TWS.Alert("生成装箱单失败！！");
                   }
               });
        }
        else {
            Runbow.TWS.Alert("您还没有选择！");
        }
    })
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }
    });


});