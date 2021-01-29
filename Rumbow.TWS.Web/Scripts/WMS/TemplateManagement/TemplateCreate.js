$(document).ready(function () {
    $("#ReturnButton").click(function () {
        window.location.href = "/WMS/TemplateManagement/index"
    });
    $("#UpdateButton").click(function () {
        var Jsonstring = addjsontotable();
        var ProjectID = $("#searchCondition_ProjectID").val();
        var CustomerID = $("#searchCondition_CustomerID").val();
        var TableName = $("#searchCondition_TableName").val();
        var ProjectName = $("#searchCondition_ProjectName").val();
        var CustomerName = $("#searchCondition_CustomerName").val();
        var TableNameCH = $("#searchCondition_TableNameCH").val();
        $.ajax({
            type: "Post",
            url: "/WMS/TemplateManagement/TemplateCreate",
            data: {
                "jsonString": Jsonstring,
                "ProjectID": ProjectID,
                "CustomerID": CustomerID,
                "TableName": TableName               
            },
            async: "false",
            success: function (data) {

                location.href = "/WMS/TemplateManagement/TemplateCreate/?TableName=" + TableName + "&TableNameCH="
                    + TableNameCH + "&ProjectName=" + ProjectName + "&CustomerName=" + CustomerName
                + "&ProjectID=" + ProjectID + "&CustomerID=" + CustomerID;
                showMsg("修改成功!", "4000");
            },
            error: function (msg) {
                showMsg("修改失败！", "4000");
            }
        });
    });
});
var box = {
    ID:'ID',
    显示名称: 'DisplayName',
    是否关键字段: 'IsKey',
    是否隐藏: 'IsHide',
    是否显示在列表: 'IsShowInList',
    是否是导入列: "IsImportColumn",
    是否是查询字段: 'IsSearchCondition',
    是否显示: 'ForView',
    字段类型: 'Type',
    显示顺序: 'Order'
};

function addjsontotable() {
    var txt = "[";
    var table = document.getElementById("resultTable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("th");

    for (var j = 1; j < row.length; j++) {
        var r = "{";
        for (var i = 0; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if (i != 0&&i!=8&&i!=1&&i!=10&&i!=9) {
                if (tds[i].children.length > 0) {
                    if (tds[i].childNodes.length > 1) {
                        r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].childNodes[1].checked + "\",";
                    }
                    
                }
            }
            if (i == 1) {
                if (tds[i].children.length > 0) {
                    r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].childNodes[1].value + "\",";
                }
                else {
                    r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].innerText.trim() + "\",";
                }
            }
            if (i == 8)
            {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].childNodes[1].value + "\",";
            }
            if (i == 9) {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].childNodes[1].value + "\",";
            }
            if (i == 10) {
                r += "\"" + box[col[i].innerHTML.trim()] + "\"\:\"" + tds[i].innerText.trim() + "\",";
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