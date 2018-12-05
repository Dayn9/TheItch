using UnityEngine;

public class PlayerAudioPlayer : AudioPlayer {

    /// <summary>
    /// Used by Player animator to play AudioClips at right time in animation
    /// </summary>

    [SerializeField] private SoundFiles steps; //group of step SFX to randomly select

    private bool jumpSoundPlayed = false;
    private bool landSoundPlayed = false;
    private bool damageSoundPlayed = false;

    /// <summary>
    /// Play a random step sound
    /// </summary>
	public void PlayStepSound()
    {
        //make sure the sound exists and able to play sound
        if (steps.Clips.Length > 0 && (!source.isPlaying || steps.Priority >= currentPriority))
        {
            currentPriority = steps.Priority;
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

    /// <summary>
    ///  Play the damage sound if it hasn't already been played
    /// </summary>
    public void PlayDamageSound()
    {
        if (!damageSoundPlayed)
        {
            damageSoundPlayed = true;
            PlaySound(2);
        }
    }

    /// <summary>
    /// Allow specific sounds to be played again by the Animator
    /// </summary>
    public void ResetJumpSound() { jumpSoundPlayed = false; }
    public void ResetLandSound() { landSoundPlayed = false; }
    public void ResetDamageSound() { damageSoundPlayed = false; }

    #endregion
}
