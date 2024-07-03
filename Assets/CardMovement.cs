using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    //public Transform carryCard;
    bool canDrag = true; /// 【解説①】カードを動かせるかどうかのフラグ
    

    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {

        CardController card = GetComponent<CardController>();
        canDrag = true;

        if (card.model.FieldCard == false) // 手札のカードなら
        {
            if (card.model.canUse == false) // マナコストより少ないカードは動かせない
            {
                canDrag = false;
            }

        }
        else
        {
            if (card.model.canAttack == false) // 攻撃不可能なカードは動かせない
            {
                canDrag = false;
            }
        }

        if (canDrag == false)
        {
            return;
        }

        cardParent = transform.parent;
        transform.SetParent(cardParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false; // blocksRaycastsをオフにする
    }

        public void OnDrag(PointerEventData eventData) // ドラッグした時に起こす処理
    {
        if (canDrag == false) ///【解説③】フラグがFalseなら処理を止める
        {
            return;
        }

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) // カードを離したときに行う処理
    {
        if (canDrag == false) ///【解説③】フラグがFalseなら処理を止める
        {
            return;
        }

        transform.SetParent(cardParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true; // blocksRaycastsをオンにする 
    }

    public IEnumerator AttackMotion(Transform target)
    {
        Vector3 currentPosition = transform.position;
        cardParent = transform.parent;

        transform.SetParent(cardParent.parent); // cardの親を一時的にCanvasにする

        transform.DOMove(target.position, 0.25f);
        yield return new WaitForSeconds(0.25f);
        transform.DOMove(currentPosition, 0.25f);
        yield return new WaitForSeconds(0.25f);

        transform.SetParent(cardParent); // cardの親を元に戻す
    }

    
}