using UnityEngine;
using Heyzap;

public class MyHeyzap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		HeyzapAds.Start("70d6db5109295d28b9ab83165d3fa95c", HeyzapAds.FLAG_NO_OPTIONS);
		HeyzapAds.ShowMediationTestSuite();
	}
}
