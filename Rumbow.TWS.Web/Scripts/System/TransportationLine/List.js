$(document).ready(function () {
    $('#searchButton').click(function () {
        $('#PageIndex').val('0');
    });

    $('#startCityTreeID').attr('value', $('#StartCityID').val());
    $('#startCityTreeName').attr('value', $('#StartCityName').val());
    $('#endCityTreeID').attr('value', $('#EndCityID').val());
    $('#endCityTreeName').attr('value', $('#EndCityName').val());

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

    $('#resultTable').find('.enableOrDisable').live('click', function () {
        var message = $(this).attr('state') === 'True' ? '您确认禁用此线路？' : '您确认启用此线路？';
        var button = $(this);
        if (confirm(message)) {
            $.send(
               '/System/TransportationLine/DelOrReuse',
               {id:$(this).attr('data-id'), state:$(this).attr('state') },
               function (data) {
                   if (data.ID > 0) {
                       $(button).parent().parent().remove();
                   } else {
                       Runbow.TWS.Alert(data.ErrorMessage);
                   }
               },
               function () {
                   Runbow.TWS.Alert("操作失败！");
               });
        }
    });
});