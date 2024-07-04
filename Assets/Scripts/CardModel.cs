using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardModel
{
    public int cardId;
    public string name;
    public int cost;
    public int power;
    public Sprite icon;
    public int mana;
    public int needmana;

    public bool canUse = false;
    public bool PlayerCard = false;
    public bool FieldCard = false;
    public bool canAttack = false;
    public bool ManaCard = false;
    public bool SRuse = false;
    public bool SRcan = false;

    public CardModel(int cardID, bool playerCard) // �f�[�^���󂯎��A���̏���
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);
        if (cardEntity == null)
        {
            Debug.LogError("CardEntity not found for cardID: " + cardID);
            return; // 途中で処理を切るゾ!
        }

        cardId = cardEntity.cardId;
        name = cardEntity.name;
        cost = cardEntity.cost;
        power = cardEntity.power;
        icon = cardEntity.icon;
        mana = cardEntity.mana;
        needmana = cardEntity.needmana;
        PlayerCard = playerCard;
    }
    
}