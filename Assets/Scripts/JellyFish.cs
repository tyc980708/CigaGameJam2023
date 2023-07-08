using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class JellyFish : BaseActor
{
    public int lightNum;
    public float dashFactor;
    public Transform lightSphere;

    // Start is called before the first frame update
    public void Start()
    {
        base.Start();

        lightSphere = this.transform.Find("LightSphere");
        
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();

        LightSphereControl();
    }

    public void LightSphereControl()
    {
        // lightSphere.localScale = curSize * Vector3.one;
        lightSphere.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius = curSize;
    }
}
