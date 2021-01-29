$(document).ready(function () {
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
            //alert('1 ' + id.toString());
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
            //alert('2 ' + id.toString());
            var descId = "ASNCondition_" + id.substr(0, id.length - 4) + "ID";
            $('#' + descId).val($(this).val());
        });
        $(".calendarRange").each(function (index) {
            var id = $(this).attr('id');
            //alert('3 ' + id.toString());
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
    $('#searchButton').click(function () {
        setPageControlVal();
    });
    $("#resultTable tr").dblclick(function () {
        var AsnNumber = $(this).children()[0].innerText;
        closePopup(AsnNumber);
    });
})

