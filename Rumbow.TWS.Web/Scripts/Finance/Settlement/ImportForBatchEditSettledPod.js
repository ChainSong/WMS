var fileImportClick = function (type) {
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

    var customer = $('#customer').val();
    if (customer == '') {
        Runbow.TWS.Alert('无调整权限');
        return false;
    }

    $('#resultCount').text(0);
    WebPortal.MessageMask.Show("导入中...");

   
    $.ajaxFileUpload({
        url: '/Finance/Settlement/ImportForBatchEditSettledPod',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: { type: type, customer: customer },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            if (data.IsSuccess) {
                $('#outPutResult').html(data.result);
                $('#resultCount').text(data.Count);
            } else {
                $('#outPutResult').html(data.result);
            }
        },
        error: function (data, status, e) {
            Runbow.TWS.Alert('导入失败');
            WebPortal.MessageMask.Close();
        }
    });
};