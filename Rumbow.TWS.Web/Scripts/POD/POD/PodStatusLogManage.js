$(document).ready(function () {
    var setPageControlVal = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "PodStatusLog_" + id;
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
    };

    $('#submitButton').click(function () {
        if ($('#Str2').val().trim() === '') {
            Runbow.TWS.Alert('车牌号 不能为空');
            return false;
        }
        if ($('#Str4').val().trim() === '') {
            Runbow.TWS.Alert('车辆类型(提/干/配) 不能为空');
            return false;
        }
        if ($('#Str4').val().trim() !== '干线车辆' && $('#Str4').val().trim() !== '提货车辆' && $('#Str4').val().trim() !== '配送车辆') {
            Runbow.TWS.Alert('车辆类型(提/干/配) 只能是 干线车辆/提货车辆/配送车辆');
            return false;
        }
        if ($('#DateTime1').val().trim() === '') {
            Runbow.TWS.Alert('实际发车时间 不能为空');
            return false;
        }
        if ($('#StrDateTime2').val() === '') {
            Runbow.TWS.Alert('预计到达时间 不能为空');
            return false;
        }

        setPageControlVal();
    });

    $('#returnButton').click(function () {
        var isOuterUser = $('#IsOuterUser').val();
        if (isOuterUser === 'False') {
            window.location.href = "/POD/POD/ViewPodAll/" + podID;
        } else {
            window.location.href = "/POD/POD/ViewPodAllForOuterUser/" + podID;
        }
    });

    $('#PodStatusLogTable').find('tbody a').each(function (index) {
        $(this).click(function () {
            var tr = $(this).parent().parent();
            var podStatusLoglID = $(this).attr('data-id');
            $.send(
            '/POD/POD/DeletePodStatusLog',
            { id: podStatusLoglID },
            function (response) {
                tr.remove();
            },
            function () {
                Runbow.TWS.Alert("删除运单车辆信息失败！");
            });
        });
    });

    $('.notKeyVal').each(function (index) {
        var id = $(this).attr("id");
        var descId = "PodStatusLog_" + id;
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