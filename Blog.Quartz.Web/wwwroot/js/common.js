
/**
 * 获取url参数
 * @param  key 
 */
function getSearchString(key) {
	var urlParamater = window.location.search;
	urlParamater = urlParamater.substring(1, urlParamater.length); // 获取URL中?之后的字符（去掉第一位的问号）
	// 以&分隔字符串，获得类似name=xiaoli这样的元素数组
	var arr = urlParamater.split("&");
	var obj = new Object();
	if (arr.length == 0)
		return '';
	// 将每一个数组元素以=分隔并赋给obj对象 
	for (var i = 0; i < arr.length; i++) {
		var tmp_arr = arr[i].split("=");
		obj[decodeURIComponent(tmp_arr[0])] = decodeURIComponent(tmp_arr[1]);
	}
	return obj[key];
}
/**
 * 获取resultful格式参数
 * @param  key 
 */
function getSearchStringResultFul(key) {
	var url = window.location.href;
	var index = url.lastIndexOf('/')
	return url.substring(index + 1);
}
/**
* 全局ajax处理
*/
layui.use('layer', function () {
	var layer = layui.layer;
	$.ajaxSetup({
		cache: false,		
		error: function (request) {
			layer.msg('响应服务器失败', {icon: 2});
		},
	});
})