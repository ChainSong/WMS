var fileImportClick = function () {
    if ($('#importExcel').val() === '') {
        Runbow.TWS.Alert("请选择要导入的ZIP文件");
        return false;
    }

    var exp = /.zip|.ZIP/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) === null) {
        Runbow.TWS.Alert("请选择ZIP格式的文件");
        $('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    }

    $('#resultCount').text(0);
    WebPortal.MessageMask.Show("导入中...");

    var customer = $('#customer').val();
    var customerName = $("#customer").find("option:selected").text();
    $.ajaxFileUpload({
        url: '/AMS/AMS/ReplyDocument_Upload',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: { customer: customer, customerName: customerName },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            $('#outPutResult').html(data.result);
            $('#resultCount').text(data.Count);
        },
        error: function (data, status, e) {
            Runbow.TWS.Alert('导入失败');
            WebPortal.MessageMask.Close();
        }
    });
};