$(document).ready(function () {
    $("#ReturnButton").click(function () {
        window.location.href = "/System/Customer/Index"
    });
});

function editTemplate(ProjectID, CustomerID, TableName, TableNameCH)
{
    var ProjectName = $("#searchCondition_ProjectID option:selected").text();
    var CustomerName = $("#searchCondition_CustomerID option:selected").text();
    window.location.href = "/WMS/TemplateManagement/TemplateCreate/?ProjectName=" + ProjectName + "&CustomerName=" + CustomerName + "&TableName=" + TableName + "&TableNameCH=" + TableNameCH + "&ProjectID=" + ProjectID + "&CustomerID=" + CustomerID;
}