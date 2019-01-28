﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextSize { Small, Large, SmallFull }

[RequireComponent(typeof(UIAnchor))]
[RequireComponent(typeof(AudioPlayer))]
public class DialogueBox : Pause {
    
    [SerializeField] private GameObject charPrefab; //gameobject prefab for individual letters
    [SerializeField] protected TextSize size; //size of the text to display 
    [SerializeField] protected Sprite[] letters; //array of all possible letter sprites

    private SpriteRenderer charachterImage; //ref to spriteRenderer of the image location 
    protected List<SpriteRenderer> text; //refs to the spriteRenderers of the letter locations

    protected int numLines; //number of lines in the dialoge box
    protected int charsPerLine; //number of charachters in each line
    private Vector2 initialOffset;
    private Vector2 offset;
    protected int dialogueChunk; //which chunk of dialogue is currently being displayed
    protected List<string> chunks; //chunks of dialogue that are displayed one at a time

    [SerializeField] private Color letterColor;
   
    public bool FirstChunk { get { return dialogueChunk == -1; } }
    
    public TextSize Size
    {
        get { return size; }
        set
        {
            //set the values based on text size
            switch (value)
            {
                case TextSize.Small: //used for charachter dialogue
                    numLines = 4;
                    charsPerLine = 24;
                    initialOffset = new Vector2(- 6.875f, 2.125f );
                    offset = new Vector2(0.75f, -1.125f);
                    break;
                case TextSize.SmallFull: //used for narration dialogue
                    numLines = 3;
                    charsPerLine = 29;
                    initialOffset = new Vector2(-10.875f, 2.125f);
                    offset = new Vector2(0.75f, -1.125f);
                    break;

                //TODO: fix values when doing disability settings
                //Also make the sprites pure white for re-coloring in Inspector
                //Also make narration full version
                case TextSize.Large:
                    numLines = 3;
                    charsPerLine = 16;
                    initialOffset = new Vector2(-6.375f, 1.375f); 
                    offset = new Vector2(1.125f, -1.375f);
                    break;
            }
        }
    }

    private void Awake()
    {
        Size = size; //set values based on size value

        dialogueChunk = -1; //default no dialogue 
        chunks = new List<string>(); 

        text = new List<SpriteRenderer>();
        GameObject newLetter;
        SpriteRenderer textRender;
        //Create all the placeHolders for charachter positions
        for(int line = 0; line< numLines; line++)
        {
            for(int cha = 0; cha<charsPerLine; cha++)
            {
                newLetter = Instantiate(charPrefab, transform);
                newLetter.name = "char" + line + "." + cha; 
                newLetter.transform.position = transform.position + 
                    new Vector3((cha * offset.x) + initialOffset.x, (line * offset.y) + initialOffset.y, 0); //TODO: make variables for adjustable 'font size'
                textRender = newLetter.GetComponent<SpriteRenderer>();

                textRender.color = letterColor;
                textRender.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
                
                text.Add(newLetter.GetComponent<SpriteRenderer>());
            }
        }
        //Create the placeholder for the face image
        GameObject charachterFace = Instantiate(charPrefab, transform);
        charachterFace.name = "faceImage";
        charachterFace.transform.position = transform.position + new Vector3(-9.0f, 0.0f, 0.0f);
        charachterImage = charachterFace.GetComponent<SpriteRenderer>();
        charachterImage.sortingLayerID = GetComponent<SpriteRenderer>().sortingLayerID;
        charachterImage.sprite = letters[12];

        SetAllRenderers(false);

        audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Update()
    {

    }

    /// <summary>
    /// display the next chunk of dialogue and an image in the dialogue box
    /// </summary>
    /// <param name="message">full message to display</param>
    /// <param name="face">image to display</param>
    /// <returns>dialogue exited</returns>
    public bool OnTriggerKeyPressed(string message, Sprite face)
    {
        charachterImage.sprite = face;
        return OnTriggerKeyPressed(message);
    }

    /// <summary>
    /// display the next chunk of dialogue and an image in the dialogue box
    /// </summary>
    /// <param name="message">full message to display</param>
    /// <returns>dialogue exited</returns>
    public virtual bool OnTriggerKeyPressed(string message)
    {
        SetAllRenderers(true);
        audioPlayer.PlaySound(0);
        //check if there new message
        if (dialogueChunk == -1)
        {
            CreateChunks(message);
        }
        //make sure there are chunks of dialogue to display
        if(chunks.Count != 0)
        {
            //advance dialogue to next or first chunk
            dialogueChunk++;
            //exit dialogue if there are no more chunks
            if (dialogueChunk >= chunks.Count)
            {
                ExitReset();
                return true; //don't display the dialogue becuase there is none
            }
            DisplayChunk(chunks[dialogueChunk]);
        }
        return false; //dialogue was displayed
    }

    public void ExitReset()
    {
        //textbox event will handle resetting if it exits
        if (GetComponent<TextboxEvent>() != null)
        {
            GetComponent<TextboxEvent>().MoveOut();
            return;
        }
        this.Reset();
    }

    /// <summary>
    /// exit out of the dialogue and reset it back to the beginning 
    /// </summary>
    public virtual void Reset()
    {
        dialogueChunk = -1; //reset dialogue
        SetAllRenderers(false);
        charachterImage.enabled = false;
    }

    /// <summary>
    /// divide a full message into chunks containing lines that will be displayed in sequence
    /// </summary>
    /// <param name="message">full message to divide</param>
    public void CreateChunks(string message)
    {
        //make sure list of chunks has only one element
        chunks.Clear(); 
        chunks.Add("");
        gameObject.SetActive(true);
        //parse throught each letter in the message
        string line = "";
        string word = "";
        for (int i = 0; i < message.Length; i++)
        {
            word += message[i];
            if (message[i] == 32 || i == message.Length - 1) //ASCII: (' ' = 32)
            {
                word = message[i] == 32 ? word.Substring(0, word.Length - 1) : word;//remove space from end of word
                //check if word extends past end of line
                if (line.Length + word.Length > charsPerLine)
                {
                    line += new string(' ', charsPerLine - line.Length); //fill in the rest of the line with spaces
                    chunks[chunks.Count - 1] += line; //add line to chunk
                    if(chunks[chunks.Count - 1].Length >= (numLines * charsPerLine)) { chunks.Add(""); } //create a new chunk when current one is full
                    line = ""; //start a new line
                }
                line += word + (line.Length + word.Length == charsPerLine ? "" : " "); //add word to line and space if not end of line
                //always add in the last line
                if (i == message.Length - 1)
                {
                    chunks[chunks.Count - 1] += line;
                }
                word = ""; //start a new word
            }
        }
        chunks[chunks.Count - 1] += new string(' ', (charsPerLine * numLines) - chunks[chunks.Count - 1].Length); //fill in the rest of the final chunk
    }

    /// <summary>
    /// sets the images for each of the individual charachters in the dialogue box
    /// </summary>
    /// <param name="message">charachters to display</param>
    protected virtual void DisplayChunk(string message)
    {
        //make sure message is valid
        if (message.Length <= (numLines * charsPerLine))
        {
            message = message.ToLower(); //make all letters lowercase
            for (int i = 0; i < message.Length; i++)
            {
                char letter = message[i];
                int spriteNum = 12; //default to blank space in case of unassign char
                //is letter an number 
                if (letter >= 48 && letter <= 57) //ASCII: ('0' = 48) ('9' = 57)
                {
                    spriteNum = letter - 48; //Sprites: ('0' = 0) ('9' = 9)
                }
                //is letter part of the alpabet
                else if (letter >= 97 && letter <= 122) //ASCII: ('a' = 97) ('z' = 122)
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
        //Something has gone wrong in Display Message, string passed in was too long 
        else
        {
            Debug.Log("message has more than" + (numLines * charsPerLine) + "charachters");
        }
    }


    protected void SetAllRenderers(bool enable)
    {
        GetComponent<SpriteRenderer>().enabled = enable;
        charachterImage.enabled = enable;
        foreach (Renderer rend in text)
        {
            rend.enabled = enable;
        }
    }
}