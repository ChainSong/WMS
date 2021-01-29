$(document).ready(function () {


    $('#searchButton').click(function () {
        setPageControlVal();

    })


    var setPageControlVal = function () {

        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');

            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];

            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += 'Start' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());

        });
    }

})
