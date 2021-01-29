$(document).ready(function () {
    $('#btnReturn').click(function () {
        window.location.href = "/System/Segment/List";
    });

    $('#btnCreate').click(function () {
        if (!checkInput()) {
            return;
        }

        var startVal = parseFloat($('#startVal').val());
        var endVal = parseFloat($('#endVal').val());
        var isEdit = $('#isEdit').val();
        if (isEdit === '0') {
            $.send(
                '/System/Segment/CreateDetail',
                { segmentID: $('#Segment_ID').val(), startVal: startVal, endVal: endVal, description: $('#description').val() },
                function (data) {
                    if (data.ID > 0) {
                        $('#segmentDetailTable tbody').append(
                            '<tr data-ID="' + data.ID + '"><td>' + startVal + '</td><td>' + endVal + '</td><td>' + $('#description').val()
                            + '</td><td><a id="edit' + data.ID + '" data-id="' + data.ID + '" href="#" class="editSegmentDetail">编辑</a>&nbsp;&nbsp;<a id="del' + data.ID + '" data-id="' + data.ID + '" href="#" class="delSegmentDetail">删除</a></tr>'
                            )
                        clearVal();
                    } else {
                        Runbow.TWS.Alert(data.ErrorMessage);
                    }
                },
                function () {
                    Runbow.TWS.Alert("新增段位失败！");
                });
        } else {
            $.send(
               '/System/Segment/EditDetail',
               { detailID: $('#detailID').val(), segmentID: $('#Segment_ID').val(), startVal: startVal, endVal: endVal, description: $('#description').val() },
               function (data) {
                   if (data.ID > 0) {
                       var trEdit = $('#segmentDetailTable tbody tr[data-id="' + $('#detailID').val() + '"]:first');
                       var tds = trEdit.find('td');
                       $(tds[0]).text(startVal);
                       $(tds[1]).text(endVal);
                       $(tds[2]).text($('#description').val());
                       clearVal();
                       $('#btnCreate').val('新增');
                       $('#btnCancel').hide();
                   } else {
                       Runbow.TWS.Alert(data.ErrorMessage);
                   }
               },
               function () {
                   Runbow.TWS.Alert("编辑段位失败！");
               });
        }
    });

    $('#btnCancel').click(function () {
        clearVal();
        $(this).hide();
        $('#btnCreate').val('新增');
    });

    $('#segmentDetailTable').find('.delSegmentDetail').live('click', function () {
        if (confirm("是否确定删除")) {
            var tr = $(this).parent().parent();
            $.send(
                '/System/Segment/DeleteSegmentDetail',
                { detailID: $(tr).attr('data-id') },
                function (data) {
                    if (data.ID > 0) {
                        var id = $(tr).attr('data-id');
                        tr.remove();
                        if ($('#detailID').val() === id) {
                            clearVal();
                            $('#btnCreate').val('新增');
                            $('#btnCancel').hide();
                        }
                    } else {
                        Runbow.TWS.Alert(data.ErrorMessage);
                    }
                    
                },
                function () {
                    Runbow.TWS.Alert("删除段位失败！");
                });
            
        } 
    });

    $('#segmentDetailTable').find('.editSegmentDetail').live('click', function () {
        var tds = $(this).parent().parent().find('td');
        $('#startVal').val($(tds[0]).text());
        $('#endVal').val($(tds[1]).text());
        $('#description').val($(tds[2]).text());
        $('#isEdit').val('1');
        $('#detailID').val($(this).attr('data-id'));
        $('#btnCreate').val('编辑');
        $('#btnCancel').show();
    });

    var checkInput = function () {
        if ($('#startVal').val() === '') {
            Runbow.TWS.Alert('请输入起始值');
            return false;
        }

        var startVal = parseFloat($('#startVal').val());

        if (isNaN(startVal)) {
            Runbow.TWS.Alert('起始值为数字类型');
            return false;
        }

        if ($('#endVal').val() === '') {
            Runbow.TWS.Alert('请输入结束值');
            return false;
        }

        var endVal = parseFloat($('#endVal').val());

        if (isNaN(endVal)) {
            Runbow.TWS.Alert('结束值为数字类型');
            return false;
        }

        if (startVal >= endVal) {
            Runbow.TWS.Alert('结束值必须大于起始值');
            return false;
        }

        return true;
    }

    var clearVal = function () {
        $('#startVal').val('');
        $('#endVal').val('');
        $('#description').val('');
        $('#isEdit').val('0');
        $('#detailID').val('');
    }
});