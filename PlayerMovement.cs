using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    //Regarding movement
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float crouchSpeed = 3.0f;
    public float walkingSpeed = 6.0f;
    public float boostSpeed = 8.0f;
    public float overallCharacterControllerSpeed;

    //Jumping related variables
    private const int maxNumberOfJumps = 2;
    private int currentJump = 0;

    //Player Input variables
    bool jumping, crouching, grounded;
    float x, y;

    //Direction of Movement Vector
    private Vector3 moveDirection = Vector3.zero;

    //Timing variables for temporary variable increases for such things such as speed
    private float timer = 0.0f;
    private float crouchCoolDownTimer = 0.0f;
    public float slidingTime = 1.0f;
    public float slideBoostDecay = 0.075f;
    public float FOVincrement = 0.075f;

    bool crouchBoosting = false;
    bool crouchDelayed = false;
    bool stopCrouch = false;    //TODO: why the fuck did you call this bool stopCrouch, refactor to 'startEasingCrouch'
    bool FOVincreasing = false;

    //Camera variables
    public float baseCameraFOV = 90.0f;
    public float slidingBoostCameraFOV = 100.0f;
    public float currentCameraFOV = 0.0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentCameraFOV = baseCameraFOV;
    }

    void Update()
    {
        overallCharacterControllerSpeed = characterController.velocity.magnitude;
        grounded = characterController.isGrounded;
        //Debug.Log("Current Player Speed is: " + speed);
        getInput();
        Movement();
        CrouchBoost();
        EaseCrouchBoost();
        IncreaseFOV();
        
        Camera.main.fieldOfView = currentCameraFOV;

        Debug.Log(currentCameraFOV);
    }
    
    private void getInput()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        jumping = Input.GetKeyDown(KeyCode.Space);
        Crouching();
    }

    private void Movement()
    {
        if (grounded)
        {
            //We are grounded, so recalculate and move the direction directly from axes
            moveDirection = new Vector3(x, 0.0f, y);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            currentJump = 0;
        }
        else
        {
            //This recalculates the direction of motion if the player happens to be in the air
            moveDirection = new Vector3(x, moveDirection.y, y);
            moveDirection = transform.TransformDirection(moveDirection);

            //speed is not multiplied with y otherwise you go flying up
            moveDirection.x *= speed;
            moveDirection.z *= speed;
        }

        if (jumping && (grounded || maxNumberOfJumps > currentJump))
        {
            moveDirection.y = jumpSpeed;
            currentJump++;
        }

        //Apply gravity, which is multiplied by deltaTime twice, once here and again when moveDirection is multiplied
        moveDirection.y -= gravity * Time.deltaTime;

        //Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void Crouching()
    {
        crouching = Input.GetKeyDown(KeyCode.LeftControl);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouchBoosting = true;
            stopCrouch = false;

            //Only zoomout the FOV when crouchsliding if the player is on the ground and is moving, changing the FOV while in the air is not enjoyable tbh imo ttyl
            if (grounded && overallCharacterControllerSpeed > 0f)
            {
                FOVincreasing = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            characterController.height = 2;
            //Having these two bools set false here accounts for the player letting go of crouch before easeCrouching has a chance to activate
            stopCrouch = false;
            FOVincreasing = false;
            speed = walkingSpeed;
            timer = 0.0f;
            crouchBoosting = false;

            CrouchCoolDown();

            currentCameraFOV = baseCameraFOV;
        }
    }

    private void CrouchBoost()
    {
        if (crouchBoosting)
        {
            if (timer == 0.0f) speed = boostSpeed;

            timer += Time.deltaTime;
            if (timer > slidingTime)
            {
                timer = 0.0f;
                crouchBoosting = false;
                stopCrouch = true;
                FOVincreasing = false;
            }
        }
    }

    private void CrouchCoolDown()
    {
        crouchDelayed = true;
        crouchCoolDownTimer += Time.deltaTime;

        if(crouchCoolDownTimer > 0.5f)
        {
            crouchDelayed = false;
            crouchCoolDownTimer = 0.0f;
        }
    }

    private void EaseCrouchBoost()
    {
        if (stopCrouch)
        {
            speed = speed - slideBoostDecay;
            currentCameraFOV = currentCameraFOV - Mathf.Exp(slideBoostDecay * 0.5f);

            slideBoostDecay = slideBoostDecay + 0.025f;

            if (speed < crouchSpeed)
            {
                speed = crouchSpeed;
                stopCrouch = false;
                slideBoostDecay = 0.075f;
            }

            if(currentCameraFOV < baseCameraFOV)
            {
                currentCameraFOV = baseCameraFOV;
            }
        }
    }

    private void IncreaseFOV()
    {
        if (FOVincreasing)
        {
            currentCameraFOV = currentCameraFOV + Mathf.Exp(FOVincrement * 0.5f);
            characterController.height = characterController.height - Mathf.Exp(FOVincrement * 0.5f);

            FOVincrement += 0.10f;

            if (currentCameraFOV > slidingBoostCameraFOV)
            {
                currentCameraFOV = slidingBoostCameraFOV;
                FOVincreasing = false;
            }

            if (characterController.height < 1f) characterController.height = 1.0f;
        }
    }
}