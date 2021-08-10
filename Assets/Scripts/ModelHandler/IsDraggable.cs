using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDraggable : MonoBehaviour
{
    public bool drag = false;
    private Vector3 currentDiskPosition;

    private void Awake()
    {
        currentDiskPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there is any disk is there on top of the disk
        RaycastHit hit;
        Debug.DrawRay(gameObject.transform.position, Vector3.up, Color.black);
        if (Physics.Raycast(gameObject.transform.position, Vector3.up, out hit, 6))
        {
            // If the disk is there then don't allove the disk to drag
            if (hit.collider.name.Contains("Disk"))
            {
                // Disable the collider so that mouse click can not recognise this disk
                drag = false;
                //GetComponent<BoxCollider>().enabled = false;
            }
        }
        else
        {
            // if there is no disk above the this disk then allow them to drad
            // Enable the collider so that mouse click can recognise this disk
            drag = true;
            //GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Pillar") &&
            DragDropScript.instance.getTarget == gameObject)
        {
            GameController.instance.checkTheCurrentMove(gameObject);
            //Debug.Log(other.name);
        }
    }

    public void SetNewPosition()
    {
        currentDiskPosition = gameObject.transform.position;
    }

    public void ResetToPosition()
    {
        gameObject.transform.position = currentDiskPosition;
    }
}
