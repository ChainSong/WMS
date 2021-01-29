$(document).ready(function () {
    $('select[id=SearchCondition_InventoryChangeTypes]').live('change', function () {
        //if ($(this).val()!='') {
            $('#searchButton').click();
            //$.ajax({
            //    type: "POST",
            //    url: "/WMS/ReportManagement/ChangeWarehouse",
            //    data: {
            //        "str": selec,
                  
            //    },
            //    async: "false",
            //    success: function (data) {

            //        var js = JSON.parse(data);
            //        document.all['SearchCondition_Area'].length = 0;
            //        for (var i = 0; i < js.length; i++) {
            //            document.all['SearchCondition_Area'].options.add(new Option(js[i]["AreaName"], js[i]["AreaName"]));
            //        }
            //    },
            //    error: function (msg) {
            //        alert(msg.val);
            //    }
            //});

        //}
        //else {
        //    document.all['SearchCondition_InventoryChangeTypes'].length = 0;
        //    document.all['SearchCondition_InventoryChangeTypes'].options.add(new Option("==请选择==", "==请选择=="));
        //}
    });
});