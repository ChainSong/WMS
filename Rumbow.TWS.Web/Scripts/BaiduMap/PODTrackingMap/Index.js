var table;
$(document).ready(function () {

    var KSlng = 108.504661;
    var KSlat = 34.444429;
    // 百度地图API功能
    var map = new BMap.Map("allmap");
    var points = new BMap.Point(KSlng, KSlat);
    map.centerAndZoom(points, 6);
    map.enableScrollWheelZoom(true);//能否缩放
    map.addEventListener("tilesloaded", function () {
        $(".BMap_cpyCtrl").remove(); //去掉百度左下角的字
    });


    var myIconblue = new BMap.Icon("/Image/BaiduMap/blue.png", new BMap.Size(36, 36));
    var myIcongreen = new BMap.Icon("/Image/BaiduMap/green.png", new BMap.Size(36, 36));
    var myIconred = new BMap.Icon("/Image/BaiduMap/red.png", new BMap.Size(36, 36));
    var myIconredWD = new BMap.Icon("/Image/BaiduMap/WD.png", new BMap.Size(36, 36));
    var points = [];
    var opts = {
        width: 250,     // 信息窗口宽度
        height: 150,     // 信息窗口高度
        //title: "信息窗口", // 信息窗口标题
        enableMessage: true//设置允许信息窗发送短息
    };
    // 编写自定义函数,创建标注
    //  function addMarker(point){
    //      var marker = new BMap.Marker(point);
    //      map.addOverlay(marker);
    //  }
    // 随机向地图添加25个标注
    //	var bounds = map.getBounds();
    //	var sw = bounds.getSouthWest();
    //	var ne = bounds.getNorthEast();
    //	var lngSpan = Math.abs(sw.lng - ne.lng);
    //	var latSpan = Math.abs(ne.lat - sw.lat);

    function addClickHandler(content, marker) {
        marker.addEventListener("mouseover", function (e) {
            openInfo(content, e)
        }
        );
        //		marker.addEventListener("mouseover",function(e){ })
    }
    function openInfo(content, e) {
        var p = e.target;
        var point = new BMap.Point(p.getPosition().lng, p.getPosition().lat);
        var infoWindow = new BMap.InfoWindow(content, opts);  // 创建信息窗口对象 
        map.openInfoWindow(infoWindow, point); //开启信息窗口
    }
    //获取覆盖物位置
    function attribute(a, e) {
        var Type = "";
        if (a == "中转运单") {
            Type = "Hub";
        }
        $.ajax({
            //type: "POST",
            url: "/BaiduMap/PODTrackingMap/CarInfoPOD",
            data: {
                CarNo: e,
                Type: Type
            },
            //async: false,
            success: function (data) {
                if (data.code = 1) {
                    table = "<div class='body-nest' id='tableStatic'><section id='flip-scroll'><table style='font-size:small;' class='table table-bordered table-striped cf'><thead class='cf'><tr><th>运单号</th><th class='numeric'>收货客户</th><th>目的地</th> <th class='numeric'>发货日期</th><th class='numeric'>预计到达日期</th><th class='numeric'>箱数</th><th class='numeric'>件数</th><th class='numeric'>操作</th></tr></thead><tbody>";
                    //循环填充表数据
                    for (i = 0; i < data.PODTrackingMap.length; i++) {
                        table += "<tr><td class='numeric'>" + data.PODTrackingMap[i].CustomerOrderNumber + "</td><td class='numeric'>" + data.PODTrackingMap[i].ReceivingCustomer + "</td><td class='numeric' title=" + data.PODTrackingMap[i].Destination + " style='display: block; width:335px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;'>" + data.PODTrackingMap[i].Destination + "</td><td class='numeric'>" + data.PODTrackingMap[i].ActualDeliveryDate + "</td><td class='numeric'>" + data.PODTrackingMap[i].EstimatedArrivalDate + "</td><td class='numeric'>" + data.PODTrackingMap[i].BoxNumber + "</td><td class='numeric'>" + data.PODTrackingMap[i].GoodsNumber + "</td><td class='numeric'><a data-carno=" + data.PODTrackingMap[i].CustomerOrderNumber + " data-type='POD' onclick='PartialView(this)' href='javascript:void(0);'>轨迹回放</a></td></tr>  ";
                    }
                    table += "</tbody></table> </section></div>";

                    layer.open({
                        type: 1,
                        title: '运输详情       <span style="margin-left:20px;color:red;">运单总数</span><<<b style="color:#336699;">' + data.PODTrackingMap.length + '</b>>>',
                        shadeClose: true,
                        shade: false,
                        maxmin: true, //开启最大化最小化按钮
                        area: ['893px', '600px'],
                        content: table
                    });
                }
            },
            error: function (msg) {
            }
        });
    }
    map.addEventListener("tilesloaded", ReloadAjax());
    // 编写自定义函数,创建标注
    function addMarker(point) {
        var marker = new BMap.Marker(point);
        map.addOverlay(marker);
    }
    $("#Ok").click(function () {
        ReloadAjax();
    });
    function addEventListener(content, marker, c) {
        marker.addEventListener("click", function (e) {
            attribute(content, c)
        }
       );
    }
    function ReloadAjax(e) {

        map.clearOverlays();
        $.ajax({
            //type: "POST",
            url: "/BaiduMap/PODTrackingMap/PODGeographicalPosition",
            data: {
                Customerordernumber: $("#CustomerOrderNumer").val(),
                EndCustomer: $("#EndCustomer").val(),
                Destination: $("#Destination").val(),
                start_DeliveryDate: $("#start_DeliveryDate").val(),
                end_DeliveryDate: $("#end_DeliveryDate").val(),
                start_PlanArrive: $("#start_PlanArrive").val(),
                end_PlanArrive: $("#end_PlanArrive").val(),
            },
            //async: false,
            success: function (data) {
                if (data.code == 1) {
                    $("#TH")[0].innerHTML = "0 ";
                    $("#GX")[0].innerHTML = "0 ";
                    $("#PS")[0].innerHTML = "0 ";
                    $("#WD")[0].innerHTML = "0 ";
                    for (var i = 0; i < data.dataTotal.length; i++) {

                        switch (data.dataTotal[i].PODType) {
                            case "提货车辆":
                                $("#TH")[0].innerHTML = data.dataTotal[i].Num + " ";
                                break;
                            case "干线车辆":
                                $("#GX")[0].innerHTML = data.dataTotal[i].Num + " ";
                                break;
                            case "配送车辆":
                                $("#PS")[0].innerHTML = data.dataTotal[i].Num + " ";
                                break;
                            case "中转运单":
                                $("#WD")[0].innerHTML = data.dataTotal[i].Num + " ";
                                break;
                            default:
                                break;
                        }
                    }

                    if (data.info.length == 1) {
                        map.centerAndZoom(new BMap.Point(data.info[0].lng, data.info[0].lat), 7);
                    }
                    points = data.info;
                    for (var i = 0; i < points.length; i++) {
                        var marker = [];
                        if (points[i].PODType == "干线车辆") {
                            marker = new BMap.Marker(new BMap.Point(points[i].lng, points[i].lat), { icon: myIconblue });  // 创建标注
                        }
                        if (points[i].PODType == "配送车辆") {
                            marker = new BMap.Marker(new BMap.Point(points[i].lng, points[i].lat), { icon: myIcongreen });  // 创建标注
                        }
                        if (points[i].PODType == "提货车辆") {
                            marker = new BMap.Marker(new BMap.Point(points[i].lng, points[i].lat), { icon: myIconred });  // 创建标注
                        }
                        if (points[i].PODType == "中转运单") {
                            marker = new BMap.Marker(new BMap.Point(points[i].lng, points[i].lat), { icon: myIconredWD });  // 创建标注
                        }
                        var content = points[i].Info;
                        var Hub = points[i].PODType;
                        var Code = points[i].Code;
                        map.addOverlay(marker);               // 将标注添加到地图中
                        addClickHandler(content, marker);
                        addEventListener(Hub, marker, Code);
                    }
                }
            },
            error: function (msg) {

            }
        });

        for (var i = 0; i < points.length; i++) {
            var marker = [];
            if (points[i].PODType == "干线车辆") {
                marker = new BMap.Marker(new BMap.Point(points[i].lng, points[i].lat), { icon: myIconblue });  // 创建标注
            }
            if (points[i].PODType == "配送车辆") {
                marker = new BMap.Marker(new BMap.Point(points[i].lng, points[i].lat), { icon: myIcongreen });  // 创建标注
            }
            if (points[i].PODType == "提货车辆") {
                marker = new BMap.Marker(new BMap.Point(points[i].lng, points[i].lat), { icon: myIconred });  // 创建标注
            }
            if (points[i].PODType == "中转运单") {
                marker = new BMap.Marker(new BMap.Point(points[i].lng, points[i].lat), { icon: myIconredWD });  // 创建标注
            }
            var content = points[i].Info;
            var Hub = points[i].PODType;
            var CarNo = points[i].CarNo;
            map.addOverlay(marker);               // 将标注添加到地图中
            addClickHandler(content, marker);
            addEventListener(Hub, marker, CarNo);
        }
        setTimeout(ReloadAjax, 90000);
    }

    var isDialogTitle = false;
    function down(e) {
        if (e.target.className.indexOf('dialog-title') != -1) {
            isDialogTitle = true;
        }
    }

    function move(e) {
        var dialog = document.getElementById('dlgTest');
        if (isDialogTitle) {//只有点击Dialog Title的时候才能拖动
            dialog.style.left = e.clientX + 'px';
            dialog.style.top = e.clientY + 'px';
        }
    }

    function up(e) {
        isDialogTitle = false;
    }

    document.addEventListener('mousedown', down);
    document.addEventListener('mousemove', move);
    document.addEventListener('mouseup', up);
});

//var pgindex = 1;                                      //当前页 
//window.onload = function ()                             //重写窗体加载的事件 
//{
//    var obj = document.getElementById("frameContent");  //获取内容层 
//    var pages = document.getElementById("pages");         //获取翻页层 
//    var allpages = Math.ceil(parseInt(obj.scrollHeight) / parseInt(obj.offsetHeight));//获取页面数量 
//    pages.innerHTML = "<b>共" + allpages + "页</b>";     //输出页面数量 
//    for (var i = 1; i <= allpages; i++) {
//        pages.innerHTML += "<a href=\"javascript:showPage('" + i + "');\">第" + i + "页</a> ";
//        //循环输出第几页 
//    }
//    pages.innerHTML += "      <a href=\"javascript:gotopage('-1');\">上一页</a>  <a href=\"javascript:gotopage('1');\">下一页</a>"
//}
//function gotopage(value) {
//    try {
//        value == "-1" ? showPage(pgindex - 1) : showPage(pgindex + 1);
//    } catch (e) {

//    }
//}
//function showPage(pageINdex) {
//    var obj = document.getElementById("frameContent");  //获取内容层 
//    var pages = document.getElementById("pages");         //获取翻页层 
//    obj.scrollTop = (pageINdex - 1) * parseInt(obj.offsetHeight);                                                                  //根据高度，输出指定的页 
//    pgindex = pageINdex;
//}
function PartialViewTrajectory(self) {
    var PodId = $(self)[0].dataset.podid;
    //弹出即全屏
    var index = layer.open({
        title: '运单详情',
        type: 2,
        content: '/POD/POD/ViewPodAll/' + PodId + '?showEditRelated=True',
        area: ['893px', '600px'],
        maxmin: true
    });
    layer.full(index);
}


//TOGGLE CLOSE
$('.nav-toggle').click(function () {
    //get collapse content selector
    var collapse_content_selector = $(this).attr('href');

    //make the collapse content to be shown or hide
    var toggle_switch = $(this);
    $(collapse_content_selector).slideToggle(function () {
        if ($(this).css('display') == 'block') {
            //change the button label to be 'Show'
            toggle_switch.html('<span class="entypo-minus-squared"></span>');
        } else {
            //change the button label to be 'Hide'
            toggle_switch.html('<span class="entypo-plus-squared"></span>');
        }
    });
});


$('.nav-toggle-alt').click(function () {
    //get collapse content selector
    var collapse_content_selector = $(this).attr('href');

    //make the collapse content to be shown or hide
    var toggle_switch = $(this);
    $(collapse_content_selector).slideToggle(function () {
        if ($(this).css('display') == 'block') {
            //change the button label to be 'Show'
            toggle_switch.html('<span class="entypo-up-open"></span>');
        } else {
            //change the button label to be 'Hide'
            toggle_switch.html('<span class="entypo-down-open"></span>');
        }
    });
    return false;
});
//CLOSE ELEMENT
$(".gone").click(function () {
    var collapse_content_close = $(this).attr('href');
    $(collapse_content_close).hide();



});
//$('.tooltitle').tooltip();