using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Parallax : MonoBehaviour {
    public GameObject[] Buildings;
    public float PositionY = -5.42f;
    public float SpaceRandomPlus = 0f;
    public float SpaceRandomMinus = 0f;
	public float OneStepSize = 6f;

    private List<GameObject> _activeBuildings = new List<GameObject>();

	// Use this for initialization
	void Start ()
	{
	    GameObject go = (GameObject)Instantiate(GetRandomBuilding(), new Vector3(0.0f, PositionY, 1f), Quaternion.identity);
	    _activeBuildings.Add(go);
	}

    void OnEnable() {
        RingManager.OnParallaxMove += Move;
    }

    void OnDisable() {
        RingManager.OnParallaxMove -= Move;
    }

    void Update()
    {
		Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2 (0, 0));
        SpriteRenderer sprite;

		GameObject go;
		int i = 0;
		while (i < _activeBuildings.Count) {
			go = _activeBuildings [i];
			sprite = go.GetComponent <SpriteRenderer>();
			float tileWidth = sprite.bounds.size.x;
			if (go.transform.position.x + tileWidth*0.5f < bottomLeft.x)
			{
				_activeBuildings.Remove (go);
				Destroy(go);
				continue;
			}
			++i;
		}
    }

    private void CreateNewIfNeedeed()
    {
        bool tryToCreateNew = true;
		float width = Camera.main.pixelWidth;
		Vector2 bottomRight = Camera.main.ScreenToWorldPoint(new Vector2 (width, 0));
        SpriteRenderer sprite;
        foreach (GameObject go in _activeBuildings)
        {
            sprite = go.GetComponent <SpriteRenderer>();
            float tileWidth = sprite.bounds.size.x*0.5f;

//            Vector3 screenPoint = Camera.main.WorldToViewportPoint(
//                new Vector3(go.transform.position.x + tileWidth - OneStepSize,
//                    go.transform.position.y,
//                    go.transform.position.z));
//            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

           
            //if (!onScreen)
            if (go.transform.position.x + tileWidth - OneStepSize > bottomRight.x)
            {
                tryToCreateNew = false;
                break;
            }
        }
        if (tryToCreateNew) AddBuilding();
    }

    /*
        float width = camera.pixelWidth;
        float height = camera.pixelHeight;

        Vector2 bottomLeft = camera.ScreenToWorldPoint(new Vector2 (0, 0));
        Vector2 bottomRight = camera.ScreenToWorldPoint(new Vector2 (width, 0));
        Vector2 topLeft = camera.ScreenToWorldPoint(new Vector2 (0, height));
        Vector2 topRight = camera.ScreenToWorldPoint(new Vector2 (width, height));
       */

    private void AddBuilding()
    {
        float backLine = GetBackLine();

        GameObject go = (GameObject)Instantiate(GetRandomBuilding(), new Vector3(), Quaternion.identity);
        SpriteRenderer sprite = go.GetComponent <SpriteRenderer>();
        go.transform.position = new Vector3(backLine + sprite.bounds.size.x*0.5f + Random.Range(-SpaceRandomMinus, SpaceRandomPlus), PositionY, 1f);
        _activeBuildings.Add(go);
    }

    private float GetBackLine()
    {
        float backLine = 0f;
        SpriteRenderer sprite;
        foreach (GameObject go in _activeBuildings)
        {
            sprite = go.GetComponent <SpriteRenderer>();
            float tileWidth = sprite.bounds.size.x*0.5f;
            backLine = Math.Max(go.transform.position.x + tileWidth, backLine);
        }

        return backLine;
    }

    public void Move()
    {

        CreateNewIfNeedeed();

        foreach (GameObject go in _activeBuildings)
        {
            Tweener t;
            t = go.transform.DOMove(new Vector3(go.transform.position.x - OneStepSize, PositionY, 1f),
                0.5f);
            t.SetEase(Ease.InCubic);
        }
    }

    private GameObject GetRandomBuilding() {
		return Buildings[(int) Mathf.Round(Random.value*(Buildings.Length-1))];
    }
}
