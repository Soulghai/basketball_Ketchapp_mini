using DG.Tweening;
using UnityEngine;

public class Hint : MonoBehaviour {

	void Start()
	{
		transform.DOScale(1.2f, 1f).SetLoops(-1, LoopType.Yoyo);
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward, 1f);
	}
}
