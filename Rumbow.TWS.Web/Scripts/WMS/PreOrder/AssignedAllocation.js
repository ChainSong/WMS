
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
    $("#SaveButton").click(function () {
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
            url: "/WMS/PreOrder/AssignedAllocationJson",
            dataType: "text",
            data: {
                "ID": $("#PreAndDetail_SearchCondition_ID")[0].value,
                "CustomerId": $("#PreAndDetail_SearchCondition_CustomerID")[0].value,
                "Criterion": '指定分配',
                "Jaonstr": Jaonstr,
            },
            success: function (data) {
                var StrHtml = JSON.parse(data)
                if (StrHtml.data == '操作失败') {
                    showMsg('操作失败，分配可能没有进行！', 4000);
                } else {
                    if (StrHtml.data[0].Type == "1") {
                        var html = $("#Evaluation2").render(StrHtml.data);
                        $("#DisInfoBody2")["empty"]();
                        $("#DisInfoBody2").append(html);
                        //if ($("#DisInfo2")[0].style.display == 'none') {
                        //    $("#DisInfo2")[0].style.display = 'block';
                        //}
                        //alert($("#DisInfo2")[0].innerHTML);
                        layer.open({
                            title: '缺货信息',
                            type: 1,
                            skin: 'layui-layer-rim', //加上边框
                            area: ['450px', '400px'], //宽高
                            content: $("#DisInfo2")[0].innerHTML
                        });

                        //openPopup("panel", true, 400, 400, null, 'DisInfo2', true);
                        //$("#popupLayer_panel")[0].style.top = "200px";
                    }
                    else {
                        window.location.href = "/WMS/PreOrder/PreOrderCreateOrEdit/?ID=" + $("#PreAndDetail_SearchCondition_ID")[0].value + '&ViewType=0&Flag=1';
                    }
                    //for (var i = 0; i < StrHtml.data.length; i++) {
                    //    for (var j = 0; j < checkBoxs.length; j++) {
                    //        if (checkBoxs[j].dataset.id == StrHtml.data[i].POID) {
                    //$(self).parent().parent().parent().children(".Status")[0].innerHTML = '<font size="3" color="red">' + status[StrHtml.data[0].Note] + '</font>';
                    //if (StrHtml.data[0].Note == '9' || StrHtml.data[0].Note == '5') {
                    //    $(self).parent()[0].innerHTML = '<label data-name="' + StrHtml.data[0].POID + '" style="cursor: pointer; color:white" class="label label-info">查看出库单</label>';
                    //}
                    //        }
                    //    }
                    //}
                }
                //showMsg(data, "3000");
                //location.reload();
            },
            error: function (msg) {
                showMsg("操作失败", "4000");
            }
        })
    })

});
var box = {
    行号: 'LineNumber',
    SKU: 'SKU', 货品描述: 'GoodsName',
    货品等级: 'GoodsType', POID: "POID",
    期望数量: 'OriginalQty',
    库位: 'Location', 仓库: 'Warehouse', 仓库ID: 'WarehouseId', 外部单号: 'ExternOrderNumber', 批次号: 'BatchNumber', 托号: 'BoxNumber', 单位: 'Unit', 规格: 'Specifications',
    UPC: 'UPC', '(库区)库位': "Location", 实际数量: 'Str15', '(库区)库位': "Location", 分配批次: 'Str19', 分配托号: 'Str20', 库区: 'Str16', 库位: 'Str17',ID:'ID'

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
                        else {
                           r += "\"" + box["库区"] + "\"\:\"\",";
                            r += "\"" + box["库位"] + "\"\:\"\",";
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
