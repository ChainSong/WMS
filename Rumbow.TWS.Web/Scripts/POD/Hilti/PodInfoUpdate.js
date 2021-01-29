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
    showDialog();
    //WebPortal.MessageMask.Show("更新中...");

    
};




var showDialog = function () {
   
    var opts = {
        'title': '发货日期',
        'content': $('#showInDialog').clone().show(),
        'buttons': {
            "确定": function () {
                //WebPortal.MessageMask.Show("更新中...");
                $.ajaxFileUpload({
                    url: '/POD/Hilti/PodInfoUpdate',
                    secureuri: false,
                    fileElementId: 'importExcel',
                    dataType: 'json',
                    data: { ActualDeliveryDate: Runbow.TWS.Popup.find('#payDate').val() },
                    success: function (data) {
                        Runbow.TWS.Popup.close();
                        $('#outPutResult').html(data.result);
                       
                    },
                    error: function (data) {
                        Runbow.TWS.Popup.close();
                        $('#outPutResult').html(data.result);
                    }
                });
                
               // Runbow.TWS.Popup.close();
                
                //$("#ActualDeliveryDate").val(Runbow.TWS.Popup.find('#payDate').val());
                
            },
            "取消": function () {
                Runbow.TWS.Popup.close();
            }
        },
        'width': '400',//default 400
        'minHeight': '300' //default 300
    };

    Runbow.TWS.Popup.show(opts);
   
};