using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void showLeaderboard()
    {
        FindObjectOfType<PlayServicesManager>().ShowLeaderBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
