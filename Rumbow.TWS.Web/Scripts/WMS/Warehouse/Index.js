$(document).ready(function () {
    $(".Ooperation").live("mouseover", function () {
        $(this).prev()[0].style.display = "none";
    });
    $(".Ooperation").live("mouseenter", function () {
        $(this).prev()[0].style.display = "";
    })
    $(".Adiv").live("mouseleave", function () {
        $(this)[0].style.display = "none";
    })
    $("tr").live("mouseenter", function () {
        $(".Adiv").each(function (a, b) {
            $(b)[0].style.display = "none";
        })

    })
    //$(function ($) {
    //    $('body').click(function () {
    //        ShowsOut()
    //    });
    //});
    $('.DropDownList').each(function (index) {
        var id = $(this).attr("id");
    });
    $('select[id=SearchCondition_SearchType]').live('change', function () {
        if ($("#SearchCondition_SearchType").val() == 2) {
            window.location.href = "/WMS/Warehouse/IndexLocation?SearchType=2"
        }
    });


    $("#addButton").live('click', function () {
        //var aa = $('#SearchCondition_SearchType option:selected').val();

        //alert(aa);
        location.href = "/WMS/Warehouse/Create?ViewType=1"
    });

    $("#addAreaButton").live('click', function () {
        location.href = "/WMS/Warehouse/AreaCreate?ViewType=1"
    });
    $("#addLocationButton").live('click', function () {
        location.href = "/WMS/Warehouse/LocationCreate?ViewType=1"
    });

    $("#EndPlaceClear").click(function () {1
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#SearchCondition_ProvinceCity').val('');

    });

    //$('select[id=SearchCondition_ID]').live('change', function () {
    //    if ($(this).val().length > 0)
    //    {
    //        var selec = $(this).val(); //获取改变的选项值

    //        $.ajax({
    //            type: "POST",
    //            url: "/WMS/Warehouse/ChangeWarehouse",
    //            data: {
    //                "str": selec,
    //            },
    //            async: "false",
    //            success: function (data) {

    //                var js = JSON.parse(data);
    //                document.all['SearchCondition_AreaID'].length = 0;
    //                for (var i = 0; i < js.length; i++) {
    //                    document.all['SearchCondition_AreaID'].options.add(new Option(js[i]["AreaName"], js[i]["ID"]));
    //                }
    //            },
    //            error: function (msg) {
    //                alert(msg.val);
    //            }
    //        });

    //    }
    //    else
    //    {
    //        document.all['SearchCondition_AreaID'].length = 0;
    //        document.all['SearchCondition_AreaID'].options.add(new Option("==请选择==", "==请选择=="));
    //    }
    //});

    //编辑
    //$('.warehouseStatus').live('click', function () {
    //    var warehousename = $(this).attr('data-name');
    //    location.href = "/WMS/Warehouse/Create/?ViewType=1&&ID=" + warehousename
    //});
    ///WMS/Warehouse/Create/@warehouse.ID?ViewType=0
    ///WMS/Warehouse/Create/?ViewType=1&&ID=@warehouse.ID  WarehouseName
})
function Edit(ID)
{
    location.href = "/WMS/Warehouse/Create/?ViewType=1&&ID=" + ID
}

function onRegionSelected(rid, rn, treeId) {
    $('#CityID').val($('#CityTreeID').attr('value'));
    $('#CityName').val($('#CityTreeName').attr('value'));
    $('#Warehouse_ProvinceCity').val($('#CityTreeName').attr('value'));  //给字段赋值

    //if (treeId === 'endPlaceTreeKey') {
    //    $('#SearchCondition_ProvinceCity').val($('#ProvinceCity').attr('value'));
    //} 
}

function onRegionAutoCompleteSelected(treeId) {
    if (treeId === 'endPlaceTreeKey') {
        $('#SearchCondition_ProvinceCity').val($('#ProvinceCity').attr('value'));
    }
}

var fileImportClick = function () {
    if ($('select[id=SearchCondition_ID]').val() == '')
    {
        //Runbow.TWS.Alert("请选择仓库！");
        showMsg("请选择仓库！", 4000);
        return false;
    }
    if ($('select[id=SearchCondition_AreaID]').val() == '') {
        //Runbow.TWS.Alert("请选择区域！");
        showMsg("请选择区域！", 4000);
        return false;
    }
    if ($('#importExcel').val() === '') {
        //Runbow.TWS.Alert("请选择要导入的Excel");
        showMsg("请选择要导入的Excel", 4000);
        return false;
    }


    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        //Runbow.TWS.Alert("请选择Excel格式的文件");
        showMsg("请选择Excel格式的文件", 4000);
        //$('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    };

    WebPortal.MessageMask.Show("导入中...");

    $.ajaxFileUpload({
        url: '/WMS/Warehouse/LocationImport',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: { "ID": $('select[id=SearchCondition_ID]').val()},
  
        success: function (data,status) {
            WebPortal.MessageMask.Close();
            if (data.IsSuccess == true) {
                $('#outPutResult').html("<h4><font color='#00dd00'>导入成功！<br/>" + data.result + "</font></h4>");
            } else {
                $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            }
        },
        error: function (data, status, e) {
            $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            WebPortal.MessageMask.Close();
        }
    });
    return false;
};
var fileImportClick2 = function () {
    if ($('select[id=SearchCondition_ID2]').val() == '') {
        //Runbow.TWS.Alert("请选择仓库！");
        showMsg("请选择仓库！", 4000);
        return false;
    }
    if ($('#importExcel2').val() === '') {
        //Runbow.TWS.Alert("请选择要导入的Excel");
        showMsg("请选择要导入的Excel", 4000);
        return false;
    }


    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel2').val()) == null) {
        //Runbow.TWS.Alert("请选择Excel格式的文件");
        showMsg("请选择Excel格式的文件", 4000);
        //$('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    };

    WebPortal.MessageMask.Show("导入中...");

    $.ajaxFileUpload({
        url: '/WMS/Warehouse/LocationAndGoodShelfImport',
        secureuri: false,
        fileElementId: 'importExcel2',
        dataType: 'json',
        data: { "ID": $('select[id=SearchCondition_ID2]').val() },

        success: function (data, status) {
            WebPortal.MessageMask.Close();
            $('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
        },
        error: function (data, status, e) {
            $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            WebPortal.MessageMask.Close();
        }
    });
    return false;
};

function WarehouseDelete(ID, WarehouseName) {
    layer.confirm('<font size="4">确认是否删除仓库【' + WarehouseName + '】？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        if (ID != "") {

            $.ajax({
                type: "POST",
                url: "/WMS/Warehouse/WarehouseDelete",
                data: {
                    "Warehouse_ID": ID,
                },
                async: "false",
                success: function (data) {

                    var js = data;
                    if (js == "True") {
                        $("#" + ID).remove();
                    }

                },
                error: function (msg) {
                    alert(msg.val);
                }

            });
        }
    });

}

//function ShowsIn(ID, obj) {
//    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
//        $(".ddiv:not(obj)").animate({
//            width: "hide",
//            width: "230%",
//            paddingRight: "hide",
//            paddingLeft: "hide",
//            marginRight: "hide",
//            marginLeft: "hide"

//        }, 100);
//        $("#operateTD" + ID).animate({
//            width: "show",
//            width: "278%",
//            paddingRight: "show",
//            paddingLeft: "show",
//            marginRight: "show",
//            marginLeft: "show"
//        });
//    }
//    //$("#operateTD" + ID)[0].style.display = "";
//}

//function ShowsOut() {
//    //$("#operateTD" + ID).fadeOut("slow");

//    $(".ddiv").animate({
//        width: "hide",
//        width: "230%",
//        paddingRight: "hide",
//        paddingLeft: "hide",
//        marginRight: "hide",
//        marginLeft: "hide"

//    }, 100);
//    //$("#operateTD" + ID)[0].style.display = "";
//}


function AddMap(WarehouseID)
{
    location.href = "/WMS/Warehouse/QRCode?WarehouseID=" + WarehouseID
}