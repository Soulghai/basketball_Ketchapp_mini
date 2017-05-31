using UnityEngine;
using UnityEngine.UI;
using DoozyUI;
using System;

public class ScreenMenu : MonoBehaviour {
	//public static event Action<int> OnAddCoins;
	public GameObject coin;
	public Text timeText;
	public UIButton videoAdsButton;
	public UIButton giftButton;
	public AudioClip sndBtnClick;

    private bool _isBtnSettingsClicked = false;
    private DateTime _giftNextDate;
    private bool _isWaitGiftTime;
    private bool _isShowBtnViveoAds = true;
    private bool _isButtonHiden;

	// Use this for initialization
	void Start () {
		//Grab the old time from the player prefs as a long
		string strTime = PlayerPrefs.GetString("dateGiftClicked");
		//_strTime = "";
		//DefsGame.BTN_GIFT_HIDE_DELAY_COUNTER = 0;
		if (strTime == "") {
			_giftNextDate = System.DateTime.UtcNow;
			DefsGame.BTN_GIFT_HIDE_DELAY = DefsGame.BTN_GIFT_HIDE_DELAY_ARR [DefsGame.BTN_GIFT_HIDE_DELAY_COUNTER];
			_giftNextDate = _giftNextDate.AddMinutes (DefsGame.BTN_GIFT_HIDE_DELAY);
			if (DefsGame.BTN_GIFT_HIDE_DELAY_COUNTER < DefsGame.BTN_GIFT_HIDE_DELAY_ARR.Length - 1) {
				++DefsGame.BTN_GIFT_HIDE_DELAY_COUNTER;
				PlayerPrefs.SetInt ("BTN_GIFT_HIDE_DELAY_COUNTER", DefsGame.BTN_GIFT_HIDE_DELAY_COUNTER);
			}
		} else {
			long _timeOld = Convert.ToInt64 (strTime);
			//Convert the old time from binary to a DataTime variable
			_giftNextDate = DateTime.FromBinary(_timeOld);
		}

		DateTime _currentDate = System.DateTime.UtcNow;
		TimeSpan _difference = _giftNextDate.Subtract (_currentDate);
		if (_difference.TotalSeconds <= 0f) {
			//timeText.enabled = false;
			_isWaitGiftTime = false;
			//giftButton.EnableButtonClicks();
		} else {
			//timeText.enabled = true;
			_isWaitGiftTime = true;
			//giftButton.DisableButtonClicks();
			timeText.text = _difference.Hours.ToString () + ":" + _difference.Minutes.ToString ();
		}

		showButtons ();
	}

	void OnEnable()
	{
		ScreenGame.OnShowMenu += ScreenGame_OnShowMenu;
		Ball.OnThrow += Ball_OnThrow;
	}

	void OnDisable()
	{
		ScreenGame.OnShowMenu -= ScreenGame_OnShowMenu;
		Ball.OnThrow -= Ball_OnThrow;
	}

	void ScreenGame_OnShowMenu ()
	{
		showButtons ();
	}

	void Ball_OnThrow() {
		hideButtons ();
	}

	void IsVideoAdsAvailable(bool _flag) {
		_isShowBtnViveoAds = _flag;
		if (_flag) {
			if (DefsGame.currentScreen == DefsGame.SCREEN_MENU) {
				UIManager.ShowUiElement ("BtnVideoAds");
				FlurryEventsManager.SendEvent ("RV_strawberries_impression", "start_screen");
			}
		} else {
			UIManager.HideUiElement ("BtnVideoAds");
		}
	}

	public void showButtons() {
		_isButtonHiden = false;

		FlurryEventsManager.SendStartEvent ("start_screen_length");

		//UIManager.ShowUiElement ("MainMenu");
		UIManager.ShowUiElement ("elementBestScore");
		UIManager.ShowUiElement ("elementCoins");
		UIManager.ShowUiElement ("BtnSkins");
		FlurryEventsManager.SendEvent ("candy_shop_impression");
		if (!_isWaitGiftTime) {
			UIManager.ShowUiElement ("BtnGift");
			FlurryEventsManager.SendEvent ("collect_prize_impression");
		}
		if (_isShowBtnViveoAds) {
			UIManager.ShowUiElement ("BtnVideoAds");
			FlurryEventsManager.SendEvent ("RV_strawberries_impression", "start_screen");
		}
		UIManager.ShowUiElement ("BtnMoreGames");
		UIManager.ShowUiElement ("BtnSound");
		UIManager.ShowUiElement ("BtnRate");
		FlurryEventsManager.SendEvent ("rate_us_impression", "start_screen");
		UIManager.ShowUiElement ("BtnLeaderboard");
		UIManager.ShowUiElement ("BtnAchievements");
		#if UNITY_ANDROID || UNITY_EDITOR
		UIManager.ShowUiElement ("BtnGameServices");
		#endif
		UIManager.ShowUiElement ("BtnMoreGames");
		UIManager.ShowUiElement ("BtnShare");
		UIManager.ShowUiElement ("BtnPlus");
		FlurryEventsManager.SendEvent ("iap_shop_impression");
		UIManager.HideUiElement ("scrMenuWowSlider");

		if (DefsGame.screenSkins) {
		if (DefsGame.screenSkins.CheckAvailableSkinBool()) UIManager.ShowUiElement ("BtnHaveNewSkin"); else
			UIManager.HideUiElement ("BtnHaveNewSkin");
		}

		_isBtnSettingsClicked = false;
	}

	public void hideButtons() {
		if (_isButtonHiden)
			return;

		_isButtonHiden = true;
		FlurryEventsManager.SendEndEvent ("start_screen_length");
		//UIManager.HideUiElement ("MainMenu");
		UIManager.HideUiElement ("elementBestScore");
		//UIManager.HideUiElement ("elementCoins");
		UIManager.HideUiElement ("BtnSkins");
		UIManager.HideUiElement ("BtnGift");
		UIManager.HideUiElement ("BtnVideoAds");
		UIManager.HideUiElement ("BtnAchievements");
		UIManager.HideUiElement ("BtnMoreGames");
		UIManager.HideUiElement ("BtnSound");
		UIManager.HideUiElement ("BtnRate");
		UIManager.HideUiElement ("BtnLeaderboard");
		UIManager.HideUiElement ("BtnShare");
		UIManager.HideUiElement ("BtnSound");
		UIManager.HideUiElement ("BtnPlus");
		UIManager.HideUiElement ("BtnGameServices");

		UIManager.HideUiElement ("BtnHaveNewSkin");
	}

	public void BtnSettingsClick() {
		_isBtnSettingsClicked = !_isBtnSettingsClicked;

		if (_isBtnSettingsClicked){
			UIManager.ShowUiElement ("BtnSound");
			UIManager.ShowUiElement ("BtnInaps");
			UIManager.ShowUiElement ("BtnGameServices");
		} else {
			UIManager.HideUiElement ("BtnSound");
			UIManager.HideUiElement ("BtnInaps");
			UIManager.HideUiElement ("BtnGameServices");
		}
	}

	public void add10Coins() {
		FlurryEventsManager.SendEvent ("collect_prize");

		for (int i = 0; i < 10; i++) {
			GameObject _coin = (GameObject)Instantiate (coin, Camera.main.ScreenToWorldPoint (giftButton.transform.position), Quaternion.identity); 
			Coin coinScript = _coin.GetComponent<Coin> ();
			coinScript.MoveToEnd();
		}
		//Savee the current system time as a string in the player prefs class
		_giftNextDate = DateTime.UtcNow;
		DefsGame.BTN_GIFT_HIDE_DELAY = DefsGame.BTN_GIFT_HIDE_DELAY_ARR [DefsGame.BTN_GIFT_HIDE_DELAY_COUNTER];
		if (DefsGame.BTN_GIFT_HIDE_DELAY_COUNTER < DefsGame.BTN_GIFT_HIDE_DELAY_ARR.Length-1) {
			++DefsGame.BTN_GIFT_HIDE_DELAY_COUNTER;
			PlayerPrefs.SetInt ("BTN_GIFT_HIDE_DELAY_COUNTER", DefsGame.BTN_GIFT_HIDE_DELAY_COUNTER);
		}
		_giftNextDate = _giftNextDate.AddMinutes (DefsGame.BTN_GIFT_HIDE_DELAY);
		PlayerPrefs.SetString("dateGiftClicked", _giftNextDate.ToBinary().ToString());
		UIManager.HideUiElement ("BtnGift");
		//timeText.enabled = true;
		//giftButton.enabled = false;
		_isWaitGiftTime = true;
		//giftButton.DisableButtonClicks();
		D.Log ("Disable Button Clicks");
	}

	void Update() {
		if (_isWaitGiftTime) {
			DateTime _currentDate = System.DateTime.UtcNow;
			TimeSpan _difference = _giftNextDate.Subtract (_currentDate);
			if ((_difference.TotalSeconds <= 0f)&&(DefsGame.currentScreen == DefsGame.SCREEN_MENU)) {
				_isWaitGiftTime = false;
				UIManager.ShowUiElement ("BtnGift");
				FlurryEventsManager.SendEvent ("collect_prize_impression");
				D.Log ("Enable Button Clicks");
			} else {
				string _minutes = _difference.Minutes.ToString ();
				if (_difference.Minutes < 10) {
					_minutes = "0" + _minutes;
				}
				string _seconds = _difference.Seconds.ToString ();
				if (_difference.Seconds < 10) {
					_seconds = "0" + _seconds;
				}
				timeText.text = _minutes + ":" + _seconds;
			}
		}
	}

	public void OnMoreAppsClicked()
	{
		//PublishingService.Instance.ShowAppShelf();
		FlurryEventsManager.SendEvent ("more_games");
	}

	public void OnVideoAdsClicked()
	{
		/*FlurryEventsManager.SendEvent ("RV_strawberries", "start_screen");

		//Defs.MuteSounds (true);

		if (!PublishingService.Instance.IsRewardedVideoReady())
		{
			NPBinding.UI.ShowAlertDialogWithSingleButton("Ads not available", "Check your Internet connection or try later!", "Ok", (string _buttonPressed) => {});
			return;
		}


		PublishingService.Instance.ShowRewardedVideo (isSuccess => {
			if (isSuccess) {
				for (int i = 0; i < 25; i++) {
					GameObject _coin = (GameObject)Instantiate (coin, Camera.main.ScreenToWorldPoint (videoAdsButton.transform.position), Quaternion.identity); 
					Coin coinScript = _coin.GetComponent<Coin> ();
					coinScript.MoveToEnd ();
				}
				FlurryEventsManager.SendEvent ("RV_strawberries_complete", "start_screen", true, 25);
			}else {
			}
			//Defs.MuteSounds (false);
		});*/
	}

	public void RateUs() {
//		FlurryEventsManager.SendEvent ("rate_us_impression", "start_screen");
//		Defs.Rate.RateUs ();
		Defs.PlaySound(sndBtnClick, 1f);
	}

	public void Share() {
//		FlurryEventsManager.SendEvent ("share");
		if (SystemInfo.deviceModel.Contains ("iPad")) {
//			Defs.shareVoxel.ShareClick ();
		} else {
//			Defs.Share.ShareClick ();
		}
		Defs.PlaySound(sndBtnClick, 1f);
	}

	public void BtnPlusClick() {
		hideButtons ();
		DefsGame.screenCoins.Show ("start_screen");
		Defs.PlaySound(sndBtnClick, 1f);
	}

	public void BtnSkinsClick() {
		FlurryEventsManager.SendEvent ("candy_shop");
		hideButtons ();
		Defs.PlaySound(sndBtnClick, 1f);
	}
}
