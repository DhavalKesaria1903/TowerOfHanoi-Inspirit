using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Total time in seconds
    [Tooltip("Add the time in Seconds")]
    public float timeRemaining = 10;
    public bool timerIsRunning = false;

    private void Start()
    {
        // To Starts the timer automatically uncomment following line
        //timerIsRunning = true;
    }

    void Update()
    {
        // Check if the timer is allowed to run or not or timer is completed
        if (timerIsRunning)
        {
            // Check the remaining time is greater then 0 or not
            if (timeRemaining > 0)
            {
                // Deduct the time from the the remaining time
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                // Once the timer is below 0 just set it 0
                timeRemaining = 0;
                // And make the timer bool to false so that it will not go below 0
                timerIsRunning = false;
                // Show Game Over screen once the time is over
                GameController.instance.GameOver("Oops! \nTime's Up! Better luck next time");
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        UIController.instance.UpdateTimerText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }
}