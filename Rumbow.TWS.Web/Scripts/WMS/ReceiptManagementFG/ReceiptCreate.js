$(document).ready(function () {
    //$('#ASNNumber').attr("ondblClick", "show()")
    //if ($("#ViewTypes").val() == "3")
    //{
    //    $("#NewDiv").removeAttr("style");
    //    $("#Newtable").removeAttr("style");
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
    $('#selectAll').click(function () {
        var checkBoxs = $("#Newtable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

    });
    $("#Newtable tbody input[type='checkbox']").live('click', function () {
        RefreshIDs();
    });
    var RefreshIDs = function () {
        var checkBoxs = $("#Newtable tbody input[type='checkbox']");
        var length = checkBoxs.length;
        var checked = 0;
        checkBoxs.each(function () {
            if ($(this).attr("checked") === "checked") {
                checked++;
            }
        });
        if (checked == checkBoxs.length) {
            $('#selectAll').attr("checked", "checked");
        } else {
            $('#selectAll').removeAttr("checked");
        }
    }

    $('#printBtn').live('click', function () {
        var checkBoxs = $("#Newtable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                sql += checkBoxs[i].name.toString() + ",";

            }
        }
        if (sql.length > 0) {
            $.ajax({
                type: "Post",
                url: "/WMS/ReportManagement/GetPrintLabel",
                data: {
                    "ID": sql.substring(0, sql.length - 1)
                },
                async: "false",
                success: function (data) {
                    if (data.ErrorCode == "1") {
                        //LODOP = getLodop();
                        //for (var i = 0; i < data.Response.length; i++) {
                        //    var QRClode = '[{"GoodsName": ' + data.Response[i].GoodsName + ', "SKU": ' + data.Response[i].SKU + ', "ProductionDate": "", "ExpirationDate": "", "BatchNumber": ' + data.Response[i].BatchNumber + ', "Manufacturer":' + data.Response[i].Manufacturer + ', "BoxNumber": ' + data.Response[i].BoxNumber + ', "QtyExpected":' + data.Response[i].QtyExpected + ', "NetWeight": "", "GrossWeight": ""  }]';
                        //    LODOP.PRINT_INIT("");
                        //    LODOP.SET_PRINT_PAGESIZE(1, 970, 700, "");
                        //    AddPrintContent(data.Response[i], QRClode);
                        //    LODOP.PRINT();
                        //}
                        var html = $("#Evaluation").render(data.Response);
                        $("#DisInfoBody")["empty"]();
                        $("#DisInfoBody").append(html);
                        doPrint("打印")


                    }
                    else { }

                },
                error: function (msg) {
                    alert(msg.val);
                }

            });
        }
        else {
            showMsg("请勾选需要打印的标签！", 4000);

        }
    });
    $('#labelRemove1').live('click', function () {
        var length = $(this).parent().parent().nextAll().length;
        var LineNumber = 0;
        var row_number = 0;
        for (i = 0; i < length; i++) {
            row_number = parseInt($(this).parent().parent().nextAll()[i].rowIndex) - 1;
            LineNumber = returnlinenumber(row_number);
            $("#Newtable tr:eq(" + parseInt($(this).parent().parent().nextAll()[i].rowIndex) + ") td:nth-child(1)").html(LineNumber);
        }
        $(this).parent().parent().remove();

    });

    $('#labelRemove2').live('click', function () {
        var length = $(this).parent().parent().nextAll().length;
        var LineNumber = 0;
        var row_number = 0;
        for (i = 0; i < length; i++) {
            row_number = parseInt($(this).parent().parent().nextAll()[i].rowIndex) - 1;
            LineNumber = returnlinenumber(row_number);
            $("#Newtable tr:eq(" + parseInt($(this).parent().parent().nextAll()[i].rowIndex) + ") td:nth-child(1)").html(LineNumber);
        }
        $(this).parent().parent().remove();

    });



    $('#returnButton').live('click', function () {
        if ($("#PageType").val() == "1") {
            location.href = "/WMS/ASNManagement/Index";
        }
        else {
            window.history.back();
        }
        //location.href = "/WMS/ReceiptManagementFG/Index"

        //$.post("/WMS/ReceiptManagementFG/Index", { "IndexViewModel": null, "Action": "查询" });
        //post('/WMS/ReceiptManagementFG/Index', { IndexViewModel: null, Action: '查询' });


    });
    $('#ReturnButton_History').live('click', function () {
        history.back();
    });

    $('#returnButton_add').live('click', function () {
        history.back();
    });
    $('#returnButtonEdit').live('click', function () {
        history.back();
    });


    function Return(ViewType) {
        if (ViewType == 2) {
            post('/WMS/ReceiptManagementFG/Index', { IndexViewModel: null, Action: '查询' });
        }
        else {
            window.history.back();
        }
    }
    function Shelves(ID) {
        location.href = "/WMS/ShelvesManagement/ReceiptReceivingDetail/?RID=" + ID
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
    $('#statusBackOK').live('click', function () {
        closePopup();
        if ($('#editReturn')[0].innerText == '添加成功！') {
            location.href = "/WMS/ReceiptManagementFG/Index"
        }
    });

    $('#NewPrint').live('click', function () {
        openPopup('NewPrintpopp', true, 350, 300, null, 'statusBackDiv');
        $("#popupLayer_NewPrintpopp")[0].style.top = "200px";
        var skuAndbatch = $("#AllSku option:selected").val().split("/");
        var sku = skuAndbatch[0];
        var batchs = skuAndbatch[1];
        var id = $("#ReceiptID").val();
        $.ajax({
            type: "Post",
            url: "/WMS/ReceiptManagementFG/GetSkuTotal",
            data: {
                "ID": id,
                "SKU": sku,
                "BoxNumber": '',
                "Batchs": batchs
            },
            async: "false",
            success: function (data) {
                $("#AllSum").val(data);
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });
    $('#UpdatePrint').live('click', function () {
        openPopup('UpdatePrintpopp', true, 350, 300, null, 'statusBackDiv2');
        $("#popupLayer_UpdatePrintpopp")[0].style.top = "200px";
        var BoxNumber = $("#AllSku2 option:selected").val();
        var id = $("#ReceiptID").val();
        $.ajax({
            type: "Post",
            url: "/WMS/ReceiptManagementFG/GetSkuTotal",
            data: {
                "ID": id,
                "SKU": '',
                "BoxNumber": BoxNumber
            },
            async: "false",
            success: function (data) {
                $("#AllSum2").val(data);
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });



    $('select[id=AllSku]').live('change', function () {
        var skuAndbatch = $("#AllSku option:selected").val().split("/");
        var sku = skuAndbatch[0];
        var batchs = skuAndbatch[1];
        var id = $("#ReceiptID").val();
        $.ajax({
            type: "Post",
            url: "/WMS/ReceiptManagementFG/GetSkuTotal",
            data: {
                "ID": id,
                "SKU": sku,
                "BoxNumber": '',
                "Batchs": batchs
            },
            async: "false",
            success: function (data) {
                $("#AllSum").val(data);
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });

    $('select[id=AllSku2]').live('change', function () {
        var BoxNumber = $("#AllSku2 option:selected").val();
        var id = $("#ReceiptID").val();
        $.ajax({
            type: "Post",
            url: "/WMS/ReceiptManagementFG/GetSkuTotal",
            data: {
                "ID": id,
                "SKU": '',
                "BoxNumber": BoxNumber
            },
            async: "false",
            success: function (data) {
                $("#AllSum2").val(data);
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });

});
var row_count = 1;
function addNew() {
    var table1 = $('#Newtable');
    var rownumber = $('#Newtable tr').length;
    var firstTr = table1.find('tbody>tr:first');
    var row = $("<tr id=Row" + rownumber + "></tr>");
    var td1 = $("<td id=td" + rownumber + "></td>");
    var td2 = $("<td></td>");
    var td3 = $("<td></td>");
    var td4 = $("<td></td>");
    var td5 = $("<td></td>");
    var td6 = $("<td></td>");
    td1.append($("<b>0000" + rownumber + "</b>")
    );
    td2.append($("<input type='text' name='count' value=''>")
   );
    td3.append($("")
  );
    td4.append($("<input type='text' name='count' value='' onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)'>")
  );
    td5.append($("<input type='text' name='count' value=''>")
  );
    td6.append($("<label class='labelRemove'  style='cursor:pointer; color:blue'>删除</label>")
  );
    row.append(td1);
    row.append(td2);
    row.append(td3);
    row.append(td4);
    row.append(td5);
    row.append(td6);
    table1.append(row);
    row_count++;
}

function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    if (pattern.test(hehe.value)) {
        hehe.value = hehe.value.replace(pattern, "");
    }
}



$("#statusBackReturn").live('click', function () {
    closePopup();
});
$("#statusBackReturn2").live('click', function () {
    closePopup();
});

function ReceiptDetailDelete() {
    $(this).parent().parent().remove();
}
function submitCheck() {
    var flag = true;
    var table = document.getElementById("Newtable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        if ($(row[j]).children(".QtyExpected").children("input").val() != "" && parseInt($(row[j]).children(".QtyExpected").children("input").val()) == 0) {
            flag = false;
        }

        //for (var i = 6; i < col.length; i++) {
        //    var tds = row[j].getElementsByTagName("td");
        //    //if (tds[7].children[0].value.trim() == "0.00" || tds[7].children[0].value.trim() == "" || tds[7].children[0].value.trim() == "0") {
        //    //    flag = false;
        //    //}
        //    if(tds)
        //}
    }
    return flag;
}



function AddReceiptAndReceiptDetail() {

    if (!submitCheck()) {
        showMsg("期望数量不能为0！", 4000);
        return false;
    }
    var JsonField = FieldSetToJson();
    var JsonTable = TableToJson();
    var AsnNumber = $('#ASNNumber').val();
    var ASNID = $('#ASNID').val();
    var ExternReceiptNumber = $('#ExternReceiptNumber').val();
    var CustomerName = $('#CustomerName').val();
    var CustomerID = $('#CustomerID').val();
    var Receipttype = $('#ReceiptType').val();
    var ReceiptDate = $('#receipt_ReceiptDate').val();
    var WarehouseID = $('#WarehouseID').val();
    var WarehouseName = $('#WarehouseName').val();
    $.ajax({
        type: "Post",
        url: "/WMS/ReceiptManagementFG/AddReceiptAndReceiptDetail",
        data: {
            "JsonTable": JsonTable,
            "ASNNumber": AsnNumber,
            "ASNID": ASNID,
            "ExternReceiptNumber": ExternReceiptNumber,
            "CustomerName": CustomerName,
            "CustomerID": CustomerID,
            "Receipttype": Receipttype,
            "ReceiptDate": ReceiptDate,
            "JsonField": JsonField,
            "WarehouseID": WarehouseID,
            "WarehouseName": WarehouseName

        },
        async: "false",
        success: function (data) {
            if (data.Errorcode == 1) {
                //$('#editReturn')[0].innerText = "添加成功！"
                //openPopup("pop3", true, 350, 200,null, 'editReturnDiv',true)
                //showMsg("添加成功！", "10000");
                location.href = "/WMS/ReceiptManagementFG/ReceiptCreate/?ID=" + data.Result + "&ViewType=3&Flag=1&PageType=1"
            } else {
                showMsg("添加失败！请联系IT检查配置文件", 4000);
            }
        },
        error: function (msg) {
            alert(msg.val);
        }

    });
}

function EditReceiptAndReceiptDetail(ID) {
    if (!submitCheck()) {
        showMsg("期望数量不能为0！", 4000);
        return false;
    }
    var ReceiptDate = $('#receipt_ReceiptDate').val();
    var ReceiptType = $('#ReceiptType').val();
    var ASNID = $('#ASNID').val();
    var ASNNumber = $('#ASNNumber').val();
    var ExternReceiptNumber = $('#ExternReceiptNumber').val();
    var CustomerName = $('#CustomerName').val();
    var CustomerID = $('#CustomerID').val();
    var ReceiptNumber = $('#ReceiptNumber').val();
    var JsonTable = TableToJson();
    var JsonField = FieldSetToJson();
    $.ajax({
        type: "Post",
        url: "/WMS/ReceiptManagementFG/EditReceiptAndReceiptDetail",
        data: {
            "JsonTable": JsonTable,
            "ID": ID,
            "ReceiptDate": ReceiptDate,
            "ReceiptType": ReceiptType,
            "CustomerID": CustomerID,
            "CustomerName": CustomerName,
            "ReceiptNumber": ReceiptNumber,
            "ASNID": ASNID,
            "ASNNumber": ASNNumber,
            "ExternNumber": ExternReceiptNumber,
            "JsonField": JsonField
        },
        async: "false",
        success: function (data) {
            if (data == "更新成功") {
                //$('#editReturn')[0].innerText="更新成功！"
                // openPopup("pop3", true, 350, 200, null, 'editReturnDiv', true)
                location.href = "/WMS/ReceiptManagementFG/Index"
                //location.href = "/WMS/ReceiptManagementFG/ReceiptCreate/?ID=" + ID + "&ViewType=3&Flag=1"
                showMsg("更新成功！", 4000);
            }

        },
        error: function (msg) {
            alert(msg.val);
        }

    });
}

function AddButtonAgain(CustomerID) {
    location.href = "/WMS/ReceiptManagementFG/ReceiptCreate/?CustomerID=" + CustomerID + "&ViewType=0"
}

function UpdateButton(ID) {
    location.href = "/WMS/ReceiptManagementFG/ReceiptCreate/?ID=" + ID + "&ViewType=2"
}

function TableToJson() {
    var JsontoModel =
  {
      行号: 'LineNumber', SKU: 'SKU', 描述: 'GoodsName', 期望数量: 'QtyExpected', ID: 'ID'
  }
    var txt = "[";
    var table = document.getElementById("Newtable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 2; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if (col[i].innerHTML.trim() != "操作") {
                if (col[i].innerHTML.trim() != "行号" && col[i].innerHTML.trim() != "SKU" && col[i].innerHTML.trim() != "货品等级" && col[i].innerHTML.trim() != "产品名称" && col[i].innerHTML.trim() != "ID" && col[i].innerHTML.trim() != "预收数量") {
                    if (tds[i].childNodes.length > 1) {
                        if (tds[i].childNodes[1].type == 'checkbox') {
                            r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + (tds[i].childNodes[1].checked == true ? 1 : 0) + "\"!,";
                        }
                        else {
                            r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].childNodes[1].value.trim() + "\"!,";
                        }
                    }
                    else {
                        r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].innerHTML.trim() + "\"!,";
                    }
                }
                else {

                    r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].innerHTML.trim() + "\"!,";
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



function FieldSetToJson() {
    var txt = "[";
    var table = document.getElementById("FieldReceiptID");
    var row = table.getElementsByTagName("tr");
    if (row.length > 1) {
        var r = "{";
        for (var j = 1; j < row.length; j++) {
            var col = row[j].getElementsByTagName("td");

            for (var i = 0; i < col.length; i++) {
                var tds = row[j].getElementsByTagName("td");
                if (tds[i].className.trim() != "TableColumnTitle" && tds[i].innerHTML.trim() != "") {
                    if (tds[i].childNodes[1].type == 'checkbox') {
                        r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + (tds[i].childNodes[1].checked == true ? 1 : 0) + "\"!,";
                    }
                    else {
                        r += "\"" + tds[i].childNodes[1].id.trim() + "\"\:\"" + tds[i].childNodes[1].value + "\"!,";
                    }

                }
            }

        }
        r = r.substring(0, r.length - 2)
        r += "}";
        txt += r;
    }
    //txt = txt.substring(0, txt.length);
    txt += "]";
    return txt;
}



function returnlinenumber(i) {
    var linnumber = "";
    if (i < 10) {
        linnumber = "0000" + i;
    }
    if (100 > i && i >= 10) {
        linnumber = "000" + i;
    }
    if (1000 > i && i >= 100) {
        linnumber = "00" + i;
    }
    if (i >= 1000) {
        linnumber = "0" + i;
    }
    return linnumber;
}



function SelectASN(CustomerID) {
    openPopup("", true, 1000, 600, '/WMS/ReceiptManagementFG/ASNQuery/?CustomerID=' + CustomerID, null, function (ASNID, CustomerID) {
        //$('#ASNNumber').val(box.ASNNumbers);
        //$('#ASNID').val(ASNID);
        //$('#ExternReceiptNumber').val(box.ExternReceiptNumbers);
        //$('#CustomerName').val(box.CustomerNames);
        //$('#CustomerID').val(CustomerID);
        //$('#ASNType').val(box.ASNTypes);
        location.href = "/WMS/ReceiptManagementFG/ReceiptCreate/?ID=" + ASNID + "&ViewType=1&CustomerID=" + CustomerID
    });
}


function Shelves(ID) {
    location.href = "/WMS/ShelvesManagement/ReceiptReceivingDetail/?RID=" + ID
}

$("#prinLabel").live('click', function () {
    var ViewType = $("#ViewTypes").val();
    var AllSum = $("#AllSum").val();
    var EverySum = $("#OneSum").val();

    if (AllSum == "0" || AllSum == '') {
        showMsg("请填写总数！", 4000);
        return;
    }
    if (EverySum == "0" || EverySum == '') {
        showMsg("请填写每拖数！", 4000);
        return;
    }
    $.ajax({
        type: "Post",
        url: "/WMS/ReportManagement/PrintUpdateReceipt",
        data: {
            "ID": $("#ReceiptID").val(),
            "SKU": $("#AllSku option:selected").val(),
            "AllSum": AllSum,
            "EverySum": EverySum

        },
        async: "false",
        success: function (data) {
            if (data.ErrorCode == "1") {
                closePopup();
                //LODOP = getLodop();
                //for (var i = 0; i < data.Response.length; i++) {
                //    var QRClode = '[{"GoodsName": ' + data.Response[i].GoodsName + ', "SKU": ' + data.Response[i].SKU + ', "ProductionDate": "", "ExpirationDate": "", "BatchNumber": ' + data.Response[i].BatchNumber + ', "Manufacturer":' + data.Response[i].Manufacturer + ', "BoxNumber": ' + data.Response[i].BoxNumber + ', "QtyExpected":' + data.Response[i].QtyExpected + ', "NetWeight": "", "GrossWeight": ""  }]';
                //    LODOP.PRINT_INIT("");
                //    LODOP.SET_PRINT_PAGESIZE(1, document.getElementById('W1').value, document.getElementById('H1').value, "A4");
                //    AddPrintContent(data.Response[i], QRClode);
                //    LODOP.PRINT();
                //}
                location.href = "/WMS/ReceiptManagementFG/ReceiptCreate/?ID=" + $("#ReceiptID").val() + "&ViewType=" + ViewType
            }
            else { }

        },
        error: function (msg) {
            alert(msg.val);
        }

    });

});
//Base64解码
//strIn，输入字符串
//成功返回一个数组，每一个元素包含一字节信息
//失败返回null
//function decodeBase64(strIn) {
//    if (!strIn.length || strIn.length % 4)
//        return null;
//    var str64 =
//        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
//    var index64 = [];
//    for (var i = 0; i < str64.length; i++)
//        index64[str64.charAt(i)] = i;
//    var c0, c1, c2, c3, b0, b1, b2;
//    var len = strIn.length;
//    var len1 = len;
//    if (strIn.charAt(len - 1) == '=')
//        len1 -= 4;
//    var result = [];
//    for (var i = 0, j = 0; i < len1; i += 4) {
//        c0 = index64[strIn.charAt(i)];
//        c1 = index64[strIn.charAt(i + 1)];
//        c2 = index64[strIn.charAt(i + 2)];
//        c3 = index64[strIn.charAt(i + 3)];
//        b0 = (c0 << 2) | (c1 >> 4);
//        b1 = (c1 << 4) | (c2 >> 2);
//        b2 = (c2 << 6) | c3;
//        result.push(b0 & 0xff);
//        result.push(b1 & 0xff);
//        result.push(b2 & 0xff);
//    }
//    if (len1 != len) {
//        c0 = index64[strIn.charAt(i)];
//        c1 = index64[strIn.charAt(i + 1)];
//        c2 = strIn.charAt(i + 2);
//        b0 = (c0 << 2) | (c1 >> 4);
//        result.push(b0 & 0xff);
//        if (c2 != '=') {
//            c2 = index64[c2];
//            b1 = (c1 << 4) | (c2 >> 2);
//            result.push(b1 & 0xff);
//        }
//    }
//    return result;
//}

//function printWithoutAlert() {
//    document.all.WebBrowser.ExecWB(6, 6);
//}
//function printSetup() {
//    document.all.WebBrowser.ExecWB(8, 1);
//}

//function printImmediately() {
//    document.all.WebBrowser.ExecWB(6, 6);
//    window.close();
//}

$("#prinLabel2").live('click', function () {
    var ViewType = $("#ViewTypes").val();
    var how = this;
    var EverySum = $("#OneSum2").val();

    if (EverySum == "0" || EverySum == '') {
        showMsg("请填写新数量！", 4000);
        return;
    }
    $.ajax({
        type: "Post",
        url: "/WMS/ReportManagement/PrintUpdateNumber",
        data: {
            "ID": $("#ReceiptID").val(),
            "BoxNumber": $("#AllSku2 option:selected").val(),
            "EverySum": EverySum

        },
        async: "false",
        success: function (data) {
            if (data.ErrorCode == "1") {
                closePopup();
                //LODOP = getLodop();
                //for (var i = 0; i < data.Response.length; i++) {
                //    var QRClode = '[{"GoodsName": ' + data.Response[i].GoodsName + ', "SKU": ' + data.Response[i].SKU + ', "ProductionDate": "", "ExpirationDate": "", "BatchNumber": ' + data.Response[i].BatchNumber + ', "Manufacturer":' + data.Response[i].Manufacturer + ', "BoxNumber": ' + data.Response[i].BoxNumber + ', "QtyExpected":' + data.Response[i].QtyExpected + ', "NetWeight": "", "GrossWeight": ""  }]';
                //    LODOP.PRINT_INIT("");
                //    LODOP.SET_PRINT_PAGESIZE(1, document.getElementById('W1').value, document.getElementById('H1').value, "A4");
                //    AddPrintContent(data.Response[i], QRClode);
                //    LODOP.PRINT();
                //}
                location.href = "/WMS/ReceiptManagementFG/ReceiptCreate/?ID=" + $("#ReceiptID").val() + "&ViewType=" + ViewType;
                //var html = $("#Evaluation").render(data.Response);
                //doPrint("打印");
            }
            else { }
        },
        error: function (msg) {
            alert(msg.val);
        }

    });

});


//-------------------------------------------------------二维码----------------------------------------------------------------------------












/** 
 * 生成二维码
 * text：待生成文字
 * type：中文还是英文，cn为中文
 * render：展示方式，table为表格方式
 * width：宽度
 * height：高度
 * 注：需要引入<@jsfile 'qrcode'/>
 */
//$.fn.qcode = function (options) {
//    if (options) {
//        var opt = {};
//        if (typeof options == 'string') {
//            opt.text = options;
//        } else {
//            if (options.text) opt.text = options.text;
//            if (options.type && options.type == 'ch') opt.text = qcodetochar(opt.text);
//            if (options.render && options.render == 'table') opt.render = options.render;
//            if (options.width) opt.width = options.width;
//            if (options.height) opt.height = options.height;
//        }

//        $(this).qrcode(opt);
//    }
//};
//function toUtf8(str) {
//    var out, i, len, c;
//    out = "";
//    len = str.length;
//    for (i = 0; i < len; i++) {
//        c = str.charCodeAt(i);
//        if ((c >= 0x0001) && (c <= 0x007F)) {
//            out += str.charAt(i);
//        } else if (c > 0x07FF) {
//            out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
//            out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
//            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
//        } else {
//            out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
//            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
//        }
//    }
//    return out;
//}
//-------------------------------------------打印-------------------------
var LODOP; //声明为全局变量  
//function Preview1() {
//    LODOP = getLodop();
//    LODOP.PRINT_INITA(0, 0, 522, 333, "打印控件功能演示_Lodop功能_自定义纸张1");
//    LODOP.SET_PRINT_PAGESIZE(0, document.getElementById('W1').value, document.getElementById('H1').value, "A4");
//    AddPrintContent("10101010101010", "郭德强");
//    LODOP.PREVIEW();
//};
//function Preview2() {
//    LODOP = getLodop();
//    LODOP.PRINT_INITA(0, 0, 522, 333, "打印控件功能演示_Lodop功能_自定义纸张2");
//    LODOP.SET_PRINTER_INDEX(getSelectedPrintIndex());
//    LODOP.SET_PRINT_PAGESIZE(1, document.getElementById('W1').value, document.getElementById('H1').value, "");
//    AddPrintContent("10101010101010", "郭德强");
//    LODOP.PREVIEW();
//};
//function Preview3() {
//    LODOP = getLodop();
//    LODOP.PRINT_INITA(0, 0, 522, 333, "打印控件功能演示_Lodop功能_自定义纸张3");
//    LODOP.SET_PRINTER_INDEX(getSelectedPrintIndex());
//    LODOP.SET_PRINT_PAGESIZE(0, 0, 0, getSelectedPageSize());
//    AddPrintContent("10101010101010", "郭德强");
//    LODOP.PREVIEW();
//};
//function Preview4() {
//    LODOP = getLodop();
//    LODOP.PRINT_INITA(0, 0, 522, 333, "打印控件功能演示_Lodop功能_自定义纸张4");
//    LODOP.SET_PRINTER_INDEX(getSelectedPrintIndex());
//    LODOP.SET_PRINT_PAGESIZE(1, document.getElementById('W4').value, document.getElementById('H4').value, "CreateCustomPage");
//    //LODOP.SET_PRINT_MODE("CREATE_CUSTOM_PAGE_NAME","我的纸张名");//对新建的纸张重命名
//    AddPrintContent("10101010101010", "郭德强");
//    LODOP.PREVIEW();
//};
//function Print5() {
//    LODOP = getLodop();
//    LODOP.PRINT_INIT("");
//    LODOP.SET_PRINT_PAGESIZE(1, document.getElementById('W1').value, document.getElementById('H1').value, "A3");
//    AddPrintContent("10101010101010", "郭德强");
//    LODOP.PRINT();
//    LODOP.PRINT_INIT("");
//    LODOP.SET_PRINT_PAGESIZE(1, document.getElementById('W1').value, document.getElementById('H1').value, "A3");
//    AddPrintContent("10101010101012", "于谦");
//    LODOP.PRINT();
//};
//function Preview6() {
//    LODOP = getLodop();
//    LODOP.PRINT_INIT("打印控件功能演示_Lodop功能_控制基本位置6");
//    LODOP.SET_PRINT_MODE("POS_BASEON_PAPER", true);
//    AddPrintContent("10101010101010", "郭德强");
//    LODOP.PREVIEW();
//};
//function SetPrint7() {
//    LODOP = getLodop();
//    LODOP.PRINT_INIT("");
//    if (LODOP.CVERSION) CLODOP.On_Return = function (TaskID, Value) { alert(Value); };
//    var strResult = LODOP.SET_PRINT_MODE("WINDOW_DEFPRINTER", getSelectedPrintIndex());
//    if (!LODOP.CVERSION) alert(strResult);
//};
//function SetPrint8() {
//    LODOP = getLodop();
//    LODOP.PRINT_INIT("");
//    if (LODOP.CVERSION) CLODOP.On_Return = function (TaskID, Value) { alert(Value); };
//    var strResult = LODOP.SET_PRINT_MODE("WINDOW_DEFPAGESIZE:" + getSelectedPrintIndex(), getSelectedPageSize());
//    //var strResult=LODOP.SET_PRINT_MODE("WINDOW_DEFPAGESIZE:"+getSelectedPrintIndex(),"LodopCustomPage");
//    if (!LODOP.CVERSION) alert(strResult);
//};
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
    LODOP.ADD_PRINT_TEXT(210, 237, 431, 20, "数量：" + strCode.QtyExpected);
    LODOP.ADD_PRINT_TEXT(230, 237, 431, 20, "净重：");
    LODOP.ADD_PRINT_TEXT(250, 237, 431, 20, "毛重：");
    LODOP.ADD_PRINT_BARCODE(75, 37, 200, 200, "QRCode", QRClode);

    //LODOP.ADD_PRINT_SETUP_BKIMG("<img border='0' src='1.jpg' style='z-index: -1'/>");
    //LODOP.SET_SHOW_MODE("BKIMG_IN_PREVIEW", 1);
    //LODOP.SET_SHOW_MODE("BKIMG_PRINT", 1);
    //LODOP.SET_SHOW_MODE("BKIMG_LEFT", 10);
    //LODOP.SET_SHOW_MODE("BKIMG_TOP", 10);

    //LODOP.SET_PRINT_STYLEA(75, 37, 200, 200, "QRCodeVersion", 7);

    //LODOP.ADD_PRINT_SETUP_BKIMG(75, 37, 200, 200, "<img border='0' src='http://qr.liantu.com/api.php?text=156156'  width='250' height='250'/> ");
    //LODOP.ADD_PRINT_SETUP_BKIMG(75, 37, 200, 200, "<img border='0' src='http://qr.liantu.com/api.php?text=156156' width='250' height='250'/> ");
    //LODOP.ADD_PRINT_IMAGE(75, 37, 200, 200,"<img border='0' src='https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=gQEX8ToAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL0MzV1Y5UkRseHhXX25PN19CRm1FAAIEq51GVwMEAAAAAA==' width='250' height='250'/> ");

};
function getSelectedPrintIndex() {
    if (document.getElementById("Radio2").checked)
        return document.getElementById("PrinterList").value;
    else return -1;
};
function getSelectedPageSize() {
    if (document.getElementById("Radio4").checked)
        return document.getElementById("PagSizeList").value;
    else return "";
};
function CreatePrinterList() {
    if (document.getElementById('PrinterList').innerHTML != "") return;
    LODOP = getLodop();
    var iPrinterCount = LODOP.GET_PRINTER_COUNT();
    for (var i = 0; i < iPrinterCount; i++) {

        var option = document.createElement('option');
        option.innerHTML = LODOP.GET_PRINTER_NAME(i);
        option.value = i;
        document.getElementById('PrinterList').appendChild(option);
    };
};
function clearPageListChild() {
    var PagSizeList = document.getElementById('PagSizeList');
    while (PagSizeList.childNodes.length > 0) {
        var children = PagSizeList.childNodes;
        for (i = 0; i < children.length; i++) {
            PagSizeList.removeChild(children[i]);
        };
    };
}
function CreatePagSizeList() {
    LODOP = getLodop();
    clearPageListChild();
    var strPageSizeList = LODOP.GET_PAGESIZES_LIST(getSelectedPrintIndex(), "\n");
    var Options = new Array();
    Options = strPageSizeList.split("\n");
    for (i in Options) {
        var option = document.createElement('option');
        option.innerHTML = Options[i];
        option.value = Options[i];
        document.getElementById('PagSizeList').appendChild(option);
    }
}

function PrintDetail(IDS) {
    $.ajax({
        type: "POST",
        url: "/WMS/ReportManagement/GetPrintLabel",
        data: {
            "ID": IDS,
        },
        async: "false",
        success: function (data) {
            if (data.ErrorCode == "1") {
                //LODOP = getLodop();
                //for (var i = 0; i < data.Response.length; i++) {
                //var QRClode = '[{"GoodsName": ' + data.Response[0].GoodsName + ', "SKU": ' + data.Response[0].SKU + ', "ProductionDate": "", "ExpirationDate": "", "BatchNumber": ' + data.Response[0].BatchNumber + ', "Manufacturer":' + data.Response[0].Manufacturer + ', "BoxNumber": ' + data.Response[0].BoxNumber + ', "QtyExpected":' + data.Response[0].QtyExpected + ', "NetWeight": "", "GrossWeight": ""  }]';
                //    LODOP.PRINT_INIT("");
                //    LODOP.SET_PRINT_PAGESIZE(1, document.getElementById('W1').value, document.getElementById('H1').value, "A4");
                //    AddPrintContent(data.Response[i], QRClode);
                //    LODOP.PRINT();
                //}
                //var str = toUtf8(QRClode);
                //$('#labelimgs').qrcode({
                //    width: 100, //宽度 
                //    height: 100, //高度 
                //    text: str
                //});
                //for (var i = 0; i < data.Response.length; i++) {
                //    data.Response[i].DateTime1 = new Date(data.Response[i].DateTime1).format("yyyy-MM-dd");
                //    data.Response[i].DateTime2 = data.Response[i].DateTime2.format("yyyy-MM-dd");
                //}

                var html = $("#Evaluation").render(data.Response);
                $("#DisInfoBody")["empty"]();
                $("#DisInfoBody").append(html);

                doPrint("打印")
                //var myDoc = {
                //    documents: document,    // 打印页面(div)们在本文档中
                //    // 打印时,only_for_print取值为显示
                //    classesReplacedWhenPrint: new Array('.only_for_print{display:block}'),
                //    copyrights: '杰创软件拥有版权  www.jatools.com'         // 版权声明必须
                //};
                //var jatoolsPrinter = getJatoolsPrinter();
                //jatoolsPrinter.print(myDoc, false);
            }
            else { }

        },
        error: function (msg) {
            alert(msg.val);
        }

    });

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
function toUtf8(str) {
    var out, i, len, c;
    out = "";
    len = str.length;
    for (i = 0; i < len; i++) {
        c = str.charCodeAt(i);
        if ((c >= 0x0001) && (c <= 0x007F)) {
            out += str.charAt(i);
        } else if (c > 0x07FF) {
            out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
            out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        } else {
            out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        }
    }
    return out;
}


