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
    [SerializeField] Transform playerHand, playerField, enemyField,playerManaField,trap;
    [SerializeField] GameObject useField,cover;
    [SerializeField] TextMeshProUGUI pBaff,eBaff;
    [SerializeField] TextMeshProUGUI playerLeaderHPText,enemyLeaderHPText,bHP,eBHP;
    [SerializeField] TextMeshProUGUI playerManaPointText, playerDefaultManaPointText;
    [SerializeField] Transform playerLeaderTransform;

    public int playerManaPoint; // �g�p����ƌ���}�i�|�C���g
    public int playerDefaultManaPoint; // ���^�[�������Ă����x�[�X�̃}�i�|�C���g
    public int playerManaPlus; //�}�i�v���X�̃J�E���g�𑫂���
    public int TrunCount;
    public int playerBlockHP;
    public int enemyBlockHP;
    public int hPCartenP;
    public int hPCartenE;
    public int PBaff;
    public int EBaff;
    GridLayoutGroup _gridLayoutGroup;

    public bool isPlayerTurn = true; //�@Public�֕ύX
    public bool isnotBattleFaiz = true;
    public bool isFileder = true;
    public bool isCartenP = true;
    public bool isCartenE = true;
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

    void StartGame() // �����l�̐ݒ� 
    {
        TrunCount = 0;

        enemyLeaderHP = 10;
        playerLeaderHP = 10;
        ShowLeaderHP();

        /// �}�i�̏����l�ݒ� ///
        playerManaPoint = 1;
        playerDefaultManaPoint = 1;
        ShowManaPoint();

        //�f�b�L���V���t��
        Shuffle();

        // ������D��z��
        SetStartHand();

        // �����Ֆ�
        CreateSporn(1, playerField);
        CreateSporn(1, enemyField);

        // ���b�ƃo�t
        playerBlockHP = 0;
        enemyBlockHP = 0;
        isCartenP = false;
        isCartenE = false;
        hPCartenP = 0;
        hPCartenE = 0;
        PBaff = 0;
        EBaff = 0;

        // �^�[���̌���
        StartCoroutine(TurnCalc());
    }
    void Shuffle() // �f�b�L���V���b�t������
    {
        // ���� n �̏����l�̓f�b�L�̖���
        int n = deck.Count;

        // n��1��菬�����Ȃ�܂ŌJ��Ԃ�
        while (n > 1)
        {
            n--;

            // k�� 0 �` n+1 �̊Ԃ̃����_���Ȓl
            int k = UnityEngine.Random.Range(0, n + 1);

            // k�Ԗڂ̃J�[�h��temp�ɑ��
            int temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    void ShowManaPoint() // �}�i�|�C���g��\�����郁�\�b�h
    {
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
        //�}�i�J�[�h�����
        SetManaCard();
    }

    void CreateCard(int cardID, Transform place)
    {
        CardController card = Instantiate(cardPrefab, place);
        // Player�̎�D�ɐ������ꂽ�J�[�h��Player�̃J�[�h�Ƃ���
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
        // Player�̎�D�ɐ������ꂽ�J�[�h��Player�̃J�[�h�Ƃ���
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
        // Player�̎�D�ɐ������ꂽ�J�[�h��Player�̃J�[�h�Ƃ���
        if (place == playerManaField)
        {
            card.ManaSporn(cardID, true);
        }
        else
        {
            card.ManaSporn(cardID, false);
        }
    }


    void DrawCard(Transform hand) // �J�[�h������
    {
        // �f�b�L���Ȃ��Ȃ�����Ȃ�
        if (deck.Count == 0)
        {
            return;
        }

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();

        if (playerHandCardList.Length < 7)
        {
            // �f�b�L�̈�ԏ�̃J�[�h�𔲂����A��D�ɉ�����
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }

        SetCanUsePanelHand();
    }

    void SetStartHand() // ��D��3���z��
    {
        for (int i = 0; i < 3; i++)
        {
            DrawCard(playerHand);
        }
    }

    void SetManaCard()
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        int Point = playerManaPoint;
        if (Point < playerManaCardList.Length)
        {
            int RS = playerManaCardList.Length - Point;
            for (int CC = 0; CC != RS; CC++)
            {
                cardPrefab.DestroyCard(playerManaCardList[0]);
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

    IEnumerator TurnCalc() // �^�[�����Ǘ�����
    {
        yield return StartCoroutine(uIManager.ShowChangeTurnPanel());
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            //EnemyTurn(); // �R�����g�A�E�g����
            StartCoroutine(EnemyTurn()); // StartCoroutine�ŌĂяo��
        }
    }
    public void PhaseCalc()
    {
        uIManager.ShowChangePhasePanel();
    }

    public void ChangeTurn() // �^�[���G���h�{�^���ɂ��鏈��
    {
        isPlayerTurn = !isPlayerTurn; // �^�[�����t�ɂ���
        uIManager.Stoper();
        ReSetCanUsePanelHand();
        StartCoroutine(TurnCalc()); // �^�[���𑊎�ɉ�
    }

    public void ChangePhase() // �^�[���G���h�{�^���ɂ��鏈��
    {
        isnotBattleFaiz = false;
        isFileder = false;
        PhaseCalc(); // �^�[���𑊎�ɉ�
    }

    void PlayerTurn()
    {
        TrunCount = TrunCount + 1;
        Debug.Log("Player�̃^�[��");
        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        CheckHPID(playerFieldCardList);

        if (isFileder == true)
        {
            PanelOn();
        }

        if(isnotBattleFaiz == true)
        {
            /// �}�i�𑝂₷
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

            DrawCard(playerHand); // ��D���ꖇ������
            if(isCartenP == true)
            {
                hPCartenP = 0;
                isCartenP = false;
            }
            if(PBaff != 0)
            {
                PBaff = 0;
                EBaff = 0;
            }
            else if (EBaff != 0)
            {
                PBaff = 0;
                EBaff = 0;
            }
        }
    }

    public void BattleFaiz()//�o�g���t�F�[�Y��ݒ�
    {
        ReSetCanUsePanelHand();
        if (TrunCount == 1)
        {
            uIManager.Stoper();
            ChangeTurn();
        }
        else
        {
            uIManager.Stoper();
            StartCoroutine(TurnCalc());
            CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
            SetAttackableFieldCard(playerFieldCardList, true);
            PanelOff();
        }
        
    }

    public void ReduceManaPoint(int cost) // �R�X�g�̕��A�}�i�|�C���g�����炷
        {
            playerManaPoint -= cost;
            ShowManaPoint();

            SetCanUsePanelHand();
         }
    public void CardEffect(int cardId) // �J�[�h�̌��ʂ����Ă���
    {
        if (1 <= cardId && cardId <= 4) //�e�͂�����
        {
            CreateSporn(cardId, playerField);
        }
        if (cardId == 5) //�e��
        {
            playerManaPlus += 1;
            playerDefaultManaPoint++;

            ShowManaPoint();
        }
        if (cardId == 6) //���e
        {
            
        }
        if(cardId == 7) //��ÃL�b�g
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
        if (cardId == 8) //�h�e�`���b�L
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
        if (cardId == 9) //�M���e
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
        }
        if (cardId == 10) //�~�������v��
        {
            
        }
        if (cardId == 11) //�h�q�����̐i�R
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
        if (cardId == 12) //�K���i�[�̑���
        {
            
        }
        if (cardId == 13) //��@��
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
        if (cardId == 14) //�g�}�K
        {

        }
        if (cardId == 15) //�X�R
        {
            //CardController attackCard = ;
            //ChangePower(attackCard);
        }
    }

    public void SetCanUsePanelMana() // ��D�̃J�[�h���擾���āA�g�p�\�ȃJ�[�h��CanUse�p�l����t����
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerManaCardList)
        {
            card.model.canUse = true;
            card.model.canAttack = true;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }

    public void ReSetCanUsePanelMana() // ��D�̃J�[�h���擾���āA�g�p�s�\�ȃJ�[�h�ɂ���
    {
        CardController[] playerManaCardList = playerManaField.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerManaCardList)
        {
            card.model.canUse = false;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }
    void SetCanUsePanelHand() // ��D�̃J�[�h���擾���āA�g�p�\�ȃJ�[�h��CanUse�p�l����t����
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

    void ReSetCanUsePanelHand() // ��D�̃J�[�h���擾���āA�g�p�s�\�ȃJ�[�h�ɂ���
    {
        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        foreach (CardController card in playerHandCardList)
        {
            card.model.canUse = false;
            card.view.SetCanUsePanel(card.model.canUse);
        }
    }

    //void EnemyTurn() // �R�����g�A�E�g����
    IEnumerator EnemyTurn() // StartCoroutine�ŌĂ΂ꂽ�̂ŁAIEnumerator�ɕύX
    {
        PanelOff();
        cover.SetActive(true);
        TrunCount = TrunCount + 1;
        isFileder = false;

        if (EBaff != 0)
        {
            PBaff = 0;
            EBaff = 0;
        }
        else if (PBaff != 0)
        {
            PBaff = 0;
            EBaff = 0;
        }

        Debug.Log("Enemy�̃^�[��");

        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();

        yield return new WaitForSeconds(1f);

        /// �G�̃t�B�[���h�̃J�[�h���U���\�ɂ��āA�΂̘g��t���� ///
        SetAttackableFieldCard(enemyFieldCardList, true);

        yield return new WaitForSeconds(1f);

        if (enemyFieldCardList.Length < 3)
        {
            int iD = UnityEngine.Random.Range(2, 4);
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
            // �U���\�J�[�h���擾
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
        ChangeTurn(); // �^�[���G���h����
    }


    public void CardBattle(CardController attackCard, CardController defenceCard)
    {

        // �U���J�[�h�ƍU�������J�[�h�������v���C���[�̃J�[�h�Ȃ�o�g�����Ȃ�
        if (attackCard.model.PlayerCard == defenceCard.model.PlayerCard)
        {
            if(attackCard.model.ManaCard == true)
            {
                if(defenceCard.model.needmana == 0)
                {
                    return;
                }
                defenceCard.model.mana += 1;
                playerManaPoint -= 1;
                attackCard.DestroyCard(attackCard);
                ShowManaPoint();
            }
            else
            {
                return;   
            }

        }

        // �U���J�[�h���A�^�b�N�\�łȂ���΍U�����Ȃ��ŏ����I������
        if (attackCard.model.canAttack == false)
        {
            return;
        }

        // �U�����̃p���[�����������ꍇ�A�U�����ꂽ�J�[�h��j�󂷂�
        if (attackCard.model.power > defenceCard.model.power)
        {
            return;
            //defenceCard.DestroyCard(defenceCard);
        }

        // �U�����ꂽ���̃p���[�����������ꍇ�A�U�����̃J�[�h��j�󂷂�
        if (attackCard.model.power < defenceCard.model.power)
        {
            return;
            //attackCard.DestroyCard(attackCard);
        }

        // �p���[�������������ꍇ�A�����̃J�[�h��j�󂷂�
        if (attackCard.model.power == defenceCard.model.power)
        {
            return;
            /*attackCard.DestroyCard(attackCard);
            defenceCard.DestroyCard(defenceCard);*/
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

        if (attackCard.model.PlayerCard == true) // attackCard���v���C���[�̃J�[�h�Ȃ�
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
            enemyLeaderHP -= attackpower; // �G�̃��[�_�[��HP�����炷
        }
        else // attackCard���G�̃J�[�h�Ȃ�
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
            playerLeaderHP -= attackpower; // �v���C���[�̃��[�_�[��HP�����炷
        }

        //enemyLeaderHP -= attackCard.model.power; // �R�����g�A�E�g����

        baff = 0;
        attackCard.model.mana -= attackCard.model.needmana;
        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);
        Debug.Log("�G��HP�́A" + enemyLeaderHP);
        ShowLeaderHP();
    }

    public void ChangePower(CardController attackCard)
    {
        attackCard.model.power = attackCard.model.power + 1;
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
}
