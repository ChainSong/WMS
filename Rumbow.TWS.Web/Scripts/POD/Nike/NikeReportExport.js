$(function () {

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
};



$(document).ready(function(){

    $('.calendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = '';
        var Begin = 'Begin';
        var End = 'End';
        if (pref === 'start') {
            descID = Begin + actualID;
        }
        else {
            descID = End + actualID;
        }
        $(this).val($('#' + descID).val());
    });


     
    var setValToHiddenControl = function () {
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID;
            if (pref === 'start') {
                descID = "Begin" + actualID;
            }
            else {
                descID = 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    };

    $('#ExportButton').click(function () { setValToHiddenControl(); });
    $('#QueryButton').click(function () { setValToHiddenControl(); });




    $('#QueryButton').click(function () {
        $('#PageIndex').val('0');
        $('#IsExport').val('False');
       
    });


    $('#ExportButton').click(function () {
        $('#IsExport').val('True');

    });


    $('#ShipperName').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Pod/Pod/GetUserShipper",
                type: "POST",
                dataType: "json",
                data: { name: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#ShipperID').val(ui.item.data.Value);
            $('#ShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#ShipperID').val('');
        }
    });

});

    var fileExportClick = function () {
   
        WebPortal.MessageMask.Show("导出中...");

    };


    var EmptyValue = function (obj) {
        var IDName = $(obj).attr('id');
        
        var val = IDName.split('_')[0];
        var Begin = 'start_' + val;
        var End = 'end_' + val;
        $('#' + Begin).val("");
        $('#' + End).val("");
    };
