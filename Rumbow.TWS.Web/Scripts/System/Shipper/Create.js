$(document).ready(function () {
    $('#SegmentID').change(function () {
        $('#SegmentName').val($('#SegmentID option:selected').text());
    });



 
});