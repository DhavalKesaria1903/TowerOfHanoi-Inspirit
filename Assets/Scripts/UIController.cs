using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] GameObject welcomePanel;
    [SerializeField] GameObject instructionPanel;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject inGameScreen;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Text winngText;
    [SerializeField] Text timerText;
    [SerializeField] Text totalSteps;
    [SerializeField] RectTransform wrongMoveIndication;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ShowHideInstruction(bool visiblity)
    {
        //TODO add the code to show the Insruction Panel
        instructionPanel.SetActive(visiblity);
    }

    public void ShowHideWelcomePanel(bool visiblity)
    {
        welcomePanel.SetActive(visiblity);
    }

    public void UpdateTimerText(string timerValue)
    {
        timerText.text = timerValue;
    }

    public void ShowHidePauseScreen(bool visiblity)
    {
        if (visiblity == true)
        {
            if (GameController.instance.isPlaying)
            {
                GameController.instance.PauseGame(true);
            }
        }
        else
        {
            if (GameController.instance.isPlaying)
            {
                GameController.instance.PauseGame(false);
            }
        }

        pauseScreen.SetActive(visiblity);
    }

    public void ShowTotalCount(int steps)
    {
        totalSteps.text = "Total Step\n" + steps.ToString();
    }

    public void ShowWrongMove()
    {
        StartCoroutine(ShowWrongImage());
    }

    IEnumerator ShowWrongImage()
    {
        wrongMoveIndication.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        wrongMoveIndication.gameObject.SetActive(false);
    }

    public void ShowHideInGameScreen(bool visiblity)
    {
        inGameScreen.SetActive(visiblity);
    }

    public void GameOver(string winningText)
    {
        winngText.text = winningText;
        gameOverPanel.SetActive(true);
    }
}
