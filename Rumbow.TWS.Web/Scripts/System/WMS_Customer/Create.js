$(document).ready(function () {

    //InputLoad();

    $('#returnButton').click(function () {
        window.location.href = "/System/WMS_Customer/Index?customerType=2"
    });

    $('#btnCreate').click(function () {      
        if ($.trim($('#StorerKey').val()) == "" || $('#StorerKey').val() == "必填") {
            layer.msg('货主代码不能为空！', {
                offset: '1px',
                anim: 6
            });
            return false;
        }
        if ($.trim($('#City').val()) == "" || $('#City').val() == "必填") {
            layer.msg('城市不能为空！', {
                offset: '1px',
                anim: 6
            });
            return false;
        }
        if ($.trim($('#Company').val()) == "" || $('#Company').val() == "必填") {
            layer.msg('店铺名称不能为空！', {
                offset: '1px',
                anim: 6
            });
            return false;
        }
        if ($.trim($('#UserDef10').val()) == "" || $('#UserDef10').val() == "必填") {
            layer.msg('店铺简称不能为空!', {
                offset: '1px',
                anim: 6
            });
            return false;
        }
        if ($.trim($('#CompanyCode').val()) == "" || $('#CompanyCode').val() == "必填") {
            layer.msg('SAP代码不能为空!', {
                offset: '1px',
                anim: 6
            });
            return false;
        }
        if ($.trim($('#AddressLine1').val()) == "" || $('#AddressLine1').val() == "必填") {
            layer.msg('地址1不能为空!', {
                offset: '1px',
                anim: 6
            });
            return false;
        }
        if ($.trim($('#Contact1').val()) == "" || $('#Contact1').val() == "必填") {
            layer.msg('联系人1不能为空！', {
                offset: '1px',
                anim: 6
            });
            return false;
        }
        if ($.trim($('#PhoneNum1').val()) == "" || $('#PhoneNum1').val() == "必填") {
            layer.msg('联系电话1不能为空！', {
                offset: '1px',
                anim: 6
            });
            return false;
        }
        
        if ($('#UserDef3').val() == "") {
            layer.msg('门店属性为必选项', {
                offset: '1px',
                anim: 6
            });
            return false;
        }
        

    })

    //function InputLoad() {
    //    $('input[class="input-validation-error form-control"]').each(function () {
    //        var oldVal = $(this).val();   //默认的提示性文本     
    //        $(this)
    //        .css({ "color": "#888" })  //灰色     
    //        .focus(function () {
    //            if ($(this).val() != oldVal) {
    //                $(this).css({ "color": "#000" })
    //            }
    //            else {
    //                $(this).val("").css({ "color": "#888" })             
    //            }
    //        })
    //        .blur(function () {
    //            if ($(this).val() == "") {
    //                $(this).val(oldVal).css({ "color": "#888" })
    //            }
    //        })
    //        .keydown(function () { $(this).css({ "color": "#000" }) })
    //    })
    //    $('input[class="form-control"]').each(function () {
    //        var oldVal = $(this).val();   //默认的提示性文本     
    //        $(this)
    //        .css({ "color": "#888" })  //灰色     
    //        .focus(function () {
    //            if ($(this).val() != oldVal) {
    //                $(this).css({ "color": "#000" })
    //            }
    //            else {
    //                $(this).val("").css({ "color": "#888" })
                   
    //            }
    //        })
    //        .blur(function () {
    //            if ($(this).val() == "") {
    //                $(this).val(oldVal).css({ "color": "#888" })
    //            }
    //        })
    //        .keydown(function () { $(this).css({ "color": "#000" }) })
    //    })
    //}

    //var objtemp = '@ViewBag.Message';
    var objtemp = $('#message').val();
    if (objtemp != undefined && objtemp != "" && objtemp != "/") {
        if (objtemp == 0) {
            layer.confirm('保存成功 , 是否返回？', {
                btn: ['确定', '取消'] //按钮
            }, function () {
                window.location.href = "/System/WMS_Customer/Index";
                //window.location.href = "/WMS/Warehouse/WarehouseAllocate?CustomerID=" + objtemp;
            }, function () {

            });
        }
        else {

        }
    }

    //判断客户名称唯一性
    //$('#Name').blur(function () {
    //    //debugger;
    //    var tempvalue = $(this).val();
    //    var id = $("#ID").val();
    //    $.ajax({
    //        type: 'POST',
    //        url: "/System/WMS_Customer/CheckName",
    //        data: { Name: tempvalue, Id: id, IsEdit: false },
    //        success: function (data) {
    //            if (data != "") {
    //                layer.msg(data, function () { });
    //                $("#Name").focus();
    //                $('#btnCreate').prop('disabled', true);
    //            } else {
    //                $('#btnCreate').prop('disabled', false);
    //            }
    //        },
    //        error: function () {
    //        }
    //    })
    //});
    //switch ($("#StoreType").val()) {
    //    case "1":
    //        $("#trSegment").css('display', '');
    //        break;
    //    default:
    //        $("#trSegment").css('display', 'none');
    //        break;
    //}

    //$('#StoreType').change(function () {
    //    var tempvalue = $(this).val();
    //    switch (tempvalue) {
    //        case "1":
    //            $("#trSegment").css('display', '');
    //            break;
    //        default:
    //            $("#trSegment").css('display', 'none');
    //            break;
    //    }
    //});

})