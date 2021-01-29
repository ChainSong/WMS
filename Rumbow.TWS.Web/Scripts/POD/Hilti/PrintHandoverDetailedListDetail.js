
$(document).ready(function () {

    
    $(".ExprotStyle").attr("disabled", "disabled");

    var trs = $("#btSave tr");
    var count = 0;
    for (var i = 1; i < trs.length; i++) {
        var Packagelength = $($(trs[i]).find('td:eq(4) input[type=text]')).length
        var Shippelength = $($(trs[i]).find('td:eq(5) input[type=text]')).length
        count += Packagelength + Shippelength;
    }

    if (count > 0) {
        $(".printStyle").attr("disabled", "disabled");
    }
    else
    {
        $(".printStyle").attr("disabled", false);
    }

    

});

var tSave = function () {


    $(".printStyle").attr("disabled", false);
    $(".ExprotStyle").attr("disabled", false);


    var arr = new Array();
    var trs = $("#btSave tr");
   
    for (var i = 1; i < trs.length; i++) {
       
       
        //var ID = $(trs[i]).find(".IDCss").val();
        //var PackagesNumber = $(trs[i]).find('.Packages').val();
        //var ShippeNO = $(trs[i]).find('.ShippeNum').val();
        var packagelength = $($(trs[i]).find('td:eq(4) input[type=text]')).length
        var shipperlength = $($(trs[i]).find('td:eq(5) input[type=text]')).length
        if (packagelength>0)
        {
            var packagesnum = $(trs[i]).find('.Packages').val();
            $($(trs[i]).find('td:eq(4)')).text('');
            $($(trs[i]).find('td:eq(4)')).text(packagesnum);
        }

        if (shipperlength > 0) {
            var packagesnum = $(trs[i]).find('.ShippeNum').val();
            $($(trs[i]).find('td:eq(5)')).text('');
            $($(trs[i]).find('td:eq(5)')).text(packagesnum);
        }


        var ID = $(trs[i]).find(".IDCss").val();
        var PackagesNumber = $($(trs[i]).find('td:eq(4)')).text();
        
        var ShippeNO = $($(trs[i]).find('td:eq(5)')).text();

        var jsonstr = { ID: ID, PackagesNumber: PackagesNumber, ShippeNO: ShippeNO };
       arr.push(jsonstr);
    }
    
    $.send('/POD/Hilti/PODColumnsInfoUpdate', { array: JSON.stringify(arr) },
        function (response)
        {
            if (response.IsSuccess) {
                Runbow.TWS.Alert(response.Message);
            }
            else
            {
                Runbow.TWS.Alert(response.Message);
            }
      }, function ()
      {
                Runbow.TWS.Alert("保存失败！");
      });
}


var SendEmail = function (datetime, ShipperName, ShipperID) {

    //alert(datetime);
    //alert(ShipperName);
    $.send('/POD/Hilti/GenerateAnnexAndSendEmail', { datetime: datetime, ShipperName: ShipperName, ShipperID: ShipperID },
        function (response) {
            if (response.IsSuccess) {
                Runbow.TWS.Alert(response.Message);
            }
            else {
                Runbow.TWS.Alert(response.Message);
            }
        }, function () {
            Runbow.TWS.Alert("系统错误！");
        });
}





var EditValue = function (obj)
{
    var value = $(obj).val();
    var d = $(obj).parent();
    d.text(value);
    $(obj).val(value);
    $(obj).hide();
}



var editInput = function ()
{
    var trs = $("#btSave tr");

    for (var i = 1; i < trs.length; i++) {
        var packagelength = $($(trs[i]).find('td:eq(4) input[type=text]')).length
        var shipperlength = $($(trs[i]).find('td:eq(5) input[type=text]')).length
        if (packagelength==0)
        {
            var packagesnum = $($(trs[i]).find('td:eq(4)')).text();
            $($(trs[i]).find('td:eq(4)')).text('');

            $($(trs[i]).find('td:eq(4)')).append('<input  type="text" value="" style="width:35px;" class="Packages" onblur="EditValue(this)"/>');
            $($(trs[i]).find('td:eq(4) input[type=text]')).val(packagesnum);
        }

        if (shipperlength == 0) {
            var shipperno = $($(trs[i]).find('td:eq(5)')).text();
            $($(trs[i]).find('td:eq(5)')).text('');
            $($(trs[i]).find('td:eq(5)')).append('<input  type="text" value="" style="width:35px;" class="ShippeNum" onblur="EditValue(this)"/>');
            $($(trs[i]).find('td:eq(5) input[type=text]')).val(shipperno);
        }
      
      

       

        
    }
}






