$(document).ready(function () {
    var datas = {};
    var t = 0;
    //WorkStationPanel
    openPopup("AsnPop", true, 300, 200, false, "WorkStationPanel", null)
    $("#WorkStationOK").click(function () {
        $("#WorkStationId").val($("#INSTRUCTION_").find('option:selected').val());
        $("#WorkStationtext")[0].innerHTML = "(" + $("#INSTRUCTION_").find('option:selected').text() + ")";
        closePopup();
        IntelligentPickGoods.ResetFun();

        //IntelligentPickGoods.GetPickUpGoodsWall($("#INSTRUCTION_").find('option:selected').val());
        //if ($("#WorkStationId").val() != "") {
        //    $.ajax({
        //        type: "POST",
        //        url: "/WMS/IntelligentOperation/CloseChannel",
        //        data: {
        //            WorkStationId: $("#WorkStationId").val()
        //        },tatata
        //        async: "false",
        //        success: function (data) {
        //            if (data.Code == 1) {
        //                //setTimeout(ReloadAjax, 1000);
        //                //$("#label").val(data.Body);
        //            }

        //        },
        //        error: function (msg) {
        //            showMsg("操作失败", "4000");
        //        }
        //    });
        //}
    })
    //setTimeout(ReloadAjax, 1000);
    function ReloadAjax() {
        if ($("#WorkStationId").val() != "") {
            $.ajax({
                type: "POST",
                url: "/WMS/IntelligentOperation/GetMessageMQ",
                data: {
                    WorkStationId: $("#WorkStationId").val()
                },
                async: "false",
                success: function (datas) {
                    if (datas.Code == 1) {
                        layer.closeAll('loading');
                        stopCount();
                        $(".Total")[0].innerHTML = "";
                        var ds = "<div  style='text-align:center'>票数：<span id='OrderProgress'>1</span>/" + datas.instructions.length + "</div>";
                        ds += "<ul  class='shelves' style='list-style:none;font-size:20px'>";
                        for (var i = 0; i < datas.instructions.length; i++) {
                            ds += "<li style='display:none'  data-releateddetailid='" + datas.instructions[i].ID + "' \
                            data-releatednumber='" + datas.instructions[i].ReleatedNumber + "' \
                            data-repickwalldetailid='" + datas.instructions[i].RePickWallDetailID + "' \
                            data-location='" + datas.instructions[i].Location + "' data-sku='" + datas.instructions[i].SKU + "' \
                            data-id='.row" + datas.instructions[i].LevelsNumber + "line" + datas.instructions[i].SerialNumber + "'>\
                            <div>订单：" + datas.instructions[i].ReleatedNumber + "</div> <div>库位：" + datas.instructions[i].Location + "</div> \
                            <div>SKU：" + datas.instructions[i].SKU + "</div> <div>数量：" + datas.instructions[i].QtyExcepted + "</div>\
                            <div>实际数量：<input type='text' name='ActualQty'class='form-control ActualQty' value='" + datas.instructions[i].QtyExcepted + "'/></div></li>";
                        }
                        $(".body")[0].innerHTML = ds + "</ul>";
                        var _html = '<div  style="height: 500px;width: 300px;background-color:#ff9900;-moz-box-shadow: 10px 10px 5px #888888; /* 老的 Firefox */box-shadow: 10px 10px 5px #888888">\
                             <table style="width: 300px;height: 100%;border:2px solid #666666;" class="ShelvesStyle">';
                        for (var i = 0; i < datas.shelvesPanel.length; i++) {
                            _html += '<tr><td><table  style="height: 100%;table-layout:fixed">  <tr class="row' + (i + 1) + '"> ';
                            for (var j = 0; j < datas.shelvesPanel[i].CellNumber; j++) {
                                _html += '<td style="layout:fixed;height:20px;overflow: hidden;border:2px solid #666666; \
                                border-radius: 4px;\
                                border:1px solid #333;\
                                box-shadow:inset 0 0 5px 5px #ccc;" class="row' + datas.shelvesPanel[i].RowNumber + 'line' + (j + 1) + '" >  </td>'
                            }
                            _html += '</tr></table></td></tr>';
                        }
                        _html += '</table></div>'
                        $("#shelvesPanel")[0].innerHTML = _html;
                        //for (var i = 0; i < datas.instructions.length; i++) {
                        //    $(".row" + datas.instructions[i].LevelsNumber + 'line' + datas.instructions[i].SerialNumber)[0].style.color = "#f0e028";
                        //    $(".row" + datas.instructions[i].LevelsNumber + 'line' + datas.instructions[i].SerialNumber)[0].innerHTML += datas.instructions[i].SKU;
                        //    $(".row" + datas.instructions[i].LevelsNumber + 'line' + datas.instructions[i].SerialNumber)[0].style.backgroundColor = "red";
                        //} 
                        hidden();
                    } else {
                        //showMsg("单子的全部完成", "4000");
                        $(".Total")[0].innerHTML = "暂无订单推送！";
                        $(".body")[0].innerHTML = "";
                        $("#shelvesPanel")[0].innerHTML = "";
                        t = setTimeout(ReloadAjax, 3000);
                    }
                    //$("#shelvesPanel")[0].innerHTML = _html;
                },
                error: function (msg) {
                    showMsg("操作失败", "4000");
                }
            });
        }
    }

    $(".PickUpGoodsWalllocation ").live("click", function () {
        var self = this;
        layer.confirm('确定需要放置在此位置？', {
            btn: ['确定', '放弃'] //按钮
        }, function () {
            var tempOrder = "";
            $(".PickUpGoodsWalllocation").each(function (a, b) {
                $(b)[0].style.backgroundColor = "";
                if ($(b)[0].innerText != "") {
                    tempOrder = $(b)[0].innerText;
                    $(b)[0].innerText = "";
                }
            });
            $(self)[0].style.backgroundColor = "red";
            $(self)[0].innerText = tempOrder;
            $(".TemporaryLocation").val($(self).data().repickwalldetailid);
            layer.closeAll();
        });
        //, function () {

        //});

    })
    $("#PickUpGoodsManagement").click(function () {
        window.location.href = "/WMS/IntelligentOperation/GoodsManagement?WorkStationId=" + $("#WorkStationId").val();
    })
    function hidden() {
        $(".shelves>li").each(function (a, b) {
            if (a == 0) {
                $(b)[0].style.display = '';
                $($(b).data().id)[0].style.color = "#f0e028";
                $($(b).data().id)[0].innerText = $(b).data().sku;
                $($(b).data().id)[0].style.backgroundColor = "red";
                //if ($(b).data().repickwalldetailid != null) {
                //    $($(b).data().repickwalldetailid)[0].style.color = "#f0e028";
                //}
                IntelligentPickGoods.GetPickUpGoodsWall($("#INSTRUCTION_").find('option:selected').val());
                IntelligentPickGoods.PickUpGoodsWallOrder($(b).data().releatednumber);
            }
        })

    }
    function stopCount() {
        clearTimeout(t)
    }
    //$("#Query").click(function () {
    //    ReloadAjax();
    //})
    $("#Ack").click(function () {
        $("#Ack").val("确认");
        var GoodsShelve = "";
        if ($(".ActualQty:first-child").val() > 0 || $(".ActualQty:first-child").val() == undefined) {
            if ($("li:first-child").length > 0) {
                $.ajax({
                    type: "POST",
                    url: "/WMS/IntelligentOperation/SubmitData",
                    data: {
                        ID: $("li:first-child").data().releateddetailid,
                        RePickWallDetailId: $("#TemporaryLocation").val(),
                        ActualQty: $(".ActualQty:first-child").val()
                    },
                    async: "false",
                    success: function (data) {
                        if (data.Code = 1) {
                            GoodsShelve = $("li:first-child").data().goodsshelve;
                            $($("li:first-child").data().id)[0].style.color = "";
                            $($("li:first-child").data().id)[0].innerHTML = ""
                            $($("li:first-child").data().id)[0].style.backgroundColor = "";
                            $("li:first-child").remove();
                            if ($("li:first-child").length > 0) {
                                $("#OrderProgress")[0].innerHTML = (parseInt($("#OrderProgress")[0].innerText) + 1);
                                $("li:first-child")[0].style.display = ''
                                $($("li:first-child").data().id)[0].style.color = "#f0e028";
                                $($("li:first-child").data().id)[0].innerHTML = $("li:first-child").data().sku;
                                $($("li:first-child").data().id)[0].style.backgroundColor = "red";
                                IntelligentPickGoods.GetPickUpGoodsWall($("#INSTRUCTION_").find('option:selected').val());
                                IntelligentPickGoods.PickUpGoodsWallOrder($("li:first-child").data().releatednumber);
                            } else {
                                IntelligentPickGoods.ack(GoodsShelve);
                            }
                        }
                    },
                    error: function (msg) {
                        showMsg("操作失败", "4000");
                    }
                });

            } else {
                var index = layer.load(0, { shade: false }); //0代表加载的风格，支持0-2
                ReloadAjax();
            }
        } else {
            showMsg("请填写数量", "4000");
        }
    });

    $("#Reset").click(function () {
        layer.confirm('确定重置？', {
            btn: ['确定', '取消'] //按钮
        }, function () {
            IntelligentPickGoods.ResetFun();
            layer.msg('成功', { icon: 1 });
        });

    });
    var IntelligentPickGoods = {
        //----------------重置----------
        ResetFun: function () {
            layer.closeAll('loading');
            if ($("#WorkStationId").val() != "") {
                $.ajax({
                    type: "POST",
                    url: "/WMS/IntelligentOperation/CloseChannel",
                    data: {
                        WorkStationId: $("#WorkStationId").val()
                    },
                    async: "false",
                    success: function (data) {
                        if (data.Code == 1) {
                            stopCount();
                            $("#Ack").val("开始");
                            //setTimeout(ReloadAjax, 1000);
                            //$("#label").val(data.Body);
                        }

                    },
                    error: function (msg) {
                        showMsg("操作失败", "4000");
                    }
                });
            }
        },
        //--------------------------Ack--------------------------
        ack: function (GoodsShelve) {
            var index = layer.load(0, { shade: false }); //0代表加载的风格，支持0-2
            if ($("#WorkStationId").val() != "") {
                $.ajax({
                    type: "POST",
                    url: "/WMS/IntelligentOperation/ValidateMQ",
                    data: {
                        WorkStationId: $("#WorkStationId").val(),
                        GoodsShelve: GoodsShelve
                    },
                    async: "false",
                    success: function (data) {
                        if (data.Code == 1) {
                            ReloadAjax();
                            //$("#label").val(data.Body);
                        }
                    },
                    error: function (msg) {
                        showMsg("操作失败", "4000");
                    }
                });
            }
        },
        //----------------------获取分拣墙--------------------------
        GetPickUpGoodsWall: function (GoodsShelve) {
            $.ajax({
                type: "POST",
                url: "/WMS/IntelligentOperation/GetPickUpGoodsWall",
                data: {
                    WorkStationId: $("#WorkStationId").val(),
                    GoodsShelve: GoodsShelve
                },
                async: false,
                success: function (data) {
                    if (data.Code = 1) {
                        var _html = '<div  style="height: 500px;width: 350px;-moz-box-shadow: 10px 10px 5px #888888; /* 老的 Firefox */box-shadow: 10px 10px 5px #888888">\
                            <table id="PickUpGoodsWall" class="PickUpGoodsWall" style="width: 350px;height: 100%;border:2px solid #666666">';
                        for (var i = 0; i < data.Rows; i++) {
                            _html += '<tr><td><table  style="height:120px;table-layout:fixed;">  <tr class="row' + (i + 1) + '"> ';
                            for (var j = 0; j < data.Cells; j++) {
                                for (var k = 0; k < data.pickUpGoodsWall.length; k++) {
                                    if (data.pickUpGoodsWall[k].RowNumber == (i + 1) && data.pickUpGoodsWall[k].CellNumber == (j + 1))
                                        if (data.pickUpGoodsWall[k].OrderNumber != null) {
                                            _html += '<td style="layout:fixed; border:2px solid #666666;word-wrap:break-word;\
                                            border-radius: 4px;\
                                            border:1px solid #333;\
                                            box-shadow:inset 0 0 5px 5px #ccc;" data-ordernumber="' + data.pickUpGoodsWall[k].OrderNumber + '" data-repickwalldetailid=' + data.pickUpGoodsWall[k].RePickWallDetailID + ' class="row' + (i + 1) + 'line' + (j + 1) + ' PickUpGoodsWalllocation ShelvesStyle" > </td>'// + data.pickUpGoodsWall[k].OrderNumber + 
                                        } else {
                                            _html += '<td style="layout:fixed; border:2px solid #666666;word-wrap:break-word;" data-ordernumber="" data-repickwalldetailid=' + data.pickUpGoodsWall[k].RePickWallDetailID + ' class="row' + (i + 1) + 'line' + (j + 1) + '  PickUpGoodsWalllocation ShelvesStyle" > </td>'
                                        }
                                }

                            }
                            _html += '</tr></table></td></tr>';
                        }
                        _html += '</table></div>'
                        $("#PutGoods")[0].innerHTML = _html;
                    }
                },
                error: function (msg) {
                    showMsg("操作失败", "4000");
                }
            });
        },
        //---------------------------------------------放置位置--------------------------------------
        PickUpGoodsWallOrder: function (order) {
            $(".PickUpGoodsWalllocation").each(function (a, b) {
                if ($(b).data().ordernumber == order) {
                    //$(b)[0].style.color = "#f0e028";
                    $(b)[0].innerHTML = order;
                    $(b)[0].style.backgroundColor = "red";
                    $("#TemporaryLocation").val($(b).data().repickwalldetailid);
                    return false;
                } else if (a == ($(".PickUpGoodsWalllocation").length - 1)) {
                    $(".PickUpGoodsWalllocation").each(function (a, b) {
                        if ($(b).data().ordernumber == "") {
                            $(b)[0].innerHTML = order;
                            $(b)[0].style.backgroundColor = "red";
                            $("#TemporaryLocation").val($(b).data().repickwalldetailid);
                            return false;
                        }

                    })
                }
            })
        }
    }

    //var Initialize = {
    //    shelvesPanel: function (id) {
    //        $.ajax({
    //            type: "POST",
    //            url: "/WMS/IntelligentOperation/ShelvesPanel",
    //            data: {
    //                id: id
    //            },
    //            async: "false",
    //            success: function (data) {
    //                if (data.Code == 1) {
    //                    //setTimeout(ReloadAjax, 1000);

    //                }

    //            },
    //            error: function (msg) {
    //                showMsg("操作失败", "4000");
    //            }
    //        });
    //    }
    //}

})