function onRegionSelected(rid, rn, treeId) {
    if (treeId === 'startCity') {
        $('#SearchCondition_StartCityID').val($('#startCityTreeID').attr('value'));
        $('#SearchCondition_StartCityName').val($('#startCityTreeName').attr('value'));
        $('#SearchCondition_StartCities').val($('#startCityTreeID').attr('value'));
    } else if (treeId === 'endCity') {
        $('#SearchCondition_EndCityID').val($('#endCityTreeID').attr('value'));
        $('#SearchCondition_EndCityName').val($('#endCityTreeName').attr('value'));
        $('#SearchCondition_EndCities').val($('#endCityTreeID').attr('value'));
    }
}

function onRegionAutoCompleteSelected(globalID) {
    if (globalID === 'startCity') {
        $('#SearchCondition_StartCityID').val($('#startCityTreeID').attr('value'));
        $('#SearchCondition_StartCityName').val($('#startCityTreeName').attr('value'));
        $('#SearchCondition_StartCities').val($('#startCityTreeID').attr('value'));
    } else if (globalID === 'endCity') {
        $('#SearchCondition_EndCityID').val($('#endCityTreeID').attr('value'));
        $('#SearchCondition_EndCityName').val($('#endCityTreeName').attr('value'));
        $('#SearchCondition_EndCities').val($('#endCityTreeID').attr('value'));
    }
}

$(document).ready(function () {

    $('#SearchCondition_CustomerOrShipperName').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Pod/Pod/GetUserShipper",
                type: "POST",
                dataType: "json",
                data: { name: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#SearchCondition_CustomerOrShipperID').val(ui.item.data.Value);
            $('#SearchCondition_CustomerOrShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#SearchCondition_CustomerOrShipperID').val('');
        }
    });

    var setHiddenValToControl = function () {
        $('#startCityTreeID').val($('#SearchCondition_StartCities').val());
        $('#startCityTreeName').val($('#SearchCondition_StartCityName').val());
        $('#endCityTreeID').val($('#SearchCondition_EndCities').val());
        $('#endCityTreeName').val($('#SearchCondition_EndCityName').val());

        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            if ($('#' + descID).val() !== '') {
                $(this).val($('#' + descID).val().split(' ')[0]);
            }
        });
    };

    var setPageControlVal = function () {
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    };

    $('#searchButton').click(function () {
        setPageControlVal();
    });

    $('#startCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#SearchCondition_StartCityID').val('');
        $('#SearchCondition_StartCityName').val('');
        $('#SearchCondition_StartCities').val('');
    });

    $('#endCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#SearchCondition_EndCityID').val('');
        $('#SearchCondition_EndCityName').val('');
        $('#SearchCondition_EndCities').val('');
    });

    $('#selectAll').live('click', function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

        RefreshIDs();
    });

    $("#resultTable tbody input[type='checkbox']").live('click', function () {
        RefreshIDs();
    });

    var RefreshIDs = function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var length = checkBoxs.length;
        var IDs = [];
        var checked = 0;
        checkBoxs.each(function () {
            if ($(this).attr("checked") === "checked") {
                var id = $(this).attr("data-ID");
                //var id = { ID: $(this).attr("data-ID") };
                IDs.push(id);
                checked++;
            }
        });

        if (checked == checkBoxs.length) {
            $('#selectAll').attr("checked", "checked");
        } else {
            $('#selectAll').removeAttr("checked");
        }

        //$('#SelectedIDs').val(JSON.stringify(IDs));
        $('#SelectedIDs').val(IDs);
    }

    $('#resultTable').find('input[type=text]').live('change', function () {
        var fee = $(this).val().trim();
        var isNum = /^\d+(\.\d+)?$/;
        if (fee === '') {
            $(this).val('0.00');
        } else {
            if (!isNum.test(fee)) {
                Runbow.TWS.Alert("请输入正确的金额格式");
                $(this).val('');
                $(this)[0].focus();
                return;
            }
        }

        var tr = $(this).parent().parent();
        var shipAmt = parseFloat(tr.find('#txtShipAmt').val());
        var bafAmt = parseFloat(tr.find('#txtBafAmt').val());
        var pointAmt = parseFloat(tr.find('#txtPointAmt').val());
        var otherAmt = parseFloat(tr.find('#txtOtherAmt').val());
        var tptalAmt = accounting.formatMoney((shipAmt + bafAmt + pointAmt + otherAmt), "￥", 2, "", ".");
        $(tr.find('td:eq(7)')).text(tptalAmt);
    });

    $('#resultTable').find('.batchAdjust').live('click', function () {
        var tr = $(this).parent().parent();
        var id = $(this).attr('data-id');
        var shipAmt = tr.find('#txtShipAmt').val().trim();
        var bafAmt = tr.find('#txtBafAmt').val().trim();
        var pointAmt = tr.find('#txtPointAmt').val().trim();
        var otherAmt = tr.find('#txtOtherAmt').val().trim();
        var remark = tr.find('#txtRemark').val().trim();
        var isNum = /^\d+(\.\d+)?$/;
        if (shipAmt === '') {
            tr.find('#txtShipAmt').val('0.00');
            shipAmt = '0';
        }

        if (bafAmt === '') {
            tr.find('#txtBafAmt').val('0.00');
            bafAmt = 0;
        }

        if (pointAmt === '') {
            tr.find('#txtPointAmt').val('0.00');
            pointAmt = 0;
        }

        if (otherAmt === '') {
            tr.find('#txtOtherAmt').val('0.00');
            otherAmt = 0;
        }

        if (!isNum.test(shipAmt)) {
            Runbow.TWS.Alert("请输入正确的运费");
            return;
        }

        if (!isNum.test(bafAmt)) {
            Runbow.TWS.Alert("请输入正确的燃油附加费");
            return;
        }

        if (!isNum.test(pointAmt)) {
            Runbow.TWS.Alert("请输入正确的点费");
            return;
        }

        if (!isNum.test(otherAmt)) {
            Runbow.TWS.Alert("请输入正确的其他费用");
            return;
        }

        $.send(
                '/Finance/Settlement/EditSettledPod',
                { id: id, shipAmt: shipAmt, bafAmt: bafAmt, pointAmt: pointAmt, otherAmt: otherAmt, remark: remark },
                function (response) {
                    Runbow.TWS.Alert(response);
                },
                function () {
                    Runbow.TWS.Alert("编辑运单结算失败！");
                });
    });

    $('#resultTable').find('.deleteSettledPod').live('click', function () {
        if (window.confirm('您确认删除此运单结算信息？')) {
            var podID = $(this).attr('data-id');
            var settledType = $('#SearchCondition_SettledType').val();
            var tr = $(this).parent().parent();
            $.send(
                '/Finance/Settlement/DeleteSettledPod',
                { id: podID, settledType: settledType },
                function (response) {
                    if (response.IsSuccess) {
                        tr.remove();
                    }
                    Runbow.TWS.Alert(response.Message);
                },
                function () {
                    Runbow.TWS.Alert("删除运单结算失败！");
                });
        }
    });

    $('#resultTable').find('.cashPay').live('click', function () {
        var podID = $(this).attr('data-id');
        var totalAmt = $(this).attr('data-TotalAmt');
        var customerOrderNumber = $(this).attr('data-CustomerOrderNumber');
        var tr = $(this).parent().parent();
        showDialog(podID, customerOrderNumber, totalAmt, tr);
    });

    $('#InvoiceButton').live('click', function () {
        var selectedIDs = $('#SelectedIDs').val();
        var settledType = $('#SearchCondition_SettledType').val();
        var customerOrShipperID = $('#SearchCondition_CustomerOrShipperID').val();
        if (customerOrShipperID === '') {
            if (settledType === '0') {
                Runbow.TWS.Alert('必须为同一家客户开票,请选择查询条件中客户,点击查询,再做开票操作');
                return;
            }
            else {
                Runbow.TWS.Alert('必须为同一家承运商开票,请选择查询条件中承运商,点击查询,再做开票操作');
                return;
            }
        }

        if (selectedIDs === '') {
            Runbow.TWS.Alert("请选择需要开票的运单");
        }

        window.location.href = "/Finance/Invoice/Invoice/?Type=" + settledType + "&CustomerOrShipperID=" + customerOrShipperID + "&IDs=" + selectedIDs;
    });

    setHiddenValToControl();

    $('#AuditButton').live('click', function () {
        var selectedIDs = $('#SelectedIDs').val();
        if (selectedIDs === '') {
            Runbow.TWS.Alert('请选择需要审核的运单');
            return;
        }

        var canFinalAudit = $('#FinalAudit').val();
        var type = $('#AuditType').val();
        showAuditDialog(type, canFinalAudit, selectedIDs);
    });

    var showAuditDialog = function (type, canFinalAudit, ids) {
        $('#AuditDate').val(new Date().Format('yyyy-MM-dd'));
        $('#AuditRemark').val('');
        var isManualSettled = $('#SearchCondition_IsManualSettled').val();
        var title;
        if (type === '1') {
            title = "短拨费审核";
        } else if (type === '2') {
            title = "配送费审核";
        } else if (type === '3') {
            title = "快递费审核";
        } else {
            title = "费用审核";
        }

        var buttons = {
            "同意": function () {
                $.send(
                        '/Finance/Settlement/AuditSettledPod',
                        { SettledPodIDs: ids, AuditDate: Runbow.TWS.Popup.find('#AuditDate').val(), AuditRemark: Runbow.TWS.Popup.find('#AuditRemark').val(), date: Runbow.TWS.Popup.find('#payDate').val(), AuditType: 1 },
                        function (response) {
                            if (response.IsSuccess) {
                                var trs = $('#resultTable tr');
                                $('#selectAll').removeAttr('checked');
                                for (var i = 1; i < trs.length; i++) {
                                    var tr = $(trs[i]);
                                    var check = tr.find('.checkForSelect');
                                    if (check.attr("checked") === "checked") {
                                        tr.remove();
                                    }
                                }
                                Runbow.TWS.Popup.close();
                                Runbow.TWS.Alert('审核同意成功');
                            }
                            else {
                                Runbow.TWS.Popup.close();
                                Runbow.TWS.Alert('审核同意失败');
                            }
                        },
                        function () {
                            Runbow.TWS.Popup.close();
                            Runbow.TWS.Alert('系统出错');
                        });
            },
            "不同意": function () {
                $.send(
                        '/Finance/Settlement/AuditSettledPod',
                        { SettledPodIDs: ids, AuditDate: Runbow.TWS.Popup.find('#AuditDate').val(), AuditRemark: Runbow.TWS.Popup.find('#AuditRemark').val(), date: Runbow.TWS.Popup.find('#payDate').val(), AuditType: 2 },
                        function (response) {
                            if (response.IsSuccess) {
                                var trs = $('#resultTable tr');
                                $('#selectAll').removeAttr('checked');
                                for (var i = 1; i < trs.length; i++) {
                                    var tr = $(trs[i]);
                                    var check = tr.find('.checkForSelect');
                                    if (check.attr("checked") === "checked") {
                                        tr.remove();
                                    }
                                }
                                Runbow.TWS.Popup.close();
                                Runbow.TWS.Alert('审核不同意成功');
                            }
                            else {
                                Runbow.TWS.Popup.close();
                                Runbow.TWS.Alert('审核不同意失败');
                            }
                        },
                        function () {
                            Runbow.TWS.Popup.close();
                            Runbow.TWS.Alert('系统出错');
                        });
            }
        };

        var width = '400'
        if (canFinalAudit === 'true') {
            $.extend(buttons, {
                "终审同意": function () {
                    $.send(
                       '/Finance/Settlement/AuditSettledPod',
                       { SettledPodIDs: ids, AuditDate: Runbow.TWS.Popup.find('#AuditDate').val(), AuditRemark: Runbow.TWS.Popup.find('#AuditRemark').val(), date: Runbow.TWS.Popup.find('#payDate').val(), AuditType: 3 },
                       function (response) {
                           if (response.IsSuccess) {
                               var trs = $('#resultTable tr');
                               $('#selectAll').removeAttr('checked');
                               for (var i = 1; i < trs.length; i++) {
                                   var tr = $(trs[i]);
                                   var check = tr.find('.checkForSelect');
                                   if (check.attr("checked") === "checked") {
                                       tr.remove();
                                   }
                               }
                               Runbow.TWS.Popup.close();
                               Runbow.TWS.Alert('终审同意成功');
                           }
                           else {
                               Runbow.TWS.Popup.close();
                               Runbow.TWS.Alert('终审同意失败');
                           }
                       },
                       function () {
                           Runbow.TWS.Popup.close();
                           Runbow.TWS.Alert('系统出错');
                       });
                },
                "终审不同意": function () {
                    $.send(
                       '/Finance/Settlement/AuditSettledPod',
                       { SettledPodIDs: ids, AuditDate: Runbow.TWS.Popup.find('#AuditDate').val(), AuditRemark: Runbow.TWS.Popup.find('#AuditRemark').val(), date: Runbow.TWS.Popup.find('#payDate').val(), AuditType: 4, isManualSettled: isManualSettled },
                       function (response) {
                           if (response.IsSuccess) {
                               var trs = $('#resultTable tr');
                               $('#selectAll').removeAttr('checked');
                               for (var i = 1; i < trs.length; i++) {
                                   var tr = $(trs[i]);
                                   var check = tr.find('.checkForSelect');
                                   if (check.attr("checked") === "checked") {
                                       tr.remove();
                                   }
                               }
                               Runbow.TWS.Popup.close();
                               Runbow.TWS.Alert('终审不同意成功');
                           }
                           else {
                               Runbow.TWS.Popup.close();
                               Runbow.TWS.Alert('终审不同意失败');
                           }
                       },
                       function () {
                           Runbow.TWS.Popup.close();
                           Runbow.TWS.Alert('系统出错');
                       });
                }
            });
            width = '600';
        }

        $.extend(buttons, {
            "取消": function () {
                alert('cancel');
                Runbow.TWS.Popup.close();
            }
        });

        var opts = {
            'title': title,
            'content': $('#showAuditDialog').clone().show(),
            'buttons': buttons,
            'width': width,
            'minHeight': '300'
        };

        Runbow.TWS.Popup.show(opts);
    };

    var showDialog = function (id, customerOrderNumber, amt, tr) {
        $('#AMT').val(amt);
        var opts = {
            'title': '现金支付',
            'content': $('#showInDialog').clone().show(),
            'buttons': {
                "确定": function () {
                    $.send(
                        '/Finance/Settlement/CashPayPod',
                        { id: id, customerOrderNumber: customerOrderNumber, totalAmt: Runbow.TWS.Popup.find('#AMT').val(), date: Runbow.TWS.Popup.find('#payDate').val(), remark: Runbow.TWS.Popup.find('#payRemark').val() },
                        function (response) {
                            if (response.IsSuccess) {
                                tr.remove();
                                Runbow.TWS.Popup.close();
                            }

                            Runbow.TWS.Alert(response.Message);
                        },
                        function () {
                            Runbow.TWS.Popup.close();
                        });
                },
                "取消": function () {
                    Runbow.TWS.Popup.close();
                }
            },
            'width': '400',//default 400
            'minHeight': '300' //default 300
        };

        Runbow.TWS.Popup.show(opts);
    };
    
    $('#ExportButton').click(function () {
        $('#IsExport').val('True');
    });
    $('#searchButton').click(function () {
        $('#IsExport').val('False');
    });

});