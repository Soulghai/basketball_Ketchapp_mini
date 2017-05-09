using UnityEngine;
using System;
using DoozyUI;

public class ScreenCoins : MonoBehaviour {
	public static event Action<int> OnAddCoinsVisual;

	private bool _isShowBtnViveoAds = true;
	// Use this for initialization

	[HideInInspector] public string PrevScreenName;

	void Start () {
		DefsGame.screenCoins = this;
	}

	public void ShowButtons() {
		UIManager.ShowUiElement ("ScreenCoinsBtnBack");
		UIManager.ShowUiElement ("BtnTier1");
		UIManager.ShowUiElement ("BtnTier2");
		if (_isShowBtnViveoAds) {
			UIManager.ShowUiElement ("ScreenCoinsBtnVideo");
			FlurryEventsManager.SendEvent ("RV_strawberries_impression", "shop");
		}
		if (DefsGame.noAds < 1) UIManager.ShowUiElement ("ScreenCoinsBtnNoAds");
		#if UNITY_IPHONE
		UIManager.ShowUiElement ("ScreenCoinsBtnRestore");
		#endif
	}

	public void HideButtons() {
		UIManager.HideUiElement ("ScreenCoinsBtnBack");
		UIManager.HideUiElement ("BtnTier1");
		UIManager.HideUiElement ("BtnTier2");
		UIManager.HideUiElement ("ScreenCoinsBtnVideo");
		UIManager.HideUiElement ("ScreenCoinsBtnNoAds");
		UIManager.HideUiElement ("ScreenCoinsBtnRestore");
	}

	void Awake() {
		Invoke("InitialRvButtonUpdate", 0.5f);
		//PublishingService.Instance.OnRewardedVideoReadyChanged += IsVideoAdsAvailable;
	}


	void OnDestroy() {
		//PublishingService.Instance.OnRewardedVideoReadyChanged -= IsVideoAdsAvailable;
	}

	private void InitialRvButtonUpdate()
	{
		//IsVideoAdsAvailable(PublishingService.Instance.IsRewardedVideoReady());
	}

	private void IsVideoAdsAvailable(bool flag) {
		_isShowBtnViveoAds = flag;
		if (flag) {
			if (DefsGame.currentScreen == DefsGame.SCREEN_IAPS) {
				UIManager.ShowUiElement ("ScreenCoinsBtnVideo");
				FlurryEventsManager.SendEvent ("RV_strawberries_impression", "shop");
			}
		} else {
			UIManager.HideUiElement ("ScreenCoinsBtnVideo");
		}
	}
		
	public void BtnTier1() {
		//IAPManager.Instance.Buy50Coins ();
	}

	private void BoughtTier1() {
		GameEvents.Send (OnAddCoinsVisual, 200);
	}

	public void BtnTier2() {
		//IAPManager.Instance.Buy200Coins ();
	}

	private void BoughtTier2() {
		GameEvents.Send (OnAddCoinsVisual, 1000);
	}

	public void BtnTier3() {
		/*FlurryEventsManager.SendEvent ("RV_strawberries", "shop");
			
		if (!PublishingService.Instance.IsRewardedVideoReady())
		{
			NPBinding.UI.ShowAlertDialogWithSingleButton("Ads not available", "Check your Internet connection or try later!", "Ok", (string _buttonPressed) => {});
			return;
		}

		FlurryEventsManager.SendEndEvent ("iap_shop_length");

		//Defs.MuteSounds (true);
		PublishingService.Instance.ShowRewardedVideo(isSuccess => {
			if (isSuccess) {
				GameEvents.Send(OnAddCoinsVisual, 25);
				FlurryEventsManager.SendEvent ("RV_strawberries_complete", "shop");
			}
			//Defs.MuteSounds (false);
			FlurryEventsManager.SendStartEvent ("iap_shop_length");
		});*/
	}

	public void Show(string prevScreenName) {
		PrevScreenName = prevScreenName;
		FlurryEventsManager.SendStartEvent ("iap_shop_length");
		FlurryEventsManager.SendEvent ("iap_shop", PrevScreenName);

		DefsGame.currentScreen = DefsGame.SCREEN_IAPS;
		DefsGame.isCanPlay = false;
		ShowButtons ();
	}

	public void Hide() {
		FlurryEventsManager.SendEndEvent ("iap_shop_length");

		DefsGame.currentScreen = DefsGame.SCREEN_MENU;
		DefsGame.isCanPlay = true;
		HideButtons ();

		FlurryEventsManager.SendEvent ("iap_shop_home", PrevScreenName);
	}

}