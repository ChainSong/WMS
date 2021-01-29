
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
    var onmouse = function () {

        $("#DisInfoBody tr").click(function () {
            $(this).addClass("btn-success").siblings("tr").removeClass("btn-success");
        });
        $("#DisInfoBody tr").mouseover(function () {
            $(this).addClass("btn-info").siblings("tr").removeClass("btn-info");
        });

        $("#DisInfoBody tr").mouseleave(function () {
            $(this).removeClass("btn-info");
        });
        $("#DisInfoBody tr").dblclick(function () {

            var Qty = $(this).children()[5].innerText;
            var Location = $(this).children()[11].innerText;
            var IID = $(this).children()[0].innerText;
            if (Location != "") {
                closePopup(Qty, Location, IID);
            }

        });

    }
    $(".LocationChildren").live("dblclick", function () {
        var self = this;
        $.ajax({
            url: "/WMS/PreOrder/GetPrdOrder_distributionInventory",
            type: "POST",
            dataType: "json",
            // 注意这里
            async: false,
            data: {

                PREID: $($(self).parent().parent()).data().podid
            },
            success: function (data) {
                if (data.Errorcode == 1) {
                    var html = $("#Evaluation").render(data.Inventory);
                    $("#DisInfoBody")["empty"]();
                    $("#DisInfoBody").append(html);

                    openPopup("selectPop", true, 830, 530, null, 'DisInfo', function (Qty, Location, IID) {

                        $($(self).parent().parent()).children(".Location").children("input")[0].value = Location;
                        if (parseFloat($($(self).parent().parent()).children(".OriginalQty")[0].innerHTML) > parseFloat(Qty)) {
                            $($(self).parent().parent()).children(".AllocatedQty").children("input")[0].value = Qty;
                        } else {
                            $($(self).parent().parent()).children(".AllocatedQty").children("input")[0].value = $($(self).parent().parent()).children(".OriginalQty")[0].innerHTML;
                        }
                        //$(self).parent().parent().children()[11].children(0).value = Location;
                        $($(self).parent().parent()).children(".IID")[0].innerText = IID;
                        //$(self).parent().parent().children()[17].innerText = IID;
                    });
                    onmouse();
                    $("#popupLayer_selectPop")[0].style.top = "50px";
                } else {
                    showMsg("没有合适的库存", 2000)
                }
            }
        });
    })





    //$("#panelBut").mouseover(function(){
    //    $("#panel").show(500);
    //});
    //$("#panel").mouseout(function () {
    //    $("#panel").hide(500);
    //    //$(this).next().hide();
    //})
    //#("#panelBut").click(function())
    $("#searchButton").click(function () {
        var t = document.getElementById("resultTable");
        var rl = t.rows.length;
        //var QtyReceived = 0; $(self)[0].style.border = "1px solid red";
        //for (i = 0; i < rl; i++) {
        //    for (var j = 11; j < 13; j++) {
        //        if (t.rows[i].cells[j].children[0].style.border == "2px solid red" || t.rows[i].cells[j].children[0].value == "") {
        //            showMsg("请检查录入信息！", 2000);
        //            return false;
        //        }
        //    }
        //}
        var Jaonstr = TableToJson("Newtable");
        if (Jaonstr.length < 10) {
            showMsg("录入信息不合法！", 2000);
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/WMS/PreOrder/ManualAllocationJson",

            data: {
                "ID": $("#PreAndDetail_SearchCondition_ID")[0].value,
                "CustomerId": $("#PreAndDetail_SearchCondition_CustomerID")[0].value,
                "Criterion": $("#Criterion")[0].value,
                "Jaonstr": Jaonstr,
            },
            success: function (data) {
                if (data.Errorcode == 1 && data.data[0].Message != "库存不足") {
                    window.location.href = "/WMS/PreOrder/PreOrderCreateOrEdit/?ID=" + $("#PreAndDetail_SearchCondition_ID")[0].value + '&ViewType=0&Flag=1';
                    //         ?ID=@item.ID&ViewType=0"
                    //post('/WMS/PreOrder/PreOrderCreateOrEdit', { ID: $("#PreAndDetail_SearchCondition_ID")[0].value, ViewType: '0' });
                } else if (data.Errorcode == 1 && data.data[0].Message == "库存不足") {
                    showMsg("库存不足！", 2000);
                } else {
                    showMsg("操作失败！,请检查 分配数量不能大于期望数量。", 2000);
                }
            },
            error: function (e) {
                showMsg("操作失败！", 2000);
            }
        })
    })


    $("#saveButton").click(function () {
        var t = document.getElementById("Newtable");
        var rl = t.rows.length;
        //var QtyReceived = 0; $(self)[0].style.border = "1px solid red";
        //for (i = 0; i < rl; i++) {
        //    for (var j = 10; j < 12; j++) {
        //        if (t.rows[i].cells[j].children[0].style.border == "2px solid red" || t.rows[i].cells[j].children[0].value == "") {
        //            showMsg("请检查录入信息！", 2000);
        //            return false;
        //        }
        //    }
        //}
        var Jaonstr = TableToJson("resultTable");
        //if (Jaonstr.length < 10) {
        //    showMsg("录入信息不合法！", 2000);
        //    return false;
        //}
        $.ajax({
            type: "POST",
            url: "/WMS/PreOrder/ManualAllocationSaveJson",

            data: {
                "ID": $("#PreAndDetail_SearchCondition_ID")[0].value,
                "CustomerId": $("#PreAndDetail_SearchCondition_CustomerID")[0].value,
                "Criterion": $("#Criterion")[0].value,
                "Jaonstr": Jaonstr,
            },
            success: function (data) {
                if (data.Errorcode == 1) {
                    window.location.href = "/WMS/PreOrder/PreOrderCreateOrEdit/?ID=" + $("#PreAndDetail_SearchCondition_ID")[0].value + '&ViewType=0&Flag=2';
                    //         ?ID=@item.ID&ViewType=0"
                    //post('/WMS/PreOrder/PreOrderCreateOrEdit', { ID: $("#PreAndDetail_SearchCondition_ID")[0].value, ViewType: '0' });
                } else {
                    showMsg("操作失败！", 2000);
                }
            },
            error: function (e) {
                showMsg("操作失败！", 2000);
            }
        })
    })

    //*******************************************判断库区库位选择是否正确*****************************************//
    //$(".Area").live("blur", function () {
    //    var self = this;
    //    $.ajax({
    //        url: "/WMS/ShelvesManagement/ChangeArea",
    //        type: "POST",
    //        dataType: "json",
    //        data: {
    //            id: $(this).parent().parent()[0].children[12].innerText,
    //            name: this.value,
    //            type: "blur"
    //        },
    //        success: function (data) {
    //            if (data.length == 0) {
    //                self.style.border = "1px solid red";
    //            } else {
    //                self.style.border = '';
    //            }

    //        }
    //    });
    //})
    //$(".Area").live("keydown", function () {
    //    $('.Area').autocomplete({
    //        source: function (request, response) {

    //            $.ajax({
    //                url: "/WMS/ShelvesManagement/ChangeArea",
    //                type: "POST",
    //                dataType: "json",
    //                data: {
    //                    id: $(this.element[0]).parent().parent()[0].children[12].innerText.trim(),
    //                    name: request.term,
    //                    type: "keydown"
    //                },
    //                success: function (data) {
    //                    response($.map(data, function (item) {
    //                        return { label: item.Text, value: item.Text, data: item }
    //                    }));
    //                }
    //            });
    //        }
    //    });
    //}).trigger('keydown');

    //$(".LocationChildren").live("blur", function () {
    //    var self = this;
    //    if (self.value != "" && self.value.split("|").length > 1) {
    //        $.ajax({
    //            url: "/WMS/ShelvesManagement/ChangeLocation",
    //            type: "POST",
    //            dataType: "json",
    //            async: false,
    //            data: {
    //                id: $($(self).parent().parent()).children(".POID")[0].innerText.trim(),
    //                AreaName: self.value.split("|")[0].trim(), //$(this).parent().parent()[0].children[7].children[0].value.trim(),
    //                name: self.value.split("|")[1].trim(),
    //                type: "blur"
    //            },
    //            success: function (data) {
    //                if (data.length == 0) {
    //                    self.style.border = "1px solid red";
    //                } else {
    //                    self.style.border = '';
    //                }
    //            },
    //            error: function (msg) {
    //                showMsg("操作失败", "2000");
    //            }

    //        });
    //    }
    //})
    //$(".Location").live("keydown", function () {
    //    $('.Location').autocomplete({
    //        minLength: 4,
    //        source: function (request, response) {

    //            $.ajax({
    //                //url: "/WMS/ShelvesManagement/ChangeLocation",
    //                url: "/WMS/InventoryManagement/GetLocationList",
    //                type: "POST",
    //                dataType: "json",
    //                data: {
    //                    location: request.term,
    //                    warehouseid: $(this.element[0]).parent().parent()[0].children[12].innerText.trim(),
    //                    areaid: 0
    //                },
    //                success: function (data) {
    //                    response($.map(data, function (item) {
    //                        return { label: item.Text, value: item.Text, data: item }
    //                    }));
    //                },
    //                error: function (msg) {
    //                    showMsg("没找到库区库位", 2000);
    //                }
    //            });
    //        },
    //        select: function (event, ui) {
    //            var t = document.getElementById("content");
    //            var row = t.getElementsByTagName("tr");
    //            var row_count = t.rows.length;
    //            for (var i = 0; i < row_count; i++) {
    //                if (ui.item.data.Text == row[i].children[8].children[0].value) {
    //                    //    showMsg("SKU已存在!", 2000);
    //                    //    return false;
    //                    //}
    //                }
    //                //$(self)[0].value = ui.item.data.Text;
    //                ////skulist.push(ui.item.data.Text);
    //                ////$('.goodsnam00001').val(ui.item.data.Value);
    //                //$(self).parent().next().children()[0].value = ui.item.data.Value;
    //                ////$(self).next.value = ui.item.data.Value;
    //                //$(self)[0].focus();
    //            }
    //        }
    //    });

    //}).trigger('keydown');
})
var box = {
    行号: 'LineNumber',
    SKU: 'SKU', SKU: 'SKU', 货品描述: 'GoodsName',
    货品等级: 'GoodsType', POID: "POID", 库区: 'Area', 库存ID: 'IID', PODID: 'ID',
    期望数量: 'OriginalQty', 实际数量: 'AllocatedQty',
    库位: 'Location', 仓库: 'Warehouse', 仓库ID: 'WarehouseId', 外部单号: 'ExternOrderNumber', 批次号: 'BatchNumber', 托号: 'BoxNumber', 单位: 'Unit', 规格: 'Specifications',
    UPC: 'UPC', '(库区)库位': "Location"

};
function TableToJson(tableid) {
    var txt = "[";
    var table = document.getElementById(tableid);
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 1; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if (box[col[i].innerText.trim()] != undefined) {
                if ($(tds[i]).children("input").length > 0) {
                    if ($(tds[i])[0].className.indexOf("Location") < 0) {
                        Numval = $(tds[i]).children("input")[0].value;
                        innerVal = Numval;
                    } else {

                        Numval = $(tds[i]).children("input")[0].value;

                        //if (col[i].innerText.trim() == "实际数量") {
                        //r += "\"" + box["期望数量"] + "\"\:\"" + Numval + "\",";
                        //r += "\"" + box["期望数量"]  + "\"\:\"" + Numval + "\",";
                        //r += "\"" + box["实际数量"]  + "\"\:\"" + Numval + "\",";
                        //} else 
                        if (Numval.split("|").length > 1) {
                            r += "\"" + box["库区"] + "\"\:\"" + Numval.split("|")[0].trim() + "\",";
                            r += "\"" + box["库位"] + "\"\:\"" + Numval.split("|")[1].trim() + "\",";
                            continue;
                        }
                        //else {
                        //return "";
                        //}
                        //}
                    }
                } else if ($(tds[i]).children("select").length > 0) {
                    innerVal = $(tds[i]).find("option:selected")[0].value;
                }
                else {
                    innerVal = tds[i].innerText.trim();
                }
                r += "\"" + box[col[i].innerText.trim()] + "\"\:\"" + innerVal + "\",";

            }
            //if ($(tds[i])[0].className != "InventoryQty") {
            //    if (i != 1) {
            //        if ($(tds[i]).children("select").length > 0) {
            //            if ($(tds[i]).children("select")[0].value.trim() == "请选择") {
            //                r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + "" + "\",";
            //            } else {
            //                r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + $(tds[i]).children("select")[0].value.trim() + "\",";
            //            }

            //        } else {
            //            r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + $(tds[i]).children("input")[0].value.trim() + "\",";
            //        }
            //    }
            //    else {
            //        r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].innerText.trim() + "\",";
            //    }
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
function BreakUp(Pointer, ListNum,linenumber) {
    ListNum = 1;
    // var html = $("#Operation").render("");
    //$("#chaifen")["empty"]();
    //$("#chaifen").parent().parent().append(html);
    var myTable = document.getElementById("resultTable");
    var rowIndex = event.srcElement.parentNode.parentNode.parentNode.rowIndex//当前行的行号
    var obj = document.getElementById('Newtable').getElementsByTagName('tr');
    var newrow = obj[rowIndex].cloneNode(true);
    // var col = document.getElementsByTagName('tr'), i, td;
    // td = col[rowIndex].getElementsByTagName('td')[0].innerHTML("");
    //这个td就是tr首个td 
    newrow.setAttribute("class", "NewTableBGColor");
    //  newrow.ondblclick = editCell;
    //alert(newrow);
    $(Pointer).parent().parent().parent().after(newrow);

    //清除  实际数量、库区库位、inventoryid
    $("#Newtable").find("tr:eq(" + (rowIndex + 1) + ") td").each(function () {
        if ($(this).attr("class") == "AllocatedQty") { $(this).find("input:eq(0)").val(""); }
        if ($(this).attr("class") == "Location") { $(this).find("input:eq(0)").val(""); }
        if ($(this).attr("class") == "IID") { $(this).html(""); }
    });

    //dom创建文本框
    var input = document.createElement("input");
    input.style.width = '80px';
    input.type = "text";
    //input.ondblclick = editCells;
    // var tab = document.getElementById("table1");  //找到这个表格
    var rows = myTable.rows;
    //if (ListNum == ListNums) {

    //    SKUListNums += 1;
    //}
    //else {
    //    ListNums = ListNum;
    //    SKUListNums = 2;
    //}
    //var cell = rows[rowIndex + 1].cells[12];
    //cell.innerHTML = "  <label class='deleteSettledPod labelPointer' data-SKU='" + rows[rowIndex].cells[1].innerText + "'  onclick='Del(this)'>删除</label>"
    //var celllocation = rows[rowIndex + 1].cells[10];
    //celllocation.innerHTML = " <input  role='textbox'aria-haspopup='true' onblur='' style='width: 100%;' class='form-control' type='text'>";//aria-autocomplete='list' value='' name='' 
    //var celllocation8 = rows[rowIndex + 1].cells[11];
    //celllocation8.innerHTML = " <input  role='textbox'aria-haspopup='true' class='form-control Location' style='width: 100%;' type='text'>";//list='" + rows[rowIndex].cells[1].innerText + rows[rowIndex].cells[3].innerText + rows[rowIndex].cells[4].innerText + rows[rowIndex].cells[5].innerText + "' aria-autocomplete='list'  name='' 
    calculateListNum(rows[rowIndex].cells[1].innerText, linenumber);
    //$(Pointer).parent().parent().next().find("input[type='text']").trigger('keydown');
};
function Del(self) {
    //var LineNumber = $(self)[0].dataset.sku;
    //var SKU = $(self).parent().parent().children()[1].innerText.trim();
    //var Type = $(self).parent().parent().children()[5].innerText.trim();
    //var Qty = parseFloat($(self).parent().parent().children()[6].innerText.trim());
    //var history = parseFloat($(self).parent().parent().children()[7].innerText.trim());
    //var ExistingQty = $(self).parent().parent().children()[8].children(0).value;
    //var Location = $(self).parent().parent().children()[9].children(0).value;
    //var BatchNumber = ($(self).parent().parent().children()[3].innerText.trim());
    //var BoxNumber = ($(self).parent().parent().children()[4].innerText.trim());
    $(self).parent().parent().parent().remove();
    //var id = de.dataset.my;
    //calculateListNum(LineNumber);
    //RemainingQty(ExistingQty, SKU, Location, 1, Type, BatchNumber, BoxNumber);

}

//***********************************计算实收数量********************************************、、
//function la(self) {
//    //获取table中一列的值
//    var SKU = $($(self).parent().parent()).children(".SKU")[0].innerText.trim();
//    //$(self).parent().parent().children()[1].innerText.trim();
//    var Type = $($(self).parent().parent()).children(".GoodsType")[0].innerText.trim();
//    //$(self).parent().parent().children()[7].innerText.trim();
//    var Qty = parseFloat($($(self).parent().parent()).children(".OriginalQty")[0].innerText.trim());
//    //var Qty = parseFloat($(self).parent().parent().children()[8].innerText.trim());
//    var history = parseFloat($($(self).parent().parent()).children(".AllocatedQty")[0].innerText.trim());
//    //parseFloat($(self).parent().parent().children()[9].innerText.trim());
//    //var ExistingQty = $(self).parent().parent().children()[8].children(0).value;
//    //var Location = $(self).parent().parent().children()[9].children(0).value;
//    //var BatchNumber = ($(self).parent().parent().children()[3].innerText.trim());
//    //var BoxNumber = ($(self).parent().parent().children()[4].innerText.trim());
//    var t = document.getElementById("content");
//    var rl = t.rows.length;
//    //var QtyReceived = 0;
//    for (i = 0; i < rl; i++) {
//        if ($(t.rows[i]).children(".SKU")[0].innerText == SKU && $(t.rows[i]).children(".GoodsType")[0].innerText.trim() == Type) {
//            history += parseFloat($(t.rows[i]).children(".AllocatedQty").children("input")[0].value.trim() == "" ? 0 : $(t.rows[i]).children(".AllocatedQty").children("input")[0].value.trim());
//            //history += parseFloat(t.rows[i].cells[10].childNodes[1].value.trim() == "" ? 0 : t.rows[i].cells[10].childNodes[1].value.trim());
//        }
//    }
//    for (i = 0; i < rl; i++) {
//        if ($(t.rows[i]).children(".AllocatedQty").children("input")[0].value == "") {
//            $(t.rows[i]).children(".AllocatedQty").children("input")[0].style.border = "1px solid red";
//        } else {
//            $(t.rows[i]).children(".AllocatedQty").children("input")[0].style.border = "";
//        }
//        //if (t.rows[i].cells[10].childNodes[1].value == "") {
//        //    t.rows[i].cells[10].childNodes[1].style.border = "1px solid red";
//        //} else {
//        //    t.rows[i].cells[10].childNodes[1].style.border = "";
//        //}
//    }
//    if (Qty.toFixed(3) < history.toFixed(3)) {
//        $(self)[0].style.border = "1px solid red";
//        showMsg("库存超出！", 2000);
//    }
//    //if (Location != "") {
//    //    RemainingQty(ExistingQty, SKU, Location, 0, Type, BatchNumber, BoxNumber);
//    //}
//}

//*************************************************计算SUK行号****************************************************//
function NumList(num) {
    var result = '';
    if (num < 10) {
        result = '0000' + num;
    } else if (num <= 99 && num >= 10) {
        result = '0000' + num;
    } else if (num <= 999 && num >= 100) {
        result = '000' + num;
    } else if (num <= 9999 && num >= 1000) {
        result = '0' + num;
    }
    return result;
}
function calculateListNum(id,linenumber) {
    //获取table中一列的值
    var t = document.getElementById("resultTable");
    var rl = t.rows.length;
    var num = 0, BreakNum = 0;
    for (i = 0; i < rl; i++) {
        //if (t.rows[i].cells[2].innerText == id) {

        //if (t.rows[i].className == "NewTableBGColor") {
        num++;Del
        //t.rows[i].cells[1].innerText = NumList(num);
        // $(t.rows[i]).children(".LineNumber")[0].innerText = NumList(num);
        //$(t.rows[i]).children(".LineNumber")[0].innerText = linenumber;
        //}
        //}
    }
}
