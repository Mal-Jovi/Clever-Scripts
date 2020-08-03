using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phyicsMovement : MonoBehaviour
{
    public Transform playerCamera;
    public Transform orientation;

    private Rigidbody rigidbody;

    //Rotation and Looking
    private float xRotation;
    private float sensitivity = 50;
    private float sensitivityMultiplier = 1;

    //Movement
    public float movementSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    public float threshold = 0.01f;
    public float maxSlopeAngle = 35.0f;

    //Crouch and Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1f);
    private Vector3 playerScale;
    public float slideForce = 400f;
    public float slideCounterForce = 0.2f;

    //Jumping
    private bool readyToJump = true;
    private float jumpCoolDown = 0.25f;
    public float jumpForce = 550f;

    //Input
    float x, y;
    bool isJumping, isSprinting, isCrouching;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        MyInput();
        //Look();
    }

    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        isJumping = Input.GetButton("Jump");
        isCrouching = Input.GetButton("Crouch");

        //Crouching
        if (Input.GetButtonDown("Crouch"))
        {
            StartCrouch();
        }
        if (Input.GetButtonUp("Crouch"))
        {
            StopCrouch();
        }
    }

    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

        //If the player is moving and and on the ground, then make them slide
        if(rigidbody.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rigidbody.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        //When the crouch button is released, return the scale of the player back to normal 
        //and correct their position when that happens
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement()
    {
        //Extra Gravity
        rigidbody.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where the player is looking
        Vector2 magnitude = FindVelocityRelativeToLook();
        float xMag = magnitude.x, yMag = magnitude.y;

        //Counteract sliding and sloppy movement
        counterMovementForce(x, y, magnitude);

        //If holding jump && ready to jump, then the player can jump
        if (readyToJump && isJumping)
        {
            Jump();
        }

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //If sliding down a ramp, add a force down so that they player stays grounded and also builds up speed
        if(isCrouching && grounded && readyToJump)
        {
            rigidbody.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go past max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;
    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            readyToJump = false;

            //Add jump forces
            rigidbody.AddForce(Vector2.up * jumpForce * 1.5f);
            rigidbody.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity
            Vector3 velocity = rigidbody.velocity;
            if (rigidbody.velocity.y < 0.5f)
            {
                rigidbody.velocity = new Vector3(velocity.x, 0, velocity.z);
            }
            else if (rigidbody.velocity.y > 0)
            {
                rigidbody.velocity = new Vector3(velocity.x, velocity.y / 2, velocity.z);
            }

            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }

    private void ResetJump()
    {
        readyToJump = false;
    }

    private void counterMovementForce(float x, float y, Vector2 magnitude)
    {
        //If the player is not on the ground or is jumping, then do nothing
        if (!grounded || isJumping) return;

        //Slow down sliding
        if (isCrouching)
        {
            rigidbody.AddForce(movementSpeed * Time.deltaTime * -rigidbody.velocity.normalized * slideCounterForce);
            return;
        }


    }

    public Vector2 FindVelocityRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rigidbody.velocity.x, rigidbody.velocity.z) * Mathf.Rad2Deg;

        float angleDifference = Mathf.DeltaAngle(lookAngle, moveAngle);
        float rightAngleDifference = 90 - angleDifference;

        float magnitude = rigidbody.velocity.magnitude;

        float yMag = magnitude * Mathf.Cos(angleDifference * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(rightAngleDifference * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool isFloor(Vector3 vector)
    {
        float angle = Vector3.Angle(Vector3.up, vector);
        return angle < maxSlopeAngle;
    }

}
