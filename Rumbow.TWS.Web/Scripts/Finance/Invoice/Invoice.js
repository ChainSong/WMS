$(document).ready(function () {
    $('.actualTotal').live('change', function () {
        var actualMoney = parseFloat($(this).val());
        if (!actualMoney) {
            $(this).val('0.00');
            actualMoney = 0;
        }

        var shouldMoney = parseFloat($(this).parent().parent().attr('data-ShouldMoney'));
        if (!shouldMoney) {
            shouldMoney = 0;
        }

        var differenct = shouldMoney - actualMoney;

        $(this).parent().next().html('￥' + differenct);
        refresh();
    });

    $('.Reason').live('change', function () {
        refresh();
    });

    var refresh = function () {
        var actualMoneys = $("#settledPodsTable tbody input[class='actualTotal']");
        var length = actualMoneys.length;
        var Values = [];
        var sum = 0.00;
        actualMoneys.each(function () {
            var actualMoney = parseFloat($(this).val());
            sum += actualMoney;
            var id = $(this).parent().parent().attr('data-id');
            var reason = $(this).parent().parent().find('.Reason').val();
            var value = { ID: id, Amt5: actualMoney, Str5: reason };
            Values.push(value);
        });

        $('#Invoice_Sum').val(sum);
        $('#ServerValues').val(JSON.stringify(Values));
    };

    var checkInput = function () {
        if ($('#Invoice_EstimateDate').val() === '') {
            Runbow.TWS.Alert('请输入预计收款时间');
            return false;
        }

        return true;
    };

    $('#submitButton').click(function () {
        refresh();

        return checkInput();
    });

    refresh();
});