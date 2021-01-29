$(function () {

    var showYear = function (data) {
        $('#YearContainer').highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false
            },
            title: {
                text: $('#Year').val() + '年度各项目运单汇总'
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        color: '#FFFFFF',
                        connectorColor: '#FFFFFF',
                        format: '<b>{point.name}</b>: {point.y}</b>{point.ID}'
                    },
                    showInLegend: true
                },
                series: {
                    point: {
                        events: {
                            click: function () {
                                var arr = JSON.parse($('#YearData').val());
                                var name = this.name;
                                for (var i = 0; i < arr.length; i++) {
                                    if (arr[i][0].toString() === name) {
                                        $('#SelectedCustomerID').val(arr[i][2]);
                                        $('#SelectedCustomerName').val(arr[i][0]);
                                        break;
                                    }
                                }
                                showMonth();
                            }
                        }
                    }
                }
            },
            series: [{
                type: 'pie',
                name: '占年度总运单',
                data: data
            }]
        });
    }

    var showMonth = function () {
        $.send(
        '/Reports/PodReport/GetCustomerMonthlyAndDailyPodCount',
        {
            year: $('#Year').val(),
            CustomerID: $('#SelectedCustomerID').val()
        },
        function (response) {
            if (response) {
                var brandsData = response.brandsData;
                var drilldownSeries = response.drilldownSeries;
                // Create the chart
                $('#MonthContainer').highcharts({
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: $('#Year').val() + '年' + $('#SelectedCustomerName').val() + '月度订单量统计'
                    },
                    subtitle: {
                        text: '点击可查看当月每天订单'
                    },
                    xAxis: {
                        type: 'category'
                    },
                    yAxis: {
                        title: {
                            text: '订单量'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                format: '{point.y}'
                            }
                        }
                    },

                    tooltip: {
                        headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                        pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y}</b> 单<br/>'
                    },

                    series: [{
                        name: '月统计',
                        colorByPoint: true,
                        data: brandsData
                    }],
                    drilldown: {
                        series: drilldownSeries
                    }
                });
            }
        });
    }


    $('#Search').click(function () {
        $.send(
        '/Reports/PodReport/ShowEveryCustomerYearPodCount',
        {
            year: $('#Year').val()
        },
        function (response) {
            if (response) {
                var data = [];
                for (var i = 0; i < response.length; i++) {
                    var kk = [response[i].name, response[i].data, response[i].ID];
                    data.push(kk);
                }

                $('#YearData').val(JSON.stringify(data));
                showYear(data);
            }
        });
    });

});