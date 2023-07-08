using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class JellyFish : BaseActor
{
    public int lightNum;
    public float durationPerLightNum;
    public float restDashDuration;
    public float dashCD;
    private float curDashCD;
    private bool canRecoverDash;
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
        
        restDashDuration = lightNum * durationPerLightNum;
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
        lightSphere.localScale = curSize * Vector3.one;
        // lightSphere.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius = curSize;
    }

    public void DashControl()
    {
        if (restDashDuration > 0f && isDashing)
        {
            restDashDuration -= Time.deltaTime;
        }

        if (restDashDuration < lightNum * durationPerLightNum && !isDashing && canRecoverDash)
        {
            restDashDuration += Time.deltaTime;
        }

        if (curDashCD > 0)
        {
            curDashCD -= Time.deltaTime;
            canRecoverDash = false;
        }

        if (restDashDuration < 0f) 
        {
            exitDashEvent.Invoke();
        }

        if (curDashCD <= 0)
        {
            if (canRecoverDash) return;

            canRecoverDash = true;
        }
    }

    public void Dash()
    {
        if (isDashing) return;

        isDashing = true;

        speed = speed * dashFactor;
        acceleration = acceleration * dashFactor;
    }

    public void ExitDash()
    {
        if (!isDashing) return;

        isDashing = false;

        restDashDuration = 0f;

        speed = speed / dashFactor;
        acceleration = acceleration / dashFactor;

        curDashCD = dashCD;
    }
}
