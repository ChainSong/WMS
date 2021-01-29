$(function () {
    $('#Search').click(function () {
        $.send(
        '/Reports/PodReport/IncomAndExpenses',
        {
            customerID: $('#Customer').val(),
            year: $('#Year').val()
        },
        function (response) {
            if (response) {
                var series = [];
                if (response.incomes) {
                    series.push({
                        type: 'column',
                        name: '收入',
                        data: response.incomes
                    });
                }
                if (response.expenses) {
                    series.push({
                        type: 'column',
                        name: '支出',
                        data: response.expenses
                    });
                }
                if (response.incomeTotal && response.expensesTotal) {
                    series.push({
                        type: 'pie',
                        name: '年总收支',
                        data: [{name:'收入',y:response.incomeTotal,color:Highcharts.getOptions().colors[0]},{name:'支出', y:response.expensesTotal,color:Highcharts.getOptions().colors[1]}],
                        center: [100, 80],
                        size: 100,
                        showInLegend: false,
                        dataLabels: {
                            enabled: false
                        }
                    });
                };
                $('#Container').highcharts({
                    chart: {
                    },
                    title: {
                        text: $('#Customer option:selected').text() + $('#Year').val() + '收支统计报表'
                    },
                    xAxis: {
                        categories: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
                    },
                    tooltip: {
                        formatter: function () {
                            var s;
                            if (this.point.name) { // the pie chart
                                s = '' +
                                    this.point.name + ': ' + this.y + ' 元';
                            } else {
                                s = '' +
                                    this.x + ': ' + this.y;
                            }
                            return s;
                        }
                    },
                    labels: {
                        items: [{
                            html: '年总收支对比',
                            style: {
                                left: '40px',
                                top: '8px',
                                color: 'black'
                            }
                        }]
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
                    series: series
                });
            }
        });
    });

});