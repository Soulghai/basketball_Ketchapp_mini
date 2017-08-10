using System.Collections.Generic;
using DoozyUI;
using UnityEngine;

public class ScreenGift : MonoBehaviour
{
	private int _coinsCount;
	private bool _isResetTimer;
	private int _giftType;
	private bool isFirstTime;
	
	private float _centerPointY = 20f;

	private void OnEnable()
	{
		GlobalEvents<OnBtnGiftClick>.Happened += OnBtnGiftClick;
		GlobalEvents<OnBtnGetRandomSkinClick>.Happened += OnBtnGetRandomSkinClick;
		GlobalEvents<OnHideGiftScreen>.Happened += OnHideGiftScreen;
	}

	private void OnDisable()
	{
		GlobalEvents<OnBtnGiftClick>.Happened -= OnBtnGiftClick;
		GlobalEvents<OnBtnGetRandomSkinClick>.Happened -= OnBtnGetRandomSkinClick;
		GlobalEvents<OnHideGiftScreen>.Happened -= OnHideGiftScreen;
	}

	private void OnHideGiftScreen(OnHideGiftScreen obj)
	{
		// Предлагаем Еще оди подарок
		if (isFirstTime)
		{
			isFirstTime = false;
			
			UIManager.ShowUiElement("ScreenGameOverBtnBack");
			UIElement element = null;
			if (_giftType == 1)
			{
				UIManager.ShowUiElement("NotifyGiftExtra");
				element = GetUIElement("NotifyGiftExtra");
			}
			else
			if (_giftType == 2)
			{
				UIManager.ShowUiElement("NotifySkinExtra");
				element = GetUIElement("NotifySkinExtra");
			}
			
			if (element)
			{
				element.customStartAnchoredPosition = new Vector3(0f, _centerPointY, 0f);
				element.useCustomStartAnchoredPosition = true;
			}
			
			return;
		} 
		
		UIManager.HideUiElement("ScreenGift");
		GlobalEvents<OnGiftCollected>.Call(new OnGiftCollected());
		GlobalEvents<OnShowMenu>.Call(new OnShowMenu());
	}
	
	private UIElement GetUIElement(string elementName)
	{
		List<UIElement> list = UIManager.GetUiElements(elementName);
		if (list.Count > 0)
		{
			return list[0];
		}
		return null;
	}

	private void OnBtnGetRandomSkinClick(OnBtnGetRandomSkinClick obj)
	{
		isFirstTime = true;
		_giftType = 2;
		ShowGiftAnimation();
	}

	private void OnBtnGiftClick(OnBtnGiftClick obj)
	{
		isFirstTime = true;
		_coinsCount = obj.CoinsCount;
		_isResetTimer = obj.IsResetTimer;
		_giftType = 1;
		ShowGiftAnimation();
	}

	private void ShowGiftAnimation()
	{
		EndShowing();
	}

	private void EndShowing()
	{
		UIManager.ShowUiElement("ScreenGift");
		if (_giftType == 1) MakeAGift();
		else if (_giftType == 2) MakeAGiftRandomSkin();
	}

	private void MakeAGift()
	{
		GlobalEvents<OnGiftShowCoinsAnimation>.Call(new OnGiftShowCoinsAnimation{CoinsCount = _coinsCount, IsResetTimer = _isResetTimer});
	}
	
	private void MakeAGiftRandomSkin()
	{
		GlobalEvents<OnGiftShowRandomSkinAnimation>.Call(new OnGiftShowRandomSkinAnimation());
	}

	public void BtnClose()
	{
		UIManager.HideUiElement("ScreenGameOverBtnBack");
		UIManager.HideUiElement("NotifyGiftExtra");
		UIManager.HideUiElement("NotifySkinExtra");
		GlobalEvents<OnHideGiftScreen>.Call(new OnHideGiftScreen());
	}
}
