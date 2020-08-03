using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class furniture : MonoBehaviour
{
    public float Value = 50;
    public float initialValue = 50f;

    public GameObject Player;
    playerData currentPlayerData;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerData = Player.GetComponent<playerData>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void takeDamage(float amount)
    {
        Value -= amount;
        //Debug.Log("furniture health is " + Value);
        if (Value < 0)
        {
            currentPlayerData.earnMoney(initialValue);
            //Debug.Log("furniture destroyed");
            Die();
            
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
