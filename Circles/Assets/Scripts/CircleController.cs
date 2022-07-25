using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class CircleController : MonoBehaviour
{
    //Circles in current scene
    List<Circle> circles = new List<Circle>();
    GameObject currCircle;
    public Circle circlePrefab;
    public Hole holePrefab;

    private float initialDistance;
    private Vector3 initialScale;

    float radMax = 2.5f;
    float radMin = 1.2f;

    public bool holding;
    public bool overCircle;
    public int holesToSpawn;
    public bool dontUpdateCirclePos;
    int currLvl;
    float xPos = 0, yPos = 0, rad = 0;
    int currScore;
    public Text currScoreTxt;
    public Text currHolesTxt;

    public GameObject circleSpawnPoint;

    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    //Time between each circle spawn
    public float waitTimer;

    bool dragging;
    bool circleSpawnerRunning;

    //Values for each accuracy level:
    public float lowAccurateVal;
    public float highAccurateVal;
    public int baseScore;
    public enum Colour { Red, Yellow, Green };
    public Color customGreen;
    public Color customRed;
    public Color customYellow;

    List<Hole> holesInScene = new List<Hole>();

    PlayServicesManager thisPlayServices;

    // Start is called before the first frame update
    void Start()
    {
        currScore = baseScore;
        thisPlayServices = FindObjectOfType<PlayServicesManager>();
        SpawnNewCircle();
        //Spawn first hole
        SpawnNewHole();
        StartCoroutine(NewHoleTimer());
    }

    public void AddScore(int amount)
    {
        currScore += amount;
    }


    public void ChangeColour(Colour col)
    {
        if (col == Colour.Red)
        {
            currCircle.GetComponentInChildren<Image>().color = customRed;
        }
        else if (col == Colour.Yellow)
        {
            currCircle.GetComponentInChildren<Image>().color = Color.yellow;

        }
        else if (col == Colour.Green)
        {
            currCircle.GetComponentInChildren<Image>().color = Color.green;
        }
    }

    public void ResetColour()
    {
        currCircle.GetComponent<SpriteRenderer>().color = Color.white;

    }

    public void DeleteAllHoles()
    {
        for (int i = 0; i < holesInScene.Count; i++)
        {
            if (holesInScene[i] != null)
            {
                Destroy(holesInScene[i].gameObject);
            }
        }
        holesInScene.Clear();
    }

    public void SpawnNewCircle()
    {
        //RandDistribute(ref xPos, ref yPos, ref rad);

        //Circle thisCircle =
        //Instantiate(circlePrefab, new Vector2(xPos, yPos), Quaternion.identity);

        //thisCircle.radius = rad;

        Circle thisCircle = Instantiate(circlePrefab, circleSpawnPoint.transform.position, Quaternion.identity);
        thisCircle.radius = 2f;
        currCircle = thisCircle.gameObject;
    }

    IEnumerator NewHoleTimer()
    {
        while (true)
        {
            circleSpawnerRunning = true;
            if (holesInScene.Count < holesToSpawn)
            {

                yield return new WaitForSeconds(waitTimer);
                SpawnNewHole();


            }
            else
            {
                //yield return null;
                EndGame();
                break;
            }

        }

    }

    public void DestroyThisHole(Hole thisHole)
    {
        for (int i = 0; i < holesInScene.Count; i++)
        {
            if (holesInScene[i] == thisHole)
            {
                Destroy(holesInScene[i].gameObject);
                holesInScene.RemoveAt(i);
            }
        }


        //StartCoroutine(WaitToUpdateCirclePos());
        holding = false;
        ResetCirclePosAndColour();
        overCircle = false;
        //dontUpdateCirclePos = true;

    }

    void EndGame()
    {

        gameOverScreen.SetActive(true);
        //Upload score to Google play
        if (thisPlayServices.connectedToGooglePlay)
        {
            Social.ReportScore(currScore, GPGSIds.leaderboard_circlesleaderboard, thisPlayServices.LeaderboardUpdate);
        }
    }

    void SpawnNewHole()
    {
        RandDistribute(ref xPos, ref yPos, ref rad,0);

        Hole thisHole =
        Instantiate(holePrefab, new Vector2(xPos, yPos), Quaternion.identity);
        thisHole.radius = rad;
        holesInScene.Add(thisHole);
    }

    Vector2 RandDistribute(ref float xPos, ref float yPos, ref float rad, int recurDepth)
    {
        xPos = Random.Range(-1.6f, 1.6f);
        yPos = Random.Range(-2f, 4.1f);
        rad = Random.Range(radMin, radMax);

        Vector2 potentialPos = new Vector2(xPos, yPos);
        if (Physics.CheckSphere(potentialPos, rad, 12) && recurDepth < 1000)
        {
            recurDepth++;
            RandDistribute(ref xPos, ref yPos, ref rad, recurDepth);
        }

        return potentialPos;
    }

    public void RestartGame()
    {
        //Disable game over screen, reset score, delete holes
        gameOverScreen.SetActive(false);
        currScore = baseScore;
        DeleteAllHoles();
        SpawnNewHole();
        StartCoroutine(NewHoleTimer());
        ResetCirclePosAndColour();


        //Just to be safe
        holding = false;
        overCircle = false;
    }

    public void ResetCirclePosAndColour()
    {

        currCircle.transform.position = circleSpawnPoint.transform.position;

        currCircle.GetComponent<Circle>().timerImg.enabled = false;
        //Hard code just for proof of concept
        currCircle.transform.localScale = new Vector3(2, 2, 0);
        ResetColour();

    }

    // Update is called once per frame
    void Update()
    {
        currScoreTxt.text = currScore.ToString();
        currHolesTxt.text = holesInScene.Count.ToString() + "/" + holesToSpawn.ToString();
        //Only run when game isn't over/Not paused
        if (!gameOverScreen.activeInHierarchy && !pauseScreen.activeInHierarchy)
        {
            if (!circleSpawnerRunning)
            {
                StartCoroutine(NewHoleTimer());
            }


            if (currScore < 0)
            {
                EndGame();
            }



            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                

                // Handle finger movements based on TouchPhase
                switch (touch.phase)
                {
                    //When the player first taps the screen
                    case TouchPhase.Began:
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit hit;
                        //If they hit the circle
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                        {
                            //Set holding to true
                            holding = true;
                        }




                        break;

                    //When a finger is fragged accross the screen
                    case TouchPhase.Moved:

                        //Update circle position to finger position
                        if (holding && !dontUpdateCirclePos)
                        {
                            Vector2 currPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                            //currCircle.GetComponent<Rigidbody>().isKinematic = true;
                            currCircle.transform.position = new Vector2(currPos.x, currPos.y);
                        }



                        break;

                    //When player lifts finger from screen
                    case TouchPhase.Ended:
                        holding = false;
                        //Reset pos if not over a circle waiting to be dropped
                        if (!overCircle)
                        {
                            ResetCirclePosAndColour();
                            if (dontUpdateCirclePos)
                            {
                                dontUpdateCirclePos = false;
                            }

                        }
                        break;
                }



            if (Input.touchCount > 1 && holding)
            {
                    Touch touch1 = Input.GetTouch(1);
                    switch (touch1.phase)
                    {
                        case TouchPhase.Began:
                            initialDistance = Vector2.Distance(touch.position, touch1.position);
                            initialScale = currCircle.transform.localScale;
                            break;

                        case TouchPhase.Moved:
                            
                            var currentDistance = Vector2.Distance(touch.position, touch1.position);
                            
                            var factor = currentDistance / initialDistance;

                            if ((initialScale.x * factor) < radMax || (initialScale.x * factor) > radMin)
                            {
                                currCircle.transform.localScale = initialScale * factor;
                            }
                            break;

                        //When player lifts finger from screen
                        case TouchPhase.Ended:

                            break;
                    }

                }

            }
        }
    }
}
        



