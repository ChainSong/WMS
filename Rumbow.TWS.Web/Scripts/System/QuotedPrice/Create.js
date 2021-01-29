$(document).ready(function () {

    $('#RelatedCustomerID').live('change',function () {
        var target = $('#Target').val();
        $('#historyQuotedPrice').hide();
        var tbody = $('#quotedPriceDetailTBody');
        var targetID;
        if (target === '0') {
            targetID = $('#TargetID option:selected').val();
        } else {
            targetID = $('#TargetID').val();
        }
        var relatedCustomerID = $('#RelatedCustomerID option:selected').val();
        $('#RelatedCustomerName').val($('#RelatedCustomerID option:selected').text());

        if (relatedCustomerID === '') {
            tbody.html('');
            $('#RelatedCustomerName').val('');
            return;
        }

        if (target === '1') {
            if (targetID === '') {
                return;
            }
        }

        $.send(
            '/System/QuotedPrice/GetSegmentDetailByTargetID',
            { target: target, targetID: targetID,relatedCustomerID: relatedCustomerID },
            function (data) {
                var innerHtml = '';
                if (data.length === 0) {
                    Runbow.TWS.Alert('该' + (target === '0' ? '客户' : '承运商') + '未进行段位关联，请先关联段位');
                    $('#TargetID').val('');
                    if (target === '1') {
                        $('#RelatedCustomerID').val('');
                    }
                    tbody.html('');
                    return;
                }

                for (var i = 0; i < data.length; i++) {
                    innerHtml += '<tr><td>' + data[i].StartVal + "</td><td>" + data[i].EndVal + '</td><td><input type="text" id="Price_' + data[i].SegmentDetailID
                        + '" data-id="' + data[i].SegmentDetailID + '" data-StartVal="' + data[i].StartVal + '" data-EndVal="' + data[i].EndVal + '" class="tablePrice"' + '</input><label class="need">*</label></td></tr>';
                }
                tbody.html(innerHtml);
            },
            function () {
                $('#TargetID').val('');
                if (target === '1') {
                    $('#RelatedCustomerID').val('');
                }
                tbody.html('');
                Runbow.TWS.Alert('无法根据' + (target === '0' ? '客户' : '承运商') + '取得对应的报价区间！');
            });
    });

    $('#TargetName').autocomplete({
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
            $('#TargetID').val(ui.item.data.Value);
            $('#TargetName').val(ui.item.data.Text);
            $('#TargetID').trigger('change');
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#TargetID').val('');
            $('#TargetID').trigger('change');
        }
    });

    $('#TargetID').change(function () {
        var target = $('#Target').val();
        $('#historyQuotedPrice').hide();
        var tbody = $('#quotedPriceDetailTBody');
        var targetID;
        if (target == 0) {
            targetID = $('#TargetID option:selected').val();
            $('#TargetName').val($('#TargetID option:selected').text());
        } else {
            targetID = $(this).val();
        }
        //var targetID = $('#TargetID option:selected').val();
        //$('#TargetName').val($('#TargetID option:selected').text());
        if (targetID === '') {
            tbody.html('');
            $('#TargetName').val('');
            return;
        }

        var relatedCustomerID;
        if (target === '1') {
            relatedCustomerID = $('#RelatedCustomerID option:selected').val();
            if (relatedCustomerID === '') {
                return;
            }
        } else {
            relatedCustomerID = 0;
        }


        $.send(
            '/System/QuotedPrice/GetSegmentDetailByTargetID',
            { target: target, targetID: targetID, relatedCustomerID: relatedCustomerID },
            function (data) {
                var innerHtml = '';
                if (data.length === 0) {
                    Runbow.TWS.Alert('该' + (target === '0' ? '客户' : '承运商') + '未进行段位关联，请先关联段位');
                    $('#TargetID').val('');
                    if (target === '1') {
                        $('#RelatedCustomerID').val('');
                    }
                    tbody.html('');
                    return;
                }

                for (var i = 0; i < data.length; i++) {
                    innerHtml += '<tr><td>' + data[i].StartVal + "</td><td>" + data[i].EndVal + '</td><td><input type="text" id="Price_' + data[i].SegmentDetailID
                        + '" data-id="' + data[i].SegmentDetailID + '" data-StartVal="' + data[i].StartVal + '" data-EndVal="' + data[i].EndVal + '" class="tablePrice"' + '</input><label class="need">*</label></td></tr>';
                }
                tbody.html(innerHtml);
                
            },
            function () {
                $('#TargetID').val('');
                if (target === '1') {
                    $('#RelatedCustomerID').val('');
                }
                tbody.html('');
                Runbow.TWS.Alert('无法根据' + (target === '0' ? '客户' : '承运商') + '取得对应的报价区间！');
            });
    });

    //$("#AutoTransportationLine").hide();
    $("#AutoTransportationLine").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/System/QuotedPrice/GetTransPortationLines",  
                type: "POST",  
                dataType: "json", 
                data: { name: request.term }, 
                success: function (data) {
                    response($.map(data, function (item) {
                        return {label:item.Text,value:item.Text,data:item}
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#TransportationLine').val(ui.item.data.Value);
            var transData = ui.item.data.Value.split('|');
            $('#TransportationLineID').val(transData[0]);
            $('#StartCityID').val(transData[1]);
            $('#StartCityName').val(transData[2]);
            $('#EndCityID').val(transData[3]);
            $('#EndCityName').val(transData[4]);
        }
    });


    
    $('#TransportationLine').change(function () {
        var val = $('#TransportationLine option:selected').val();
        $('#historyQuotedPrice').hide();
        if(val === ''){
            $('#TransportationLineID').val('');
            $('#StartCityID').val('');
            $('#StartCityName').val('');
            $('#EndCityID').val('');
            $('#EndCityName').val('');
            $("#AutoTransportationLine").val('');
        }else{
            var transData = val.split('|');
            $('#TransportationLineID').val(transData[0]);
            $('#StartCityID').val(transData[1]);
            $('#StartCityName').val(transData[2]);
            $('#EndCityID').val(transData[3]);
            $('#EndCityName').val(transData[4]);
            $("#AutoTransportationLine").val($('#TransportationLine option:selected').text());
        }
    });

    $('#ShipperTypeID').change(function () {
        $('#historyQuotedPrice').hide();
        if ($('#ShipperTypeID option:selected').val() === '') {
            $('#ShipperTypeName').val('');
        } else {
            $('#ShipperTypeName').val($('#ShipperTypeID option:selected').text());
        }
    });

    $('#PodTypeID').change(function () {
        $('#historyQuotedPrice').hide();
        if ($('#PodTypeID option:selected').val() === '') {
            $('#PodTypeName').val('');
        } else {
            $('#PodTypeName').val($('#PodTypeID option:selected').text());
        }
    });

    $('#TplOrTtlID').change(function () {
        $('#historyQuotedPrice').hide();
        if ($('#TplOrTtlID option:selected').val() === '') {
            $('#TplOrTtlName').val('');
        } else {
            $('#TplOrTtlName').val($('#TplOrTtlID option:selected').text());
        }
    });

    var checkDropDownListRequired = function () {
        if ($('#Target').val() === '0' && $('#TargetID option:selected').val() === '') {
            Runbow.TWS.Alert("请选择客户");
            return false;
        }

        if ($('#Target').val() === '1' && $('#TargetID').val() === '') {
            Runbow.TWS.Alert("请选择承运商");
            return false;
        }

        if ($('#Target').val() === '1') {
            if ($('#RelatedCustomerID option:selected').val() === '') {
                Runbow.TWS.Alert("请选择关联客户");
                return false;
            }
        }

        if ($('#TransportationLine option:selected').val() === '' || $('#TransportationLineID').val() === '') {
            Runbow.TWS.Alert("请选择线路");
            return false;
        }

        if ($('#ShipperTypeID option:selected').val() === '') {
            Runbow.TWS.Alert("请选择运输方式");
            return false;
        }
        
        if ($('#PodTypeID option:selected').val() === '') {
            Runbow.TWS.Alert("请选择运单类型");
            return false;
        }

        if ($('#TplOrTtlID option:selected').val() === '') {
            Runbow.TWS.Alert("请选择整车零担");
            return false;
        }

        return true;
    }
    
    var checkInputRequired = function () {
        var returnVal = true;
        $('.tablePrice').each(function (index) {
            var price = $(this).val();
            if (price === '' || !parseFloat(price)) {
                returnVal = false;
            }
        });

        if (returnVal === false) {
            Runbow.TWS.Alert("段位金额不能为空或输入有误，请更正");
            return false;
        }

        return true;
    }

    var checkRequired = function () {

        if ($('#EffectiveStartTime').val() === '') {
            Runbow.TWS.Alert('请输入开始日期');
            return false;
        }

        if ($('#EffectiveEndTime').val() !== '' && ($('#EffectiveEndTime').val() <= $('#EffectiveStartTime').val())) {
            Runbow.TWS.Alert('截至日期必须大于开始日期');
            return false;
        }
        if (!checkInputRequired()) {
            return false;
        }

        return true;
    }

    $('#btnViewHistory').click(function () {
        if (!checkDropDownListRequired()) {
            return;
        }

        var target = $('#Target').val();
        var relatedCustomerID;
        var targetID;
        if (target === '0') {
            targetID = $('#TargetID option:selected').val();
            relatedCustomerID = 0;
        } else {
            targetID = $('#TargetID').val();
            relatedCustomerID = $('#RelatedCustomerID option:selected').val();
        }

        var podTypeID = $('#PodTypeID option:selected').val();
        var shipperTypeID = $('#ShipperTypeID option:selected').val();
        var tplOrTtlID = $('#TplOrTtlID option:selected').val();

        $.send(
            '/System/QuotedPrice/GetHistoryQuetedPrice',
            { target: target, targetID: targetID,transportationLineID:$('#TransportationLineID').val(),shipperTypeID:shipperTypeID, podTypeID:podTypeID, tplOrTtlID:tplOrTtlID, relatedCustomerID: relatedCustomerID},
            function (data) {
                var innerHtml = '';
                for (var i = 0; i < data.length; i++) {
                    innerHtml += '<tr><td>' + data[i].TargetName + '</td><td>' + data[i].TransportationLine + '</td><td>' + data[i].ShipperType + '</td><td>'
                    + data[i].PodType + '</td><td>' + data[i].TtlOrTpl + '</td><td>' + data[i].EffectiveTime + '</td><td>' + data[i].QuotedPrice + '</td></tr>';
                }

                $('#historyQuotedPriceTbody').html(innerHtml);
                $('#historyQuotedPrice').show();
            },
            function () {
                Runbow.TWS.Alert("无法根据客户或承运商取得对应的历史报价！");
            });
        

    });

    $('#btnCreate').click(function () {
        if (!checkRequired()) {
            return false;
        }

        var data = '[';
        $('.tablePrice').each(function (index) {
            var segmentDetailID = $(this).attr('data-id');
            var startVal = $(this).attr('data-StartVal');
            var endVal = $(this).attr('data-EndVal');
            var price = $(this).val();

            data += '{SegmentDetailID:"' + segmentDetailID + '",StartVal:"' + startVal + '",EndVal:"' + endVal + '",Price:"' + price + '"},'
        });

        data = data.substr(0, data.length - 1);
        data += ']';
        $('#SettedConfigs').val(data);

        return true;

    });

  
});
