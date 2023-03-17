using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class FengYunBianHuanRP : MonoBehaviour
{
    public Text LogText;
    public Button TakeHandCardsButton;
    public Button TakeMessageButton;
    public GridLayoutGroup CardsBox;
    public UICard boxCard;

    private fengyunbianhuanModel model;

    private void Start()
    {
        //����boxcards��������ʱ����UI����ӿ���
        model.boxCards
            .ObserveAdd()
            .Subscribe(_ =>
            {
                UICard card = Instantiate(boxCard, CardsBox.transform);
                //card.transform.SetParent(CardsBox.transform, false);
                card.Init(1, _.Value);
            })
            .AddTo(this);

        //����boxcards�������ʱ���Ƴ�UI�е���Ӧ����
        model.boxCards
            .ObserveRemove()
            .Subscribe(_ =>
            {
                foreach (var card in CardsBox.GetComponentsInChildren<UICard>())
                {
                    if(card.cardId == _.Value.id)
                    {
                        Destroy(card.gameObject);
                    }
                }
            })
            .AddTo(this);

        //�������boxcards����Ϊ�գ��������Ʊ�����̣�����UI
        model.boxCards
            .ObserveCountChanged()
            .Skip(1)
            .Select(_ => _ == 0)
            .Subscribe(_ =>
            {
                Destroy(gameObject);
            })
            .AddTo(this);

        //�ֵ��Լ�ѡ��ʱ����ѡ������Ӽ�������ȡ��ѡ��Ƭid����ɫ
        model.isTarget
            .Select(_ => _ == true)
            .Subscribe(_ =>
            {
                foreach (var card in CardsBox.GetComponentsInChildren<UICard>())
                {
                    CardColorEnum cardColor = new CardColorEnum();
                    foreach (var cardfs in model.boxCards)
                    {
                        if(card.cardId == cardfs.id)
                        {
                            if (cardfs.color.Contains(CardColorEnum.Black))
                            {
                                cardColor = CardColorEnum.Black;
                            }
                            else
                            {
                                if(cardfs.color.Contains(CardColorEnum.Red))
                                {
                                    cardColor = CardColorEnum.Red;
                                }
                                else
                                {
                                    cardColor = CardColorEnum.Blue;
                                }
                            }
                        }
                    }
                    card.SetClickAction(() =>
                    {
                        model.chooseCardInfo.Add(card.cardId, cardColor);
                    });
                }
                
            }).AddTo(this);

        //����choosecard����ʱ����������ύ��ť״̬
        model.chooseCardInfo
            .ObserveAdd()           
            .Subscribe(_ =>
            {
                if(model.isTarget.Value == true)
                {
                    TakeHandCardsButton.interactable = true;
                    if((_.Value == CardColorEnum.Black && GameManager.Singleton.players[GameManager.SelfPlayerId].GetMessageCount(CardColorEnum.Black) == 0)
                    || (_.Value == CardColorEnum.Red && GameManager.Singleton.players[GameManager.SelfPlayerId].GetMessageCount(CardColorEnum.Red) == 0)
                    || (_.Value == CardColorEnum.Blue && GameManager.Singleton.players[GameManager.SelfPlayerId].GetMessageCount(CardColorEnum.Blue) == 0))
                    {
                        TakeMessageButton.interactable = true;
                    }
                    else
                    {
                        TakeMessageButton.interactable = false;
                    }
                }
            }).AddTo(this);

            
    }

    //��ʼ��UI������ģ��
    public void InitUI(List<CardFS> cards)
    {
        model = new fengyunbianhuanModel(cards);
    }

    public void SetTarget(bool isTarget)
    {
        model.isTarget.Value = isTarget;
    }

    public CardFS PopCardFromBox(int cardId)
    {
        for (int i = 0; i < model.boxCards.Count; i++)
        {
            var card = model.boxCards[i];
            if (card.id == cardId)
            {
                model.boxCards.RemoveAt(i);
                return card;
            }
        }
        // If card with the specified id is not found, return null or throw an exception
        return null; // or throw new Exception("Card not found in the box");
    }

    public void TakeHandCard()
    {
        int selfId = GameManager.SelfPlayerId;
        int cardId = model.chooseCardInfo.Last().Key;
        GameManager.Singleton.SendFengYunBianHuanChooseCardToHandCard(selfId, cardId);
        TakeMessageButton.interactable = false;
        TakeHandCardsButton.interactable = false;
    }

    public void TakeMessage()
    {
        int selfId = GameManager.SelfPlayerId;
        int cardId = model.chooseCardInfo.Last().Key;
        GameManager.Singleton.SengFengyunBianHuanChooseCardToMessage(selfId, cardId);
        TakeMessageButton.interactable = false;
        TakeHandCardsButton.interactable = false;
    }
}
