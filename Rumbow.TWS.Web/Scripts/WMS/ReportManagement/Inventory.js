$(document).ready(function () {
    if ($('#SearchCondition_CustomerID').val() == "") {
        $('#SearchCondition_CustomerID option:first').next().attr("selected", "selected");

    } else {

    }
    $('select[id=SearchCondition_Warehouse]').live('change', function () {
        if ($(this).val()!='') {
            var selec = $(this).val(); //获取改变的选项值

            $.ajax({
                type: "POST",
                url: "/WMS/ReportManagement/ChangeWarehouse",
                data: {
                    "str": selec,
                  
                },
                async: "false",
                success: function (data) {

                    var js = JSON.parse(data);
                    document.all['SearchCondition_Area'].length = 0;
                    for (var i = 0; i < js.length; i++) {
                        document.all['SearchCondition_Area'].options.add(new Option(js[i]["AreaName"], js[i]["AreaName"]));
                    }
                },
                error: function (msg) {
                    alert(msg.val);
                }
            });

        }
        else {
            document.all['SearchCondition_Area'].length = 0;
            document.all['SearchCondition_Area'].options.add(new Option("==请选择==", "==请选择=="));
        }
    });

    $('#searchButton').click(function () {
        setPageControlVal();
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
            /*debugger*/;
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


});