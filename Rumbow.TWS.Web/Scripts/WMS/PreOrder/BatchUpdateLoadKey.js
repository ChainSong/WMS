$(document).ready(function () {

    $("tr").live("mouseenter", function () {
        $(".Adiv").each(function (a, b) {
            $(b)[0].style.display = "none";
        });

    });

    $('.DropDownList').each(function (index) {
        var id = $(this).attr("id");
    });
    
});



//导入按钮
var fileImportClick = function () {
    if ($('select[id=SearchCondition_CustomerID]').val() == '') {
        //Runbow.TWS.Alert("请选择客户！");
        showMsg("请选择客户！", 4000);
        return false;
    }
    if ($('select[id=SearchCondition_ID]').val() == '') {
        //Runbow.TWS.Alert("请选择仓库！");
        showMsg("请选择仓库！", 4000);
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
       
        url: '/WMS/PreOrder/BatchIimportUpdateLoadKey',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: {
            Action: "批量更新LoadKey",
            CustomerID: $('#StorerID').val()
        },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            //$('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
            $('#outPutResult').html("<h4>" + data.result + "</h4>");
        },
        error: function (data, status, e) {
            //$('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            $('#outPutResult').html("<h4>" + data.result + "</h4>");
            WebPortal.MessageMask.Close();
        }
    });
    return false;
};    /*导入按钮结束*/

var fileImportClick1 = function () {
    if ($('select[id=SearchCondition_CustomerID]').val() == '') {
        //Runbow.TWS.Alert("请选择客户！");
        showMsg("请选择客户！", 4000);
        return false;
    }
    if ($('select[id=SearchCondition_ID]').val() == '') {
        //Runbow.TWS.Alert("请选择仓库！");
        showMsg("请选择仓库！", 4000);
        return false;
    }
    if ($('#importExcel1').val() === '') {
        //Runbow.TWS.Alert("请选择要导入的Excel");
        showMsg("请选择要导入的Excel", 4000);
        return false;
    }


    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel1').val()) == null) {
        //Runbow.TWS.Alert("请选择Excel格式的文件");
        showMsg("请选择Excel格式的文件", 4000);
        //$('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    };

    WebPortal.MessageMask.Show("导入中...");


    $.ajaxFileUpload({

        url: '/WMS/PreOrder/BatchIimportUpdateLoadKey',
        secureuri: false,
        fileElementId: 'importExcel1',
        dataType: 'json',
        data: {
            Action:"批量更新订单品级",
            CustomerID: $('#StorerID').val()
        },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            //$('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
            $('#outPutResult1').html("<h4>" + data.result + "</h4>");
        },
        error: function (data, status, e) {
            //$('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            $('#outPutResult1').html("<h4>" + data.result + "</h4>");
            WebPortal.MessageMask.Close();
        }
    });
    return false;
};    /*导入按钮结束*/

