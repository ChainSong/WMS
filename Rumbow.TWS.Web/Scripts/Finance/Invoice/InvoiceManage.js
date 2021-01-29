$(document).ready(function () {
    $('#SearchCondition_CustomerOrShipperName').autocomplete({
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
            $('#SearchCondition_CustomerOrShipperID').val(ui.item.data.Value);
            $('#SearchCondition_CustomerOrShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#SearchCondition_CustomerOrShipperID').val('');
        }
    });

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

    $('#searchButton').click(function () {
        setPageControlVal();
    });

    $('#resultTable').find('.CompleteOrCancel').live('click', function () {
        var id = $(this).attr('data-id');
        var completeState = $(this).attr('data-currentCompleteState');
        var message = completeState === 'False' ? "您确认完成此发票" : "您确认取消完成此发票";
        if (window.confirm(message)) {
            var tr = $(this).parent().parent();
            $.send(
                '/Finance/ReceiveOrPayOrders/CancelOrCompleteInvoice',
                { id: id,currentState:completeState },
                function (response) {
                    if (response.IsSuccess) {
                        tr.remove();
                    }
                    Runbow.TWS.Alert(response.Message);
                },
                function () {
                    Runbow.TWS.Alert("操作失败！");
                });
        }
    });
    $('#resultTable').find('.Delete').live('click', function () {
        if (window.confirm('您确认作废此发票？')) {
            var id = $(this).attr('data-id');
            var tr = $(this).parent().parent();
            $.send(
                '/Finance/Invoice/DeleteInvoice',
                { id: id},
                function (response) {
                    if (response.IsSuccess) {
                        tr.remove();
                    }
                    Runbow.TWS.Alert(response.Message);
                },
                function () {
                    Runbow.TWS.Alert("发票作废失败！");
                });
        }
    });

    $('.editInvoiceNumber').live('click', function () {
        showDialog($(this).attr('data-id'));
    });

    var showDialog = function (id) {
        var opts = {
            'title': '发票号更新',
            'content': $('#showInDialog').clone().show(),
            'buttons': {
                "确定": function () {
                    $.send(
                        '/Finance/Invoice/UpdateInvoiceNumber',
                        { id: id, invoiceNumber: Runbow.TWS.Popup.find('#InvoiceNumber').val() },
                        function (response) {
                            $("#resultTable tbody tr[data-id='" + id + "']").children('td').eq(1).html(Runbow.TWS.Popup.find('#InvoiceNumber').val());                            
                            Runbow.TWS.Popup.close();
                        },
                        function () {
                            Runbow.TWS.Popup.close();
                    });
                },
                "取消": function () {
                    Runbow.TWS.Popup.close();
                }
            },
            'width': '400',//default 400
            'minHeight': '200' //default 300
        };
        Runbow.TWS.Popup.show(opts);
    };

    setHiddenValToControl();

});