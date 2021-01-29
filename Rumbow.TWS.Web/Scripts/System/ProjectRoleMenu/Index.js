$(function () {
    var setting = { view: { selectedMulti: false }, check: { enable: true }, data: { key: { checked: 'isChecked' }, simpleData: { enable: true } } };

    $.fn.zTree.init($('#menuTree'), setting, Menus);
    $("#Role").change(function () {
        Refresh();
    });

    function Refresh() {
        
        window.location = Url + $("#Role").val();
    }

    $('#submitProjectRoleMenu').click(function () {
        if ($("#Role").val() === "") {
            layer.msg("请选择角色", function () { });
            return;
        }
        $.send(
            Url,
            { ProjectRoleID: $("#Role").val(), MenuItems: GetCheckedItemIDs() },
            function (response) {
                layer.msg(response, { icon: 1, time: 1000 }, function () { window.location.href = '/System/Role/RoleManage'; });
            },
            function () {
                layer.msg("角色菜单设置失败！", function () { });
            });
    });

    $('#centerclose').click(function () {
        $.fn.zTree.getZTreeObj("menuTree").expandAll(false);
    });

    $('#centeropen').click(function () {
        $.fn.zTree.getZTreeObj("menuTree").expandAll(true);
    });


    function GetCheckedItemIDs() {
        var ids = [];
        $.each($.fn.zTree.getZTreeObj("menuTree").getCheckedNodes(), function (i, o) {
            ids.push(o.id);
        });
        return ids;
    }
});

