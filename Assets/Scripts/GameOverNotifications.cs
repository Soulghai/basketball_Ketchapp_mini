using System.Collections.Generic;
using DoozyUI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameOverNotifications : MonoBehaviour
{
    [SerializeField] private Text _nextCharacterText;
    [SerializeField] private Text _shareText;
    
    private float _centerPointY = 20f;
    private const float ItemHeightHalf = 50f;
    private const float HeightStep = 120f;
    private bool _isGiftAvailable;
    private bool _isRewardedAvailable;
    private readonly List<string> _activeNamesList  = new List<string>();

    private int _showCounter;
    private int _showCounterGlobal;
    
    private int shareRewardValue;

    private bool _isGotNewCharacter;
    private int _giftValue;
    private bool isVisual;

    private void Start()
    {
        _showCounterGlobal = PlayerPrefs.GetInt("showCounterGlobal", 0);
    }

    private void OnEnable()
    {
        // Глобальные
        GlobalEvents<OnShowNotifications>.Happened += ShowNotifications;
        GlobalEvents<OnRewardedAvailable>.Happened += IsRewardedAvailable;
        GlobalEvents<OnGiftAvailable>.Happened += IsGiftAvailable;
        
        // Внутренние
        GlobalEvents<OnGotNewCharacter>.Happened += OnGotNewCharacter;
        GlobalEvents<OnBtnRateClick>.Happened += OnBtnRateClick;
        GlobalEvents<OnGiftCollected>.Happened += OnGiftCollected;
    }

    private void OnDisable()
    {
        GlobalEvents<OnShowNotifications>.Happened -= ShowNotifications;
        GlobalEvents<OnRewardedAvailable>.Happened -= IsRewardedAvailable;
        GlobalEvents<OnGiftAvailable>.Happened -= IsGiftAvailable;
        GlobalEvents<OnGotNewCharacter>.Happened -= OnGotNewCharacter;
        GlobalEvents<OnBtnRateClick>.Happened -= OnBtnRateClick;
        GlobalEvents<OnGiftCollected>.Happened -= OnGiftCollected;
    }
    
    private void ShowNotifications(OnShowNotifications e)
    {

        DefsGame.CurrentScreen = DefsGame.SCREEN_NOTIFICATIONS;
            
        ++_showCounter;
        PlayerPrefs.SetInt("showCounterGlobal", ++_showCounterGlobal);

        float ran = Random.value;
        
        // Важность - Высокая

        if (DefsGame.CoinsCount >= 200 && DefsGame.QUEST_CHARACTERS_Counter < DefsGame.FaceAvailable.Length - 1)
        {
            _activeNamesList.Add("NotifyNewCharacter");
        }

        if (_isGiftAvailable
            /*||_activeNamesList.Count == 0 && Random.value < 0.5f
            ||_activeNamesList.Count == 1 && Random.value < 0.25f*/)
        {
            AddNoifyGift();
        }
        
        // Важность - Средняя
        
        if (_activeNamesList.Count < 4 && _isGotNewCharacter && DefsGame.RateCounter == 0)
        {
            _activeNamesList.Add("NotifyRate");
            _isGotNewCharacter = false;
        }
        
        if (_activeNamesList.Count < 4 && (DefsGame.GameplayCounter == 3 || (DefsGame.GameplayCounter-3) % 5 == 0)/* && _isRewardedAvailable*/)
        {
            _activeNamesList.Add("NotifyRewarded");
        }

        if (_activeNamesList.Count < 4 && (DefsGame.CurrentPointsCount > DefsGame.GameBestScore * 0.5f
                                           || _isGotNewCharacter
                                           || _activeNamesList.Count == 0 && ran < 0.3f
                                           || _activeNamesList.Count == 1 && ran < 0.25f))
        {
            _activeNamesList.Add("NotifyShare");
            if (DefsGame.CoinsCount > 100 && DefsGame.CoinsCount < 155)
                shareRewardValue = 180-DefsGame.CoinsCount;
            else
            {
                if (ran < 0.5f) shareRewardValue = 45;
                else shareRewardValue = 50;
            }
            _shareText.text = shareRewardValue.ToString();
        }
        
        // Важность - Низкая
        
        if (_giftValue == 0 && _activeNamesList.Count < 4 && (_activeNamesList.Count == 0 && ran > 0.7f
                                           || _activeNamesList.Count == 1 && ran > 0.75f
                                           || _activeNamesList.Count == 2 && ran > 0.80f))
        {
            _activeNamesList.Add("NotifyGiftWaiting");
        }

        if (_activeNamesList.Count < 4 && (_activeNamesList.Count == 0 && ran > 0.7f
                                              || _activeNamesList.Count == 1 && ran > 0.75f
                                              || _activeNamesList.Count == 2 && ran > 0.80f)
        && ran > 0.5f)
        {
            AddNotifyNextSkin();
        }

        // Перемешиваем элементы списка, чтобы они располагались рандомно по оси У
        ShuffleItems();
        SetItemsPositions();
        ShowItems();
    }

    private void ShuffleItems()
    {
        // Перемешиваем 10 раз
        for (int n = 0; n < 10; n++)
        for (int i = 0; i < _activeNamesList.Count; i++)
        {
            int j = Random.Range(0, _activeNamesList.Count-1);
            if (j != i)
            {
                string str = _activeNamesList[i];
                _activeNamesList[i] = _activeNamesList[j];
                _activeNamesList[j] = str;
            }
        }
    }

    private void AddNoifyGift()
    {
        _activeNamesList.Add("NotifyGift");
        if (DefsGame.CoinsCount > 100 && DefsGame.CoinsCount < 155)
            _giftValue = 180-DefsGame.CoinsCount;
        else
        {
            if (Random.value < 0.5f) _giftValue = 45;
            else _giftValue = 50;
        }
    }
    
    private void AddNotifyNextSkin(int spendMoneyCount = 0)
    {
        if (DefsGame.CoinsCount - spendMoneyCount < 200 && DefsGame.QUEST_CHARACTERS_Counter < DefsGame.FaceAvailable.Length - 1)
        {
            _activeNamesList.Add("NotifyNextCharacter");
            int toNextSkin = 200 - (DefsGame.CoinsCount-spendMoneyCount);
            _nextCharacterText.text = toNextSkin.ToString();
        }
    }

    private void SetItemsPositions()
    {
        float startPos = CalcStartPosition(_activeNamesList.Count);
        
        for (int i = 0; i < _activeNamesList.Count; i++)
        {
            var element = GetUIElement(_activeNamesList[i]);
            if (element)
            {
                element.customStartAnchoredPosition = new Vector3(0f, startPos + i*HeightStep, 0f);
                element.useCustomStartAnchoredPosition = true;
            }
        }
    }

    private void ShowItems()
    {
        UIManager.ShowUiElement("ScreenGameOver");
        UIManager.ShowUiElement("ScreenNotiftyBtnClose");
        for (int i = 0; i < _activeNamesList.Count; i++)
        {
            var element = GetUIElement(_activeNamesList[i]);
            if (element)
            {
                UIManager.ShowUiElement(_activeNamesList[i]);
            }
        }
        isVisual = true;

    }

    private float CalcStartPosition(int notificationCounter)
    {
        return _centerPointY - notificationCounter * HeightStep * 0.5f + ItemHeightHalf;
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

    private void HideAndRemoveNotifications()
    {
        HideNotifications();
        _activeNamesList.Clear();
        UIManager.HideUiElement("ScreenGameOver");
    }

    private void HideNotifications()
    {
        foreach (string t in _activeNamesList)
        {
            var element = GetUIElement(t);
            if (element)
            {
                UIManager.HideUiElement(t);
            }
        }
        
        isVisual = false;
        
        UIManager.HideUiElement("ScreenNotiftyBtnClose");
    }

    private void IsRewardedAvailable(OnRewardedAvailable e)
    {
        _isRewardedAvailable = e.IsAvailable;
    }

    private void IsGiftAvailable(OnGiftAvailable e)
    {
        _isGiftAvailable = e.IsAvailable;

        if (!_isGiftAvailable || !isVisual || _activeNamesList.IndexOf("NotifyGiftWaiting") == -1) return;

        AddNoifyGift();
   
        int idNotifyOld = _activeNamesList.IndexOf("NotifyGiftWaiting");
        if (idNotifyOld != -1)
        {
            
            var element = GetUIElement(_activeNamesList[idNotifyOld]);
            UIManager.HideUiElement(_activeNamesList[idNotifyOld]);
            var element2 = GetUIElement("NotifyGift");
            if (element)
            {
                element2.customStartAnchoredPosition = element.customStartAnchoredPosition;
                element2.useCustomStartAnchoredPosition = true;
            }
            _activeNamesList.RemoveAt(idNotifyOld);
        }
        
        UIManager.ShowUiElement("NotifyGift");
    }
    
    private void OnGotNewCharacter(OnGotNewCharacter obj)
    {
        _isGotNewCharacter = true;
    }
    
    private void OnGiftCollected(OnGiftCollected obj)
    {
        SetItemsPositions();
        ShowItems();
        DefsGame.CurrentScreen = DefsGame.SCREEN_MENU;
    }
    
    //----------------------------------------------------
    // Touches
    //----------------------------------------------------

    public void BtnRateClick()
    {
        GlobalEvents<OnBtnRateClick>.Call(new OnBtnRateClick());
        UIManager.HideUiElement("NotifyRate");
    }
    
    private void OnBtnRateClick(OnBtnRateClick obj)
    {
        DefsGame.RateCounter = 1;
        PlayerPrefs.SetInt("RateCounter", 1);
    }
    
    public void BtnShareClick()
    {
        GlobalEvents<OnBtnShareClick>.Call(new OnBtnShareClick());
        UIManager.HideUiElement("NotifyShare");
    }
    
    public void BtnNewSkinClick()
    {
        HideNotifications();
        int id = _activeNamesList.IndexOf("NotifyNewCharacter"); 
        if (id != -1) _activeNamesList.RemoveAt(id);

        AddNotifyNextSkin(200);
        
        GlobalEvents<OnBtnGetRandomSkinClick>.Call(new OnBtnGetRandomSkinClick());
        GlobalEvents<OnHideMenu>.Call(new OnHideMenu());
    }
    
    public void BtnGiftClick()
    {
        HideNotifications();
        int id = _activeNamesList.IndexOf("NotifyGift"); 
        if (id != -1) _activeNamesList.RemoveAt(id);
        
        _activeNamesList.Add("NotifyGiftWaiting");
            
        GlobalEvents<OnBtnGiftClick>.Call(new OnBtnGiftClick{CoinsCount = _giftValue, IsResetTimer = true});
        _giftValue = 0;
        GlobalEvents<OnHideMenu>.Call(new OnHideMenu());
    }
    
    public void BtnRewardedClick()
    {
        GlobalEvents<OnShowRewarded>.Call(new OnShowRewarded());
        UIManager.HideUiElement("NotifyRewarded");
    }

    public void BtnClose()
    {
        HideAndRemoveNotifications();
        GlobalEvents<OnStartGame>.Call(new OnStartGame());
    }
}