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

    $(function ($) {
        $('body').click(function () {
            ShowsOut()
        });
    });
    $("#addButton").live('click', function () {
        window.location.href = "/WMS/Warehouse/GoodsShelvesCreate?CustomerID=" + $("#SearchCondition_CustomerID").val() + "&WarehouseID=" + $("#SearchCondition_WarehouseID").val() + "&ViewType=1"
    });
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
            var ID = $(this).children()[3].innerText.trim();        
                closePopup(ID);
        });
    }
    
});

function ShowsIn(ID, obj) {
    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
        $(".ddiv:not(obj)").animate({
            width: "hide",
            width: "230%",
            paddingRight: "hide",
            paddingLeft: "hide",
            marginRight: "hide",
            marginLeft: "hide"

        }, 100);
        $("#operateTD" + ID).animate({
            width: "show",
            width: "278%",
            paddingRight: "show",
            paddingLeft: "show",
            marginRight: "show",
            marginLeft: "show"
        });
    }
    //$("#operateTD" + ID)[0].style.display = "";
}

function ShowsOut() {
    //$("#operateTD" + ID).fadeOut("slow");

    $(".ddiv").animate({
        width: "hide",
        width: "230%",
        paddingRight: "hide",
        paddingLeft: "hide",
        marginRight: "hide",
        marginLeft: "hide"

    }, 100);
    //$("#operateTD" + ID)[0].style.display = "";
}


function WarehouseEdit(ID)
{
    window.location.href = "/WMS/Warehouse/GoodsShelvesCreate?ViewType=2"+"&ID="+ID
}

function WarehouseDelete(ID, CustomerID, WarehouseID)
{
    layer.confirm('<font size="4">确认是否删除？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "Get",
            url: "/WMS/Warehouse/DeleteGoodsShelf",
            data: {
                "ID": ID,
                "CustomerID": CustomerID,
                "WarehouseID": WarehouseID
            },
            async: "false",
            success: function (data) {
                if (data = "OK") {
                    location.href = "/WMS/Warehouse/GoodsShelves/";
                    showMsg("删除成功!", "4000");
                }
            },
            error: function (msg) {
                showMsg("删除失败！", "4000");
            }
        });
    });
}

var GoodsShelfImportClick = function () {
    if ($('select[id=StorerID]').val() == '') {
        //Runbow.TWS.Alert("请选择仓库！");
        showMsg("请选择客户！", 4000);
        return false;
    }

    if ($('#importExcel').val() === '') {
        //Runbow.TWS.Alert("请选择要导入的Excel");
        showMsg("请选择要导入的Excel", 4000);
        return false;
    }


    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        //Runbow.TWS.Alert("请选择Excel格式的文件");
        showMsg("请选择Excel格式的文件", 4000);
        //$('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    };

    WebPortal.MessageMask.Show("导入中...");

    $.ajaxFileUpload({
        url: '/WMS/Warehouse/GoodsShelfImportClick',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: { "CustomerID": $('select[id=StorerID]').val(), "WarehouseID": $('select[id=WareHouseID]').val() },

        success: function (data, status) {
            WebPortal.MessageMask.Close();
            $('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
        },
        error: function (data, status, e) {
            $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            WebPortal.MessageMask.Close();
        }
    });
    return false;
};
var GoodsShelfImportClick2 = function () {
    if ($('select[id=StorerID2]').val() == '') {
        //Runbow.TWS.Alert("请选择仓库！");
        showMsg("请选择客户！", 4000);
        return false;
    }

    if ($('#importExcel2').val() === '') {
        //Runbow.TWS.Alert("请选择要导入的Excel");
        showMsg("请选择要导入的Excel", 4000);
        return false;
    }


    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel2').val()) == null) {
        //Runbow.TWS.Alert("请选择Excel格式的文件");
        showMsg("请选择Excel格式的文件", 4000);
        //$('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    };

    WebPortal.MessageMask.Show("导入中...");

    $.ajaxFileUpload({
        url: '/WMS/Warehouse/GoodsShelfLocationImportClick',
        secureuri: false,
        fileElementId: 'importExcel2',
        dataType: 'json',
        data: { "CustomerID": $('select[id=StorerID2]').val(), "WarehouseID": $('select[id=WareHouseID2]').val() },

        success: function (data, status) {
            WebPortal.MessageMask.Close();
            $('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
        },
        error: function (data, status, e) {
            $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            WebPortal.MessageMask.Close();
        }
    });
    return false;
};
