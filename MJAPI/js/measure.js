$(function(){
	//下载app按钮划入划出状态
	$('.lamp a').on('mouseover',function(){
		$(this).css('background','url(/img/measure/btnHover.png) no-repeat');
	})
	$('.lamp a').on('mouseout',function(){
		$(this).css('background','url(/img/measure/btn.png) no-repeat');
	})
	//点击下载app按钮出现遮罩
	$('.lamp a').on('click',function(){	
		$('.bannerMask').show();
		$('.banner').addClass('modal-active');
	})
	//点击遮罩上按钮遮罩消失
	$('.bannerMask a').on('click',function(){
		$('.bannerMask').hide();
		$('.banner').removeClass('modal-active');
	})
	//管理
	$('.navCon ._fr a').toggle(function(){
		$(this).text('完成');
		$('.itemBtn').hide();
		$('.deleteBtn').show();
	},function(){
		$(this).text('管理');
		$('.itemBtn').show();
		$('.deleteBtn').hide();
	})

})
