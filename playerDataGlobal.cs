using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "newGlobalPlayerData")]
public class playerDataGlobal : ScriptableObject
{
    public string entityName;

    public float health;
    public float damage;
    public float money;
    public float propertyDamage;

    private bool beingHit = false;

    public void isDead()
    {
        if (health < 1)
        {
            SceneManager.LoadScene("gameEnd");
        }
    }

    public void takeDamage(float amount)
    {
        health -= amount;
    }

    public void earnMoney(float amount)
    {
        money += amount;
    }

    public void causePropertyDamage(float amount)
    {
        propertyDamage += amount;
    }
}
