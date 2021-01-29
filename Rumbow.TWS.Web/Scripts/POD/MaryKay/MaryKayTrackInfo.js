$(document).ready(function () {
    $('.calendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = '';
        var Begin = 'Begin';
        var End = 'End';
        if (pref === 'start') {
            descID = Begin + actualID;
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

    $('#QueryButton').click(function () { setValToHiddenControl(); });

    $('#QueryButton').click(function () {
        $('#PageIndex').val('0');
        $('#IsExport').val('False');

    });


    $('#ExprotButton').click(function () {
        $('#IsExport').val('True');
    });

});

var CheckALL = function () {
    if ($("#CheckAll").attr("checked")) {
        $(":checkbox").attr("checked", true);
    } else {
        $(":checkbox").attr("checked", false);
    }
}

var EmptyValue = function (obj) {
    var IDName = $(obj).attr('id');

    var val = IDName.split('_')[0];
    var Begin = 'start_' + val;
    var End = 'end_' + val;
    $('#' + Begin).val("");
    $('#' + End).val("");
};


var DeleteTrack = function () {

    var arr = new Array();
    var trs = $("#TrackInfo tr");

    for (var i = 1; i < trs.length; i++) {

        var Podid = "";
        //var check = $($(trs[i]).find('td:eq(0) input[type=checkbox]'));
        var check = $(trs[i]).find('.CheckTrack');
        if (check[0].checked) {
            Podid = check[0].id;
            var jsonstr = { PODID: Podid };
            arr.push(jsonstr);
        }

    }

    $.send('/POD/MaryKay/DeleteTrackInfoByID', { array: JSON.stringify(arr) },
        function (response) {
            if (response.IsSuccess) {
                Runbow.TWS.Alert(response.Message);
                for (var i = 1; i < trs.length; i++) {
                    var check = $(trs[i]).find('.CheckTrack');
                    if (check[0].checked) {
                        $(check).parent().parent().remove();
                    }
                   
                }
            }
            else {
                Runbow.TWS.Alert(response.Message);
            }
        }, function () {
            Runbow.TWS.Alert("保存失败！");
        });
}








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





var UpLoadMK = function () {

    var arr = new Array();
    var trs = $("#TrackInfo tr");

    for (var i = 1; i < trs.length; i++) {

        var Podid = "";
        //var check = $($(trs[i]).find('td:eq(0) input[type=checkbox]'));
        var check = $(trs[i]).find('.CheckTrack');
        if (check[0].checked) {
            Podid = check[0].id;
            var jsonstr = { PODID: Podid };
            arr.push(jsonstr);
        }

    }

    $.send('/POD/MaryKay/UpLoadMK', { array: JSON.stringify(arr) },
        function (response) {
            if (response.IsSuccess) {
                Runbow.TWS.Alert(response.Message);
                for (var i = 0 ; i < response.IDS.length; i++) {
                    var id = response.IDS[i];

                    var CheckBox = document.getElementById(id);

                    $(CheckBox).parent().parent().remove();

                }
            }
            else {
                Runbow.TWS.Alert(response.Message);
                for (var i = 0 ; i < response.IDS.length; i++) {
                    var id = response.IDS[i];

                    var CheckBox = document.getElementById(id);

                    $(CheckBox).parent().parent().remove();

                }
            }
        }, function () {
            Runbow.TWS.Alert("失败！");
        });
}





var GetYUNDATrackInfo = function () {

    $.send('/POD/MaryKay/GetYUNDATrackInfo',"",
        function (response) {
            if (response.IsSuccess) {
                Runbow.TWS.Alert(response.Message);
                

               

            }
            else {
                Runbow.TWS.Alert(response.Message);
            }
        }, function () {
            Runbow.TWS.Alert("失败！");
        });
}