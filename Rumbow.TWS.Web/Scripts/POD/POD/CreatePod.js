$(document).ready(function () {
    var setHiddenValToControl = function () {
        $('#startCityTreeID').val($('#Pod_StartCityID').val());
        $('#startCityTreeName').val($('#Pod_StartCityName').val());
        $('#endCityTreeID').val($('#Pod_EndCityID').val());
        $('#endCityTreeName').val($('#Pod_EndCityName').val());

        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "Pod_" + id;
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
                if ($(this).hasClass('datetimeval') && $(this).val() !== '') {
                    $(this).val($(this).val().split(' ')[0]);
                }
            }
        });
    };

    $('select[id=Pod_PODStateID]').change(function () {
        $('#Pod_PODStateName').val($('#Pod_PODStateID option:selected').text());
    }).trigger('change');

    $('select[id=Pod_CustomerID]').change(function () {
        window.location.href = "/POD/POD/CreatePod/?customerID=" + $(this).val();
        //$('#Pod_CustomerName').val($('#Pod_CustomerID option:selected').text());
    });

    if ($('select[id=Pod_CustomerID]').val()) {
        $('#Pod_CustomerName').val($('#Pod_CustomerID option:selected').text());
    }

    $('select[id=Pod_PODTypeID]').change(function () {
        $('#Pod_PODTypeName').val($('#Pod_PODTypeID option:selected').text());
    }).trigger('change');

    $('select[id=Pod_TtlOrTplID]').change(function () {
        $('#Pod_TtlOrTplName').val($('#Pod_TtlOrTplID option:selected').text());
    }).trigger('change');



    $('select[id=Pod_ShipperTypeID]').change(function () {
        $('#Pod_ShipperTypeName').val($('#Pod_ShipperTypeID option:selected').text());
    }).trigger('change');

    $('#submitButton').click(function () {
        if (!checkInput()) {
            return false;
        }

        setPageControlVal();
    });

    $('#startCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $(this).next().val('');
        $(this).next().next().val('');
    });

    $('#endCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $(this).next().val('');
        $(this).next().next().val('');
    });

    var checkInput = function () {

        if (!$('#Pod_CustomerID').val()) {
            Runbow.TWS.Alert("请选择客户");
            return false;
        }

        if ($('#Pod_CustomerOrderNumber').val() === "") {
            Runbow.TWS.Alert("请输入客户单号");
            return false;
        }

        if ($('#Pod_StartCityID').val() === "") {
            Runbow.TWS.Alert("请选择起运城市");
            return false;
        }

        if ($('#Pod_EndCityID').val() === "") {
            Runbow.TWS.Alert("请选择目的城市");
            return false;
        }

        if (!checkFloat($('#Pod_BoxNumber').val())) {
            Runbow.TWS.Alert("请输入正确箱数");
            return false;
        }

        if (!checkFloat($('#Pod_GoodsNumber').val())) {
            Runbow.TWS.Alert("请输入正确件数");
            return false;
        }

        if (!checkFloat($('#Pod_Weight').val())) {
            Runbow.TWS.Alert("请输入正确重量");
            return false;
        }

        if (!checkFloat($('#Pod_Volume').val())) {
            Runbow.TWS.Alert("请输入正确体积");
            return false;
        }

        var customerID = $('#Pod_CustomerID').val();
        if (customerID === '2') {
            var ttlOrTplType = $('#Pod_TtlOrTplID option:selected').text();
            if (ttlOrTplType === 'FTL') {
                if ($('#Str29').val() === '') {
                    Runbow.TWS.Alert("请输入整车吨位");
                    return false;
                }
                if (!parseInt($('#Str29').val())) {
                    Runbow.TWS.Alert("请输入正确吨位:2,5,10,15,20");
                    return false;
                }
            }

            var paymentMethod = $('#Str22').val();
            if (paymentMethod === '1') {
                if ($('#Str30').val() === '') {
                    Runbow.TWS.Alert("请输入代收款方式");
                    return false;
                }
            }
        }

        return true;
    };

    var setPageControlVal = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "Pod_" + id;
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
    };

    var checkFloat = function (val) {
        if (val === "" || !isNaN(val) || parseFloat(val)) {
            return true;
        }
        return false;
    };

    setHiddenValToControl();
    $('#Pod_CustomerOrderNumber').focus();

    $("#Pod_ShipperName").autocomplete({
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
            $('#Pod_ShipperID').val(ui.item.data.Value);
            $('#Pod_ShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#Pod_ShipperID').val('');
        }
    });


    //TODO: these code are not common, according to every customer's requirement, will changed frequently
    var customerID = $('#Pod_CustomerID').val();
    if (customerID === '15') {
        var currentDate = new Date();
        var IsEditModel = $('#IsEditModel').val();

        if (IsEditModel !== 'True') {
            $('#DateTime1').val(currentDate.Format('yyyy-MM-dd') + ' 22:00');
            $('#Pod_CustomerOrderNumber').val('OCJ' + currentDate.Format("yyyyMMddhhmmss"));
            $('#Pod_ActualDeliveryDate').val('');
        } 
        
        $('#DateTime4, #DateTime3').live('focusout', function () {
            var endTime = $('#DateTime4').val().replace(/-/g, "/");
            var startTime = $('#DateTime3').val().replace(/-/g, "/");
            if (startTime !== '' && endTime != '') {
                var timeSpan = Date.parse(endTime) - Date.parse(startTime);
                if (timeSpan) {
                    $('#Str5').val((timeSpan / (1000 * 60 * 60)).toFixed(3));
                }
            }
        });

        $('#DateTime8, #DateTime7').live('focusout', function () {
            var sendTime = $('#DateTime8').val().replace(/-/g, "/");
            var actualArrTime = $('#DateTime7').val().replace(/-/g, "/");
            if (sendTime !== '' && actualArrTime != '') {
                var timeSpan1 = Date.parse(sendTime) - Date.parse(actualArrTime);
                if (timeSpan1) {
                    $('#Str12').val((timeSpan1 / (1000 * 60 * 60)).toFixed(2));
                }
            }
        });

        $('#Pod_Volume, #Pod_GoodsNumber').live('change', function () {
            if ($('#Pod_Volume').val() !== '' && $('#Pod_GoodsNumber').val() !== '') {
                var volume = parseFloat($('#Pod_Volume').val());
                var goodNumber = parseFloat($('#Pod_GoodsNumber').val());
                if (volume && goodNumber) {
                    $('#Str6').val((volume / goodNumber).toFixed(6));
                }
            }
        });

        $('#startCityTreeName, #endCityTreeName').live('focusout', function () {
            var startCity = $('#startCityTreeName').val();
            var endCity = $('#endCityTreeName').val();
            if (startCity !== '' && endCity != '') {
                if ((startCity === '上海' && endCity === '武汉') || (startCity === '武汉' && endCity === '上海')) {
                    $('#Str16').val(2);
                }

                if ((startCity === '上海' && endCity === '哈尔滨') || (startCity === '哈尔滨' && endCity === '上海')) {
                    $('#Str16').val(4);
                }

                var actualDeliveryDate = $('#DateTime1').val();
                var timeSpan = $('#Str16').val();
                if (actualDeliveryDate !== '' && timeSpan !== '') {
                    var date1 = new Date(Date.parse(actualDeliveryDate.replace(/-/g, "/")));
                    var span = parseInt(timeSpan);
                    if (date1 && span) {
                        date1.addDays(span);
                        var t = date1.Format("yyyy-MM-dd");
                        var t1 = t;
                        if ($('#Pod_PODTypeID').val() === '49') {
                            t = t + ' 16:00';
                        } else if ($('#Pod_PODTypeID').val() === '50') {
                            t = t + ' 10:00';
                        }
                        else {
                            t = t + ' 16:00';
                        }
                        $('#DateTime6').val(t);
                        $('#DateTime9').val(t1+' 22:00');
                    }
                }
            }
        });

        $('#Pod_PODTypeID, #DateTime1, #Str16').live('focusout', function () {
            var actualDeliveryDate = $('#DateTime1').val();
            var timeSpan = $('#Str16').val();
            if (actualDeliveryDate !== '' && timeSpan !== '') {
                var date1 = new Date(Date.parse(actualDeliveryDate.replace(/-/g, "/")));
                var span = parseInt(timeSpan);
                if (date1 && span) {
                    date1.addDays(span);
                    var t = date1.Format("yyyy-MM-dd");
                    if ($('#Pod_PODTypeID').val() === '49') {
                        t = t + ' 16:00';
                    } else if ($('#Pod_PODTypeID').val() === '50') {
                        t = t + ' 10:00';
                    }
                    else {
                        t = t + ' 16:00';
                    }
                    $('#DateTime6').val(t);
                }
            }
        });
    }

    if (customerID === '2') {
        var editModel = $('#IsEditModel').val();
        var shipperTypeID = $('#Pod_ShipperTypeID').val();
        var shipperTypeName = $('#Pod_ShipperTypeID option:selected').text();
        var ttlOrTplType = $('#Pod_TtlOrTplID option:selected').text();
        if (ttlOrTplType === 'LTL') {
            $('#Str29').attr('disabled', 'disabled');
        }
        else {
            $('#Str29').removeAttr('disabled');
        }

        $('#Pod_TtlOrTplID').live('change', function () {
            var ttlOrTplType = $('#Pod_TtlOrTplID option:selected').text();
            if (ttlOrTplType === 'LTL') {
                $('#Str29').val('');
                $('#Str29').attr('disabled', 'disabled');
            }
            else {
                $('#Str29').removeAttr('disabled');
            }
        });

        if ((shipperTypeID === '5' && shipperTypeName === '航空') || (shipperTypeID === '36' && shipperTypeName === '快递')) {
            $('#Str11').removeAttr('disabled')
        } else {
            $('#Str11').attr('disabled', 'disabled');
        }

        $('#Str24').change(function () {
            var netWeight = parseFloat($(this).val());
            if (netWeight) {
                $('#Str9').val(netWeight * 1.04);
            }
        });

        var paymentMethod = $('#Str22').val();
        if (paymentMethod === '1') {
            $('#Str30').removeAttr('disabled');
            $('#Str14').removeAttr('disabled');
        } else {
            $('#Str30').val('');
            $('#Str14').val('');
            $('#Str30').attr('disabled', 'disabled');
            $('#Str14').attr('disabled', 'disabled');
        }


        $('#Str22').change(function () {
            if ($(this).val() === '1') {
                $('#Str30').removeAttr('disabled');
                $('#Str14').removeAttr('disabled');
            } else {
                $('#Str30').val('');
                $('#Str14').val('');
                $('#Str30').attr('disabled', 'disabled');
                $('#Str14').attr('disabled', 'disabled');
            }
        });

        $('#Pod_ShipperTypeID').change(function () {
            var shipperTypeID = $('#Pod_ShipperTypeID').val();
            var shipperTypeName = $('#Pod_ShipperTypeID option:selected').text();
            if ((shipperTypeID === '5' && shipperTypeName === '航空') || (shipperTypeID === '36' && shipperTypeName === '快递')) {
                $('#Str11').removeAttr('disabled');
            } else {
                $('#Str11').val('');
                $('#Str11').attr('disabled', 'disabled');
            }
        });

        if (editModel !== 'True') {
            $('#Pod_StartCityID').val('10');
            $('#startCityTreeID').val('10');
            $('#Pod_StartCityName').val('上海');
            $('#startCityTreeName').val('上海');
        }

        $('#Pod_ActualDeliveryDate, #Str23').live('change', function () {
            var startCityID = $('#Pod_StartCityID').val();
            var endCityID = $('#Pod_EndCityID').val();
            var actualDeliveryTime = $('#Pod_ActualDeliveryDate').val();

            if (startCityID !== '' && endCityID !== '' && actualDeliveryTime !== '') {
                $.send(
                    '/POD/Hilti/GetServicePeriod',
                    {
                        customerID: 2, startCityID: startCityID, endCityID: endCityID
                    },
                    function (response) {
                        if (response) {
                            var period = parseInt(response.Period);
                            var isBeloneToShanghai = false;
                            $.send(
                                '/POD/Hilti/IsBeloneToShanghai',
                                {
                                    cityID: endCityID
                                },
                                function (res) {
                                    if (res.IsSuccess===true) {
                                        if ($('#Str23').val() !== '') {
                                            var t = $('#Str23').val().split(':')[0];
                                            if (t) {
                                                if (parseInt(t) > 12) {
                                                    period += 1;
                                                }
                                            }
                                        }    
                                    }
                                    var date = new Date(actualDeliveryTime)
                                    date.addDays(period);
                                    $('#DateTime2').val(date.Format('yyyy-MM-dd'));
                                },
                                function (res) {
                                    var date = new Date(actualDeliveryTime)
                                    date.addDays(period);
                                    $('#DateTime2').val(date.Format('yyyy-MM-dd'));
                                });                           
                        }
                    });
            }

        });
    }
});

