$(document).ready(function () {
    $('#resultTable').find('.ClosePOD').live('click', function () {
        if (confirm("确定关闭吗，关闭后，数据将不能被RF枪下载?")) {
            var ID = $(this).attr('data-id');
            var tr = $(this).parent().parent();
            $.send(
                '/POD/Adidas/ClosePOD',
                { id: ID },
                function (response) {
                    if (response.IsSuccess) {
                        tr.remove();
                    }
                    Runbow.TWS.Alert(response.Message);
                },
                function () {
                    Runbow.TWS.Alert("关闭操作失败，请重试！");
                });
        };
    });
});