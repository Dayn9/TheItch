using UnityEngine;

public class PlayerAudioPlayer : AudioPlayer {

    [SerializeField] private SoundFiles steps;

    private bool jumpSound = false;

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

    public void PlayJumpSound()
    {
        if (!jumpSound)
        {
            jumpSound = true;
            PlaySound(0);
        }
       
    }   

    public void PlayDamageSound()
    {
        PlaySound(0);
    }

    public void ResetJumpSound()
    {
        jumpSound = false;
    }
}
