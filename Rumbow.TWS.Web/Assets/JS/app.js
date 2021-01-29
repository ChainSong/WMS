
//BACKGROUND CHANGER

$(function () {
    $("#button-bg").click(function () {
        $("body").css({
            "background": "url('assets/img/bg5.jpg')  no-repeat center   fixed",
            "background-size": "cover"
        });
    });
    $("#button-bg2").click(function () {
        $("body").css({
            "background": "url('assets/img/bg2.jpg') no-repeat center   fixed ",//no-repeat center center fixed
            "background-size": "cover"
        });
    });


    $("#button-bg3").click(function () {
        $("body").css({
            "background": "url('assets/img/bg.jpg') no-repeat center   fixed ",//no-repeat center center fixed
            "background-size": "cover"
        });


    });

    $("#button-bg5").click(function () {
        $("body").css({
            "background": "url('assets/img/giftly.png') no-repeat center   fixed ",
            "background-size": "cover"
        });

    });

    $("#button-bg6").click(function () {
        $("body").css({
            "background": "#2c3e50"
        });

    });

    $("#button-bg7").click(function () {
        $("body").css({
            "background": "url('assets/img/bg3.png') no-repeat center   fixed",
            "background-size": "cover"
        });

    });
    $("#button-bg8").click(function () {
        $("body").css({
            "background": "url('assets/img/bg8.jpg') no-repeat center   fixed",//no-repeat center center fixed
            "background-size": "cover"
        });
    });
    $("#button-bg9").click(function () {
        $("body").css({
            "background": "url('assets/img/bg9.jpg') no-repeat center   fixed",//no-repeat center center fixed
            "background-size": "cover"
        });
    });

    $("#button-bg10").click(function () {
        $("body").css({
            "background": "url('assets/img/bg10.jpg') no-repeat center   fixed",//no-repeat center center fixed
            "background-size": "cover"
        });
    });
    $("#button-bg11").click(function () {
        $("body").css({
            "background": "url('assets/img/bg11.jpg') no-repeat center   fixed",//no-repeat center center fixed
            "background-size": "cover"
        });
    });
    $("#button-bg12").click(function () {
        $("body").css({
            "background": "url('assets/img/bg12.jpg') no-repeat center   fixed", //no-repeat center center fixed
            "background-size": "cover"
        });
    });

    $("#button-bg13").click(function () {
        $("body").css({
            "background": "url('assets/img/bg13.jpg') no-repeat center   fixed",
            "background-size": "cover"
        });

    });
    /**
     * Background Changer end
     */
});

//TOGGLE CLOSE
$('.nav-toggle').click(function () {
    //get collapse content selector
    var collapse_content_selector = $(this).attr('href');

    //make the collapse content to be shown or hide
    var toggle_switch = $(this);
    $(collapse_content_selector).slideToggle(function () {
        if ($(this).css('display') == 'block') {
            //change the button label to be 'Show'
            toggle_switch.html('<span class="entypo-minus-squared"></span>');
        } else {
            //change the button label to be 'Hide'
            toggle_switch.html('<span class="entypo-plus-squared"></span>');
        }
    });
});


$('.nav-toggle-alt').click(function () {
    //get collapse content selector
    var collapse_content_selector = $(this).attr('href');

    //make the collapse content to be shown or hide
    var toggle_switch = $(this);
    $(collapse_content_selector).slideToggle(function () {
        if ($(this).css('display') == 'block') {
            //change the button label to be 'Show'
            toggle_switch.html('<span class="entypo-up-open"></span>');
        } else {
            //change the button label to be 'Hide'
            toggle_switch.html('<span class="entypo-down-open"></span>');
        }
    });
    return false;
});
//CLOSE ELEMENT
$(".gone").click(function () {
    var collapse_content_close = $(this).attr('href');
    $(collapse_content_close).hide();



});

//tooltip
$('.tooltitle').tooltip(); 