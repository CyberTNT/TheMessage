﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public int SelfPlayerId = 0;
    public bool dir;
    public GameUI gameUI;

    public Dictionary<int, Player> players = new Dictionary<int, Player>();
    public Dictionary<int, CardFS> cardsHand = new Dictionary<int, CardFS>(); //<id, card>

    public PhaseEnum curPhase { get; private set; }
    public SecretTaskEnum task { get; private set; }

    public int CurTurnPlayerId { get; private set; }
    public int CurMessagePlayerId { get; private set; }
    public int CurWaitingPlayerId { get; private set; }
    public bool IsBingShiTan { get; private set; }

    public int SelectCardId
    {
        get { return _SelectCardId; }
        set
        {
            if (gameUI.Cards.ContainsKey(_SelectCardId)) gameUI.Cards[_SelectCardId].OnSelect(false);
            if (_SelectCardId == value)
            {
                _SelectCardId = -1;
            }
            else if (value == -1)
            {
                _SelectCardId = value;
            }
            else
            {
                _SelectCardId = value;
                gameUI.Cards[_SelectCardId].OnSelect(true);
            }
            SelectPlayerId = -1;
            Debug.Log("cardId" + _SelectCardId);
        }
    }
    private int _SelectCardId = -1;

    public int SelectPlayerId
    {
        get { return _SelectPlayerId; }
        set
        {
            if (gameUI.Players.ContainsKey(_SelectPlayerId)) gameUI.Players[_SelectPlayerId].OnSelect(false);
            if (_SelectPlayerId == value)
            {
                _SelectPlayerId = -1;
            }

            // 取消选中玩家
            if (value == -1)
            {
                _SelectPlayerId = value;
            }
            // 判断出牌时选中玩家
            else if (cardsHand.ContainsKey(_SelectCardId))
            {
                switch (cardsHand[_SelectCardId].cardName)
                {
                    case CardNameEnum.Wei_Bi:
                    case CardNameEnum.Ping_Heng:
                    case CardNameEnum.Shi_Tan:
                        if (value == SelfPlayerId)
                        {
                            string name = LanguageUtils.GetCardName(cardsHand[_SelectCardId].cardName);
                            Debug.LogError("不能选自己作为" + name + "的目标");
                        }
                        else if (gameUI.Players.ContainsKey(value))
                        {
                            _SelectPlayerId = value;
                            gameUI.Players[_SelectPlayerId].OnSelect(true);
                        }
                        break;
                    case CardNameEnum.Li_You:
                        if (gameUI.Players.ContainsKey(value))
                        {
                            _SelectPlayerId = value;
                            gameUI.Players[_SelectPlayerId].OnSelect(true);
                        }
                        break;
                }
            }
            Debug.Log("_SelectPlayerId" + _SelectPlayerId);
        }
    }
    private int _SelectPlayerId = -1;

    public uint seqId;
    //public int topColor; // 黑色牌声明的颜色
    //public int topCardCount;
    //public int wantColor;

    public int onTurnPlayerId = -1;
    //public 
    private static GameManager gameManager;
    private int DeckNum;

    public static GameManager Singleton
    {
        get
        {
            if (gameManager == null)
            {
                gameManager = new GameManager();
                gameManager.Init();
            }
            return gameManager;
        }
    }
    private GameManager()
    {

    }

    public void Init()
    {
        GameObject windowGo = GameObject.Find("GameUI");
        if (gameUI == null && windowGo != null)
        {
            gameUI = windowGo.GetComponent<GameUI>();
            if (gameUI == null)
            {
                gameUI = windowGo.AddComponent<GameUI>();
            }
        }
        else
        {
            //TODO
        }
    }

    private void InitDatas()
    {

    }
    public void InitPlayers(int num)
    {
        players.Clear();
        for (int i = 0; i < num; i++)
        {
            Player player = new Player(i);
            players.Add(i, player);
        }
    }
    private void InitCards(List<CardFS> cards)
    {
        foreach (var card in cards)
        {
            card.isHand = true;
            this.cardsHand.Add(card.id, card);
        }
    }

    public PlayerColorEnum GetPlayerColor()
    {
        return players[SelfPlayerId].playerColor[0];
    }

    private void OnCardUse(int user, CardFS cardUsed, int target = -1)
    {
        if (players.ContainsKey(user))
        {
            players[user].cardCount = players[user].cardCount - 1;
        }
        if (user == SelfPlayerId && cardsHand.ContainsKey(cardUsed.id))
        {
            cardsHand.Remove(cardUsed.id);
        }
        else if(user == SelfPlayerId && !cardsHand.ContainsKey(cardUsed.id))
        {
            Debug.LogError("no card in hand," + cardUsed.id);
        }
        gameUI.OnUseCard(user, target, cardUsed);
        string targetInfo;
        targetInfo = target == -1 ? "" : "对" + target + "号玩家";
        gameUI.AddMsg(string.Format("{0}号玩家{1}使用了{2};", user, targetInfo, LanguageUtils.GetCardName(cardUsed.cardName)));
    }

    #region 服务器消息处理

    // 通知客户端：初始化游戏
    public void OnReceiveGameStart(int player_num, PlayerColorEnum playerColor, SecretTaskEnum secretTask)
    {
        task = secretTask;

        InitPlayers(player_num);
        players[SelfPlayerId].playerColor = new List<PlayerColorEnum>() { playerColor };
        gameUI.InitPlayers(player_num);

        InitCards(new List<CardFS>());
        gameUI.InitCards(0);
        //gameUI.AddMsg(string.Format("你摸了{0}张牌, {1}", cards.Count, GetCardsInfo(cards)));
    }
    // 自己摸牌
    public void OnReceivePlayerDrawCards(List<CardFS> cards)
    {
        string cardInfo = "";
        foreach (var card in cards)
        {
            cardsHand[card.id] = card;
            cardInfo += LanguageUtils.GetCardName(card.cardName) + ",";
        }
        //DeckNum = DeckNum - 1;
        //SetDeckNum(DeckNum);
        int total = players[SelfPlayerId].DrawCard(cards.Count);
        gameUI.DrawCards(cards);
        if (gameUI.Players[SelfPlayerId] != null) gameUI.Players[SelfPlayerId].OnDrawCard(total, cards.Count);
        gameUI.AddMsg(string.Format("你摸了{0}张牌; {1}", cards.Count, cardInfo));

    }
    //玩家弃牌
    public void OnReceiveDiscards(int playerId, List<CardFS> cards)
    {
        //Debug.LogError("" + playerId + "号玩家弃牌 " +  cards.Count);
        string cardInfo = "";
        if (players.ContainsKey(playerId))
        {
            players[playerId].cardCount = players[playerId].cardCount - cards.Count;
        }
        if (gameUI.Players.ContainsKey(playerId))
        {
            gameUI.Players[playerId].Discard(cards);
        }

        if (playerId == SelfPlayerId)
        {
            foreach (var card in cards)
            {
                int cardId = card.id;
                if (cardsHand.ContainsKey(cardId)) cardsHand.Remove(cardId);

                cardInfo += LanguageUtils.GetCardName(card.cardName) + ",";
            }
            gameUI.DisCards(cards);
        }
        gameUI.AddMsg(string.Format("{0}号玩家弃了{1}张牌; {2}", playerId, cards.Count, cardInfo));

    }
    //其他角色摸牌
    public void OnReceiveOtherDrawCards(int id, int num)
    {
        int total = players[id].DrawCard(num);
        if (gameUI.Players[id] != null)
        {
            gameUI.Players[id].OnDrawCard(total, num);
        }
        gameUI.AddMsg(string.Format("{0}号玩家摸了{1}张牌", id, num));
    }
    // 通知客户端，到谁的哪个阶段了
    public void OnReceiveTurn(int playerId, int messagePlayerId, int waitingPlayerId, PhaseEnum phase, int waitSecond, uint seqId)
    {
        if(playerId != CurTurnPlayerId)
        {
            gameUI.AddMsg(string.Format("{0}号玩家回合开始", playerId));
        }

        //Debug.Log("____________________OnTurn:" + playerId + "," + messagePlayerId + "," + waitingPlayerId);
        if (waitingPlayerId == 0)
        {
            this.seqId = seqId;
        }
        curPhase = phase;
        if (CurTurnPlayerId != playerId)
        {
            gameUI.Players[CurTurnPlayerId].OnTurn(false);
        }

        if (gameUI.Players[CurTurnPlayerId] != null)
        {
            gameUI.Players[playerId]?.OnTurn(true);
        }
        CurTurnPlayerId = playerId;
        if(CurTurnPlayerId != SelfPlayerId)
        {
            gameUI.ShowWeiBiSelect(false);
        }

        if (CurMessagePlayerId != messagePlayerId)
        {
            gameUI.Players[CurMessagePlayerId].OnMessageTurnTo(false);
        }
        if (gameUI.Players[CurTurnPlayerId] != null)
        {
            gameUI.Players[messagePlayerId]?.OnMessageTurnTo(true);
        }
        CurMessagePlayerId = messagePlayerId;

        if (CurWaitingPlayerId != messagePlayerId)
        {
            gameUI.Players[CurWaitingPlayerId].OnWaiting(0);
        }
        if (gameUI.Players[CurTurnPlayerId] != null)
        {
            gameUI.Players[waitingPlayerId]?.OnWaiting(waitSecond);
        }
        CurWaitingPlayerId = waitingPlayerId;

        //gameUI.SetTurn();
    }

    // 通知客户端，谁对谁使用了试探
    public void OnRecerveUseShiTan(int user, int targetUser, int cardId = 0)
    {
        CardFS card = null;
        if (players.ContainsKey(user))
        {
            players[user].cardCount = players[user].cardCount - 1;
        }
        if (user == SelfPlayerId && cardsHand.ContainsKey(cardId))
        {
            card = cardsHand[cardId];
            cardsHand.Remove(cardId);
        }
        //Debug.LogError("________________ OnRecerveUseShiTan," + cardId);
        gameUI.OnUseCard(user, targetUser, card);

        gameUI.AddMsg(string.Format("{0}号玩家对{1}号玩家使用了试探;", user, targetUser ));
    }
    // 向被试探者展示试探，并等待回应
    public void OnReceiveShowShiTan(int user, int targetUser, CardFS card, int waitingTime, uint seqId)
    {
        this.seqId = seqId;
        if (gameUI.Players.ContainsKey(CurWaitingPlayerId))
        {
            gameUI.Players[CurWaitingPlayerId].OnWaiting(0);
        }
        if (gameUI.Players.ContainsKey(targetUser))
        {
            gameUI.Players[targetUser].OnWaiting(waitingTime);
        }
        //自己是被使用者，展示
        if (targetUser == SelfPlayerId)
        {
            IsBingShiTan = true;
            gameUI.ShowShiTanInfo(card, waitingTime);
        }
    }
    // 被试探者执行试探
    public void OnReceiveExcuteShiTan(int playerId, bool isDrawCard)
    {
        if (playerId == SelfPlayerId)
        {
            gameUI.HideShiTanInfo();
        }
    }
    // 通知客户端使用利诱的结果
    public void OnRecerveUseLiYou(int user, int target, CardFS cardUsed, CardFS card, bool isJoinHand)
    {
        OnCardUse(user, cardUsed, target);

        gameUI.ShowTopCard(card);
        if (isJoinHand)
        {
            if (players.ContainsKey(user))
            {
                players[user].cardCount += 1;

                gameUI.AddMsg(string.Format("{0}号玩家将牌堆顶的{1}加入手牌", user, LanguageUtils.GetCardName(card.cardName)));
            }

            if(user == SelfPlayerId)
            {
                gameUI.DrawCards(new List<CardFS>() { card });
            }
        }
        else
        {
            if (players.ContainsKey(target))
            {
                players[target].AddMessage(card);

                gameUI.AddMsg(string.Format("{0}号玩家将牌堆顶的{1}收为情报", target, LanguageUtils.GetCardName(card.cardName)));
            }
        }
    }
    // 通知客户端使用平衡的结果 //弃牌部分走 OnReceiveDiscards
    public void OnReceiveUsePingHeng(int user, int target, CardFS cardUsed)
    {
        OnCardUse(user, cardUsed, target);
    }
    // 通知所有人威逼的牌没有，展示所有手牌
    public void OnReceiveUseWeiBiShowHands(int user, int target, CardFS cardUsed, List<CardFS> cards)
    {
        OnCardUse(user, cardUsed, target);
        if(user == SelfPlayerId)
        {
            string cardInfo = "";
            foreach (var card in cards)
            {
                cardsHand[card.id] = card;
                cardInfo += LanguageUtils.GetCardName(card.cardName) + ",";
            }
            gameUI.AddMsg(string.Format("{0}号玩家向你展示了手牌，{1}", target, cardInfo));
        }
    }
    // 通知所有人威逼等待给牌
    public void OnReceiveUseWeiBiGiveCard(int user, int target, CardFS cardUsed, CardNameEnum cardWant, int waitTime, uint seq)
    {
        OnCardUse(user, cardUsed, target);

        this.seqId = seq;
        if (gameUI.Players.ContainsKey(CurWaitingPlayerId))
        {
            gameUI.Players[CurWaitingPlayerId].OnWaiting(0);
        }
        if (gameUI.Players.ContainsKey(target))
        {
            gameUI.Players[target].OnWaiting(waitTime);
        }

        if(target == SelfPlayerId)
        {
            gameUI.ShowWeiBiGiveCard(cardWant, user, waitTime);
        }
        //Debug.LogError(cardUsed.cardName);
    }

    // 通知所有人威逼给牌
    public void OnReceiveExcuteWeiBiGiveCard(int user, int target, CardFS cardGiven)
    {
        int total = players[user].DrawCard(1);
        players[target].cardCount = players[target].cardCount - 1;

        if (gameUI.Players[user] != null)
        {
            gameUI.Players[user].OnDrawCard(total, 1);
        }

        if (gameUI.Players.ContainsKey(target))
        {
            gameUI.Players[target].Discard(new List<CardFS>() {cardGiven });
        }

        if(user == SelfPlayerId)
        {
            cardsHand[cardGiven.id] = cardGiven;
            gameUI.DrawCards(new List<CardFS>() { cardGiven });
        }
        if(SelfPlayerId == target)
        {
            cardsHand.Remove(cardGiven.id);
            gameUI.DisCards(new List<CardFS>() { cardGiven });
        }

        gameUI.AddMsg(string.Format("{0}号玩家给了{1}号玩家一张牌{2}", target, user, LanguageUtils.GetCardName(cardGiven.cardName)));
    }
    #endregion


    #region 向服务器发送请求
    public void SendEndWaiting()
    {
        ProtoHelper.SendEndWaiting(seqId);
    }

    public void SendUseCard()
    {
        if (SelectCardId != -1 && cardsHand.ContainsKey(SelectCardId))
        {
            CardNameEnum card = cardsHand[SelectCardId].cardName;
            switch (card)
            {
                //使用试探
                case CardNameEnum.Shi_Tan:
                    if (SelectPlayerId != -1 && SelectPlayerId != 0)
                    {
                        ProtoHelper.SendUseCardMessage_ShiTan(SelectCardId, SelectPlayerId, this.seqId);
                    }
                    else
                    {
                        Debug.LogError("请选择正确的试探目标");
                    }
                    break;
                //使用威逼, 只打开选择界面， 不发送请求
                case CardNameEnum.Wei_Bi:
                    if (SelectPlayerId != -1 && SelectPlayerId != 0)
                    {
                        gameUI.ShowWeiBiSelect(true);
                        return;
                    }
                    else
                    {
                        Debug.LogError("请选择正确的威逼目标");
                    }
                    break;
                //使用利诱
                case CardNameEnum.Li_You:
                    if (SelectPlayerId != -1)
                    {
                        ProtoHelper.SendUseCardMessage_LiYou(SelectCardId, SelectPlayerId, this.seqId);
                    }
                    else
                    {
                        Debug.LogError("请选择正确的利诱目标");
                    }
                    break;
                //使用平衡
                case CardNameEnum.Ping_Heng:
                    if (SelectPlayerId != -1)
                    {
                        ProtoHelper.SendUseCardMessage_PingHeng(SelectCardId, SelectPlayerId, this.seqId);
                    }
                    else
                    {
                        Debug.LogError("请选择正确的平衡目标");
                    }
                    break;
            }
        }

        SelectCardId = -1;
    }
    public void SendDoShiTan(int cardId)
    {
        ProtoHelper.SendDoShiTan(cardId, seqId);
        SelectCardId = -1;
    }

    public void SendDoWeiBi(int cardId)
    {
        Debug.LogError("威逼给牌" + cardId);
        ProtoHelper.SendDoWeiBi(cardId, seqId);
        SelectCardId = -1;
    }

    public void DrawCard()
    {
        //ProtoHelper.SendDiscardMessage(0, 0);

    }

    #endregion
}

public enum SecretTaskEnum
{
    Killer = 0, // 你的回合中，一名红色和蓝色情报合计不少于2张的人死亡
    Stealer = 1, // 你的回合中，有人宣胜，则你代替他胜利
    Collector = 2, // 你获得3张红色情报或者3张蓝色情报
}

public enum PhaseEnum
{
    Draw_Phase = 0,   // 摸牌阶段
    Main_Phase = 1,   // 出牌阶段
    Send_Phase = 2,   // 传递阶段
    Fight_Phase = 3,   // 争夺阶段
    Receive_Phase = 4, // 接收阶段
}
