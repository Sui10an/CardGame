// BAKAMANUKE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngineInternal;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uIManager;
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, playerField, enemyField,playerManaField,selectField;
    [SerializeField] GameObject useField,cover,selectUi;
    [SerializeField] TextMeshProUGUI pBuff,eBuff,pDown,eDown;
    [SerializeField] TextMeshProUGUI playerLeaderHPText,enemyLeaderHPText,pBHP,eBHP;
    [SerializeField] TextMeshProUGUI playerManaPointText, playerDefaultManaPointText;
    [SerializeField] Transform playerLeaderTransform;

    public int playerManaPoint; // �g�p�}�i�̐�
    public int playerDefaultManaPoint; // �݌v�}�i��
    public int playerManaPlus; //�}�i�����̃J�E���g
    public int TurnCount;
    public int playerBlockHP;
    public int enemyBlockHP;
    public int hPCurtainP;
    public int hPCurtainE;
    public int PBuff;
    public int EBuff;
    GridLayoutGroup _gridLayoutGroup;

    public bool isPlayerTurn = true; //??��?��@Public??��?��֕ύX
    public bool isNotBattlePhase = true;
    public bool isFielder = true;
    public bool isCurtainP = true;
    public bool isCurtainE = true;
    public bool Bom = true;
    public bool KP = true; // ケアパッケージを使�?ため
    public bool JS = true;// �?のサー�?
    public bool CPC = true;// �?のサー�?
    public bool CMC = true;
    public bool Lock;
    public int playerHurts, enemyHurts;
    public bool isBeginMatch;
    List<int> deck = new List<int>{2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,11,11,12,12,13,13,14,14,15,15};  //

    public static GameManager instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        isBeginMatch = true;
        StartStart();
    }

    public void StartStart()
    {
        uIManager.isStart = false;
        StartCoroutine(GameStart());
    }

    public void Reset()
    {
        TurnCount = 0;

        enemyLeaderHP = 10;
        playerLeaderHP = 10;
        playerBlockHP = 0;
        enemyBlockHP = 0;
        ShowLeaderHP();
        playerManaPoint = 0;
        playerDefaultManaPoint = 6;
        ShowManaPoint();

        // オプション�?ろい�?
        isCurtainP = false;
        isCurtainE = false;
        hPCurtainP = 0;
        hPCurtainE = 0;
        Curtain();
        PBuff = 0;
        EBuff = 0;
        ChangeBuff();
        playerHurts = 0;
        enemyHurts = 0;
        KP = false;
        JS = false;
        CPC = false;
        CMC = false;
        Lock = true;
        isNotBattlePhase = true;
        isPlayerTurn = true;
        cover.SetActive(false);

        //�?�?キのシャ�?フル
        deck = new List<int>{2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,11,11,12,12,13,13,14,14,15,15};
        Shuffle();

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        foreach(CardController card in playerHandCardList)
        {
            Destroy(card.gameObject);
        }
        CardController[] playerManaFieldCardList = playerManaField.GetComponentsInChildren<CardController>();
        foreach(CardController card in playerManaFieldCardList)
        {
            Destroy(card.gameObject);
        }
        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        foreach(CardController card in playerFieldCardList)
        {
            Destroy(card.gameObject);
        }
        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();
        foreach(CardController card in enemyFieldCardList)
        {
            Destroy(card.gameObject);
        }
    }

    IEnumerator GameStart() // 初期値の設�?
    {
        Reset();
        uIManager.Reset();
        if(isBeginMatch)
        {
            yield return uIManager.StartBeginMatch();
        }else
        {
            yield return uIManager.Title();
        }

        // 初期手札を�?�る
        SetStartHand();
        playerManaPoint = 3;
        ShowManaPoint();

        // 初期盤面
        CreateCard(1, playerField);
        CreateCard(1, enemyField);

        // ターンの開�?
        isPlayerTurn = true;
        StartCoroutine(TurnCalc());
    }
    public void Shuffle() // �?�?キをシャ�?フルする
    {
        // 整数 n の初期値は�?�?キの枚数
        int n = deck.Count;

        // n�?1より小さくなるまで繰り返す
        while (n > 1)
        {
            n--;

            // kは 0 ?�? n+1 の間�?�ランダ�?な値
            int k = UnityEngine.Random.Range(0, n + 1);

            // k番目のカードをtempに代入
            int temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    void ShowManaPoint() // ??��?��}??��?��i??��?��|??��?��C??��?��??��?��??��?��g??��?��??��?��\??��?��??��?��??��?��??��?��??��?��?��???��?��\??��?��b??��?��h
    {
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
        //??��?��}??��?��i??��?��J??��?��[??��?��h??��?��??��?��??��?��??��?��??��?��
        SetManaCard();
    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        // Player??��?��̎�D??��?��ɐ�??��?��??��?��??��?��??��?��??��?��ꂽ??��?��J??��?��[??��?��h??��?��??��?��Player??��?��̃J??��?��[??��?��h??��?��Ƃ�??��?��??��?��
        if (place == playerHand)
        {
            card.Init(cardID, true);
        }else
        if(place == playerField)
        {
            card.SpawnCard(cardID, true);
        }else
        {
            card.SpawnCard(cardID, false);
        }
    }
    void CreateMana(Transform place, bool isManaPlus)
    {
        CardController card = Instantiate(cardPrefab, place);
        // Player??��?��̎�D??��?��ɐ�??��?��??��?��??��?��??��?��??��?��ꂽ??��?��J??��?��[??��?��h??��?��??��?��Player??��?��̃J??��?��[??��?��h??��?��Ƃ�??��?��??��?��
        if(!isManaPlus)
        {
            card.ManaSpawn(false);
        }
        else
        {
            card.ManaSpawn(true);
        }
    }


    void DrawCard(Transform hand) // カードを引く
    {
        // �?�?キがな�?なら引かな�?
        if (deck.Count == 0)
        {
            StopAllCoroutines();
            StartCoroutine(uIManager.Result(false));
            return;
        }

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();

        if (playerHandCardList.Length < 10)
        {
            // �?�?キの一番上�?�カードを抜き取り、手札に�?える
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }
    }

    void SetStartHand() // 手札�?5枚�?�る
    {
        for (int i = 0; i < 5; i++)
        {
            DrawCard(playerHand);
        }
    }

    public void SetManaCard()
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        foreach(CardController manas in playerManaCardList)
        {
            Destroy(manas.gameObject);
        }
        for(int i = 0; i < playerManaPoint; i++)
        {
            CreateMana(playerManaField, false);
        }
        for(int i = 0; i < playerManaPlus; i++)
        {
            CreateMana(playerManaField, true);
        }
    }
    public void AddMana(int addCount)
    {
        playerManaPoint += addCount;
        if(playerManaPoint > 6)
        {
            playerManaPoint = 6;
            playerManaPlus = 0;
        }
        SetManaCard();
    }

    IEnumerator TurnCalc() //
    {
        Lock = true;
        yield return StartCoroutine(uIManager.ShowChangeTurnPanel());
        Lock = false;
        if (isPlayerTurn)
        {
            if(isNotBattlePhase)
            {
                PlayerTurn();
            }else
            {
            }
        }
        else
        {
            StartCoroutine(EnemyTurn()); // StartCoroutine??��?��ŌĂяo??��?��??��?��
        }
    }
    public void PhaseCalc()
    {
        if(!Lock)
        {
            uIManager.ShowChangePhasePanel();
        }
    }

    public void ChangeTurn() // ターンエンド�?�タンにつける処�?
    {
        isPlayerTurn = !isPlayerTurn;
        isNotBattlePhase = true;// ターンを�?にする
        PanelOff();
        uIManager.Stoper();
        ReSetCanUsePanelHand();
        ReSetCanUsePanelMana();
        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        foreach(CardController gun in playerFieldCardList)
        {
            if(gun.model.isChange)
            {
                gun.GunChange();
            }
        }
        SetAttackAbleFieldCard(playerFieldCardList, false);
        StartCoroutine(TurnCalc()); // ターンを相手に回す
    }

    void PlayerTurn()
    {
        Lock = false;
        TurnCount = TurnCount + 1;
        Debug.Log("Playerのターン");
        PanelOn();
        CardController[] playerFieldList = playerField.GetComponentsInChildren<CardController>();
        foreach(CardController card in playerFieldList)
        {
            card.model.manaPlus = 0;
            card.model.useCount = 0;
        }

        /// 弾薬箱の処�?
        CardController[] manas = playerManaField.GetComponentsInChildren<CardController>();
        foreach(CardController mana in manas)
        {
            if(!mana.model.ManaCard)
            {
                mana.model.ManaCard = true;
                mana.view.SetBreakPanel(false);
            }
        }

        AddMana(1);
        ShowManaPoint();

        if(TurnCount != 1)
        {
            DrawCard(playerHand); // 手札を一枚加える
        }
        for(int i = 0; i < playerHurts; i++)
        {
            DrawCard(playerHand);
        }
        playerHurts = 0;
        SetCanUsePanelHand();
        if(isCurtainP == true)
        {
            hPCurtainP = 0;
            isCurtainP = false;
            eDown.text = hPCurtainP.ToString();
        }
        if(PBuff != 0)
        {
            PBuff = 0;
            pBuff.text = null;
        }
    }

    public void BattlePhase()//??��?��o??��?��g??��?��??��?��??��?��t??��?��F??��?��[??��?��Y??��?��??��?��ݒ�
    {
        if (isNotBattlePhase == false)
        {
            uIManager.Stoper();
            ChangeTurn();
        }
        else
        {
            ReSetCanUsePanelHand();
            ReSetCanUsePanelMana();
            uIManager.Stoper();
            PanelOff();
            isNotBattlePhase = false;
            StartCoroutine(TurnCalc());
            CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
            SetAttackAbleFieldCard(playerFieldCardList, true);
        }
    }

    public void ReduceManaPoint(int cost) //??��?��R??��?��X??��?��g??��?��̕�??��?��A??��?��}??��?��i??��?��|??��?��C??��?��??��?��??��?��g??��?��??��?��??��?��??��?��??��?��炷
    {
        playerManaPoint -= cost;
        ShowManaPoint();

        SetCanUsePanelHand();
    }
    public IEnumerator CardEffect(int cardId) // カード�?�効果表
    {
        if (1 <= cardId && cardId <= 4) //�?の出現
        {
            CreateCard(cardId, playerField);
        }
        if (cardId == 5) //弾薬箱
        {
            playerManaPlus += 1;
            ShowManaPoint();
        }
        if (cardId == 6) //ボンバ�?�
        {
            PanelOff();
            ReSetCanUsePanelHand();
            Lock = true;
            CardController[] handList = playerHand.GetComponentsInChildren<CardController>();
            int Count = UnityEngine.Random.Range(0, handList.Length);
            CardController card1 = handList[Count];
            yield return StartCoroutine(card1.BreakCard());
            CardController[] cardList = enemyField.GetComponentsInChildren<CardController>();
            Bom = true;
            foreach(CardController card in cardList)
            {
                card.view.SetBomPanel(Bom);
            }
        }
        if(cardId == 7) //回復
        {
            if(isPlayerTurn == true)
            {
                playerLeaderHP += 2;
            }
            else
            {
                enemyLeaderHP += 2;
            }
            ShowLeaderHP();
        }
        if (cardId == 8) //�?甲1
        {
            if (isPlayerTurn == true)
            {
                playerBlockHP += 1;
            }
            else
            {
                enemyBlockHP += 1;
            }
            ShowLeaderHP();
        }
        if (cardId == 9) //光よ!
        {
            if(isPlayerTurn == true)
            {
                isCurtainP = true;
                hPCurtainP -= 1;
            }
            else
            {
                isCurtainE = true;
                hPCurtainE -= 1;
            }
            Curtain();
        }
        if (cardId == 10) //ケアパッケージ輸送中
        {
            KP = true;
            Selecter();
        }
        if (cardId == 11) //�?甲2
        {
            if (isPlayerTurn == true)
            {
                playerBlockHP += 2;
            }
            else
            {
                enemyBlockHP += 2;
            }
            ShowLeaderHP();
        }
        if (cardId == 12) //�?サー�?
        {
            JS = true;
            Selecter();
        }
        if (cardId == 13) //バッファー
        {
            if (isPlayerTurn == true)
            {
                PBuff += 1;
                ChangeBuff();
            }
            else
            {
                EBuff += 1;
                ChangeBuff();
            }
        }
        if (cardId == 14) //拡張
        {
            PanelOff();
            ReSetCanUsePanelHand();
            Lock = true;
            Kakumaga();
        }
        if (cardId == 15) //スコ
        {
            PanelOff();
            ReSetCanUsePanelHand();
            Lock = true;
            ChangePower();
        }
        SetCanUsePanelHand();
    }

    public void SetCanUsePanelMana() // ??��?��??��?��D??��?��̃J??��?��[??��?��h??��?��??��?��??��?��擾??��?��??��?��??��?��āA??��?��g??��?��p??��?��\??��?��ȃJ??��?��[??��?��h??��?��??��?��CanUse??��?��p??��?��l??��?��??��?��??��?��??��?��t??��?��??��?��??��?��??��?��
    {
        if(!Lock && isNotBattlePhase)
        {
            CardController[] guns = playerField.GetComponentsInChildren<CardController>();
            bool itokawaJoshin = false;
            foreach(CardController gun in guns)
            {
                if(gun.model.needMana > 0 && gun.model.manaPlus <= gun.model.manaPlusPlus)
                {
                    itokawaJoshin = true;
                }
            }
            if(itokawaJoshin)
            {
                CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
                foreach (CardController card in playerManaCardList)
                {
                    if(card.model.ManaCard)
                    {
                        card.model.canUse = true;
                        card.model.canAttack = true;
                        card.view.SetCanUsePanel(card.model.canUse);
                    }
                }
            }else
            {
                CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
                foreach (CardController card in playerManaCardList)
                {
                    card.model.canUse = false;
                    card.model.canAttack = false;
                    card.view.SetCanUsePanel(card.model.canUse);
                }
            }
        }
    }

    public void ReSetCanUsePanelMana() // ??��?��??��?��D??��?��̃J??��?��[??��?��h??��?��??��?��??��?��擾??��?��??��?��??��?��āA??��?��g??��?��p??��?��s??��?��\??��?��ȃJ??��?��[??��?��h??��?��ɂ�??��?��??��?��
    {
        if(!Lock)
        {
            CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
            foreach (CardController card in playerManaCardList)
            {
                card.model.canUse = false;
                card.model.canAttack = false;
                card.view.SetCanUsePanel(card.model.canUse);
            }
        }
    }
    public void SetCanUsePanelHand() // ??��?��??��?��D??��?��̃J??��?��[??��?��h??��?��??��?��??��?��擾??��?��??��?��??��?��āA??��?��g??��?��p??��?��\??��?��ȃJ??��?��[??��?��h??��?��??��?��CanUse??��?��p??��?��l??��?��??��?��??��?��??��?��t??��?��??��?��??��?��??��?��
    {
        if(!Lock && isNotBattlePhase)
        {
            CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
            //CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
            foreach (CardController card in playerHandCardList)
            {
                if (card.model.cost <= playerManaPoint)
                {
                    card.model.canUse = true;
                    card.view.SetCanUsePanel(card.model.canUse);
                    CanUse(card);
                }
                else
                {
                    card.model.canUse = false;
                    card.view.SetCanUsePanel(card.model.canUse);
                }
            }
        }
    }

    public void ReSetCanUsePanelHand() // ??��?��??��?��D??��?��̃J??��?��[??��?��h??��?��??��?��??��?��擾??��?��??��?��??��?��āA??��?��g??��?��p??��?��s??��?��\??��?��ȃJ??��?��[??��?��h??��?��ɂ�??��?��??��?��
    {
        if(!Lock)
        {
            CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
            foreach (CardController card in playerHandCardList)
            {
                card.model.canUse = false;
                card.view.SetCanUsePanel(card.model.canUse);
            }
        }
    }

    IEnumerator EnemyTurn() // StartCoroutine??��?��ŌĂ΂ꂽ??��?��̂ŁAIEnumerator??��?��ɕύX
    {
        Lock = true;
        cover.SetActive(true);
        TurnCount = TurnCount + 1;

        if (EBuff != 0)
        {
            EBuff = 0;
            eBuff.text = null;
        }

        if(hPCurtainE != 0)
        {
            hPCurtainE = 0;
            isCurtainE = false;
            pDown.text = hPCurtainE.ToString();
        }

        enemyHurts = 0;

        Debug.Log("Enemyのターン");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();
        foreach(CardController gun in enemyFieldCardList)
        {
            gun.model.useCount = 0;
        }

        yield return new WaitForSeconds(1f);

        if (enemyFieldCardList.Length < 3)
        {
            int iD = UnityEngine.Random.Range(2, 5);
            Debug.Log(iD);
            CreateCard(iD, enemyField);
        }

        yield return new WaitForSeconds(1f);

        CardController[] enemyFieldCardListSecond = enemyField.GetComponentsInChildren<CardController>();
        for(int EC = 0; EC < enemyFieldCardListSecond.Length; EC++)
        {
            CardController useCard = enemyFieldCardListSecond[EC];
            if(useCard.model.needMana != 0)
            {
                useCard.model.mana += 1;
                useCard.view.Show(useCard.model);
            }
        }

        yield return new WaitForSeconds(0.5f);

        SetAttackAbleFieldCard(enemyFieldCardListSecond, true);

        yield return new WaitForSeconds(1f);

        while (Array.Exists(enemyFieldCardListSecond, card => card.model.canAttack))
        {
            // ??��?��U??��?��??��?��??��?��\??��?��J??��?��[??��?��h??��?��??��?��??��?��擾
            CardController[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardListSecond, card => card.model.canAttack);

            CardController attackCard = enemyCanAttackCardList[0];

            yield return StartCoroutine(attackCard.movement.AttackMotion(playerLeaderTransform));
            AttackToLeader(attackCard);

            yield return new WaitForSeconds(1f);
        }
        cover.SetActive(false);
        ChangeTurn(); // ??��?��^??��?��[??��?��??��?��??��?��G??��?��??��?��??��?��h??��?��??��?��??��?��??��?��
    }


    public void CardBattle(CardController attackCard, CardController defenceCard)
    {
        // ??��?��U??��?��??��?��??��?��J??��?��[??��?��h??��?��ƍU??��?��??��?��??��?��??��?��??��?��??��?��??��?��J??��?��[??��?��h??��?��??��?��??��?��??��?��??��?��??��?��??��?��v??��?��??��?��??��?��C??��?��??��?��??��?��[??��?��̃J??��?��[??��?��h??��?��Ȃ�o??��?��g??��?��??��?��??��?��??��?��??��?��Ȃ�
        if (attackCard.model.PlayerCard == defenceCard.model.PlayerCard)
        {
            if(attackCard.model.ManaCard == true)
            {
                if(defenceCard.model.needMana == 0 || defenceCard.model.manaPlus > defenceCard.model.manaPlusPlus)
                {
                    return;
                }
                defenceCard.model.mana += 1;
                playerManaPoint -= 1;
                defenceCard.model.manaPlus += 1;
                attackCard.DestroyCard(attackCard);
                SetCanUsePanelMana();
                ChangeManaText(defenceCard);
                ShowManaPoint();
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    void SetAttackAbleFieldCard(CardController[] cardList, bool canAttack)
    {
        foreach(CardController card in cardList)
        {
            card.model.canAttack = false;
            card.view.SetCanAttackPanel(false);
            if(card.model.mana >= card.model.needMana && card.model.canUseCount > card.model.useCount)
            {
                card.model.canAttack = canAttack;
                card.view.SetCanAttackPanel(canAttack);
            }
        }
    }
    void ChangeManaText(CardController card)
    {
        card.view.manas.text = card.model.mana.ToString();
    }
    void CheckHP()
    {
        if(playerLeaderHP > 5)
        {
            return;
        }
        CardController[] guns = playerField.GetComponentsInChildren<CardController>();
        foreach (CardController card in guns)
        {
            if (card.model.cardId == 1 && card.model.changeCount < card.model.canChangeCount)
            {
                card.view.ChangeSR(true);
            }
        }

    }
    public int playerLeaderHP;
    public int enemyLeaderHP;

    public void AttackToLeader(CardController attackCard)
    {
        if (attackCard.model.canAttack == false)
        {
            return;
        }
        if(attackCard.model.PlayerCard)
        {
            int AP = attackCard.model.power + PBuff + hPCurtainE - enemyBlockHP;
            if(AP > 0)
            {
                enemyBlockHP = 0;
                enemyLeaderHP -= AP;
                enemyHurts += AP;
            }else
            {
                enemyBlockHP = -AP;
            }
        }else
        {
            int AP = attackCard.model.power + EBuff + hPCurtainP - playerBlockHP;
            if(AP > 0)
            {
                playerBlockHP = 0;
                playerLeaderHP -= AP;
                playerHurts += AP;
                CheckHP();
            }else
            {
                playerBlockHP = -AP;
            }
        }
        if(attackCard.model.isChange)
        {
            attackCard.model.changeCount++;
            if(attackCard.model.changeCount >= attackCard.model.canChangeCount)
            {
                attackCard.view.ChangeSR(false);
                attackCard.GunChange();
            }
        }
        attackCard.model.mana -= attackCard.model.needMana;
        attackCard.model.useCount++;
        if(attackCard.model.mana < attackCard.model.needMana || attackCard.model.useCount >= attackCard.model.canUseCount || attackCard.model.needMana == 0)
        {
            attackCard.model.canAttack = false;
            attackCard.view.SetCanAttackPanel(false);
        }
        attackCard.view.Show(attackCard.model);
        ShowLeaderHP();
    }

    public void ChangePower()
    {
        CPC = true;
        CardController[] ACC = playerField.GetComponentsInChildren<CardController>();
        foreach (CardController card in ACC)
        {
            if(card.model.cardId == 2){
                card.view.SetAap(true);
            }
        }
    }
    public void Kakumaga()
    {
        CMC = true;
        CardController[] ACC = playerField.GetComponentsInChildren<CardController>();
        foreach (CardController card in ACC)
        {
            if(card.model.cardId != 1)
            {
                card.view.SetAap(true);
            }
        }
    }
    public void ShowLeaderHP()
    {
        if (playerLeaderHP <= 0)
        {
            playerLeaderHP = 0;
            StopAllCoroutines();
            StartCoroutine(uIManager.Result(false));
        }
        if (enemyLeaderHP <= 0)
        {
            enemyLeaderHP = 0;
            StopAllCoroutines();
            StartCoroutine(uIManager.Result(true));
        }
        playerLeaderHPText.text = playerLeaderHP.ToString();
        enemyLeaderHPText.text = enemyLeaderHP.ToString();
        pBHP.text = playerBlockHP.ToString();
        eBHP.text = enemyBlockHP.ToString();
    }
    public void PanelOn()
    {
        if(!Lock && isNotBattlePhase)
        {
            isFielder = true;
            _gridLayoutGroup = useField.GetComponent<GridLayoutGroup>();
            _gridLayoutGroup.enabled = true;
            useField.SetActive(true);
            useField.transform.SetSiblingIndex(4);
        }
    }
    public void PanelOff()
    {
        if(!Lock && isNotBattlePhase)
        {
            isFielder = false;
            useField.SetActive(false);
            _gridLayoutGroup = useField.GetComponent<GridLayoutGroup>();
            _gridLayoutGroup.enabled = false;
            useField.transform.SetAsFirstSibling();
        }
    }
    public void ChangeBuff()
    {
        if(PBuff != 0)
        {
            pBuff.text = PBuff.ToString();
        }else
        {
            pBuff.text = null;
        }
        if(EBuff != 0)
        {
            eBuff.text = EBuff.ToString();
        }else
        {
            eBuff.text = null;
        }
    }
    public void Curtain()
    {
        eDown.text = hPCurtainP.ToString();
        pDown.text = hPCurtainE.ToString();
    }
    public void Selecter()
    {
        ReSetCanUsePanelHand();
        Lock = true;
        selectUi.SetActive(true);
        if (isPlayerTurn == true)
        {
            int COUNT = 0;
            if(KP == true)
            {
                foreach (int item in deck)
                {
                    if(item >= 5)
                    {
                        //deck.RemoveAt(item);
                        CardController card = Instantiate(cardPrefab, selectField);
                        card.Sle(item,false);
                        COUNT += 1;
                    }
                    if(COUNT == 3){
                        break;
                    }
                }
            }
            if(JS == true)
            {
                foreach (int item in deck)
                {
                    if(item <= 4)
                    {
                        //deck.RemoveAt(item);
                        CardController card = Instantiate(cardPrefab, selectField);
                        card.Sle(item,false);
                        COUNT += 1;
                    }
                    if(COUNT == 2){
                        break;
                    }
                }
            }
        }
        else
        {
            int COUNT = 0;
            if(KP == true)
            {
                foreach (int item in deck)
                {
                    if(item >= 5)
                    {
                        //deck.RemoveAt(item);
                        CardController card = Instantiate(cardPrefab, selectField);
                        card.Sle(item,false);
                        COUNT += 1;
                    }
                    if(COUNT == 3){
                        break;
                    }
                }
            }
            if(JS == true)
            {
                foreach (int item in deck)
                {
                    if(item <= 4)
                    {
                        //deck.RemoveAt(item);
                        CardController card = Instantiate(cardPrefab, selectField);
                        card.Sle(item,false);
                        COUNT += 1;
                    }
                    if(COUNT == 2){
                        break;
                    }
                }
            }
        }
    }
    public void CosshonK(int ID)
    {
        CardController card = Instantiate(cardPrefab, playerHand);
        card.Init(ID,true);

        selectUi.SetActive(false);
        CardController[] SelectList = selectField.GetComponentsInChildren<CardController>();
        int SC = SelectList.Length;
        for (int DC = 0; DC != SC; DC++)
        {
            cardPrefab.DestroyCard(SelectList[DC]);
        }
        SetCanUsePanelHand();

        int n = 0;
        while (n < deck.Count)
        {
            if(deck[n] == ID)
            {
                deck.RemoveAt(n);
                break;
            }
            n++;
        }
        KP = false;
        Shuffle();
    }
    public void CosshonJ(int ID)
    {
        CardController card = Instantiate(cardPrefab, playerField);
        card.SpawnCard(ID,true);

        selectUi.SetActive(false);
        CardController[] SelectList = selectField.GetComponentsInChildren<CardController>();
        int SC = SelectList.Length;
        for (int DC = 0; DC != SC; DC++)
        {
            cardPrefab.DestroyCard(SelectList[DC]);
        }
        SetCanUsePanelHand();

        int n =0;
        while (n < deck.Count + 1)
        {
            if(deck[n] == ID)
            {
                deck.RemoveAt(n);
                break;
            }
            n++;
        }
        KP = false;
        Shuffle();
    }

    public void AppOffer()
    {
        CardController[] ACC = playerField.GetComponentsInChildren<CardController>();
        foreach (CardController card in ACC)
        {
            card.view.SetAap(false);
        }
    }

    public void ToCanChangeSR()
    {
        playerLeaderHP = 5;
        CheckHP();
        ShowLeaderHP();
    }

    public void CanUse(CardController card)
    {
        CanUseGun(card);
        CanUseSuko(card);
        CanUseGure(card);
        CanUseKakumaga(card);
        CanUseManaPlus(card);
        void CanUseManaPlus(CardController card)
        {
            if(card.model.cardId == 5)
            {
                CardController[] manas = playerManaField.GetComponentsInChildren<CardController>();
                bool itokawaJoshin = false;
                if(manas.Length >= 5)
                {
                    itokawaJoshin = true;
                }
                if(itokawaJoshin)
                {
                    card.model.canUse = false;
                    card.view.SetCanUsePanel(card.model.canUse);
                }
            }
        }
        void CanUseGun(CardController card)
        {
            if((0 < card.model.cardId && card.model.cardId <= 4) || card.model.cardId == 12)
            {
                CardController[] guns = playerField.GetComponentsInChildren<CardController>();
                bool itokawaJoshin = false;
                if(guns.Length >= 3)
                {
                    itokawaJoshin = true;
                }
                if(itokawaJoshin)
                {
                    card.model.canUse = false;
                    card.view.SetCanUsePanel(card.model.canUse);
                }
            }
        }
        void CanUseSuko(CardController card)
        {
            if(card.model.cardId == 15)
            {
                CardController[] guns = playerField.GetComponentsInChildren<CardController>();
                bool itokawaJoshin = true;
                foreach(CardController gun in guns)
                {
                    if(gun.model.cardId == 2 && !gun.model.isSuko)
                    {
                        itokawaJoshin = false;
                    }
                }
                if(itokawaJoshin)
                {
                    card.model.canUse = false;
                    card.view.SetCanUsePanel(card.model.canUse);
                }
            }
        }
        void CanUseGure(CardController card)
        {
            if(card.model.cardId == 6)
            {
                CardController[] hands = playerHand.GetComponentsInChildren<CardController>();
                if(hands.Length == 1)
                {
                    card.model.canUse = false;
                    card.view.SetCanUsePanel(card.model.canUse);
                }
            }
        }
        void CanUseKakumaga(CardController card)
        {
            if(card.model.cardId == 14)
            {
                CardController[] guns = playerField.GetComponentsInChildren<CardController>();
                bool itokawaJoshin = true;
                foreach(CardController gun in guns)
                {
                    if(gun.model.needMana > 0 && !gun.model.isKakumaga)
                    {
                        itokawaJoshin = false;
                    }
                }
                if(itokawaJoshin)
                {
                    card.model.canUse = false;
                    card.view.SetCanUsePanel(card.model.canUse);
                }
            }
        }
    }

    public void CardEffectStart(int cardId)
    {
        StartCoroutine(CardEffect(cardId));
    }
}