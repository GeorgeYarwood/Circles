using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    public Button vibrationBtn;
    public Button musicBtn;
    public Button soundFXBtn;

    const string VibrationKey = "VibrationKey";
    const string MusicKey = "MusicKey";
    const string SoundFXKey = "SoundFXKey";
    // Start is called before the first frame update
    void Awake()
    {
        GetCurrPrefs();
    }

    void GetCurrPrefs()
    {
        //if we've not saved vibration preferences before
        if (!PlayerPrefs.HasKey(VibrationKey))
        {
            //Default to vibration on
            PlayerPrefs.SetInt(VibrationKey, true ? 1 : 0);
        }
        if (!PlayerPrefs.HasKey(MusicKey))
        {
            //Default to Sound on
            PlayerPrefs.SetInt(MusicKey, true ? 1 : 0);
        }
        if (!PlayerPrefs.HasKey(SoundFXKey))
        {
            //Default to Sound on
            PlayerPrefs.SetInt(SoundFXKey, true ? 1 : 0);
        }

    }
    public void ToggleVibration()
    {
        bool currPref = PlayerPrefs.GetInt(VibrationKey) == 1 ? true : false;
        bool newPref = !currPref;
        PlayerPrefs.SetInt(VibrationKey, newPref ? 1 : 0);
    }

    public void ToggleSoundFX()
    {
        bool currPref = PlayerPrefs.GetInt(SoundFXKey) == 1 ? true : false;
        bool newPref = !currPref;
        PlayerPrefs.SetInt(SoundFXKey, newPref ? 1 : 0);
    }

    public void ToggleMusic()
    {
        bool currPref = PlayerPrefs.GetInt(MusicKey) == 1 ? true : false;
        bool newPref = !currPref;
        PlayerPrefs.SetInt(MusicKey, newPref ? 1 : 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt(VibrationKey) == 1 ? true : false)
        {
            vibrationBtn.GetComponentInChildren<Text>().text = "Vibration: On";
        }
        else
        {
            vibrationBtn.GetComponentInChildren<Text>().text = "Vibration: Off";
        }

        if (PlayerPrefs.GetInt(MusicKey) == 1 ? true : false)
        {
            musicBtn.GetComponentInChildren<Text>().text = "Music: On";
        }
        else
        {
            musicBtn.GetComponentInChildren<Text>().text = "Music: Off";
        }

        if (PlayerPrefs.GetInt(SoundFXKey) == 1 ? true : false)
        {
            soundFXBtn.GetComponentInChildren<Text>().text = "SoundFX: On";
        }
        else
        {
            soundFXBtn.GetComponentInChildren<Text>().text = "SoundFX: Off";
        }
    }
}
