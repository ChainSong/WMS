$(document).ready(function () {
    $('#selectAll').click(function () {
        var checkBoxs = $("#resultTable tbody input[type='checkbox']");

        if ($(this).attr("checked") === "checked") {
            //$(this.dataset.status!==-1){ 
            checkBoxs.each(function (a, h) {
                //$(h)[0].attr("checked", "checked");
                if ($(h)[0].dataset.status != -1) {
                    $(h)[0].checked = true;
                }
            })
            //checkBoxs.attr("checked", "checked"); 
            //}
        } else {
            checkBoxs.removeAttr("checked");
        }
    });
    $('#PrintButton').live('click', function () {
        layer.confirm('<font size="4">确认是否批量打印加工单？</font>', {
            btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
            //shade: [0.8, '#393D49'],
            title: ['提示', 'font-size:18px;']
            //按钮
        }, function (index) {
            layer.close(index);
            var id = TableToJson();
            if (id == "") {
                showMsg("请选择需要打印的加工单！", 4000);
                return false;
            }
            else {
                window.location.href = '/WMS/MachiningManagement/PrintMachining?id=' + id;
            }
        });

    });
    $(function ($) {
        $('body').click(function () {
            ShowsOut()
        });
    });
    $('#addButton').live('click', function () {
        window.location.href = "/WMS/MachiningManagement/BucketMachiningAddSave/?Flag=1"+"&ShowSubmit=" + $("#ShowSubmit").val();;
    });
  
});

function ShowsIn(ID, obj) {
    if ($("#operateTD" + ID)[0].style.display != "" && $("#operateTD" + ID)[0].style.display != "block") {
        $(".ddiv:not(obj)").animate({
            width: "hide",
            width: "330%",
            paddingRight: "hide",
            paddingLeft: "hide",
            marginRight: "hide",
            marginLeft: "hide"

        }, 100);
        $("#operateTD" + ID).animate({
            width: "show",
            width: "368%",
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
        width: "330%",
        paddingRight: "hide",
        paddingLeft: "hide",
        marginRight: "hide",
        marginLeft: "hide"

    }, 100);
    //$("#operateTD" + ID)[0].style.display = "";
}

function Edit(ID, CustomerID, CustomerName) {
    window.location.href = "/WMS/MachiningManagement/BucketMachiningAddSave/?ID=" + ID + "&Flag=3" + "&CustomerID=" + CustomerID + "&CustomerName=" + CustomerName + "&ShowSubmit=" + $("#ShowSubmit").val();
}

function deleteMachining(ID) {
    layer.confirm('<font size="4">确认是否删除？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        $.ajax({
            url: "/WMS/MachiningManagement/MachiningDelete",
            type: "GET",
            data: {
                ID: ID
            },
            success: function (data) {
                if (data == "") {
                    showMsg("删除成功！", "4000");
                    window.location.href = "/WMS/MachiningManagement/BucketMachiningIndex/";
                }

            },
            error: function (data, status, e) {
                showMsg("删除失败！" + e, "4000");

            }
        });
    });
}
function TableToJson() {
    var a = "";
    var checkBoxs = $("#resultTable tbody input[type='checkbox']");
    for (var i = 0; i < checkBoxs.length; i++) {
        if (checkBoxs[i].checked) {
            a += checkBoxs[i].dataset.id + ",";
        }
    }
    a = a.substring(0, a.length - 1);
    return a;
}
function Print(id) {
    layer.confirm('<font size="4">确认是否打印加工单？</font>', {
        btn: ['确定', '放弃'], icon: 3, area: ['400px', '200px'], shift: 0, closeBtn: 1,
        //shade: [0.8, '#393D49'],
        title: ['提示', 'font-size:18px;']
        //按钮
    }, function (index) {
        layer.close(index);
        window.location.href = '/WMS/MachiningManagement/PrintMachining?id=' + id;
    });

}