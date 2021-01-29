
function onRegionSelected(rid, rn, treeId) {
        $('#CityID').val($('#CityTreeID').attr('value'));
        $('#CityName').val($('#CityTreeName').attr('value'));
        $('#Warehouse_ProvinceCity').val($('#CityTreeName').attr('value'));  //给字段赋值
}

function onRegionAutoCompleteSelected(globalID) {

        $('#CityID').val($('#CityTreeID').attr('value'));
        $('#CityName').val($('#CityTreeName').attr('value'));
        $('#Warehouse_ProvinceCity').val($('#CityTreeName').attr('value'));  //给字段赋值
    
}
$(document).ready(function ()
{
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
    //编辑
    $('.locationStatus').live('click', function () {
        var location = $(this).attr('data-name');
        window.location.href = "/WMS/Warehouse/LocationCreate/?ViewType=2&&ID=" + location + "&&WarehouseID=" + $("#Warehouse_ID").val()
    });

    $('.AreaCreateStatus').live('click', function () {
        var area = $(this).attr('data-id');
        var warehouse = $(this).attr('data-name');
        window.location.href = "/WMS/Warehouse/AreaCreate/?ViewType=2&&ID=" + area + "&&WarehouseID=" + warehouse;
        ///WMS/Warehouse/AreaCreate/?ViewType=2&&ID=@area.ID&&WarehouseID=@Model.Warehouse.ID
    });


    $('#CityTreeName').val($('#Warehouse_ProvinceCity').val()); //初始化进入页面时，更新显示

    ///返回按钮操作
    $('#returnButton').live('click', function () {
        //window.history.back();
        location.href = "/WMS/Warehouse/Index"
    });

    ///跳转到库区界面
    $('#addArea').live('click', function () {
        location.href = "/WMS/Warehouse/AreaCreate?ViewType=1&&WarehouseID=" + $("#Warehouse_ID").val();  ///获取库区ID
    });



    $('#CityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#CityID').val('')
        $('#CityName').val('')
    });

    $("#addWarehouse").live('click', function () {
        location.href = "/WMS/Warehouse/Create?ViewType=1"
    });

    $("#submitButton").live('click',function(){
        if ($('#Warehouse_WarehouseName').val() == "")
        {
            Runbow.TWS.Alert("请输入仓库名称！");
            return false;
        }


    });
});


function LocationDelete(ID, Location) {
    layer.confirm('<font size="4">确认是否删除库位【' + Location + '】？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        if (ID != "") {
            $.ajax({
                type: "POST",
                url: "/WMS/Warehouse/LocationDelete",
                data: {
                    "Location_ID": ID,
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

function AreaDelete(ID, AreaName) {
    layer.confirm('<font size="4">确认是否删除库区【' + AreaName + '】？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        if (ID != "") {

            $.ajax({
                type: "POST",
                url: "/WMS/Warehouse/AreaDelete",
                data: {
                    "Area_ID": ID,
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