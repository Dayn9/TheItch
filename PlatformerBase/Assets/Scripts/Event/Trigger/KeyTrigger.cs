using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTrigger : IndicatorTrigger
{
    protected override void Update()
    {
        if (!Global.paused)
        {
            //check if in contact with the player and player is interacting 
            if (playerTouching && CheckInput())
            {
                if (!questCompleted)
                {
                    CheckQuest();
                    if (questCompleted)
                    {
                        After?.Invoke();
                        audioPlayer.PlaySound(1);
                    }
                    else
                    {
                        Before?.Invoke();
                        audioPlayer.PlaySound(0);
                    }
                }
                else
                {
                    Before?.Invoke();
                    audioPlayer.PlaySound(0);
                }
            }
        }
    }
}
