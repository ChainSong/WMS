$(document).ready(function () {
    $('#resultTable').find('.DelOrReuseSegment').live('click', function () {
        var message = $(this).attr('state') === 'True' ? '您确认停用此段位？' : '您确认启用此段位？';
        return confirm(message);
    });
});