//YXDR打印上架单
function PrintReceipt() {

    doPrint("打印")
}

function doPrint(how) {
    //打印文档对象
    var myDoc = {
        settings: { topMargin: 50, leftMargin: 50, bottomMargin: 50, rightMargin: 50 },
        documents: document,    // 打印页面(div)们在本文档中
        // 打印时,only_for_print取值为显示
        classesReplacedWhenPrint: new Array('.only_for_print{display:block}'),
        copyrights: '杰创软件拥有版权  www.jatools.com'         // 版权声明必须
    };

    var jatoolsPrinter = getJatoolsPrinter();
    if (how == '打印预览...') {
        jatoolsPrinter.printPreview(myDoc);//打印预览
    }
    else if (how == '打印...') {
        jatoolsPrinter.print(myDoc, true);

    }
    else {
        jatoolsPrinter.print(myDoc, true);//直接打印
    }

}

