String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

function Check() {
    var psw1 = document.getElementById('Psw1').value;
    var psw2 = document.getElementById('Psw2').value;
   
  
    if (psw1.trim() == "" || psw2.trim() == "") {
        layer.msg('密码不能为空!');
        return false;
    }

    if (psw1 || psw2) {
        if (!psw1 || !psw2 || psw1 != psw2) {
            layer.msg('输入的密码不一致!');
            return false;
        }
        else {
            return true;
        }

    }

    return false;
  }
  
