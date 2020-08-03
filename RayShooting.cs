using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RayShooting : MonoBehaviour
{
    //Variables affecting shooting
    public float damage = 34f;
    public float range = 100f;
    public float impactForce = 50f;
    public float fireRate = 15f;

    public Camera playerCamera;

    //muzzle flash particle system object
    public ParticleSystem muzzleFlash;
    
    //bullet impact effect game object
    public GameObject impactEffect;

    //fire rate variable
    private float nexTimeToFire = 0f;

    //Keep track of ammo and clips remaining
    public float currentClip = 5f;
    public float totalClips = 4f;
    public float maxClipCap = 5f;

    //Text Objects for displaying clips and total ammo
    public TextMeshProUGUI currentMag;
    public TextMeshProUGUI totalAmmo;

    //Time it takes for reloading
    public float reloadTime = .75f;
    private bool isReloading = false;

    //Check if the clip is empty, to prevent shooting
    private bool clipEmpty = false;

    //Animator Controller
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentMag = currentMag.GetComponent<TextMeshProUGUI>();
        totalAmmo = totalAmmo.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading) return;

        if(currentClip != maxClipCap)
        {
            StartCoroutine(Reload());
        }
    
        if (clipEmpty) return;

        if (Input.GetButtonDown("Fire1") && Time.time >= nexTimeToFire)
        {
            nexTimeToFire = Time.time + 1f / fireRate; 
            Shoot();
        }
        //animator.SetBool("gunFired", false);
    }

    void Shoot()
    {
        //animator.SetBool("gunFired", true);
        muzzleFlash.Play();

        RaycastHit hitData;

        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitData, range))
        {
            //Detect if the target hit was a raptor, if so then reduce its health
            raptor currentTarget = hitData.transform.GetComponent<raptor>();
            if(currentTarget != null)
            {
                currentTarget.takeDamage(damage);
            }

            furniture currentFurniture = hitData.transform.GetComponent<furniture>();
            if(currentFurniture != null)
            {
                currentFurniture.takeDamage(damage);
            }
        }
        
        //check if the object hit has a rigidbody, and if so, apply a force
        //TODO: make hit detection only work for objects with the "shootable" tag, and only allow furniture not raptors, have a force applied to them
        if(hitData.rigidbody != null){
            hitData.rigidbody.AddForce(-hitData.normal * impactForce);
        }

        GameObject impactGO = Instantiate(impactEffect, hitData.point, Quaternion.LookRotation(hitData.normal));
        Destroy(impactGO, 0.25f);

        if(currentClip > 0)
        {
            currentClip -= 1;
        }
        
        if(currentClip == 0)
        {
            clipEmpty = true;
        }

    }

    IEnumerator Reload()
    {
  
        if (Input.GetKeyDown(KeyCode.R) && totalClips > 0)
        {
            isReloading = true;
            animator.SetBool("Reloading", true);
        
            yield return new WaitForSeconds(reloadTime);
            currentClip = maxClipCap;
            totalClips -= 1f;

            isReloading = false;
            animator.SetBool("Reloading", false);
            clipEmpty = false;
        }

        currentMag.text = currentClip.ToString("F0");
        totalAmmo.text = totalClips.ToString("F0");
    }
}