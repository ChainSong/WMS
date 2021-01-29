$(function () {
    $('#submitProjectCustomerOrShipper').click(function () {
        var url = $(document.forms[0]).attr('action');
        $.send(
        url,
        { CustomerOrShipperIDs: $('#SelectedConfig').val(), Target:$('#Target').val() },
        function (response) {
            Runbow.TWS.Alert(response);
        },
        function () {
            Runbow.TWS.Alert("设置失败！");
        });
    });

    var checkBoxsForMapping = $(".checkBoxForMapping");
    var checkBoxsForIsDefault = $('.checkBoxForIsDefault')
    $('#selectAll').click(function () {
        if ($(this).attr("checked") === "checked") {
            checkBoxsForMapping.attr("checked", "checked");
        } else {
            checkBoxsForMapping.removeAttr("checked");
        }

        RefreshConfigs();
    });

    checkBoxsForMapping.click(function () {
        RefreshConfigs();
    });

    checkBoxsForIsDefault.click(function () {
        if ($(this).attr("checked") === "checked") {
            checkBoxsForIsDefault.removeAttr("checked");
            $(this).attr("checked", "checked");
        }

        RefreshConfigs();
    });

    var RefreshConfigs = function () {
        var length = checkBoxsForMapping.length;
        var customerOrShipperIDs = [];
        var checked = 0;
        checkBoxsForMapping.each(function () {
            var checkboxForIsDefault = $(this).parent().parent().find('.checkBoxForIsDefault');
            if ($(this).attr("checked") === "checked") {
                var id;
                if ($(checkboxForIsDefault).attr("checked") === "checked") {
                    id = { ProjectShipperOrCustomerID: $(this).attr("data-ID"),CustomerOrShipperID:$(this).attr("data-CustomerOrShipperID"), IsDefault: true };
                } else {
                    id = { ProjectShipperOrCustomerID: $(this).attr("data-ID"),CustomerOrShipperID:$(this).attr("data-CustomerOrShipperID"), IsDefault: false };
                }
                
                customerOrShipperIDs.push(id);
                checked++;
            } else {
                $(checkboxForIsDefault).removeAttr("Checked");
            }

        });

        if (checked == checkBoxsForMapping.length) {
            $('#selectAll').attr("checked", "checked");
        } else {
            $('#selectAll').removeAttr("checked");
        }

        $('#SelectedConfig').val(JSON.stringify(customerOrShipperIDs));
    }

    $(document).ready(function () {
        RefreshConfigs();
    });
})