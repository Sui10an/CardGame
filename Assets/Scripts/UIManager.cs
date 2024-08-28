using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject changeTurnPanel,changePhasePanel;
    [SerializeField] Text changeTurnText;
    

    public IEnumerator ShowChangeTurnPanel()
    {
        changeTurnPanel.SetActive(true);

        if (GameManager.instance.isPlayerTurn == true)
        {
            if (GameManager.instance.isnotBattleFaiz == false)
            {
                changeTurnText.text = "Battle Faiz";
            }
            else
            {
                changeTurnText.text = "Your Turn";
            }
        }
        else
        {
            changeTurnText.text = "Enemy Turn";
        }

        yield return new WaitForSeconds(2);

        changeTurnPanel.SetActive(false);
    }

    public void ShowChangePhasePanel()
    {
        changePhasePanel.SetActive(true);
    }
    public void Stoper()
    {
        changePhasePanel.SetActive(false);
    }
}