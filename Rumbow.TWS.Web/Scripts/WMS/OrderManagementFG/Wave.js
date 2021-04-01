$('#AddWaveButton').live('click', function () {
    openPopup('pop99', true, 350, 300, null, 'DistributionDiv');
    $("#popupLayer_pop99")[0].style.top = "200px";
});
$("#WaveReturn").live('click', function () {
    closePopup();
});

$("#WaveBackOK").live('click', function () {
    var CustomerID = $("#customer").find("option:selected").val();
    var CustomerName = $("#customer").find("option:selected").text();
    var WarehouseID = $("#warehouse").find("option:selected").val();
    var WarehouseName = $("#warehouse").find("option:selected").text();
    var IsExpressCompany = $("#IsExpressCompany").is(":checked") == true ? 1 : 0;
    var IsSinglePriece = $("#IsSinglePriece").is(":checked") == true ? 1 : 0;
    var WaveCount = $("#WaveCount").val();

    $.post("/WMS/OrderManagement/CreateWave", { IsExpressCompany: IsExpressCompany, IsSinglePriece: IsSinglePriece, CustomerID: CustomerID, CustomerName: CustomerName, WarehouseID: WarehouseID, WarehouseName: WarehouseName, WaveCount: WaveCount }, function (result) {
        //alert(result);
        if (result == "") {
            showMsg("创建波次成功！", 4000);
            closePopup();
            //加载页面
        } else {
            showMsg(result, 4000);
        }
    });
});

$(document).ready(function () {
    $(".Ooperation").live("mouseover", function () {
        $(this).prev()[0].style.display = "none";
    });
    $(".Ooperation").live("mouseenter", function () {
        $(this).prev()[0].style.display = "";

    })
    $(".Adiv").live("mouseleave", function () {
        $(this)[0].style.display = "none";
    })
    $("tr").live("mouseenter", function () {
        $(".Adiv").each(function (a, b) {
            $(b)[0].style.display = "none";
        })
    })
    if ($("#resultTable tbody")[0].rows.length < 1) {
        $("#resultTable").removeAttr("style");
        $("#tables").removeAttr("style");
        $("#OperateTh").removeAttr("style");
    }

    //HideTr();
    $('#exportButton').live('click', function () {
        var a = TableToJson();
        if (a == "") {
            return true;
        }
        else {

            window.location.href = "/WMS/OrderManagement/ExportOrder?IDs=" + a + "&CustomerID=" + $("#SearchCondition_CustomerID").val();

            return false;
        }
    });
    $('#searchButton').click(function () {
        setPageControlVal();
    });


    $('.DynamicCalendarRange').each(function (index) {
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
        $(this).val($('#' + descID).val());


    });
    var setHiddenValToControl = function () {
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
    }

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
                descID += 'Start' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    }



    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

    });

});
