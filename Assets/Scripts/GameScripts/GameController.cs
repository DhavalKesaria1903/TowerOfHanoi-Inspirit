using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Singleton instnce for GameController class
    /// </summary>
    public static GameController instance;

    /// <summary>
    /// Bool to check if the game is playing or not
    /// </summary>
    public bool isPlaying = false;

    /// <summary>
    /// List of all the available disk
    /// </summary>
    [SerializeField]
    public GameObject[] disks;

    /// <summary>
    /// Waypoint to get the Pole Position
    /// </summary>
    private Vector3 wayPointA, wayPointB, wayPointC;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    GameObject[] diskSeq;

    /// <summary>
    /// List of position for the point A
    /// </summary>
    [SerializeField]
    Vector3[] posASeq;

    /// <summary>
    /// List of position for the point B
    /// </summary>
    [SerializeField]
    Vector3[] posBSeq;

    /// <summary>
    /// Sequence Counter
    /// </summary>
    int currentSequence = -1;

    /// <summary>
    /// Total number of disks
    /// </summary>
    [SerializeField]
    public int totalDisks = 5;
    
    /// <summary>
    /// Movement speed of the disk from one position to another
    /// </summary>
    [SerializeField]
    public float speed = 1;
    
    /// <summary>
    /// Currect state
    /// </summary>
    int state = 0;

    /// <summary>
    /// Save the current movement Index
    /// </summary>
    int currentMove = 0;

    /// <summary>
    /// Total user move count
    /// </summary>
    [HideInInspector]
    public int totalCount = 0;

    /// <summary>
    /// Check For The Correct Step
    /// </summary>
    public bool isStepCorrect = false;

    /// <summary>
    /// If want to freeze the input
    /// </summary>
    public bool freezeInput = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        freezeInput = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Do Tween
        DOTween.Init();

        //Get the position of the All the 3 way points
        wayPointA = GameObject.Find("WayPointA").transform.position;
        wayPointB = GameObject.Find("WayPointB").transform.position;
        wayPointC = GameObject.Find("WayPointC").transform.position;

        //Initialize the array with the larger number then reqire just of the best practice
        diskSeq = new GameObject[150];
        posASeq = new Vector3[150];
        posBSeq = new Vector3[150];

        //Pass the data to main Hanoi Logic fnction
        Hanoi(totalDisks, wayPointA, wayPointC, wayPointB);

        for (int i = 0; i < Mathf.Pow(2, totalDisks) - 1; i++)
        {
            //Debug.Log("Move disk " + diskSeq[i] + " from " + posASeq[i] + " to " + posBSeq[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            state++;
        }
    }

    public void StartGame()
    {
        isPlaying = true;
        //StartCoroutine(MoveAll());
        UIController.instance.ShowHideWelcomePanel(false);
        UIController.instance.ShowHideInGameScreen(true);
        GetComponent<Timer>().timerIsRunning = true;
        freezeInput = false;
    }

    IEnumerator MoveAll()
    {
        //Debug.Log("Pow = " + (Mathf.Pow(2, totalDisks) - 2));
        for (int i = 0; i < Mathf.Pow(2, totalDisks) - 1; i++)
        {
            yield return StartCoroutine(Move(diskSeq[i], posASeq[i], posBSeq[i]));
            if (i == Mathf.Pow(2, totalDisks) - 2)
                state++;
        }
    }

    void Hanoi(int n, Vector3 from, Vector3 to, Vector3 aux)
    {
        if (n == 1)
        {
            AddToSequence(disks[0], from, to);
            return;
        }
        Hanoi(n - 1, from, aux, to);
        AddToSequence(disks[n - 1], from, to);
        Hanoi(n - 1, aux, to, from);
    }

    void AddToSequence(GameObject disk, Vector3 from, Vector3 to)
    {
        currentSequence++;
        diskSeq[currentSequence] = disk;
        posASeq[currentSequence].Set(from.x, from.y, from.z);
        posBSeq[currentSequence].Set(to.x, to.y, to.z);
    }

    public void checkTheCurrentMove(GameObject disk)
    {
        if (disk != null)
        {
            if (!DragDropScript.instance.isMouseDragging && IsDiskIsAtCorrectLocation(disk))
            {
                if (disk == diskSeq[currentMove])
                {
                    isStepCorrect = true;
                    UIController.instance.ShowWrongMove(false);
                    StartCoroutine(Move(diskSeq[currentMove], disk.transform.position, posBSeq[currentMove]));
                    currentMove++;
                    DragDropScript.instance.getTarget = null;
                }
            }
            else
            {
                disk.GetComponent<IsDraggable>().ResetToPosition();
                return;
            }
        }
    }

    public bool IsDiskIsAtCorrectLocation(GameObject disk)
    {
        if(posBSeq[currentMove].x > 1.0f && disk.GetComponent<IsDraggable>().curretSelectedPole == "Pillar3" ||
            posBSeq[currentMove].x < -1.0f && disk.GetComponent<IsDraggable>().curretSelectedPole == "Pillar1" ||
            posBSeq[currentMove].x > -0.5f && posBSeq[currentMove].x < 0.5f && disk.GetComponent<IsDraggable>().curretSelectedPole == "Pillar2")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator Move(GameObject disk, Vector3 posA, Vector3 posB)
    {
        Debug.Log("Disk = " + disk.name + ", posA = " + posA.ToString() + ", posB = " + posB.ToString());

        disk.transform.DOMove(posA, 2 / speed);
        yield return new WaitForSeconds(2 / speed);
        disk.transform.DOMove(posB, 1 / speed);
        yield return new WaitForSeconds(1 / speed);

        RaycastHit hit;
        if (Physics.Raycast(posB + Vector3.down * 2, Vector3.down, out hit, 6))
        {
            disk.transform.DOMove(hit.point + Vector3.up * .3f, 2 / speed);
            yield return new WaitForSeconds(2 / speed);
        }

        disk.GetComponent<IsDraggable>().SetNewPosition();

        isStepCorrect = false;

        if (currentMove == Mathf.Pow(2, totalDisks) - 2)
        {
            GameOver("Great! \nYou have sucessfully solved the Tower of Hanoi!");
        }
    }

    public void PauseGame(bool pauseStatus)
    {
        Time.timeScale = pauseStatus ? 0 : 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        //TODO: Add logic for Restarting the game
        SceneManager.LoadScene("LoadingScene");
    }

    public void GameOver(string gameOverMessage)
    {
        StopAllCoroutines();
        UIController.instance.GameOver(gameOverMessage);
    }
}
