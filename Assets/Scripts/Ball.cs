using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    public static event Action OnBallInBasket;
    public static event Action<int> OnGoal;
    public static event Action OnThrow;
    public static event Action<float> OnMiss;
    public static event Action OnCoinSensor;

    public ParticleSystem ParticleTrail;
    public GameObject Hint;
    public GameObject BallVsGroundAnimator;
    private int _targetLinePointCount;
    public GameObject TargetLinePoint;
    private const int TargetHintPartCountMax = 15;
    private int _targetHintPartCount;
    private GameObject[] _targetLinePoints = new GameObject[15 + TargetHintPartCountMax];
    private Vector3 _mouseTarget;
    private int _indicatorCurrentParth = 0;
    private Rigidbody2D _body;

    private Vector3 _startScale;
    private readonly Vector3 _mouseStartPosition = new Vector3(-1.5f, 2.6f, 1f);
    private Vector3 _targetLosePosition;

    private bool _isSetStartPoint;
    private bool _isThrow;
    private float _lifeTime;
    private float _lifeDelay = 1.0f;
    private bool _isGoal;
    private bool _isGoalTrigger;
    private bool _isShowBall;
    private bool _isLose = false;
    private bool _isHideBall = true;

    private bool _isDrawTargetLine = false;

    private float _oldVelocityY;

    private int _pointsCount;
    private Vector3 _oldMousePosition;
    private Vector3 _mousePosition;
    private bool _isShield;

    private bool _isTryThrow;
    private Sprite _sprite;
    private SpriteRenderer _spriteRenderer;
    private AudioClip _ballThrow;
    private AudioClip _ballRespown;
    private AudioClip _ballVsGround;
    private AudioClip _ballVsRing;
    private AudioClip _ballVsCoin;
    private float _hintTime = 0f;
    private bool _isRing;
    private AudioClip _ballVsShield;
    private AudioClip _ballVsWeb;
    private bool _isWeb;
    private int _hintCounter;
    private bool _isGoalTrigger2;

    // Use this for initialization
    void Start()
    {
        DefsGame.Ball = this;

        _ballThrow = Resources.Load<AudioClip>("snd/Ball/start_ball");
        _ballRespown = Resources.Load<AudioClip>("snd/Ball/ball_respawn");
        _ballVsGround = Resources.Load<AudioClip>("snd/Ball/BallVsGround");
        _ballVsShield = Resources.Load<AudioClip>("snd/Ball/BallVsShield");
        _ballVsWeb = Resources.Load<AudioClip>("snd/Ball/BallVsWeb");
        _ballVsRing = Resources.Load<AudioClip>("snd/Ball/BallVsRing");
        _ballVsCoin = Resources.Load<AudioClip>("snd/bonus_green");

        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        LoadSprites (DefsGame.currentFaceID);
        _targetLinePointCount = _targetLinePoints.Length - TargetHintPartCountMax;

        _startScale = transform.localScale;

        for (int i = 0; i < _targetLinePoints.Length; i++)
        {
            _targetLinePoints[i] = (GameObject) Instantiate(TargetLinePoint, transform.position, Quaternion.identity);
        }

        _targetHintPartCount = TargetHintPartCountMax;
        _hintCounter = 0;

        _targetLosePosition = _mouseStartPosition;
        SetNewSkin(DefsGame.currentFaceID);

        ParticleTrail.Stop();
        _isThrow = false;
    }

    public void SetNewSkin(int id) {
        LoadSprites (DefsGame.currentFaceID);
        _spriteRenderer.sprite = _sprite;
    }

    private void LoadSprites(int id){
        //if ((Sprite)_sprite) Resources.UnloadAsset(_sprite);
        _sprite = Resources.Load<Sprite>("gfx/Balls/ball" +(id+1).ToString());
    }

    public void Respown(Vector3 position)
    {
        if (_isLose && DefsGame.currentPointsCount == 0) _mouseTarget = _targetLosePosition;
        else
            _mouseTarget = _mouseStartPosition;

        if (_isLose)
        {
            if (DefsGame.currentPointsCount < 3)
            {
                ++_hintCounter;
            }
            if (_hintCounter >= 3) {
                _targetHintPartCount = TargetHintPartCountMax;
                _hintCounter = 0;
            }
        }

        Hint.SetActive(true);

        _mouseTarget = _targetLosePosition;

        transform.position = new Vector3(position.x + 0.75f, position.y - 1.1f, position.z);

        _lifeTime = 0f;
        _isShield = false;
        _isRing = false;
        _isWeb = false;
        _isLose = false;
        _isGoal = false;
        _isGoalTrigger = false;
        _isGoalTrigger2 = false;
        _isSetStartPoint = false;
        _isHideBall = false;
        _isShowBall = true;
        transform.localScale = new Vector3(0f, 0f, transform.localScale.z);
        transform.rotation = Quaternion.identity;
        _isThrow = false;
        _body.isKinematic = true;
        _body.velocity = new Vector2();
        _body.angularVelocity = 0f;
        _body.rotation = 0;

        _isTryThrow = false;
        _pointsCount = 1;
        Defs.PlaySound(_ballRespown);
        ParticleTrail.Stop();
    }

    public void Hide()
    {
        _isHideBall = true;
    }

    public void Show()
    {
        _isShowBall = true;
    }

    // Update is called once per frame
    void Update()
    {
        if ((_oldVelocityY > 0f) && (_body.velocity.y <= 0f))
        {
            D.Log("Gothca");
        }

        _oldVelocityY = _body.velocity.y;

        if (!_isThrow)
        {
            if ((!_isSetStartPoint)&&((DefsGame.currentScreen == DefsGame.SCREEN_GAME)||(DefsGame.currentScreen == DefsGame.SCREEN_MENU)))
                SetStartPoint();

            if (_isDrawTargetLine) DrawTargetLine();

            if ((_isSetStartPoint) && (InputController.IsTouchOnScreen(TouchPhase.Ended)))
            {
                _isTryThrow = true;
            }

            if ((_isTryThrow) && (!_isShowBall))
            {
                ThrowBall();
            }

            if (_isShowBall)
            {
                if (transform.localScale.x < _startScale.x)
                {
                    transform.localScale = new Vector3(transform.localScale.x + 0.1f, transform.localScale.y + 0.1f,
                        transform.localScale.z);
                }
                else
                {
                    _isDrawTargetLine = true;
                    _isShowBall = false;
                    transform.localScale = new Vector3(_startScale.x, _startScale.y, _startScale.z);
                }
            }
        }
        else
        {
            HideTargetLine();

            if (_isHideBall)
            {
                if (transform.localScale.x > 0f)
                {
                    transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f,
                        transform.localScale.z);
                }
                else
                {
                    _isHideBall = false;
                    transform.localScale = new Vector3(0f, 0f, transform.localScale.z);
                    //if (_isLose && !DefsGame.RingManager.WaitMoveToStartPosition) Respown(_startPosition);
                }
            }

            if (_isGoal)
            {
                _lifeTime += Time.deltaTime;
                if (_lifeTime >= _lifeDelay)
                {
                    GameEvents.Send(OnBallInBasket);
                    _isHideBall = true;
                    _body.isKinematic = true;
                    if (_targetHintPartCount > 0)
                    {
                        _targetHintPartCount -= 4;
                        if (_targetHintPartCount < 0) _targetHintPartCount = 0;
                    }
                    _lifeTime = -10000f;
                }
            }

            if (_isLose)
            {
                _lifeTime += Time.deltaTime;
                if (_lifeTime >= _lifeDelay)
                {
                    _isHideBall = true;
                    _body.isKinematic = true;
                    _lifeTime = -10000f;
                    GameEvents.Send(OnMiss, 0.1f);
                }
            }
            else
            {
                float width = Camera.main.pixelWidth;
                Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
                Vector2 bottomRight = Camera.main.ScreenToWorldPoint(new Vector2(width, 0));
                if ((transform.position.x < bottomLeft.x - 1f) || (transform.position.x > bottomRight.x + 1f))
                {
                    LoseTrigger(true);
                }
            }
        }
    }

    private void SetStartPoint()
    {
        if (InputController.IsTouchOnScreen(TouchPhase.Began))
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _isSetStartPoint = true;
            D.Log("Set Start Point");
        }
    }

    private void DrawTargetLine()
    {
        if (_isSetStartPoint)
            if (InputController.IsTouchOnScreen(TouchPhase.Moved))
            {
                _oldMousePosition = _mousePosition;
                _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                _mouseTarget.x += (_mousePosition.x - _oldMousePosition.x) * 2f;
                _mouseTarget.y += (_mousePosition.y - _oldMousePosition.y) * 2f;

                if (_mouseTarget.y > ScreenSettings.ScreenHeight * 0.5f)
                {
                    _mouseTarget.y = ScreenSettings.ScreenHeight * 0.5f;
                }
            }

        Vector2 dist = new Vector2(_mouseTarget.x - transform.position.x, _mouseTarget.y - transform.position.y);

        CutDistance(ref dist);

        Vector2 force = CalcForce(dist.x, dist.y);
        float t = force.y / Physics.gravity.y;

        float _pX = transform.position.x;
        float _pY = transform.position.y;
        float dTime = t / 30f;
        float velY = -force.y;
        float velX = dist.x / 30f;

        IncTargetHint();

        GameObject _object;
        float _scale = 1f;
        int id;
        for (int i = 0; i < 60; i++)
        {
            _pX += velX;
            velY += Physics.gravity.y * dTime;
            _pY += velY * dTime;


            if (i % 2 != 0) continue;

            id = i / 2;
            if (id < _targetLinePointCount + _targetHintPartCount)
            {
                _object = _targetLinePoints[id];
                if (!_object.activeSelf) _object.SetActive(true);
                if (id == _targetLinePointCount - 1)
                    _scale = 1.0f;
                //else
                //	if (i == indicatorCurrentParth)
                //	_scale = 1f;
                else
                {
                    if (id < _targetLinePointCount)
                        _scale = 0.15f + id / (float) _targetLinePointCount * 0.45f;
                    else
                        _scale = 0.5f - (id - TargetHintPartCountMax) * (0.45f / TargetHintPartCountMax);
                }

                if (id == _indicatorCurrentParth) _scale *= 1.35f;

                _object.transform.localScale = Vector3.Lerp(_object.transform.localScale,
                    new Vector3(_scale, _scale, 1f),
                    0.2f);

                _object.transform.position = new Vector3(_pX, _pY, 1f);
            }
        }
    }

    private void IncTargetHint()
    {
        _hintTime += Time.deltaTime;
        if (_hintTime >= 0.05f)
        {
            _hintTime = 0f;
            if (_indicatorCurrentParth < _targetLinePointCount - 1)
                ++_indicatorCurrentParth;
            else
                _indicatorCurrentParth = 0;
        }
    }

    private void HideTargetLine()
    {
        GameObject _object;
        for (int i = 0; i < _targetLinePointCount + TargetHintPartCountMax; i++)
        {
            _object = _targetLinePoints[i];
            if (_object.activeSelf)
            //_object.transform.localScale = Vector3.Lerp(_object.transform.localScale, new Vector3(0, 0, 1f), 0.05f);
            if (_object.transform.localScale.x > 0f)
            {
                _object.transform.localScale = new Vector3(
                    _object.transform.localScale.x - 0.1f,
                    _object.transform.localScale.y - 0.1f,
                    _object.transform.localScale.z);
            }
            else
            {
                _object.SetActive(false);
            }
        }
    }

    private void ThrowBall()
    {
        if (!_isSetStartPoint) return;

        Hint.SetActive(false);

        ++DefsGame.ThrowsCounter;

        _targetLosePosition = _mouseTarget;

        GameEvents.Send(OnThrow);

        Vector2 dist = new Vector2(_mouseTarget.x - transform.position.x, _mouseTarget.y - transform.position.y);

        CutDistance(ref dist);

        _pointsCount = 3;

        _body.velocity = new Vector2();
        _body.angularVelocity = 0f;
        _body.isKinematic = false;

        Vector2 force = CalcForce(dist.x, dist.y);
        _body.AddForce(new Vector2(force.x * 74.4f, force.y * 74.4f)); //49.45
        _body.AddTorque(1);
        _isThrow = true;

        Defs.PlaySound(_ballThrow);
        ParticleTrail.Play();
    }

    Vector2 CalcForce(float distX, float distY)
    {
        Vector2 force = new Vector2();
        force.y = Mathf.Sqrt(2f * distY * (-Physics.gravity.y));
        float t = force.y / -Physics.gravity.y;
        force.x = distX / t;
        return force;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_isGoal)
            return;
        if (other.gameObject.CompareTag("LoseTrigger"))
        {
            if (!_isGoal) LoseTrigger(true);
        } else
        if (other.CompareTag("GoalTrigger"))
        {
            _isGoalTrigger = true;
            if (_isLose)
            {
                _lifeTime = -1000000f;
            }
        }
        else if (other.CompareTag("GoalTrigger2"))
        {
            if (_isGoalTrigger)
            {
                _isGoal = true;
                _lifeDelay = 1.5f;
                _lifeTime = 0;

                if (_isLose)
                {
                    if (_isGoalTrigger2) _pointsCount = 50;
                    else if ((!_isRing) && (!_isShield)) _pointsCount = 30;
                    else if ((_isRing) && (!_isShield)) _pointsCount = 20;
                    else if ((!_isRing) && (_isShield)) _pointsCount = 20;
                    else if ((_isRing) && (_isShield)) _pointsCount = 10;
                    _isLose = false;
                }
                else
                {
                    if ((!_isRing) && (!_isShield)) _pointsCount = 3;
                    else if ((_isRing) && (!_isShield)) _pointsCount = 2;
                    else if ((!_isRing) && (_isShield)) _pointsCount = 2;
                    else if ((_isRing) && (_isShield)) _pointsCount = 1;
                }

                ++DefsGame.QUEST_GOALS_Counter;
                PlayerPrefs.SetInt("QUEST_GOALS_Counter", DefsGame.QUEST_GOALS_Counter);
                GameEvents.Send(OnGoal, _pointsCount);
            }
            else
            {
                _isGoalTrigger2 = true;
            }
        }
        else if (other.CompareTag("CoinSensor"))
        {
            Defs.PlaySound(_ballVsCoin);
            GameEvents.Send(OnCoinSensor);
            D.Log("CoinSensor");
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("LoseTrigger"))
        {
            if (!_isGoal) LoseTrigger();

            Defs.PlaySound(_ballVsGround, Mathf.Min(_body.velocity.sqrMagnitude, 180f)/180f);
            Instantiate(BallVsGroundAnimator,
                new Vector3(transform.position.x, transform.position.y - 0.26f, transform.position.z),
                Quaternion.identity);
        } else
        if (other.gameObject.CompareTag("ShieldTrigger")) {
            _isShield = true;
            Defs.PlaySound(_ballVsGround, Mathf.Min(_body.velocity.sqrMagnitude, 150f)/150f);
            GameObject go = (GameObject)Instantiate(BallVsGroundAnimator,
                new Vector3(transform.position.x + 0.33f, transform.position.y, transform.position.z),
                Quaternion.identity);
            go.transform.Rotate(Vector3.forward, 90f);
        } else
        if (other.gameObject.CompareTag("ShieldTopTrigger")) {
            _isShield = true;
            Defs.PlaySound(_ballVsGround, Mathf.Min(_body.velocity.sqrMagnitude, 150f)/150f);
            Instantiate(BallVsGroundAnimator,
                new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z),
                Quaternion.identity);
        } else
        if (other.gameObject.CompareTag("RingTrigger")) {
            _isRing = true;
            Defs.PlaySound(_ballVsRing, Mathf.Min(_body.velocity.sqrMagnitude, 100f)/100f);
        } else
        if (other.gameObject.CompareTag("WebTrigger")) {
            if (!_isWeb)
            Defs.PlaySound(_ballVsWeb, Mathf.Min(_body.velocity.sqrMagnitude, 60f)/60f);
            _isWeb = true;
        }
	}

    private void LoseTrigger(bool immediately = false)
    {
        if (immediately)
        {
            _isLose = true;
            _lifeTime = 0f;
            _lifeDelay = 0f;
            return;
        }

        if (_isLose)
        {
            _lifeDelay = 1f;
        }
        else
        {
            _isLose = true;
            _lifeTime = 0f;
            _lifeDelay = 2f;
        }
    }

    private void CutDistance(ref Vector2 dist) {
		if (dist.y < 1f) {
			dist.y = 1f;
		}

		if (dist.x < 1.0f) {
			dist.x = 1.0f;
		} else if (dist.x > 8.5f) {
			dist.x = 8.5f;
		}
	}
}
