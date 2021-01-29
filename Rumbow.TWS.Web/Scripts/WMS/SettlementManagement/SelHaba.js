$(document).ready(function () {
    $("#AddButton").live('click', function () {

        var Time = document.getElementById("CreateTime").value;
        if (Time == "") {
            alert("请选择新增结算日期");

        }
        else {
            location.href = "/WMS/SettlementManagement/Settlementpage?date=" + Time;

        }
    });

    $('#delete').live('click', function () {
        var ids = $(this).attr('data-id');
        var row = $(this).parent().parent();
        if (window.confirm("确认删除此报价？")) {
            $.send(
                '/WMS/SettlementManagement/Delete',
                { ids: ids },
                function (data) {
                    if (data.ReturnVal === 1) {
                        row.remove();
                    } else {
                        Runbow.TWS.Alert(data.Message)
                    }
                },
                function () {
                    Runbow.TWS.Alert("删除报价失败，请联系IT");
                });
        }
    });
});