$(function(){
	var winH=$(window).height();
	var ulH=winH-46;
	var divH=ulH/4-30;
	$('section ul').css('height',ulH);
	$('.l_process').css('height',divH);
	$('section ul li h2').css('height',divH);
	window.onresize=function(){
		var winH=$(window).height();
		var ulH=winH-46;
		var divH=ulH/4-30;
	    $('section ul').css('height',ulH);
	    $('.l_process').css('height',divH);
	    $('section ul li h2').css('height',divH);
	}
	$('.l_modi').on('click',function(){
	    $('#fMask').show();
	    alert($('#fMask'));
		$('.z_FBox').show();
	})
	$('.editClo').on('click',function(){
	    $('.z_FBox').hide();

		$('#fMask').hide();
	})
})
$(function(){
						select($('.zSel'),$('.z_sel .z_unit div'));
					})
					var flag=0;
					function select(obtn1,obtn2){
					obtn1.on('click',function(){
					if(flag==0){
						flag=1;
						$(this).next('.z_unit').css('display','block');//让地区列表展开	
					}else{
						flag=0;
						$(this).next('.z_unit').css('display','none');//让地区列表展开	
					}
					})
					/*下拉菜单的点击*/
				    obtn2.each(function(ind,ele){
					$(ele).on('click',function(){
						flag=0;
						$(this).parent().parent().find('a').text($(this).text());//给标题父项赋值		
						$(this).parent().css('display','none'); //让下拉菜单消失
					})
				})
				}