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
    $('#returnButton').live('click', function () {
        history.back();
    })
    $(".Locationcheck").live('dblclick', function () {
        var obj = this;
        //.next().next().next().next()[0];
        openPopup("LocationPop", true, 1000, 600, '/WMS/Warehouse/IndexLocation/?flag=1', null, function (Location) {
            $(obj).val(Location);
        });
        $("#popupLayer_LocationPop")[0].style.top = "50px";
    });
    $('#saveButton').live('click', function () {
        if ($("#SearchCondition_CustomerID").val() == "") {
            showMsg("请选择客户！", "4000");
            return;
        }
        if ($("#SearchCondition_WareHouseID").val() == "") {
            showMsg("请选择仓库！", "4000");
            return;
        }
        
        if ($("#SearchCondition_GoodsShelvesName").val() == "") {
            showMsg("请输入货架名称！", "4000");
            return;
        }
        if ($('#rowAndcellTable')[0].rows.length == 1)
        {
            showMsg("请设置货架层列！", "4000");
            return;
        }
        //if ($("#SearchCondition_Levels").val() == "") {
        //    showMsg("请输入层数！", "4000");
        //    return;
        //}
        if ($("#LocationTable tr").length > 0) {
            var tr = document.getElementById('LocationTable').getElementsByTagName('tr');
            for (var i = 1; i < $("#LocationTable tr").length; i++) {
                if ($(tr[i]).children(".Locationcheck2").children("input")[0].value == "")
                {
                    showMsg("请输入库位！", "4000");
                    return;
                }
                if ($(tr[i]).children(".LevelsNumber").children("input")[0].value == "")
                {
                    showMsg("请输入第几层！", "4000");
                    return;
                }
                if ($(tr[i]).children(".SerialNumber").children("input")[0].value == "")
                {
                    showMsg("请输入第几格！", "4000");
                    return;
                }
            }
        }
        var JsonString = FieldSetToJson();
        var LocationJson = addjsontotable();
        var RowAndCelljsons = RowAndCelljson();
        $.ajax({
            type: "Post",
            url: "/WMS/Warehouse/GoodsShelvesCreate",
            data: {
                "JsonString": JsonString,
                "LocationJson":LocationJson,
                "ViewType": $('#ViewType').val(),
                "ID": $('#SearchCondition_ID').val() == "" ? 0 : $('#SearchCondition_ID').val(),
                "CustomerID": $("#SearchCondition_CustomerID").val(),
                "WarehouseID": $("#SearchCondition_WareHouseID").val(),
                "Rows": $('#rowAndcellTable')[0].rows.length-1,
                "RowAndCelljsons": RowAndCelljson
            },
            async: "false",
            success: function (data) {
                if (data = "OK") {
                    location.href = "/WMS/Warehouse/GoodsShelves/";
                    showMsg("操作成功!", "4000");
                }
            },
            error: function (msg) {
                showMsg("操作失败！", "4000");
            }
        });
    });
});
function FieldSetToJson() {
    var JsontoModel =
{
    SearchCondition_CustomerID: 'CustomerID',
    SearchCondition_WareHouseID: 'WarehouseID',
    SearchCondition_GoodsShelvesName: 'GoodsShelvesName',
    //SearchCondition_Levels: 'Levels',
    //SearchCondition_Grids:'Grids',
    SearchCondition_Weights:'Weights',
    SearchCondition_Lengths:'Lengths',
    SearchCondition_Widths:'Widths',
    SearchCondition_Heights: 'Heights',
}
    var txt = "[";
    var table = document.getElementById("table_body");
    var row = table.getElementsByTagName("tr");
    if (row.length > 1) {
        var r = "{";
        for (var j =0; j < row.length; j++) {
            var col = row[j].getElementsByTagName("td");

            for (var i = 0; i < col.length; i++) {
                var tds = row[j].getElementsByTagName("td");
                if (tds[i].className.trim() != "TableColumnTitle") {
                    //if ($(tds[i]).children("select").length > 0) {
                        r += "\"" + JsontoModel[tds[i].childNodes[1].id.trim()] + "\"\:\"" + tds[i].childNodes[1].value + "\",";
                    //}
                    //else {
                    //    r += "\"" + JsontoModel[tds[i].childNodes[0].id.trim()] + "\"\:\"" + tds[i].childNodes[0].value + "\",";
                    //}
                }
            }
        }
        r = r.substring(0, r.length - 1)
        r += "}";
        txt += r;
    }
    txt += "]";
    return txt;
}

function AddNew(obj)
{
    var table1 = $('#LocationTable');
    var td0 = $(" <td style='position: relative'>\
                              <div class='Adiv' style='position: absolute; display: none; width: 500px; left: 50px; height: 31px;'>\
                                  <label id='labelRemove' style='cursor: pointer;' class='btn btn-primary btn-xs   labelRemove' onclick='Del(this)'>删除</label>\
                                  <label style='cursor: pointer;' class='btn btn-primary btn-xs   addNew' onclick='AddNew(this)'>添加</label>\
                              </div>\
                              <label style='cursor: pointer;' class='btn btn-primary btn-xs Ooperation'>操作</label>\
                          </td>");
    var row = $("<tr></tr>");
    var td1 = $("<td class='Locationcheck2'></td>");
    var td2 = $("<td class='LevelsNumber'></td>");
    var td3 = $("<td class='SerialNumber'></td>");
    td1.append($("<input type='text'  class='form-control  Locationcheck'  value=''/>"));
    td2.append($("<input type='text'  class='form-control'  value='' onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)'/>"));
    td3.append($("<input type='text'  class='form-control'  value=''onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)'/>"));
    row.append(td0);
    row.append(td1);
    row.append(td2);
    row.append(td3);
    table1.append(row);
}

function AddNewCells(obj) {
    var table1 = $('#rowAndcellTable');
    var lengths = table1[0].rows.length;
    var td0 = $(" <td style='position: relative'>\
                              <div class='Adiv' style='position: absolute; display: none; width: 500px; left: 50px; height: 31px;'>\
                                  <label id='labelRemove' style='cursor: pointer;' class='btn btn-primary btn-xs   labelRemove' onclick='DelCells(this)'>删除</label>\
                                  <label style='cursor: pointer;' class='btn btn-primary btn-xs   AddNewCells' onclick='AddNewCells(this)'>添加</label>\
                              </div>\
                              <label style='cursor: pointer;' class='btn btn-primary btn-xs Ooperation'>操作</label>\
                          </td>");
    var row = $("<tr></tr>");
    var td1 = $("<td class='Rows'></td>");
    var td2 = $("<td class='Cells'></td>");
    td1.append($("<input type='text'  class='form-control   Locationcheck'  onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)'  value=" + lengths + ">"));
    td2.append($("<input type='text'  class='form-control'  value=1 onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)'/>"));
    row.append(td0);
    row.append(td1);
    row.append(td2);
    table1.append(row);
}

function Del(obj)
{
    $(obj).parent().parent().parent().remove();
}
function DelCells(obj) {
    $(obj).parent().parent().parent().remove();
    var lengths = $('#rowAndcellTable')[0].rows.length;
    var table = document.getElementById("rowAndcellTable");
    var row = table.getElementsByTagName("tr");
    for (var i = 1; i < lengths; i++)
    {
        var tds = row[i].getElementsByTagName("td");
        $(tds[1]).children("input")[0].value = i;
    }
}

function addjsontotable() {
    var box =
{
    '库区|库位': 'Location', 第几层: 'LevelsNumber', 第几格: 'SerialNumber'
}
    var txt = "[";
    var table = document.getElementById("LocationTable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 1; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if ($(tds[i]).children("input").length > 0) {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("input")[0].value.trim() + "\",";
            } else if ($(tds[i]).children("select").length > 0) {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("select").find('option:selected').val().trim() + "\",";
            } else {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].innerText.trim().trim() + "\",";
            }


        }
        r = r.substring(0, r.length - 1)
        r += "},";
        txt += r;
    }
   
    txt = txt.substring(0, txt.length - 1);
    if (txt.length > 0) {
        txt += "]";
    }
    return txt;
}
function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    //var re = new RegExp(pattern);
    if (pattern.test(hehe.value)) {

        hehe.value = hehe.value.replace(pattern, "");
    }
}

function RowAndCelljson() {
    var box =
{
    第几层: 'RowNumber', 格数: 'CellNumber'
}
    var txt = "[";
    var table = document.getElementById("rowAndcellTable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 1; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if ($(tds[i]).children("input").length > 0) {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("input")[0].value.trim() + "\",";
            } else if ($(tds[i]).children("select").length > 0) {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + $(tds[i]).children("select").find('option:selected').val().trim() + "\",";
            } else {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].innerText.trim().trim() + "\",";
            }
        }
        r = r.substring(0, r.length - 1)
        r += "},";
        txt += r;
    }

    txt = txt.substring(0, txt.length - 1);
    if (txt.length > 0) {
        txt += "]";
    }
    return txt;
}