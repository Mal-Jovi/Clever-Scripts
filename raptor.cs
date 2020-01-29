using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class raptor : MonoBehaviour
{
    public Transform thisObject;
    public Transform target;

    private NavMeshAgent navComponent;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navComponent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(target.position, transform.position);

        if (target)
        {
            navComponent.SetDestination(target.position);
        }
        else
        {
            if(target = null)
            {
                target = this.gameObject.GetComponent<Transform>();
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Cone")
        {
           Debug.Log(collision.gameObject.name);
           Destroy(gameObject);
        }
        
    }
}
