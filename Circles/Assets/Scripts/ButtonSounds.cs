using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    SoundController soundController;

    // Start is called before the first frame update
    void Start()
    {
        soundController = FindObjectOfType<SoundController>();

    }

    public void PlayButtonNoise()
    {
        soundController.PlaySound(SoundController.AudioTypes.ButtonClick);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
