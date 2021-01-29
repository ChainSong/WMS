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
   // closePopup();

   


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
            var hiddenActionButton = $('#HideActionButton').val();
            var showEditRelated = $('#ShowEditRelated').val();
            window.location.href = "/POD/POD/QueryPod/?customerID=" + $(this).val() + "&hideActionButton=" + hiddenActionButton + "&showEditRelated=" + showEditRelated;
            //$('#Pod_CustomerName').val($('#Pod_CustomerID option:selected').text());
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
            $(this).val($('#' + descID).val());
        });
    };
    //$("#preview").click(function () {
    //    if ($('select[id=SearchCondition_CustomerID]').val() != 32)
    //    {
    //       alert("暂只支持此宝胜打印！");
    //        return false;
    //    }
    //})
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
        $('#PageIndex').val('0');
        setPageControlVal();
        $('#IsForExport').val('False');
        $('#ExportType').val('');
    });

    $('#exportButton').click(function () {
        setPageControlVal();
        $('#IsForExport').val('True');
        $('#ExportType').val('Pod');       
    });

    $('#exportAllButton').click(function () {
        setPageControlVal();
        $('#IsForExport').val('True');
        $('#ExportType').val('PodAll');
    });

    $('#exportTrackButton').click(function () {
        setPageControlVal();
        $('#IsForExport').val('True');
        $('#ExportType').val('PodTrack');
    });
    //导出图片
    //$('#exportPhotoButton').click(function () {
    //    //setPageControlVal();
    //    //$('#IsForExport').val('True');
    //    //$('#ExportType').val('PodPhoto');
    //    var startDate = $('#start_ActualDeliveryDate').val();
    //    var endDate = $('#end_ActualDeliveryDate').val();
    //    if (DateDiff(endDate, startDate)> 1)
    //    {
    //        Runbow.TWS.Alert("日期范围不要超过一天");
    //        return false;
    //    }
    //});
    

    $('#exportPhotoButton').live("click", function () {
        var startDate = $('#start_ActualDeliveryDate').val();
        var endDate = $('#end_ActualDeliveryDate').val();
        
        if (endDate.substring(5, 7) - startDate.substring(5, 7) == 1 && endDate.substring(8, 10) - startDate.substring(8, 10) > 0) {
            Runbow.TWS.Alert("日期范围不能超过一个月！");
            return false;
        }
        else if (endDate.substring(0, 4) - startDate.substring(0, 4) >= 1) {
            Runbow.TWS.Alert("日期范围不能超过一个月！");
            return false;
        }
        else
        {
            fjxz();
            //Runbow.TWS.Alert("附件下载中，请耐心等待！")
        }
    });
   

    //if (DateDiff(endDate, startDate) > 1) {
    //    Runbow.TWS.Alert("日期范围不要超过一天");
    //    return false;
    //}
    
    //function DateDiff(endDate, startDate) {

    //    var arrDate, objDate1, objDate2, intDays;

    //    arrDate = endDate.split("-");

    //    objDate1 = new Date(arrDate[1] + '-' + arrDate[2] + '-' + arrDate[0]);

    //    arrDate = startDate.split("-");

    //    objDate2 = new Date(arrDate[1] + '-' + arrDate[2] + '-' + arrDate[0]);
        
    //    intDays = parseInt(Math.abs(objDate1 - objDate2) / 1000 / 60 / 60 / 24);

    //    return intDays;
    //}

    //function GetDateDiff(startDate, endDate) {
    //    var startTime = new Date(Date.parse(startDate.replace(/-/g, "/"))).getTime();
    //    var endTime = new Date(Date.parse(endDate.replace(/-/g, "/"))).getTime();
    //    var dates = Math.abs((startTime - endTime)) / (1000 * 60 * 60 * 24);
    //    return dates;
    //}



    $('.Pager span').click(function () {
        $('#IsForExport').val('False');
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

    $('#resultTable').find('.preview').live('click', function () {
        var podID = $(this).attr('data-id');

        var form = $("<form>");
        form.attr('style', 'display:none');
        form.attr('target', '');
        form.attr('method', 'post');
        form.attr('action', '/Pod/Pod/Preview?id=' + podID + '');
        var input1 = $('<input>');
        input1.attr('type', 'hidden');
        input1.attr('name', 'demo');
        input1.attr('value', 'Export');
        var input2 = $('<input>');
        input2.attr('type', 'hidden');
        input2.attr('name', 'fileId');
        input2.attr('value', "fileId");
        $('body').append(form);
        form.append(input1);
        form.append(input2);

        form.submit();
        form.remove();
        //$.ajax({
        //    url: "/Pod/Pod/Preview",
        //    type: "POST",
        //    //dataType: "json",
        //    data: { id: podID },
        //    success: function (data) {
        //        Runbow.TWS.Alert(data.results);
        //    }
        //    //$.send(
        //    //   '/POD/POD/Preview',
        //    //   { id: podID },
        //    //   function (response) {

        //    //   },
        //    //   function () {
        //    //       Runbow.TWS.Alert("打印预览失败！");
        //    //});
        //})
    })
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

    setHiddenValToControl();
});




function fjxz() {
    openPopup("", true, 150, 100, null, 'PodFJxz', true);
}


