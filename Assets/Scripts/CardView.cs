using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, powerText, costText, attackText;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject canAttackPanel, canUsePanel, changeSrPanel;
    [SerializeField] GameObject manaCost, attackCost;

    public void Show(CardModel cardModel) // cardModelのデータ取得と反映
    {
        nameText.text = cardModel.name;
        powerText.text = cardModel.power.ToString();
        costText.text = cardModel.cost.ToString();
        iconImage.sprite = cardModel.icon;
        costText.text = cardModel.cost.ToString();
        attackText.text = cardModel.needmana.ToString();
        if (0 < cardModel.cardId && cardModel.cardId < 5)
        {
            attackCost.SetActive(true);
        }
        else
        {
            manaCost.SetActive(true);
        }
    }

    public void SetCanAttackPanel(bool flag)
    {
        canAttackPanel.SetActive(flag);
    }

    public void SetCanUsePanel(bool flag) // フラグに合わせてCanUsePanelを付けるor消す
    {
        canUsePanel.SetActive(flag);
    }

    public void ChangeSR(bool flag)
    {
        changeSrPanel.SetActive(flag);
    }
}