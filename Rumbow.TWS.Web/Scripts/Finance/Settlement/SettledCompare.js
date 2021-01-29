var fileImportClick = function (type) {
    if ($('#importExcel').val() === '') {
        Runbow.TWS.Alert("请选择要导入的Excel");
        return false;
    }

    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) === null) {
        Runbow.TWS.Alert("请选择Excel格式的文件");
        $('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    }

    return true;
};

$(document).ready(function () {
    $('#ShipperName').autocomplete({
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
            $('#ShipperID').val(ui.item.data.Value);
            $('#ShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#ShipperID').val('');
        }
    });

    $('#resultTable tr input[type=text]').live('keyup', function () {
        var td = $(this).parent().parent();
        var tr = td.parent();
        var shipAmt = parseFloat(td.find('#ShipAmt').val());
        var pointAmt = parseFloat(td.find('#PointAmt').val());
        var bafAmt = parseFloat(td.find('#BAFAmt').val());
        var otherAmt = parseFloat(td.find('#OtherAmt').val());
        var CompareTotalAmt = parseFloat(td.find('label[for=CompareTotalAmt]').text());

        if (isNaN(shipAmt)) {
            shipAmt = 0;
        }

        if (isNaN(pointAmt)) {
            pointAmt = 0;
        }

        if (isNaN(bafAmt)) {
            bafAmt = 0;
        }

        if (isNaN(otherAmt)) {
            otherAmt = 0;
        }

        var totalAmt = shipAmt + pointAmt + bafAmt + otherAmt;

        if (CompareTotalAmt === totalAmt) {
            tr.removeClass('Red');
        } else {
            tr.addClass('Red');
        }

        td.find('label[for=TotalAmt]').text(totalAmt);
    });

    $('#FollowTheImport').live('click', function () {
        var buttonTd = $(this).parent();
        var remark = buttonTd.find('#Remark').val();
        var inputTD = $(this).parent().prev();
        var CompareShipAmt = inputTD.find('label[for=CompareShipAmt]').text();
        var ComparePointAmt = inputTD.find('label[for=ComparePointAmt]').text();
        var CompareBafAmt = inputTD.find('label[for=CompareBAFAmt]').text();
        var CompareOtherAmt =inputTD.find('label[for=CompareOtherAmt]').text();
        var CompareTotalAmt = inputTD.find('label[for=CompareTotalAmt]').text();
        var id = buttonTd.parent().attr('data-id');
        
        $.send(
                '/Finance/Settlement/EditSettledPod',
                { id: id, shipAmt: CompareShipAmt, bafAmt: CompareBafAmt, pointAmt: ComparePointAmt, otherAmt: CompareOtherAmt, remark: remark,settledType: $('#SettledType').val()},
                function (response) {
                    if (response === '运单结算修改成功') {
                        inputTD.find('#ShipAmt').val(CompareShipAmt);
                        inputTD.find('#PointAmt').val(ComparePointAmt);
                        inputTD.find('#BAFAmt').val(CompareBafAmt);
                        inputTD.find('#OtherAmt').val(CompareOtherAmt);
                        inputTD.find('label[for=TotalAmt]').text(CompareTotalAmt)
                        buttonTd.parent().removeClass('Red');
                    } else {
                        Ronbow.TWS.Alert(response);
                    }
                },
                function () {
                    Runbow.TWS.Alert("运单结算修改失败");
                });

    });

    $('#FollowTheManual').live('click', function () {
        var buttonTd = $(this).parent();
        var remark = buttonTd.find('#Remark').val();
        var inputTD = $(this).parent().prev();
        var shipAmt = parseFloat(inputTD.find('#ShipAmt').val());
        var pointAmt = parseFloat(inputTD.find('#PointAmt').val());
        var bafAmt = parseFloat(inputTD.find('#BAFAmt').val());
        var otherAmt = parseFloat(inputTD.find('#OtherAmt').val());
        
        if (isNaN(shipAmt)) {
            inputTD.find('#ShipAmt').val('');
            Runbow.TWS.Alert('运费格式不正确，请重新输入');
            return;
        }

        if (isNaN(pointAmt)) {
            inputTD.find('#PointAmt').val('');
            Runbow.TWS.Alert('点费格式不正确，请重新输入');
            return;
        }

        if (isNaN(bafAmt)) {
            inputTD.find('#BAFAmt').val('');
            Runbow.TWS.Alert('燃油附加费格式不正确，请重新输入');
            return;
        }

        if (isNaN(otherAmt)) {
            inputTD.find('#OtherAmt').val('');
            Runbow.TWS.Alert('其他费用格式不正确，请重新输入');
            return;
        }

        var totalAmt = inputTD.find('label[for=TotalAmt]').text();
        var id = buttonTd.parent().attr('data-id');
        $.send(
                '/Finance/Settlement/EditSettledPod',
                { id: id, shipAmt: inputTD.find('#ShipAmt').val(), bafAmt: inputTD.find('#BAFAmt').val(), pointAmt: inputTD.find('#PointAmt').val(), otherAmt: inputTD.find('#OtherAmt').val(), remark: remark, settledType: $('#SettledType').val() },
                function (response) {
                    Runbow.TWS.Alert(response);
                },
                function () {
                    Runbow.TWS.Alert("运单结算修改失败");
                });
    });
});