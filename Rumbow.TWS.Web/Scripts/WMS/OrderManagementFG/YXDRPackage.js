$(document).ready(function () {

    $('select[id=box]').live('change', function () {
        var str = $(this).val();
        var array = str.split(',');
        $(this).parent().parent().next().find('#Length').val(array[0]);
        $(this).parent().parent().next().find('#Width').val(array[1]);
        $(this).parent().parent().next().find('#Height').val(array[2]);
        $(this).parent().parent().parent().parent().parent().find('#ScanSKU').focus();
    });
    //if ($('select[id=box]').length > 1) {
    //    boxchanged($('select[id=box]')[1]);
    //}
    $("#submitButton").live('click', function () {
        var flag = 0;
        var jsonString = "["
        $(".counts").each(function (a, b) {
            if ($(b).children()[4].rows[1].cells[1].children.Length.value == "" || $(b).children()[4].rows[1].cells[3].children.Width.value == "" ||
                $(b).children()[4].rows[1].cells[5].children.Height.value == "" || $(b).children()[5].rows[1].cells[0].children.SKU.value == "" ||
                //$(b).children()[5].rows[1].cells[1].children.UPC.value == "" ||
                $(b).children()[5].rows[1].cells[2].children.GoodsType.value == "" || //$(b).children()[5].rows[1].cells[3].children.GoodsName.value == "" ||
                $(b).children()[5].rows[1].cells[4].children.Qty.value == ""
                || $(b).children()[4].rows[2].cells[3].children.NetWeight.value == "" || $(b).children()[4].rows[2].cells[3].children.NetWeight.value == "0") {
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
                        if (i == 1) {
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
                url: "/WMS/OrderManagement/YXDRPackage",
                data: {
                    "ID": $("#OrderID").val(),
                    "JsonPackage": jsonString,
                    "flag": 0

                },
                async: "false",
                success: function (data) {
                    if (data == "") {
                        //弹出层提示
                        layer.confirm('保存成功！', {
                            btn: ['确定'] //按钮
                        }, function () {
                            location.href = "/WMS/OrderManagement/YXDRPackage/?ID=" + $("#OrderID").val();
                        });
                        //alert("保存成功！")
                        //location.href = "/WMS/OrderManagement/YXDRPackage/?ID=" + $("#OrderID").val();

                    }
                    else {
                        showMsg("保存失败！" + data, 4000);
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
                $(b).children()[5].rows[1].cells[2].children.GoodsType.value == "" || //$(b).children()[5].rows[1].cells[3].children.GoodsName.value == "" ||
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

            for (var j = 0; j < row1.length; j++) {
                if (j == 0) {
                    var col = row1[j].getElementsByTagName("td");
                    for (var i = 1; i < col.length; i++) {
                        var tds = row1[j].getElementsByTagName("td");
                        if (i == 1) {
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
        jsonString += "]"
        if (flag == 0) {
            $.ajax({
                type: "Post",
                url: "/WMS/OrderManagement/YXDRPackage",
                data: {
                    "ID": $("#OrderID").val(),
                    "JsonPackage": jsonString,
                    "flag": 1

                },
                async: "false",
                success: function (data) {
                    if (data == "") {

                        //弹出层提示
                        layer.confirm('包装交接完成！', {
                            btn: ['确定'] //按钮
                        }, function () {
                            location.href = "/WMS/OrderManagement/YXDRPackage/?ID=" + $("#OrderID").val();
                        });
                        //location.href = "/WMS/ReceiptManagement/ReceiptCreate/?ID=" + ID + "&ViewType=3&Flag=1"
                        //showMsg("包装交接完成！", 4000);
                        //location.href = "/WMS/OrderManagement/YXDRPackage/?ID=" + $("#OrderID").val();
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

    $("#returnButton").live('click', function () {
        history.back();
    });
    $("#addButton").live('click', function () {
        var sums = 0;
        if ($(".counts").length > 0) {
            var boxtext = $(".counts")[$(".counts").length - 1].innerText.trim();
            //sums = parseInt(boxtext.toString().substring(1, boxtext.toString().indexOf("箱")));
            sums = $(".counts").length;
        }
        sums = sums + 1;
        // var sums = $(".counts").length + 1;
        var n = returnlinenumber(sums);
        //var BoxNum = $("#label_OrderNumber")[0].innerHTML.trim() + n; 老的获取箱号方法
        var BoxNum = GetMaxBox();
        var OrderId = $("#OrderID").val();
        //$.ajax({
        //    type: "Post",
        //    url: "/WMS/OrderManagement/GetMaxBoxnumber",
        //    data: {
        //        "OrderID": $("#OrderID").val()
        //    },
        //    async: false,
        //    success: function (data) {
        //        BoxNum = data;
        //    },
        //    error: function (msg) {
        //        alert(msg.val);
        //    }

        //});
        var obj = document.getElementById('packagesModel');
        var newObj = obj.cloneNode(true);
        newObj.setAttribute("style", "margin-bottom:40px;border:2px solid #e8eef4");
        newObj.setAttribute("class", "counts");
        $(newObj.children[4].rows[2].cells[1].children.PackageNumber)[0].value = BoxNum;
        newObj.children[0].innerText = "第" + sums + "箱";
        //var oNewDiv = document.createElement("td");
        //oNewDiv = " <div id='operateTD' style='float:right;display:none;width:250px;' class='ddiv' >   <div class='CheckOutboundOrder' data-id='' style='text-align:center; width:75px; float:left; border:solid 1px ;background-color:#f0ad4e;border-radius:3px;cursor:pointer;color:white; '  onclick='printDiv('" + BoxNum + "',0)'>打印托运单</div> <div class='CheckOutboundOrder' data-id='' style='text-align:center; width:75px; float:left; border:solid 1px ;background-color:#f0ad4e;border-radius:3px;cursor:pointer;color:white; '  onclick='printboxDiv('" + BoxNum + "',0)''>打印箱清单</div>"
        //+ "<div class='CheckOutboundOrder' data-id='' style='text-align:center; width:75px; float:left; border:solid 1px ;background-color:#f0ad4e;border-radius:3px;cursor:pointer;color:white; '  onclick='printboxmDiv('" + BoxNum + "',0)'>打印箱唛</div> </div> </td><td><div style='text-align:center; width:75px; border:solid 1px ;border-radius:3px;cursor:pointer;color:white;background-color:#5bc0de '  onmouseover ='ShowsIn('" + BoxNum + "',this)'  >打印</div>";

        //newObj.children[4].childNodes[1].appendChild(oNewDiv.toString());
        //newObj.children[4].childNodes[1].innerHTML += "<tr><td>asdddssadasdasdasdasds</td></tr>";

        //newObj.children[4].childNodes[1].childNodes[5].innerHTML+="<tr><td colspan='2'> <div id='operateTD' style='float:right;display:none;width:250px;' class='ddiv' >   <div class='CheckOutboundOrder' data-id='' style='text-align:center; width:75px; float:left; border:solid 1px ;background-color:#f0ad4e;border-radius:3px;cursor:pointer;color:white; '  onclick='printDiv('"+BoxNum+"',0)'>打印托运单</div> <div class='CheckOutboundOrder' data-id='' style='text-align:center; width:75px; float:left; border:solid 1px ;background-color:#f0ad4e;border-radius:3px;cursor:pointer;color:white; '  onclick='printboxDiv('"+BoxNum+"',0)''>打印箱清单</div>"
        //+"<div class='CheckOutboundOrder' data-id='' style='text-align:center; width:75px; float:left; border:solid 1px ;background-color:#f0ad4e;border-radius:3px;cursor:pointer;color:white; '  onclick='printboxmDiv('"+BoxNum+"',0)'>打印箱唛</div> </div> </td><td><div style='text-align:center; width:75px; border:solid 1px ;border-radius:3px;cursor:pointer;color:white;background-color:#5bc0de'  onmouseover ='ShowsIn("+BoxNum+",this)'  >打印</div></td></tr>";
        ////newObj.children[4].innerHTML="";

        //newObj.setAttribute("id", "packages" + sums);
        //newObj.children[1].innerHTML = "<label style='font-size:20px'> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;       </label><label style='font-size:20px'>扫描SKU: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </label><input class='form-control'  style = 'width:22%' type='text' id='ScanSKU'/>";
        $(".counts").first().before(newObj);
        var h = $(document).height() - $(window).height();
        $(document).scrollTop(h);
        //$(".counts").last()[0].children[5].rows[1].cells[0].children.SKU.focus();
        $(".counts").first()[0].children[4].childNodes[1].childNodes[0].innerHTML += "<td colspan='2'> <div id='operateTD" + BoxNum + "' style='float:right;display:none;width:250px;' class='ddiv' >   <div class='CheckOutboundOrder' data-id='' style='text-align:center; width:75px; float:left; border:solid 1px ;background-color:#f0ad4e;border-radius:3px;cursor:pointer;color:white; '  onclick=\"printDiv('" + BoxNum + "',0,'" + OrderId + "')\">打印托运单</div> <div class='CheckOutboundOrder' data-id='' style='text-align:center; width:75px; float:left; border:solid 1px ;background-color:#f0ad4e;border-radius:3px;cursor:pointer;color:white; '  onclick=\"printboxDiv('" + BoxNum + "',0,'" + OrderId + "')\">打印箱清单</div>"
            + "<div class='CheckOutboundOrder' data-id='' style='text-align:center; width:75px; float:left; border:solid 1px ;background-color:#f0ad4e;border-radius:3px;cursor:pointer;color:white; '  onclick=\"printboxmDiv('" + BoxNum + "',0,'" + OrderId + "')\">打印箱唛</div> </div> </td><td><div style='text-align:center; width:75px; border:solid 1px ;border-radius:3px;cursor:pointer;color:white;background-color:#5bc0de'  onmouseover =\"ShowsIn('" + BoxNum + "',this)\" >打印</div></td>"
        $(".counts").first()[0].children.ScanSKU.focus();
        boxchanged($('select[id=box]')[1]);
        //document.getElementById('packagesModel').scrollIntoView();
        document.getElementsByTagName("td")[0].scrollIntoView();
    });

    $("#ScanSKU").live('keydown', function () {
        if (event.keyCode == 13) {
            if ($(this).val() == "") {
                showMsg("扫描数据不能为空!", "4000");
                PlaySound();
                return false;
            }
            if ($("#chkZero").get(0).checked == true) {
                $(this).val("00" + $(this).val());
            }

            var inputsku = $(this);




            inputsku.append("aa");
            if (CheckQTYOver(inputsku.val())) {
                showMsg("SKU已满足数量!", "4000");
                PlaySound();
                return false;
            }
            inputsku.select();
            var thistr = inputsku.parent()[0].children.resultTableModel;
            if (thistr == undefined) {
                thistr = $(inputsku.parent()[0].children.resultTable)[0];
            }
            if (thistr.rows.length <= 1) {
                addNewRow($(thistr.rows[0]).children().eq(0))
            }
            var rows_count = thistr.rows.length - 1;
            //初始的时候，直接查询数据并赋值
            if (rows_count == 1 && $(thistr.rows[1]).children().eq(0).find("input").val().trim() == "") {
                if (CheckSKU(inputsku.val())) {
                    $(thistr.rows[1]).children().eq(0).find("input").val(nowsku[0])
                    $(thistr.rows[1]).children().eq(1).find("input").val(nowsku[1])
                    $(thistr.rows[1]).children().eq(2).find("input").val(nowsku[3])
                    //$(thistr.rows[1]).children().eq(3).find("input").val(nowsku[2])//描述
                    $(thistr.rows[1]).children().eq(3).find("input").val("")//描述
                    $(thistr.rows[1]).children().eq(4).find("input").val(1);
                }
                else {
                    showMsg("SKU不存在!", "4000");
                    PlaySound();
                    return false;
                }
                //$.ajax({
                //    url: "/WMS/OrderManagement/GetSKUlist",
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

                BoxQtySum($(this).parent());//把父节点传进方法计算此箱总数

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
                    //$(thistr.rows[rows_count]).children().eq(3).find("input").val(nowsku[2])//描述
                    $(thistr.rows[rows_count]).children().eq(3).find("input").val("")//描述
                    $(thistr.rows[rows_count]).children().eq(4).find("input").val(1)

                    QtyChanged();
                    BoxQtySum($(this).parent());
                    return true;
                }
                else {
                    showMsg("SKU不存在!", "4000");
                    PlaySound();
                    return false;
                }
                //$.ajax({
                //    url: "/WMS/OrderManagement/GetSKUlist",
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
            BoxQtySum($(this).parent());
        }
    });
    $("#SKU").live('keydown', function () {
        $(this).autocomplete({
            source: function (request, response) {
                if (request.term.length > 5) {
                    $.ajax({
                        url: "/WMS/OrderManagement/GetSKUlist",
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
    //判断 如果第一箱箱号是空的 则获取最新箱号赋值给他
});

//单箱数量改变时汇总(1扫描SKU，2删除行使用，3手动输入数量)
function BoxQtySum(par) {
    if (par == undefined) {//三种调用传进来的都是父节点div
        return false;
    }

    //计算此箱总数量
    var boxQty = 0;
    var text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 该箱总件数：";//文本前摇
    for (var i = 0; i < $(par).children()[5].rows.length; i++) {
        if (i != 0) {//表头不计
            if ($(par).children()[5].rows[i].cells[4].children.Qty.value != '') {                     //QTY列需非空或非数字
                if (parseInt($(par).children()[5].rows[i].cells[4].children.Qty.value) != NaN) {
                    boxQty += parseInt($(par).children()[5].rows[i].cells[4].children.Qty.value);
                }
            }
        }
    }
    $(par).children()[3].innerHTML = text + boxQty;
}

function QtyChanged(obj) {
    if (obj != undefined) {

        if (isNaN($(obj).val())) {
            showMsg("请输入数字！", 4000);
            $(obj).focus();
            $(obj).select();
            return;
        }
        var a = $(obj).val();//若输入空值，赋值0
        if (a == "") {
            $(obj).val("0");
        }
        //调用此箱数量改变事件
        BoxQtySum($(obj).parent().parent().parent().parent().parent());
    }

    var qty = parseInt($("#totalcount")[0].innerHTML.toString().substring(5, $("#totalcount")[0].innerHTML.length));
    qty = qty - qty;
    $(".counts").each(function (a, b) {
        for (var i = 0; i < $(b).children()[5].rows.length; i++) {
            if (i != 0) {
                if ($(b).children()[5].rows[i].cells[4].children.Qty.value != '') {
                    if (parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value) != NaN) {
                        qty += parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value);
                    }
                }
            }
        }
    });

    //当某个SKU输入的数量大于订单数量时提示超出
    var singleSKU = "";
    var singleQty = 0;//当前页面这个SKU的总数量
    if (obj != undefined) {
        if (!isNaN($(obj).val())) {
            var tr = $(obj).parent().parent();//父节点tr
            singleSKU = $(tr).find('#SKU').val();//获取输入数量的SKU           
        }
    }
    $(".counts").each(function (a, b) {
        for (var i = 0; i < $(b).children()[5].rows.length; i++) {
            if (i != 0) {
                if ($(b).children()[5].rows[i].cells[0].children.SKU.value == singleSKU) {

                    if (parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value) != obj) {
                        singleQty += parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value);
                    }
                }
            }
        }
    });
    var orderdetaillist = eval($("#OrderDetail").val());
    $.each(orderdetaillist, function (index, item) {
        if (singleSKU == item.SKU && singleQty == item.Qty) {
            showMsg("SKU已满足数量!", "4000");

        }
        else if (singleSKU == item.SKU && singleQty > item.Qty) {
            showMsg("SKU：" + singleSKU + "数量超出，请检查！", "4000");
            PlaySound();

        }
    });


    $("#packagecount")[0].innerHTML = "已完成数量:" + qty;
    var qtytotal = parseInt($("#totalcount")[0].innerHTML.toString().substring(5, $("#totalcount")[0].innerHTML.length));
    $("#lastcount")[0].innerHTML = "剩余数量:" + (qtytotal - qty);
    if (qty == qtytotal) {
        $("#msgtd")[0].innerHTML = "订单已完成";
        $("#msgtd").css("color", "green")
    }
    else if (qty > qtytotal) {
        $("#msgtd")[0].innerHTML = "已超出订单数量";
        $("#msgtd").css("color", "red")
    }
    else {
        $("#msgtd")[0].innerHTML = "";
    }
}
function boxchanged(obj) {
    var str = $(obj);

    var array = str.val().split(',');
    $(str).parent().parent().next().find('#Length').val(array[0]);
    $(str).parent().parent().next().find('#Width').val(array[1]);
    $(str).parent().parent().next().find('#Height').val(array[2]);
}
//验证SKU是否完成
function CheckQTYOver(obj) {

    var IsSuss = false;
    var totalqty = 0;//当前页面该SKU总数量
    $(".counts").each(function (a, b) {
        for (var i = 0; i < $(b).children()[5].rows.length; i++) {
            if (i != 0) {
                if ($(b).children()[5].rows[i].cells[0].children.SKU.value == obj) {
                    var isexist = false;
                    if (parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value) != obj) {
                        totalqty += parseInt($(b).children()[5].rows[i].cells[4].children.Qty.value);
                    }
                }
            }
        }
    });

    var orderdetaillist = eval($("#OrderDetail").val());
    $.each(orderdetaillist, function (index, item) {
        if (obj == item.SKU && totalqty >= item.Qty) {
            IsSuss = true;
        }
    });
    return IsSuss;
}

//播放声音
function PlaySound() {
    $("#Audio")[0].play();

}
var nowsku = new Array();
function CheckSKU(obj) {
    var resualt = false;
    var orderdetaillist = eval($("#OrderDetail").val());
    $.each(orderdetaillist, function (index, item) {
        if (item.SKU == obj || item.UPC == obj) {
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
                    chayilist[chayileng][3] = parseInt(item2[1]) - parseInt(item.Qty);
                    chayilist[chayileng][4] = item.Article;
                    chayilist[chayileng][5] = item.Size;
                    chayilist[chayileng][6] = item.Location;
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
            chayilist[chayileng][3] = 0 - parseInt(item.Qty);

            chayilist[chayileng][4] = item.Article;
            chayilist[chayileng][5] = item.Size;
            chayilist[chayileng][6] = item.Location;
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
        $("#showtable2").append("<tr><td>SKU</td><td>Article Size</td><td>库位</td><td>订单数量</td><td>包装数量</td><td>差异数量</td></tr>");
        $.each(data, function (index, item) {
            var chayiqty = item[3];
            if (chayiqty > 0) {
                $("#showtable2").append("<tr><td >" + item[0] + "</td><td >" + item[4] + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + item[5] + "</td><td >" + item[6] + "</td><td>" + item[1] + "</td><td>" + item[2] + "</td><td >" + "+" + (item[3]) + "</td></tr>")
            }
            else {
                $("#showtable2").append("<tr><td >" + item[0] + "</td><td >" + item[4] + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + item[5] + "</td><td >" + item[6] + "</td><td>" + item[1] + "</td><td>" + item[2] + "</td><td >" + (item[3]) + "</td></tr>")

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
    td6.append($("<input type='text'  class='form-control' style='width:100%' id='UPC'  readonly='true' /> "));
    row.append(td1);
    row.append(td6);
    row.append(td2);
    row.append(td3);
    row.append(td4);
    row.append(td5);
    table1.append(row);
}
function addNewRow(o) {
    var table1 = $(o[0].parentNode.parentNode.parentNode);
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
function deleteNew(o) {
    var rows = $(o.parentNode.parentNode.parentNode.parentNode)[0].rows.length - 1;
    //if (rows <= 1) {
    //    showMsg("请至少保留一行！", 4000);
    //    return;
    //}
    var tr = $(o.parentNode.parentNode);
    var pardiv = $(o).parent().parent().parent().parent().parent();
    tr.remove();
    QtyChanged();
    BoxQtySum(pardiv);
}

function deleteDiv(obj) {

    layer.confirm('<font size="4">确认是否删除本箱？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,

        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        var sums = $(".counts").length;
        //var froms = $(obj).parent().parent().parent().parent().parent().prevAll().length - 2;
        //var lengths = $(obj).parent().parent().parent().parent().parent().nextAll().length;

        if (sums == 1) {
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
        //先删除数据库的packagenumber
        //var packnum = $(obj.parentNode.parentNode.parentNode)[0].getElementsByTagName("td")[10].children[0].value;
        //if ($(obj.parentNode.parentNode.parentNode)[0].getElementsByTagName("td").length > 16) {
        //    packnum = $(obj.parentNode.parentNode.parentNode)[0].getElementsByTagName("td")[12].children[0].value;
        //}
        packnum = $(obj.parentNode.parentNode.parentNode)[0].getElementsByTagName("td")[12].children[0].value;
        var isOk = DeletePackInfo(packnum);
        if (isOk) {
            var s = $(obj.parentNode.parentNode.parentNode.parentNode.parentNode).remove();
            var bcount = $(".counts").length;
            $(".counts").each(function (a, b) {
                $(b)[0].children[0].innerHTML = "第" + (bcount - a) + "箱";
            });
        }
        else {
            showMsg("删除失败，请重试！", 4000);
            return;
        }
        QtyChanged();
    });
}
//删除包装信息
function DeletePackInfo(packagenumber) {
    var isDelete = false;
    $.ajax({
        type: "Post",
        url: "/WMS/OrderManagement/DeletePackInfo",
        data: { "PackageKey": packagenumber },
        async: false,
        success: function (data) {
            if (data == "") {
                isDelete = true;
            }
        },
        error: function (msg) {
            alert("网络连接失败！");
        }
    });
    return isDelete;
}
//打印托运单
function printDiv(id, type, OrderID) {
    //$.ajax({
    //    type: "Post",
    //    url: "/WMS/OrderManagement/print",
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
    //if ($("#CustomerName").val().toString().indexOf("NIKE") > -1) {
    //    window.location.href = "/WMS/NikeOSRBJPrint/PrintPod?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    //}
    //else
    if ($("#CustomerName").val().toString().indexOf("YXDR") > -1) {
        var href = "/WMS/YXDRPackagePrint/PrintPod?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    else if ($("#CustomerName").val().toString().indexOf("爱库存") > -1) {
        var href = "/WMS/YXDRPackagePrint/PrintPod?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    else {
        var href = "/WMS/NikeOSRBJPrint/PrintPod?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    layer.open({
        type: 2,
        title: '打印托运单',
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['800px', '600px'],
        content: href,
        move: '.layui-layer-title',
        moveOut: true
    });

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
function printboxDiv(id, type, OrderID) {
    if ($("#CustomerName").val().toString().indexOf("YXDR") > -1) {
        var href = "/WMS/YXDRPackagePrint/PrintBoxList?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    else if ($("#CustomerName").val().toString().indexOf("爱库存") > -1) {
        var href = "/WMS/YXDRPackagePrint/PrintBoxListAKC?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    else {
        var href = "/WMS/NikeOSRBJPrint/PrintBox?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    layer.open({
        type: 2,
        title: '打印箱清单',
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['800px', '600px'],
        content: href,
        move: '.layui-layer-title',
        moveOut: true
    });
}


//打印总箱单
function printtotalboxDiv(id, type, OrderID) {
    if ($("#CustomerName").val().toString().indexOf("YXDR") > -1) {
        var href = "/WMS/YXDRPackagePrint/PrintTotalBoxList?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    else if ($("#CustomerName").val().toString().indexOf("爱库存") > -1) {
        var href = "/WMS/YXDRPackagePrint/PrintTotalBoxList?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    else {
        var href = "/WMS/YXDRPackagePrint/PrintTotalBoxList?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    layer.open({
        type: 2,
        title: '打印总箱单',
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['800px', '600px'],
        content: href,
        move: '.layui-layer-title',
        moveOut: true
    });
}

//打印箱唛
function printboxmDiv(id, type, OrderID) {
   
    if ($("#CustomerName").val().toString().indexOf("YXDR") > -1) {
        var href = "/WMS/YXDRPackagePrint/PrintCarton?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    else if ($("#CustomerName").val().toString().indexOf("爱库存") > -1) {
        var href = "/WMS/YXDRPackagePrint/PrintCarton?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    else {
        var href = "/WMS/NikeOSRBJPrint/PrintBoxm?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    layer.open({
        type: 2,
        title: '打印箱唛',
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['800px', '600px'],
        content: href,
        move: '.layui-layer-title',
        moveOut: true
    });
}
//YXDR打印报关箱唛 
function PrintCustomsCarton(id, type, OrderID) {

    var href = "/WMS/YXDRPackagePrint/PrintCustomsCarton?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    layer.open({
        type: 2,
        title: '打印报关箱唛',
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['800px', '600px'],
        content: href,
        move: '.layui-layer-title',
        moveOut: true
    });
}

//导出箱清单
function ExportBoxDetails(id, type, OrderID) {
    if ($("#CustomerName").val().toString().indexOf("YXDRSHH") > -1) {
        window.location.href = "/WMS/YXDRPackagePrint/ExportBoxDetail?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    else if ($("#CustomerName").val().toString().indexOf("YXDRBJ") > -1) {
        window.location.href = "/WMS/YXDRPackagePrint/ExportBoxDetail?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
    if ($("#CustomerName").val().toString().indexOf("爱库存") > -1) { //爱库存。什么鬼哦
        window.location.href = "/WMS/YXDRPackagePrint/ExportBoxDetail?id=" + id + "&type=" + type + "&OrderID=" + OrderID;
    }
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
        url: "/WMS/OrderManagement/Outs",
        data: {
            "ID": $("#OrderID").val(),
            type: 1
        },
        async: "false",
        success: function (data) {
            if (data == "") {
                showMsg("出库完成！", 4000);
                location.href = "/WMS/OrderManagement/Index"
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
//获取系统箱号
function GetMaxBox() {
    var Boxnumber = "";
    //$("#CustomerName").val()
    $.ajax({
        type: "Post",
        url: "/WMS/OrderManagement/GetMaxBoxnumber",
        data: {
            "OrderID": $("#OrderID").val()
        },
        async: false,
        success: function (data) {
            Boxnumber = data;
            //return Boxnumber;
        },
        error: function (msg) {
            alert(msg.val);
        }

    });
    return Boxnumber
}

function addQty(data, inputsku) {
    var thistr = inputsku.parent().parent().parent().parent()[0];
    var row_count = thistr.rows.length - 1;
    var Qty = 0;

    var isfind = false;//是否找到扫描的sku
    var skuline = 0;//找到的sku line
    var qtyline = 0;//空行的line
    for (var i = 1; i < row_count; i++) {

        if (inputsku == $(thistr.rows[i]).children().eq(0).find("input").val().trim() && $(thistr.rows[i]).children().eq(3).find("input").val().trim() != "") {
            isfind = true;
            skuline = i;
        }
        else {
            if ($(thistr.rows[i]).children().eq(3).find("input").val().trim() == "") {
                qtyline = i;
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

//定位箱号的位置
function SearchBox() {
    if (event.keyCode == 13) {
        var Boxno = $("#SearchBoxID").val();
        $(".counts").each(function (a, b) {
            if (b.children[0].innerHTML.toString().replace("第", "").replace("箱", "") == Boxno) {
                document.getElementById(b.id.toString()).scrollIntoView();

                //$(b.children[0]).animate({ scrollTop: $(b.children[0]).offset().top + 100000 }, 1000);
            }
        });
        //document.getElementById('packagesTRB20170306349900138').scrollIntoView();
    }
}

