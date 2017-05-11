using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class RingManager : MonoBehaviour {
    public static event Action OnParallaxMove;
	public GameObject[] Rings;
	public GameObject Ball;
    public GameObject CoinPrefab;

    private GameObject _coinObject;
    private GameObject _prevRing;
	[HideInInspector] public GameObject NextRing;

	private const float StartXPosition = -4.6f;

    [HideInInspector] public bool WaitMoveToStartPosition;

    private AudioClip[] _audioClips = new AudioClip[10];
    private int _applauseID;

    // Use this for initialization
	void Start () {
		DefsGame.RingManager = this;

	    for (int i = 0; i < 10; i++)
	    {
	        _audioClips[i] = Resources.Load<AudioClip>("snd/Applause/applause" + (i+1).ToString());
	    }
	    _applauseID = -1;
		Init ();
	}

	void OnEnable() {
	    global::Ball.OnGoal += Ball_OnGoal;
		global::Ball.OnBallInBasket += Ball_OnBallInBasket;
	    global::Ball.OnCoinSensor += Ball_OnCoinSensor;
	}

	void OnDisable() {
	    global::Ball.OnGoal -= Ball_OnGoal;
		global::Ball.OnBallInBasket -= Ball_OnBallInBasket;
	    global::Ball.OnCoinSensor -= Ball_OnCoinSensor;
	}

    private void Ball_OnCoinSensor()
    {
        AddTenPoints();
    }

    private void Ball_OnGoal(int pointsCount)
    {
	    MoveToSky();
        if (pointsCount > 3)
        {
            if (_applauseID < _audioClips.Length - 1) ++_applauseID;
            else _applauseID = 0;
            Defs.PlaySound(_audioClips[_applauseID]);
        }
    }

    private void MoveToSky()
    {
        RingHead ringHead = NextRing.GetComponentInChildren<RingHead>();
        ringHead.MoveToSky();
    }

    private void Ball_OnBallInBasket()
	{
		Vector3 newPoint = ChooseNextPoint ();
	    MoveCurrentBaskets(new Vector3(-newPoint.x*0.5f, newPoint.y, 1f));
		_prevRing = NextRing;

		CreateNewRing (new Vector3(newPoint.x*0.5f, newPoint.y, 1f));
	}

    public void Miss()
    {
        if (DefsGame.currentPointsCount != 0) MoveToStartPosition();
        else
        {
            RespownBall();
            //DefsGame.CoinSensor.Hide();
        }
    }

    private void Init()
    {
        WaitMoveToStartPosition = false;
        _prevRing = null;
        NextRing = null;
	    Vector3 newPoint = ChooseNextPoint ();
        CreateFirstRing (new Vector3(-newPoint.x*0.5f, -2.5f, 1f));
        CreateNewRing (new Vector3(newPoint.x*0.5f, -1.0f, 1f));
    }

    private void MoveToStartPosition()
    {
	    Vector3 newPoint = ChooseNextPoint ();

        Tweener t = _prevRing.transform.DOMove (new Vector3 (-newPoint.x*0.5f, -2.5f, 1f), 0.5f);
        t.SetEase (Ease.InCubic);

        t = NextRing.transform.DOMove (new Vector3 (newPoint.x*0.5f, -1.0f, 1f), 0.5f);
        t.SetEase (Ease.InCubic);
        WaitMoveToStartPosition = true;
        t.OnComplete (() => {
            //DefsGame.CoinSensor.Hide();
            RespownBall();
            WaitMoveToStartPosition = false;
        });
    }

    private void MoveCurrentBaskets(Vector3 newPoint)
    {
        RingObject ringObject = _prevRing.GetComponent<RingObject> ();
        Tweener t = _prevRing.transform.DOMove (new Vector3 (-10f, _prevRing.transform.position.y, 1f), 0.5f);
        t.SetEase (Ease.InCubic);
        ringObject.Remove ();

        t = NextRing.transform.DOMove (new Vector3 (newPoint.x, NextRing.transform.position.y, 1f), 0.5f);
        t.SetEase (Ease.InCubic);

        GameEvents.Send(OnParallaxMove);
    }

    private void CreateFirstRing(Vector3 newPoint) {
		_prevRing = GetInactveRing ();
	    _prevRing.transform.position = new Vector3(newPoint.x - 10f, newPoint.y, newPoint.z);
		Tweener t = _prevRing.transform.DOMove (newPoint, 0.5f);
		t.SetEase (Ease.InCubic);
		RingHead ringHead = _prevRing.GetComponentInChildren<RingHead> ();
		ringHead.Hide();
	}

    private void CreateNewRing(Vector3 newPoint) {
		NextRing = GetInactveRing ();
	    NextRing.transform.position = new Vector3(newPoint.x + 10f, newPoint.y, newPoint.z);
		RingHead ringHead = NextRing.GetComponentInChildren<RingHead> (true);
		ringHead.Show ();
		//RingObject ringObject = _nextRing.GetComponent<RingObject> ();
		//ringObject.ShieldVisual.Hide();

		Tweener t = NextRing.transform.DOMove (newPoint, 0.5f);
		t.SetEase (Ease.InCubic);
		t.OnComplete (() => {
			RespownBall();
		    if ((DefsGame.QUEST_GOALS_Counter == 2) || ((DefsGame.QUEST_GOALS_Counter > 5) && (DefsGame.QUEST_GOALS_Counter + 2) % 5 == 0))
		    {
		        DefsGame.IsNeedToShowCoin = true;
		        RingObject ro = NextRing.GetComponent<RingObject>();
		        DefsGame.CoinSensor.gameObject.transform.position = new Vector3(
		            ro.PointsPlace.transform.position.x,
		            ro.PointsPlace.transform.position.y + 1.5f,
		            ro.PointsPlace.transform.position.z);
		        DefsGame.CoinSensor.Init (ro);
		        DefsGame.CoinSensor.Show (true);
		    }
		});
	}

    private Vector3 ChooseNextPoint() {
        float posX;

        if ((DefsGame.ScreenGame.IsGameOver)||(DefsGame.currentPointsCount < 5)) posX = Random.Range(5f, 6.5f); else
        if (DefsGame.currentPointsCount < 10) posX = Random.Range(5.5f, 7.5f); else
        if (DefsGame.currentPointsCount < 20) posX = Random.Range(6.0f, 8.5f); else
        if (DefsGame.currentPointsCount < 30) posX = Random.Range(6.5f, 9.5f); else
        if (DefsGame.currentPointsCount < 40) posX = Random.Range(7f, 10.5f); else
        if (DefsGame.currentPointsCount < 50) posX = Random.Range(7.5f, 11.5f);
        else
            posX = Random.Range(8f, 12.4f);
		 
		return new Vector3(posX, Random.Range(-2.4f, 0.8f), 1f);
	}

    private GameObject GetInactveRing() {
		GameObject ring;
		if (!Rings [0].activeSelf)
			ring = Rings [0];
		else if (!Rings [1].activeSelf)
			ring = Rings [1];
		else if (!Rings [2].activeSelf)
			ring = Rings [2];
		else
			ring = Rings [3];

		ring.SetActive (true);

		return ring;
	}

	private void RespownBall() {
		Ball ballScript = Ball.GetComponent<Ball> ();
		ballScript.Respown (_prevRing.transform.position);
	}

	private void AddTenPoints()
	{
		for (int i = 0; i < 10; i++)
		{//Camera.main.ScreenToWorldPoint(
			GameObject coin = (GameObject) Instantiate(CoinPrefab, DefsGame.CoinSensor.gameObject.transform.position,
				Quaternion.identity);
			Coin coinScript = coin.GetComponent<Coin>();
			coinScript.MoveToEnd();
		}
		DefsGame.CoinSensor.Hide();
	}
}
