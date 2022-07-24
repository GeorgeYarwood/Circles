using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    public Button vibrationBtn;
    public Button soundBtn;

    const string VibrationKey = "VibrationKey";
    const string SoundKey = "SoundKey";
    // Start is called before the first frame update
    void Start()
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

        
       
    }
    public void ToggleVibration()
    {
        bool currPref = PlayerPrefs.GetInt(VibrationKey) == 1 ? true : false;
        bool newPref = !currPref;
        PlayerPrefs.SetInt(VibrationKey, newPref ? 1 : 0);
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
    }
}
