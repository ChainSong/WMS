$(document).ready(function () {
    $(".Butdel").click(function () {
        var self = this;
        $.ajax({
            type: "POST",
            url: "/WMS/InventoryManagement/DelDirectAddInventory",
            data: {
                "Id": self.dataset.id
            },
            async: "false",
            success: function (data) {
                if (data.Code == 1) {
                    showMsg("操作成功!", "4000");
                    $(self).parent().parent().remove();
                }
                else {
                    showMsg("操作失败!", "4000");
                }
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
            }
        });
    })
})