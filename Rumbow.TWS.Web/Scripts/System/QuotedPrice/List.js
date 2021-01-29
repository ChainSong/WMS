$(document).ready(function () {
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
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#TargetID').val('');
        }
    });

    $('#searchButton').click(function () {
        var target = $('#Target').val();
        var targetID;
        if (target === '0') {
            targetID = $('#TargetID option:selected').val();
        } else {
            targetID = $('#TargetID').val();
            if (targetID === '' || targetID === '0') {
                Runbow.TWS.Alert('请选择承运商后再查询');
                return;
            }
        }
        
        
        var podTypeID = $('#PodTypeID option:selected').val(), shipperTypeID = $('#ShipperTypeID option:selected').val(),
            tplOrTtlID = $('#TplOrTtlID option:selected').val(), effectiveStartTime = $('#EffectiveStartTime').val(),
            effectiveEndTime = $('#EffectiveEndTime').val();
        var startCityID = $('#StartCityID').val(), endCityID = $('#EndCityID').val();

        var relatedCustomerID;
        if (target === '0') {
            relatedCustomerID = 0;
        } else {
            if ($('#RelatedCustomerID option:selected').val() === '') {
                relatedCustomerID = 0;
            }
            else {
                relatedCustomerID = $('#RelatedCustomerID option:selected').val();
            }
        }
        var transportationLineID = '';
        if ($('#TransportationLine option:selected').val() !== '') {
            transportationLineID = $('#TransportationLine option:selected').val().split('|')[0];
        }

        $.send(
            '/System/QuotedPrice/GetHistoryQuetedPrice',
            { target: target, targetID: targetID, transportationLineID: transportationLineID, shipperTypeID: shipperTypeID, podTypeID: podTypeID, tplOrTtlID: tplOrTtlID, effectiveStartTime: effectiveStartTime, effectiveEndTime: effectiveEndTime, startCityID: startCityID,endCityID: endCityID, relatedCustomerID: relatedCustomerID },
            function (data) {
                var innerHtml = '';
                for (var i = 0; i < data.length; i++) {
                    innerHtml += '<tr><td>' + data[i].TargetName + '</td><td>';
                    if (target === '1') {
                        innerHtml += data[i].RelatedCustomerName + '</td><td>';
                    }
                    innerHtml += data[i].TransportationLine + '</td><td>' + data[i].ShipperType + '</td><td>'
                    + data[i].PodType + '</td><td>' + data[i].TtlOrTpl + '</td><td>' + data[i].EffectiveTime + '</td><td>' + data[i].QuotedPrice + '</td><td><a href="#" class="deleteQuotedPrice" data-id="' 
                    + data[i].IDs + '" >删除</a></td></tr>';
                }

                if (innerHtml === '') {
                    var spanlength = target === '1' ? 9 : 8;
                    innerHtml = '<tr><td colspan="' + spanlength + '" style="text-align:center">无报价</td></tr>';
                }

                $('#historyQuotedPriceTbody').html(innerHtml);
            },
            function () {
                Runbow.TWS.Alert("无法根据客户或承运商取得对应的报价！");
            });
    });

    $('.deleteQuotedPrice').live('click', function () {
        var ids = $(this).attr('data-id');
        var row = $(this).parent().parent();

        if (window.confirm("确认删除此报价？")) {
            $.send(
           '/System/QuotedPrice/DeleteQuetedPrice',
           {ids:ids},
           function (data) {
               if (data.ReturnVal === 1) {
                   row.remove();
               } else {
                   Runbow.TWS.Alert(data.Message)
               }
               
           },
           function () {
               Runbow.TWS.Alert("删除报价失败！");
           });
        }

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
});

function onRegionSelected(rid, rn, treeId) {
    if (treeId === 'startCityTreeKey') {
        $('#StartCityID').val($('#startCityTreeID').attr('value'));
        $('#StartCityName').val($('#startCityTreeName').attr('value'));
    } else if (treeId === 'endCityTreeKey') {
        $('#EndCityID').val($('#endCityTreeID').attr('value'));
        $('#EndCityName').val($('#endCityTreeName').attr('value'));
    }
}

function onRegionAutoCompleteSelected(globalID) {
    if (globalID === 'startCityTreeKey') {
        $('#StartCityID').val($('#startCityTreeID').attr('value'));
        $('#StartCityName').val($('#startCityTreeName').attr('value'));
    } else if (globalID === 'endCityTreeKey') {
        $('#EndCityID').val($('#endCityTreeID').attr('value'));
        $('#EndCityName').val($('#endCityTreeName').attr('value'));
    }
}