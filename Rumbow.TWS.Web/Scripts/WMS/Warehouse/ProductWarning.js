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
    $("#addButton").live('click', function () {
        location.href = "/WMS/Warehouse/ProductWarningCreate/?ViewType=1" 
    });

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }
    });
    $('#DelButton').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");
        var sql = "";
        for (var i = 0; i < checkBoxs.length; i++) {
            if (checkBoxs[i].checked) {
                sql += checkBoxs[i].name.toString() + ",";

            }
        }
        if (sql.length <= 0) {
            showMsg("请勾选SKU！", 4000);
            return;
        }
        else {
            layer.confirm('<font size="4">确认是否删除？</font>', {
                btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
                //shade: [0.8, '#393D49'],
                title: ['提示', 'font-size:18px;']
                //按钮
            }, function (index) {
                layer.close(index);
                $.ajax({
                    type: "POST",
                    url: "/WMS/Warehouse/ProductWarningDelete",
                    data: {
                        "IDS": sql.substring(0, sql.length - 1)

                    },
                    async: "false",
                    success: function (data) {
                        if (data == "OK") {
                            showMsg("删除成功！", "4000");

                            location.href = "/WMS/Warehouse/ProductWarning/?CustomerID=" + $('#SearchCondition_CustomerID').val() + "&WarehouseID=" + $('#SearchCondition_WarehouseID').val();
                        }
                    },
                    error: function (msg) {
                        showMsg("添加失败", "4000");
                    }
                });
            });
        }
    });
});

$("#resultTable td").live('dblclick', function () {
    if (this.className == "MinNumberEdit")
    {
        var a = this.innerText;
        this.innerHTML = "<input type='text' style='width:180px'  class='form-control MinNumberUpdate' onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)' value=" + a + " ></input>"
    }
    if (this.className == "MaxNumberEdit") {
        var a = this.innerText;
        this.innerHTML = "<input type='text' style='width:180px'  class='form-control MaxNumberUpdate' onpropertychange='replaceNotNumber(this)' oninput='replaceNotNumber(this)' value=" + a + " ></input>"
    }
});
$('.MinNumberUpdate').live('blur', function () {
    var ID = $($(this).parent().parent()).children(".SKUID")[0].innerText;
    var MinNumber = this.value;
    var obj = this;
    $.ajax({
        type: "POST",
        url: "/WMS/Warehouse/ProductWarningEdit",
        data: {
            "ID": ID,
            "MinNumber": MinNumber,
            "MaxNumber": "0"


        },
        async: "false",
        success: function (data) {
            if (data == "OK") {
                obj.parentNode.innerHTML = MinNumber;
            }
        },
        error: function (msg) {
            showMsg("修改失败", "4000");
        }
    });


});
$('.MaxNumberUpdate').live('blur', function () {
    var ID = $($(this).parent().parent()).children(".SKUID")[0].innerText;
    var MaxNumber = this.value;
    var obj = this;
    $.ajax({
        type: "POST",
        url: "/WMS/Warehouse/ProductWarningEdit",
        data: {
            "ID": ID,
            "MinNumber": "0",
            "MaxNumber": MaxNumber


        },
        async: "false",
        success: function (data) {
            if (data == "OK") {
                obj.parentNode.innerHTML = MaxNumber;
            }
        },
        error: function (msg) {
            showMsg("修改失败", "4000");
        }
    });


});

function ProductWarningDelete(ID, CustomerID,WarehouseID) {
    layer.confirm('<font size="4">确认是否删除？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift:0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        $.ajax({
            type: "POST",
            url: "/WMS/Warehouse/ProductWarningDelete",
            data: {
                "IDS": ID
                
            },
            async: "false",
            success: function (data) {
                if (data == "OK") {
                    showMsg("删除成功！", "4000");
                  
                    location.href = "/WMS/Warehouse/ProductWarning/?CustomerID=" + CustomerID + "&WarehouseID=" + WarehouseID;
                }
            },
            error: function (msg) {
                showMsg("添加失败", "4000");
            }
        });
    });
}


function replaceNotNumber(hehe) {
    var pattern = /[^\d.]/g;
    //var re = new RegExp(pattern);
    if (pattern.test(hehe.value)) {

        hehe.value = hehe.value.replace(pattern, "");
    }
}
























//function ShowsIn(ID, obj) {
//    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
//        $(".ddiv:not(obj)").animate({
//            width: "hide",
//            width: "230%",
//            paddingRight: "hide",
//            paddingLeft: "hide",
//            marginRight: "hide",
//            marginLeft: "hide"

//        }, 100);
//        $("#operateTD" + ID).animate({
//            width: "show",
//            width: "278%",
//            paddingRight: "show",
//            paddingLeft: "show",
//            marginRight: "show",
//            marginLeft: "show"
//        });
//    }
//    //$("#operateTD" + ID)[0].style.display = "";
//}

//function ShowsOut() {
//    //$("#operateTD" + ID).fadeOut("slow");

//    $(".ddiv").animate({
//        width: "hide",
//        width: "230%",
//        paddingRight: "hide",
//        paddingLeft: "hide",
//        marginRight: "hide",
//        marginLeft: "hide"

//    }, 100);
//    //$("#operateTD" + ID)[0].style.display = "";
//}
