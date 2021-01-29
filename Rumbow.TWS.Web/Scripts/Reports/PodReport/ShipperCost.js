$(function () {
    $('#ShipperName').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Pod/Pod/GetUserShipper",
                type: "POST",
                dataType: "json",
                data: { name: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Text, data: item }
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#ShipperID').val(ui.item.data.Value);
            $('#ShipperName').val(ui.item.data.Text);
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#ShipperID').val('');
        }
    });

    $('#Search').click(function () {
        if ($('#ShipperID').val() === '') {
            Runbow.TWS.Alert('请录入并选择承运商');
            return;
        }

        $.send(
        '/Reports/PodReport/ShipperCost',
        {
            ShipperID: $('#ShipperID').val(),
            Year: $('#Year').val()
        },
        function (response) {
            if (response) {
                var brandsData = response.brandsData;
                var drilldownSeries = response.drilldownSeries;
                // Create the chart
                $('#Container').highcharts({
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: $('#Year').val() + '年' + $('#ShipperName').val() + '成本统计'
                    },
                    subtitle: {
                        text: '点击可查看月成本'
                    },
                    xAxis: {
                        type: 'category'
                    },
                    yAxis: {
                        title: {
                            text: '成本'
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
    });

});