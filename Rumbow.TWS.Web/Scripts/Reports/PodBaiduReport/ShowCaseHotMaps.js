$(document).ready(function () {
    var map = new BMap.Map("container");          // 创建地图实例
    var point = new BMap.Point(116.418261, 39.921984);
    map.centerAndZoom(point, 5);             // 初始化地图，设置中心点坐标和地图级别
   
    map.enableScrollWheelZoom(); // 允许滚轮缩放
    if (!isSupportCanvas()) {
        alert('热力图目前只支持有canvas支持的浏览器,您所使用的浏览器不能使用热力图功能~')
    }
    $("#searchButton").click(function () {
     
        $.ajax({
            url: "/Reports/PodBaiduReport/ShowCaseHotMap",
            type: "POST",
            data: {
                Customer: $("#Request_Customer")[0].value,
                City: $("#startCityTreeName")[0].value,
                StartActualDeliveryDate: $("#start_StartActualDeliveryDate")[0].value,
                EndActualDeliveryDate: $("#end_StartActualDeliveryDate")[0].value,
                Type: $("#Request_HotMapType")[0].value
            },
            success: function (data) {
                if (data.str.length > 2)
                {
                   
                    showMsg("无数据！", 4000);
                    return;
                }
                if ($("#startCityTreeName")[0].value === "") {
                    var map = new BMap.Map("container");
                    var point = new BMap.Point(116.418261, 39.921984);
                    map.centerAndZoom(point, 5);             // 初始化地图，设置中心点坐标和地图级别
                } else {
                    var map = new BMap.Map("container");
                    var point = new BMap.Point(data.lng, data.lat);
                    map.centerAndZoom(point, 12);             // 初始化地图，设置中心点坐标和地图级别
                }
                var points = JSON.parse(data.data);
                heatmapOverlay = new BMapLib.HeatmapOverlay({ "radius": 20 });
                map.addOverlay(heatmapOverlay);
                heatmapOverlay.setDataSet({ data: points, max: 500 });
                closeHeatmap();
                heatmapOverlay.show(); 
            },
            error: function () {
                showMsg("操作失败！", 4000);
            }

        })
    })

 
    //closeHeatmap();
    function setGradient() {
        /*格式如下所示:
        {
            0:'rgb(102, 255, 0)',
            .5:'rgb(255, 170, 0)',
            1:'rgb(255, 0, 0)'
        }*/
        var gradient = {};
        var colors = document.querySelectorAll("input[type='color']");
        colors = [].slice.call(colors, 0);
        colors.forEach(function (ele) {
            gradient[ele.getAttribute("data-key")] = ele.value;
        });
        heatmapOverlay.setOptions({ "gradient": gradient });
    }
    //判断浏览区是否支持canvas
    function isSupportCanvas() {
        var elem = document.createElement('canvas');
        return !!(elem.getContext && elem.getContext('2d'));
    }

})

$(function () {
    $('.calendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = 'Request_';
        if (pref === 'start') {
            descID += 'StartActualDeliveryDate';
        }
        else {
            descID += 'EndActualDeliveryDate';
        }
        if ($('#' + descID).val() !== '') {

            $(this).val($('#' + descID).val().split(' ')[0]);
        }
    });
    $('#startCityTreeName').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#SearchCondition_StartCityID').val('');
        $('#SearchCondition_StartCityName').val('');
        $('#SearchCondition_StartCities').val('');
    });
})

//是否显示热力图
function openHeatmap() {
    heatmapOverlay.show();
}
function closeHeatmap() {
    heatmapOverlay.hide();
}