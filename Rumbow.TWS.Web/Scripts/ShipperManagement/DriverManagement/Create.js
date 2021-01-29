$(document).ready(function () {

    $('#submitButton').click(function () {
        if (checkInput()) {
            return true;
        } else {
            return false;
        }
    });
             

     //$('#returnButton').live('click', function () {
     //    window.location.href = '/ShipperManagement/DriverManagement/Index?useSession=true';

     //});

     //$('#returnbutton').live('click', function () {
     //    window.location.href = '/ShipperManagement/DriverManagement/Index';
     //});

    //查询返回
     $('#returnButton').live('click', function () {
         window.location.href = '/ShipperManagement/DriverManagement/Index?useSession=true';
     });

    ////提交返回
    // $('#returnbutton').live('click', function () {
    //     window.location.href = '/shippermanagement/driverManagement/Index';
    // });
});


   



//输入信息
var checkInput = function () {
    if ($('#CreateDriver_DriverName').val() == "") {
        Runbow.TWS.Alert("请输入司机姓名");
        return false;
    }
    

    if ($('#CreateDriver_DriverBirthday').val() == "") {
        Runbow.TWS.Alert("请选择出生日期");
        return false;
    }

    if ($('#CreateDriver_DriverPhone').val() == "") {
        Runbow.TWS.Alert("请输入联系电话");
        return false;
    }

    if ($('#CreateDriver_DriverStartServeForRunbowDate').val() == "") {
        Runbow.TWS.Alert("请选择开始为服务时间");
        return false;
    }

    if ($('#CreateDriver_DriverIDCard').val() == "") {
        Runbow.TWS.Alert("请输入身份证号码");
        return false;
    }
    if ($('#CreateDriver_DriverIDCard').val().length != 15 && $('#CreateDriver_DriverIDCard').val().length != 18) {
        Runbow.TWS.Alert("请输入正确的15位或者18位身份证号码");
        return false;
    }

    if ($('#CreateDriver_DriverCardNo').val() == "") {
        Runbow.TWS.Alert("请输入驾驶证档案号");
        return false;
    }

    if ($('#CreateDriver_DriverLogisticsCompany').val() == "") {
        Runbow.TWS.Alert("请输入物流公司");
        return false;
    }


    if ($('#CreateDriver_DriverLogisticsContactPerson').val() == "") {
        Runbow.TWS.Alert("请输入物流公司联系人");
        return false;
    }

    

    if ($('#CreateDriver_DriverLogisticsCompanyContactPhone').val() == "") {
        Runbow.TWS.Alert("请输入物流公司联系电话");
        return false;
    }

    if ($('#CreateDriver_DriverCarNo').val() == "") {
        Runbow.TWS.Alert("请输入驾驶车辆牌号");
        return false;
    }
    if ($('#CreateDriver_DriverCarNo').val().length > 7 || $('#CreateDriver_DriverCarNo').val().length < 7){
        Runbow.TWS.Alert("请输入正确的车辆牌号");
        return false;
    }

    if ($('#CreateDriver_DriverRegistrationNo').val() == "") {
        Runbow.TWS.Alert("请输入司机登记号");
        return false;
    }

    if ($('#CreateDriver_DriverRegistrationCardSignedAddress').val() == "") {
        Runbow.TWS.Alert("请输入登记证签发地");
        return false;
    }

    if ($('#CreateDriver_DriverNextYearCheckDate').val() == "") {
        Runbow.TWS.Alert("请选择下次年审日期");
        return false;
    }

    if ($('#CreateDriver_DriverFirstTimeGetCardDate').val() == "") {
        Runbow.TWS.Alert("请选择初次驾照领证日期");
        return false;
    }

    if ($('#CreateDriver_DriverNextYearCheckBodyDate').val() == "") {
        Runbow.TWS.Alert("请选择下次体检日期");
        return false;
    }

    if ($('#CreateDriver_DriverServiceArea').val() == "") {
        Runbow.TWS.Alert("请输入服务区域");
        return false;
    }


    if ($('#CreateDriver_DriverMainRoute').val() == "") {
        Runbow.TWS.Alert("请输入主要行驶路线");
        return false;
    }

    
    return true;
}

//联系电话
$(function () {
    $('#CreateDriver_DriverPhone').keyup(function () {
        this.value = this.value.replace(/[^\d]/g, '')
    });
    $('#CreateDriver_DriverPhone').attr({ maxlength: "11" });
});

//身份证号码
$(function () {
    
////    if ($('#CreateDriver_DriverIDCard').val().length == 15 || $('#CreateDriver_DriverIDCard').val().length == 18) {
////        return true;
////    }
////    else {
////        alert("请输入正确的身份证号码");
////        return false;
////    }
    $('#CreateDriver_DriverIDCard').attr({ maxlength: "18" });
});


////限制物流公司输入为文字
//$(function () {
//    $('#CreateDriver_DriverLogisticsCompany').keyup(function () {
//        this.value = this.value.replace(/[^\u4E00-\u9FA5]/, '')
//    })
//});

//物流公司联系电话
$(function () {
    $('#CreateDriver_DriverLogisticsCompanyContactPhone').keyup(function () {
        this.value = this.value.replace(/[^\d]/g, '')
    });
    //$('#CreateDriver_DriverLogisticsCompanyContactPhone').attr({ maxlength: "11" });
});

////限制司机姓名为文字
//$(function () {
//$('#CreateDriver_DriverName').keyup(function () {
//    this.value = this.value.replace(/[^\u4E00-\u9FA5]/, '')
//})
//});

////登记证签发地
//$(function () {
//    $('#CreateDriver_DriverRegistrationCardSignedAddress').keyup(function () {
//        this.value = this.value.replace(/[^\u4E00-\u9FA5]/, '')
//    })
//});