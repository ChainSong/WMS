$(document).ready(function () {

});


function doPrint() {

    $.ajax({
        url: "/WMS/OrderManagement/PrintOrderNike",
        type: "POST",
        dataType: "json",
        data: {},
        success: function (data) {
            for (var i = 0; i < 2; i++) {

                bdhtml = window.document.body.innerHTML;
                sprnstr = "<!--startprint-->";
                eprnstr = "<!--endprint-->";
                prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 17);
                prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
                //window.document.body.innerHTML += prnhtml;
                $("#dayin").append(prnhtml);

            }
            $("#PageNext")[0].style.display = "";
            window.print();
            //$("#PageNexta")[0].style.display = "none";

            //var hkey_root, hkey_path, hkey_key
            //hkey_root = "HKEY_CURRENT_USER"
            //hkey_path = "\Software\Microsoft\Internet Explorer\PageSetup\ "
            //function pagesetup_null() {
            //    try {
            //        var RegWsh = new ActiveXObject("WScript.Shell")
            //        hkey_key = "header"
            //        RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "")
            //        hkey_key = "footer"
            //        RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "")
            //    } catch (e) { }
            //}
            //bdhtml = window.document.body.innerHTML;
            //sprnstr = "<!--startprint-->";
            //eprnstr = "<!--endprint-->";
            //prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 17);
            //prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            //window.document.body.innerHTML = prnhtml;
            //window.print();
        }
    });
    //bdhtml = window.document.body.innerHTML;
    //sprnstr = "<!--startprint-->";
    //eprnstr = "<!--endprint-->";
    //prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 17);
    //prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
    //window.document.body.innerHTML = prnhtml;
    //window.print();
}

