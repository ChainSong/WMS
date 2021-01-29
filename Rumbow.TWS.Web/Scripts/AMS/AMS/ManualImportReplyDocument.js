var fileImportClick = function () {
    if (confirm("确定导入吗？")) {
        //$('#resultCount').text(0);
        WebPortal.MessageMask.Show("导入中...");

        //var customer = $('#customer').val();
        //var customerName = $("#customer").find("option:selected").text();
        $.ajaxFileUpload({
            url: '/AMS/AMS/SetImportReplyDocument',
            secureuri: false,
            fileElementId: 'importExcel',
            dataType: 'json',
            //data: { customer: customer, customerName: customerName },
            success: function (data, status) {
                WebPortal.MessageMask.Close();
                Runbow.TWS.Alert('导入成功');
                //alert("导入成功");
                window.location.href = "ManualImportReplyDocument";
                //$('#outPutResult').html(data.result);
                //$('#resultCount').text(data.Count);
            },
            error: function (data, status, e) {
                Runbow.TWS.Alert('导入失败');
                WebPortal.MessageMask.Close();
            }
        });
    }
};