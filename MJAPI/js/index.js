$(function(){
  load1();
})
window.onresize=load1;
function load1(){
	var btn1=$('.dGrade li:nth-of-type(1)');
	var btn2=$('.dGrade li:nth-of-type(2)');
	var btn3=$('.dGrade li:nth-of-type(3)');
	var disUL=$('.district ul li');  //地区项
	var duF=$('.z_sel .uF');  //所选城市
	var distr=$('.district ul');
	var z_uW=$('.z_sel .z_unit');
	var z_sel=$('.zSel');
	var z_unit=$('.z_sel .z_unit div');
	var hDis=$('header div.t');
	var disArea=$('.z_dis');
	var fMask=$('#fMask');
	var fMask1=$('#fMask1');//针对头部显示下移的浮层
	var hCity=$('header div.t span.tt');  //头部city
	var hArea=$('header div.t span.tth'); //头部地区
	var duF=$('.z_sel .uF');  //所选城市
	var w =document.documentElement.clientWidth;
	var h =document.documentElement.clientHeight;
	var ffo=$('header div.t .ffo');
	var infoBtn=$('.dGrade img');
	var z_Tariff=$('.z_Tariff');
	var zKown=$('.z_Tariff li:nth-of-type(3) a')
    z_uW.css('width',(0.9*w-2)+'px');
	disUL.css('width',(0.86*w-46)/3+'px');
	z_Tariff.css('margin-left',-0.45*w+'px');
	//点击上传
	//$('.dUP').on('click',function(){
	//	$('.lwj0').css('display','block');
	//	fMask.css('display','block');
	//})
	////点击确定--浮层隐藏
	//$('aside .l_btn a.l_sure').on('click',function(){
	//	$('.lwj0').css('display','none');
	//	fMask.css('display','none');
	//})
	
	/*完善1*/
	distr.each(function(ind,ele){
		mR0($(ele).find('li'));
	})
	
	tabBtn(btn1);
	tabBtn(btn2);
	tabBtn(btn3);
	cityOne();
	select(z_sel,z_unit);
	//资费等级说明按钮点击
	infoBtn.on('click',function(){
		z_Tariff.css('display','block');
		fMask.css('display','block');
	})
	zKown.on('click',function(){
		fMask.css('display','none');
		z_Tariff.css('display','none');
	})
	//头部地区点击
	hDis.on('click',function(){
		if(flaga==0){
		disArea.css('display','block');
		fMask1.css('display','block');
		ffo.css('-webkit-transform','rotate(180deg)');
		ffo.css('-moz-transform','rotate(180deg)');
		ffo.css('-ms-transform','rotate(180deg)');
		ffo.css('transform','rotate(180deg)');
		 flaga=1;
		}else{
			flaga=0;
			ffo.css('-webkit-transform','rotate(0deg)');
			ffo.css('-moz-transform','rotate(0deg)');
			ffo.css('-ms-transform','rotate(0deg)');
			ffo.css('transform','rotate(0deg)');
			disArea.css('display','none');
			fMask1.css('display','none');
		}
	})
	
	disUL.on('click',function(){
		flaga=0;
		ffo.css('-webkit-transform','rotate(0deg)');
		ffo.css('-moz-transform','rotate(0deg)');
		ffo.css('-ms-transform','rotate(0deg)');
		ffo.css('transform','rotate(0deg)');
		hArea.text($(this).text());
		hCity.text(duF.text());		
		disArea.slideUp();
		fMask1.hide();
	})
//针对底部条兼容问题-调改
	if(h>490){
		$('.dBott').css('position','absolute');
	}else{
		$('.dBott').css('position','relative');
	}

}

//列表项的第三项margin-right:0px;
		function mR0(obj){
 		obj.each(function(i, ele) {   			
        	if ((i + 1) % 3 == 0) {
                $(ele).css("margin-right", "0px");
       		 } else {
        		$(ele).css("margin-right", "20px");
          	}
    	});
		}
//等级选择	
	function tabBtn(obj){
		obj.on('click',function(){
			$(this).siblings().removeClass('dActive');
			$(this).addClass('dActive');
			$(this).siblings().find('.t').css('display','none');
			$(this).siblings().find('.th').css('display','none');
			$(this).find('.t').css('display','block');
			$(this).find('.th').css('display','block');
		})
	}
/*----切换城市-----*/	
	var flag=0;  /*控制显示隐藏的标识*/
	var flaga=0;
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
			//$(this).parent().parent().parent().next('.Consumption').find('.un2').text($(this).text());
			cityOne();
		})
	})
	}
 //城市选择
 function  cityOne(){ 	
 			switch ($('.z_sel .uF').text()){
				case "北京":
				$('.district ul').css('display','none');
				$('.district ul').eq(0).css('display','block');
				break;
				case "天津":
				$('.district ul').css('display','none');
				$('.district ul').eq(1).css('display','block');
				break;
				case "贵阳":
				$('.district ul').css('display','none');
				$('.district ul').eq(2).css('display','block');
				break;
				default:
				break;
			}
}