$(document).ready(function () {
    function a() {
        var Tishi = $('#Tishi').val();
        if (Tishi == "添加失败") {
            showMsg("添加失败,请检查是否存在", 4000);
        }
    }
    a();
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
    $(".AddUPCButton").live("click", function () {
        var skuid = $(this).data().skuid;
        window.location.href = "/WMS/product/ProductDetail?ID=" + skuid;
    })

    $(".DelButton").live("click", function () {
        var self = this;
        var id = $(this).data().id;
        $.ajax({
            url: "/WMS/Product/DelProductDetail",
            type: "POST",
            //            dataType: "json",
            // 注意这里
            async: false,
            data: {
                ID: id
            },
            success: function (data) {
                if (data.Code == "1") {
                    $(self).parent().parent().parent().remove();
                } else {
                    showMsg("操作失败！", 4000);
                }
            }
        });
    })
})
$(function () {
    $('#returnButton').click(function () {
        window.location.href = "/WMS/Product/Index";
    });
    $('#submitButton').click(function () {

        if (!checkInput()) {
            return false;
        }
        //setPageControlVal();//没有新增、编辑SKU方法 Bob

    });
})

var checkInput = function () {

    if (!$('#productStorerInfo_StorerID').val()) {
        showMsg("请选择货主", "4000");
        return false;
    }

    if ($('#productStorerInfo_SKU').val() === "") {
        showMsg("请输入SKU", "4000");
        //Runbow.TWS.Alert("请输入品名");
        return false;
    }
    if ($('#productStorerInfo_GoodsName').val() === "") {
        //Runbow.TWS.Alert("请输入品名");
        showMsg("请输入货品名称", "4000");
        return false;
    }
    //状态
    if ($('#productStorerInfo_Status').val() === "") {
        //Runbow.TWS.Alert("请选择状态");
        showMsg("请选择状态", "4000");
        return false;
    }

    if ($('#productStorerInfo_Type').val() === "") {
        //Runbow.TWS.Alert("请选择货品种类");
        showMsg("请选择货品种类", "4000");
        return false;
    }

    //if (!($('#productStorerInfo_SKUClassification').val())) {
    //    Runbow.TWS.Alert("请选择SKU分类");
    //    return false;
    //}
    //SKU分组
    //if (!($('#productStorerInfo_SKUGroup').val())) {
    //    Runbow.TWS.Alert("请选择SKU分组");
    //    return false;
    //}

    //if (!($('#productStorerInfo_ManufacturerSKU').val())) {
    //    Runbow.TWS.Alert("请输入正确重量");
    //    return false;
    //}

    //if (!checkFloat($('#productStorerInfo_RetailSKU').val())) {
    //    Runbow.TWS.Alert("请输入制造商SUK");
    //    return false;
    //}

    //var customerID = $('#Pod_CustomerID').val();
    //if (customerID === '2') {
    //    var ttlOrTplType = $('#Pod_TtlOrTplID option:selected').text();
    //    if (ttlOrTplType === 'FTL') {
    //        if ($('#Str29').val() === '') {
    //            Runbow.TWS.Alert("请输入整车吨位");
    //            return false;
    //        }
    //        if (!parseInt($('#Str29').val())) {
    //            Runbow.TWS.Alert("请输入正确吨位:2,5,10,15,20");
    //            return false;
    //        }
    //    }

    //    var paymentMethod = $('#Str22').val();
    //    if (paymentMethod === '1') {
    //        if ($('#Str30').val() === '') {
    //            Runbow.TWS.Alert("请输入代收款方式");
    //            return false;
    //        }
    //    }
    //}

    return true;
};