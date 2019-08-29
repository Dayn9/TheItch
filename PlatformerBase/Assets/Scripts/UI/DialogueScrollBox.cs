using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public override bool OnTriggerKeyPressed(string message)
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
                return false;
            }

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
        return false;
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
            totallChunkLength = message.TrimEnd(' ').Length; //length of actual message
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
        if (scroll && !paused)
        {
            currentLetterIndex += scrollSpeed * Time.deltaTime;
            if(currentLetterIndex > totallChunkLength)
            {
                currentLetterIndex = totallChunkLength;
                scroll = false;
            }

            for (int i = 0; i < (numLines * charsPerLine); i++)
            {
                //convert letters to sprite and set sprite when inside current index 
                text[i].sprite = letters[i <= currentLetterIndex ? getSpriteNum(recievedMessage[i]) : letters.Length - 1];
            }
            //reset the quote marks
            openSingleQuote = false;
            openDoubleQuote = false;
        }
    }
}
