//打印预览
//function Printpreview() {
//    //$.ajax({
//    //    type: "post",
//    //    url: "/WMS/Warehouse/PrintWareHouseCheck",
//    //    data: {},
//    //    async: "false",
//    //    datatype:"json",
//    //    success: function (data) {
//    //        data = JSON.parse(data)
//    //        var WarehouseCheck = data.data1[0];
//    //        var WarehouseCheckDetail = data.WarehouseCheckDetailCollection;
//    //        var a = $("#WareHouseCheck").html(WarehouseCheck);
//    //        var b = $("#WareHouseCheckDetail").html(WarehouseCheckDetail);
            
//    //    },
//    //    error: function (msg) {
//    //        showMsg("查询失败", "4000");
//    //    }
//    //});
//    wb.ExecWB(7, 1)
//}

////打印
//function Print() {
//    wb.ExecWB(6, 1)
//}

////打印设置
//function Printsetup() {
//    wb.ExecWB(8, 1)
//}



function Print() {
    document.all.WebBrowser.ExecWB(6, 1);
}
function Printsetup() {
    document.all.WebBrowser.ExecWB(8, 1);
}
function Printpreview() {
    //document.all.WebBrowser.ExecWB(7, 1);
    document.getElementById("WebBrowser").ExecWB(7, 1);
}