$(document).ready(function () {
    var setHiddenValToControl = function () {
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID;
            if (pref === 'start') {
                descID = actualID;
            }
            else {
                descID = 'End' + actualID;
            }
            $(this).val($('#' + descID).val());
        });
    };

    var setPageControlVal = function () {
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID;
            if (pref === 'start') {
                descID = actualID;
            }
            else {
                descID = 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    };

    $('#searchButton').click(function () {
        setPageControlVal();
    });

    setHiddenValToControl();
    $('.input-validation-error').removeClass('input-validation-error');

    $('.ArrivalInNormalClass').each(function (i, k) {
        $(this).val($(this).attr('val'));
    });

    $('.ActualArrivalDateCaledar').live('change', function () {
        var actualArrivateDate = $(this).val();
        var tr = $(this).parent().parent();
        var differenceDate = tr.find('#DifferenceDate');
        var ArrivalInNormal = tr.find('#ArrivalInNormal');
        var tmpDate = $(this).next().val();
        if (actualArrivateDate !== '' && tmpDate !== '') {
            var date1 = new Date(actualArrivateDate);
            var date2 = new Date(tmpDate);
            var day = parseInt((date1 - date2) / 86400000);
            differenceDate.val(day);
            if (day <= 0) {
                ArrivalInNormal.val("Y");
            } else {
                ArrivalInNormal.val("N");
            }
        }
    });
    $('.TrackClass').live('click', function () {
        var tr = $(this).parent().parent().parent();
        if (tr) {

            var podID = $(this).attr('data-PODID'), systemNumber = $(this).attr('data-SystemNumber'), customerOrderNumber = $(this).attr('data-CustomerOrderNumber');
            var trackDateObj = tr.find('#TrackDate' + podID), locationObj = tr.find('#Location'), goodStatusObj = tr.find('#GoodsStatus'), csusesOfDelaysObj = tr.find('#CausesOFDelays'), csusesOfDelaysTypeObj = tr.find('#CausesOFDelaysType');
            var trackDate = trackDateObj.val(), location = locationObj.val(), goodStatus = goodStatusObj.val(), csusesOfDelays = csusesOfDelaysObj.val(), csusesOfDelaysType = csusesOfDelaysTypeObj.val();
            var lastTrackMessage = tr.find('#lastTrackMessage');
            if (location.trim() === '') {
                Runbow.TWS.Alert('请为运单"' + customerOrderNumber + '"填入在途位置');
                return;
            }
            if (csusesOfDelays.trim() !== '' && csusesOfDelaysType.trim() ==='') {
                Runbow.TWS.Alert('请为运单"' + customerOrderNumber + '选择延误原因分类');
                return;
            }
           
            $.send(
            '/POD/Hilti/AddPodTrack',
            { podID: podID, systemNumber: systemNumber, customerOrderNumber: customerOrderNumber, trackDate: trackDate, location: location, goodsStatus: goodStatus, csusesOfDelays: csusesOfDelays, causesOFDelaysType: csusesOfDelaysType },
            function (response) {
                if (response.TrackID) {
                    trackDateObj.val(new Date().Format('yyyy-MM-dd'));
                    locationObj.val('');
                    goodStatusObj.val('在途');
                    csusesOfDelaysObj.val('');
                    csusesOfDelaysTypeObj.val('仓库导致');
                    Runbow.TWS.Alert('运单"' + customerOrderNumber + '"跟踪新增成功');
                    lastTrackMessage.text(trackDate + " " + location + " " + goodStatus + " " + csusesOfDelays + " " + csusesOfDelaysType);
                    csusesOfDelaysTypeObj.val('');
                }
                
            },
            function () {
                Runbow.TWS.Alert('运单"' + customerOrderNumber + '"跟踪新增失败！');
            });

        }
    });

    $('.ReplyDocumentClass').live('click', function () {
        var tr = $(this).parent().parent().parent();
        if (tr) {
            var replyDocumentID = $(this).attr('data-PodReplyDocumentID'), podID = $(this).attr('data-PODID'), systemNumber = $(this).attr('data-SystemNumber'), customerOrderNumber = $(this).attr('data-CustomerOrderNumber');
            var actualArrivalDateObj = tr.find('#ActualArrivalDate'+podID), arrivalInNormalObj = tr.find('#ArrivalInNormal'), differenceDateObj = tr.find('#DifferenceDate'), signPeopleObj = tr.find('#SignPeople');
            var actualArrivalDate = actualArrivalDateObj.val(), arrivalInNormal = arrivalInNormalObj.val(), differenceDate = differenceDateObj.val(), signPeople = signPeopleObj.val();
            var lastTrackMessage = tr.find('#lastTrackMessage');
            if (replyDocumentID === '' && actualArrivalDate.trim() === '') {
                Runbow.TWS.Alert('请为运单"' + customerOrderNumber + '"填入实际到货日期');
                return;
            }

            if (replyDocumentID === '' && signPeople.trim() === '') {
                Runbow.TWS.Alert('请为运单"' + customerOrderNumber + '"填入实际签收人');
                return;
            };

            $.send(
            '/POD/Hilti/AddPodReplyDocument',
            {
                podID: podID, systemNumber: systemNumber, customerOrderNumber: customerOrderNumber,
                actualArrivalDate: actualArrivalDate, arrivalInNormal: arrivalInNormal, differenceDate: differenceDate, signPeople: signPeople, replyDocumentID: replyDocumentID
            },
            function (response) {
                if (response.ReplyDocumentID) {
                    if (replyDocumentID === '') {
                        Runbow.TWS.Alert('运单"' + customerOrderNumber + '"回单信息新增成功');
                        $.send(
                        '/POD/Hilti/GetPodLastTrack',
                        {podID: podID},
                        function (resp) {                            
                            if (resp && lastTrackMessage) {
                                lastTrackMessage.text(resp.DateTime1 + " " + resp.Str1 + " " + resp.Str2 + " " + resp.Str3);
                            }
                        });
                    } else {
                        Runbow.TWS.Alert('运单"' + customerOrderNumber + '"回单信息更新成功');
                    }

                }

            },
            function () {
                if (replyDocumentID === '') {
                    Runbow.TWS.Alert('运单"' + customerOrderNumber + '"回单信息新增失败');
                } else {
                    Runbow.TWS.Alert('运单"' + customerOrderNumber + '"回单信息更新失败');
                }
            });

        }
    });


    $('#resultTable').find('.deletePod').live('click', function () {
        var podID = $(this).attr('data-id');
        var tr = $(this).parent().parent();
        $.send(
            '/POD/POD/DeletePOD',
            { id: podID },
            function (response) {
                if (response.IsSuccess) {
                    tr.remove();

                }
                Runbow.TWS.Alert(response.Message);
            },
            function () {
                Runbow.TWS.Alert("删除运单失败！");
            });
    });

});