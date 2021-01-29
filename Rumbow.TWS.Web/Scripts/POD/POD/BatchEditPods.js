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

    //else if (treeId === 'startPlace') {
    //    $('#startPlaceID').val($('#startPlaceTreeID').attr('value'));
    //    $('#startPlaceName').val($('#startPlaceTreeName').attr('value'));

    //} else if (treeId === 'endPlace') {
    //    $('#endPlaceID').val($('#endPlaceTreeID').attr('value'));
    //    $('#endPlaceName').val($('#endPlaceTreeName').attr('value'));
    //}
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

    //else if (globalID === 'startPlace') {
    //    $('#startPlaceID').val($('#startPlaceTreeID').attr('value'));
    //    $('#startPlaceName').val($('#startPlaceTreeName').attr('value'));

    //} else if (globalID === 'endPlace') {
    //    $('#endPlaceID').val($('#endPlaceTreeID').attr('value'));
    //    $('#endPlaceName').val($('#endPlaceTreeName').attr('value'));
    //}
}

$(document).ready(function () {

    $('#SearchCondition_ShipperName').autocomplete({
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
            $('#SearchCondition_ShipperID').val(ui.item.data.Value);
            $('#SearchCondition_ShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#SearchCondition_ShipperID').val('');
        }
    });


    $('#allocateShipperAutoComplete').autocomplete({
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
            $('#allocateShipperID').val(ui.item.data.Value);
            $('#allocateShipperAutoComplete').val(ui.item.data.Text);
            $('#allocatePodShipperManually').removeAttr('disabled');
        }
    });

    var setHiddenValToControl = function () {
        $('#startCityTreeID').val($('#SearchCondition_StartCities').val());
        $('#startCityTreeName').val($('#SearchCondition_StartCityName').val());
        $('#endCityTreeID').val($('#SearchCondition_EndCities').val());
        $('#endCityTreeName').val($('#SearchCondition_EndCityName').val());
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

        $('.DropDownList').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id.substr(0, id.length - 4) + "ID";
            $(this).val($('#' + descId).val());
        });

        $('select[id=SearchCondition_CustomerID]').live('change', function () {
            var id = $('#SearchCondition_PODStateID').val();
            if (id === '') {
                id = 0;
            }
            var hiddenActionButton = $('#HideActionButton').val();
            var isSplitPod = $('#IsSplit').val();
            var showEditRelated = $('#ShowEditRelated').val();
            var isAllocateShipper = $('#IsAllocateShipper').val();
            var isSettled = $('#IsSettled').val();
            var settledType = $('#SettltedType').val();
            var isUsedForOriginalPOD = $('#IsUsedForOriginalPOD').val();
            var customerID = $(this).val();
            var extenFeeType;
            var isUsedForSendForecast = $('#IsUsedForSendForecast').val();
            var isWeixinStatus = $('#IsWenXinStatus').val();
            var isReturnPodStatus = $("#IsReturnPodStatus").val();
            if ($('#IsExternFee').val() === 'True') {
                extenFeeType = $('#ExternFeeType').val();
            }

            var manualSettledType;
            if ($('#IsManualSettled').val() === 'True') {
                manualSettledType = $('#ManualSettledType').val();
            }
            var IsPODDistributionVehicle = $('#IsPODDistributionVehicle').val();
            var WaybillReach = $('#WaybillReach').val();

            window.location.href = "/POD/POD/BatchEditPods/" + id + "?hideActionButton=" + hiddenActionButton + "&isSplitPod=" + isSplitPod + "&showEditRelated=" + showEditRelated
            + "&isAllocateShipper=" + isAllocateShipper + "&isSettled=" + isSettled + "&settledType=" + settledType + "&isUsedForOriginalPOD=" + isUsedForOriginalPOD + "&customerID=" + customerID + "&extenFeeType=" + extenFeeType + "&manualSettledType=" + manualSettledType + "&isUsedForSendForecast=" + isUsedForSendForecast + "&IsWenXinStatus=" + isWeixinStatus
            + "&IsPODDistributionVehicle=" + IsPODDistributionVehicle + "&WaybillReach=" + WaybillReach + "&IsReturnPodStatus=" + isReturnPodStatus;
        });

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
            } else if ($(this).attr("type") === "DropDownList") {
                var desc = $('#' + descId);
                if (desc.val() === '1' || desc.val() === 'Y' || desc.val === 'y' || desc.val() === '是') {
                    $(this).val('1');
                } else {
                    $(this).val('0');
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

    $('#startPlaceClear').live('click', function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#startPlaceID').val('');
        $('#startPlaceName').val('');
    });

    $('#endPlaceClear').live('click', function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#endPlaceID').val('');
        $('#endPlaceName').val('');
    });

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }
        //var chkother= document.getElementsByTagName("input");
        //for (var i = 0; i < chkother[i].length; i++) {
        //    if (chkother[i].name != "selectAll") {
        //        if (chkother[i].type == "checkbox") {
        //            if ($(this).attr("checked") === "checked") {
        //                chkother[i].checked = true;
        //            }
        //            else {
        //                chkother[i].checked = false;
        //            }
        //        }
        //    }
        //}
        RefreshIDs();
    });

    $("#resultTable tbody input[type='checkbox']").live('click', function () {
        RefreshIDs();
    });

    $('#SendForecastEmail').live('click', function () {
        if ($('#SearchCondition_CustomerID').val() === '') {
            Runbow.TWS.Alert("为提高效率,一次只能对同一客户运单发送预报,请选择客户并点击查询");
            $('#resultTable tbody tr').remove();
            return;
        }

        var ids = $('#SelectedIDs').val();
        if (ids === '') {
            Runbow.TWS.Alert('请选择运单');
            return;
        }

        $.send(
        '/POD/POD/SendForecastEmail',
        { CustomerID: $('#SearchCondition_CustomerID').val(), ids: ids },
        function (response) {
            Runbow.TWS.Alert(response.Message);
        },
        function () {
            Runbow.TWS.Alert('预报邮件发送失败！');
        });
    });

    $('#conformButton').click(function () {
        var ids = $('#SelectedIDs').val();
        var name = $(this).val();
        if (ids === '') {
            Runbow.TWS.Alert('请选择运单');
            return;
        }
        var targetStatus = $(this).attr('data-id');
        var targetStatusName = $(this).attr('data-name');
        $.send(
        '/POD/POD/BatchEditPodStatusAsync',
        { ids: ids, podStateID: targetStatus, podStateName: targetStatusName },
        function (response) {
            for (var i = 0; i < response.length; i++) {
                var td = $('#resultTable tr[data-id="' + response[i].ID + '"] td:eq(5)');
                if (td) {
                    td.html(targetStatusName);
                }
            }
            Runbow.TWS.Alert("运单" + name + "成功");
        },
        function () {
            Runbow.TWS.Alert("运单" + name + "失败！");
        });

    });

    //微信二维码
    $('#allocatePodWenXin').click(function () {
        var ids = $('#SelectedIDs').val();
        var name = $(this).val();
        if (ids === '') {
            Runbow.TWS.Alert('请选择运单wenxin');
            return;
        }
        $.send(
        '/POD/POD/BatchEditPodWenXin',
        { ids: ids },
        function (response) {
            //for (var i = 0; i < response.length; i++) {
            //    var td = $('#resultTable tr[data-id="' + response[i].ID + '"] td:eq(3)');
            //    if (td) {
            //        td.html(targetStatusName);
            //    }
            //}
            Runbow.TWS.Alert("运单" + name + "成功");
        },
        function () {
            Runbow.TWS.Alert("运单" + name + "失败！");
        });

    });

    var RefreshIDs = function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var length = checkBoxs.length;
        var IDs = [];
        var checked = 0;
        checkBoxs.each(function () {
            if ($(this).attr("checked") === "checked") {
                var id = { ID: $(this).attr("data-ID") };
                IDs.push(id);
                checked++;
            }
        });

        if (checked == checkBoxs.length) {
            $('#selectAll').attr("checked", "checked");
        } else {
            $('#selectAll').removeAttr("checked");
        }

        $('#SelectedIDs').val(JSON.stringify(IDs));
    }

    $('#resultTable').find('.splitPod').live('click', function () {
        var podID = $(this).attr('data-id');
        var splitNumber = $(this).prev().val();
        if (splitNumber === '') {
            Runbow.TWS.Alert('请输入要拆分的运单数');
            return;
        }

        if (isNaN(splitNumber)) {
            Runbow.TWS.Alert('请输入数字');
            return;
        }

        if (window.confirm('您确认将此运单拆分成' + splitNumber + '个')) {
            var tr = $(this).parent().parent();
            $.send(
                '/POD/POD/SplitPod',
                { id: podID, splitNumber: splitNumber },
                function (response) {
                    if (response.IsSuccess) {
                        tr.remove();
                        Runbow.TWS.Alert(response.Message);
                    }
                },
                function () {
                    Runbow.TWS.Alert("拆分运单失败！");
                });
        }
    });

    $('#allocateShipperAutoComplete').live('change', function () {
        if ($(this).val() === '') {
            $('#allocateShipperID').val('');
            $('#allocatePodShipperManually').attr('disabled', 'disabled');
        } else {
            $('#allocatePodShipperManually').removeAttr('disabled');
        }
    });

    $('#allocatePodShipperManually').live('click', function () {
        var ids = $('#SelectedIDs').val();
        if (ids === '') {
            Runbow.TWS.Alert('请选择需要分配的运单');
            return;
        }

        var shipperID = $('#allocateShipperID').val();
        var shipperName = $('#allocateShipperAutoComplete').val();
        if (shipperID === '') {
            Runbow.TWS.Alert('请选择正确承运商');
            return;
        }
        //var shipperID = $('#allocateShiperDrop option:selected').val();
        //var shipperName = $('#allocateShiperDrop option:selected').text();
        $.send(
        '/POD/POD/AllocatePodShipperManuallyAsync',
        { ids: ids, shipperID: shipperID, shipperName: shipperName },
        function (response) {
            for (var i = 0; i < response.length; i++) {
                var td = $('#resultTable tr[data-id="' + response[i].ID + '"] td:eq(4)');
                if (td) {
                    td.html(response[i].Description);
                }
            }
        },
        function () {
            Runbow.TWS.Alert("运单分配承运商失败！");
        });
    });

    $('#allocatePodShipper').click(function () {
        var ids = $('#SelectedIDs').val();
        if (ids === '') {
            Runbow.TWS.Alert('请选择需要分配的运单');
            return;
        }

        $.send(
        '/POD/POD/AllocatePodShipperAsync',
        { ids: ids },
        function (response) {
            for (var i = 0; i < response.length; i++) {
                var td = $('#resultTable tr[data-id="' + response[i].ID + '"] td:eq(4)');
                if (td) {
                    td.html((response[i].Description === '' || response[i].Description === 'Null') ? '<font color="red">无对应承运商<br/>请手动分配</font>' : response[i].Description);
                }
            }
        },
        function () {
            Runbow.TWS.Alert("运单分配承运商失败！");
        });

    });

    $('#BackStatus').click(function () {
        var ids = $('#SelectedIDs').val();
        var name = $(this).val();
        if (ids === '') {
            Runbow.TWS.Alert('请选择需要回退的运单');
            return;
        }

        $.send(
        '/POD/POD/BackPodStatus',
        { ids: ids, targetStatusID: $('#ReturnPodStatusID').val() },
        function (response) {
            for (var i = 0; i < response.ID.length; i++) {
                var td = $('#resultTable tr[data-id="' + response.ID[i] + '"] td:eq(5)');
                if (td) {
                    td.html('<font color="red">' + response.StatusName + '</font>');
                }
            }
            Runbow.TWS.Alert("运单回退成功");
        },
        function () {
            Runbow.TWS.Alert("运单回退失败！");
        });

    });

    $('#SettledPod').live('click', function () {
        var ids = $('#SelectedIDs').val();
        if (ids === '') {
            Runbow.TWS.Alert('请选择需要结算的运单');
            return false;
        }

        $('#SelectedSettledPodIDs').val(ids);
    });

    $('#resultTable').find('.deletePod').live('click', function () {
        if (window.confirm('您确认删除订单？')) {
            var podID = $(this).attr('data-id');
            var tr = $(this).parent().parent();
            $.send(
                '/POD/POD/DeletePOD',
                { id: podID },
                function (response) {
                    if (response.IsSuccess) {
                        tr.remove();

                    }
                    Runbow.TWS.Alert(response.Message);
                },
                function () {
                    Runbow.TWS.Alert("删除运单失败！");
                });
        }
    });

    setHiddenValToControl();

    $('#ManualSettledForShipper').live('click', function () {
        var ids = $('#SelectedIDs').val();
        var title;
        var alertMessage;
        if ($('#ManualSettledType').val() == 1) {
            title = "手动结算承运商费用";
            alertMessage = "请选择需要结算的运单";
        } else {
            return;
        }

        if (ids === '') {
            Runbow.TWS.Alert(alertMessage);
            return;
        }

        showManualSettledShipperDialog(ids, title);
        $('#manualSettledShipper').autocomplete({
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
                $('#manualSettledShipperID').val(ui.item.data.Value);
                $('#manualSettledShipper').val(ui.item.data.Text);
            }
        });

    });

    $('#genExternFee').live('click', function () {
        var ids = $('#SelectedIDs').val();
        var title;
        var alertMessage;
        if ($('#ExternFeeType').val() === '1') {
            title = "生成短拨费用";
            alertMessage = "请选择需要短拨的运单";
        } else if ($('#ExternFeeType').val() === '2') {
            title = "生成配送费用";
            alertMessage = "请选择需要配送的运单";
        } else if ($('#ExternFeeType').val() === '3') {
            title = "生成快递费用";
            alertMessage = "请选择需要快递的运单";
        }

        if (ids === '') {
            Runbow.TWS.Alert(alertMessage);
            return;
        }

        var type = $(this).attr('data-Type');

        showDialog(ids, title, type);

        $('#externFeeShipper').autocomplete({
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
                $('#externFeeShipperID').val(ui.item.data.Value);
                $('#externFeeShipper').val(ui.item.data.Text);
            }
        });
    });

    var showManualSettledShipperDialog = function (ids, title) {
        var showDialog = $('#ManualSettledForShipperDialog').clone();
        var opts = {
            'title': title,
            'content': $('#ManualSettledForShipperDialog').show(),
            'buttons': {
                "确定": function () {
                    var shipperID = $('#manualSettledShipperID').val(), shipperName = $('#manualSettledShipper').val();
                    if (shipperID === '') {
                        alert('请输入承运商');
                        return;
                    }

                    var manualSettledShipperDate = $('#manualSettledShipperDate').val();
                    if (manualSettledShipperDate === '') {
                        alert('请输入结算日期');
                        return;
                    }

                    var manualSettledShipperFee = $('#manualSettledShipperFee').val();
                    if (manualSettledShipperFee === '') {
                        alert('请录入结算费用');
                        return;
                    }

                    var isNum = /^\d+(\.\d+)?$/;
                    if (!isNum.test(manualSettledShipperFee)) {
                        alert("请输入正确的结算费用");
                        return;
                    }
                    var apportionType = $('#apportionType').val();
                    var remark = $('#manualSettledShipperRemark').val();

                    $.send(
                        '/POD/Pod/ManualSettledPod',
                        {
                            ids: ids, shipperID: shipperID, shipperName: shipperName, settledDate: manualSettledShipperDate,
                            fee: manualSettledShipperFee, remark: remark, apportionType: apportionType
                        },
                        function (response) {
                            if (response.IsSuccess) {
                                $('#manualSettledShipper').val('');
                                $('#manualSettledShipperID').val('');
                                $('#manualSettledShipperDate').val('');
                                $('#manualSettledShipperFee').val('');
                                $('#apportionType').val('1');
                                $('#manualSettledShipperRemark').val('');
                                $('#outDialog').append(showDialog);
                                Runbow.TWS.Popup.close();
                                $('#SelectedIDs').val('');
                                var trs = $('#resultTable tr');
                                $('#selectAll').removeAttr('checked');
                                for (var i = 1; i < trs.length; i++) {
                                    var tr = $(trs[i]);
                                    var check = tr.find('.checkForSelect');
                                    if (check.attr("checked") === "checked") {
                                        tr.remove();
                                    }
                                }
                                Runbow.TWS.Alert("运单手动结算应付运费成功");
                            }
                        },
                        function (exception) {
                            $('#manualSettledShipper').val('');
                            $('#manualSettledShipperID').val('');
                            $('#manualSettledShipperDate').val('');
                            $('#manualSettledShipperFee').val('');
                            $('#apportionType').val('1');
                            $('#manualSettledShipperRemark').val('');
                            $('#outDialog').append(showDialog);
                            Runbow.TWS.Popup.close();
                            Runbow.TWS.Alert("程序出错");
                        });
                },
                "取消": function () {
                    $('#manualSettledShipper').val('');
                    $('#manualSettledShipperID').val('');
                    $('#manualSettledShipperDate').val('');
                    $('#manualSettledShipperFee').val('');
                    $('#apportionType').val('1');
                    $('#manualSettledShipperRemark').val('');
                    $('#outDialog').append(showDialog);
                    Runbow.TWS.Popup.close();
                }
            },
            'width': '500',//default 400
            'minHeight': '300' //default 300
        };

        Runbow.TWS.Popup.show(opts);
    }

    var showDialog = function (ids, title, type) {

        var showDialog = $('#showInDialog').clone();
        var opts = {
            'title': title,
            'content': $('#showInDialog').show(),
            'buttons': {
                "确定": function () {
                    var shipperID = $('#externFeeShipperID').val(), shipperName = $('#externFeeShipper').val();
                    if (shipperID === '') {
                        alert('请输入承运商');
                        return;
                    }

                    var shipperDate = $('#externFeeDate').val();
                    if (shipperDate === '') {
                        alert('请输入日期');
                        return;
                    }

                    var startPlaceID = $('#startPlaceTreeID').val(), startPlaceName = $('#startPlaceTreeName').val();
                    if (startPlaceID === '') {
                        alert('请录入起运地');
                        return;
                    }

                    var endPlaceID = $('#endPlaceTreeID').val(), endPlaceName = $('#endPlaceTreeName').val();
                    if (endPlaceID === '') {
                        alert('请录入目的地地');
                        return;
                    }

                    var fee = $('#externFee').val();
                    if (fee === '') {
                        alert('请录入费用');
                        return;
                    }

                    var isNum = /^\d+(\.\d+)?$/;
                    if (!isNum.test(fee)) {
                        alert("请输入正确的费用");
                        return;
                    }

                    var remark = $('#externFeeRemark').val();

                    $.send(
                        '/POD/Pod/ExternFeePod',
                        {
                            ids: ids, type: type, shipperID: shipperID, shipperName: shipperName,
                            shipperDate: shipperDate, startPlaceID: startPlaceID, startPlaceName: startPlaceName,
                            endPlaceID: endPlaceID, endPlaceName: endPlaceName, fee: fee, remark: remark, settledType: $('#extenFeeSettledType').val()
                        },
                        function (response) {
                            if (response.IsSuccess) {
                                $('#externFeeShipper').val('');
                                $('#externFeeShipperID').val('');
                                $('#startPlaceTreeName').val('');
                                $('#startPlaceTreeID').val('');
                                $('#endPlaceTreeName').val('');
                                $('#endPlaceTreeID').val('');
                                $('#externFee').val('');
                                $('#externFeeRemark').val('');
                                $('#dialog').append(showDialog);
                                Runbow.TWS.Popup.close();
                                $('#SelectedIDs').val('');
                                var trs = $('#resultTable tr');
                                $('#selectAll').removeAttr('checked');
                                for (var i = 1; i < trs.length; i++) {
                                    var tr = $(trs[i]);
                                    var check = tr.find('.checkForSelect');
                                    if (check.attr("checked") === "checked") {
                                        tr.remove();
                                    }
                                }
                                Runbow.TWS.Alert("费用增加成功");
                            }
                        },
                        function (exception) {
                            $('#externFeeShipper').val('');
                            $('#externFeeShipperID').val('');
                            $('#startPlaceTreeName').val('');
                            $('#startPlaceTreeID').val('');
                            $('#endPlaceTreeName').val('');
                            $('#endPlaceTreeID').val('');
                            $('#externFee').val('');
                            $('#externFeeRemark').val('');
                            $('#dialog').append(showDialog);
                            Runbow.TWS.Popup.close();
                            Runbow.TWS.Alert("程序出错");
                        });
                },
                "取消": function () {
                    $('#externFeeShipper').val('');
                    $('#externFeeShipperID').val('');
                    $('#startPlaceTreeName').val('');
                    $('#startPlaceTreeID').val('');
                    $('#endPlaceTreeName').val('');
                    $('#endPlaceTreeID').val('');
                    $('#externFee').val('');
                    $('#externFeeRemark').val('');
                    $('#dialog').append(showDialog);
                    Runbow.TWS.Popup.close();
                }
            },
            'width': '500',//default 400
            'minHeight': '300' //default 300
        };

        Runbow.TWS.Popup.show(opts);
    };
    $("#IsPODDistributionVehicles").click(function () {


        //openPopup("", true, 400, 400, null, 'DisInfo', true);
        openPopup("", true, 400, 350, null, "Evaluation", true);
        //openPopup("", true, 500, 100, '/POD/POD/PODDistributionVehicle/?ids=' + ids, null, function (ids) {
        //    location.href = "/POD/POD/BatchEditPods/0?IsPODDistributionVehicle=true"
        //});
        //BatchEditPods
        //openPopup("", true, 1000, 600, '/WMS/ReceiptManagement/ASNQuery/?CustomerID=' + CustomerID, null, function (ASNID, CustomerID) {
        //    //$('#ASNNumber').val(box.ASNNumbers);
        //    //$('#ASNID').val(ASNID);
        //    //$('#ExternReceiptNumber').val(box.ExternReceiptNumbers);
        //    //$('#CustomerName').val(box.CustomerNames);
        //    //$('#CustomerID').val(CustomerID);
        //    //$('#ASNType').val(box.ASNTypes);
        //    location.href = "/WMS/ReceiptManagement/ReceiptCreate/?ID=" + ASNID + "&ViewType=1"

        //});
    });
    $('select[id=SearchCondition_podDistributionVehicle_PODType]').live('change', function () {
        if (this.value == "干线车辆" || this.value == "提货车辆") {
            $("#Hub")[0].style.display = '';
        } else {
            $("#Hub")[0].style.display = 'none';
        }
    });
    $("#WaybillReachs").click(function () {

        //openPopup("", true, 400, 400, null, 'DisInfo', true);
        openPopup("", true, 400, 200, null, "WaybillReachPopup", true);
        //openPopup("", true, 500, 100, '/POD/POD/PODDistributionVehicle/?ids=' + ids, null, function (ids) {
        //    location.href = "/POD/POD/BatchEditPods/0?IsPODDistributionVehicle=true"
        //});
        //BatchEditPods
        //openPopup("", true, 1000, 600, '/WMS/ReceiptManagement/ASNQuery/?CustomerID=' + CustomerID, null, function (ASNID, CustomerID) {
        //    //$('#ASNNumber').val(box.ASNNumbers);
        //    //$('#ASNID').val(ASNID);
        //    //$('#ExternReceiptNumber').val(box.ExternReceiptNumbers);
        //    //$('#CustomerName').val(box.CustomerNames);
        //    //$('#CustomerID').val(CustomerID);
        //    //$('#ASNType').val(box.ASNTypes);
        //    location.href = "/WMS/ReceiptManagement/ReceiptCreate/?ID=" + ASNID + "&ViewType=1"

        //});
    });
    $("#confirm").click(function () {
        var ids = $('#SelectedIDs').val();
        var name = $(this).val();
        if (ids === '') {
            showMsg('请选择运单', "4000");
            return;
        }
        var CarNo = $("#SearchCondition_podDistributionVehicle_CarNo").val();
        var DriverName = $("#SearchCondition_podDistributionVehicle_DriverName").val();
        var DriverPhone = $("#SearchCondition_podDistributionVehicle_DriverPhone").val();
        var PODType = $("#PODType").val();
        var StartTime = $("#StartTime").val();
        if (CarNo == ""  || StartTime == "") {
            showMsg("请填写完整信息！","4000");
            return false;
        }
        $.send(
            '/POD/Pod/PODDistributionVehicle',
            {
                ids: ids,
                CarNo: CarNo,
                DriverName: DriverName,
                DriverPhone: DriverPhone,
                PODType: PODType,
                StartTime: StartTime
            },
            function (response) {
                if (response.Code == 1) {
                    closePopup();
                    showMsg('操作成功！', 4000);
                   
                } else {
                    closePopup();
                    showMsg('操作失败！', 4000);
                   
                }
            });


    });
    $("#cancelPapup").click(function () {
        closePopup();
    })
    $("#SearchCondition_podDistributionVehicle_CarNo").autocomplete({
        source: function (request, response) {

            $.ajax({
                url: "/POD/POD/GetCarInfo",
                type: "POST",
                dataType: "json",
                data: {
                    CarNo: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
        select: function (event, ui) {
            //$('.goodstype' + LineNumber).val(ui.item.data.GoodsType);  
            $("#SearchCondition_podDistributionVehicle_CarNo")[0].value = ui.item.data.value;
            $("#SearchCondition_podDistributionVehicle_DriverName")[0].value = ui.item.data.Name;
            $("#SearchCondition_podDistributionVehicle_DriverPhone")[0].value = ui.item.data.Phone;
        }
    })
    
    $("#PODconfirm").click(function () {
        var ids = $('#SelectedIDs').val();
        var name = $(this).val();
        if (ids === '') {
            showMsg('请选择运单', 4000);
            return false;
        }
        var EndTime = $("#EndTime").val();
        var PODType = $("select[id=SearchCondition_podDistributionVehicle_PODType]").val();
        var Hub = $("#SearchCondition_podDistributionVehicle_Hub").val();
        if (EndTime == "") {
            showMsg("请填写完整信息！", "4000");
            return false;
        }
        $.send(
            '/POD/Pod/WaybillReach',
            {
                ids: ids,
                EndTime: EndTime,
                PODType: PODType,
                Hub: Hub
            },
            function (response) {
                if (response.Code == 1) {
                    closePopup();
                    showMsg('操作成功！', 4000);
                  
                } else {
                    closePopup();
                    showMsg('操作失败！', 4000);
                 
                }
            });


    });
});