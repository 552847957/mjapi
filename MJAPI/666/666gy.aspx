<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="666gy.aspx.cs" Inherits="MJAPI._666._666gy" %>

<!doctype html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=0.1,maximum-scale=1">
    <title>666活动</title>
    <link type="text/css" href="666.css" rel="stylesheet">
    <script type="text/javascript" src='jquery-1.8.3.min.js'></script>
    <script type="text/javascript" charset="utf-8">
        $(function () {
            var join = $('.bottom .t');
            var login = $('.login');
            var clo = $('.login .cur_hover');
            var Inp0 = $('.login .t　input');
            var Inp1 = $('.login .th　.oo');
            join.on('click', function () {
                login.css('display', 'block');
                $('#fMask').css('display', 'block');
            })
            clo.on('click', function () {
                login.css('display', 'none');
                $('#fMask').css('display', 'none');
            })

            /*searchFn(Inp0);
            searchFn(Inp1);*/
        })

    </script>

</head>
<body>
    <div id='content'>
        <img src='img/666gy.png' class='img' />
        <div class='bottom'>
            <a href="tel:400-102-3100">
                <img src="img/phone.png" class="o" /></a>
            <img src="img/join.png" class="t" />
        </div>
        <!--登录入口-->
        <div class="login">
            <img src="img/clo.png" class="cur_hover" />
            <ul class="bsizep">
                <li class='o'>免费预约上门量房</li>
                <li class='t'>
                    <input type="text" id="name" placeholder='请留下您的称呼' /></li>
                <li class='t'>
                    <input type="text" id="phone" placeholder='请输入手机号' /></li>
                <li class='th'>
                    <input type="text" id="code" placeholder='请输入验证码' class='oo' /><a id="getcode" class='tt iline'>获取验证码</a></li>

            </ul>
            <div class='fo'><a id="yy" class="iline">马上预约</a></div>
        </div>
    </div>

    <div id="fMask"></div>
</body>
</html>
<script>

    function setCookie(name, value, time) {
        var strsec = getsec(time);
        var exp = new Date();
        exp.setTime(exp.getTime() + strsec * 1);
        document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
    }
    function getsec(str) {

        var str1 = str.substring(1, str.length) * 1;
        var str2 = str.substring(0, 1);
        if (str2 == "s") {
            return str1 * 1000;
        }
        else if (str2 == "h") {
            return str1 * 60 * 60 * 1000;
        }
        else if (str2 == "d") {
            return str1 * 24 * 60 * 60 * 1000;
        }
    }
    function getCookie(name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

        if (arr = document.cookie.match(reg))

            return unescape(arr[2]);
        else
            return null;
    }

    (function () {
        $(function () {
            $("#yy").click(function () {

                $.post("/HD/add2", { name:"贵阳-微信-"+ name, phone: phone, code: code }, function (data) {

                });

                var name = $("#name").val();
                var phone = $("#phone").val();
                var code = $("#code").val();
                if ($.trim(name).length == 0) {
                    alert("请输入正确的称呼");
                    return;
                }
                if (!(/\d{11}/g.test(phone))) {
                    alert("请输入正确的手机号");
                    return;
                }
                if (!(/\d{6}/g.test(code))) {
                    alert("请输入正确的验证码");
                    return;
                }
                $.post("/HD/add", { name: name, phone: phone, code: code }, function (data) {
                    alert(data);
                });
            });
        });


        $(function () {
            $("#getcode").click(function () {
                var phone = $("#phone").val();

                if (getCookie('t')) { $("#getcode").html("发送太频繁了"); return false; } else {

                    $.post("/HD/SendMsg", { phone: phone }, function (data) {
                        setCookie("t", "60", "s60");
                        var num = 60;
                        var timer = setInterval(function () {
                            num -= 1;
                            if (num == 0) {
                                clearInterval(timer);
                                $("#getcode").html("获取验证码");
                            } else {
                                $("#getcode").html(num);
                            }
                        }, 1000);
                    });

                }


            });
        });
    })()
</script>
