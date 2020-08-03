using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDataMonitor : MonoBehaviour
{
    public playerDataGlobal currentPlayerDataGlobal;

    private void Start()
    {
        currentPlayerDataGlobal.health = 100;
    }

    void Update()
    {
        currentPlayerDataGlobal.isDead();
    }
}
