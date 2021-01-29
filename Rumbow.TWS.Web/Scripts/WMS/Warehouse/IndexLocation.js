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

    $('.DropDownList').each(function (index) {

        var id = $(this).attr("id");      
    });
    $('select[id=WLSearchCondition_SearchType]').live('change', function () {
   
        if ($("#WLSearchCondition_SearchType").val() == 1) {
            window.location.href = "/WMS/Warehouse/Index?SearchType=1"
        }
       
    });
    if ($('#searchFlag').val() == 1) {

        $("#resultTableLocation tr").click(function () {
            $(this).addClass("btn-success").siblings("tr").removeClass("btn-success");
        });
        $("#resultTableLocation tr").mouseover(function () {
            $(this).addClass("btn-info").siblings("tr").removeClass("btn-info");
    });

        $("#resultTableLocation tr").mouseleave(function () {
            $(this).removeClass("btn-info");
        });
        $("#resultTableLocation tr").dblclick(function () {
            //var rowIndex = $("#resultTable tr").index($(this));
            var Location = $(this).children()[1].innerText.trim() + "|" + $(this).children()[0].innerText.trim();

            closePopup(Location);

        });
    }
    //var warehousename = $(this).attr('data-name');
    //编辑
    $('.LocationStatus').live('click', function () {
        var location = $(this).attr('data-name');
        window.location.href = "/WMS/Warehouse/LocationCreate/?ViewType=2&&ID=" + location + "&&WLSearchCondition_SearchType=2";
        //

    });

    //$("#addButtonLocation").live('click', function () {
    //    location.href = "/WMS/Warehouse/LocationCreate?ViewType=1"
    //});

    $('#addButtonLocation').live('click', function () {     
        location.href = "/WMS/Warehouse/LocationCreate?" + "AreaID=" + $("#Area_ID").val() + "&&ViewType=1";
    });

    $('select[id=WLSearchCondition_WarehouseID]').live('change', function () {
        if ($(this).val().length > 0) {
            var selec = $(this).val(); //获取改变的选项值
            $.ajax({
                type: "POST",
                url: "/WMS/Warehouse/ChangeWarehouse",
                data: {
                    "str": selec,
                },
                async: "false",
                success: function (data) {

                    var js = JSON.parse(data);
                    document.all['WLSearchCondition_AreaID'].length = 0;
                    for (var i = 0; i < js.length; i++) {
                        document.all['WLSearchCondition_AreaID'].options.add(new Option(js[i]["AreaName"], js[i]["ID"]));
                    }
                },
                error: function (msg) {
                    alert(msg.val);
                }
            });
        }       
    });

    $('#searchButtonLocation').live('click', function () {
        $('#PageIndex').val(0);
    });
    })



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