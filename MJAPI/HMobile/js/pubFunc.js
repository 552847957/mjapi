//-----------------加载之外----------------
//搜索栏或者输入框点入的效果函数
function searchFn(obj,class1,classD) {
					var val = obj.val();
					obj.addClass(class1);
					obj.on('focus', function() {
						if ($(this).val() == val) {
							$(this).val('');
							$(this).removeClass(class1);
						}
						$(this).addClass(classD);
					})
					obj.on('blur', function() {
						if ($(this).val() == '') {
							$(this).val(val);
							$(this).removeClass(classD);
						}
						$(this).addClass(class1);
					})
}

//针对底部条兼容问题-调改
function dBott(obj){
	var h=window.innerHeight|| document.documentElement.clientHeight|| document.body.clientHeight;
	if(h>490){
		obj.css('position','absolute');
	}else{
		obj.css('position','relative');
	}
}


//针对需求预约中的下一步定位
function dzStep(obj){
	var h=window.innerHeight|| document.documentElement.clientHeight|| document.body.clientHeight;
	if(h>540){
		obj.css('position','absolute');
		obj.css('bottom',36+'px');
	}else{
		obj.css('position','relative');
		obj.css('bottom',0+'px');
	}
}