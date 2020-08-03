using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerDataDisplay : MonoBehaviour
{
    public playerDataGlobal currentPlayerDataGlobal;

    public Text healthText;
    public Text moneyText;

    void Start()
    {
        healthText = healthText.GetComponent<Text>();
        moneyText = moneyText.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = currentPlayerDataGlobal.health.ToString("F0");
        moneyText.text = currentPlayerDataGlobal.money.ToString("F0");
    }
}
