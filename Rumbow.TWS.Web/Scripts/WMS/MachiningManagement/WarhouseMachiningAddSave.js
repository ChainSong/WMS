$(document).ready(function () {
    $(".numberCheck").live('keydown', function () {
        replaceNotNumber(this);
    });
    $(".numberCheck").live('keyup', function () {
        replaceNotNumber(this);
    });
    $('#returnButton').live('click', function () {
         history.go(-1);
    });
    $(".SKU").live('dblclick', function () {
        var obj = this;
        openPopup("AsnPop", true, 1000, 600, '/WMS/Product/index/?flag=1&customerID=' + $("#CustomerID").val(), null, function (Sku, GoodsName) {
            
            $(obj).val(Sku);
            $(obj).parent().next().children().val(GoodsName)
            //$('#goodsname' + LineNumber).val(GoodsName);

        });
        $("#popupLayer_AsnPop")[0].style.top = "50px";

    });
    $(".SKUU").live('dblclick', function () {
        var obj = this;
        openPopup("AsnPop", true, 1000, 600, '/WMS/Product/index/?flag=1&customerID=' + $("#CustomerID").val(), null, function (Sku, GoodsName) {
            $(obj).val(Sku);
        });
        $("#popupLayer_AsnPop")[0].style.top = "50px";

    });
    $(".Location").live('dblclick', function () {
        var obj = this;
        openPopup("LocationPop", true, 1000, 600, '/WMS/Warehouse/IndexLocation/?flag=1', null, function (Location) {
            $(obj).val(Location);
            

        });
        $("#popupLayer_LocationPop")[0].style.top = "50px";

    });
    $('#saveButton').live('click', function () {
        if ($('#searchCondition_MachiningNumber').val() == "") {
            showMsg("请输入加工单号！", "4000");
            return;
        }
        var lengths = $('#resultTable')[0].rows.length - 1;
        if (lengths > 0) {
            for (var i = 1; i <= lengths; i++) {

        //        if ($('#resultTable')[0].rows[i].cells[0].children[0].value == '') {
        //            showMsg("SKU不能为空!", 4000);
        //            return;
        //        }
        //        if ($('#resultTable')[0].rows[i].cells[1].children[0].value == '') {
        //            showMsg("品名不能为空!", 4000);
        //            return;
        //        }
                if ($('#resultTable')[0].rows[i].cells[4].children[1].value == '') {
                    showMsg("要求完工时间不能为空!", 4000);
                    return;
                }
        //        if ($('#resultTable')[0].rows[i].cells[7].children[0].value == '') {
        //            showMsg("实收重量不能为空!", 4000);
        //            return;
        //        }
        //        if (parseFloat($('#resultTable')[0].rows[i].cells[7].children[0].value) > parseFloat($('#resultTable')[0].rows[i].cells[6].children[0].value)) {
        //            showMsg("实收重量不能大于预计重量!", 4000);
        //            return;
        //        }
        //        if ($('#resultTable')[0].rows[i].cells[8].children[0].value == '') {
        //            showMsg("冲洗重量不能为空!", 4000);
        //            return;
        //        }
        //        if ($('#resultTable')[0].rows[i].cells[10].children[0].value == '') {
        //            showMsg("冲洗放置库位不能为空!", 4000);
        //            return;
        //        }
        //        if ($('#resultTable')[0].rows[i].cells[10].children[0].value.indexOf('|')<0) {
        //            showMsg("冲洗放置库位格式不正确!", 4000);
        //            return;
        //        }
        //        if ($('#resultTable')[0].rows[i].cells[15].children[0].value == '') {
        //            showMsg("实际灌装桶数不能为空!", 4000);
        //            return;
        //        }
        //        if ($('#resultTable')[0].rows[i].cells[17].children[0].value == '') {
        //            showMsg("灌装放置库位不能为空!", 4000);
        //            return;
        //        }
        //        if ($('#resultTable')[0].rows[i].cells[17].children[0].value.indexOf('|') < 0) {
        //            showMsg("灌装放置库位格式不正确!", 4000);
        //            return;
        //        }
        //        if ($('#resultTable')[0].rows[i].cells[18].children[0].value == '') {
        //            showMsg("余料不能为空!", 4000);
        //            return;
        //        }
        //        if ($('#resultTable')[0].rows[i].cells[20].children[0].value == '') {
        //            showMsg("余料放置库位不能为空!", 4000);
        //            return;
        //        }
                
            }
        }
        var str = "";
        $('.checkForSelect').each(function (index) {
            if ($(this).attr('checked') === 'checked') {
                str += "" + $(this).attr('data-id') + "" + ",";
            }
        })
        var MachiningNumber = $('#searchCondition_MachiningNumber').val();
        var CarOrBoxNumber = $('#searchCondition_CarOrBoxNumber').val();
        var ExpectDate = $('#searchCondition_ExpectDate').val();
        var Tel = $('#searchCondition_Tel').val();
        var CustomerID = $('#CustomerID').val();
        var CustomerName = $('#CustomerName').val();
        var Detail = addjsontotable();
        var IDS = $('#IDS').val();
        var ViewType = $('#ViewType').val();
        var MachiningType = $('#MachiningType').val();
        var IDDS = str;
        $.ajax({
            url: "/WMS/MachiningManagement/WarhouseMachiningAddSave",
            type: "POST",
            dataType: "json",
            data: {
                MachiningNumber: MachiningNumber,
                CarOrBoxNumber: CarOrBoxNumber,
                ExpectDate: ExpectDate,
                Tel: Tel,
                CustomerID: CustomerID,
                CustomerName:CustomerName,
                DetailJson: Detail,
                IDS: IDS,
                MachiningType:MachiningType,
                Flag: 1,
                ViewType: ViewType,
                IDDS: IDDS
               
            },
            success: function (data) {
                if (data.ErrorCode == 0)
                {
                    window.location.href = "/WMS/MachiningManagement/WarhouseMachiningIndex/?ShowSubmit=" + $("#ShowSubmit").val();
                }
                if (data.ErrorCode == 1) {
                    showMsg("新增失败！" + data.OrderInfo, "4000");
                }
            },
            error: function (data, status, e) {
                showMsg("新增失败！" + e, "4000");

            }
        });

    });

    $('#submitButton').live('click', function () {
        if ($('#searchCondition_MachiningNumber').val() == "") {
            showMsg("请输入加工单号！", "4000");
            return;
        }
        var lengths = $('#resultTable')[0].rows.length - 1;
        if (lengths > 0) {
            for (var i = 1; i <= lengths; i++) {

                if ($('#resultTable')[0].rows[i].cells[0].children[0].value == '') {
                    showMsg("SKU不能为空!", 4000);
                    return;
                }
                if ($('#resultTable')[0].rows[i].cells[1].children[0].value == '') {
                    showMsg("品名不能为空!", 4000);
                    return;
                }
                if ($('#resultTable')[0].rows[i].cells[4].children[1].value == '') {
                    showMsg("要求完工时间不能为空!", 4000);
                    return;
                }
                if ($('#resultTable')[0].rows[i].cells[7].children[0].value == '') {
                    showMsg("实收重量不能为空!", 4000);
                    return;
                }
                if (parseFloat($('#resultTable')[0].rows[i].cells[7].children[0].value) > parseFloat($('#resultTable')[0].rows[i].cells[6].children[0].value)) {
                    showMsg("实收重量不能大于预计重量!", 4000);
                    return;
                }
                if ($('#resultTable')[0].rows[i].cells[8].children[0].value == '') {
                    showMsg("冲洗重量不能为空!", 4000);
                    return;
                }
                if ($('#resultTable')[0].rows[i].cells[10].children[0].value == '') {
                    showMsg("冲洗放置库位不能为空!", 4000);
                    return;
                }
                //if ($('#resultTable')[0].rows[i].cells[10].children[0].value.indexOf('|') < 0) {
                //    showMsg("冲洗放置库位格式不正确!", 4000);
                //    return;
                //}
                if ($('#resultTable')[0].rows[i].cells[15].children[0].value == '') {
                    showMsg("实际灌装桶数不能为空!", 4000);
                    return;
                }
                if ($('#resultTable')[0].rows[i].cells[17].children[0].value == '') {
                    showMsg("灌装放置库位不能为空!", 4000);
                    return;
                }
                if ($('#resultTable')[0].rows[i].cells[17].children[0].value.indexOf('|') < 0) {
                    showMsg("灌装放置库位格式不正确!", 4000);
                    return;
                }
                if ($('#resultTable')[0].rows[i].cells[18].children[0].value == '') {
                    showMsg("余料不能为空!", 4000);
                    return;
                }
                if ($('#resultTable')[0].rows[i].cells[20].children[0].value == '') {
                    showMsg("余料放置库位不能为空!", 4000);
                    return;
                }

            }
        }
        layer.confirm('<font size="4">提交后将不能修改，是否确认提交？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var str = "";
            $('.checkForSelect').each(function (index) {
                if ($(this).attr('checked') === 'checked') {
                    str += "" + $(this).attr('data-id') + "" + ",";
                }
            })
            var IDDS = str;
            var MachiningNumber = $('#searchCondition_MachiningNumber').val();
            var CarOrBoxNumber = $('#searchCondition_CarOrBoxNumber').val();
            var ExpectDate = $('#searchCondition_ExpectDate').val();
            var Tel = $('#searchCondition_Tel').val();
            var CustomerID = $('#CustomerID').val();
            var CustomerName = $('#CustomerName').val();
            var Detail = addjsontotable();
            var IDS = $('#IDS').val();
            var ViewType = $('#ViewType').val();
            var MachiningType = $('#MachiningType').val();
            $.ajax({
                url: "/WMS/MachiningManagement/WarhouseMachiningAddSave",
                type: "POST",
                dataType: "json",
                data: {
                    MachiningNumber: MachiningNumber,
                    CarOrBoxNumber: CarOrBoxNumber,
                    ExpectDate: ExpectDate,
                    Tel: Tel,
                    CustomerID: CustomerID,
                    CustomerName: CustomerName,
                    DetailJson: Detail,
                    IDS: IDS,
                    MachiningType: MachiningType,
                    Flag: 2,
                    ViewType: ViewType,
                    IDDS: IDDS
                   
                },
                success: function (data) {
                    if (data.ErrorCode == 0) {
                        window.location.href = "/WMS/MachiningManagement/WarhouseMachiningIndex/?ShowSubmit=true";
                    }
                    if (data.ErrorCode == 1) {
                        showMsg("新增失败！" + data.OrderInfo, "4000");
                    }
                },
                error: function (data, status, e) {
                    showMsg("新增失败！" + e, "4000");

                }
            });
        });
    });
});

function AddRow(objs) {
    var rowIndex = event.srcElement.parentNode.parentNode.rowIndex;//当前行的行号
    var obj = document.getElementById('resultTable').getElementsByTagName('tr');
    var newrow = obj[rowIndex].cloneNode(true);  
    $(objs).parent().parent().after(newrow);
   
};

function DeleteRow(objs)
{
    $(objs).parent().parent().remove();
}


var box = {
    品名: 'GoodsName',
    SKU: 'SKU',
    批号: 'BatchNumber',
    货物类别: 'SKUType',
    要求完工时间: 'ExpectCompleteTime',
    铅封号: 'QianFengNumber',
    预计重量（吨）: 'ExpectWeight',
    实收重量（吨）: 'ActualWeight',
    冲洗数量（吨）: 'WashWeight',
    库存出库其他转入（吨）: 'OtherWeight',
    包装规格: 'PackageType',
    灌装规格（吨）: 'FillingType',
    预计灌装桶数: 'EstimateFillingCount',
    实际灌装桶数: 'ActualFillingBucket',
    余料（吨）: 'MoreThanExpected',
    灌装后合计重量（吨）: 'FillingWeightSUM',
    灌装后合计桶数: 'FillingBucketSUM',
    配比后SKU: 'ProportioningSKU',
    实际损耗（吨）: 'ActualLossWeight',
    实际损耗率: 'ActualLossRate',
    冲洗放置库位: 'WashLocation',
    灌装放置库位: 'Location',
    余料放置库位: 'MoreThanLocation',
    备注: 'Remark',
    冲洗桶规格: 'WashSpecifications',
    灌装桶规格: 'Specifications',
    余料桶规格: 'MoreThanSpecifications'
   
};
//明细表
function addjsontotable() {
    var txt = "[";
    var table = document.getElementById("resultTable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 0; i < col.length-1; i++) {
            var tds = row[j].getElementsByTagName("td");
                if (i == 4) {                
                    r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].childNodes[tds[i].childNodes.length-1].value.trim() + "\",";
                   
                }
                else {
                    r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].childNodes[0].value.trim() + "\",";
                }
            

        }
        r = r.substring(0, r.length - 1)
        r += "},";

        txt += r;
    }

    txt = txt.substring(0, txt.length - 1);
    txt += "]";
    return txt;
}
function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    if (pattern.test(hehe.value)) {

        hehe.value = hehe.value.replace(pattern, "");
    }
}