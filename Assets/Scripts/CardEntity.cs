using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]

public class CardEntity : ScriptableObject
{
    public int cardId;
    public new string name;
    public int cost;
    public int power;
    public int mana;
    public int needmana;
    public int manaplus;
    public int manapluspuls;
    public Sprite icon;
    public CardEntity changeCard;
    public int canChangeCount;
    public int canUseCount;
}
