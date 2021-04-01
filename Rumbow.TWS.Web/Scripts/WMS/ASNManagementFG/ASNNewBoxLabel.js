//退货仓 -
var txt = "确认是否打印上架单";
var Total;
$(document).ready(function () {
    if (sessionStorage.getItem("WmsUserName") == "SHDZ01") {
        $("#statusBack").show();
    }

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

    if ($("#resultTable tbody")[0].rows.length < 1) {
        $("#resultTable").removeAttr("style");
        $("#tables").removeAttr("style");
        $("#OperateTh").removeAttr("style");
    }

    $('#newboxButton').click(function () {

        layer.open({
            title: '<h4 style="color: #ff0000;text-align:center">请输入新建箱号所需数据</h4> ',
            type: 1,
            closeBtn: 0,
            skin: 'layui-layer-rim', //加上边框
            area: ['300px', '250px'], //宽高
            content: $("#NewBoxLabel")
        });

        //prompt层
        //layer.prompt({ title: '<font color="red">请输入新建箱的数量，并确认</font>', formType: 0 }, function (pass, index) {
        //    if (isNaN(pass)) {
        //        layer.alert('<font color="red">请输入数字！</font>');
        //        return;
        //    }
        //    layer.close(index);
        //    //layer.msg('您的输入：' + pass);
        //    Total = pass;
        //    //layer.prompt({ title: '随便写点啥，并确认', formType: 2 }, function (text, index) {
        //    //    layer.close(index);
        //    //    layer.msg('演示完毕！您的口令：' + pass + '<br>您最后写下了：' + text);
        //    //});
        //    $.ajax({
        //        url: "/WMS/ASNManagementFG/newbox",
        //        type: "POST",
        //        dataType: "json",
        //        data: {
        //            customerid: $('#SearchCondition_CustomerID').val(),
        //            warehouseid: $('#SearchCondition_WarehouseID').val(),
        //            total: Total
        //        },
        //        success: function (data) {
        //            if (data.Result != 0) {
        //                if (data.Result == Total) {
        //                    //勾选
        //                    $("#searchButton").click();

        //                    //默认勾选
        //                    var checkBoxs = $("#checkForSelect");
        //                    for (var i = 0; i < checkBoxs.length; i++) {
        //                        if ($("#data_ID").val()) {
        //                            checkBoxs.attr("checked", "checked");
        //                        }
        //                    }
        //                    //layer.confirm('已新建并勾选' + Total +'个箱号，是否立刻开始打印？', {
        //                    //    btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //                    //    title: ['提示', 'font-size:18px;']
        //                    //    //按钮
        //                    //}, function (index) {
        //                    //    layer.close(index);
        //                    //    }, function (index) {
        //                    //        layer.close(index);
        //                    //    });
        //                }
        //                else {
        //                    layer.alert('<font color="red">新建箱异常！预计' + Total + '个，实际新建' + data.Result +'个箱号。</font>')
        //                }
        //            }
        //            else {
        //                layer.alert('<font color="red">新建箱失败，请重试！</font>')
        //            }
        //        },
        //        error: function (data){
        //            layer.alert('<font color="red">新建箱失败，请重试！</font>')
        //        }
        //    });

        //});

        
        // 默认勾选
        //var checkBoxs = $(".checkForSelect");
        //for (var i = 0; i < checkBoxs.length; i++) {
        //    if (checkBoxs[i].dataset.int1 == "0") {
        //        checkBoxs[i].defaultChecked = true;
        //    }
        //}

    });








    $("#printButton").live("click", function () {
        layer.confirm('<font size="4">确认是否批量打印箱号？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            if ($("#resultTable tbody input[type='checkbox']:checked").size() == 0) {//加了一个判断，当用户没有选择要打印订单时,提示 20170807
                showMsg("请选择单据！", "4000");
                return;
            }
            var str = '';
            $('.checkForSelect').each(function (index) {
                if ($(this).attr('checked') === 'checked') {
                    str += "" + $(this).attr('data-id') + "" + ",";
                }
            })
            if ($("#ProjectName").val() == "NIKEReturn") {
                location.href = '/WMS/ASNManagementFG/PrintASNNewBoxLabel?ids=' + str.substring(0, str.length - 1) + "&type=1";
            }
        });
    });

    $('#searchButton').click(function (event) {
        if ($('#SearchCondition_CustomerID').val() == '') {
            //小tips
            layer.tips('请选择客户', '#SearchCondition_CustomerID', {
                tips: [1, '#3595CC'],
                time: 2000
            });
            event.preventDefault();
        }
        else if ($('#SearchCondition_WarehouseID').val() == '')
        {
            //小tips
            layer.tips('请选择仓库', '#SearchCondition_WarehouseID', {
                tips: [1, '#3595CC'],
                time: 2000
            });
            event.preventDefault();
        }
        setPageControlVal();
    });

    $('.DynamicCalendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = 'SearchCondition_';
        if (pref === 'start') {
            descID += 'Start' + actualID;
        }

        else {
            descID += 'End' + actualID;
        }
        $(this).val($('#' + descID).val());
    });

    $('.DropDownList').each(function (index) {
        var id = $(this).attr("id");
        var descId = "SearchCondition_" + id.substr(0, id.length - 4) + "ID";
        $(this).val($('#' + descId).val());
    });

    $('select[id=SearchCondition_CustomerID]').live('change', function () {
    });

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }
    });

    $("#resultTable tbody input[type='checkbox']").live('click', function () {
        RefreshIDs();
    });

    setHiddenValToControl();

    //默认勾选
    //var checkBoxs = $(".checkForSelect");
    //for (var i = 0; i < checkBoxs.length; i++) {
    //    if (checkBoxs[i].dataset.int1 == "0") {
    //        checkBoxs[i].defaultChecked = true;
    //    }
    //}
});


function NewOkBoxAndNo() {
    var total = $("#Newboxnumber").val();
    if ($("#ExternReceiptNumber").val() == "") {
        layer.alert('请输入需要新建箱号的外部单号', {
            skin: 'layui-layer-lan' //样式类名
            , icon: 2
            , closeBtn: 0
            , btn: ['确定'] //单击按钮
            , btn1: function (index, layero) {
                layer.close(index);
                //$("#ExternReceiptNumber").focus();
                //$("ExternReceiptNumber").select();
            },
            success: function (layero) {
                var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                btn.href = 'javascript:void(0)';
                btn.focus();
            }
        });
        return;
    }
    if ($("#Newboxnumber").val() == "") {
        layer.alert('请输入需要打印箱标贴的数量', {
            skin: 'layui-layer-lan' //样式类名
            , icon: 2
            , closeBtn: 0
            , btn: ['确定'] //单击按钮
            , btn1: function (index, layero) {
                layer.close(index);
                //$("#Newboxnumber").focus();
                //$("#Newboxnumber").select();
            },
            success: function (layero) {
                var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                btn.href = 'javascript:void(0)';
                btn.focus();
            }
        });
        return;
    }
    if ($("#GoodsType").val() == "") {
        layer.alert('请选择产品等级', {
            skin: 'layui-layer-lan' //样式类名
            , icon: 2
            , closeBtn: 0
            , btn: ['确定'] //单击按钮
            , btn1: function (index, layero) {
                layer.close(index);
                //$("#sku").focus();
                //$("#sku").select();
            },
            success: function (layero) {
                var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                btn.href = 'javascript:void(0)';
                btn.focus();
            }
        });
        return;
    }
    
            $.ajax({
                url: "/WMS/ASNManagementFG/newbox",
                type: "POST",
                dataType: "json",
                data: {
                    customerid: $('#SearchCondition_CustomerID').val(),
                    warehouseid: $('#SearchCondition_WarehouseID').val(),
                    ExternReceiptNumber: $("#ExternReceiptNumber").val(),
                    total: total ,
                    GoodsType: $("#GoodsType").val()
                },
                success: function (data) {
                    if (data.Result != 0) {
                        if (data.Result == total) {
                            //勾选
                            $("#searchButton").click();

                            //默认勾选
                            var checkBoxs = $("#checkForSelect");
                            for (var i = 0; i < checkBoxs.length; i++) {
                                if ($("#data_ID").val()) {
                                    checkBoxs.attr("checked", "checked");
                                }
                            }
                            //layer.confirm('已新建并勾选' + Total +'个箱号，是否立刻开始打印？', {
                            //    btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                            //    title: ['提示', 'font-size:18px;']
                            //    //按钮
                            //}, function (index) {
                            //    layer.close(index);
                            //    }, function (index) {
                            //        layer.close(index);
                            //    });
                        }
                        else {
                            layer.alert('<font color="red">新建箱异常！预计' + $("#Newboxnumber").val() + '个，实际新建' + data.Result +'个箱号。</font>')
                        }
                    }
                    else {
                        layer.alert('<font color="red">新建箱失败，请重试！</font>')
                    }
                },
                error: function (data){
                    layer.alert('<font color="red">新建箱失败，请重试！</font>')
                }
            });


   

    //$.ajax({
    //    type: "Post",
    //    url: '/Receipt/NewSetSKUNo',
    //    data: { "AsnNumber": $("#receiptnum").val(), "GoodsType": $("#GoodsType").val(), "SKU": "00" + $("#sku").val(), "No": $("#tempNo").val(), "BoxNumber": $("#Newboxnumber").val() },
    //    dataType: "json",
    //    async: false,
    //    success: function (data) {
    //        if (data.Code == "1") {
    //            if (data.data[0].SKU == "00" + $("#sku").val() && data.data[0].BoxNumber == $("#Newboxnumber").val()) {
    //                $("#SuccessAudio")[0].play();
    //                $("#skuid")[0].innerText = $("#GoodsType").val() + "区域:" + data.data[0].No + "号箱";
    //                $("#boxid")[0].innerText = "箱号:" + data.data[0].BoxNumber;
    //                $("#tempBoxNumber").val(data.data[0].BoxNumber);
    //                //$("#tempNo").val(data.data[0].No);
    //                $("#sku").focus();
    //                $("#sku").select();
    //                layer.closeAll();
    //                $("#skunumid")[0].innerText = "已扫件数：0";
    //            }
    //            else {
    //                $("#Audio")[0].play();
    //                layer.alert('错误提示', {
    //                    skin: 'layui-layer-lan' //样式类名
    //                    , icon: 2,
    //                    title: '绑定冲突'
    //                    , content: '<div>SKU:' + data.data[0].SKU + '</div><div> 序号：' + data.data[0].No + '</div><div> 箱号：' + data.data[0].BoxNumber + '</div>'
    //                    , closeBtn: 0
    //                    , btn: ['确定'] //单击按钮
    //                    , btn1: function (index, layero) {
    //                        layer.close(index);
    //                        $("#sku").focus();
    //                        $("#sku").select();
    //                        document.getElementById("sku").select();
    //                        $("#skuid")[0].innerText = "";
    //                        $("#boxid")[0].innerText = "";
    //                    },
    //                    success: function (layero) {
    //                        var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
    //                        btn.href = 'javascript:void(0)';
    //                        btn.focus();
    //                    }
    //                })

    //            }

    //        }
    //        if (data.Code == "0") {
    //            $("#Audio")[0].play();
    //            layer.alert('错误提示', {
    //                skin: 'layui-layer-lan' //样式类名
    //                , icon: 2
    //                , closeBtn: 0,
    //                title: '箱号绑定冲突'
    //                , content: '<div>SKU:' + data.data[0].SKU + '</div><div> 序号：' + data.data[0].No + '</div><div> 箱号：' + data.data[0].BoxNumber + '</div>'
    //                , btn: ['确定'] //单击按钮
    //                , btn1: function (index, layero) {
    //                    layer.close(index);
    //                    $("#sku").focus();

    //                    document.getElementById("sku").select();

    //                },
    //                success: function (layero) {
    //                    var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
    //                    btn.href = 'javascript:void(0)';
    //                    btn.focus();
    //                }
    //            })
    //        }
    //        if (data.Code == "-1") {
    //            $("#Audio")[0].play();
    //            layer.alert(ex.Message, {
    //                skin: 'layui-layer-lan' //样式类名
    //                , icon: 2
    //                , closeBtn: 0
    //                , btn: ['确定'] //单击按钮
    //                , btn1: function (index, layero) {
    //                    layer.close(index);
    //                    $("#sku").focus();
    //                    document.getElementById("sku").select();
    //                    $("#skuid")[0].innerText = "";
    //                    $("#boxid")[0].innerText = "";
    //                },
    //                success: function (layero) {
    //                    var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
    //                    btn.href = 'javascript:void(0)';
    //                    btn.focus();
    //                }
    //            })
    //        }

    //    },
    //    error: function (msg) {

    //    }

    //});



}

function NewCancelBoxAndNo() {
    layer.closeAll();
    $("#ExternReceiptNumber").focus();
    $("#ExternReceiptNumber").select();
    $("#Newboxnumber").focus();
    $("#Newboxnumber").select();
}


function TableToJson() {
    var a = "";
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            a += checkBoxs[i].name + ",";
        }
    }
    a = a.substring(0, a.length - 1);
    return a;
}
function TableToJsonJC() {
    var txt = "{\"jCRequestLists\":[";
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            var r = "{";
            r += "\"ID\":\"" + checkBoxs[i].dataset.id + "\",";
            r += "\"CustomerID\":\"" + checkBoxs[i].dataset.customerid + "\",";
            r += "\"WarehouseID\":\"" + checkBoxs[i].dataset.warehouseid + "\",";
            r += "\"RelateNumber\":\"" + checkBoxs[i].dataset.receiptnumber + "\",";
            r = r.substring(0, r.length - 1)
            r += "},";
            txt += r;
        }
    }
    txt = txt.substring(0, txt.length - 1);
    txt += "]}";
    return txt;
}
var setHiddenValToControl = function () {
    $('.notKeyVal').each(function (index) {
        var id = $(this).attr("id");
        var descId = "SearchCondition_" + id;
        if ($(this).attr("type") === "checkbox") {
            if ($('#' + descId).val() === '1') {
                $(this).attr('checked', 'checked');
            } else {
                $(this).removeAttr('checked');
            }

        } else if ($(this).attr("type") === "DropDownList") {
            var desc = $('#' + descId);
            if (desc.val() === '1' || desc.val() === 'Y' || desc.val === 'y' || desc.val() === '是') {
                $(this).val('1');
            } else {
                $(this).val('0');
            }

        } else {
            $(this).val($('#' + descId).val());
        }
    });
}
var RefreshIDs = function () {
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    var length = checkBoxs.length;
    var checked = 0;
    checkBoxs.each(function () {
        if ($(this).attr("checked") === "checked") {
            checked++;
        }
    });
    if (checked == checkBoxs.length) {
        $('#selectAll').attr("checked", "checked");
    } else {
        $('#selectAll').removeAttr("checked");
    }
}
var setPageControlVal = function () {
    $('.notKeyVal').each(function (index) {
        var id = $(this).attr("id");
        var descId = "SearchCondition_" + id;
        if ($(this).attr("type") === "checkbox") {
            var isChecked = document.getElementById(id).checked;
            if (isChecked) {
                $('#' + descId).val("1");
            } else {
                $('#' + descId).val("0");
            }
        } else {
            $('#' + descId).val($(this).val());
        }
    });
    $('.DropDownList').each(function (index) {
        var id = $(this).attr("id");
        var descId = "SearchCondition_" + id.substr(0, id.length - 4) + "ID";
        $('#' + descId).val($(this).val());
    });
    $('.calendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = 'SearchCondition_';
        if (pref === 'start') {
            descID += 'Start' + actualID;
        }
        else {
            descID += 'End' + actualID;
        }
        $('#' + descID).val($(this).val());
    });
}
function printPick(ID) {
    layer.confirm('<font size="4">确认是否打印上架单？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        if ($("#ProjectName").val() == "NIKE") {
            location.href = '/WMS/ReceiptManagement/PrintShelvesNike?rid=' + ID + "&Flag=1";
        }
        else if ($("#ProjectName").val() == "YXDR") {
            location.href = '/WMS/ReceiptManagement/PrintShelvesYXDR?rid=' + ID + "&Flag=1";
        }
        else if ($("#ProjectName").val() == "延锋百利得") {
            location.href = '/WMS/ReceiptManagement/PrintShelvesYFBLD?rid=' + ID + "&Flag=1";
        }
        else {
            location.href = '/WMS/ReceiptManagement/PrintShelvesNike?rid=' + ID + "&Flag=1";
        }
    });
}
function ShowsIn(ID, obj) {
    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
        $(".ddiv:not(obj)").animate({
            width: "hide",
            width: "400%",
            paddingRight: "hide",
            paddingLeft: "hide",
            marginRight: "hide",
            marginLeft: "hide"
        }, 100);
        $("#operateTD" + ID).animate({
            width: "show",
            width: "445%",
            paddingRight: "show",
            paddingLeft: "show",
            marginRight: "show",
            marginLeft: "show"
        });
    }
}
function ShowsOut() {
    $(".ddiv").animate({
        width: "hide",
        width: "400%",
        paddingRight: "hide",
        paddingLeft: "hide",
        marginRight: "hide",
        marginLeft: "hide"
    }, 100);
}
function GenerateBarCode(receiptId) {
    window.location.href = '/WMS/ReceiptManagement/GenerateBarCode?ID=' + receiptId + "&ViewType=0";
}
function ScanBarCode(receiptId) {
    window.location.href = '/WMS/ReceiptManagement/ScanBarCode?ID=' + receiptId + "&ViewType=0";
}