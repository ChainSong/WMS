
$(function () {
    $('#submitProjectUserCustomer').click(function () {
        if (!checkInput()) {
            return;
        }

        $.send(
        '/System/Project/ProjectUserCustomerAllocate',
        { projectID: $('#ProjectID').val(), userID: $('#UserID').val(), selectedCustomers: $('#SelectedCustomers').val() },
        function (response) {
            Runbow.TWS.Alert("设置成功");
        },
        function () {
            Runbow.TWS.Alert("用户客户数据权限设置失败！");
        });
        
    });

    var checkInput = function () {
        if ($('#UserID').val() === '') {
            Runbow.TWS.Alert("请选择用户");
            return false;
        }

        if ($('#SelectedCustomers').val() === '') {
            Runbow.TWS.Alert("请选择客户");
            return false;
        }

        return true;
    };

    var checkBoxsForMapping = $(".checkForSelect");

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

    var RefreshConfigs = function () {
        var length = checkBoxsForMapping.length;
        var customerIDs = [];
        var checked = 0;
        checkBoxsForMapping.each(function () {
            if ($(this).attr("checked") === "checked") {
                customerIDs.push($(this).attr('data-id'));
                checked++;
            }
        });

        if (checked == checkBoxsForMapping.length) {
            $('#selectAll').attr("checked", "checked");
        } else {
            $('#selectAll').removeAttr("checked");
        }

        $('#SelectedCustomers').val(JSON.stringify(customerIDs));
    };

    $('#UserID').change(function () {
        $('.checkForSelect').removeAttr('checked');
        var userID = $(this).val();
        if (userID === '') {
            return;
        } else {
            $.send(
            '/System/Project/GetUserProjectCustomers',
            { projectID: $('#ProjectID').val(), userID: userID },
               function (response) {
                   for (var i = 0; i < response.length; i++) {
                       $('.checkForSelect[data-id=' + response[i] + ']').attr('checked', 'checked');
                   }
                   RefreshConfigs();
               },
               function () {
                   Runbow.TWS.Alert("获取用户项目角色失败！");
               });
        }

    });

    $(document).ready(function () {
        RefreshConfigs();
    });
})