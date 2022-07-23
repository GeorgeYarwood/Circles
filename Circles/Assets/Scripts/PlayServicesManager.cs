using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class PlayServicesManager : MonoBehaviour
{
    public bool connectedToGooglePlay;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        LogInToGooglePlay();
    }


    private void LogInToGooglePlay()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    public void ShowLeaderBoard()
    {
        if (!connectedToGooglePlay)
        {
            LogInToGooglePlay();
            
        }
        Social.ShowLeaderboardUI();
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        if(status == SignInStatus.Success)
        {
            connectedToGooglePlay = true;
        }
        else
        {
            connectedToGooglePlay = false;
        }
        
    }

    public void LeaderboardUpdate(bool success)
    {
        if (success) Debug.Log("Updated Leaderboard");
        else
        {
            Debug.Log("Leaderboard update failed!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
