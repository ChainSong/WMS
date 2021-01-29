$(document).ready(function () {
    $(".Ooperation").live("mouseover", function () {
        $(this).prev()[0].style.display = "none";
    });
    $(".Ooperation").live("mouseenter", function () {
        $(this).prev()[0].style.display = "";

    });
    $(".Adiv").live("mouseleave", function () {
        $(this)[0].style.display = "none";
    });
    $("tr").live("mouseenter", function () {
        $(".Adiv").each(function (a, b) {
            $(b)[0].style.display = "none";
        })
    });
    $(".numberCheck").live('keydown', function () {
        replaceNotNumber(this);
    });
    $(".numberCheck").live('keyup', function () {
        replaceNotNumber(this);
    });
    $(".AddButton").live('click', function () {
        var self = this;
        var myTable = document.getElementById("resultTable");
        var rowIndex = event.srcElement.parentNode.parentNode.parentNode.rowIndex;
        var obj = document.getElementById('resultTable').getElementsByTagName('tr');
        var newrow = obj[rowIndex].cloneNode(true);
        $(newrow).children(".SKU").children("input")[0].value = '';
        $(newrow).children(".GoodsName").children("input")[0].value = '';
        $(newrow).children(".ToQty").children("input")[0].value = '';
        newrow.setAttribute("class", "NewTableBGColor");
        $(self).parent().parent().parent().after(newrow);
    });
    $(".DelButton").live('click', function () {
        var self = this;
        var length = $("#resultTable tbody")[0].rows.length;
        if (length == 1)
        {
            showMsg("最后一行不能删除！", "4000");
            return;
        }
        $(self).parent().parent().parent().remove();
    });
    $("#backButton").live('click', function () {
        history.back();
    });
    $(".SKUQuery").live('dblclick', function () {
        var obj = this;
        openPopup("SKUPop", true, 1000, 600, '/WMS/Product/index/?flag=1&customerID=' + $("#CustomerID").val(), null, function (Sku, GoodsName, UPC) {
            var length = $("#resultTable tbody")[0].rows.length;
            for (var i = 1; i <= length; i++) {
                if ($($('#resultTable')[0].rows[i]).children(".SKU").children("input")[0].value == Sku) {
                    showMsg("此SKU已添加", 4000);
                    return;
                }
            }
            $(obj).val(Sku);
            $($(obj).parent().parent()).children(".GoodsName").children("input")[0].value = GoodsName;
        });
        $("#popupLayer_SKUPop")[0].style.top = "50px";

    });
    $("#SubmitButton").live('click', function () {
        if ($("input[name=FromQty]")[0].value == '' || $("input[name=FromQty]")[0].value == '0') {
            showMsg("请填写拆箱数量", 4000);
            return;
        }
        else {
            if ($("input[name=FromQty]")[0].value > $("#InventoryQty").val())
            {
                showMsg("拆箱数量不能大于库存数量", 4000);
                return;
            }
        }
        
        var length = $("#resultTable tbody")[0].rows.length;
        for (var i = 1; i <= length; i++) {
            if ($($('#resultTable')[0].rows[i]).children(".SKU").children("input")[0].value == '') {
                showMsg("请选择SKU", 4000);
                return;
            }
            if ($($('#resultTable')[0].rows[i]).children(".ToQty").children("input")[0].value == '' || $($('#resultTable')[0].rows[i]).children(".ToQty").children("input")[0].value == '0') {
                showMsg("请填写数量", 4000);
                return;
            }
        }
        layer.confirm('<font size="4">确认是否提交？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var ToSKUJson = TableToJson();
            $.ajax({
                type: "POST",
                url: "/WMS/InventoryManagement/Unboxing_akzo",
                data: {
                    "IDS": $("#IDS").val(),
                    "UnboxingQty":$("#FromQty").val(),
                    "ToSKUJson": TableToJson()
                },
                async: "false",
                success: function (data) {
                    if (data == "") {
                        showMsg("拆箱成功!", "4000");
                    }
                    else {
                        showMsg("拆箱失败:" + data, "4000");
                    }
                },
                error: function (msg) {
                    showMsg("操作失败", "4000");
                }
            });
        });
    });
});
function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    if (pattern.test(hehe.value)) {

        hehe.value = hehe.value.replace(pattern, "");
    }
}
var box = {
    SKU: 'SKU',
    名称: 'GoodsName', 
    数量: 'AllocatedQty'
};
function TableToJson() {
    var txt = "[";
    var table = document.getElementById("resultTable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 1; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            var innerVal = '';
            if ($(tds[i]).children("input").length > 0) {
                innerVal = $(tds[i]).children("input")[0].value;                                                         
            }
            r += "\"" + box[col[i].innerText.trim()] + "\"\:\"" + innerVal + "\",";
        }
            r = r.substring(0, r.length - 1)
            r += "},";
            txt += r;
        }
        txt = txt.substring(0, txt.length - 1);
        txt += "]";
        return txt;
    }
