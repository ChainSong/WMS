$("#btnAddOrder").live('click', function () {
   
    OpenOrderWindow();
}); 

function OpenOrderWindow() {
    var obj = this;
    openPopup("OrderSelectPop", true, 1000, 600, '/WMS/OrderECManagement/OrderSelect?PrintID=' + $("#hdHeaderID").val()+'', null, function (id_select, CustomerID, CustomerName, WarehouseID, WarehouseName) {
        var headerID = $("#hdHeaderID").val();
        //alert(id_select);
        //生成打印单和明细
        var PrintID = parseInt($("#hdHeaderID").val());
        var PrintKey = $("#hdHeaderKey").val();
        $.post("/WMS/OrderECManagement/CreateOrUpdatePrintHeaderAndDetail", { CustomerID: parseInt(CustomerID), CustomerName: CustomerName, WarehouseID: WarehouseID, WarehouseName: WarehouseName, ids: id_select, PrintID: PrintID, PrintKey: PrintKey }, function (data) {
            if (data["IsSuccess"]) {
                window.location.href = "/WMS/OrderECManagement/PrintDetail?ID=" + data.Result.PrintHeaderCollection[0].ID;
            }
        }, "json");
    });
    $("#popupLayer_OrderSelectPop")[0].style.top = "50px";
}

$("#btnRelate").live('click', function () {
    OpenRelate();
});

function OpenRelate() {
    openPopup('RelateExpressKeyPop', true, 350, 300, null, 'relateDiv');
    $("#popupLayer_RelateExpressKeyPop")[0].style.top = "200px";
    $("#RelateOrderKey").val('');
    $("#RelateExpressCompany").val('');
    $("#RelateExpressKey").val('');
    $("#RelateOrderKey").select();
}

$("#DivReturn").live('click', function () {
    closePopup();
});

$("#DivOK").live('click', function () {
    var headerID = parseInt($("#hdHeaderID").val());
    var RelateOrderKey = $("#RelateOrderKey").val();
    var RelateExpressKey = $("#RelateExpressKey").val();
    if (RelateOrderKey == '' || RelateExpressKey == '') {
        showMsg("订单号或快递单号不能为空", 4000);
        return;
    }
    else {
        $.post("/WMS/OrderECManagement/RelateExpressKey", { ID: headerID, OrderKey: RelateOrderKey, ExpressKey: RelateExpressKey }, function (data) {
            if (data["IsSuccess"]) {
                //showMsg("绑定成功", "4000");                
                window.location.href = "/WMS/OrderECManagement/PrintDetail?ID=" + headerID + "&ISopen=" + 1;
            }
            else {
                showMsg(data.msg, 3000);
                $("#RelateExpressKey").select();
            }
        }, "json");
    }
});

$(function () {
    var PrintID = parseInt($("#hdHeaderID").val());
    if (PrintID == "0") {
        OpenOrderWindow();
    }
    var Isopen = parseInt($("#Isopen").val())
    if (Isopen > 0) {
        OpenRelate();
    }
});

$("#RelateOrderKey").live('keydown', function (event) {
    if (event.keyCode == 13) {
        //验证订单是否正确 获取订单对应快递公司
        var data = eval($("#details").val());
        var ExpressCompany = "";
        var bl = false;
        $.each(data, function (i, obj) {
            if (obj["OrderKey"] == $("#RelateOrderKey").val()) {
                bl = true;
                ExpressCompany = obj["ExpressCompany"];
                return false;
            }
        });
        if (!bl) {
            showMsg("订单号不正确", 4000);
        } else {
            $("#RelateExpressCompany").val(ExpressCompany);
            $("#RelateExpressKey").focus();
        }
    }
});