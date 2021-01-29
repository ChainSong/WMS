$(document).ready(function () {
    $(".Ooperation").live("mouseover", function () {
        $(this).prev()[0].style.display = "none";
    });
    $(".Ooperation").live("mouseenter", function () {
        $(this).prev()[0].style.display = "";
    })
    $(".Adiv").live("mouseleave", function () {
        $(this)[0].style.display = "none";
    })
    $("tr").live("mouseenter", function () {
        $(".Adiv").each(function (a, b) {
            $(b)[0].style.display = "none";
        })
    })
    $('#returnButton').click(function () {
        history.back();
        //window.location.href = history.go(-1);
        //"/WMS/Product/Index";
    });
    //得到当前的单元格
    var currentCell;
    function editCell(event) {
        if (event == null) {
            currentCell = window.event.srcElement;
        }
        else {
            currentCell = event.target;
        }
        if (currentCell.tagName == "TD") {
            //用单元格的值来填充文本框的值
            input.value = currentCell.innerHTML;
            //当文本框丢失焦点时调用last
            input.onblur = last;
            input.ondblclick = last;
            currentCell.innerHTML = "";
            //把文本框加到当前单元格上.
            currentCell.appendChild(input);
            //根据liu_binq63 的建议修定下面的bug 非常感谢
            input.focus();
        }
    }
    function last() {
        //充文本框的值给当前单元格
        currentCell.innerHTML = input.value;
    }
    $(".AddButton").live("click", function () {
        // var html = $("#Operation").render("");
        //$("#chaifen")["empty"]();
        //$("#chaifen").parent().parent().append(html);
        var myTable = document.getElementById("resultTable");
        var rowIndex = event.srcElement.parentNode.parentNode.parentNode.rowIndex;//当前行的行号
        var obj = document.getElementById('resultTable').getElementsByTagName('tr');
        var newrow = obj[rowIndex].cloneNode(true);
        // var col = document.getElementsByTagName('tr'), i, td;
        // td = col[rowIndex].getElementsByTagName('td')[0].innerHTML("");
        //这个td就是tr首个td 
        newrow.setAttribute("class", "NewTableBGColor");
        //  newrow.ondblclick = editCell;
        //alert(newrow);
        $(this).parent().parent().parent().parent().after(newrow);
        //dom创建文本框
        //var input = document.createElement("input");
        //input.style.width = '80px';
        //input.type = "text";
        //input.ondblclick = editCells;
        // var tab = document.getElementById("table1");  //找到这个表格
        var rows = myTable.rows;

        //var cell = rows[rowIndex + 1].cells[15];
        //cell.innerHTML = " <label class='deleteSettledPod labelPointer'  onclick='Del(this," + rows[rowIndex].cells[1].innerText + ")'>删除</label>"
        //var celllocation = rows[rowIndex + 1].cells[7];
        //celllocation.innerHTML = " <input  role='textbox'aria-haspopup='true' class='form-control' onblur=la('" + rows[rowIndex].cells[1].innerText + "','" + rows[rowIndex].cells[6].innerText + "','" + rows[rowIndex].cells[11].children[0].value + "','" + rows[rowIndex].cells[12].children[0].value + "') aria-autocomplete='list' value='" + subtraction(rows[rowIndex].cells[6].innerText, rows[rowIndex].cells[7].children[0].value) + "' name='warehouse' style='width: 80px;' type='text'>";
        //var celllocation = rows[rowIndex + 1].cells[10];
        //celllocation.innerHTML = "  <input type='text' class='Location form-control' style='width: 120px;' placeholder='' value='' autofocus/>"
        ////" <input  role='textbox'aria-haspopup='true' onblur='la(" + rows[rowIndex].cells[1].innerText + "," + rows[rowIndex].cells[6].innerText + ")' aria-autocomplete='list' value='0.00' name='warehouse' style='width: 80px;' type='text'>";
        //calculateListNum(rows[rowIndex].cells[1].innerText);
        //$(Pointer).parent().parent().next().find("input[type='text']").trigger('keydown');
    });
    $("#submitButton").live("click", function () {
        var Jaonstr = getTabledata();
        if (Jaonstr.length > 10) {
            $.ajax({
                url: "/WMS/Product/ProductDetail",
                type: "POST",
                data: {
                    "Jaonstr": Jaonstr,
                },
                success: function (data) {
                    if (data.Code == "1") {
                        self.location = document.referrer;
                        //history.back();
                        //window.location.href = "/WMS/Product/Index";
                    } else {
                        showMsg("提交失败！", 4000);
                    }
                    //if (data == "True") {
                    //    post('/WMS/ShelvesManagement/Index', { ShelvesModel: null, Action: '查询' });
                    //}
                },
                error: function () {
                    showMsg("操作失败！", 4000);
                }

            })

        } else {
            showMsg("操作失败！", 4000);
        }
    });
    $(".DelButton").live("click", function () {
        var i = $("#resultTable>tbody>tr").length;
        if (i > 1) {
            $(this).parent().parent().parent().remove();
        } else {
            showMsg("已是最后一行！", 4000)
        }
    });

    var box = {
        UPC: 'UPC',
        UPC名称: 'UPCName', UPC类型: 'UPCType', UPC数量: 'UPCNumber',
    };
    function getTabledata() {
        var txt = "[";
        var table = document.getElementById("resultTable");
        var row = table.getElementsByTagName("tr");
        var col = row[0].getElementsByTagName("th");
        for (var j = 1; j < row.length; j++) {
            var r = "{";
            for (var i = 1; i < col.length; i++) {
                //var tds = row[j].getElementsByTagName("td");
                //var aaa = $(row[j].getElementsByTagName("td")).children("input");
                if ($(row[j].getElementsByTagName("td")[1]).children("input")[0].value != "") {
                    r += "\"" + box[col[i].innerText.trim()] + "\"\:\"" + $(row[j].getElementsByTagName("td")[i]).children("input")[0].value + "\",";
                } else {
                    showMsg("UPC不能为空", 4000);
                    return "";
                }
            }
            r += "\"StorerID\"\:\"" + $("#productStorerInfo_StorerID")[0].value + "\",";
            r += "\"SKU\"\:\"" + $("#productStorerInfo_SKU")[0].value + "\",";
            r = r.substring(0, r.length - 1)
            r += "},";
            txt += r;
        }
        txt = txt.substring(0, txt.length - 1)
        txt += "]";
        return txt;
    };

})
function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    //var re = new RegExp(pattern);
    if (pattern.test(hehe.value)) {

        hehe.value = hehe.value.replace(pattern, "");
    }
}
