$(document).ready(function () {
    $('#addButton').click(function () {
        window.location.href = "/System/WMS_Customer/Create/";
    });
    $(".Ooperation").live("mouseover", function () {
        $(this).prev()[0].style.display = "none";
    });
    $(".Ooperation").live("mouseenter", function () {
        $(this).prev()[0].style.display = "";
    })
    $(".Adiv").live("mouseleave", function () {
        $(this)[0].style.display = "none";
    })
    $("tr").live("mouseenter", function () {
        $(".Adiv").each(function (a, b) {
            $(b)[0].style.display = "none";
        })
    })
    $('.shipperStatus').click(function () {
        var shipper = $(this).attr('data-name');
        var model = $(this).attr('data-id');
        location.href = "/System/WMS_Customer/Create/" + shipper + "?ViewType=1" + "&customerType=" + model + "&customerid=" + $("#CustomerID").val();
    });
    $("#portButtonTemplet").click(function () {
        demo('/System/WMS_Customer/StorerDemoExecl');
    })
    $('#searchButton').live('click', function () {
        $('#PageIndex').val(0);
    });
});


function DeleteCustomer(id) {
    layer.confirm('您确认要删除吗？', {
        btn: ['确认', '取消'] //按钮
    }, function () {
        $.ajax({
            type: 'POST',
            url: "/System/WMS_Customer/Delete",
            data: {
                StorerKey: id,
                CustomerID: $("#CustomerID").val()
            },
            success: function (data) {
                layer.msg('删除成功！', { icon: 1,time:1000 }, function () { window.location.href = '/System/WMS_Customer/Index'; });
            },
            error: function () {
            }
        });
    }, function () {

    });
}

var fileImportClick = function () {
    if ($('#importExcel').val() === '') {
        showMsg("请选择要导入的Excel", 4000);
        return false;
    }
    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        showMsg("请选择Excel格式的文件", 4000);
        $(this).before(fileImport);
        return false;
    };
    WebPortal.MessageMask.Show("导入中...");
    $.ajaxFileUpload({
        url: '/System/WMS_Customer/ExeclStorer',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: { StorerID: $("#StorerID").val() },
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            if (data.IsSuccess == true) {
                $('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
            }
            else {
                $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            }
        },
        error: function (data, status, e) {
            $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            WebPortal.MessageMask.Close();
        }
    });
};



function demo(url) {
    var form = $("<form>");
    form.attr('style', 'display:none');
    form.attr('target', '');
    form.attr('method', 'post');
    form.attr('action', url);
    var input1 = $('<input>');
    input1.attr('type', 'hidden');
    input1.attr('name', 'demo');
    input1.attr('value', 'Export');
    var input2 = $('<input>');
    input2.attr('type', 'hidden');
    input2.attr('name', 'fileId');
    input2.attr('value', "fileId");
    $('body').append(form);
    form.append(input1);
    form.append(input2);

    form.submit();
    form.remove();
}

