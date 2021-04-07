$(document).ready(function () {
    //查看差异
    $("#ShowDiff").live('click', function () {
        if ($("#AsnNumber").val() == "") {
            //layer.msg("请选择ASN单号");           
            $("#Audio")[0].play();
            layer.confirm('请选择ASN单号！', {
                btn: ['确定'] //按钮
            }, function () {
                layer.closeAll();
            });
            return;
        }
        openPopup("Prepop", true, 1000, 600, '/WMS/ASNManagement/ShowDiff?AsnNumber=' + $("#AsnNumber").val(), null, null);
        $("#popupLayer_Prepop")[0].style.top = "50px";

    });

    $("#PrintBoxLabel").live('click', function () {
        PrintBoxLabel();
        
    })

    $("#ScanClear").live('click', function () {
        layer.confirm('<font size="4">确认是否删除重扫？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
                layer.close(index);
                $.ajax({
                    url: "/WMS/ASNManagement/ClearAsnBoxNumber",
                    type: "POST",
                    dataType: "text",
                    data: {
                        AsnNumber: $("#AsnNumber").val(),
                        ScanBoxNumber: $("#ScanBoxNumber").val()//箱号
                    },
                    success: function (data) {
                        if (data == "") {
                            layer.closeAll();
                            layer.alert('删除成功,请重扫', {
                                skin: 'layui-layer-lan' //样式类名
                                , icon: 1
                                , closeBtn: 0
                                , btn: ['确定'] //单击按钮
                                , btn1: function (index, layero) {
                                    layer.closeAll(index);
                                    var flagScanBoxNumber = 0;
                                    var tables = $("#Newtable")[0]

                                    for (var i = 0; i < tables.rows.length; i++) {
                                        if ($('#ScanBoxNumber').val() == tables.rows[i].cells[0].innerText) {
                                            flagScanBoxNumber = 1;
                                            tables.rows[i].cells[4].innerText = 0;
                                        }
                                        
                                    }
                                    for (var j = tables.rows.length-1; j > 0; j--)
                                    {
                                        if ($('#ScanBoxNumber').val() == tables.rows[j].cells[0].innerText && tables.rows[j].cells[3].innerText == "0") {
                                            $('#Newtable tr').eq(j).remove();
                                            
                                        }
                                    }
                                   
                                    if (flagScanBoxNumber == 0) {
                                        $("#Audio")[0].play();
                                        $(this).blur();
                                        let _this = this;
                                        layer.confirm('箱号不存在', {
                                            btn: ['确定'] //按钮
                                        }, function () {
                                            layer.closeAll();
                                            //$("#ScanBoxNumber").select();
                                            //$("#ScanBoxNumber").focus();
                                            $(_this).select();
                                            $(_this).focus();
                                        });
                                        return;
                                    }
                                    $('#ScanSKU').val('');
                                    $("#box")[0].innerText = "";
                                    $("#sku")[0].innerText = "";
                                    $("#expectedQty")[0].innerText = "";
                                    $("#receivedQty")[0].innerText = "";
                                    document.getElementById('ScanSKU').focus();
                                },
                                success: function (layero) {
                                    var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                                    btn.href = 'javascript:void(0)';
                                    btn.focus();
                                }
                            })
                        }
                        else
                        {
                            layer.alert(data, {
                                skin: 'layui-layer-lan' //样式类名
                                , icon: 2
                                , closeBtn: 0
                                , btn: ['确定'] //单击按钮
                                , btn1: function (index, layero) {
                                    layer.close(index);
                                    $("#ScanBoxNumber").focus();
                                    document.getElementById("ScanBoxNumber").select();
                                },
                                success: function (layero) {
                                    var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                                    btn.href = 'javascript:void(0)';
                                    btn.focus();
                                }
                            })
                        }
                    }
                });
        })

    })

    //检查差异
    $("#CheckDiff").live('click', function () {
        if ($("#TempCustomerID").val() == "103") {
            checkDiffReturn();
        }
        else {
            checkDiff();
        }
        
    });
    $('#AsnNumber').live("keydown", function (e) {
        if (e.keyCode == 13) {
            document.getElementById('ScanBoxNumber').focus();
        }
    })
    $('#ScanBoxNumber').live("keydown", function (e) {
        if (e.keyCode == 13) {
            if ($("#ScanBoxNumber").val() == "") {
                $("#ScanBoxNumber").blur();
                let _this = this;
                $("#Audio")[0].play();
                layer.confirm('扫描的箱号不能为空！', {
                    btn: ['确定'] //按钮
                }, function () {
                    layer.closeAll();
                    $(_this).focus();
                });
                return;
            };
            var flagScanBoxNumber = 0;
            var tables = $("#Newtable")[0]
            for (var i = 0; i < tables.rows.length; i++) {
                if ($('#ScanBoxNumber').val() == tables.rows[i].cells[0].innerText) {
                    flagScanBoxNumber = 1;
                }
            }
            if (flagScanBoxNumber == 0) {
                $("#Audio")[0].play();
                $(this).blur();
                let _this = this;
                layer.confirm('箱号不存在', {
                    btn: ['确定'] //按钮
                }, function () {
                    layer.closeAll();
                    //$("#ScanBoxNumber").select();
                    //$("#ScanBoxNumber").focus();
                    $(_this).select();
                    $(_this).focus();
                });
                return;
            }
            $('#ScanSKU').val('');
            $("#box")[0].innerText = "";
            $("#sku")[0].innerText = "";
            $("#expectedQty")[0].innerText = "";
            $("#receivedQty")[0].innerText = "";
            document.getElementById('ScanSKU').focus();
        }
    })
    $('#ScanSKU').live("keydown", function (e) {
        if (e.keyCode == 13) {            
            if ($("#ScanSKU").val() == "")
            {
                $("#ScanSKU").blur();
                let _this = this;
                $("#Audio")[0].play();
                layer.confirm('扫描的SKU不能为空！', {
                    btn: ['确定'] //按钮
                }, function () {
                    layer.closeAll();
                    $(_this).focus();
                });
                return;
            }
            if ($("#ScanSKU").val() == "123456") {
                if ($("#TempCustomerID").val() == "103") {
                    checkDiffReturn();
                }
                else {
                    checkDiff();
                }
                return;
            }

            var tables = $("#Newtable tbody tr")
            var flagScanSKU = 0;
            $('#ScanSKU').val("00" + $('#ScanSKU').val());
            for (var i = 0; i < tables.length; i++) {
                if (tables[i].cells[0].innerText == $('#ScanBoxNumber').val()
                    && tables[i].cells[1].innerText == $("#ScanSKU").val()) {
                    if ($('#sku')[0].innerText != $('#ScanSKU').val()) {
                        $('#receivedQty')[0].innerText = "";
                        $('#receivedQty')[0].color = "red";
                    }
                    if (parseInt(tables[i].cells[3].innerText) != 0) {
                        if (parseInt(tables[i].cells[4].innerText) >= parseInt(tables[i].cells[3].innerText)) {
                            //layer.msg("入库数量不能超过期望数量
                            $("#Audio")[0].play();
                            $("#ScanSKU").blur();                            
                            layer.confirm('入库数量不能超过期望数量', {
                                btn: ['确定'] //按钮
                            }, function () {
                                layer.closeAll();
                                $("#ScanSKU").select();
                                $("#ScanSKU").focus();
                                //$('#ScanSKU').val('')
                            });

                            return;
                        }
                    }

                }
                //if (tables[i].cells[1].innerText == $("#ScanSKU").val() && tables[i].cells[0].innerText == $("#ScanBoxNumber").val()) {
                //    flagScanSKU = 1;
                //}
                if (tables[i].cells[1].innerText == $("#ScanSKU").val()) {
                    flagScanSKU = 1;
                }
            }
            if (flagScanSKU == 0) {
                $("#Audio")[0].play();
                $(this).blur();
                let _this = this;
                layer.confirm('扫描有误,SKU不存在', {
                    btn: ['确定'] //按钮
                }, function () {
                    layer.closeAll();
                    //$('#ScanSKU').val('')
                    $(_this).select();
                    $(_this).focus();
                });
                //layer.msg("扫描有误,SKU不存在");
                //$('#ScanSKU').val('')
                return;
            }
            $.ajax({
                url: "/WMS/ASNManagement/AsnScanQtyUpdate",
                type: "POST",
                async: false,
                dataType: "text",
                data: {
                    AsnNumber: $("#AsnNumber").val(),
                    str2: $("#ScanBoxNumber").val(),//箱号
                    SKU: $("#ScanSKU").val()
                },
                success: function (data) {                     
                    if (data == "") {
                        var flagAsnScanQtyUpdate = 0;
                        for (var i = 0; i < tables.length; i++) {
                            if (tables[i].cells[0].innerText == $('#ScanBoxNumber').val()
                                && tables[i].cells[1].innerText == $("#ScanSKU").val()) {
                                flagAsnScanQtyUpdate = 1;
                            }

                            //正常扫描
                            if (tables[i].cells[0].innerText == $('#ScanBoxNumber').val()
                                && tables[i].cells[1].innerText == $("#ScanSKU").val()
                                && parseInt(tables[i].cells[3].innerText) != 0) {
                                $('#box')[0].innerText = tables[i].cells[0].innerText;
                                $('#sku')[0].innerText = tables[i].cells[1].innerText;
                                $('#expectedQty')[0].innerText = tables[i].cells[3].innerText;
                                $('#receivedQty')[0].innerText = $('#receivedQty')[0].innerText == "" ?
                                    parseInt(tables[i].cells[4].innerText) + 1 : parseInt($('#receivedQty')[0].innerText) + 1;
                                if ($('#receivedQty')[0].innerText == parseInt(tables[i].cells[3].innerText)) {
                                    $('#receivedQty')[0].color = "blue";
                                }
                                tables[i].cells[4].innerText = parseInt(tables[i].cells[4].innerText) + 1;
                            }
                            //串箱第二次扫描
                            if (tables[i].cells[0].innerText == $('#ScanBoxNumber').val()
                                && tables[i].cells[1].innerText == $("#ScanSKU").val()
                                && parseInt(tables[i].cells[3].innerText) == 0) {
                                $('#box')[0].innerText = tables[i].cells[0].innerText;
                                $('#sku')[0].innerText = tables[i].cells[1].innerText;
                                $('#expectedQty')[0].innerText = 0;
                                $('#receivedQty')[0].innerText = $('#receivedQty')[0].innerText == "" ?
                                    parseInt(tables[i].cells[4].innerText) + 1 : parseInt($('#receivedQty')[0].innerText) + 1;
                                tables[i].cells[4].innerText = parseInt(tables[i].cells[4].innerText) + 1;
                            }
                        }
                        if (flagAsnScanQtyUpdate == 0) {
                            var myTable = document.getElementById("Newtable");
                            var rowIndex = i;//当前行的行号
                            //var obj = document.getElementById('Newtable').getElementsByTagName('tr');
                            //var newrow = obj[rowIndex].cloneNode(true);
                            var row = $("<tr style='background-color:gray'></tr>");
                            var td0 = $("<td>" + $('#ScanBoxNumber').val() + "</td>");
                            var td1 = $("<td>" + $("#ScanSKU").val() + "</td>");
                            var td2 = $("<td>" + $("#ScanSKU").val() + "</td>");
                            var td3 = $("<td>0</td>");
                            var td4 = $("<td>1</td>");
                            row.append(td0);
                            row.append(td1);
                            row.append(td2);
                            row.append(td3);
                            row.append(td4);
                            $(tables[tables.length - 1]).after(row);
                            $('#box')[0].innerText = $('#ScanBoxNumber').val();
                            $('#sku')[0].innerText = $("#ScanSKU").val();
                            $('#expectedQty')[0].innerText = 0;
                            $('#receivedQty')[0].innerText = 1;
                        }                        
                        $('#ScanSKU').val('')                       
                    } else {                         
                         //layer.msg(data, { time: 5 * 1000 });
                        //$('#ScanSKU').val('')
                        $("#ScanSKU").blur();            
                        $("#Audio")[0].play();
                        layer.confirm('错误：' + data, {
                            btn: ['确定'] //按钮
                        }, function () {
                            layer.closeAll();
                            $("#ScanSKU").select();
                            $("#ScanSKU").focus();
                        });
                       
                    }
                }
            });
        }
    })
    $('.AsnNumbercheck').live('dblclick', function () {
        var self = this;
        openPopup("Prepop", true, 1000, 600, '/WMS/ASNManagement/PopupIndex', null, function (AsnNumber) {
            window.location.href = "/WMS/ASNManagement/AsnScanIndex/?AsnNumber=" + AsnNumber
            $('#AsnNumber').val(AsnNumber);
        });
        $("#popupLayer_Prepop")[0].style.top = "50px";
    });
    $("#ExportDiff").live('click', function () {
        var AsnNumber = $("#AsnNumber").val();
        var form = $("<form>");
        form.attr('style', 'display:none');
        form.attr('target', '');
        form.attr('method', 'post}');
        form.attr('action', '/WMS/ASNManagement/ExportDiff');//'/WMS/PreOrder/ReportDirect'
        var input1 = $('<input>');
        input1.attr('type', 'hidden');
        input1.attr('name', 'demo');
        input1.attr('value', 'Export');
        var input2 = $('<input>');
        input2.attr('type', 'hidden');
        input2.attr('name', 'fileId');
        input2.attr('value', "fileId");
        var input3 = $('<input id="AsnNumber" name="AsnNumber" type="hidden" value="' + AsnNumber + '" />');
        $('body').append(form);
        form.append(input1);
        form.append(input2);
        form.append(input3);
        form.submit();
        form.remove();
    });
})


function PrintBoxLabel() {
    $.ajax({
        url: "/WMS/ASNManagement/GetAsnScanBoxSum",
        type: "POST",
        dataType: "json",
        data: {
            AsnNumber: $("#AsnNumber").val(),
            ScanBoxNumber: $("#ScanBoxNumber").val()//箱号
        },
        success: function (data) {
            if (data.Code == "1") {
                $("#externnumber")[0].innerText = data.data[0].ExternReceiptNumber;
                $("#boxbarcode")[0].innerText = data.data[0].str3;
                $("#skucount")[0].innerText = data.data[0].QtyReceived;
                $.each($(".bcTarget"), function (a, b) {
                    BarcodeC(b, $(b).html());
                });
                $("#page1")[0].style.display = "";
                PrintJartools();
                setTimeout(function () { $("#page1")[0].style.display = "none"; }, 1000)
            }
            else if (data.Code == "0") {
                layer.alert('未查询到数据，打印失败', {
                    skin: 'layui-layer-lan' //样式类名
                    , icon: 2
                    , closeBtn: 0
                    , btn: ['确定'] //单击按钮
                    , btn1: function (index, layero) {
                        layer.close(index);
                    },
                    success: function (layero) {
                        var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                        btn.href = 'javascript:void(0)';
                        btn.focus();
                    }
                })
            }
            else {
                layer.alert('打印失败', {
                    skin: 'layui-layer-lan' //样式类名
                    , icon: 2
                    , closeBtn: 0
                    , btn: ['确定'] //单击按钮
                    , btn1: function (index, layero) {
                        layer.close(index);
                    },
                    success: function (layero) {
                        var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                        btn.href = 'javascript:void(0)';
                        btn.focus();
                    }
                })
            }
        }
    });
}
function BarcodeC(obj, data) {
    $(obj).empty().barcode(data, "code128", { barWidth: 1, barHeight: 35, showHRI: false });
}

//打印
function PrintJartools() {
    doPrint("打印")
}

function doPrint(how) {
    //打印文档对象
    var myDoc = {
        settings: { topMargin: 50, leftMargin: 50, bottomMargin: 50, rightMargin: 50 },
        documents: document,    // 打印页面(div)们在本文档中
        // 打印时,only_for_print取值为显示
        classesReplacedWhenPrint: new Array('.only_for_print{display:block}'),
        copyrights: '杰创软件拥有版权  www.jatools.com'         // 版权声明必须
    };
    var jatoolsPrinter = getJatoolsPrinter();
    // 调用打印方法
    if (how == '打印预览...')
        jatoolsPrinter.printPreview(myDoc);   // 打印预览

    else if (how == '打印...')
        jatoolsPrinter.print(myDoc, true);   // 打印前弹出打印设置对话框

    else
        jatoolsPrinter.print(myDoc, false);       // 不弹出对话框打印
}

function demo(url) {
    // $.send('/WMS/Product/demoExecl');
    // 绑定导出按钮

    var form = $("<form>");
    form.attr('style', 'display:none');
    form.attr('target', '');
    form.attr('method', 'post');
    form.attr('action', url);//'/WMS/PreOrder/ReportDirect'
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
function checkDiffReturn()
{
    if ($("#AsnNumber").val() == "") {
        //layer.msg("请选择ASN单号");
        $("#Audio")[0].play();
        layer.confirm('请选择ASN单号！', {
            btn: ['确定'] //按钮
        }, function () {
            layer.closeAll();
        });
        return;
    }
    if ($("#ScanBoxNumber").val() == "") {
        //layer.msg("请扫描箱号");
        $("#Audio")[0].play();
        layer.confirm('请扫描箱号', {
            btn: ['确定'] //按钮
        }, function () {
            layer.closeAll();
        });

        return;
    }
    var flagCheckDiff = 0;
    var tables = $("#Newtable")[0]
    for (var i = 0; i < tables.rows.length; i++) {
        if ($('#ScanBoxNumber').val() == tables.rows[i].cells[0].innerText) {
            flagCheckDiff = 1;
        }
    }
    if (flagCheckDiff == 0) {
        $("#Audio")[0].play();
        layer.confirm('箱号不存在', {
            btn: ['确定'] //按钮
        }, function () {
            layer.closeAll();
            //$("#ScanBoxNumber").val('');              
            //$("#ScanBoxNumber").focus();
            $("#ScanBoxNumber").select();
        });

        return;
    }
    $.ajax({
        url: "/WMS/ASNManagement/CheckDiffReturn",
        type: "POST",
        dataType: "json",
        data: {
            AsnNumber: $("#AsnNumber").val(),
            ScanBoxNumber: $("#ScanBoxNumber").val()//箱号
        },
        success: function (data) {
            if (data.Code == "0") {
                $("#SuccessAudio")[0].play();
                layer.confirm('该箱无差异，请扫下一箱', {
                    btn: ['确定'] //按钮
                    ,closeBtn: 0
                    , btn1: function (index, layero) {
                        layer.closeAll();                      
                        PrintBoxLabel();
                        $("#ScanBoxNumber").val('');
                        $("#ScanBoxNumber").focus();
                        $("#ScanSKU").val('');
                    },
                    success: function (layero) {
                        var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                        btn.href = 'javascript:void(0)';
                        btn.focus();
                    }
                });


            }
            else if (data.Code == "1") {
                $("#Audio")[0].play();
                var html = $("#CheckRFDifference").render(data.data);
                //页面层
                layer.open({
                    type: 1,
                    title: '本箱差异明细',
                    skin: 'layui-layer-rim', //加上边框
                    area: ['800px', '600px'], //宽高
                    content: showmsgRF(html)
                });
            }
            else {
                $("#Audio")[0].play();
                layer.alert('异常错误', {
                    skin: 'layui-layer-lan' //样式类名
                    , icon: 2
                    , closeBtn: 0
                    , btn: ['确定'] //单击按钮
                    , btn1: function (index, layero) {
                        layer.close(index);
                        $("#ScanBoxNumber").focus();
                        $("#ScanBoxNumber").select();
                    },
                    success: function (layero) {
                        var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                        btn.href = 'javascript:void(0)';
                        btn.focus();
                    }
                })
            }
        }
    });
}
function checkDiff() {
    if ($("#AsnNumber").val() == "") {
        //layer.msg("请选择ASN单号");
        $("#Audio")[0].play();
        layer.confirm('请选择ASN单号！', {
            btn: ['确定'] //按钮
        }, function () {
            layer.closeAll();
        });
        return;
    }
    if ($("#ScanBoxNumber").val() == "") {
        //layer.msg("请扫描箱号");
        $("#Audio")[0].play();
        layer.confirm('请扫描箱号', {
            btn: ['确定'] //按钮
        }, function () {
            layer.closeAll();
        });

        return;
    }
    var flagCheckDiff = 0;
    var tables = $("#Newtable")[0]
    for (var i = 0; i < tables.rows.length; i++) {
        if ($('#ScanBoxNumber').val() == tables.rows[i].cells[0].innerText) {
            flagCheckDiff = 1;
        }
    }
    if (flagCheckDiff == 0) {
        $("#Audio")[0].play();
        layer.confirm('箱号不存在', {
            btn: ['确定'] //按钮
        }, function () {
            layer.closeAll();
            //$("#ScanBoxNumber").val('');              
            //$("#ScanBoxNumber").focus();
            $("#ScanBoxNumber").select();
        });

        return;
    }
    $.ajax({
        url: "/WMS/ASNManagement/CheckDiff",
        type: "POST",
        dataType: "text",
        data: {
            AsnNumber: $("#AsnNumber").val(),
            ScanBoxNumber: $("#ScanBoxNumber").val()//箱号
        },
        success: function (data) {
            if (data == "") {                
                $("#SuccessAudio")[0].play();
                layer.confirm('该箱无差异，请扫下一箱', {
                    btn: ['确定'] //按钮
                    , btn1: function (index, layero) {
                        layer.closeAll();
                        $("#ScanBoxNumber").val('');

                        $("#ScanBoxNumber").focus();
                        $("#ScanSKU").val('');
                    },
                    success: function (layero) {
                        var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                        btn.href = 'javascript:void(0)';
                        btn.focus();
                    }
                });
                //}, function () {
                //    layer.closeAll();
                //    $("#ScanBoxNumber").val('');
                //    //layer.alert("该箱无差异，请扫下一箱");
                //    $("#ScanBoxNumber").focus();
                //});

            }
            else {
                $("#Audio")[0].play();
                layer.alert('本箱有差异', {
                    skin: 'layui-layer-lan' //样式类名
                    , icon: 2
                    , closeBtn: 0
                    , btn: ['确定'] //单击按钮
                    , btn1: function (index, layero) {
                        layer.close(index);
                        $("#ScanBoxNumber").focus();
                        $("#ScanBoxNumber").select();
                    },
                    success: function (layero) {
                        var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                        btn.href = 'javascript:void(0)';
                        btn.focus();
                    }
                })
            }
        }
    });
}
//显示结果
function showmsgRF(html) {
    return dataTable = ' <table>\
        <thead>\
            <tr>\
                <th>箱号</th>\
                <th>SKU</th>\
                <th>差异数量</th>\
            </tr>\
        </thead>\
        <tbody id="CheckRFDifferencePopup">' + html + '</tbody> </table>' +
        ' <div style="text-align:center; margin-top: 10px; margin-right: 10px">'+
            '<input id = "ScanClear" style = "width:100px;margin-left:10px" type = "button" value = "本箱重扫" class="form-control btn-danger" />'+
                '<input id="PrintBoxLabel" style="width:100px;margin-left:10px" type="button" value="打印箱唛" class="form-control btn-primary" />'+
    '</div >'
}

function deleteSKU(ID, obj) {
    layer.confirm('<font size="4">确认是否删除？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        var tr = $(obj).parent().parent().parent();
        $.send(
            '/InteWareCD/Product/DelProduct',
            {
                ID: ID
            },
            function (response) {
                if (response.IsSuccess) {
                    tr.remove();

                }
                layer.msg(response.Message);

            },
            function () {

                layer.msg("删除失败，请联系IT");
            });
    });

};

