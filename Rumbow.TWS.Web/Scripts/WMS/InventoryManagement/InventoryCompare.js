$(document).ready(function () {
    $("#portButtonTemplet").click(function () {
        demo();
    })
    function demo(url) {
        var ID = $('select[id=StorerID]')[0].value;
        var url = '/WMS/InventoryManagement/ReportExportExecl';
        var form = $("<form>");
        form.attr('style', 'display:none');
        form.attr('target', '');
        form.attr('method', 'post}');
        form.attr('action', url);//'/WMS/PreOrder/ReportExportExecl'
        var input1 = $('<input>');
        input1.attr('type', 'hidden');
        input1.attr('name', 'demo');
        input1.attr('value', 'Export');
        var input2 = $('<input>');
        input2.attr('type', 'hidden');
        input2.attr('name', 'fileId');
        input2.attr('value', "fileId");
        var input3 = $('<input id="ID" name="ID" type="hidden" value="' + ID + '" />');
        $('body').append(form);
        form.append(input1);
        form.append(input2);
        form.append(input3);

        form.submit();
        form.remove();
    }
});

var fileImportClick = function () {
    if ($('#importExcel').val() === '') {
        showMsg("Please select the Excel to import!", 2000);
        return false;
    }

    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        showMsg("Please select Excel formatted files!", 4000);
        $(this).before(fileImport);
        return false;
    };

    //var load = $('#importExcel').val();
    layer.msg('对比中，请稍后...', {
        icon: 16
      , shade: 0.3
    });

    //$('#outPutResult').html("<h3><font color='#FF0000'>Import successful!</font></h3>");

    //WebPortal.MessageMask.Show("Importing...");
    $.ajaxFileUpload({
        url: "/WMS/InventoryManagement/InventoryCompare",
        secureuri: false,
        fileElementId: 'importExcel',
        type: "POST",
        dataType: "json",
        data: {
            CustomerName: $('#StorerID option:selected').text(),
            CustomerID: $('#StorerID').val(),
            Type:"importexcel"
        },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            if (data.IsSuccess == true) {
                //$('#importExcel').val() = load;
                $('#outPutResult').html(data.result);
                //fileExportClick();
            } else {
                $('#outPutResult').html(data.result);
            }
        },
        error: function (data, status, e) {
            WebPortal.MessageMask.Close();
            showMsg(data.responseText, 4000);
        }
    });
};

var fileExportClick = function () {
    //加载层-风格4
    layer.msg('加载中', {
        icon: 16
      , shade: 0.01
    });
    //WebPortal.MessageMask.Show("Exporting...");
    $.ajaxFileUpload({
        url: "/WMS/InventoryManagement/InventoryCompare",
        secureuri: false,
        fileElementId: 'importExcel',
        type: "POST",
        dataType: "json",
        data: {
            CustomerName: $('#StorerID option:selected').text(),
            CustomerID: $('#StorerID').val(),
            Type: "exportexcel"
        },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            if (data.IsSuccess == true) {
                $('#outPutResult').html(data.result);
            } else {
                $('#outPutResult').html(data.result);
            }
        },
        error: function (data, status, e) {
            WebPortal.MessageMask.Close();
            showMsg(data.responseText, 4000);
        }
    });
};
