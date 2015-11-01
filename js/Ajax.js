function ajax(url, onsuccess, onfail) {
    var xmlhttp = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject('Microsoft.XMLHTTP');
    xmlhttp.open("POST", url, true);
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            if (xmlhttp.status == 200) {
                onsuccess(xmlhttp.responseText);
            }else {
                onfail(xmlhttp.status);
            }
        }
    }
    xmlhttp.send(); //这时才开始发送请求
}

function Email() {
    var email = document.getElementById("Account")
    ajax("../../RegiSter.ashx?Action=users&&account=" + email.value, function (restext) {
        if (restext == "NO") {
            document.getElementById("unm").innerHTML = "用户已存在";
        } else if (restext == "OK") {
            document.getElementById("unm").innerHTML = "OK";
        } else {
            document.getElementById("unm").innerHTML = "Unexpected situation";
        }
    });
}

function Zz() {
    var email = document.getElementById("Account")
    if (email.value) {
        var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (filter.test(email.value)) {
            document.getElementById("unm").innerHTML = "";
            Email();
        } else if (!(filter.test(email.value)) && email.value != "") {
            document.getElementById("unm").innerHTML = "ID shit";
        }
    }
}

function Password(){
    var password = document.getElementById("inputPassword").value;
    var cpassword = document.getElementById("confirmpassword").value;
    if (password === cpassword) {
        document.getElementById("unm1").innerHTML = "密码格式正确"
    }
    else if (password !== cpassword) {
        document.getElementById("unm1").innerHTML = "两次输入密码不相同"
    }
}

function Mmzz() {
    var mima = document.getElementById("inputPassword")
    if (mima.value) {
        var filter = /^[0-9 | A-Z | a-z]{6,16}$/
        if (filter.test(mima.value)) {
            Password();
        } else {
            document.getElementById("unm1").innerHTML = "密码格式为6-16位字母或者数字"
        }
    }
}

function GetSession() {
    var yzm = document.getElementById("code")
    if (yzm) {
        ajax("../../RegiSter.ashx?Action=codes&&YZM=" + yzm.value, function (restext) {
            if (restext == "NO") {
                document.getElementById("unm2").innerHTML = "验证码不正确";
            } else if (restext == "OK") {
                document.getElementById("unm2").innerHTML = "";
            } else {
                document.getElementById("unm2").innerHTML = "Unexpected situation";
            }
        });
    }
}

function SignIn() {
    var unm = document.getElementById("unm").innerHTML;
    var unm1 = document.getElementById("unm1").innerHTML;
    var unm2 = document.getElementById("unm2").innerHTML;
    var s1 = document.getElementById("submit");
    if (unm === "OK" && unm1 == "密码格式正确" && unm2 === "") {
        s1.type = "submit";
    }
    else
    {
        alert("输入有误请重新输入");
    }
}

function Go1() {
    var account = document.getElementById("Account");
    account.focus();
    var h = document.getElementById("h");
    if (h.value == 2) {
        alert("账号有误");
    }
    else if (h.value == 3) {
        alert("密码有误");
    }
    else if (h.value == 4) {
        alert("密有误");
    }
}

function checkUser() {
    var userName = document.getElementById("Account").value;
    var password = document.getElementById("Password").value;
    ajax("Login.ashx?UserName=" + userName + "&&Password=" + password, function (restext) {
        if (restext == "OK") {
            window.location.href = "lyb.ashx?action=login";
        }
        else if (restext == "NO") {
            alert("登录失败");
        }
        else if (restext == "error") {
            alert("登录出现异常");
        }
    });
}


function lista(k) {
    var ule = document.getElementById("ul1");
    ule.innerHTML = "";
    ajax("lybAjax.ashx?action=lia", function (restext) {
        var index = restext;
        document.getElementById("pages").innerHTML = restext;
        document.getElementById("page").innerHTML = k;
        if (k <3) {
            for (var i = 1; i <= index; i++) {
                if (i <= 5) {
                    $("#ul1").append("   <li><a onclick='page(" + i + ")'>" + i + "</a></li>");
                } else {
                    break;
                }
            }
        }else if(k>=3)
        {
            var j = 0;
                for (var i = k - 2; i <= index; i++) {
                    if (j < 5) {
                        $("#ul1").append("   <li><a onclick='page(" + i + ")'>" + i + "</a></li>");
                        j++;
                    } else {
                        break;
                    }
                
            }
        }
    });
}

function page(j) {
    var i = j;
    ajax("lybAjax.ashx?action=page&&index=" + i, function (restext) {
        var comments = JSON.parse(restext);
        var ulComments = document.getElementById("txt");
        ulComments.innerHTML = "";//先清除上次加载的内容
        for (var i = 0; i < comments.length; i++) {
            if (comments[i] != null) {
                var comment = comments[i];
                if (comment.img != null) {
                   
                    $("#txt").append("<div class='container' style='opacity:1'><div class='panel panel-primary'><div class='panel-heading'><h3 class='panel-title'>" + comment.Title + "</h3></div><div class='panel-body'><div style='overflow: hidden';text-overflow: 'ellipsis';word-break:'break-all';><p>" + comment.msg + "</p><img src=" + comment.img + " width='250px' height='150px'/></div><time class='pull-right'>" + comment.Time + "</time><br><p class='pull-right'>" + comment.name + "</p></div></div></div>");
                    
                } else {
                    $("#txt").append("<div class='container' style='opacity:1'><div class='panel panel-primary'><div class='panel-heading'><h3 class='panel-title'>" + comment.Title + "</h3></div><div class='panel-body'><div style='overflow: hidden';text-overflow: 'ellipsis';word-break:'break-all';><p>" + comment.msg + "</p></div><time class='pull-right'>" + comment.Time + "</time><br><p class='pull-right'>" + comment.name + "</p></div></div></div>");
                }
            }
        }
    });
    lista(j);
}

function Skip() {
    var index = document.getElementById("txtskip").value;
    page(index);
}

function Again() {
    var index = document.getElementById("page").innerHTML;
    page(index);
}