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
})
