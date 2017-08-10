using System;
using DoozyUI;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSkins : MonoBehaviour
{
    public GameObject FacebookMark;

    public GameObject haveNewSkin;
    public GameObject skin10;
    public GameObject skin11;
    public GameObject skin12;
    public GameObject skin2;
    public GameObject skin3;
    public GameObject skin4;
    public GameObject skin5;
    public GameObject skin6;
    public GameObject skin7;
    public GameObject skin8;
    public GameObject skin9;
    public GameObject TwitterMark;
    public static event Action<int> OnAddCoinsVisual;

    private void Awake()
    {
        DefsGame.screenSkins = this;
    }

    private void Start()
    {
        if (DefsGame.FaceAvailable[1] == 1) FacebookMark.SetActive(false);
        if (DefsGame.FaceAvailable[2] == 1) TwitterMark.SetActive(false);
    }

    private void SetSkin(int _id)
    {
        FlurryEventsManager.SendEvent("candy_purchase_<" + _id + ">");

        if (_id == DefsGame.CurrentFaceId)
            return;

        if (DefsGame.FaceAvailable[_id] == 1)
        {
            DefsGame.CurrentFaceId = _id;
            PlayerPrefs.SetInt("currentFaceID", DefsGame.CurrentFaceId);
            DefsGame.Ball.SetNewSkin(_id);
        }
        else if (DefsGame.CoinsCount >= DefsGame.FacePrice[_id - 1] || _id == 1 || _id == 2)
        {
            if (_id == 1)
            {
                if (DefsGame.FaceAvailable[_id] == 0)
                {
                    Application.OpenURL("https://twitter.com/umbrellafun");
                    FacebookMark.SetActive(false);
                }
            }
            else if (_id == 2)
            {
                if (DefsGame.FaceAvailable[_id] == 0)
                {
                    Application.OpenURL("https://www.facebook.com/umbrellafun/");
                    TwitterMark.SetActive(false);
                }
            }
            else
            {
                GameEvents.Send(OnAddCoinsVisual, -DefsGame.FacePrice[_id - 1]);
            }


            DefsGame.FaceAvailable[_id] = 1;
            DefsGame.CurrentFaceId = _id;
            PlayerPrefs.SetInt("currentFaceID", DefsGame.CurrentFaceId);
            PlayerPrefs.SetInt("faceAvailable_" + _id, 1);
            DefsGame.Ball.SetNewSkin(_id);

            ++DefsGame.QUEST_CHARACTERS_Counter;
            PlayerPrefs.SetInt("QUEST_CHARACTERS_Counter", DefsGame.QUEST_CHARACTERS_Counter);

            //DefsGame.gameServices.ReportProgressWithGlobalID (DefsGame.gameServices.ACHIEVEMENT_NEW_SKIN, 1);

            //DefsGame.gameServices.ReportProgressWithGlobalID (DefsGame.gameServices.ACHIEVEMENT_COLLECTION, DefsGame.QUEST_CHARACTERS_Counter);

            ChooseColorForButtons();

            FlurryEventsManager.SendEvent("candy_purchase_completed_<" + _id + ">");
        } else {
            HideButtons();
            FlurryEventsManager.SendEndEvent("candy_shop_length");

            DefsGame.screenCoins.Show("candy_shop");
        }
    }

    private void ShowButtons()
    {
        UIManager.ShowUiElement("ScreenSkinsBtnBack");
        UIManager.ShowUiElement("BtnSkin1");
        UIManager.ShowUiElement("BtnSkin2");
        UIManager.ShowUiElement("BtnSkin3");
        UIManager.ShowUiElement("BtnSkin4");
        UIManager.ShowUiElement("BtnSkin5");
        UIManager.ShowUiElement("BtnSkin6");
        UIManager.ShowUiElement("BtnSkin7");
        UIManager.ShowUiElement("BtnSkin8");
        UIManager.ShowUiElement("BtnSkin9");
        UIManager.ShowUiElement("BtnSkin10");
        UIManager.ShowUiElement("BtnSkin11");
        UIManager.ShowUiElement("BtnSkin12");
    }

    private void HideButtons()
    {
        UIManager.HideUiElement("ScreenSkinsBtnBack");
        UIManager.HideUiElement("BtnSkin1");
        UIManager.HideUiElement("BtnSkin2");
        UIManager.HideUiElement("BtnSkin3");
        UIManager.HideUiElement("BtnSkin4");
        UIManager.HideUiElement("BtnSkin5");
        UIManager.HideUiElement("BtnSkin6");
        UIManager.HideUiElement("BtnSkin7");
        UIManager.HideUiElement("BtnSkin8");
        UIManager.HideUiElement("BtnSkin9");
        UIManager.HideUiElement("BtnSkin10");
        UIManager.HideUiElement("BtnSkin11");
        UIManager.HideUiElement("BtnSkin12");
    }

    public void SetSkin1()
    {
        SetSkin(0);
    }

    public void SetSkin2()
    {
        SetSkin(1);
    }

    public void SetSkin3()
    {
        SetSkin(2);
    }

    public void SetSkin4()
    {
        SetSkin(3);
    }

    public void SetSkin5()
    {
        SetSkin(4);
    }

    public void SetSkin6()
    {
        SetSkin(5);
    }

    public void SetSkin7()
    {
        SetSkin(6);
    }

    public void SetSkin8()
    {
        SetSkin(7);
    }

    public void SetSkin9()
    {
        SetSkin(8);
    }

    public void SetSkin10()
    {
        SetSkin(9);
    }

    public void SetSkin11()
    {
        SetSkin(10);
    }

    public void SetSkin12()
    {
        SetSkin(11);
    }

    public void Show()
    {
        FlurryEventsManager.SendStartEvent("candy_shop_length");

        DefsGame.CurrentScreen = DefsGame.SCREEN_SKINS;
        DefsGame.isCanPlay = false;
        ChooseColorForButtons();
        ShowButtons();
    }

    public void Hide()
    {
        FlurryEventsManager.SendEndEvent("candy_shop_length");

        DefsGame.CurrentScreen = DefsGame.SCREEN_MENU;
        DefsGame.isCanPlay = true;
        ChooseColorForButtons();
        HideButtons();
    }

    public void ChooseColorForButtons()
    {
        if (DefsGame.FaceAvailable[1] == 1)
        {
            skin2.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin2.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin2.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin2.GetComponentInChildren<Text>().text = DefsGame.FacePrice[0].ToString();
        }
        if (DefsGame.FaceAvailable[2] == 1)
        {
            skin3.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin3.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin3.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin3.GetComponentInChildren<Text>().text = DefsGame.FacePrice[1].ToString();
        }
        if (DefsGame.FaceAvailable[3] == 1)
        {
            skin4.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin4.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin4.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin4.GetComponentInChildren<Text>().text = DefsGame.FacePrice[2].ToString();
        }
        if (DefsGame.FaceAvailable[4] == 1)
        {
            skin5.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin5.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin5.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin5.GetComponentInChildren<Text>().text = DefsGame.FacePrice[3].ToString();
        }
        if (DefsGame.FaceAvailable[5] == 1)
        {
            skin6.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin6.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin6.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin6.GetComponentInChildren<Text>().text = DefsGame.FacePrice[4].ToString();
        }
        if (DefsGame.FaceAvailable[6] == 1)
        {
            skin7.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin7.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin7.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin7.GetComponentInChildren<Text>().text = DefsGame.FacePrice[5].ToString();
        }
        if (DefsGame.FaceAvailable[7] == 1)
        {
            skin8.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin8.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin8.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin8.GetComponentInChildren<Text>().text = DefsGame.FacePrice[6].ToString();
        }
        if (DefsGame.FaceAvailable[8] == 1)
        {
            skin9.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin9.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin9.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin9.GetComponentInChildren<Text>().text = DefsGame.FacePrice[7].ToString();
        }
        if (DefsGame.FaceAvailable[9] == 1)
        {
            skin10.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin10.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin10.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin10.GetComponentInChildren<Text>().text = DefsGame.FacePrice[8].ToString();
        }
        if (DefsGame.FaceAvailable[10] == 1)
        {
            skin11.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin11.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin11.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin11.GetComponentInChildren<Text>().text = DefsGame.FacePrice[9].ToString();
        }
        if (DefsGame.FaceAvailable[11] == 1)
        {
            skin12.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.white;
            skin12.GetComponentInChildren<Text>().text = "";
        }
        else
        {
            skin12.GetComponentInChildren<UIButton>().GetComponent<Image>().color = Color.black;
            skin12.GetComponentInChildren<Text>().text = DefsGame.FacePrice[10].ToString();
        }
    }

    public void CheckAvailableSkin()
    {
        for (var i = 1; i < DefsGame.FaceAvailable.Length; i++)
            if (DefsGame.FaceAvailable[i] == 0 && DefsGame.CoinsCount >= DefsGame.FacePrice[i - 1])
            {
                haveNewSkin.SetActive(true);
                //return true;
                return;
            }
        haveNewSkin.SetActive(false);
        //return false;
    }

    public bool CheckAvailableSkinBool()
    {
        for (var i = 1; i < DefsGame.FaceAvailable.Length; i++)
            if (DefsGame.FaceAvailable[i] == 0 && DefsGame.CoinsCount >= DefsGame.FacePrice[i - 1])
            {
                haveNewSkin.SetActive(true);
                return true;
            }
        haveNewSkin.SetActive(false);
        return false;
    }
}