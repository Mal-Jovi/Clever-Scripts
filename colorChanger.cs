using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class colorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button currentButton;
    public ColorBlock buttonColor;
    public Text buttonText;

    bool mouseIn = false;

    [SerializeField] Color cleverRed;

    // Start is called before the first frame update
    void Start()
    {
        currentButton = GetComponent<Button>();
        buttonColor = GetComponent<Button>().colors;
        buttonText = GetComponentInChildren<Text>();
    }

    void Update()
    {
       
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {

        Debug.Log("true");
        mouseIn = true;

        buttonColor.normalColor = Color.red;
        GetComponent<Button>().colors = buttonColor;

        buttonText.color = cleverRed;

    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {

        Debug.Log("false");
        mouseIn = false;

        buttonColor.normalColor = Color.white;
        GetComponent<Button>().colors = buttonColor;

        buttonText.color = Color.white;

    }
}

    
