using System;
using UnityEngine;
using Heyzap;

public class MyHeyzap : MonoBehaviour {
	public static event Action GiveReward;
	[HideInInspector] public bool IsRewardedVideoReady;
	[HideInInspector] public int VideoAdCointer;
	private bool _firstInterstitialShowed;
	// Use this for initialization
	void Start ()
	{
		DefsGame.MyHeyzap = this;
		IsRewardedVideoReady = false;
		VideoAdCointer = 0;
		HeyzapAds.Start("70d6db5109295d28b9ab83165d3fa95c", HeyzapAds.FLAG_NO_OPTIONS);
		HeyzapAds.ShowMediationTestSuite();
		
		// Interstitial ads are automatically fetched
		HZInterstitialAd.Fetch("app-launch");
		HZVideoAd.Fetch("video");
		HZIncentivizedAd.Fetch("rewarded");
		HZIncentivizedAd.AdDisplayListener listener = delegate(string adState, string adTag){
			if ( adState.Equals("incentivized_result_complete") ) {
				// The user has watched the entire video and should be given a reward.
				GameEvents.Send(GiveReward);
			}
			if ( adState.Equals("incentivized_result_incomplete") ) {
				// The user did not watch the entire video and should not be given a   reward.
				
			}
			if ( adState.Equals("hide") ) {
				// Sent when an ad has been removed from view.
				// This is a good place to unpause your app, if applicable.
				Defs.MuteSounds (false);
			}
			if ( adState.Equals("available") )
			{
				IsRewardedVideoReady = true;
				// Sent when an ad has been loaded and is ready to be displayed,
				//   either because we autofetched an ad or because you called
				//   `Fetch`.
			}
		};
 
		HZIncentivizedAd.SetDisplayListener(listener);
		
		HZVideoAd.AdDisplayListener listenerVideo = delegate(string adState, string adTag){
			if ( adState.Equals("hide") ) {
				// Sent when an ad has been removed from view.
				// This is a good place to unpause your app, if applicable.
				Defs.MuteSounds (false);
			}
		};
 
		HZVideoAd.SetDisplayListener(listenerVideo);
	}

	public void ShowStartInterstitial()
	{
		if (HZInterstitialAd.IsAvailable("app-launch"))
		{
			HZShowOptions showOptions = new HZShowOptions();
			showOptions.Tag = "app-launch";
			HZInterstitialAd.ShowWithOptions(showOptions);

			_firstInterstitialShowed = true;
		}
	}

	public void ShowVideo()
	{
		// Later, such as after a level is completed
		if (HZVideoAd.IsAvailable("video")) {
			HZShowOptions showOptions = new HZShowOptions();
			showOptions.Tag = "video";
			HZVideoAd.ShowWithOptions(showOptions);
			DefsGame.MyHeyzap.VideoAdCointer = 0;
			Defs.MuteSounds (true);
		}
	}
	
	public void ShowRewarded()
	{
		// Later, such as after a level is completed
		if (HZIncentivizedAd.IsAvailable("rewarded")) {
			HZIncentivizedShowOptions showOptions = new HZIncentivizedShowOptions();
			showOptions.Tag = "rewarded";
			HZIncentivizedAd.ShowWithOptions(showOptions);
			
			Defs.MuteSounds (true);
			DefsGame.MyHeyzap.VideoAdCointer = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!_firstInterstitialShowed)
		{
			ShowStartInterstitial();
		}
	}
}
