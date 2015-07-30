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
	var w=window.innerWidth	|| document.documentElement.clientWidth	|| document.body.clientWidth;
	var h=window.innerHeight|| document.documentElement.clientHeight|| document.body.clientHeight;
	var ffo=$('header div.t .ffo');
	var infoBtn=$('.dGrade img');
	var z_Tariff=$('.Tariff');   //******有调整
	var zKown=$('.z_FBox li:nth-of-type(3) a'); //******有调整
	var z_FBox=$('.z_FBox'); //******有调整
    z_uW.css('width',(0.9*w-2)+'px');
	disUL.css('width',(0.86*w-46)/3+'px');
	z_FBox.css('margin-left',-0.45*w+'px'); //******有调整 
	/*2015-7-3*/
	//点击上传
	$('.dUP').on('click',function(){
		window.location.href = "/app/L_mask";		
	})
	
	//点击确定--浮层隐藏
	/*$('aside .l_btn a.l_sure').on('click',function(){
		$('.lwj0').css('display','none');
		fMask.css('display','none');
	})*/
	/*end-2015-7-3*/
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
		z_FBox.css('display','none');
	})
	var dSumA=$('.dSummit a');
	var z_sucBox=$('.z_suc');
	fBoxShowHide(dSumA,z_sucBox,zKown,fMask);
	//alert(z_sucBox);
	/*--------2015-7-20-----------*/
	var myTags=$('.myTags'); //我的标签浮框
	var TagsBtn=$('.dMyTags'); //我的标签按钮
	var TagsBtn1=$('.dMyTags1'); //我的标签按钮
	var tagSpan=$('.myTags li:nth-of-type(2) div.tags span');//我的标签下的9个标签
	var clo=$('.z_FBox li:nth-of-type(1) img');
	var zEnter=$('.myTags li:nth-of-type(3) a'); //确定
	fBoxShowHide(TagsBtn,myTags,clo,fMask);
	myTags.css('margin-top',-208+'px');
	z_Tariff.css('margin-top',-148+'px');
	mR1(tagSpan,3,'5%');
	tagSpan.attr('flag1',0);
	
	
	//标签被选状态同步
	function  seleing(){
		var arr1=[];
		var str='';
		arr1=[];
		str='';
		for(var i=0;i<$('.myTags li:nth-of-type(2) div.tags span.active').length;i++){
			arr1.push($('.myTags li:nth-of-type(2) div.tags span.active').eq(i).text());
			}
			for(var i=0;i<arr1.length;i++){
					str+="<span class='c active iline _fl'><img src='/Hmobile/img/z_clo2.png' />"+arr1[i]+"</span>"
			}
			
			if(arr1.length<3){
				TagsBtn1.html(
				"<span class='iline _fl'>我的标签:</span>"+str+"<span class='c add iline _fl' style='width: 55.4px; margin-right: 0px;'>+</span>"
				);
			}else{
				TagsBtn1.html(
				"<span class='iline _fl'>我的标签:</span>"+str+""
				);
			}
			//选择标签后----.dMyTags1 span
			$('.dMyTags1 span.c').css('width',(0.76*w-77)/3+'px');
			$('.dMyTags1 span.c:nth-last-child(1)').css('margin-right',0);
			var addBtn=$('.dMyTags1 span.c.add');
			fBoxShowHide(addBtn,myTags,clo,fMask);			
			
			var DelTag = $('.dMyTags1 span.c.active img');

	    //点击生成的标签后的叉号----需要删除当前标签+重置对应flag1+删除对应选择面版上的标签（3步）

			DelTag.on('click',function(){
				$(this).parent().remove();
				var spanActi=$('.myTags li:nth-of-type(2) div.tags span.active');
				for(var i=0;i<spanActi.length;i++){
					if($(this).parent().text()==spanActi.eq(i).text()){
						spanActi.eq(i).removeClass('active');
						spanActi.eq(i).attr('flag1',0);
					}
				}
				seleing();				
		})
	}
	//点击确定
	zEnter.on('click',function(){
			if(tagSpan.hasClass('active')){
				seleing();
				TagsBtn.css('display','none');
				TagsBtn1.css('display','block');
				fMask.css('display','none');
				myTags.css('display','none');
			
			}else{
				fMask.css('display','none');
				myTags.css('display','none');
				TagsBtn.css('display','block');
				TagsBtn1.css('display','none');
			}
	
	})
	tagSpan.on('click',function(){
			if($(this).attr('flag1')==0 && $('.myTags li:nth-of-type(2) div.tags span.active').length<4){
				$(this).addClass('active');
				$(this).attr('flag1',1);
			}else{
				$(this).removeClass('active');
				$(this).attr('flag1',0);
			}
			if( $('.myTags li:nth-of-type(2) div.tags span.active').length==4){
				$(this).removeClass('active');
				$(this).attr('flag1',0);
				alert('亲,最多选3个哦~!~');
			}
		})
	
	
	/*--------end 2015-7-21-----------*/
	
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
 
//------------------2015-7-20新加
//******浮层显示-隐藏方法
	function fBoxShowHide(btn1,box1,clo1,fMask){
		//	alert(btn1);
		btn1.on('click',function(){
			fMask.css('display','block');
			box1.css('display','block');
		})
		clo1.on('click',function(){
			fMask.css('display','none');
			box1.css('display','none');
		})
	}
//*****完善列表项的第三项margin-right:0px;方法
function mR1(obj,n,num){
 		obj.each(function(i, ele) {   			
        	if ((i + 1) % n == 0) {
                $(ele).css("margin-right", "0px");
       		 } else {
        		$(ele).css("margin-right",num);
          	}
    	});
		}	