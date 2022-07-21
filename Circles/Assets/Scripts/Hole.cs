using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius;
    int scoreToAdd =0;
    CircleController circleController;
    bool dropNow;

    void Start()
    {
        gameObject.transform.localScale = new Vector3(radius, radius, 0);
        circleController = FindObjectOfType<CircleController>();
        dropNow = false;
    }

    IEnumerator AutoDropTimer()
    {
        yield return new WaitForSeconds(1.5f);
        dropNow = true;

    }

    // Update is called once per frame
    void Update()
    {
        radius = gameObject.transform.localScale.x;



    }


    void OnTriggerExit(Collider collision)
    {
        Circle thisCircle;
        //If a circle hits this hole
        if (thisCircle = collision.gameObject.GetComponent<Circle>())
        {
            circleController.ResetColour();
        }
    }

    void OnTriggerStay(Collider collision)
    {
        circleController.overCircle = true;
        Circle thisCircle;
        //If a circle hits this hole
        if (thisCircle = collision.gameObject.GetComponent<Circle>())
        {
            float PosDiffX, PosDiffY, RadDiff = 0;
            PosDiffX = thisCircle.transform.position.x - transform.position.x;
            PosDiffY = thisCircle.transform.position.y - transform.position.y;
            RadDiff = thisCircle.radius - radius;
            //If it's directly over it (Factoring in leniency)
            if (Mathf.Abs(PosDiffX) <= 0.4 && Mathf.Abs(PosDiffY) <= 0.4)
            {
                //Start timer for auto-drop
                StartCoroutine(AutoDropTimer());
                float absRadDiff = Mathf.Abs(RadDiff);
                //If the radius is the same (Factoring in leniency)

                if (absRadDiff <= circleController.greenVal)
                {
                    circleController.ChangeColour(CircleController.Colour.Green);
                    scoreToAdd = 10;
                }

                else if (absRadDiff >= circleController.greenVal && absRadDiff < circleController.redVal)
                {
                    circleController.ChangeColour(CircleController.Colour.Yellow);
                    scoreToAdd = 4;
                }
                else if (absRadDiff >= circleController.yellowVal)
                {
                    circleController.ChangeColour(CircleController.Colour.Red);
                    scoreToAdd = -5;
                }

                if (!circleController.holding || dropNow)
                {
                    //Tell controller to spawn a new circle and hole
                    Handheld.Vibrate();

                    //Hide/Destroy it
                    thisCircle.gameObject.SetActive(false);

                    Destroy(thisCircle);
                    circleController.holesInScene--;

                    circleController.AddScore(scoreToAdd);
                    gameObject.SetActive(false);
                    circleController.SpawnNewCircle();
                    circleController.overCircle = false;
                    Destroy(this);
                }
            }
            else
            {
                circleController.ResetColour();
                circleController.overCircle = false;
            }
        }

    }
       
}


