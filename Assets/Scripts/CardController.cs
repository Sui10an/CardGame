using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public CardView view; // �J�[�h�̌����ڂ̏���
    public CardModel model; // �J�[�h�̃f�[�^������
    public CardMovement movement;  // �ړ�(movement)�Ɋւ��邱�Ƃ𑀍�

    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }

    public void Init(int cardID, bool playerCard) // �J�[�h�𐶐��������ɌĂ΂��֐�
    {
        model = new CardModel(cardID, playerCard); // �J�[�h�f�[�^�𐶐�
        view.Show(model); // �\��
    }
    public void ManaSporn(int cardID, bool playerCard) // �J�[�h�𐶐��������ɌĂ΂��֐�
    {
        model = new CardModel(cardID, playerCard); // �J�[�h�f�[�^�𐶐�
        view.Show(model); // �\��
        model.FieldCard = true; // �t�B�[���h�̃J�[�h�̃t���O�𗧂Ă�
        model.ManaCard = true;
    }
    public void SpornCard(int cardID, bool playerCard) // �J�[�h�𐶐��������ɌĂ΂��֐�
    {
        model = new CardModel(cardID, playerCard); // �J�[�h�f�[�^�𐶐�
        view.Show(model); // �\��
        model.FieldCard = true; // �t�B�[���h�̃J�[�h�̃t���O�𗧂Ă�
    }
    public void SRSporn(int cardID, bool playerCard) // �J�[�h�𐶐��������ɌĂ΂��֐�
    {
        model = new CardModel(cardID, playerCard); // �J�[�h�f�[�^�𐶐�
        view.Show(model); // �\��
        model.FieldCard = true;// �t�B�[���h�̃J�[�h�̃t���O�𗧂Ă�
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
        model.FieldCard = true; // �t�B�[���h�̃J�[�h�̃t���O�𗧂Ă�
        model.canUse = false;
        if (model.cardId == 1)
        {
            model.SRuse = true;
        }
        view.SetCanUsePanel(model.canUse); // �o��������CanUsePanel������
        Destroy(this.gameObject);
    }
    
}