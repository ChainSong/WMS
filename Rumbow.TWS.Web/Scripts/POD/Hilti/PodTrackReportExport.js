$(document).ready(function () {

    $('#ShipperName').autocomplete({
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

            $('#ShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {

        }
    });

    if ($('#UpOrDown').val()) {
        
        $('.CssSpanDown').css('display', 'none')
        $("#TableTwo").slideDown("slow");
        $('.CssSpanUp').css('display', 'block')
       
    }

    $('.calendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = '';
        var Begin = 'Begin';
        var End = 'End';
        if (pref === 'start') {
            descID = Begin+actualID;
        }
        else {
            descID = End + actualID;
        }
        $(this).val($('#' + descID).val());
    });
    
    

    var setValToHiddenControl = function () {
       

        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID;
            if (pref === 'start') {
                descID = "Begin" + actualID;
            }
            else {
                descID = 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });

        

    };

    $('#QueryButton').click(function () {
        if ($('#ReportNameValue').val() == "") {
            Runbow.TWS.Alert("报表名称不能为空！");
            return false;
        }
       


       
        

        setValToHiddenControl();

        //if ($('#ReportNameValue').val() == "KPI统计报表") {
        //    alert($('#BeginOrderDate').val());
        //    if ($('#BeginOrderDate').val() == "" || $('#EndOrderDate').val() == "") {
        //        Runbow.TWS.Alert("警告！KPI统计报表,必须用订单日期来查询。");
        //        return false;
        //    }


        //}
    });
    $('#ExportButton').click(function () {
        if ($('#ReportNameValue').val() == "") {
            Runbow.TWS.Alert("报表名称不能为空！");
            return false;
        }
       

        setValToHiddenControl();


        //if ($('#ReportNameValue').val() == "KPI统计报表") {
        //    if ($('#BeginOrderDate').val() == "" || $('#EndOrderDate').val() == "") {
        //        Runbow.TWS.Alert("警告！KPI统计报表,必须用订单日期来查询。");
        //        return false;
        //    }


        //}
    });
    
   

    //$('.CssSpanUp').css('display', 'none')

    $('.CssSpanDown').click(function () {
        $("#TableTwo").slideDown("slow");
        $('.CssSpanUp').css('display', 'block')
        $('.CssSpanDown').css('display', 'none')
        

    });


    $('.CssSpanUp').click(function () {

        $("#TableTwo").slideUp("slow");
        $('.CssSpanUp').css('display', 'none')
        $('.CssSpanDown').css('display', 'block')
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




function onRegionSelected(rid, rn, treeId) {
    $('#EndProvinceID').val($('#ProvinceTreeID').attr('value'));
    $('#EndProvince').val($('#ProvinceTreeName').attr('value'));

    $('#EndCityID').val($('#CityTreeID').attr('value'));
    $('#EndCity').val($('#CityTreeName').attr('value'));
}



function onRegionAutoCompleteSelected(globalID) {
   if (globalID === 'endCity') {
        $('#EndCityID').val($('#CityTreeID').attr('value'));
        $('#EndCity').val($('#CityTreeName').attr('value'));
    }
}


$(document).ready(function () {
    var setHiddenValToControl = function () {
        $('#ProvinceTreeName').val($('#EndProvince').val());
        $('#CityTreeName').val($('#EndCity').val());
        $('#ProvinceTreeID').val($('#EndProvinceID').val());
        $('#CityTreeID').val($('#EndCityID').val());
        
    }

    $('#ProvinceClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#EndProvince').val('')
        $('#EndProvinceID').val('')
    });

    $('#CityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#EndCity').val('')
        $('#EndCityID').val('')
    });

    setHiddenValToControl();
});







