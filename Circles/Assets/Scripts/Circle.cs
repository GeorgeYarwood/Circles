using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Circle : MonoBehaviour
{
    public float radius;
    public Image timerImg;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(radius, radius, 0);
        timerImg = GetComponentInChildren<Image>();
        timerImg.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        radius = gameObject.transform.localScale.x;
        
    }

  
}
