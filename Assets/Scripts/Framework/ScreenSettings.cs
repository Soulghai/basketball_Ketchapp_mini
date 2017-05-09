using UnityEngine;

[ExecuteInEditMode]

public class ScreenSettings : MonoBehaviour
{
	public int FrameRate = 60;

	public float GameWidth = 16f;
    public float GameHeight = 9f;
	public float GameAspect {get; private set;}

	public static float ScreenWidth {get; private set;}
	public static float ScreenHeight {get; private set;}
	public static float ScreenAspect {get; private set;}

    public static float ScreenWidthAspect {get; private set;}
    public static float ScreenHeightAspect {get; private set;}

	void Awake()
	{
		SetScreenSettings();
		SetScreenSize();
	}
		
	#if UNITY_EDITOR
	void OnRenderObject()
	{
	    if (Application.isPlaying) return;
	    SetScreenSettings();
	    SetScreenSize();
	}
	#endif

    private void SetScreenSettings()
	{
		//Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Application.targetFrameRate = FrameRate;
	}

    private void SetScreenSize()
	{
		GameAspect = GameWidth / GameHeight;
		ScreenAspect = Camera.main.aspect;

		if (ScreenAspect > GameAspect)
		{
			ScreenWidth = GameHeight * ScreenAspect;
			ScreenHeight = GameHeight;
		}
		else
		{
			ScreenWidth = GameWidth;
			ScreenHeight = GameWidth / ScreenAspect;
		}

	    ScreenWidthAspect = ScreenWidth / GameWidth;
	    ScreenHeightAspect = ScreenHeight / GameHeight;

		Camera.main.orthographicSize = ScreenHeight / 2f;
	}
}
