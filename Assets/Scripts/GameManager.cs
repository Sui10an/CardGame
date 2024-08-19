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

    public int playerManaPoint; // ??申?申g??申?申p??申?申??申?申??申?申??申?申��鐃�??申?申??申?申}??申?申i??申?申|??申?申C??申?申??申?申??申?申g
    public int playerDefaultManaPoint; // ??申?申??申?申??申?申^??申?申[??申?申??申?申??申?申??申?申??申?申??申?申??申?申��鐃�??申?申??申?申??申?申x??申?申[??申?申X??申?申��}??申?申i??申?申|??申?申C??申?申??申?申??申?申g
    public int playerManaPlus; //??申?申}??申?申i??申?申v??申?申??申?申??申?申X??申?申��J??申?申E??申?申??申?申??申?申g??申?申??申?申?申??申?申??申?申
    public int TrunCount;
    public int playerBlockHP;
    public int enemyBlockHP;
    public int hPCartenP;
    public int hPCartenE;
    public int PBaff;
    public int EBaff;
    GridLayoutGroup _gridLayoutGroup;

    public bool isPlayerTurn = true; //??申?申@Public??申?申����X
    public bool isnotBattleFaiz = true;
    public bool isFileder = true;
    public bool isCartenP = true;
    public bool isCartenE = true;
    public bool Bom = true;
    public bool KP = true; // ��宴�≪�������宴�若�吾��篏帥�?������
    public bool JS = true;// ��?�����泣�若�?
    public bool CPC = true;// ��?�����泣�若�?
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

    void StartGame() // ��������ゃ��荐�絎? 
    {
        TrunCount = 0;

        enemyLeaderHP = 10;
        playerLeaderHP = 10;
        ShowLeaderHP();

        /// ??申?申}??申?申i??申?申��鐃�??申?申??申?申??申?申l??申?申��鐃� ///
        playerManaPoint = 1;
        playerDefaultManaPoint = 1;
        ShowManaPoint();

        // ��������激�с�潟�?��������?
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

        //��?��?��������激�ｃ�?������
        Shuffle();

        // ����������������?����
        SetStartHand();

        // ��������ら��
        CreateSporn(1, playerField);
        CreateSporn(1, enemyField);

        // ��帥�若�潟�����紮?
        StartCoroutine(TurnCalc());
    }
    public void Shuffle() // ��?��?��������激�ｃ�?������������
    {
        // ��贋�� n �����������ゃ����?��?������������
        int n = deck.Count;

        // n��?1������絨���������������障�х弘���菴����
        while (n > 1)
        {
            n--;

            // k��� 0 ?�? n+1 �������?������潟����?������
            int k = UnityEngine.Random.Range(0, n + 1);

            // k��������������若�����temp���篁ｅ��
            int temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    void ShowManaPoint() // ??申?申}??申?申i??申?申|??申?申C??申?申??申?申??申?申g??申?申??申?申\??申?申??申?申??申?申??申?申??申?申?申???申?申\??申?申b??申?申h
    {
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
        //??申?申}??申?申i??申?申J??申?申[??申?申h??申?申??申?申??申?申??申?申??申?申
        SetManaCard();
    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        // Player??申?申��鐃�D??申?申��鐃�??申?申??申?申??申?申??申?申??申?申���??申?申J??申?申[??申?申h??申?申??申?申Player??申?申��J??申?申[??申?申h??申?申��鐃�??申?申??申?申
        if (place == playerHand)
        {
            card.Init(cardID, true);
        }
        else
        {
            card.SpornCard(cardID, false);
        }
    }
    void CreateSporn(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        // Player??申?申��鐃�D??申?申��鐃�??申?申??申?申??申?申??申?申??申?申���??申?申J??申?申[??申?申h??申?申??申?申Player??申?申��J??申?申[??申?申h??申?申��鐃�??申?申??申?申
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
        // Player??申?申��鐃�D??申?申��鐃�??申?申??申?申??申?申??申?申??申?申���??申?申J??申?申[??申?申h??申?申??申?申Player??申?申��J??申?申[??申?申h??申?申��鐃�??申?申??申?申
        if (place == playerManaField)
        {
            card.ManaSporn(cardID, true);
        }
        else
        {
            card.ManaSporn(cardID, false);
        }
    }


    void DrawCard(Transform hand) // �����若�����綣����
    {
        // ��?��?�����������?������綣���������?
        if (deck.Count == 0)
        {
            return;
        }

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();

        if (playerHandCardList.Length < 7)
        {
            // ��?��?������筝����筝��?������若�������������������������������?������
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }

        SetCanUsePanelHand();
    }

    void SetStartHand() // ��������?3����?����
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
            //EnemyTurn(); // ??申?申R??申?申??申?申??申?申??申?申??申?申g??申?申A??申?申E??申?申g??申?申??申?申??申?申??申?申
            StartCoroutine(EnemyTurn()); // StartCoroutine??申?申������o??申?申??申?申
        }
    }
    public void PhaseCalc()
    {
        uIManager.ShowChangePhasePanel();
    }

    public void ChangeTurn() // ��帥�若�潟����潟���?���帥�潟����ゃ����������?
    {
        isPlayerTurn = !isPlayerTurn; // ��帥�若�潟����?���������
        uIManager.Stoper();
        ReSetCanUsePanelHand();
        StartCoroutine(TurnCalc()); // ��帥�若�潟����御�����������
    }

    public void ChangePhase() // ??申?申^??申?申[??申?申??申?申??申?申G??申?申??申?申??申?申h??申?申{??申?申^??申?申??申?申??申?申����鐃�??申?申���鐃�??申?申
    {
        isFileder = false;
        PhaseCalc(); // ??申?申^??申?申[??申?申??申?申??申?申??申?申??申?申��鐃�
    }

    void PlayerTurn()
    {
        TrunCount = TrunCount + 1;
        Debug.Log("Player�����帥�若��");
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

            /// 綣乗��膊宴�������?
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

            DrawCard(playerHand); // ���������筝�������������
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

    public void BattleFaiz()//??申?申o??申?申g??申?申??申?申??申?申t??申?申F??申?申[??申?申Y??申?申??申?申��鐃�
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

    public void ReduceManaPoint(int cost) // ??申?申R??申?申X??申?申g??申?申��鐃�??申?申A??申?申}??申?申i??申?申|??申?申C??申?申??申?申??申?申g??申?申??申?申??申?申??申?申??申?申���
        {
            playerManaPoint -= cost;
            ShowManaPoint();

            SetCanUsePanelHand();
         }
    public void CardEffect(int cardId) // �����若���?���号��茵�
    {
        if (1 <= cardId && cardId <= 4) //��?�����榊��
        {
            CreateSporn(cardId, playerField);
        }
        if (cardId == 5) //綣乗��膊�
        {
            playerManaPlus += 1;
            playerDefaultManaPoint++;

            ShowManaPoint();
        }
        if (cardId == 6) //�����潟���?�
        {
            CardController[] cardList = enemyField.GetComponentsInChildren<CardController>();
            Bom = true;
            useField.SetActive(false);
            foreach(CardController card in cardList)
            {
                card.view.SetBomPanel(Bom);
            }
        }
        if(cardId == 7) //���緇�
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
        if (cardId == 8) //茖?���1
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
        if (cardId == 9) //������!
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
        if (cardId == 10) //��宴�≪�������宴�若�梧叱���筝�
        {
            KP = true;
            Selecter();
        }
        if (cardId == 11) //茖?���2
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
        if (cardId == 12) //��?��泣�若�?
        {
            JS = true;
            Selecter();
        }
        if (cardId == 13) //�����������＜��
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
        if (cardId == 14) //��≦宍
        {
            useField.SetActive(false);
            Cakuthou();
        }
        if (cardId == 15) //��鴻��
        {
            useField.SetActive(false);
            ChangePower();
        }
    }

    public void SetCanUsePanelMana() // ??申?申??申?申D??申?申��J??申?申[??申?申h??申?申??申?申??申?申���??申?申??申?申??申?申��A??申?申g??申?申p??申?申��\??申?申��J??申?申[??申?申h??申?申??申?申CanUse??申?申p??申?申l??申?申??申?申??申?申??申?申t??申?申??申?申??申?申??申?申
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerManaCardList)
        {
            card.model.canUse = true;
            card.model.canAttack = true;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }

    public void ReSetCanUsePanelMana() // ??申?申??申?申D??申?申��J??申?申[??申?申h??申?申??申?申??申?申���??申?申??申?申??申?申��A??申?申g??申?申p??申?申s??申?申\??申?申��J??申?申[??申?申h??申?申��鐃�??申?申??申?申
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerManaCardList)
        {
            card.model.canUse = false;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }
    void SetCanUsePanelHand() // ??申?申??申?申D??申?申��J??申?申[??申?申h??申?申??申?申??申?申���??申?申??申?申??申?申��A??申?申g??申?申p??申?申��\??申?申��J??申?申[??申?申h??申?申??申?申CanUse??申?申p??申?申l??申?申??申?申??申?申??申?申t??申?申??申?申??申?申??申?申
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

    void ReSetCanUsePanelHand() // ??申?申??申?申D??申?申��J??申?申[??申?申h??申?申??申?申??申?申���??申?申??申?申??申?申��A??申?申g??申?申p??申?申s??申?申\??申?申��J??申?申[??申?申h??申?申��鐃�??申?申??申?申
    {
        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerHandCardList)
        {
            card.model.canUse = false;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }

    
    IEnumerator EnemyTurn() // StartCoroutine??申?申���������??申?申����AIEnumerator??申?申����X
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

        Debug.Log("Enemy�����帥�若��");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        yield return new WaitForSeconds(1f);

        /// ??申?申G??申?申��t??申?申B??申?申[??申?申??申?申??申?申h??申?申��J??申?申[??申?申h??申?申??申?申??申?申U??申?申??申?申??申?申��\??申?申��鐃�??申?申��A??申?申����g??申?申??申?申t??申?申??申?申??申?申??申?申 ///
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
            // ??申?申U??申?申??申?申??申?申��\??申?申J??申?申[??申?申h??申?申??申?申??申?申���
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
        ChangeTurn(); // ??申?申^??申?申[??申?申??申?申??申?申G??申?申??申?申??申?申h??申?申??申?申??申?申??申?申
    }


    public void CardBattle(CardController attackCard, CardController defenceCard)
    {

        // ??申?申U??申?申??申?申??申?申J??申?申[??申?申h??申?申��U??申?申??申?申??申?申??申?申??申?申??申?申??申?申J??申?申[??申?申h??申?申??申?申??申?申??申?申??申?申??申?申??申?申v??申?申??申?申??申?申C??申?申??申?申??申?申[??申?申��J??申?申[??申?申h??申?申��鐃�o??申?申g??申?申??申?申??申?申??申?申??申?申��鐃�
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

        if (attackCard.model.PlayerCard == true) // attackCard??申?申??申?申??申?申v??申?申??申?申??申?申C??申?申??申?申??申?申[??申?申��J??申?申[??申?申h??申?申��鐃�
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
            enemyLeaderHP -= attackpower; // ??申?申G??申?申?申???申?申??申?申[??申?申_??申?申[??申?申??申?申HP??申?申??申?申??申?申??申?申??申?申���
        }
        else // attackCard??申?申??申?申??申?申G??申?申��J??申?申[??申?申h??申?申��鐃�
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
            playerLeaderHP -= attackpower; // ??申?申v??申?申??申?申??申?申C??申?申??申?申??申?申[??申?申?申???申?申??申?申[??申?申_??申?申[??申?申??申?申HP??申?申??申?申??申?申??申?申??申?申���
        }

        //enemyLeaderHP -= attackCard.model.power; // ??申?申R??申?申??申?申??申?申??申?申??申?申g??申?申A??申?申E??申?申g??申?申??申?申??申?申??申?申

        attackCard.model.mana -= attackCard.model.needmana;
        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
        Debug.Log("??申?申G??申?申??申?申HP??申?申��A" + enemyLeaderHP);
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