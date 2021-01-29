$(document).ready(function () {



    $('#returnButton').live('click', function () {
        window.location.href = "/WMS/MachiningManagement/WarhouseMachiningIndex/";
    });

});

function Machining(IDS, Qty, CustomerID, CustomerName, SKU, GoodsName)
{
    window.location.href = "/WMS/MachiningManagement/WarhouseMachiningAddSave/?IDS=" + IDS + "&Qty=" + Qty + "&CustomerID=" + CustomerID + "&CustomerName=" + CustomerName + "&Flag=1" + "&SKU=" + SKU + "&GoodsName=" + GoodsName+"&ShowSubmit=" + $("#ShowSubmit").val();;
}