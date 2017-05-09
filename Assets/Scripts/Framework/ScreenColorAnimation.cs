using UnityEngine;

public class ScreenColorAnimation : MonoBehaviour {
	private SpriteRenderer _spr;
	private bool _isShowAnimation;
	private bool _isHideAnimation;
	private float _alphaMax;
	private float _speed;
	//private var funcClose:Function;
	private bool _isAutoHide;
	private bool _isAnimation;

	// Use this for initialization
	void Start () {
		_spr = GetComponent<SpriteRenderer> ();
		_isShowAnimation = false;
		_isHideAnimation = false;
		_alphaMax = 0.8f;
		_isAutoHide = false;
		_speed = 0.1f;
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		if (_isShowAnimation) {
			Color color = _spr.color;
				if (color.a < _alphaMax) color.a += _speed;
			else {
					color.a = _alphaMax;
				_isShowAnimation = false;
				if (_isAutoHide) Hide();
			}
			_spr.color = color;
		}

		if (_isHideAnimation) {
			Color color = _spr.color;
			if (color.a > 0f) color.a -= _speed;
			else {
				color.a = 0f;
				_isHideAnimation = false;
				gameObject.SetActive(false);
			}
			_spr.color = color;
		}

	}

	public void SetColor(float red, float green, float blue) {
		_spr.color = new Color (red, green, blue, _spr.color.a);
	}

	public void SetAlphaMax(float value) {
		_alphaMax = value;
	}

	public void SetAutoHide(bool flag) {
		_isAutoHide = flag;
	}

	public void SetAnimation(bool flag, float speed = 0.05f) {
		_isAnimation = flag;
		_speed = speed;
	}

	//public void setExitByClick(_func:Function) {
	//	funcClose = _func;
	//	bmp.addEventListener(MouseEvent.CLICK, funcMouseClick, false, 0, true);
	//}

//	void OnMouseUp() {
//		if (funcClose != null) {
//			funcClose();
//			funcClose = null;
//		}
//	}

	public void Show() {
		_isShowAnimation = true;
		_isHideAnimation = false;
		Color color = _spr.color;
		if (_isAnimation) {
			color.a = 0f;
		} else {
			color.a = _alphaMax;
		}
		_spr.color = color;
		gameObject.SetActive(true);
	}

	public void Hide() {
		_isHideAnimation = true;
		_isShowAnimation = false;
		Color color = _spr.color;
		if (_isAnimation) {
			color.a = _alphaMax;
		} else {
			color.a = 0f;
			gameObject.SetActive(false);
		}
		_spr.color = color;
	}
}
