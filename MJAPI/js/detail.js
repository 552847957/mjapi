$(function () {
    //信息修改及保存
    $('.editBtn a').on('click', function () {
        $('.editBtn').hide();
        $('.saveBtn').show();
        $('.type input').removeClass('noBorder');
        $('.type input').removeAttr('readonly', 'readonly');
        $('.type textarea').removeClass('noBorder');
        $('.type textarea').removeAttr('readonly', 'readonly');
    })
    $('.saveBtn a').on('click', function () {
        $('.saveBtn').hide();
        $('.editBtn').show();
        $('.type input').addClass('noBorder');
        $('.type input').attr('readonly', 'readonly');
        $('.type textarea').addClass('noBorder');
        $('.type textarea').attr('readonly', 'readonly');
    })
    /*页面底部logo划过效果*/
    $('.l_gklogo').on('mouseover', function () {
        $(this).children('img').attr('src', '/img/footer/l_gklogob.png');
    })
    $('.l_gklogo').on('mouseout', function () {
        $(this).children('img').attr('src', '/img/footer/l_gklogo.png');
    })
})

/*zxl*/
$(function () {
    function load0() {
        h = $(window).height();
        w = $(window).width();
        $('.zLayer .zo').css('width', (w - 400) + 'px');
        $('.zLayer .zo').css('max-height', (h - 2) + 'px');
        $('.zLayer .zo img').css('height', (h - 20) + 'px');
        $('.zLayer .zo img').css('max-height', (h - 20) + 'px');
        $(' .zLayer .aUl').css('height', (h - 2) + 'px');
        $(' .zLayer .aUl').css('max-height', (h - 2) + 'px');
        $('.zLayer ul.littImg').css('height', (h - 120) + 'px');
        $('.zLayer ul.littImg li').css('height', (0.25 * (h - 120) - 10) + 'px');
    }
    load0();
    window.onresize = load0;
    //划过浮层消失
    $('.zLayer ul.littImg li div').on('mouseover', function () {
        $(this).find('.ms').hide();
    })
    $('.zLayer ul.littImg li div').on('mouseout', function () {
        $(this).find('.ms').show();
    })
    //tab选项卡
    var liD = $('.zLayer ul.littImg li div.iline');
    var imgBig = $('.zLayer .zo img');
    var preBtn = $('.zLayer a.pre');
    var nexBtn = $('.zLayer a.nex');

    liD.each(function (ind, ele) {
        $(ele).on('click', function () {
            liQ($(ele));
        })
    })
    var num = 0;

    function liQ(obj) {
        var ix = obj.find('img').attr('inx');
        liD.eq(0).find('span').css('background', 'url(/img/detail/zIcon.png) no-repeat -43px -7px');
        liD.eq(1).find('span').css('background', 'url(/img/detail/zIcon.png) no-repeat -164px -7px');
        liD.eq(2).find('span').css('background', 'url(/img/detail/zIcon.png) no-repeat -84px -7px');
        liD.eq(3).find('span').css('background', 'url(/img/detail/zIcon.png) no-repeat -124px -7px');
        liD.removeClass('active');
        console.log(num)
        obj.addClass('active');
        var org = obj.find('img').attr('org'); //获取标签上自定义属性的路径			
        imgBig.attr('src', org); //为大图赋值路径	
        imgBig.attr('inx', ix);
        var i = imgBig.attr('inx');
        num = i;
        if (liD.eq(0).hasClass('active')) {
            obj.find('span').css('background', 'url(/img/detail/zIcon.png) no-repeat -43px -52px');
        }
        if (liD.eq(1).hasClass('active')) {
            obj.find('span').css('background', 'url(/img/detail/zIcon.png) no-repeat -164px -52px');
        }
        if (liD.eq(2).hasClass('active')) {
            obj.find('span').css('background', 'url(/img/detail/zIcon.png) no-repeat -84px -52px');
        }
        if (liD.eq(3).hasClass('active')) {
            obj.find('span').css('background', 'url(/img/detail/zIcon.png) no-repeat -124px -52px');
        }

    }
    //左右切换
    preBtn.on('click', function () {
        num--;
        liQ(liD.eq(num));
    })
    nexBtn.on('click', function () {
        num++;
        if (num == 4) {
            num = 0;
        }
        liQ(liD.eq(num));
    })
    //zxl-点击方位图位置--浮层显示与消失
    $('.pic a').on('click', function () {


        //初始化 div
        var str = $(this).attr("urls").split(',');

        var obj = { up: '/img/detail/z1.png', down: '/img/detail/z2.png', left: '/img/detail/z3.png', right: '/img/detail/z4.png' };
        for (var i = 0; i < str.length; i++) {
            if (str.indexOf("up")) {
                obj.up=str[i];
            }
            if (str.indexOf("down")) {
                obj.down = str[i];
            }
            if (str.indexOf("left")) {
                obj.left = str[i];
            }
            if (str.indexOf("right")) {
                obj.right = str[i];
            }
        }
        
        var $big = $("#dp_big").attr("src", obj.up);
        var $up = $("#dp_up").attr("src", obj.up).attr("org", obj.up);
        var $down = $("#dp_down").attr("src", obj.down).attr("org", obj.down);;
        var $left = $("#dp_left").attr("src", obj.left).attr("org", obj.left);;
        var $right = $("#dp_right").attr("src", obj.right).attr("org", obj.right);;
        $('.zLayer').css('display', 'block');
    })
    $('.zLayer .topTit a').on('click', function () {



        $('.zLayer').css('display', 'none');
    })


    $(".des").click(function () {

        //初始化 div
        var str = $(this).attr("urls").split(',');

        var $a = $(".dp_bc");


        var obj = { up: '/img/detail/xl.jpg', down: '/img/detail/xl.jpg', left: '/img/detail/xl.jpg', right: '/img/detail/xl.jpg' };
        for (var i = 0; i < str.length; i++) {
            if (str[i].indexOf("up") > 0) {
                obj.up = str[i];
                $($a[0]).attr("href", "/LF/GetFile?url="+str[i]);
            }
            if (str[i].indexOf("down") > 0) {
                obj.down = str[i];
                $($a[1]).attr("href", "/LF/GetFile?url=" + str[i]);
            }
            if (str[i].indexOf("left") > 0) {
                obj.left = str[i];
                $($a[2]).attr("href", "/LF/GetFile?url=" + str[i]);
            }
            if (str[i].indexOf("right") > 0) {
                obj.right = str[i];
                $($a[3]).attr("href", "/LF/GetFile?url=" + str[i]);
            }
        }

        var $big = $("#dp_big").attr("src", obj.up);
        var $up = $("#dp_up").attr("src", obj.up).attr("org", obj.up);
        var $down = $("#dp_down").attr("src", obj.down).attr("org", obj.down);;
        var $left = $("#dp_left").attr("src", obj.left).attr("org", obj.left);;
        var $right = $("#dp_right").attr("src", obj.right).attr("org", obj.right);;

        $('.zLayer').css('display', 'block');
    });
})