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

    AudioSource bgSource;
    AudioSource fxSource;
    public enum Sounds { HoverCircle, CircleDropGreen, CircleDropYellow, ButtonClick, AddScore};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlaySound(Sounds sound)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
