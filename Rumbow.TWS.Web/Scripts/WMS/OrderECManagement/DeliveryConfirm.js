
$(document).ready(function () {
    $('#addButton').live('click', function () {
        var customerID = $('#SearchCondition_CustomerID').val();
        var warehouseID = $('#SearchCondition_WarehouseName').val();

        var customerName = $('#SearchCondition_CustomerID').find("option:selected").text();
        var warehouseName = $('#SearchCondition_WarehouseName').find("option:selected").text();
        if (customerID == '' || warehouseID == '') {
            showMsg("新增时请选择客户和仓库！", 1000);
            return false;
        }
        window.location.href = "/WMS/OrderECManagement/DeliverDetail?ID=0&Type=0&customerID=" + customerID + "&warehouse=" + warehouseName + "";//新增进入交接单明细
    })
    if ($('#SearchCondition_CustomerID').val() == "") {
        $('#SearchCondition_CustomerID option:first').next().attr("selected", "selected");

    } else {

    }

    //全选与反选
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

    });

})

//获取选中的ID
function TableToJson() {
    var a = "";
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");

    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            a += checkBoxs[i].name + ",";
        }
    }

    a = a.substring(0, a.length - 1);

    return a;
}
