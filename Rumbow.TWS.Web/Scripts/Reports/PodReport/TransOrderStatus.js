

//var PageIndexs = 0;
//var PageCounts = 0;
$(function () {
    var Time = "";
    $('.calendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = 'transOrderRequest_';
        if (pref === 'start') {
            descID += 'StartTime';
        }
        else {
            descID += 'EndTime';
        }
        if ($('#' + descID).val() !== '') {

            $(this).val($('#' + descID).val().split(' ')[0]);
        }
    });
    $('#startCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#SearchCondition_StartCityID').val('');
        $('#SearchCondition_StartCityName').val('');
        $('#SearchCondition_StartCities').val('');
    });

    $('#endCityClear').click(function () {
        $(this).prev().find('.RegionName').val('');
        $(this).prev().find('.RegionID').val('');
        $('#SearchCondition_EndCityID').val('');
        $('#SearchCondition_EndCityName').val('');
        $('#SearchCondition_EndCities').val('');
    });
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
    $('#searchButton').click(function () {
        if ($('#transOrderRequest_ID').val().trim() !=="") {
            $('#GraphicalReports').hide();
        } else {
            $("#GraphicalReports").show();
        }
        $('#container').show();
        $('#containers').hide();
        Time = "";
        $.send(
        '/Reports/PodReport/TransOrderStatus',
        {
            id: $('#transOrderRequest_ID').val(),
            Customers: $('#SearchCondition_CustomerID').val(),
            ShipperName: $('#ShipperName').val(),
            //Consignee: $('#ConsigneeName').val(),
            startCityTreeName: $('#startCityTreeName').val(),
            endCityTreeName: $('#endCityTreeName').val(),
            startTime: $('#start_ActualDeliveryDate').val(),
            endTime: $('#end_ActualDeliveryDate').val()

        },
        function (response) {

            //$('#container').show();
            //$('#containers').hide();
            $("#QueryList").show();
           
            var AllData = JSON.parse(response.Alldata);
            var sbDate = response.sbDate;
            var brandsDataTotal = response.brandsDataTotal;
            var brandsDataAdidas = response.brandsDataAdidas;
            var brandsDataNIKE = response.brandsDataNIKE;
            var brandsDataHilti = response.brandsDataHilti;
            var brandsDataAKZO = response.brandsDataAKZO;
            var pie1 = (response.pie1);
            var pie2 = (response.pie2);
            var pie3 = (response.pie3);
            var pie4 = (response.pie4);
            $("#PageIndex").val(parseInt(0));
            $("#PageCount").val(response.PageCount);
            $('#' + '_pager_pageCount').text(parseInt(response.PageCount));
            $('#' + '_pager_pageIndex').text(parseInt(1))
            initialize();
           // var sum = JSON.parse(response.Alldata).l;
            document.getElementById("sum").innerText = AllData.length;
            if (AllData.length > 0) {
                $("#QueryList").show();
            } else {
                $("#QueryList").hide();
            }
            
           // $("#sum")["append"]( AllData.length());
            var html = $("#Evaluation").render(AllData);
            $("#data")["empty"]();
            $("#data")["append"](html);
            if ($('#SearchCondition_CustomerID').val() == "0" && $("#transOrderRequest_ID").val()=="") {
        
                if (response) {
                    $(function () {
                        $('#container').highcharts({
                            chart: {
                              //  type: 'column'
                            },
                            title: {
                                text: '运单量状态'
                            },
                            subtitle: {
                                text: ''
                            },
                            xAxis: {
                                categories: sbDate
                            },
                            yAxis: {
                                min: 0,
                                title: {
                                    text: '单量 (单)'
                                }
                            },
                            tooltip: {
                              
                                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                    '<td style="padding:0"><b>{point.y:.1f} 单</b></td></tr>',
                                footerFormat: '</table>',
                                shared: true,
                                useHTML: true
                            },
                            plotOptions: {
                                column: {
                                    pointPadding: 0.2,
                                    borderWidth: 0,
                                    dataLabels: {
                                        enabled: true,
                                        style: {
                                            fontSize: '12',
                                            color: 'RoyalBlue'
                                        }
                                    }
                                }, series: {
                                    point: {
                                        events: {
                                            click: function (e) {

                                                Time = this.category;
                                              
                                                $.send(
                                                '/Reports/PodReport/TransOrderStatus',
                                                {
                                                    id: $('#transOrderRequest_ID').val(),
                                                    Customers: $('#SearchCondition_CustomerID').val(),
                                                    ShipperName: $('#ShipperName').val(),
                                                    Consignee: $('#ConsigneeName').val(),
                                                    startTime: $('#start_ActualDeliveryDate').val(),
                                                    endTime: $('#end_ActualDeliveryDate').val(),
                                                    Time: Time
                                                },
                                                  function (responsedata) {
                                                      if (responsedata) {
                                                       
                                                          $('#PageAll').hide();
                                                          $('#container').hide();
                                                          $('#containers').show();
                                                          $("#Page").show();
                                                          $("#PageIndexs").val(parseInt(0));
                                                          $("#PageCounts").val(responsedata.PageCount);
                                                          $('#' + '_pager_pageCounts').text(parseInt(responsedata.PageCount));
                                                          $('#' + '_pager_pageIndexs').text(parseInt(1))
                                                          initializes();
                                                          var AllDatas = JSON.parse(responsedata.Alldata);
                                                          var sbDates = responsedata.sbDate;
                                                          var brandsDataTotals = responsedata.brandsDataTotal;
                                                          var brandsDataAdidass = responsedata.brandsDataAdidas;
                                                          var brandsDataNIKEs = responsedata.brandsDataNIKE;
                                                          var brandsDataHiltis = responsedata.brandsDataHilti;
                                                          var brandsDataAKZOs = responsedata.brandsDataAKZO;
                                                          document.getElementById("sum").innerText = AllDatas.length;
                                                          if (AllDatas.length > 0) {
                                                              $("#QueryList").show();
                                                              $("#datas").show();
                                                              $("#data").hide();
                                                          } else {
                                                              $("#QueryList").hide();
                                                          }
                                                          // $("#sum")["append"]( AllData.length());
                                                          var htmls = $("#Evaluation").render(AllDatas);
                                                          $("#datas")["empty"]();
                                                          $("#datas")["append"](htmls);
                                                          $(function () {
                                                              $('#containers').highcharts({
                                                                  chart: {
                                                                      type: 'column'
                                                                  },
                                                                  title: {
                                                                      text: '运单量状态'
                                                                  },
                                                                  subtitle: {
                                                                      text: ''
                                                                  },
                                                                  xAxis: {
                                                                      categories: sbDates
                                                                  },
                                                                  yAxis: {
                                                                      min: 0,
                                                                      title: {
                                                                          text: '单量 (单)'
                                                                      }
                                                                  },
                                                                  tooltip: {
                                                                      headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                                                      pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                                                          '<td style="padding:0"><b>{point.y:.1f} 单</b></td></tr>',
                                                                      footerFormat: '</table>',
                                                                      shared: true,
                                                                      useHTML: true
                                                                  },
                                                                  plotOptions: {
                                                                      column: {
                                                                          pointPadding: 0.2,
                                                                          borderWidth: 0,
                                                                          dataLabels: {
                                                                              enabled: true,
                                                                              style: {
                                                                                  fontSize: '12',
                                                                                  color: 'RoyalBlue'
                                                                              }
                                                                          }
                                                                      }, series: {
                                                                          point: {
                                                                              events: {
                                                                                  click: function (e) {
                                                                                      document.getElementById("sum").innerText = AllData.length;
                                                                                      $('#container').show();
                                                                                      $('#containers').hide();
                                                                                      $("#datas").hide();
                                                                                      $("#data").show();
                                                                                      $("#Page").hide();
                                                                                      $('#PageAll').show();
                                                                                      Time = "";
                                                                                  }
                                                                              }
                                                                          }
                                                                      }
                                                                  },
                                                                  series: [{
                                                                      name: 'Total',
                                                                      colors: '#feeeed',
                                                                      data: JSON.parse(brandsDataTotals)

                                                                  }, {
                                                                      name: '订单下达',
                                                                      colors: '#f47920',
                                                                      data: JSON.parse(brandsDataAdidass)

                                                                  }, {
                                                                      name: '干线发车',
                                                                      colors: '#80752c',

                                                                      data: JSON.parse(brandsDataNIKEs)
                                                                  }, {
                                                                      name: '到达终端',
                                                                      colors: '#2a5caa',

                                                                      data: JSON.parse(brandsDataHiltis)
                                                                  }, {
                                                                      name: '运单签收',
                                                                      colors: '#ad9b01',
                                                                      data: JSON.parse(brandsDataAKZOs)

                                                                  }]
                                                              });
                                                          });
                                                      }
                                                  })
                                            }
                                        }
                                    }
                                }

                            },
                            series: [{
                                type: 'column',
                                name: 'Total',
                                colors: '#feeeed',
                                data: JSON.parse(brandsDataTotal)

                            }, {
                                type: 'column',
                                name: '订单下达',
                                colors: '#f47920',

                                data: JSON.parse(brandsDataAdidas)
                            }, {
                                type: 'column',
                                name: '干线发车',
                                colors: '#80752c',
                                data: JSON.parse(brandsDataNIKE)

                            }, {
                                type: 'column',
                                name: '到达终端',
                                colors: '#2a5caa',

                                data: JSON.parse(brandsDataHilti)
                            }, {
                                type: 'column',
                                name: '运单签收',
                                colors: '#ad9b01',
                                data: JSON.parse(brandsDataAKZO)

                            }, {
                                type: 'pie',
                                name: '总',
                                data: [{
                                    name: '总',
                                    y: 0,
                                    colors: '#feeeed',
                                },{
                                    name: '订单下达',
                                    y: pie1,
                                    colors: '#f47920',
                                }, {
                                    name: '干线发车',
                                    y: pie2,
                                    colors: '#80752c',
                                }, {
                                    name: '到达终端',
                                    y: pie3,
                                    colors: '#2a5caa',
                                }, {
                                    name: '运单签收',
                                    y: pie4,
                                    colors: '#ad9b01',
                                }],
                                center: [150,20],
                                size: 100,
                                showInLegend: false,
                                dataLabels: {
                                    enabled: false
                                }
                            }]
                        });
                    });
                }
            }
            else {
                if (sbDate.length > 0) {
                    $(function () {
                        $('#container').highcharts({
                            chart: {
                                type: 'column'
                            },
                            title: {
                                text: '运单量状态'
                            },
                            subtitle: {
                                text: ''
                            },
                            xAxis: {
                                categories: sbDate
                            },
                            yAxis: {
                                min: 0,
                                title: {
                                    text: '单量 (单)'
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                    '<td style="padding:0"><b>{point.y:.1f} 单</b></td></tr>',
                                footerFormat: '</table>',
                                shared: true,
                                useHTML: true
                            },
                            plotOptions: {
                                column: {
                                    pointPadding: 0.2,
                                    borderWidth: 0,
                                    dataLabels: {
                                        enabled: true,
                                        style: {
                                            fontSize: '12',
                                            color: 'RoyalBlue'
                                        }
                                    }
                                }
                                //, series: {
                                //    point: {
                                //        events: {
                                //            click: function (e) {
                                //                $('#container').show();
                                //                $('#containers').hide();
                                //            }
                                //        }
                                //    }
                                //}
                            },
                            series: [{
                                name: 'Total',
                                colors: '#feeeed',
                                data: JSON.parse(brandsDataTotal)

                            }, {
                                name: '订单下达',
                                colors: '#f47920',
                                data: JSON.parse(brandsDataAdidas)

                            }, {
                                name: '干线发车',
                                colors: '#80752c',

                                data: JSON.parse(brandsDataNIKE)
                            }, {
                                name: '到达终端',
                                colors: '#2a5caa',

                                data: JSON.parse(brandsDataHilti)
                            }, {
                                name: '运单签收',
                                colors: '#ad9b01',
                                data: JSON.parse(brandsDataAKZO)
                            }]
                        });
                    });
                } else {
                    $("#container").hide();
                }
            }
            });

    });
    var initialize = function () {
        pageIndex = parseInt($("#PageIndex").val());
        pageCount = parseInt($("#PageCount").val());
        if (pageCount > 0) {
            
            $('#' + '_pager_pageIndex').text(parseInt(parseInt($("#PageIndex").val()) + 1));
            $('#' + '_pager_pageCount').text(parseInt($("#PageCount").val()))
            $('#' + '_pager').show();
        }

        if (pageIndex == 0) {
            $('#' + '_first' + ', #' + '_prev').hide();
            $('div#pager span.first, div#pager span.prev').hide();
        } else {
            $('#' + '_first' + ', #' + '_prev').show();
            $('div#pager span.first, div#pager span.prev').show();
        }

        if (pageIndex == pageCount - 1) {
            $('#' + '_next' + ', #' + '_last').hide();
            $('div#pager span.next, div#pager span.last').hide();
        } else {
            $('#' + '_next' + ', #' + '_last').show();
            $('div#pager span.next, div#pager span.last').show();
        }

        if (pageIndex == 0 && pageCount == 0) {
            $('#' + '_pager').hide();
        } else {
            $('#' + '_pager').show();
        }  

    }

    var setup = function (pageIndex) {
        var form = null;
        if ('') {
            form = document.getElementById('');
        } else {
            form = document.forms[0];
        }

        $('#PageIndex').val(pageIndex);
        $.send(
       '/Reports/PodReport/ALLTransOrderStatusPaging',
       {
           id: $('#transOrderRequest_ID').val(),
           Customers: $('#SearchCondition_CustomerID').val(),
           ShipperName: $('#ShipperName').val(),
           Consignee: $('#ConsigneeName').val(),
           startTime: $('#start_ActualDeliveryDate').val(),
           endTime: $('#end_ActualDeliveryDate').val(),
           Time: "",
           PageIndex: pageIndex
       },
         function (responsedata) {

             var html = $("#Evaluation").render(JSON.parse(responsedata));
             $("#data")["empty"]();
             $("#data")["append"](html);
             if (pageCount > 0) {
                 $('#' + '_pager_pageIndex').text(parseInt($("#PageIndex").val()) + 1);
                 $('#' + '_pager_pageCount').text(parseInt($("#PageCount").val()))
                 $('#' + '_pager').show();
             }

             if (pageIndex == 0) {
                 $('#' + '_first' + ', #' + '_prev').hide();
                 $('div#pager span.first, div#pager span.prev').hide();
             } else {
                 $('#' + '_first' + ', #' + '_prev').show();
                 $('div#pager span.first, div#pager span.prev').show();
             }
             if (pageIndex == pageCount - 1) {
                 $('#' + '_next' + ', #' + '_last').hide();
                 $('div#pager span.next, div#pager span.last').hide();
             } else {
                 $('#' + '_next' + ', #' + '_last').show();
                 $('div#pager span.next, div#pager span.last').show();
             }
             if (pageIndex == 0 && pageCount == 0) {
                 $('#' + '_pager').hide();
             } else {
                 $('#' + '_pager').show();
             }

         });
    }
    
    $('#' + '_prev').click(function () {
        var pageIndex = parseInt($("#PageIndex").val()) - 1;
        setup(pageIndex);
    })
    $('#' + '_next').click(function () {
        var pageIndex = parseInt($("#PageIndex").val()) + 1;
        setup(pageIndex);
    })
    $('#' + '_first').click(function () {
        var pageIndex = 0;
        setup(pageIndex);
    })
    $('#' + '_last').click(function () {
        var pageIndex = pageCount - 1;
        setup(pageIndex);
    })

    $('#' + '_customNumber').keypress(function (event) {
        var keycode = event.keycode ? event.keycode : event.which;
        if (keycode == '13') {
            var value = parseInt($(this).val());
            if (!isNaN(value) && value > 0 && value <= pageCount && value != pageIndex + 1) {
                var pageIndex = value - 1;
                setup(pageIndex);
            }
            return false;
        }
    });
    //initialize();
    var initializes = function () {
        pageIndex = parseInt($("#PageIndexs").val());
        pageCount = parseInt($("#PageCounts").val());
        if (pageCount > 0) {
            $('#' + '_pager_pageIndexs').text(parseInt($("#PageIndexs").val()) + 1);
            $('#' + '_pager_pageCounts').text(parseInt($("#PageCounts").val()))
            $('#' + '_pagers').show();
        }  
        if (pageIndex == 0) {
            $('#' + '_firsts' + ', #' + '_prevs').hide();
            $('div#pager span.first, div#pager span.prev').hide();
        } else {
            $('#' + '_firsts' + ', #' + '_prevs').show();
            $('div#pager span.first, div#pager span.prev').show();
        }
        if (pageIndex == pageCount - 1) {
            $('#' + '_nexts' + ', #' + '_lasts').hide();
            $('div#pager span.next, div#pager span.last').hide();
        } else {
            $('#' + '_nexts' + ', #' + '_lasts').show();
            $('div#pager span.next, div#pager span.last').show();
        }
        if (pageIndex == 0 && pageCount == 0) {
            $('#' + '_pagers').hide();
        } else {
            $('#' + '_pagers').show();
        }

    }

    var setups = function (pageIndex) {
        var form = null;
        if ('') {
            form = document.getElementById('');
        } else {
            form = document.forms[0];
        }

        $('#PageIndexs').val(pageIndex);
        $.send(
       '/Reports/PodReport/ALLTransOrderStatusPaging',
       {
           id: $('#transOrderRequest_ID').val(),
           Customers: $('#SearchCondition_CustomerID').val(),
           ShipperName: $('#ShipperName').val(),
           Consignee: $('#ConsigneeName').val(),
           startTime: $('#start_ActualDeliveryDate').val(),
           endTime: $('#end_ActualDeliveryDate').val(),
           Time: Time,
           PageIndex: pageIndex
       },
         function (responsedata) {

             var html = $("#Evaluation").render(JSON.parse(responsedata));
             $("#datas")["empty"]();
             $("#datas")["append"](html);
             if (pageCount > 0) {
                 $('#' + '_pager_pageIndexs').text(parseInt($("#PageIndexs").val()) + 1);
                 $('#' + '_pager_pageCounts').text(parseInt($("#PageCounts").val()))
                 $('#' + '_pagers').show();
             }

             if (pageIndex == 0) {
                 $('#' + '_firsts' + ', #' + '_prevs').hide();
                 $('div#pager span.first, div#pager span.prev').hide();
             } else {
                 $('#' + '_firsts' + ', #' + '_prevs').show();
                 $('div#pager span.first, div#pager span.prev').show();
             }
             if (pageIndex == pageCount - 1) {
                 $('#' + '_nexts' + ', #' + '_lasts').hide();
                 $('div#pager span.next, div#pager span.last').hide();
             } else {
                 $('#' + '_nexts' + ', #' + '_lasts').show();
                 $('div#pager span.next, div#pager span.last').show();
             }
             if (pageIndex == 0 && pageCount == 0) {
                 $('#' + '_pagers').hide();
             } else {
                 $('#' + '_pagers').show();
             }

         });
    }

    $('#' + '_prevs').click(function () {
        var pageIndex = parseInt($("#PageIndexs").val()) - 1;
        setups(pageIndex);
    })
    $('#' + '_nexts').click(function () {
        var pageIndex = parseInt($("#PageIndexs").val()) + 1;
        setups(pageIndex);
    })
    $('#' + '_firsts').click(function () {
        var pageIndex = 0;
        setups(pageIndex);
    })
    $('#' + '_lasts').click(function () {
        var pageIndex = pageCount - 1;
        setups(pageIndex);
    })

    $('#' + '_customNumbers').keypress(function (event) {
        var keycode = event.keycode ? event.keycode : event.which;
        if (keycode == '13') {
            var value = parseInt($(this).val());
            if (!isNaN(value) && value > 0 && value <= pageCount && value != pageIndex + 1) {
                var pageIndex = value - 1;
                setups(pageIndex);
            }
            return false;
        }
    });
 })
 
$(function () {
   
  
});



