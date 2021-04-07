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

    $('#ASNNumbers').val("系统自动生成");
    $('#Status').val("新增");

    $('#returnButton').live('click', function () {
        //history.back();
        if ($("#ReturnViewType").val() == "get") {
            history.back();
        } else {
            //history.back();
            post('/WMS/ASNManagementSF/Index', { IndexViewModel: null, Action: '查询' });
        }
        //post('/WMS/ASNManagementSF/Index', { IndexViewModel: null, Action: '查询' });
    })

    $('#returnButtons').live('click', function () {
        if ($("#ReturnViewType").val() == "get") {
            //history.back();
            //post('/WMS/ASNManagementSF/Index', { IndexViewModel: null, Action: '查询' });
            var url = $(window.parent.document).find(".active a").attr('href');
            url = url.toString().split(',')[2];
            url = url.substring(1, url.length - 2);
            location.href = url;
        } else {
            //history.back();
            //post('/WMS/ASNManagementSF/Index', { IndexViewModel: null, Action: '查询' });
            var url = $(window.parent.document).find(".active a").attr('href');
            url = url.toString().split(',')[2];
            url = url.substring(1, url.length - 2);
            location.href = url;
        }
        //post('/WMS/ASNManagementSF/Index', { IndexViewModel: null, Action: '查询' });
    })

    $('#returnButton_Edit').live('click', function () {

        if ($("#ReturnViewType").val() == "get") {
            //history.back();
            post('/WMS/ASNManagementSF/Index', { IndexViewModel: null, Action: '查询' });
        } else {
            //history.back();
            post('/WMS/ASNManagementSF/Index', { IndexViewModel: null, Action: '查询' });
        }
    })

    $('#returnButton_Add').live('click', function () {
        if ($("#ReturnViewType").val() == "get") {
            history.back();
        } else {
            history.back();
        }
    })

    if ($("#Viewtypes").val() == "1") {
        addNew();
        //var tr = document.getElementById('Newtable').getElementsByTagName('tr');
        //var UnitNum = tr[1].cells[6].children[0];
        //var SpecificationsNum = tr[1].cells[7].children[0];
        //var UnitList=$("#UnitLists");
        //var SpecificationsLists=$("#SpecificationsLists");
        //for (var i = 0; i < UnitList[0].length; i++)
        //{
        //    var opt = new Option(UnitList[0][i].innerText, UnitList[0][i].innerText);
        //    UnitNum.options.add(opt);
        //}
        //for (var i = 0; i < SpecificationsLists[0].length; i++) {
        //    var opt = new Option(SpecificationsLists[0][i].innerText, SpecificationsLists[0][i].innerText);
        //    SpecificationsNum.options.add(opt);
        //}
    }
    else if ($("#Viewtypes").val() == "0" || $("#Viewtypes").val() == "3") {
        $("#NewDiv").removeAttr("style");
        $("#Newtable").removeAttr("style");
    }

    $("select[name='Unit']").live('change', function () {
        var Units = $(this)[0].value;
        var UnitAndSpecifications = $(this).parent().next()[0].children.Specifications;
        var UnitAndSpecificationsList = $("#UnitAndSpecificationsList");
        UnitAndSpecifications.length = 0;
        for (var i = 0; i < UnitAndSpecificationsList[0].length; i++) {
            if (UnitAndSpecificationsList[0][i].value == Units) {
                var opt = new Option(UnitAndSpecificationsList[0][i].text, UnitAndSpecificationsList[0][i].text);
                UnitAndSpecifications.options.add(opt);
            }
        }
    });

    $(".skuChildren").live('dblclick', function () {
        var obj = this;
        openPopup("AsnPop", true, 1000, 600, '/WMS/Product/index/?flag=1&customerID=' + $("#CustomerID").val(), null, function (Sku, GoodsName, UPC) {
            var rows = $('#Newtable')[0].rows.length;
            var rowIndex = $(obj).parent().parent().prevAll().length + 1;
            //for (var i = 1; i < rows; i++) {
            //    if (i != rowIndex) {
            //        var skus = $('#Newtable')[0].rows[i].cells[1].children.sku.value;
            //        if (skus == Sku) {
            //            showMsg("SKU已存在!", "4000");
            //            $(obj)[0].value = '';
            //            $(obj)[0].focus();
            //            return false;
            //        }
            //    }
            //}
            $(obj).val(Sku);
            $($(obj).parent().parent()).children(".GoodsName").children("input")[0].value = GoodsName;
            //$('#goodsname' + LineNumber).val(GoodsName);
            //$($(obj).parent().parent()).children("UPC").children("input")[0].value = UPC;
            //$('#UPC' + LineNumber).val(UPC);
        });
        $("#popupLayer_AsnPop")[0].style.top = "50px";
    });

    $(".UPCChildren").live('dblclick', function () {
        var obj = this;
        openPopup("AsnPop", true, 1000, 600, '/WMS/Product/index/?flag=1&customerID=' + $("#CustomerID").val(), null, function (Sku, GoodsName, UPC) {
            var rows = $('#Newtable')[0].rows.length;
            var rowIndex = $(obj).parent().parent().prevAll().length + 1;
            //for (var i = 1; i < rows; i++) {
            //    if (i != rowIndex) {
            //        var skus = $('#Newtable')[0].rows[i].cells[1].children.sku.value;
            //        if (skus == Sku) {
            //            showMsg("SKU已存在!", "4000");
            //            $(obj)[0].value = '';
            //            $(obj)[0].focus();
            //            return false;
            //        }
            //    }
            //}
            $(obj).val(UPC);
            $($(obj).parent().parent()).children(".GoodsName").children("input")[0].value = GoodsName;
            //$('#goodsname' + LineNumber).val(GoodsName);
            $($(obj).parent().parent()).children(".SKU").children("input")[0].value = Sku;
            //$('#UPC' + LineNumber).val(UPC);
        });
        $("#popupLayer_AsnPop")[0].style.top = "50px";
    });

    //$(".skuChildren").autocomplete({
    //    source: function (request, response) {
    //        if (request.term.length > 5) {
    //            $.ajax({
    //                url: "/WMS/Product/GetSKUlist",
    //                type: "POST",
    //                dataType: "json",
    //                data: { sku: request.term, CustomerID: $("select[id=CustomerID]")[0].value },
    //                success: function (data) {
    //                    response($.map(data, function (item) {
    //                        return { label: item.Text, value: item.Text, data: item }
    //                    }));
    //                },
    //                error: function (msg) {
    //                    showMsg("未找到", "4000");
    //                }
    //            })
    //        };
    //    },
    //    select: function (event, ui) {
    //        $(this).val(ui.item.data.Text);
    //        $($(this).parent().parent()).children(".GoodsName").children("input")[0].value = ui.item.data.Value;
    //        $($(this).parent().parent()).children(".QtyExpected").children("input")[0].focus();
    //        //$('#goodsname' + LineNumber).val(ui.item.data.Value);
    //        //$('.countsum' + LineNumber).focus();
    //        //$('.goodstype' + LineNumber).val(ui.item.data.GoodsType);
    //        //var rows = $('#Newtable')[0].rows.length;
    //        //var rowIndex = $(this).parent().parent().prevAll().length + 1;
    //        //for (var i = 1; i < rows; i++) {
    //        //    if (i != rowIndex) {
    //        //        var skus = $('#Newtable')[0].rows[i].cells[1].children.sku.value;
    //        //        if (skus == ui.item.data.Text) {
    //        //            showMsg("SKU已存在!", "4000");
    //        //            $(this)[0].value = '';
    //        //            return false;
    //        //        }
    //        //    }
    //        //}
    //        //skulist.push(ui.item.data.Text);
    //        //$('#goodsname' + LineNumber).val(ui.item.data.Value);
    //        //$('.countsum' + LineNumber).focus();
    //    }
    //})
    //$(".skuChildren").live('KeyPress', function () {
    //    if (event.keyCode == 13) {
    //        $.ajax({
    //            url: "/WMS/Product/GetSKUlists",
    //            type: "POST",
    //            dataType: "json",
    //            data: { sku: request.term, CustomerID: $("select[id=CustomerID]")[0].value },
    //            success: function (data) {
    //                $(this).val(data[0].Sku);
    //                $($(this).parent().parent()).children(".GoodsName").children("input")[0].value = ui.item.data.Value;
    //                $($(this).parent().parent()).children(".QtyExpected").children("input")[0].focus();
    //            },
    //            error: function (msg) {
    //                showMsg("未找到", "4000");
    //            }
    //        })
    //    }
    //});
})

var row_count = 1;
//得到明细中的总条数
var asndetailcount = 0;
var skulist = new Array();

function GetSkuList(obj) {
    if (event.keyCode == 13) {
        $.ajax({
            url: "/WMS/Product/GetSKUlists",
            type: "POST",
            dataType: "json",
            data: { sku: $(obj).val(), CustomerID: $("select[id=CustomerID]")[0].value },
            success: function (data) {
                if (data.length > 0) {
                    $(obj).val(data[0].Value);
                    $($(obj).parent().parent()).children(".GoodsName").children("input")[0].value = data[0].Text;
                    $($(obj).parent().parent()).children(".QtyExpected").children("input")[0].focus();
                }
                else {
                    showMsg("未找到该SKU!", "4000");
                    $(obj).focus();
                    $(obj).select();
                }
            },
            error: function (msg) {
                showMsg("未找到", "4000");
            }
        })
    }
}

function addNew() {
    var table = $('#Newtable');
    var rownumber = $('#Newtable tr').not('#trDemo').length;
    var trRow = $('#trDemo').clone(true, true);
    $(trRow)[0].style.display = "";
    $(trRow).removeAttr('id');
    var LineNumber = ReturnLineNumber(rownumber);
    $(trRow).children('.LineNumber')[0].innerText = LineNumber;
    table.append(trRow);

    row_count++;
    //var table1 = $('#Newtable');
    ////if (table1.find("tr").length > 1) {
    ////    var lastTr = table1.find('tbody>tr:last>td:first').children()[0].innerText;
    ////    row_count = parseInt(lastTr) + 1;
    ////}
    //var rownumber = $('#Newtable tr').length;
    //var firstTr = table1.find('tbody>tr:first');
    //var LineNumber = ReturnLineNumber(rownumber);
    //var td0 = $(" <td style='position: relative'>\
    //                <div class='Adiv' style='position: absolute; display: none; width: 500px; left: 50px; height: 31px;'>\
    //                    <label id='labelRemove' style='cursor: pointer;' class='btn btn-primary btn-xs   labelRemove'>删除</label>\
    //                    <label style='cursor: pointer;' class='btn btn-primary btn-xs   addNew' onclick='addNew(this)'>新增一行</label>\
    //                </div>\
    //                <label style='cursor: pointer;' class='btn btn-primary btn-xs Ooperation'>操作</label>\
    //            </td>");
    //var row = $("<tr id=Row" + rownumber + "></tr>");
    //var td1 = $("<td class='LineNumber'></td>");
    //var td2 = $("<td class='SKU'></td>");
    //var td3 = $("<td class='GoodsName'></td>");
    //// var td4 = $("<td></td>");
    //var td4 = $("<td class='UPC'></td>");
    //var td = "td";
    //var td5 = $("<td class='QtyExpected'></td>");
    //var td6 = $("<td  style='position: absolute;left: 86%;width: 120px;height:44px'></td>");
    //td1.append($("<b>" + LineNumber + "</b>"));
    //td2.append($("<input type='text' onkeydown='GetSkuList(this)' id='sku" + LineNumber + "'   style='width:180px'  class='form-control skuChildren skucheck" + LineNumber + "'  value=''/>"));
    //td3.append($("<input type='text' id='goodsname" + LineNumber + "'  name='goodsname' Readonly='true' style='width:255px' class='form-control goodsname" + LineNumber + "'   value=''>"));
    ////td4.append($("<input type='dropdownlist' name='goodstype' id='productlevels" + LineNumber + "'   class='productlevels" + LineNumber + "' value=''>"));
    //td4.append($("<input type='text' id='UPC" + LineNumber + "'   style='width:180px'  class='form-control UPCChildren UPCcheck" + LineNumber + "'  value=''/>"));
    //td5.append($("<input type='text' name='countsum' style='width:80px' class='form-control countsum" + LineNumber + "' value='' onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)'>"));
    //row.append(td0);
    //row.append(td1);
    //row.append(td2);
    //row.append(td3);
    ////row.append(td4);
    //row.append(td4);
    //row.append(td5);
    //for (var i = 7; i <= table1.find("thead>tr>th").length ; i++) {
    //    td = td + (i).toString();
    //    if (table1.find("thead>tr>th")[i - 1].innerHTML.trim() == "生产日期") {
    //        td = $("<td></td>").append($("<input type='text' style='width:180px;cursor: pointer;' class='form-control'  name='dongtai" + i + "'  onclick='ShowCalendar(this)'/>"));
    //    }
    //    else if (table1.find("thead>tr>th")[i - 1].innerHTML.trim() == "单位") {
    //        td = $("<td class='Unit'></td>").append($("<select id='Unit" + LineNumber + "'  name='Unit' style='width:130px'   class='form-control Unit" + LineNumber + "'   value='' />"));
    //    }
    //    else if (table1.find("thead>tr>th")[i - 1].innerHTML.trim() == "规格") {
    //        td = $("<td class='Specifications'></td>").append($("<select id='Specifications" + LineNumber + "'  name='Specifications' style='width:130px'   class='form-control Specifications" + LineNumber + "'   value='' />"));
    //    }
    //    else {
    //        if (table1.find("thead>tr>th")[i - 1].innerHTML.trim() == "操作") {
    //            td = $("<td style='position: absolute;left: 86%;width: 120px;height:44px'></td>").append($("<Lable id='labelRemove' style='cursor:pointer; color:white' class='label label-info' ><b>删除</b></Lable>")).append($("<Lable style='cursor:pointer; color:white;width:120px' class='label label-info' onclick='addNew()'><b>新增一行</b></Lable>"));;
    //        }
    //        else {
    //            if (table1.find("thead>tr>th")[i - 1].innerHTML.trim() == "批次号" && $("#AsnandDetails_asn_CustomerID").val() == "90") {
    //                var myDate = new Date();
    //                //获取当前年
    //                var year = myDate.getFullYear();
    //                //获取当前月
    //                var month = myDate.getMonth() + 1;
    //                //获取当前日
    //                var date = myDate.getDate();
    //                var now = year +p(month) +p(date);
    //                td = $("<td></td>").append($("<input type='text' style='width:180px' class='form-control'  name='dongtai" + i + "' value='" + now + "'/>"));
    //            } else {
    //                td = $("<td></td>").append($("<input type='text' style='width:180px' class='form-control'  name='dongtai" + i + "' value=''/>"));
    //            }
    //        }
    //    }
    //    row.append(td);
    //}
    //table1.append(row);
    //var tr = document.getElementById('Newtable').getElementsByTagName('tr');
    //var UnitNum = $(tr[rownumber]).children(".Unit").children("select")[0];
    //var SpecificationsNum = $(tr[rownumber]).children(".Specifications").children("select")[0];
    ////var SpecificationsNum = tr[rownumber].cells[5].children[0];
    //var UnitList = $("#UnitLists");
    //var SpecificationsLists = $("#SpecificationsLists");
    //for (var i = 0; i < UnitList[0].length; i++) {
    //    if (UnitList[0][i].innerText == "") {
    //        var opt = new Option(UnitList[0][i].innerText, UnitList[0][i].innerText, true, true);
    //    }
    //    else {
    //        var opt = new Option(UnitList[0][i].innerText, UnitList[0][i].innerText);
    //    }
    //    UnitNum.options.add(opt);
    //}
    //for (var i = 0; i < SpecificationsLists[0].length; i++) {
    //    var opt = new Option(SpecificationsLists[0][i].innerText, SpecificationsLists[0][i].innerText);
    //    SpecificationsNum.options.add(opt);
    //}
    //row_count++;
}
function p(s) {
    return s < 10 ? '0' + s : s;
}
function Del(de) {
    //var a = $(de).parent().parent().children()[0].innerText;
    $(de).parent().parent().parent().remove();
    //calculateListNum(1);
    var tbody = $('#resultTable');
    var tr = $(tbody).find('tr').not('#trDemo').length;
    if (tr > 0) {
        for (var i = 0; i < tr; i++) {
            $($(tbody).find('tr').not('#trDemo')[i]).children('.LineNumber')[0].innerText = NumList(i + 1);
        }
    }
    else {
        showMsg("请先添加一行！", 4000);
    }

    //var t = document.getElementById("resultTable");
    //var rl = t.rows.length;
    //var num = 0;
    //var froms = $(de).parent().parent().prevAll().length;
    //var lengths = $(de).parent().parent().nextAll().length;
    //if (rl > 0) {
    //    for (i = 0; i < rl; i++) {
    //        num++;
    //        $(t.rows[i]).children(".LineNumber")[0].innerText = NumList(parseInt(froms) + i + 1);
    //        //    var num = NumList(parseInt(froms) + i + 1);
    //        //    t.rows[parseInt(froms) + i].cells[0].innerText = num;
    //        //    t.rows[parseInt(froms) + i].cells[4].children.batchs00001.id = "batchs" + num;
    //        //    t.rows[parseInt(froms) + i].cells[5].children.boxnumber00001.id = "boxnumber" + num;
    //        //    t.rows[parseInt(froms) + i].cells[6].children.Unit00001.id = "Unit" + num;
    //        //    t.rows[parseInt(froms) + i].cells[7].children.Specifications00001.id = "Specifications" + num;
    //        //    t.rows[parseInt(froms) + i].cells[8].children['0'].id = "inventoryQty" + num;
    //    }
    //} else {
    //    showMsg("请先添加一行！", 4000);
    //}
}

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

function contains(arr, obj) {
    var i = arr.length;
    while (i--) {
        if (arr[i] === obj) {
            return true;
        }
    }
    return false;
}

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

function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    //var re = new RegExp(pattern);
    if (pattern.test(hehe.value)) {

        hehe.value = hehe.value.replace(pattern, "");
    }
}

$('#labelRemove').live('click', function () {
    var length = $(this).parent().parent().parent().nextAll().length;
    var LineNumber = 0;
    var row_number = 0;
    for (i = 0; i < length; i++) {
        row_number = parseInt($(this).parent().parent().parent().nextAll()[i].rowIndex) - 1;
        LineNumber = ReturnLineNumber(row_number);
        $("#Newtable tr:eq(" + parseInt($(this).parent().parent().parent().nextAll()[i].rowIndex) + ") td:nth-child(2)").html("<b>" + LineNumber + "</b>");
        //    $("#Newtable tr:eq(" + parseInt($(this).parent().parent().nextAll()[i].rowIndex) + ") td:nth-child(2)")[0].children.sku.id = "sku" + LineNumber;
        //    $("#Newtable tr:eq(" + parseInt($(this).parent().parent().nextAll()[i].rowIndex) + ") td:nth-child(3)")[0].children.goodsname.id = "goodsname" + LineNumber;
        //    $("#Newtable tr:eq(" + parseInt($(this).parent().parent().nextAll()[i].rowIndex) + ") td:nth-child(4)")[0].children.UPC.id = "UPC" + LineNumber;
    }
    $(this).parent().parent().parent().remove();
    asndetailcount--;
});

$('select[id=CustomerID]').live('change', function () {
    window.location.href = "/WMS/ASNManagementSF/ASNCreateOrEdit/?ID=0" + "&customerID=" + $(this).val() + "&ViewType=1";
});

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

//从view页面到edit页面
function edits(ID) {
    location.href = "/WMS/ASNManagementSF/ASNCreateOrEdit/?ID=0" + ID + "&ViewType=2"
}

function Receipt(ID, CustomerID) {
    location.href = "/WMS/ReceiptManagement/ReceiptCreate/?ID=" + ID + "&ViewType=1&CustomerID=" + CustomerID
}

//继续添加
function addangian() {
    var customerID = $("#AsnandDetails_asn_CustomerID").val();
    location.href = "/WMS/ASNManagementSF/ASNCreateOrEdit/?ID=0" + "&customerID=" + customerID + "&ViewType=1"
}

function AddASNAndASNDetails() {
    var CustomerID = $('#CustomerID').val();
    if (CustomerID == 0) {
        showMsg("请选择客户!", "4000");
        return;
    }
    var ExternReceiptNumber = $('#ExternReceiptNumber').val();
    if (ExternReceiptNumber == "") {
        showMsg("请填写外部单号!", "4000");
        return;
    }
    var ASNType = $("#ASNType").find("option:selected").text();
    if (ASNType == "" || ASNType == null) {
        showMsg("请选择预入库单类型!", "4000");
        return;
    }
    var ExpectDate = $('#ExpectDate').val();
    if (ExpectDate == "") {
        showMsg("请选择预入库日期!", "4000");
        return;
    }
    var WarehouseID = $('#WarehouseID').val();
    if (WarehouseID == 0 || WarehouseID == null) {
        showMsg("请选择仓库!", "4000");
        return;
    }
    var CustomerName = $('select#CustomerID').find('option:selected').text();
    var Remark = $('#Remark').val();
    //zi
    var JsonTable = addjsontotable();
    var JsonField = FieldSetToJson();
    if (JsonTable.toString() == "]") {
        showMsg("请添加明细!", 4000);
        return;
    }
    var lengths = $('#Newtable').find('tr').not('#trDemo').length - 1;
    if (lengths > 0) {
        var tr = $('#Newtable').find('tr').not('#trDemo'); //document.getElementById('Newtable').getElementsByTagName('tr');
        for (var i = 1; i <= lengths; i++) {
            //$(tr[rownumber]).children(".Unit").children("select")[0]
            if ($(tr[i]).children(".SKU").children("input")[0].value == '') {
                showMsg("SKU有空值!", 4000);
                return;
            }
            if ($(tr[i]).children(".GoodsName").children("input")[0].value == '') {
                showMsg("产品名称有空值!", 4000);
                return;
            }
            if ($(tr[i]).children(".QtyExpected").children("input")[0].value == '') {
                showMsg("预收数量有空值!", 4000);
                return;
            }
        }
    }
    var Warehousename = $('select#WarehouseID').find('option:selected').text();
    $.ajax({
        type: "POST",
        url: "/WMS/ASNManagementSF/AddASNAndASNDetails",
        data: {
            "JsonTable": JsonTable,
            "ExternReceiptNumber": ExternReceiptNumber,
            "CustomerName": CustomerName,
            "CustomerID": CustomerID,
            "ASNType": ASNType,
            "Remark": Remark,
            "WarehouseID": WarehouseID,
            "Warehousename": Warehousename,
            "ExpectDate": ExpectDate,
            "JsonField": JsonField
        },
        async: "false",
        success: function (data) {
            if (data == "外部单号已存在") {
                showMsg(data, "4000");
                return;
            } else if (data == 0 || data == "") {
                showMsg("添加失败", "4000");
                return;
            } else if (data.indexOf("添加成功") >= 0) {
                //showMsg("添加成功！", "4000");
                var d = data.substring(4, data.length);
                location.href = "/WMS/ASNManagementSF/ASNCreateOrEdit/?ID=" + d + "&ViewType=3";
            } else {
                showMsg("添加失败,失败原因为:" + data, "4000");
                return;
            }
        },
        error: function (msg) {
            showMsg("添加失败1", "4000");
        }
    });
}

function UpdateASNAndASNDetail(asndetailcount) {
    var JsonTable = TableToJson(asndetailcount);
    if (JsonTable.toString() == "]") {
        showMsg("请添加明细!", "4000");
        return;
    }
    var ID = $('#ASNIDs').val();
    var ExternReceiptNumber = $('#ExternReceiptNumbers').val();
    var CustomerID = $('#CustomerID').val();
    if (CustomerID == 0) {
        showMsg("请选择客户!", "4000");
        return;
    }
    var ASNNumbers = $('#ASNNumber').val();
    var Createtime = $('#Createtime').val();
    var Creator = $('#Creator').val();
    var ExpectDates = $('#ExpectDate').val();

    if (ExpectDates == "") {
        showMsg("请选择预入库时间!", "4000");
        return;
    }
    //var CustomerName = $('select#CustomerID').find('option:selected').text();
    var CustomerName = $('#CustomerName').val();
    var ASNType = $('#ASNType').val();
    var Remark = $('#Remark').val();
    var WarehouseID = $('#WarehouseID').val();
    if (WarehouseID == 0) {
        showMsg("请选择仓库!", "4000");
        return;
    }
    //var lengths = $('#Newtable')[0].rows.length - 1;
    var lengths = $('#Newtable').find('tr').not('#trDemo').length - 1;
    if (lengths > 0) {
        var tr = $('#Newtable').find('tr').not('#trDemo');
        //var tr = document.getElementById('Newtable').getElementsByTagName('tr');
        for (var i = 1; i <= lengths; i++) {
            //$(tr[rownumber]).children(".Unit").children("select")[0]
            if ($(tr[i]).children(".SKU").children("input")[0].value == '') {
                showMsg("SKU有空值!", 4000);
                return;
            }
            if ($(tr[i]).children(".GoodsName").children("input")[0].value == '') {
                showMsg("产品名称有空值!", 4000);
                return;
            }
            if ($(tr[i]).children(".QtyExpected").children("input")[0].value == '') {
                showMsg("预收数量有空值!", 4000);
                return;
            }
        }
    }
    //var Warehousename = $('select#WarehouseID').find('option:selected').text();
    var Warehousename = $('#WarehouseName').val();
    var JsonField = FieldSetToJson();
    $.ajax({
        type: "Post",
        url: "/WMS/ASNManagementSF/UpdateASNAndASNDetails",
        data: {
            "JsonTable": JsonTable,
            "ID": ID,
            "ExternReceiptNumber": ExternReceiptNumber,
            "CustomerName": CustomerName,
            "CustomerID": CustomerID,
            "ASNType": ASNType,
            "Remark": Remark,
            "WarehouseID": WarehouseID,
            "Warehousename": Warehousename,
            "ExpectDate": ExpectDates,
            "Createtime": Createtime,
            "Creator": Creator,
            "ASNNumber": ASNNumbers,
            "JsonField": JsonField
        },
        async: "false",
        success: function (data) {
            location.href = "/WMS/ASNManagementSF/ASNCreateOrEdit/?ID=" + ID + "&ViewType=3";
            showMsg("修改成功!", "4000");
        },
        error: function (msg) {
            showMsg("修改失败！", "4000");
        }
    });
}

//主表
function FieldSetToJson() {
    var txt = "[";
    var table = document.getElementById("table_body");
    var row = table.getElementsByTagName("tr");
    if (row.length > 1) {
        var r = "{";
        for (var j = 1; j < row.length; j++) {
            var col = row[j].getElementsByTagName("td");
            for (var i = 0; i < col.length; i++) {
                var tds = row[j].getElementsByTagName("td");
                if (tds[i].className.trim() != "TableColumnTitle" && tds[i].innerHTML.trim() != "") {
                    //if (j == 0 && i == 5) {
                    //    r += "\"" + tds[i].childNodes[3].id.trim() + "\"\:\"" + tds[i].childNodes[3].value + "\",";
                    //}
                    //else {
                    //    r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + tds[i].childNodes[1].value + "\",";
                    //}
                    if (tds[i].childNodes.length > 1 && tds[i].childNodes[1].type == 'checkbox') {
                        r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + (tds[i].childNodes[1].checked == true ? 1 : 0) + "\",";
                    } else if (tds[i].childNodes.length > 1 && tds[i].childNodes[1].type == 'text/javascript' && tds[i].childNodes[3].type == 'text') {
                        r += "\"" + tds[i].childNodes[3].id.trim() + "\"\:\"" + tds[i].childNodes[3].value + "\",";
                    }
                    else {
                        if (tds[i].childNodes.length == 0 && tds[i].childNodes[0].value == '新增') {
                            r += "\"Status\"\:\"" + 1 + "\",";
                        } else if (tds[i].childNodes.length > 1 && tds[i].childNodes[1].value == '新增') {
                            r += "\"Status\"\:\"" + 1 + "\",";
                        } else if (tds[i].childNodes.length > 1) {
                            r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + tds[i].childNodes[1].value + "\",";
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

//修改的时候获取子表
function TableToJson(lastrows) {
    lastrows = parseInt($('#asndetailcount').val()) + parseInt(asndetailcount);
    var txt = "[";
    var table = document.getElementById("Newtable");
    var row = $(table).find('tr').not('#trDemo'); //table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j <= lastrows; j++) {
        var r = "{";
        for (var i = 1; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");

            if (col[i].innerHTML.trim() != "行号") {
                r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].childNodes[1].value.trim() + "\"!,";
            }
            else {
                r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].innerText.trim() + "\"!,";
            }
        }
        r = r.substring(0, r.length - 2)
        r += "},";
        txt += r;
    }
    for (var j = lastrows + 1; j < row.length; j++) {
        var r = "{";
        for (var i = 0; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if (i != 0) {
                if (col[i].innerHTML.trim() != "行号") {
                    r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].childNodes[0].value.trim() + "\"!,";
                }
                else {
                    r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].innerText.trim() + "\"!,";
                }
            }
        }
        r = r.substring(0, r.length - 2)
        r += "},";
        txt += r;
    }
    txt = txt.substring(0, txt.length - 1);
    txt += "]";
    return txt;
}

//添加的时候获取子表数据
function addjsontotable() {
    var txt = "[";
    var table = document.getElementById("Newtable");
    var row = $(table).find('tr').not('#trDemo'); //table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 1; i < col.length; i++) {
            var tds = $(row[j]).find('td'); /*row[j].getElementsByTagName("td");*/
            if (i != 0 && i != 1) {
                r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].children[0].value.trim() + "\"!,";
            }
            else {
                r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].innerText.trim() + "\"!,";
            }
        }
        r = r.substring(0, r.length - 2)
        r += "},";
        txt += r;
    }
    txt = txt.substring(0, txt.length - 1);
    txt += "]";
    return txt;
}

//单条转入库单
function TrunReceipt() {
    var ASNIDs = $('#ASNIDs').val;
    location.href = "/WMS/StorageManagement/ReceiptCreate?ID=" + ASNIDs + "&ViewType=2";
}

