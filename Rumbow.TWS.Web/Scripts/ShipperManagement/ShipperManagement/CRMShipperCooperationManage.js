$(document).ready(function () {
    $('#insertTransportationLine').click(function () {
        if ($('#Name').val() === '') {
            Runbow.TWS.Alert('请输入客户名称');
            return;
        }
    });


    $('#resultTable').find('.deleteCRMShipperCooperation').live('click', function () {
        var id = $(this).attr('data-id');
        var tr = $(this).parent().parent();
        $.send(
            '/ShipperManagement/ShipperManagement/DeleteCRMShipperCooperation',
            { id: id },
            function (response) {
                if (response.IsSuccess) {
                    tr.remove();
                } else {
                    Runbow.TWS.Alert(response.Message);
                }
            },
            function () {
                Runbow.TWS.Alert("删除失败！");
            });
    });

    $('#return').click(function () {
        var id = $('#CRMShipperID').val();
        var ViewType = $('#ViewType').val();
        window.location.href = '/ShipperManagement/ShipperManagement/Create/' + id + '?ViewType=' + ViewType;
    });
});