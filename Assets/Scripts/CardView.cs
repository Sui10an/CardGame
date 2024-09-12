using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, powerText, costText, attackText;
    [SerializeField] public TextMeshProUGUI manas;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject canAttackPanel, canUsePanel, changeSrPanel, changeSrButton, selectPanel, Aap;
    [SerializeField] GameObject manaCost, attackCost, manaPanel, breakPanel;

    public void Show(CardModel cardModel) // cardModelのデータ取得と反映
    {
        nameText.text = cardModel.name;
        powerText.text = cardModel.power.ToString();
        costText.text = cardModel.cost.ToString();
        iconImage.sprite = cardModel.icon;
        costText.text = cardModel.cost.ToString();
        manas.text = cardModel.mana.ToString();
        attackText.text = cardModel.needMana.ToString();
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

    public void SetCanUsePanel(bool flag) // �t���O�ɍ��킹��CanUsePanel��t����or����
    {
        Debug.Log(flag);
        canUsePanel.SetActive(flag);
    }
    public void SetManaPanel(bool flag)
    {
        manaPanel.SetActive(flag);
    }

    public void ChangeSR(bool flag)
    {
        changeSrPanel.SetActive(flag);
        changeSrButton.SetActive(flag);
    }
    public void SetBomPanel(bool flag) // �t���O�ɍ��킹��CanUsePanel��t����or����
    {
        selectPanel.SetActive(flag);
    }
    public void SetAap(bool flag)
    {
        Aap.SetActive(flag);
    }
    public void SetBreakPanel(bool flag)
    {
        breakPanel.SetActive(flag);
    }
}