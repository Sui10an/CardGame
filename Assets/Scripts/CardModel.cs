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
    public int needMana;
    public int manaPlus;
    public int manaPlusPlus;

    public bool canUse = false;
    public bool PlayerCard = false;
    public bool canAttack = false;
    public CardEntity rootCard;
    public CardEntity changeCard;
    public bool isChange;
    public int changeCount;
    public int canChangeCount;
    public int canUseCount;
    public int useCount;
    public bool isKakumaga;
    public bool isSuko;
    public Kinds kinds = Kinds.other;
    public enum Kinds
    {
        hand,
        mana,
        manaPlus,
        playerGun,
        enemyGun,
        other

    }

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
        needMana = cardEntity.needmana;
        manaPlus = cardEntity.manaplus;
        manaPlusPlus = cardEntity.manapluspuls;
        PlayerCard = playerCard;
        rootCard = cardEntity;
        changeCard = cardEntity.changeCard;
        canChangeCount = cardEntity.canChangeCount;
        canUseCount = cardEntity.canUseCount;
    }
}