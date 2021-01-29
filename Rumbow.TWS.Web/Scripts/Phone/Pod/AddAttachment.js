window.onload = function () {
    try {
        //动态创建一个canvas元，并获取他2D context。如果出现异常则表示不支持
        document.createElement("canvas").getContext("2d");
        alert("浏览器支持HTML5 CANVAS");
    } catch (e) {
        alert("浏览器不支持HTML5 CANVAS");
    }

    window.addEventListener("DOMContentLoaded", function () {
        var canvas = document.getElementById("canvas"),
            content = canvascanvas.getContext("2d"),
            video = document.getElementByIdx("video"),
            videoObj = { "video": true },
            errBack = function (error) {
                console.log("Video capture error:", error.code);
            };

        if (navigator.getUserMedia) {
            navigator.getUserMedia(videoObj, function (stream) {
                video.src = stream;
            },
            errBack);
        } else if (navigator.webkitGetUserMedia) {
            navigator.webkitGetUserMedia(videoObj, function (stream) {
                video.src = window.webkitURL.createObjectURL(stream);
                video.play();
            },
            errBack);
        }

        $("#snap").click(function () {
            content.drawImage(video, 0, 0, 320, 320);
        });
    },
    false);

    var interval = setInterval(CatchCode, "3000");

    function CatchCode() {
        $("#snap").click();
        //实际运用可不写，测试代 ， 为单击拍照按钮就获取了当前图像，有其他用途
        var canvans = document.getElementById("canvas");
        //获取浏览器页面的画布对象
        //以下开始编 数据
        var imgData = canvans.toDataURL();
        //将图像转换为base64数据
        var base64Data = imgData.substr(22);
        //在前端截取22位之后的字符串作为图像数据
        //开始异步上
        $.post("UploadImgCode.ashx", { "img": base64Data }, function (data, status) {
            if (status == "success") {
                if (data == "OK") {
                    alert("二维 已经解析");
                }
                else {
                    // alert(data);
                }
            }
            else {
                alert("数据上 失败");
            }
        }, "text");
    }
}