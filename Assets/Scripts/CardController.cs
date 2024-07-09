using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view; // カードの見た目の処理
    public CardModel model; // カードのデータを処理
    public CardMovement movement;  // �ړ�(movement)�Ɋւ��邱�Ƃ𑀍�

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
        model.FieldCard = true; // �t�B�[���h�̃J�[�h�̃t���O�𗧂Ă�
        model.ManaCard = true;
    }
    public void SpornCard(int cardID, bool playerCard) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard); // カードデータを生成
        view.Show(model); // 表示
        model.FieldCard = true; // �t�B�[���h�̃J�[�h�̃t���O�𗧂Ă�
        view.SetManaPanel(true);
        if(GameManager.instance.JS == true && GameManager.instance.playerManaPoint >0)
        {
            model.mana += 1;
            GameManager.instance.playerManaPoint -= 1;
            GameManager.instance.JS = false;
            GameManager.instance.SetManaCard();
            view.manas.text = model.mana.ToString();
        }
    }
    public void SRSporn(int cardID, bool playerCard) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard); // カードデータを生成
        view.Show(model); // 表示
        model.FieldCard = true;// �t�B�[���h�̃J�[�h�̃t���O�𗧂Ă�
        model.SRuse = true;
        model.mana += 10;
    }
    public void Sle(int cardID, bool playerCard) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID, playerCard); // カードデータを生成
        view.Show(model); // 表示
        view.SetCanUsePanel(true);
        view.SetAap(true);
    }

    public void DestroyCard(CardController card)
    {
        Destroy(card.gameObject);
    }

    public void DropField()
    {
        GameManager.instance.ReduceManaPoint(model.cost);
        GameManager.instance.CardEffect(model.cardId);
        model.FieldCard = true; // �t�B�[���h�̃J�[�h�̃t���O�𗧂Ă�
        model.canUse = false;
        if (model.cardId == 1)
        {
            model.SRuse = true;
        }
        view.SetCanUsePanel(model.canUse); // �o��������CanUsePanel������
        Destroy(this.gameObject);
    }
    public void BomberF()
    {
        GameManager.instance.PanelOn();
        GameObject enemyField = transform.parent.gameObject;
        CardController[] cardList = enemyField.GetComponentsInChildren<CardController>();
        foreach(CardController Ecard in cardList)
        {
            Ecard.view.SetBomPanel(false);
        }
    }
    public void SelectEffect(CardController card)
    {
        if(GameManager.instance.KP == true)
        {
            int CardID = card.model.cardId;
            GameManager.instance.CosshonK(CardID);
            GameManager.instance.KP = false;
        }
        if(GameManager.instance.JS == true)
        {
            int CardID = card.model.cardId;
            GameManager.instance.CosshonJ(CardID);
        }
        if(GameManager.instance.CPC == true)
        {
            card.model.power += 1;
            GameManager.instance.CPC = false;
            GameManager.instance.Shuffle();
            GameManager.instance.AppOffer();
        }
        if (GameManager.instance.CMC == true)
        {
            card.model.manapluspuls += 1;
            GameManager.instance.CMC = false;
            GameManager.instance.AppOffer();
        }
    }
}