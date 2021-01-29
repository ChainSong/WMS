$(document).ready(function () {
    $('#attributionName').attr('value', $('#CRMShipper_Attribution').val());

    //document.getElementById("CRMShipper_DateTime1").className = "calendarRange form-control";
     
    //document.getElementById("CRMShipper_DateTime2").className = "calendarRange form-control";
    //document.getElementById("CRMShipper_DateTime3").className = "calendarRange form-control";

    //document.getElementById("attributionName").className = "form-control";

    var rating = $('#CRMShipper_Rating').val();
    var recommended = $('#CRMShipper_Recommended').val();

    if (rating === '') {
        rating = 0;
    }

    if (recommended === '') {
        recommended = 0;
    }

    var ratingOptions = {
        max: 5,
        value: rating,
        enabled: $('#ViewType').val() == '0' ? false : true,
        after_click: function (ret) {
            $('#CRMShipper_Rating').val(ret.number);
        }
    };

    var recommendedOptions = {
        max: 5,
        value: recommended,
        enabled: $('#ViewType').val() == '0' ? false : true,
        after_click: function (ret) {
            $('#CRMShipper_Recommended').val(ret.number);
        }
    };

    $('#RatingDiv').rater(ratingOptions);

    $('#RecommendedDiv').rater(recommendedOptions);

    $('#AttributionClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#CRMShipper_Attribution').val('');
    });

    $('#returnButton').click(function () {
        window.location.href = '/ShipperManagement/ShipperManagement/Index?useSession=true';
    });

    $('#CRMShipper_RegisteredCapital').keyup(function () {
        var val = parseInt($(this).val());
        if (val) {
            if (val <= 100) {
                $('#CRMShipper_RegisteredCapitalRange').val('100万以内');
            } else if (val > 100 && val <= 300) {
                $('#CRMShipper_RegisteredCapitalRange').val('100万-300万');
            } else if (val > 300 && val <= 500) {
                $('#CRMShipper_RegisteredCapitalRange').val('300万-500万');
            } else if (val > 500 && val <= 800) {
                $('#CRMShipper_RegisteredCapitalRange').val('500万-800万');
            } else if (val > 800 && val <= 1200) {
                $('#CRMShipper_RegisteredCapitalRange').val('800万-1200万');
            } else if (val > 1200 && val <= 2000) {
                $('#CRMShipper_RegisteredCapitalRange').val('1200万-2000万');
            } else if (val > 2000) {
                $('#CRMShipper_RegisteredCapitalRange').val('2000万以上');
            }
        } else {
            $('#CRMShipper_RegisteredCapitalRange').val('');
        }
    });

    $('#CRMShipper_AnnualTurnover').keyup(function () {
        var val = parseInt($(this).val());
        if (val) {
            if (val <= 500) {
                $('#CRMShipper_AnnualTurnoverRange').val('500万以内');
            } else if (val > 500 && val <= 800) {
                $('#CRMShipper_AnnualTurnoverRange').val('500万-800万');
            } else if (val > 800 && val <= 1200) {
                $('#CRMShipper_AnnualTurnoverRange').val('800万-1200万');
            } else if (val > 1200 && val <= 2000) {
                $('#CRMShipper_AnnualTurnoverRange').val('1200万-2000万');
            } else if (val > 2000 && val <= 5000) {
                $('#CRMShipper_AnnualTurnoverRange').val('2000万-5000万');
            } else if (val > 5000 && val <= 10000) {
                $('#CRMShipper_AnnualTurnoverRange').val('5000万-1亿');
            } else if (val > 10000) {
                $('#CRMShipper_AnnualTurnoverRange').val('1亿以上');
            }
        } else {
            $('#CRMShipper_AnnualTurnoverRange').val('');
        }
    });

    $('#CRMShipper_WarehouseArea').keyup(function () {
        var val = parseInt($(this).val());
        if (val) {
            if (val <= 300) {
                $('#CRMShipper_WarehouseAreaRange').val('300以内');
            } else if (val > 300 && val <= 500) {
                $('#CRMShipper_WarehouseAreaRange').val('300-500');
            } else if (val > 500 && val <= 800) {
                $('#CRMShipper_WarehouseAreaRange').val('500-800');
            } else if (val > 800 && val <= 1000) {
                $('#CRMShipper_WarehouseAreaRange').val('800-1000');
            } else if (val > 1000 && val <= 1500) {
                $('#CRMShipper_WarehouseAreaRange').val('1000-1500');
            } else if (val > 1500 && val <= 2000) {
                $('#CRMShipper_WarehouseAreaRange').val('1500-2000');
            } else if (val > 2000) {
                $('#CRMShipper_WarehouseAreaRange').val('2000以上');
            }
        } else {
            $('#CRMShipper_AnnualTurnoverRange').val('');
        }
    });

    $('#CRMShipper_TrunkOfVehicle').keyup(function () {
        var val = parseInt($(this).val());
        if (val) {
            if (val <= 8) {
                $('#CRMShipper_TrunkOfVehicleRange').val('8辆以下');
            } else if (val > 8 && val <= 12) {
                $('#CRMShipper_TrunkOfVehicleRange').val('8辆-12辆');
            } else if (val > 12 && val <= 18) {
                $('#CRMShipper_TrunkOfVehicleRange').val('12-18辆');
            } else if (val > 18 && val <= 25) {
                $('#CRMShipper_TrunkOfVehicleRange').val('18-25辆');
            } else if (val > 25) {
                $('#CRMShipper_TrunkOfVehicleRange').val('25辆以上');
            } 
        } else {
            $('#CRMShipper_AnnualTurnoverRange').val('');
        }
    });

    $('#CRMShipper_DeliveryOfVehicle').keyup(function () {
        var val = parseInt($(this).val());
        if (val) {
            if (val <= 8) {
                $('#CRMShipper_DeliveryOfVehicleRange').val('8辆以下');
            } else if (val > 8 && val <= 12) {
                $('#CRMShipper_DeliveryOfVehicleRange').val('8辆-12辆');
            } else if (val > 12 && val <= 18) {
                $('#CRMShipper_DeliveryOfVehicleRange').val('12-18辆');
            } else if (val > 18 && val <= 25) {
                $('#CRMShipper_DeliveryOfVehicleRange').val('18-25辆');
            } else if (val > 25) {
                $('#CRMShipper_DeliveryOfVehicleRange').val('25辆以上');
            }
        } else {
            $('#CRMShipper_AnnualTurnoverRange').val('');
        }
    });

    $('#submitButton').click(function () {
        if ($('#ViewType').val() === '1' && $('#CRMShipper_Name').val() === '') {
            Runbow.TWS.Alert('请输入企业名称');
            return false;
        }
    });
});

function onRegionSelected(rid, rn, treeId) {
    $('#CRMShipper_Attribution').val($('#attributionName').attr('value'));
}

function onRegionAutoCompleteSelected(globalID) {
    $('#CRMShipper_Attribution').val($('#attributionName').attr('value'));
}