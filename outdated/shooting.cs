using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class shooting : MonoBehaviour
{
    public GameObject damageCone;
    public Collider damageConeCollider;

    public Animator animator;

    bool isMoving = false;

    private float horizontalMovement;
    private float verticalMovement;

    [Space]
    public float punchStrength = 0.2f;
    public int punchVibrato = 5;
    public float punchDuration = 0.3f;
    [Range(0, 1)]
    public float punchElasticity = 0.5f;

    public Transform shotgunModel;
    private Vector3 shotgunLocalPos;


    void Awake()
    {   
        damageConeCollider = damageCone.GetComponent<CapsuleCollider>();
        damageConeCollider.isTrigger = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        shotgunLocalPos = shotgunModel.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        fireGun();

        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
        animator.SetFloat("movementSpeed", Math.Abs(horizontalMovement + verticalMovement));

       
        
    }

    private void fireGun()
    {
        if (Input.GetMouseButtonDown(0))
        {
            damageConeCollider.enabled = true;
            //damageConeCollider.isTrigger = true;
            Invoke("damageConeColliderDeactivate", 0.05f);   //This will invoke the method to reset the trigger status of the shotgun to false after the defined number of seconds
            //animator.SetBool("gunFired", true);
            Sequence s = DOTween.Sequence();
            s.Append(shotgunModel.DOPunchPosition(new Vector3(0, 0, -punchStrength), punchDuration, punchVibrato, punchElasticity));

        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("gunFired", false);
        }
    }

    //Resets the damage cone of the shotgun to false
    void damageConeColliderDeactivate()
    {
        //damageConeCollider.isTrigger = false;
        damageConeCollider.enabled = false;
    }


}
