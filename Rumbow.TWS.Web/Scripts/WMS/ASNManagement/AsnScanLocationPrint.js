var LODOP;
$(document).ready(function () {
    //$("[data-toggle='popover']").popover();
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
    $("#printlabelagain").live('click', function () {
        if ($("#AsnNumber").val() == "") {
            let _this = this;
            $("#Audio")[0].play();
            layer.confirm('ASN单号不能为空', {
                btn: ['确定'] //按钮
            }, function () {
                layer.closeAll();
                $("#AsnNumber").focus();
            });
            return;
        }
        if ($("#ScanBoxNumber").val() == "") {
            let _this = this;
            $("#Audio")[0].play();
            layer.confirm('箱号不能为空', {
                btn: ['确定'] //按钮
            }, function () {
                layer.closeAll();
                $("#ScanBoxNumber").focus();
                $("#ScanBoxNumber").select();
            });
            return;
        }
        $.ajax({
            url: "/WMS/ASNManagement/GetLocationLabelBySKUAll",
            type: "POST",
            dataType: "json",
            data: {
                AsnNumber: $("#AsnNumber").val().trim(),
                ScanBoxNumber: $("#ScanBoxNumber").val().trim()
            },
            success: function (data) {
                if (data.Code == "1") {
                    var tablediffs = $("#printLocationLabeltable");
                    $('#printLocationLabeltable tbody').html('');

                    for (var i = 0; i < data.data.length; i++) {
                        var tr = "<tr style='text-align: center'>" +
                            "<td> " + data.data[i].SKU + "</td>" +
                            "<td> " + data.data[i].Qty + "</td>" +
                            "<td> " + data.data[i].Location + "</td>" +
                            "<td> " + data.data[i].QtyReceived + "</td>" +
                            "<td> <label style='cursor: pointer;' class='btn btn-danger btn-sm' onclick=PrintLocationLabel('" + data.data[i].ASNNumber + "','" + data.data[i].BoxNumber + "','" + data.data[i].SKU + "','" + data.data[i].Location  + "')>补打一张</label></td>" +
                            "</tr > ";
                        tablediffs.append(tr);
                    }
                    layer.open({
                        type: 1,
                        skin: 'layui-layer-rim', //加上边框
                        area: ['515px', '600px'], //宽高
                        content: $("#printLocationLabel"),
                        end: function () {
                            $("#ScanSKU").select();
                        }
                    });
                }

            }
        });


    })

    //检查差异
    $("#CheckDiff").live('click', function () {
        checkDiff();
    });
    $("#ProductLevelCode").live('change', function () {
        document.getElementById('ScanSKU').focus();
        document.getElementById('ScanSKU').select();
    })
    $('#AsnNumber').live("keydown", function (e) {
        if (e.keyCode == 13) {
            document.getElementById('ScanBoxNumber').focus();
            document.getElementById('ScanBoxNumber').select();
        }
    })
    $('#ScanBoxNumber').live("keydown", function (e) {
        if (e.keyCode == 13) {
            if ($("#AsnNumber").val() == "") {
                let _this = this;
                $("#Audio")[0].play();
                layer.confirm('ASN单号不能为空', {
                    btn: ['确定'] //按钮
                }, function () {
                    layer.closeAll();
                    $("#AsnNumber").focus();
                });
                return;
            }
            if ($("#ScanBoxNumber").val() == "") {
                let _this = this;
                $("#Audio")[0].play();
                layer.confirm('箱号不能为空', {
                    btn: ['确定'] //按钮
                }, function () {
                    layer.closeAll();
                        $("#ScanBoxNumber").focus();
                        $("#ScanBoxNumber").select();
                });
                return;
            }
            if (CheckBoxNumber())
            {
               
                $("#SuccessAudio")[0].play();
                $("#ScanSKU").focus();
                $("#ScanSKU").select();
            }
            else
            {
                $("#Audio")[0].play();
                layer.confirm('箱号不正确', {
                    btn: ['确定'] //按钮
                }, function () {
                    layer.closeAll();
                    $("#ScanBoxNumber").focus();
                    $("#ScanBoxNumber").select();
                });
            }
            
        }
    });

    $('#ScanSKU').live("keydown", function (e) {
        if (e.keyCode == 13) {
            if ($("#AsnNumber").val() == "") {
                let _this = this;
                $("#Audio")[0].play();
                layer.confirm('ASN单号不能为空', {
                    btn: ['确定'] //按钮
                }, function () {
                    layer.closeAll();
                        $("#AsnNumber").focus();
                });
                return;
            }
            if ($("#ScanBoxNumber").val() == "") {
                let _this = this;
                $("#Audio")[0].play();
                layer.confirm('箱号不能为空', {
                    btn: ['确定'] //按钮
                }, function () {
                    layer.closeAll();
                        $("#ScanBoxNumber").focus();
                        $("#ScanBoxNumber").select();
                });
                return;
            }
            if ($("#ScanSKU").val() == "") {
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
            if (CheckSKU()) {
                $.ajax({
                    url: "/WMS/ASNManagement/GetLocationLabelBySKU",
                    type: "POST",
                    dataType: "json",
                    data: {
                        AsnNumber: $("#AsnNumber").val().trim(),
                        ScanBoxNumber: $("#ScanBoxNumber").val().trim(),
                        ScanSKU: "00" + $("#ScanSKU").val().trim()
                    },
                    success: function (data) {
                        if (data.Code == "1") {
                          
                            $("#SuccessAudio")[0].play();
                            var location = data.data[0].Location;
                            var locationArrary = location.split('-');
                            //$("#passageway")[0].innerText = data.data[0].GoodsType+'|'+locationArrary[0];
                            //if (locationArrary.length > 1) {
                            //    $("#goodsshelves")[0].innerText = locationArrary[1];
                            //}
                            //if (locationArrary.length > 2) {
                            //    $("#floor")[0].innerText = locationArrary[2];
                            //}
                            //if (locationArrary.length > 1) {
                            //    $("#goodsshelves")[0].innerText = locationArrary[0] + "-" + locationArrary[1];
                            //}
                            //if (locationArrary.length > 2) {
                            //    $("#floor")[0].innerText = data.data[0].GoodsType + '|' + locationArrary[2];
                            //}
                            $("#externnumber")[0].innerText = data.data[0].ExternReceiptNumber
                            $("#location")[0].innerText = location;
                            $("#boxbarcode")[0].innerText = data.data[0].str2
                            //try {
                            //    LODOP = getLodop();
                            //    LODOP.PRINT_INIT("");
                            //    LODOP.SET_PRINT_MODE("PRINT_END_PAGE", 1);
                            //    LODOP.ADD_PRINT_HTML(0, 0, 329, 204, $("#printdiv").html());
                            //    LODOP.PRINT();

                            //    //LODOP.PREVIEW();
                            //} catch (err) {
                            //    console.log(JSON.stringify(err));
                            //}
                            PrintJartools();
                            $("#ScanSKU").focus();
                            document.getElementById("ScanSKU").select();
                        }
                        else if (data.Code == "0") {
                            $("#Audio")[0].play();
                            layer.alert("该SKU库位标签已全部打印完成", {
                                skin: 'layui-layer-lan' //样式类名
                                , icon: 2
                                , closeBtn: 0
                                , btn: ['确定'] //单击按钮
                                , btn1: function (index, layero) {
                                    layer.close(index);
                                    $("#ScanSKU").focus();
                                    document.getElementById("ScanSKU").select();
                                },
                                success: function (layero) {
                                    var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                                    btn.href = 'javascript:void(0)';
                                    btn.focus();
                                }
                            })
                        }
                        else if (data.Code == "-1") {
                            $("#Audio")[0].play();
                            layer.alert(data.data, {
                                skin: 'layui-layer-lan' //样式类名
                                , icon: 2
                                , closeBtn: 0
                                , btn: ['确定'] //单击按钮
                                , btn1: function (index, layero) {
                                    layer.close(index);
                                    $("#ScanSKU").focus();
                                    document.getElementById("ScanSKU").select();
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
            else {
                layer.confirm('条码有误！', {
                    btn: ['确定'] //按钮
                }, function () {
                    layer.closeAll();
                        $("#ScanSKU").focus();
                        document.getElementById("ScanSKU").select();
                });
            } 
        }
        
     
    })
    $('.AsnNumbercheck').live('dblclick', function () {
        var self = this;
        openPopup("Prepop", true, 1000, 600, '/WMS/ASNManagement/PopupIndex', null, function (AsnNumber) {
          
            $.ajax({
                url: "/WMS/ASNManagement/GetLocationLabelByASNNumber",
                type: "POST",
                dataType: "text",
                data: {
                    AsnNumber: AsnNumber.trim()
                },
                success: function (data) {
                    if (data == "0") {
                        $("#Audio")[0].play();
                        layer.alert('库位标签未生成', {
                            skin: 'layui-layer-lan' //样式类名
                            , icon: 2
                            , closeBtn: 0
                            , btn: ['确定'] //单击按钮
                            , btn1: function (index, layero) {
                                layer.close(index);
                                $("#AsnNumber").focus();
                                document.getElementById("AsnNumber").select();
                            },
                            success: function (layero) {
                                var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                                btn.href = 'javascript:void(0)';
                                btn.focus();
                            }
                        })

                    }
                    else if (data == "-1") {
                        $("#Audio")[0].play();
                        layer.alert('数据加载失败', {
                            skin: 'layui-layer-lan' //样式类名
                            , icon: 2
                            , closeBtn: 0
                            , btn: ['确定'] //单击按钮
                            , btn1: function (index, layero) {
                                layer.close(index);
                                $("#AsnNumber").focus();
                                document.getElementById("AsnNumber").select();
                            },
                            success: function (layero) {
                                var btn = layero[0].getElementsByClassName('layui-layer-btn')[0].getElementsByTagName('A')[0];
                                btn.href = 'javascript:void(0)';
                                btn.focus();
                            }
                        })
                    }
                    else {
                        $('#AsnNumber').val(AsnNumber.trim());
                        document.getElementById('ScanBoxNumber').focus();
                        document.getElementById('ScanBoxNumber').select();
                    }
                }
            });



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
function BarcodeC(obj, data) {
    $(obj).empty().barcode(data, "code128", { barWidth: 1, barHeight: 15, showHRI: false });
}
$(function () {
    $.each($(".bcTarget"), function (a, b) {
        BarcodeC(b, $(b).html());
    });
})
function Print() {
    $("#printdiv").jqprint({
        debug: false,
        importCSS: true,
        printContainer: true,
        operaSupport: true
    });
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


function PrintLocationLabel(ASNNumber, BoxNumber, SKU,Location)
{
    $.ajax({
        url: "/WMS/ASNManagement/GetLocationLabelBySKUAndLocation",
        type: "POST",
        dataType: "json",
        data: {
            AsnNumber: ASNNumber,
            ScanBoxNumber: BoxNumber,
            ScanSKU: SKU,
            Location: Location
        },
        success: function (data) {
            if (data.Code == "1") {
                $("#SuccessAudio")[0].play();
                var location = data.data[0].Location;
                var locationArrary = location.split('-');
                //$("#passageway")[0].innerText = data.data[0].GoodsType + '|' + locationArrary[0];
                //if (locationArrary.length > 1) {
                //    $("#goodsshelves")[0].innerText = locationArrary[0]+"-"+ locationArrary[1];
                //}
                //if (locationArrary.length > 2) {
                //    $("#floor")[0].innerText = data.data[0].GoodsType + '|' +locationArrary[2];
                //}
                $("#externnumber")[0].innerText = data.data[0].ExternReceiptNumber
                $("#location")[0].innerText = location;
                $("#boxbarcode")[0].innerText = data.data[0].str2
                $.each($(".bcTarget"), function (a, b) {
                    BarcodeC(b, $(b).html());
                });
                //try {
                //    LODOP = getLodop();
                //    LODOP.PRINT_INIT("");
                //    LODOP.SET_PRINT_MODE("PRINT_END_PAGE", 1);
                //    LODOP.ADD_PRINT_HTML(0, 0, 329, 204, $("#printdiv").html());
                //    LODOP.PRINT();

                //    //LODOP.PREVIEW();
                //} catch (err) {
                //    console.log(JSON.stringify(err));
                //}
                PrintJartools();

            }
            else
            {
                $("#Audio")[0].play();
                layer.alert("打印失败", {
                    skin: 'layui-layer-lan' //样式类名
                    , icon: 2
                    , closeBtn: 0
                    , btn: ['确定'] //单击按钮
                    , btn1: function (index, layero) {
                        layer.close(index);
                        $("#ScanSKU").focus();
                        document.getElementById("ScanSKU").select();
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
            }
        }
    });
}

function CheckBoxNumber()
{
    var results = false;
    $.ajax({
        url: "/WMS/ASNManagement/CheckLocationLabelByBoxNumber",
        type: "POST",
        dataType: "text",
        async: false,
        data: {
            AsnNumber: $("#AsnNumber").val(),
            ScanBoxNumber: $("#ScanBoxNumber").val()//箱号
        },
        success: function (data) {
            if (data == "1") {
                results = true;
               
            }
        }
    });
    return results;
}
function CheckSKU() {
    var result = false;
    $.ajax({
        url: "/WMS/ASNManagement/CheckLocationLabelBySKU",
        type: "POST",
        dataType: "text",
        async: false,
        data: {
            AsnNumber: $("#AsnNumber").val(),
            ScanBoxNumber: $("#ScanBoxNumber").val(),//箱号
            SKU: "00"+$("#ScanSKU").val()
        },
        success: function (data) {
            if (data == "1") {
                result = true;
            }
        }
    });
    return result;
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

