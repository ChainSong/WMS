

//盘点差异
$("#CheckButton").live('click', function () {
    openPopup("popCheck", true, 1000, 400, null, 'CheckDiv', true);
    $("#popupLayer_popCheck")[0].style.top = "100px";
});

$("#statusBackReturn").live('click', function () {
    closePopup();
});


function exportDiff(checknumber)
{
    //window.location.href = "/WMS/Warehouse/WareHouseCheckEdit?CheckNumber=" + checknumber + "&flag=导出差异"
}

//完成盘点
function Done() {
    var CheckNumber = $('#SearchCondition_CheckNumber').val();
    layer.confirm('<font size="4">确认是否完成盘点单</font>【' + CheckNumber + "】", {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        var actualQty = TableToJson();
        $.ajax({
            type: "Post",
            url: "/WMS/Warehouse/GetWareHouseCheckDone",
            data: {
                "CheckNumber": CheckNumber,
                "jsonString": actualQty
            },
            async: "false",
            success: function (datasss) {
                if (datasss == "盘点完成") {
                   
                    location.href = "/WMS/Warehouse/WareHouseCheckDetail";
                } else {
                    showMsg(datasss, "4000");
                }
                //location.reload();
            }, error: function (msg) {
                showMsg("查询失败", "4000");
            }

        });
    });
   
   
}


function TableToJson() {
    var isdeall = $('#IS_Deall').val();
    var JsontoModel =
{
    实际数量: 'ActualQty',ID:'ID'
}
    var txt = "[";
    var table = document.getElementById("RoleTable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 0; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if (col[i].innerHTML.trim() == "实际数量") {
                if (isdeall.trim() == "0") {
                    r += "\"" + JsontoModel["实际数量"] + "\"\:" + tds[i].childNodes[0].value.trim() + ",";//.childNodes[1].value.trim() + ",";   $(tds[i]).find("input")[0].value
                }
                else {
                    r += "\"" + JsontoModel["实际数量"] + "\"\:" + tds[i].innerHTML.trim() + ",";// tds[i].childNodes[0].value.trim()
                }
            }
            else if (col[i].innerHTML.trim() == "ID") {
                r += "\"" + JsontoModel["ID"] + "\"\:\"" + tds[i].innerHTML.trim() + "\",";
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
