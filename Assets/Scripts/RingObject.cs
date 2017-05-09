using UnityEngine;

public class RingObject : MonoBehaviour
{
    public RingHead RingHead;
    public Transform PointsPlace;
	private float _time;
	private const float Delay = 1.0f;
	private bool _isRemove;

	// Use this for initialization
	void Start () {
		_isRemove = false;
		_time = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (_isRemove) {
			_time += Time.deltaTime;
			if (_time >= Delay) {
				_time = 0f;
				_isRemove = false;
				gameObject.SetActive (false);
			}
		}
	}

	public void Remove() {
		_time = 0f;
		_isRemove = true;
	}
}
