$(document).ready(function () {
    var invoiceID = $('#Invoice_ID').val();
    var invoiceNumber = $('#Invoice_InvoiceNumber').val();
    var target = $('#Invoice_Target').val();
    var relatedCustomerID = $('#Invoice_RelatedCustomerID').val();
    var customerOrShipperID = $('#Invoice_CustomerOrShipperID').val();
    var customerOrShipperName = $('#Invoice_CustomerOrShipperName').val();
    var remainAmtTd = $('#invoiceTable tr:eq(1) td:eq(3)');
    var remainAmt;
    if ($(remainAmtTd).text() === '') {
        remainAmt = 0;
    } else {
        remainAmt = parseFloat($(remainAmtTd).text());
    }

    $('#historyTable').find('.deleteReceiveOrPay').live('click', function () {
        if (window.confirm('您确认删除此记录？')) {
            var id = $(this).attr('data-id');
            var tr = $(this).parent().parent();
            var amtTemp = $(tr).children('td').eq(1);
            var amt = parseFloat(amtTemp.text().substring(1));
            $.send(
                '/Finance/ReceiveOrPayOrders/DeleteReceiveOrPayOrder',
                { id: id },
                function (response) {
                    if (response.IsSuccess) {
                        remainAmt += amt;
                        $(remainAmtTd).text(remainAmt.toString());
                        tr.remove();
                    }
                    else {
                        Runbow.TWS.Alert(response.Message);
                    }
                },
                function () {
                    Runbow.TWS.Alert("删除失败！");
                });
        }
    });

    $('#confirmButton').live('click', function () {
        var completeState = $(this).attr('value');
        var button = $(this);
        var message = completeState === '确认完成' ? "您确认完成此发票" : "您确认取消完成此发票";
        var currentState = completeState === '确认完成' ? false : true;
        if (window.confirm(message)) {
            $.send(
                '/Finance/ReceiveOrPayOrders/CancelOrCompleteInvoice',
                { id: invoiceID, currentState: currentState },
                function (response) {
                    if (response.IsSuccess) {
                        if (!currentState) {
                            $('#ReceiveOrPayAction').hide();
                            button.attr('value', '取消确认');
                            $('#submitButton').hide();
                        } else {
                            $('#ReceiveOrPayAction').show();
                            button.attr('value', '确认完成');
                            $('#submitButton').show();
                        }
                    }
                    Runbow.TWS.Alert(response.Message);
                },
                function () {
                    Runbow.TWS.Alert("操作失败！");
                });
        }
    });

    $('#submitButton').live('click', function () {
        if (!checkInput()) {
            return;
        }
        $.send(
                '/Finance/ReceiveOrPayOrders/AddReceiveOrPayOrders',
                { InvoiceID: invoiceID, InvoiceNumber: invoiceNumber, Target: target, CustomerOrShipperID: customerOrShipperID, CustomerOrShipperName: customerOrShipperName, AMT: $('#ReceiveOrPayAmt').val().trim(), Date: $('#ReceiveOrPayDate').val().trim(), Remark: $('#ReceiveOrPayRemark').val().trim(), RelatedCustomerID: relatedCustomerID },
                function (response) {
                    if (response.IsSuccess) {
                        $('#historyTable tbody').append(
                            '<tr><td>' + $('#ReceiveOrPayDate').val().trim() + '</td><td>￥' + $('#ReceiveOrPayAmt').val().trim() + '</td><td>' + $('#ReceiveOrPayRemark').val().trim()
                            + '</td><td><a id="deleteReceiveOrPay" class="deleteReceiveOrPay" data-id="' + response.ID + '" href="#">删除</a></td></tr>'
                            );
                        remainAmt -= parseFloat($('#ReceiveOrPayAmt').val().trim());
                        $(remainAmtTd).text(remainAmt.toString());
                        clearControl();
                    }
                    else {
                        Runbow.TWS.Alert(response.Message);
                    }
                },
                function () {
                    Runbow.TWS.Alert("失败！");
                });
    });

    var clearControl = function(){
        $('#ReceiveOrPayAmt').val('');
        $('#ReceiveOrPayDate').val('');
        $('#ReceiveOrPayRemark').val('');
    };

    var checkInput = function () {
        var receiveOrPayAmt = $('#ReceiveOrPayAmt').val().trim();
        if (receiveOrPayAmt === '') {
            Runbow.TWS.Alert('请输入金额');
            return false;
        }

        if (parseFloat(receiveOrPayAmt) > remainAmt) {
            Runbow.TWS.Alert('输入金额必须小于剩余金额');
            return false;
        }

        var date = $('#ReceiveOrPayDate').val().trim();
        if (date === '') {
            Runbow.TWS.Alert('请输入日期');
            return false;
        }

        return true;
    };
});