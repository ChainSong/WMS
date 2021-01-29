$(document).ready(function () {

    $('select[id=Location_LocationType]').live('change', function () {
        if ($("#Location_LocationType").val() == 2) {
            $('.GoodsShelf')[0].style.display = "none";
        }
        else {
            $('.GoodsShelf')[0].style.display = "";
        }
    });


    ///提交前验证
    $('#submitButton').live('click', function ()
    {
        var WarehouseValidate = $('#Warehouse_ID option:selected')[0];
        var AreaValidate = $('#Area_ID option:selected')[0];
        if (typeof(WarehouseValidate) != "undefined") {
            if (WarehouseValidate.value == "") {
                showMsg("仓库信息不能为空！", 4000);
                //alert("仓库信息不能为空")
                return false;
            }
        }
        else {
            if ($('#ViewType').val() == 1) {
                //只有新增的时候，验证下拉框不为空
                return false;
            }
        }
         
        if (typeof(AreaValidate) != "undefined") {
            if (AreaValidate.value == "") {
                showMsg("库区信息不能为空！", 4000);
                //alert("库区信息不能为空");
                return false;
            }
        }
        else {
            if ($('#ViewType').val() == 1) {
                //只有新增的时候，验证下拉框不为空
                return false;
            }
        }

        if ($('#Location_Location').val() == '')
        {
            showMsg("库位名称不能为空！", 4000);
            return false;
        }

    })

    ///返回按钮操作
    //$('#returnButton').live('click', function () {
    //    window.history.back();
    //});
    $('#returnButton').live('click', function () {

        if ($('#WLSearchCondition_SearchType').val() == 2)
        {
            //window.history.back();
            window.location.href = "/WMS/Warehouse/IndexLocation?SearchType=2"
        }
        else if ($('#WLSearchCondition_SearchType').val() == "")
        {
            history.back();
        }
        else
        {
            location.href = "/WMS/Warehouse/AreaCreate?ID=" + $("#Area_ID").val() + "&&WarehouseID=" + $("#Warehouse_ID").val() + "&&ViewType=2";
        }
    });
    $('#AddLocation').live('click', function () {
        location.href = "/WMS/Warehouse/LocationCreate?"+"AreaID="+$("#Area_ID").val() + "&&ViewType=1";
    });

    var num = 0;

    ///仓库下拉选中
    $('#Warehouse_ID').live('change', function () {       //当仓库ID的值发生变化时，方法被激活。
        var url = "/WMS/Warehouse/UpdateWarehouseAreaList";
        var url2 = "/WMS/Warehouse/UpdateWarehouseGoodsShelfList";
        var data = 'WarehouseID=' + $("#Warehouse_ID").val() + '&num=' + num;
        $.getJSON(url, data, function (data) {
            $('#Area_ID').html('');
            $('#Area_ID').empty(); //先清空之前的绑定数据
            $('#Area_ID').append($("<option></option>").val("").html("---请选择---"));              //加入请选择
            $.each(data, function (id, item) {
                //循环获取对应的数据，绑定在控件名为plan_id_sch 的dropdownlist下
                $('#Area_ID').append($("<option></option>").val(item.ID).html(item.AreaName));
            });
            num++;
        });
        $.getJSON(url2, data, function (data) {
            $('#GoodsShelf_ID').html('');
            $('#GoodsShelf_ID').empty(); //先清空之前的绑定数据
            $('#GoodsShelf_ID').append($("<option></option>").val("").html("---请选择---"));              //加入请选择
            $.each(data, function (id, item) {
                //循环获取对应的数据，绑定在控件名为plan_id_sch 的dropdownlist下
                $('#GoodsShelf_ID').append($("<option></option>").val(item.ID).html(item.GoodsShelvesName));
            });
            num++;
        });
    });

    ///库区下拉选中
    $("#Area_ID").live('change', function ()
    {
        
    })



});