$(document).ready(function () {
    $('.checkForSelect').click(function () {
        if ($(this).attr("checked") === "checked") {
            $('.checkForSelect').removeAttr('checked');
            $(this).attr('checked', 'checked');
        }
    });

    $('#UserID').change(function () {
        $('.checkForSelect').removeAttr('checked');
        var userID = $(this).val();
        if (userID === '') {
            return;
        } else {
            $.send(
            '/System/Role/GetUserProjectRole',
            { userID: userID },
               function (response) {
                   $('.checkForSelect[data-id=' + response + ']').attr('checked', 'checked');
               },
               function () {
                   Runbow.TWS.Alert("获取用户项目角色失败！");
               });
        }

    });

    $('#submitProjectUser').click(function () {
        var projectRoleID = '';
        var userID = $('#UserID').val();

        if (userID === '') {
            Runbow.TWS.Alert('请选择用户');
            return;
        }

        $('.checkForSelect').each(function (index) {
            if ($(this).attr('checked') === 'checked') {
                projectRoleID = $(this).attr('data-id');
            }
        });

        if (projectRoleID === '') {
            Runbow.TWS.Alert('请选择需要设置的角色');
            return;
        }

        $.send(
            '/System/Project/ProjectUserAllocate',
            { userID: userID, projectRoleID: projectRoleID },
               function (response) {
                   Runbow.TWS.Alert(response);
               },
               function () {
                   Runbow.TWS.Alert("项目用户设置失败！");
               });
    });
});