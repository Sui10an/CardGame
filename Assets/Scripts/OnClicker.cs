using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClicker : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        // クリックされた時に行いたい処理
        /*if (model.SRcan == true)
        {
            card.SRSporn(100, true);
            Destroy(card.gameObject);
        }*/
    }
}
