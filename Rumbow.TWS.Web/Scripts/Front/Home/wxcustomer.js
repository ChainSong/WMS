$(document).ready(function () {
    $('#resultTable').find('.updateWXCustomer').live('click', function () {
        if (confirm("确定审核吗?")) {
            var ID = $(this).attr('data-id');
            var tr = $(this).parent().parent();
            $.send(
                '/Front/Home/UpdateWXCustomer',
                { id: ID },
                function (response) {
                    if (response.IsSuccess) {
                        tr.remove();
                    }
                    Runbow.TWS.Alert(response.Message);
                },
                function () {
                    Runbow.TWS.Alert("审核用户失败！");
                });
        };
    });
});