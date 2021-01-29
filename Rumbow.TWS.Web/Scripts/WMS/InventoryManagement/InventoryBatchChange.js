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


    $('select[id=InventorySearchCondition_CustomerID]').live('change', function () {
        window.location.href = "/WMS/InventoryManagement/InventoryBatchChange/?customerID=" + $(this).val();
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

    //$('form').bind('submit', checkform);

    //查询按钮
    $("#searchButton").live("click", function () {
        //验证必填项
        if ($("#InventorySearchCondition_CustomerID").val() == "") {
            layer.tips('请选择客户', '#InventorySearchCondition_CustomerID');
            return false;
        }
        if ($("#InventorySearchCondition_Warehouse").val() == "") {
            layer.tips('请选择仓库', '#InventorySearchCondition_Warehouse');
            return false;
        }
        if ($("#InventorySearchCondition_Area").val() == "") {
            layer.tips('请选择库区', '#InventorySearchCondition_Area');
            return false;
        }
        if ($("#InventorySearchCondition_Location").val() == "") {
            layer.tips('请填写库位', '#InventorySearchCondition_Location');
            return false;
        }
        if ($("#InventorySearchCondition_GoodsType").val() == "") {
            layer.tips('请选择品级', '#InventorySearchCondition_GoodsType');
            return false;
        }
        if ($("#InventorySearchCondition_SKU").val() == "") {
            layer.tips('请填写SKU', '#InventorySearchCondition_SKU');
            return false;
        }
       
    });
     
 

    //更新
    $("#updateButton").live('click', function () {        
        var invents = [];
        $("#resultTable tbody tr").each(function (a, b) {
            if ($(this).find('td').eq(0).find('input[type="checkbox"]').attr("checked") === "checked") {
                var inven = {
                    CustomerName: $(b).find('td').eq(1).text().trim(),//客户名称
                    Warehouse: $(b).find('td').eq(2).text().trim(),//仓库
                    Area: $(b).find('td').eq(3).find('span').eq(0).text().trim(),//库区
                    Location: $(b).find('td').eq(3).find('span').eq(1).text().trim(),//库位
                    SKU: $(b).find('td').eq(4).text().trim(),//SKU
                    GoodsName: $(b).find('td').eq(5).text().trim(),//产品名称
                    GoodsType: $(b).find('td').eq(7).text().trim(),//品级
                    InventoryType: $(b).find('td').eq(8).find('input').val().trim(),//库存等级
                    Qty: $(b).find('td').eq(9).text().trim(),//数量
                    BatchNumber: $(b).find('td').eq(10).text().trim(),//原批次号
                    Unit: $(b).find('td').eq(13).text().trim(),//单位
                    str5: $(b).find('td').eq(12).text().trim(),//原生产日期
                    str3: $(b).find('td').eq(15).find('input').eq(0).val().trim(),//新批次
                    str4: $(b).find('td').eq(15).find('input').eq(1).val().trim(),//新日期
                    CustomerID: $(b).find('input[name=InvenCustomerID]').eq(0).val().trim(),
                    IDS: $(b).find('input[type="checkbox"]').attr("data-ID")//记一下修改被的库存ID
                };              
                invents.push(inven);
            }
        });        
        
        if (invents.length<=0) {
            layer.msg('请先选择需要修改的库存', { icon: 5 });
           
        } else {
            
            $.ajax({
                type: "Post",
                url: "/WMS/InventoryManagement/UpdateInventChange",
                data: {
                    "inventorys": JSON.stringify(invents)
                },
                success: function (data) {                   
                    if (data == "") {
                        //showMsg("保存成功！", 4000);
                        layer.msg('修改成功！', { icon: 1 });
                        setTimeout("aaa();", 1200);
                    }
                    else {
                        layer.msg('修改失败：' + data, { icon: 2 });
                    }
                },
                error: function (msg) {
                    alert(msg.val);
                }

            });
        }


    });

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }
        RefreshIDs();
    });

    $("#resultTable tbody input[type='checkbox']").live('click', function () {
        RefreshIDs();
    });

    var RefreshIDs = function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var length = checkBoxs.length;
        var IDs = [];
        var checked = 0;
        checkBoxs.each(function () {
            if ($(this).attr("checked") === "checked") {
                var id = { ID: $(this).attr("data-ID") };
                IDs.push(id);
                checked++;
            }
        });

        if (checked == checkBoxs.length) {
            $('#selectAll').attr("checked", "checked");
        } else {
            $('#selectAll').removeAttr("checked");
        }
    };


});


//查询验证
//function checkform() {
//    var selects = $('#selectway tr')
//    var name = selects.eq(0).find('select').eq(0).find('option:selected').text();
//    var warehouse = selects.eq(0).find('select').eq(1).find('option:selected').text();
//    var area = selects.eq(0).find('select').eq(2).find('option:selected').text();
//    var location = selects.eq(0).find('td').eq(7).find('input').val();
//    var type = selects.eq(1).find('select').eq(0).find('option:selected').text();
//    var sku = selects.eq(1).find('td').eq(3).find('input').val();
//    var goodtype = selects.eq(1).find('select').eq(1).find('option:selected').text();
//    if (name != '==请选择==' && warehouse != '==请选择==' && area != '==请选择==' && location != '' && type != '==请选择==' && goodtype != '==请选择=='&& sku != '') {
//        return true;
//    }
//    else {
//        showMsg("客户，仓库，库区，库位，库存类型，sku，货品等级不可为空！" , 2000);
//        return false;
//    }
//}

function aaa() {
    document.getElementById("searchButton").click();
}

 