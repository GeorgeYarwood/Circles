using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleController : MonoBehaviour
{
    //Circles in current scene
    List <Circle> circles = new List<Circle>();
    GameObject currCircle;
    public Circle circlePrefab;
    public Hole holePrefab;

    private float initialDistance;
    private Vector3 initialScale;

    float radMax = 0.45f;
    float radMin = 0.3f;

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

    //Time between each circle spawn
    public float waitTimer;

    bool dragging;

    //Values for each accuracy level:
    public float lowAccurateVal;
    public float highAccurateVal;

    public enum Colour { Red, Yellow, Green };

    public Color customGreen = new Color32(163/255, 240/255, 171/255, 94/255);
    public Color customRed = new Color32(224/255,132/255,122/255,88/255);
    public Color customYellow = new Color32(247/255,243/255,171/255,97/255);
    
    List<Hole> holesInScene = new List<Hole>();


    // Start is called before the first frame update
    void Start()
    {
        SpawnNewCircle();
        //Spawn first hole
        SpawnNewHole();
        StartCoroutine(NewHoleTimer());
    }

    public void AddScore(int amount)
    {
        currScore+= amount;
    }

   
    public void ChangeColour(Colour col)
    {
        if(col== Colour.Red)
        {
            currCircle.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if(col == Colour.Yellow)
        {
            currCircle.GetComponent<SpriteRenderer>().color = Color.yellow;

        }
        else if(col == Colour.Green)
        {
            currCircle.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void ResetColour()
    {
        currCircle.GetComponent<SpriteRenderer>().color = Color.white;

    }

    public void DeleteAllHoles()
    {
        for(int i = 0; i < holesInScene.Count; i++)
        {
            if(holesInScene[i] != null)
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
        thisCircle.radius = 0.45f;
        currCircle = thisCircle.gameObject;
    }

    IEnumerator NewHoleTimer()
    {
        while (true)
        {
            if(holesInScene.Count < holesToSpawn)
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
        for(int i = 0; i< holesInScene.Count; i++)
        {
            if(holesInScene [i] == thisHole)
            {
                Destroy(holesInScene[i].gameObject);
                holesInScene.RemoveAt(i);
            }
        }
        overCircle = false;
        holding = false;
        //StartCoroutine(WaitToUpdateCirclePos());
        ResetCirclePosAndColour();
        dontUpdateCirclePos = true;

    }

    void EndGame()
    {
        gameOverScreen.SetActive(true);
    }

    void SpawnNewHole()
    {
        RandDistribute(ref xPos, ref yPos, ref rad);

        Hole thisHole =
        Instantiate(holePrefab, new Vector2(xPos, yPos), Quaternion.identity);
        thisHole.radius = rad;
        holesInScene.Add(thisHole);
    }

    Vector2 RandDistribute(ref float xPos, ref float yPos, ref float rad)
    {
        xPos = Random.Range(-1.6f, 1.6f);
        yPos = Random.Range(-2f, 4.1f);
        rad = Random.Range(radMin, radMax);

        Vector2 potentialPos = new Vector2(xPos, yPos);
        if(Physics.CheckSphere(potentialPos, rad, 12))
        {
            RandDistribute(ref xPos, ref yPos, ref rad);
        }

        return potentialPos;
    }
    
    public void RestartGame()
    {
        //Disable game over screen, reset score, delete holes
        gameOverScreen.SetActive(false);
        currScore = 0;
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
            ResetColour();
        
    }

    IEnumerator JustReleasedFingerCooldown()
    {
        dontUpdateCirclePos = true;
        yield return new WaitForEndOfFrame();
        dontUpdateCirclePos = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currScoreTxt.text = currScore.ToString();
        currHolesTxt.text = holesInScene.Count.ToString() + "/" + holesToSpawn.ToString();
        //Only run when game isn't over
        if (!gameOverScreen.activeInHierarchy) 
        {

           
            if (currScore < 0)
            {
                EndGame();
            }

            if (Input.touchCount == 0)
            {
                holding = false;
                if (dontUpdateCirclePos)
                {
                    dontUpdateCirclePos = false;
                }
                if (!overCircle)
                {
                    ResetCirclePosAndColour();
                }
               
                


            }

            else if (Input.touchCount > 0 && !dontUpdateCirclePos)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (Input.touchCount >= 1)
                    {
                        holding = true;

                        if (Input.touchCount > 1)
                        {
                            var touchZero = Input.GetTouch(0);
                            var touchOne = Input.GetTouch(1);

                            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
                            {
                                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                                initialScale = currCircle.transform.localScale;
                            }
                            else
                            {
                                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                                var factor = currentDistance / initialDistance;

                                if ((initialScale.x * factor) < radMax || (initialScale.x * factor) > radMin)
                                {
                                    currCircle.transform.localScale = initialScale * factor;
                                }
                            }
                        }
                    }
                }
            }

            if (holding && !dontUpdateCirclePos)
            {
                Vector2 currPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                //currCircle.GetComponent<Rigidbody>().isKinematic = true;
                currCircle.transform.position = new Vector2(currPos.x, currPos.y);
            }
        }

        
    }
}
