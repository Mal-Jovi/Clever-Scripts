using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class colorChangerOLD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button currentButton;

    bool mouseIn = false;

    // Start is called before the first frame update
    void Start()
    {
        currentButton = currentButton.GetComponent<Button>();
        
    }

    void Update()
    {
        changeColor();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("true");
        mouseIn = true;
        
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("false");
        mouseIn = false;
    }

    void changeColor()
    {
        var tempColor = currentButton.GetComponentInChildren<Text>().font.material.color;

        if (mouseIn)
        {
            currentButton.GetComponentInChildren<Text>().font.material.color = new Color32(253, 89, 90, 255);
        }
        else {
            currentButton.GetComponentInChildren<Text>().font.material.color = new Color32(0, 0, 0, 255); ;
        }
        
    }
} 
