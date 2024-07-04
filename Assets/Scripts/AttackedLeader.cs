using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedLeader : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        /// �U��
        // attacker��I���@�}�E�X�|�C���^�[�ɏd�Ȃ����J�[�h���A�^�b�J�[�ɂ���
        CardController attackCard = eventData.pointerDrag.GetComponent<CardController>();

        GameManager.instance.AttackToLeader(attackCard, true);
    }
}