using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class JellyFish : BaseActor
{
    public int lightNum;
    public float durationPerLightNum;
    private float restDashDuration;
    public Transform lightSphere;

    [HideInInspector]
    public UnityEvent dashEvent;
    [HideInInspector]
    public UnityEvent exitDashEvent;

    // Start is called before the first frame update
    public void Start()
    {
        base.Start();

        lightSphere = this.transform.Find("LightSphere");
        
        dashEvent.AddListener(Dash);
        exitDashEvent.AddListener(ExitDash);
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();

        LightSphereControl();
        DashControl();
    }

    public void LightSphereControl()
    {
        lightSphere.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius = curSize;
    }

    public void DashControl()
    {
        if (restDashDuration > 0f)
        {
            restDashDuration -= Time.deltaTime;
        }

        // if (restDashDuration <= 0f) isDashing = false;
        
    }

    public void Dash()
    {
        float restDashDuration = lightNum * durationPerLightNum;
        print(restDashDuration);

        speed = speed * dashFactor;
        acceleration = acceleration * dashFactor;
    }

    public void ExitDash()
    {
        float restDashDuration = 0f;

        speed = speed / dashFactor;
        acceleration = acceleration / dashFactor;
    }
}
