$(document).ready(function () {
    $(".Ooperation").live("mouseover", function () {
        $(this).prev()[0].style.display = "none";
    });
    $(".Ooperation").live("mouseenter", function () {
        $(this).prev()[0].style.display = "";
    })
    $(".Adiv").live("mouseleave", function () {
        $(this)[0].style.display = "none";
    })
    $("tr").live("mouseenter", function () {
        $(".Adiv").each(function (a, b) {
            $(b)[0].style.display = "none";
        })
    })
    if ($('#HiddenViewType').val() == 2) {
    }
    else if ($('#HiddenViewType').val() == 0) {
        $("#NewDiv").removeAttr("style");
        $("#Newtable").removeAttr("style");
    }
    //if ($("#Adjusttype").val() != null && $("#Adjusttype").val() != "") {
    //    $("#AdjustmentType").val($("#Adjusttype").val());
    //}
    if ($("#HiddenViewType").val() != "2" && $("#HiddenViewType").val() != "0") {
        $("#PrintLabel")[0].style.display = "none";
    }
    else {
        $("#PrintLabel")[0].style.display = "none";
    }
    $('select[id=CustomerID]').live('change', function () {
        window.location.href = "/WMS/InventoryManagement/AdjustGoodsTypeAddorEditorView/?ID=0&ViewType=1&customerID=" + $(this).val();
    });

    if ($('#HiddenViewType').val() == 1) {
        changestype();
        $('#AdjustmentNumber').val("系统自动生成");
    }
    if ($('#HiddenViewType').val() == 2 || $('#HiddenViewType').val() == 0) {
        var table = document.getElementById("Newtable");
        var row = table.getElementsByTagName("tr");
        for (var j = 1; j < row.length; j++) {
            var col = row[j].getElementsByTagName("td");
            for (var i = 1; i < col.length; i++) {
            }
        }
        changestype();
    }
    $('#backButton').live('click', function () {
        //history.back();
        window.location.href = "/WMS/InventoryManagement/Index"
    })
    //$('select[id=AdjustmentType]').live('change', function () {
    //    changestype();
    //});
    if ($('#AdjustmentNumber').val() == "系统自动生成") {
        addNew();
        if ($("#Adjustlocation").val() != "" & $("#Adjustlocation").val() != null) {
            AddLocationNew("00001", $("#Adjustlocation").val().toString().trim(), $("#AdjustSku").val().toString().trim(), $("#AdjustUPC").val().toString().trim(), $("#AdjustBatchNumber").val(),
                $("#AdjustBoxNumber").val(), $("#AdjustUnit").val(), $("#AdjustSpecifications").val(),
                $("#GoodsName").val(), $("#Qty").val(), $("#GoodsType").val())
        }
    }
    for (var i = 0; i < length; i++) {

    }
    $("#PrintLabel").live('click', function () {
        openPopup('NewPrintpopp', true, 350, 250, null, 'PrintDiv');
        $("#popupLayer_NewPrintpopp")[0].style.top = "200px";
    });
    $("#PrintCancel").live('click', function () {
        closePopup();
    });
    $("#PrintOK").live('click', function () {
        $.ajax({
            url: "/WMS/InventoryManagement/GetPrintByAdjust",
            type: "POST",
            dataType: "json",
            async: false,
            data: { AdjustNumber: $("#adjustNumbers").val() },
            success: function (data) {
                if (data.ErrorCode == "1") {
                    closePopup();
                    var html = $("#Evaluation").render(data.Response);
                    $("#DisInfoBody")["empty"]();
                    $("#DisInfoBody").append(html);
                    doPrint("打印")
                    //LODOP = getLodop();
                    //for (var i = 0; i < data.AdjustInfo.length; i++) {
                    //    var QRClode = '[{"GoodsName": ' + data.AdjustInfo[i].GoodsName + ', "SKU": ' + data.AdjustInfo[i].SKU + ', "ProductionDate": "", "ExpirationDate": "", "BatchNumber": ' + data.AdjustInfo[i].BatchNumber + ', "Manufacturer":' + data.AdjustInfo[i].Manufacturer + ', "BoxNumber": ' + data.AdjustInfo[i].BoxNumber + ', "ToQty":' + data.AdjustInfo[i].ToQty + ', "NetWeight": "", "GrossWeight": ""  }]';
                    //    LODOP.PRINT_INIT("");
                    //    LODOP.SET_PRINT_PAGESIZE(1, 970, 700, "");
                    //    AddPrintContent(data.AdjustInfo[i], QRClode);
                    //    LODOP.PRINT();
                    //}

                }

            }
        });
    });
})
function AddPrintContent(strCode, QRClode) {
    LODOP.SET_PRINT_STYLE("FontColor", 16711680);
    LODOP.ADD_PRINT_RECT(62, 16, 459, 217, 0, 1);
    LODOP.SET_PRINT_STYLEA(4, "FontColor", 0);
    LODOP.ADD_PRINT_TEXT(70, 237, 431, 20, "名称:" + strCode.GoodsName);
    LODOP.ADD_PRINT_TEXT(90, 237, 431, 20, "产品编码：" + strCode.SKU);
    LODOP.ADD_PRINT_TEXT(110, 237, 431, 20, "生产日期：");
    LODOP.ADD_PRINT_TEXT(130, 237, 431, 20, "过期日期：");
    LODOP.ADD_PRINT_TEXT(150, 237, 431, 20, "生产批次：" + strCode.BatchNumber);
    LODOP.ADD_PRINT_TEXT(170, 237, 431, 20, "供应商：" + strCode.Manufacturer);
    LODOP.ADD_PRINT_TEXT(190, 237, 431, 20, "流水号：" + strCode.BoxNumber);
    LODOP.ADD_PRINT_TEXT(210, 237, 431, 20, "数量：" + strCode.ToQty);
    LODOP.ADD_PRINT_TEXT(230, 237, 431, 20, "净重：");
    LODOP.ADD_PRINT_TEXT(250, 237, 431, 20, "毛重：");
    LODOP.ADD_PRINT_BARCODE(75, 37, 200, 200, "QRCode", QRClode);
};
function changestype() {
    if ($("#AdjustmentType").val() == "库存冻结单" || $("#hide").val() == "库存冻结单") {
        $("#lblnumber")[0].innerHTML = "冻结单号";
        $("#lbltime")[0].innerHTML = "冻结时间";
        $("#lblcounts")[0].innerHTML = "冻结数量";
        $("#lblreson")[0].innerHTML = "冻结原因";
        $("#lbloldcount")[0].innerHTML = "可冻结库存";
        if ($('#HiddenViewType').val() == 1) {
            document.getElementById("lblhold").style.display = "block";
            document.getElementById("ishold").style.display = "block";
            Cleartext();
            //$(".ToLocation").children("input").attr("readonly", "readonly");
            $(".ToGoodsType").children("select").attr("disabled", "true");
        }
    }
    if ($("#AdjustmentType").val() == "库存调整单" || $("#hide").val() == "库存调整单") {
        $("#lblnumber")[0].innerHTML = "调整单号";
        $("#lbltime")[0].innerHTML = "调整时间";
        $("#lblcounts")[0].innerHTML = "调整后数量";
        $("#lblreson")[0].innerHTML = "调整原因";
        $("#lbloldcount")[0].innerHTML = "可调整库存";
        if ($('#HiddenViewType').val() == 1) {
            document.getElementById("lblhold").style.display = "none";
            document.getElementById("ishold").style.display = "none";
            Cleartext();
            //$(".ToLocation").children("input").attr("readonly", "readonly");
            $(".ToGoodsType").children("select").removeAttr("disabled");
        }
    }
    if ($("#AdjustmentType").val() == "库存移动单" || $("#hide").val() == "库存移动单") {
        $("#lblnumber")[0].innerHTML = "移动单号";
        $("#lbltime")[0].innerHTML = "移动时间";
        $("#lblcounts")[0].innerHTML = "移动数量";
        $("#lblreson")[0].innerHTML = "移动原因";
        $("#lbloldcount")[0].innerHTML = "可移动库存";
        if ($('#HiddenViewType').val() == 1) {
            document.getElementById("lblhold").style.display = "none";
            document.getElementById("ishold").style.display = "none";
            Cleartext();
            $(".ToLocation").children("input").removeAttr("readonly");
            $(".ToGoodsType").children("select").attr("disabled", "true");
        }
    }
}
function Cleartext() {
    var table = document.getElementById("Newtable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        for (var i = 0; i < col.length - 1; i++) {
            var tds = row[j].getElementsByTagName("td");
            if (i >= 2 && i != 3 && i != 4 && i != 5 && i != 6 && i != 7 && i != 8 && i != 14) {
                tds[i].childNodes[0].value = "";
            }
            if (i == 3 || i == 4 || i == 5 || i == 6 || i == 7 || i == 8 || i == 14) {
                if (tds[i].childNodes[0].length > 1) {
                    tds[i].childNodes[0].length = 0
                }
            }
        }
    }
}
//行号处理
function ReturnLineNumber(row_count) {
    var linnumber = "";
    if (row_count < 10) {
        linnumber = "0000" + row_count;
    }
    if (100 > row_count && row_count >= 10) {
        linnumber = "000" + row_count;
    }
    if (1000 > row_count && row_count >= 100) {
        linnumber = "00" + row_count;
    }
    if (row_count >= 1000) {
        linnumber = "0" + row_count;
    }
    return linnumber;
}
//创建select
function createSelect() {
    var mySelect = document.createElement("sku");
    mySelect.id = "skulistSelect";
    document.body.appendChild(mySelect);
}
//原库位     
$(".OldLocation").live("keydown", function () {

    var LineNumber = $(this).parent().prev()[0].innerText;

    var self = this;
    var area_location = "";
    if (window.event.keyCode == 13) {
        $.ajax({
            url: "/WMS/InventoryManagement/GetLocationListAkzo",
            type: "POST",
            dataType: "json",
            async: false,
            data: { location: $(this).val(), warehouseid: $("#WarehouseID").val(), areaid: 0 },
            success: function (data) {
                if (data != null && data.length > 0) {
                    area_location = data[0]["Text"];
                }
            }
        });
    }
    if (area_location == "") {
        return;
    }
    $(this).val(area_location);
    $(this).parent().parent().find(".ToLocation").children().val(area_location);//ToLocationChild
    $//根据选中的库位    查出sku集合       并生成下拉列表框
    $.ajax({
        url: "/WMS/InventoryManagement/GetInventoryskuList",
        type: "POST",
        dataType: "json",
        data: {
            CustomerID: $("#CustomerID").val(),
            location: area_location,
            sku: "", upc: "", goodstype: "", batchnumber: "",
            boxnumber: "", warehouse: $("#WarehouseID option:selected")[0].innerText,
            Unit: "", Specifications: ""
        },
        success: function (data) {
            if (data == null || data == "") {
                showMsg("该库位上面没有可用sku", "4000");
                return;
            }
            else {
                var skuSelect = $(self).parent().parent().children(".SKU").children("select")[0];
                var upcSelect = $(self).parent().parent().children(".UPC").children("select")[0];
                var batchnumberSelect = $(self).parent().parent().children(".BatchNumber").children("select")[0];
                var boxnumberSelect = $(self).parent().parent().children(".BoxNumber").children("select")[0];
                var UnitSelect = $(self).parent().parent().children(".Unit").children("select")[0];
                var SpecificationsSelect = $(self).parent().parent().children(".Specifications").children("select")[0];
                skuSelect.length = 0;
                upcSelect.length = 0;
                batchnumberSelect.length = 0;
                boxnumberSelect.length = 0;
                UnitSelect.length = 0;
                SpecificationsSelect.length = 0;
                var opts = new Option("", "", true, true);
                var optu = new Option("", "", true, true);
                var optba = new Option("", "", true, true);
                var optbo = new Option("", "", true, true);
                var optun = new Option("", "", true, true);
                var optsp = new Option("", "", true, true);
                skuSelect.options.add(opts);
                upcSelect.options.add(optu);
                batchnumberSelect.options.add(optba);
                boxnumberSelect.options.add(optbo);
                UnitSelect.options.add(optun);
                SpecificationsSelect.options.add(optsp);
                for (var i = 0; i < data.length; i++) {
                    if (data[i].SKU != "" && data[i].SKU != null) {
                        var flag = 0;
                        for (var m = 0; m < skuSelect.length; m++) {
                            if (skuSelect[m].innerText == data[i].SKU + "|" + data[i].GoodsType)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            var opt0 = new Option(data[i].SKU + "|" + data[i].GoodsType, data[i].SKU + "|" + data[i].GoodsType);
                            skuSelect.options.add(opt0);
                        }
                    }
                    if (data[i].UPC != "" && data[i].UPC != null) {
                        var flag = 0;
                        for (var m = 0; m < upcSelect.length; m++) {
                            if (upcSelect[m].innerText == data[i].UPC)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            var opt0 = new Option(data[i].UPC, data[i].UPC);
                            upcSelect.options.add(opt0);
                        }
                    }
                    if (data[i].BatchNumber != "" && data[i].BatchNumber != null) {
                        var flag = 0;
                        for (var m = 0; m < batchnumberSelect.length; m++) {
                            if (batchnumberSelect[m].innerText == data[i].BatchNumber)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            var opt1 = new Option(data[i].BatchNumber, data[i].BatchNumber);
                            batchnumberSelect.options.add(opt1);
                        }
                    }
                    if (data[i].BoxNumber != "" && data[i].BoxNumber != null) {
                        var flag = 0;
                        for (var m = 0; m < boxnumberSelect.length; m++) {
                            if (boxnumberSelect[m].innerText == data[i].BoxNumber)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            var opt = new Option(data[i].BoxNumber, data[i].BoxNumber);
                            boxnumberSelect.options.add(opt);
                        }
                    }
                    if (data[i].Unit != "" && data[i].Unit != null) {
                        var flag = 0;
                        for (var m = 0; m < UnitSelect.length; m++) {
                            if (UnitSelect[m].innerText == data[i].Unit)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            var opt = new Option(data[i].Unit, data[i].Unit);
                            UnitSelect.options.add(opt);
                        }
                    }
                    if (data[i].Specifications != "" && data[i].Specifications != null) {
                        var flag = 0;
                        for (var m = 0; m < SpecificationsSelect.length; m++) {
                            if (SpecificationsSelect[m].innerText == data[i].Specifications)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            var opt = new Option(data[i].Specifications, data[i].Specifications);
                            SpecificationsSelect.options.add(opt);
                        }
                    }
                }
                var objSelect = $($(self).parent().parent()).children(".ToGoodsType").children("select")[0];
                objSelect.length = 0;
                //var opt1 = new Option("A品", "A品");
                //var opt2 = new Option("B品", "B品");
                //var opt3 = new Option("C品", "C品");
                //var opt4 = new Option("D品", "D品");
                //objSelect.options.add(opt1);
                //objSelect.options.add(opt2);
                //objSelect.options.add(opt3);
                //objSelect.options.add(opt4);
                var ProductLevel = $("#ProductLevel");
                for (var i = 0; i < ProductLevel[0].length; i++) {
                    if (ProductLevel[0][i].innerText == "") {
                        var opt = new Option(ProductLevel[0][i].innerText, ProductLevel[0][i].innerText, true, true);
                    }
                    else {
                        var opt = new Option(ProductLevel[0][i].innerText, ProductLevel[0][i].innerText);
                    }
                    objSelect.options.add(opt);
                }
            }
        }
    })
});

$("select[name = 'sku']").live("change", function () {
    var self = this;
    GetLinkageMenu(self, 1)
})

$("select[name = 'upc']").live("change", function () {
    var self = this;
    GetLinkageMenu(self, 2)
})
$("select[name = 'batchnumber']").live("change", function () {
    var self = this;
    GetLinkageMenu(self, 3)
})
$("select[name = 'boxnumber']").live("change", function () {
    var self = this;
    GetLinkageMenu(self, 4)
})
$("select[name = 'Unit']").live("change", function () {
    var self = this;
    GetLinkageMenu(self, 5)
})
$("select[name = 'Specifications']").live("change", function () {
    var self = this;
    GetLinkageMenu(self, 6)
})
function GetLinkageMenu(self, num) {
    var CustomerID = $("#CustomerID").val();
    var location = $($(self).parent().parent()).children(".FromLocation").children("input")[0];
    var sku = $($(self).parent().parent()).children(".SKU").children("select")[0];
    var goodstype = $($(self).parent().parent()).children(".SKU").children("select").find('option:selected').val().split("|")[1];
    var upc = $($(self).parent().parent()).children(".UPC").children("select")[0];
    var batchnumber = $($(self).parent().parent()).children(".BatchNumber").children("select")[0];
    var boxnumber = $($(self).parent().parent()).children(".BoxNumber").children("select")[0];
    var warehouse = $("#WarehouseID option:selected")[0].innerText;
    var Unit = $($(self).parent().parent()).children(".Unit").children("select")[0];
    var Specifications = $($(self).parent().parent()).children(".Specifications").children("select")[0];

    $.ajax({
        url: "/WMS/InventoryManagement/GetInventoryskuList",
        type: "POST",
        dataType: "json",
        data: {
            CustomerID: CustomerID,
            location: location.value,
            sku: $(sku).find('option:selected').val().split("|")[0],
            upc: $(upc).find('option:selected').val(),
            goodstype: goodstype,
            batchnumber: $(batchnumber).find('option:selected').val(),//$($(self).parent().parent()).children(".BatchNumber").children("select").find('option:selected').val(),
            boxnumber: $(boxnumber).find('option:selected').val(),// $($(self).parent().parent()).children(".BoxNumber").children("select").find('option:selected').val(),
            warehouse: warehouse,// $("#WarehouseID option:selected")[0].innerText,
            Unit: $(Unit).find('option:selected').val(),
            Specifications: $(Specifications).find('option:selected').val()
        },
        success: function (data) {
            if (data.length > 0) {
                var batchnumberSelect = $(self).parent().parent().children(".BatchNumber").children("select")[0];
                var boxnumberSelect = $(self).parent().parent().children(".BoxNumber").children("select")[0];
                var UnitSelect = $(self).parent().parent().children(".Unit").children("select")[0];//document.getElementById("Unit" + LineNumber);
                var SpecificationsSelect = $(self).parent().parent().children(".Specifications").children("select")[0];
                if (num < 2) {
                    batchnumberSelect.length = 0;
                    var optba = new Option("", "", true, true);
                    batchnumberSelect.options.add(optba);
                    if (num < 3) {
                        boxnumberSelect.length = 0;
                        var optbo = new Option("", "", true, true);
                        boxnumberSelect.options.add(optbo);

                        if (num < 4) {
                            UnitSelect.length = 0;
                            var optun = new Option("", "", true, true);
                            UnitSelect.options.add(optun);
                            if (num < 5) {
                                SpecificationsSelect.length = 0;
                                var optsp = new Option("", "", true, true);
                                SpecificationsSelect.options.add(optsp);
                            }
                        }
                    }
                }
                for (var i = 0; i < data.length; i++) {
                    if (data[i].BatchNumber != "" && data[i].BatchNumber != null && num < 3) {
                        var flag = 0;
                        for (var m = 0; m < batchnumberSelect.length; m++) {
                            if (batchnumberSelect[m].innerText == data[i].BatchNumber)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            var opt1 = new Option(data[i].BatchNumber, data[i].BatchNumber);
                            batchnumberSelect.options.add(opt1);
                        }
                    }
                    if (data[i].BoxNumber != "" && data[i].BoxNumber != null && num < 4) {

                        var flag = 0;
                        for (var m = 0; m < boxnumberSelect.length; m++) {
                            if (boxnumberSelect[m].innerText == data[i].BoxNumber)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            var opt = new Option(data[i].BoxNumber, data[i].BoxNumber);
                            boxnumberSelect.options.add(opt);
                        }
                    }
                    if (data[i].Unit != "" && data[i].Unit != null && num < 5) {


                        var flag = 0;
                        for (var m = 0; m < UnitSelect.length; m++) {
                            if (UnitSelect[m].innerText == data[i].Unit)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            var opt = new Option(data[i].Unit, data[i].Unit);
                            UnitSelect.options.add(opt);
                        }
                    }
                    if (data[i].Specifications != "" && data[i].Specifications != null && num < 7) {


                        var flag = 0;
                        for (var m = 0; m < SpecificationsSelect.length; m++) {
                            if (SpecificationsSelect[m].innerText == data[i].Specifications)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            var opt = new Option(data[i].Specifications, data[i].Specifications);
                            SpecificationsSelect.options.add(opt);
                        }
                    }
                }

                if ($("#AdjustmentType").val() == "库存调整单") {
                    $(self).parent().parent().children(".ToLocation").children("input")[0].value = $(self).parent().parent().children(".FromLocation").children("input")[0].value;
                    //$(self).parent().parent().children(".ToLocation").children("input").attr("readonly", "readonly");
                    $(self).parent().parent().children(".ToGoodsType").children("input").disabled = true;
                } else {
                    $(self).parent().parent().children(".ToGoodsType").children("input").disabled = false;
                }
                $(self).parent().parent().children(".FromGoodsType").children("input")[0].value = data[0].GoodsType
                $(self).parent().parent().children(".GoodsName").children("input")[0].value = data[0].GoodsName
                var objSelect = $(self).parent().parent().children(".ToGoodsType").children("select")[0];
                for (var i = 0; i < objSelect.options.length; i++) {
                    if (objSelect[i].value == data[0].GoodsType) {
                        objSelect.options[i].selected = true;
                        break;
                    }
                }
                var n = 0;
                var lens = data.length;
                if (lens > 0) {
                    for (var i = 0; i < lens; i++) {
                        if ((data[i].SKU == $(sku).find("option:selected").val().split("|")[0] || $(sku).find("option:selected").val().split("|")[0] == "")
                            && (data[i].BatchNumber == $(batchnumber).find("option:selected")[0].value || $(batchnumber).find("option:selected")[0].value == "")
                           && (data[i].BoxNumber == $(boxnumber).find("option:selected")[0].value || $(boxnumber).find("option:selected")[0].value == "")
                            && (data[i].Unit == $(Unit).find("option:selected")[0].value || $(Unit).find("option:selected")[0].value == "")) {
                            n += parseFloat(data[i].qty);
                        }
                    }
                }
                $(self).parent().parent().children(".FromQty").children("input")[0].value = n;
                $(self).parent().parent().children(".ToQty").children("input")[0].value = n;
            }
            else {
                if ($("#AdjustmentType").val() == "库存调整单") {
                    $(self).parent().parent().children(".ToLocation").children("input")[0].value = "";
                    //$(self).parent().parent().children(".ToLocation").children("input").attr("readonly", "readonly");
                    $(self).parent().parent().children(".ToGoodsType").children("input").disabled = true;
                } else {
                    $(self).parent().parent().children(".ToGoodsType").children("input").disabled = false;
                }
                $(self).parent().parent().children(".FromGoodsType").children("input")[0].value = "";
                $(self).parent().parent().children(".GoodsName").children("input")[0].value = "";
                $(self).parent().parent().children(".FromQty").children("input")[0].value = "";
                var objSelect = $(self).parent().parent().children(".ToGoodsType").children("input")[0];
                var objSelect = $(self).parent().parent().children(".ToQty").children("input")[0].value = "";
            }
        },
        error: function (data, status, e) {
            var a = 1;
        }
    })
}
$(".ToLocationChild").live("keydown", function () {
    var LineNumber = $(this).parent().parent().children(".LineNumber")[0].innerText;
    var area_location = "";
    if (window.event.keyCode == 13) {
        $.ajax({
            url: "/WMS/InventoryManagement/GetLocationListAkzo",
            type: "POST",
            dataType: "json",
            async: false,
            data: { location: $(this).val(), warehouseid: $("#WarehouseID").val(), areaid: 0 },
            success: function (data) {
                if (data != null && data.length > 0) {
                    area_location = data[0]["Text"];
                }
            }
        });
    }
    if (area_location == "") {
        return;
    }
    $(this).val(area_location);
    //$('#ToLocation' + LineNumber).autocomplete({

    //    source: function (request, response) {
    //        if (request.term.length > 5) {
    //            $.ajax({
    //                url: "/WMS/InventoryManagement/GetLocationList",
    //                type: "POST",
    //                dataType: "json",
    //                data: { location: request.term, warehouseid: $("#WarehouseID").val(), areaid: 0 },
    //                success: function (data) {
    //                    response($.map(data, function (item) {
    //                        return { label: item.Text, value: item.Text, data: item }
    //                    }));
    //                }
    //            })
    //        };
    //    },
    //    select: function (event, ui) {
    //        $(this).val(ui.item.data.Text);
    //    }
    //})
});
//$(".ToLocationChild").live("keydown", function () {
//    var LineNumber = $(this).parent().parent().children(".LineNumber")[0].innerText;
//    $('#ToLocation' + LineNumber).autocomplete({

//        source: function (request, response) {
//            if (request.term.length > 5) {
//                $.ajax({
//                    url: "/WMS/InventoryManagement/GetLocationList",
//                    type: "POST",
//                    dataType: "json",
//                    data: { location: request.term, warehouseid: $("#WarehouseID").val(), areaid: 0 },
//                    success: function (data) {
//                        response($.map(data, function (item) {
//                            return { label: item.Text, value: item.Text, data: item }
//                        }));
//                    }
//                })
//            };
//        },
//        select: function (event, ui) {
//            $(this).val(ui.item.data.Text);
//        }
//    })
//});

var row_count = 1;
//得到明细中的总条数
var asndetailcount = 0;
var skulist = new Array();
function addNew() {
    var table1 = $('#Newtable');
    var rownumber = $('#Newtable tr').length;
    var firstTr = table1.find('tbody>tr:first');
    var LineNumber = ReturnLineNumber(rownumber);
    var row = $("<tr id=Row" + rownumber + "</tr>");
    var td0 = $(" <td style='position: relative;width:50px;'></td>");
    var td1 = $("<td class='LineNumber' style='width:50px'></td>");
    var td2 = $("<td class='FromLocation' style='width:130px'></td>");
    var td3 = $("<td class='SKU' style='width:200px'></td>");
    var td4 = $("<td class='UPC' style='width:130px'></td>");
    var td5 = $("<td class='BatchNumber' style='width:130px'></td>");
    var td6 = $("<td class='BoxNumber' style='width:130px'></td>");
    var td7 = $("<td class='Unit' style='width:80px'></td>");
    var td8 = $("<td class='Specifications' style='width:80px'></td>");
    var td9 = $("<td class='FromGoodsType' style='width:80px'></td>");
    var td10 = $("<td class='GoodsName' style='width:150px'></td>");
    var td11 = $("<td class='FromQty' style='width:80px'></td>");
    var td12 = $("<td class='ToLocation' style='width:130px'></td>");
    var td13 = $("<td class='ToQty' style='width:80px'></td>");
    var td14 = $("<td class='ToGoodsType' style='width:80px'></td>");
    var td15 = $("<td class='AdjustmentReason' style='width:130px'></td>");
    td0.append($("<div class='Adiv' style='position: absolute; display: none; width: 500px; left: 50px; height: 31px;'>\
                                    <label id='labelRemove' style='cursor: pointer;' class='btn btn-primary btn-xs'>删除</label>\
                                    <label style='cursor: pointer;' class='btn btn-primary btn-xs   addNew' onclick='addNew()'>新增</label>\
                                </div>\
                                <label style='cursor: pointer;' class='btn btn-primary btn-xs Ooperation'>操作</label> "));
    td1.append($("<label style='width:50px'>" + LineNumber + "</label>"));
    td2.append($("<input type='text' id='OldLocation" + LineNumber + "' name='OldLocation'  style='width:130px'  class='form-control OldLocation'  value=''/>"));
    td3.append($("<select id='sku" + LineNumber + "' name='sku'  style='width:200px'  class='form-control skuChild sku" + LineNumber + "' />"));
    td4.append($("<select id='upc" + LineNumber + "' name='upc'  style='width:130px'  class='form-control upcChild upc" + LineNumber + "' />"));
    td5.append($("<select id='batchnumber" + LineNumber + "' name='batchnumber'  style='width:130px'  class='form-control batchnumber" + LineNumber + "' />"));
    td6.append($("<select id='boxnumber" + LineNumber + "' name='boxnumber'  style='width:130px'  class='form-control boxnumber" + LineNumber + "' />"));
    td7.append($("<select id='Unit" + LineNumber + "' name='Unit'  style='width:80px'  class='form-control Unit" + LineNumber + "' />"));
    td8.append($("<select id='Specifications" + LineNumber + "' name='Specifications'  style='width:80px'  class='form-control Specifications" + LineNumber + "' />"));
    td9.append($("<input type='text' name='FromGoodsType' id='FromGoodsType" + LineNumber + "' Readonly='Readonly' style='width:80px' class='form-control FromGoodsType" + LineNumber + "'   value='' />"));
    td10.append($("<input type='text'   name='GoodsName' id='GoodsName" + LineNumber + "'  Readonly='Readonly' style='width:150px' class='form-control GoodsName" + LineNumber + "'   value='' />"));
    td11.append($("<input type='text'   name='FromQty' id='FromQty" + LineNumber + "'  Readonly='Readonly' style='width:80px' class='form-control FromQty" + LineNumber + "'   value='' />"));
    if ($("#AdjustmentType").val() == "库存冻结单" || $("#hide").val() == "库存冻结单") {
        //$(".ToLocation").children("input").attr("readonly", "readonly");
        td12.append($("<input type='text'   name='ToLocationChild' id='ToLocation" + LineNumber + "' style='width:130px' class='form-control ToLocationChild'   value='' />"));
    }
    if ($("#AdjustmentType").val() == "库存调整单" || $("#hide").val() == "库存调整单") {
        //$(".ToLocation").children("input").attr("readonly", "readonly");
        td12.append($("<input type='text'   name='ToLocationChild' id='ToLocation" + LineNumber + "' style='width:130px'  class='form-control ToLocationChild'   value='' />"));
    }
    if ($("#AdjustmentType").val() == "库存移动单" || $("#hide").val() == "库存移动单") {
        //$(".ToLocation").children("input").removeAttr("readonly");
        td12.append($("<input type='text'   name='ToLocationChild' id='ToLocation" + LineNumber + "' style='width:130px' class='form-control ToLocationChild'   value='' />"));
    }
    if ($("#AdjustmentType").val() == "库存品级调整单" || $("#hide").val() == "库存品级调整单") {
        //$(".ToLocation").children("input").removeAttr("readonly");
        td12.append($("<input type='text'   name='ToLocationChild' id='ToLocation" + LineNumber + "' style='width:130px' class='form-control ToLocationChild'   value='' />"));
    }
    td13.append($("<input type='text'   name='ToQty' id='ToQty" + LineNumber + "' style='width:80px'  class='form-control ToQty" + LineNumber + "'   value='' onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)' />"));
    if ($("#AdjustmentType").val() == "库存冻结单" || $("#hide").val() == "库存冻结单") {
        //$(".ToGoodsType").children("select").attr("disabled", "true");
        td14.append($("<select id='ToGoodsType" + LineNumber + "'  name='ToGoodsType' style='width:80px' disabled='true'  class='form-control ToGoodsType" + LineNumber + "'   value='' />"));
    }
    if ($("#AdjustmentType").val() == "库存品级调整单" || $("#hide").val() == "库存品级调整单") {
        //$(".ToGoodsType").children("select").attr("disabled", "true");
        td14.append($("<select id='ToGoodsType" + LineNumber + "'  name='ToGoodsType' style='width:80px'  class='form-control ToGoodsType" + LineNumber + "'   value='' />"));
    }
    if ($("#AdjustmentType").val() == "库存调整单" || $("#hide").val() == "库存调整单") {
        //$(".ToGoodsType").children("select").removeAttr("disabled");
        td14.append($("<select id='ToGoodsType" + LineNumber + "'  name='ToGoodsType' style='width:80px' class='form-control ToGoodsType" + LineNumber + "'   value='' />"));
    }
    if ($("#AdjustmentType").val() == "库存移动单" || $("#hide").val() == "库存移动单") {
        //$(".ToGoodsType").children("select").attr("disabled", "true");
        td14.append($("<select id='ToGoodsType" + LineNumber + "'  name='ToGoodsType' style='width:80px'  disabled='true'  class='form-control ToGoodsType" + LineNumber + "'   value='' />"));
    }
    td15.append($("<input type='text' name='AdjustmentReason' style='width:130px'  class='form-control AdjustmentReason" + LineNumber + "' value=''   />"));
    row.append(td0);
    row.append(td1);
    row.append(td2);
    row.append(td3);
    row.append(td4);
    row.append(td5);
    row.append(td6);
    row.append(td7);
    row.append(td8);
    row.append(td9);
    row.append(td10);
    row.append(td11);
    row.append(td12);
    row.append(td13);
    row.append(td14);
    row.append(td15);
    table1.append(row);
    row_count++;
    $('#OldLocation' + LineNumber).focus();
}
function contains(arr, obj) {
    var i = arr.length;
    while (i--) {
        if (arr[i] === obj) {
            return true;
        }
    }
    return false;
}
function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    if (pattern.test(hehe.value)) {
        hehe.value = hehe.value.replace(pattern, "");
    }
}

$('#labelRemove').live('click', function () {
    if ($("#Newtable tbody tr").length == 1) {
        showMsg("请至少保留一行！", "4000");
    } else {
        $(this).parent().parent().parent().remove();
    }
});
//暂存
var CustomerID = "";
var CustomerName = "";
var AID = "";
var IsSubmit = true;//用于判断是否直接提交  true:代表先点击暂存再提交    false:直接提交
var message = "";
function SameSKU() {
    var lengths = $('#Newtable')[0].rows.length - 1;
    if (lengths > 0) {
        for (var i = 1; i < lengths; i++) {
            var sku1 = $('#Newtable')[0].rows[i].cells[2].children[0].value.trim();
            var batch1 = $('#Newtable')[0].rows[i].cells[3].children[0].value.trim();
            var box1 = $('#Newtable')[0].rows[i].cells[4].children[0].value.trim();
            var goodtype1 = $('#Newtable')[0].rows[i].cells[5].children[0].value.trim();
            var str1 = sku1 + batch1 + box1 + goodtype1;
            for (var j = i + 1; j <= lengths; j++) {
                var sku2 = $('#Newtable')[0].rows[j].cells[2].children[0].value.trim();
                var batch2 = $('#Newtable')[0].rows[j].cells[3].children[0].value.trim();
                var box2 = $('#Newtable')[0].rows[j].cells[4].children[0].value.trim();
                var goodtype2 = $('#Newtable')[0].rows[j].cells[5].children[0].value.trim();
                var str2 = sku2 + batch2 + box2 + goodtype2;

                if (str1 == str2) {

                    return false;
                }
            }
        }
    }
    else { return false; }

    return true;
}
function AddAdjustAndAdjustDetail(IsSubmit) {
    CustomerID = $('#CustomerID').val();
    if (CustomerID == 0) {
        showMsg("请选择客户!", "4000");
        return;
    }
    CustomerName = $('select#CustomerID').find('option:selected').text();

    //var AdjustmentType = $("#AdjustmentType").find("option:selected").text();
    var AdjustmentType = $("#AdjustmentType").val();
    if (AdjustmentType == "==请选择==" || AdjustmentType == null) {
        CustomerID = "";
        showMsg("请选择预入库单类型!", "4000");
        return;
    }
    var WarehouseID = $('#WarehouseID').val();
    if (WarehouseID == 0 || WarehouseID == null) {
        CustomerID = "";
        showMsg("请选择仓库!", "4000");
        return;
    }
    if (!SameSKU()) {
        showMsg("SKU不能重复!", 4000);
        return;
    }
    var Warehousename = $('select#WarehouseID').find('option:selected').text();
    var adjustmenttime = $('#AdjustmentAndAdjustmentDetails_adjustment_AdjustmentTime').val();
    //var AdjustmentReason = $('#AdjustmentReason').val();
    var AdjustmentRemark = $('#AdjustmentRemark').val();
    var lengths = $('#Newtable')[0].rows.length - 1;
    if (lengths > 0) {
        for (var i = 1; i <= lengths; i++) {

            if ($($('#Newtable')[0].rows[i]).children(".FromLocation").children("input")[0].value == '') {
                CustomerID = "";
                showMsg("请选择原库位!", 4000);
                return;
            }
            if ($($('#Newtable')[0].rows[i]).children(".ToLocation").children("input")[0].value == '') {
                CustomerID = "";
                showMsg("请选择新库位!", 4000);
                return;
            }
            if (AdjustmentType == "库存移动单") {
                if ($($('#Newtable')[0].rows[i]).children(".FromLocation").children("input")[0].value == $($('#Newtable')[0].rows[i]).children(".ToLocation").children("input")[0].value) {
                    CustomerID = "";
                    showMsg("新库位不能跟原库位一样!", 4000);
                    return;
                }
                if ($($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '' || $($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '0') {
                    CustomerID = "";
                    showMsg("移动数量不能为0!", 4000);
                    return;
                }
            }
            if (AdjustmentType == "库存冻结单") {
                if ($($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '' || $($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '0') {
                    CustomerID = "";
                    showMsg("冻结数量不能为0!", 4000);
                    return;
                }
            }
        }
    }
    //子表数据
    var JsonTable = addjsontotable();
    //主表数据
    var JsonField = FieldSetToJson();
    var Warehousename = $('select#WarehouseID').find('option:selected').text();
    $.ajax({
        type: "Post",
        url: "/WMS/InventoryManagement/AddAdjustAndAdjustDetail",
        data: {
            "JsonTable": JsonTable,
            "CustomerID": CustomerID,
            "CustomerName": CustomerName,
            "AdjustmentType": AdjustmentType,
            "adjustmenttime": adjustmenttime,
            "AdjustmentReason": "",
            "JsonField": JsonField,
            "WarehouseID": 0,
            "Warehousename": Warehousename,
            "ADID": $('#hiddenid').val(),
            "AdjustmentRemark": AdjustmentRemark
        },
        async: "false",
        success: function (data) {

            if (data == 0 || data == "") {
                showMsg("暂存失败", "4000");
                return;
            }
            if (data.indexOf("添加成功") >= 0) {

                AID = data.substring(4, data.length);
                $('#hiddenid').val(AID);
                if (IsSubmit) {
                    showMsg("暂存成功", "4000");
                    message = "";
                    skulist.length = 0;
                    document.getElementById("zancunButton").disabled = true;

                }
                else {
                    $.ajax({
                        type: "Post",
                        url: "/WMS/InventoryManagement/UpdateAndInsertInventory",
                        data: {
                            "JsonTable": JsonTable,
                            "CustomerName": CustomerName,
                            "warehouse": Warehousename,
                            "Inventorytype": AdjustmentType,
                            "aid": AID
                        },
                        async: "false",
                        success: function (data) {
                            //if (data == 0 || data == "") {
                            //    showMsg("提交失败", "4000");
                            //    return;
                            //} else
                            if (data.indexOf("提交成功") >= 0) {
                                showMsg("提交成功", "4000");
                                message = "";
                                skulist.length = 0;
                                var d = data.substring(4, data.length);


                                //if (AdjustmentType == "库存调整单") {
                                //    window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0&Adjustflag=调整打印标记";
                                //}
                                //else {
                                //    window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0";
                                //}
                                window.location.href = "/WMS/InventoryManagement/AdjustGoodsTypeAddorEditorView/?ID=" + d + "&ViewType=0";

                            } else {
                                showMsg("提交失败" + data, "4000");
                                message = "";
                                skulist.length = 0;
                                CustomerID = "";
                                return;
                            }
                        },
                        error: function (msg) {
                            showMsg("提交失败", "4000");
                        }
                    });
                }
            }
            else {
                showMsg(data, "4000");
            }
        },
        error: function (msg) {

            showMsg("暂存失败", "4000");
        }
    });
}
//更新操作
function UpdateAdjustAndAdjustDetail(ID) {
    if (!Submitconfirm()) {
        return;
    };
    CustomerID = $('select#CustomerID').find('option:selected')[0].value;
    CustomerName = $('select#CustomerID').find('option:selected').text();
    //var AdjustmentType = $("#AdjustmentType").find("option:selected").text();
    var AdjustmentType = $("#AdjustmentType").val();
    var WarehouseID = $('#WarehouseID').val();
    var Warehousename = $('select#WarehouseID').find('option:selected').text();
    var adjustmenttime = $('#AdjustmentAndAdjustmentDetails_adjustment_AdjustmentTime').val();
    //var AdjustmentReason = $('#AdjustmentReason').val();
    var AdjustmentRemark = $('#AdjustmentRemark').val();
    var adjustmentnumber = $('#AdjustmentNumber').val();
    //子表数据
    var JsonTable = bianjiaddjsontotable();
    //主表数据
    var JsonField = FieldSetToJson();
    var lengths = $('#Newtable')[0].rows.length - 1;
    if (lengths > 0) {
        for (var i = 1; i <= lengths; i++) {

            if ($($('#Newtable')[0].rows[i]).children(".FromLocation").children("input")[0].value == '') {
                CustomerID = "";
                showMsg("请选择原库位!", 4000);
                return;
            }
            if ($($('#Newtable')[0].rows[i]).children(".ToLocation").children("input")[0].value == '') {
                CustomerID = "";
                showMsg("请选择新库位!", 4000);
                return;
            }
            if (AdjustmentType == "库存移动单") {
                if ($($('#Newtable')[0].rows[i]).children(".FromLocation").children("input")[0].value == $($('#Newtable')[0].rows[i]).children(".ToLocation").children("input")[0].value) {
                    CustomerID = "";
                    showMsg("新库位不能跟原库位一样!", 4000);
                    return;
                }
                if ($($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '' || $($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '0') {
                    CustomerID = "";
                    showMsg("移动数量不能为0!", 4000);
                    return;
                }
            }
            if (AdjustmentType == "库存冻结单") {
                if ($($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '' || $($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '0') {
                    CustomerID = "";
                    showMsg("冻结数量不能为0!", 4000);
                    return;
                }
            }
        }
    }
    $.ajax({
        type: "Post",
        url: "/WMS/InventoryManagement/UpdateAdjustAndAdjustDetail",
        data: {
            "JsonTable": JsonTable,
            "CustomerID": CustomerID,
            "CustomerName": CustomerName,
            "AdjustmentType": AdjustmentType,
            "adjustmenttime": adjustmenttime,
            "AdjustmentReason": "",
            "JsonField": JsonField,
            "WarehouseID": WarehouseID,
            "Warehousename": Warehousename,
            "ID": ID,
            "adjustmentnumber": adjustmentnumber,
            "AdjustmentRemark": AdjustmentRemark
        },
        async: "false",
        success: function (data) {
            if (data == 0 || data == "") {
                showMsg("更新失败", "4000");
                return;
            } else if (data.indexOf("更新成功") >= 0) {
                if (IsSubmit) {
                    showMsg("更新成功", "4000");
                    //document.getElementById("editbtn").disabled = true;
                } else {
                    $.ajax({
                        type: "Post",
                        url: "/WMS/InventoryManagement/UpdateAndInsertInventory",
                        data: {
                            "JsonTable": JsonTable,
                            "CustomerName": CustomerName,
                            "warehouse": Warehousename,
                            "Inventorytype": AdjustmentType,
                            "aid": ID
                        },
                        async: "false",
                        success: function (data) {
                            //if (data == 0 || data == "") {
                            //    showMsg("提交失败", "4000");
                            //    return;
                            //} else
                            if (data.indexOf("提交成功") >= 0) {
                                showMsg("提交成功", "4000");
                                var d = data.substring(4, data.length);
                                //if (AdjustmentType == "库存调整单") {
                                //    window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0&Adjustflag=调整打印标记";
                                //}
                                //else {
                                //    window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0";
                                //}
                                window.location.href = "/WMS/InventoryManagement/AdjustGoodsTypeAddorEditorView/?ID=" + d + "&ViewType=0";
                            }
                            else {
                                showMsg("提交失败" + data, "4000");
                            }
                        },
                        error: function (msg) {
                            showMsg("提交失败", "4000");
                        }
                    });
                }
            }
            else { showMsg(data, "4000") }
        },
        error: function (msg) {
            showMsg("更新失败", "4000");
        }
    })
}
//继续添加
function addangian() {
    var customerID = $('#CustomerID').val();
    location.href = "/WMS/InventoryManagement/AdjustGoodsTypeAddorEditorView/?ID=0" + "&customerID=" + customerID + "&ViewType=1"
}
//新增页面提交
function submitClick() {
    if (!Submitconfirm()) {
        return;
    };
    AddAdjustAndAdjustDetail(false);
}
//编辑页面提交
function Submitconfirm() {
    var lengths = $('#Newtable')[0].rows.length - 1;
    if (lengths > 0) {
        for (var i = 1; i <= lengths; i++) {
            if ($($('#Newtable')[0].rows[i]).children(".FromLocation").children("input")[0].value == '') {
                showMsg("请选择原库位!", 4000);
                return false;
            }
            if ($($('#Newtable')[0].rows[i]).children(".SKU").children("select").find('option:selected').val()[0] == '') {
                showMsg("请选择SKU!", 4000);
                return false;
            }
            if ($($('#Newtable')[0].rows[i]).children(".FromQty").children("input")[0].value == '' || $($('#Newtable')[0].rows[i]).children(".FromQty").children("input")[0].value == '0') {
                showMsg("可调整数量为0!", 4000);
                return false;
            }
            if ($($('#Newtable')[0].rows[i]).children(".ToLocation").children("input")[0].value == '') {
                showMsg("请选择新库位!", 4000);
                return false;
            }
            if ($("#AdjustmentType").val() == "库存移动单") {
                if ($($('#Newtable')[0].rows[i]).children(".FromLocation").children("input")[0].value == $($('#Newtable')[0].rows[i]).children(".ToLocation").children("input")[0].value) {
                    CustomerID = "";
                    showMsg("新库位不能跟原库位一样!", 4000);
                    return;
                }
                if ($($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '' || $($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '0') {
                    CustomerID = "";
                    showMsg("移动数量不能为0!", 4000);
                    return;
                }
            }
            if ($("#AdjustmentType").val() == "库存冻结单") {
                if ($($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '' || $($('#Newtable')[0].rows[i]).children(".ToQty").children("input")[0].value == '0') {
                    CustomerID = "";
                    showMsg("冻结数量不能为0!", 4000);
                    return;
                }
            }
        }
    }
    else { return false; }
    return true;
}
function editsubmit() {
    var ID = $("#hiddenid").val();
    if (!Submitconfirm()) {
        return;
    };
    if (CustomerID == "") {
        IsSubmit = false;
        UpdateAdjustAndAdjustDetail(ID);
    } else {
        var JsonTable = bianjiaddjsontotable();
        //var AdjustmentType = $("#AdjustmentType").find("option:selected").text();
        var AdjustmentType = $("#AdjustmentType").val();
        var Warehousename = $('select#WarehouseID').find('option:selected').text();
        $.ajax({
            type: "Post",
            url: "/WMS/InventoryManagement/UpdateAndInsertInventory",
            data: {
                "JsonTable": JsonTable,
                "CustomerName": CustomerName,
                "warehouse": Warehousename,
                "Inventorytype": AdjustmentType,
                "aid": ID
            },
            async: "false",
            success: function (data) {
                if (data.indexOf("提交成功") >= 0) {
                    showMsg("提交成功", "4000");
                    var d = data.substring(4, data.length);
                    if (AdjustmentType == "库存调整单") {
                        window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0&Adjustflag=调整打印标记";
                    }
                    else {
                        window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0";
                    }

                }
                else {
                    showMsg("提交失败:" + data, "4000");
                }
            },
            error: function (msg) {
                showMsg("提交失败", "4000");
            }
        });
    }
}

//添加的时候获取主表数据
function FieldSetToJson() {
    var txt = "[";
    var table = document.getElementById("table_body");
    var row = table.getElementsByTagName("tr");
    if (row.length > 1) {
        var r = "{";
        for (var j = 0; j < row.length; j++) {
            var col = row[j].getElementsByTagName("td");

            for (var i = 0; i < col.length; i++) {
                var tds = row[j].getElementsByTagName("td");
                if (i != 5 && tds[i].className.trim() != "TableColumnTitle" && tds[i].innerHTML.trim() != "" && tds[i].childNodes[1].id != "ishold" && tds[i].childNodes[1].id != "") {
                    if (tds[i].childNodes[1].type == 'checkbox') {
                        r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + (tds[i].childNodes[1].checked == true ? 1 : 0) + "\",";
                    }
                    else {
                        r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + tds[i].childNodes[1].value + "\",";
                    }
                }
            }
        }
        r = r.substring(0, r.length - 1)
        r += "}";
        txt += r;
    }
    txt += "]";
    return txt;
}
//添加的时候获取子表数据
//暂存
var box = {
    原库位: 'FromLocation',
    SKU: 'SKU',
    UPC: 'UPC',
    货品等级: 'FromGoodsType',
    货品描述: 'GoodsName',
    可调整库存: 'FromQty',
    可移动库存: 'FromQty',
    可冻结库存: 'FromQty',
    新库位: 'ToLocation',
    调整数量: 'ToQty',
    移动数量: 'ToQty',
    冻结数量: 'ToQty',
    调整等级: 'ToGoodsType',
    调整原因: 'AdjustmentReason',
    移动原因: 'AdjustmentReason',
    冻结原因: 'AdjustmentReason',
    批次号: 'BatchNumber',
    托号: 'BoxNumber',
    单位: 'Unit',
    规格: 'Specifications'
};

//暂存子表
function addjsontotable() {
    var txt = "[";
    var table = document.getElementById("Newtable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");

    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 2; i < col.length - 1; i++) {
            var tds = row[j].getElementsByTagName("td");
            if ($(tds[i]).children("input").length > 0) {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("input")[0].value.trim() + "\",";
            } else if ($(tds[i]).children("select").length > 0) {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("select").find('option:selected').val().trim() + "\",";
            } else {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].innerText.trim().trim() + "\",";
            }
        }
        r = r.substring(0, r.length - 1)
        r += "},";

        txt += r;
    }

    txt = txt.substring(0, txt.length - 1);
    txt += "]";
    return txt;
}

function bianjiaddjsontotable() {
    var txt = "[";
    var table = document.getElementById("Newtable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");

    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 2; i < col.length - 1; i++) {
            var tds = row[j].getElementsByTagName("td");
            if ($(tds[i]).children("input").length > 0) {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("input")[0].value.trim() + "\",";
            } else if ($(tds[i]).children("select").length > 0) {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("select").find('option:selected').val().trim() + "\",";
            } else {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].innerText.trim().trim() + "\",";
            }
        }
        r = r.substring(0, r.length - 1)
        r += "},";
        txt += r;
    }
    txt = txt.substring(0, txt.length - 1);
    txt += "]";
    return txt;
}
function AddLocationNew(LineNumber, locations, AdjustSku, AdjustUPC, AdjustBatchNumber, AdjustBoxNumber, AdjustUnit, AdjustSpecifications, GoodsName, Qty, GoodsType) {
    $('#OldLocation' + LineNumber).val(locations);
    $('#FromGoodsType' + LineNumber).val(GoodsType);
    $('#GoodsName' + LineNumber).val(GoodsName);
    $('#FromQty' + LineNumber).val(Qty);
    $('#ToQty' + LineNumber).val(Qty);
    $('#ToLocation' + LineNumber).val(locations);
    var skuSelect = document.getElementById("sku" + LineNumber);
    var upcSelect = document.getElementById("upc" + LineNumber);
    var batchnumberSelect = document.getElementById("batchnumber" + LineNumber);
    var boxnumberSelect = document.getElementById("boxnumber" + LineNumber);
    var UnitSelect = document.getElementById("Unit" + LineNumber);
    var SpecificationsSelect = document.getElementById("Specifications" + LineNumber);
    var ToGoodsTypeSelect = document.getElementById("ToGoodsType" + LineNumber);
    var opts = new Option(AdjustSku, AdjustSku, true, true);
    var optupc = new Option(AdjustUPC, AdjustUPC, true, true);
    var optba = new Option(AdjustBatchNumber, AdjustBatchNumber, true, true);
    var optbo = new Option(AdjustBoxNumber, AdjustBoxNumber, true, true);
    var optun = new Option(AdjustUnit, AdjustUnit, true, true);
    var optsp = new Option(AdjustSpecifications, AdjustSpecifications, true, true);
    var optgo = new Option(GoodsType, GoodsType, true, true);
    skuSelect.options.add(opts);
    upcSelect.options.add(optupc);
    batchnumberSelect.options.add(optba);
    boxnumberSelect.options.add(optbo);
    UnitSelect.options.add(optun);
    SpecificationsSelect.options.add(optsp);
    ToGoodsTypeSelect.options.add(optgo);
    if ($("#AdjustmentType").val() == "库存调整单") {
        $('#ToLocation' + LineNumber).val(locations);
        //$('input[name=ToLocation').attr("readonly", "readonly");
        document.getElementById('ToGoodsType' + LineNumber).disabled = true;

    } else {
        document.getElementById('ToGoodsType' + LineNumber).disabled = false;
    }
}



function doPrint(how) {
    //打印文档对象
    var myDoc = {
        settings: { topMargin: 50, leftMargin: 50, bottomMargin: 50, rightMargin: 50 },
        documents: document,    // 打印页面(div)们在本文档中
        // 打印时,only_for_print取值为显示
        classesReplacedWhenPrint: new Array('.only_for_print{display:block}'),
        copyrights: '杰创软件拥有版权  www.jatools.com'         // 版权声明必须
    };
    var jatoolsPrinter = getJatoolsPrinter();
    // 调用打印方法
    if (how == '打印预览...')
        jatoolsPrinter.printPreview(myDoc);   // 打印预览

    else if (how == '打印...')
        jatoolsPrinter.print(myDoc, true);   // 打印前弹出打印设置对话框

    else
        jatoolsPrinter.print(myDoc, true);       // 不弹出对话框打印
}