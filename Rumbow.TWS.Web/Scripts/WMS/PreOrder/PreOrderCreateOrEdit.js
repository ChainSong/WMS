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
    if ($('#ViewType').val() == 2) {
        var tr = document.getElementById('Newtable').getElementsByTagName('tr');
        var lens = tr.length;
        for (var i = 1; i < lens; i++) {
            var sku = $(tr[i]).children(".SKU").children("input").val();
            var upc = $(tr[i]).children(".UPC").children("input").val();
            var batchsNum = $(tr[i]).children(".BatchNumber").children("select")[0];
            var boxnumberNum = $(tr[i]).children(".BoxNumber").children("select")[0];
            var UnitNum = $(tr[i]).children(".Unit").children("select")[0];
            var SpecificationsNum = $(tr[i]).children(".Specifications").children("select")[0];
            var batchSelected = $(tr[i]).children(".BatchNumber")[0].innerText.trim();
            var boxnumberSelected = $(tr[i]).children(".BoxNumber")[0].innerText.trim();
            var UnitSelected = $(tr[i]).children(".Unit")[0].innerText.trim();
            var SpecificationsSelected = $(tr[i]).children(".Specifications")[0].innerText.trim();
            var Goods = $($(tr[i]).children(".GoodsType")[0]).find("option:selected")[0].value;
            var InventoryQty = $(tr[i]).children(".InventoryQty");
            var opt1 = new Option("请选择", "请选择");
            var opt2 = new Option("请选择", "请选择");
            var opt3 = new Option("请选择", "请选择");
            var opt4 = new Option("请选择", "请选择");
            batchsNum.options.add(opt1);
            boxnumberNum.options.add(opt2);
            UnitNum.options.add(opt3);
            SpecificationsNum.options.add(opt4);
            $.ajax({
                url: "/WMS/PreOrder/GetBatchlist",
                type: "POST",
                dataType: "json",
                async: false,
                data: { sku: sku, CustomerID: $('#CustomerName').val(), Warehouse: $('#Warehouse').val() },
                success: function (data) {
                    var lens = data.length;
                    if (lens > 0) {
                        for (var i = 0; i < lens; i++) {
                            if (data[i].BatchNumber != batchSelected) {
                                if (data[i].BatchNumber != '') {
                                    var flag = 0;
                                    for (var m = 0; m < batchsNum.length; m++) {
                                        if (batchsNum[m].innerText == data[i].BatchNumber)
                                        { flag = 1; }
                                    }
                                    if (flag == 0) {
                                        var opt = new Option(data[i].BatchNumber, data[i].BatchNumber);
                                        batchsNum.options.add(opt);
                                    }
                                }
                            }
                            if (data[i].BoxNumber != boxnumberSelected) {
                                if (data[i].BoxNumber != '') {
                                    var flag = 0;
                                    for (var m = 0; m < boxnumberNum.length; m++) {
                                        if (boxnumberNum[m].innerText == data[i].BoxNumber)
                                        { flag = 1; }
                                    }
                                    if (flag == 0) {
                                        var opt = new Option(data[i].BoxNumber, data[i].BoxNumber);
                                        boxnumberNum.options.add(opt);
                                    }
                                }
                            }
                            if (data[i].Unit != UnitSelected) {
                                if (data[i].Unit != '') {
                                    var flag = 0;
                                    for (var m = 0; m < UnitNum.length; m++) {
                                        if (UnitNum[m].innerText == data[i].Unit)
                                        { flag = 1; }
                                    }
                                    if (flag == 0) {
                                        var opt = new Option(data[i].Unit, data[i].Unit);
                                        UnitNum.options.add(opt);
                                    }
                                }
                            }
                            if (data[i].Specifications != SpecificationsSelected) {
                                if (data[i].Specifications != '') {
                                    var flag = 0;
                                    for (var m = 0; m < SpecificationsNum.length; m++) {
                                        if (SpecificationsNum[m].innerText == data[i].Specifications)
                                        { flag = 1; }
                                    }
                                    if (flag == 0) {
                                        var opt = new Option(data[i].Specifications, data[i].Specifications);
                                        SpecificationsNum.options.add(opt);
                                    }
                                }
                            }
                        }
                    }
                }
            });
            $.ajax({
                url: "/WMS/PreOrder/GetBatchlist",
                type: "POST",
                dataType: "json",
                data: {
                    sku: sku, CustomerID: $('#CustomerName').find("option:selected")[0].value,
                    Warehouse: $('#Warehouse').find("option:selected")[0].value,
                    BatchNumber: batchSelected,
                    GoodsType: Goods,
                    BoxNumber: boxnumberSelected
                },
                success: function (data) {
                    var lens = data.length;
                    var m = 0;
                    if (lens > 0) {
                        for (var i = 0; i < lens; i++) {
                            m += parseFloat(data[i].InventoryQty);
                        }
                    }
                    $(InventoryQty).children("input")[0].value = m;
                }
            });
            var opt = new Option(batchSelected, batchSelected, true, true);
            batchsNum.options.add(opt);
            var opt = new Option(boxnumberSelected, boxnumberSelected, true, true);
            boxnumberNum.options.add(opt);
            var opt = new Option(UnitSelected, UnitSelected, true, true);
            UnitNum.options.add(opt);
            var opt = new Option(SpecificationsSelected, SpecificationsSelected, true, true);
            SpecificationsNum.options.add(opt);
            var batchLength = batchsNum.length;
            for (var j = 0; j < batchLength; j++) {
                if (batchsNum[j].innerText == "" || batchsNum[j].value == "") {
                    batchsNum.options.remove(j);
                    batchLength--;
                    j--;
                }
            }
            var boxnumberLength = boxnumberNum.length;
            for (var j = 0; j < boxnumberLength; j++) {
                if (boxnumberNum[j].innerText == "" || boxnumberNum[j].value == "") {
                    boxnumberNum.options.remove(j);
                    boxnumberLength--;
                    j--;
                }
            }
            var UnitLength = UnitNum.length;
            for (var j = 0; j < UnitLength; j++) {
                if (UnitNum[j].innerText == "" || UnitNum[j].value == "") {
                    UnitNum.options.remove(j);
                    UnitLength--;
                    j--;
                }
            }
            var SpecificationsLength = SpecificationsNum.length;
            for (var j = 0; j < SpecificationsLength; j++) {
                if (SpecificationsNum[j].innerText == "" || SpecificationsNum[j].value == "") {
                    SpecificationsNum.options.remove(j);
                    SpecificationsLength--;
                    j--;
                }
            }
        }
    }
    else if ($('#ViewType').val() == 0) {
        $("#NewDiv").removeAttr("style");
        $("#Newtable").removeAttr("style");
    }
    $("#ReturnIndex").live("click", function () {
        if ($("#ReturnViewType").val() == "get") {
            history.back();
        } else {
            window.location.href = "/WMS/PreOrder/Index";
        }
    });
    $("#ReturnHistory").live("click", function () {
        if ($("#backFlag").val() == 1) {
            window.location.href = "/WMS/OrderManagement/Index";
        } else {
            if ($("#ReturnViewType").val() == "get") {
                window.location.href = "/WMS/PreOrder/Index";
            } else {
                window.location.href = "/WMS/PreOrder/Index";
            }
        }
    });
    $('select[id=CustomerID]').live('change', function () {
        var hiddenActionButton = $('#HideActionButton').val();
        var showEditRelated = $('#ShowEditRelated').val();
        var ViewType = $('#ViewType').val();
        var customerID = $(this).val();
        window.location.href = "/WMS/PreOrder/PreOrderCreateOrEdit/?customerID=" + $(this).val() + "&hideActionButton=" + hiddenActionButton + "&showEditRelated=" + showEditRelated + "&ViewType=1";
    });
    $("#CheckOutboundOrder").live("click", function () {
        $.ajax({
            url: "/WMS/PreOrder/CheckOutboundOrder",
            type: "POST",
            dataType: "json",
            data: {
                Id: $(this)[0].dataset.id,
            },
            success: function (data) {
                if (data.ErrorCode == 1) {
                    if (data.OrderInfo.length == 1) {
                        window.location = "/WMS/OrderManagement/OrderDetailView/?ID=" + data.OrderInfo[0].ID + "&ViewType=3";
                    } else {
                        var html = $("#CheckOutboundOrderList").render(data.OrderInfo);
                        $("#CheckOutboundOrderBody")["empty"]();
                        $("#CheckOutboundOrderBody").append(html);
                        openPopup("", true, 400, 400, null, 'CheckOutbound', true);
                    }
                } else {
                    showMsg("没有生成出库单！", 4000);
                }
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
            }
        });
    })
    $("#searchButton").click(function () {
        if (!yanzhengExtern()) {
            showMsg("请填写外部单号！,谢谢", 4000);
            return false;
        }
        if (!yanzhen()) {
            showMsg("请检查产品编码、期望数量是否填写完整！", 4000);
            return false;
        }
        if (!SameSKU()) {
            showMsg("产品编码不能重复!", 4000);
            return;
        }
        var PreOrderJson = PreOrder();
        var PreOrderDetailJson = addjsontotable();
        var CustomerID = $("#CustomerName").val();
        var WarehouseID = $("#Warehouse").val();
        var PreOrderNumber = $("#PreOrderNumber").val();
        var ExternOrderNumber = $("#ExternOrderNumber").val();
        var ViewType = $("#ViewType").val();
        function post(URL, PARAMS) {
            var temp = document.createElement("form");
            temp.action = URL;
            temp.method = "post";
            temp.style.display = "none";
            for (var x in PARAMS) {
                var opt = document.createElement("textarea");
                opt.name = x;
                opt.value = PARAMS[x];
                temp.appendChild(opt);
            }
            document.body.appendChild(temp);
            temp.submit();
            return temp;
        }
        $.ajax({
            url: "/WMS/PreOrder/AddPreOrderCreateOrEdit",
            type: "POST",
            dataType: "json",
            data: {
                PreOrderJson: PreOrderJson,
                PreOrderDetailJson: PreOrderDetailJson,
                CustomerID: CustomerID,
                WarehouseID: WarehouseID,
                PreOrderNumber: PreOrderNumber,
                ExternOrderNumber: ExternOrderNumber,
                ViewType: ViewType
            },
            success: function (data) {
                if (data.Errorcode == 1) {
                    window.location = "/WMS/PreOrder/PreOrderCreateOrEdit/?ID=" + data.ID + "&ViewType=0";
                } else {
                    showMsg("操作失败," + data.error, 4000);
                }
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
            }
        });
    })
    $('#Province').autocomplete({
        source: function (request, response) {
            var a = this;
            $.ajax({
                url: "/WMS/PreOrder/GetCitys",
                type: "POST",
                dataType: "json",
                data: {
                    find: "Province",
                    Province: $('#Province').val(),
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
    });
    $('#City').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/WMS/PreOrder/GetCity",
                type: "POST",
                dataType: "json",
                data: {
                    find: "City",
                    Province: $('#Province').val(),
                    City: $('#City').val(),
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
    });
    $('#District').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/WMS/PreOrder/GetCity",
                type: "POST",
                dataType: "json",
                data: {
                    find: "District",
                    City: $('#City').val(),
                    District: $('#District').val()
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
    });
    $("#AutomaticAllocation").live('click', (function () {
        layer.confirm('<font size="4">确认是否自动分配</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            title: ['提示', 'font-size:18px;']
        }, function (index) {
            layer.close(index);
            $.ajax({
                url: "/WMS/PreOrder/AutomaticAllocation",
                type: "POST",
                dataType: "text",
                data: {
                    ids: $("#PreAndDetail_SearchCondition_ID").val(),
                },
                success: function (data) {
                    var StrHtml = JSON.parse(data)
                    if (StrHtml.data == '操作失败') {
                        showMsg('操作失败，分配可能没有进行！', 4000);
                    } else {
                        if (StrHtml.data[0].Note == "9") {
                            showMsg('操作成功', 4000);
                            $("#actionButtonDiv").innerHTML = "<a class='btn btn-success' style='text-decoration: none; color: white' hear='/WMS/PreOrder/Index'>返回</a>"
                            $("#AutomaticAllocation")[0].style.display = "none";
                            $("#ManualAllocation")[0].style.display = "none";
                            $("#Complete")[0].style.display = "none";
                            $("#Cancel")[0].style.display = "none";
                            $("#Cancel")[0].style.display = "none";
                        }
                    }
                },
                error: function (msg) {
                    showMsg("操作失败", "4000");
                }
            });
        });
    }));
    $("#ManualAllocation").live('click', (function () {
        window.location.href = "/WMS/PreOrder/ManualAllocation/?ID=" + $("#PreAndDetail_SearchCondition_ID").val();
    }));
})
var ListNums = 0;;
var SKUListNums = 0;
function BreakUp(Pointer, ListNum) {
    var myTable = document.getElementById("resultTable");
    var rowIndex = event.srcElement.parentNode.parentNode.parentNode.rowIndex;
    var obj = document.getElementById('Newtable').getElementsByTagName('tr');
    var newrow = obj[rowIndex].cloneNode(true);
    $(Pointer).parent().parent().parent().after(newrow);
    //dom创建文本框
    var input = document.createElement("input");
    input.style.width = '80px';
    input.type = "text";
    var rows = myTable.rows;
    if (ListNum == ListNums) {
        SKUListNums += 1;
    }
    else {
        ListNums = ListNum;
        SKUListNums = 2;
    }
    var celllocation1 = $(rows[rowIndex]).children(".SKU");
    celllocation1.innerHTML = " <input type='text' onkeydown='GetSkuList(this)'  style='width:130px' class='form-control skucheck' value='' />";
    var celllocation2 = $(rows[rowIndex]).children(".GoodsName");
    celllocation2.innerHTML = " <input type='text'  style='width:150px' readonly='true' class=' form-control goodsnam' value='' />";
    calculateListNum(rows[rowIndex].cells[1].innerText);
};
function GetSkuList(obj) {
    if (event.keyCode == 13) {
        //$.ajax({
        //    url: "/WMS/Product/GetSKUlists",
        //    type: "POST",
        //    dataType: "json",
        //    data: { sku: $(obj).val(), CustomerID: $("select[id=CustomerID]")[0].value },
        //    success: function (data) {
        //        if (data.length > 0) {
        //            $(obj).val(data[0].Value);
        //            $($(obj).parent().parent()).children(".GoodsName").children("input")[0].value = data[0].Text;
        //            //$($(obj).parent().parent()).children(".QtyExpected").children("input")[0].focus();
        //        }
        //        else {
        //            showMsg("未找到该SKU!", "4000");
        //        }
        //    },
        //    error: function (msg) {
        //        showMsg("未找到", "4000");
        //    }
        //})
    }
}
function AddNew(Pointer, ListNum) {
    var myTable = document.getElementById("resultTable");
    var rowIndex = event.srcElement.parentNode.parentNode.parentNode.rowIndex;//当前行的行号
    var obj = document.getElementById('Newtable').getElementsByTagName('tr');
    var newrow = obj[rowIndex].cloneNode(true);
    $(Pointer).parent().parent().parent().after(newrow);
    //dom创建文本框
    var input = document.createElement("input");
    input.style.width = '80px';
    input.type = "text";
    var rows = myTable.rows;
    if (ListNum == ListNums) {
        SKUListNums += 1;
    }
    else {
        ListNums = ListNum;
        SKUListNums = 2;
    }
    var celllocation1 = $(rows[rowIndex]).children(".SKU");
    celllocation1.innerHTML = " <input type='text' name=' ' style='width:130px' class='form-control skucheck' value='' />";
    var celllocation2 = rows[rowIndex].cells[2];
    celllocation2.innerHTML = " <input type='text' name='goodsname' style='width:150px' readonly='true' class=' form-control goodsnam' value='' />";
    calculateListNum(rows[rowIndex].cells[1].innerText);
};
//function Del(de, LineNumber) {
//    $(de).parent().parent().remove();
//    //var id = de.dataset.my;
//    calculateListNum(LineNumber);
//}
function Del(de) {
    $(de).parent().parent().parent().remove();
    var t = document.getElementById("resultTable");
    var rl = t.rows.length;
    var num = 0;
    var froms = $(de).parent().parent().prevAll().length;
    var lengths = $(de).parent().parent().nextAll().length;
    if (rl > 0) {
        for (i = 0; i < rl; i++) {
            num++;
            $(t.rows[i]).children(".LineNumber")[0].innerText = NumList(parseInt(froms) + i + 1);
        }
    } else {
        showMsg("请先添加一行！", 4000);
    }
}
//*************************************************计算行号****************************************************//
function NumList(row_count) {
    var linnumber = '';
    if (row_count < 10) {
        linnumber = "0000" + row_count;
    } else if (100 > row_count && row_count >= 10) {
        linnumber = "000" + row_count;
    } else if (1000 > row_count && row_count >= 100) {
        linnumber = "00" + row_count;
    } else if (10000 > row_count && row_count >= 1000) {
        linnumber = "0" + row_count;
    }
    return linnumber;
}
function calculateListNum(id) {
    //获取table中一列的值
    var t = document.getElementById("resultTable");
    var rl = t.rows.length;
    var num = 0, BreakNum = 0;
    for (i = 0; i < rl; i++) {
        num++;
        $(t.rows[i]).children(".LineNumber")[0].innerText = NumList(num);
    }
}
$('.skucheck').live('dblclick', function () {
    var self = this;
    openPopup("Prepop", true, 1000, 600, '/WMS/Product/index/?flag=1&customerID=' + $('select[id=CustomerName]')[0].value, null, function (Sku, GoodsName, UPC) {
        $(self)[0].value = Sku;
        $($(self).parent().parent()).children(".GoodsName").children("input")[0].value = GoodsName;
        $($(self).parent().parent()).children(".UPC").children("input")[0].value = "";
        $.ajax({
            url: "/WMS/PreOrder/GetBatchlist",
            type: "POST",
            dataType: "json",
            data: { sku: Sku, CustomerID: $('select[id=CustomerName]')[0].value, Warehouse: $('select[id=Warehouse]')[0].value, BatchNumber: '', GoodsType: '', BoxNumber: '' },
            success: function (data) {
                var lens = data.length;
                var BatchId = $($(self).parent().parent()).children(".BatchNumber").children("select")[0];
                var BoxnumberId = $($(self).parent().parent()).children(".BoxNumber").children("select")[0];
                var UnitId = $($(self).parent().parent()).children(".Unit").children("select")[0];
                var SpecificationsId = $($(self).parent().parent()).children(".Specifications").children("select")[0];
                var inventoryId = $($(self).parent().parent()).children(".InventoryQty")[0];
                var GoodsTypeId = $($(self).parent().parent()).children(".GoodsType").children("select")[0];
                var sel = BatchId;
                var sel2 = BoxnumberId;
                var sel3 = UnitId;
                var sel4 = SpecificationsId;
                sel.length = 0;
                sel2.length = 0;
                sel3.length = 0;
                sel4.length = 0;
                var opt1 = new Option("请选择", "请选择", true, true);
                var opt2 = new Option("请选择", "请选择", true, true);
                var opt3 = new Option("请选择", "请选择", true, true);
                var opt4 = new Option("请选择", "请选择", true, true);
                sel.options.add(opt1);
                sel2.options.add(opt2);
                sel3.options.add(opt3);
                sel4.options.add(opt4);
                if (lens > 0) {
                    for (var i = 0; i < lens; i++) {
                        if (data[i].BatchNumber != '') {
                            var flag = 0;
                            for (var m = 0; m < sel.length; m++) {
                                if (sel[m].innerText == data[i].BatchNumber)
                                { flag = 1; }
                            }
                            if (flag == 0) {
                                var opt = new Option(data[i].BatchNumber, data[i].BatchNumber);
                                sel.options.add(opt);
                            }
                        }
                        if (data[i].BoxNumber != '') {
                            var flag = 0;
                            for (var m = 0; m < sel2.length; m++) {
                                if (sel2[m].innerText == data[i].BoxNumber)
                                { flag = 1; }
                            }
                            if (flag == 0) {
                                var opt2 = new Option(data[i].BoxNumber, data[i].BoxNumber);
                                sel2.options.add(opt2);
                            }
                        }
                        if (data[i].Unit != '') {
                            var flag = 0;
                            for (var m = 0; m < sel3.length; m++) {
                                if (sel3[m].innerText == data[i].Unit)
                                { flag = 1; }
                            }
                            if (flag == 0) {
                                var opt3 = new Option(data[i].Unit, data[i].Unit);
                                sel3.options.add(opt3);
                            }
                        }
                        if (data[i].Specifications != '') {
                            var flag = 0;
                            for (var m = 0; m < sel4.length; m++) {
                                if (sel4[m].innerText == data[i].Specifications)
                                { flag = 1; }
                            }
                            if (flag == 0) {
                                var opt4 = new Option(data[i].Specifications, data[i].Specifications);
                                sel4.options.add(opt4);
                            }
                        }
                    }
                }
                var BatchNumbers = $(BatchId).find("option:selected")[0].value == "请选择" ? "" : $(BatchId).find("option:selected")[0].value;
                var BoxNumbers = $(BoxnumberId).find("option:selected")[0].value == "请选择" ? "" : $(BoxnumberId).find("option:selected")[0].value;
                var Goods = $(GoodsTypeId).find("option:selected")[0].value;
                $.ajax({
                    url: "/WMS/PreOrder/GetBatchlist",
                    type: "POST",
                    dataType: "json",
                    data: {
                        sku: Sku, CustomerID: $('select[id=CustomerName]')[0].value,
                        Warehouse: $('select[id=Warehouse]')[0].value,
                        BatchNumber: BatchNumbers,
                        GoodsType: Goods,
                        BoxNumber: BoxNumbers
                    },
                    success: function (data) {
                        var lens = data.length;
                        var m = 0;
                        if (lens > 0) {
                            for (var i = 0; i < lens; i++) {
                                m += parseFloat(data[i].InventoryQty);
                            }
                        }
                        $(inventoryId).children("input")[0].value = m.toFixed(2);
                    }
                });
            }
        });
    });
    $("#popupLayer_Prepop")[0].style.top = "50px";
});
$('.upccheck').live('dblclick', function () {
    var self = this;
    openPopup("Prepop", true, 1000, 600, '/WMS/Product/index/?flag=1&customerID=' + $('select[id=CustomerID]')[0].value, null, function (Sku, GoodsName, UPC) {
        $(self)[0].value = UPC;
        $($(self).parent().parent()).children(".GoodsName").children("input")[0].value = GoodsName;
        $($(self).parent().parent()).children(".SKU").children("input")[0].value = Sku;
        $.ajax({
            url: "/WMS/PreOrder/GetBatchlist",
            type: "POST",
            dataType: "json",
            data: {
                sku: Sku,
                CustomerID: $('select[id=CustomerID]')[0].value,
                Warehouse: $('select[id=Warehouse]')[0].value,
                BatchNumber: '',
                GoodsType: '',
                BoxNumber: '',
                UPC: UPC
            },
            success: function (data) {
                var lens = data.length;
                var BatchId = $($(self).parent().parent()).children(".BatchNumber").children("select")[0];
                var BoxnumberId = $($(self).parent().parent()).children(".BoxNumber").children("select")[0];
                var UnitId = $($(self).parent().parent()).children(".Unit").children("select")[0];
                var SpecificationsId = $($(self).parent().parent()).children(".Specifications").children("select")[0];
                var inventoryId = $($(self).parent().parent()).children(".InventoryQty")[0];
                var GoodsTypeId = $($(self).parent().parent()).children(".GoodsType").children("select")[0];
                var sel = BatchId;
                var sel2 = BoxnumberId;
                var sel3 = UnitId;
                var sel4 = SpecificationsId;
                sel.length = 0;
                sel2.length = 0;
                sel3.length = 0;
                sel4.length = 0;
                var opt1 = new Option("请选择", "请选择", true, true);
                var opt2 = new Option("请选择", "请选择", true, true);
                var opt3 = new Option("请选择", "请选择", true, true);
                var opt4 = new Option("请选择", "请选择", true, true);
                sel.options.add(opt1);
                sel2.options.add(opt2);
                sel3.options.add(opt3);
                sel4.options.add(opt4);
                if (lens > 0) {
                    for (var i = 0; i < lens; i++) {
                        if (data[i].BatchNumber != '') {
                            var flag = 0;
                            for (var m = 0; m < sel.length; m++) {
                                if (sel[m].innerText == data[i].BatchNumber)
                                { flag = 1; }
                            }
                            if (flag == 0) {
                                var opt = new Option(data[i].BatchNumber, data[i].BatchNumber);
                                sel.options.add(opt);
                            }
                        }
                        if (data[i].BoxNumber != '') {
                            var flag = 0;
                            for (var m = 0; m < sel2.length; m++) {
                                if (sel2[m].innerText == data[i].BoxNumber)
                                { flag = 1; }
                            }
                            if (flag == 0) {
                                var opt2 = new Option(data[i].BoxNumber, data[i].BoxNumber);
                                sel2.options.add(opt2);
                            }
                        }
                        if (data[i].Unit != '') {
                            var flag = 0;
                            for (var m = 0; m < sel3.length; m++) {
                                if (sel3[m].innerText == data[i].Unit)
                                { flag = 1; }
                            }
                            if (flag == 0) {
                                var opt3 = new Option(data[i].Unit, data[i].Unit);
                                sel3.options.add(opt3);
                            }
                        }
                        if (data[i].Specifications != '') {
                            var flag = 0;
                            for (var m = 0; m < sel4.length; m++) {
                                if (sel4[m].innerText == data[i].Specifications)
                                { flag = 1; }
                            }
                            if (flag == 0) {
                                var opt4 = new Option(data[i].Specifications, data[i].Specifications);
                                sel4.options.add(opt4);
                            }
                        }
                    }
                }
                var BatchNumbers = $(BatchId).find("option:selected")[0].value == "请选择" ? "" : $(BatchId).find("option:selected")[0].value;
                var BoxNumbers = $(BoxnumberId).find("option:selected")[0].value == "请选择" ? "" : $(BoxnumberId).find("option:selected")[0].value;
                var Goods = $(GoodsTypeId).find("option:selected")[0].value;
                $.ajax({
                    url: "/WMS/PreOrder/GetBatchlist",
                    type: "POST",
                    dataType: "json",
                    data: {
                        sku: Sku, CustomerID: $('select[id=CustomerID]')[0].value,
                        Warehouse: $('select[id=Warehouse]')[0].value,
                        BatchNumber: BatchNumbers,
                        GoodsType: Goods,
                        BoxNumber: BoxNumbers,
                        UPC: UPC
                    },
                    success: function (data) {
                        var lens = data.length;
                        var m = 0;
                        if (lens > 0) {
                            for (var i = 0; i < lens; i++) {
                                m += parseFloat(data[i].InventoryQty);
                            }
                        }
                        $(inventoryId).children("input")[0].value = m.toFixed(2);
                    }
                });
            }
        });
    });
    $("#popupLayer_Prepop")[0].style.top = "50px";
});
$("select[name='batchs00001']").live('change', function () {
    var inventoryId = $($(this).parent().parent()).children(".InventoryQty").children("input")[0];
    var Sku = $($(this).parent().parent()).children(".SKU").children("input")[0].value
    var GoodsTypes = $($(this).parent().parent()).children(".GoodsType").children("select").find("option:selected")[0].value;
    var BoxNumbers = $($(this).parent().parent()).children(".BoxNumber").children("select").find("option:selected")[0].value;
    var BoxNumberID = $($(this).parent().parent()).children(".BoxNumber").children("select")[0];
    var BatchNumbers = $(this)[0].value;
    var UnitID = UnitID = $($(this).parent().parent()).children(".Unit").children("select")[0];
    var SpecificationsID = $($(this).parent().parent()).children(".Specifications").children("select")[0];
    var sel = BoxNumberID;
    var sel2 = UnitID;
    var sel3 = SpecificationsID;
    $.ajax({
        url: "/WMS/PreOrder/GetBatchlist",
        type: "POST",
        dataType: "json",
        data: { sku: Sku, CustomerID: $('select[id=CustomerID]')[0].value, Warehouse: $('select[id=Warehouse]')[0].value, BatchNumber: BatchNumbers, GoodsType: GoodsTypes },
        success: function (data) {
            var lens = data.length;
            var m = 0;
            if (lens > 0) {
                sel.length = 0;
                sel2.length = 0;
                sel3.length = 0;
                var opt1 = new Option("请选择", "请选择", true, true);
                var opt2 = new Option("请选择", "请选择", true, true);
                var opt3 = new Option("请选择", "请选择", true, true);
                sel.options.add(opt1);
                sel2.options.add(opt2);
                sel3.options.add(opt3);
                for (var i = 0; i < lens; i++) {
                    m += parseFloat(data[i].InventoryQty);
                    if (data[i].BoxNumber != '') {
                        var flag = 0;
                        for (var n = 0; n < sel.length; n++) {
                            if (sel[n].innerText == data[i].BoxNumber)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            if (sel.length == 1) {
                                var opt = new Option(data[i].BoxNumber, data[i].BoxNumber, true, true);
                            }
                            else {
                                sel[1].defaultSelected = false;
                                var opt = new Option(data[i].BoxNumber, data[i].BoxNumber);
                            }
                            sel.options.add(opt);
                        }
                    }
                    if (data[i].Unit != '') {
                        var flag = 0;
                        for (var n = 0; n < sel2.length; n++) {
                            if (sel2[n].innerText == data[i].Unit)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            if (sel2.length == 1) {
                                var opt2 = new Option(data[i].Unit, data[i].Unit, true, true);
                            }
                            else {
                                sel2[1].defaultSelected = false;
                                var opt2 = new Option(data[i].Unit, data[i].Unit);
                            }
                            sel2.options.add(opt2);
                        }
                    }
                    if (data[i].Specifications != '') {
                        var flag = 0;
                        for (var n = 0; n < sel3.length; n++) {
                            if (sel3[n].innerText == data[i].Specifications)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            if (sel3.length == 1) {
                                var opt3 = new Option(data[i].Specifications, data[i].Specifications, true, true);
                            }
                            else {
                                sel3[1].defaultSelected = false;
                                var opt3 = new Option(data[i].Specifications, data[i].Specifications);
                            }
                            sel3.options.add(opt3);
                        }
                    }
                }
            }
            inventoryId.value = m.toFixed(3);
        }
    });
});
$("select[name='boxnumber00001']").live('change', function () {
    var inventoryId = $($(this).parent().parent()).children(".InventoryQty").children("input")[0];
    var Sku = $($(this).parent().parent()).children(".SKU").children("input")[0].value
    var GoodsTypes = $($(this).parent().parent()).children(".GoodsType").children("select").find("option:selected")[0].value;
    var BoxNumbers = $(this)[0].value;
    var BatchNumbers = $($(this).parent().parent()).children(".BatchNumber").children("select").find("option:selected")[0].value;
    var UnitID = $($(this).parent().parent()).children(".Unit").children("select")[0];
    var Units = $($(this).parent().parent()).children(".Unit").children("select").find("option:selected")[0].value;
    var SpecificationsID = $($(this).parent().parent()).children(".Specifications").children("select")[0];
    var Specifications = $($(this).parent().parent()).children(".Specifications").children("select").find("option:selected")[0].value;
    var sel2 = UnitID;
    var sel3 = SpecificationsID;
    sel2.length = 0;
    sel3.length = 0;
    var opt2 = new Option("请选择", "请选择", true, true);
    var opt3 = new Option("请选择", "请选择", true, true);
    sel2.options.add(opt2);
    sel3.options.add(opt3);
    $.ajax({
        url: "/WMS/PreOrder/GetBatchlist",
        type: "POST",
        dataType: "json",
        data: { sku: Sku, CustomerID: $('select[id=CustomerID]')[0].value, Warehouse: $('select[id=Warehouse]')[0].value, BatchNumber: BatchNumbers, GoodsType: GoodsTypes, BoxNumber: BoxNumbers },
        success: function (data) {
            var lens = data.length;
            var m = 0;
            if (lens > 0) {
                sel2.length = 0;
                sel3.length = 0;
                var opt2 = new Option("请选择", "请选择", true, true);
                var opt3 = new Option("请选择", "请选择", true, true);
                sel2.options.add(opt2);
                sel3.options.add(opt3);
                for (var i = 0; i < lens; i++) {
                    m += parseFloat(data[i].InventoryQty);
                    if (data[i].Unit != '') {
                        var flag = 0;
                        for (var n = 0; n < sel2.length; n++) {
                            if (sel2[n].innerText == data[i].Unit)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            if (sel2.length == 1) {
                                var opt2 = new Option(data[i].Unit, data[i].Unit, true, true);
                            }
                            else {
                                sel2[1].defaultSelected = false;
                                var opt2 = new Option(data[i].Unit, data[i].Unit);
                            }
                            sel2.options.add(opt2);
                        }
                    }
                    if (data[i].Specifications != '') {
                        var flag = 0;
                        for (var n = 0; n < sel3.length; n++) {
                            if (sel3[n].innerText == data[i].Specifications)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            if (sel3.length == 1) {
                                var opt3 = new Option(data[i].Specifications, data[i].Specifications, true, true);
                            }
                            else {
                                sel3[1].defaultSelected = false;
                                var opt3 = new Option(data[i].Specifications, data[i].Specifications);
                            }
                            sel3.options.add(opt3);
                        }
                    }
                }
            }
            inventoryId.value = m.toFixed(3);
        }
    });
});
$("select[name='Unit00001']").live('change', function () {
    var inventoryId = $($(this).parent().parent()).children(".InventoryQty").children("input")[0];
    var Sku = $($(this).parent().parent()).children(".SKU").children("input")[0].value
    var GoodsTypes = $($(this).parent().parent()).children(".GoodsType").children("select").find("option:selected")[0].value;
    var BoxNumbers = $($(this).parent().parent()).children(".BoxNumber").children("select").find("option:selected")[0].value;
    var BatchNumbers = $($(this).parent().parent()).children(".BatchNumber").children("select").find("option:selected")[0].value;
    var UnitID = $($(this).parent().parent()).children(".Unit").children("select").find("option:selected")[0].value;
    var SpecificationsID = $($(this).parent().parent()).children(".Specifications").children("select")[0];
    var Units = $(this)[0].value;
    var sel3 = SpecificationsID;
    var opt3 = new Option("请选择", "请选择", true, true);
    sel3.options.add(opt3);
    $.ajax({
        url: "/WMS/PreOrder/GetBatchlist",
        type: "POST",
        dataType: "json",
        data: { sku: Sku, CustomerID: $('select[id=CustomerID]')[0].value, Warehouse: $('select[id=Warehouse]')[0].value, BatchNumber: BatchNumbers, GoodsType: GoodsTypes, BoxNumber: BoxNumbers, Unit: Units },
        success: function (data) {
            var lens = data.length;
            var m = 0;
            if (lens > 0) {
                sel3.length = 0;
                var opt3 = new Option("请选择", "请选择", true, true);
                sel3.options.add(opt3);
                for (var i = 0; i < lens; i++) {
                    m += parseFloat(data[i].InventoryQty);
                    if (data[i].Specifications != '') {
                        var flag = 0;
                        for (var n = 0; n < sel3.length; n++) {
                            if (sel3[n].innerText == data[i].Specifications)
                            { flag = 1; }
                        }
                        if (flag == 0) {
                            if (sel3.length == 1) {
                                var opt3 = new Option(data[i].Specifications, data[i].Specifications, true, true);
                            }
                            else {
                                sel3[1].defaultSelected = false;
                                var opt3 = new Option(data[i].Specifications, data[i].Specifications);
                            }
                            sel3.options.add(opt3);
                        }
                    }
                }
            }
            inventoryId.value = m.toFixed(3);
        }
    });
});
$("select[name='Specifications00001']").live('change', function () {
    var inventoryId = $($(this).parent().parent()).children(".InventoryQty").children("input")[0];
    var Sku = $($(this).parent().parent()).children(".SKU").children("input")[0].value;
    var GoodsTypes = $($(this).parent().parent()).children(".GoodsType").children("select")[0].find("option:selected")[0].value;
    var BoxNumbers = $($(this).parent().parent()).children(".BoxNumber").children("select")[0].find("option:selected")[0].value;
    var BatchNumbers = $($(this).parent().parent()).children(".BatchNumber").children("select")[0].find("option:selected")[0].value;
    var UnitID = $($(this).parent().parent()).children(".Unit").children("select")[0];
    var SpecificationsID = $($(this).parent().parent()).children(".Specifications").children("select")[0];
    var Specifications = SpecificationsID.find("option:selected")[0].value;
    var Units = UnitID.find("option:selected")[0].value;
    $.ajax({
        url: "/WMS/PreOrder/GetBatchlist",
        type: "POST",
        dataType: "json",
        data: { sku: Sku, CustomerID: $('select[id=CustomerID]')[0].value, Warehouse: $('select[id=Warehouse]')[0].value, BatchNumber: BatchNumbers, GoodsType: GoodsTypes, BoxNumber: BoxNumbers, Unit: Units, Specifications: Specifications },
        success: function (data) {
            var lens = data.length;
            var m = 0;
            if (lens > 0) {
                for (var i = 0; i < lens; i++) {
                    m += parseFloat(data[i].InventoryQty);
                }
            }
            inventoryId.val(m.toFixed(2));
        }
    });
});
$(".skucheck").live("keydown", function () {
    var self = this;
    $('.skucheck').autocomplete({
        source: function (request, response) {
            if (request.term.length > 5) {

                $.ajax({
                    url: "/WMS/Product/GetSKUlist",
                    type: "POST",
                    dataType: "json",
                    data: { sku: request.term, CustomerID: $('select[id=CustomerID]')[0].value, UPC: "" },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Text, value: item.Text, data: item }
                        }));
                    }
                })
            };
        },
        select: function (event, ui) {
            var t = document.getElementById("resultTable");
            var row = t.getElementsByTagName("tr");
            var row_count = t.rows.length;
            $(self)[0].value = ui.item.data.Text;
            $($(self).parent().parent()).children(".GoodsName").children("input")[0].value = ui.item.data.Value;
            $.ajax({
                url: "/WMS/PreOrder/GetBatchlist",
                type: "POST",
                dataType: "json",
                data: { sku: ui.item.data.Text, CustomerID: $('select[id=CustomerID]')[0].value, Warehouse: $('select[id=Warehouse]')[0].value, BatchNumber: '', GoodsType: '', BoxNumber: '' },
                success: function (data) {
                    var lens = data.length;
                    var BatchId = $($(self).parent().parent()).children(".BatchNumber").children(".select")[0];
                    var BoxnumberId = $($(self).parent().parent()).children(".BoxNumber").children(".select")[0];
                    if (lens > 0) {
                        var sel = BatchId;
                        var sel2 = BoxnumberId;
                        sel.length = 0;
                        sel2.length = 0;
                        var opt1 = new Option("请选择", "请选择", true, true);
                        var opt2 = new Option("请选择", "请选择", true, true);
                        sel.options.add(opt1);
                        sel2.options.add(opt2);
                        for (var i = 0; i < lens; i++) {
                            if (data[i].BatchNumber != '') {
                                var flag = 0;
                                for (var m = 0; m < sel.length; m++) {
                                    if (sel[m].innerText == data[i].BatchNumber)
                                    { flag = 1; }
                                }
                                if (flag == 0) {
                                    var opt = new Option(data[i].BatchNumber, data[i].BatchNumber);
                                    sel.options.add(opt);
                                }
                            }
                            if (data[i].BoxNumber != '') {
                                var flag = 0;
                                for (var m = 0; m < sel2.length; m++) {
                                    if (sel2[m].innerText == data[i].BoxNumber)
                                    { flag = 1; }
                                }
                                if (flag == 0) {
                                    var opt2 = new Option(data[i].BoxNumber, data[i].BoxNumber);
                                    sel2.options.add(opt2);
                                }
                            }
                        }
                    }
                    var BatchNumbers = $($(self).parent().parent()).children(".BatchNumber").children("select").find("option:selected")[0].value;
                    var BoxNumbers = $($(self).parent().parent()).children(".BoxNumber").children("select").find("option:selected")[0].value;
                    var Goods = $($(self).parent().parent()).children(".GoodsType").children("select").find("option:selected")[0].value;
                    var inventoryId = $($(self).parent().parent()).children(".InventoryQty").children("select")[0];
                    $.ajax({
                        url: "/WMS/PreOrder/GetBatchlist",
                        type: "POST",
                        dataType: "json",
                        data: { sku: ui.item.data.Text, CustomerID: $('select[id=CustomerID]')[0].value, Warehouse: $('select[id=Warehouse]')[0].value, BatchNumber: BatchNumbers, GoodsType: Goods, BoxNumber: BoxNumbers },
                        success: function (data) {
                            var lens = data.length;
                            var m = 0;
                            if (lens > 0) {
                                for (var i = 0; i < lens; i++) {
                                    m += parseFloat(data[i].InventoryQty);
                                }
                            }
                            inventoryId.val(m.toFixed(2));
                        }
                    });
                }
            });
            $(self)[0].focus();
        }
    })
}).trigger('keydown');
var row_count = 1;
//得到明细中的总条数
var asndetailcount = 0;
var skulist = new Array();
function contains(arr, obj) {
    var i = arr.length;
    while (i--) {
        if (arr[i] === obj) {
            return true;
        }
    }
    return false;
}
//行号处理
function ReturnLineNumber(row_count) {
    var linnumber = "";
    if (row_count < 10) {
        linnumber = "0000" + row_count;
    } else if (100 > row_count && row_count >= 10) {
        linnumber = "000" + row_count;
    } else if (1000 > row_count && row_count >= 100) {
        linnumber = "00" + row_count;
    } else if (10000 > row_count && row_count >= 1000) {
        linnumber = "0" + row_count;
    }
    return linnumber;
}
function PreOrder() {
    var txt = "[";
    var table = document.getElementById("conditionTable");
    var row = table.getElementsByTagName("tr");
    if (row.length > 1) {
        var r = "{";
        for (var j = 0; j < row.length; j++) {
            var col = row[j].getElementsByTagName("td");
            for (var i = 0; i < col.length; i++) {
                var tds = row[j].getElementsByTagName("td");
                if (tds[i].className.trim() != "TableColumnTitle" && tds[i].innerHTML.trim() != "") {
                    if (tds[i].childNodes[1].type == 'select-one') {
                        //r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + (tds[i].childNodes[1][1].value.trim()) + "\",";
                        r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + (tds[i].childNodes[1].value) + "\",";
                    }
                    else {
                        if (i != 1) {
                            if ($(tds[i]).children('input')[0].name.trim() == 'Status') {
                                r += "\"" + $(tds[i]).children('input')[0].name.trim() + "\"\:\"" + $(tds[i]).children('input')[0].value.trim() + "\",";
                            }
                            else {
                                r += "\"" + $(tds[i]).children('input')[0].id.trim() + "\"\:\"" + $(tds[i]).children('input')[0].value.trim() + "\",";
                            }
                        }
                        else {
                            if ($(tds[i]).children('input')[0].name.trim() == 'OrderTime') {
                                r += "\"" + $(tds[i]).children('input')[0].id.trim() + "\"\:\"" + $(tds[i]).children('input')[0].value.trim() + "\",";
                            }
                            else {
                                r += "\"" + $(tds[i])[0].children[0].id.trim() + "\"\:\"" + $(tds[i])[0].children[0].value.trim() + "\",";
                            }
                        }
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
function SameSKU() {
    var result = true;
    $("#Newtable >tbody tr").each(function (a, b) {
        var SKU = $(b).children(".SKU").children("input").val().trim();
        var UPC = $(b).children(".UPC").children("input").val().trim();
        var BatchNumber = $(b).children(".BatchNumber").children("select").val().trim();
        var BoxNumber = $(b).children(".BoxNumber").children("select").val().trim();
        var GoodsType = $(b).children(".GoodsType").children("select").val().trim();
        var Unit = $(b).children(".Unit").children("select").val().trim();
        var Specifications = $(b).children(".Specifications").children("select").val().trim();
        var str = SKU + UPC + BatchNumber + BoxNumber + GoodsType + Unit + Specifications;
        $("#Newtable >tbody tr").each(function (c, d) {
            if (a < c) {
                var SKU1 = $(d).children(".SKU").children("input").val().trim();
                var UPC1 = $(d).children(".UPC").children("input").val().trim();
                var BatchNumber1 = $(d).children(".BatchNumber").children("select").val().trim();
                var BoxNumber1 = $(d).children(".BoxNumber").children("select").val().trim();
                var GoodsType1 = $(d).children(".GoodsType").children("select").val().trim();
                var Unit1 = $(d).children(".Unit").children("select").val().trim();
                var Specifications1 = $(d).children(".Specifications").children("select").val().trim();
                var str2 = SKU1 + UPC1 + BatchNumber1 + BoxNumber1 + GoodsType1 + Unit1 + Specifications1;
                if (str == str2) {
                    result = false;
                    return result;
                }
            }
        })
    })
    return result;
}
//验证外部单号
function yanzhengExtern() {
    if ($("#ExternOrderNumber").val() == "") {
        return false;
    } else {
        return true;
    }
}
function yanzhen() {
    var ea = true;
    $("#resultTable>tr").each(function (a, b) {
        $(b).children("td").each(function (c, d) {
            if ($(d)[0].className == "SKU" || $(d)[0].className == "GoodsName" || $(d)[0].className == "OriginalQty") {
                if ($(d).children("input")[0].value == "") {
                    ea = false;
                    return ea;
                }
            }
        })
    })
    return ea;
}
//添加的时候获取子表数据
function addjsontotable() {
    var txt = "[";
    var table = document.getElementById("Newtable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 1; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if ($(tds[i])[0].className != "InventoryQty") {
                if (i != 1) {
                    if ($(tds[i]).children("select").length > 0) {
                        if ($(tds[i]).children("select")[0].value.trim() == "请选择") {
                            r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + "" + "\",";
                        } else {
                            r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + $(tds[i]).children("select")[0].value.trim() + "\",";
                        }
                    } else {
                        r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + $(tds[i]).children("input")[0].value.trim() + "\",";
                    }
                }
                else {
                    r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].innerText.trim() + "\",";
                }
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
//$("#AddPreOrder").click(function () {
//    window.location.href = "/WMS/PreOrder/PreOrderCreateOrEdit";
//});
function Cancel(self) {
    if ($(self)[0].dataset.status > 3) {
        showMsg("此状态不能取消！", 2500);
        return false;
    }
    layer.confirm('<font size="4">确认是否取消</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        title: ['提示', 'font-size:18px;']
    }, function (index) {
        layer.close(index);
        $.ajax({
            url: "/WMS/PreOrder/Cancel",
            type: "POST",
            dataType: "json",
            data: {
                ids: $(self)[0].dataset.id,
                CustomerID: $(self)[0].dataset.customerid
            },
            success: function (data) {
                window.location = "/WMS/PreOrder/Index"
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
                window.location = "/WMS/PreOrder/Index"
            }
        });
    });
}
function Complete(Id) {
    layer.confirm('<font size="4">确认是否完成</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        title: ['提示', 'font-size:18px;']
    }, function (index) {
        layer.close(index);
        $.ajax({
            url: "/WMS/PreOrder/OrderFinish",
            type: "POST",
            dataType: "json",
            data: {
                ids: Id,
            },
            success: function (data) {
                showMsg("操作成功!", "4000");
                location.href = "/WMS/PreOrder/OrderFinish/ids=" + Id;
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
            }
        });
    });
}
function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    if (pattern.test(hehe.value)) {
        hehe.value = hehe.value.replace(pattern, "");
    }
}