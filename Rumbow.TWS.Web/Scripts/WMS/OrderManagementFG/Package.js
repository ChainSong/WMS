$(document).ready(function () {
   
    $('select[id=box]').live('change', function () {
        var str = $(this).val();
        var array = str.split(',');
        $(this).parent().parent().next().find('#Length').val(array[0]);
        $(this).parent().parent().next().find('#Width').val(array[1]);
        $(this).parent().parent().next().find('#Height').val(array[2]);
    });
    boxchanged($('select[id=box]')[1]);

    $("#submitButton").live('click', function () {
        var flag = 0;
        var jsonString="["
        $(".counts").each(function (a, b) {
            if ($(b).children()[4].rows[1].cells[1].children.Length.value == "" || $(b).children()[4].rows[1].cells[3].children.Width.value == "" ||
                $(b).children()[4].rows[1].cells[5].children.Height.value == "" || $(b).children()[5].rows[1].cells[0].children.SKU.value == "" ||
                //$(b).children()[5].rows[1].cells[1].children.UPC.value == "" ||
                $(b).children()[5].rows[1].cells[2].children.GoodsType.value == "" || $(b).children()[5].rows[1].cells[3].children.GoodsName.value == "" ||
                $(b).children()[5].rows[1].cells[4].children.Qty.value == "")
            {
                flag = 1;
                showMsg("有空值！", 4000);
                return;
            }
            //if (flag == 1)
            //{ return;}
            var c = a;
            var tables = $(b).children(); 
            var table1 = tables[4];
            var table2 = tables[5];
            var txt = "";
            var r = "{";
         
            var row1 = table1.rows;
            var row2 = table2.rows;
      
            for (var j = 0; j < row1.length; j++) {
                if (j == 0) {
                    var col = row1[j].getElementsByTagName("td");
                    for (var i = 1; i < col.length; i++) {
                        var tds = row1[j].getElementsByTagName("td");
                        if (i ==1) {
                            r += "\"str2\"\:\"" + tds[i].childNodes[0].value + "\",";
                        }
                    }
                }
                else {
                    var col = row1[j].getElementsByTagName("td");
                    for (var i = 1; i < col.length; i++) {
                        var tds = row1[j].getElementsByTagName("td");
                        if (i != 2 && i != 4) {
                            r += "\"" + tds[i].childNodes[0].id.trim() + "\"\:\"" + tds[i].childNodes[0].value + "\",";
                        }
                    }
                }
            }
            //r = r.substring(0, r.length - 1)
            //r += "},";
            //txt += r;
            for (var j = 1; j < row2.length; j++) {
                var r2 = r;
                var col = row2[j].getElementsByTagName("td");
                for (var i = 0; i < col.length; i++) {
                    var tds = row2[j].getElementsByTagName("td");
                    if (i != 5) {
                        if (i == 4) {
                            r2 += "\"" + tds[i].childNodes[0].id.trim() + "\"\:" + tds[i].childNodes[0].value + ",";
                        }
                        else {
                            r2 += "\"" + tds[i].childNodes[0].id.trim() + "\"\:\"" + tds[i].childNodes[0].value + "\",";
                        }
                    }
                }
            
                r2 = r2.substring(0, r2.length - 1)
                r2 += "},";
                txt += r2;
            }
       
            //txt = txt.substring(0, txt.length - 1);
            jsonString += txt;
        });
        jsonString = jsonString.substring(0, jsonString.length - 1);
        jsonString += "]";
        if (flag == 0) {
            $.ajax({
                type: "Post",
                url: "/WMS/OrderManagementFG/Package",
                data: {
                    "ID": $("#OrderID").val(),
                    "JsonPackage": jsonString,
                    "flag": 0

                },
                async: "false",
                success: function (data) {
                    if (data == "") {
                        //location.href = "/WMS/ReceiptManagement/ReceiptCreate/?ID=" + ID + "&ViewType=3&Flag=1"
                        showMsg("包装完成！", 4000);
                    }
                    else {
                        showMsg("包装失败！" + data, 4000);
                    }

                },
                error: function (msg) {
                    alert(msg.val);
                }

            });
        }
    });
    $("#packageButton").live('click', function () {
        var flag = 0;
        var jsonString = "["
        $(".counts").each(function (a, b) {
            if ($(b).children()[4].rows[1].cells[1].children.Length.value == "" || $(b).children()[4].rows[1].cells[3].children.Width.value == "" ||
                $(b).children()[4].rows[1].cells[5].children.Height.value == "" || $(b).children()[5].rows[1].cells[0].children.SKU.value == "" ||
                //$(b).children()[5].rows[1].cells[1].children.UPC.value == "" ||
                $(b).children()[5].rows[1].cells[2].children.GoodsType.value == "" || $(b).children()[5].rows[1].cells[3].children.GoodsName.value == "" ||
                $(b).children()[5].rows[1].cells[4].children.Qty.value == "") {
                flag = 1;
                showMsg("有空值！", 4000);
                return;
            }
            //if (flag == 1)
            //{ return; }
            var c = a;
            var tables = $(b).children();
            var table1 = tables[4];
            var table2 = tables[5];
            var txt = "";
            var row1 = table1.rows;
            var row2 = table2.rows;
            var r = "{";
            for (var j = 1; j < row1.length; j++) {
                var col = row1[j].getElementsByTagName("td");
                for (var i = 1; i < col.length; i++) {
                    var tds = row1[j].getElementsByTagName("td");
                    if (i != 2 && i != 4) {
                        r += "\"" + tds[i].childNodes[0].id.trim() + "\"\:\"" + tds[i].childNodes[0].value + "\",";
                    }
                }
            }
            //r = r.substring(0, r.length - 1)
            //r += "},";
            //txt += r;
            for (var j = 1; j < row2.length; j++) {
                var r2 = r;
                var col = row2[j].getElementsByTagName("td");
                for (var i = 0; i < col.length; i++) {
                    var tds = row2[j].getElementsByTagName("td");
                    if (i != 5) {
                        if (i == 4) {
                            r2 += "\"" + tds[i].childNodes[0].id.trim() + "\"\:" + tds[i].childNodes[0].value + ",";
                        }
                        else {
                            r2 += "\"" + tds[i].childNodes[0].id.trim() + "\"\:\"" + tds[i].childNodes[0].value + "\",";
                        }
                    }
                }

                r2 = r2.substring(0, r2.length - 1)
                r2 += "},";
                txt += r2;
            }

            //txt = txt.substring(0, txt.length - 1);
            jsonString += txt;
        });
        jsonString = jsonString.substring(0, jsonString.length - 1);
        jsonString += "]"
        if (flag == 0) {
            $.ajax({
                type: "Post",
                url: "/WMS/OrderManagementFG/Package",
                data: {
                    "ID": $("#OrderID").val(),
                    "JsonPackage": jsonString,
                    "flag": 1

                },
                async: "false",
                success: function (data) {
                    if (data == "") {
                        //location.href = "/WMS/ReceiptManagement/ReceiptCreate/?ID=" + ID + "&ViewType=3&Flag=1"
                        showMsg("包装交接完成！", 4000);
                    }
                    else {
                        showMsg("包装交接失败！" + data, 4000);
                    }

                },
                error: function (msg) {
                    alert(msg.val);
                }

            });
        }
    });
     
    $("#printBoxButton").live("click", function () {
        layer.confirm('<font size="4">确认是否批量打印箱清单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var str = $("#OrderID").value;


            location.href = '/WMS/ReceiptManagement/PrintShelves?rid=' + str.substring(0, str.length - 1) + "&Flag=1";
        });
    });

    $("#returnButton").live('click', function (){
        history.back();
    });
    $("#addButton").live('click', function () {
        var boxtext = $(".counts")[$(".counts").length - 1].innerText.trim();
        var sums = parseInt(boxtext.toString().substring(1, boxtext.toString().indexOf("箱")));
        sums = sums + 1;
  
        // var sums = $(".counts").length + 1;
        var n = returnlinenumber(sums);
        var BoxNum = $("#label_OrderNumber")[0].innerHTML.trim() + n;
        var obj = document.getElementById('packagesModel');
        var newObj = obj.cloneNode(true);
        newObj.setAttribute("style", "margin-bottom:40px;border:2px solid #e8eef4");
        newObj.setAttribute("class", "counts");
        newObj.setAttribute("readonly", "true");
        $(newObj.children[4].rows[2].cells[1].children.PackageNumber)[0].value = BoxNum;
        newObj.children[0].innerText = "第" + sums + "箱"
        //newObj.setAttribute("id", "packages" + sums);
        //newObj.children[1].innerHTML = "<label style='font-size:20px'> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;       </label><label style='font-size:20px'>扫描SKU: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </label><input class='form-control'  style = 'width:22%' type='text' id='ScanSKU'/>";
        $(".counts").last().after(newObj);
        var h = $(document).height() - $(window).height();
        $(document).scrollTop(h);
        //$(".counts").last()[0].children[5].rows[1].cells[0].children.SKU.focus();
        $(".counts").last()[0].children.ScanSKU.focus();
        boxchanged( $('select[id=box]')[$('select[id=box]').length-1]);
    });

    $("#ScanSKU").live('keydown', function () {
        if (event.keyCode == 13) {
            if ($(this).val()=="") {
                showMsg("扫描数据不能为空!", "4000");
                return false;
            }
            //if ($("#chkZero").get(0).checked == true) {
            //    $(this).val("00" + $(this).val());
            //}
            var inputsku = $(this);
          
            inputsku.select();
            var thistr = inputsku.parent()[0].children.resultTableModel;
            if (thistr == undefined) {
                thistr = $(inputsku.parent()[0].children.resultTable)[0];
            }
            var rows_count = thistr.rows.length - 1;
            //初始的时候，直接查询数据并赋值
            if (rows_count == 1 && $(thistr.rows[1]).children().eq(3).find("input").val().trim() == "") {
                if (CheckSKU(inputsku.val())) {
                    $(thistr.rows[1]).children().eq(0).find("input").val(nowsku[0])
                    $(thistr.rows[1]).children().eq(1).find("input").val(nowsku[1])
                    $(thistr.rows[1]).children().eq(2).find("input").val(nowsku[3])
                    $(thistr.rows[1]).children().eq(3).find("input").val(nowsku[2])
                    $(thistr.rows[1]).children().eq(4).find("input").val(1);
                }
                else {
                    showMsg("SKU不存在!", "4000");
                    return false;
                }
                //$.ajax({
                //    url: "/WMS/OrderManagementFG/GetSKUlist",
                //    type: "POST",
                //    dataType: "json",
                //    async: false,
                //    data: { sku: inputsku.val(), ID: $("#OrderID").val() },
                //    success: function (data) {
                //        if (data <= 0) {
                //            showMsg("SKU不存在!", "4000");
                //            return false;
                //        }
                //        else {
                //            $(thistr.rows[1]).children().eq(0).find("input").val(data[0].Text)
                //            $(thistr.rows[1]).children().eq(1).find("input").val(data[0].GoodsType)
                //            $(thistr.rows[1]).children().eq(2).find("input").val(data[0].Value)
                //            $(thistr.rows[1]).children().eq(3).find("input").val(1);
                //        }
                //    }
                //})
                QtyChanged();
                return true;
            }
            var Isexist = false;
            var Isexistline = 0;
            for (var i = 1; i <= rows_count; i++) {
                if ($(thistr.rows[i]).children().eq(0).find("input").val().trim() == inputsku.val().trim() || $(thistr.rows[i]).children().eq(1).find("input").val().trim() == inputsku.val().trim()) {
                    Isexist = true;
                    Isexistline = i;
                }
            }
            if (Isexist) {
                var qty = parseInt($(thistr.rows[Isexistline]).children().eq(4).find("input").val().trim()) + 1;
                $(thistr.rows[Isexistline]).children().eq(4).find("input").val(qty);

            }
            else {
                if (CheckSKU(inputsku.val())) {
                    addNew($(thistr.rows[rows_count]).children().eq(0).find("input")[0]);
                    thistr = $(inputsku.parent()[0].children.resultTable)[0];
                    if (thistr == undefined) {
                        thistr = inputsku.parent()[0].children.resultTableModel;
                    }
                    rows_count = thistr.rows.length - 1;
                    $(thistr.rows[rows_count]).children().eq(0).find("input").val(nowsku[0])
                    $(thistr.rows[rows_count]).children().eq(1).find("input").val(nowsku[1])
                    $(thistr.rows[rows_count]).children().eq(2).find("input").val(nowsku[3])
                    $(thistr.rows[rows_count]).children().eq(3).find("input").val(nowsku[2])
                    $(thistr.rows[rows_count]).children().eq(4).find("input").val(1)
                    QtyChanged();
                    return true;
                }
                else {
                    showMsg("SKU不存在!", "4000");
                    return false;
                }
                //$.ajax({
                //    url: "/WMS/OrderManagementFG/GetSKUlist",
                //    type: "POST",
                //    dataType: "json",
                //    async: false,
                //    data: { sku: inputsku.val(), ID: $("#OrderID").val() },
                //    success: function (data) {
                //        if (data.length <= 0) {
                //            showMsg("SKU不存在!", "4000");
                //            return false;
                //        }
                //        else {
                //            addNew($(thistr.rows[rows_count]).children().eq(0).find("input")[0]);
                //            thistr = $(inputsku.parent()[0].children.resultTable)[0];
                //            if (thistr == undefined) {
                //                thistr = inputsku.parent()[0].children.resultTableModel;
                //            }
                //            rows_count = thistr.rows.length - 1;
                //            $(thistr.rows[rows_count]).children().eq(0).find("input").val(data[0].Text)
                //            $(thistr.rows[rows_count]).children().eq(1).find("input").val(data[0].GoodsType)
                //            $(thistr.rows[rows_count]).children().eq(2).find("input").val(data[0].Value)
                //            $(thistr.rows[rows_count]).children().eq(3).find("input").val(1)
                //            QtyChanged();
                //            return true;
                //        }
                //    }
                //})

            }
            QtyChanged();
        }
    });
    $("#SKU").live('keydown', function () {


        //if (event.keyCode == 13) {
        // var inputsku = $(this);
        //    var inputsku = $(this);
        //    $.ajax({
        //        url: "/WMS/OrderManagementFG/GetSKUlist",
        //        type: "POST",
        //        dataType: "json",
        //        data: { sku: $(this).val(), ID: $("#OrderID").val() },
        //        success: function (data) {

        //            addQty(data, inputsku);
        //        }
        //    })

        //}

        $(this).autocomplete({
            source: function (request, response) {
                if (request.term.length > 5) {
                    $.ajax({
                        url: "/WMS/OrderManagementFG/GetSKUlist",
                        type: "POST",
                        dataType: "json",
                        data: { sku: request.term, ID: $("#OrderID").val() },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return { label: item.Text, value: item.Text, data: item }
                            }));
                        }
                    })
                };
            },
            select: function (event, ui) {
                var row_count = $(this).parent().parent().parent().parent()[0].rows.length - 1;
                for (var i = 1; i <= row_count; i++) {
                    var s = $(this).parent().parent().parent().parent()[0].rows[i].cells[0].children.SKU.value;
                    if (s == ui.item.data.Text) {
                        showMsg("SKU已存在!", "4000");
                        return false;
                    }
                }
                var SkuQty = 0;
                $(".counts").each(function (a, b) {
                    var c = a;
                    var tables = $(b).children();
                    var table2 = tables[2];
                    var row2 = table2.rows;
                    for (var j = 1; j < row2.length; j++) {
                        var col = row2[j].getElementsByTagName("td");
                        if (col[0].children.SKU.value.toString().trim() == ui.item.data.Text.toString().trim()) {
                            SkuQty += parseInt(col[3].children.Qty.value);

                        }
                    }
                });

                var Qty = parseInt(ui.item.data.Qty) - SkuQty;


                $(this).parent().next().find('#GoodsType').val(ui.item.data.GoodsType);
                $(this).parent().next().next().find('#GoodsName').val(ui.item.data.Value);
                $(this).parent().next().next().next().find('#Qty').val(Qty);
            }
        })

    });

});

function boxchanged(obj) {
    var str = $(obj);

    var array = str.val().split(',');
    $(str).parent().parent().next().find('#Length').val(array[0]);
    $(str).parent().parent().next().find('#Width').val(array[1]);
    $(str).parent().parent().next().find('#Height').val(array[2]);
}


var nowsku = new Array();
function CheckSKU(obj) {
    var resualt = false;
    var orderdetaillist = eval($("#OrderDetail").val());
    $.each(orderdetaillist, function (index, item) {
        if (item.SKU == obj ||item.UPC==obj) {
            resualt = true;
            nowsku[0] = item.SKU;
            nowsku[1] = item.UPC;
            nowsku[2] = item.GoodsName;
            nowsku[3] = item.GoodsType;
            return false;
        }
    });
    if (!resualt) {
        nowsku.length = 0;
    }
    return resualt;
}
function QtyChanged(obj) {
    if (obj != undefined) {

        if (isNaN($(obj).val())) {
            showMsg("请输入数字！", 4000);
            $(obj).focus();
            $(obj).select();
            return;
        }
    }

    var qty = parseInt($("#totalcount")[0].innerHTML.toString().substring(5, $("#totalcount")[0].innerHTML.length));
    qty = qty - qty;
    $(".counts").each(function (a, b) {
        for (var i = 0; i < $(b).children()[5].rows.length; i++) {
            if (i != 0) {
                if (parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value) != NaN) {
                    qty += parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value);
                }
            }
        }
       
       
    });
    $("#packagecount")[0].innerHTML="已完成数量:" + qty;
    var qtytotal = parseInt($("#totalcount")[0].innerHTML.toString().substring(5, $("#totalcount")[0].innerHTML.length));
    $("#lastcount")[0].innerHTML = "剩余数量:" + (qtytotal - qty);
    if (qty == qtytotal) {
        $("#msgtd")[0].innerHTML = "订单已完成";
        $("#msgtd").css("color","green") 
    }
    else if (qty > qtytotal) {
        $("#msgtd")[0].innerHTML = "已超出订单数量";
        $("#msgtd").css("color", "red")
    }
    else {
        $("#msgtd")[0].innerHTML = "";
    }
}
//检测差异
function CheckDiffent() {
    //先汇总界面数据
    var packagelist = new Array();
    $(".counts").each(function (a, b) {
        for (var i = 0; i < $(b).children()[5].rows.length; i++) {
            if (i != 0) {
                if ($(b).children()[5].rows[i].cells[0].children.SKU.value != "") {
                    var isexist = false;
                    if (parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value) != NaN) {
                        $.each(packagelist, function (index, item) {
                            if (item[0] == $(b).children()[5].rows[i].cells[0].children.SKU.value) {
                                packagelist[index][1] = parseInt(packagelist[index][1]) + parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value);
                                isexist = true;
                                return false;
                            }
                        });
                        if (!isexist) {
                            var indexcount = packagelist.length;
                            packagelist[indexcount] = new Array();
                            packagelist[indexcount][0] = $(b).children()[5].rows[i].cells[0].children.SKU.value;
                            packagelist[indexcount][1] = parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value);
                        }
                    }
                }
            }
        }
    });
    //packagelist
    var chayilist = new Array();
    //循环订单明细
    var orderdetaillist = eval($("#OrderDetail").val());
    $.each(orderdetaillist, function (index, item) {
        var isexist = false;
        $.each(packagelist, function (index2, item2) {
            if (item.SKU == item2[0]) {
                isexist = true;
                if (item.Qty == item2[1]) {
                }
                else {
                    var chayileng = chayilist.length;
                    chayilist[chayileng] = new Array();
                    chayilist[chayileng][0] = item.SKU;
                    chayilist[chayileng][1] = item.Qty;
                    chayilist[chayileng][2] = item2[1];
                    chayilist[chayileng][3] = parseInt(item2[1])-parseInt(item.Qty);
                }
                return false;
            }
        });
        if (!isexist) {
            var chayileng = chayilist.length;
            chayilist[chayileng] = new Array();
            chayilist[chayileng][0] = item.SKU;
            chayilist[chayileng][1] = item.Qty;
            chayilist[chayileng][2] = 0;
            chayilist[chayileng][3] = 0- parseInt(item.Qty);
        }
    });
    if (chayilist.length <= 0) {
        alert("无差异！");
    }
    else {
        showmsg(chayilist);
    }
}

//显示结果
function showmsg(data) {
    if ($("#showdata2")[0].style.display == 'none') {
        $("#showdata2")[0].style.display = 'block';
        $("#showtable2").append("<tr><td>SKU</td><td>订单数量</td><td>包装数量</td><td>差异数量</td></tr>");
        $.each(data, function (index, item) {
            var chayiqty = item[3];
            if (chayiqty > 0) {
                $("#showtable2").append("<tr><td >" + item[0] + "</td><td>" + item[1] + "</td><td>" + item[2] + "</td><td >" + "+" + (item[3]) + "</td></tr>")
            }
            else {
                $("#showtable2").append("<tr><td >" + item[0] + "</td><td>" + item[1] + "</td><td>" + item[2] + "</td><td >" + (item[3]) + "</td></tr>")

            }
        });
        //alert($("#showtable").innerText);
        //$("#showtable").append("<tr><td>sku</td><td>qty</td><td>lot</td><td>lot2</td><td>unit</td><td>uom</td><td>location</td></tr>")
        //$.each(objtotal, function (index, item) {
        //    $("#showtable").append("<tr><td>" + item[0] + "</td><td>" + item[1] + "</td><td>" + item[2] + "</td><td>" + item[3] + "</td><td>" + item[4] + "</td><td>" + item[5] + "</td><td>" + item[6] + "</td></tr>")
        //});
    }
}
//关闭结果
function CloseDiv() {
    $("#showdata2")[0].style.display = 'none';
    $("#showtable2").html("");
}
function addNew(o) {
    var table1 = $(o.parentNode.parentNode.parentNode.parentNode);
    var row = $("<tr></tr>");
    //var firstTr = table1.find('tbody>tr:first');
    var td1 = $("<td></td>");
    var td2 = $("<td></td>");
    var td3 = $("<td></td>");
    var td4 = $("<td></td>");
    var td6 = $("<td></td>");
    var td = "td";
    var td5 = $("<td style='background-color:white;'></td>");
    td1.append($("<input type='text'  class='form-control' style='width:100%' id='SKU' readonly='true' /> "));
    td2.append($("<input type='text'  class='form-control' style='width:100%' id='GoodsType' readonly='true' /> "));
    td3.append($("<input type='text'  class='form-control' style='width:100%' id='GoodsName' readonly='true' /> "));
    td4.append($("<input type='text'  class='form-control' style='width:100%' id='Qty' onchange='QtyChanged(this)' /> "));
    //td5.append($(" <label  style='cursor:pointer; color:white; font-size:15px' class='label label-info' onclick='addNew(this)'>新增行</label>"));
    td5.append($(" <label  style='cursor:pointer; color:white; font-size:15px' class='label label-info' onclick='deleteNew(this)'>删除行</label>"));
    td6.append($("<input type='text'  class='form-control' style='width:100%' id='UPC' readonly='true' /> "));
    row.append(td1);
    row.append(td6);
    row.append(td2);
    row.append(td3);
    row.append(td4);
    row.append(td5);
    table1.append(row);
    }

function deleteNew(o)
{
    var rows = $(o.parentNode.parentNode.parentNode.parentNode)[0].rows.length - 1;
    if (rows <= 1)
    {
        showMsg("请至少保留一行！", 4000);
        return;
    }
    var tr = $(o.parentNode.parentNode);
    tr.remove();
}

function deleteDiv(obj)
{
    var sums = $(".counts").length;
    //var froms = $(obj).parent().parent().parent().parent().parent().prevAll().length - 2;
    //var lengths = $(obj).parent().parent().parent().parent().parent().nextAll().length;
   
    if (sums == 1)
    {
        showMsg("请至少保留一箱！", 4000);
        return;
    }
    //for (i = 0; i < lengths; i++) {
    //    //row_number = parseInt($(obj).parent().parent().parent().parent().parent().prevAll()[i].children[0].rows[2].cells[1].children.PackageNumber.value) - 1;
    //    //$(obj).parent().parent().parent().parent().parent().prevAll()[i].children[0].rows[2].cells[1].children.PackageNumber.value = row_number;
    //    var num = parseInt(froms) + i + 1;
    //    var n = returnlinenumber(parseInt(froms) + i + 1);
    //    var BoxNum = $("#label_OrderNumber")[0].innerHTML.trim() + n;
    //    $(obj).parent().parent().parent().parent().parent().nextAll()[i].children[0].innerText = "第" + num + "箱"
    //    $(obj).parent().parent().parent().parent().parent().nextAll()[i].children[1].rows[2].cells[1].children.PackageNumber.value = BoxNum;

    //}
    var s = $(obj.parentNode.parentNode.parentNode.parentNode.parentNode).remove();
}
//打印托运单
function printDiv(id,type) {
    //$.ajax({
    //    type: "Post",
    //    url: "/WMS/OrderManagementFG/print",
    //    data: {
    //        "ID": id,
    //        "type": type
    //    },
    //    async: "false",
    //    success: function (data) {

    //        showMsg("打印完成！", 4000);

    //    },
    //    error: function (msg) {
    //        alert(msg.val);
    //    }

    //});
    
    window.location.href = "/WMS/NikeOSRBJPrint/PrintPod?id=" + id + "&type=" + type;
    //openPopup("", true, 1000, 600, "/WMS/NikeOSRBJPrint/PrintPod?id=" + id + "&type=" + type, null, true);
}
function ShowsIn(ID, obj) {
    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
        $(".ddiv:not(obj)").animate({
            width: "hide",
            width: "150%",
            paddingRight: "hide",
            paddingLeft: "hide",
            marginRight: "hide",
            marginLeft: "hide"

        }, 100);
        $("#operateTD" + ID).animate({
            width: "show",
            width: "90%",
            paddingRight: "show",
            paddingLeft: "show",
            marginRight: "show",
            marginLeft: "show"
        });
    }
    //$("#operateTD" + ID)[0].style.display = "";
}

//打印箱清单
function printboxDiv(id, type) {
    if ($("#CustomerName").val().toString().indexOf("NikeOSR") > -1) {
        window.location.href = "/WMS/NikeOSRBJPrint/PrintBox?id=" + id + "&type=" + type;
    }
    else if ($("#CustomerName").val().toString().indexOf("NFS")>-1) {
        window.location.href = "/WMS/NikeOSRBJPrint/PrintBox?id=" + id + "&type=" + type; //20170228
    }
    else {
        window.location.href = "/WMS/NikeOSRBJPrint/PrintBox?id=" + id + "&type=" + type;
    }
    
    //openPopup("", true, 1000, 600, "/WMS/NikeOSRBJPrint/PrintBox?id=" + id + "&type=" + type, null, true);
    
    //$.ajax({
    //    type: "Post",
    //    url: "/WMS/OrderManagementFG/printbox",
    //    data: {
    //        "ID": id,
    //        "type": type
    //    },
    //    async: "false",
    //    success: function (data) {
           
    //            showMsg("打印完成！", 4000);
               
    //    },
    //    error: function (msg) {
    //        alert(msg.val);
    //    }

    //});
}
//打印箱唛
function printboxmDiv(id, type) {
    window.location.href = "/WMS/NikeOSRBJPrint/PrintBoxm?id=" + id + "&type=" + type;
    //openPopup("", true, 1000, 600, "/WMS/NikeOSRBJPrint/PrintBoxm?id=" + id + "&type=" + type, null, true);
    //$.ajax({
    //    type: "Post",
    //    url: "/WMS/OrderManagementFG/printboxm",
    //    data: {
    //        "ID": id,
    //        "type": type
    //    },
    //    async: "false",
    //    success: function (data) {

    //        showMsg("打印完成！", 4000);

    //    },
    //    error: function (msg) {
    //        alert(msg.val);
    //    }

    //});
}
function ShowsOut() {
    //$("#operateTD" + ID).fadeOut("slow");

    $(".ddiv").animate({
        width: "hide",
        width: "600%",
        paddingRight: "hide",
        paddingLeft: "hide",
        marginRight: "hide",
        marginLeft: "hide"

    }, 100);
    //$("#operateTD" + ID)[0].style.display = "";
}

function Outs() {
    $.ajax({
        type: "Post",
        url: "/WMS/OrderManagementFG/Outs",
        data: {
            "ID": $("#OrderID").val(),
            type: 1
        },
        async: "false",
        success: function (data) {
            if (data == "") {
                showMsg("出库完成！", 4000);
                location.href = "/WMS/OrderManagementFG/Index"
            }
            else {
                showMsg("出库失败：" + data, 4000);
            }
        },
        error: function (msg) {
            alert(msg.val);
        }

    });
}
function addQty(data, inputsku) {
    var thistr = inputsku.parent().parent().parent().parent()[0];
    var row_count = thistr.rows.length - 1;
    var Qty = 0;

        var isfind = false;//是否找到扫描的sku
        var skuline = 0;//找到的sku line
        var qtyline=0;//空行的line
        for (var i = 1; i < row_count; i++) {
            
            if (inputsku == $(thistr.rows[i]).children().eq(0).find("input").val().trim() && $(thistr.rows[i]).children().eq(3).find("input").val().trim()!="") {
                isfind = true;
                skuline = i;
            }
            else
            {
                if($(thistr.rows[i]).children().eq(3).find("input").val().trim() == "")
                {
                    qtyline=i;
                }
            }

        }
        if (isfind) {
            if ($(thistr.rows[skuline]).children().eq(3).find("input").val().trim() == "") {
                Qty = 1;

            }
            else {
                Qty = parseInt($(thistr.rows[skuline]).children().eq(3).find("input").val().trim()) + 1;
            }
            $(thistr.rows[skuline]).children().eq(1).find("input").val(data[0].GoodsType)
            $(thistr.rows[skuline]).children().eq(2).find("input").val(data[0].GoodsName)
            $(thistr.rows[skuline]).children().eq(3).find("input").val(Qty)
        }
        else {
            if (qtyline != 0) {
                $(thistr.rows[qtyline]).children().eq(1).find("input").val(data[0].GoodsType)
                $(thistr.rows[qtyline]).children().eq(2).find("input").val(data[0].GoodsName)
                $(thistr.rows[qtyline]).children().eq(3).find("input").val(1)
            }
            else {
                addNew(inputsku[0]);
                $(inputsku.parent().parent().parent().parent()[0].rows[row_count + 1]).children().eq(1).find("input").val(data[0].GoodsType)
                $(inputsku.parent().parent().parent().parent()[0].rows[row_count + 1]).children().eq(2).find("input").val(data[0].GoodsName)
                $(inputsku.parent().parent().parent().parent()[0].rows[row_count + 1]).children().eq(3).find("input").val(1)
            }

        }
    
}

function returnlinenumber(i) {
    var linnumber = "";
    if (i < 10) {
        linnumber = "00" + i;
    }
    if (100 > i && i >= 10) {
        linnumber = "0" + i;
    }
    if (1000 > i && i >= 100) {
        linnumber = i;
    }
    return linnumber;
}

