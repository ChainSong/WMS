$(document).ready(function () {

    //挂车选择
    $('select[id=CRMVehicle_CarType]').live('change', function () {
        if ($("#CRMVehicle_CarType").val() == '挂车') {
            $("#guache").show();
        } else {
            $("#guache").hide();
        }
    });

    if ($("#CRMVehicle_CarType").val() == "挂车") {
        $("#guache").show();
    } else {
        $("#guache").hide();
    }


    //提交
    $('#submitButton').click(function () {
        if (checkInput()) {
            return true;
        } else {
            return false;
        }
    });

    //查询返回
    $('#returnButton').live('click', function () {
        window.location.href = '/ShipperManagement/VehicleManagement/Index?useSession=true';
    });

    //$('#returnbutton').live('click',function () {
    //    window.history.go(-1);

    //});

});


//输入信息
var checkInput = function () {

    //判断车牌号码是否为空
    if ($('#CRMVehicle_CarNo').val() == "") {
        Runbow.TWS.Alert("请输入车牌号码");
        return false;
    }

    //限制输入的车牌号码为7位
    if ($('#CRMVehicle_CarNo').val().length < 7 || $('#CRMVehicle_CarNo').val().length > 7) {
        Runbow.TWS.Alert("请输入正确的7位车牌号码");
        return false;
    }
    
    //判断营运证号是否为空
    if ($('#CRMVehicle_RunNo').val() == "") {
        Runbow.TWS.Alert("请输入营运证号");
        return false;
    }
    //判断车型编码是否为空 你
    if ($('#CRMVehicle_CarTypeNo').val() == "") {
        Runbow.TWS.Alert("请输入车型编码");
        return false;
    }
    //判断车辆VIN是否为空
    if ($('#CRMVehicle_CarVin').val() == "") {
        Runbow.TWS.Alert("请输入车辆VIN");
        return false;
    }
    if ($('#CRMVehicle_CarVin').val().length < 17) {
        Runbow.TWS.Alert("请输入正确的17位车辆VIN")
        return false;
    }

    //判断物流公司是否为空
    if ($('#CRMVehicle_LogisticCompany').val() == "") {
        Runbow.TWS.Alert("请输入物流公司");
        return false;
    }
    //判断物流公司安全专员联系电话是否为空
    if ($('#CRMVehicle_SecurityContactNum').val() == "") {
        Runbow.TWS.Alert("请输入物流公司安全专员联系电话");
        return false;
    }

    //判断已行驶公里数是否为空
    if ($('#CRMVehicle_DrivedJourney').val() == "") {
        Runbow.TWS.Alert("请输入已行驶公里数");
        return false;
    }

    if ($('#CRMVehicle_DrivedJourney').val() == 0) {
        Runbow.TWS.Alert("请输入已行驶公里数");
        return false;
    }
    //判断资质是否为空
    if ($('#CRMVehicle_Qualify').val() == "") {
        Runbow.TWS.Alert("请输入资质");
        return false;
    }


    //判断车身颜色是否为空
    if ($('#CRMVehicle_CarBodyColor').val() == "") {
        Runbow.TWS.Alert("请输入车身颜色");
        return false;
    }
    //判断生产厂家是否为空
    if ($('#CRMVehicle_Manufacturer').val() == "") {
        Runbow.TWS.Alert("请输入生产厂家");
        return false;
    }

    //判断整备质量是否为空
    if ($('#CRMVehicle_EntireCarWeight').val() == "") {
        Runbow.TWS.Alert("请输入整备质量");
        return false;
    }

    if ($('#CRMVehicle_EntireCarWeight').val() == 0) {
        Runbow.TWS.Alert("请输入整备质量");
        return false;
    }

    if ($('#CRMVehicle_BoardlotDate').val() == "") {
        Runbow.TWS.Alert("请选择上牌日期");
        return false;
    }

    if ($('#CRMVehicle_NextYearCheckDate').val() == "") {
        Runbow.TWS.Alert("请选择下次年检日期");
        return false;
    }

    if ($('#CRMVehicle_StartServiceDate').val() == "") {
        Runbow.TWS.Alert("请选择加入服务时间");
        return false;
    }

    if ($('#CRMVehicle_InsuranceEndDate').val() == "") {
        Runbow.TWS.Alert("请选择保险有效截至日期");
        return false;
    }

    return true;
}






//联系电话为数字且最大长度为11位
$(function () {
    $('#CRMVehicle_SecurityContactNum').keyup(function () {
        this.value = this.value.replace(/[^\d]/g, '')
    });
    $('#CRMVehicle_SecurityContactNum').attr({ maxlength: "11" });
});

//已行驶公里数
$(function () {
    $('#CRMVehicle_DrivedJourney').keyup(function () {
        this.value = this.value.replace(/[^\d]/g, '')
    });
});

//整备质量
$(function () {
    $('#CRMVehicle_EntireCarWeight').keyup(function () {
        this.value = this.value.replace(/[^\d]/g, '')
    });
});

//限制资质输入为文字
$(function () {
    $('#CRMVehicle_Qualify').keyup(function () {
        this.value = this.value.replace(/[^\u4E00-\u9FA5]/, '')
    })
});

////限制物流公司输入为文字
//$(function () {
//    $('#CRMVehicle_LogisticCompany').keyup(function () {
//        this.value = this.value.replace(/[^\u4E00-\u9FA5]/, '')
//    })
//});

//限制车身颜色输入为文字
$(function () {
    $('#CRMVehicle_CarBodyColor').keyup(function () {
        this.value = this.value.replace(/[^\u4E00-\u9FA5]/, '')
    })
});

////限制生产厂家输入为文字
//$(function () {
//    $('#CRMVehicle_Manufacturer').keyup(function () {
//        this.value = this.value.replace(/[^\u4E00-\u9FA5]/, '')
//    })
//});


//车辆VIN为17位
$(function () {

    $('#CRMVehicle_CarVin').attr({ maxlength: "17" });

});

//核载为数字
$(function () {
    $('#CRMVehicle_LoadWeight').keyup(function () {
        this.value = this.value.replace(/[^\d]/g, '')
    });
});

//核定人数为数字
$(function () {
    $('#CRMVehicle_LoadPerson').keyup(function () {
        this.value = this.value.replace(/[^\d]/g, '')
    });
});

//总质量
$(function () {
    $('#CRMVehicle_TotalWeight').keyup(function () {
        this.value = this.value.replace(/[^\d]/g, '')
    });
});

//牵引总质量
$(function () {
    $('#CRMVehicle_TractionWeight').keyup(function () {
        this.value = this.value.replace(/[^\d]/g, '')
    });
});


