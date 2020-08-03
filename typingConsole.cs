using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class typingConsole : MonoBehaviour
{
    public GameObject typingCanvasContainer;

    public GameObject shotgun;

    public GameObject player;
    private playerData playerData;

    public GameObject Computer;
    public GameObject uiCanvas;

    //This is the text document, currently a text document is assigned to a computer terminal by the unity interface
    public static TextAsset document;

    //Turns the assigned text document into an array of strings, where each element is a word
    private string[] documentWords;
    private string fullDocument;

    string input;                       //string that captures text typed at the current frame
    string userString;                  //The user string typed so far

    bool consoleActive;                 //Boolean to capture the state interaction with the console, and acts as a check for when to start capturing user input

    int count;                          //length of userString

    int lastCalculatedScore;            //The score acheived from the last computer session

    public TextMeshProUGUI onScreenText;
    public TextMeshProUGUI userText;


    // Start is called before the first frame update
    void Start()
    {
        uiCanvas.SetActive(false);

        document = (TextAsset)Resources.Load("testDocument", typeof(TextAsset));
        documentWords = (document.text).Split(' ');
        fullDocument = document.text;

        onScreenText = onScreenText.GetComponent<TextMeshProUGUI>();
        userText = userText.GetComponent<TextMeshProUGUI>();

        playerData = player.GetComponent<playerData>();

        for (int i =0; i<documentWords.Length; i++)
        {
            //Debug.Log(documentWords[i]);
        }

        userString = "";
        input = "";
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        removeCharacter();
        if (consoleActive)
        {
            input = Input.inputString;
            

            if (input != "")
            {
                char currentLetterTyped = input[0];         //get the letter typed at the current frame
                if (currentLetterTyped != "\b"[0])
                {
                    typedText(currentLetterTyped);          //update the current user string
                }

                if (currentLetterTyped.Equals('\n') || currentLetterTyped.Equals('\r'))
                {
                    lastCalculatedScore = calculateScore();
                    playerData.earnMoney(lastCalculatedScore);
                    Debug.Log("Score is: " + lastCalculatedScore);
                    dropConsole();
                }

            }
            else
            {
                return;                //No input given, do nothing
            }
        }

        userText.text = userString;
        onScreenText.text = fullDocument;
    }

    public void summonConsole()
    {
        consoleActive = true;
        shotgun.SetActive(false);
        typingCanvasContainer.SetActive(true);
        onScreenText.text = document.text;
        uiCanvas.SetActive(true);
    }

    public void dropConsole()
    {
        consoleActive = false;
        shotgun.SetActive(true);
        typingCanvasContainer.SetActive(false);
        uiCanvas.SetActive(false);
    }

    public void typedText(char c)
    {
        count = userString.Length;

        if (c.Equals(' ') && userString[count - 1].Equals(' '))
        {
            Debug.Log("double space detected");
            return;
        }
        else
        {
            userString = userString + c;
            Debug.Log("user string is currently: " + userString);
        }
    }

    public void removeCharacter()
    {
        count = userString.Length;

        if (Input.GetKeyDown(KeyCode.Backspace) && count>0)
        {
            userString = userString.Remove(count - 1);
            Debug.Log("backspace detected, last character removed, new string is now: " + userString);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        if (userString.Equals("e"))
        {
            userString = userString.Remove(0);
        };
    }

    public int calculateScore()
    {
        userString = Regex.Replace(userString, @"\t|\n|\r", "");

        int totalScore = 0;
        int wordScore = 0;

        int userWordLength = 0;
        int docWordLength = 0;

        string[] userTypedWords = userString.Split(' ');

        Debug.Log("User type words array length is " + userTypedWords.Length);
        for(int i=0; i<userTypedWords.Length; i++)
        {
            userWordLength = userTypedWords[i].Length;
            docWordLength = documentWords[i].Length;
            
            int maxLength = userWordLength;

            if (userWordLength > docWordLength)
            {
                maxLength = docWordLength;
            }

            for (int j=0; j<maxLength; j++)
            {
                if(string.Equals(userTypedWords[i][j], documentWords[i][j]))
                {
                    wordScore++;
                }
                else
                {
                    wordScore--;
                }

                if(userWordLength != docWordLength)
                {
                    wordScore = wordScore - Mathf.Abs(docWordLength - userWordLength);
                }
            }
            totalScore = totalScore + wordScore;
            //Debug.Log("Word Score is " + wordScore + " for user word " + userTypedWords[i] + " and doc word " + documentWords[i] + " with a new total score of " + totalScore);
            wordScore = 0;
        }
        return totalScore;
    }
}