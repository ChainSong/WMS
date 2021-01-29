$(document).ready(function () {
    ///删除操作
    $("#delOperate").live('click', function () {
        if (confirm("是否确认删除？")) {
            var item = $(this).attr('data-name');
            var tr = $(this).parent().parent();
            $.send(
               '/ContractManagement/Contract/DeleteContract',
               { id: item },
            function (response) {
                if (response.IsSuccess) {
                    tr.remove();
                    var temp = $("Legend")[1].innerHTML;  //获取
                    reg = /[0-9]+/;
                    var a = temp.match(reg);
                    temp = temp.replace(reg, (parseInt(a[0]) - 1));
                    $("Legend")[1].innerHTML = temp;

                }
                Runbow.TWS.Alert(response.Message);
            },
            function () {
                Runbow.TWS.Alert("删除合同失败！");
            });
        }
    });

    
    ///顺延操作
    $("#delayOperate").live('click', function () {

        var tr = $(this).parent().parent();
        if (tr[0].all[8].innerText.length < 1)
        {
            alert("请填写实际到期日期，再顺延！")
            return;
        }
        if (confirm("是否确认顺延？")) {
            var item = $(this).attr('data-name');
            
            $.send(
               '/ContractManagement/Contract/ContractExtension',
               { id: item },
            function (response) {
                if (response.IsSuccess) {
                    var temp = tr[0].all[8].innerText;
                    var ptemp = temp.substring(0, 4);
                    var newptemp = parseInt(ptemp) + 1;
                    temp = temp.replace(ptemp, newptemp);
                    tr[0].all[8].innerText = temp;
                }
                Runbow.TWS.Alert(response.Message);

            },
            function () {
                Runbow.TWS.Alert("顺延合同失败！");
            });
        }
    });

    ///单选框的选项保持
    $("#SearchType1").live("change", function ()
    {
        $("#SearchType1").val($(":input[type='radio']:checked").val());
    });

    ///单选框的选项保持
    $("#SearchType2").live("change", function () {
        $("#SearchType2").val($(":input[type='radio']:checked").val());
    });
});