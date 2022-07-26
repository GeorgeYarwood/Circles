using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadGameEasy()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadGameMedium()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadGameHard()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
