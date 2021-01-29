$(document).ready(function () {
    $('#RelatedCustomerID').change(function () {
        clearDataFromPageControl();
        SetStartCityForHilti();
        $('#ShipperName').val('');
    });

    $("#ShipperName").autocomplete({
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
            clearDataFromPageControl();
            SetStartCityForHilti();
            $('#ShipperID').val(ui.item.data.Value);
            $('#ShipperName').val(ui.item.data.Text);
            $('#btnSearch').trigger('click');

        }
    }).change(function () {
        if ($(this).val() === '') {
            clearDataFromPageControl();
            SetStartCityForHilti();
        }
    });

    $('#btnSearch').click(function () {
        if ($('#ShipperID').val() === '' || $('#ShipperID').val() === '0') {
            Runbow.TWS.Alert('请选择承运商');
            $('#ShipperName').focus();
            clearDataFromPageControl();
            $('#ShipperName').val('');
            return false;
        }

        return true;
    });


    var SetStartCityForHilti = function () {
        if ($('#RelatedCustomerID').val() === '2') {
            $('#startCityTreeID').val('10');
            $('#StartCityID').val('10');
            $('#startCityTreeName').val('上海');
            $('#StartCityName').val('上海');
        }
    };

    var clearDataFromPageControl = function () {
        $('#ShipperID').val('');
        $('#startCityTreeID').val('');
        $('#StartCityID').val('');
        $('#startCityTreeName').val('');
        $('#StartCityName').val('');
        $('#endCityTreeID').val('');
        $('#EndCityID').val('');
        $('#endCityTreeName').val('');
        $('#EndCityName').val('');
        $('#ShipperRegionCoveredTable tbody tr').remove();
        $('#ShipperRelatedInfo_Str1').val('');
        $('#ShipperRelatedInfo_Str2').val('');
        $('#ShipperRelatedInfo_Str6').val('');
        $('#ShipperRelatedInfo_Str7').val('');
    };

    $('#submitForecast').click(function () {
        if ($('#ShipperID').val() === '' || $('#ShipperID').val() === '0') {
            Runbow.TWS.Alert("请选择承运商");
            return;
        }

        //if ($('#ShipperRelatedInfo_Str1').val() === '') {
        //    Runbow.TWS.Alert("请输入收件人，以;间隔");
        //    return;
        //}

        //if ($('#ShipperRelatedInfo_Str6').val() === '') {
        //    Runbow.TWS.Alert("请输入邮件内容");
        //    return;
        //}

        $.send(
                '/System/Shipper/ManageShipperEmailInfo',
                { Type: 1, ProjectID: $('#ProjectID').val(), RelatedCustomerID: $('#RelatedCustomerID').val(), ShipperID: $('#ShipperID').val(), ShipperName: $('#ShipperName').val(), EmailAddress: $('#ShipperRelatedInfo_Str1').val(), EmailContent: $('#ShipperRelatedInfo_Str6').val() },
                function (data) {
                    if (data.IsSuccess) {
                        Runbow.TWS.Alert('发送预报邮件信息配置成功');
                    }
                    else {
                        Runbow.TWS.Alert("发送预报邮件信息配置失败！");
                    }
                },
                function () {
                    Runbow.TWS.Alert("发送预报邮件信息配置失败！");
                });
    });

    $('#submitSendList').click(function () {
        if ($('#ShipperID').val() === '' || $('#ShipperID').val() === '0') {
            Runbow.TWS.Alert("请选择承运商");
            return;
        }

        //if ($('#ShipperRelatedInfo_Str2').val() === '') {
        //    Runbow.TWS.Alert("请输入收件人，以;间隔");
        //    return;
        //}

        //if ($('#ShipperRelatedInfo_Str7').val() === '') {
        //    Runbow.TWS.Alert("请输入邮件内容");
        //    return;
        //}

        $.send(
            '/System/Shipper/ManageShipperEmailInfo',
            { Type: 2, ProjectID: $('#ProjectID').val(), RelatedCustomerID: $('#RelatedCustomerID').val(), ShipperID: $('#ShipperID').val(), ShipperName: $('#ShipperName').val(), EmailAddress: $('#ShipperRelatedInfo_Str2').val(), EmailContent: $('#ShipperRelatedInfo_Str7').val() },
            function (data) {
                if (data.IsSuccess) {
                    Runbow.TWS.Alert('发送清单邮件信息配置成功');
                }
                else {
                    Runbow.TWS.Alert("发送清单邮件信息配置失败！");
                }
            },
            function () {
                Runbow.TWS.Alert("发送清单邮件信息配置失败！");
            });
    });

    $('#submitRegion').click(function () {
        if ($('#ShipperID').val() === '' || $('#ShipperID').val() === '0') {
            Runbow.TWS.Alert("请选择承运商");
            return;
        }

        if ($('#StartCityID').val() === '' || $('#StartCityID').val() === '0' || $('#StartCityName').val() === '') {
            Runbow.TWS.Alert("请输入起运城市");
            return;
        }

        if ($('#EndCityID').val() === '' || $('#EndCityID').val() === '0' || $('#EndCityName').val() === '') {
            Runbow.TWS.Alert("请输入目的城市");
            return;
        }

        $.send(
                '/System/Shipper/ManageShipperRegionCovered',
                { ProjectID: $('#ProjectID').val(), RelatedCustomerID: $('#RelatedCustomerID').val(), ShipperID: $('#ShipperID').val(), ShipperName: $('#ShipperName').val(), StartCityID: $('#StartCityID').val(), StartCityName: $('#StartCityName').val(), EndCityID: $('#EndCityID').val(), EndCityName: $('#EndCityName').val() },
                function (data) {
                    if (data.IsSuccess) {
                        $('#ShipperRegionCoveredTable tbody').append(
                            '<tr><td>' + data.StartCityName + '</td><td>'
                            + data.EndCityName + '</td><td><label id="deleteShipperRegionCovered" class="labelPointer"  data-ProjectID="'
                            + data.ProjectID + '" data-RelatedCustomerID="'
                            + data.RelatedCustomerID + '" data-ShipperID="'
                            + data.ShipperID + '" data-StartCityID="'
                            + data.StartCityID + '" data-EndCityID="'
                            + data.EndCityID + '">删除</label></td></tr>'
                            );
                        $('#endCityClear').trigger('click');
                        $('#endCityTreeName').focus();
                    }
                    else {
                        Runbow.TWS.Alert("承运商地区配置失败！");
                    }
                },
                function () {
                    Runbow.TWS.Alert("承运商地区配置失败！");
                });
    });

    $('#ShipperRegionCoveredTable').find('tbody label').live('click', function () {
        var tr = $(this).parent().parent();
        $.send(
        '/System/Shipper/DeleteShipperRegionCovered',
        { ProjectID: $(this).attr('data-ProjectID'), RelatedCustomerID: $(this).attr('data-RelatedCustomerID'), ShipperID: $(this).attr('data-ShipperID'), StartCityID: $(this).attr('data-StartCityID'), EndCityID: $(this).attr('data-EndCityID') },
        function (response) {
            if (response.IsSuccess) {
                tr.remove();
            } else {
                Runbow.TWS.Alert("删除承运商地区配置失败！");
            }
        },
        function () {
            Runbow.TWS.Alert("删除承运商地区配置失败！");
        });
    });

    $('#startCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#StartCityID').val('');
        $('#StartCityName').val('');
    });

    $('#endCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#EndCityID').val('');
        $('#EndCityName').val('');
    });

    SetStartCityForHilti();
});

function onRegionSelected(rid, rn, treeId) {
    if (treeId === 'startCity') {
        $('#StartCityID').val($('#startCityTreeID').attr('value'));
        $('#StartCityName').val($('#startCityTreeName').attr('value'));
    } else if (treeId === 'endCity') {
        $('#EndCityID').val($('#endCityTreeID').attr('value'));
        $('#EndCityName').val($('#endCityTreeName').attr('value'));
        $('#submitRegion').focus();
    }
}

function onRegionAutoCompleteSelected(globalID) {
    if (globalID === 'startCity') {
        $('#StartCityID').val($('#startCityTreeID').attr('value'));
        $('#StartCityName').val($('#startCityTreeName').attr('value'));
    } else if (globalID === 'endCity') {
        $('#EndCityID').val($('#endCityTreeID').attr('value'));
        $('#EndCityName').val($('#endCityTreeName').attr('value'));
        $('#submitRegion').focus();
    }
}