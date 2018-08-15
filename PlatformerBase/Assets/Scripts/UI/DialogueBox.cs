using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIAnchor))]
public class DialogueBox : MonoBehaviour {

    [SerializeField] private GameObject charPrefab; //gameobject prefab for individual letters
    [SerializeField] private Sprite[] letters; //array of all possible sprites

    private SpriteRenderer charachterImage; 
    private List<SpriteRenderer> text;

    private const int numLines = 3; //number of lines in the dialoge box
    private const int charsPerLine = 16; //number of charachters in each line
    private int dialogueChunk; //which chunk of dialogue is currently being displayed

    private void Start()
    {
        gameObject.SetActive(false);
        text = new List<SpriteRenderer>();
        dialogueChunk = -1; //no dialogue 

        GameObject newLetter;
        for(int line = 0; line< numLines; line++)
        {
            for(int cha = 0; cha<charsPerLine; cha++)
            {
                newLetter = Instantiate(charPrefab, transform);
                newLetter.name = "char" + line + "." + cha; 
                newLetter.transform.position = transform.position + 
                    new Vector3((cha * 1.125f) - 6.375f, (line * -1.375f) + 1.375f, 0); //TODO: make variables for adjustable 'font size'
                text.Add(newLetter.GetComponent<SpriteRenderer>());
            }
        }

        GameObject charachterFace = Instantiate(charPrefab, transform);
        charachterFace.name = "faceImage";
        charachterFace.transform.position = transform.position + new Vector3(-9.0f, 0.0f, 0.0f);
        charachterImage = charachterFace.GetComponent<SpriteRenderer>();
    }

    public void OnTriggerKeyPressed(string message)
    {
        gameObject.SetActive(true);
        //check if there new message
        if(dialogueChunk == -1)
        {
            //break up dialogue into chunks
            dialogueChunk = 0;
        }
        //message is already being displayed
        else
        {
            //advance dialogue to next chunk if there is one

            //exit dialogue if there are no more chunks
        }
    }

    public void Reset()
    {
        dialogueChunk = -1; //reset dialogue
        gameObject.SetActive(false);
    }

    public void DisplayMessage(string message, Sprite face)
    {
        DisplayMessage(message);
        charachterImage.sprite = face;
    }

    public void DisplayMessage(string message)
    {
        gameObject.SetActive(true);
        //TODO: method that breaks message up into sections of (numLines * charsPerLine)
        
        //parse throught each letter in the message
        string modifiedMessage = "";
        string line = "";
        string word = "";
        for (int i = 0; i < message.Length; i++)
        {
            word += message[i];
            if (message[i] == 32 || i == message.Length - 1) //ASCII: (' ' = 32)
            {
                word = message[i] == 32 ? word.Substring(0, word.Length - 1) : word;//remove space from end of word
                //check if word extends past end of line
                if(line.Length + word.Length > charsPerLine)
                {
                    line += new string(' ', charsPerLine - line.Length); //fill in the rest of the line with spaces
                    modifiedMessage += line; //add line to message
                    line = ""; //start a new line
                }
                line += word + (line.Length + word.Length == charsPerLine ? "" : " "); //add word to line and space if not end of line
                //always add in the last line
                if (i == message.Length - 1)
                {
                    modifiedMessage += line;
                }
                word = ""; //start a new word
            }    
        }
        Display(modifiedMessage);
    }

    /// <summary>
    /// sets the images for eaach of the individual charachters in the dialogue box
    /// </summary>
    /// <param name="message">charachters to display</param>
    private void Display(string message)
    {
        //make sure message is valid
        if(message.Length <= (numLines * charsPerLine))
        {
            message = message.ToLower(); //make all letters lowercase
            for(int i = 0; i < message.Length; i++)
            { 
                char letter = message[i]; 
                int spriteNum = 12; //default to blank space in case of unassign char
                //is letter an number 
                if (letter >= 48 && letter <= 57) //ASCII: ('0' = 48) ('9' = 57)
                {
                    spriteNum = letter - 48; //Sprites: ('0' = 0) ('9' = 9)
                }
                //is letter part of the alpabet
                else if(letter >= 97 && letter <= 122) //ASCII: ('a' = 97) ('z' = 122)
                {
                    spriteNum = letter - 84; //Sprites: ('a' = 13) ('z' = 38)
                }
                //other charachters
                else
                {
                    switch ((int)letter)
                    {
                        case 46: //ASCII: ('.' = 46)
                            spriteNum = 10; //Sprites: ('.' = 10)
                            break;
                        case 44: //ASCII: (',' = 44)
                            spriteNum = 11; //Sprites: (',' = 11)
                            break;
                    }
                }
                text[i].sprite = letters[spriteNum];
            }
        }
        else
        {
            Debug.Log("message has more than" + (numLines * charsPerLine) + "charachters");
        }
    }

}
