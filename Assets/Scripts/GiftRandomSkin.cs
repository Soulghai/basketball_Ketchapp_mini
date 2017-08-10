using DoozyUI;
using UnityEngine;
using UnityEngine.UI;

public class GiftRandomSkin : MonoBehaviour
{
	[SerializeField] private Image _sprRenderer;

	private void OnEnable()
	{
		GlobalEvents<OnGiftShowRandomSkinAnimation>.Happened += OnGiftShowRandomSkinAnimation;
	}

	private void OnDisable()
	{
		GlobalEvents<OnGiftShowRandomSkinAnimation>.Happened -= OnGiftShowRandomSkinAnimation;
	}

	private void OnGiftShowRandomSkinAnimation(OnGiftShowRandomSkinAnimation obj)
	{
		int id = GetRandomAvailableSkin();
		if (id != -1)
		{
//			if (_sprRenderer.sprite) Resources.UnloadAsset(_sprRenderer.sprite);
			_sprRenderer.sprite = Resources.Load<Sprite>("gfx/Skins/Skin" + "1");
			transform.localScale = Vector3.one;
			UIManager.ShowUiElement("ScreenGiftImageSkin");
			
			Invoke("ShowBtnClose", 1.5f);

			GlobalEvents<OnBuySkin>.Call(new OnBuySkin {Id = id});
		}
	}

	private int GetRandomAvailableSkin()
	{
		if (DefsGame.QUEST_CHARACTERS_Counter == DefsGame.FaceAvailable.Length - 1) return -1;
		int tryCount = Random.Range(0, DefsGame.FaceAvailable.Length);
		int i = -1;
		while (i < tryCount)
		{
			for (int id = 1; id < DefsGame.FaceAvailable.Length; id++)
			{
				if (DefsGame.FaceAvailable[id] == 0)
				{
					++i;
					if (i == tryCount)
					{
						return id;
					}
				}
			}
		}
		return -1;
	}

	public void BtnClose()
	{
		UIManager.HideUiElement("ScreenGiftImageSkin");
		UIManager.HideUiElement("ScreenGiftBtnPlay");
		
		GlobalEvents<OnHideGiftScreen>.Call(new OnHideGiftScreen());
	}

	private void ShowBtnClose()
	{
		UIManager.ShowUiElement("ScreenGiftBtnPlay");
	}
}
