$(document).ready(function () {

    //    $('#returnbutton').live('click', function () {
    //        window.history.go(-1);
    //        //window.location.href = '/shippermanagement/vehiclemanagement/index';
    //    });
    //})



    //$('#searchButton').click(function () {
    //    $('#PageIndex').val('0');
    //    setPageControlVal();
    //    //$('#IsForExport').val('False');
    //    //$('#ExportType').val('');
    //});


    //onclick = "javascript:window.location.href='/ShipperManagement/VehicleManagement/Create?ViewType=1'"
    $('#addButton').live('click', function () {
        window.location.href = "/ShipperManagement/VehicleManagement/Create";

    })


    $('#resultTable').find('#UpdateCRMVehicle').live('click', function () {
        var crmID = $(this).attr('data-id');
        var tr = $(this).parent().parent();
        window.location.href = "/ShipperManagement/VehicleManagement/Create?id=" + crmID + "&ViewType=3";
        //$.send(
        //    '/CRM/Crm/CrmBasView2',
        //    { id: crmID });

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


    $('#resultTable').find('#deleteCRMVehicle').live('click', function () {
        if (confirm("您确认删除此车辆？")) {
            var vehicleID = $(this).attr('data-id');
            var tr = $(this).parent().parent();
            $.send(
                '/ShipperManagement/VehicleManagement/DeleteCRMVehicle',
                { id: vehicleID },
                function (response) {
                    if (response.IsSuccess) {
                        tr.remove();
                    }
                    Runbow.TWS.Alert(response.Message);
                },
                function () {
                    Runbow.TWS.Alert("删除车辆信息失败！");
                });
        }
    });


    //$('#sijifenpei').click(function () {
    //    window.location.href = "/ShipperManagement/VehicleManagement/VehicleToDriver";
    //    var carno = $(this).attr('data-name');
    //    $('#VehicleNo').val() == carno;


    //});

});



//$('#resultTable').find('.deletePod').live('click', function () {
//    var podID = $(this).attr('data-id');
//    var tr = $(this).parent().parent();
//    $.send(
//        '/POD/POD/DeletePOD',
//        { id: podID },
//        function (response) {
//            if (response.IsSuccess) {
//                tr.remove();

//            }
//            Runbow.TWS.Alert(response.Message);
//        },
//        function () {
//            Runbow.TWS.Alert("删除运单失败！");
//        });
//});