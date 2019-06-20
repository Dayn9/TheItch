using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reseter : Inventory {

    /// <summary>
    /// in charge of reseting the game when the player dies 
    /// </summary>
    
    public void ResetGame()
    {
        Player.GetComponent<IHealthObject>().FullHeal(); //fully heal the player
        Heartbeat.BPM = 81; //set the BPM to default value to be set in first scene
        Player.GetComponent<AbilityHandler>().LockAll(); //re lock all the abilities
        Player.GetComponent<AbilityHandler>().Unlock(0);
        Items.Clear();
        startPosition = Vector2.one;

        BackgroundAudioPlayer.menu = true;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
