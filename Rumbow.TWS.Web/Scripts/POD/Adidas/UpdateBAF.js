$(document).ready(function () {
    $('#searchButton').click(function () {
        setPageControlVal();
    });
    $('#addButton').click(function () {
        window.location.href = '/POD/Adidas/AddBAF';
    });
    
    $('.calendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = 'BAF';
        if (pref === 'start') {
            descID += 'Start' + actualID;
        }
        else {
            descID += 'End' + actualID;
        }
        $(this).val($('#' + descID).val());
    });
    var setPageControlVal = function () {
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID = 'BAF';
            if (pref === 'start') {
                descID += 'Start' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    }
  
})

function del(del) {
    $.send(
   '/POD/Adidas/Edit',
   {
       id: del.id,
   },
   function (response) {
       if (response) {
           $(del).parent().parent().remove();
       }
       else {
           alert("操作失败！");
       }
   })
}