using UnityEngine;

public class PlayerAudioPlayer : AudioPlayer {

    [SerializeField] private SoundFiles steps;

    private bool jumpSoundPlayed = false;
    private bool landSoundPlayed = false;

    /// <summary>
    /// Play a random step sound
    /// </summary>
	public void PlayStepSound()
    {
        //make sure the sound exists and able to play sound
        if (steps.Clips.Length > 0 && (!source.isPlaying || steps.Overrides))
        {
            source.clip = steps.Clips[Random.Range(0, steps.Clips.Length -1)];
            source.Play();
        }
    }

    #region animator calls

    /// <summary>
    /// Play the jump sound if it hasn't already been played
    /// </summary>
    public void PlayJumpSound()
    {
        if (!jumpSoundPlayed)
        {
            jumpSoundPlayed = true;
            PlaySound(0);
        }
       
    }

    /// <summary>
    /// Play the land sound if it hasn't already been played
    /// </summary>
    public void PlayLandSound()
    {
        if (!landSoundPlayed)
        {
            landSoundPlayed = true;
            PlaySound(1);
        }

    }

    public void PlayDamageSound()
    {
        PlaySound(0);
    }

    public void ResetJumpSound()
    {
        jumpSoundPlayed = false;
    }

    public void ResetLandSound()
    {
        landSoundPlayed = false;
    }

    #endregion
}
