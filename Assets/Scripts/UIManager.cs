using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject changeTurnPanel, changePhasePanel, waitStart, win, lose, youLose;
    [SerializeField] Image changeTurnPanelImage;
    [SerializeField] Sprite start, yourTurn, enemyTurn, battlePhaseButton, battlePhase, joshin;
    public bool isStart;

    void Awake()
    {
        changeTurnPanelImage = changeTurnPanel.GetComponent<Image>();
    }

    public IEnumerator ShowChangeTurnPanel()
    {

        if (GameManager.instance.isPlayerTurn == true)
        {
            if (GameManager.instance.isNotBattlePhase == false)
            {
                yield return StartCoroutine(ShowPanel(battlePhase, 15f, 1.5f));
            }
            else
            {
                yield return StartCoroutine(ShowPanel(yourTurn, 15f, 1.5f));
            }
        }
        else
        {
            yield return StartCoroutine(ShowPanel(enemyTurn, 15f, 1.5f));
        }
    }

    public void ShowChangePhasePanel()
    {
        changePhasePanel.SetActive(true);
    }

    public void Stoper()
    {
        changePhasePanel.SetActive(false);
    }

    public IEnumerator ShowPanel(Sprite sprite, float speed, float time)
    {
        changeTurnPanelImage.sprite = sprite;
        changeTurnPanelImage.color = new Color(0, 0, 0, 1f);
        changeTurnPanel.SetActive(true);
        while(changeTurnPanelImage.color.r < 1f)
        {
            Color color = changeTurnPanelImage.color;
            color.r += speed/255f;
            color.g += speed/255f;
            color.b += speed/255f;
            //color.a += speed/255f;
            changeTurnPanelImage.color = color;
            yield return new WaitForSeconds(1f/60f);
        }
        yield return new WaitForSeconds(time);
        while(changeTurnPanelImage.color.r > 0)
        {
            Color color = changeTurnPanelImage.color;
            color.r -= speed/255f;
            color.g -= speed/255f;
            color.b -= speed/255f;
            //color.a -= speed/255f;
            changeTurnPanelImage.color = color;
            yield return new WaitForSeconds(1f/60f);
        }
        changeTurnPanel.SetActive(false);
        changeTurnPanelImage.color = new Color(1f, 1f, 1f, 1f);
    }

    public IEnumerator Title()
    {
        yield return ShowPanel(start, 10f, 2f);
    }

    public IEnumerator StartBeginMatch()
    {
        waitStart.SetActive(true);
        yield return new WaitUntil(() => isStart);
        waitStart.SetActive(false);
        GameManager.instance.isBeginMatch = false;
    }

    public void StartMatch()
    {
        isStart = true;
    }

    public void End()
    {
        GameManager.instance.isBeginMatch = true;
        isStart = true;
    }

    public IEnumerator Result(bool isWin)
    {
        if(isWin)
        {
            isStart = false;
            win.SetActive(true);
            yield return new WaitUntil(() => isStart);
            win.SetActive(false);
            GameManager.instance.StartStart();
        }else
        {
            int random = Random.Range(0,100);
            GameObject itokawaJoshin = random < 99 ? lose : youLose;
            isStart = false;
            itokawaJoshin.SetActive(true);
            yield return new WaitUntil(() => isStart);
            itokawaJoshin.SetActive(false);
            GameManager.instance.StartStart();
        }
    }

    public void Reset()
    {
        changeTurnPanel.SetActive(false);
        changePhasePanel.SetActive(false);
    }

    public IEnumerator IJ()
    {
        yield return ShowPanel(joshin, 7f, 0);
    }
}