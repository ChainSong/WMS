function onRegionSelected(rid, rn, treeId) {
    if (treeId === 'startCity') {
        $('#SearchCondition_StartCityID').val($('#startCityTreeID').attr('value'));
        $('#SearchCondition_StartCityName').val($('#startCityTreeName').attr('value'));
    } else if (treeId === 'endCity') {
        $('#SearchCondition_EndCityID').val($('#endCityTreeID').attr('value'));
        $('#SearchCondition_EndCityName').val($('#endCityTreeName').attr('value'));
    }
}

function onRegionAutoCompleteSelected(globalID) {
    if (globalID === 'startCity') {
        $('#SearchCondition_StartCityID').val($('#startCityTreeID').attr('value'));
        $('#SearchCondition_StartCityName').val($('#startCityTreeName').attr('value'));
    } else if (globalID === 'endCity') {
        $('#SearchCondition_EndCityID').val($('#endCityTreeID').attr('value'));
        $('#SearchCondition_EndCityName').val($('#endCityTreeName').attr('value'));
    }
}

$(document).ready(function () {
    $('#SearchCondition_ShipperName').autocomplete({
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
            $('#SearchCondition_ShipperID').val(ui.item.data.Value);
            $('#SearchCondition_ShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#SearchCondition_ShipperID').val('');
        }
    });

    var setHiddenValToControl = function () {
        $('#startCityTreeID').val($('#SearchCondition_StartCityID').val());
        $('#startCityTreeName').val($('#SearchCondition_StartCityName').val());
        $('#endCityTreeID').val($('#SearchCondition_EndCityID').val());
        $('#endCityTreeName').val($('#SearchCondition_EndCityName').val());
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id;
            if ($(this).attr("type") === "checkbox") {
                if ($('#' + descId).val() === '1') {
                    $(this).attr('checked', 'checked');
                } else {
                    $(this).removeAttr('checked');
                }

            } else if ($(this).attr("type") === "DropDownList") {
                var desc = $('#' + descId);
                if (desc.val() === '1' || desc.val() === 'Y' || desc.val === 'y' || desc.val() === '是') {
                    $(this).val('1');
                } else {
                    $(this).val('0');
                }

            } else {
                $(this).val($('#' + descId).val());
            }
        });

        $('.DropDownList').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id.substr(0, id.length - 4) + "ID";
            $(this).val($('#' + descId).val());
        });

        $('select[id=SearchCondition_CustomerID]').live('change', function () {
            window.location.href = "/Reports/PodReport/ShowPodAll/?customerID=" + $(this).val();
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
            $(this).val($('#' + descID).val());
        });
    };

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

    $('#searchButton').click(function () {
        $('#PageIndex').val('0');
        setPageControlVal();
    });

    $('#startCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#SearchCondition_StartCityID').val('')
        $('#SearchCondition_StartCityName').val('')
    });

    $('#endCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#SearchCondition_EndCityID').val('')
        $('#SearchCondition_EndCityName').val('')
    });

    setHiddenValToControl();

});