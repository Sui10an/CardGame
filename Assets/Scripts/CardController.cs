using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view; // カードの見た目の処理
    public CardModel model; // カードのデータを処理
    public CardMovement movement;  // 移動(movement)に関することを操作

    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }

    public void Init(int cardID, bool playerCard) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard); // カードデータを生成
        view.Show(model); // 表示
    }
    public void ManaSporn(int cardID, bool playerCard) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard); // カードデータを生成
        view.Show(model); // 表示
        model.FieldCard = true; // フィールドのカードのフラグを立てる
        model.ManaCard = true;
    }
    public void SpornCard(int cardID, bool playerCard) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard); // カードデータを生成
        view.Show(model); // 表示
        model.FieldCard = true; // フィールドのカードのフラグを立てる
    }
    public void SRSporn(int cardID, bool playerCard) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard); // カードデータを生成
        view.Show(model); // 表示
        model.FieldCard = true;// フィールドのカードのフラグを立てる
        model.SRuse = true;
        model.mana += 10;
    }

    public void DestroyCard(CardController card)
    {
        Destroy(card.gameObject);
    }

    public void DropField()
    {
        GameManager.instance.ReduceManaPoint(model.cost);
        GameManager.instance.CardEffect(model.cardId);
        model.FieldCard = true; // フィールドのカードのフラグを立てる
        model.canUse = false;
        if (model.cardId == 1)
        {
            model.SRuse = true;
        }
        view.SetCanUsePanel(model.canUse); // 出した時にCanUsePanelを消す
        Destroy(this.gameObject);
    }
    
}