$(document).ready(function () {
    $('#btnReturn').click(function () {
        window.history.back();
    });
    $('#btnSettledPod').click(function () {
        $("#btnSettledPod").attr("disabled", true);
        return true;
    });
    
});





