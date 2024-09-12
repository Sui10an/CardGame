using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// �U������鑤�̃R�[�h
public class AttackedCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        /// �U��
        // attacker��I���@�}�E�X�|�C���^�[�ɏd�Ȃ����J�[�h���A�^�b�J�[�ɂ���
        CardController attackCard = eventData.pointerDrag.GetComponent<CardController>();

        // defender��I���@
        CardController defenseCard = GetComponent<CardController>();

        // �o�g������
        if(attackCard != null)
        {
            if(attackCard.gameObject.GetComponent<CardMovement>().canDrag)
            {
                GameManager.instance.CardBattle(attackCard, defenseCard);
            }
        }
    }
}