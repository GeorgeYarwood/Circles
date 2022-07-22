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

    float autoDropTimer = 50f;
    float remainingTime;

    void Start()
    {
        gameObject.transform.localScale = new Vector3(radius, radius, 0);
        circleController = FindObjectOfType<CircleController>();
    }

    IEnumerator AutoDropTimer(Circle thisCircle)
    {
        dropNow = false;
        thisCircle.timerImg.enabled = true;
        while (remainingTime > 0)
        {
            //set the fill amount to the remaining time
            thisCircle.timerImg.fillAmount = Mathf.InverseLerp(0, autoDropTimer, remainingTime);
            remainingTime--;
            yield return new WaitForSeconds(1f);
        }
        
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
        if (thisCircle = collision.gameObject.GetComponent<Circle>())
        {
            remainingTime = autoDropTimer;
            circleController.ResetColour();
            circleController.overCircle = false;
            thisCircle.timerImg.enabled = false;
            StopAllCoroutines();
        }

       
    }


    void OnTriggerEnter(Collider collision)
    {
        remainingTime = autoDropTimer;
    }
    
  
    void OnTriggerStay(Collider collision)
    {
        
        Circle thisCircle;
        //If a circle hits this hole
        if (thisCircle = collision.gameObject.GetComponent<Circle>())
        {
            
            float PosDiffX, PosDiffY, RadDiff = 0;
            PosDiffX = thisCircle.transform.position.x - transform.position.x;
            PosDiffY = thisCircle.transform.position.y - transform.position.y;
            RadDiff = thisCircle.radius - radius;
            //If it's directly over it (Factoring in leniency)
            if (Mathf.Abs(PosDiffX) <= 0.25 && Mathf.Abs(PosDiffY) <= 0.25)
            {
                circleController.overCircle = true;
                //Start timer for auto-drop
                StartCoroutine(AutoDropTimer(thisCircle));
                //float absRadDiff = Mathf.Abs(RadDiff);
                float negRadDiff = -RadDiff;
                //If the radius is the same (Factoring in leniency)
                if(thisCircle.radius <= radius)
                {
                    if (negRadDiff <= circleController.highAccurateVal)
                    {
                        circleController.ChangeColour(CircleController.Colour.Green);
                        scoreToAdd = 5;
                    }

                    else if (negRadDiff >= circleController.highAccurateVal && negRadDiff < circleController.lowAccurateVal)
                    {
                        circleController.ChangeColour(CircleController.Colour.Yellow);
                        scoreToAdd = 3;
                    }
                    else if (negRadDiff >= circleController.lowAccurateVal)
                    {
                        circleController.ChangeColour(CircleController.Colour.Red);
                        scoreToAdd = -5;
                    }
                }
                else
                {
                    //If the circle is too big, always be red
                    circleController.ChangeColour(CircleController.Colour.Red);
                    scoreToAdd = -3;
                }
                if (!circleController.holding || dropNow)
                {
                    //Tell controller to spawn a new circle and hole
                    Handheld.Vibrate();
                    if (dropNow)
                    {
                        scoreToAdd -= 2;
                    }
                    circleController.AddScore(scoreToAdd);
                    circleController.DestroyThisHole(this);
                }
            }
            else
            {
                circleController.overCircle = false;
            }
         }
    }

   
       
}


