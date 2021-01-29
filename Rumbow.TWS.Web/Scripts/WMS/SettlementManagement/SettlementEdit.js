//盘点差异
$("#SettlementButton").live('click', function () {
    openPopup("popSettlement", true, 1000, 400, null, 'SettlementDiv', true);
    $("#popupLayer_popSettlement")[0].style.top = "100px";
});

$("#statusBackReturn").live('click', function () {
    closePopup();
});


function exportDiff(Settlementnumber)
{
    window.location.href = "/WMS/SettlementManagement/SettlementEdit?SettlementNumber=" + Settlementnumber + "&flag=导出差异"
}

//完成盘点
function Done() {
    var SettlementNumber = $('#SearchCondition_SettlementNumber').val();
    layer.confirm('<font size="4">确认是否完成盘点单</font>【' + SettlementNumber + "】", {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        var actualQty = TableToJson();
        $.ajax({
            type: "Post",
            url: "/WMS/SettlementManagement/GetSettlementDone",
            data: {
                "SettlementNumber": SettlementNumber,
                "jsonString": actualQty
            },
            async: "false",
            success: function (datasss) {
                if (datasss == "结算完成") {
                   
                    location.href = "/WMS/SettlementManagement/SettlementDetail";
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
                if (tds[i].childNodes.length > 1) {
                    r += "\"" + JsontoModel["实际数量"] + "\"\:" + tds[i].childNodes[1].value.trim() + ",";
                }
                else {
                    r += "\"" + JsontoModel["实际数量"] + "\"\:" + tds[i].childNodes[0].value.trim() + ",";
                }
            }
            else if (col[i].innerHTML.trim() == "ID")
            {
                if (tds[i].childNodes.length > 1) {
                    r += "\"" + JsontoModel["ID"] + "\"\:\"" + tds[i].childNodes[1].value.trim() + "\",";
                }
                else {
                    r += "\"" + JsontoModel["ID"] + "\"\:\"" + tds[i].innerHTML.trim() + "\",";
                }
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
