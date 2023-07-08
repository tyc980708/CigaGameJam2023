using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class JellyFish : BaseActor
{
    public float evoLevel = 1f;
    public int lightNum;
    public float sizePerLightNum;
    public float durationPerLightNum;
    public float restDashDuration;
    public float dashDurationPercentage;
    public float dashCD;
    private float curDashCD;
    private bool canRecoverDash;
    public Transform lightSphere;

    public LightSpot lightSpot_1;
    public LightSpot lightSpot_2;
    public LightSpot lightSpot_3;

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
        LightSpotControl();
    }

    public void LightSphereControl()
    {
        size = 1 + (lightNum-1) * sizePerLightNum;

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

        speed = speed / dashFactor;
        acceleration = acceleration / dashFactor;

        curDashCD = dashCD;
    }

    public void LightSpotControl()
    {
        // Sync EvoLevel
        lightSpot_1.evoLevel = this.evoLevel;
        lightSpot_2.evoLevel = this.evoLevel;
        lightSpot_3.evoLevel = this.evoLevel;


        //  Dash Particle & LightSpot Size

        float maxDashDuration = lightNum * durationPerLightNum;
        dashDurationPercentage = restDashDuration / maxDashDuration;
        if (lightNum == 1)
        {
            lightSpot_1.gameObject.SetActive(true);
            lightSpot_2.gameObject.SetActive(false);
            lightSpot_3.gameObject.SetActive(false);

            lightSpot_1.restPercentage = dashDurationPercentage;

            if (isDashing) 
            {
                lightSpot_1.isDashing = true;
                lightSpot_2.isDashing = false;
                lightSpot_3.isDashing = false;
            }
        }
        else if (lightNum == 2)
        {
            lightSpot_1.gameObject.SetActive(true);
            lightSpot_2.gameObject.SetActive(true);
            lightSpot_3.gameObject.SetActive(false);

            if (dashDurationPercentage >= 0.5)
            {
                lightSpot_1.restPercentage = (dashDurationPercentage - 0.5f) / 0.5f;
                lightSpot_2.restPercentage = 1f;

                if (isDashing) 
                {
                    lightSpot_1.isDashing = true;
                    lightSpot_2.isDashing = true;
                    lightSpot_3.isDashing = false;
                }
            }
            else
            {
                lightSpot_1.restPercentage = 0f;
                lightSpot_2.restPercentage = dashDurationPercentage / 0.5f;

                if (isDashing) 
                {
                    lightSpot_1.isDashing = true;
                    lightSpot_2.isDashing = false;
                    lightSpot_3.isDashing = false;
                }
            }
        }
        else if (lightNum == 3)
        {
            lightSpot_1.gameObject.SetActive(true);
            lightSpot_2.gameObject.SetActive(true);
            lightSpot_3.gameObject.SetActive(true);
        }

        if (!isDashing)
        {
            lightSpot_1.isDashing = false;
            lightSpot_2.isDashing = false;
            lightSpot_3.isDashing = false;
        }
        
    }
}
