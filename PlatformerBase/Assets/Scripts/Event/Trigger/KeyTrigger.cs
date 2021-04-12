using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : IndicatorTrigger
{
    protected override void Update()
    {
        if (!paused)
        {
            //check if in contact with the player and player is interacting 
            if (playerTouching && CheckInput())
            {
                if (!questCompleted)
                {
                    CheckQuest();
                    if (questCompleted)
                    {
                        CallAfter();
                        audioPlayer.PlaySound(1);
                    }
                    else
                    {
                        CallBefore();
                        audioPlayer.PlaySound(0);
                    }
                }
                else
                {
                    CallBefore();
                    audioPlayer.PlaySound(0);
                }
            }
        }
    }
}
