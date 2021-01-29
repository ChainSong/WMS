$(document).ready(function () {
    $("#printButton").click(function () {
        //openPopup("", true, 350, 200, null, 'PoenPrint', true);
        var SystemNumber = "";
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                SystemNumber += "" + checkBoxs[i].id.toString() + ",";
            }
        }
        if (SystemNumber.length > 1) {
            openPopup("", true, 1000, 600, "/POD/PrintPOD/PrintDemo?SystemNumber=" + SystemNumber, null, true);
        } else {
            showMsg("请选择要打印的运单！", "4000");
        }

    })

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
            $('#SearchCondition_ShipperID').val(ui.item.data.Value);
            $('#SearchCondition_ShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#SearchCondition_ShipperID').val('');
        }
    });
    $('#allocateShipperAutoComplete').autocomplete({
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
            $('#ChangeCarriers').removeAttr('disabled');
            $('#allocateShipperID').val(ui.item.data.Value);
            $('#allocateShipperAutoComplete').val(ui.item.data.Text);

        }
    });
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].type == "checkbox") {
                if (!checkBoxs[i].disabled) {
                    checkBoxs[i].checked = this.checked ? true : false;
                }
            }
        }
    });
    $('#searchButton').click(function () {
        setPageControlVal();
    });
    $('#startCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#SearchCondition_StartCityID').val('');
        $('#startCityTreeName').val('');
        $('#SearchCondition_StartCities').val('');
    });

    $('#endCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#SearchCondition_EndCityID').val('');
        $('#endCityTreeName').val('');
        $('#SearchSearchCondition_EndCities').val('');
    });

    $('.calendarRange').each(function (index) {
        //$('#startCityTreeID').val($('#SearchSearchCondition_StartCities').val());
        //$('#startCityTreeName').val($('#SearchSearchCondition_StartCityName').val());
        //$('#endCityTreeID').val($('#SearchSearchCondition_EndCities').val());
        //$('#endCityTreeName').val($('#SearchSearchCondition_EndCityName').val());
        $('#startCityTreeID').val($('#SearchCondition_StartCityID').val());
        $('#startCityTreeName').val($('#SearchCondition_StartCityName').val());
        $('#endCityTreeID').val($('#SearchCondition_EndCityID').val());
        $('#endCityTreeName').val($('#SearchCondition_EndCityName').val());

        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[0];
        var descID = 'SearchCondition_';
        if (actualID === 'start') {//SearchCondition_StartShelvesTime
            descID += 'ActualDeliveryDate';//+ actualID;
        } else {
            descID += 'EndActualDeliveryDate';//+ actualID;
        }
        $(this).val($('#' + descID).val());
    });
    var setPageControlVal = function () {
        $('#SearchCondition_StartCityID').val($('#startCityTreeID').val());
        $('#SearchCondition_StartCityName').val($('#startCityTreeName').val());
        $('#SearchCondition_EndCityID').val($('#endCityTreeID').val());
        $('#SearchCondition_EndCityName').val($('#endCityTreeName').val());
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[0];
            var descID = 'SearchCondition_';
            if (actualID === 'start') {//SearchCondition_StartShelvesTime
                descID += 'ActualDeliveryDate';//+ actualID;
            } else {
                descID += 'EndActualDeliveryDate';// + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    }


    //是否勾选
    //function IsChoose() {
    //    var Choose = "";
    //    var checkBoxs = $("#resultTable tbody input[type='checkbox']");

    //    for (var i = 0; i < checkBoxs.length; i++) {
    //        if (checkBoxs[i].checked) {
    //            Choose += "" + checkBoxs[i].id.toString() + ",";
    //        }
    //    }
    //    if (Choose.length > 0) {
    //        Choose = Choose.toString().substring(0, Choose.toString().length - 1);
    //    }
    //    return Choose;
    //}


    $('select[id=SearchCondition_Carriers]').live('change', function () {
        if (this.value == "1") {
            $("#SearchCondition_Carriersdrop_down")[0].style.display = "";
        } else {
            $("#SearchCondition_Carriersdrop_down")[0].style.display = "none";
        }

    });


    //function DateDiff(endDate, startDate) {

    //    var arrDate, objDate1, objDate2, intDays;

    //    arrDate = endDate.split("-");

    //    objDate1 = new Date(arrDate[1] + '-' + arrDate[2] + '-' + arrDate[0]);

    //    arrDate = startDate.split("-");

    //    objDate2 = new Date(arrDate[1] + '-' + arrDate[2] + '-' + arrDate[0]);

    //    intDays = parseInt(Math.abs(objDate1 - objDate2) / 1000 / 60 / 60 / 24);

    //    return intDays;

    //}
})







