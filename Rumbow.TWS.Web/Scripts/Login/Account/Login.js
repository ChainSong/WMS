//var loginClick = function () {
//    if ($('#projectWrapper').is(':visible')) {
//        $('#ProjectID').val($('#Projects').val());
//    }

//    var projectID = parseInt($('#ProjectID').val());
//    if (isNaN(projectID) || projectID < 1 ) {
//        $('#UserName').trigger('change');
//        return false;
//    }

//    return true;
//};

$(document).ready(function () {
    $('input[id="txtUser"]').focus();
    //alert($("#Is_Valid_NameOrPwd").val());
    //console.info($("#Is_Valid_NameOrPwd").val());
    //if ($("#Is_Valid_NameOrPwd").val() == "false") {
    //    showMsg("用户名或密码错误！", 4000);
    //}
    //$('#UserName').change(function () {
    //    $('#projectWrapper').hide();
    //    var projectList = $('#Projects');
    //    projectList.find('option').remove();

    //    $.get(
    //        '/Login/Account/GetUserProjects',
    //        { UserName: $('#UserName').val() },
    //        function (data) {
    //            var isArray = $.isArray(data);
    //            if (isArray) {
    //                var txt = '';
    //                for (var i = 0; i < data.length; i++) {
    //                    txt += '<option value="' + data[i].ProjectID + '">' + data[i].ProjectName + '</option>';
    //                }

    //                projectList.html(txt);
    //                if (data.length > 1){
    //                    $('#projectWrapper').show();
    //                } else {
    //                    $('#projectWrapper').hide();
    //                    $('#ProjectID').val(data[0].ProjectID);
    //                }   
    //            } else {
    //                $('#projectWrapper').hide();
    //                $('#ProjectID').val(-1);
    //                alert("该用户没有分配到项目中,不能登录");
    //            }
    //        });
    //});

    //单独一张背景图片js
    if ($("#ProjectName").val() == "Aden") {

        $("#aaa")[0].style.background = '#FFF url("/Assets/Img/aden.png") repeat-y center top ';//根据projectid=0时，显示此背景图片
    }

    $('#txtUser').change(function () {
        var user = document.getElementById("txtUser").value;
        if (user == "") {
            showMsg("请输入用户名！", 4000);
            return;
        }
        $.ajax({
            type: 'get',
            url: '/Login/Account/GetUserProjects',
            data: { userName: user },
            cache: false,
            dataType: 'json',
            success: function (data) {
                var isArray = $.isArray(data);
                if (!isArray) {
                    document.getElementById("projectId").value = "";
                    showMsg(data, 4000);
                }
                else {
                    document.getElementById("projectId").value = data[0].ProjectID;
                }
            },
            error: function () {
                document.getElementById("projectId").value = "";
                showMsg("请求出错！", 4000);
            }
        });
    });

});



function showMsg(msg, time) {
    document.getElementsByClassName("popMsg")[0].innerText = msg;
    document.getElementsByClassName("popMsg")[0].style.display = "block";
    setTimeout(hideMsg, time);
}

function hideMsg() {
    document.getElementsByClassName("popMsg")[0].style.display = "none";
}

var pid = "";

function login() {
    var user = document.getElementById("txtUser");
    var pwd = document.getElementById("txtPwd");
    if (user.value == "") {
        showMsg("请输入用户名！", 4000);
        return false;
    }
    if (pwd.value == "") {
        showMsg("请输入密码！", 4000);
        return false;
    }
    var projectID = parseInt($('#projectId').val());
    if (isNaN(projectID) || projectID < 1) {
        $('#txtUser').trigger('change');
        return false;
    }

    //var projectId = $("#projectId").val();
    //var pwd = $("#txtPwd").val();
    var is_login = true;
    $.ajax({
        type: 'post',
        async: false,
        url: '/Login/Account/IsLogin',
        data: { UserName: user.value, Password: pwd.value, ProjectID: projectID },
        cache: false,
        success: function (data) {
            if (data != "true") {
                showMsg("用户名或密码错误！", 4000);
                is_login= false;
            }
        },
        error: function () {
            showMsg("请求出错！", 4000);
        }
    });
    return is_login;
    //return true;
}

