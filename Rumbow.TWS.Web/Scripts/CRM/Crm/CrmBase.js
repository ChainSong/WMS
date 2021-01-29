function onRegionSelected(rid, rn, treeId) {
    $('#CityID').val($('#CityTreeID').attr('value'));
    $('#City').val($('#CityTreeName').attr('value'));
};

function onRegionAutoCompleteSelected(globalID) {
    $('#CityID').val($('#CityTreeID').attr('value'));
    $('#City').val($('#CityTreeName').attr('value'));
}

$(document).ready(function () {
    var setHiddenValToControl = function () {
        $('#CityTreeID').val($('#CityID').val());
        $('#CityTreeName').val($('#City').val());
    };

    $('#CityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $(this).next().val('');
        $(this).next().next().val('');
    });

   
  


    var setPageControlVal = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id;
            if ($(this).attr("type") === "checkbox") {
                var isChecked = document.getElementById(id).checked;
                if (isChecked) {
                    $('#' + descId).val("1");
                } else {
                    $('#' + descId).val("0");
                }
            } else {
                $('#' + descId).val($(this).val());
            }
        });

        $('.DropDownList').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id.substr(0, id.length - 4) + "ID";
            $('#' + descId).val($(this).val());
        });

        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    };
    setHiddenValToControl();
});