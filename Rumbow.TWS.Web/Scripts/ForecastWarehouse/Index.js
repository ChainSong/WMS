








function showDialog(id, state) {

    var opts = {
        'title': '发货日期',
        'content': $('#showInDialog').show(),
        'buttons': {
            "确定": function () {
                $.ajaxFileUpload({
                    url: '/ForecastWarehouse/ForecastWarehouse/SpecifiedDeliveryDate',
                    secureuri: false,
                    fileElementId: 'importExcel',
                    dataType: 'json',
                    data: { Id: id ,state:state},
                    success: function (data) {
                        //WebPortal.MessageMask.Close();
                        $('#outPutResult').html(data.result);
                    },
                    error: function (data) {

                        //WebPortal.MessageMask.Close();
                        $('#outPutResult').html(data.result);
                    }
                });
                Runbow.TWS.Popup.close();
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