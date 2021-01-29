$(document).ready(function () {
    //转退货与服务明细联动
    $('#TransferorReturn').live('change', function () {
        $('#ServiceDetail').empty();
        $('#DestinationCode').empty();
        var TransferorReturn = $('#TransferorReturn').val();
        $.ajax({
            type: "POST",
            url: "/ForecastWarehouse/GapPickingNote/ChangeTransferOrReturn",
            data: {
                "str": TransferorReturn,
            },
            async: "false",
            success: function (data) {
                $.each(data.data1, function (i, item) {
                    $("<option></option>").val(item["Value"]).text(item["Text"]).appendTo($("#ServiceDetail"));
                });
                $.each(data.data2, function (i, item) {
                    $("<option></option>").val(item["Value"]).text(item["Text"]).appendTo($('#DestinationCode'));
                });
            },
            error: function (msg) {
                alert(msg.val);
            }
        });
    })

    $('#submitButton').live('click', function () {
        var TransferorReturn = document.getElementById("TransferorReturn");
        var CartonQuantity = document.getElementById("CartonQuantity");
        var ExpectedDeliveryDate = document.getElementById("ExpectedDeliveryDate");
        var ExpectedArrivalDate = document.getElementById("ExpectedArrivalDate");

        if (CartonQuantity.value == "")
        {
            showMsg("请输入箱数", 4000);
            return false;
        }

        if (TransferorReturn.value.length == 0) {
            showMsg("请选择转货或退货", 4000);
            return false;
        }
        if (ExpectedDeliveryDate.value == "") {
            showMsg("请选择预计到货日期", 4000);
            return false;
        }
        if (ExpectedArrivalDate.value == "") {
            showMsg("请选择预计提货日期", 4000);
            return false;
        }
        

    });

    $('#printButton').live('click', function () {
        window.location.href = '/ForecastWarehouse/GapPickingNote/PickingNotes';

    });

})