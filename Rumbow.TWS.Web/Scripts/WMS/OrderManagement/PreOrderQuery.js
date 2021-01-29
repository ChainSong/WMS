$(document).ready(function () {
    $("#PreTable tr").click(function () {
        $(this).addClass("btn-success").siblings("tr").removeClass("btn-success");
    });
    $("#PreTable tr").mouseover(function () {
        $(this).addClass("btn-info").siblings("tr").removeClass("btn-info");
    });

    $("#PreTable tr").mouseleave(function () {
        $(this).removeClass("btn-info");
    });
    $("#PreTable tr").dblclick(function () {
        var PreID = $(this).children()[4].innerText;
        closePopup(PreID);
    });
});