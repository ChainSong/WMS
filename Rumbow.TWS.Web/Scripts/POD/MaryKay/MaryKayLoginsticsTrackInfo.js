
var Ids = '';
$(document).ready(function () {
   
    if ($("#MkOrderNo").val().length > 0 || $("#ExpressOrderNo").val().length > 0 ||  $("#TrackInfoTypeID  option:selected").val
        ().length > 0 || $("#BeginTrackDateTime").val().length > 0 || $("#EndTrackDateTime").val().length > 0)
    {
       
        $("#ExprotButton").removeAttr("disabled");
    }

    $('#ExprotButton').click(function () {
        $('#IsExport').val('True');
        layer.msg('正在导出', { icon: 16 });
    });

    $("#start_TrackDateTime").change(function () {
        if ($("#start_TrackDateTime").val().length > 0)
            $("#ExprotButton").removeAttr("disabled");
        else
            $("#ExprotButton").attr("disabled", "disabled");
    });

    $("#end_TrackDateTime").change(function () {
        if ($("#end_TrackDateTime").val().length > 0)
            $("#ExprotButton").removeAttr("disabled");
        else
            $("#ExprotButton").attr("disabled", "disabled");
    });

    $("#ExpressOrderNo").blur(function () {
        if ($("#ExpressOrderNo").val().length > 0)
            $("#ExprotButton").removeAttr("disabled");
        else
            $("#ExprotButton").attr("disabled", "disabled");
    });

    $("#MkOrderNo").blur(function () {
        if ($("#MkOrderNo").val().length > 0)
            $("#ExprotButton").removeAttr("disabled");
        else
            $("#ExprotButton").attr("disabled", "disabled");
    });

    $("#TrackInfoTypeID").live('change', function () {
        if ($("#TrackInfoTypeID option:selected").val().length > 0)
            $("#ExprotButton").removeAttr("disabled");
        else
            $("#ExprotButton").attr("disabled", "disabled");
    });

    //全选
    $("#chdAll").click(function () {
        if ($("#chdAll").is(':checked')) {
            $("[type='checkbox']").prop({ checked: true })
        } else {
            $("[type='checkbox']").removeAttr("checked");
            Ids = '';
        }
    });

    //批量删除
    $("#btnDelete").click(function () {
        var paramObj;
        $("[type='checkbox']").each(function () {
            if ($(this).is(':checked')) {    //判断哪些选中拼接roleid 
                if ($(this).attr('data-id') != undefined) {
                    paramObj = $(this).attr('data-id');
                    Ids += ',' + '"' + paramObj + '"';
                }
            }
        });

        //将双引号变成单引号
        Ids = Ids.replace(/"([^"]*)"/g, "'$1'");

        if (Ids.length == 0) //如果没有选中提示信息
        {
            layer.tips('您未选择要删除的订单！', '#btnDelete');
        } else {
            //删除前询问 避免导致误删
            DeleteSingle(Ids, true);
        }

    });


    $('.calendarRange').each(function (index) {
        var id = $(this).attr('id');
        var pref = id.split('_')[0];
        var actualID = id.split('_')[1];
        var descID = '';
        var Begin = 'Begin';
        var End = 'End';
        if (pref === 'start') {
            descID = Begin + actualID;
        }
        else {
            descID = End + actualID;
        }
        $(this).val($('#' + descID).val());
    });


    var setValToHiddenControl = function () {
        $('.calendarRange').each(function (index) {
            var id = $(this).attr('id');
            var pref = id.split('_')[0];
            var actualID = id.split('_')[1];
            var descID;
            if (pref === 'start') {
                descID = "Begin" + actualID;
            }
            else {
                descID = 'End' + actualID;
            }
            $('#' + descID).val($(this).val());
        });


    };

    $('#QueryButton').click(function () {
        setValToHiddenControl();
    });

    $('#QueryButton').click(function () {
        $('#PageIndex').val('0');
        $('#IsExport').val('False');
    });
});



var EmptyValue = function (obj) {
    var IDName = $(obj).attr('id');

    var val = IDName.split('_')[0];
    var Begin = 'start_' + val;
    var End = 'end_' + val;
    $('#' + Begin).val("");
    $('#' + End).val("");
};

//单个删除
function DeleteSingle(Ids, IsP) {
    if (!IsP) {
        Ids = '"' + Ids + '"';
        Ids = Ids.replace(/"([^"]*)"/g, "'$1'");
    }
    //删除前询问 避免导致误删
    layer.confirm('您确认要删除吗？', {
        btn: ['确认', '取消'] //按钮
    }, function () {
        $.ajax({
            type: 'POST',
            url: "/POD/MaryKay/DeleteOrderNo",
            data: {
                Ids: Ids
            },
            success: function (data) {
                window.location.href = '/POD/MaryKay/MaryKayLoginsticsTrackInfo';
            },
            error: function () {

            }

        });
    }, function () {
        Ids = '';
    });
}

//查看物流详情
function Detail(objId) {
    layer.open({
        type: 2,
        title: '物流跟踪信息',
        shadeClose: true,
        shade: false,
        maxmin: true, //开启最大化最小化按钮
        area: ['800px', '500px'],
        content: '/POD/MaryKay/MaryKayLoginsticsTrackDetail?Id=' + objId
    });
    //window.location.href = '/POD/MaryKay/MaryKayLoginsticsTrackDetail?Id=' + objId;
}

//导入物流跟踪信息
function fileImportClick() {
    layer.open({
        type: 2,
        title: '物流跟踪信息覆盖更新',
        shadeClose: true,
        shade: false,
        scrolling: 'no',
        maxmin: true, //开启最大化最小化按钮
        area: ['800px', '500px'],
        content: '/POD/MaryKay/MaryKayLogisticImport'
    });
}
