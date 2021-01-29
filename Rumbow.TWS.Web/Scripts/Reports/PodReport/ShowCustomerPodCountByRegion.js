$(function () {
    $('#Search').click(function () {
        $.send(
        '/Reports/PodReport/ShowCustomerPodCountByRegion',
        {
            customerID: $('#Customer').val(),
            year: $('#Year').val(),
            month: $('#Month').val()
        },
        function (response) {
            if (response) {
                var brandsData = response.brandsData;
                var drilldownSeries = response.drilldownSeries;
                var month;
                if ($('#Month').val() === '0') {
                    month = '';
                } else {
                    month = $('#Month').val() + '月';
                }

                // Create the chart
                $('#Container').highcharts({
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: $('#Year').val() + '年' + month + $('#Customer option:selected').text() + '地区订单量统计'
                    },
                    subtitle: {
                        text: '点击可查看下级地区订单'
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
                        name: '地区统计',
                        colorByPoint: true,
                        data: brandsData
                    }],
                    drilldown: {
                        series: drilldownSeries
                    }
                });
            }
        });
    });

});