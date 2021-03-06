﻿$(document).ready(function () {
    var setPageControlVal = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr('id');
            var descId = "PodFee_" + id;

            if ($(this).attr("type") === "checkbox") {
                var isChecked = document.getElementById(id).checked;
                if (isChecked) {
                    $('#' + descId).val('1');
                } else {
                    $('#' + descId).val('0');
                }
            } else {
                $('#' + descId).val($(this).val());
            }
        });
    };

    $('#submitButton').click(function () {
        setPageControlVal();
    });

    $('#returnButton').click(function () {
        window.history.back();
    });

    $('.notKeyVal').each(function (index) {
        var id = $(this).attr("id");
        var descId = "PodFee_" + id;
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
            if ($(this).hasClass('datetimeval') && $(this).val() !== '') {
                $(this).val($(this).val().split(' ')[0]);
            }
        }
    });
});