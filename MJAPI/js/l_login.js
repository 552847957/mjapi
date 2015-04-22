$(function(){
	//设置页面为浏览器全屏高度
	var conH=$(window).height();
	$('.l_container').css('height',conH+'px');
	window.onresize=function(){
		var conH=$(window).height();
	    $('.l_container').css('height',conH+'px');
	}
	//弹窗上的复选框样式
	$('.l_whiteBox .fou img').toggle(function(){
		$(this).attr('src','/img/login/check.png');
	},function(){
		$(this).attr('src','/img/login/checkno.png');
	})
	//点击登录出现遮罩与弹窗
	$('.l_loginBtn').on('click',function(){
		$('#fMask').show();		
		$('.l_whiteBox').show();
		$('body').addClass('modal-active');
	})
	//点击弹窗上的X遮罩与弹窗消失
	$('.l_whiteBox .one h2 img').on('click',function(){
		$('.l_whiteBox').hide();
		$('#fMask').hide();
		$('body').removeClass('modal-active');
	})
	//弹窗关闭按钮hover状态
	$('.l_whiteBox .one h2 img').on('mouseover',function(){
		$(this).attr('src','/img/login/cloh.png');
	})
	$('.l_whiteBox .one h2 img').on('mouseout',function(){
		$(this).attr('src','/img/login/clo.png');
	})
})
