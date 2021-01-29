
var startPoint1;
var endPoint1;
var startPoint2;
var endPoint2;
var content;
$(document).ready(function () {

    GetPoint();

    // 百度地图API功能
    var map = new BMap.Map("container");
    map.centerAndZoom(new BMap.Point(118.454, 32.955), 6);
    map.enableScrollWheelZoom();
    var myIconTruck = new BMap.Icon("/Image/BaiduMap/TruckCar.png", new BMap.Size(40, 26));
    map.addEventListener("tilesloaded", function () {
        $(".BMap_cpyCtrl").remove(); //去掉百度左下角的字
    });
    var myP1 = new BMap.Point(startPoint1, endPoint1);    //起点
    var myP2 = new BMap.Point(startPoint2, endPoint2);    //终点
    var myIcon = new BMap.Icon("http://developer.baidu.com/map/jsdemo/img/Mario.png", new BMap.Size(32, 70), {    //小车图片
        //offset: new BMap.Size(0, -5),    //相当于CSS精灵
        imageOffset: new BMap.Size(0, 0)    //图片的偏移量。为了是图片底部中心对准坐标点。
    });
    var driving2 = new BMap.DrivingRoute(map, { renderOptions: { map: map, autoViewport: true } });    //驾车实例
    driving2.search(myP1, myP2);    //显示一条公交线路

    var lushu;
    // 实例化一个驾车导航用来生成路线
    var drv = new BMap.DrivingRoute(map, {
        onSearchComplete: function (res) {
            if (drv.getStatus() == BMAP_STATUS_SUCCESS) {
                var plan = res.getPlan(0);
                var arrPois = [];
                for (var j = 0; j < plan.getNumRoutes() ; j++) {
                    var route = plan.getRoute(j);
                    arrPois = arrPois.concat(route.getPath());
                }
                map.addOverlay(new BMap.Polyline(arrPois));
                map.setViewport(arrPois);

                lushu = new BMapLib.LuShu(map, arrPois, {
                    defaultContent: content,//默认显示文字
                    autoView: true,//是否开启自动视野调整，如果开启那么路书在运动过程中会根据视野自动调整 http://developer.baidu.com/map/jsdemo/img/car.png
                    icon: new BMap.Icon('http://developer.baidu.com/map/jsdemo/img/car.png', new BMap.Size(40, 26), { anchor: new BMap.Size(27, 13) }),
                    speed: 1000,
                    enableRotation: true,//是否设置marker随着道路的走向进行旋转
                });
            }
        }
    });
    drv.search(myP1, myP2);


    setTimeout(function () {
        lushu.start();
    }, 1500);

});

//获取起点和终点
function GetPoint() {
    $.ajax({
        url: "/BaiduMap/PODTrackingMap/PartialPODView",
        data: {
            Type: $("#Type").val(),
            ID: $("#CarNo").val()
        },
        async: false,
        success: function (data) {
            if (data.Code == 1) {
                startPoint1 = data.PODTrackingMap[0].Longitude;    //起点
                endPoint1 = data.PODTrackingMap[0].Latitude;
                startPoint2 = data.PODTrackingMap[data.PODTrackingMap.length - 1].Longitude;    //终点
                endPoint2 = data.PODTrackingMap[data.PODTrackingMap.length - 1].Latitude;
               
                content = "<span style='position:absolute;left:5px;'><b style='color:red;'>起点</b>:" + data.PODTrackingMap[0].GeographicalPosition + "</span></br><b style='color:red;'>终点:</b> " + data.PODTrackingMap[data.PODTrackingMap.length - 1].GeographicalPosition;
            }
        },
        error: function (msg) {

        }
    })
}


