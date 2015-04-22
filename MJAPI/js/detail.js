$(function(){
	//信息修改及保存
	$('.editBtn a').on('click',function(){
		$('.editBtn').hide();
		$('.saveBtn').show();
		$('.type input').removeClass('noBorder');
		$('.type input').removeAttr('readonly','readonly');
		$('.type textarea').removeClass('noBorder');
		$('.type textarea').removeAttr('readonly','readonly');
	})
	$('.saveBtn a').on('click',function(){
		$('.saveBtn').hide();
		$('.editBtn').show();
		$('.type input').addClass('noBorder');
		$('.type input').attr('readonly','readonly');
		$('.type textarea').addClass('noBorder');
		$('.type textarea').attr('readonly','readonly');
	})
	/*页面底部logo划过效果*/
	$('.l_gklogo').on('mouseover',function(){
		$(this).children('img').attr('src','img/footer/l_gklogob.png');
	})
	$('.l_gklogo').on('mouseout',function(){
		$(this).children('img').attr('src','img/footer/l_gklogo.png');
	})
})
