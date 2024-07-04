using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClicker : MonoBehaviour, IPointerClickHandler
{

[SerializeField] CardController card;
    public void OnPointerClick(PointerEventData eventData)
    {
        // 爆弾用
        if(GameManager.instance.Bom == true)
        {
            Destroy(card.gameObject);
            GameManager.instance.Bom = false;
            card.view.SetCanUsePanel(true);
            //GameManager.OffSP();
        }
    }
}
