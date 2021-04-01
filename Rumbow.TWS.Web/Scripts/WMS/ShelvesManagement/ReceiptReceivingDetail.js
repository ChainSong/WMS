$(document).ready(function () {

    //function selectList () {
    //    $.ajax({
    //        url: "/WMS/ShelvesManagement/asdasdadasd",
    //        type: "POST",
    //        data: {
    //            "RID": "740",
    //        },
    //        success: function (data) {
    //            var html = $("#personTemplate").render(data.data);
    //            $("#editTable").html(html);
    //        }
    //    })
    //}
    //selectList();

    // 创建滚动条 
    window.onscroll = function () {
        //var scrollTop = document.body.scrollTop;
        var scrollTop = document.documentElement.scrollTop;
        if (scrollTop >= 400) {
            if ($("#onscrollDIV")[0]!=null && $("#onscrollDIV")[0] != undefined)
            {
                $("#onscrollDIV")[0].style.display = "";
            }
        } else {
            if ($("#onscrollDIV")[0] != null && $("#onscrollDIV")[0] != undefined) {
                $("#onscrollDIV")[0].style.display = "none";
            }            
        }
    }

    var target = 0;
    var d1 = document.getElementById("NewDiv");
    var d2 = document.getElementById("onscroll");
    $("#onscroll").scroll(function () {

        if (target == 1) return;
        target = 2;
        d1.scrollLeft = d2.scrollLeft;
        target = 0;
    })

    $(".Ooperation").on("mouseover", function () {
        $(this).prev()[0].style.display = "none";
    });
    $(".Ooperation").on("mouseenter", function () {
        $(this).prev()[0].style.display = "";
    })
    $(".Adiv").on("mouseleave", function () {
        $(this)[0].style.display = "none";
    })
    $("tr").on("mouseenter", function () {
        $(".Adiv").each(function (a, b) {
            $(b)[0].style.display = "none";
        })
    })
    $(".DelButton").click(function () {
        var self = this;
        var LineNumber = $(self).data().linenumber;
        $(self).parent().parent().parent().remove();
        //var id = de.dataset.my;
        calculateListNum(LineNumber);
    })

    // $(function () {
    //    var table = document.getElementById("resultTable");
    //    var row = table.getElementsByTagName("tr");
    //    for (var j = 1; j < row.length; j++) {
    //        //var location = row[j].cells[9].children[0].value;
    //        var location = $(row[j]).children(".Location").children(".Location")[0].value
    //        $.ajax({
    //            url: "/WMS/InventoryManagement/GetLocationMax",
    //            type: "Get",
    //            data: { location: location, warehouseid: $("#WarehouseID").val() },
    //            async: false,
    //            success: function (data) {
    //                $(row[j]).children(".RemainingNum").children("input")[0].value = data;
    //                //row[j].cells[16].children[0].value = data;
    //            },
    //            error: function (msg) {
    //                alert(msg);
    //            }
    //        });
    //    }
    //});
    var ListNums = 0;;
    var SKUListNums = 0;

    $(".AddButton").click(function () {
        var self = this;
        var ListNum = $(self).data().linenumber;
        // var html = $("#Operation").render("");
        //$("#chaifen")["empty"]();
        //$("#chaifen").parent().parent().append(html);
        var myTable = document.getElementById("resultTable");
        var rowIndex = event.srcElement.parentNode.parentNode.parentNode.rowIndex;//当前行的行号
        var obj = document.getElementById('resultTable').getElementsByTagName('tr');
        var newrow = obj[rowIndex].cloneNode(true);
        // var col = document.getElementsByTagName('tr'), i, td;
        // td = col[rowIndex].getElementsByTagName('td')[0].innerHTML("");
        //这个td就是tr首个td 
        newrow.setAttribute("class", "NewTableBGColor");
        //  newrow.ondblclick = editCell;
        //alert(newrow);
        $(self).parent().parent().parent().after(newrow);
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
        var cell = rows[rowIndex + 1].cells[0];
        cell.innerHTML = "  <div class='Adiv' style='position: absolute; display: none; width: 500px; left: 50px; height: 31px;'> \
                                    <label style='cursor: pointer;' class=' btn btn-primary btn-xs DelButton'  data-LineNumber=" + rows[rowIndex].cells[1].innerText + " )'>删除</label>\
                                </div>\
                                <label style='cursor: pointer;' class='btn btn-primary btn-xs Ooperation'>操作</label>"
        //<label class='deleteSettledPod labelPointer'  onclick='Del(this," + rows[rowIndex].cells[1].innerText + ")'>删除</label>"
        $(rows[rowIndex + 1]).children(".QtyReceived").children("input")[0].value = 0;

        //var celllocation = $(rows[rowIndex + 1]).children('.Location');//.cells[10];
        //celllocation.innerHTML = "  <input type='text' class='Location form-control' style='width: 120px;' placeholder='' value='' autofocus/>"
        ////" <input  role='textbox'aria-haspopup='true' onblur='la(" + rows[rowIndex].cells[1].innerText + "," + rows[rowIndex].cells[6].innerText + ")' aria-autocomplete='list' value='0.00' name='warehouse' style='width: 80px;' type='text'>";
        calculateListNum(rows[rowIndex].cells[1].innerText);
        //$(Pointer).parent().parent().next().find("input[type='text']").trigger('keydown');
    })

    $("#intelligentDispatch").live('click', function () {
        openPopup("", true, 350, 300, null, 'intelligentDispatchPanel', true);
    });

    $("#intelligentDispatchRT").live('click', function () {
        closePopup();
    });
    $("#intelligentDispatchOK").live('click', function () {       
        if ($('#WorkStation').val() == "") {
            showMsg("请选择操作台", 4000);
            return;
        }
        $.ajax({
            type: "post",
            url: "/WMS/ShelvesManagement/AddInstructions",
            data: {
                "ids": $("#RID").val(),
                "WorkStation": $('#WorkStation').val(),
                "WarehouseQueue": "",
                "Priority": $('#Priority').val()
            },
            //async: "false",
            success: function (data) {
                if (data.Code == "1") {
                    closePopup();
                    showMsg("发送成功！", 4000);
                    //location.href = "/WMS/OrderManagement/Index"
                }
                else {
                    showMsg("发送失败！" + data.Message, 4000);
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });


    });



    $("#SubmitOK").click(function () {

        layer.confirm('<font size="4">确认是否提交？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var a = validationTableToJson("resultTable", 0);
            if (!a) {
                showMsg("请检查输入是否合法！", 4000)
                return false;
            }

            WebPortal.MessageMask.Show("正在保存...");

            var Jaonstr = TableToJson("resultTable", 0);
            var table = document.getElementById("resultTable");
            var row = table.getElementsByTagName("tr");
            //for (var j = 1; j < row.length; j++) {
            //    var Location1 = $(row[j]).children(".Location").children("input").val();

            //    var Qty1 = $(row[j]).children(".Qty").children("input").val();//row[j].cells[7].children[0].value;

            //    for (var i = j + 1; i < row.length; i++) {
            //        var Location2 = $(row[i]).children(".Location").children("input").val(); //row[i].cells[10].children[0].value;
            //        var Qty2 = $(row[i]).children(".Qty").children("input").val();//.cells[7].children[0].value;
            //        if (Location1 == Location2) {
            //            Qty1 = parseFloat(Qty1) + parseFloat(Qty2)
            //        }
            //    }
            //    if ((parseFloat(Qty1).toFixed(2) - parseFloat($(row[j]).children(".RemainingNum").children("input").val()) > 0)) {//.cells[14].children[0].value)
            //        showMsg("保存失败，库位" + Location1 + "超出最大库存", 4000);
            //        return;
            //    }
            //}
            $.ajax({
                url: "/WMS/ShelvesManagement/StagingReceipt",
                dataType:"json",
                type: "POST",
                data: {
                    "CustomerID": $("#CustomerID").val(),
                    "Jaonstr": Jaonstr
                },
                async: "false",//同步
                success: function (data) {
                    WebPortal.MessageMask.Close();
                    if (data.IsSuccess == true) {
                        //post('/WMS/ShelvesManagement/Index', { ShelvesModel: null, Action: '查询' });
                        //多层级页面跳转，准确跳回所在菜单主页
                        var url = $(window.parent.document).find(".active a").attr('href');
                        url = url.toString().split(',')[2];
                        url = url.substring(1, url.length - 2);
                        location.href = url;
                    } else {
                        //showMsg("操作失败！", 4000);
                        showMsg(data.result, 8000);
                    }
                },
                error: function (e) {
                    WebPortal.MessageMask.Close();
                    showMsg("操作失败！", 4000);
                }
            })
        });
    })

    function post(URL, PARAMS) {
        var temp = document.createElement("form");
        temp.action = URL;
        temp.method = "post";
        temp.style.display = "none";
        for (var x in PARAMS) {
            var opt = document.createElement("textarea");
            opt.name = x;
            opt.value = PARAMS[x];
            // alert(opt.name)        
            temp.appendChild(opt);
        }
        document.body.appendChild(temp);
        temp.submit();
        return temp;
    }

    ///已注释
    $("#AddInventory").click(function () {
        //if (validation("AddInventory")) {
        //    showMsg("实收数量和预计数量不等！", "4000");
        //    return false;
        //}
        //
        if (AllAreaLocationNoNull()) {
            //showMsg("库位不能为空!", "4000");
            showMsg("库区库位格式不正确!", "4000");
            return false;
        }
        layer.confirm('<font size="4">确认是否加入库存？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);

            if (AllAreaLocationNoNull()) {
                showMsg("库区或库位不能为空或不存在！", "4000");
                return false;
            }
            var str = TableToJson("resultTable", 1)
            if (!str) {
                showMsg("请检查输入是否合法！", 4000)
                return false;
            }
            this.disabled = true;
            
            $.ajax({
                url: "/WMS/ShelvesManagement/AddshelvesAndInventory",
                type: "POST",
                data: {
                    "Jaonstr": str
                },
                success: function (data) {
                    if (data == "True") {
                        //showMsg("操作成功！", 4000);
                        document.getElementById('SubmitOK').disabled = true;
                        post('/WMS/ShelvesManagement/Index', { ShelvesModel: null, Action: '查询' });
                        this.disabled = false;
                    } else {
                        this.disabled = false;
                        showMsg("操作失败！", 4000);
                    }
                },
                error: function () {
                    this.disabled = false;
                    showMsg("操作失败！", 4000);
                }
            })
        });
    })

    $('#searchButton').click(function () {
        setPageControlVal();
    });
    //***********************************************时间控件绑定*******************************************//
    //$('.calendarRange').each(function (index) {
    //    var id = $(this).attr('id');
    //    var pref = id.split('_')[0];
    //    var actualID = id.split('_')[1];
    //    var descID = 'SearchCondition_';
    //    if (pref === 'start') {
    //        descID += 'StartTime';
    //    }
    //    else {
    //        descID += 'Endtime';
    //    }
    //    $(this).val($('#' + descID).val());
    //});
    //var setPageControlVal = function () {
    //    $('.calendarRange').each(function (index) {
    //        var id = $(this).attr('id');
    //        var pref = id.split('_')[0];
    //        var actualID = id.split('_')[1];
    //        var descID = 'SearchCondition_';
    //        if (pref === 'start') {
    //            descID += 'StartTime';
    //        }
    //        else {
    //            descID += 'Endtime';
    //        }
    //        $('#' + descID).val($(this).val());
    //    });
    //}
    //*******************************************判断库区库位选择是否正确*****************************************//
    $(".Area").on("blur", function () {
        var self = this;
        $.ajax({
            url: "/WMS/ShelvesManagement/ChangeArea",
            type: "POST",
            dataType: "json",
            data: {
                id: $(this).parent().parent()[0].children[0].innerText,
                name: this.value,
                type: "blur"
            },
            success: function (data) {
                if (data.length == 0) {
                    self.style.borderColor = '#FF0000';
                } else {
                    self.style.borderColor = '';
                }

            }
        });
    })


    $(".Area").on("keydown", function () {
        $('.Area').autocomplete({
            source: function (request, response) {

                $.ajax({
                    url: "/WMS/ShelvesManagement/ChangeArea",
                    type: "POST",
                    dataType: "json",
                    data: {
                        id: $(this.element[0]).parent().parent()[0].children[0].innerText,
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

    //$(".Location").live("blur", function () {
    //    var self = this;
    //    if (this.value == "") {
    //        self.style.borderColor = '#FF0000';
    //    } else {

    //        if (this.value != undefined && this.value.split("|").length > 1) {
    //            //$.ajax({
    //            //    url: "/WMS/ShelvesManagement/ChangeLocation",
    //            //    type: "POST",
    //            //    dataType: "json",
    //            //    data: {
    //            //        id: $($(this).parent().parent()).children(".WarehouseID")[0].innerHTML,
    //            //        AreaName: this.value.split("|")[0].trim(),
    //            //        name: this.value.split("|")[1].trim(),
    //            //        type: "blur"
    //            //    },
    //            //    success: function (data) {
    //            //        if (data.length == 0) {
    //            //            self.style.borderColor = '#FF0000';
    ////        } else {
    //            //            self.style.borderColor = '';
    ////        }

    //            //    },
    //            //    error: function (data, status, e) {
    //            //        self.style.borderColor = '#FF0000';
    ////    }
    //            //});
    //        } else {
    //            self.style.borderColor = '#FF0000';
    //        }
    //    }
    //})
    $(".Location").on("keydown", function () {
        var left = this;
        $('.Location').autocomplete({
            minLength: 4,
            source: function (request, response) {

                $.ajax({
                    url: "/WMS/InventoryManagement/GetLocationList",
                    type: "POST",
                    dataType: "json",
                    data: {
                        location: request.term,
                        warehouseid: $("#WarehouseID").val(),
                        //$(this.element[0]).parent().parent()[0].children[0].innerText,
                        areaid: 0
                    },
                    //data: {
                    //    id: $(this.element[0]).parent().parent()[0].children[0].innerText,
                    //    AreaName: $(this.element[0]).parent().parent()[0].children[8].children[0].value,
                    //    name: request.term,
                    //    type: "keydown"
                    //},
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.Text, value: item.Text, data: item }
                        }));
                    }
                });
            },
            select: function (event, ui) {
                var ss = $(left).parent().children(".RemainingNum").children("input");
                $.ajax({
                    url: "/WMS/InventoryManagement/GetLocationMax",
                    type: "Get",
                    data: { location: ui.item.data.Text, warehouseid: $("#WarehouseID").val() },

                    success: function (data) {
                        ss[0].value = data;
                    },
                    error: function (msg) {
                        alert(msg);
                    }
                });
            }

        });
    }).trigger('keydown');
})

var box = {
    仓库ID: 'WarehouseID',
    收货单行号: 'LineNumber', 产品编码: 'SKU', 产品行号: 'SKULineNumber',
    货品名称: 'GoodsName', 货品等级: 'GoodsType', 条码: 'UPC',
    期望数量: 'QtyExpected', 实际数量: 'QtyReceived', 库区: 'Area',
    库位: 'Location', 备注: 'Remark', 客户ID: 'CustomerID', "客户/供应商": 'CustomerName', 批次号: 'BatchNumber',
    收货单号: 'ReceiptNumber', 外部单号: 'ExternReceiptNumber', ASNID: 'ASNID', ASNNumber: 'ASNNumber',
    RDID: 'RDID', RID: 'RID', 仓库: 'Warehouse', 托号: 'BoxNumber', 生产日期: 'DateTime1', 单位: 'Unit', 规格: 'Specifications'
};
//***********************************************TableToJson******************************************//
function TableToJson(tableid, type) {
    var txt = "[";
    var table = document.getElementById(tableid);
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 1; i < col.length; i++) {

                var tds = row[j].getElementsByTagName("td");
                var innerVal = '';
            if ($(tds[i])[0].className != "RemainingNum") {
                if ($(tds[i]).children("input").length > 0) {
                    //if ($(tds[i]).children("input")[0].value)
                    if ($(tds[i])[0].className.indexOf("Location") < 0) {
                        Numval = $(tds[i]).children("input")[0].value;
                            innerVal = Numval;
                    } else {
                        Numval = $(tds[i]).children("input")[0].value;
                        if (Numval.length > 3) {
                            r += "\"" + box["库区"] + "\"\:\"" + Numval.split("|")[0] + "\",";
                            r += "\"" + box["库位"] + "\"\:\"" + Numval.split("|")[1] + "\",";
                            continue;
                        } else {
                            r += "\"" + box["库区"] + "\"\:\"" + "\",";
                            r += "\"" + box["库位"] + "\"\:\"" + "\",";
                            continue;
                    }
                    }
                } else if ($(tds[i]).children("select").length > 0) {
                    //innerVal = "\"" + box[col[i].innerText.trim()] + "\"\:\"" + innerVal + "\",";
                    innerVal = $(tds[i]).find("option:selected")[0].value;
                }
                else {
                    innerVal = tds[i].innerText.trim();
                }
                r += "\"" + box[col[i].innerText.trim()] + "\"\:\"" + innerVal + "\",";
            }
            //if (type == 0) {
            //    if (row[j].className == 'NewTableBGColor') {
            //        var tdsval = innerVal == "" ? 0 : innerVal;
            //        switch (i) {
            //            case 7:
            //                tds[7].innerHTML = " <input style='width: 80px;' onblur=la(" + tdsval + ",this) data-name=" + tdsval + " type='text' value='" + tdsval + "'>";
            //                break;
            //                //case 8:
            //                //    tds[8].innerHTML = " <input  class='ui-autocomplete-input Area'  role='textbox'aria-haspopup='true' aria-autocomplete='list'style='width: 80px;' type='text'value='" + innerVal + "'>";
            //                //    break;
            //            case 10:
            //                tds[10].innerHTML = " <input  class='ui-autocomplete-input Shelves'  role='textbox'aria-haspopup='true' aria-autocomplete='list' name='warehouse'  style='width: 120px;' type='text' value='" + innerVal + "'>";
            //                break;
            //            case 11:
            //                tds[11].innerHTML = " <input  class='Location ui-autocomplete-input' style='width: 120px;' type='text' value='" + innerVal + "'>";
            //                break;
            //            case 12:
            //                tds[12].innerHTML = " <input  class='Location ui-autocomplete-input' style='width: 120px;' type='text' value='" + innerVal + "'>";
            //                break;
            //        }
            //    }
            //} else {
            //    $(tds).children(".OoperationTD").style.display = "none";
            //    col[14].style.display = "none";
            //    switch (i) {
            //        case 7:
            //            tds[7].innerText = tds[i].childNodes[1].value;
            //            break;
            //            //case 8:
            //            //    tds[8].innerHTML = innerVal;
            //            //    break;
            //        case 10:
            //            tds[10].innerHTML = innerVal;
            //            break;
            //        case 11:
            //            tds[11].innerHTML = innerVal;
            //            break;
            //        case 12:
            //            tds[12].innerHTML = innerVal;
            //            break;
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

//*******************************************************双击弹出库位选择*******************************************//
$(".Location").on('dblclick', function () {
    var obj = this;
    var ss = $($(this).parent().parent()).children(".RemainingNum").children("input")[0];
    //.next().next().next().next()[0];
    openPopup("LocationPop", true, 1000, 600, '/WMS/Warehouse/IndexLocation/?flag=1', null, function (Location) {
        $(obj).val(Location);
        $.ajax({
            url: "/WMS/InventoryManagement/GetLocationMax",
            type: "Get",
            data: { location: Location, warehouseid: $("#WarehouseID").val() },

            success: function (data) {
                ss.value = data;
            },
            error: function (msg) {
                alert(msg);
            }
        });
    });
    $("#popupLayer_LocationPop")[0].style.top = "50px";
});


//*******************************************************验证*******************************************//
function validationTableToJson(tableid) {
    var txtBool = true;
    var table = document.getElementById(tableid);
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {

        for (var i = 1; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if ($(tds[i])[0].className == "QtyReceived") {
                if (isNaN(tds[i].childNodes[1].value) && tds[i].childNodes[1].value == 0) {
                    txtBool = false;
                    return txtBool;
                }
                }
                        }
                    }
    return txtBool;
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





//function BreakUp(Pointer, ListNum) {
//    // var html = $("#Operation").render("");
//    //$("#chaifen")["empty"]();
//    //$("#chaifen").parent().parent().append(html);
//    var myTable = document.getElementById("resultTable");
//    var rowIndex = event.srcElement.parentNode.parentNode.parentNode.rowIndex;//当前行的行号
//    var obj = document.getElementById('resultTable').getElementsByTagName('tr');
//    var newrow = obj[rowIndex].cloneNode(true);
//    // var col = document.getElementsByTagName('tr'), i, td;
//    // td = col[rowIndex].getElementsByTagName('td')[0].innerHTML("");
//    //这个td就是tr首个td 
//    newrow.setAttribute("class", "NewTableBGColor");
//    //  newrow.ondblclick = editCell;
//    //alert(newrow);
//    $(Pointer).parent().parent().parent().after(newrow);
//    //dom创建文本框
//    var input = document.createElement("input");
//    input.style.width = '80px';
//    input.type = "text";
//    //input.ondblclick = editCells;
//    // var tab = document.getElementById("table1");  //找到这个表格
//    var rows = myTable.rows;
//    if (ListNum == ListNums) {

//        SKUListNums += 1;
//    }
//    else {
//        ListNums = ListNum;
//        SKUListNums = 2;
//    }
//    var cell = rows[rowIndex + 1].cells[0];
//    cell.innerHTML = "  <div class='Adiv' style='position: absolute; display: none; width: 500px; left: 50px; height: 31px;'> \
//                                    <label style='cursor: pointer;' class=' btn btn-primary btn-xs DelButton'  data-LineNumber=" + rows[rowIndex].cells[1].innerText + " )'>删除</label>\
//                                </div>\
//                                <label style='cursor: pointer;' class='btn btn-primary btn-xs Ooperation'>操作</label>"




//    //<label class='deleteSettledPod labelPointer'  onclick='Del(this," + rows[rowIndex].cells[1].innerText + ")'>删除</label>"
//    $(rows[rowIndex + 1]).children(".QtyReceived").children("input")[0].value = 0;

//    //var celllocation = $(rows[rowIndex + 1]).children('.Location');//.cells[10];
//    //celllocation.innerHTML = "  <input type='text' class='Location form-control' style='width: 120px;' placeholder='' value='' autofocus/>"
//    ////" <input  role='textbox'aria-haspopup='true' onblur='la(" + rows[rowIndex].cells[1].innerText + "," + rows[rowIndex].cells[6].innerText + ")' aria-autocomplete='list' value='0.00' name='warehouse' style='width: 80px;' type='text'>";
//    calculateListNum(rows[rowIndex].cells[1].innerText);
//    //$(Pointer).parent().parent().next().find("input[type='text']").trigger('keydown');
//};
//function subtraction(a, b) {
//    if (a > b) {
//        return a - b;
//    } else {
//        return 0;
//    }
//}
//function onAddTR(trObj, html) {
//    $(trObj).parent().parent().after(html);
//}
//function Del(de, LineNumber) {
//$(de).parent().parent().parent().remove();
////var id = de.dataset.my;
//calculateListNum(LineNumber);
//}



//*********************************主要是限制预收数量是否等于实际数量*******************************//
//function validation(Parameters) {
//    //获取table中一列的值
//    var t = document.getElementById("editTable");
//    var rl = t.rows.length;
//    var QtyReceived = 0, QtyExpected = 0;

//    for (i = 0; i < rl; i++) {
//        if (t.rows[i].className != "NewTableBGColor") {

//            QtyExpected += parseInt(t.rows[i].cells[6].innerText);
//        }
//        QtyReceived += parseInt(t.rows[i].cells[7].childNodes[1].value);
//        //t.rows[i].cells[7].childNodes[1].style.border = "1px solid #aaa";
//    }
//    if (Parameters == "AddInventory") {
//        return QtyReceived == QtyExpected ? false : true;
//    } else {
//        return QtyReceived > QtyExpected ? true : false;
//    }

//}
//************************************判断库区和库位不为空**********************************************//
function AllAreaLocationNoNull() {
    var t = document.getElementById("resultTable");
    var row = t.getElementsByTagName("tr");
    var rl = t.rows.length
    for (i = 1; i < rl; i++) {

        //if (t.rows[i].className == "NewTableBGColor") {
        if ($(row[i]).children(".Location").children("input").val() == "" || $(row[i]).children(".Location").children("input").val().indexOf('|') == -1) {
            return true;
        }
        //} else if ($(row[i]).children(".Location").children("input").style.borderColor == "rgb(255, 0, 0)") {
        //    return true;
        //}
    }
    return false;
}
//***********************************计算实收数量********************************************
function la(id, QtyExpected, BatchNumber, BoxNumber) {
    //获取table中一列的值
    var t = document.getElementById("resultTable");
    var rl = t.rows.length;
    var QtyReceived = 0;
    var a = this;
    for (i = 1; i < rl; i++) {
        if (t.rows[i].cells[1].innerText == id && t.rows[i].cells[11].children[0].value == BatchNumber && t.rows[i].cells[10].children[0].value == BoxNumber) {
            //if (t.rows[i].className == "NewTableBGColor") {
            if (t.rows[i].cells[7].childNodes[1].value == "") {
                t.rows[i].cells[7].childNodes[1].style.border = "1px solid red";
                //t.rows[i].cells[13].childNodes.css("border-color", "red");//.css("NullTableBGColor");
            } else {
                if (isNaN(t.rows[i].cells[7].childNodes[1].value)) {
                    showMsg("请输入数字！", 4000);
                }
                QtyReceived += parseInt(t.rows[i].cells[7].childNodes[1].value);

                //t.rows[i].cells[7].childNodes[1].style.border = "1px solid #aaa";
            }
        }
    }
    for (i = 1; i < rl; i++) {
        if (t.rows[i].cells[1].innerText == id && t.rows[i].cells[11].children[0].value == BatchNumber && t.rows[i].cells[12].children[0].value == BoxNumber) {
            if (QtyReceived != QtyExpected) {
                t.rows[i].cells[7].childNodes[1].style.border = "1px solid red";
            } else {
                t.rows[i].cells[7].childNodes[1].style.border = "";
                //if (parseInt(t.rows[i].cells[7].childNodes[1].value) == 0) {
                //    t.rows[i].cells[7].childNodes[1].style.border = "2px solid red";
                //}
            }
        }

    }
    }
//var fileImportClick = function () {
//    if ($('#importExcel').val() === '') {
//        Runbow.TWS.Alert("请选择要导入的Excel");
//        return false;
//    }
//    var exp = /.xls$|.xlsx$/;
//    var fileImport = $('#fileImport').clone();
//    if (exp.exec($('#importExcel').val()) == null) {
//        Runbow.TWS.Alert("请选择Excel格式的文件");
//        $('#importExcel').remove();
//        $(this).before(fileImport);
//        return false;
//    };
//    WebPortal.MessageMask.Show("导入中...");

//    $.ajaxFileUpload({
//        url: '/System/QuotedPrice/ExeclQuotedPrice',
//        secureuri: false,
//        fileElementId: 'importExcel',
//        dataType: 'json',
//        data: {},
//        success: function (data, status) {
//            WebPortal.MessageMask.Close();
//            Runbow.TWS.Alert('导入成功');
//        },
//        error: function (data, status, e) {
//            Runbow.TWS.Alert('导入失败');
//            WebPortal.MessageMask.Close();
//        }
//    });
//};
//*************************************************计算SKU行号****************************************************//
function NumList(num) {
    //function GenerateID(str) {
    var pad = "00000";
    return pad.substring(0, pad.length - num.toString().length) + num
    //}
    //var result = '';
    //if (num < 10) {
    //    result = '00' + num;
    //} else if (num <= 99 && num >= 10) {
    //    result = '0' + num;
    //} else if (num <= 999 && num >= 100) {
    //    result = num;
    //}

    //return result;
}

//function ReturnLineNumber(row_count) {
//    var linnumber = "";
//    if (row_count < 10) {
//        linnumber = "0000" + row_count;
//    }
//    if (100 > row_count && row_count >= 10) {
//        linnumber = "000" + row_count;
//    }
//    if (1000 > row_count && row_count >= 100) {
//        linnumber = "00" + row_count;
//    }
//    if (row_count >= 1000) {
//        linnumber = "0" + row_count;
//    }
//    return linnumber;
//}
function calculateListNum(id) {
    //获取table中一列的值
    var t = document.getElementById("resultTable");
    var rl = t.rows.length;
    var num = 1, BreakNum = 0;

    for (i = 1; i < rl; i++) {
        if (t.rows[i].cells[1].innerText == id) {

            if (t.rows[i].className == "NewTableBGColor") {
                num++;
                t.rows[i].cells[3].innerText = NumList(num);
            }
        }
    }
}

//function CheckLocationMax() {      
//    var table = document.getElementById("resultTable");
//    var row = table.getElementsByTagName("tr");
//    for (var j = 1; j < row.length; j++) {
//        var Location1 = row[j].cells[10].children[0].value;

//        var Qty1 = row[j].cells[7].children[0].value;

//        for (var i = j + 1; i < row.length - 1; i++)
//        {
//            var Location2 = row[j].cells[10].children[0].value;
//            var Qty2 = row[j].cells[7].children[0].value;
//            if (Location1 == Location2)
//            {
//                Qty1 = parseFloat(Qty1) + (Qty2)
//            }
//        }
//        if (parseFloat(Qty1).toFixed(2) < parseFloat(row[j].cells[14].children[0].value).toFixed(2))
//        {
//            showMsg("库位" + Location1+"超出最大库存", 4000);
//            return;
//        }
//    }
//}