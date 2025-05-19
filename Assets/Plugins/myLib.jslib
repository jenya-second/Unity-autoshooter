mergeInto(LibraryManager.library, {
	
	SaveExtern : function (data){
		var dataString = UTF8ToString(data);
		var myobj = JSON.parse(dataString);
		console.log(myobj);
		player.setData(myobj);
	},

	LoadExtern : function (){
		player.getData().then(_data => {
			const myjson = JSON.stringify(_data);
			console.log(myjson);
			myGameInstance.SendMessage('Progress', 'SetPlayerInfo', myjson);
			var desk = ysdk.deviceInfo.isDesktop() ? 0 : 1;
			myGameInstance.SendMessage('Progress', 'SetSystem', desk);
		});
	},
  
	ShowAddExtern: function (){
		ysdk.adv.showFullscreenAdv({
			callbacks: {
				onOpen: () => {
				  console.log('Simple ad open.');
				},
				onClose: () => {
				  console.log('Simple ad closed.');
				  myGameInstance.SendMessage('Progress', 'OnCloseSimpleAdv');
				},
			}
		});
	},
  
	ShowRewardedVideoExtern : function () {
		ysdk.adv.showRewardedVideo({
			callbacks: {
				onOpen: () => {
				  console.log('Video ad open.');
				},
				onRewarded: () => {
				  console.log('Rewarded!');
				  myGameInstance.SendMessage('Progress', 'RewardRecieved');
				},
				onClose: () => {
				  console.log('Video ad closed.');
				  myGameInstance.SendMessage('Progress', 'OnCloseRewardVideo');
				},
				onError: (e) => {
				  console.log('Error while open video ad:', e);
				  myGameInstance.SendMessage('Progress', 'OnCloseRewardVideo');
				}
			}
		})
	},
	
	GetTime : function(){
		var time = ysdk.serverTime();
		myGameInstance.SendMessage('Progress', 'SetTime', time);
	},
	
	GRAReady : function() {
		ysdk.features.LoadingAPI.ready()
	}
});