$(document).ready(function () {
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }
    });

    $('#backButton').click(function () {
        history.back();
    });
    $('#submitButton').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                sql += checkBoxs[i].name.toString() + ",";

            }
        }
        if (sql.length <= 0) {
            showMsg("请勾选SKU！", 4000);
            return;
        }
        else {
            sql = sql.substring(0, sql.length - 1);
            $.ajax({
                type: "POST",
                url: "/WMS/Warehouse/ProductWarningAdd",
                data: {
                    "IDS": sql,
                    "WarehouseID": $('#SearchCondition_WarehouseID').val(),
                    "CustomerID": $('#SearchCondition_CustomerID').val(),
                    "WarehouseName":$('#SearchCondition_WarehouseID option:selected').text(),
                    "MinNumber": $('#SearchCondition_MinNumber').val(),
                    "MaxNumber": $('#SearchCondition_MaxNumber').val()
                },
                async: "false",
                success: function (data) {
                    if (data == "OK")
                    {
                        showMsg("设置成功！", "4000");
                        //window.location.reload();
                        location.href = "/WMS/Warehouse/ProductWarningCreate/?CustomerID=" + $('#SearchCondition_CustomerID').val() + "&ViewType=3";
                    }
                },
                error: function (msg) {
                    showMsg("添加失败", "4000");
                }
            });
        }
    });
});


function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    //var re = new RegExp(pattern);
    if (pattern.test(hehe.value)) {

        hehe.value = hehe.value.replace(pattern, "");
    }
}
