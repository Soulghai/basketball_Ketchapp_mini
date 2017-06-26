using UnityEngine;
using System.Collections;

public class UMBDemo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		UMBCrossPromo.OnDidLoad += ()=>{
			Debug.Log("Did load");
		};
	
		UMBCrossPromo.OnDidFailToLoad += (string error)=>{
			Debug.Log("OnDidFailToLoad "+error);
		};
		UMBCrossPromo.OnDidClose += ()=>{
			Debug.Log("OnDidClose");
		};
		UMBCrossPromo.OnDidOpenStoreForAppWithId += (string appId)=>{
			Debug.Log("OnDidOpenStoreForAppWithId "+appId);
		};
		UMBCrossPromo.OnDidCloseStore += ()=>{
			Debug.Log("OnDidCloseStore");
		};
		UMBCrossPromo.OnDidCallActionWithUrl += (string actionurl)=>{
			Debug.Log("OnDidCallActionWithUrl "+actionurl);
		};
		UMBCrossPromo.OnDidTrackInstall += ()=>{
			Debug.Log("OnDidTrackInstall");
		};
		UMBCrossPromo.OnDidFailToTrackInstall += (string error)=>{
			Debug.Log("OnDidFailToTrackInstall "+error);
		};


		
		
	}
	
	public void onShowButton() {
		UMBCrossPromo.Show("com.umbrella.fake");
	}

	public void onTrackButton() {
		UMBCrossPromo.Track("com.umbrella.fake");
	}

	public void onShowAndHideDelayed() {
		UMBCrossPromo.Show("com.umbrella.fake");
		StartCoroutine(DelayedHide());
	}

	protected IEnumerator DelayedHide() {
		yield return new WaitForSeconds (10f);
		UMBCrossPromo.Hide();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
