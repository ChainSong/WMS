$(document).ready(function () {
    
    $('.SubmitButton').live('click', function () {
        var endcity = $('#EndCity').val();
        var endcityid = $('#EndCityID').val();
        var period = $('#Period').val();

        $.send(
            '/POD/Hilti/AddOrUpdatePeriod',
            { EndCity: endcity,EndCityID:endcityid, Period: period },
            function (response) {
                if (response.IsSuccess) {
                    $('.ResultValue').append(response.ResultValue);
                } else {
                    $('.ResultValue').append(response.ResultValue);
                }
            }, function () {
                Runbow.TWS.Alert("失败！");
            });
    });


    $("#SellName").keyup(function () {
        
        $.send(
       '/POD/Hilti/GetHiltiDriverPhone',
       { SellName: $('#SellName').val() },
       function (response) {
           if (response.IsSuccess) {
               $('#SellPhone').val(response.Phone);
           } else {
               $('#SellPhone').val(response.Phone);
           }
       }, function () {

       });
    });



    $('.SellSubmitButton').live('click', function () {
        var SellName = $('#SellName').val();
        var SellPhone = $('#SellPhone').val();
        

        $.send(
            '/POD/Hilti/AddOrUpdateSellInfo',
            { SellName: SellName, SellPhone: SellPhone},
            function (response) {
                if (response.IsSuccess) {
                    $('.ResultValue').append(response.ResultValue);
                } else {
                    $('.ResultValue').append(response.ResultValue);
                }
            }, function () {
                Runbow.TWS.Alert("失败！");
            });
    });
});





function onRegionSelected(rid, rn, treeId) {
   
    $('#EndCityID').val($('#CityTreeID').attr('value'));
    $('#EndCity').val($('#CityTreeName').attr('value'));

    $.send(
           '/POD/Hilti/GetServicePeriodInfo',
           { EndCity: $('#CityTreeName').attr('value') },
           function (response) {
               if (response.IsSuccess) {
                   $('#Period').val(response.Period);
               } else {
                   $('#Period').val(response.Period);
               }
           }, function () {
               
           });
}



function onRegionAutoCompleteSelected(globalID) {
    if (globalID === 'endCity') {
        $('#EndCityID').val($('#CityTreeID').attr('value'));
        $('#EndCity').val($('#CityTreeName').attr('value'));
    }
}


$(document).ready(function () {
    var setHiddenValToControl = function () {
        
        $('#CityTreeName').val($('#EndCity').val());
       
        $('#CityTreeID').val($('#EndCityID').val());

    }

  



    $('#CityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#EndCity').val('')
        $('#EndCityID').val('')
    });

    setHiddenValToControl();




});
