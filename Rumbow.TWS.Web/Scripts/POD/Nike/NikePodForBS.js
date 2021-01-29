$(document).ready(function () {

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
            $('#ChangeCarriers').removeAttr('disabled');
            $('#allocateShipperID').val(ui.item.data.Value);
            $('#allocateShipperAutoComplete').val(ui.item.data.Text);

        }
    });
    //$('select[id=Condition_IsConversion]').live('change', function () {

    //    $("#NikeForBS")[0].style.display =  $("#NikeForBS")[0].style.display== "none" ? "" : "none";
    //    $("#Cancel")[0].style.display =  $("#Cancel")[0].style.display =="none" ? "" : "none";
    //    $("#allocateShipperAutoComplete")[0].style.display = $("#allocateShipperAutoComplete")[0].style.display == "none" ? "" : "none";
    //    $("#ChangeCarriers")[0].style.display = $("#ChangeCarriers")[0].style.display == "none" ? "" : "none";

    //});

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].type == "checkbox") {
                if (!checkBoxs[i].disabled) {
                    checkBoxs[i].checked = this.checked ? true : false;
                }
            }
        }

        //if ($(this).attr("checked") === "checked") {


        //    checkBoxs.attr("checked", "checked");
        //} else {
        //    checkBoxs.removeAttr("checked");
        //}
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
    //$("#allocateShipperID").blur(function () {
    //    $('#allocateShipperAutoComplete').removeAttr('disabled');
    //})

    $("#NikeForBS").click(function () {
     
      
        var Choose = IsChoose();
        if (Choose == "") {
            $("#ChangeCarriers").popover();
            return;
        }
        openPopup("", true, 300, 250, null, "Popu");
        //$("#ChangeCarriers").popover('hide');
        //var allocateShipperAutoComplete = $("#allocateShipperAutoComplete").val();
        //var allocateShipperID = "";
        //if (allocateShipperAutoComplete != "") {
        //    allocateShipperID = $("#allocateShipperID").val();
        //}
        //var table = document.getElementById("BodyTable");
        //var row = table.getElementsByTagName("tr");
        //var col = row[0].getElementsByTagName("th");
        //for (var j = 0; j < row.length; j++) {
        //    if (row[j].getElementsByTagName("td")[0].childNodes[1].checked) {
        //        row[j].getElementsByTagName("td")[3].innerText = allocateShipperAutoComplete;
        //        row[j].getElementsByTagName("td")[13].innerText = allocateShipperID;
        //    }
        //}
    })
    //是否勾选
    function IsChoose() {
        var Choose = "";
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                Choose += "" + checkBoxs[i].id.toString() + ",";
            }
        }
        if (Choose.length > 0) {
            Choose = Choose.toString().substring(0, Choose.toString().length - 1);
        }
        return Choose;
    }

    $("#Cancel").click(function () {
        var a = TableToJson("resultTable", "已转运单");
        $.ajax({
            url: "/POD/Nike/CancelNikePodForBS",
            type: "POST",

            data: {
                "str": a
                //"name": rowIndex,
            },
            success: function (data) {
                if (data == "删除成功") {
                    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
                    for (var i = 0; i < checkBoxs.length; i++) {
                        if (checkBoxs[i].checked == true) {
                            $("#" + checkBoxs[i].id).parent().parent().remove()
                        }
                      //  $("#550007").remove();
                    }
                }
            },
            error: function (data) {
                Runbow.TWS.Alert('失败');

            }
        })
    })
    // $("#searchButton").click(function () {
    //var Choose = IsChoose();
    //var startDate = $("#start_DeliveryTime").val();
    //var endDate = $("#end_DeliveryTime").val();
    //if (DateDiff(endDate, startDate) > 3) {
    //    alert("日期范围不要超过三天");
    //    return false;
    //}
    //if (Choose == "") {
    //    $("#NikeForBS").popover();
    //    return;
    // }
    //    $("#NikeForBS").popover('hide');
    //})
    $("#NOsubmit").click(function () {//NikeForBS
        closePopup();
    })
    function Carriers()
    {
        var state = $("#Condition_Carriers option:selected").text();
        $("#ChangeCarriers").popover('hide');
      

        var allocateShipperAutoComplete = $("#allocateShipperAutoComplete").val();
        if (state != "更改承运商") {
            allocateShipperAutoComplete = "";
        }
        var allocateShipperID = "0";
        if (allocateShipperAutoComplete != "") {
            allocateShipperID =parseInt($("#allocateShipperID").val());
        }
        var table = document.getElementById("BodyTable");
        var row = table.getElementsByTagName("tr");
        var col = row[0].getElementsByTagName("th");
        for (var j = 0; j < row.length; j++) {
            if (row[j].getElementsByTagName("td")[0].childNodes[1].checked && row[j].getElementsByTagName("td")[10].innerText.trim() != "已转运单") {
                row[j].getElementsByTagName("td")[3].innerText = allocateShipperAutoComplete;
                row[j].getElementsByTagName("td")[13].innerText = allocateShipperID;
            }
        }

    }
    $('select[id=Condition_Carriers]').live('change', function () {
        if (this.value == "1") {
            $("#Condition_Carriersdrop_down")[0].style.display = "";
        } else {
            $("#Condition_Carriersdrop_down")[0].style.display = "none";
        }
        //$("#NikeForBS")[0].style.display =  $("#NikeForBS")[0].style.display== "none" ? "" : "none";
        //$("#Cancel")[0].style.display =  $("#Cancel")[0].style.display =="none" ? "" : "none";
        //$("#allocateShipperAutoComplete")[0].style.display = $("#allocateShipperAutoComplete")[0].style.display == "none" ? "" : "none";
        //$("#ChangeCarriers")[0].style.display = $("#ChangeCarriers")[0].style.display == "none" ? "" : "none";
    });


    $("#OKsubmit").click(function () {//NikeForBS

        $("#NikeForBS").popover('hide');
        closePopup();//$("#tesetSelect").find("option:selected").text();$("#Condition_IsConversion option:selected").text();
        var state = $("#Condition_Carriers option:selected").text();
        if (state != "沿用原承运商") {
                Carriers();
        }
        var a = TableToJson("resultTable", "未转运单");
        if (a.length < 50) {
            alert("未选择需要转换运单！");
            return false;
        }
        $.ajax({
            url: "/POD/Nike/AddNikePodForBS",
            type: "POST",
            data: {
                "str": a,
                "ShipperName": $("#allocateShipperAutoComplete").val()
                //"name": rowIndex,
            },
            success: function (data) {
                if (data == "运单转换成功") {
                    var table = document.getElementById("BodyTable");
                    var row = table.getElementsByTagName("tr");
                    var col = row[0].getElementsByTagName("th");
                    for (var j = 0; j < row.length; j++) {
                        if (row[j].getElementsByTagName("td")[0].childNodes[1].checked) {
                            row[j].getElementsByTagName("td")[10].innerText = "已转运单";
                        }
                    }
                    alert(data);
                }

            },
            error: function (data) {
                Runbow.TWS.Alert('失败');

            }
        })

    })


    function DateDiff(endDate, startDate) {

        var arrDate, objDate1, objDate2, intDays;

        arrDate = endDate.split("-");

        objDate1 = new Date(arrDate[1] + '-' + arrDate[2] + '-' + arrDate[0]);

        arrDate = startDate.split("-");

        objDate2 = new Date(arrDate[1] + '-' + arrDate[2] + '-' + arrDate[0]);

        intDays = parseInt(Math.abs(objDate1 - objDate2) / 1000 / 60 / 60 / 24);

        return intDays;

    }
})

var box = {
    系统运单号: 'SystemNumber',
    客户运单号: 'CustomerOrderNumber',
    客户: 'CustomerName', 承运商: 'ShipperName', 运单状态: 'PODStateName',
    运输类型: 'ShipperTypeName', 起运城市: 'StartCityName',
    目的城市: 'EndCityName', 运单类型: 'PODTypeName', 发货日期: 'ActualDeliveryDate',
    ID: 'ID',
    客户ID: 'CustomerID', 承运商ID: 'ShipperID', 箱数: 'BoxNumber', 重量: 'Weight', 件数: 'GoodsNumber',
    体积: 'Volume', ShipperTypeID: "ShipperTypeID", Str1: "Str1", Str2: "Str2", Str3: "Str3", Str4: "Str4", Str5: "Str5", Str6: "Str6",
    Str7: "Str7", Str8: "Str8", Str9: "Str9", PODTypeID: "PODTypeID", PODTypeName: "PODTypeName", TtlOrTplID: "TtlOrTplID",
    TtlOrTplName: "TtlOrTplName"
};
function TableToJson(tableid, state) {
    var txt = "[";
    var table = document.getElementById(tableid);
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var tds = row[j].getElementsByTagName("td");
        if (tds[10].innerText.trim() == state) {
            var r = "{";
            if (row[j].getElementsByTagName("td")[0].childNodes[1].checked) {
                for (var i = 0; i < col.length; i++) {
                    if ( i == 1 || i == 3  || i == 11 || i == 13) {

                        var innerVal = '';
                        innerVal = tds[i].innerText.trim();

                        r += "\"" + box[col[i].innerText.trim()] + "\"\:\"" + innerVal + "\",";
                    }

                }
                r = r.substring(0, r.length - 1)
                r += "},";
                txt += r;
            }
        }
    }
    txt = txt.substring(0, txt.length - 1);
    txt += "]";
    return txt;
}






