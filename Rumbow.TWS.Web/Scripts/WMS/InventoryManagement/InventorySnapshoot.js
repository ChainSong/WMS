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
    //$('select[id=CustomerID]').live('change', function () {


    //    window.location.href = "/WMS/InventoryManagement/Index/?customerID=" + $(this).val();
    //});
    $(function ($) {
        $('body').click(function () {
            ShowsOut()
        });
    });
    $('select[id=CustomerID]').live('change', function () {
        //if ($(this).val().length > 0) {
        var selec = $(this).val(); //获取改变的选项值
        document.all['warehousename'].length = 0;
        if (selec != "") {
            $.ajax({
                type: "POST",
                url: "/WMS/InventoryManagement/ChangeCustomer",
                data: {
                    "ID": selec == null ? 0 : selec,
                },
                async: "false",
                success: function (data) {

                    var js = JSON.parse(data);
                    if (js.length != 0) {

                        for (var i = 0; i < js.length; i++) {
                            document.all['warehousename'].options.add(new Option(js[i]["Text"], js[i]["Value"]));
                        }
                    }

                },
                error: function (msg) {
                    alert(msg.val);
                }
            });
        }
        //}
    });

    var setPageControlVal = function () {
        $('.DropDownList').each(function (index) {
            var id = $(this).attr("id");
            var descId = "InventorySearchCondition_" + id.substr(0, id.length - 4) + "ID";
            $('#' + descId).val($(this).val());
        });
        $(".calendarRange").each(function (index) {
            var id = $(this).attr('id');
         
            var actualID = id;

            var descID = 'InventorySearchCondition_';
            
                descID +=  actualID;
          
            $('#' + descID).val($(this).val());
        });
    };



    $('#searchButton').click(function () {
        setPageControlVal();
    });
    $('#OutboundOrder').click(function () {
        setPageControlVal();
    });

    //全选
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }
    });

    //客户  仓库联动
    $('select[id=AdjustmentCondition_CustomerID]').live('change', function () {
        //window.location.href = "/WMS/InventoryManagement/Index/?customerID=" + $(this).val();
    });
    ////仓库  库区联动
    //$('select[id=AdjustmentCondition_Warehouse]').live('change', function () {
    //    //window.location.href = "/WMS/InventoryManagement/Index/?warehouseID=" + $(this).val() + "&customerID=" + $("#AdjustmentCondition_CustomerID").val();

    //});
    ////库区  库位联动
    //$('select[id=AdjustmentCondition_str19]').live('change', function () {
    //    //window.location.href = "/WMS/InventoryManagement/Index/?warehouseID=" + $("#AdjustmentCondition_Warehouse").val() + "&warehouseAreaID=" + $(this).val();
    //});
})
$("#confirmReturn").live('click', function () {
    closePopup();
});
function ShowsOut() {
    //$("#operateTD" + ID).fadeOut("slow");

    $(".ddiv").animate({
        width: "hide",
        width: "400%",
        paddingRight: "hide",
        paddingLeft: "hide",
        marginRight: "hide",
        marginLeft: "hide"

    }, 100);
    //$("#operateTD" + ID)[0].style.display = "";
}