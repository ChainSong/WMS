$(document).ready(function () {
    var setPageControlVal = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "PodStatusTrack_" + id;
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
        setPageControlVal();
        var Status = ['提货', '干线', '配送'];
        var TrackStatus = ['订单下达', '提货调车', '到车情况', '装车情况', '离开情况', '到达HUB', '干线配载', '干线发车', '干线跟踪', '到达终端', '配送调车', '配送跟踪', '运单签收', '回单上传']

        if ($.trim($('#Str1').val()) === '') {
            Runbow.TWS.Alert('运单状态不能为空');
            return false;
        }

        var i1 = Status.length;
        var StatusOk = false;
        while (i1--) {
            if (Status[i1] === $.trim($('#Str1').val())) {
                StatusOk = true;
                break;
            }
        }

        if (!StatusOk) {
            Runbow.TWS.Alert('运单状态不合法');
            return false;
        }

        if ($.trim($('#Str2').val()) === '') {
            Runbow.TWS.Alert('跟踪状态不能为空');
            return false;
        }

        var i2 = TrackStatus.length;
        var TrackStatusOk = false;
        while (i2--) {
            if (TrackStatus[i2] === $.trim($('#Str2').val())) {
                TrackStatusOk = true;
                break;
            }
        }

        if (!TrackStatusOk) {
            Runbow.TWS.Alert('跟踪状态不合法');
            return false;
        }


        if ($.trim($('#DateTime1').val()) === '') {
            Runbow.TWS.Alert('运时间不能为空');
            return false;
        }

        return true;
    });

    $('#returnButton').click(function () {
        var isOuterUser = $('#IsOuterUser').val();
        if (isOuterUser === 'False') {
            window.location.href = "/POD/POD/ViewPodAll/" + podID;
        } else {
            window.location.href = "/POD/POD/ViewPodAllForOuterUser/" + podID;
        }
    });

    $('#PodStatusTrackTable').find('tbody a').each(function (index) {
        $(this).click(function () {
            var tr = $(this).parent().parent();
            var podStatusTrackID = $(this).attr('data-id');
            $.send(
            '/POD/POD/DeletePodStatusTrack',
            { id: podStatusTrackID },
            function (response) {
                tr.remove();
            },
            function () {
                Runbow.TWS.Alert("删除运单状态跟踪失败！");
            });
        });
    });

    $('.notKeyVal').each(function (index) {
        var id = $(this).attr("id");
        var descId = "PodStatusTrack_" + id;
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