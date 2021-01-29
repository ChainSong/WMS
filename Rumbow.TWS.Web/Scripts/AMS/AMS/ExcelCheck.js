var fileImportClick = function () {
    if ($('#importExcel').val() === '') {
        Runbow.TWS.Alert("请选择要导入的Excel");
        return false;
    }
    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) === null) {
        Runbow.TWS.Alert("请选择Excel格式的文件");
        $('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    }
    //$('#resultCount').text(0);
    WebPortal.MessageMask.Show("验证中...");

    //var customer = $('#customer').val();
    $.ajaxFileUpload({
        url: '/AMS/AMS/ExcelCheck',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: { customer: 'test' },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            $('#outPutResult').html(data.result);
            //$('#resultCount').text(data.Count);
        },
        error: function (data, status, e) {
            Runbow.TWS.Alert('验证失败');
            WebPortal.MessageMask.Close();
        }
    });
};