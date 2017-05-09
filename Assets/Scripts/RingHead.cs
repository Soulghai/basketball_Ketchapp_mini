using UnityEngine;
using DG.Tweening;

public class RingHead : MonoBehaviour {
	public  Animator ShieldAnimator;
	public  Animator RingAnimator;
	public  Animator RingBodyAnimator;

    private Vector3 _startPosition;

	public void Show() {
	    gameObject.SetActive(true);
	}

	public void Hide() {
	    gameObject.SetActive(false);
	}

    public void MoveToSky()
    {
	    Invoke("AnimationHide", 0.4f);

        PlayGoalAnimation();
    }

	private void AnimationHide()
	{
		_startPosition = gameObject.transform.position;
                Tweener t = gameObject.transform.DOMove (
                    new Vector3 (gameObject.transform.position.x, 6f, 1f), 0.5f);
                t.SetEase (Ease.InCubic);
                t.OnComplete (() => {
                    gameObject.transform.position = _startPosition;
                    Hide();
                });
	}

	private void PlayGoalAnimation()
    {
		ShieldAnimator.SetTrigger("isGoal");
		RingAnimator.SetTrigger("isGoal");
	    RingBodyAnimator.SetTrigger("isGoal");

	    RingObject ro = DefsGame.RingManager.NextRing.GetComponent<RingObject>();
	    RingAnimator.gameObject.transform.position = new Vector3(
		    ro.PointsPlace.transform.position.x,
		    ro.PointsPlace.transform.position.y - 0.53f,
		    ro.PointsPlace.transform.position.z);
    }
}
