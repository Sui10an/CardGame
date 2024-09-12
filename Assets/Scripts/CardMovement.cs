using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    //public Transform carryCard;
    public bool canDrag = true; /// �y����@�z�J�[�h�𓮂����邩�ǂ����̃t���O

    public void OnBeginDrag(PointerEventData eventData) // �h���b�O���n�߂�Ƃ��ɍs������
    {

        CardController card = GetComponent<CardController>();
        canDrag = false;

        if (card.model.kinds == CardModel.Kinds.hand) // ��D�̃J�[�h�Ȃ�
        {
            if (card.model.canUse == true) // �}�i�R�X�g��菭�Ȃ��J�[�h�͓������Ȃ�
            {
                canDrag = true;
            }
        }else
        if(card.model.kinds == CardModel.Kinds.mana)
        {
            if (card.model.canAttack == true && card.model.canUse) // �U���s�\�ȃJ�[�h�͓������Ȃ�
            {
                canDrag = true;
            }
        }else
        if(card.model.kinds == CardModel.Kinds.playerGun)
        {
            if (card.model.canAttack == true) // �U���s�\�ȃJ�[�h�͓������Ȃ�
            {
                canDrag = true;
            }
        }

        if (canDrag == false)
        {
            return;
        }

        cardParent = transform.parent;
        transform.SetParent(cardParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycasts���I�t�ɂ���
    }

    public void OnDrag(PointerEventData eventData) // �h���b�O�������ɋN��������
    {
        if (canDrag == false) ///�y����B�z�t���O��False�Ȃ珈�����~�߂�
        {
            return;
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // �J�[�h�𗣂����Ƃ��ɍs������
    {
        if (canDrag == false) ///�y����B�z�t���O��False�Ȃ珈�����~�߂�
        {
            return;
        }

        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycasts���I���ɂ��� 
    }

    public IEnumerator AttackMotion(Transform target)
    {
        Vector3 currentPosition = transform.position;
        cardParent = transform.parent;

        transform.SetParent(cardParent.parent); // card�̐e���ꎞ�I��Canvas�ɂ���

        transform.DOMove(target.position, 0.25f);
        yield return new WaitForSeconds(0.25f);
        transform.DOMove(currentPosition, 0.25f);
        yield return new WaitForSeconds(0.25f);

        transform.SetParent(cardParent); // card�̐e�����ɖ߂�
    }
}