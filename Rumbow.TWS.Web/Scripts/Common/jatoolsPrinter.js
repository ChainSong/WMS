function checkJatoolsPrinterInstalled() {
    var support = false, err = null, bs = ["MSIE", "Firefox", "Chrome"];
    for (var i = 0; i < bs.length; i++) {
        if (navigator.userAgent.indexOf(bs[i]) > -1) {
            support = true;
            break;
        }
    }
    if (!support) {
        err = "杰表打印控件不支持本浏览器!";
    } else if (navigator.userAgent.indexOf('Chrome') > -1) {
        var a = navigator.plugins, installed = false;
        for (var f = 0; f < a.length; f++) {
            if (a[f].name.indexOf('jatoolsPrinter') == 0) {
                installed = true;
                break;
            }
        }
        if (!installed)
            err = "杰表打印控件未安装，请点击<a href='jatoolsPrinter.crx'>此处</a>安装.";
    }
    if (err) {
        showError(err);
    }
}
function showError(err) {
    var buttons = document.getElementsByTagName("input");
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].disabled = true;
    }
    var _errs = document.getElementById("errs");
    _errs.innerHTML = err;
    _errs.style.display = 'block';
}
// 查看源文件,
function viewSource() {
    var fromURL = document.URL.replace(/^http[s]?\:\/\/.*?\//i, '');
    window
			.showModalDialog(
					'/sourceviewer/view.jsp?from=' + escape(fromURL),
					null,
					'dialogWidth=1024px;dialogHeight=670px;status=no;help=no;scroll=no;resizable=yes');
}
// / JP 定义
function jpExit() {
    getJP().exit();
}
function JP(proxy) {
    // 取得 documents 属性
    function ___(doc, id) {
        return doc.getElementById(id);
    }
    function ___getCSS(doc) {
        var result = '<style>';
        var sheets = doc.styleSheets;
        for (var i = 0; i < sheets.length; i++) {
            var sheet = sheets[i];
            try {
                var rules = sheet.cssRules;
                if (rules) {
                    for (var k = 0; k < rules.length; k++) {
                        result += rules[k].cssText || '';
                    }
                }
            } catch (e) {
            }
        }
        return result + '</style>';
    }
    function ___outerHTML(doc, node) {
        if (doc.doctype)
            node.setAttribute('_strict', 'true');
        return node.outerHTML || (function (n) {
            var div = doc.createElement('div'), h;
            div.appendChild(n.cloneNode(true));
            h = div.innerHTML;
            div = null;
            return h;
        })(node);
    }
    function ___getDocumentItem(doc, myDoc) {
        if (typeof (doc.getElementById) != 'undefined') {
            var result = "NSAPI://" + ___getCSS(doc) + "--\n\n\n--";
            if (myDoc.pages) {
                for (var i = 0; i < myDoc.pages.length; i++) {
                    var page = myDoc.pages[i];
                    if (typeof (page.substring) != 'undefined') {
                        page = ___(doc, page);
                    }
                    result += ("<div id='page" + (i + 1) + "'>"
							+ ___outerHTML(doc, page) + "</div>");
                }
            } else {
                var i = 0;
                while (true) {
                    var page = ___(doc, (myDoc.pagePrefix || '') + 'page'
									+ (i + 1));
                    if (!page)
                        break;
                    result += ___outerHTML(doc, page);
                    i++;
                }
            }
            return result;
        } else if (doc.html && doc.all) {
            var result = "NSAPI://*" + doc.html;
            return result;
        } else if (doc.html) {
            var result = "NSAPI:// --\n\n\n--";
            if (!doc.html.push) {
                doc.html = [doc.html];
            }
            for (var i = 0; i < doc.html.length; i++) {
                result += ("<div id='page" + (i + 1) + "'>" + doc.html[i] + "</div>");
            }
            return result;
        } else
            return doc;
    }
    function ___myDoc(myDoc) {
        myDoc.documents = ___getPrintedHTML(myDoc);
        if (myDoc.footer && myDoc.footer.html.innerHTML) {
            myDoc.footer.html = myDoc.footer.html.innerHTML;
        }
        if (myDoc.header && myDoc.header.html.innerHTML) {
            myDoc.header.html = myDoc.header.html.innerHTML;
        }
        return myDoc;
    }
    function ___getPrintedHTML(myDoc) {
        var docs = myDoc.documents, result = null;
        if (typeof (docs.push) != 'undefined') {
            result = [];
            for (var i = 0; i < docs.length; i++) {
                result.push(___getDocumentItem(docs[i], myDoc));
            }
            return result;
        } else {
            return ___getDocumentItem(docs, myDoc);
        }
    }
    function ___getDocumentHTML(target) {
        var result = "<html><head><style>" + ___getCSS(target.ownerDocument)
				+ "</style></head><body>"
				+ ___outerHTML(target.ownerDocument, target) + '</body></html>';
        return result;
    }
    function ___getWholeDocumentHTML(doc) {
        var result = "<html><head><base href='" + doc.URL + "'/><style>"
				+ ___getCSS(doc) + "</style></head><body>" + doc.body.innerHTML
				+ '</body></html>';
        return result;
    }
    function __getLic() {
        var oj = document.getElementById("ojatoolsPrinter");
        if (oj) {
            return oj.getAttribute("license") || "";
        }
        return "";
    }
    return ({
        proxy: proxy || null,
        isCRX: true,
        "extension": "jpnmbckmknckdkijflpiigdmfedhglnl",// pekcejhlaepnebiacngogmhpacgomfnp;jpnmbckmknckdkijflpiigdmfedhglnl
        // pfgnmidinmbbhpphafocpdilhgmfldjh;fbdfdjidlbaaclpflhcdceeoemlkbehd;
        "callbacks": [],
        "eventIndex": 0,
        "initialize": function () {
            var self = this;
            if (!proxy) {
                this.sendMessage({
                    method: "new",
                    lic: __getLic()
                });
                window.addEventListener('message', function (e) {
                    self.callbacks[e.data.event].apply(null,
                            e.data.params || [e.data.data]);
                });
                // window.onbeforeunload = window.onunload = jpExit;
            }
            return this;
        },
        "isInstalled": function (callback) {
            var result = false;
            if (this.proxy) {
                callback(typeof this.proxy.printPreview != 'undefined');
            } else {
                this.sendMessage({
                    method: "isInstalled"
                }, callback);
            }
        },
        "registerCallback": function (callback) {
            if (callback) {
                var index = "event-" + this.eventIndex++;
                this.callbacks[index] = callback;
                return index;
            } else
                return "";
        },
        "emptyCallback": function () {
        },
        "sendMessage": function (msg, callback) {
            chrome.runtime.sendMessage(this.extension, msg, callback
							|| this.emptyCallback);
        },
        "about": function () {
            this.proxy ? this.proxy.about() : this.sendMessage({
                method: "about"
            });
        },
        "exit": function () {
            this.sendMessage({
                method: "exit"
            });
            alert("exit");
        },
        "printPreview": function (myDoc, prompt) {
            if (this.proxy)
                this.proxy.printPreview(myDoc, prompt);
            else {
                myDoc = ___myDoc(myDoc);
                this.registerMyDocListeners(myDoc);
                this.sendMessage({
                    method: "printPreview",
                    params: [myDoc, prompt ? true : false]
                });
            }
        },
        "getPrinters": function (callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.getPrinters());
            else
                this.sendMessage({
                    method: "getPrinters",
                    event: this.registerCallback(callback)
                });
        },
        "getPapers": function (printer, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.getPapers(printer));
            else
                this.sendMessage({
                    method: "getPapers",
                    params: [printer],
                    event: this.registerCallback(callback)
                });
        },
        "isCustomPaperSupported": function (printer, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.isCustomPaperSupported(printer));
            else
                this.sendMessage({
                    method: "isCustomPaperSupported",
                    params: [printer],
                    event: this.registerCallback(callback)
                });
        },
        "registerMyDocListeners": function (myDoc) {
            var events = ["done", "onState", "listener", "onPagePrinted"];
            for (var i = 0; i < events.length; i++) {
                var e = events[i];
                if (myDoc[e]) {
                    myDoc[e] = this.registerCallback(myDoc[e]);
                    myDoc._hasCallback = true;
                }
            }
            if (myDoc.dragDesigner && myDoc.dragDesigner.ok) {
                myDoc.dragDesigner.ok = this
						.registerCallback(myDoc.dragDesigner.ok);
                myDoc._hasCallback = true;
            }
        },
        "print": function (myDoc, prompt) {
            if (this.proxy)
                this.proxy.print(myDoc, prompt);
            else {
                myDoc = ___myDoc(myDoc);
                this.registerMyDocListeners(myDoc);
                this.sendMessage({
                    method: "print",
                    params: [myDoc, prompt ? true : false]
                });
            }
        },
        "isExcelInstalled": function (callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.isExcelInstalled());
            else
                this.sendMessage({
                    method: "isExcelInstalled",
                    event: this.registerCallback(callback)
                });
        },
        "getDefaultPrinter": function (callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.getDefaultPrinter());
            else
                this.sendMessage({
                    method: "getDefaultPrinter",
                    event: this.registerCallback(callback)
                });
        },
        "getVersion": function (callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.getVersion());
            else
                this.sendMessage({
                    method: "getVersion",
                    event: this.registerCallback(callback)
                });
        },
        "isImplemented": function (method, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.isImplemented(method));
            else
                this.sendMessage({
                    method: "isImplemented",
                    params: [method],
                    event: this.registerCallback(callback)
                });
        },
        "getLocalMacAddress": function (callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.getLocalMacAddress());
            else
                this.sendMessage({
                    method: "getLocalMacAddress",
                    event: this.registerCallback(callback)
                });
        },
        "getCPUSerialNo": function (callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.getCPUSerialNo());
            else
                this.sendMessage({
                    method: "getCPUSerialNo",
                    event: this.registerCallback(callback)
                });
        },
        "setOffsetPage": function (offset, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.setOffsetPage(offset));
            else
                this.sendMessage({
                    method: "setOffsetPage",
                    params: [offset],
                    event: this.registerCallback(callback)
                });
        },
        "isPrintableFileType": function (filetype, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.isPrintableFileType(filetype));
            else
                this.sendMessage({
                    method: "isPrintableFileType",
                    params: [filetype],
                    event: this.registerCallback(callback)
                });
        },
        "setDragCSS": function (settingid, styles, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.setDragCSS(
								settingid, styles));
            else
                this.sendMessage({
                    method: "setDragCSS",
                    params: [settingid, styles],
                    event: this.registerCallback(callback)
                });
        },
        "clearLastSettings": function (settingid, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.clearLastSettings(settingid));
            else
                this.sendMessage({
                    method: "clearLastSettings",
                    params: [settingid],
                    event: this.registerCallback(callback)
                });
        },
        "printTIFF": function (url, margins, how, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.printTIFF(url,
								margins, how));
            else
                this.sendMessage({
                    method: "printTIFF",
                    params: [url, margins, how],
                    event: this.registerCallback(callback)
                });
        },
        "printDocument": function (file, sets, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.printDocument(
								file, sets));
            else
                this.sendMessage({
                    method: "printDocument",
                    params: [file, sets],
                    event: this.registerCallback(callback)
                });
        },
        "exportAsImage": function (element, path, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.exportAsImage(
								element, path));
            else {
                var id = element.id;
                var ownerdoc = element.ownerDocument;
                var tmpid = element.id = "tmp" + new Date().getTime();
                var html = ___getWholeDocumentHTML(ownerdoc);
                element.id = id;
                this.sendMessage({
                    method: "exportAsImage",
                    params: [{
                        html: html,
                        element: tmpid
                    }, path],
                    event: this.registerCallback(callback)
                });
            }
        },
        "exportAsExcel": function (tableEl, path, showProgress, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.exportAsExcel(
								tableEl, path, showProgress));
            else {
                var html = ___getDocumentHTML(tableEl);
                this.sendMessage({
                    method: "exportAsExcel",
                    params: [html, path, showProgress],
                    event: this.registerCallback(callback)
                });
            }
        },
        "setupNormalOffset": function (settingid, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.setupNormalOffset(settingid));
            else
                this.sendMessage({
                    method: "setupNormalOffset",
                    params: [settingid],
                    event: this.registerCallback(callback)
                });
        },
        "download": function (url, file, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.download(url,
								file));
            else
                this.sendMessage({
                    method: "download",
                    params: [url, file],
                    event: this.registerCallback(callback)
                });
        },
        "printToImage": function (myDoc, path, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.printToImage(
								myDoc, path));
            else {
                myDoc = ___myDoc(myDoc);
                this.registerMyDocListeners(myDoc);
                this.sendMessage({
                    method: "printToImage",
                    params: [myDoc, path],
                    event: this.registerCallback(callback)
                });
            }
        },
        "printToPDF": function (myDoc, path, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.printToPDF(
								myDoc, path));
            else {
                myDoc = ___myDoc(myDoc);
                this.registerMyDocListeners(myDoc);
                this.sendMessage({
                    method: "printToPDF",
                    params: [myDoc, path],
                    event: this.registerCallback(callback)
                });
            }
        },
        "liveUpdate": function (expected, url, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.liveUpdate(
								expected, url));
            else {
                this.sendMessage({
                    method: "liveUpdate",
                    params: [expected, url],
                    event: this.registerCallback(callback)
                });
            }
        },
        "getFonts": function (callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.getFonts());
            else
                this.sendMessage({
                    method: "getFonts",
                    event: this.registerCallback(callback)
                });
        },
        "copy": function (data, format, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.copy(data,
								format));
            else
                this.sendMessage({
                    method: "copy",
                    params: [data, format],
                    event: this.registerCallback(callback)
                });
        },
        "copied": function (format, callback) {
            if (this.proxy)
                (callback || this.nothing)
						.call(this, this.proxy.copied(format));
            else
                this.sendMessage({
                    method: "copied",
                    params: [format],
                    event: this.registerCallback(callback)
                });
        },
        "writeString": function (file, encode, data, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.writeString(
								file, encode, data));
            else
                this.sendMessage({
                    method: "writeString",
                    params: [file, encode, data],
                    event: this.registerCallback(callback)
                });
        },
        "writeBase64": function (file, data, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.writeBase64(
								file, data));
            else
                this.sendMessage({
                    method: "writeBase64",
                    params: [file, data],
                    event: this.registerCallback(callback)
                });
        },
        "readString": function (file, encode, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.readString(
								file, encode));
            else
                this.sendMessage({
                    method: "readString",
                    params: [file, encode],
                    event: this.registerCallback(callback)
                });
        },
        "readBase64": function (file, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.readBase64(file));
            else
                this.sendMessage({
                    method: "readBase64",
                    params: [file],
                    event: this.registerCallback(callback)
                });
        },
        "readHTML": function (file, defencode, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.readHTML(file,
								defencode));
            else
                this.sendMessage({
                    method: "readHTML",
                    params: [file, defencode],
                    event: this.registerCallback(callback)
                });
        },
        "chooseFile": function (ext, defext, saveselect, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy.chooseFile(
								ext, defext, saveselect));
            else
                this.sendMessage({
                    method: "chooseFile",
                    params: [ext, defext, saveselect],
                    event: this.registerCallback(callback)
                });
        },
        "showPageSetupDialog": function (callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.showPageSetupDialog());
            else
                this.sendMessage({
                    method: "showPageSetupDialog",
                    event: this.registerCallback(callback)
                });
        },
        "getLastSettings": function (settingid, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.getLastSettings(settingid));
            else
                this.sendMessage({
                    method: "getLastSettings",
                    params: [settingid],
                    event: this.registerCallback(callback)
                });
        },
        "getAbsoluteURL": function (relativeURL, p1, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.getAbsoluteURL(relativeURL, p1));
            else
                this.sendMessage({
                    method: "getAbsoluteURL",
                    params: [relativeURL, p1],
                    event: this.registerCallback(callback)
                });
        },
        "setLastSettings": function (settingid, doc, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.setLastSettings(settingid, doc));
            else
                this.sendMessage({
                    method: "setLastSettings",
                    params: [settingid, doc],
                    event: this.registerCallback(callback)
                });
        },
        "setDefaultPrinter": function (printer, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.setDefaultPrinter(printer));
            else
                this.sendMessage({
                    method: "setDefaultPrinter",
                    params: [printer],
                    event: this.registerCallback(callback)
                });
        },
        "openFile": function (file, callback) {
            if (this.proxy)
                (callback || this.nothing)
						.call(this, this.proxy.openFile(file));
            else
                this.sendMessage({
                    method: "openFile",
                    params: [file],
                    event: this.registerCallback(callback)
                });
        },
        "getPrinterInfo": function (printer, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.getPrinterInfo(printer));
            else
                this.sendMessage({
                    method: "getPrinterInfo",
                    params: [printer],
                    event: this.registerCallback(callback)
                });
        },
        "getPrinterStatus": function (printer, text, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.getPrinterStatus(printer, text));
            else
                this.sendMessage({
                    method: "getPrinterStatus",
                    params: [printer, text],
                    event: this.registerCallback(callback)
                });
        },
        "nothing": function () {
        },
        "setPrintBackground": function (back, callback) {
            if (this.proxy)
                (callback || this.nothing).call(this, this.proxy
								.setPrintBackground(back));
            else
                this.sendMessage({
                    method: "setPrintBackground",
                    params: [back],
                    event: this.registerCallback(callback)
                });
        }
    }).initialize();
};
var _jp = null;
/*
 * 取得打印控件声明，并检测是否正确安装 调用方法: 1. 异步调用 getJatoolsPrinter(function(result){ //
 * result = {error: "错误信息",code:0/1/2 }
 * 
 * 
 * if(result.code) { // result.code=0 , 安装正确 ;result.code=1 ,没有声明 ;
 * result.code=2 有声明，但没设置正确 if(result.errorMessage) { } } })
 */
function getJatoolsPrinter(doc) {
    if (!_jp) {
        var doc = doc || document;
        var proxy = navigator.userAgent.match(/(msie\s|trident.*rv:)([\w.]+)/i)
				? doc.getElementById('ojatoolsPrinter')
				: doc.getElementById('ejatoolsPrinter');
        _jp = new JP(proxy);
    }
    return _jp;
}
function isChrome45() {
    var chrome = navigator.userAgent.match(/Chrom(e|ium)\/([0-9]+)\./);
    if (chrome) {
        var ver = parseInt(chrome[2], 10);
        if (ver >= 35) {
            return true;
        }
    }
    return false;
}
isChrome45() && window.addEventListener('load', function () {
    _jp = new JP();
    _jp.isInstalled(function (result) {
        if (!result) {
            alert("未安装！")
        }
    });
}, false);
