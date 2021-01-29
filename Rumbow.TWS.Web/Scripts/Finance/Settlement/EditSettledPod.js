$(document).ready(function () {
    var checkInput = function () {
        var shipAmt = $('#ShipAmt').val().trim(), bafAmt = $('#BafAmt').val().trim(),
            pointAmt = $('#PointAmt').val().trim(), otherAmt = $('#OtherAmt').val().trim();
        var isNum = /^\d+(\.\d+)?$/;
        if (shipAmt === '') {
            $('#ShipAmt').val('0.00');
            shipAmt = 0;
        }

        if (bafAmt === '') {
            $('#BafAmt').val('0.00');
            bafAmt = 0;
        }

        if (pointAmt === '') {
            $('#PointAmt').val('0.00');
            pointAmt = 0;
        }

        if (otherAmt === '') {
            $('#OtherAmt').val('0.00');
            otherAmt = 0;
        }

        if (!isNum.test(shipAmt)) {
            Runbow.TWS.Alert("请输入正确的运费");
            return false;
        }

        if (!isNum.test(bafAmt)) {
            Runbow.TWS.Alert("请输入正确的燃油附加费");
            return false;
        }

        if (!isNum.test(pointAmt)) {
            Runbow.TWS.Alert("请输入正确的点费");
            return false;
        }

        if (!isNum.test(otherAmt)) {
            Runbow.TWS.Alert("请输入正确的其他费用");
            return false;
        }

        return true;
    };

    $('#editButton').click(function () {
        if (!checkInput()) {
            return;
        }

        var shipAmt = $('#ShipAmt').val().trim(), bafAmt = $('#BafAmt').val().trim(),
            pointAmt = $('#PointAmt').val().trim(), otherAmt = $('#OtherAmt').val().trim(),
            remark = $('#remark').val().trim(), settledType = $('#SettledType').val();
        var id = $('#ID').val();
        $.send(
                '/Finance/Settlement/EditSettledPod',
                { id: id, shipAmt: shipAmt, bafAmt: bafAmt, pointAmt: pointAmt, otherAmt: otherAmt, remark: remark, settledType: settledType },
                function (response) {
                    Runbow.TWS.Alert(response);
                },
                function () {
                    Runbow.TWS.Alert("编辑运单结算失败！");
                });
    });
});