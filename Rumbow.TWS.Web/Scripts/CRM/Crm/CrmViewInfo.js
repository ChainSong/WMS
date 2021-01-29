$(document).ready(function () {
    $('#btnReturn').click(function () {
        var typeid = $("#TypeID").val();
        window.location.href = "/CRM/Crm/CrmManage/" + typeid;
    });

    $('#DeleteCrmTrack').live('click', function () {
        var ID = $(this).attr('data-id');
        var fieldset = $(this).parent().parent();
       
        $.send(
            '/CRM/Crm/DeleteCrmTrackinfo',
            { id: ID },
            function (response) {
                if (response.IsSuccess) {
                    fieldset.remove();
                }
                Runbow.TWS.Alert(response.Message);
            },
            function () {
                Runbow.TWS.Alert("删除失败！");
            });


    });

});