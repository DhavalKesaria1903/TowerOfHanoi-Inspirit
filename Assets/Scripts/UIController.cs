using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] GameObject welcomePanel;
    [SerializeField] GameObject instructionPanel;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject inGameScreen;
    [SerializeField] Text timerText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void ShowHideInGameScreen(bool visiblity)
    {
        inGameScreen.SetActive(visiblity);
    }

    public void GameOver()
    { 
        //TODO add the logic for UI when game is Over
    }
}
