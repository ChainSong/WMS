var index;
var OrderID;
var boxAttr;//箱型
$(document).ready(function () {

    //if ($('#CustomerIDs').val() == "") {
    //    $('#CustomerIDs option:first').next().attr("selected", "selected");

    //} else {

    //}

    initialization();
    $('#CustomerIDs').keydown(function (event) {
        if (event.which == 13) {
            if ($('#CustomerIDs').val().trim().length > 0 && $('#CustomerIDs').val().trim() != '==请选择==') {
                document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:green;">请选择仓库！</h1>';
                $('#CustomerIDs').attr('readOnly', 'true');
                $('#CustomerIDs').attr("disabled", "disabled")//不让改
                $('#Warehouses').removeAttr('readOnly');
                $('#Warehouses').focus();
                index = 1;
            } else {
                document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">请选择客户！</h1>';
            }
        }
    });
    $('#Warehouses').keydown(function (event) {
        if (event.which == 13) {
            if ($('#Warehouses').val().trim().length > 0 && $('#Warehouses').val().trim() != '==请选择==') {
                document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:green;">请扫快递单号！</h1>';
                $('#Warehouses').attr('readOnly', 'true');
                $('#Warehouses').attr("disabled", "disabled")//不让改
                $('#PackageCollection_Scan').removeAttr('readOnly');
                $('#PackageCollection_Scan').focus();
                index = 2;
            } else {
                document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">请选择仓库！</h1>';
            }
        }
    });
    $("#PackageCollection_Scan").keydown(function (event) {
        if (event.which == 13) {
            if (index == 2) {
                //上一步是选择仓库，这步扫描的应是 快递单号
                checkExpressNumber();
            } else if (index == 3) {
                //上一步是扫描快递单号，这步扫描的应是 订单号
                checkOrderNumber();
            } else if (index == 4) {
                //上一步是扫描订单号，这步扫描的应是 SKU
                checkSKU();
            } else if (index == 6) {
                //上一步是扫描SKU，这步扫描的应是 SKU,直到数量满足
                checkSKU();
            } else if (index == 9) {
                //上一步是扫描SKU，这步扫描的应是 SKU,直到数量满足
                checkExpressNumber();
            } else {
                //上一步是其他操作
            }
        } else {
            //按其他按键
        }
    });
    $("#engineval").keydown(function (event) {
        if (event.which == 13) {
            if ($(this).val().trim().length > 0) {
                var selec = $(this).val(); //获取改变的选项值
                $.ajax({
                    type: "POST",
                    url: "/WMS/OrderECManagement/getSupplieTypeListJSON",
                    data: {
                        "PackageType": selec,
                    },
                    async: "false",
                    success: function (data) {
                        if (data == "") {
                            document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">耗材类型不存在！</h1>';
                            $("#engineval").select();
                            //$("#engineval").blur();
                        } else {
                            var obj = JSON.parse(data);

                            var str = obj[0].Code;
                            boxAttr = str;//将后台传来的箱型存起来
                            var array = str.split(',');
                            $("#PackageCollection_Length").html(array[0] / 100);
                            $("#PackageCollection_Width").html(array[1] / 100);
                            $("#PackageCollection_Height").html(array[2] / 100);
                            var vo = (array[0] / 100) * (array[1] / 100) * (array[2] / 100);
                            $("#volume").html(vo.toFixed(5));

                            //$("#PackageCollection_Length").html(obj[0].Str1 / 100);
                            //$("#PackageCollection_Width").html(obj[0].Str2 / 100);
                            //$("#PackageCollection_Height").html(obj[0].Str3 / 100);
                            //var vo = (obj[0].Str1 / 100) * (obj[0].Str2 / 100) * (obj[0].Str3 / 100);
                            //$("#volume").html(vo.toFixed(5));
                            $("#engineval").attr("readOnly", "true");
                            document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:green;">请扫SKU！</h1>';
                            $("#PackageCollection_Scan").removeAttr("readOnly");
                            $("#PackageCollection_Scan").focus();
                        }
                    },
                    error: function (msg) {
                        document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">' + msg.val + '</h1>';
                        $("#engineval").select();
                        $("#engineval").blur();
                    }
                });
            } else {
                document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">耗材类型不能为空！</h1>';
                $("#engineval").focus();
            }
        } else if (event.which == 40 || event.which == 38) {
            $("#engineval").removeClass("form-control");
            $("#engineval").hide();
            $("#PackageCollection_PackageType").attr("class", "form-control");
            $("#PackageCollection_PackageType").show();
            $("#PackageCollection_PackageType").focus();
        } else {
        }
    });
    $('select[id=PackageCollection_PackageType]').keydown(function (event) {
        if ($(this).val().trim().length > 0) {
            if (event.which == 13) {
                var selec = $(this).val(); //获取改变的选项值
                $.ajax({
                    type: "POST",
                    url: "/WMS/OrderECManagement/getSupplieTypeListJSON",
                    data: {
                        "PackageType": selec,
                    },
                    async: "false",
                    success: function (data) {
                        var obj = JSON.parse(data);
                        var str = obj[0].Code;
                        var array = str.split(',');
                        $("#PackageCollection_Length").html(array[0] / 100);
                        $("#PackageCollection_Width").html(array[1] / 100);
                        $("#PackageCollection_Height").html(array[2] / 100);
                        var vo = (array[0] / 100) * (array[1] / 100) * (array[2] / 100);
                        $("#volume").html(vo.toFixed(5));

                        //$("#PackageCollection_Length").html(obj[0].Str1 / 100);
                        //$("#PackageCollection_Width").html(obj[0].Str2 / 100);
                        //$("#PackageCollection_Height").html(obj[0].Str3 / 100);
                        //var vo = (obj[0].Str1 / 100) * (obj[0].Str2 / 100) * (obj[0].Str3 / 100);
                        //$("#volume").html(vo.toFixed(5));
                        $("#engineval").attr("readOnly", "true");
                        document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:green;">请扫SKU！</h1>';
                        $("#PackageCollection_Scan").removeAttr("readOnly");
                        $("#PackageCollection_Scan").focus();
                    },
                    error: function (msg) {
                        document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">' + msg.val + '</h1>';
                        $("#engineval").blur();
                    }
                });
            }
            else { }
        }
        else {
            document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">耗材类型不能为空！</h1>';
            $("#engineval").blur();
        }
    });
    $("#PackageCollection_PackageType").keydown(function (event) {
        if (event.which == 13) {
            $("#PackageCollection_PackageType").removeClass("form-control");
            $("#PackageCollection_PackageType").hide();
            $("#engineval").attr("class", "form-control");
            $("#engineval").show();
            showinput2($("#PackageCollection_PackageType").val());
        }
    });

    $('#CloseButton').live('click', function () {
        if ($("#PackageCollection_ExpressNumber").val().trim().length > 0) {
            layer.confirm('当前信息未保存，是否继续？', {
                btn: ['继续', '返回'] //按钮
            }, function (index) {
                //layer.msg('的确很重要', { icon: 1 });
                layer.close(index);
                initialization();
                $("#PackageCollection_ExpressNumber").focus();
            }, function (index) {
                //layer.msg('也可以这样', {
                //    time: 20000, //20s后自动关闭
                //    btn: ['明白了', '知道了']
                //});
            });
        }
    });
});

function eachul(obj) {
    //if (obj.constructor == Array) {
    //    var str = "<table cellspacing='0' style='width:100%;'>";
    //    for (var i = 0, len = obj.length; i < len; i++) {
    //        str += "<tr id='" + obj[i].SKU + "'><td style='text-align:center;width:33.5%;'>" + eachul(obj[i].SKU) + "</td><td style='text-align:center;width:34.5%;'>" + eachul(obj[i].QTY) + "</td><td style='color:red;text-align:center;width:auto;' id='diff'>" + eachul(obj[i].Diff) + "</td></tr>";
    //    }
    //    str += "</table>";
    //    return str;
    //}
    //if (obj.constructor == Object) {
    //    var str = "<table cellspacing='0' style='width:100%;'>";
    //    for (var i in obj) {
    //        str += "<tr id='" + obj[i].SKU + "'><td style='text-align:center;width:31%;'>" + eachul(obj[i].SKU) + "</td><td style='text-align:center;width:32%;'>" + eachul(obj[i].QTY) + "</td><td style='color:red;text-align:center;width:auto;' id='diff'>" + eachul(obj[i].Diff) + "</td></tr>";
    //    }
    //    str += "</table>";
    //    return str;
    //}
    //return obj;

    if (obj.constructor == Array) {
        var str = "<table cellspacing='0' style='width:100%;'>";
        for (var i = 0, len = obj.length; i < len; i++) {
            str += "<tr><td name='SKU' style='text-align:center;width:33.5%;'>" + eachul(obj[i].SKU) + "</td><td name='QTY' style='text-align:center;width:34.5%;'>" + eachul(obj[i].QTY) + "</td><td name='Diff' style='color:red;text-align:center;width:auto;' id='diff'>" + eachul(obj[i].Diff) + "</td></tr>";

            //str += "<tr id='" + obj[i].SKU + "'><td style='text-align:center;width:33.5%;'>" + eachul(obj[i].SKU) + "</td><td style='text-align:center;width:34.5%;'>" + eachul(obj[i].QTY) + "</td><td style='color:red;text-align:center;width:auto;' id='diff'>" + eachul(obj[i].Diff) + "</td></tr>";
        }
        str += "</table>";
        return str;
    }
    if (obj.constructor == Object) {
        var str = "<table cellspacing='0' style='width:100%;'>";
        for (var i in obj) {
            str += "<tr><td  name='SKU' style='text-align:center;width:31%;'>" + eachul(obj[i].SKU) + "</td><td name='QTY' style='text-align:center;width:32%;'>" + eachul(obj[i].QTY) + "</td><td name='Diff' style='color:red;text-align:center;width:auto;' id='diff'>" + eachul(obj[i].Diff) + "</td></tr>";
        }
        str += "</table>";
        return str;
    }
    return obj;

}
function eachtr(obj) {
    if (obj.constructor == Array) {
        var str = "<table>";
        for (var i = 0, len = obj.length; i < len; i++) {
            str += "<tr><td><input type='checkbox' id='" + obj[i].OrderNumber + "' />" + eachtr(obj[i].OrderNumber) + "</td></tr>";
        }
        str += "</table>";
        return str;
    }
    if (obj.constructor == Object) {
        var str = "<table>";
        for (var i in obj) {
            str += "<tr><td><input type='checkbox' id='" + obj[i].OrderNumber + "' />" + i + " : " + eachtr(obj[i].OrderNumber) + "</td></tr>";
        }
        str += "</table>";
        return str;
    }
    return obj;
}
function showinput2(s) {
    $("#engineval").val(s);
}
function fa2(s) {
    $("#PackageCollection_PackageType").val("");
}

function loadSKUlist() {
    var ID = $("#PackageCollection_ExpressNumber").val().trim();
    var CustomerID = $("#CustomerIDs option:selected").val();
    var Warehouse = $("#Warehouses option:selected").val();
    $.ajax({
        type: "Post",
        url: "/WMS/OrderECManagement/CheckExpressOrder",
        data: {
            "Number": ID,
            "Type": "Order",
            "CustomerID": CustomerID,
            "WarehouseID": Warehouse
        },
        async: "false",
        success: function (data) {
            var obj = jQuery.parseJSON(data);
            if (obj[0].OrderNumber == $("#PackageCollection_OrderNumber").val().trim()) {
                document.getElementById("SkuListTable").innerHTML = eachul(obj);
            } else {
                document.getElementById("SkuListTable").innerHTML = "";
                //取消选中关联列表中对应的选中框
                //$("input[type=checkbox][checked]").removeAttr("checked");
                document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">订单号不匹配！</h1>';
                $("#PackageCollection_Scan").blur();
            }
        },
        error: function (msg) {
            document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">' + msg.val + '</h1>';
            $("#PackageCollection_Scan").blur();
        }
    });
}
function Save() {
    var CustomerID = $("#CustomerIDs option:selected").val();
    var Warehouse = $("#Warehouses option:selected").text();
    var express2 = $("#PackageCollection_ExpressNumber").attr("value");
    var PackageType = $("#PackageCollection_PackageType").val().trim();

    $.ajax({
        type: "Post",
        url: "/WMS/OrderECManagement/SaveExpressPackage",
        data: {
            "CustomerID": CustomerID,
            "WarehouseName": Warehouse,
            "OrderID": OrderID,
            "ExpressNumber": express2,
            "PackageType": boxAttr,
            "PackageCode": $("#engineval").val()
            //"PackageType": PackageType             
        },
        async: "false",
        success: function (data) {
            var obj = jQuery.parseJSON(data);
            if (obj[0].Mess == "成功") {
                Next();
            } else {
                document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">保存失败:' + obj[0].Mess + '</h1>';
                $("#PackageCollection_Scan").blur();
            }
        },
        error: function (msg) {
            document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">"保存失败！"</h1>';

            $("#PackageCollection_Scan").blur();
        }
    });
}
function checkSKU() {
    if ($("#PackageCollection_Scan").val().trim().length > 0) {
        //验证客户和仓库是否选择了


        //验证当前SKU不在sku列表中
        var va;
        if ($('#ScanMode').attr('checked')) {
            va = "00" + $("#PackageCollection_Scan").val().trim();
        } else {
            va = $("#PackageCollection_Scan").val().trim();
        }

        var isover = "NO";
        //扣减



        $("#SkuListTable table tr").each(function (a, b) {
            if ($(b).find('td').eq(0).text() == va) {
                if ($(b).find('td').eq(2).text() == "0") {//扫完了                   
                    $(b).find('td').eq(2).attr("style", 'color:#FF00FF');
                    document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">此SKU数量已满足，请勿重复扫描！</h1>';
                    PlaySound();
                    $("#PackageCollection_Scan").select();
                    $("#PackageCollection_Scan").blur();
                    isover = "YES";
                    return;
                } else {
                    $(b).find('td').eq(2).text(parseFloat($(b).find('td').eq(2).text()) + 1);
                    document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:green;">请继续扫描SKU！</h1>';
                    if ($(b).find('td').eq(2).text() == "0") {
                        $(b).hide();
                    }
                    index = 6;
                    $("#PackageCollection_Scan").select();
                    isover = "YES";
                    return;
                }
            } else {

            }
        }
        )

        if (isover == "NO") {

            document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">SKU不存在，请检查！</h1>';
            PlaySound();
            $("#PackageCollection_Scan").select();
            //$("#PackageCollection_Scan").blur();
        }
        else {

            var count = 0;
            $("#SkuListTable table tr").each(function () {
                if ($(this)[0].style.display == "none") {
                    count++;
                }
            });
            if (count == $("#SkuListTable table tr").length) {
                $("#PackageCollection_Scan").val("");
                $("#PackageCollection_Scan").attr("readOnly", "true");
                index = 9;
                Save();
                //Next();
            }
        }

        //if ($("#" + va).length > 0) {
        //    //验证当前扫描的SKU数量不为0
        //    if ($("#" + va).children().eq(2)[0].innerHTML == "0") {
        //        $("#" + va).children().eq(2)[0].innerHTML = "0";
        //        $("#" + va).children().eq(2)[0].style.color = "#FF00FF";
        //        document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">SKU数量已满足，请勿重复扫描！</h1>';
        //        $("#PackageCollection_Scan").blur();
        //    } else {
        //        $("#" + va).children().eq(2)[0].innerHTML = parseFloat($("#" + va).children().eq(2)[0].innerHTML) + 1;
        //        document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:green;">请继续扫描SKU！</h1>';
        //        //验证数量为0 隐藏SKU
        //        if ($("#" + va).children().eq(2)[0].innerHTML == "0") {
        //            $("#" + va).hide();
        //        }
        //        index = 6;
        //        $("#PackageCollection_Scan").select();
        //    }
        //    //验证所有差异数量为0时，提示
        //    var count = 0;
        //    $("#SkuListTable table tr").each(function () {
        //        if ($(this)[0].style.display == "none") {
        //            count++;
        //        }
        //    });
        //    if (count == $("#SkuListTable table tr").length) {
        //        $("#PackageCollection_Scan").val("");
        //        $("#PackageCollection_Scan").attr("readOnly", "true");
        //        index = 9;
        //        Save();
        //        Next();
        //    }
        //} else {
        //    document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">SKU不存在，请检查！</h1>';
        //    $("#PackageCollection_Scan").blur();
        //}
    } else {
        document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">SKU为空，请重新扫描！</h1>';
        $("#PackageCollection_Scan").focus();
    }
}
function checkOrderNumber() {
    if ($("#PackageCollection_Scan").val().trim().length > 0) {
        //验证订单号 加载数据
        if ($("#PackageCollection_Scan").val().trim() == $("#PackageCollection_OrderNumber").val().trim()) {
            loadSKUlist();
            //选中关联列表中对应的选中框
            //$("#" + $("#PackageCollection_OrderNumber").val() + "").attr("checked", true);
            $("#PackageCollection_Scan").val("");
            $("#PackageCollection_Scan").attr("readOnly", "true");
            document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:green;">请选耗材！</h1>';
            $("#engineval").removeAttr("readOnly");
            $("#engineval").focus();
            index = 4;
        } else {
            document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">订单号不匹配！</h1>';
            PlaySound();
            $("#PackageCollection_Scan").select();
            //$("#PackageCollection_Scan").blur();
        }
    } else {
        document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">订单号为空，请重新扫描！</h1>';
        PlaySound();
        $("#PackageCollection_Scan").focus();
        //$("#PackageCollection_Scan").blur();
    }
}
function checkExpressNumber() {
    if ($("#PackageCollection_Scan").val().trim().length > 0) {
        var ID = $("#PackageCollection_Scan").val().trim();
        var CustomerID = $("#CustomerIDs option:selected").val();
        var Warehouse = $("#Warehouses option:selected").val();

        //验证是否是取消单

        let result = "";
        $.ajax({
            url: "/WMS/OrderECManagement/ValidOrderCancel",
            type: "POST",
            data:
            {
                OrderNumber: $("#PackageCollection_Scan").val().trim(),//订单号
                customerID: CustomerID,
                warehouse: Warehouse,
                type: 2
            },
            async: false,
            success: function (data) {
                result = data;
            }
        })
        if (result != "") {
            //$("#PackageCollection_Scan").blur();//失去焦点，防止误扫描
            //layer.alert("该订单已取消！", {
            //    //skin: 'layui-layer-lan' //样式类名
            //    //, closeBtn: 0
            //}, function (index) {
            //    layer.close(index);
            //    PlaySound();//该快递单状态不正确，播放声音
            //    $('#PackageCollection_Scan').select();
            //    $('#PackageCollection_Scan').focus();
            //});
            document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">该订单已取消！</h1>';
            PlaySound();
            $('#PackageCollection_Scan').select();
            $('#PackageCollection_Scan').focus();
            return false;
        }

        $.ajax({
            type: "Post",
            url: "/WMS/OrderECManagement/CheckExpressOrder",
            data: {
                "Number": ID,
                "Type": "Express",
                "CustomerID": CustomerID,
                "WarehouseID": Warehouse
            },
            async: "false",
            success: function (data) {
                var obj = jQuery.parseJSON(data);
                if (obj[0].AssociatedStatus == "0") {
                    document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">无关联信息，请重新扫描！</h1>';
                    PlaySound();
                    $("#PackageCollection_Scan").select();
                    //$("#PackageCollection_Scan").blur();
                }
                else {
                    if (obj[0].PackeStatus == "0") {
                        $("#PackageCollection_ExpressNumber").val($("#PackageCollection_Scan").val().trim());
                        $("#PackageCollection_ExpressCompany").val(obj[0].ExpressCompany);
                        $("#PackageCollection_OrderNumber").val(obj[0].OrderNumber);
                        OrderID = obj[0].OrderID;
                        document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:green;">请扫订单号！</h1>';
                        $("#PackageCollection_Scan").select();
                        index = 3;
                    } else {
                        //1 待处理 3 已拣货 4 已复检 6 已包装 8 已交接 9 已出库
                        var status;
                        if (obj[0].OrderStatus == "1") {
                            status = "待处理";
                        } else if (obj[0].OrderStatus == "3") {
                            status = "已拣货";
                        }
                        else if (obj[0].OrderStatus == "6") {
                            status = "已包装";
                        }
                        else if (obj[0].OrderStatus == "8") {
                            status = "已交接";
                        } else if (obj[0].OrderStatus == "9") {
                            status = "已出库";
                        } else {
                            status = obj[0].OrderStatus;
                        }
                        document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">当前状态【' + status + '】 不允许包装，请重新扫描！</h1>';
                        PlaySound();
                        $("#PackageCollection_Scan").select();
                        //$("#PackageCollection_Scan").blur();
                    }
                }
            },
            error: function (msg) {
                document.getElementById('AssociatedTable').innerHTML = msg.val;
                PlaySound();
                $("#PackageCollection_Scan").blur();
            }
        });
    } else {
        document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">快递单号为空，请重新扫描！</h1>';
        PlaySound();
        $("#PackageCollection_Scan").focus();
        //$("#PackageCollection_Scan").blur();
    }
}
function initialization() {
    //$('#CustomerIDs').focus();
    $('#Warehouses').attr('readOnly', 'true');
    $('#Warehouses').val('');
    $('#PackageCollection_Scan').attr('readOnly', 'true');
    $('#PackageCollection_Scan').val('');
    $('#PackageCollection_ExpressNumber').attr('readOnly', 'true');
    $('#PackageCollection_ExpressNumber').val('');
    $('#PackageCollection_ExpressCompany').attr('readOnly', 'true');
    $('#PackageCollection_ExpressCompany').val('');
    $('#PackageCollection_OrderNumber').attr('readOnly', 'true');
    $('#PackageCollection_OrderNumber').val('');
    $('#engineval').attr('readOnly', 'true');
    $('#engineval').attr('class', 'form-control');
    $('#engineval').show();
    $('#engineval').val('');
    $('#PackageCollection_PackageType').removeClass('form-control');
    $('#PackageCollection_PackageType').hide();
    $('#PackageCollection_Length').html('');
    $('#PackageCollection_Width').html('');
    $('#PackageCollection_Height').html('');
    $('#volume').html('');
    $('#AssociatedTable').attr('readOnly', 'true');
    document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:red;">请选择客户！</h1>';
    document.getElementById('SkuListTable').innerHTML = '';
    //废弃的控件
    //$('#CheckSKU').attr('readOnly', 'true');
    //$('#CheckSKU').val('');
    //$('#SaveButton').attr('readOnly', 'true');
    //$('#ScanMode').removeAttr('checked');
}
function Next() {
    $('#PackageCollection_Scan').removeAttr('readOnly');
    $('#PackageCollection_Scan').val('');
    $('#PackageCollection_Scan').focus();
    $('#PackageCollection_ExpressNumber').attr('readOnly', 'true');
    $('#PackageCollection_ExpressNumber').val('');
    $('#PackageCollection_ExpressCompany').attr('readOnly', 'true');
    $('#PackageCollection_ExpressCompany').val('');
    $('#PackageCollection_OrderNumber').attr('readOnly', 'true');
    $('#PackageCollection_OrderNumber').val('');
    $('#engineval').attr('readOnly', 'true');
    $('#engineval').attr('class', 'form-control');
    $('#engineval').show();
    $('#engineval').val('');
    $('#PackageCollection_PackageType').removeClass('form-control');
    $('#PackageCollection_PackageType').hide();
    $('#PackageCollection_Length').html('');
    $('#PackageCollection_Width').html('');
    $('#PackageCollection_Height').html('');
    $('#volume').html('');
    $('#AssociatedTable').attr('readOnly', 'true');
    document.getElementById('AssociatedTable').innerHTML = '<h1 style="color:green;">保存成功！<br/>请扫下一单快递单号！</h1>';
    document.getElementById('SkuListTable').innerHTML = '';
    boxAttr = "";
}

//播放声音
function PlaySound() {
    $("#Audio")[0].play();

}