$(document).ready(function () {

    var message = $('#Message').val();
    var prevPodID = $('#PrevPodID').val();
    var nextPodID = $('#NextPodID').val();
    var currentFolder = $('#CurrentFolder').val();
    var currentImageExtension = $('#CurrentImageExtension').val().toLowerCase();
    var currentImageName = $('#CurrentImageName').val();
    var images = $('#ImageNames').val().split('|');

    $('#ImageControl').hide();
    $('#prevImageButton').hide();
    $('#nextImageButton').hide();

    var refreshImageButton = function () {
        $('#nextImageButton').show();
        $('#prevImageButton').show();

        if (currentImageName === '' || images === '') {
            $('#nextImageButton').hide();
            $('#prevImageButton').hide();
        }
        for (var i = 0; i < images.length; i++) {
            if (images[i] === currentImageName) {
                if (i === 0) {
                    $('#prevImageButton').hide();
                }

                if (i === images.length - 1) {
                    $('#nextImageButton').hide();
                }
            }
        }
    };

    var refreshImageDiv = function () {
        $('#leftDiv').show();
        $('#rightDiv').show();

        if (currentImageName === '' || images === '') {
            $('#leftDiv').hide();
            $('#rightDiv').hide();
        }
        for (var i = 0; i < images.length; i++) {
            if (images[i] === currentImageName) {
                if (i === 0) {
                    $('#leftDiv').hide();
                }

                if (i === images.length - 1) {
                    $('#rightDiv').hide();
                }
            }
        }
    };

    
    var showDefaultImageOrDownLoad = function () {
        if (currentImageName === '') {
            $('#ImageControl').attr('src', $('#URLPrev').val() + "NotImageFile.jpg");
            $('#ImageControl').show();
            $('#waitingImg').hide();
            return;
        }

        if (currentImageExtension !== '.jpg' && currentImageExtension !== '.bmp' && currentImageExtension !== '.png' && currentImageExtension !== '.icon' && currentImageExtension !== '.gif') {
            $('#ImageControl').attr('src', $('#URLPrev').val() + "NotImageFile.jpg");
            //window.open($('#URLPrev').val() + $('#CurrentFolder').val() + '/' + $('#CurrentImageName').val());
        }
        else {
            $('#ImageControl').attr('src', $('#URLPrev').val() + $('#CurrentFolder').val() + '/' + $('#CurrentImageName').val());
        }

        $('#ImageControl').show();
        $('#waitingImg').hide();

        refreshImageButton();
        refreshImageDiv();
    };

    if (prevPodID === '0') {
        $('#backButton').attr('disabled', 'disabled');
    }

    if (nextPodID === '0') {
        $('#nextButton').attr('disabled', 'disabled');
    }
    

    $('#backButton').click(function () {
        window.location.href = '/POD/POD/ImageAuditDetail/?id=' + prevPodID;
    });

    $('#nextButton').click(function () {
        window.location.href = '/POD/POD/ImageAuditDetail/?id=' + nextPodID;
    });

    $('#IsOK').change(function () {
        var isOK = $('#IsOK').val();
        if (isOK === 'true') {
            $('#Remark').val('');
        }
    });

    $('#AutidButton').click(function () {
        var isOK = $('#IsOK').val();
        var remark = $('#Remark').val();

        if (isOK === 'false' && remark === '') {
            Runbow.TWS.Alert('审核不合格请输入不合格原因');
            return false;
        }

        return true;

    });


    $('#prevImageButton,#leftDiv').click(function () {
        for (var i = 1; i < images.length; i++) {
            if (images[i] === currentImageName) {
                $('#CurrentImageName').val(images[i - 1]);
                currentImageName = images[i - 1];
                var tempArray = currentImageName.split('.');
                $('#CurrentImageExtension').val('.' + tempArray[tempArray.length - 1].toLowerCase());
                currentImageExtension = $('#CurrentImageExtension').val();
                break;
            }
        }
        showDefaultImageOrDownLoad();
        
    });

    $('#nextImageButton,#rightDiv').click(function () {
        for (var i = 0; i < images.length - 1; i++) {
            if (images[i] === currentImageName) {
                $('#CurrentImageName').val(images[i + 1]);
                currentImageName = images[i + 1];
                var tempArray = currentImageName.split('.');
                $('#CurrentImageExtension').val('.' + tempArray[tempArray.length - 1].toLowerCase());
                currentImageExtension = $('#CurrentImageExtension').val();
                break;
            }
        }
        showDefaultImageOrDownLoad();
       
    });

    

    setTimeout(showDefaultImageOrDownLoad, 1000);

    if (message !== '') {
        Runbow.TWS.Alert(message);
    }
});