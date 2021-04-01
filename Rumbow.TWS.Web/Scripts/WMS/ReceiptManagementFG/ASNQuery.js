$(document).ready(function () {
    $("#ASNTable tr").click(function () {
        $(this).addClass("btn-success").siblings("tr").removeClass("btn-success");
    });
     $("#ASNTable tr").mouseover(function () {
        $(this).addClass("btn-info").siblings("tr").removeClass("btn-info");
    });
   
    $("#ASNTable tr").mouseleave(function () {
        $(this).removeClass("btn-info");
    });
    
    $("#ASNTable tr").dblclick(function () {
        var length = $("#ASNTable tr").length;
        var ASNID = $(this).children()[4].innerText;
        var CustomerID = $(this).children()[6].innerText;
        //for (i = 0; i < length; i++) {
            //alert($(this).attr('class'));
            //if ($("#ASNTable tr")[i].className == "btn-success") {
                //var ASNNumber=$("#ASNTable tr")[i].id;
                //var ASNID = $("#ASNTable tr")[i].childNodes[9].innerHTML;
                //var ExternReceiptNumber = $("#ASNTable tr")[i].childNodes[11].innerHTML;
                //var CustomerName = $("#ASNTable tr")[i].childNodes[3].innerHTML;
                //var CustomerID = $("#ASNTable tr")[i].childNodes[13].innerHTML;
                //var ASNType = $("#ASNTable tr")[i].childNodes[15].innerHTML;
                //var box = {
                //    ASNNumbers:ASNNumber,ASNIDs: ASNID, ExternReceiptNumbers: ExternReceiptNumber, CustomerNames: CustomerName, CustomerIDs: CustomerID, ASNTypes: ASNType
                //    }
                //closePopup($("#ASNTable tr")[i].id, ASNID, ExternReceiptNumber, CustomerName, CustomerID, CustomerID);
                closePopup(ASNID, CustomerID);

            //}


        //}
    });
});

$('#statusBackOK').live('click', function () {

    var length = $("#ASNTable tr").length;
   
    for(i=0;i<length;i++)
    {
        
        if ($("#ASNTable tr")[i].className == "btn-success")
        {
            //var ASNNumber=$("#ASNTable tr")[i].id;
            var ASNID = $("#ASNTable tr")[i].childNodes[9].innerHTML;
            //var ExternReceiptNumber = $("#ASNTable tr")[i].childNodes[11].innerHTML;
            //var CustomerName = $("#ASNTable tr")[i].childNodes[3].innerHTML;
            var CustomerID = $("#ASNTable tr")[i].childNodes[13].innerHTML;
            //var ASNType = $("#ASNTable tr")[i].childNodes[15].innerHTML;
            //var box = {
            //    ASNNumbers:ASNNumber,ASNIDs: ASNID, ExternReceiptNumbers: ExternReceiptNumber, CustomerNames: CustomerName, CustomerIDs: CustomerID, ASNTypes: ASNType
            //    }
            //closePopup($("#ASNTable tr")[i].id, ASNID, ExternReceiptNumber, CustomerName, CustomerID, CustomerID);
            closePopup(ASNID, CustomerID);
                   
        }


    }

    
});

$('#statusBackReturn').live('click', function () {
    closePopup();
});

//$('#searchButton').live('click', function () {
//    location.href = "/WMS/ReceiptManagementFG/ASNQuery";
//});