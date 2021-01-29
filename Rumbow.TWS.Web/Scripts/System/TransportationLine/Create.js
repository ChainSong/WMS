$(document).ready(function () {
    $('#btnCreate').click(function () {
        if ($('#StartCityID').val() === '') {
            Runbow.TWS.Alert('请选择起始城市');
            return false;
        }

        if ($('#EndCityID').val() === '') {
            Runbow.TWS.Alert('请选择到达城市');
            return false;
        }

        return true;
    });

    $('#btnReturn').click(function () {
        window.location.href = "/System/TransportationLine/List";
    });

    $('#startCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $(this).next().val('');
        $(this).next().next().val('');
    });

    $('#endCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $(this).next().val('');
        $(this).next().next().val('');
    });

    $('#startCityTreeID').attr('value',$('#StartCityID').val());
    $('#startCityTreeName').attr('value',$('#StartCityName').val());
    $('#endCityTreeID').attr('value',$('#EndCityID').val());
    $('#endCityTreeName').attr('value',$('#EndCityName').val());
});