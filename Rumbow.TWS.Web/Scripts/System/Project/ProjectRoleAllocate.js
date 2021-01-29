$(function () {
    var IsSuccess = '@ViewBag.IsSuccess';
    $("#btnNext").hide();

    $('#submitProjectRole').click(function () {
        var url = $(document.forms[0]).attr('action');
        $.send(
        url,
        { MenuIDs: $('#SelectedRoleIDs').val() },
        function (response) {
          
           
                $("#submitProjectRole").hide();
                $("#btnNext").show();

                layer.tips('提交成功,点击下一步给项目角色设置菜单权限', '#btnNext', {
                    tips: [4, '#78BA32']
                });
            
            //Runbow.TWS.Alert(response);
        },
        function () {
            Runbow.TWS.Alert("项目角色设置失败！");
        });
    });

    var checkBoxs = $("#RoleTable tbody input[type='checkbox']");
    $('#selectAll').click(function () {
        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

        RefreshMenuIDs();
    });

    checkBoxs.click(function () {
        RefreshMenuIDs();
    });

    var RefreshMenuIDs = function () {
        var length = checkBoxs.length;
        var MenuIDs = [];
        var checked = 0;
        checkBoxs.each(function () {
            if ($(this).attr("checked") === "checked") {
                var id = { ID: $(this).attr("data-ID") };
                MenuIDs.push(id);
                checked++;
            }
        });

        if (checked == checkBoxs.length) {
            $('#selectAll').attr("checked", "checked");
        } else {
            $('#selectAll').removeAttr("checked");
        }

        $('#SelectedRoleIDs').val(JSON.stringify(MenuIDs));
    }

    $(document).ready(function () {
        RefreshMenuIDs();
    });

})