$(document).ready(function () {
    $('#returnButton').click(function () {
        var returnStep = 0;
        if ($('#ReturnStep').val() !== '') {
            returnStep = parseInt($('#ReturnStep').val());
        }

        if (returnStep === 0) {
            returnStep = 1;
        }

        window.history.go(-returnStep);
    });

    var customerID = $('#PodAll_Pod_CustomerID').val();
    //if ($('#baiduMapContainer') && (customerID === '1' || customerID === '2' || customerID === '8')) {
    //    var map = new BMap.Map('baiduMapContainer');
    //    if (map != null) {
    //        map.addControl(new BMap.NavigationControl());               // 添加平移缩放控件
    //        map.addControl(new BMap.ScaleControl());                    // 添加比例尺控件
    //        map.addControl(new BMap.OverviewMapControl());
    //        var myGeo = new BMap.Geocoder();
    //        var trackTableTrs = $('#TrackTable tbody tr');
    //        var adds = [];
    //        var addsPoint = [];
    //        var startCity = $('#StartCityTd')[0].innerText;
    //        var endCity = $('#EndCityTd')[0].innerText;
    //        if (customerID === '8' && (startCity === '太仓CLC' || startCity === '太仓CRW')) {
    //            adds.push('太仓');
    //        } else {
    //            adds.push(startCity);
    //        }

    //        trackTableTrs.each(function (index, tr) {
    //            var trackTd;
    //            var location;
    //            if (customerID === '1') {
    //                location = $(tr).find('td:eq(1)')[0].innerText;
    //            } else if (customerID === '2') {
    //                location = $(tr).find('td:eq(1)')[0].innerText;
    //            } else if (customerID === '8') {
    //                location = $(tr).find('td:eq(16)')[0].innerText;
    //            }

    //            if (location !== '') {
    //                adds.push(location);
    //            }
    //        });

    //        adds.push(endCity);

    //        if (adds.length > 1) {
    //            $('#baiduMapFieldset').show();
    //        }

    //        map.centerAndZoom(new BMap.Point(117.282699, 31.866942), 7);
    //        map.enableScrollWheelZoom();
    //        var index = 0;
    //        var bdGEO = function () {
    //            var add = adds[index];
    //            geocodeSearch(add);
    //            index++;
    //        }

    //        var geocodeSearch = function (add) {
    //            if (index < adds.length) {
    //                setTimeout(bdGEO, 400);
    //            }

    //            myGeo.getPoint(add, function (point) {
    //                if (point) {
    //                    addsPoint.push(new BMap.Point(point.lng, point.lat));
    //                    map.addOverlay(new BMap.Marker(point));
    //                }
    //            });

    //        }

    //        bdGEO();
    //        setTimeout(function () {
    //            if (addsPoint.length > 0) {
    //                map.centerAndZoom(addsPoint[0], 7);
    //            }

    //            if (addsPoint.length > 1) {
    //                var driving = new BMap.DrivingRoute(map);
    //                for (var i = 0 ; i < addsPoint.length - 1; i++) {
    //                    driving.search(addsPoint[i], addsPoint[i + 1]);
    //                }

    //                driving.setSearchCompleteCallback(function () {
    //                    var pts = driving.getResults().getPlan(0).getRoute(0).getPath();    //通过驾车实例，获得一系列点的数组

    //                    var polyline = new BMap.Polyline(pts, { strokeColor: "red", strokeWeight: 6, strokeOpacity: 0.5 });
    //                    map.addOverlay(polyline);
    //                });

    //                for (var i = 0 ; i < addsPoint.length; i++) {
    //                    var place = new BMap.Marker(addsPoint[i]);
    //                    map.addOverlay(place);
    //                }

    //                var labStart = new BMap.Label("起点", { position: addsPoint[0] });
    //                var labEnd = new BMap.Label("终点", { position: addsPoint[addsPoint.length - 1] });

    //                map.addOverlay(labStart);
    //                map.addOverlay(labEnd);

    //                if (addsPoint.length > 2) {
    //                    if (addsPoint[addsPoint.length - 1].lng != addsPoint[addsPoint.length - 2].lng) {
    //                        var labelCurrent = new BMap.Label("当前点", { position: addsPoint[addsPoint.length - 2] });
    //                        map.addOverlay(labelCurrent);
    //                    }
    //                }
    //            }
    //            //var polyline = new BMap.Polyline(addsPoint, { strokeColor: "red", strokeWeight: 6, strokeOpacity: 0.5 });
    //            //map.addOverlay(polyline);
    //        }, 4000);

    //    }
    //}

});