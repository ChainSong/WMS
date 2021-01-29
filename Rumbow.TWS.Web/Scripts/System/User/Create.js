$(document).ready(function () {
    $('#UserType').change(function () {
        if ($(this).val() === "2") {
            $('#ShipperID').hide();
            $('#CustomerID').hide();
            $('#lblCustomerOrShipper').text('');
        } else if ($(this).val() === "0") {
            $('#ShipperID').hide();
            $('#CustomerID').show();
            $('#lblCustomerOrShipper').text('客户');
        } else if ($(this).val() === "1") {
            $('#ShipperID').show();
            $('#CustomerID').hide();
            $('#lblCustomerOrShipper').text('承运商');
        }
    }).trigger('change');


    $('#btnSave').click(function () {

        if ($('#UserType').val() === "0" && $('#CustomerID').val() === "") {
            Runbow.TWS.Alert("请选择客户");
            return false;
        }

        if ($('#UserType').val() === "1" && $('#ShipperID').val() === "") {
            Runbow.TWS.Alert("请选择承运商");
            return false;
        }
        
        return true;
    });
    $('#CityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#startCityTreeName').val('');
        $('#startCityTreeID').val('');
    });
   
    $('#btnSave').click(function () {
        $("#RuleArea").val($("#startCityTreeID").val());
        $("#RuleAreaName").val($("#startCityTreeName").val());
    });
    $("#startCityTreeName").val($("#RuleAreaName").val());
});

