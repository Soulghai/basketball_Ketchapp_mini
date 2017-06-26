using UnityEngine;
using VoxelBusters.NativePlugins;
using VoxelBusters.Utility;

public class Share : MonoBehaviour {

	void Start() {
		Defs.ShareVoxel = this;
	}

	public void ShareClick ()
	{
		string _shareLink = "http://www.umbrella.wtf/Ballstar";

		#if UNITY_IOS

		_shareLink = "http://www.umbrella.wtf/Ballstar";
		#endif

		string _shareText = "Wow! I Just Scored ["+DefsGame.gameBestScore.ToString()+ "] in #YummMonsters! Can You Beat Me? @tabtale " + _shareLink;


		string _screenShotPath = Application.persistentDataPath + "/promo1.jpg";

		if (Random.value > 0.5f) {
			_screenShotPath = Application.persistentDataPath + "/promo2.jpg";
		}

		ShareImageAtPathUsingShareSheet (_shareText, _screenShotPath);
	}

	void ShareImageAtPathUsingShareSheet(string _shareText, string _screenShotPath) {
		// Create share sheet
		ShareSheet _shareSheet 	= new ShareSheet();	

		_shareSheet.Text = _shareText;
		_shareSheet.AttachImageAtPath(_screenShotPath);

		// Show composer
		NPBinding.UI.SetPopoverPointAtLastTouchPosition();
		NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);
	}

	void FinishedSharing (eShareResult _result)
	{
		Debug.Log("Finished sharing");
		Debug.Log("Share Result = " + _result);
	}

	
}