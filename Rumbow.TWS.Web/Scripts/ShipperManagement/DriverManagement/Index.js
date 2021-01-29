$(document).ready(function () {
    $('#resultTable').find('#UpdateCRMDriver').live('click', function () {
        var crmID = $(this).attr('data-id');
        var tr = $(this).parent().parent();
        window.location.href = "/ShipperManagement/DriverManagement/Create?id=" + crmID + "&type=3";
        //$.send(
        //    '/CRM/Crm/CrmBasView2',
        //    { id: crmID });

    });

    $('#resultTable').find('#deleteCRMDriver').live('click', function () {
        var driverID = $(this).attr('data-id');
        var tr = $(this).parent().parent();
        $.send(
            '/ShipperManagement/DriverManagement/DeleteCRMDriver',
            { id: driverID },
            function (response) {
                if (response.IsSuccess) {
                    tr.remove();
                }
                Runbow.TWS.Alert(response.Message);
            },
            function () {
                Runbow.TWS.Alert("删除司机信息失败！");
            });
    });
    
    $('#searchButton').click(function () {
        $("#PageIndex").val('0');
        setPageControlVal();
    });

    $('.calendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = 'SearchCondition_';
        if (pref === 'start') {
            descID += 'Start' + actualID;
        }
        else {
            descID += 'End' + actualID;
        }
        $(this).val($('#' + descID).val());
    });
    var setPageControlVal = function () {
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += 'Start' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    }

});