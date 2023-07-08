using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Friend : JellyFish
{

    public Color helpedColor;
    
    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();

        ColorControl();
    }

    public void ColorControl()
    {
        if (isHelped)
        {
            lightSphere.GetComponent<SpriteRenderer>().color = Color.Lerp(lightSphere.GetComponent<SpriteRenderer>().color, helpedColor, Time.deltaTime);
        }
    }
}
