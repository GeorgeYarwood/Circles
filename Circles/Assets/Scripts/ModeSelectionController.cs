using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionController : MonoBehaviour
{
    public Button mediumBtn;
    public Button hardBtn;
    const string TutorialKey = "TutorialKey";

    // Start is called before the first frame update
    void Start()
    {
        //Check if we've played the tutorial before
        if (!PlayerPrefs.HasKey(TutorialKey))
        {
            mediumBtn.interactable = false;
            hardBtn.interactable = false;
            
        }
        else
        {
            mediumBtn.interactable = true;
            hardBtn.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
