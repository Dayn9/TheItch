using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScrollBox : DialogueBox
{

    [SerializeField] private float scrollSpeed;

    private string recievedMessage;
    private int totallChunkLength = 0;
    private float currentLetterIndex = 0;

    private bool scroll = false;

    // <summary>
    /// display the next chunk of dialogue and an image in the dialogue box
    /// </summary>
    /// <param name="message">full message to display</param>
    public override void OnTriggerKeyPressed(string message)
    {
        SetAllRenderers(true);
        audioPlayer.PlaySound(0);
        //check if there new message
        if (dialogueChunk == -1)
        {
            CreateChunks(message);
        }
        //make sure there are chunks of dialogue to display
        if (chunks.Count != 0)
        {
            if (currentLetterIndex < totallChunkLength)
            {
                currentLetterIndex = totallChunkLength;
                return;
            }

            //advance dialogue to next or first chunk
            dialogueChunk++;
            //exit dialogue if there are no more chunks
            if (dialogueChunk >= chunks.Count)
            {
                ExitReset();
                return; //don't display the dialogue becuase there is none
            }
            DisplayChunk(chunks[dialogueChunk]);
        }
    }


    /// <summary>
    /// exit out of the dialogue and reset it back to the beginning 
    /// </summary>
    public override void Reset()
    {
        totallChunkLength = 0;
        currentLetterIndex = 0;
        scroll = false;
        base.Reset();
    }

    protected override void DisplayChunk(string message)
    {
        //make sure message is valid
        if (message.Length <= (numLines * charsPerLine))
        {
            recievedMessage = message.ToLower();
            totallChunkLength = message.Length;
            currentLetterIndex = 0;
            scroll = true;
        }
        //Something has gone wrong in Display Message, string passed in was too long 
        else
        {
            Debug.Log("message has more than" + (numLines * charsPerLine) + "charachters");
        }
    }

    private void Update()
    {
        if (scroll)
        {
            currentLetterIndex += scrollSpeed * Time.deltaTime;
            if(currentLetterIndex > totallChunkLength)
            {
                currentLetterIndex = totallChunkLength;
                scroll = false;
            }

            for (int i = 0; i < totallChunkLength; i++)
            {
                char letter = recievedMessage[i];
                int spriteNum = 12; //default to blank space in case of unassign char

                if (i <= currentLetterIndex)
                {
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
                }

                text[i].sprite = letters[spriteNum];
            }
        }
    }
}
