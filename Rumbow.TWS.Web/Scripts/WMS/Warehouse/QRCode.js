var id = "";
var a = false;
var OperationObj = null;
var ChargeObj = null;
$(document).ready(function () {
    var body = $("#resultTable");
    body.mousedown(function () {
        a = true;
    });
    body.mouseup(function () {
        a = false;
    });
    $("#OperationTable").removeAttr("style");
    $("#OperationTable tr").click(function () {
        $(this).addClass("btn-success").siblings("tr").removeClass("btn-success");
    });
    $("#OperationTable tr").mouseover(function () {
        $(this).addClass("btn-info").siblings("tr").removeClass("btn-info");
    });

    $("#OperationTable tr").mouseleave(function () {
        $(this).removeClass("btn-info");
    });
    $("#OperationTable tr").dblclick(function () {
        //var rowIndex = $("#resultTable tr").index($(this));
        $("#OperationID").val($(this).children()[0].innerText.trim());
        if (OperationObj.innerText == "⇨" || OperationObj.innerText == "⇩") {
            OperationObj.attributes["mapid"].value = $("#OperationID").val();
            OperationObj.style.color = "#9c23e5";
        }
        if (MapIDCheck($(this).children()[0].innerText, 2, 0) && OperationObj.attributes["maptype"].value!="5") {
            layer.confirm('<font size="4">此操作台已关联，是否继续？</font>', {
                btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                //shade: [0.8, '#393D49'],
                title: ['提示', 'font-size:18px;']
                //按钮
            }, function (index) {
                layer.close(index);
                MapIDCheck( $("#OperationID").val(),2, 1)
                OperationObj.attributes["mapid"].value =  $("#OperationID").val();
                OperationObj.innerText = "✔";
                OperationObj.style.fontSize = "60%";
                OperationObj.style.color = "#fff";
                OperationObj.style.padding = "0px";
                OperationObj.style.textAlign = "center";
                closePopup();
                return;
            });
        }
        else {
            if (OperationObj.attributes["maptype"].value != "5") {
                OperationObj.attributes["mapid"].value = $("#OperationID").val();
                OperationObj.innerText = "✔";
                OperationObj.style.fontSize = "60%";
                OperationObj.style.color = "#fff";
                OperationObj.style.padding = "0px";
                OperationObj.style.textAlign = "center";
            }
        }
        //OperationObj.attributes["mapid"].value = $("#OperationID").val();
        //OperationObj.innerText = "✔";
        //OperationObj.style.fontSize = "60%";
        //OperationObj.style.color = "#fff";
        //OperationObj.style.padding = "0px";
        //OperationObj.style.textAlign = "center";
        closePopup();

    });


    $("#ChargeTable").removeAttr("style");
    $("#ChargeTable tr").click(function () {
        $(this).addClass("btn-success").siblings("tr").removeClass("btn-success");
    });
    $("#ChargeTable tr").mouseover(function () {
        $(this).addClass("btn-info").siblings("tr").removeClass("btn-info");
    });

    $("#ChargeTable tr").mouseleave(function () {
        $(this).removeClass("btn-info");
    });
    $("#ChargeTable tr").dblclick(function () {
        //var rowIndex = $("#resultTable tr").index($(this));
        var s=$(this).children()[0].innerText.trim();
        if (MapIDCheck(s,4, 0)) {
            layer.confirm('<font size="4">此充电桩已关联，是否继续？</font>', {
                btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                //shade: [0.8, '#393D49'],
                title: ['提示', 'font-size:18px;']
                //按钮
            }, function (index) {
                layer.close(index);
                MapIDCheck(s,4, 1)
                ChargeObj.attributes["mapid"].value = s;
                ChargeObj.innerText = "✔";
                ChargeObj.style.fontSize = "60%";
                ChargeObj.style.color = "#fff";
                ChargeObj.style.padding = "0px";
                ChargeObj.style.textAlign = "center";
                closePopup();
                return;
            });
        }
        else {
            ChargeObj.attributes["mapid"].value = s;
            ChargeObj.innerText = "✔";
            ChargeObj.style.fontSize = "60%";
            ChargeObj.style.color = "#fff";
            ChargeObj.style.padding = "0px";
            ChargeObj.style.textAlign = "center";
        }
        closePopup();

    });

    $("#portButtonTemplet").live('click', function () {
        var JsonTable = addjsontotable();
        post('/WMS/ReportManagement/ExportBussiness', { JsonTable: JsonTable });
    });
    $('select[id=SearchCondition_WarehouseID]').live('change', function () {
        window.location.href = "/WMS/Warehouse/QRCode/?customerID=" + $("#SearchCondition_CustomerID").val() + "&WarehouseID=" + $("#SearchCondition_WarehouseID").val();
    });


    $("#addButton").live('click', function () {
        var m = $("#lengths").val();
        var n = $("#widths").val();
        if (m == "")
        {
            showMsg("请输入长度!", "1000");
            return;
        }
        if (n == "") {
            showMsg("请输入宽度!", "1000");
            return;
        }
        addNew(m, n);
    });
    $("#SendButton").live('click', function () {
        var MapTypejson = MapTypejsontotable();
        $.ajax({
            type: "POST",
            url: "/WMS/Warehouse/SendMap",
            data: {
                "MapTypejson":MapTypejson
            },
            async: "false",
            success: function (data) {
                if (data == "") {
                    showMsg("发送成功！", "4000");
                }
                else {
                    showMsg("发送失败！", "4000");
                }
            },
            error: function (msg) {
            }
        });
    });
    $("#resultTable td").mouseover(function (e) {
        if (this.attributes["maptype"].value.trim() == "3" && this.attributes["mapid"].value.trim() != "0") {
            $.ajax({
                type: "Get",
                url: "/WMS/Warehouse/GetLocationByGoodShelf",
                data: {
                    "id": this.attributes["mapid"].value,
                    "WarehouseID": $("#WarehouseID").val()
                },
                async: "false",
                success: function (data) {
                    var table1 = $('#LocationTable');
                    $("#LocationTable tbody").empty()
                    for (var i = 0; i < data.length; i++) {
                        var row = $("<tr></tr>");
                        var td0 = $("<td>" + data[i].GoodsShelvesName + "</td>");
                        var td1 = $("<td>" + data[i].Location + "</td>");
                        row.append(td0);
                        row.append(td1);
                        table1.append(row);
                    }
                    show(e, "mydiv1");
                },
                error: function (msg) {

                }
            });
        }
        else if ((this.attributes["maptype"].value.trim() == "2" || this.attributes["maptype"].value.trim() == "5") && this.attributes["mapid"].value.trim() != "0")
        {
            var OperationList = $("#OperationList");
            if (OperationList[0].length > 0) {
                var table1 = $('#OpTable');
                $("#OpTable tbody").empty();
                for (var i = 0; i < OperationList[0].length; i++) {
                    if (this.attributes["mapid"].value.trim() == OperationList[0][i].value.trim())
                    {
                        var row = $("<tr></tr>");
                        var td0 = $("<td>" + OperationList[0][i].innerText + "</td>");
                        row.append(td0);
                        table1.append(row);
                       
                    }
                }
                show(e, "mydiv2");
            }
        }
        else if (this.attributes["maptype"].value.trim() == "4" && this.attributes["mapid"].value.trim() != "0") {
            var ChargeList = $("#ChargeList");
            if (ChargeList[0].length > 0) {
                var table1 = $('#ChTable');
                $("#ChTable tbody").empty();
                for (var i = 0; i < ChargeList[0].length; i++) {
                    if (this.attributes["mapid"].value.trim() == ChargeList[0][i].value.trim()) {
                        var row = $("<tr></tr>");
                        var td0 = $("<td>" + ChargeList[0][i].innerText + "</td>");
                        row.append(td0);
                        table1.append(row);

                    }
                }
                show(e, "mydiv3");
            }
        }
        else {
            hide();
        }
    })
    $("#resultTable").mouseleave(function (e) {
        hide();
    })

    $("#typeTable td").bind("click", function () {
        var s = this.innerText;
         id = this.id;
        $("#lb")[0].innerText = s;
    })
    $("#DeleteButton").live('click', function () {
        layer.confirm('<font size="4">确认是否删除当前平面图？</font>', {
            btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            $.ajax({
                type: "POST",
                url: "/WMS/Warehouse/MapDelete",
                data: {
                    "WareHouseID": $("#WarehouseID").val()
                },
                async: "false",
                success: function (data) {                   
                    showMsg("删除成功！", "4000");
                    location.href = "/WMS/Warehouse/QRCode?WarehouseID=" + $("#WarehouseID").val()
                },
                error: function (msg) {
                    showMsg("删除失败", "4000");
                }
            });
        });
    });
    $("#lengths").bind('input propertychange', function() {
        var pattern = /[^0-9]/g;
        if (pattern.test(this.value)) {
            this.value = this.value.replace(pattern, "");
        }
    });

    $("#widths").bind('input propertychange', function () {
        var pattern = /[^0-9]/g;
        if (pattern.test(this.value)) {
            this.value = this.value.replace(pattern, "");
        }
    });
    $("#resultTable td").live('dblclick', function () {
        if (this.attributes['maptype'].value == 3) {
            var obj = this;
            openPopup("GoodsShelfPop", true, 1000, 600, '/WMS/Warehouse/GoodsShelves/?customerID=' + $('#CustomerID').val() + '&Flag=1' + '&WarehouseID=' + $('#WarehouseID').val(), null, function (GoodsShelfID) {
                if (MapIDCheck(GoodsShelfID,3,0)) {
                    layer.confirm('<font size="4">此货架已关联，是否继续？</font>', {
                        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                        //shade: [0.8, '#393D49'],
                        title: ['提示', 'font-size:18px;']
                        //按钮
                    }, function (index) {
                        layer.close(index);
                        MapIDCheck(GoodsShelfID,3, 1)
                        obj.attributes["mapid"].value = GoodsShelfID;
                        obj.innerText = "✔";
                        obj.style.fontSize = "60%";
                        obj.style.color = "#fff";
                        obj.style.padding = "0px";
                        obj.style.textAlign = "center";
                        return;
                    });
                }
                else {
                    obj.attributes["mapid"].value = GoodsShelfID;
                    obj.innerText = "✔";
                    obj.style.fontSize = "60%";
                    obj.style.color = "#fff";
                    obj.style.padding = "0px";
                    obj.style.textAlign = "center";
                }
                //obj.style.font-size="60%";

                //font-size:60%;color:#f00;padding:0px;text-align:center
            });
            $("#popupLayer_GoodsShelfPop")[0].style.top = "50px";
        }
        if (this.attributes['maptype'].value == 2) {
            OperationObj = this;
            openPopup("OperationPop", true, 1000, 600, null, 'Operation')
            $("#popupInlineDiv")[0].style.height = "550px";
            $("#popupInlineDiv")[0].style.overflowY = "scroll";
            $("#popupLayer_OperationPop")[0].style.top = "50px";
        }
        if (this.attributes['maptype'].value == 4) {
            ChargeObj = this;
            openPopup("ChargePop", true, 1000, 600, null, 'Charge');
            $("#popupInlineDiv")[0].style.height = "550px";
            $("#popupInlineDiv")[0].style.overflowY = "scroll";
            //$("#OperationTable")[0].style.height = "500px";
            $("#popupLayer_ChargePop")[0].style.top = "50px";
        }
        if (this.attributes['maptype'].value == 5 && (this.innerText == "⇨" || this.innerText == "⇩"))
        {
            OperationObj = this;
            openPopup("OperationPop", true, 1000, 600, null, 'Operation')
            $("#popupInlineDiv")[0].style.height = "550px";
            $("#popupInlineDiv")[0].style.overflowY = "scroll";
            $("#popupLayer_OperationPop")[0].style.top = "50px";
        }
    })

    $("#resultTable td").live('click', function () {
        //$("#StatusbackID").val(this.id);
        //openPopup('pop99', true, 350, 300, null, 'statusBackDiv');
        //$("#popupLayer_pop99")[0].style.top = "200px";
        var i = id;
        switch (i) {
            case "1":
                this.style.backgroundColor = "#00ff21";
                this.attributes["maptype"].value = "1";
                this.attributes["mapid"].value = 0;
                this.attributes["str1"].value = 0;
                this.innerText = "";
                break;
            case "2":
                this.style.backgroundColor = "#9c23e5";              
                if (this.attributes["maptype"].value != "2")
                {
                    this.attributes["maptype"].value = "2";
                    this.attributes["mapid"].value = 0;
                    this.attributes["str1"].value = 0;
                    this.innerText = "";
                }
                //this.attributes["mapid"].value = 0;
                //this.innerText = "";
                break;
            case "3":

                this.style.backgroundColor = "#3131f7";
                if (this.attributes["maptype"].value != "3") {
                    this.attributes["maptype"].value = "3";
                    this.attributes["mapid"].value = 0;
                    this.attributes["str1"].value = 0;
                    this.innerText = "";
                }
                //Confirms(this.id)
                break;
            case "4":
                this.style.backgroundColor = "#ecf520";               
                if (this.attributes["maptype"].value != "4") {
                    this.attributes["maptype"].value = "4";
                    this.attributes["mapid"].value = 0;
                    this.attributes["str1"].value = 0;
                    this.innerText = "";
                }
                //this.attributes["mapid"].value = 0;
                //this.innerText = "";
                break;
            case "5":
                this.style.backgroundColor = "#564c4c";
                this.attributes["maptype"].value = "5";
                this.attributes["mapid"].value = 0;
                this.attributes["str1"].value = 0;
                this.innerText = "〇";
                this.style.fontSize = "60%";
                this.style.color = "#00ff21";
                this.style.padding = "0px";
                this.style.textAlign = "center";
                break;
            case "6":
                this.style.backgroundColor = "#f00";
                this.attributes["maptype"].value = "6";
                this.attributes["mapid"].value = 0;
                this.innerText = "";
                break;
            case "51":
                this.style.backgroundColor = "#564c4c";
                this.attributes["maptype"].value = "5";
                this.attributes["mapid"].value = 0;
                this.attributes["str1"].value = 1;
                this.innerText = "⇨";
                this.style.fontSize = "60%";
                this.style.color = "#00ff21";
                this.style.padding = "0px";
                this.style.textAlign = "center";
                break;
            case "52":
                this.style.backgroundColor = "#564c4c";
                this.attributes["maptype"].value = "5";
                this.attributes["mapid"].value = 0;
                this.attributes["str1"].value = 2;
                this.innerText = "⇩";
                this.style.fontSize = "60%";
                this.style.color = "#00ff21";
                this.style.padding = "0px";
                this.style.textAlign = "center";
                break;
            default:
                break;
        }
    });
    //$("#statusBackOK").live('click', function () {
    //    //var s = $("#StatusbackID").val();
    //    var i=id;
    //    switch (i)
    //    {
    //        case "1":
    //            this.style.backgroundColor = "#00ff21";
    //            this.className = "1";
    //            this.GoodsShelfID = "0";

    //            //$("#" + s)[0].style.backgroundColor = "#00ff21";
    //            //$("#" + s)[0].className = "1";
    //            //$("#" + s)[0].GoodsShelfID = "0";
    //            closePopup();
    //            break;
    //        case "2":
    //            this.style.backgroundColor = "#f00";
    //            this.className = "2";
    //            this.GoodsShelfID = "0";


    //            //$("#" + s)[0].style.backgroundColor = "#f00";
    //            //$("#" + s)[0].className = "2";
    //            //$("#" + s)[0].GoodsShelfID = "0";
    //            closePopup();
    //            break;
    //        case "3":
    //            $("#" + s)[0].style.backgroundColor = "#3131f7";
    //            $("#" + s)[0].className = "3";
    //            closePopup();
    //            Confirms(s)
    //            break;
    //        case "4":
    //            $("#" + s)[0].style.backgroundColor = "#ecf520";
    //            $("#" + s)[0].className = "4";
    //            $("#" + s)[0].GoodsShelfID = "0";
    //            closePopup();
    //            break;
    //        case "5":
    //            $("#" + s)[0].style.backgroundColor = "#111010";
    //            $("#" + s)[0].className = "5";
    //            $("#" + s)[0].GoodsShelfID = "0";
    //            closePopup();
    //            break;
    //        case "6":
    //            $("#" + s)[0].style.backgroundColor = "#e8eef4";
    //            $("#" + s)[0].className = "6";
    //            $("#" + s)[0].GoodsShelfID = "0";
    //            closePopup();
    //            break;
    //        case "7":
    //            $("#" + s)[0].style.backgroundColor = "#0ff";
    //            $("#" + s)[0].className = "7";
    //            $("#" + s)[0].GoodsShelfID = "0";
    //            closePopup();
    //            break;
    //        case "8":
    //            $("#" + s)[0].style.backgroundColor = "#564c4c";
    //            $("#" + s)[0].className = "8";
    //            $("#" + s)[0].GoodsShelfID = "0";
    //            closePopup();
    //            break;
    //        case "9":
    //            $("#" + s)[0].style.backgroundColor = "#9c23e5";
    //            $("#" + s)[0].className = "9";
    //            $("#" + s)[0].GoodsShelfID = "0";
    //            closePopup();
    //            break;
    //        default:
    //            break;
    //    }
    //});
    $("#SaveButton").live('click', function () {
        if ($("#resultTable tr").length == 0)
        {
            showMsg("请先生成平面图！", "4000");
            return;
        }
        if ($("#SearchCondition_WarehouseID").val() == "") {
            showMsg("请选择仓库！", "4000");
            return;
        }
        var JsonTable = addjsontotable();
        var MapTypejson = MapTypejsontotable();
        $.ajax({
            type: "POST",
            url: "/WMS/Warehouse/SaveQRCode",
            data: {
                "JsonString": JsonTable,
                "WareHouseID": $("#WarehouseID").val(),
                "Length": $("#lengths").val(),
                "Width": $("#widths").val(), 
                "Flag": 1,
                "MapTypejson": MapTypejson
                
            },
            async: "false",
            success: function (data) {
                if (data == "OK")
                {
                    showMsg("保存成功", "4000");
                    location.href = "/WMS/Warehouse/QRCode?WarehouseID=" + $("#WarehouseID").val()
                }
            },
            error: function (msg) {
                showMsg("保存失败", "4000");
            }
        });
    });
    $("#EditButton").live('click', function () {
        if ($("#SearchCondition_WarehouseID").val() == "") {
            showMsg("请选择仓库！", "4000");
            return;
        }
        var JsonTable = Editjsontotable();
        var MapTypejson = MapTypejsontotable();
        $.ajax({
            type: "POST",
            url: "/WMS/Warehouse/EditQRCode",
            data: {
                "JsonString": JsonTable,
                "WareHouseID": $("#WarehouseID").val(),
                "Flag": 2,
                "MapTypejson": MapTypejson
            },
            async: "false",
            success: function (data) {
                if (data == "OK") {
                    showMsg("设置成功", "4000");
                }
            },
            error: function (msg) {
                showMsg("设置失败", "4000");
            }
        });
    });
    $("#statusBackReturn").live('click', function () {
        closePopup();
    });
});

function addNew(m, n) {
    $("#resultTable").empty();
    var table1 = $("#resultTable");
    
    for (var i = 0; i < m; i++)
    {
        var k1 = i;
        var k = ReturnLineNumber(k1) + k1;
        var Y=i;
        var row = $("<tr></tr>");
        for (var j = 0; j < n; j++)
        {
            var X=j;
            var s1 = j;
            var s = ReturnLineNumber(s1) + s1;
            var h=s+k;
            var td = $("<td id=" + h + " Y=" + Y + " X=" + X + " Str1=0 MapID=0 MapType=1 style='background-color:#00ff21;'  onMouseOver='changeColor(this)' onMouseout='changeColor(this)'></td>");
            row.append(td);
        }
        table1.append(row);
    }
}

function ReturnLineNumber(row_count) {
    var linnumber = "";
    if (row_count < 10) {
        linnumber = "00" ;
    }
    if (100 > row_count && row_count >= 10) {
        linnumber = "0" ;
    }
    
    return linnumber;
}

function addjsontotable() {
    var txt = "[";
    var table = document.getElementById("resultTable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("td");

    for (var j = 0; j < row.length; j++) {
       
        for (var i = 0; i < col.length; i++) {
            var r = "{";
            var tds = row[j].getElementsByTagName("td");           
            r += "\"" + "QRCode" + "\"\:\"" + tds[i].id + "\"," + "\"" + "X" + "\"\:\"" + tds[i].attributes["x"].value + "\"," + "\"" + "Y" + "\"\:\"" +
            tds[i].attributes["y"].value + "\"," + "\"" + "MapType" + "\"\:\"" + tds[i].attributes["MapType"].value + "\"," + "\"" + "MapID" + "\"\:\""
            + tds[i].attributes["MapID"].value + "\","+ "\"" + "Str1" + "\"\:\"" + tds[i].attributes["Str1"].value + "\",";
            r = r.substring(0, r.length - 1)
            r += "},";
            txt += r;
        }
       
    }

    txt = txt.substring(0, txt.length - 1);
    txt += "]";
    return txt;
}
function Editjsontotable() {
    var txt = "[";
    var table = document.getElementById("resultTable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("td");

    for (var j = 0; j < row.length; j++) {

        for (var i = 0; i < col.length; i++) {
            var r = "{";
            var tds = row[j].getElementsByTagName("td");
            r += "\"" + "QRCode" + "\"\:\"" + tds[i].id + "\"," + "\"" + "MapType" + "\"\:\"" + tds[i].attributes['maptype'].value.trim()
                + "\"," + "\"" + "MapID" + "\"\:\"" + tds[i].attributes['mapid'].value + "\"," + "\"" + "Str1" + "\"\:\"" + tds[i].attributes["Str1"].value + "\",";
            r = r.substring(0, r.length - 1)
            r += "},";
            txt += r;
        }

    }

    txt = txt.substring(0, txt.length - 1);
    txt += "]";
    return txt;
}
function MapTypejsontotable() {
    var txt = "";
    var table = document.getElementById("resultTable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("td");

    for (var j = 0; j < row.length; j++) {

        for (var i = 0; i < col.length; i++) {
            var r = "";
            var tds = row[j].getElementsByTagName("td");
            if (tds[i].attributes['maptype'].value.trim() == "5")
            {
                r += tds[i].attributes['maptype'].value.trim() + " " + tds[i].attributes['str1'].value.trim() + " " + tds[i].attributes['mapid'].value.trim() + "\r\n";
            }
            else
            {
                r += tds[i].attributes['maptype'].value.trim() + " " + tds[i].attributes['mapid'].value.trim() + " " + 0 + "\r\n";
            }
            txt += r;
        }

    }
    return txt;
}
function MapIDCheck(MapID,MapType,flag) {
    var table = document.getElementById("resultTable");
    var row = table.getElementsByTagName("tr");
    var col = row[0].getElementsByTagName("td");
    for (var j = 0; j < row.length; j++) {
        for (var i = 0; i < col.length; i++) {
            var tds = row[j].getElementsByTagName("td");
            if (tds[i].attributes['mapid'].value.trim() == MapID && tds[i].attributes['maptype'].value.trim() == MapType) {
                if (flag == 1)
                {
                    tds[i].innerText = "";
                    tds[i].attributes["mapid"].value = 0;
                }
                return true;

                }
            }
           
        }
}

function Confirms(s) {
    layer.confirm('<font size="4">是否关联货架？</font>', {
        btn: ['是', '否'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        openPopup("GoodsShelfPop", true, 1000, 600, '/WMS/Warehouse/GoodsShelves/?customerID=' + $('#CustomerID').val() + '&Flag=1' + '&WarehouseID=' + $('#WarehouseID').val(), null, function (GoodsShelfID) {
            $("#" + s)[0].GoodsShelfID = GoodsShelfID;

        });
        $("#popupLayer_GoodsShelfPop")[0].style.top = "50px";
    });

}

function changeColor(obj) {
    if (a == true) {
        var i = id;
        switch (i) {
            case "1":
                obj.style.backgroundColor = "#00ff21";
                obj.attributes["maptype"].value = "1";
                obj.attributes["mapid"].value = 0;
                obj.innerText = "";
                break;
            case "2":
                obj.style.backgroundColor = "#9c23e5";
                if (obj.attributes["maptype"].value != "2") {
                    obj.attributes["maptype"].value = "2";
                    obj.attributes["mapid"].value = 0;
                    obj.innerText = "";
                }
                //this.attributes["mapid"].value = 0;
                //this.innerText = "";
                break;
            case "3":

                obj.style.backgroundColor = "#3131f7";
                if (obj.attributes["maptype"].value != "3") {
                    obj.attributes["maptype"].value = "3";
                    obj.attributes["mapid"].value = 0;
                    obj.innerText = "";
                }
                //Confirms(this.id)
                break;
            case "4":
                obj.style.backgroundColor = "#ecf520";
                if (obj.attributes["maptype"].value != "4") {
                    obj.attributes["maptype"].value = "4";
                    obj.attributes["mapid"].value = 0;
                    obj.innerText = "";
                }
                //this.attributes["mapid"].value = 0;
                //this.innerText = "";
                break;
            case "5":
                obj.style.backgroundColor = "#564c4c";
                obj.attributes["maptype"].value = "5";
                obj.attributes["mapid"].value = 0;
                obj.attributes["str1"].value = 0;
                obj.innerText = "〇";
                obj.style.fontSize = "60%";
                obj.style.color = "#00ff21";
                obj.style.padding = "0px";
                obj.style.textAlign = "center";
                break;
            case "6":
                obj.style.backgroundColor = "#f00";
                obj.attributes["maptype"].value = "6";
                obj.attributes["mapid"].value = 0;
                obj.innerText = "";
                break;
            case "51":
                obj.style.backgroundColor = "#564c4c";
                obj.attributes["maptype"].value = "5";
                obj.attributes["mapid"].value = 0;
                obj.attributes["str1"].value = 1;
                obj.innerText = "⇨";
                obj.style.fontSize = "60%";
                obj.style.color = "#00ff21";
                obj.style.padding = "0px";
                obj.style.textAlign = "center";
                break;
            case "52":
                obj.style.backgroundColor = "#564c4c";
                obj.attributes["maptype"].value = "5";
                obj.attributes["mapid"].value = 0;
                obj.attributes["str1"].value = 2;
                obj.innerText = "⇩";
                obj.style.fontSize = "60%";
                obj.style.color = "#00ff21";
                obj.style.padding = "0px";
                obj.style.textAlign = "center";
                break;

            default:
                break;
        }
    }
    //else {
    //    if (obj.attributes["maptype"].value.trim() == "3") {
         
    //    }
    //}

}
//function ShowDiv(obj) {
//    $.ajax({
//        type: "Get",
//        url: "/WMS/Warehouse/GetLocationByGoodShelf",
//        data: {
//            "id": obj.attributes["mapid"].value,
//            "WarehouseID": $("#WarehouseID").val()
//        },
//        async: "false",
//        success: function (data) {
//            var table1 = $('#LocationTable');
//            $("#LocationTable tbody").empty()
//            for (var i = 0; i < data.length; i++) {
//                var row = $("<tr></tr>");
//                var td = $("<td>" + data[i] + "</td>");
//                row.append(td);
//                table1.append(row);
//            }
//            show(obj, 'mydiv1')
//        },
//        error: function (msg) {

//        }
//    });
//}

//function ShowDiv(obj)
//{
//    $.ajax({
//        type: "Get",
//        url: "/WMS/Warehouse/GetLocationByGoodShelf",
//        data: {
//            "id": obj.attributes["mapid"].value,
//            "WarehouseID": $("#WarehouseID").val()
//        },
//        async: "false",
//        success: function (data) {
//            var table1 = $('#LocationTable');
//            $("#LocationTable tbody").empty()
//            for (var i = 0; i < data.length; i++) {
//                var row = $("<tr></tr>");
//                var td = $("<td>" + data[i] + "</td>");
//                row.append(td);
//                table1.append(row);
//            }
//            show(obj, 'mydiv1')
//        },
//        error: function (msg) {

//        }
//    });
//}



function post(URL, PARAMS) {
    var temp = document.createElement("form");
    temp.action = URL;
    temp.method = "post";
    temp.style.display = "none";
    for (var x in PARAMS) {
        var opt = document.createElement("textarea");
        opt.name = x;
        opt.value = PARAMS[x];
        // alert(opt.name)        
        temp.appendChild(opt);
    }
    document.body.appendChild(temp);
    temp.submit();
    return temp;
}
                                 
             

//function addjsontotable() {
//    var txt = "[";
//    var table = document.getElementById("resultTable");
//    var row = table.getElementsByTagName("tr");
//    var col = row[0].getElementsByTagName("th");
//    for (var j = 1; j < row.length; j++) {
//        var r = "{";
//        for (var i = 0; i < col.length - 1; i++) {
//            var tds = row[j].getElementsByTagName("td");
//            if ($(tds[i]).children("input").length > 0) {
//                r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + $(tds[i]).children("input")[0].value.trim() + "\",";
//            } else if ($(tds[i]).children("select").length > 0) {
//                r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + $(tds[i]).children("select").find('option:selected').val().trim() + "\",";
//            } else {
//                r += "\"" + col[i].innerHTML.trim() + "\"\:\"" + tds[i].innerText.trim().trim() + "\",";
//            }


//        }
//        r = r.substring(0, r.length - 1)
//        r += "},";
//        txt += r;
//    }
//    txt = txt.substring(0, txt.length - 1);
//    txt += "]";
//    return txt;
//}


function show(e,id) {

    var objDiv = $("#" + id + "");

    $(objDiv).css("display", "block");

    $(objDiv).css("left", e.clientX + 1);

    $(objDiv).css("top", e.clientY + 10);

}

function hide() {

    var objDiv = $("#" + id + "");

    $("#mydiv1").css("display", "none");
    $("#mydiv2").css("display", "none");
    $("#mydiv3").css("display", "none");

}
