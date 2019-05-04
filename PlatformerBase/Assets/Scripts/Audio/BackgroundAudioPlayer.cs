using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(DontDestroy))] //background audio player shouldn't be destroyed
public class BackgroundAudioPlayer : AudioPlayer
{
    private int soundP = 0;

    public static bool menu = true;

    private AudioSource[] sources;
    private float[] targetVolumes;
    private float[] currentVolumes;
    [SerializeField] private float fadeRate;

    [SerializeField] private AudioMixer backgroundMixer;

    

    private void Awake()
    {
        sources = GetComponents<AudioSource>();

        /*soundDict = new Dictionary<string, SoundFile>();
        foreach (SoundFile soundFile in sounds)
        {
            soundDict.Add(soundFile.Name, soundFile);
        }*/



        if (soundP == 0)
        {
            soundP = 2;
            sources[0].clip = sounds[0].Clip;
            sources[1].clip = sounds[1].Clip;
            backgroundMixer.SetFloat("TrackAVol", -40);
            backgroundMixer.SetFloat("TrackBVol", -80);

            targetVolumes = new float[] { 0, -80 };
            currentVolumes = new float[] { -40, -80 };
        }

        foreach (AudioSource source in sources)
        {
            source.loop = true;
            source.mute = muted; //make sure the source isn't playing while muted 
            source.Play();
        }
    }



    private void Update()
    {
        backgroundMixer.GetFloat("TrackAVol", out currentVolumes[0]);
        backgroundMixer.GetFloat("TrackBVol", out currentVolumes[1]);

        backgroundMixer.SetFloat("TrackAVol", Mathf.Lerp(currentVolumes[0], targetVolumes[0], fadeRate));
        backgroundMixer.SetFloat("TrackBVol", Mathf.Lerp(currentVolumes[1], targetVolumes[1], fadeRate));

        if (!menu)
        {
            if (Heartbeat.BPM <= 32)
            {
                if (soundP != 2)
                {
                    soundP = 2;
                    sources[0].clip = sounds[2].Clip;
                    sources[0].Play();
                    targetVolumes = new float[] { 0, -80 };
                }
            }
            else if (Heartbeat.BPM >= 168)
            {
                if (soundP != 3)
                {
                    soundP = 3;
                    sources[0].clip = sounds[3].Clip;
                    sources[0].Play();
                    targetVolumes = new float[] { 0, -80 };
                }
            }
            else
            {
                if (soundP != 1)
                {
                    soundP = 1;
                    sources[1].Play();
                    targetVolumes = new float[] { -80, 0 };
                }
            }
        }
        else
        {
            if(soundP != 0)
            {
                soundP = 0;
                targetVolumes = new float[] { 0, -80 };
                sources[0].clip = sounds[0].Clip;
                sources[0].Play();
            }
        }
    } 
}
