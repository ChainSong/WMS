$(document).ready(function () {
    $('#SelectedCustomerOrShipperID, #SelectedCustomerID').live('change',function () {
        var customerOrShipperID = $('#SelectedCustomerOrShipperID option:selected').val();
        var relatedCustomerID;
        if($('#Target').val() === '0'){
            relatedCustomerID = 0;
        }else{
            relatedCustomerID = $('#SelectedCustomerID option:selected').val();
        }

        $.send(
            '/System/Project/GetCustomerOrShipperSegment',
            { target : $('#Target').val(), customerOrShipperID : customerOrShipperID, relatedCustomerID : relatedCustomerID, projectID : $('#ProjectID').val()},
            function (data) {
                if (data.SegmentID === 0) {
                    $('#SelectedCustomerOrShipperSegment').val("");
                    $('#submitProjectRole').removeAttr('disabled');
                } else{
                    $('#SelectedCustomerOrShipperSegment').val(data.SegmentID);
                    $('#submitProjectRole').attr('disabled', 'disabled');
                }
            },
            function () {
                Runbow.TWS.Alert("系统出错！");
            });
    }).trigger('change');

    $('#SelectedCustomerOrShipperSegment').live('change', function () {
        var customerOrShipperID = $('#SelectedCustomerOrShipperID option:selected').val();
        var relatedCustomerID;
        if ($('#Target').val() === '0') {
            relatedCustomerID = 0;
        } else {
            relatedCustomerID = $('#SelectedCustomerID option:selected').val();
        }
        var segmentID = $('#SelectedCustomerOrShipperSegment option:selected').val();
        if (segmentID === '') {
            $('#submitProjectRole').removeAttr('disabled');
            return;
        }
        $.send(
            '/System/Project/GetCustomerOrShipperSegment',
            { target: $('#Target').val(), customerOrShipperID: customerOrShipperID, relatedCustomerID: relatedCustomerID, projectID: $('#ProjectID').val() },
            function (data) {
                if (data.SegmentID === parseInt(segmentID)) {
                    $('#submitProjectRole').attr('disabled', 'disabled');
                } else {
                    $('#submitProjectRole').removeAttr('disabled');
                }

            },
            function () {
                Runbow.TWS.Alert("系统出错！");
            });
    });

    //$('#SelectedCustomerOrShipperID').change(function () {
    //    var data = $('#SelectedCustomerOrShipperID option:selected').val();
    //    var segmentID = data.split('|')[2];
    //    if (segmentID === '0') {
    //        $('#SelectedCustomerOrShipperSegment').val('');
    //    } else {
    //        $('#SelectedCustomerOrShipperSegment').val(segmentID);
    //    }
    //}).trigger('change');

    //$('#SelectedCustomerOrShipperSegment').change(function () {
    //    var data = $('#SelectedCustomerOrShipperID option:selected').val();
    //    var originalSegmentID = data.split('|')[2];
    //    var currentSegmentID = $('#SelectedCustomerOrShipperSegment option:selected').val();
    //    if ((originalSegmentID === currentSegmentID) || (originalSegmentID === '0' && currentSegmentID == '')) {
    //        $('#submitProjectRole').attr('disabled', 'disabled');
    //    } else {
    //        $('#submitProjectRole').removeAttr('disabled');
    //    }

    //}).trigger('change');

    $('#submitProjectRole').click(function () {
        var customerOrShipperID = $('#SelectedCustomerOrShipperID option:selected').val();
        var target = $('#Target').val();
        var projectID = $('#ProjectID').val();
        var relatedCustomerID;
        var msg;
        if (target === '0') {
            relatedCustomerID = 0;
            msg = '客户';
        } else {
            relatedCustomerID = $('#SelectedCustomerID option:selected').val();
            msg = '承运商';
        }
        var segmentID = $('#SelectedCustomerOrShipperSegment option:selected').val();

        if (window.confirm("修改" + msg + '段位设置，将自动作废该' + msg + '在此段位上的报价设置并启用新段位之前的报价,是否继续？')) {
            $.send(
            $(document.forms[0]).attr('action'),
            { customerOrShipperID: customerOrShipperID, segmentID: segmentID, target: target, projectID: projectID, relatedCustomerID: relatedCustomerID },
            function (data) {
                if (data.result) {
                    $('#submitProjectRole').attr('disabled', 'disabled');
                    Runbow.TWS.Alert(msg + "段位设置成功");
                } else {
                    Runbow.TWS.Alert(msg + "段位设置失败");
                }
            },
            function () {
                Runbow.TWS.Alert("段位设置失败！");
            });
        }


        //var projectShipperOrCustomerID = $('#SelectedCustomerOrShipperID option:selected').val().split('|')[0];
        //var oldSegmentID = $('#SelectedCustomerOrShipperID option:selected').val().split('|')[2];
        //var customerOrShipperID = $('#SelectedCustomerOrShipperID option:selected').val().split('|')[1];

        //if (oldSegmentID === '') {
        //    oldSegmentID = '0';
        //}

        //var segmentID = $('#SelectedCustomerOrShipperSegment option:selected').val();
        //if (segmentID === '') {
        //    segmentID = '0';
        //}

        //var target = $('#Target').val();
        //var projectID = $('#ProjectID').val();
        //var msg = '';
        //if (target === '0') {
        //    msg = '客户';
        //} else {
        //    msg = '承运商';
        //}

        //if (window.confirm("修改" + msg + '段位设置，将自动作废该' + msg + '在此段位上的报价设置并启用新段位之前的报价,是否继续？')) {
        //    $.send(
        //    $(document.forms[0]).attr('action'),
        //    { projectShipperOrCustomerID: projectShipperOrCustomerID, customerOrShipperID: customerOrShipperID, oldSegmentID: oldSegmentID, newSegmentID: segmentID, target: target, projectID: projectID },
        //    function (data) {
        //        if (data.result) {
        //            $('#SelectedCustomerOrShipperID option:selected').attr('value', projectShipperOrCustomerID + '|' + customerOrShipperID + '|' + segmentID);
        //            $('#submitProjectRole').attr('disabled', 'disabled');
        //            Runbow.TWS.Alert(msg + "段位设置成功");
        //        } else{
        //            Runbow.TWS.Alert(msg + "段位设置失败");
        //        }
        //    },
        //    function () {
        //        Runbow.TWS.Alert("新增段位失败！");
        //    });
        //}
    });
});