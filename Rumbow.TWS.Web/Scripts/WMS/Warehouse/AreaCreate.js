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

    ///返回按钮操作
    //$('#returnButton').live('click', function () {
    //    window.history.back();
    //});
    $('#returnButton').live('click', function () {
        //history.back();
        if ($("#Warehouse_ID").val() == "0") {
            //location.href = "/WMS/Warehouse/Create?ID=" + $("#WarehouseID").val() + "&&ViewType=0";
            history.back();
        }
        else
        {
            location.href = "/WMS/Warehouse/Create?ID=" + $("#Warehouse_ID").val() + "&&ViewType=0";
        }
    });
    ///设计二级联动

    $('#AddArea').live('click', function () {

        if ($('#Area_AreaName').val() == '')
        {
            showMsg("库区名称不能为空！", 4000);
            return false;
        }
        location.href = "/WMS/Warehouse/AreaCreate?ViewType=1&&WarehouseID=" + $("#Warehouse_ID").val();  ///获取库区ID
    });

    $('#submitButton').click(function(){

        if ($('#Area_AreaName').val() == '') {
            showMsg("库区名称不能为空！",4000);
            return false;
        }
    
    })
       

    $('.LocationCreateStatus').live('click', function () {
        var location = $(this).attr('data-id');
        var AreaID = $(this).attr('data-name');

        window.location.href = "/WMS/Warehouse/LocationCreate/?ViewType=2&&ID=" + location + "&&AreaID=" + AreaID + "&&WarehouseID" + $("#Warehouse_ID").val()

        //<a href="/WMS/Warehouse/LocationCreate/?ViewType=2&&ID=@location.ID&&AreaID=@location.AreaID">编辑</a>*@
    });

    
});

function LocationDelete(ID,Location) {
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