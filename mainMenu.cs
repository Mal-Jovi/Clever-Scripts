using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class mainMenu : MonoBehaviour
{
    public Image blackScreen;

    public Button enterGameButton;
    public Button settingsButton;
    public Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        Button enterBTN = enterGameButton.GetComponent<Button>();
        enterBTN.onClick.AddListener(enterOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        quitGame();
    }

    

    void enterOnClick()
    {
        Debug.Log("clicked");
        SceneManager.LoadScene("gameMain", LoadSceneMode.Single);
    }

    void openSettings()
    {
        
    }

    void quitGame()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }


    
}
