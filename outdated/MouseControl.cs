using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public float horizontalSpeed = 60.0f;
    public float verticalSpeed = 60.0f;
    
    float mouseX;
    float mouseY;

    private float xAxisClamp = 0.0f;

    [SerializeField] private Transform playerBody;

    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Get the mouse delta, This is not in the range -1 to 1.
        mouseX = Time.deltaTime * horizontalSpeed * Input.GetAxis("Mouse X");
        mouseY = Time.deltaTime * verticalSpeed * Input.GetAxis("Mouse Y");

        xAxisClamp += mouseY;

        if(xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampxAxisRotationToValue(270.0f);
        }
        else if(xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampxAxisRotationToValue(90.0f);
        }


        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    private void ClampxAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
