$(document).ready(function () {
    var datas = {};
    Highcharts.setOptions({
        colors: ['#608d68', '#4c74a1', '#dba762', '#8b5e78', '#b3b3b3']
    });
    // 对Date的扩展，将 Date 转化为指定格式的String 
    // 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
    // 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
    // 例子： 
    // (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
    // (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
    Date.prototype.Format = function (fmt) { //author: meizz 
        var o = {
            "M+": this.getMonth() + 1,                 //月份 
            "d+": this.getDate(),                    //日 
            "h+": this.getHours(),                   //小时 
            "m+": this.getMinutes(),                 //分 
            "s+": this.getSeconds(),                 //秒 
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
            "S": this.getMilliseconds()             //毫秒 
        };
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }

    document.getElementById('time').innerHTML = (new Date().getFullYear() + "-" + (new Date().getMonth() + 1) + "-" + new Date().getDate() + " " + new Date().getHours() + ":" + new Date().getMinutes() + ":" + new Date().getSeconds());
    setInterval('document.getElementById("time").innerHTML=(new Date().getFullYear() + "-" + (new Date().getMonth() +1) + "-" + new Date().getDate() + " " + new Date().getHours() + ":" + new Date().getMinutes() + ":" + new Date().getSeconds());', 1000);

    var Map = {
        OrderQuantity: function (asd) {
            var data = [];
            var kk = ['New ' + asd.yi + "", asd.yi];//新建
            data.push(kk);
            kk = ['Allocated ' + asd.wu + "", asd.wu];//分配
            data.push(kk);
            //kk = ['打印' + asd.dayin + "单", asd.dayin];
            //data.push(kk);
            //kk = ['拣货' + asd.qi + "单", asd.qi];
            //data.push(kk);
            kk = ['Pick & Packed ' + asd.ba + "", asd.ba];//包装
            data.push(kk);
            kk = ['Shipped ' + asd.jiu + "", asd.jiu];//完成
            data.push(kk);
            kk = ['Cancelled ' + asd.quxiao + "", asd.quxiao];//取消
            data.push(kk);
            //kk = ['非当天' + asd.zuotian + "单", asd.zuotian];
            //data.push(kk);

            $('#Efficiency').highcharts({
                chart: {
                    options3d: {
                        enabled: true,
                        alpha: 45,
                        beta: 0
                    },
                    type: 'pie',
                    backgroundColor: 'rgba(0,0,0,0)'
                },
                title: {
                    text: 'Order Status',//'Browser market shares at a specific website, 2014
                    style: { "font-size": "25px", "font-family": "微软雅黑", "font-weight": "bold" },
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                credits: {
                    text: '',
                    href: ''
                },
                //subtitle: {
                //    text: 'YTD Total:  kg'
                //},
                exporting: { enabled: false },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        depth: 35,
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}'
                        },
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true
                    }
                },

                series: [{
                    type: 'pie',
                    name: '占比',
                    data: data
                }]
            });
        },
        TimelyRate: function (asd) {
            $('#TimelyRate').highcharts({
                chart: {
                    type: 'bar',
                    marginTop: 80,
                    marginRight: 40,
                    backgroundColor: 'rgba(0,0,0,0)'
                },
                credits: {
                    text: '',
                    href: ''
                },
                exporting: { enabled: false },
                title: {
                    text: 'Shipping On-time & Order Fulfillment Rate',//Total fruit consumption, grouped by gender
                    style: { "font-size": "25px", "font-family": "微软雅黑", "font-weight": "bold" }
                },
                xAxis: {
                    categories: ['Same-day', 'Next-day', 'Order Fulfillment Rate'],
                    title: {
                        text: null
                    }
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '',
                        align: 'high'
                    },
                    labels: {
                        overflow: 'justify'
                    }

                },
                plotOptions: {
                    bar: {
                        dataLabels: {
                            enabled: true
                        },
                        colorByPoint: true
                    },
                    series: {
                        pointWidth: 30 //柱子之间的距离值
                    },
                    column: {
                        stacking: 'normal',
                        depth: 40
                    }

                },
                tooltip: {
                    valueSuffix: ' %'
                },

                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'top',
                    x: -40,
                    y: 100,
                    floating: true,
                    borderWidth: 1,
                    backgroundColor: '#FFFFFF',
                    enabled: false
                },
                credits: {
                    enabled: false
                },

                series: [{
                    name: '%',
                    data: [100.00, 100.00, 100.00]
                }]
            });
        },
        WeeksOrders: function (asd) {
            var data1 = [];
            var data2 = [];
            var Total = 0;
            var data = [];
            for (var i = 0; i < asd.length; i++) {
                Total += asd[i].num;
                var kk = [asd[i].num];
                data1.push(kk);
                kk = [asd[i].Data];
                data.push(kk);
            }
            $('#WeeksOrders').highcharts({
                chart: {
                    backgroundColor: 'rgba(0,0,0,0)'
                },
                title: {
                    text: 'Weekly Volume',
                    style: { "font-size": "25px", "font-family": "微软雅黑", "font-weight": "bold" },
                    x: -20 //center 
                },
                exporting: { enabled: false },
                subtitle: {
                    text: '',//Source: WorldClimate.com
                    x: -20
                },
                credits: {
                    text: '',
                    href: ''
                },
                xAxis: {
                    categories: data
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: false
                    }
                },
                yAxis: {
                    title: {
                        text: 'Quantity'
                    },
                    stackLabels: {
                        enabled: true,
                        decimals: 2,
                        style: {
                            fontWeight: 'bold',
                            point: "1f",
                            color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                        }
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    valueSuffix: '条'
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },

                series: [{
                    name: 'Orders',
                    data: data1
                }, { name: 'YTD Total: ' + Total + ' ' }]
            });
        },
        CarbonEmissions: function (asd, Total) {

            var OfficeElectric = [];
            var NfsElectric = [];
            var DigitalElectric = [];
            var InlineElectric = [];
            var data = [];
            for (var i = 0; i < asd.length; i++) {
                var kk = [asd[i].Data];
                data.push(kk);
                kk = [asd[i].OfficeElectric];
                OfficeElectric.push(kk);
                kk = [asd[i].NfsElectric];
                NfsElectric.push(kk);
                kk = [asd[i].DigitalElectric];
                DigitalElectric.push(kk);
                kk = [asd[i].InlineElectric];
                InlineElectric.push(kk);
            }
            $('#OrderQuantity').highcharts({
                chart: {
                    type: 'column',

                    backgroundColor: 'rgba(0,0,0,0)',
                    marginTop: 80,
                    marginRight: 40
                },
                credits: {
                    text: '',
                    href: ''
                },
                exporting: { enabled: false },
                title: {
                    text: 'Carbon Emission  Calculation',//Total fruit consumption, grouped by gender
                    style: { "font-size": "25px", "font-family": "微软雅黑", "font-weight": "bold" }
                },
                //name: '' + Total.TotalElectric + ' kg'
                xAxis: {
                    categories: data
                },
                //subtitle: {
                //    text: 
                //},
                yAxis: {
                    stackLabels: {
                        enabled: true,
                        style: {
                            decimals: 2,
                            fontWeight: 'bold',
                            color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                        }
                    },
                    allowDecimals: false,
                    min: 0,
                    title: {
                        text: 'Kg'
                    }
                },
                plotOptions: {

                },
                tooltip: {
                    headerFormat: '<b>{point.key}</b><br>',
                    pointFormat: '<span style="color:{series.color}">\u25CF</span> {series.name}: {point.y} / {point.stackTotal}'
                },
                plotOptions: {
                    column: {
                        stacking: 'normal',
                        depth: 40
                    },
                    series: {
                        pointWidth: 30 //柱子之间的距离值
                    },
                },

                series: [{
                    name: 'Office',
                    data: OfficeElectric,
                    stack: 'male'
                }, {
                    name: 'NFS',
                    data: NfsElectric,
                    stack: 'male'
                }, {
                    name: 'Digital',
                    data: DigitalElectric,
                    stack: 'male'
                }, {
                    name: 'Inline',
                    data: InlineElectric,
                    stack: 'male'
                }, {
                    name: 'YTD Total:' + Total.TotalElectric + ' kg'
                }]
            });
        }
    }
    ReloadAjax();
    function ReloadAjax() {
        $.ajax({
            url: "/Home/MonitoringReport",
            type: 'POST',
            dataType: 'json',
            async: false,
            success: function (data) {
                if (data.ErrorCode) {
                    datas = data;
                }
            },
            error: function (msg) {
                var a = msg;
            }
        });
        Map.OrderQuantity(datas.OrderQuantity[0]);
        //Map.Efficiency(datas.Efficiency);
        Map.TimelyRate(datas.TimelyRate[0]);
        Map.WeeksOrders(datas.WeeksOrders);
        Map.CarbonEmissions(datas.CarbonEmissions, datas.TotalElectri);
        setTimeout(ReloadAjax, 100000);
    }
    $("#refresh").click(function () {
        $.ajax({
            url: "/Home/MonitoringReport",
            type: 'POST',
            dataType: 'json',
            async: false,
            success: function (data) {
                if (data.ErrorCode) {
                    datas = data;
                }
            },
            error: function (msg) {
                var a = msg;
            }
        });
        Map.OrderQuantity(datas.OrderQuantity[0]);
        //Map.Efficiency(datas.Efficiency);
        Map.TimelyRate(datas.TimelyRate[0]);
        Map.WeeksOrders(datas.WeeksOrders);
        Map.CarbonEmissions(datas.CarbonEmissions, datas.TotalElectri);
    })
})
