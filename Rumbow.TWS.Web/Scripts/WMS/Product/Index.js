$(document).ready(function () {
    if ($('#ProductSKU_StorerID').val() == "") {
        $('#ProductSKU_StorerID option:first').next().attr("selected", "selected");

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
    //添加UPC
    $(".AddButton").live("click", function () {
        var skuid = $(this).data().skuid;
        window.location.href = "/WMS/product/productdetail?ID=" + skuid;
    })
    //编辑
    $('.editSettledPod').live('click', function () {
        var CustomerId = $("#ProductSKU_StorerID")[0].value;
        var settledpod = $(this).attr('data-id');
        if (CustomerId != "") {
            location.href = "/WMS/Product/AddProduct/?ID=" + settledpod + "&typeid=3&CustomerId=" + CustomerId;
        } else {
            location.href = "/WMS/Product/AddProduct/?ID=" + settledpod + "&typeid=3";
        }
    })
})

$(function () {
    if ($('#searchFlag').val() == 1) {
        $("#tables").removeAttr("style");
        $("#resultTable tr").click(function () {
            $(this).addClass("btn-success").siblings("tr").removeClass("btn-success");
        });
        $("#resultTable tr").mouseover(function () {
            $(this).addClass("btn-info").siblings("tr").removeClass("btn-info");
        });
        $("#resultTable tr").mouseleave(function () {
            $(this).removeClass("btn-info");
        });
        $("#resultTable tr").dblclick(function () {
            //var rowIndex = $("#resultTable tr").index($(this));
            var Sku = $(this).children()[1].innerText;
            var GoodsName = $(this).children()[2].innerText;
            var UPC = $(this).children()[3].innerText;
            if (Sku != "SKU") {
                closePopup(Sku, GoodsName, UPC);
            }
        });
    }
    //新增
    $('#AddButton').click(function () {
        var CustomerId = $("#ProductSKU_StorerID")[0].value;
        if (CustomerId != "") {
            window.location.href = "/WMS/Product/AddProduct?typeid=1&CustomerId=" + CustomerId;
        } else {
            window.location.href = "/WMS/Product/AddProduct?typeid=1";
        }
    });
    $('#ExeclButton').click(function () {
        window.location.href = "/WMS/Product/ExeclProduct";
    });
    //下载模板
    $("#portButtonTemplet").click(function () {
        var CustomerId = $("#ProductSKU_StorerID")[0].value;
        demo('/WMS/Product/ProductdemoExecl/?CustomerId=' + CustomerId);
        //window.location.href = "http://www.runbow.com.cn:8080/Picture/DemoExcel/货品导入模板.xlsx";
    })
    //更新SKU价格
    $("#importPriceButton").click(function () {
        if ($('#importExcel').val() === '') {
            //showMsg("请选择要导入的Excel", 4000);
            layer.alert('请选择要导入的Excel')
            return false;
        }
        var exp = /.xls$|.xlsx$|/;
        var fileImport = $('#fileImport').clone();
        if (exp.exec($('#importExcel').val()) == null) {
            //showMsg("请选择Excel格式的文件", 4000);
            layer.alert('请选择Excel格式的文件')
            $(this).before(fileImport);
            return false;
        };
        layer.confirm('是否更新SKU价格？', {
            btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
        //按钮
        }, function (index) {
            //WebPortal.MessageMask.Show("导入中...");
            $.ajaxFileUpload({
                url: '/WMS/Product/ImportPrice',
                secureuri: false,
                fileElementId: 'importExcel',
                dataType: 'json',
                data: { StorerID: $("#StorerID").val(), pathFile: $("#importExcel").val() },
                success: function (data, status) {
                    //WebPortal.MessageMask.Close();
                    layer.close(index);
                    if (data.IsSuccess == true) {
                        $('#outPutResult').html("<h4><font color='#33cc70'>更新成功！" + data.result + "</font></h4>");
                    }
                    else {
                        $('#outPutResult').html("<h4><font color='#FF0000'>更新失败！<br/>" + data.result + "</font></h4>");
                    }
                },
                error: function (data, status, e) {
                    //WebPortal.MessageMask.Close();
                    layer.close(index);
                    $('#outPutResult').html("<h4><font color='#FF0000'>更新失败！<br/>" + data.result + "</font></h4>");
                }
            });
        }, function () {
        });
    })
    //更新羽绒服标记
    $("#importDownCoatButton").click(function () {
        if ($('#importExcel').val() === '') {
            //showMsg("请选择要导入的Excel", 4000);
            layer.alert('请选择要导入的Excel')
            return false;
        }
        var exp = /.xls$|.xlsx$|/;
        var fileImport = $('#fileImport').clone();
        if (exp.exec($('#importExcel').val()) == null) {
            //showMsg("请选择Excel格式的文件", 4000);
            layer.alert('请选择Excel格式的文件')
            $(this).before(fileImport);
            return false;
        };
        layer.confirm('是否更新羽绒服标记？', {
            btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            //WebPortal.MessageMask.Show("导入中...");
            $.ajaxFileUpload({
                url: '/WMS/Product/importDownCoat',
                secureuri: false,
                fileElementId: 'importExcel',
                dataType: 'json',
                data: { StorerID: $("#StorerID").val(), pathFile: $("#importExcel").val() },
                success: function (data, status) {
                    //WebPortal.MessageMask.Close();
                    layer.close(index);
                    if (data.IsSuccess == true) {
                        $('#outPutResult').html("<h4><font color='#33cc70'>更新成功！" + data.result + "</font></h4>");
                    }
                    else {
                        $('#outPutResult').html("<h4><font color='#FF0000'>更新失败！<br/>" + data.result + "</font></h4>");
                    }
                },
                error: function (data, status, e) {
                    //WebPortal.MessageMask.Close();
                    layer.close(index);
                    $('#outPutResult').html("<h4><font color='#FF0000'>更新失败！<br/>" + data.result + "</font></h4>");
                }
            });
        }, function () {
        });
    })
});

function demo(url) {
    var form = $("<form>");
    form.attr('style', 'display:none');
    form.attr('target', '');
    form.attr('method', 'post');
    form.attr('action', url);//'/WMS/PreOrder/ReportDirect'
    var input1 = $('<input>');
    input1.attr('type', 'hidden');
    input1.attr('name', 'demo');
    input1.attr('value', 'Export');
    var input2 = $('<input>');
    input2.attr('type', 'hidden');
    input2.attr('name', 'fileId');
    input2.attr('value', "fileId");
    $('body').append(form);
    form.append(input1);
    form.append(input2);

    form.submit();
    form.remove();
}
//删除
function deleteSKU(ID,CustomerID, obj) {
    layer.confirm('<font size="4">是否删除？</font>', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        var tr = $(obj).parent().parent().parent();
        $.send(
            '/WMS/Product/DelProduct',
            {
                ID: ID,
                CustomerID: CustomerID
            },
            function (response) {
                if (response.IsSuccess) {
                    tr.remove();
                }
                //showMsg(response.Message, 4000);
                layer.alert(response.Message)
            },
            function () {
                //showMsg("删除失败！", 4000);
                layer.alert("删除失败！")
            });
    });
};
//导入
var fileImportClick = function () {
    if ($('#importExcel').val() === '') {
        //showMsg("请选择要导入的Excel", 4000);
        layer.alert("请选择要导入的Excel")
        return false;
    }
    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        //showMsg("请选择Excel格式的文件", 4000);
        layer.alert("请选择Excel格式的文件")
        $(this).before(fileImport);
        return false;
    };
    layer.confirm('是否导入SKU？', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        //WebPortal.MessageMask.Show("导入中...");
        $.ajaxFileUpload({
            url: '/WMS/Product/ExeclProduct',
            secureuri: false,
            fileElementId: 'importExcel',
            dataType: 'json',
            data: { StorerID: $("#StorerID").val() },
            success: function (data, status) {
                //WebPortal.MessageMask.Close();
                layer.close(index);
                if (data.IsSuccess == true) {
                    $('#outPutResult').html("<h4><font color='#33cc70'>" + data.result + "</font></h4>");
                }
                else {
                    $('#outPutResult').html("<h4><font color='#FF0000'>" + data.result + "</font></h4>");
                }
            },
            error: function (data, status, e) {
                //WebPortal.MessageMask.Close();
                layer.close(index);
                $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            }
        });
    }, function () {
    });
};
