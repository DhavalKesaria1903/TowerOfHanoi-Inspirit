using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
 
    [Header("Camera Settings")]
    [SerializeField] private float _RotationSpeed;
    [SerializeField] public Camera cam;

    [Header("Mouse Settings")]
    private float _MouseX;
    private float _MouseY;


    // Start is called before the first frame update
    void Start()
    {
          cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.freezeInput)
        {
            // Get the mouse settings
            _MouseX += Input.GetAxis("Mouse X") * _RotationSpeed;
            _MouseY += Input.GetAxis("Mouse Y") * _RotationSpeed;

            cam.transform.rotation = Quaternion.Euler(-_MouseY, _MouseX, 0);           // Rotate the target cam

            transform.rotation = Quaternion.Euler(0, _MouseX, 0);       //Rotate the player with camera
        }
    }
}
