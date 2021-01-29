var ping = 0;
var ID = '';

function pings(fen, oObject) {
    var oobj = $(oObject).parent().parent().parent().parent().parent().attr('id');
    if (fen == "1") {
        ping = fen;
        ID = oobj
        //demo
        //弹出框（name/父窗体是否可点/width/height/Url(div的id)/是否在当前页的IFrame中打开/ 方法 fun）
        //function VerQuery() {
        //    openPopup("name", true, 700, 400, "index.aspx", false, function (name, verId) {
        //        $("#a").val(a);
        //    });
        //}
        openPopup("", true, 300, 150, null, "divSetting");
    }
    else {

        DelBtn(oobj, fen);
    }
}
function del() {
    var ValFrom = $("#ValFrom").val();
    var id = ID;
    if (ping != 0 && ValFrom != '') {
        closePopup()
        $.ajax({
            type: "POST",
            url: "/Phone/Pod/PhoneScore",
            data: {
                "id": id,
                "ping": ping,
                "ValFrom": ValFrom
            },
            async: "false",
            success: function (data) {
                //$("#" + id).remove(1);
                $("#" + id).slideUp(300, function () {
                    //移除父级div
                    $("#" + id).remove();
                });
            },
            error: function (msg) {
                alert(msg.val);
            }
        });
        document.getElementById("stars1-input").value = '';
        document.getElementById("ValFrom").innerText = '';
        ID = '';
        ping = 0;
    }
}
function DelBtn(oObject, fen) {
    if (fen != 0) {
        var id = oObject;
       
        $.ajax({
            type: "POST",
            url: "/Phone/Pod/PhoneScore",
            data: {
                "id": id,
                "ping": fen,
                "ValFrom": $("ValFrom").val()
            },
            async: "false",
            success: function (data) {
                $("#" + id).slideUp(300, function () {
                    //移除父级div
                    $("#" + id).remove();
                });
            },
            error: function (msg) {
                alert(msg.val);
            }
        });
        document.getElementById("stars1-input").value = '';
        ping = 0;
    }
}
var myScroll,
	pullDownEl, pullDownOffset,
	pullUpEl, pullUpOffset,
	generatedCount = 0;
/**
 * 滚动翻页 （自定义实现此方法）
 * myScroll.refresh();		// 数据加载完成后，调用界面更新方法
 */
function pullUpAction() {
    setTimeout(function () {	// <-- Simulate network congestion, remove setTimeout from production!
     
        var el, li, i;
        el = document.getElementById('thelist');
        $.ajax({
            type: "POST",
            url: "/Phone/Pod/QueryPod",
            data: {
                "id": $("#id").val(),
                "index": $("#PageIndex").val(),
                "Type": $("#Type").val()
            },
            async: "false",
            success: function (data) {
                if (JSON.parse(data)["WeiQueryPod"] != null) {

                    document.getElementById('PageIndex').value = parseInt($("#PageIndex").val()) + 1;
                    var el = document.getElementById('thelist');
                    var Type = JSON.parse(data)["Type"];
                    for (i = 0; i < JSON.parse(data)["WeiQueryPod"].length ; i++) {
                        var TransOrderNumber = JSON.parse(data)["WeiQueryPod"][i]["TransOrderNumber"];
                        var UID = JSON.parse(data)["WeiQueryPod"][i]["UID"];
                        var OriginLoadCity = JSON.parse(data)["WeiQueryPod"][i]["OriginLoadCity"];
                        var DestinationCity = JSON.parse(data)["WeiQueryPod"][i]["DestinationCity"];
                        var ArrTime = JSON.parse(data)["WeiQueryPod"][i]["ArrTime"];
                        var PlanTtlPiecQuantity = JSON.parse(data)["WeiQueryPod"][i]["PlanTtlPiecQuantity"];
                        var CarrierCode = JSON.parse(data)["WeiQueryPod"][i]["CarrierCode"];
                        li = document.createElement('li');
                        li.id = UID;
                        li.className = 'ui-li-static ui-body-inherit';
                        var Str ='<div class="ui-block-a" style="width: 100%"> <a href="javascript:void(0)">' + CarrierCode + '</a>';
                        if (Type == 1) {
                            Str += ' <div class="shop-rating" style="width: 120px; float: right"><ul class="rating-level"><li><a class="one-star" onclick="pings(1,this)" star:value="1" href="javascript:void(0)">1</a></li><li><a class="two-stars" onclick="pings(2,this)" star:value="2" href="javascript:void(0)">2</a></li><li><a class="three-stars" onclick="pings(3,this)" star:value="3" href="javascript:void(0)">3</a></li><li><a class="four-stars" onclick="pings(4,this)" star:value="4" href="javascript:void(0)">4</a></li><li><a class="five-stars" onclick="pings(5,this)" star:value="5" href="javascript:void(0)">5</a></li></ul> </div>';
                        } else {
                            Str += ' <span style="float: right">5</span>'
                        }
                        Str+='</div><div style="float: left; margin-top: -10px;"><h6>'+TransOrderNumber+'| '+ArrTime +'</h6></div>';
                        li.innerHTML = Str;
                        el.appendChild(li, el.childNodes[0]);
                    }
                }
            },
            error: function (msg) {
                alert(msg.val);
            }
        });
        myScroll.refresh();		// 数据加载完成后，调用界面更新方法 Remember to refresh when contents are loaded (ie: on ajax completion)
    }, 1000);	// <-- Simulate network congestion, remove setTimeout from production!
}

/**
 * 初始化iScroll控件
 */
function loaded() {
    pullDownEl = document.getElementById('pullDown');
    pullDownOffset = pullDownEl.offsetHeight;
    pullUpEl = document.getElementById('pullUp');
    pullUpOffset = pullUpEl.offsetHeight;

    myScroll = new iScroll('wrapper', {
        scrollbarClass: 'myScrollbar', /* 重要样式 */
        useTransition: false, /* 此属性不知用意，本人从true改为false */
        topOffset: pullDownOffset,
        onRefresh: function () {
            if (pullDownEl.className.match('loading')) {
                pullDownEl.className = '';
                pullDownEl.querySelector('.pullDownLabel').innerHTML = '';
            } else if (pullUpEl.className.match('loading')) {
                pullUpEl.className = '';
                pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉加载更多...';
            }
        },
        onScrollMove: function () {
            if (this.y < (this.maxScrollY - 5) && !pullUpEl.className.match('flip')) {
                pullUpEl.className = 'flip';
                pullUpEl.querySelector('.pullUpLabel').innerHTML = '松手开始更新...';
                this.maxScrollY = this.maxScrollY;
            } else if (this.y > (this.maxScrollY + 5) && pullUpEl.className.match('flip')) {
                pullUpEl.className = '';
                pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉加载更多...';
                this.maxScrollY = pullUpOffset;
            }
        },
        onScrollEnd: function () {
            if (pullUpEl.className.match('flip')) {
                pullUpEl.className = 'loading';
                pullUpEl.querySelector('.pullUpLabel').innerHTML = '加载中...';
                pullUpAction();	// Execute custom function (ajax call?)
            }
        },
        onBeforeScrollStart: function(e) { 
            var target = e.target; 
            while (target.nodeType != 1) 
                target = target.parentNode; 
            if (target.tagName != 'SELECT' && target.tagName != 'INPUT' && target.tagName != 'TEXTAREA' && target.tagName != 'BUTTON') {
                e.preventDefault();
            }
        }
    });

    setTimeout(function () { document.getElementById('wrapper').style.left = '0'; }, 800);
}

//初始化绑定iScroll控件 
document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
document.addEventListener('DOMContentLoaded', loaded, false);


//var myScroll,
//	pullDownEl, pullDownOffset,
//	pullUpEl, pullUpOffset,
//	generatedCount = 0;

///**
// * 下拉刷新 （自定义实现此方法）
// * myScroll.refresh();		// 数据加载完成后，调用界面更新方法
// */
//function pullDownAction() {
//    setTimeout(function () {	// <-- Simulate network congestion, remove setTimeout from production!
//        //var el, li, i;
//        //el = document.getElementById('thelist');

//        //  $.ajax({
//        //    type: "POST",
//        //    url: "/Phone/Pod/QueryPod",
//        //    data: {
//        //        "id": "",
//        //        "index": $("#PageIndex").val()
//        //    },
//        //    async: "false",
//        //    success: function (data) {

//        //        var el = document.getElementById('thelist');
//        //        for (i = 0; i < JSON.parse(data)["WeiQueryPod"].length ; i++) {
//        //            li = document.createElement('li');
//        //            li.id = JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"];
//        //            li.className = 'ui-li-static ui-body-inherit';
//        //            li.onclick = 'DelBtn(' + JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"] + ')';
//        //            li.innerHTML = '  <div class="ui-block-a"><a href="#">' + JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"] + ' </a><h6>' + JSON.parse(data)["WeiQueryPod"][i]["@Pod.ProjectId"], JSON.parse(data)["WeiQueryPod"][i]["@Pod.ProjectName"] + '</h6> <br>  </div><p style="float: right;" class="RatingDiv"></p> <input id="CRMShipper_Rating" name="CRMShipper.Rating" type="hidden" value="" /> ';
//        //            el.insertBefore(li, el.childNodes[0]);
//        //        }
//        //    },
//        //    error: function (msg) {
//        //        alert(msg.val);
//        //    }
//        setTimeout(function () {	// <-- Simulate network congestion, remove setTimeout from production!
//            var el, li, i;
//            el = document.getElementById('thelist');

//            for (i=0; i<9; i++) {
//                li = document.createElement('li');
//                li.innerText = 'Generated row ' + (++generatedCount);
//                el.insertBefore(li, el.childNodes[0]);
//            }

//            myScroll.refresh();		//数据加载完成后，调用界面更新方法   Remember to refresh when contents are loaded (ie: on ajax completion)
//        }, 1000);
//        //})


//        myScroll.refresh();		//数据加载完成后，调用界面更新方法   Remember to refresh when contents are loaded (ie: on ajax completion)
//    }, 1000);	// <-- Simulate network congestion, remove setTimeout from production!
//}

///**
// * 滚动翻页 （自定义实现此方法）
// * myScroll.refresh();		// 数据加载完成后，调用界面更新方法
// */

//function pullUpAction() {
//    setTimeout(function () {	// <-- Simulate network congestion, remove setTimeout from production!
//        //var el, li, i;
//        //el = document.getElementById('thelist');

//        //$.ajax({
//        //    type: "POST",
//        //    url: "/Phone/Pod/QueryPod",
//        //    data: {
//        //        "id": "",
//        //        "index": $("#PageIndex").val()
//        //    },
//        //    async: "false",
//        //    success: function (data) {

//        //        if (data!=null)
//        //        document.getElementById('PageIndex').value = parseInt($("#PageIndex").val()) + 1;
//        //        var el = document.getElementById('thelist');
//        //        //var htmls;
//        //        for (i = 0; i < JSON.parse(data)["WeiQueryPod"].length ; i++) {


//        //            //htmls += '<li id="' + JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"] + '" class="ui-li-static ui-body-inherit" onclick="DelBtn(' + JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"] + ')">';
//        //            //htmls += '<div class="ui-block-a"><a href="#">' + JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"] + '</a><h6>1 Adidas' + i + '</h6><br /></div>';
//        //            //htmls += '<p style="float: right;" class="RatingDiv"></p>';
//        //            //htmls += '</li> ';
//        //            //li = document.createElement('li');
//        //            //li.id = JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"];
//        //            //li.className = 'ui-li-static ui-body-inherit ';
//        //            //li.setAttribute("onclick", "   DelBtn(" + JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"] + ")");
//        //            //p = document.createElement('p');
//        //            //p.setAttribute("class", " RatingDiv");
//        //            //p.setAttribute("style", " float: right");
//        //            //hidden = document.createElement('hidden');
//        //            //hidden.id = 'CRMShipper_Rating';
//        //            //hidden.setAttribute("Name", " CRMShipper.Rating");
//        //            //hidden.value = '';
//        //            //div + '<p style="float: right;" class="RatingDiv"></p> <input id="CRMShipper_Rating" name="CRMShipper.Rating" type="hidden" value="" />';
//        //            //var div = '<div><a href="#">' + JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"] + ' </a><h6>' + JSON.parse(data)["WeiQueryPod"][i]["@Pod.ProjectId"] + '，' + JSON.parse(data)["WeiQueryPod"][i]["@Pod.ProjectName"] + '</h6> <br>  </div>';
//        //            //div += '<p  class="RatingDiv" style="float: right" ></p> <input id="CRMShipper_Rating" name="CRMShipper.Rating" type="hidden" value="" />';
//        //           //p.innerHTML = div;
//        //            //li.appendChild(p);
//        //            //li.appendChild(hidden);
//        //            li = document.createElement('li');
//        //            li.id = JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"];
//        //            li.className = 'ui-li-static ui-body-inherit';
//        //            li.setAttribute("onclick", "   DelBtn(" + JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"] + ")");
//        //            li.innerHTML = '  <div class="ui-block-a"><a href="#">' + JSON.parse(data)["WeiQueryPod"][i]["CustomerCode"] + ' </a><h6>' + JSON.parse(data)["WeiQueryPod"][i]["ProjectId"] + '' + JSON.parse(data)["WeiQueryPod"][i]["ProjectName"] + '</h6> <br>  </div><p style="float: right;" class="RatingDiv"><div class="shop-rating" style="text-align:right;margin:0px auto 0px auto;"> <ul class="rating-level" id="stars1" ><li><a class="one-star"   href="#">1</a></li><li><a class="two-stars"   href="#">2</a></li><li><a class="three-stars"  href="#">3</a></li><li><a class="four-stars"   href="#">4</a></li>  <li><a class="five-stars"   href="#">5</a></li></ul> <span class="result" id="(@Pod.CustomerCode+1)"></span><input type="hidden"  name="a" value="" size="2" /> </div></p><input id="CRMShipper_Rating" name="CRMShipper.Rating" type="hidden" value="" />';
//        //            el.appendChild(li, el.childNodes[0]);
//        //           // $(el).append(li);
//        //           // el.innerHTML += htmls;
//        //            //$(".RatingDiv")
//        //        }
//        //    },
//        //    error: function (msg) {
//        //        alert(msg.val);
//        //    }
//        //})
//        setTimeout(function () {	// <-- Simulate network congestion, remove setTimeout from production!
//            var el, li, i;
//            el = document.getElementById('thelist');

//            for (i = 0; i < 9; i++) {
//                li = document.createElement('li');
//                li.innerText = 'Generated row ' + (++generatedCount);
//                el.appendChild(li, el.childNodes[0]);
//            }

//            myScroll.refresh();		//数据加载完成后，调用界面更新方法   Remember to refresh when contents are loaded (ie: on ajax completion)
//        }, 1000);
//        myScroll.refresh();		// 数据加载完成后，调用界面更新方法 Remember to refresh when contents are loaded (ie: on ajax completion)
//    }, 1000);	// <-- Simulate network congestion, remove setTimeout from production!
//}

///**
// * 初始化iScroll控件
// */
//function loaded() {
//    pullDownEl = document.getElementById('pullDown');
//    pullDownOffset = pullDownEl.offsetHeight;
//    pullUpEl = document.getElementById('pullUp');
//    pullUpOffset = pullUpEl.offsetHeight;

//    myScroll = new iScroll('wrapper', {
//        scrollbarClass: 'myScrollbar', /* 重要样式 */
//        useTransition: false, /* 此属性不知用意，本人从true改为false */
//        topOffset: pullDownOffset,
//        onRefresh: function () {
//            if (pullDownEl.className.match('loading')) {
//                pullDownEl.className = '';
//                pullDownEl.querySelector('.pullDownLabel').innerHTML = '下拉刷新...';
//            } else if (pullUpEl.className.match('loading')) {
//                pullUpEl.className = '';
//                pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉加载更多...';
//            }
//        },
//        onScrollMove: function () {
//            if (this.y > 5 && !pullDownEl.className.match('flip')) {
//                pullDownEl.className = 'flip';
//                pullDownEl.querySelector('.pullDownLabel').innerHTML = '松手开始更新...';
//                this.minScrollY = 0;
//            } else if (this.y < 5 && pullDownEl.className.match('flip')) {
//                pullDownEl.className = '';
//                pullDownEl.querySelector('.pullDownLabel').innerHTML = '下拉刷新...';
//                this.minScrollY = -pullDownOffset;
//            } else if (this.y < (this.maxScrollY - 5) && !pullUpEl.className.match('flip')) {
//                pullUpEl.className = 'flip';
//                pullUpEl.querySelector('.pullUpLabel').innerHTML = '松手开始更新...';
//                this.maxScrollY = this.maxScrollY;
//            } else if (this.y > (this.maxScrollY + 5) && pullUpEl.className.match('flip')) {
//                pullUpEl.className = '';
//                pullUpEl.querySelector('.pullUpLabel').innerHTML = '上拉加载更多...';
//                this.maxScrollY = pullUpOffset;
//            }
//        },
//        onScrollEnd: function () {
//            if (pullDownEl.className.match('flip')) {
//                pullDownEl.className = 'loading';
//                pullDownEl.querySelector('.pullDownLabel').innerHTML = '加载中...';
//                pullDownAction();	// Execute custom function (ajax call?)
//            } else if (pullUpEl.className.match('flip')) {
//                pullUpEl.className = 'loading';
//                pullUpEl.querySelector('.pullUpLabel').innerHTML = '加载中...';
//                pullUpAction();	// Execute custom function (ajax call?)
//            }
//        }
//    });

//    setTimeout(function () { document.getElementById('wrapper').style.left = '0'; }, 800);
//}

////初始化绑定iScroll控件 
//document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
//document.addEventListener('DOMContentLoaded', loaded, false);















