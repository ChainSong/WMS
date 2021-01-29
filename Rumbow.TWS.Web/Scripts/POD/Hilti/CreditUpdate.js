
 function myfileImportClick () {
        if ($('#importExcel').val() === '') {
            Runbow.TWS.Alert("请选择要导入的Excel");
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

        WebPortal.MessageMask.Show("更新中...");
    };

 function myFresh() {
     WebPortal.MessageMask.Show("更新中...");
     $.ajax({
         type: "post",
         url: "/POD/Hilti/UpdatePOD",
         dataType: "json",
         success: function (data) {
             $('#msg').html(data);
         },
         error: function (data) {
             alert(data);
         }
     });
     WebPortal.MessageMask.Close();
 };



