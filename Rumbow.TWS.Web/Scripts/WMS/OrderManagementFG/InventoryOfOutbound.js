//var datas = {};
//var originaldatas = {};
$(document).ready(function () {
    //(function () {
    //    var id = $("#PreAndDetail_SearchCondition_ID")[0].value;
    //    function a() {
    //        //var id = $("#PreAndDetail_SearchCondition_ID")[0].value;
    //        $.ajax({
    //            url: "/WMS/PreOrder/GetPrdOrder_distributionInventory",
    //            type: "POST",
    //            dataType: "json",
    //            // 注意这里
    //            async: false,
    //            data: {
    //                Id: id
    //            },
    //            success: function (data) {
    //                datas = data;
    //                originaldatas = data;
    //            }
    //        });
    //    }
    //    return a();
    //})();
    window.onload = (
    $("#content>tr").each(function (a, b) {
        var td = $(b).children("td");
        if (a > 8) {
            td.children(".LineNumber").innerHTML = "0" + (a + 1);
        } else {
            td.children(".LineNumber").innerHTML = "00" + (a + 1);
        }
        //if (datas.Errorcode == 1) {

        //    //for (var i = 0; i < .length; i++) {
        //    //    if (i == 7)
        //    //    {
        //    var datalist = "<datalist  class='Location' id='" + td[1].innerText + td[3].innerText + "'style='width: 100%;'>";
        //    for (var j = 0; j < datas.Inventory.length; j++) {
        //        if (td[1].innerText == datas.Inventory[j].SKU && td[3].innerText == datas.Inventory[j].GoodsType) {
        //            datalist += "<option data-SKU='" + datas.Inventory[j].SKU + "' data-Qty='" + datas.Inventory[j].Qty + "' data-name='" + datas.Inventory[j].Qty + "' value='" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "'>" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "</option>";
        //        }
        //    }
        //    datalist += "</datalist >";
        //    $("#actionButtonDiv")[0].innerHTML += datalist;
        //    if (td[6].children[0].value == "" && td[7].children[0].value == "") {
        //        for (var j = 0; j < datas.Inventory.length; j++) {
        //            if (td[1].innerText == datas.Inventory[j].SKU && td[3].innerText == datas.Inventory[j].GoodsType) {
        //                var num = datas.Inventory[j].Qty > (td[4].innerText - td[5].innerText) ? (td[4].innerText - td[5].innerText) : datas.Inventory[j].Qty;
        //                td[6].innerHTML = ' <input type="text" class="Location"   style="width: 100%;" value="' + num + '" />';
        //                td[7].innerHTML = '  <input type="text" class="Location"  list="' + td[1].innerText + td[3].innerText + '" style="width: 100%;" value="' + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + '" />';
        //                var AreaLocation = datas.Inventory[j].Area + "|" + datas.Inventory[j].Location;
        //                RemainingQty(num, td[1].innerHTML, AreaLocation, 0, datas.Inventory[0].GoodsType)
        //                break;
        //            }
        //        }
        //    }
        //}
    }));




    $('.Location').live('change', function () {
        var self = this.list;
        if (self != null) {
            for (var i = 0; i < self.children.length; i++) {
                if (self.innerText == this.value) {
                    //var Qtu5 = $(this).parent().parent()[0].children[5].innerText;
                    //var Qtu6 = $(this).parent().parent()[0].children[6].innerText;
                    var Qtys = RemainingSKUQty(this);
                    var num = self.children[i].dataset.name > Qtys ? Qtys : self.children[i].dataset.name;
                    $(this).parent().parent()[0].children[7].children[0].value = num;
                    RemainingQty(num, self.children[i].dataset.sku, this.value, 0, datas.Inventory[0].GoodsType)
                }
                //if (this.list.innerText = this.value) {
                //    var Qtu4 = $(this).parent().parent()[0].children[4].innerText;
                //    var Qtu5 = $(this).parent().parent()[0].children[5].innerText;
                //    //var num = this.list.children[i].dataset.name > (Qtu4 - Qtu5) ? (Qtu4 - Qtu5) : this.list.children[i].dataset.name;
                //    //$(this).parent().parent().children[6].children[1].value = num;
                //}
            }
        }
    });
    //******************************************计算数量****************************************//
    //function RemainingSKUQty(self) {
    //    //获取table中一列的值
    //    var SKU = $(self).parent().parent().children()[1].innerText.trim();
    //    var Type = $(self).parent().parent().children()[3].innerText.trim();
    //    var Qty = parseFloat($(self).parent().parent().children()[4].innerText.trim());
    //    var history = parseFloat($(self).parent().parent().children()[5].innerText.trim());
    //    var t = document.getElementById("content");
    //    var rl = t.rows.length;
    //    //var QtyReceived = 0;
    //    for (i = 0; i < rl; i++) {
    //        if (t.rows[i].cells[1].innerText == SKU && t.rows[i].cells[3].innerText == Type) {
    //            history += parseFloat(t.rows[i].cells[6].childNodes[1].value.trim() == "" ? 0 : t.rows[i].cells[6].childNodes[1].value.trim());
    //        }
    //    }
    //    for (i = 0; i < rl; i++) {
    //        if (t.rows[i].cells[6].childNodes[1].value == "") {
    //            t.rows[i].cells[6].childNodes[1].style.border = "1px solid red";
    //        } else {
    //            t.rows[i].cells[6].childNodes[1].style.border = "";
    //        }
    //    }
    //    if (Qty <= history) {
    //        $(self)[0].style.border = "1px solid red";
    //        showMsg("库存超出！", 4000);
    //        return 0;
    //    } else {
    //        return Qty - history;
    //    }
    //}
    //function RemainingQty(Qty, SKU, Location, Type, GoodsType) {
    //    var totalQty = 0;
    //    var remainingQty = 0;
    //    var AreaLocation = datas.Inventory[0].Area + "|" + datas.Inventory[0].Location;
    //    $("#content>tr").each(function (a, b) {
    //        var td = $(b).children("td");
    //        if (datas.Errorcode == 1 && td[1].innerText == SKU && td[7].children[0].value == Location) {
    //            totalQty += parseInt(td[6].children[0].value);
    //            var datalist = "";// "<datalist  class='Location' id='" + td[1].innerText + td[3].innerText + "'   style='width: 100%;'>";
    //            for (var j = 0; j < datas.Inventory.length; j++) {
    //                //if(SKU==td[2].innerText)
    //                if (SKU == datas.Inventory[j].SKU && Location == Location && GoodsType == datas.Inventory[j].GoodsType) {
    //                    if (Type == 0) {
    //                        remainingQty = (datas.Inventory[j].Qty - totalQty);
    //                        if (remainingQty < 0) {
    //                            remainingQty = 0;
    //                            showMsg("当前库位库存超出！", 4000);
    //                        }
    //                    }
    //                    else {
    //                        remainingQty = datas.Inventory[j].name + totalQty;
    //                        if (remainingQty > datas.Inventory[j].Qty) {
    //                            remainingQty = datas.Inventory[j].Qty;
    //                        }
    //                    }
    //                    datalist += "<option data-SKU='" + datas.Inventory[j].SKU + "'  data-Qty='" + datas.Inventory[j].Qty + "' data-name='" + remainingQty + "' value='" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "'>" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "</option>";
    //                } else {
    //                    //datalist += "<option data-SKU='" + datas.Inventory[j].SKU + "' data-Qty='" + datas.Inventory[j].Qty + "' data-name='" + datas.Inventory[j].Qty + "' value='" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "'>" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "</option>";
    //                }
    //            }
    //            $("#" + SKU + GoodsType)[0].innerHTML = datalist;
    //        }

    //    })


    //if (Type == 1) {
    //    for (var i = 0; i < datas.Inventory.length; i++) {
    //        var AreaLocation = datas.Inventory[0].Area + "|" + datas.Inventory[0].Location;
    //        if (datas.Inventory[0].Location == Location && datas.Inventory[0].SKU == SKU)
    //            datas.Inventory[i].Qty = datas.Inventory[i].Qty + Qty

    //        var datalist = "";// "<datalist  class='Location' id='" + td[1].innerText + td[3].innerText + "'   style='width: 100%;'>";
    //        for (var j = 0; j < datas.Inventory.length; j++) {
    //            var AreaLocation = datas.Inventory[0].Area + "|" + datas.Inventory[0].Location;
    //            if (SKU == datas.Inventory[j].SKU && Location == AreaLocation) {
    //                datalist += "<option data-SKU='" + datas.Inventory[j].SKU + "' data-name='" + datas.Inventory[j].Qty + "' value='" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "'>" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "</option>";
    //            }
    //        }
    //        //  datalist += "</datalist >";
    //        $("#" + SKU + GoodsType).innerHTML = datalist;
    //        //return 0;
    //    }
    //} else {
    //    for (var i = 0; i < datas.Inventory.length; i++) {
    //        var AreaLocation = datas.Inventory[0].Area + "|" + datas.Inventory[0].Location;
    //        if (AreaLocation == Location && datas.Inventory[0].SKU == SKU)
    //            if (datas.Inventory[i].Qty - Qty >= 0) {
    //                datas.Inventory[i].Qty = datas.Inventory[i].Qty - Qty;

    //                var datalist = "";// "<datalist  class='Location' id='" + td[1].innerText + td[3].innerText + "'   style='width: 100%;'>";
    //                for (var j = 0; j < datas.Inventory.length; j++) {
    //                    var AreaLocation = datas.Inventory[0].Area + "|" + datas.Inventory[0].Location;
    //                    if (SKU == datas.Inventory[j].SKU && Location == AreaLocation) {
    //                        datalist += "<option data-SKU='" + datas.Inventory[j].SKU + "' data-name='" + datas.Inventory[j].Qty + "' value='" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "'>" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "</option>";
    //                    }
    //                }
    //                //  datalist += "</datalist >";
    //                $("#" + SKU + GoodsType)[0].innerHTML = datalist;
    //                //return 0; 
    //            } else {
    //                showMsg("该库区库存不足", 4000);
    //                //dataQty = 0;
    //            }
    //    } 
    //}

    //}

    function yanzhen() {
        //var table = document.getElementById("Newtable");
        //var row = table.getElementsByTagName("tr");
        //var col = row[0].getElementsByTagName("th");


        //for (var j = 1; j < row.length; j++) {

        //    //for (var i = 0; i < col.length - 1; i++) {
        //        var tds = row[j].getElementsByTagName("td>td");
        //    //}
        //}
        if ($("#ExternOrderNumber").val() == "") {
            return false;
        }

        var ea = true;
        $("#resultTable>tr").each(function (a, b) {

            $(b).children("td").each(function (c, d) {
                if ($(d)[0].className == "SKU" || $(d)[0].className == "GoodsName" || $(d)[0].className == "OriginalQty") {
                    if ($(d).children("input")[0].value == "") {
                        ea = false;
                        return ea;
                    }
                }
                //if (c == 1 || c == 2 || c == 9) {
                //    if ($(d)[0].children[0].value == "") {
                //        ea = false;
                //        return ea;
                //    }
                //}
            })
        })
        return ea;
    }
    $("#searchButton").click(function () {
        var t = document.getElementById("content");
        var rl = t.rows.length;
        //var QtyReceived = 0; $(self)[0].style.border = "1px solid red";
        for (i = 0; i < rl; i++) {
            for (var j = 10; j < 10; j++) {
                if (t.rows[i].cells[j].children[0].style.border == "1px solid red" || t.rows[i].cells[j].children[0].value == "") {
                    showMsg("请检查录入信息！", 4000);
                    return false;
                }
            }
        }
        if (!yanzhen()) {
            showMsg("请检查是否填写完整！", 4000);
            return false;
        }
        var Jaonstr = TableToJson("resultTable");
        if (Jaonstr.length < 10) {
            showMsg("录入信息不合法！", 4000);
            return false;
        }
        var PreOrderJson = PreOrder();
        if (PreOrderJson.length < 10) {
            showMsg("录入信息不合法！", 4000);
            return false;
        }
        $.ajax({
            url: "/WMS/OrderManagementFG/InventoryOfOutboundJson",
            type: "Post",
            data: {
                "CustomerId": $("#PreAndDetail_SearchCondition_CustomerID")[0].value,
                "PreOrderJson": PreOrderJson,
                "Jaonstr": Jaonstr,
            },
            success: function (data) {
                if (data.Code = 1) {
                    if (data.Result == 9) {
                        window.location = "/WMS/OrderManagementFG/Index";
                    } else if (data.Result == 0) {
                        showMsg("外部单号重复", 4000);
                    } else if (data.Result == 1) {
                        showMsg("已生成预出库单，但是分配失败，请检查库存！", 4000);
                    }

                } else {
                    showMsg("操作失败！请检查录入信息", 4000);
                }
                //if (data == "分配完成" || data == "完成") {
                //    window.location.href = "/WMS/PreOrder/PreOrderCreateOrEdit/?ID=" + $("#PreAndDetail_SearchCondition_ID")[0].value + '&ViewType=0';
                //    //         ?ID=@item.ID&ViewType=0"
                //    //post('/WMS/PreOrder/PreOrderCreateOrEdit', { ID: $("#PreAndDetail_SearchCondition_ID")[0].value, ViewType: '0' });
                //} else {
                //    showMsg(data, 4000);
                //}
                //showMsg(data, 4000);
                //if (data == "True") {
                //    post('/WMS/ShelvesManagement/Index', { ShelvesModel: null, Action: '查询' });
                //}
            },
            error: function (e) {
                showMsg("操作失败！", 4000);
            }
        })
    })
    //*******************************************判断库区库位选择是否正确*****************************************//
    $(".Area").live("blur", function () {
        var self = this;
        $.ajax({
            url: "/WMS/ShelvesManagement/ChangeArea",
            type: "POST",
            dataType: "json",
            data: {
                id: $(this).parent().parent()[0].children[13].innerText,
                name: this.value,
                type: "blur"
            },
            success: function (data) {
                if (data.length == 0) {
                    self.style.border = "1px solid red";
                } else {
                    self.style.border = '';
                }

            }
        });
    })
    $(".Area").live("keydown", function () {
        $('.Area').autocomplete({
            source: function (request, response) {

                $.ajax({
                    url: "/WMS/ShelvesManagement/ChangeArea",
                    type: "POST",
                    dataType: "json",
                    data: {
                        id: $(this.element[0]).parent().parent()[0].children[13].innerText.trim(),
                        name: request.term,
                        type: "keydown"
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Text, value: item.Text, data: item }
                        }));
                    }
                });
            },
        });
    }).trigger('keydown');

    $(".Location").live("blur", function () {
        var self = this;
        if (self.value.split("|").length > 1) {
            $.ajax({
                url: "/WMS/ShelvesManagement/ChangeLocation",
                type: "POST",
                dataType: "json",
                async: false,
                data: {
                    id: $(self).parent().parent()[0].children[15].innerText,
                    AreaName: self.value.split("|")[0].trim(), //$(this).parent().parent()[0].children[7].children[0].value.trim(),
                    name: self.value.split("|")[1].trim(),
                    type: "blur"
                },
                success: function (data) {
                    if (data.length == 0) {
                        self.style.border = "1px solid red";
                    } else {
                        self.style.border = '';
                    }
                },
                error: function (msg) {
                    showMsg("操作失败", "4000");
                }

            });
        }
    })
    $(".Location").live("keydown", function () {
        $('.Location').autocomplete({
            minLength: 4,
            source: function (request, response) {

                $.ajax({
                    //url: "/WMS/ShelvesManagement/ChangeLocation",
                    url: "/WMS/InventoryManagement/GetLocationList",
                    type: "POST",
                    dataType: "json",
                    data: {
                        location: request.term,
                        warehouseid: $(this.element[0]).parent().parent()[0].children[15].innerText.trim(),
                        areaid: 0
                        //id: $(this.element[0]).parent().parent()[0].children[11].innerText.trim(),
                        //AreaName: $(this.element[0]).parent().parent()[0].children[7].children[0].value.trim(),
                        //name: request.term,
                        //type: "keydown"
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Text, value: item.Text, data: item }
                        }));
                    },
                    error: function (msg) {
                        showMsg("没找到库区库位", 4000);
                    }
                });
            },
            select: function (event, ui) {
                //$('.goodstype' + LineNumber).val(ui.item.data.GoodsType); 
                var t = document.getElementById("content");
                var row = t.getElementsByTagName("tr");
                var row_count = t.rows.length;
                for (var i = 0; i < row_count; i++) {
                    if (ui.item.data.Text == row[i].children[11].children[0].value) {
                        //    showMsg("SKU已存在!", 4000);
                        //    return false;
                        //}
                    }
                    //$(self)[0].value = ui.item.data.Text;
                    ////skulist.push(ui.item.data.Text);
                    ////$('.goodsnam00001').val(ui.item.data.Value);
                    //$(self).parent().next().children()[0].value = ui.item.data.Value;
                    ////$(self).next.value = ui.item.data.Value;
                    //$(self)[0].focus();
                }
            }
        });

    }).trigger('keydown');
})

var box = {
    行号: 'LineNumber',
    SKU: 'SKU', UPC: 'UPC', 货品描述: 'GoodsName',
    货品等级: 'GoodsType', POID: "POID",
    期望数量: 'OriginalQty', 实际数量: 'AllocatedQty', 库区: 'Area', 批次号: 'BatchNumber',
    库位: 'Location', 仓库: 'Warehouse', 仓库ID: 'WarehouseId', 外部单号: 'ExternOrderNumber', 托号: 'BoxNumber', 单位: 'Unit', 规格: 'Specifications'
};
function TableToJson(tableid) {
    var txt = "[";
    var table = document.getElementById(tableid);
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 0; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if ($(tds[i])[0].className != "AllocatedQty") {
                //innerVal = tds[i].childNodes[1].innerText.trim();
                if ($(tds[i]).children("select").length > 0) {

                    r += "\"" + box[col[i].innerText.trim()] + "\"\:\"" + $(tds[i]).children(".OriginalQty").children("input")[0].value.trim() + "\",";

                } else if ($(tds[i]).children("input").length > 0) {
                    if ($(tds[i])[0].className.indexOf("Location") < 0) {
                        //if ($(tds[i]).children(".Location").length > 0) {
                        r += "\"" + box[col[i].innerText.trim()] + "\"\:\"" + $(tds[i]).children("input")[0].value.trim() + "\",";

                    } else {
                        if (tds[i].childNodes[1].value.split("|").length > 1) {
                            r += "\"" + 'Area' + "\"\:\"" + tds[i].childNodes[1].value.split("|")[0].trim() + "\",";
                            r += "\"" + 'Location' + "\"\:\"" + tds[i].childNodes[1].value.split("|")[1].trim() + "\",";
                        } else {
                            tds[i].childNodes[1].style.border = "1px solid red";
                            return "";
                        }

                    }
                } else {
                    r += "\"" + box[col[i].innerText.trim()] + "\"\:\"" + tds[i].innerText.trim() + "\",";
                }
                //var tdsval = innerVal == "" ? 0 : innerVal;
                //switch (i) {
                //    case 7:
                //        tds[7].innerHTML = " <input style='width: 80px;' onblur=la(" + tdsval + ",this) data-name=" + tdsval + " type='text' value='" + tdsval + "'>";
                //        break;
                //    case 8:
                //        tds[8].innerHTML = " <input  class='ui-autocomplete-input Area'  role='textbox'aria-haspopup='true' aria-autocomplete='list'style='width: 80px;' type='text'value='" + innerVal + "'>";
                //        break;
                //}
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


function PreOrder() {
    var txt = "[";
    var table = document.getElementById("conditionTable");
    var row = table.getElementsByTagName("tr");
    if (row.length > 1) {
        var r = "{";
        //$("#conditionTable>tbody tr").each(function (a, b) {
        //    var col = row[j].getElementsByTagName("b>input");
        //    for (var i = 0; i < col.length; i++) {
        //    }
        //})

        for (var j = 0; j < row.length; j++) {
            var col = row[j].getElementsByTagName("td");
            for (var i = 0; i < col.length; i++) {
                var tds = row[j].getElementsByTagName("td");
                if (tds[i].className.trim() != "TableColumnTitle" && tds[i].innerHTML.trim() != "") {
                    //else {
                    //    r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + tds[i].childNodes[1].value + "\",";
                    //}
                    if (tds[i].childNodes[1].type == 'select-one') {

                        r += "\"" + $(tds[i]).children('select')[0].id.trim() + "\"\:\"" + $(tds[i]).children('select')[0].value.trim() + "\",";

                    }
                    else {
                        if (j == 1 && i == 6) {
                            r += "\"" + $(tds[i]).children('input')[0].id.trim() + "\"\:\"" + $(tds[i]).children('input')[0].value.trim() + "\",";
                        }
                            //if (j == 0 && i == 3) {
                            //    r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + tds[i].childNodes[1].value + "\",";
                            //}
                        else {
                            //if (j == 4 && i == 1) {
                            //    r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + 1 + "\",";
                            //} else {  $(tds[5]).children('input')[0]
                            if ($(tds[i]).children('input')[0].name.trim() == 'Status') {
                                r += "\"" + tds[i].childNodes[1].name.trim() + "\"\:\"" + tds[i].childNodes[1].id.trim() + "\",";
                            }
                            else {
                                r += "\"" + $(tds[i]).children('input')[0].id.trim() + "\"\:\"" + $(tds[i]).children('input')[0].value.trim() + "\",";
                            }
                            //}
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
//得到当前的单元格
var currentCell;
function editCell(event) {
    if (event == null) {
        currentCell = window.event.srcElement;
    }
    else {
        currentCell = event.target;
    }
    if (currentCell.tagName == "TD") {
        //用单元格的值来填充文本框的值
        input.value = currentCell.innerHTML;
        //当文本框丢失焦点时调用last
        input.onblur = last;
        input.ondblclick = last;
        currentCell.innerHTML = "";
        //把文本框加到当前单元格上.
        currentCell.appendChild(input);
        //根据liu_binq63 的建议修定下面的bug 非常感谢
        input.focus();
    }
}
function last() {
    //充文本框的值给当前单元格
    currentCell.innerHTML = input.value;
}
var ListNums = 0;;
var SKUListNums = 0;
function BreakUp(Pointer, ListNum) {
    ListNum = 1;
    // var html = $("#Operation").render("");
    //$("#chaifen")["empty"]();
    //$("#chaifen").parent().parent().append(html);
    var myTable = document.getElementById("resultTable");
    var rowIndex = event.srcElement.parentNode.parentNode.rowIndex//当前行的行号
    var obj = document.getElementById('resultTable').getElementsByTagName('tr');
    var newrow = obj[rowIndex].cloneNode(true);
    // var col = document.getElementsByTagName('tr'), i, td;
    // td = col[rowIndex].getElementsByTagName('td')[0].innerHTML("");
    //这个td就是tr首个td 
    newrow.setAttribute("class", "NewTableBGColor");
    //  newrow.ondblclick = editCell;
    //alert(newrow);
    $(Pointer).parent().parent().after(newrow);
    //dom创建文本框
    var input = document.createElement("input");
    input.style.width = '80px';
    input.type = "text";
    //input.ondblclick = editCells;
    // var tab = document.getElementById("table1");  //找到这个表格
    var rows = myTable.rows;
    if (ListNum == ListNums) {

        SKUListNums += 1;
    }
    else {
        ListNums = ListNum;
        SKUListNums = 2;
    }

    //var datalist = "<select class='Location'    style='width: 100%;'>'";
    //for (var j = 0; j < datas.Inventory.length; j++) {
    //    if (rows[rowIndex].cells[1].innerText == datas.Inventory[j].SKU) {
    //        datalist += "<option  value='" + datas.Inventory[j].Qty + "'>" + datas.Inventory[j].Area + "|" + datas.Inventory[j].Location + "</option>";
    //    }
    //}
    //datalist += "</select>";
    //if (td[6].children[0].value == "" && td[7].children[0].value == "") {
    //    var num = datas.Inventory[0].Qty > (td[4].innerText - td[5].innerText) ? (td[4].innerText - td[5].innerText) : datas.Inventory[0].Qty;
    //    td[6].innerHTML = '<input type="text" class="Location"   style="width: 100%;" value="' + num + '" />';
    //    td[7].innerHTML = datalist;
    //} 
    var cell = rows[rowIndex + 1].cells[10];
    cell.innerHTML = "  <label class='deleteSettledPod labelPointer' data-SKU='" + rows[rowIndex].cells[1].innerText + "'  onclick='Del(this)'>删除</label>"
    var celllocation = rows[rowIndex + 1].cells[8];
    celllocation.innerHTML = " <input  role='textbox'aria-haspopup='true' onblur='la(this)' aria-autocomplete='list' value='' name='' style='width: 100%;' type='text'>";//
    var celllocation9 = rows[rowIndex + 1].cells[9];
    celllocation9.innerHTML = " <input  role='textbox'aria-haspopup='true'class='Location' list='" + rows[rowIndex].cells[1].innerText + rows[rowIndex].cells[4].innerText + "' aria-autocomplete='list'  name='' style='width: 100%;' type='text'>";
    calculateListNum(rows[rowIndex].cells[1].innerText);
    //$(Pointer).parent().parent().next().find("input[type='text']").trigger('keydown');
};
function Del(self) {
    LineNumber = $(self)[0].dataset.sku;
    $(self).parent().parent().remove();
    //var id = de.dataset.my;
    calculateListNum(LineNumber);
}

//***********************************计算实收数量********************************************、、
function la(self) {
    //获取table中一列的值
    var SKU = $(self).parent().parent().children(".SKU")[0].innerText.trim();
    var Type = $(self).parent().parent().children(".GoodsType")[0].innerText.trim();
    var Qty = 0; //parseFloat($(self).parent().parent().children()[4].innerText.trim());
    var history = 0;// parseFloat($(self).parent().parent().children()[5].innerText.trim());
    var QtyAll = 0;
    var t = document.getElementById("content");
    var rl = t.rows.length;
    //var QtyReceived = 0;
    for (i = 0; i < rl; i++) {
        if ($(t.rows[i]).children(".SKU")[0].innerText.trim() == SKU && $(t.rows[i]).children(".GoodsType")[0].innerText.trim() == Type) {
            Qty += parseFloat($(t.rows[i]).children(".OriginalQty").children("input")[0].value.trim() == "" ? 0 : $(t.rows[i]).children(".OriginalQty").children("input")[0].value.trim());
            history += parseFloat($(t.rows[i]).children(".AllocatedQty")[0].innerText.trim() == "" ? 0 : $(t.rows[i]).children(".AllocatedQty")[0].innerText.trim());
            QtyAll = parseFloat($(t.rows[i]).children(".OriginalQty")[0].innerText.trim() == "" ? 0 : $(t.rows[i]).children(".OriginalQty")[0].innerText.trim());
        }
    }
    for (i = 0; i < rl; i++) {
        if ($(t.rows[i]).children(".OriginalQty").children("input")[0].value.trim() == "") {
            $(t.rows[i]).children(".OriginalQty").children("input")[0].style.border = "1px solid red";
            //t.rows[i].cells[10].childNodes[1].style.border = "1px solid red";
        } else {
            $(t.rows[i]).children(".OriginalQty").children("input")[0].style.border = "";
            //t.rows[i].cells[10].childNodes[1].style.border = "";
        }
    }
    //if (Qty + history > QtyAll) {
    //    $(self)[0].style.border = "1px solid red";
    //    showMsg("库存超出！", 4000);
    //}

}

//*************************************************计算SUK行号****************************************************//
function NumList(num) {
    var result = '';
    if (num < 10) {
        result = '00' + num;
    } else if (num < 99 && num >= 10) {
        result = '0' + num;
    }
    return result;
}
function calculateListNum(id) {
    //获取table中一列的值
    var t = document.getElementById("content");
    var rl = t.rows.length;
    var num = 0, BreakNum = 0;
    for (i = 0; i < rl; i++) {
        if (t.rows[i].cells[1].innerText == id) {

            //if (t.rows[i].className == "NewTableBGColor") {
            num++;
            t.rows[i].cells[0].innerText = NumList(num);
            //}
        }
    }
}
