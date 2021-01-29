var roleIds = '';
$(document).ready(function () {

    //全选
    $("#chdAll").click(function () {
        if ($("#chdAll").is(':checked')) {
            $("[type='checkbox']").prop({ checked: true })
        } else {
            $("[type='checkbox']").removeAttr("checked");
            roleIds = '';
        }
    });

    //批量删除
    $("#btnDelete").click(function () {

        $("[type='checkbox']").each(function () {
            if ($(this).is(':checked')) {    //判断哪些选中拼接roleid 
                if ($(this).attr('data-id') != undefined) {

                    roleIds += ',' + $(this).attr('data-id');
                }
            }
        });

        if (roleIds.length == 0) //如果没有选中提示信息
        {
            layer.tips('您未选择要删除的角色！', '#btnDelete');
        } else {
            //删除前询问 避免导致误删
            layer.confirm('您确认要删除吗？', {
                btn: ['确认', '取消'] //按钮
            }, function () {
                $.ajax({
                    type: 'POST',
                    url: "/System/Role/DeleteRole",
                    data: {
                        paremIds: roleIds
                    },
                    success: function (data) {
                        layer.msg('删除成功！', { icon: 1, time: 1000 }, function () { window.location.href = '/System/Role/RoleManage'; });
                    },
                    error: function () {

                    }

                });
            }, function () {
                roleIds = '';
            });
        }


    });

});


//编辑角色
function EditRole(roleId) {
    window.location.href = "/System/Role/EditRole/" + roleId;
}


//删除角色
function DeleteRole(roleId) {
    //删除前询问 避免导致误删
    layer.confirm('您确认要删除吗？', {
        btn: ['确认', '取消'] //按钮
    }, function () {
        $.ajax({
            type: 'POST',
            url: "/System/Role/DeleteRole",
            data: {
                paremIds: roleId
            },
            success: function (data) {
                layer.msg('删除成功！', { icon: 1, time: 1000 }, function () { window.location.href = '/System/Role/RoleManage'; });
            },
            error: function () {

            }

        });
    });
}

//分配菜单
function DoMenu(objtemp) {
    window.location.href = '/System/ProjectRoleMenu/Index?id=' + objtemp;
}