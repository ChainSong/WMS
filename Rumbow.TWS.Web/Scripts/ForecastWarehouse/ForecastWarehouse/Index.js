








function showDialog(id, state) {

    var opts = {
        'title': '指定日期发货',
        'content': $('#showInDialog').show(),
        'buttons': {
            "确定": function () {
                $.ajax({
                    type: 'POST',
                    url: '/ForecastWarehouse/ForecastWarehouse/SpecifiedDeliveryDate',
                    secureuri: false,
                    dataType: 'json',
                    data: { Id: id, state: state, PickTime: Runbow.TWS.Popup.find('#zhi2').val() },
                    success: function (data) {
                        //WebPortal.MessageMask.Close();
                        //$('#outPutResult').html(data.result);
                        $('#query').trigger('click');
                 },
                    error: function (data) {

                        //WebPortal.MessageMask.Close();
                        //$('#outPutResult').html(data.result);
                        alert('请输入正确日期格式');
                       
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

function showDialog2(id, state) {

    var opts = {
        'title': '要求提货时间',
        'content': $('#showInDialog2').show(),
        'buttons': {
            "确定": function () {
                $.ajax({
                    type:'POST',
                    url: '/ForecastWarehouse/ForecastWarehouse/SpecifiedDeliveryDate2',
                    secureuri: false,
                    fileElementId: 'importExcel',
                    dataType: 'json',
                    data: { Id: id, state: state, PickTime: Runbow.TWS.Popup.find('#zhi2').val() },
                    success: function (data) {
                        //WebPortal.MessageMask.Close();
                        //$('#outPutResult').html(data.result);
                        $('#query').trigger('click');
                        
                    },
                    error: function (data) {

                        //WebPortal.MessageMask.Close();
                        alert('请输入正确日期格式');
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
function tiao() {


}