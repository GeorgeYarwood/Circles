using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip HoverCircle;
    public AudioClip CircleDropGreen;
    public AudioClip CircleDropYellow;
    public AudioClip CircleDropRed;
    public AudioClip ButtonClick;
    public AudioClip AddScore;
    public AudioClip bgMusic;

    public Dictionary<AudioTypes, AudioClip> AudioClips = new Dictionary<AudioTypes, AudioClip>();

    AudioSource bgSource;
    AudioSource fxSource;
    public enum AudioTypes { HoverCircle, CircleDropGreen, CircleDropYellow, CircleDropRed, ButtonClick, AddScore};
    const string MusicKey = "MusicKey";
    const string SoundFXKey = "SoundFXKey";

    void Start()
    {
        DontDestroyOnLoad(this);

        AudioClips[AudioTypes.HoverCircle] = HoverCircle;    
        AudioClips[AudioTypes.CircleDropGreen] = CircleDropGreen;    
        AudioClips[AudioTypes.CircleDropYellow] = CircleDropYellow;    
        AudioClips[AudioTypes.CircleDropRed] = CircleDropRed;    
        AudioClips[AudioTypes.ButtonClick] = ButtonClick;    
        AudioClips[AudioTypes.AddScore] = AddScore;    


      

        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        if(sources.Length == 0)
        {
            bgSource = gameObject.AddComponent<AudioSource>();
            fxSource = gameObject.AddComponent<AudioSource>();
            bgSource.clip = bgMusic;
            bgSource.Play();
            bgSource.loop = true;
        }
        else
        {
            bgSource = sources[1];
            fxSource = sources[0];
        }

       
    }


    public void PlaySound(AudioTypes sound)
    {
        fxSource.clip = AudioClips[sound];
        fxSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        bool music = PlayerPrefs.GetInt(MusicKey) == 1 ? true : false;
        bool soundFX = PlayerPrefs.GetInt(SoundFXKey) == 1 ? true : false;
        if (!music)
        {
           
            bgSource.mute = true;
        }
        else
        {
            
            bgSource.mute = false;
        }
        if (!soundFX)
        {
            fxSource.mute = true;
        }
        else
        {
            fxSource.mute = false;
        }
    }
}
