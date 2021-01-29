$(document).ready(function () {
    if ($('#InventorySearchCondition_CustomerID').val() == "") {
        $('#InventorySearchCondition_CustomerID option:first').next().attr("selected", "selected");

    } else {

    }
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

    //$(function ($) {
    //    $('body').click(function () {
    //        ShowsOut()
    //    });
    //});
    $('select[id=InventorySearchCondition_CustomerID]').live('change', function () {
        window.location.href = "/WMS/InventoryManagement/InventorySearch/?customerID=" + $(this).val();
    });
    $('select[id=InventorySearchCondition_Warehouse]').live('change', function () {
        var AreaLists = $("#AreaLists");
        var Area = document.getElementById("InventorySearchCondition_Area");
        document.getElementById("InventorySearchCondition_Area").innerHTML = "";
        if (document.getElementById("InventorySearchCondition_Warehouse").value == "") {
            document.getElementById("InventorySearchCondition_Area").innerHTML = "<option value=\"==请选择==\">==请选择==</option>";
        } else {
            for (var i = 0; i < AreaLists[0].length; i++) {
                if (AreaLists[0][i].value == $("#InventorySearchCondition_Warehouse").val()) {
                    var opt = new Option(AreaLists[0][i].text, AreaLists[0][i].text);
                    Area.options.add(opt);
                }
            }
        }
    });
    //$('select[id=InventorySearchCondition_Warehouse]').live('change', function () {
    //    window.location.href = "/WMS/InventoryManagement/InventorySearch/?warehouseID=" + $(this).val();
    //});

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }
    });
    $("#OutboundOrder").click(function () { 
        var CustomerId = $("select[id=InventorySearchCondition_CustomerID]")[0].value;
        //var Warehouse = self.dataset.warehouse;
        var Warehouse = $('#InventorySearchCondition_Warehouse option:selected')[0].text;
        if (CustomerId == "" || Warehouse == "==请选择==")
        {
            showMsg("请选择客户和仓库！", 4000);
            return false;
        } 
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                sql += checkBoxs[i].dataset.id.toString();
            }
        }
        if (sql.length > 0) {
            sql = sql.toString().substring(0, sql.toString().length - 1);
            location.href = "/WMS/OrderManagement/InventoryOfOutbound/?Ids=" + sql + "&CustomerId=" + CustomerId + "&Warehouse=" + Warehouse;
        }
        else {
            showMsg("请勾选库存！", 4000);
            return;
        }
    })

    $("#PrintInventorylabel").click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                sql += checkBoxs[i].dataset.id.toString();
            }
        }
        if (sql.length > 0) {
            sql = sql.toString().substring(0, sql.toString().length - 1);
            $.ajax({
                type: "POST",
                url: "/WMS/InventoryManagement/GetPrintLabel",
                data: {
                    "ID": sql,
                },
                async: "false",
                success: function (data) {
                    var html = $("#Evaluation").render(data.Response);
                    $("#DisInfoBody")["empty"]();
                    $("#DisInfoBody").append(html);
                    doPrint("打印")
                },
                error: function (msg) {
                    showMsg(msg, "4000");
                }
            })
            }
        else {
            showMsg("请勾选！", 4000);
            return;
        }
    })
});
function GetInventoryViewByLocation(warehouse,customerid,location) {
    window.location.href = '/WMS/InventoryManagement/GetInventoryView/?Warehouse=' + warehouse + '&CustomerID=' + customerid + '&location=' + location;
};

function GetInventoryViewBySKU(warehouse,customerid, SKU) {
    window.location.href = '/WMS/InventoryManagement/GetInventoryView/?Warehouse=' + warehouse + '&CustomerID=' + customerid + '&SKU=' + SKU;
};

function tiaoadjustment(CustomerID, AdjustType, AdjustLocation, AdjustSku,AdjustUPC, AdjustBatchNumber, AdjustBoxNumber, AdjustUnit, AdjustSpecifications, WarehouseName, GoodsName, Qty, GoodsType) {
    if ($("#ProjectName").val() == "Akzo") {
        location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust_akzo/?ID=0" + "&customerID=" + CustomerID + "" + "&ViewType=1&AdjustType=" + AdjustType +
            "&Adjustlocation=" + AdjustLocation.toString().trim() + "&AdjustSku=" + AdjustSku.toString().trim() + "&AdjustUPC=" + AdjustUPC.toString().trim() + "&AdjustBatchNumber=" + AdjustBatchNumber +
            "&AdjustBoxNumber=" + AdjustBoxNumber + "&AdjustUnit=" + AdjustUnit + "&AdjustSpecifications=" + AdjustSpecifications + "&WarehouseName=" + WarehouseName +
            "&GoodsName=" + GoodsName + "&Qty=" + Qty + "&GoodsType=" + GoodsType
    }
    else {
        location.href = "/WMS/InventoryManagement/AddorEditorViewAdjust/?ID=0" + "&customerID=" + CustomerID + "" + "&ViewType=1&AdjustType=" + AdjustType +
                    "&Adjustlocation=" + AdjustLocation.toString().trim() + "&AdjustSku=" + AdjustSku.toString().trim() + "&AdjustUPC=" + AdjustUPC.toString().trim() + "&AdjustBatchNumber=" + AdjustBatchNumber +
                    "&AdjustBoxNumber=" + AdjustBoxNumber + "&AdjustUnit=" + AdjustUnit + "&AdjustSpecifications=" + AdjustSpecifications + "&WarehouseName=" + WarehouseName +
                    "&GoodsName=" + GoodsName + "&Qty=" + Qty + "&GoodsType=" + GoodsType
    }
}
//阿克苏拆箱
function Unboxing_akzo(IDS)
{
    location.href = "/WMS/InventoryManagement/Unboxing_akzo/?IDS=" + IDS
}
//解冻
function Unfreeze(ID, oneself) {
    layer.confirm('<font size="4">确认是否解冻</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "POST",
            url: "/WMS/InventoryManagement/Unfreeze",
            data: {
                "ID": ID,
            },
            async: "false",
            success: function (data) {
                showMsg("解冻成功!", "4000");
                $(oneself)[0].style.visibility = "hidden";
                $(oneself).parent().parent().prev().prev().prev()[0].innerText = "已解冻"
            },
            error: function (msg) {
                showMsg("解冻失败", "4000");
            }
        });
    });


}
function outbound(CustomerId, Warehouse, Ids) {
    //var CustomerId = self.dataset.customerid;
    //var Area = self.dataset.area;
    //var Location = self.dataset.location;
    //var SKU = self.dataset.sku;
    //var GoodsType = self.dataset.goodstype;
    //var Warehouse = self.dataset.warehouse;
    //var Ids = self.dataset.id;
    if (Ids.split(",").length > 1) {
        Ids = Ids.substring(0, Ids.length - 1).trim();
        location.href = "/WMS/OrderManagement/InventoryOfOutbound/?Ids=" + Ids + "&CustomerId=" + CustomerId + "&Warehouse=" + Warehouse;
    }
}

//function ShowsIn(ID, obj) {
//    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
//        $(".ddiv:not(obj)").animate({
//            width: "hide",
//            width: "400%",
//            paddingRight: "hide",
//            paddingLeft: "hide",
//            marginRight: "hide",
//            marginLeft: "hide"

//        }, 100);
//        $("#operateTD" + ID).animate({
//            width: "show",
//            width: "445%",
//            paddingRight: "show",
//            paddingLeft: "show",
//            marginRight: "show",
//            marginLeft: "show"
//        });
//    }
//    //$("#operateTD" + ID)[0].style.display = "";
//}

//function ShowsOut() {
//    //$("#operateTD" + ID).fadeOut("slow");

//    $(".ddiv").animate({
//        width: "hide",
//        width: "400%",
//        paddingRight: "hide",
//        paddingLeft: "hide",
//        marginRight: "hide",
//        marginLeft: "hide"

//    }, 100);
//    //$("#operateTD" + ID)[0].style.display = "";
//}
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
