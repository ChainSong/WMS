$(document).ready(function () {
    if ($('#SearchCondition_CustomerID').val() == "") {
        $('#SearchCondition_CustomerID option:first').next().attr("selected", "selected");

    } else {

    }
    //$('.calendarRangeReWrite').each(function (index) {

    //    var id = $(this).attr('id');
    //    var pref = id.split('_')[0];
    //    var actualID = id.split('_')[1];
    //    var descID = 'SearchCondition_';
    //    if (pref === 'start') {
    //        descID += 'Begin' + actualID;
    //    }

    //    else {
    //        descID += 'End' + actualID;
    //    }
    //    $(this).val($('#' + descID).val());


    //});
    var setPageControlVal = function () {
        $(".calendarRangeReWrite").each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];

            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += 'Begin' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    };
    $("#searchButton").click(function () {
        setPageControlVal();
    });

});