var txt = "确认是否打印上架单";
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
    if ($("#resultTable tbody").length == 0) {
        $("#resultTable").removeAttr("style");
        $("#tables").removeAttr("style");
        $("#OperateTh").removeAttr("style");
    } else if ($("#resultTable tbody")[0].rows.length < 1) {
        $("#resultTable").removeAttr("style");
        $("#tables").removeAttr("style");
        $("#OperateTh").removeAttr("style");
    }

    $(function ($) {
        $('body').click(function () {
            ShowsOut()
        });
    });
    $("#printButton").live("click", function () {
        layer.confirm('<font size="4">确认是否批量打印上架单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var str = '';
            $('.checkForSelect').each(function (index) {
                if ($(this).attr('checked') === 'checked') {
                    str += "'" + $(this).attr('data-id') + "'" + ",";
                }
            })
            if ($("#ProjectName").val() == "NIKE") {
                location.href = '/WMS/ReceiptManagement/PrintShelvesNike?rid=' + str.substring(0, str.length - 1) + "&Flag=1";
            }
            else {
                location.href = '/WMS/ReceiptManagement/PrintShelvesNike?rid=' + str.substring(0, str.length - 1) + "&Flag=1";
            }
        });

        // }
        //$.ajax({
        //    type: 'GET',
        //    url: '/WMS/ReceiptManagement/PrintShelves',
        //    data: { id: "@ViewBag.Id" },
        //    cache: false,

        //    success: function (data) {
        //        var a = JSON.parse(data);
        //        var html = $('#Evaluation').render(a);
        //        $('#PrintArea')['empty']();
        //        $('#PrintArea').append(html);
        //    }, error: function (err) {
        //        alert("错误！");
        //    }
        //});
        //$.ajax({
        //    type: "GET",
        //    url: "/WMS/ReceiptManagement/PrintShelves",
        //    data: { rid: str },
        //    cache: false,
        //    success: function (data)
        //    {
        //        //if (data != "")
        //        //{
        //        //    location.href = '/WMS/ReceiptManagement/PrintShelves'
        //        //}
        //    }, error: function (msg)
        //    {
        //        alert("");
        //    }
        //});
    });




    $('#searchButton').click(function () {
        setPageControlVal();
    });

    $('#addButton').click(function () {
        setPageControlVal();
    });
    $('.DynamicCalendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = 'SearchCondition_';
        if (pref === 'start') {
            descID += 'Start' + actualID;
        }

        else {
            descID += 'End' + actualID;
        }
        $(this).val($('#' + descID).val());


    });
    var setHiddenValToControl = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id;
            if ($(this).attr("type") === "checkbox") {
                if ($('#' + descId).val() === '1') {
                    $(this).attr('checked', 'checked');
                } else {
                    $(this).removeAttr('checked');
                }

            } else if ($(this).attr("type") === "DropDownList") {
                var desc = $('#' + descId);
                if (desc.val() === '1' || desc.val() === 'Y' || desc.val === 'y' || desc.val() === '是') {
                    $(this).val('1');
                } else {
                    $(this).val('0');
                }

            } else {
                $(this).val($('#' + descId).val());
            }
        });
    }

    $('.DropDownList').each(function (index) {
        var id = $(this).attr("id");
        var descId = "SearchCondition_" + id.substr(0, id.length - 4) + "ID";
        $(this).val($('#' + descId).val());
    });

    var setPageControlVal = function () {
        $('.notKeyVal').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id;
            if ($(this).attr("type") === "checkbox") {
                var isChecked = document.getElementById(id).checked;
                if (isChecked) {
                    $('#' + descId).val("1");
                } else {
                    $('#' + descId).val("0");
                }
            } else {
                $('#' + descId).val($(this).val());
            }
        });

        $('.DropDownList').each(function (index) {
            var id = $(this).attr("id");
            var descId = "SearchCondition_" + id.substr(0, id.length - 4) + "ID";
            $('#' + descId).val($(this).val());
        });

        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];

            var descID = 'SearchCondition_';
            if (pref === 'start') {
                descID += 'Start' + actualID;
            }
            else {
                descID += 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });
    }

    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            checkBoxs.attr("checked", "checked");
        } else {
            checkBoxs.removeAttr("checked");
        }

    });
    $("#resultTable tbody input[type='checkbox']").live('click', function () {
        RefreshIDs();
    });
    setHiddenValToControl();


    $("#statusBackOK").live('click', function () {
        if ($('#backStatusid').val() == "") {
            //alert("请选择要回退的状态");
            showMsg("请选择要回退的状态", "4000");
            return;
        }

        $.ajax({

            url: "/WMS/ReceiptManagement/BackStatus",
            type: "POST",
            dataType: "text",
            data: {
                ID: $('#StatusbackID').val().toString(),
                ToStatus: $('#backStatusid').val().toString(),
            },

            //async: "false",
            success: function (data) {
                if (data == "") {
                    showMsg("回退成功！", "10000");
                    location.href = "/WMS/ReceiptManagement/Index"
                    //$('#BackReturn')[0].innerText = "回退成功！"
                    //openPopup("pop33", true, 350, 200, null, 'BackStatusReturnDiv', true)
                }
                else {
                    showMsg("回退失败！失败单号：" + data, "4000");
                }
            },
            error: function (msg) {
                alert(msg.val);
            }

        });
    });



});



function TableToJson() {
    var a = "";

    var checkBoxs = $("#resultTable tbody input[type='checkbox']");

    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            a += checkBoxs[i].name + ",";
        }
    }

    a = a.substring(0, a.length - 1);


    return a;
}

//if ($(this).attr("checked") == "checked" && $(this).id.toString() != "selectAll") {
//    a = document.getElementsByName($(this).name);
//}

//var table = document.getElementById('resultTable');
//var tr = table.getElementsByTagName('tr');
//for (var i = 1; i < tr.length; i++) {

//    var chks=document.getElementsByTagName('input');

//    for (var i = 0; i < chks.length; i++) {
//    }

//    var id = tr[i].id;



//}





function ReceiptDelete(ReceiptNumber) {

    if (window.confirm('您确认取消此入库单？')) {
        if (ReceiptNumber != "") {

            $.ajax({
                type: "POST",
                url: "/WMS/ReceiptManagement/ReceiptDelete",
                data: {
                    "ReceiptNumber": ReceiptNumber,
                },
                async: "false",
                success: function (data) {

                    var js = data;
                    location.href = "/WMS/ReceiptManagement/Index";
                    //window.location.reload();
                    if (js == "StatusWarning") {
                        alert("当前状态不允许取消，请退回到1状态！");
                    }
                },
                error: function (msg) {
                    alert(msg.val);
                }

            });
        }
    }

}

var fileImportClick = function () {
    if ($('#importExcel').val() === '') {
        //Runbow.TWS.Alert("请选择要导入的Excel");
        showMsg("请选择要导入的Excel", "4000");
        return false;
    }

    var exp = /.xls$|.xlsx$/;
    var fileImport = $('#fileImport').clone();
    if (exp.exec($('#importExcel').val()) == null) {
        //Runbow.TWS.Alert("请选择Excel格式的文件");
        showMsg("请选择Excel格式的文件", "4000");
        //$('#importExcel').remove();
        $(this).before(fileImport);
        return false;
    };

    WebPortal.MessageMask.Show("导入中...");

    $.ajaxFileUpload({
        url: '/WMS/ReceiptManagement/ImportReceiptUpdate_Batch',
        secureuri: false,
        fileElementId: 'importExcel',
        dataType: 'json',
        data: {},
        success: function (data, status) {
            WebPortal.MessageMask.Close();
            $('#outPutResult').html("<h4><font color='#33cc70'>导入成功！<br/>" + data.result + "</font></h4>");
        },
        error: function (data, status, e) {
            $('#outPutResult').html("<h4><font color='#FF0000'>导入失败！<br/>" + data.result + "</font></h4>");
            WebPortal.MessageMask.Close();
        }
    });
    return false;
};
//$('select[id=SearchCondition_CustomerID]').live('change', function () {
//    window.location.href = "/WMS/ReceiptManagement/Index/?customerID=" + $(this).val();
//});


//function editButton(ID, CustomerID) {
//    location.href = "/WMS/ReceiptManagement/ReceiptCreate/?ID=" + ID + "&ViewType=2&PageType=2&CustomerID=" + CustomerID
//}

//function Shelves(ID) {
//    location.href = "/WMS/ShelvesManagement/ReceiptReceivingDetail/?RID=" + ID
//}

function statusBackClick(ID, Status) {
    $("#StatusbackID").val(ID);

    $('#backStatusid').children("span").children().unwrap();

    openPopup('popp', true, 350, 300, null, 'statusBackDiv');
    $("#popupLayer_popp")[0].style.top = "200px";
    for (var i = 0; i < $('#backStatusid').children().length; i++) {
        var a = $('#backStatusid').children()[i].value;
        if (a >= Status) {
            $('#backStatusid').children('option[value=' + a + ']').wrap('<span>').hide();
        }

    }

    //$('#backStatusid').children('option[value="-1"]').wrap('<span>').hide();
    //$('#backStatusid').find("option:selected").unwrap();    
}


$("#statusBackReturn").live('click', function () {
    closePopup();
});



$("#statusBack").live('click', function () {

    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    var sql = "";
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            sql += checkBoxs[i].name.toString() + ",";

        }
    }
    if (sql.length > 0) {

        $("#statusBack").popover('destroy');
        $("#StatusbackID").val(sql.toString().substring(0, sql.toString().length - 1));
        openPopup("pop11", true, 350, 300, null, 'statusBackDiv', true);
        $("#popupLayer_pop11")[0].style.top = "200px";
    }
    else {
        //Runbow.TWS.Alert("请勾选入库单！");
        //$("#statusBack").popover('show');
        showMsg("请勾选入库单！", 4000);

    }

});

var RefreshIDs = function () {
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    var length = checkBoxs.length;
    //var IDs = [];
    var checked = 0;
    checkBoxs.each(function () {
        if ($(this).attr("checked") === "checked") {
            //var id = { ID: $(this).attr("data-ID") };
            //IDs.push(id);
            checked++;
        }
    });

    if (checked == checkBoxs.length) {
        $('#selectAll').attr("checked", "checked");
    } else {
        $('#selectAll').removeAttr("checked");
    }

    //$('#SelectedIDs').val(JSON.stringify(IDs));
}


function printPick(ID) {
    layer.confirm('<font size="4">确认是否打印上架单？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        if ($("#ProjectName").val() == "NIKE") {
            location.href = '/WMS/ReceiptManagement/PrintShelvesNike?rid=' + ID + "&Flag=1";
        }
        else {
            location.href = '/WMS/ReceiptManagement/PrintShelvesNike?rid=' + ID + "&Flag=1";
        }
    });
}


function ShowsIn(ID, obj) {
    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
        $(".ddiv:not(obj)").animate({
            width: "hide",
            width: "400%",
            paddingRight: "hide",
            paddingLeft: "hide",
            marginRight: "hide",
            marginLeft: "hide"

        }, 100);
        $("#operateTD" + ID).animate({
            width: "show",
            width: "445%",
            paddingRight: "show",
            paddingLeft: "show",
            marginRight: "show",
            marginLeft: "show"
        });
    }
    //$("#operateTD" + ID)[0].style.display = "";
}

function ShowsOut() {
    //$("#operateTD" + ID).fadeOut("slow");

    $(".ddiv").animate({
        width: "hide",
        width: "400%",
        paddingRight: "hide",
        paddingLeft: "hide",
        marginRight: "hide",
        marginLeft: "hide"

    }, 100);
    //$("#operateTD" + ID)[0].style.display = "";
}

