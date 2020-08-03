using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class raptor : MonoBehaviour
{
    public Transform thisObject;
    public Transform target;

    private NavMeshAgent navComponent;

    public playerDataGlobal currentplayerDataGlobal;

    public float health = 100f;
    public float damage = 10;
    public float value = 10;
    
    public float attackCoolDownTime;
    public float attackCoolDownTimeResetValue;

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

        playerTracking();
    }

    public void playerTracking()
    {
        navComponent.SetDestination(target.position);

        float distance = Vector3.Distance(target.transform.position, this.gameObject.transform.position);
        //Debug.Log("the current distance between the player and raptor is: " + distance);
        if (distance < 2)
        {
            if(attackCoolDownTime > 0)
            {
                attackCoolDownTime -= Time.deltaTime;
            }
            else
            {
                attackCoolDownTime = attackCoolDownTimeResetValue;
                currentplayerDataGlobal.takeDamage(damage);
            }
        }
    }

    public void takeDamage(float amount)
    {
        health -= amount;
        if (health <= 1.0f)
        {
            currentplayerDataGlobal.earnMoney(value);
            Destroy(gameObject);
        }
    }

}