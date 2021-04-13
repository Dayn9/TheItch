using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour {

    /// <summary>
    /// contains the sound files that can be played by an object and the ability to play those sounds
    /// </summary>

    [SerializeField] protected SoundFile[] sounds; //array of all sounds available to this Audio Player
    private Dictionary<string, SoundFile> soundDict; //dictionary to get sound files by name 

    protected AudioSource source; //ref to the attached audioSource
    public bool Loop { set { source.loop = value; } }

    protected int currentPriority = 0;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        soundDict = new Dictionary<string, SoundFile>();
        foreach(SoundFile soundFile in sounds)
        {
            soundDict.Add(soundFile.Name, soundFile);
        }

        source.mute = Global.muted; //make sure the source isn't playing while muted 
    }

    /// <summary>
    /// Plays a sound by index
    /// </summary>
    /// <param name="index">array index in sounds </param>
    public void PlaySound(int index)
    {
        //make sure source can play sound and 
        if((!source.isPlaying || (sounds[index].Priority >= currentPriority && !sounds[index].Loops)) 
            && (index >= 0 && index < sounds.Length))
        {
            currentPriority = sounds[index].Priority;
            source.clip = sounds[index].Clip;
            source.Play();
        }
    }
    

    public void PlaySoundOverride(int index)
    {
        currentPriority = sounds[index].Priority;
        source.clip = sounds[index].Clip;
        source.Play();

        Debug.Log("FOR TESTING SOUNDS ONLY");
    }

    /// <summary>
    /// Plays a sound by name
    /// </summary>
    /// <param name="name">name of the sound</param>
    public void PlaySound(string name)
    {
        //make sure the sound exists and able to play sound
        if ((!source.isPlaying || (soundDict[name].Priority >= currentPriority && !soundDict[name].Loops)) 
            && soundDict.ContainsKey(name))
        {
            currentPriority = soundDict[name].Priority;
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

    protected virtual void Update()
    {
        source.mute = Global.muted;
    }
}

[System.Serializable]
public struct SoundFile
{
    /// <summary>
    /// Contains data about sound files
    /// </summary>
    [SerializeField] private AudioClip clip; //clip to play
    [SerializeField] private int priority; //priority given to this sound effect
    [SerializeField] private bool loops;
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
    public int Priority { get { return priority; } }
    public bool Loops { get { return loops; } }
}

[System.Serializable]
public struct SoundFiles
{
    /// <summary>
    /// Contains data about sound files
    /// </summary>
    [SerializeField] private AudioClip[] clips; //clip to play
    [SerializeField] private int priority; //priority give to these sound effects

    private string name; //name of the clip

    public string Name
    {
        get
        {
            //find the name of the clip 
            if (name == null || name.Trim() == "")
            {
                name = clips[0].name; //set the clip name
            }
            return name;
        }
    }
    public AudioClip[] Clips { get { return clips; } }
    public int Priority { get { return priority; } }
}



