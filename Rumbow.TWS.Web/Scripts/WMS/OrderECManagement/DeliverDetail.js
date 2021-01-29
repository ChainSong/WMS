//import { stat } from "fs";

$(document).ready(function () {
    //9状态不允许继续扫描称重和提交
    if (parseInt($('#hdStatus').val()) == 9) {
        $("#ScanExpress").attr('disabled', true);//9状态
        $("#btnSave").attr('disabled', true);
        $("#btnSubmitOut").attr('disabled', true);
    } else {
        //start();
    }

    if ($('#CustomerID').val()=="108") {
        $('#Express').find("option[value='德邦']").attr("selected", "selected");

    } else {

    }



    $("#ScanExpress").focus();//获取焦点

    //交接称重
    //扫描触发
    $("#ScanExpress").live('keydown', function () {
        if (event.keyCode == 13) {
            if ($(this).val().trim().length > 0) {

                //var weight = $("#yyzq");//先获取到自定义控件对象
                //var str = "";
                ////把自定义控件的innerHTML变成jquery对象去遍历下面的param
                //$.each($(weight[0].innerHTML), function (i, obj) {
                //    if (obj["name"] == "Tag") {
                //        str = obj["value"];
                //        return;
                //    }
                //});
                ////alert(str);
                //if (isNaN(str)) {
                //    showMsg("重量异常,重量不是数字！", 2000);
                //    return false;
                //}
                //if (str == "" || parseFloat(str) < 0.08 || parseFloat(str) > 100) {
                //    showMsg("重量异常,请确认重量在0.08-100kg之间", 2000);
                //    return false;
                //}

                //判断是否是取消单
                //验证当前订单是否是取消单
                let result = "";
                $.ajax({
                    url: "/WMS/OrderECManagement/ValidOrderCancel",
                    type: "POST",
                    data:
                    {
                        OrderNumber: $(this).val(),//快递单号
                        customerID: $('#CustomerID').val(),
                        warehouse: $('#Warehouse').val(),
                        type: 2
                    },
                    async: false,
                    success: function (data) {
                        result = data;
                    }
                })
                if (result != "") {
                    $(this).blur();//失去焦点，防止误扫描
                    layer.alert("该订单已取消！", {
                        //skin: 'layui-layer-lan' //样式类名
                        //, closeBtn: 0
                    }, function (index) {
                        layer.close(index);
                        PlaySound();//该快递单状态不正确，播放声音
                        $('#ScanExpress').select();
                        $('#ScanExpress').focus();
                    });
                    return false;
                }

                var message = ValidateDeliverIsExist($(this).val());
                //验证该单当前状态
                if (message != '') {//不能交接
                    $(this).blur();//失去焦点，防止误扫描
                    layer.alert("当前" + message, {
                        skin: 'layui-layer-lan' //样式类名
                        , closeBtn: 0
                    }, function (index) {
                        layer.close(index);
                        PlaySound();//该快递单状态不正确，播放声音
                        $('#ScanExpress').select();
                        $('#ScanExpress').focus();
                    });
                    return false;
                }
                else {
                    $(this).blur();
                    $("#ScanExpress").attr("disabled", true)
                    //显示关联到的订单
                    ShowOKData($(this).val());

                    $('#btnUpload').focus();
                    //$('#btnConfrim').focus();
                }
            }

        }
    })

    //确定按钮keydown事件
    $('#btnConfrim').live('keydown', function () {
        if (event.keyCode == 13) {
            var express = $('#ScanExpress').val();
            if (express != '') {
                //还是要验证一下快递单状态
                var message = ValidateDeliverIsExist(express);

                if (message != '') {
                    layer.alert("当前" + message, {
                        skin: 'layui-layer-lan' //样式类名
                        , closeBtn: 0
                    }, function (index) {
                        PlaySound();//该快递单状态不正确，播放声音
                        layer.close(index);
                        var tbList = document.getElementById('tbList');//清空待上传信息
                        tbList.innerHTML = "";
                        $("#ScanExpress").attr("disabled", false)
                        $('#ScanExpress').select();
                        //$('#ScanExpress').focus();
                    });

                    //layer.confirm('当前：' + message, { icon: 2, title: '提示' }, function (index) {
                    //    PlaySound();//该快递单状态不正确，播放声音
                    //    layer.close(index);
                    //    var tbList = document.getElementById('tbList');//清空待上传信息
                    //    tbList.innerHTML = "";
                    //    $("#ScanExpress").attr("disabled", false)
                    //    $('#ScanExpress').select();
                    //    //$('#ScanExpress').focus();
                    //});
                }
                else {
                    ShowOKData(express);
                    $('#btnUpload').focus();
                }
            }
            else {
                $('#ScanExpress').focus();
                return false;
            }
        }

    })

})


//验证输入的快递单状态
function ValidateDeliverIsExist(express) {
    var a = "";
    $.ajax({
        url: "/WMS/OrderECManagement/VilidateDeliverExpress",
        type: "POST",
        data:
        {
            ExpressNumber: express,//快递单号
            customerID: $('#CustomerID').val(),
            warehouse: $('#Warehouse').val()
        },
        async: false,
        success: function (data) {
            var result = data;
            a = result;
        }
    });
    return a;
}

//显示待上传信息
function ShowOKData(_obj) {
    var tbList = document.getElementById('tbList');
    tbList.innerHTML = "";
    //查询
    $.ajax({
        url: "/WMS/OrderECManagement/DeliverUploadData",
        type: "POST",
        data:
        {
            ExpressNumber: _obj,//快递单号
            customerID: $('#CustomerID').val(),
            warehouse: $('#Warehouse').val()
        },
        async: false,
        success: function (data) {
            var html = "";
            if (data.error == 1) {
                var deliver = data.data;
                for (var i = 0; i < deliver.length; i++) {
                    var tr = deliver[i];
                    html += '<tr>';
                    html += "<td style='width:190px;font-size:16px;display:none'>" + tr.OrderNumber + "</td>";
                    html += "<td style='width:190px;font-size:16px'>" + tr.str1 + "</td>";
                    //html += "<td><div id='btnUpload' style='text-align:center; width:75px; border:solid 1px ;border-radius:3px;cursor:pointer;color:white;background-color:#5bc0de'  onclick =\"UpLoadData('" + tr.OrderNumber + "',this)\" >上传</div></td>";
                    //html += "<td style='width:100px'><input  type='button' class='btn btn-success' style='text-align:center; vertical-align:middle; width:75px; border:solid 1px ;border-radius:3px;cursor:pointer;color:white;background-color:#5bc0de' value='" + tr.str1 + "' onclick='btnUploadClick()' id='btnUpload'\"></td>";
                    html += "<td style='width:100px'><input  type='button' class='btn btn-success' style='text-align:center; vertical-align:middle; width:75px; border:solid 1px ;border-radius:3px;cursor:pointer;color:white;background-color:#5bc0de' value='" + tr.str2 + "'  id='btnUpload'\"></td>";
                    html += "<td>" + tr.str3 + "</td>";
                    html += "</tr>";
                }
                $('#tbList').append(html);
            }
            else {
                showMsg("未找到需要交接的订单！", 2000);

            }
        }
    });

}




//上传
$('#btnUpload').live('keydown', function (event) {

    if (event.keyCode == 13) {

        //var weight = $("#yyzq");//先获取到自定义控件对象
        var strweight = $("#deliverweight").val();
        //把自定义控件的innerHTML变成jquery对象去遍历下面的param
        //$.each($(weight[0].innerHTML), function (i, obj) {
        //    if (obj["name"] == "Tag") {
        //        strweight = obj["value"];
        //    }
        //});
        //*验证重量，电子秤调好之后打开这段代码*
        if (strweight == "") {
            showMsg("重量不能为空！", 2000);
            return false;
        }
        if (!checkNum(strweight)) {
            showMsg("重量必须是数字格式", 2000);
            return false;
        }

        if (strweight == "" || parseFloat(strweight) < 0.08 || parseFloat(strweight) > 100) {
            showMsg("重量异常,请确认重量在0.08-100kg之间", 2000);
            return false;
        }
        if ($('#ScanExpress').val() != "") {
            var message = ValidateDeliverIsExist($('#ScanExpress').val());
            if (message != '') {
                layer.alert("当前" + message, {
                    skin: 'layui-layer-lan' //样式类名
                    , closeBtn: 0
                }, function (index) {
                    PlaySound();//该快递单状态不正确，播放声音
                    layer.close(index);
                    var tbList = document.getElementById('tbList');//清空待上传信息
                    tbList.innerHTML = "";
                    $("#ScanExpress").attr("disabled", false);

                    $('#ScanExpress').select();
                    $('#ScanExpress').focus();
                });

                //layer.confirm('当前：' + message + "请重新扫描交接！", { icon: 2, title: '提示' }, function (index) {
                //    PlaySound();//该快递单状态不正确，播放声音
                //    layer.close(index);
                //    var tbList = document.getElementById('tbList');//清空待上传信息
                //    tbList.innerHTML = "";
                //    $('#ScanExpress').select();
                //    $('#ScanExpress').focus();

                //});
                return false;
            }
        }
        else {
            showMsg("快递单号不能为空！", 4000)
            return false;
        }


        var SendComplete = true;//发送状态
        //上传
        //获取订单号
        var ordernumber = $('#tbList').find('tr').eq(0).children('td').eq(0).text();
        var externnumer = $('#tbList').find('tr').eq(0).children('td').eq(1).text();
        if (ordernumber == "") {
            layer.alert("未获取到订单号！");
            return false;
        }

        var result = "";
        var Deliverkey = $('#hdHeaderKey').val();
        var DeliverID = $('#hdHeaderID').val();
        var ExpressCompany = $('#Express').val();
        if (ExpressCompany == "") {
            layer.alert("请选择快递公司！")
            return false;
        }
        //验证当前订单是否是取消单
        $.ajax({
            url: "/WMS/OrderECManagement/ValidOrderCancel",
            type: "POST",
            data:
            {
                OrderNumber: ordernumber,//订单号
                customerID: $('#CustomerID').val(),
                warehouse: $('#Warehouse').val(),
                type: 1
            },
            async: false,
            success: function (data) {
                result = data;
            }
        })
        if (result == "") {

            //拼接json数据
            var jsonString = "[";
            var jsontxt1 = "{";
            jsontxt1 += "\"OrderNumber\"\:\"" + ordernumber + "\",";//订单号            
            jsontxt1 += "\"DeliverID\"\:\"" + DeliverID + "\",";//交接单ID
            jsontxt1 += "\"Deliverkey\"\:\"" + Deliverkey + "\",";//交接单号           
            jsontxt1 += "\"ExpressNumber\"\:\"" + $('#ScanExpress').val().trim() + "\",";//快递单号
            jsontxt1 += "\"BoxWeight\"\:\"" + strweight + "\",";//重量
            jsontxt1 += "},";
            jsonString += jsontxt1.substring(0, jsontxt1.length - 1);
            jsonString += "]";
            var res1 = "";
            res1 = DeliverHeaderAndDetailAdd(jsonString, ExpressCompany, Deliverkey, 0);

            if (res1 != "") {
                //成功刷新明细界面
                window.location.href = "/WMS/OrderECManagement/DeliverDetail/?ID=" + res1 + "&Type=1&customerID=" + $('#CustomerID').val() + "&warehouse=" + $('#Warehouse').val() + "";
            }
            else {//如果是交接写入的时候失败了，不刷新界面，重新（扫描快递单）交接一次
                var uploadstatus = $(this).parent("td").parent("tr").find("td").eq(3);
                $(uploadstatus).html('该单交接失败，请重新扫描快递单号交接！');
                $("#ScanExpress").attr('disabled', false);
                $(this).parent("td").parent("tr").find("td").eq(2).children().remove();//上传按钮删掉
                PlaySound();//交接单在写入的时候失败了播放声音
                $(uploadstatus).css('color', 'red');
                $(uploadstatus).css("font-size", '20px');
                //$('#ScanExpress').val('');
                $('#ScanExpress').select();
                return false;
            }
        }
        else {
            //layer.alert("该订单为取消订单！"), { icon: 2 };
            var uploadstatus = $(this).parent("td").parent("tr").find("td").eq(3);
            $(uploadstatus).html('交接失败:' + result);
            $(uploadstatus).css('color', 'red');
            $(uploadstatus).css("font-size", '22px');
            PlaySound();//扫描到了取消单播放声音
            SendComplete = false;
            //$('#ScanExpress').val();
            $('#ScanExpress').attr("disabled", false);
            $('#ScanExpress').select();
        }
    }
});

//为了使点击事件和回车事件效果一样，在点击的时候调用回车事件
$("#btnUpload").live('click', function (e) {
    if (e.offsetX > 0) {//因为按钮选中事件会触发点击事件，所以这里使用判断  lrg
        var event = $.Event("keydown");
        event.keyCode = 13;
        $(this).trigger(event);
    }

});

//新增交接单或者保存交接单ajax提交
//返回当前交接单ID用于刷新明细界面
function DeliverHeaderAndDetailAdd(jsonString, ExpressCompany, Deliverkey, flag) {
    var res = "";
    //保存交接信息（新建的需要新建交接单主表）
    $.ajax({
        url: "/WMS/OrderECManagement/DeliverHeaderAndDetailAdd",
        type: "POST",
        data:
        {
            customerID: $('#CustomerID').val(),//客户ID
            warehouse: $('#Warehouse').val(),//仓库名称
            "JsonString": jsonString,
            ExpressCompany: ExpressCompany,//快递公司
            DeliverKey: Deliverkey,
            flag: flag//新增一条的时候是0 保存的时候是1，如果用户点了保存这一票则要删除所有重新保存

        },
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.IsSuccess) {//成功！
                res = data.result;
                // result = true;
            }
            else {
                res = "";
                // result = false;
            }
        }
    })
    return res;

}

//删除行,传进明细行号和当前对象
function DeleteRow(detailkey, _obj) {
    //获取行数
    var rowslength = $('#Newtbody tr').length;
    if (rowslength <= 1) {
        showMsg("请至少保留一行！", 4000);
        return false;
    }
    layer.confirm('<font size="4">是否删除此行交接单？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);

        //因为删除时需要改订单状态，所以点删除后台就直接删了吧，
        //删除的两个条件（交接单号和快递单号）
        //var index = $(_obj).parents("tr").index();//获取tr当前的索引
        //var tr2 = $('#Newtbody2 tr').eq(index);
        //var deliverkey = $('#hdHeaderKey').val();//交接单号
        //var expressno = $(tr2).children()[3].innerText;//快递单号

        var deliverkey = $('#hdHeaderKey').val();//交接单号
        var expressno = $(_obj).parents('tr').children('td').eq(2).text(); //快递单号

        if (deliverkey != '' && expressno != '') {
            $.ajax({
                url: "/WMS/OrderECManagement/DeliverDetailDelete",
                type: "POST",
                data:
                {
                    customerID: $('#CustomerID').val(),
                    warehouse: $('#Warehouse').val(),
                    DeliverKey: deliverkey,
                    ExpressNumber: expressno
                },
                async: false,
                success: function (data) {
                    if (data == '') {
                        window.location.href = "/WMS/OrderECManagement/DeliverDetail/?ID=" + $('#hdHeaderID').val() + "&Type=1&customerID=" + $('#CustomerID').val() + "&warehouse=" + $('#Warehouse').val() + "";
                    }
                    else {
                        showMsg("删除失败，请查看订单状态！", 4000);
                        return;
                    }
                }
            })
        }
    });
}

//保存界面上的交接明细
$('#btnSave').live('click', function () {
    var flag = 0;
    if ($('#hdStatus').val() != "0") {
        showMsg("该交接单已提交出库，不允许再次保存！");
        flag = 1;
        return false;
    }

    var jsonString = "[";//最外边   
    var txt = "";
    var Deliverkey = $('#hdHeaderKey').val();
    var DeliverID = $('#hdHeaderID').val();
    var ExpressCompany = $('#Express').val();
    if (ExpressCompany == '') {
        flag = 1;
    }
    var rowlength = $('#Newtbody tr').length;
    if (rowlength > 0) {
        $('#Newtbody tr').each(function (a, b) {//each遍历所有行
            var trText = "{";
            trText += "\"DeliverID\"\:\"" + DeliverID + "\",";
            trText += "\"DeliverKey\"\:\"" + Deliverkey + "\",";
            trText += "\"DeliverDetailKey\"\:\"" + $(b).children()[1].innerText + "\",";

            trText += "\"ExpressNumber\"\:\"" + $(b).children()[2].innerText + "\",";
            trText += "\"OrderNumber\"\:\"" + $(b).children()[4].innerText + "\",";
            trText += "\"PackBoxKey\"\:\"" + $(b).children()[6].innerText + "\",";
            trText += "\"BoxWeight\"\:\"" + $(b).children()[3].innerText + "\",";
            trText += "},";
            txt += trText;
        })
        jsonString += txt;
        jsonString = jsonString.substring(0, jsonString.length - 1);
        jsonString += "]";

        if (flag == 0) {

            var res1 = DeliverHeaderAndDetailAdd(jsonString, ExpressCompany, Deliverkey, 1);//保存flag传1
            if (res1 != "") {//成功！
                //成功刷新明细界面
                //alert("保存成功！");
                //弹出层提示
                layer.confirm('保存成功！', {
                    btn: ['确定'] //按钮
                }, function () {
                    window.location.href = "/WMS/OrderECManagement/DeliverDetail/?ID=" + res1 + "&Type=1&customerID=" + $('#CustomerID').val() + "&warehouse=" + $('#Warehouse').val() + "";
                });






            }
            else {
                showMsg("保存失败！", 4000);
                return;
            }
        }
    }
    else {
        showMsg("没有需要保存的信息！", 4000);
    }
})

//打印交接清单
$('#btnPrintDeliver').live('click', function () {
    var href = "";
    if ($('#ProjectName').val() == 'YXDR') {
        href = "/WMS/OrderECManagement/PrintDeliveryYXDR/?deliverID=" + $('#hdHeaderID').val();
    }
    else {
        href = "/WMS/OrderECManagement/PrintDelivery/?deliverID=" + $('#hdHeaderID').val();
    }
    layer.open({
        type: 2,
        title: '',
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['1000px', '600px'],
        content: href
    });


})

//提交出库
$('#btnSubmitOut').live('click', function () {
    
    var rowlength = $('#Newtbody tr').length;
    if (rowlength <= 0) {
        showMsg("没有需要交接的订单！", 4000);
        return false;
    }


    layer.confirm('<font size="4">确定要提交出库吗？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        if ($('#hdStatus').val() != "0") {
            showMsg("该交接单已提交出库，不允许再次提交！");
            return false;
        }
        ////加一步验证，nike的cord项目需要再验证一遍取消单，因为可能在称重的时候由于并发造成的取消单刚好卡在称重的时候推过来
        //if ($("#ProjectName").val() == 'NIKE') {
            let valieresult;
            $.ajax({
                url: "/WMS/OrderECManagement/ValidDeliverOrderCancel",
                type: "POST",
                data:
                {
                    DeliverID: $('#hdHeaderID').val()
                },
                async: false,
                success: function (data) {
                    valieresult = data;
                }
            });
             
            //交接明细里面存在取消单
            if (valieresult.code == 1) {
                //把取消的订单弹出来给这帮人看看
                let resultmsgstr = "<div style='color: red; font-size: 16px;padding:15px'><p>取消订单如下：</p>";
                for (var i = 0; i < valieresult.data.length; i++) {
                    resultmsgstr += "<p>" + valieresult.data[i] + "</p> ";
                }
                resultmsgstr += "</div>";
                let index222 = layer.open({
                    type: 1,
                    title: ['检测到取消订单：', 'font-size:18px;'],
                    skin: 'layui-layer-rim', //加上边框
                    area: ['450px', '300px'], //宽高
                    content: resultmsgstr

                });
                if (index222) {
                    return false;
                }
            } else if (valieresult.code == 2) {//报错了
                showMsg("交接失败：" + valieresult.msg, 4000);
                return false;
            }
        //}


        //先校验交接清单里的订单在包装表里面的快递单在交接明细里是不是都存在
        var flag = 0;
        var result = "";
        var submitstring = "";//提交出库返回（空则成功，否则失败）
        $.ajax({
            url: "/WMS/OrderECManagement/DeliverCompleteInfoValidate",
            type: "POST",
            data:
            {
                DeliverID: $('#hdHeaderID').val(),
                customerID: $('#CustomerID').val()
            },
            async: false,
            success: function (data) {
                result = data;

            }
        })
        if (result != '') {//返回不是空，代表报错或者订单没有快递单号
            layer.confirm(result, { icon: 2, title: '提示' }, function (index) {
                layer.close(index);
            });

            return false;
        }
        else {
            if ($("#ProjectName").val() == "吉特") {
                //判断交接单中所有的订单是否处于可进行出库操作
                $.ajax({
                    type: "POST",
                    url: "/WMS/OrderECManagement/CheckExpressStatus",
                    data: {
                        DeliverID: $('#hdHeaderID').val(),
                        customerID: $('#CustomerID').val()
                    },
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.Code == 2) {
                            showMsg("检查交接单失败！", 4000);
                        }
                        else if (data.Code == 0) {
                            //if (data.data.length <= 0) {
                            //出库操作（扣库存，改状态！）
                            $.ajax({
                                url: "/WMS/OrderECManagement/DeliverOut",
                                type: "POST",
                                data:
                                {
                                    DeliverID: $('#hdHeaderID').val()
                                },
                                async: false,
                                success: function (data) {
                                    submitstring = data;
                                }
                            });
                            if (submitstring != '') {
                                showMsg(submitstring, 4000);
                                return false;
                            }
                            else {//提交出库成功！，刷新界面
                                //showMsg("出货成功！", 4000);
                                //alert("出库成功！");
                                layer.confirm('提交出库成功！', {
                                    btn: ['确定'] //按钮
                                }, function () {
                                    window.location.href = "/WMS/OrderECManagement/DeliverDetail/?ID=" + $('#hdHeaderID').val() + "&Type=1&customerID=" + $('#CustomerID').val() + "&warehouse=" + $('#Warehouse').val() + "";
                                });

                            }
                            //}
                        }
                        else if (data.Code == 1) {
                            var html = $("#CheckDifference").render(data.data);
                            //页面层
                            layer.open({
                                title: '<h4 style="color: #ff0000;text-align:center">以下快递状态不可进行出库操作</h4>',
                                type: 1,
                                skin: 'layui-layer-rim', //加上边框
                                area: ['380px', '210px'], //宽高
                                content: showmsgOut(html)
                            });

                            $("#OrderOutID").val($('#hdHeaderID').val());
                           // TempObj = $(obj);
                        }
                        else {
                            showMsg("检查交接单失败！", 4000);
                        }

                    },
                    error: function (msg) {
                        showMsg("检查交接单失败！", 4000);
                    } 
                });
            }
            else {
                $.ajax({
                    url: "/WMS/OrderECManagement/DeliverOut",
                    type: "POST",
                    data:
                    {
                        DeliverID: $('#hdHeaderID').val()
                    },
                    async: false,
                    success: function (data) {
                        submitstring = data;
                    }
                })
                if (submitstring != '') {
                    showMsg(submitstring, 4000);
                    return false;
                }
                else {//提交出库成功！，刷新界面
                    //showMsg("出货成功！", 4000);
                    //alert("出库成功！");
                    layer.confirm('提交出库成功！', {
                        btn: ['确定'] //按钮
                    }, function () {
                        window.location.href = "/WMS/OrderECManagement/DeliverDetail/?ID=" + $('#hdHeaderID').val() + "&Type=1&customerID=" + $('#CustomerID').val() + "&warehouse=" + $('#Warehouse').val() + "";
                    });

                }

            }

            
        }
    });
})

//悬浮事件
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

//播放声音
function PlaySound() {
    $("#Audio")[0].play();
}

$("#OutBack").live('click', function () {
    layer.closeAll();
});

//菜单切换
function tabSwitch2(_this, content_frefix, active) {

    var tabs = document.getElementsByName(_this.name);
    var number = tabs.length;
    for (var i = 0; i < number; i++) {
        var tab = tabs[i];
        tab.className = "";//移除class
        document.getElementById(content_frefix + i).style.display = "none";//两个tab页都隐藏     
    }
    _this.className = "easytab_active";
    document.getElementById(content_frefix + active).style.display = "block";//开启选择的tab页

}


//上传点击事件
//function btnUploadClick() {
//    $("#btnUpload").keydown();
//}


//webscoket连接电子秤
function start() {
    let weighttxt = $('#deliverweight');
    let weightmsg = $('#weightmsg');
    var wsImpl = window.WebSocket || window.MozWebSocket;
    window.ws = new wsImpl('ws://127.0.0.1:8089/');
    ws.onmessage = function (evt) {

        weightmsg.text('电子秤连接成功');
        weightmsg.css('color', 'green');
        weighttxt.val(evt.data);
    };

    // when the connection is established, this method is called
    ws.onopen = function () {
        weightmsg.text('连接成功');
    };

    // when the connection is closed, this method is called
    ws.onclose = function () {
        weightmsg.text('电子秤连接失败');
        weightmsg.css('color', 'red');
        //weighttxt.val('');
        heartCheck.start();
    };
}

var heartCheck = {
    timeout: 3000, //重连时间
    timeoutObj: null,
    start: function () {
        this.timeoutObj = setTimeout(function () {
            start(); //这里重新创建 websocket 对象并赋值
        }, this.timeout);
    }
};
function checkNum(e) {
    var reg = /^[0-9]+.?[0-9]*$/;//用来验证数字，包括小数的正则

    if (e != "") {
        if (!reg.test(e)) {
            return false;
        }
    }
    return true;
} 


function showmsgOut(html) {
    return dataTable = " <table>\
        <thead>\
            <tr>\
                <th>系统单号</th>\
                <th>外部单号</th>\
                <th>快递单号</th>\
            </tr>\
        </thead>\
        <tbody id='CheckDifferencePopup'>" + html + "</tbody> </table> \
     <div style='text-align:center;margin-top:10px'>\
            \
            <input type='hidden' id='OrderOutID'/>\
            <input type='hidden' id='OrderOutObj'/>\
        </div >"
}





