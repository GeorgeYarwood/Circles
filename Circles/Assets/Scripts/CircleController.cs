using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleController : MonoBehaviour
{
    //Circles in current scene
    List <Circle> circles = new List<Circle>();
    GameObject currSelection;
    public Circle circlePrefab;
    public Hole holePrefab;

    private float initialDistance;
    private Vector3 initialScale;

    float radMax = 3.4f;
    float radMin = 0.8f;

    public bool holding;
    public bool overCircle;
    public int holesToSpawn;
    public int holesInScene;
    int currLvl;
    float xPos = 0, yPos = 0, rad = 0;
    int currScore;
    public Text currScoreTxt;

    public GameObject circleSpawnPoint;

    //Time between each circle spawn
    public float waitTimer = 10f;

    bool dragging;

    //Values for each accuracy level:
    public float redVal = 1f;
    public float yellowVal = 0.75f;
    public float greenVal = 0.2f;

    public enum Colour { Red, Yellow, Green };
    

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewCircle();

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
            currSelection.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if(col == Colour.Yellow)
        {
            currSelection.GetComponent<SpriteRenderer>().color = Color.yellow;

        }
        else if(col == Colour.Green)
        {
            currSelection.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void ResetColour()
    {
        currSelection.GetComponent<SpriteRenderer>().color = Color.white;

    }

    public void SpawnNewCircle()
    {
        //RandDistribute(ref xPos, ref yPos, ref rad);

        //Circle thisCircle =
        //Instantiate(circlePrefab, new Vector2(xPos, yPos), Quaternion.identity);

        //thisCircle.radius = rad;

        Circle thisCircle = Instantiate(circlePrefab, circleSpawnPoint.transform.position, Quaternion.identity);
        thisCircle.radius = 1.4f;
        
    }

    IEnumerator NewHoleTimer()
    {
        while (true)
        {
            if(holesInScene < holesToSpawn)
            {
                yield return new WaitForSeconds(waitTimer);
                SpawnNewHole();
                holesInScene++;
            }
            else
            {
                yield return null;
            }
         
        }
        
    }

    void SpawnNewHole()
    {
        RandDistribute(ref xPos, ref yPos, ref rad);

        Hole thisHole =
        Instantiate(holePrefab, new Vector2(xPos, yPos), Quaternion.identity);
        thisHole.radius = rad;
    }

    Vector2 RandDistribute(ref float xPos, ref float yPos, ref float rad)
    {
        xPos = Random.Range(-1.6f, 1.6f);
        yPos = Random.Range(-4.1f, 4.1f);
        rad = Random.Range(radMin, radMax);

        Vector2 potentialPos = new Vector2(xPos, yPos);
        if(Physics.CheckSphere(potentialPos, rad, 12))
        {
            RandDistribute(ref xPos, ref yPos, ref rad);
        }

        return potentialPos;
    }
    
    // Update is called once per frame
    void Update()
    {
        currScoreTxt.text = currScore.ToString();


        if (Input.touchCount == 0 && currSelection != null)
        {
            holding = false;
            if (!overCircle)
            {
                currSelection.transform.position = circleSpawnPoint.transform.position;
            }



            //if (currSelection != null)
            //{
            //    currSelection.GetComponent<Rigidbody>().isKinematic = false;
            //}
            //currSelection = null;
        }

        else if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //If we hit a circle
                if (hit.collider.gameObject.GetComponent<Circle>())
                {
                    currSelection = hit.collider.gameObject;
                 
                    if(Input.touchCount >=1)
                    {
                        holding = true;

                        if (Input.touchCount > 1)
                        {
                            var touchZero = Input.GetTouch(0);
                            var touchOne = Input.GetTouch(1);

                            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
                            {
                                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
                                initialScale = currSelection.transform.localScale;
                            }
                            else
                            {
                                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                                var factor = currentDistance / initialDistance;

                                if ((initialScale.x * factor) < radMax || (initialScale.x * factor) > radMin)
                                {
                                    currSelection.transform.localScale = initialScale * factor;
                                }
                            }
                        }
                    }
                }
             }
        }

        if (holding)
        {
            Vector2 currPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            //currSelection.GetComponent<Rigidbody>().isKinematic = true;
            currSelection.transform.position = new Vector2(currPos.x, currPos.y);
        }
    }
}
