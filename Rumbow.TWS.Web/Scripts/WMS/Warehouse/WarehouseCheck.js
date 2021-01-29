$(document).ready(function () {
    if ($('#vmtype').val() == "1") {
        //全部盘点
        $("#Cone").hide();
        $("#CTwo").hide();
        $("#Tone").hide();
        $("#TTwo").hide();
        $("#trOne").hide();
        $("#Tone5").hide();
        $("#TTwo5").hide();
        //$("#trTwo").hide();
    }
    if ($('#vmtype').val() == "2") {
        //库位盘点
        $("#Cone").show();
        $("#CTwo").show();
        $("#Tone").show();
        $("#TTwo").show();
        $("#Tone5").hide();
        $("#TTwo5").hide();
        //$("#trTwo").hide();
        document.getElementById("Cone").innerText = "开始库位";
        document.getElementById("CTwo").innerText = "结束库位";
        $("#trOne").hide();
    }
    if ($('#vmtype').val() == "3" || $('#vmtype').val() == "10") {
        //品名盘点
        $("#Cone").show();
        $("#CTwo").show();
        $("#Tone").show();
        $("#TTwo").show();
        $("#Tone5").hide();
        $("#TTwo5").hide();
        //$("#trTwo").hide();
        document.getElementById("Cone").innerText = "开始SKU";
        document.getElementById("CTwo").innerText = "结束SKU";
        $("#trOne").hide();
    }
    if ($('#vmtype').val() == "4") {
        //小货量盘点
        $("#Cone").show();
        $("#CTwo").show();
        $("#Tone").show();
        $("#TTwo").show();
        $("#Tone5").hide();
        $("#TTwo5").hide();
        //$("#trTwo").hide();
        document.getElementById("Cone").innerText = "SKU上限数量";
        document.getElementById("CTwo").innerText = "SKU下限数量";
        $("#trOne").hide();
    }
    if ($('#vmtype').val() == "5") {
        //变动库位盘点
        $("#Cone").show();
        $("#CTwo").show();
        $("#Tone").hide();
        $("#TTwo").hide();
        $("#Tone5").show();
        $("#TTwo5").show();
        //$("#trTwo").hide();
        document.getElementById("Cone").innerText = "开始时间";
        document.getElementById("CTwo").innerText = "结束时间";
        $("#trOne").show();
    }
    if ($('#vmtype').val() == "6") {
        //手工录入
        $("#trOne").hide();
        $("#Tone5").hide();
        $("#TTwo5").hide();
        //$("#trTwo").hide();
    }
    //空库位盘点
    if ($("#vmtype").val() == "8") {
        $("#Cone").hide();
        $("#CTwo").hide();
        $("#Tone").hide();
        $("#TTwo").hide();
        $("#trOne").hide();
        $("#Tone5").hide();
        $("#TTwo5").hide();
    }
    //客户  仓库联动
    $('select[id=SearchCondition_CustomerID]').live('change', function () {
        window.location.href = "/WMS/Warehouse/WareHouseCheck/?customerID=" + $(this).val() + "&externNumber=" + $('#SearchCondition_ExternNumber').val();
    });
    //仓库  库区联动
    $('select[id=SearchCondition_Warehouse]').live('change', function () {
        window.location.href = "/WMS/Warehouse/WareHouseCheck/?warehouseID=" + $(this).val() + "&customerID=" + $('select[id=SearchCondition_CustomerID]').val() + "&externNumber=" + $('#SearchCondition_ExternNumber').val();
    });
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
                descID += 'Start' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    }
    $('select[id=SearchCondition_Type]').live('change', function () {
        $("#SearchCondition_str1").val("");
        $("#SearchCondition_str2").val("");
        $("#SearchCondition_str3").val("");
        $("#SearchCondition_str4").val("");
        if ($(this).val() == "1") {
            //全部盘点
            $("#Cone").hide();
            $("#CTwo").hide();
            $("#Tone").hide();
            $("#TTwo").hide();
            $("#trOne").hide();
            $("#Tone5").hide();
            $("#TTwo5").hide();
            //$("#trTwo").hide();
        }
        if ($(this).val() == "2") {
            //库位盘点
            $("#Cone").show();
            $("#CTwo").show();
            $("#Tone").show();
            $("#TTwo").show();
            $("#Tone5").hide();
            $("#TTwo5").hide();
            document.getElementById("Cone").innerText = "开始库位";
            document.getElementById("CTwo").innerText = "结束库位";
            $("#trOne").hide();
            //$("#trTwo").hide();
        }
        if ($(this).val() == "3" || $(this).val() == "10") {
            //品名盘点
            $("#Cone").show();
            $("#CTwo").show();
            $("#Tone").show();
            $("#TTwo").show();
            $("#Tone5").hide();
            $("#TTwo5").hide();
            document.getElementById("Cone").innerText = "开始SKU";
            document.getElementById("CTwo").innerText = "结束SKU";
            $("#trOne").hide();
            //$("#trTwo").hide();
        }
        if ($(this).val() == "4") {
            //小货量盘点
            $("#Cone").show();
            $("#CTwo").show();
            $("#Tone").show();
            $("#TTwo").show();
            $("#Tone5").hide();
            $("#TTwo5").hide();
            document.getElementById("Cone").innerText = "SKU上限数量";
            document.getElementById("CTwo").innerText = "SKU下限数量";
            $("#trOne").hide();
            //$("#trTwo").hide();
        }
        if ($(this).val() == "5") {
            //变动库位盘点
            $("#Cone").show();
            $("#CTwo").show();
            $("#Tone").hide();
            $("#TTwo").hide();
            $("#Tone5").show();
            $("#TTwo5").show();
            document.getElementById("Cone").innerText = "开始时间";
            document.getElementById("CTwo").innerText = "结束时间";
            $("#trOne").show();
            //$("#trTwo").hide();
        }
        if ($(this).val() == "6") {
            //手工录入
            $("#trOne").hide();
            $("#Tone5").hide();
            $("#TTwo5").hide();
            //$("#trTwo").hide();
        }
        //空库位盘点
        if ($(this).val() == "8") {
            $("#Cone").hide();
            $("#CTwo").hide();
            $("#Tone").hide();
            $("#TTwo").hide();
            $("#trOne").hide();
            $("#Tone5").hide();
            $("#TTwo5").hide();
        }
    });

    //提交查询按钮并返回数据
    $("#searchButton").click(function () {
        if ($("#SearchCondition_CustomerID option:selected").val() == "") {
            showMsg("请选择客户！", "2000");
            return false;
        }
        if ($("#SearchCondition_Warehouse option:selected").val() == "") {
            showMsg("请选择仓库！", "2000");
            return false;
        }
        //门店盘点需选择库区，库区对应门店
        if (($('#SearchCondition_Type').val() == '9' || $('#SearchCondition_Type').val() == '10') && $("#SearchCondition_Area option:selected").val() == "") {
            showMsg("请选择库区！", "2000");
            return false;
        }
        //门店及品名盘点需选择库区
        if ($('#SearchCondition_Type').val() == '10'&& ($('#SearchCondition_str1').val() == "" || $('#SearchCondition_str2').val() == "")) {
            showMsg("请输入品名！", "2000");
            return false;
        }
        if ($('#SearchCondition_Type').val() == '2' || $('#SearchCondition_Type').val() == '3' || $('#SearchCondition_Type').val() == '4') {
            if ($('#SearchCondition_str1').val() == "") {
                showMsg("请检查库位或品名或SKU数量！", "2000");
                return false;
            }
        }
        if ($('#SearchCondition_Type').val() == '5') {
            var chk_value = [];
            $('input[name="roles"]:checked').each(function () {
                chk_value.push($(this).val());
            });
            if(chk_value.length == 0)
            {
                showMsg("业务类型至少勾选一项！", "2000");
                return false;
            }
        }
        setPageControlVal();
    });
    //提交保存按钮并返回数据
    $("#SaveButton").click(function () {
        var ExtNumbmer = $('#SearchCondition_ExternNumber').val();
        var rows = $("#RoleTable")[0].rows.length;
        if (ExtNumbmer == '') {
            showMsg("外部盘点号不能为空！", "2000");
            return false;
        }
        if ($("#SearchCondition_Checkdate").val() == "") {
            showMsg("请选择日期！", "2000");
            return false;
        }
        if ($("#SearchCondition_CustomerID option:selected").val() == "") {
            showMsg("请选择客户！", "2000");
            return false;
        }
        if ($("#SearchCondition_Warehouse option:selected").val() == "") {
            showMsg("请选择仓库！", "2000");
            return false;
        }
        if ($("#SearchCondition_Area option:selected").val() == "") {
            showMsg("请选择库区！", "2000");
            return false;
        }
        if (rows == '1') {

            showMsg("保存失败，没有盘点明细！", "2000");
            return false;
        }
    })
});