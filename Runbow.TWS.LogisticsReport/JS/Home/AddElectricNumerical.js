$(document).ready(function () {
    $("#Oksubmit").click(function () {
        var Date = $("#ThisTime")[0].value;
        if (Date == "") {
            alert("请填写度数");
            return false;
        }
        $.ajax({
            url: "/Home/AddElectricNumerical",
            type: 'POST',
            dataType: 'json',
            async: false,
            data: {
                Date: $("#ThisTime")[0].value,
                Office: $("#Office")[0].value,
                NFS: $("#NFS")[0].value,
                Digital: $("#Digital")[0].value,
                Inline: $("#Inline")[0].value
            },
            success: function (data) {
                if (data.ErrorCode == 1) {
                    alert("添加成功")
                }
            },
            error: function (msg) {
                var a = msg;
            }
        });
    })
    $("#Goreport").click(function () {
        window.location = "/Home/Index";
    })

    $("#ThisTime").change(function () {
        $.ajax({
            url: "/Home/GetElectricNumerical",
            type: 'POST',
            dataType: 'json',
            async: false,
            data: {
                Date: $("#ThisTime")[0].value
            },
            success: function (data) {
                if (data.ErrorCode == 1) {
                    $("#Office")[0].value = data.ElectricMeter.OfficeElectric,
                    $("#NFS")[0].value = data.ElectricMeter.NfsElectric,
                    $("#Digital")[0].value = data.ElectricMeter.DigitalElectric,
                    $("#Inline")[0].value = data.ElectricMeter.InlineElectric
                }
            },
            error: function (msg) {
                var a = msg;
            }
        });
    })
})