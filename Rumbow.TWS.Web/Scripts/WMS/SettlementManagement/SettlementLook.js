$(document).ready(function () {
   
});

function Returns() {
    //location.href = history.back();//多层级页面跳转，准确跳回所在菜单主页
    var url = $(window.parent.document).find(".active a").attr('href');
    url = url.toString().split(',')[2];
    url = url.substring(1, url.length - 2);
    location.href = url;
}