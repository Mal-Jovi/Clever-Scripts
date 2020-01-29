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
        setAlpha(enterGameButton.GetComponentInChildren<Text>(), 0.0f);                                                         //Sets text alpha, here we are setting the enter text alpha to zero instead of in the TextFadeIn function otherwise it would get set to zero until the black screen coroutine finished
        setAlpha(settingsButton.GetComponentInChildren<Text>(), 0.0f);

        StartCoroutine(ImageFadeOut(blackScreen, 2.0f, 1.0f, 0.0f, 0.025f, 0.05f));                                             //Used to fade-in the start screen background by actually fading-out a black screen that covers it

        enterGameButton = enterGameButton.GetComponent<Button>();                                                               //Initializes the enter button
        settingsButton = settingsButton.GetComponent<Button>();                                                                 //Initializes the settings button
        backButton = backButton.GetComponent<Button>();                                                                         //Initializes the back button in the settings menu

        StartCoroutine(TextFadeIn(enterGameButton.GetComponentInChildren<Text>(), 4.0f, 0.0f, 1.0f, -0.025f, 0.05f));           //Used to fade-in the enter button, this is placed here after the button has been initialized
        StartCoroutine(TextFadeIn(settingsButton.GetComponentInChildren<Text>(), 4.5f, 0.0f, 1.0f, -0.025f, 0.05f));            //Used to fade-in the settings button, this is placed here after the button has been initialized

        // enterGameButton.onClick.AddListener(startGame);                                                                         //When the enter button is pressed, the gameStart function is called which then loads the gameMain Scene  
        // settingsButton.onClick.AddListener(openSettings);

        backButton.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ImageFadeOut(Image image, float delay, float startAlpha, float endAlpha, float increment, float duration)
    {
        yield return new WaitForSeconds(delay);         //the given delay time before starting the fade
        image = image.GetComponent<Image>();            //Obtains the image object
        var imageAlpha = image.color;                   //gives a variable the current color property of the image object, not a reference, and image.color.a can't be directly altered in one line so the variable is required here
        imageAlpha.a = startAlpha;                      //Sets the alpha value of the temporary alpha variable

        while (imageAlpha.a != endAlpha)                //Iterates so long as the alpha target value isn't reached 
        {
            imageAlpha.a = imageAlpha.a - increment;            //Increments the alpha temp variable by the designated amount
            yield return new WaitForSeconds(duration);          //Time delay designated, this is used to slowly apply the increments rather than it happening in one nanosecond
            image.color = imageAlpha;                           //set the color property of the image to the new color property variable which has the altered alpha

        }
    }

    private IEnumerator TextFadeIn(Text text, float delay, float startAlpha, float endAlpha, float increment, float duration)
    {
        var textAlpha = text.color;                     //gives a variable the current color property of the image object, not a reference, and image.color.a can't be directly altered in one line so the variable is required here
        textAlpha.a = startAlpha;                       //Sets the alpha value of the temporary alpha variable
        yield return new WaitForSeconds(delay);         //the given delay time before starting the fade

        while (textAlpha.a != endAlpha)                 //Iterates so long as the alpha target value isn't reached 
        {
            textAlpha.a = textAlpha.a - increment;              //Increments the alpha temp variable by the designated amount
            yield return new WaitForSeconds(duration);          //Time delay designated, this is used to slowly apply the increments rather than it happening in one nanosecond
            text.color = textAlpha;                             //Sets the color property of the image to the new color property variable which has the altered alpha
        }
    }

    void setAlpha(Text text, float startAlpha)
    {
        var textAlpha = text.color;         //gives a variable the current color property of the image object, not a reference, and image.color.a can't be directly altered in one line so the variable is required here
        textAlpha.a = startAlpha;           //Sets the alpha value of the temporary alpha variable
        text.color = textAlpha;             //Sets the color property of the image to the new color property variable which has the altered alpha
    }

    void startGame()
    {
        SceneManager.LoadScene("gameMain", LoadSceneMode.Single);
    }

    void openSettings()
    {
        enterGameButton.enabled = false;
        settingsButton.enabled = false;
        backButton.enabled = true;
    }

    
}
