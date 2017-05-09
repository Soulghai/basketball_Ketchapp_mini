using UnityEngine;

public class DestroyByTime : MonoBehaviour {
	public float LifeTime = 3f;

	void Awake() {
		Destroy (gameObject, LifeTime);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
