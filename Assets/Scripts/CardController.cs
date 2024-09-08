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
    public void ManaSpawn(bool isManaPlus) // カードを生成した時に呼ばれる関数
    {
        model = new CardModel(0, true); // カードデータを生成
        view.Show(model); // 表示
        if(!isManaPlus)
        {
            model.FieldCard = true; // �t�B�[���h�̃J�[�h�̃t���O�𗧂Ă�
            model.ManaCard = true;
        }else
        {
            model.FieldCard = true;
            view.SetBreakPanel(true);
        }
    }
    public void SpawnCard(int cardID, bool playerCard) // カードを生成した時に呼ばれる関数
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
        Destroy(gameObject);
        GameManager.instance.CardEffectStart(model.cardId);
        //StartCoroutine(GameManager.instance.CardEffect(model.cardId));
    }

    public void BomStart()
    {
        StartCoroutine(BomberF());
    }
    public IEnumerator BomberF()
    {
        GameObject enemyField = transform.parent.gameObject;
        CardController[] cardList = enemyField.GetComponentsInChildren<CardController>();
        foreach(CardController ECard in cardList)
        {
            ECard.view.SetBomPanel(false);
        }
        yield return StartCoroutine(BreakCard());
        GameManager.instance.Lock = false;
        GameManager.instance.PanelOn();
        GameManager.instance.SetCanUsePanelHand();
    }
    public void GunChange()//スナイパーライフルorピストルに変化。カードのSRChangeButtonから発動
    {
        if(model.canAttack)
        {
            if(!model.isChange)
            {
                model.name = model.changeCard.name;
                model.power = model.changeCard.power;
                model.icon = model.changeCard.icon;
                model.isChange = true;
            }else
            {
                model.name = model.rootCard.name;
                model.power = model.rootCard.power;
                model.icon = model.rootCard.icon;
                model.isChange = false;
            }
            view.Show(model);
        }
    }

    public IEnumerator BreakCard()
    {
        for(int i = 0; i < 2; i++)
        {
            view.SetBreakPanel(true);
            yield return new WaitForSeconds(0.1f);
            view.SetBreakPanel(false);
            yield return new WaitForSeconds(0.1f);
        }
        for(int i = 0; i < 2; i++)
        {
            view.SetBreakPanel(true);
            yield return new WaitForSeconds(0.05f);
            view.SetBreakPanel(false);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }

    public void SelectEffect(CardController card)
    {
        if(GameManager.instance.KP == true)
        {
            int CardID = card.model.cardId;
            GameManager.instance.Lock = false;
            GameManager.instance.CosshonK(CardID);
        }
        if(GameManager.instance.JS == true)
        {
            int CardID = card.model.cardId;
            GameManager.instance.Lock = false;
            GameManager.instance.CosshonJ(CardID);
        }
        if(GameManager.instance.CPC == true)
        {
            card.model.power += 1;
            card.model.isSuko = true;
            GameManager.instance.CPC = false;
            GameManager.instance.AppOffer();
            GameManager.instance.Lock = false;
            GameManager.instance.PanelOn();
            GameManager.instance.SetCanUsePanelHand();
        }
        if (GameManager.instance.CMC == true)
        {
            card.model.manaPlusPlus += 1;
            card.model.isKakumaga = true;
            GameManager.instance.CMC = false;
            GameManager.instance.AppOffer();
            GameManager.instance.Lock = false;
            GameManager.instance.PanelOn();
            GameManager.instance.SetCanUsePanelHand();
        }
    }
}