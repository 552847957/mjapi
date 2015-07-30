$(function(){
	l_ready();
	window.onresize=l_ready;
	function l_ready(){
		var secW=$('section').width();
		var secPW=$('section p').width();
		var secLiW=$('section li').width();
		$('section').height(secW);
		$('section p').height(secPW);
		$('section p img').height(secPW);
		$('section li').height(secLiW);
		$('section li a').height(secLiW);
		$('section p').css('margin-top',(secW-secPW)/2)
    }
	/*setDate();
	setInterval(setDate,1000);
	function setDate(){
		var d = new Date();
		var m=d.getMinutes();
		if(m<10){m="0"+m}
    	var str = (d.getMonth()+1)+"月"+d.getDate()+"日"+d.getHours()+":"+m;
    	$('header p').html(str);
   }*/
})
