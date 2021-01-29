$(document).ready(function () {

    //模糊
    $('#VehicleNo').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/ShipperManagement/VehicleManagement/GetAllVehiclesbyVID",
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
            $('#VID').val(ui.item.data.Value);
            $('#VehicleNo').val(ui.item.data.Text);
            $.ajax({
                url: "/ShipperManagement/VehicleManagement/GetCRM_VehicleMappingDriver",
                type: "POST",
                dataType: "json",
                data: { name: ui.item.data.Text },
                success: function (data) {
                    var dataList = document.getElementById("dataList");
                    var rowDataList = dataList.getElementsByTagName("tr");
                    var table = document.getElementById("you");
                    var row = table.getElementsByTagName("tr");
                    var html = $("#Evaluations").render(data.Result.CRMDriverCollection);
                    $("#youTbody")["append"](html);
                    ListDisabled();
                }
            });
        }
    }).change(function () {
        if ($(this).val() === '') {
            $('#VID').val('');
        }
    });


    //右边表格查询
    $('#searchButtons').click(function () {
        if ($('#VehicleNo').val() == "") {
            alert('请输入车牌号码');

        }
        else {
            var vehicleno = $('#VehicleNo').val();
            var drivername = $('#DriverNames').val();
            $.ajax({
                url: "/ShipperManagement/VehicleManagement/GetCRMVehicleMappingDriver",
                type: "POST",
                dataType: "json",
                data: { name: vehicleno, drivername: drivername },
                success: function (data) {

                    var html = $("#Evaluations").render(data.Result.CRMDriverCollection);
                    $("#youTbody")["empty"]();
                    $("#youTbody")["append"](html);

                    ListDisabled();
                }
            });
        }


    });


    ////选中司机，已选择的司机显示在右边的表格上
    //$("#dataList :checkbox").live('click', function () {
    //    if (this.checked) {
    //        var checkbox = document.getElementsByName("checkbox");
    //        //创建td，添加一个checkbox
    //        var atd = document.createElement('td');
    //        atd.innerHTML = "<input type ='checkbox'  id=" + this.value + ">";

    //        //创建td，将值增加到td中
    //        var ctd = document.createElement("td");
    //        ctd.innerHTML = this.value;
    //        this.disabled = true; //选中后变灰不可用

    //        //创建tr，将td增加到tr中
    //        var tr = document.createElement('tr');
    //        tr.appendChild(atd);
    //        tr.appendChild(ctd);

    //        //将tr增加到youTbody表格中
    //        var you = document.getElementById("youTbody");
    //        you.appendChild(tr);
    //    }

    //});



    var selectAll = document.getElementById("selectAll"); //获取全选checkbox
    var dataList = document.getElementById("dataList"); //获取tbody
    var input = document.getElementsByTagName("input"); //获取tbody下的input
    var inputList = $("#dataList :checkbox"); //定义tbody下的checkbox
    //点击全选
    $('#selectAll').click(function () {
        var inputList = $("#dataList :checkbox");
        for (var i = 0; i < inputList.length; i++) {
            if (inputList[i].type == "checkbox") {
                if (!inputList[i].disabled) {
                    inputList[i].checked = this.checked ? true : false;
                }
            }
        }
    });
    //全选，取消选中其中一个，取消全选
    inputList.live("click", function () {
        $('#selectAll').attr("checked", inputList.length == $('[name = "checkbox"]:checked').length ? true : false);
    });

    //右侧表格全选
    $('#reAll').click(function () {
        var table = document.getElementById("you"); //获取you的表格
        var row = table.getElementsByTagName("tr"); //id为you表格的tr
        for (var i = 1; i < row.length; i++) {
            row[i].getElementsByTagName("td")[0].childNodes[0].checked = this.checked ? true : false;
        }
    });

    //选择，点击<<，删除
    $('#bk').click(function () {
        var dataList = document.getElementById("dataList"); //获取左边id为dataList的tbody
        var rowDataList = dataList.getElementsByTagName("tr"); //左边id为dataList的tbody的tr
        var youTbody = document.getElementById("youTbody"); //获取右边表格中的tbody的id
        $("#you").each(function () {
            $("input[type='checkbox']", this).each(function () {
                var id = $(this).attr('id');
                if (this.checked) {
                    for (var i = 0; i < rowDataList.length; i++) {
                        var col = rowDataList[i].getElementsByTagName("td");
                        if (id == col[1].innerText) {
                            col[0].childNodes[1].disabled = false;
                            col[0].childNodes[1].checked = false;
                        }
                        if (this.id != 'reAll') {

                            $(this).parent().parent().remove();
                        }
                        else {
                            this.checked = false;
                        }

                    }
                }
            });

        });
    });

    //点击下一页或上一页，再返回后，已选择的仍然选中
    function ListDisabled() {
        var dataList = document.getElementById("dataList"); //获取左边id为dataList的tbody
        var rowDataList = dataList.getElementsByTagName("tr"); //左边id为dataList的tbody的tr
        var table = document.getElementById("you"); //获取you的表格
        var row = table.getElementsByTagName("tr"); //id为you表格的tr
        for (var j = 0; j < rowDataList.length; j++) {
            for (var i = 1; i < row.length; i++) {
                if (rowDataList[j].getElementsByTagName("td")[1].innerText.trim() == row[i].getElementsByTagName("td")[1].innerText.trim()) {
                    rowDataList[j].getElementsByTagName("td")[0].childNodes[1].checked = true;
                    rowDataList[j].getElementsByTagName("td")[0].childNodes[1].disabled = true;
                }
            }
        }
    }


    //查询
    $('#searchButton').click(function () {
        var initialize = function () {
            pageIndex = parseInt($("#PageIndex").val());
            pageCount = parseInt($("#PageCount").val());
            //总页数大于0,当前第 页，共 页
            if (pageCount > 0) {
                $('#_pager_pageIndex').text(parseInt($("#PageIndex").val()) + 1);
                $('#_pager_pageCount').text(parseInt($("#PageCount").val()))
                $('#_pager').show();
            }
            ////首页
            //if (pageIndex == 0) {
            //    $('#' + '_first' + ', #' + '_prev').hide();
            //    $('div#pager span.first, div#pager span.prev').hide();
            //    $('#_next').show();
            //    $('#_last').show();
            //}
            //末页
            //if (pageIndex == pageCount - 1) {
            //    $('#' + '_next' + ', #' + '_last').hide();
            //    $('div#pager span.next, div#pager span.last').hide();
            //    $("#_prev").show();
            //    $('#_first').show();
            //}

            //if (pageIndex > 0 && pageIndex < pageCount - 1) {
            //    $("#_prev").show();
            //    $('#_first').show();
            //    $('#_next').show();
            //    $('#_last').show();
            //}
            if (pageCount > 1 && pageIndex == 0) {
                $('#' + '_first' + ', #' + '_prev').hide();
                $('div#pager span.first, div#pager span.prev').hide();
                $('#_next').show();
                $('#_last').show();
            }

            if (pageCount > 1 && pageIndex == pageCount - 1) {
                $('#' + '_next' + ', #' + '_last').hide();
                $('div#pager span.next, div#pager span.last').hide();
                $("#_prev").show();
                $('#_first').show();
            }


            if (pageIndex == 0 && pageCount == 1) {
                $('#_pager_pageIndex').show();
                $('#_pager_pageCount').show();
                $('#_next').hide();
                $('#_last').hide();

            }
        }
        var str = $("#DriverName").val();
        $.ajax({
            url: "/ShipperManagement/VehicleManagement/SearchVehicleToDriver",
            type: "POST",
            dataType: "json",
            data: { driverName: str },
            success: function (data) {
                var html = $("#Evaluation").render(data.Result.CRMDriverCollection);
                $("#dataList")["empty"]();
                $("#dataList")["append"](html);
                $('#' + 'PageCount').val(data.Result.PageCount);
                $('#selectAll')[0].checked = false;
                $('#selectAll')[0].disabled = false;
                initialize();
                ListDisabled();
            }
        });

    });


    //分页控件
    $(function () {
        var initialize = function () {
            pageIndex = parseInt($("#PageIndex").val());
            pageCount = parseInt($("#PageCount").val());
            //总页数大于0
            if (pageCount > 0) {
                $('#_pager_pageIndex').text(parseInt($("#PageIndex").val()) + 1);
                $('#_pager_pageCount').text(parseInt($("#PageCount").val()))
                $('#_pager').show();
            }
            //首页
            if (pageIndex == 0) {
                $('#' + '_first' + ', #' + '_prev').hide();
                $('div#pager span.first, div#pager span.prev').hide();
                $('#_next').show();
                $('#_last').show();
            }
            //末页
            if (pageIndex == pageCount - 1) {
                $('#' + '_next' + ', #' + '_last').hide();
                $('div#pager span.next, div#pager span.last').hide();
                $("#_prev").show();
                $('#_first').show();
            }

            if (pageIndex > 0 && pageIndex < pageCount - 1) {
                $("#_prev").show();
                $('#_first').show();
                $('#_next').show();
                $('#_last').show();
            }

            if (pageIndex == 0 && pageCount == 0) {
                $('#_pager').hide();
            }
        }

        var setup = function (pageIndex) {
            var form = null;
            if ('') {
                form = document.getElementById('');
            } else {
                form = document.forms[0];
            }
            $('#' + 'PageIndex').val(pageIndex)

            //$(form).submit();
            $.ajax({
                url: "/ShipperManagement/VehicleManagement/VehicleToDriver",
                type: "POST",
                dataType: "json",
                data: { vehicleNo: $("#VehicleNo").val(), VID: $("#VID").val(), Index: pageIndex },
                success: function (data) {
                    var html = $("#Evaluation").render(data.Result.CRMDriverCollection);
                    $("#dataList")["empty"]();
                    $("#dataList")["append"](html);
                    $('#' + 'PageCount').val(data.Result.PageCount);
                    $('#selectAll')[0].checked = false;
                    $('#selectAll')[0].disabled = false;
                    initialize();
                    ListDisabled();
                }
            });

        }

        $('#_prev').click(function () {
            var pageIndex = parseInt($("#PageIndex").val()) - 1;
            setup(pageIndex);
        })
        $('#_next').click(function () {
            var pageIndex = parseInt($("#PageIndex").val()) + 1;
            setup(pageIndex);
        })
        $('#_first').click(function () {
            var pageIndex = 0;
            setup(pageIndex);
        })
        $('#_last').click(function () {
            var pageIndex = pageCount - 1;
            setup(pageIndex);
        })
        $('#_customNumber').keypress(function (event) {

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
        initialize();
    });
  
        //全选,点击>>,数据添加
     $('#go').click(function () {
            var inputListFenye = $("#dataList :checkbox"); //定义tbody下的checkbox
            for (var i = 0; i < inputListFenye.length; i++) {
                if (inputListFenye[i].type == "checkbox") {
                    if (inputListFenye[i].checked) {    //只有当checkbox选中时，点击>>后，才变不可用
                        inputListFenye[i].disabled = true;
                    }
                }
            }
            if ($('#selectAll')[0].checked) {
                var dataList = document.getElementById("dataList");
                var rowDataList = dataList.getElementsByTagName("tr");
                var table = document.getElementById("you");
                var row = table.getElementsByTagName("tr");
                if (row.length == 1) {    //id为you表格
                    for (var j = 0; j < rowDataList.length; j++) {
                        //创建td，添加一个checkbox
                        var atd = document.createElement('td');
                        atd.innerHTML = "<input type ='checkbox'  id=" + rowDataList[j].getElementsByTagName("td")[1].innerText + ">";
                        //创建td，将值增加到td中
                        var ctd = document.createElement("td");
                        ctd.innerHTML = rowDataList[j].getElementsByTagName("td")[1].innerText;
                        //创建tr，将td增加到tr中
                        var tr = document.createElement('tr');
                        tr.appendChild(atd);
                        tr.appendChild(ctd);
                        //将tr增加到you表格中
                        var you = document.getElementById("you");
                        you.appendChild(tr);
                    }
                }
                else {
                    for (var j = 0; j < rowDataList.length; j++) {
                        for (var i = 1; i < row.length; i++) {
                            if (rowDataList[j].getElementsByTagName("td")[1].innerText.trim() == row[i].getElementsByTagName("td")[1].innerText.trim()) {
                                table.deleteRow(i);
                            }
                        }
                        //创建td，添加一个checkbox
                        var atd = document.createElement('td');
                        atd.innerHTML = "<input type ='checkbox'  id=" + rowDataList[j].getElementsByTagName("td")[1].innerText + ">";
                        //创建td，将值增加到td中
                        var ctd = document.createElement("td");
                        ctd.innerHTML = rowDataList[j].getElementsByTagName("td")[1].innerText;
                        //创建tr，将td增加到tr中
                        var tr = document.createElement('tr');
                        tr.appendChild(atd);
                        tr.appendChild(ctd);
                        //将tr增加到you表格中
                        var you = document.getElementById("you");
                        you.appendChild(tr);
                    }
                }
            }

            else {
                var dataList = document.getElementById("dataList");
                var rowDataList = dataList.getElementsByTagName("tr");
                var table = document.getElementById("you");
                var row = table.getElementsByTagName("tr");
                for (var j = 0; j < rowDataList.length; j++) {
                    if ($('#dataList :checkbox')[j].checked) {
                        for (var i = 1; i < row.length; i++) {
                            if (rowDataList[j].getElementsByTagName("td")[1].innerText.trim() == row[i].getElementsByTagName("td")[1].innerText.trim()) {
                                table.deleteRow(i);
                            }

                        }
                        //创建td，添加一个checkbox
                        var atd = document.createElement('td');
                        atd.innerHTML = "<input type ='checkbox'  id=" + rowDataList[j].getElementsByTagName("td")[1].innerText + ">";
                        //创建td，将值增加到td中
                        var ctd = document.createElement("td");
                        ctd.innerHTML = rowDataList[j].getElementsByTagName("td")[1].innerText;
                        //创建tr，将td增加到tr中
                        var tr = document.createElement('tr');
                        tr.appendChild(atd);
                        tr.appendChild(ctd);
                        //将tr增加到you表格中
                        var you = document.getElementById("you");
                        you.appendChild(tr);


                    }
                }
            }
        });
   


    //提交
    $('#submitButton').click(function () {
        if ($('#VehicleNo').val() == "") {
            Runbow.TWS.Alert("请输入车牌号码");
        }

        else {
            var str = TableToJson();
            $.send(
             "/ShipperManagement/VehicleManagement/AddVehicleToDriver",
              { vehicle: $("#VehicleNo").val(), jsonStr: str },
              function (response) {
                  if (response == 'True') {
                      Runbow.TWS.Alert("提交成功");
                  } else {
                      Runbow.TWS.Alert("提交失败！");
                  }
              },
              function () {
                  Runbow.TWS.Alert("失败");
              });
        }
        //var str = TableToJson();
        //$.send(
        // "/ShipperManagement/ShipperManagement/AddShipperToVehicle",
        //  { shipper: $("#ShipperName").val(), jsonStr: str },
        //  function (response) {
        //      if (response == 'True') {
        //          Runbow.TWS.Alert("提交成功");
        //      } else {
        //          Runbow.TWS.Alert("提交失败！");
        //      }
        //  },
        //  function () {
        //      Runbow.TWS.Alert("失败");
        //  });
    });

    function TableToJson(tableid) {
        var txt = "[";
        var dataList = document.getElementById("dataList");
        var rowDataList = dataList.getElementsByTagName("tr");
        var table = document.getElementById('you');
        var row = table.getElementsByTagName("tr");
        //var col = row[0].getElementsByTagName("th");

        for (var j = 1; j < row.length; j++) {
            var r = "{";
            //var rowDataLists = rowDataList[i].getElementsByTagName("td");
            var tds = row[j].getElementsByTagName("td");
            r += "\"" + "DriverName" + "\"\:\"" + tds[0].childNodes[0].id + "\",";
            r = r.substring(0, r.length - 1)
            r += "},";
            txt += r;
        }

        txt = txt.substring(0, txt.length - 1);
        txt += "]";
        return txt;
    }


});

  //  ; + "DriverPhone" + "\"\:\"" + rowDataLists[2].innerText