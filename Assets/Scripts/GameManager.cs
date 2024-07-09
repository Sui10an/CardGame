using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uIManager;
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, playerField, enemyField,playerManaField,selectField;
    [SerializeField] GameObject useField,cover,selectUi;
    [SerializeField] TextMeshProUGUI pBaff,eBaff,pDown,eDown;
    [SerializeField] TextMeshProUGUI playerLeaderHPText,enemyLeaderHPText,bHP,eBHP;
    [SerializeField] TextMeshProUGUI playerManaPointText, playerDefaultManaPointText;
    [SerializeField] Transform playerLeaderTransform;

    public int playerManaPoint; // ?��g?��p?��?��?��?��ƌ�?��?��}?��i?��|?��C?��?��?��g
    public int playerDefaultManaPoint; // ?��?��?��^?��[?��?��?��?��?��?��?��Ă�?��?��?��x?��[?��X?��̃}?��i?��|?��C?��?��?��g
    public int playerManaPlus; //?��}?��i?��v?��?��?��X?��̃J?��E?��?��?��g?��?���?��?��
    public int TrunCount;
    public int playerBlockHP;
    public int enemyBlockHP;
    public int hPCartenP;
    public int hPCartenE;
    public int PBaff;
    public int EBaff;
    GridLayoutGroup _gridLayoutGroup;

    public bool isPlayerTurn = true; //?��@Public?��֕ύX
    public bool isnotBattleFaiz = true;
    public bool isFileder = true;
    public bool isCartenP = true;
    public bool isCartenE = true;
    public bool Bom = true;
    public bool KP = true; // ケアパッケージを使うため
    public bool JS = true;// 銃のサーチ
    public bool CPC = true;// 銃のサーチ
    public bool CMC = true;
    List<int> deck = new List<int>() {2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,11,11,12,12,13,13,14,14,15,15};  //

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
        StartGame();
    }

    void StartGame() // 初期値の設定 
    {
        TrunCount = 0;

        enemyLeaderHP = 10;
        playerLeaderHP = 10;
        ShowLeaderHP();

        /// ?��}?��i?��̏�?��?��?��l?��ݒ� ///
        playerManaPoint = 1;
        playerDefaultManaPoint = 1;
        ShowManaPoint();

        // オプションいろいろ
        playerBlockHP = 0;
        enemyBlockHP = 0;
        isCartenP = false;
        isCartenE = false;
        hPCartenP = 0;
        hPCartenE = 0;
        PBaff = 0;
        EBaff = 0;
        KP = false;
        JS = false;
        CPC = false;
        CMC = false;

        //デッキのシャッフル
        Shuffle();

        // 初期手札を配る
        SetStartHand();

        // 初期盤面
        CreateSporn(1, playerField);
        CreateSporn(1, enemyField);

        // ターンの開始
        StartCoroutine(TurnCalc());
    }
    public void Shuffle() // デッキをシャッフルする
    {
        // 整数 n の初期値はデッキの枚数
        int n = deck.Count;

        // nが1より小さくなるまで繰り返す
        while (n > 1)
        {
            n--;

            // kは 0 ～ n+1 の間のランダムな値
            int k = UnityEngine.Random.Range(0, n + 1);

            // k番目のカードをtempに代入
            int temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    void ShowManaPoint() // ?��}?��i?��|?��C?��?��?��g?��?��\?��?��?��?��?���??��\?��b?��h
    {
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
        //?��}?��i?��J?��[?��h?��?��?��?��?��
        SetManaCard();
    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        // Player?��̎�D?��ɐ�?��?��?��?��?��ꂽ?��J?��[?��h?��?��Player?��̃J?��[?��h?��Ƃ�?��?��
        if (place == playerHand)
        {
            card.Init(cardID, true);
        }
        else
        {
            card.Init(cardID, false);
        }
    }
    void CreateSporn(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        // Player?��̎�D?��ɐ�?��?��?��?��?��ꂽ?��J?��[?��h?��?��Player?��̃J?��[?��h?��Ƃ�?��?��
        if (place == playerField)
        {
            card.SpornCard(cardID, true);
        }
        else
        {
            card.SpornCard(cardID, false);
        }
    }
    void CreateMana(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        // Player?��̎�D?��ɐ�?��?��?��?��?��ꂽ?��J?��[?��h?��?��Player?��̃J?��[?��h?��Ƃ�?��?��
        if (place == playerManaField)
        {
            card.ManaSporn(cardID, true);
        }
        else
        {
            card.ManaSporn(cardID, false);
        }
    }


    void DrawCard(Transform hand) // カードを引く
    {
        // デッキがないなら引かない
        if (deck.Count == 0)
        {
            return;
        }

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();

        if (playerHandCardList.Length < 7)
        {
            // デッキの一番上のカードを抜き取り、手札に加える
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }

        SetCanUsePanelHand();
    }

    void SetStartHand() // 手札を3枚配る
    {
        for (int i = 0; i < 3; i++)
        {
            DrawCard(playerHand);
        }
    }

    public void SetManaCard()
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        int Point = playerManaPoint;
        if (Point < playerManaCardList.Length)
        {
            int RS = playerManaCardList.Length - Point;
            for (int CC = 0; CC != RS; CC++)
            {
                cardPrefab.DestroyCard(playerManaCardList[CC]);
            }
        }
        else
        {
            for (int M = playerManaCardList.Length; M < Point; M++)
            {
                MakeCard(playerManaField);
            }
        }
    }
    void MakeCard(Transform mpt)
    {
        int cardID = 0;
        CreateMana(cardID, mpt);
    }

    IEnumerator TurnCalc() // 
    {
        yield return StartCoroutine(uIManager.ShowChangeTurnPanel());
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            //EnemyTurn(); // ?��R?��?��?��?��?��g?��A?��E?��g?��?��?��?��
            StartCoroutine(EnemyTurn()); // StartCoroutine?��ŌĂяo?��?��
        }
    }
    public void PhaseCalc()
    {
        uIManager.ShowChangePhasePanel();
    }

    public void ChangeTurn() // ターンエンドボタンにつける処理
    {
        isPlayerTurn = !isPlayerTurn; // ターンを逆にする
        uIManager.Stoper();
        ReSetCanUsePanelHand();
        StartCoroutine(TurnCalc()); // ターンを相手に回す
    }

    public void ChangePhase() // ?��^?��[?��?��?��G?��?��?��h?��{?��^?��?��?��ɂ�?��鏈�?��
    {
        isFileder = false;
        PhaseCalc(); // ?��^?��[?��?��?��?��?��ɉ�
    }

    void PlayerTurn()
    {
        TrunCount = TrunCount + 1;
        Debug.Log("Playerのターン");
        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        CheckHPID(playerFieldCardList);

        if (isFileder == true)
        {
            PanelOn();
        }

        if(isnotBattleFaiz == true)
        {
            CardController[] playerFieldList = playerField.GetComponentsInChildren<CardController>();
            foreach(CardController card in playerFieldList){
                card.model.manaplus = 0;
            }

            /// 弾薬箱の処理
            if(playerManaPlus != 0)
            {
                for(int MPP = 0;MPP < playerManaPlus; MPP++)
                {
                    playerManaPoint++;
                }
                playerManaPlus = 0;
            }
            playerDefaultManaPoint++;
            playerManaPoint++;
            ShowManaPoint();

            DrawCard(playerHand); // 手札を一枚加える
            if(isCartenP == true)
            {
                hPCartenP = 0;
                isCartenP = false;
                eDown.text = hPCartenP.ToString();
            }
            if(PBaff != 0)
            {
                PBaff = 0;
                pBaff.text = null;
            }
        }
    }

    public void BattleFaiz()//?��o?��g?��?��?��t?��F?��[?��Y?��?��ݒ�
    {
        ReSetCanUsePanelHand();
        ReSetCanUsePanelMana();
        if (TrunCount == 1 || isnotBattleFaiz == false)
        {
            uIManager.Stoper();
            ChangeTurn();
        }
        else
        {
            isnotBattleFaiz = false;
            uIManager.Stoper();
            StartCoroutine(TurnCalc());
            CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
            SetAttackableFieldCard(playerFieldCardList, true);
            PanelOff();
        }
        
    }

    public void ReduceManaPoint(int cost) // ?��R?��X?��g?��̕�?��A?��}?��i?��|?��C?��?��?��g?��?��?��?��?��炷
        {
            playerManaPoint -= cost;
            ShowManaPoint();

            SetCanUsePanelHand();
         }
    public void CardEffect(int cardId) // カードの効果表
    {
        if (1 <= cardId && cardId <= 4) //銃の出現
        {
            CreateSporn(cardId, playerField);
        }
        if (cardId == 5) //弾薬箱
        {
            playerManaPlus += 1;
            playerDefaultManaPoint++;

            ShowManaPoint();
        }
        if (cardId == 6) //ボンバー
        {
            CardController[] cardList = enemyField.GetComponentsInChildren<CardController>();
            Bom = true;
            useField.SetActive(false);
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
        if (cardId == 8) //装甲1
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
                isCartenP = true;
                hPCartenP -= 1;
            }
            else
            {
                isCartenE = true;
                hPCartenE -= 1;
            }
            Carthen();
        }
        if (cardId == 10) //ケアパッケージ輸送中
        {
            KP = true;
            Selecter();
        }
        if (cardId == 11) //装甲2
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
        if (cardId == 12) //銃サーチ
        {
            JS = true;
            Selecter();
        }
        if (cardId == 13) //バッファー
        {
            if (isPlayerTurn == true)
            {
                PBaff += 1;
                ChangeBaff(PBaff);
            }
            else
            {
                EBaff += 1;
                ChangeBaff(EBaff);
            }
        }
        if (cardId == 14) //拡張
        {
            useField.SetActive(false);
            Cakuthou();
        }
        if (cardId == 15) //スコ
        {
            useField.SetActive(false);
            ChangePower();
        }
    }

    public void SetCanUsePanelMana() // ?��?��D?��̃J?��[?��h?��?��?��擾?��?��?��āA?��g?��p?��\?��ȃJ?��[?��h?��?��CanUse?��p?��l?��?��?��?��t?��?��?��?��
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerManaCardList)
        {
            card.model.canUse = true;
            card.model.canAttack = true;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }

    public void ReSetCanUsePanelMana() // ?��?��D?��̃J?��[?��h?��?��?��擾?��?��?��āA?��g?��p?��s?��\?��ȃJ?��[?��h?��ɂ�?��?��
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerManaCardList)
        {
            card.model.canUse = false;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }
    void SetCanUsePanelHand() // ?��?��D?��̃J?��[?��h?��?��?��擾?��?��?��āA?��g?��p?��\?��ȃJ?��[?��h?��?��CanUse?��p?��l?��?��?��?��t?��?��?��?��
    {
        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        //CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerHandCardList)
        {
            if (card.model.cost <= playerManaPoint)
            {
                card.model.canUse = true;
                card.view.SetCanUsePanel(card.model.canUse);
            }
            else
            {
                card.model.canUse = false;
                card.view.SetCanUsePanel(card.model.canUse);
            }
        }
    }

    void ReSetCanUsePanelHand() // ?��?��D?��̃J?��[?��h?��?��?��擾?��?��?��āA?��g?��p?��s?��\?��ȃJ?��[?��h?��ɂ�?��?��
    {
        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerHandCardList)
        {
            card.model.canUse = false;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }

    
    IEnumerator EnemyTurn() // StartCoroutine?��ŌĂ΂ꂽ?��̂ŁAIEnumerator?��ɕύX
    {
        PanelOff();
        cover.SetActive(true);
        TrunCount = TrunCount + 1;
        isFileder = false;

        if (EBaff != 0)
        {
            EBaff = 0;
            eBaff.text = null;
        }
        if(hPCartenE != 0)
        {
            pDown.text = hPCartenE.ToString();
        }

        Debug.Log("Enemyのターン");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        yield return new WaitForSeconds(1f);

        /// ?��G?��̃t?��B?��[?��?��?��h?��̃J?��[?��h?��?��?��U?��?��?��\?��ɂ�?��āA?��΂̘g?��?��t?��?��?��?�� ///
        SetAttackableFieldCard(enemyFieldCardList, true);

        yield return new WaitForSeconds(1f);

        if (enemyFieldCardList.Length < 3)
        {
            int iD = UnityEngine.Random.Range(2, 5);
            Debug.Log(iD);
            CreateCard(iD, enemyField);
        }

        CardController[] enemyFieldCardListSecond = enemyField.GetComponentsInChildren<CardController>();
        for(int EC = 0;EC < enemyFieldCardListSecond.Length; EC++)
        {
            CardController useCard = enemyFieldCardListSecond[EC];
            useCard.model.mana += 1;
        }

        yield return new WaitForSeconds(1f);

        while (Array.Exists(enemyFieldCardListSecond, card => card.model.canAttack))
        {
            // ?��U?��?��?��\?��J?��[?��h?��?��?��擾
            CardController[] enemyCanAttackCardList = Array.FindAll(enemyFieldCardListSecond, card => card.model.canAttack);
            CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();

            CardController attackCard = enemyCanAttackCardList[0];

            
            yield return StartCoroutine(attackCard.movement.AttackMotion(playerLeaderTransform));
            AttackToLeader(attackCard, false);

            yield return new WaitForSeconds(1f);

            enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();
        }
        isnotBattleFaiz = true;
        isFileder = true;
        cover.SetActive(false);
        ChangeTurn(); // ?��^?��[?��?��?��G?��?��?��h?��?��?��?��
    }


    public void CardBattle(CardController attackCard, CardController defenceCard)
    {

        // ?��U?��?��?��J?��[?��h?��ƍU?��?��?��?��?��?��?��J?��[?��h?��?��?��?��?��?��?��v?��?��?��C?��?��?��[?��̃J?��[?��h?��Ȃ�o?��g?��?��?��?��?��Ȃ�
        if (attackCard.model.PlayerCard == defenceCard.model.PlayerCard)
        {
            if(attackCard.model.ManaCard == true)
            {
                if(defenceCard.model.needmana == 0 || defenceCard.model.manaplus> defenceCard.model.manapluspuls )
                {
                    return;
                }
                defenceCard.model.mana += 1;
                playerManaPoint -= 1;
                defenceCard.model.manaplus += 1;
                attackCard.DestroyCard(attackCard);
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
        attackCard.model.canAttack = false;
    }
    void SetAttackableFieldCard(CardController[] cardList, bool canAttack) 
    {
        
        foreach (CardController card in cardList)
        {
            if(card.model.mana >= card.model.needmana)
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
    void CheckHPID(CardController[] cardList)
    {
        if(playerLeaderHP > 5)
        {
            return;
        }
        foreach (CardController card in cardList)
        {
            if (card.model.cardId == 1)
            {
                card.model.SRcan = true;
                card.view.ChangeSR(true);
            }
        }

    }
    public int playerLeaderHP;
    public int enemyLeaderHP;

    public void AttackToLeader(CardController attackCard, bool isPlayerCard)
    {
        if (attackCard.model.canAttack == false)
        {
            return;
        }
        int baff = 0;
        if(isPlayerTurn == true)
        {
            baff = PBaff;
        }
        else
        {
            baff = EBaff;
        }
        int attackpower = attackCard.model.power + baff;

        if (attackCard.model.PlayerCard == true) // attackCard?��?��?��v?��?��?��C?��?��?��[?��̃J?��[?��h?��Ȃ�
        {
            if(enemyBlockHP != 0)
            {
                int AP = attackpower;
                AP -= enemyBlockHP;
                if(AP > 0)
                {
                    enemyBlockHP = 0;
                }
                else if(AP <= 0)
                {
                    enemyBlockHP -= attackpower;
                    ShowLeaderHP();
                    return;
                }
                attackpower = AP;
            }
            enemyLeaderHP -= attackpower; // ?��G?���??��?��[?��_?��[?��?��HP?��?��?��?��?��炷
        }
        else // attackCard?��?��?��G?��̃J?��[?��h?��Ȃ�
        {
            if(isCartenP == true)
            {
                attackpower += hPCartenP;
            }
            if (playerBlockHP != 0)
            {
                int AP = attackpower;
                AP -= playerBlockHP;
                if (AP > 0)
                {
                    playerBlockHP = 0;
                }
                else if (AP <= 0)
                {
                    playerBlockHP -= attackpower;
                    attackCard.model.mana -= attackCard.model.needmana;
                    attackCard.model.canAttack = false;
                    attackCard.view.SetCanAttackPanel(false);
                    ShowLeaderHP();
                    return;
                }
                attackpower = AP;
            }
            playerLeaderHP -= attackpower; // ?��v?��?��?��C?��?��?��[?���??��?��[?��_?��[?��?��HP?��?��?��?��?��炷
        }

        //enemyLeaderHP -= attackCard.model.power; // ?��R?��?��?��?��?��g?��A?��E?��g?��?��?��?��

        attackCard.model.mana -= attackCard.model.needmana;
        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
        Debug.Log("?��G?��?��HP?��́A" + enemyLeaderHP);
        ChangeManaText(attackCard);
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
    public void Cakuthou()
    {
        CMC = true;
        CardController[] ACC = playerField.GetComponentsInChildren<CardController>();
        foreach (CardController card in ACC)
        {
            card.view.SetAap(true);
        }
    }
    public void ShowLeaderHP()
    {
        if (playerLeaderHP <= 0)
        {
            playerLeaderHP = 0;
        }
        if (enemyLeaderHP <= 0)
        {
            enemyLeaderHP = 0;
        }
        playerLeaderHPText.text = playerLeaderHP.ToString();
        enemyLeaderHPText.text = enemyLeaderHP.ToString();
        bHP.text = playerBlockHP.ToString();
        eBHP.text = enemyBlockHP.ToString();
    }
    public void PanelOn()
    {
        _gridLayoutGroup = useField.GetComponent<GridLayoutGroup>();
        _gridLayoutGroup.enabled = true;
        useField.SetActive(true);
        useField.transform.SetSiblingIndex(4);
    }
    public void PanelOff()
    {
        useField.SetActive(false);
        _gridLayoutGroup = useField.GetComponent<GridLayoutGroup>();
        _gridLayoutGroup.enabled = false;
        useField.transform.SetAsFirstSibling();
    }
    public void ChangeBaff(int Baff)
    {
        if(isPlayerTurn == true)
        {
            pBaff.text = Baff.ToString();
        }
        else
        {
            eBaff.text = Baff.ToString();
        }
    }
    public void Carthen()
    {
        if(hPCartenP != 0)
        {
            eDown.text = hPCartenP.ToString();
        }
        else if(hPCartenE != 0)
        {
            pDown.text = hPCartenE.ToString();
        }
    }
    public void Selecter()
    {
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
    public void CosshonK(int ID){
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
    }
    public void CosshonJ(int ID){
        CardController card = Instantiate(cardPrefab, playerField);
        card.SpornCard(ID,true);

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
    }
    public void AppOffer()
    {
        CardController[] ACC = playerField.GetComponentsInChildren<CardController>();
        foreach (CardController card in ACC)
        {
            card.view.SetAap(false);
        }
    }
}