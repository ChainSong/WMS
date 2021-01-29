$(document).ready(function () {
    var flag = 1;
    $("#UpdateVolumeOK").live('click', function () {
        AjaxSendEpacklistEmail();
    })

    $("#UpdateVolumeOKCancel").live('click', function () {
        closePopup();
    });
    $("#CompleteDateOK").live('click', function () {

        if ($("#start_CompleteDate").val() == '' || $("#end_CompleteDate").val()=='') {
            showMsg("请输入完成时间", 3000);
            return;
        }
        $.ajax({
            url: "/WMS/ReportManagement/GetReportSendEmail",
            type: "POST",
            async: false,
            dataType: "json",
            data: {
                "StoreID": $("#CompleteStorerKey").val(),
                "CustomerID": $("#CompleteCustomerIDS").val(),
                "start_CompleteDate": $("#start_CompleteDate").val(),
                "end_CompleteDate": $("#end_CompleteDate").val()
            },
            success: function (data) {
                if (data.code == "1") {
                    var result = data.Result;
                    var result2 = data.Result2;
                    //库存统计
                    if (result[0].length > 0) {
                        var inventoryTotal = 0;
                        for (var i = 0; i < result[0].length; i++) {
                            if (result[0][i].BU == "APP") {
                                $("#inventoryAPP")[0].innerText = result[0][i].Qty;
                                inventoryTotal = inventoryTotal + result[0][i].Qty;
                            }
                            if (result[0][i].BU == "EQP") {
                                $("#inventoryEQP")[0].innerText = result[0][i].Qty;
                                inventoryTotal = inventoryTotal + result[0][i].Qty;
                            }
                            if (result[0][i].BU == "FTW") {
                                $("#inventoryFTW")[0].innerText = result[0][i].Qty;
                                inventoryTotal = inventoryTotal + result[0][i].Qty;
                            }
                        }
                        $("#inventoryTotal")[0].innerText = inventoryTotal;
                    }
                    //冻结数量
                    //if (result[8].length > 0) {
                    //    var frezzTotal = 0;
                    //    for (var i = 0; i < result[8].length; i++) {
                    //        if (result[8][i].BU == "APP") {
                    //            $("#frezzAPP")[0].innerText = result[8][i].Qty;
                    //            frezzTotal = frezzTotal + result[8][i].Qty;
                    //        }
                    //        if (result[8][i].BU == "EQP") {
                    //            $("#frezzEQP")[0].innerText = result[8][i].Qty;
                    //            frezzTotal = frezzTotal + result[8][i].Qty;
                    //        }
                    //        if (result[8][i].BU == "FTW") {
                    //            $("#frezzFTW")[0].innerText = result[8][i].Qty;
                    //            frezzTotal = frezzTotal + result[8][i].Qty;
                    //        }
                    //    }
                    //    $("#frezzTotal")[0].innerText = frezzTotal;
                    //}
                    //收货数量
                    if (result[1].length > 0) {
                        var ShouHuoTotal = 0;
                        for (var i = 0; i < result[1].length; i++) {
                            if (result[1][i].BU == "APP") {
                                $("#ShouHuoAPP")[0].innerText = result[1][i].Qty;
                                ShouHuoTotal = ShouHuoTotal + result[1][i].Qty;
                            }
                            if (result[1][i].BU == "EQP") {
                                $("#ShouHuoEQP")[0].innerText = result[1][i].Qty;
                                ShouHuoTotal = ShouHuoTotal + result[1][i].Qty;
                            }
                            if (result[1][i].BU == "FTW") {
                                $("#ShouHuoFTW")[0].innerText = result[1][i].Qty;
                                ShouHuoTotal = ShouHuoTotal + result[1][i].Qty;
                            }
                        }
                        $("#ShouHuoTotal")[0].innerText = ShouHuoTotal;
                    }
                    //退货入库数量
                    if (result[2].length > 0) {
                        var TuiHuoTotal = 0;
                        for (var i = 0; i < result[2].length; i++) {
                            if (result[2][i].BU == "APP") {
                                $("#TuiHuoAPP")[0].innerText = result[2][i].Qty;
                                TuiHuoTotal = TuiHuoTotal + result[2][i].Qty;
                            }
                            if (result[2][i].BU == "EQP") {
                                $("#TuiHuoEQP")[0].innerText = result[2][i].Qty;
                                TuiHuoTotal = TuiHuoTotal + result[2][i].Qty;
                            }
                            if (result[2][i].BU == "FTW") {
                                $("#TuiHuoFTW")[0].innerText = result[2][i].Qty;
                                TuiHuoTotal = TuiHuoTotal + result[2][i].Qty;
                            }
                        }
                        $("#TuiHuoTotal")[0].innerText = TuiHuoTotal;
                    }
                    //门店补货数量
                    if (result[3].length > 0) {
                        var BuHuoTotal = 0;
                        for (var i = 0; i < result[3].length; i++) {
                            if (result[3][i].BU == "APP") {
                                $("#BuHuoAPP")[0].innerText = result[3][i].Qty;
                                BuHuoTotal = BuHuoTotal + result[3][i].Qty;
                            }
                            if (result[3][i].BU == "EQP") {
                                $("#BuHuoEQP")[0].innerText = result[3][i].Qty;
                                BuHuoTotal = BuHuoTotal + result[3][i].Qty;
                            }
                            if (result[3][i].BU == "FTW") {
                                $("#BuHuoFTW")[0].innerText = result[3][i].Qty;
                                BuHuoTotal = BuHuoTotal + result[3][i].Qty;
                            }
                        }
                        $("#BuHuoTotal")[0].innerText = BuHuoTotal;
                    }
                    //直发数量
                    if (result[9].length > 0) {
                        var ZhiFATotal = 0;
                        for (var i = 0; i < result[9].length; i++) {
                            if (result[9][i].BU == "APP") {
                                $("#ZhiFAAPP")[0].innerText = result[9][i].Qty;
                                ZhiFATotal = ZhiFATotal + result[9][i].Qty;
                            }
                            if (result[9][i].BU == "EQP") {
                                $("#ZhiFAEQP")[0].innerText = result[9][i].Qty;
                                ZhiFATotal = ZhiFATotal + result[9][i].Qty;
                            }
                            if (result[9][i].BU == "FTW") {
                                $("#ZhiFAFTW")[0].innerText = result[9][i].Qty;
                                ZhiFATotal = ZhiFATotal + result[9][i].Qty;
                            }
                        }
                        $("#ZhiFaTotals")[0].innerText = ZhiFATotal;
                    }
                    //O2O数量
                    if (result[10].length > 0) {
                        var O2OTotal = 0;
                        for (var i = 0; i < result[10].length; i++) {
                            if (result[10][i].BU == "APP") {
                                $("#O2OAPP")[0].innerText = result[10][i].Qty;
                                O2OTotal = O2OTotal + result[10][i].Qty;
                            }
                            if (result[10][i].BU == "EQP") {
                                $("#O2OEQP")[0].innerText = result[10][i].Qty;
                                O2OTotal = O2OTotal + result[10][i].Qty;
                            }
                            if (result[10][i].BU == "FTW") {
                                $("#O2OFTW")[0].innerText = result[10][i].Qty;
                                O2OTotal = O2OTotal + result[10][i].Qty;
                            }
                        }
                        $("#O2OTotal")[0].innerText = O2OTotal;
                    }
                    //门店调拨调入数量
                    if (result[4].length > 0) {
                        var DiaoRuTotal = 0;
                        for (var i = 0; i < result[4].length; i++) {
                            if (result[4][i].BU == "APP") {
                                $("#DiaoRuAPP")[0].innerText = result[4][i].Qty;
                                DiaoRuTotal = DiaoRuTotal + result[4][i].Qty;
                            }
                            if (result[4][i].BU == "EQP") {
                                $("#DiaoRuEQP")[0].innerText = result[4][i].Qty;
                                DiaoRuTotal = DiaoRuTotal + result[4][i].Qty;
                            }
                            if (result[4][i].BU == "FTW") {
                                $("#DiaoRuFTW")[0].innerText = result[4][i].Qty;
                                DiaoRuTotal = DiaoRuTotal + result[4][i].Qty;
                            }
                        }
                        $("#DiaoRuTotal")[0].innerText = DiaoRuTotal;
                    }
                    //门店调拨调出数量
                    if (result[5].length > 0) {
                        var DiaoChuTotal = 0;
                        for (var i = 0; i < result[5].length; i++) {
                            if (result[5][i].BU == "APP") {
                                $("#DiaoChuAPP")[0].innerText = result[5][i].Qty;
                                DiaoChuTotal = DiaoChuTotal + result[5][i].Qty;
                            }
                            if (result[5][i].BU == "EQP") {
                                $("#DiaoChuEQP")[0].innerText = result[5][i].Qty;
                                DiaoChuTotal = DiaoChuTotal + result[5][i].Qty;
                            }
                            if (result[5][i].BU == "FTW") {
                                $("#DiaoChuFTW")[0].innerText = result[5][i].Qty;
                                DiaoChuTotal = DiaoChuTotal + result[5][i].Qty;
                            }
                        }
                        $("#DiaoChuTotal")[0].innerText = DiaoChuTotal;
                    }

                    //索赔调减
                    if (result[6].length > 0) {
                        var AdjustmentTotal = 0;
                        for (var i = 0; i < result[6].length; i++) {
                            if (result[6][i].BU == "APP") {
                                $("#AdjustmentAPP")[0].innerText = result[6][i].Qty;
                                AdjustmentTotal = AdjustmentTotal + result[6][i].Qty;
                            }
                            if (result[6][i].BU == "EQP") {
                                $("#AdjustmentEQP")[0].innerText = result[6][i].Qty;
                                AdjustmentTotal = AdjustmentTotal + result[6][i].Qty;
                            }
                            if (result[6][i].BU == "FTW") {
                                $("#AdjustmentFTW")[0].innerText = result[6][i].Qty;
                                AdjustmentTotal = AdjustmentTotal + result[6][i].Qty;
                            }
                        }
                        $("#AdjustmentTotal")[0].innerText = AdjustmentTotal;
                    }
                    //索赔调增
                    if (result[7].length > 0) {
                        var AdjustmentAddTotal = 0;
                        for (var i = 0; i < result[7].length; i++) {
                            if (result[7][i].BU == "APP") {
                                $("#AdjustmentAddAPP")[0].innerText = result[7][i].Qty;
                                AdjustmentAddTotal = AdjustmentAddTotal + result[7][i].Qty;
                            }
                            if (result[7][i].BU == "EQP") {
                                $("#AdjustmentAddEQP")[0].innerText = result[7][i].Qty;
                                AdjustmentAddTotal = AdjustmentAddTotal + result[7][i].Qty;
                            }
                            if (result[7][i].BU == "FTW") {
                                $("#AdjustmentAddFTW")[0].innerText = result[7][i].Qty;
                                AdjustmentAddTotal = AdjustmentAddTotal + result[7][i].Qty;
                            }
                        }
                        $("#AdjustmentAddTotal")[0].innerText = AdjustmentAddTotal;
                    }

                    if (result2.length > 0) {
                        var myDate = new Date();
                        //获取当前年
                        var year = myDate.getFullYear();
                        //获取当前月
                        var month = myDate.getMonth() + 1;
                        //获取当前日
                        var date = myDate.getDate();
                        $("#attachmentcontent")[0].innerText = "附件为" + result2[0].UserDef10 + " - CSC, " + year + "年" + month + "月" + date + "日库存报表及明细 （" + result2[0].Company + " ），库存报表请以“可用数量”为准，请知悉，谢谢!";
                        $("#DayReportBottom")[0].innerHTML = "SHCSC 曹友群/钱鸣/朱陈颖/陈磊<br />" +
                            "联系电话： (+021)57719958 <br />" +
                            "邮箱：shcsc_nike@runbow.com.cn" +
                            "<br/> <br />" +
                            "如有问题请联系，谢谢！！" +
                            "<hr style='color:aliceblue;text-align:left' />" +
                            "<div style='font-weight:bold;color:#38667d'>" +
                            "Best Regards！<br />" +
                            "曹友群/钱鸣/朱陈颖/陈磊<br />" +
                            "SHCSC<br />" +
                            "RUNBOW<br />" +
                            "T：(+021)57719958" +
                            "</div>" +
                            "<div style='color:#6f7dc4'>http://www.runbow.com.cn</div>" +
                            "<div style='color:#ff9933'>虹迪股份—供应链专家</div>" +
                            "<div>shcsc_nike@runbow.com.cn</div>";

                    }
                }

            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.status);
                alert(XMLHttpRequest.readyState);
                alert(textStatus);
            }
        });



        $.ajax({
            url: "/WMS/ReportManagement/ReportSendEmail",
            type: "POST",
            async: false,
            dataType: "text",
            data: {
                "StoreID": $("#CompleteStorerKey").val(),
                "CustomerID": $("#CompleteCustomerIDS").val(),
                "ContentHtml": $("#DayReport")[0].innerHTML.replace(/\"/g, "'"),
                "start_CompleteDate": $("#start_CompleteDate").val(),
                "end_CompleteDate": $("#end_CompleteDate").val()
            },
            success: function (data) {
                if (data == "") {
                    //layer.close(index2);
                    closePopup();
                    window.location.href = "/WMS/ReportManagement/NikeReportSendEmail";
                    layer.msg("发送成功");
                }
                else {
                    layer.msg("发送失败：" + data);
                    layer.close(index2);
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.status);
                alert(XMLHttpRequest.readyState);
                alert(textStatus);
            }
        });
    })

    $("#CompleteDateCancel").live('click', function () {
        closePopup();
    });
});
function sendEmail(StorerKey, CustomerIDS) {
    layer.confirm('<font size="4">确认是否发送每日报表邮件？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);      
        openPopup('CompleteDateDiv', true, 450, 300, null, 'CompleteDateDiv');
        $("#CompleteStorerKey").val(StorerKey);
        $("#CompleteCustomerIDS").val(CustomerIDS);
        $("#popupLayer_CompleteDateDiv")[0].style.top = "200px";

        //var index2 = layer.load(0, { shade: false }); //0代表加载的风格，支持0-2
       
    });
}

function sendEpackListEmail(StorerKey, CustomerIDS) {
    layer.confirm('<font size="4">确认是否发送Epacklist邮件？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        //var index2 = layer.load(0, { shade: false }); //0代表加载的风格，支持0-2
        $("#StorerKeyEpacklist").val(StorerKey);
        $("#CustomerIDSEpacklist").val(CustomerIDS);
        openPopup('UpdateVolume', true, 450, 300, null, 'UpdateVolumeDiv');
        $("#popupLayer_UpdateVolume")[0].style.top = "200px";

    });
}

function AjaxSendEpacklistEmail() {
    if ($('#DriverName').val() == "") {
        showMsg("请输入司机姓名", "4000");
        return;
    }
    if ($("#DriverTel").val() == '') {
        showMsg("请输入联系方式", 3000);
        return;
    }
    if ($("#CarNo").val() == '') {
        showMsg("请输入车牌号", 3000);
        return;
    }
    if ($("#ExpectTime").val() == '') {
        showMsg("请输入预计到达时间", 3000);
        return;
    }
    if ($("#start_CompleteDate2").val() == '' || $("#end_CompleteDate2").val() == '') {
        showMsg("请输入完成时间", 3000);
        return;
    }
    var indexok = layer.load(0, { shade: [0.1, '#fff'] }); //0代表加载的风格，支持0-2
    $('#UpdateVolumeOK').prop('disabled', true);
    $.ajax({
        url: "/WMS/ReportManagement/GetEpackListSendEmail",
        type: "POST",
        async: false,
        dataType: "json",
        data: {
            "StoreID": $("#StorerKeyEpacklist").val(),
            "CustomerID": $("#CustomerIDSEpacklist").val(),
            "start_CompleteDate": $("#start_CompleteDate2").val(),
            "end_CompleteDate": $("#end_CompleteDate2").val()
        },
        success: function (data) {
            if (data.code == "1") {
                var result = data.Result;
                var result2 = data.Result2;
                if (result[0].length > 0) {
                    var EpackListBoxTotal = 0;
                    var EpackListSKUTotal = 0;
                    for (var i = 0; i < result[0].length; i++) {
                        if (result[0][i].BU == "APP") {
                            $("#EpackListBoxAPP")[0].innerText = result[0][i].InventoryQty;
                            $("#EpackListSKUAPP")[0].innerText = result[0][i].Qty;
                            EpackListBoxTotal = EpackListBoxTotal + result[0][i].InventoryQty;
                            EpackListSKUTotal = EpackListSKUTotal + result[0][i].Qty;
                        }
                        if (result[0][i].BU == "EQP") {
                            $("#EpackListBoxEQP")[0].innerText = result[0][i].InventoryQty;
                            $("#EpackListSKUEQP")[0].innerText = result[0][i].Qty;
                            EpackListBoxTotal = EpackListBoxTotal + result[0][i].InventoryQty;
                            EpackListSKUTotal = EpackListSKUTotal + result[0][i].Qty;
                        }
                        if (result[0][i].BU == "FTW") {
                            $("#EpackListBoxFTW")[0].innerText = result[0][i].InventoryQty;
                            $("#EpackListSKUFTW")[0].innerText = result[0][i].Qty;
                            EpackListBoxTotal = EpackListBoxTotal + result[0][i].InventoryQty;
                            EpackListSKUTotal = EpackListSKUTotal + result[0][i].Qty;
                        }
                    }
                    $("#EpackListBoxTotal")[0].innerText = EpackListBoxTotal;
                    $("#EpackListSKUTotal")[0].innerText = EpackListSKUTotal;
                }
                if (result2.length > 0) {
                    var myDate = new Date();
                    //获取当前月
                    var month = myDate.getMonth() + 1;
                    //获取当前日
                    var date = myDate.getDate();
                    $("#EpAttachmentContent")[0].innerText = "附件为" + month + "月" + date + "日" + result2[0].Company + "补货明细清单及统计表："
                    $("#EpackListBottom")[0].innerHTML = "SHCSC 曹友群/钱鸣/朱陈颖/陈磊<br />" +
                            "联系电话：(+021)57719958 <br />" +
                            "邮箱：shcsc_nike@runbow.com.cn" +
                            "<br/> <br />" +
                            "如有问题请联系，谢谢！！" +
                            "<hr style='color:aliceblue;text-align:left' />" +
                            "<div style='font-weight:bold;color:#38667d'>" +
                            "Best Regards！<br />" +
                            "曹友群/钱鸣/朱陈颖/陈磊<br />" +
                            "SHCSC<br />" +
                            "RUNBOW<br />" +
                            "T：(+021)57719958" +
                            "</div>" +
                            "<div style='color:#6f7dc4'>http://www.runbow.com.cn</div>" +
                            "<div style='color:#ff9933'>虹迪股份—供应链专家</div>" +
                            "<div>shcsc_nike@runbow.com.cn</div>";
                    
                }
            }

        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status);
            alert(XMLHttpRequest.readyState);
            alert(textStatus);
        }
    });
    $.ajax({
        url: "/WMS/ReportManagement/EpackListReportSendEmail",
        type: "POST",
        async: false,
        dataType: "text",
        data: {
            "StoreID": $("#StorerKeyEpacklist").val(),
            "CustomerID": $("#CustomerIDSEpacklist").val(),
            "ContentHtml": $("#EpackList")[0].innerHTML.replace(/\"/g, "'"),
            "DriverName": $("#DriverName").val(),
            "DriverTel": $("#DriverTel").val(),
            "CarNo": $("#CarNo").val(),
            "ExpectTime": $("#ExpectTime").val(),
            "start_CompleteDate": $("#start_CompleteDate2").val(),
            "end_CompleteDate": $("#end_CompleteDate2").val()
        },
        beforeSend: function () {
            flag = 0
        },
        success: function (data) {
            if (data == "") {
                flag = 1;
                layer.close(indexok);
                window.location.href = "/WMS/ReportManagement/NikeReportSendEmail";
                layer.msg("发送成功");
            }
            else {
                flag = 1;
                layer.msg("发送失败：" + data);
                layer.close(indexok);
            }
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest.status);
            alert(XMLHttpRequest.readyState);
            alert(textStatus);
        }
    });
}