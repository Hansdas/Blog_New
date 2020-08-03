define(['jquery'], function ($) {
	/**
 * iframe自适应高度
 */
	function setIframeHeight(iframeId) {
		$('#' + iframeId).each(function (index) {
			var that = $(this);
			(function () {
				setInterval(function () {
					//setIframeHeight(that[0]);
					if (that[0]) {
						var iframeWin = that[0].contentWindow || that[0].contentDocument.parentWindow;
						if (iframeWin.document.body) {
							that[0].height = iframeWin.document.body.scrollHeight;
						}
					}
				}, 300);
			})(that);
		});
	};
	return {
		setIframeHeight: setIframeHeight
	}
});


