using UnityEngine;
using UnityEngine.UI;

public class BestScore : MonoBehaviour {

	public Text textField;
	public Image img;
    private int _pointsCount = 0;
    private float _startScale;

    private bool _isPointAdded;
    private float _time = 0f;
    private const float Delay = 0.5f;
    private bool _isShowAnimation = true;
    private bool _isHideAnimation = false;
    private AudioClip _sndNewHighScore;

    // Use this for initialization
	void Start () {
		Color color = textField.color;
		color.a = 0f;
		textField.color = color;
		img.color = new Color(img.color.r, img.color.g, img.color.b, color.a);
		_startScale = img.transform.localScale.x;
		_pointsCount = DefsGame.gameBestScore;
		textField.text = _pointsCount.ToString ();
	    _sndNewHighScore = Resources.Load<AudioClip>("snd/fanfares");
	}

	public void ShowAnimation() {
		_isHideAnimation = false;
		_isShowAnimation = true;
	}

	public void HideAnimation() {
		_isShowAnimation = false;
		_isHideAnimation = true;
	}

	// Update is called once per frame
	void Update () {
		if (_isShowAnimation) {
			Color color = textField.color;
			if (textField.color.a < 1f) {
				color.a += 0.1f;
			} else {
				_isShowAnimation = false;
				color.a = 1f;
			}
			textField.color = color;
			img.color = new Color(img.color.r, img.color.g, img.color.b, color.a);
		}

		if (_isHideAnimation) {
			Color color = textField.color;
			if (textField.color.a > 0f) {
				color.a -= 0.1f;
			} else {
				_isHideAnimation = false;
				color.a = 0f;
			}
			textField.color = color;
			img.color = new Color(img.color.r, img.color.g, img.color.b, color.a);
		}

		if (_isPointAdded) {
			_time += Time.deltaTime;
			if (_time > Delay) {
				_time = 0f;
				_isPointAdded = false;
				MakeAnimation ();
			}
		}

		if (img.transform.localScale.x > _startScale) {
			img.transform.localScale = new Vector3 (img.transform.localScale.x - 2.0f*Time.deltaTime, img.transform.localScale.y - 2.0f*Time.deltaTime, 1f);
		}
	}

	void MakeAnimation() {
		_pointsCount = DefsGame.gameBestScore;
		textField.text = _pointsCount.ToString ();
		img.transform.localScale = new Vector3 (_startScale * 1.4f, _startScale * 1.4f, 1f);
		Defs.PlaySound(_sndNewHighScore);

	}

	public void UpdateVisual() {
		// Здесь только визуальная обработка. Изменение BestScore в Points
		if (DefsGame.gameBestScore > _pointsCount) {
			_isPointAdded = true;
		}
	}
}
