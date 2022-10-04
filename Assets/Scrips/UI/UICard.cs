using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animator;
    public Button button;
    public Text textName;
    public GameObject goArrowUp;
    public GameObject goArrowLeft;
    public GameObject goArrowRight;
    public Image imgColor;
    public Image imgColor2;
    public Image image;
    public GameObject goLock;
    public Text textShitan;
    public GameObject goShiTan;
    public Transform transContainer;
    public GameObject goUnknown;

    private int cardId;
    private CardNameEnum cardName;
    private List<PlayerColorEnum> shiTanColor;

    public string cardDes
    {
        get
        {
            if (cardId > 0)
            {
                switch (cardName)
                {
                    case CardNameEnum.ChengQing:
                        return "���壺���ƽ׶Σ�����һ����ɫ��ǰ��һ�ź��鱨����Ҳ������һ����ɫ����ʱ����ʹ��";
                    case CardNameEnum.DiaoBao:
                        return "����������׶Σ����������泯�´���������鱨��";
                    case CardNameEnum.JieHuo:
                        return "�ػ�����׶Σ����������鱨�ƶ�������ǰ��";
                    case CardNameEnum.LiYou:
                        return "���գ����ƽ׶Σ�ѡ��һ����ɫ�����ƶѶ���һ�������������鱨������������������ռ����Ż����ͬɫ�鱨�����Ϊ�����鱨����������ơ�";
                    case CardNameEnum.PingHeng:
                        return "ƽ�⣺���ƽ׶Σ�ѡ��һ��������ɫ����������������������ƣ���������3���ơ�";
                    case CardNameEnum.PoYi:
                        return "���룺�鱨���ݽ׶Σ����鱨���ݵ��Լ���ǰʱʹ�á����������鱨����Ϊ��ɫ������Խ��䷭������1����";
                    case CardNameEnum.ShiTan:
                        string shitan = "";
                        shitan += shiTanColor.Contains(PlayerColorEnum.Blue) ? LanguageUtils.GetIdentityName(PlayerColorEnum.Blue) : "";
                        shitan += shiTanColor.Contains(PlayerColorEnum.Red) ? LanguageUtils.GetIdentityName(PlayerColorEnum.Red) : "";
                        shitan += shiTanColor.Contains(PlayerColorEnum.Green) ? LanguageUtils.GetIdentityName(PlayerColorEnum.Green) : "";
                        shitan += ":��һ����\n";
                        shitan += shiTanColor.Contains(PlayerColorEnum.Blue) ? "" : LanguageUtils.GetIdentityName(PlayerColorEnum.Blue);
                        shitan += shiTanColor.Contains(PlayerColorEnum.Red) ? "" : LanguageUtils.GetIdentityName(PlayerColorEnum.Red);
                        shitan += shiTanColor.Contains(PlayerColorEnum.Green) ? "" : LanguageUtils.GetIdentityName(PlayerColorEnum.Green);
                        shitan += ":��һ����\n";

                        return "���ƽ׶Σ����������泯�½���һ��������ɫ�����������Լ����������ʵִ��:\n" + shitan + "ִ��֮���������Ƴ���Ϸ";
                    case CardNameEnum.WeiBi:
                        return "���ƣ����ƽ׶Σ�ѡ��һ��������ɫ������ �����塿���ػ񡿡����������󵼡��е�һ���������Է��������һ���������Ŀ������Է�û�У�����������չʾ���㡣";
                    case CardNameEnum.WuDao:
                        return "�󵼣�����׶Σ����������鱨�ƶ�����ǰ��ɫ���ڵĽ�ɫ��ǰ��";
                }
                return "";
            }
            else
            {
                return "";
            }
        }
    }
    void Awake()
    {
        button.onClick.AddListener(() =>
        {
            if (GameManager.Singleton.cardsHand.ContainsKey(cardId))
            {
                if (GameManager.Singleton.SelectCardId == cardId)
                {
                    GameManager.Singleton.SelectCardId = -1;
                }
                else
                {
                    GameManager.Singleton.SelectCardId = cardId;
                }
            }
        });
    }

    public void SetClickAction(UnityEngine.Events.UnityAction onClick)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClick);
    }

    public void Init(int index, CardFS cardInfo = null)
    {
        if (cardInfo != null)
        {
            SetInfo(cardInfo);
        }
        gameObject.SetActive(true);
        index = index % 4;
        index = index == 0 ? 4 : index;
        animator.SetTrigger("InitCard" + index);
    }

    public void SetInfo(CardFS cardInfo)
    {
        cardId = cardInfo.id;
        cardName = cardInfo.cardName;
        shiTanColor = cardInfo.shiTanColor;
        if (cardInfo.id > 0)
        {
            goUnknown.SetActive(false);

            textName.text = LanguageUtils.GetCardName(cardInfo.cardName);
            goArrowLeft.SetActive(cardInfo.direction == DirectionEnum.Left);
            goArrowRight.SetActive(cardInfo.direction == DirectionEnum.Right);
            goArrowUp.SetActive(cardInfo.direction == DirectionEnum.Up);

            imgColor.color = GameUtils.GetCardColor(cardInfo.color[0]);
            imgColor2.gameObject.SetActive(cardInfo.color.Count > 1);
            if (cardInfo.color.Count > 1)
            {
                imgColor2.color = GameUtils.GetCardColor(cardInfo.color[1]);
            }

            goLock.SetActive(cardInfo.canLock);

            image.sprite = Resources.Load<Sprite>("Images/cards/" + cardInfo.cardName.ToString());
            if (cardInfo.cardName == CardNameEnum.ShiTan)
            {
                goShiTan.SetActive(true);
                string black = cardInfo.shiTanColor.Contains(PlayerColorEnum.Green) ? "+1" : "-1";
                string red = cardInfo.shiTanColor.Contains(PlayerColorEnum.Red) ? "+1" : "-1";
                string blue = cardInfo.shiTanColor.Contains(PlayerColorEnum.Blue) ? "+1" : "-1";
                //textShitan.text = "<color=#0000FF>" + blue + "</color>\n"
                //    + "<color=#FF0000>" + red + "</color>\n"
                //    + "<color=#000000>" + black + "</color>";
                textShitan.text = blue + "  " + red + "  " + "<color=#000000>" + black + "</color>";
            }
            else
            {
                goShiTan.SetActive(false);
            }
        }
        // ����
        else
        {
            goUnknown.SetActive(true);
        }
    }

    public void OnSelect(bool select)
    {
        float y = select ? 30 : 0;
        transContainer.localPosition = new Vector3(0, y);
    }

    public void OnSend()
    {
        Debug.Log("��������ȥ��" + textName.text);
        Destroy(gameObject);
    }

    public void OnDiscard()
    {
        //TODO
        Debug.Log("��������" + textName.text);
        Destroy(gameObject);
    }

    public bool IsUnknown()
    {
        return goUnknown.activeSelf;
    }

    public void TurnOn(CardFS cardInfo)
    {
        gameObject.SetActive(true);
        animator.SetTrigger("TurnOn");
        SetInfo(cardInfo);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
#if UNITY_STANDALONE
        if(cardId > 0)
        {
            GameUI.ShowDesInfo(cardDes, eventData.position);
        }
#endif
    }

    public void OnPointerExit(PointerEventData eventData)
    {
#if UNITY_STANDALONE
        GameUI.HideDesInfo();
#endif
    }
    private Coroutine showInfoCorout;

    public void PointerUp()
    {
#if UNITY_ANDROID
        //GameUI.HideDesInfo();
        if(showInfoCorout!=null)
        {
            StopCoroutine(showInfoCorout);
        }
#endif
    }

    public void PointerDown()
    {
#if UNITY_ANDROID
        GameUI.HideDesInfo();
        if (cardId > 0)
        {
            showInfoCorout = StartCoroutine(ShowInfo());
        }
        //GameUI.ShowDesInfo(roleDes, eventData.position);
#endif
    }

    private IEnumerator ShowInfo()
    {
        yield return new WaitForSeconds(0.2f);
        GameUI.ShowDesInfo(cardDes, transform.position);
    }
}
