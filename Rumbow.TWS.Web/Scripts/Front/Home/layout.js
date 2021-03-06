function toggleMenu() {
    var menu = document.getElementById("sidebar");
    var content = document.getElementById("content");
    if (menu.style.display == "block") {
        menu.style.display = "none";
        menu.style.width = 0;
        content.style.marginLeft = 0;
    }
    else {
        menu.style.display = "block";
        menu.style.width = 240 + "px";
        content.style.marginLeft = 240 + "px";

    }
}

function isIE() { //ie?
    if (!!window.ActiveXObject || "ActiveXObject" in window)
        return true;
    else
        return false;
}

function resetSize() {
    $('#body').height(document.body.offsetHeight);
    $('#sidebar').height(document.body.offsetHeight);
}

window.onload = window.onresize = resetSize;

$(document).ready(function () {
    $('#myIframe').height(document.body.offsetHeight);
    $('#sidebar > ul > li.submenu > ul > li.submenu > ul > li > a').click(function(e)
    {
        $(this).parent().siblings().removeClass("active");
        $(this).parent().parent().parent().siblings().find("ul > li").removeClass("active");
        $(this).parent().addClass("active");
    });
	
	/* === Sidebar navigation === */
	
	$('#sidebar > ul > li.submenu > a').click(function(e)
	{
		e.preventDefault();
		var submenu = $(this).siblings('ul');
		var li = $(this).parent();
		var submenus = $('#sidebar > ul > li.submenu > ul');
		var submenus_parents = $('#sidebar > ul > li.submenu');
		if(li.hasClass('open'))
		{
			if(($(window).width() > 768) || ($(window).width() < 479)) {
				submenu.slideUp();
			} else {
				submenu.fadeOut();
			}
			li.removeClass('open');
		} else 
		{
			if(($(window).width() > 768) || ($(window).width() < 479)) {
				submenus.slideUp();			
				submenu.slideDown();
			} else {
				submenus.fadeOut();			
				submenu.fadeIn();
			}
			submenus_parents.removeClass('open');		
			li.addClass('open');	
		}
	});

	$('#sidebar > ul > li.submenu > ul > li.submenu > a').click(function(e)
	{
	    e.preventDefault();
	    var submenu = $(this).siblings('ul');
	    submenu.toggle();
	});
	
	var ul = $('#sidebar > ul');
	
    /*
	$('#sidebar > a').click(function(e)
	{
		e.preventDefault();
		var sidebar = $('#sidebar');
		if(sidebar.hasClass('open'))
		{
			sidebar.removeClass('open');
			ul.slideUp(250);
		} else 
		{
			sidebar.addClass('open');
			ul.slideDown(250);
		}
	});
	*/
	/* === Resize window related === */
	$(window).resize(function()
	{
		if($(window).width() > 479)
		{
			ul.css({'display':'block'});	
			$('#content-header .btn-group').css({width:'auto'});		
		}
		if($(window).width() < 479)
		{
			ul.css({'display':'none'});
			fix_position();
		}
		if($(window).width() > 768)
		{
			$('#user-nav > ul').css({width:'auto',margin:'0'});
            $('#content-header .btn-group').css({width:'auto'});
		}
	});
	
	if($(window).width() < 468)
	{
		ul.css({'display':'none'});
		fix_position();
	}
	
	if($(window).width() > 479)
	{
	   $('#content-header .btn-group').css({width:'auto'});
		ul.css({'display':'block'});
	}
	
	/* === Tooltips === */
	$('.tip').tooltip();	
	$('.tip-left').tooltip({ placement: 'left' });	
	$('.tip-right').tooltip({ placement: 'right' });	
	$('.tip-top').tooltip({ placement: 'top' });	
	$('.tip-bottom').tooltip({ placement: 'bottom' });	
	
    /* === Search input typeahead === */
    /*
	$('#search input[type=text]').typeahead({
		source: ['Dashboard','Form elements','Common Elements','Validation','Wizard','Buttons','Icons','Interface elements','Support','Calendar','Gallery','Reports','Charts','Graphs','Widgets'],
		items: 4
	});*/
	
	/* === Fixes the position of buttons group in content header and top user navigation === */
	function fix_position()
	{
		var uwidth = $('#user-nav > ul').width();
		$('#user-nav > ul').css({width:uwidth,'margin-left':'-' + uwidth / 2 + 'px'});
        
        var cwidth = $('#content-header .btn-group').width();
        $('#content-header .btn-group').css({width:cwidth,'margin-left':'-' + uwidth / 2 + 'px'});
	}
	
	/* === Style switcher === */
	$('#style-switcher i').click(function()
	{
		if($(this).hasClass('open'))
		{
			$(this).parent().animate({marginRight:'-=190'});
			$(this).removeClass('open');
		} else 
		{
			$(this).parent().animate({marginRight:'+=190'});
			$(this).addClass('open');
		}
		$(this).toggleClass('icon-arrow-left');
		$(this).toggleClass('icon-arrow-right');
	});
	
	$('#style-switcher a').click(function()
	{
		var style = $(this).attr('href').replace('#','');
		$('.skin-color').attr('href','css/maruti.'+style+'.css');
		$(this).siblings('a').css({'border-color':'transparent'});
		$(this).css({'border-color':'#aaaaaa'});
	});
	
	$('.lightbox_trigger').click(function(e) {
		
		e.preventDefault();
		
		var image_href = $(this).attr("href");
		
		if ($('#lightbox').length > 0) {
			
			$('#imgbox').html('<img src="' + image_href + '" /><p><i class="icon-remove icon-white"></i></p>');
		   	
			$('#lightbox').slideDown(500);
		}
		
		else { 
			var lightbox = 
			'<div id="lightbox" style="display:none;">' +
				'<div id="imgbox"><img src="' + image_href +'" />' + 
					'<p><i class="icon-remove icon-white"></i></p>' +
				'</div>' +	
			'</div>';
				
			$('body').append(lightbox);
			$('#lightbox').slideDown(500);
		}
		
	});
	

	$('#lightbox').on('click', function() { 
		$('#lightbox').hide(200);
	});
	
});

function hello() {
    notifyMe();
}
//window.setInterval(hello, 1000);

function notifyMe() {
    // Let's check if the browser supports notifications
    if (!("Notification" in window)) {
        alert("This Browser Does Not Support Desktop Notification");
    }
    // Let's check if the user is  okay to get some notification
    else if (Notification.permission === "granted") {
        // If it's okay let's create a notification
        var v = "Successfully Received 15 Orders!";
        var notification = new Notification("Hi Here!", {
            body: v,
            icon: 'http://img0.imgtn.bdimg.com/it/u=1253747632,4261714029&fm=27&gp=0.jpg',
            tag: 'ss',
            renotify: 'true'
        });
        //notification.onclick = function (event) {
        //    event.preventDefault(); // prevent the browser from focusing the Notification's tab
        //    window.open('http://localhost:19680/WMS/ASNManagement/Index', '_blank');
        //}
    }
    // Otherwise, we need to ask the user for permission
    else if (Notification.permission !== 'denied') {
        Notification.requestPermission(function (permission) {
            // If the user is okay, let's create a notification
            if (permission === "granted") {
                var notification = new Notification("Hi Here!");
            }
        });
    }
    // At last, if the user already denied any notification, and you 
    // want to be respectful there is no need to bother them any more.
    
}



