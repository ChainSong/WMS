﻿$(function () {

});

var fileImportClick = function () {
    if ($('#importExcel').val() === '') {
        Runbow.TWS.Alert("请选择要导入的运单Excel");
        return false;
    }

    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        Runbow.TWS.Alert("请选择Excel格式的文件");
        $('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    };

    WebPortal.MessageMask.Show("导入中...");

    $.ajaxFileUpload({
        url: '/CRM/Crm/PodImport',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: {},
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            $('#outPutResult').html(data.result)
        },
        error: function (data, status, e) {
            Runbow.TWS.Alert('导入运单失败');
            WebPortal.MessageMask.Close();
        }
    });
};