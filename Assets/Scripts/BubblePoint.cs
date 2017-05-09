using UnityEngine;
using UnityEngine.UI;

public class BubblePoint : MonoBehaviour {
	private Text _text;
	private float _speedAlpha;

	// Use this for initialization
	void Start () {
		_text = GetComponentInChildren<Text> ();
		_speedAlpha = Random.Range (2.5f, 3.4f);
	}
	
	// Update is called once per frame
	void Update () {


		//if (transform.localScale.x > 1f) {
			transform.localScale = new Vector3 (transform.localScale.x + 2.5f*Time.deltaTime, transform.localScale.x + 2.5f*Time.deltaTime, 1);
		//}

		transform.position = new Vector3(transform.position.x, transform.position.y + 0.6f*Time.deltaTime, 1);

		Color color = _text.color;
		if (_text.color.a > 0) {
			color.a -= _speedAlpha*Time.deltaTime;
			_text.color = color;
		} else
			Destroy (gameObject);
	}
}
