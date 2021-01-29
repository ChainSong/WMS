function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
//完成补货单做修改库存动作
var IDs = '';
var oneself = '';
function CompleteByID(ID, oneselfs) {
    layer.confirm('<font size="4">确认是否完成</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        IDs = ID;
        oneself = oneselfs;
        if (IDs != "" && IDs != 0) {
            $.ajax({
                type: "POST",
                url: "/WMS/InventoryManagement/ComplateReplenishment",
                dataType: "json",
                data: {
                    "ID": IDs,
                },
                async: "false",
                success: function (data) {
                    showMsg("完成成功!", "4000");
                    closePopup();
                    window.location.href = "/WMS/InventoryManagement/AddorEditorViewReplenishment/?ID=" + ID + "&ViewType=0";
                },
                error: function (msg) {
                    showMsg("操作失败", 4000);
                }
            });
        }
    });
}
//生成补货单
function GenerateReplenishment() {
    var CustomerID = $("#CustomerID").find("option:selected").val();
    var CustomerName = $("#CustomerID").find("option:selected").text();

    var WarehouseID = $("#WarehouseID").find("option:selected").val();
    var WarehouseName = $("#WarehouseID").find("option:selected").text();
    var Remark = $("#Remark").val();
    var SKU = $("#ConditionSKU").val();

    if (CustomerID == "") {
        showMsg("请选择客户", "4000");
    }

    if (CustomerID == "") {
        showMsg("请选择仓库", "4000");
    }
    var Qty = $("#Qty").val();
    
    $.ajax({
        url: "/WMS/InventoryManagement/GenerateReplenishment",
        type: "POST",
        dataType: "json",
        data: { CustomerID: CustomerID, CustomerName: CustomerName, WarehouseID: WarehouseID, WarehouseName: WarehouseName, Remark: Remark, SKU: SKU,Qty:Qty },
        success: function (data) {
            if (data["IsSuccess"] == true) {
                window.location.href = "/WMS/InventoryManagement/AddorEditorViewReplenishment/?ID=" + data["ID"] + "&ViewType=0";
            }
        }
    });
}
$(document).ready(function () {
    //生成补货单
    //if ($('#HiddenViewType').val() == 1) {
    //    $("#btnGenerate").live("click", function () {
            
    //    });
    //}
    
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
    $(".addNew").live('click', function () {

        addNew($("#AdjustmentType").val());
    });

    if ($('#HiddenViewType').val() == 2) {

    }
    else if ($('#HiddenViewType').val() == 0) {
        $("#NewDiv").removeAttr("style");
        $("#Newtable").removeAttr("style");
    }



    if ($("#Adjusttype").val() != null && $("#Adjusttype").val() != "") {
        $("#AdjustmentType").val($("#Adjusttype").val());
    }
    if ($("#HiddenViewType").val() != "2" && $("#HiddenViewType").val() != "0") {
        //$("#PrintLabel")[0].style.display = "none";
    }
    else {
        //$("#PrintLabel")[0].style.display = "";
    }
    //if ($("#HiddenViewType").val() == "0" && $("#Adjustflag").val() == "调整打印标记") {
    //    openPopup('NewPrintpopp', true, 350, 300, null, 'PrintDiv');
    //    $("#popupLayer_NewPrintpopp")[0].style.top = "200px";
    //}
    //判断是否新增选择下拉框跳转
    var AddAdjustType = GetQueryString("AddAdjustType");
    if (AddAdjustType != null) {
        $("#AdjustmentType").val(AddAdjustType);
        changestype();
    }

    $('select[id=CustomerID]').live('change', function () {
        window.location.href = "/WMS/InventoryManagement/AddorEditorViewReplenishment/?ID=0&customerID=" + $(this).val() + "&ViewType=1";
    });

    if ($('#HiddenViewType').val() == 1) {
        changestype();
        $('#ReplenishmentNumber').val("系统自动生成");
    }
    if ($('#HiddenViewType').val() == 2 || $('#HiddenViewType').val() == 0) {
        var table = document.getElementById("Newtable");
        var row = table.getElementsByTagName("tr");
        for (var j = 1; j < row.length; j++) {
            var col = row[j].getElementsByTagName("td");
            for (var i = 1; i < col.length; i++) {
                //col[1].innerHTML = "<b>" + ReturnLineNumber(j) + "</b>";
                //fuzhi(ReturnLineNumber(j));
            }
        }
        changestype();
    }
    $('#backButton').live('click', function () {
        //history.back();
        window.location.href = "/WMS/InventoryManagement/Replenishment"
    })
    //替换指定传入参数的值,paramName为参数,replaceWith为新值  
    function replaceParamVal(paramName, replaceWith) {
        var oUrl = this.location.href.toString();
        var re = eval('/(' + paramName + '=)([^&]*)/gi');
        var nUrl = oUrl.replace(re, paramName + '=' + replaceWith);
        //window.location = nUrl;
        return nUrl;
    }
    function addUrlPara(name, value) {
        var currentUrl = window.location.href.split('#')[0];
        if (/\?/g.test(currentUrl)) {
            if (/name=[-\w]{4,25}/g.test(currentUrl)) {
                currentUrl = currentUrl.replace(/name=[-\w]{4,25}/g, name + "=" + value);
            } else {
                currentUrl += "&" + name + "=" + value;
            }
        } else {
            currentUrl += "?" + name + "=" + value;
        }
        if (window.location.href.split('#')[1]) {
            return currentUrl + '#' + window.location.href.split('#')[1];
            //window.location.href = currentUrl + '#' + window.location.href.split('#')[1];
        } else {
            //window.location.href = currentUrl;
            return currentUrl;
        }
    }
    $('select[id=AdjustmentType]').live('change', function () {
        var adjustment_type = $("#AdjustmentType").find("option:selected").text();
        var nurl = replaceParamVal("AdjustType", adjustment_type);
        if (GetQueryString("AdjustType") == null) {
            nurl = addUrlPara("AddAdjustType", adjustment_type);
        }
        if (GetQueryString("AddAdjustType") != null) {
            nurl = replaceParamVal("AddAdjustType", adjustment_type);
        }
        window.location.href = nurl;
        //window.location.reload();
        //changestype();
    });
    if ($('#AdjustmentNumber').val() == "系统自动生成") {
        addNew($("#AdjustmentType").val());
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
    //		LODOP.ADD_PRINT_TEXT(15,137,157,25,"交通银行（      ）");
    //		LODOP.SET_PRINT_STYLEA(2,"FontName","隶书");
    //		LODOP.SET_PRINT_STYLEA(2,"FontSize",11);
    //		LODOP.SET_PRINT_STYLEA(2,"FontColor",0);
    //		LODOP.ADD_PRINT_TEXT(41,213,100,20,"2008年11月9日");
    //		LODOP.ADD_PRINT_TEXT(17,281,100,20,"个人业务受理书");
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

    //LODOP.SET_PRINT_STYLEA(75, 37, 200, 200, "QRCodeVersion", 7);

    //LODOP.ADD_PRINT_SETUP_BKIMG(75, 37, 200, 200, "<img border='0' src='http://qr.liantu.com/api.php?text=156156'  width='250' height='250'/> ");
    //LODOP.ADD_PRINT_SETUP_BKIMG(75, 37, 200, 200, "<img border='0' src='http://qr.liantu.com/api.php?text=156156' width='250' height='250'/> ");
    //LODOP.ADD_PRINT_IMAGE(75, 37, 200, 200,"<img border='0' src='https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQEX8ToAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL0MzV1Y5UkRseHhXX25PN19CRm1FAAIEq51GVwMEAAAAAA==' width='250' height='250'/> ");

};

function changestype() {
    if ($("#AdjustmentType").val() == "库存冻结单" || $("#hide").val() == "库存冻结单") {
        $("#lblcounts")[0].innerHTML = "冻结数量";
        $("#lblreson")[0].innerHTML = "冻结原因";
        $("#lbloldcount")[0].innerHTML = "可冻结库存";
        if ($('#HiddenViewType').val() == 1) {
            document.getElementById("lblhold").style.display = "block";
            document.getElementById("ishold").style.display = "block";
            Cleartext();
            $('input[name=ToLocation').removeAttr("readonly");
            $('input[name=ToGoodsType').removeAttr("readonly");
        }
    }
    if ($("#AdjustmentType").val() == "库存调整单" || $("#hide").val() == "库存调整单") {
        $("#lblcounts")[0].innerHTML = "调整后数量";
        $("#lblreson")[0].innerHTML = "调整原因";
        $("#lbloldcount")[0].innerHTML = "可调整库存";
        if ($('#HiddenViewType').val() == 1) {
            document.getElementById("lblhold").style.display = "none";
            document.getElementById("ishold").style.display = "none";
            $('input[name=ToLocationChild]').attr("disabled", "disabled");
            $('input[name=ToQty]').attr("disabled", "disabled");
            $('select[name=ToGoodsType]').removeAttr("disabled");
            Cleartext();
        }
    }
    if ($("#AdjustmentType").val() == "库存移动单" || $("#hide").val() == "库存移动单") {
        $("#lblcounts")[0].innerHTML = "移动数量";
        $("#lblreson")[0].innerHTML = "移动原因";
        $("#lbloldcount")[0].innerHTML = "可移动库存";
        if ($('#HiddenViewType').val() == 1) {
            document.getElementById("lblhold").style.display = "none";
            document.getElementById("ishold").style.display = "none";
            $('input[name=ToLocationChild]').removeAttr("disabled");
            $('input[name=ToQty]').removeAttr("disabled");
            $('select[name=ToGoodsType]').attr("disabled", "disabled");
            Cleartext();
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
            if (i >= 1 && i != 2 && i != 3 && i != 4 && i != 5 && i != 6 && i != 12) {
                tds[i].childNodes[0].value = "";
            }
            if (i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 12) {
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

//function fuzhi(LineNumber) {

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

                //var b = " <select class='form-control' style='width:160px' id='sku" + LineNumber + "'>"
                var skuSelect = $(self).parent().parent().children(".SKU").children("select")[0];
                //document.getElementById("sku" + LineNumber);
                var upcSelect = $(self).parent().parent().children(".UPC").children("select")[0];
                var batchnumberSelect = $(self).parent().parent().children(".BatchNumber").children("select")[0];
                //document.getElementById("batchnumber" + LineNumber);
                var boxnumberSelect = $(self).parent().parent().children(".BoxNumber").children("select")[0];
                //document.getElementById("boxnumber" + LineNumber);
                var UnitSelect = $(self).parent().parent().children(".Unit").children("select")[0];
                //.getElementById("Unit" + LineNumber);
                var SpecificationsSelect = $(self).parent().parent().children(".Specifications").children("select")[0];
                //document.getElementById("Specifications" + LineNumber);
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
    //var location = $($(self).parent().parent()).children(".FromLocation").children("input")[0].val();
    //var sku = $($(self).parent().parent()).children(".SKU").children("select").find('option:selected').val().split("|")[0];
    //var goodstype = $($(self).parent().parent()).children(".SKU").children("select").find('option:selected').val().split("|")[1];
    ////var upc = $($(self).parent().parent()).children(".UPC").children("input");
    ////var batchnumber = $($(self).parent().parent()).children(".BatchNumber").children("input");
    ////var boxnumber = $($(self).parent().parent()).children(".BoxNumber").children("input");
    //var warehouse = $("#WarehouseID option:selected")[0].innerText;
    ////var Unit = $($(self).parent().parent()).children(".Unit").children("input");
    ////var Specifications = $($(self).parent().parent()).children(".Specifications").children("input");
    //GetLinkageMenu(location, sku, "", goodstype, "", "", warehouse, "", "", self, 1)
})

$("select[name = 'upc']").live("change", function () {
    var self = this;
    GetLinkageMenu(self, 2)
})
$("select[name = 'batchnumber']").live("change", function () {
    var self = this;
    GetLinkageMenu(self, 3)
})
//$("select[name = 'batchnumber']").live("change", function () {
//    var self = this;
//    GetLinkageMenu(self, 4)
//})
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
            //$("#OldLocation" + LineNumber).val(),
            sku: $(sku).find('option:selected').val().split("|")[0],
            upc: $(upc).find('option:selected').val(),
            //$($(self).parent().parent()).children(".SKU").children("select")[0].find('option:selected').val(),
            //$('#sku' + LineNumber + ' option:selected').val(),
            goodstype: goodstype,// $(self).find('option:selected').val().split('|')[1],
            //$($(self).parent().parent()).children(".GoodsTyoe").children("select")[0].find('option:selected').val(),
            //$('#sku' + LineNumber + ' option:selected').val().substring($('#sku' + LineNumber + ' option:selected').val().indexOf('-') + 1),
            batchnumber: $(batchnumber).find('option:selected').val(),//$($(self).parent().parent()).children(".BatchNumber").children("select").find('option:selected').val(),
            boxnumber: $(boxnumber).find('option:selected').val(),// $($(self).parent().parent()).children(".BoxNumber").children("select").find('option:selected').val(),
            warehouse: warehouse,// $("#WarehouseID option:selected")[0].innerText,
            Unit: $(Unit).find('option:selected').val(),
            Specifications: $(Specifications).find('option:selected').val()
            //$("#batchnumber" + LineNumber).val() == null ? "" : $("#batchnumber" + LineNumber).val(),
            //boxnumber: $($(self).parent().parent()).children(".GoodsTyoe").children("select")[0].find('option:selected').val();
            //$("#boxnumber" + LineNumber).val() == null ? "" : $("#boxnumber" + LineNumber).val(), warehouse: $("#WarehouseID option:selected")[0].innerText
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

                //batchnumberSelect.length = 0;
                //var optba = new Option("", "", true, true);
                //batchnumberSelect.options.add(optba);
                //boxnumberSelect.length = 0;

                for (var i = 0; i < data.length; i++) {
                    if (data[i].BatchNumber != "" && data[i].BatchNumber != null && num < 3) {


                        var flag = 0;
                        for (var m = 0; m < batchnumberSelect.length; m++) {
                            if (batchnumberSelect[m].innerText == data[i].BatchNumber)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            //if (batchnumberSelect.length == 1) {
                            //    var opt1 = new Option(data[i].BatchNumber, data[i].BatchNumber, true, true);
                            //}
                            //else {
                            //    batchnumberSelect[1].defaultSelected = false;
                            //    var opt1 = new Option(data[i].BatchNumber, data[i].BatchNumber);
                            //}
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
                            //if (boxnumberSelect.length == 1) {
                            //    var opt = new Option(data[i].BoxNumber, data[i].BoxNumber, true, true);
                            //}
                            //else {
                            //    boxnumberSelect[1].defaultSelected = false;
                            //    var opt = new Option(data[i].BoxNumber, data[i].BoxNumber);
                            //}
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
                            //if (UnitSelect.length == 1) {
                            //    var opt = new Option(data[i].Unit, data[i].Unit, true, true);
                            //}
                            //else {
                            //    UnitSelect[1].defaultSelected = false;
                            //    var opt = new Option(data[i].Unit, data[i].Unit);
                            //}
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
                            //if (SpecificationsSelect.length == 1) {
                            //    var opt = new Option(data[i].Specifications, data[i].Specifications, true, true);
                            //}
                            //else {
                            //    SpecificationsSelect[1].defaultSelected = false;
                            //    var opt = new Option(data[i].Specifications, data[i].Specifications);
                            //}
                            var opt = new Option(data[i].Specifications, data[i].Specifications);
                            SpecificationsSelect.options.add(opt);
                        }
                    }
                }

                if ($("#AdjustmentType").val() == "库存调整单") {
                    //var SpecificationsSelect = 
                    $(self).parent().parent().children(".ToLocation").children("input")[0].value = $(self).parent().parent().children(".FromLocation").children("input")[0].value;
                    //$("#OldLocation" + LineNumber).val();
                    //$('#ToLocation' + LineNumber).val($("#OldLocation" + LineNumber).val());
                    $(self).parent().parent().children(".ToLocation").children("input").attr("readonly", "readonly");
                    //$('input[name=ToLocation' + LineNumber + ']').attr("readonly", "readonly")

                    $(self).parent().parent().children(".ToGoodsType").children("input").disabled = true;
                    //document.getElementById('ToGoodsType' + LineNumber).disabled = true;
                } else {
                    $(self).parent().parent().children(".ToGoodsType").children("input").disabled = false;
                    //document.getElementById('ToGoodsType' + LineNumber).disabled = false;
                }
                $(self).parent().parent().children(".FromGoodsType").children("input")[0].value = data[0].GoodsType
                //$('#FromGoodsType' + LineNumber).val(data[0].GoodsType);
                $(self).parent().parent().children(".GoodsName").children("input")[0].value = data[0].GoodsName
                //$('#GoodsName' + LineNumber).val(data[0].GoodsName);


                var objSelect = $(self).parent().parent().children(".ToGoodsType").children("select")[0];
                //document.getElementById('ToGoodsType' + LineNumber);
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
                            //$(self).parent().parent().children(".SKU").children("select").find("option:selected").val().split("|")[0]
                            //data[i].SKU == $('#sku' + LineNumber + ' option:selected').val().substring(0, $('#sku' + LineNumber + ' option:selected').val().indexOf('-'))
                            && (data[i].BatchNumber == $(batchnumber).find("option:selected")[0].value || $(batchnumber).find("option:selected")[0].value == "")
                            //$(self).parent().parent().children(".BatchNumber").children("select").find("option:selected")[0].value
                            //$("#batchnumber" + LineNumber).val() && 
                           && (data[i].BoxNumber == $(boxnumber).find("option:selected")[0].value || $(boxnumber).find("option:selected")[0].value == "")
                            //$(self).parent().parent().children(".BoxNumber").children("select").find("option:selected")[0].value
                            && (data[i].Unit == $(Unit).find("option:selected")[0].value || $(Unit).find("option:selected")[0].value == "")) {
                            //$(self).parent().parent().children(".Unit").children("select").find("option:selected")[0].value) {
                            //$("#Unit" + LineNumber).val() && data[i].Specifications == $("#Specifications" + LineNumber).val()) {
                            n += parseFloat(data[i].qty);
                        }
                    }
                }
                $(self).parent().parent().children(".FromQty").children("input")[0].value = n;
                $(self).parent().parent().children(".ToQty").children("input")[0].value = n;
                //$('#FromQty' + LineNumber).val(n);
                //$('#ToQty' + LineNumber).val(n);

            }
            else {
                if ($("#AdjustmentType").val() == "库存调整单") {
                    $(self).parent().parent().children(".ToLocation").children("input")[0].value = "";
                    //$('#ToLocation' + LineNumber).val("");
                    $(self).parent().parent().children(".ToLocation").children("input").attr("readonly", "readonly");
                    //$('input[name=ToLocation' + LineNumber + ']').attr("readonly", "readonly");
                    $(self).parent().parent().children(".ToGoodsType").children("input").disabled = true;
                    //document.getElementById('ToGoodsType' + LineNumber).disabled = true;
                } else {
                    $(self).parent().parent().children(".ToGoodsType").children("input").disabled = false;
                    //document.getElementById('ToGoodsType' + LineNumber).disabled = false;
                }
                $(self).parent().parent().children(".FromGoodsType").children("input")[0].value = "";
                //$('#FromGoodsType' + LineNumber).val("");
                $(self).parent().parent().children(".GoodsName").children("input")[0].value = "";
                //$('#GoodsName' + LineNumber).val("");
                $(self).parent().parent().children(".FromQty").children("input")[0].value = "";
                //$('#FromQty' + LineNumber).val("");

                var objSelect = $(self).parent().parent().children(".ToGoodsType").children("input")[0];
                //document.getElementById('ToGoodsType' + LineNumber);
                var objSelect = $(self).parent().parent().children(".ToQty").children("input")[0].value = "";
                //$('#ToQty' + LineNumber).val("");
            }
        },
        error: function (data, status, e) {
            var a = 1;
        }

    })
}
$(".ToLocationChild").live("keydown", function () {
    var LineNumber = $(this).parent().parent().children(".LineNumber")[0].innerText;
    //$(this).parent().prev().prev().prev().prev().prev().prev().prev().prev().prev().prev()[0].innerText;
    $('#ToLocation' + LineNumber).autocomplete({

        source: function (request, response) {
            if (request.term.length > 5) {
                $.ajax({
                    url: "/WMS/InventoryManagement/GetLocationList",
                    type: "POST",
                    dataType: "json",
                    data: { location: request.term, warehouseid: $("#WarehouseID").val(), areaid: 0 },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Text, value: item.Text, data: item }
                        }));
                    }
                })
            };
        },
        select: function (event, ui) {
            $(this).val(ui.item.data.Text);
            //$('#ToLocation' + LineNumber).val(ui.item.data.Text);
        }
    })
});

var row_count = 1;
//得到明细中的总条数
var asndetailcount = 0;
var skulist = new Array();
function addNew(AdjustmentType) {
    var table1 = $('#Newtable');
    var rownumber = $('#Newtable tr').length;
    var firstTr = table1.find('tbody>tr:first');
    var LineNumber = ReturnLineNumber(rownumber);
    var row = $("<tr id=Row" + rownumber + "</tr>");
    var td0 = $(" <td style='position: relative'></td>");


    var td1 = $("<td class='LineNumber'></td>");
    var td2 = $("<td class='FromLocation'></td>");
    var td3 = $("<td class='SKU'></td>");
    var td4 = $("<td class='UPC'></td>");
    var td5 = $("<td class='BatchNumber'></td>");

    var td6 = $("<td class='BoxNumber'></td>");
    var td7 = $("<td class='Unit'></td>");
    var td8 = $("<td class='Specifications'></td>");
    var td9 = $("<td class='FromGoodsType'></td>");
    var td10 = $("<td class='GoodsName'></td>");
    var td11 = $("<td class='FromQty'></td>");
    var td12 = $("<td class='ToLocation'></td>");
    var td13 = $("<td class='ToQty'></td>");
    var td14 = $("<td class='ToGoodsType'></td>");
    var td15 = $("<td class='AdjustmentReason'></td>");
    //$("<td style='position: absolute;left: 88%;width: 100px;height:40px;background-color: #fff;'></td>");

    td0.append($("<div class='Adiv' style='position: absolute; display: none; width: 500px; left: 50px; height: 31px;'>\
                                    <label id='labelRemove' style='cursor: pointer;' class='btn btn-primary btn-xs'>删除</label>\
                                    <label style='cursor: pointer;' class='btn btn-primary btn-xs   addNew'>新增</label>\
                                </div>\
                                <label style='cursor: pointer;' class='btn btn-primary btn-xs Ooperation'>操作</label> "));
    td1.append($("<label>" + LineNumber + "</label>"));
    td2.append($("<input type='text' id='OldLocation" + LineNumber + "' name='OldLocation'  style='width:130px'  class='form-control OldLocation'  value=''/>"));
    td3.append($("<select id='sku" + LineNumber + "' name='sku'  style='width:130px'  class='form-control skuChild sku" + LineNumber + "' />"));
    td4.append($("<select id='upc" + LineNumber + "' name='upc'  style='width:130px'  class='form-control upcChild upc" + LineNumber + "' />"));
    td5.append($("<select id='batchnumber" + LineNumber + "' name='batchnumber'  style='width:130px'  class='form-control batchnumber" + LineNumber + "' />"));
    td6.append($("<select id='boxnumber" + LineNumber + "' name='boxnumber'  style='width:130px'  class='form-control boxnumber" + LineNumber + "' />"));
    td7.append($("<select id='Unit" + LineNumber + "' name='Unit'  style='width:130px'  class='form-control Unit" + LineNumber + "' />"));
    td8.append($("<select id='Specifications" + LineNumber + "' name='Specifications'  style='width:130px'  class='form-control Specifications" + LineNumber + "' />"));
    td9.append($("<input type='text' name='FromGoodsType' id='FromGoodsType" + LineNumber + "' Readonly='true' style='width:130px' class='form-control FromGoodsType" + LineNumber + "'   value='' />"));
    td10.append($("<input type='text'   name='GoodsName' id='GoodsName" + LineNumber + "'  Readonly='true' style='width:130px' class='form-control GoodsName" + LineNumber + "'   value='' />"));
    td11.append($("<input type='text'   name='FromQty' id='FromQty" + LineNumber + "'  Readonly='true' style='width:130px' class='form-control FromQty" + LineNumber + "'   value='' />"));
    if (AdjustmentType == "库存调整单") {
        td12.append($("<input type='text'   name='ToLocationChild' id='ToLocation" + LineNumber + "' style='width:130px' disabled='disabled'  class='form-control ToLocationChild'   value='' />"));
        td13.append($("<input type='text'   name='ToQty' id='ToQty" + LineNumber + "' style='width:130px' disabled='disabled'  class='form-control ToQty" + LineNumber + "'   value='' onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)' />"));
        td14.append($("<select id='ToGoodsType" + LineNumber + "'  name='ToGoodsType' style='width:130px'   class='form-control ToGoodsType" + LineNumber + "'   value='' />"));
    }
    else {
        td12.append($("<input type='text'   name='ToLocationChild' id='ToLocation" + LineNumber + "' style='width:130px'  class='form-control ToLocationChild'   value='' />"));
        td13.append($("<input type='text'   name='ToQty' id='ToQty" + LineNumber + "' style='width:130px'  class='form-control ToQty" + LineNumber + "'   value='' onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)' />"));
        td14.append($("<select id='ToGoodsType" + LineNumber + "'  name='ToGoodsType' style='width:130px' disabled='disabled'  class='form-control ToGoodsType" + LineNumber + "'   value='' />"));

    }
    td15.append($("<input type='text' name='AdjustmentReason' style='width:130px'  class='form-control AdjustmentReason" + LineNumber + "' value=''   />"));
    //($("<Lable id='labelRemove' style='cursor:pointer; color:white' class='label label-info' ><b>删除</b></Lable>")).append('&nbsp;&nbsp;');
    //td15.append($("<Lable style='cursor:pointer; color:white;' class='label label-info' onclick='addNew()'><b>新增</b></Lable>"));
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
    var ProductLevel = $("#ProductLevel");
    var objSelect = td14[0].childNodes[0];
    for (var i = 0; i < ProductLevel[0].length; i++) {
        if (ProductLevel[0][i].innerText == "") {
            var opt = new Option(ProductLevel[0][i].innerText, ProductLevel[0][i].innerText, true, true);
        }
        else {
            var opt = new Option(ProductLevel[0][i].innerText, ProductLevel[0][i].innerText);
        }
        objSelect.options.add(opt);
    }

    $('#OldLocation' + LineNumber).focus();
    //fuzhi(LineNumber);
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
    //var t = document.getElementById("Newtable");
    //var froms = $(this).parent().parent().prevAll().length;
    //var length = $(this).parent().parent().nextAll().length;
    //var LineNumber = 0;
    //var row_number = 0;
    $(this).parent().parent().parent().remove();
    //for (i = 0; i < length; i++) {
    //    var num = ReturnLineNumber(parseInt(froms) + i + 1);

    //    //row_number = parseInt($(this).parent().parent().nextAll()[i].rowIndex) - 1;
    //    //LineNumber = ReturnLineNumber(row_number);
    //    t.rows[parseInt(froms) + i + 1].cells[1].children.OldLocation.id = "OldLocation" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[2].children.sku.id = "sku" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[3].children.batchnumber.id = "batchnumber" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[4].children.boxnumber.id = "boxnumber" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[5].children.Unit.id = "Unit" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[6].children.Specifications.id = "Specifications" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[7].children.FromGoodsType.id = "FromGoodsType" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[8].children.GoodsName.id = "GoodsName" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[9].children.FromQty.id = "FromQty" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[10].children.ToLocation.id = "ToLocation" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[11].children.ToQty.id = "ToQty" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[12].children.ToGoodsType.id = "ToGoodsType" + num;
    //    t.rows[parseInt(froms) + i + 1].cells[0].innerHTML = "<label>" + num + "</label>";
    //    //$("#Newtable tr:eq(" + parseInt($(this).parent().parent().nextAll()[i].rowIndex) + ") td:nth-child(1)").html("<b>" + LineNumber + "</b>");


    //}

    //asndetailcount--;
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

    var AdjustmentType = $("#AdjustmentType").find("option:selected").text();
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
    var AdjustmentReason = $('#AdjustmentReason').val();

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
            //if ($('#Newtable')[0].rows[i].cells[4].children.boxnumber.value == '' || $('#Newtable')[0].rows[i].cells[4].children.boxnumber.value == '请选择') {
            //    showMsg("请选择托号!", 4000);
            //    return;
            //}
        }
    }
    //子表数据
    var JsonTable = addjsontotable();
    //if (message!="") {
    //    showMsg(message, "4000");
    //    message = "";
    //    skulist.length = 0;
    //    CustomerID = "";
    //    return;
    //}
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
            "AdjustmentReason": AdjustmentReason,
            "JsonField": JsonField,
            "WarehouseID": 0,
            "Warehousename": Warehousename,
            "ADID": $('#hiddenid').val()
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


                                if (AdjustmentType == "库存调整单") {
                                    window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0&Adjustflag=调整打印标记";
                                }
                                else {
                                    window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0";
                                }


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

            showMsg("暂存失败 "4000");
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
    var AdjustmentType = $("#AdjustmentType").find("option:selected").text();
    var WarehouseID = $('#WarehouseID').val();
    var Warehousename = $('select#WarehouseID').find('option:selected').text();
    var adjustmenttime = $('#AdjustmentAndAdjustmentDetails_adjustment_AdjustmentTime').val();
    var AdjustmentReason = $('#AdjustmentReason').val();
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
            //if ($('#Newtable')[0].rows[i].cells[4].children[0].value == '' || $('#Newtable')[0].rows[i].cells[4].children[0].value == '请选择') {
            //    showMsg("请选择托号!", 4000);
            //    return;
            //}
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
            "AdjustmentReason": AdjustmentReason,
            "JsonField": JsonField,
            "WarehouseID": WarehouseID,
            "Warehousename": Warehousename,
            "ID": ID,
            "adjustmentnumber": adjustmentnumber
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
                                if (AdjustmentType == "库存调整单") {
                                    window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0&Adjustflag=调整打印标记";
                                }
                                else {
                                    window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0";
                                }
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
    location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=0" + "&customerID=" + customerID + "&ViewType=1"
}
//新增页面提交
function submitClick() {
    if (!Submitconfirm()) {
        return;
    };
    //if (CustomerID == "") {
    //    IsSubmit = false;

    AddAdjustAndAdjustDetail(false);
    //} else {
    //    var lengths = $('#Newtable')[0].rows.length - 1;
    //    if (lengths > 0) {
    //        for (var i = 1; i <= lengths; i++) {
    //            if ($('#Newtable')[0].rows[i].cells[1].children.OldLocation.value == '') {
    //                showMsg("请选择原库位!", 4000);
    //                return;
    //            }
    //            if ($('#Newtable')[0].rows[i].cells[10].children.ToLocation.value == '') {
    //                showMsg("请选择新库位!", 4000);
    //                return;
    //            }

    //        }
    //    }
    //    var JsonTable = addjsontotable();
    //    var AdjustmentType = $("#AdjustmentType").find("option:selected").text();
    //    var Warehousename = $('select#WarehouseID').find('option:selected').text();

    //    $.ajax({
    //        type: "Post",
    //        url: "/WMS/InventoryManagement/UpdateAndInsertInventory",
    //        data: {
    //            "JsonTable": JsonTable,
    //            "CustomerName": CustomerName,
    //            "warehouse": Warehousename,
    //            "Inventorytype": AdjustmentType,
    //            aid: AID==""?0:AID
    //        },
    //        async: "false",
    //        success: function (data) {
    //            if (data == 0 || data == "") {
    //                showMsg("提交失败", "4000");
    //                return;
    //            }
    //            if (data.indexOf("提交成功") >= 0) {
    //                showMsg("提交成功", "4000");
    //                message = "";
    //                skulist.length = 0;
    //                var d = data.substring(4, data.length);
    //                if (AdjustmentType == "库存调整单") {
    //                    window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0&Adjustflag=调整打印标记";
    //                }
    //                else {
    //                    window.location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=" + d + "&ViewType=0";
    //                }


    //            } else {
    //                showMsg(data, "4000");
    //                message = "";
    //                skulist.length = 0;
    //                return;
    //            }
    //        },
    //        error: function (msg) {
    //            showMsg("提交失败", "4000");
    //        }
    //    });
    //}
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
            //if ($('#Newtable')[0].rows[i].cells[4].children[0].value == '' || $('#Newtable')[0].rows[i].cells[4].children[0].value == '请选择') {
            //    showMsg("请选择托号!", 4000);
            //    return false;
            //}
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
        var AdjustmentType = $("#AdjustmentType").find("option:selected").text();
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
                //if (data == 0 || data == "") {
                //    showMsg("提交失败", "4000");
                //    return;
                //} else
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
    调整后数量: 'ToQty',
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
                //if (col[i].innerHTML.trim() == '新库位') {
                //    r += "\"Area\"\:\"" + $(tds[i]).children("input")[0].value.split("|")[0] + "\",";
                //    r += "\"Location\"\:\"" + $(tds[i]).children("input")[0].value.split("|")[1] + "\",";
                //} else {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("input")[0].value.trim() + "\",";
                //}
            } else if ($(tds[i]).children("select").length > 0) {
                //if (col[i].innerHTML.trim() == '原库位') {
                //    r += "\"Area\"\:\"" + $(tds[i]).children("select").find('option:selected').val().split("|")[0];
                //    r += "\"Location\"\:\"" + $(tds[i]).children("select").find('option:selected').val().split("|")[1];
                //} else {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("select").find('option:selected').val().trim() + "\",";
                //r += "\"""\"\:\"" + $(tds[i]).children("select").find('option:selected').val().split("|")[1];
                //}
            } else {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].innerText.trim().trim() + "\",";
            }

            //if (i >= 1) {
            //    r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].childNodes[0].value.trim() + "\",";
            //}
            // if (i == 2 || i == 3 || i ==4 || i == 10) {
            //    r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].childNodes[0].value.trim() + "\",";
            //if (i == 2) {
            //    if (contains(skulist, tds[2].childNodes[0].value.trim())) {
            //        message ="SKU:"+tds[2].childNodes[0].value.trim()+ "已存在";
            //    }
            //    skulist.push(tds[2].childNodes[0].value.trim());
            //}
            //}
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
                //if (col[i].innerHTML.trim() == '新库位') {
                //    r += "\"Area\"\:\"" + $(tds[i]).children("input")[0].value.split("|")[0] + "\",";
                //    r += "\"Location\"\:\"" + $(tds[i]).children("input")[0].value.split("|")[1] + "\",";
                //} else {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("input")[0].value.trim() + "\",";
                //}
            } else if ($(tds[i]).children("select").length > 0) {
                //if (col[i].innerHTML.trim() == '原库位') {
                //    r += "\"Area\"\:\"" + $(tds[i]).children("select").find('option:selected').val().split("|")[0];
                //    r += "\"Location\"\:\"" + $(tds[i]).children("select").find('option:selected').val().split("|")[1];
                //} else {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("select").find('option:selected').val().trim() + "\",";
                //r += "\"""\"\:\"" + $(tds[i]).children("select").find('option:selected').val().split("|")[1];
                //}
            } else {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].innerText.trim().trim() + "\",";
            }
            //if (i >= 1 && i != 2 && i != 3 && i != 4 && i != 10) {
            //    r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].children[0].value + "\",";
            //} if (i == 2 || i == 3 || i == 4 || i == 10) {
            //    if (i >= 2) {
            //    r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].children[0].value.trim() + "\",";
            //}
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
    var ProductLevel = $("#ProductLevel");
    for (var i = 0; i < ProductLevel[0].length; i++) {
        if (ProductLevel[0][i].innerText == "") {
            var opt = new Option(ProductLevel[0][i].innerText, ProductLevel[0][i].innerText, true, true);
        }
        else {
            var opt = new Option(ProductLevel[0][i].innerText, ProductLevel[0][i].innerText);
        }
        ToGoodsTypeSelect.options.add(opt);
    }
    //var optgo1 = new Option("A品", "A品", true, true);
    //var optgo2 = new Option("B品", "B品");
    //var optgo3 = new Option("C品", "C品");
    //var optgo4 = new Option("D品", "D品");


    skuSelect.options.add(opts);
    upcSelect.options.add(optupc);
    batchnumberSelect.options.add(optba);
    boxnumberSelect.options.add(optbo);
    UnitSelect.options.add(optun);
    SpecificationsSelect.options.add(optsp);
    //ToGoodsTypeSelect.options.add(optgo1);
    //ToGoodsTypeSelect.options.add(optgo2);
    //ToGoodsTypeSelect.options.add(optgo3);
    //ToGoodsTypeSelect.options.add(optgo4);
    if ($("#AdjustmentType").val() == "库存移动单") {
        $('#ToLocation' + LineNumber).val(locations);
        //$('input[name=ToLocation').attr("readonly", "readonly");
        document.getElementById('ToGoodsType' + LineNumber).disabled = true;

    } else {
        document.getElementById('ToGoodsType' + LineNumber).disabled = false;
        document.getElementById('ToQty' + LineNumber).disabled = true;
        document.getElementById('ToLocation' + LineNumber).disabled = true;

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