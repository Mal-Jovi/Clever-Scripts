using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class typingComputer : MonoBehaviour
{
    //How close the player has to be in order interact with a typing console
    public float radius = 2.5f;

    public GameObject interactText;

    public GameObject player;
    public Camera playerCamera;
    
    typingConsole typingConsole;

    NavMeshAgent currentRaptor;

    bool computerActivated = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // Start is called before the first frame update
    private void Start()
    {
        typingConsole = this.GetComponent<typingConsole>();

        //GameEvents.current.onTextPopUpTrigger += onTextPopUp;
    }

    // Update is called once per frame
    void Update()
    {
        proximityPopUp();
        detectInteraction();
        exitConsole();
    }

    private void onTextPopUp()
    {

    }

    public void proximityPopUp()
    {
        if((this.gameObject.transform.position - player.transform.position).sqrMagnitude < (radius * radius)){
            interactText.SetActive(true);
        }
        else
        {
            interactText.SetActive(false);
        }
    }

    public void detectInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 3f))
            {
                typingComputer computer = hit.transform.GetComponent<typingComputer>();
                if (computer != null)
                {
                    //This IF/ELSE can stops enemy motion
                    if (!computerActivated)
                    {
                        //Debug.Log("hit computer");
                        computerActivated = true;
                        freezeAllMotion();
                        typingConsole.summonConsole();
                        player.GetComponent<gameController>().pauseDisabled = true;
                        //Debug.Log(player.GetComponent<gameController>().pauseDisabled);
                    }
                }
            }
        }
    }

    public void exitConsole()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("dropping and unfreezing ");
            computerActivated = false;
            unfreezeAllMotion();
            typingConsole.dropConsole();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            player.GetComponent<gameController>().pauseDisabled = false;
        }
    }

    public void freezeAllMotion()
    {
        //find all raptors in the scene based on the "Raptor" tag
        GameObject[] activeRaptors = GameObject.FindGameObjectsWithTag("Raptor");

        //set the speed of the AI nav component to 0
        foreach (GameObject obj in activeRaptors)
        {
            Debug.Log("freezing");
            currentRaptor = obj.GetComponent<NavMeshAgent>();
            currentRaptor.speed = 0;
        }

        //freeze player motion
        player.GetComponent<PlayerMovement>().speed = 0f;

        playerCamera.GetComponent<MouseControl>().horizontalSpeed = 0f;
        playerCamera.GetComponent<MouseControl>().verticalSpeed = 0f;

        //player.GetComponent
    }

    //Undoes freezeAllMotion by setting all the altered values back to their defaults
    //TODO more intelligently keep track of what the original value was instead of relying on constants, the mouse speeds should be set back to the user
    //defined values not constants you dong
    public void unfreezeAllMotion()
    {
        GameObject[] activeRaptors = GameObject.FindGameObjectsWithTag("Raptor");

        foreach (GameObject obj in activeRaptors)
        {
            Debug.Log("unfreezing");
            currentRaptor = obj.GetComponent<NavMeshAgent>();
            currentRaptor.speed = 5;
        }

        player.GetComponent<PlayerMovement>().speed = 6f;

        playerCamera.GetComponent<MouseControl>().horizontalSpeed = 60f;
        playerCamera.GetComponent<MouseControl>().verticalSpeed = 60f;
    }
}
