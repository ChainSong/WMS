$(document).ready(function () {
    var clearControl = function () {
        $('#PlateNumber').val('');
        $('#Pilot').val('');
        $('#JobNumber').val('');
        $('#Contract').val('');
        $('#Str1').val('');
        $('#Str2').val('');
        $('#Str3').val('');
        $('#Str4').val('');
        $('#Str7').val('');
    };

    var checkInput = function () {
        if ($('#PlateNumber').val() === '') {
            Runbow.TWS.Alert("请输入车牌号");
            return false;
        }

        return true;
    };

    var disablePlateNumber = function () {
        if ($('#IsEdit').val() === 'True') {
            $('#PlateNumber').attr('disabled', 'disabled');
        } else {
            $('#PlateNumber').removeAttr('disabled');
        }
    };
    
    $('#EditInList').live('click',function () {
        var tds = $('#resultTable tr[data-id=' + $(this).attr('data-id') + '] td');
        $('#PlateNumber').val($(tds[0]).html());
        $('#Pilot').val($(tds[1]).html());
        $('#JobNumber').val($(tds[2]).html());
        $('#Contract').val($(tds[3]).html());
        $('#Str1').val($(tds[4]).html());
        $('#Str2').val($(tds[5]).html());
        $('#Str3').val($(tds[6]).html());
        $('#Str4').val($(tds[7]).html());
        $('#Str7').val($(tds[8]).html());
        $('#VehicleID').val($(this).attr('data-id'));
        $('#IsEdit').val('True');
        disablePlateNumber();
        $('#btnCreateOrUpdate').val('编辑');
    });

    $('#DeleteInList').live('click',function () {
        if (window.confirm("确定删除此车辆？")) {
            var tr = $(this).parent().parent();
            var id = $(this).attr('data-id');
            $.send(
        '/System/Shipper/DeleteVehicle',
        {
            id: id
        },
        function (response) {
            if (response.IsSuccess) {
                if ($('#IsEdit').val() === 'True' && $('#VehicleID').val() === id) {
                    clearControl();
                    $('#VehicleID').val('');
                    $('#IsEdit').val('False');
                    disablePlateNumber();
                    $('#btnCreateOrUpdate').val('新增');
                }
                tr.remove();
            }
        },
        function () {
            
            Runbow.TWS.Alert("删除车辆失败！");
        });
        };
    });

    $('#btnCreateOrUpdate').click(function () {
        if (!checkInput()) {
            return;
        }
        
        $.send(
        '/System/Shipper/AddOrUpdateVehicle',
        {
            isEdit: $('#IsEdit').val(), id: $('#VehicleID').val() === '' ? '0' : $('#VehicleID').val(), shipperID: $('#ShipperID').val(), plateNumber: $('#PlateNumber').val(),
            pilot: $('#Pilot').val(), jobNumber: $('#JobNumber').val(), contract: $('#Contract').val(), str1: $('#Str1').val(), str2: $('#Str2').val(),
            str3: $('#Str4').val(), str4: $('#Str4').val(), str7: $('#Str7').val()
        },
        function (response) {
            if (response.IsSuccess) {
                if (response.IsEdit) {
                    var tds = $('#resultTable tr[data-id=' + response.Vehicle.ID + '] td');
                    $(tds[1]).html(response.Vehicle.Pilot);
                    $(tds[2]).html(response.Vehicle.JobNumber);
                    $(tds[3]).html(response.Vehicle.Contract);
                    $(tds[4]).html(response.Vehicle.Str1);
                    $(tds[5]).html(response.Vehicle.Str2);
                    $(tds[6]).html(response.Vehicle.Str3);
                    $(tds[7]).html(response.Vehicle.Str4);
                    $(tds[8]).html(response.Vehicle.Str7);
                    
                } else {
                    $('#resultTable tbody').append('<tr data-id="' + response.Vehicle.ID + '"><td>' + response.Vehicle.PlateNumber + '</td><td>' + response.Vehicle.Pilot
                    + '</td><td>' + response.Vehicle.JobNumber + '</td><td>' + response.Vehicle.Contract + '</td><td>' + response.Vehicle.Str1 + '</td><td>' + response.Vehicle.Str2
                    + '</td><td>' + response.Vehicle.Str3 + '</td><td>' + response.Vehicle.Str4 + '</td><td>' + response.Vehicle.Str7
                    + '</td><td><a href="#" id="EditInList" data-id="' + response.Vehicle.ID + '">编辑</a> <a href="#" id="DeleteInList" data-id="' + response.Vehicle.ID + '">删除</a></td></tr>');
                }
                clearControl();
                $('#VehicleID').val('');
                $('#IsEdit').val('False');
                disablePlateNumber();
                $('#btnCreateOrUpdate').val('新增');
            }
        },
        function () {
            if ($('#IsEdit').val() === 'False') {
                Runbow.TWS.Alert("新增车辆失败！");
            } else {
                Runbow.TWS.Alert("编辑车辆失败！");
            }
            
        });
    });
});