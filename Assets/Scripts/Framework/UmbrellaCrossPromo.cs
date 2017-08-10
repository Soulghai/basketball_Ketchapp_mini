using UnityEngine;

public class UmbrellaCrossPromo : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		if ((DefsGame.noAds != 0)||(DefsGame.GameSessionCointer == 0)) UMBCrossPromo.Show("YOUR-BUNDLE-ID");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
