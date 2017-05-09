using UnityEngine;

public class SpringVisualization : MonoBehaviour {
	public Transform point1;
	public Transform point2;
	private float _startDistance;

	// Use this for initialization
	void Start () {
		_startDistance = Vector3.Distance (point1.position, point2.position);
	}

	// Update is called once per frame
	void Update () {
		transform.position = CalcPosition (point1.position, point2.position);
		transform.rotation = Quaternion.AngleAxis(CalcAngle (point1.position, point2.position) * Mathf.Rad2Deg, Vector3.forward);
		float newScale = Vector3.Distance (point1.position, point2.position)/_startDistance;
		transform.localScale = new Vector3 (newScale, Mathf.Min(newScale, 1f), 1f);
	}

	private Vector3 CalcPosition(Vector3 point1, Vector3 point2) {
		return (point1 + point2)*0.5f;
	}

	private float CalcAngle(Vector3 point1, Vector3 point2) {
		return  Mathf.Atan2 (point1.y - point2.y, point1.x - point2.x);
	}
}
