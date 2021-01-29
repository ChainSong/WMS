$(document).ready(function () {
   


    $('#Shipper').autocomplete({
        source: function (request, response) {

            $.ajax({
                url: "/Pod/Pod/GetUserShipper",
                type: "POST",
                dataType: "json",
                data: { name: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#ShipperID').val(ui.item.data.Value);
            $('#Shipper').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#ShipperID').val('');
        }
    });

    //$('.calendarRange').each(function (index) {
    //    var id = $(this).attr('id');
    //    var pref = id.split('_')[0];
    //    var actualID = id.split('_')[1];
    //    var descID = '';
    //    var Begin = 'Begin';
    //    var End = 'End';
    //    //var date = Date.now()
    //   // var datetime = "";
    //   // datetime = date.getFullyear()+"-"+date.getMonth()+"-"+date.getDate();
    //   //$("#start_DistributionDate").val(datetime);
    //   //$("#end_DistributionDate").val(datetime);
    //    if (pref === 'start') {
    //        descID = Begin + actualID;
    //    }
    //    else {
    //        descID = End + actualID;
    //    }
    //    $(this).val($('#' + descID).val());
    //});



    var setValToHiddenControl = function () {

           

            $('.calendarRange').each(function (index) {
                var id = $(this).attr('id');
                var pref = id.split('_')[0];
                var actualID = id.split('_')[1];
                var descID;
                var calendarRangeID;
                if (pref === 'start') {
                    descID = "Begin" + actualID;
                    calendarRangeID = "start_" + actualID;
                   
                }
                else {
                    descID = 'End' + actualID;
                    calendarRangeID = "end_" + actualID;
                   
                   
                }
                $('#' + descID).val($(this).val());
                //$('#' + calendarRangeID).val($(this).val());
                
                
            });
        };
   

    $('#QueryButton').click(function () {
        if (CheckEmptyValue()==false) {
            return false;
        }
        setValToHiddenControl();
    });




});


var EmptyValue = function (obj) {
    var IDName = $(obj).attr('id');

    var val = IDName.split('_')[0];
    var Begin = 'start_' + val;
    var End = 'end_' + val;
    $('#' + Begin).val("");
    $('#' + End).val("");
};


var CheckEmptyValue = function ()
{
   
        if ($('#Shipper').val() == "") {
            Runbow.TWS.Alert("承运商不能为空！");
            return false;
        }
        if ($('#start_DistributionDate').val() == "") {
            Runbow.TWS.Alert("分配开始时间不能为空！");
            return false;
        }
        if ($('#end_DistributionDate').val() == "") {
            Runbow.TWS.Alert("分配结束时间不能为空！");
            return false;
        }
  
}












