$(document).ready(function () {
    var setHiddenValToControl = function () {
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            if ($('#' + descID).val() !== '') {
                $(this).val($('#' + descID).val().split(' ')[0]);
            }
        });
    };

    var setPageControlVal = function () {
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    };

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']:not(.HasAudit)");
        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

        RefreshIDs();
    });

    $("#SearchCondition_ShipperName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Pod/Pod/GetUserShipper",
                type: "POST",
                dataType: "json",
                data: { name: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#SearchCondition_ShipperID').val(ui.item.data.Value);
            $('#SearchCondition_ShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#SearchCondition_ShipperID').val('');
        }
    });

    $("#resultTable tbody input[type='checkbox']:not(.HasAudit)").live('click', function () {
        RefreshIDs();
    });


    $("#resultTable tbody #cancelAudit").live('click', function () {
        var tr = $(this).parent().parent();
        $.send(
       '/Pod/Pod/CancelAttachmentAudit',
       { attachmentID: $(this).attr('data-AttachmentID'), podID: $(this).attr('data-PodReplyDocumentID') },
       function (response) {
           if (response === "True") {
               tr.remove();
               Runbow.TWS.Alert('取消审核成功');
           } else {
               Runbow.TWS.Alert('取消审核失败！');
           }
       },
       function () {
           Runbow.TWS.Alert('取消审核失败！');
       });
    });

    $("#resultTable tbody #AddRemark").live('click', function () {
        if ($(this).prev().val() === '') {
            Runbow.TWS.Alert('请输入异常备注');
            return;
        }

        $.send(
       '/Pod/Pod/SetAttachmentRemark',
       { id: $(this).attr('data-id'), remark: $(this).prev().val() },
       function (response) {
           Runbow.TWS.Alert('异常备注添加成功');
       },
       function () {
           Runbow.TWS.Alert('异常备注添加失败');
       });
    });

    $('#auditButton').click(function () {
        var ids = $('#SelectedIDs').val();
        var name = $(this).val();
        if (ids === '') {
            Runbow.TWS.Alert('请选择回单');
            return;
        }

        $.send(
        '/POD/POD/AuditPodReplyDocumentAsync',
        { ids: ids },
        function (response) {
            for (var i = 0; i < response.length; i++) {
                var tr = $('#resultTable tr[data-SystemNumber="' + response[i].SystemNumber + '"]');
                if (tr) {
                    tr.remove();
                }
            }
            Runbow.TWS.Alert(response.length + "票回单审核成功");
        },
        function () {
            Runbow.TWS.Alert("回单审核失败！");
        });

    });

    var RefreshIDs = function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']:not(.HasAudit)");
        var length = checkBoxs.length;
        var IDs = [];
        var checked = 0;
        checkBoxs.each(function () {
            if ($(this).attr("checked") === "checked") {
                var id = { SystemNumber: $(this).attr("data-ID") };
                IDs.push(id);
                checked++;
            }
        });

        if (checked == checkBoxs.length) {
            $('#selectAll').attr("checked", "checked");
        } else {
            $('#selectAll').removeAttr("checked");
        }

        $('#SelectedIDs').val(JSON.stringify(IDs));
    }

    $('#searchButton').click(function () {
        var selectedStartTime = $('#start_ActualDeliveryDate').val();
        if (selectedStartTime === '') {
            $('#start_ActualDeliveryDate').val($('#StartTime').val());
        } else {
            var today = new Date($('#start_ActualDeliveryDate').val());
            var starDate = new Date($('#StartTime').val());
            if (today < starDate) {
                Runbow.TWS.Alert("实际发货日期起始时间必须在" + $('#StartTime').val() + "之后");
                return false;
            }
        }
        setPageControlVal();
        $('#IsForExport').val('False');
    });

    $('#exportButton').click(function () {
        var selectedStartTime = $('#start_ActualDeliveryDate').val();
        if (selectedStartTime === '') {
            $('#start_ActualDeliveryDate').val($('#StartTime').val());
        } else {
            var today = new Date($('#start_ActualDeliveryDate').val());
            var starDate = new Date($('#StartTime').val());
            if (today < starDate) {
                Runbow.TWS.Alert("实际发货日期起始时间必须在" + $('#StartTime').val() + "之后");
                return false;
            }
        }
        setPageControlVal();
        $('#IsForExport').val('True');
    });


    setHiddenValToControl();
});