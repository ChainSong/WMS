
$(document).ready(function () {
    //$("#accordhidden").addClass("yc");
    $("#ahidden").attr("value", "-")
    $("#ahidden").click(function () {
    if ($(".yc").css("display") == "none") {
                $("#accordhidden").removeClass("yc");
                $("#accordhidden").addClass("xs");
                $("#ahidden").attr("value", "-")
            }
            else {
                $("#accordhidden").removeClass("xs");
                $("#accordhidden").addClass("yc");
                $("#ahidden").attr("value", "+")
            }
    });
    $('#distable').find('#carModels').live('change', function () {
        var selectedIndex = Runbow.TWS.Popup.find('#carModels').val();
        var selectedIDs = $('#SelectedIDs').val();
        var scoressum = 0;
        var startfee = 0;
        var podcount = selectedIDs.split(",").length;
        if (selectedIndex!=0) {
            $.ajax({
                type: "post",
                url: "/POD/Distribution/AuditPod",
                data: "PodIDs=" + selectedIDs + "&" + "carModels=" + selectedIndex + "&" + "type=" + '1' + "&", //要传的值 
                success: function (data) {
                    if (data) {
                        if (selectedIndex == 3) {
                            Runbow.TWS.Popup.find('#sumWeight').val(data)
                            if (parseFloat(data) < 10000) {
                                Runbow.TWS.Popup.find('#amountTo').val(10000 * 0.07);
                            }
                            else { Runbow.TWS.Popup.find('#amountTo').val(parseFloat(data) * 0.07); }
                            
                        }
                        else {
                            if (selectedIndex==1) {
                                scoressum = parseFloat(data - 1) * 30;
                                Runbow.TWS.Popup.find('#startFee').val(260);
                                startfee = 260;
                            }
                            else if (selectedIndex==2) {
                                scoressum = parseFloat(data - 1) * 50;
                                Runbow.TWS.Popup.find('#startFee').val(350);
                                startfee = 350;
                            }
                            Runbow.TWS.Popup.find('#pointCharges').val(scoressum)
                            Runbow.TWS.Popup.find('#scores').val(parseFloat(data));
                            Runbow.TWS.Popup.find('#amountTo').val(scoressum + startfee);
                            Runbow.TWS.Popup.find('#total').val(parseFloat(scoressum + startfee) / parseFloat(podcount));
                            
                        }
                        
                    }
                    else {
                        alert("您所选择的的运单重量均为空");
                    }
                }
            });
        }
        
        if (selectedIndex==1||selectedIndex == 2) {
            Runbow.TWS.Popup.find('#sumWeight').val("")
        }
        else if (selectedIndex==3) {
            Runbow.TWS.Popup.find('#pointCharges').val("")
            Runbow.TWS.Popup.find('#scores').val("");
            Runbow.TWS.Popup.find('#total').val("");
            Runbow.TWS.Popup.find('#startFee').val("");
        }
        else {
            Runbow.TWS.Popup.find('#pointCharges').val("")
            Runbow.TWS.Popup.find('#scores').val("");
            Runbow.TWS.Popup.find('#total').val("");
            Runbow.TWS.Popup.find('#startFee').val("");
            Runbow.TWS.Popup.find('#sumWeight').val("")
            Runbow.TWS.Popup.find('#amountTo').val("");
        }
  
    })
    var keyup = function () {
        var selectedIDs = $('#SelectedIDs').val();
        var sumWeight = Runbow.TWS.Popup.find('#sumWeight').val();
        var deliveryFeeFactory = Runbow.TWS.Popup.find('#deliveryFeeFactory').val();
        var deliveryFeeJiatuo = Runbow.TWS.Popup.find('#deliveryFeeJiatuo').val();
        var unloadingCosts = Runbow.TWS.Popup.find('#unloadingCosts').val();
        var startFee = Runbow.TWS.Popup.find('#startFee').val();
        var fuelCosts = Runbow.TWS.Popup.find('#fuelCosts').val();
        var packagesFare = Runbow.TWS.Popup.find('#packagesFare').val();
        var scores = Runbow.TWS.Popup.find('#scores').val();
        var pointCharges = Runbow.TWS.Popup.find('#pointCharges').val();
        var permitFees = Runbow.TWS.Popup.find('#permitFees').val();
        var otherCosts = Runbow.TWS.Popup.find('#otherCosts').val();
        var carModels = Runbow.TWS.Popup.find('#carModels').val();
        var idcount = selectedIDs.split(",").length;
        var sum = 0;
        if (carModels != 0) {
            if (packagesFare != "") {
                //if (carModels==1||carModels==2) {
                //    Runbow.TWS.Popup.find('#startFee').val("");
                //    sum += parseFloat(pointCharges);
                //}
                sum += parseFloat(packagesFare);
                Runbow.TWS.Popup.find('#pointCharges').val("");
                Runbow.TWS.Popup.find('#scores').val("");
                Runbow.TWS.Popup.find('#startFee').val("");
                Runbow.TWS.Popup.find('#sumWeight').val("")
                
            }
            //else {
                if (deliveryFeeFactory != "") {
                    sum = parseFloat(deliveryFeeFactory);
                }
                if (deliveryFeeJiatuo != "") {
                    sum += parseFloat(deliveryFeeJiatuo);
                }
                if (unloadingCosts != "") {
                    sum += parseFloat(unloadingCosts);
                }
                if (startFee != "") {
                    sum += parseFloat(startFee);
                }
                if (fuelCosts != "") {
                    sum += parseFloat(fuelCosts);
                }
                if (pointCharges != "") {
                    sum += parseFloat(pointCharges);
                }
                if (permitFees != "") {
                    sum += parseFloat(permitFees);
                }
                if (otherCosts != "") {
                    sum += parseFloat(otherCosts);
                }
                if (sumWeight != "" || sumWeight != 0) {
                    if (parseFloat(sumWeight)<10000) {
                        sum += 10000 * 70 / 1000;
                    }
                    else {
                        sum += sumWeight * 70 / 1000;
                    }
                   
                } 
            //}
            Runbow.TWS.Popup.find('#amountTo').val(sum);
            if (carModels != 3) {
                Runbow.TWS.Popup.find('#total').val(parseFloat(sum) / parseFloat(idcount));
            }
            
        }
    
    }

    $('#distable').find('#deliveryFeeFactory').live('keyup', keyup);
    $('#distable').find('#deliveryFeeJiatuo').live('keyup', keyup);
    $('#distable').find('#unloadingCosts').live('keyup', keyup);
    $('#distable').find('#startFee').live('keyup', keyup);
    $('#distable').find('#fuelCosts').live('keyup', keyup);
    $('#distable').find('#packagesFare').live('keyup', keyup);
    $('#distable').find('#permitFees').live('keyup', keyup);
    $('#distable').find('#otherCosts').live('keyup', keyup);
    $('#distable').find('#pointCharges').live('keyup', keyup);


    $('#total').attr("readOnly", true);
    $('#total').attr("disabled", "true");
    $('#amountTo').attr("readOnly", true);
    $('#amountTo').attr("disabled", "true");
    $('#total').css({ "background-color": "#999" });
    $('#amountTo').css({ "background-color": "#999" });

    $('#ExportButton').click(function () {
        $('#IsDaoChu').val('True');
        if ($('#IsForQuery').val()=='false') {
            RefreshIDs();
            var selectedID = $('#SelectedIDs').val();
            if (selectedID === '') {
                Runbow.TWS.Alert('请选择需要导出运单');
                return false;
            }
        }
    });
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
        $("#accordhidden").addClass("xs");
        $('#IsDaoChu').val('false');
        
    });

   
    $('#resultTable').find('.deletePod').live('click', function () {
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
    });

    $('#cancelSettlement').live('click', function () {
        RefreshIDs();
        var podID = $('#SelectedIDs').val();
        if (podID === '') {
            Runbow.TWS.Alert('请选择需要取消的运单');
            return false;
        }
        $.ajax({
            type: "post",
            url: "/POD/Distribution/deletePodFee",
            data: "podID="+podID, //要传的值 
            success: function (data) {
                if (data) {
                    alert("取消成功");
                    var trs = $('#resultTable tr');
                    $('#selectAll').removeAttr('checked');
                    for (var i = 1; i < trs.length; i++) {
                        var tr = $(trs[i]);
                        var check = tr.find('.checkForSelect');
                        if (check.attr("checked") === "checked") {
                            tr.remove();
                        }
                    }
                }
                else {
                    alert("取消失败");
                }
            }

        });
    })

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
    $('#citysettlement').live('click', function () {
        RefreshIDs();
        var selectedIDs = $('#SelectedIDs').val();
        if (selectedIDs === '') {
            Runbow.TWS.Alert('请选择需要配送及结算的运单');
            return;
        }
        $.ajax({
            type: "post",
            url: "/POD/Distribution/selectPodFee",
            data: "" , //要传的值 
            success: function (data) {
                if (data) {
                    Runbow.TWS.Popup.find('#startBatchNumber').val(data);
                }
                else {
                   alert("失败");
                }
            }
        });
        showAuditDialog(selectedIDs);
    });

});


var showAuditDialog = function (ids) {
    var title = "车辆分配及结算";
    var buttons = {
        " 确定": function () {
            var startBatchNumber = Runbow.TWS.Popup.find('#startBatchNumber').val();
            var aarriers = Runbow.TWS.Popup.find('#aarriers').val();
            var carNumber = Runbow.TWS.Popup.find('#carNumber').val();
            var sumWeight = Runbow.TWS.Popup.find('#sumWeight').val();
            var deliveryFeeFactory = Runbow.TWS.Popup.find('#deliveryFeeFactory').val();

            var deliveryFeeJiatuo = Runbow.TWS.Popup.find('#deliveryFeeJiatuo').val();
            var unloadingCosts = Runbow.TWS.Popup.find('#unloadingCosts').val();
            var startFee = Runbow.TWS.Popup.find('#startFee').val();
            var fuelCosts = Runbow.TWS.Popup.find('#fuelCosts').val();
            var packagesFare = Runbow.TWS.Popup.find('#packagesFare').val();

            var scores = Runbow.TWS.Popup.find('#scores').val();
            var pointCharges = Runbow.TWS.Popup.find('#pointCharges').val();
            var permitFees = Runbow.TWS.Popup.find('#permitFees').val();
            var otherCosts = Runbow.TWS.Popup.find('#otherCosts').val();
            var total = Runbow.TWS.Popup.find('#total').val();
            var amountTo = Runbow.TWS.Popup.find('#amountTo').val();
            var carModels = Runbow.TWS.Popup.find('#carModels').val();
            var remarks = Runbow.TWS.Popup.find('#remarks').val();
            $.ajax({
                type: "post",
                url: "/POD/Distribution/AuditPod",
                data: "PodIDs=" + ids + "&" + "startBatchNumber=" + startBatchNumber + "&" + "aarriers="+aarriers+"&"+
                    "carNumber=" + carNumber + "&" + "sumWeight=" + sumWeight + "&" + "deliveryFeeFactory=" + deliveryFeeFactory + "&" +
                    "deliveryFeeJiatuo=" + deliveryFeeJiatuo + "&" + "unloadingCosts=" + unloadingCosts + "&" + "startFee=" + startFee + "&" +
                    "fuelCosts=" + fuelCosts + "&" + "packagesFare=" + packagesFare + "&" +"scores=" + scores + "&" + "pointCharges=" + pointCharges + "&" +
                    "permitFees=" + permitFees + "&" + "otherCosts=" + otherCosts + "&" + "total=" + total + "&" + "amountTo=" + amountTo + "&" +
                    "carModels=" + carModels + "&" + "remarks=" + remarks + "&", //要传的值 
                success: function (data) {
                    if (data) {
                        if (data!=true) {
                            alert(data);
                            return;
                        }
                        var trs = $('#resultTable tr');
                        $('#selectAll').removeAttr('checked');
                        for (var i = 1; i < trs.length; i++) {
                            var tr = $(trs[i]);
                            var check = tr.find('.checkForSelect');
                            if (check.attr("checked") === "checked") {
                                tr.remove();
                            }
                        }
                    }
                    else {
                        alert("结算失败");
                    }
                    Runbow.TWS.Popup.close();
                }
            });
        }
    };

    $.extend(buttons, {
        "取消": function () {
            Runbow.TWS.Popup.close();
        }
    });
    var contents = $('#jieshuan').clone().show();
    var opts = {
        'title': title,
        //'content': $('#showAuditDialog').clone().show(),
        'content': contents,
        'buttons': buttons,
        'width': '500',
        'minHeight': '450'
    };

    Runbow.TWS.Popup.show(opts);
};
