function onRegionSelected(rid, rn, treeId) {
        $('#SearchCondition_StartCityID').val($('#startCityTreeID').attr('value'));
        $('#CRMInfo_City').val($('#CityTreeName').attr('value'));
}

$(document).ready(function () {
    var setHiddenValToControl = function () {
        $('#CityTreeName').val($('#CRMInfo_City').val());
    }

    $('#CityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#CRMInfo_City').val('')
    });

    $('#CrmInfoTable').find('.DeleteCrm').live('click', function () {
        var crmID = $(this).attr('data-id');
        var tr = $(this).parent().parent();
        $.send(
            '/CRM/Crm/DeleteCrminfo',
            { id: crmID },
            function (response) {
                if (response.IsSuccess) {
                    tr.remove();

                }
                Runbow.TWS.Alert(response.Message);
            },
            function () {
                Runbow.TWS.Alert("删除失败！");
            });
        
     
    });


    $('#CrmInfoTable').find('#UpdateCrm').live('click', function () {
        var crmID = $(this).attr('data-id');
        var tr = $(this).parent().parent();
        window.location.href = "/CRM/Crm/CrmBasView2/" + crmID;
        //$.send(
        //    '/CRM/Crm/CrmBasView2',
        //    { id: crmID });
        
    });


   
    setHiddenValToControl();
});