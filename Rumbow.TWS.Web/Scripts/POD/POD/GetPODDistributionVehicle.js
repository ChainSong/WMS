$(document).ready(function () {

    $("#IsPODDistributionVehicles").click(function () {


        //openPopup("", true, 400, 400, null, 'DisInfo', true);
        openPopup("Evaluation", true, 400, 350, null, "Evaluation", true);
        $("#popupLayer_Evaluation")[0].style.top = "200px";
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
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].type == "checkbox") {
                if (!checkBoxs[i].disabled) {
                    checkBoxs[i].checked = this.checked ? true : false;
                }
            }
        }
    });
    $('.calendarRange').each(function (index) {
        //$('#startCityTreeID').val($('#SearchCondition_StartCities').val());
        //$('#startCityTreeName').val($('#SearchCondition_StartCityName').val());
        //$('#endCityTreeID').val($('#SearchCondition_EndCities').val());
        //$('#endCityTreeName').val($('#SearchCondition_EndCityName').val());
        $('#startCityTreeID').val($('#Condition_StartCities').val());
        $('#startCityTreeName').val($('#Condition_StartCityName').val());
        $('#endCityTreeID').val($('#Condition_EndCities').val());
        $('#endCityTreeName').val($('#Condition_EndCityName').val());

        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[0];
        var descID = 'Condition_';
        if (actualID === 'start') {//Condition_StartShelvesTime

            descID += 'StartDeliveryTime';//+ actualID;

        } else {

            descID += 'EndDeliveryTime';//+ actualID;

        }
        $(this).val($('#' + descID).val());
    });
    $('#searchButton').click(function () {
        setPageControlVal();
    });
    $('#startCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        //   $('#SearchCondition_StartCityID').val('');
        $('#startCityTreeName').val('');
        //    $('#SearchCondition_StartCities').val('');
    });

    $('#endCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        //  $('#SearchCondition_EndCityID').val('');
        $('#endCityTreeName').val('');
        // $('#SearchCondition_EndCities').val('');
    });
    var setPageControlVal = function () {
        $('#startCityTreeID').val($('#Condition_StartCities').val());
        $('#Condition_StartCityName').val($('#startCityTreeName').val());
        $('#endCityTreeID').val($('#Condition_EndCities').val());
        $('#Condition_EndCityName').val($('#endCityTreeName').val());
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[0];
            var descID = 'Condition_';
            if (actualID === 'start') {//Condition_StartShelvesTime
                descID += 'StartDeliveryTime';//+ actualID;
            } else {
                descID += 'EndDeliveryTime';// + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    }
    //$("#WaybillReachs").click(function () {

    //    //openPopup("", true, 400, 400, null, 'DisInfo', true);
    //    openPopup("Evaluation", true, 400, 200, null, "WaybillReachPopup", true);
    //    $("#popupLayer_Evaluation")[0].style.top = "200px";
    //    //openPopup("", true, 500, 100, '/POD/POD/PODDistributionVehicle/?ids=' + ids, null, function (ids) {
    //    //    location.href = "/POD/POD/BatchEditPods/0?IsPODDistributionVehicle=true"
    //    //});
    //    //BatchEditPods
    //    //openPopup("", true, 1000, 600, '/WMS/ReceiptManagement/ASNQuery/?CustomerID=' + CustomerID, null, function (ASNID, CustomerID) {
    //    //    //$('#ASNNumber').val(box.ASNNumbers);
    //    //    //$('#ASNID').val(ASNID);
    //    //    //$('#ExternReceiptNumber').val(box.ExternReceiptNumbers);
    //    //    //$('#CustomerName').val(box.CustomerNames);
    //    //    //$('#CustomerID').val(CustomerID);
    //    //    //$('#ASNType').val(box.ASNTypes);
    //    //    location.href = "/WMS/ReceiptManagement/ReceiptCreate/?ID=" + ASNID + "&ViewType=1"

    //    //});
    //});
    $("#confirm").click(function () {
    
        var ids = ""; //= $('#SelectedIDs').val();
    
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].type == "checkbox") {
                if (checkBoxs[i].checked) {
                    if ($(checkBoxs[i]).parent().parent().children("td")[10].innerText != "已分配") {
                        ids += $(checkBoxs[i])[0].dataset.id + ",";
                        //checkBoxs[i].checked = this.checked ? true : false;
                    }
                }
            }
        }
        if (ids.length > 2) {
            ids = ids.toString().substring(0, ids.toString().length - 1);
        }
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
        var GPSCode = $("#GPSCode").val();
       
        if (CarNo == ""  || StartTime == "") {
            showMsg("请填写完整信息！", "4000");
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
                StartTime: StartTime,
                GPSCode: GPSCode
            },
            function (response) {
                if (response.Code == 1) {
                    for (var i = 0; i < checkBoxs.length; i++) {
                        if (checkBoxs[i].type == "checkbox") {
                            if (checkBoxs[i].checked) {
                                $(checkBoxs[i]).parent().parent().children("td")[10].innerText = "已分配";
                                //checkBoxs[i].checked = this.checked ? true : false;
                            }
                        }
                    }
                    closePopup();
                    showMsg('操作成功！', 4000);

                } else {
                    closePopup();
                    showMsg('操作失败！', 4000);

                }
            });


    });
    $("#Cancel").click(function () {
        var ids = ""; //= $('#SelectedIDs').val(); 
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].type == "checkbox") {
                if (checkBoxs[i].checked) {
                    if ($(checkBoxs[i]).parent().parent().children("td")[10].innerText != "") {
                        ids += $(checkBoxs[i])[0].dataset.id + ",";
                        //checkBoxs[i].checked = this.checked ? true : false;
                    }
                }
            }
        }
        if (ids.length > 2) {
            ids = ids.toString().substring(0, ids.toString().length - 1);
        }
        var PODType = $("select[id=CancelType]").val();

        
        $.send(
            '/POD/Pod/CancelPODDistributionVehicle',
            {
                ids: ids,
                PODType: PODType

            },
            function (response) {
                if (response.Code == 1) {
                    for (var i = 0; i < checkBoxs.length; i++) {
                        if (checkBoxs[i].type == "checkbox") {
                            if (checkBoxs[i].checked) {
                                $(checkBoxs[i]).parent().parent().remove();
                            }
                        }
                    }
                    showMsg('操作成功！', 4000);

                } else {
                    showMsg('操作失败！', 4000);

                }
            });


    })
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
            $("#SearchCondition_podDistributionVehicle_DriverName")[0].value=ui.item.data.DriverName;
            $("#SearchCondition_podDistributionVehicle_DriverPhone")[0].value = ui.item.data.Phone;
        }
    })

    $("#PODconfirm").click(function () {
        var ids = $('#SelectedIDs').val();
        var name = $(this).val();
        if (ids === '') {
            showMsg('请选择运单', "4000");
            return;
        }
        var EndTime = $("#EndTime").val();
        var PODType = $("select[id=SearchCondition_podDistributionVehicle_PODType]").val();
        if (EndTime == "") {
            showMsg("请填写完整信息！", "4000");
            return false;
        }
        $.send(
            '/POD/Pod/WaybillReach',
            {
                ids: ids,
                EndTime: EndTime,
                PODType: PODType
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