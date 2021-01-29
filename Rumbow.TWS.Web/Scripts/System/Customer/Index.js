$(document).ready(function () {
    $('#addButton').click(function () {
        window.location.href = "Create";
    });

    $('.shipperStatus').click(function () {
        var shipper = $(this).attr('data-name');
        var model = $(this).attr('data-id');
        location.href = "Create/" + shipper + "?ViewType=1" + "&customerType=" + model;


    });
});


function DeleteCustomer(id) {
    //删除前询问 避免导致误删
    layer.confirm('您确认要删除吗？', {
        btn: ['确认', '取消'] //按钮
    }, function () {
        $.ajax({
            type: 'POST',
            url: "/System/Customer/Delete",
            data: {
                ID: id
            },
            success: function (data) {
                layer.msg('删除成功！', { icon: 1,time:1000 }, function () { window.location.href = '/System/Customer/Index'; });
            },
            error: function () {
            }
        });
    }, function () {

    });
}