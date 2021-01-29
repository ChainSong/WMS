var EmptyValue = function (obj) {
    var IDName = $(obj).attr('id');

    var val = IDName.split('_')[0];
    var Begin = 'start_' + val;
    var End = 'end_' + val;
    $('#' + Begin).val("");
    $('#' + End).val("");
};



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
        $('#IsExportTrack').val('False');
        $('#IsExport').val('False');

    });

    
    $('#ExprotButton').click(function () {
       
        $('#IsExport').val('True');
        $('#IsExportTrack').val('False');
    });

    


    $('#ExprotTrackButton').click(function () {
        
        $('#IsExportTrack').val('True');
        $('#IsExport').val('False');
    });
});