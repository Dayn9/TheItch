using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : Global {

    /// <summary>
    /// contains the sound files that can be played by an object and the ability to play those sounds
    /// </summary>

    [SerializeField] private SoundFile[] sounds; //array of all sounds available to this Audio Player
    private Dictionary<string, SoundFile> soundDict; //dictionary to get sound files by name 

    private AudioSource source; //ref to the attached audioSource

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        soundDict = new Dictionary<string, SoundFile>();
        foreach(SoundFile soundFile in sounds)
        {
            soundDict.Add(soundFile.Name, soundFile);
        }

        source.mute = muted; //make sure the source isn't playing while muted 
    }

    /// <summary>
    /// Plays a sound by index
    /// </summary>
    /// <param name="index">array index in sounds </param>
    public void PlaySound(int index)
    {
        //make sure source can play sound and 
        if((!source.isPlaying || sounds[index].Overrides) && (index >= 0 && index < sounds.Length))
        {
            source.clip = sounds[index].Clip;
            source.Play();
        }
    }

    /// <summary>
    /// Plays a sound by name
    /// </summary>
    /// <param name="name">name of the sound</param>
    public void PlaySound(string name)
    {
        //make sure the sound exists and able to play sound
        if (soundDict.ContainsKey(name) && (!source.isPlaying || soundDict[name].Overrides))
        {
            source.clip = soundDict[name].Clip;
            source.Play();
        }
    }

    /// <summary>
    /// Plays a sound from a random selection
    /// </summary>
    /// <param name="min">minimun index</param>
    /// <param name="max">maximum index</param>
    public void PlayRandomSound(int min, int max)
    {
        //make sure the input values are in range
        if(min >= 0 && max < sounds.Length)
        {
            PlaySound(Random.Range(min, max));
        }
    }
}

[System.Serializable]
public struct SoundFile
{
    /// <summary>
    /// Contains data about sound files
    /// </summary>
    [SerializeField] private AudioClip clip; //clip to play
    [SerializeField] private bool overrides; //true if the clip overrides current sound being played

    private string name; //name of the clip

    public string Name {
        get {
            //find the name of the clip 
            if(name == null || name.Trim() == "")
            {
                name = clip.name; //set the clip name
            }
            return name;
        }
    }
    public AudioClip Clip { get { return clip; } }
    public bool Overrides { get { return overrides; } }
}