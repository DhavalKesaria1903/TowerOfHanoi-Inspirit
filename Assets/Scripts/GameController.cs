using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public GameObject[] disks;

    private Vector3 wayPointA, wayPointB, wayPointC;

    [SerializeField]
    GameObject[] diskSeq;

    [SerializeField]
    Vector3[] posASeq;

    [SerializeField]
    Vector3[] posBSeq;

    int i = -1;

    [SerializeField]
    public int totalDisks = 5;
    
    [SerializeField]
    public float speed = 1;
    
    int state = 0;


    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

        wayPointA = GameObject.Find("WayPointA").transform.position;
        wayPointB = GameObject.Find("WayPointB").transform.position;
        wayPointC = GameObject.Find("WayPointC").transform.position;

        diskSeq = new GameObject[150];
        posASeq = new Vector3[150];
        posBSeq = new Vector3[150];

        Hanoi(totalDisks, wayPointA, wayPointC, wayPointB);

        for (int i = 0; i < Mathf.Pow(2, totalDisks) - 1; i++)
        {
            Debug.Log("Move disk " + diskSeq[i] + " from " + posASeq[i] + " to " + posBSeq[i]);
        }
        StartCoroutine(MoveAll());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            state++;
        }
    }

    IEnumerator MoveAll()
    {
        Debug.Log("Pow = " + (Mathf.Pow(2, totalDisks) - 2));
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
        i++;
        diskSeq[i] = disk;
        posASeq[i].Set(from.x, from.y, from.z);
        posBSeq[i].Set(to.x, to.y, to.z);
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
            Debug.Log(hit.collider.name);
            Debug.DrawRay(posB + Vector3.down * 2, Vector3.down, Color.black);
            Debug.Log(">>>>" + "Name = " + hit.collider.name + " & Position = " + hit.point + Vector3.up * .12f);
            disk.transform.DOMove(hit.point + Vector3.up * .3f, 2 / speed);
            yield return new WaitForSeconds(2 / speed);
        }
    }
}
