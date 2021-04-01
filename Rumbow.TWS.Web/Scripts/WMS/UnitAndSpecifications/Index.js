$(document).ready(function () {
    $(".delete").live("click", function () {
        var self = $(this);
        //询问框

        layer.confirm('确认删除？', {
            btn: ['确认', '取消'] //按钮
        }, function () {
            $.ajax({
                type: "POST",
                url: "/WMS/UnitAndSpecifications/DeleteUnitAndSpecifications",
                data: {
                    "id": $(self).data().id
                },
                async: "false",
                success: function (data) {

                    if (data.Code == 1) {
                        layer.msg('成功');
                        $(self).parent().parent().remove();
                    } else {
                        layer.msg('失败');
                    }
                },
                error: function (msg) {
                    alert(msg.val);
                }
            });
        }, function () {
            
        });
        
    });

    $("#AddButton").live("click", function () {
        window.location.href = '/WMS/UnitAndSpecifications/AddUnitAndSpecifications';
    });
});