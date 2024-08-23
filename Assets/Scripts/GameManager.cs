// BAKAMANUKE
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

    public int playerManaPoint; // égópÉ}ÉiÇÃêî
    public int playerDefaultManaPoint; // ó›åvÉ}Éiêî
    public int playerManaPlus; //É}Éiâ¡ë¨ÇÃÉJÉEÉìÉg
    public int TrunCount;
    public int playerBlockHP;
    public int enemyBlockHP;
    public int hPCartenP;
    public int hPCartenE;
    public int PBaff;
    public int EBaff;
    GridLayoutGroup _gridLayoutGroup;

    public bool isPlayerTurn = true; //??øΩ?øΩ@Public??øΩ?øΩ÷ïœçX
    public bool isnotBattleFaiz = true;
    public bool isFileder = true;
    public bool isCartenP = true;
    public bool isCartenE = true;
    public bool Bom = true;
    public bool KP = true; // „Ç±„Ç¢„Éë„ÉÉ„Ç±„Éº„Ç∏„Çí‰Ωø„Å?„Åü„ÇÅ
    public bool JS = true;// Èä?„ÅÆ„Çµ„Éº„É?
    public bool CPC = true;// Èä?„ÅÆ„Çµ„Éº„É?
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

    void StartGame() // ÂàùÊúüÂÄ§„ÅÆË®≠ÂÆ? 
    {
        TrunCount = 0;

        enemyLeaderHP = 10;
        playerLeaderHP = 10;
        ShowLeaderHP();

        /// ??øΩ?øΩ}??øΩ?øΩi??øΩ?øΩÃèÔøΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩl??øΩ?øΩ›íÔøΩ ///
        playerManaPoint = 1;
        playerDefaultManaPoint = 1;
        ShowManaPoint();

        // „Ç™„Éó„Ç∑„Éß„É≥„Å?„Çç„ÅÑ„Ç?
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

        //„É?„É?„Ç≠„ÅÆ„Ç∑„É£„É?„Éï„É´
        Shuffle();

        // ÂàùÊúüÊâãÊú≠„ÇíÈ?ç„Çã
        SetStartHand();

        // ÂàùÊúüÁõ§Èù¢
        CreateSporn(1, playerField);
        CreateSporn(1, enemyField);

        // „Çø„Éº„É≥„ÅÆÈñãÂß?
        StartCoroutine(TurnCalc());
    }
    public void Shuffle() // „É?„É?„Ç≠„Çí„Ç∑„É£„É?„Éï„É´„Åô„Çã
    {
        // Êï¥Êï∞ n „ÅÆÂàùÊúüÂÄ§„ÅØ„É?„É?„Ç≠„ÅÆÊûöÊï∞
        int n = deck.Count;

        // n„Å?1„Çà„ÇäÂ∞è„Åï„Åè„Å™„Çã„Åæ„ÅßÁπ∞„ÇäËøî„Åô
        while (n > 1)
        {
            n--;

            // k„ÅØ 0 ?Ω? n+1 „ÅÆÈñì„?Æ„É©„É≥„ÉÄ„É?„Å™ÂÄ§
            int k = UnityEngine.Random.Range(0, n + 1);

            // kÁï™ÁõÆ„ÅÆ„Ç´„Éº„Éâ„Çítemp„Å´‰ª£ÂÖ•
            int temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    void ShowManaPoint() // ??øΩ?øΩ}??øΩ?øΩi??øΩ?øΩ|??øΩ?øΩC??øΩ?øΩ??øΩ?øΩ??øΩ?øΩg??øΩ?øΩ??øΩ?øΩ\??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ?øΩ???øΩ?øΩ\??øΩ?øΩb??øΩ?øΩh
    {
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
        //??øΩ?øΩ}??øΩ?øΩi??øΩ?øΩJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ
        SetManaCard();
    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        // Player??øΩ?øΩÃéÔøΩD??øΩ?øΩ…êÔøΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÍÇΩ??øΩ?øΩJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩPlayer??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ∆ÇÔøΩ??øΩ?øΩ??øΩ?øΩ
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
        // Player??øΩ?øΩÃéÔøΩD??øΩ?øΩ…êÔøΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÍÇΩ??øΩ?øΩJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩPlayer??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ∆ÇÔøΩ??øΩ?øΩ??øΩ?øΩ
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
        // Player??øΩ?øΩÃéÔøΩD??øΩ?øΩ…êÔøΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÍÇΩ??øΩ?øΩJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩPlayer??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ∆ÇÔøΩ??øΩ?øΩ??øΩ?øΩ
        if (place == playerManaField)
        {
            card.ManaSporn(cardID, true);
        }
        else
        {
            card.ManaSporn(cardID, false);
        }
    }


    void DrawCard(Transform hand) // „Ç´„Éº„Éâ„ÇíÂºï„Åè
    {
        // „É?„É?„Ç≠„Åå„Å™„Å?„Å™„ÇâÂºï„Åã„Å™„Å?
        if (deck.Count == 0)
        {
            return;
        }

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();

        if (playerHandCardList.Length < 7)
        {
            // „É?„É?„Ç≠„ÅÆ‰∏ÄÁï™‰∏ä„?Æ„Ç´„Éº„Éâ„ÇíÊäú„ÅçÂèñ„Çä„ÄÅÊâãÊú≠„Å´Âä?„Åà„Çã
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }

        SetCanUsePanelHand();
    }

    void SetStartHand() // ÊâãÊú≠„Ç?3ÊûöÈ?ç„Çã
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
            //EnemyTurn(); // ??øΩ?øΩR??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩg??øΩ?øΩA??øΩ?øΩE??øΩ?øΩg??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ
            StartCoroutine(EnemyTurn()); // StartCoroutine??øΩ?øΩ≈åƒÇ—èo??øΩ?øΩ??øΩ?øΩ
        }
    }
    public void PhaseCalc()
    {
        uIManager.ShowChangePhasePanel();
    }

    public void ChangeTurn() // „Çø„Éº„É≥„Ç®„É≥„Éâ„?ú„Çø„É≥„Å´„Å§„Åë„ÇãÂá¶Áê?
    {
        isPlayerTurn = !isPlayerTurn; // „Çø„Éº„É≥„ÇíÈÄ?„Å´„Åô„Çã
        uIManager.Stoper();
        ReSetCanUsePanelHand();
        StartCoroutine(TurnCalc()); // „Çø„Éº„É≥„ÇíÁõ∏Êâã„Å´Âõû„Åô
    }

    public void ChangePhase() // ??øΩ?øΩ^??øΩ?øΩ[??øΩ?øΩ??øΩ?øΩ??øΩ?øΩG??øΩ?øΩ??øΩ?øΩ??øΩ?øΩh??øΩ?øΩ{??øΩ?øΩ^??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ…Ç¬ÇÔøΩ??øΩ?øΩÈèàÔøΩ??øΩ?øΩ
    {
        isFileder = false;
        PhaseCalc(); // ??øΩ?øΩ^??øΩ?øΩ[??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ…âÔøΩ
    }

    void PlayerTurn()
    {
        TrunCount = TrunCount + 1;
        Debug.Log("Player„ÅÆ„Çø„Éº„É≥");
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

            /// ÂºæËñ¨ÁÆ±„ÅÆÂá¶Áê?
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

            DrawCard(playerHand); // ÊâãÊú≠„Çí‰∏ÄÊûöÂä†„Åà„Çã
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

    public void BattleFaiz()//??øΩ?øΩo??øΩ?øΩg??øΩ?øΩ??øΩ?øΩ??øΩ?øΩt??øΩ?øΩF??øΩ?øΩ[??øΩ?øΩY??øΩ?øΩ??øΩ?øΩ›íÔøΩ
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

    public void ReduceManaPoint(int cost) // ??øΩ?øΩR??øΩ?øΩX??øΩ?øΩg??øΩ?øΩÃïÔøΩ??øΩ?øΩA??øΩ?øΩ}??øΩ?øΩi??øΩ?øΩ|??øΩ?øΩC??øΩ?øΩ??øΩ?øΩ??øΩ?øΩg??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÁÇ∑
        {
            playerManaPoint -= cost;
            ShowManaPoint();

            SetCanUsePanelHand();
         }
    public void CardEffect(int cardId) // „Ç´„Éº„Éâ„?ÆÂäπÊûúË°®
    {
        if (1 <= cardId && cardId <= 4) //Èä?„ÅÆÂá∫Áèæ
        {
            CreateSporn(cardId, playerField);
        }
        if (cardId == 5) //ÂºæËñ¨ÁÆ±
        {
            playerManaPlus += 1;
            playerDefaultManaPoint++;

            ShowManaPoint();
        }
        if (cardId == 6) //„Éú„É≥„Éê„?º
        {
            CardController[] handList = playerHand.GetComponentsInChildren<CardController>();
            int Counttt = UnityEngine.Random.Range(1, handList.Length);
            CardController card1 = handList[Counttt];
            card1.DestroyCard(card1);
            CardController[] cardList = enemyField.GetComponentsInChildren<CardController>();
            Bom = true;
            useField.SetActive(false);
            foreach(CardController card in cardList)
            {
                card.view.SetBomPanel(Bom);
            }
        }
        if(cardId == 7) //ÂõûÂæ©
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
        if (cardId == 8) //Ë£?Áî≤1
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
        if (cardId == 9) //ÂÖâ„Çà!
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
        if (cardId == 10) //„Ç±„Ç¢„Éë„ÉÉ„Ç±„Éº„Ç∏Ëº∏ÈÄÅ‰∏≠
        {
            KP = true;
            Selecter();
        }
        if (cardId == 11) //Ë£?Áî≤2
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
        if (cardId == 12) //Èä?„Çµ„Éº„É?
        {
            JS = true;
            Selecter();
        }
        if (cardId == 13) //„Éê„ÉÉ„Éï„Ç°„Éº
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
        if (cardId == 14) //Êã°Âºµ
        {
            useField.SetActive(false);
            Cakuthou();
        }
        if (cardId == 15) //„Çπ„Ç≥
        {
            useField.SetActive(false);
            ChangePower();
        }
    }

    public void SetCanUsePanelMana() // ??øΩ?øΩ??øΩ?øΩD??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÊìæ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩƒÅA??øΩ?øΩg??øΩ?øΩp??øΩ?øΩ¬î\??øΩ?øΩ»ÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩCanUse??øΩ?øΩp??øΩ?øΩl??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩt??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerManaCardList)
        {
            card.model.canUse = true;
            card.model.canAttack = true;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }

    public void ReSetCanUsePanelMana() // ??øΩ?øΩ??øΩ?øΩD??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÊìæ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩƒÅA??øΩ?øΩg??øΩ?øΩp??øΩ?øΩs??øΩ?øΩ\??øΩ?øΩ»ÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ…ÇÔøΩ??øΩ?øΩ??øΩ?øΩ
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerManaCardList)
        {
            card.model.canUse = false;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }
    void SetCanUsePanelHand() // ??øΩ?øΩ??øΩ?øΩD??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÊìæ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩƒÅA??øΩ?øΩg??øΩ?øΩp??øΩ?øΩ¬î\??øΩ?øΩ»ÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩCanUse??øΩ?øΩp??øΩ?øΩl??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩt??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ
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

    void ReSetCanUsePanelHand() // ??øΩ?øΩ??øΩ?øΩD??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÊìæ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩƒÅA??øΩ?øΩg??øΩ?øΩp??øΩ?øΩs??øΩ?øΩ\??øΩ?øΩ»ÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ…ÇÔøΩ??øΩ?øΩ??øΩ?øΩ
    {
        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerHandCardList)
        {
            card.model.canUse = false;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }

    
    IEnumerator EnemyTurn() // StartCoroutine??øΩ?øΩ≈åƒÇŒÇÍÇΩ??øΩ?øΩÃÇ≈ÅAIEnumerator??øΩ?øΩ…ïœçX
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

        Debug.Log("Enemy„ÅÆ„Çø„Éº„É≥");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        yield return new WaitForSeconds(1f);

        /// ??øΩ?øΩG??øΩ?øΩÃÉt??øΩ?øΩB??øΩ?øΩ[??øΩ?øΩ??øΩ?øΩ??øΩ?øΩh??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩ??øΩ?øΩU??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ¬î\??øΩ?øΩ…ÇÔøΩ??øΩ?øΩƒÅA??øΩ?øΩŒÇÃòg??øΩ?øΩ??øΩ?øΩt??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ ///
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
            // ??øΩ?øΩU??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ¬î\??øΩ?øΩJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÊìæ
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
        ChangeTurn(); // ??øΩ?øΩ^??øΩ?øΩ[??øΩ?øΩ??øΩ?øΩ??øΩ?øΩG??øΩ?øΩ??øΩ?øΩ??øΩ?øΩh??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ
    }


    public void CardBattle(CardController attackCard, CardController defenceCard)
    {

        // ??øΩ?øΩU??øΩ?øΩ??øΩ?øΩ??øΩ?øΩJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ∆çU??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩv??øΩ?øΩ??øΩ?øΩ??øΩ?øΩC??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ[??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ»ÇÔøΩo??øΩ?øΩg??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ»ÇÔøΩ
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

        if (attackCard.model.PlayerCard == true) // attackCard??øΩ?øΩ??øΩ?øΩ??øΩ?øΩv??øΩ?øΩ??øΩ?øΩ??øΩ?øΩC??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ[??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ»ÇÔøΩ
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
            enemyLeaderHP -= attackpower; // ??øΩ?øΩG??øΩ?øΩ?øΩ???øΩ?øΩ??øΩ?øΩ[??øΩ?øΩ_??øΩ?øΩ[??øΩ?øΩ??øΩ?øΩHP??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÁÇ∑
        }
        else // attackCard??øΩ?øΩ??øΩ?øΩ??øΩ?øΩG??øΩ?øΩÃÉJ??øΩ?øΩ[??øΩ?øΩh??øΩ?øΩ»ÇÔøΩ
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
            playerLeaderHP -= attackpower; // ??øΩ?øΩv??øΩ?øΩ??øΩ?øΩ??øΩ?øΩC??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ[??øΩ?øΩ?øΩ???øΩ?øΩ??øΩ?øΩ[??øΩ?øΩ_??øΩ?øΩ[??øΩ?øΩ??øΩ?øΩHP??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩÁÇ∑
        }

        //enemyLeaderHP -= attackCard.model.power; // ??øΩ?øΩR??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩg??øΩ?øΩA??øΩ?øΩE??øΩ?øΩg??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ??øΩ?øΩ

        attackCard.model.mana -= attackCard.model.needmana;
        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
        Debug.Log("??øΩ?øΩG??øΩ?øΩ??øΩ?øΩHP??øΩ?øΩÕÅA" + enemyLeaderHP);
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