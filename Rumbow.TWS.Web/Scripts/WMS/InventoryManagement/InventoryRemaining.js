$(document).ready(function () {
   
})

//批量导入新增
function fileImportClick() {
    if ($('#importExcel').val() === '') {
        showMsg("请选择要导入的Excel", "4000");
        return false;
    }
    var CustomerID = $('#CustomerID option:selected').val();
    var DateTime1 = $('#InventorySearchCondition_DateTime1').val();
    
    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        showMsg("请选择Excel格式的文件", "4000");
        $(this).before(fileImport);
        return false;
    };
    WebPortal.MessageMask.Show("导入中...");
    //var CustomerID = $("#CustomerID").val()
    //var customername = $('select#CustomerID').find('option:selected').text();
    //var warehousename = $('select#warehousename').find('option:selected').text();
    $.ajaxFileUpload({
        url: '/WMS/InventoryManagement/DirectAddInventoryImports',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: {
            CustomerID: CustomerID,
            Data: DateTime1
        },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            $('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
        },
        error: function (data, status, e) {
            $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            WebPortal.MessageMask.Close();
        }
    });
    return false;
}