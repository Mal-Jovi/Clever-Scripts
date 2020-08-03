using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private const int maxJump = 3;
    private int currentJump = 0;

    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(characterController.isGrounded)
        {
            //We are grounded, so recalculate and move the direction directly from axes
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            currentJump = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && (characterController.isGrounded || maxJump > currentJump))
        {
            moveDirection.y = jumpSpeed;
            currentJump++;
        }

     

        //Apply gravity, which is multiplied by deltaTime twice, once here and again when moveDirection is multiplied
        moveDirection.y -= gravity * Time.deltaTime;

        //Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
    
}
