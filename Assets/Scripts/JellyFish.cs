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
    private float maxDashDuration;
    public float restDashDuration;
    public float dashDurationPercentage;
    public float dashCD;
    private float curDashCD;
    private bool canRecoverDash;
    public Transform lightSphere;

    public LightSpot lightSpot_1;
    public LightSpot lightSpot_2;
    public LightSpot lightSpot_3;

    public bool isHelped = false;
    public bool isInDanger = false;
    public float touchedTimer;
    public float touchedRecoverFactor = 1f;

    [HideInInspector]
    public UnityEvent dashEvent;
    [HideInInspector]
    public UnityEvent exitDashEvent;

    // Start is called before the first frame update
    public void Start()
    {
        base.Start();

        lightSphere = this.transform.Find("LightSphere");
        
        restDashDuration = (lightNum + 3f * (evoLevel - 1f)) * durationPerLightNum;
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
        EvoControl();
        HurtControl();
    }

    public void LightSphereControl()
    {
        size = 0.5f + lightNum * sizePerLightNum * evoLevel * touchedRecoverFactor;

        if (touchedRecoverFactor < 1f)
        {
            touchedRecoverFactor += 1f * Time.deltaTime;
        }

        if (touchedRecoverFactor > 1f) touchedRecoverFactor = 1f;


        lightSphere.localScale = curSize * Vector3.one;
        // lightSphere.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius = curSize;
    }

    public void DashControl()
    {
        maxDashDuration = (lightNum + 3f * (evoLevel - 1f)) * durationPerLightNum;

        if (restDashDuration > 0f && isDashing)
        {
            restDashDuration -= Time.deltaTime;
        }

        if (restDashDuration > maxDashDuration)
        {
            restDashDuration -= Time.deltaTime;
        }

        if (restDashDuration < maxDashDuration && !isDashing && canRecoverDash)
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
        // Sync evoLevel
        lightSpot_1.evoLevel = this.evoLevel;
        lightSpot_2.evoLevel = this.evoLevel;
        lightSpot_3.evoLevel = this.evoLevel;

        // Sync touchedRecoverFactor
        lightSpot_1.touchedRecoverFactor = touchedRecoverFactor;
        lightSpot_2.touchedRecoverFactor = touchedRecoverFactor;
        lightSpot_3.touchedRecoverFactor = touchedRecoverFactor;


        //  Dash Particle & LightSpot Size
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

            if (dashDurationPercentage >= 0.666)
            {
                lightSpot_1.restPercentage = (dashDurationPercentage - 0.666f) / 0.333f;
                lightSpot_2.restPercentage = 1f;
                lightSpot_3.restPercentage = 1f;

                if (isDashing) 
                {
                    lightSpot_1.isDashing = true;
                    lightSpot_2.isDashing = true;
                    lightSpot_3.isDashing = true;
                }
            }
            else if (dashDurationPercentage >= 0.333 && dashDurationPercentage < 0.666)
            {
                lightSpot_1.restPercentage = 0f;
                lightSpot_2.restPercentage = (dashDurationPercentage - 0.333f) / 0.333f;
                lightSpot_3.restPercentage = 1f;

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
                lightSpot_2.restPercentage = 0f;
                lightSpot_3.restPercentage = dashDurationPercentage / 0.333f;

                if (isDashing) 
                {
                    lightSpot_1.isDashing = true;
                    lightSpot_2.isDashing = false;
                    lightSpot_3.isDashing = false;
                }
            }
        }

        if (!isDashing)
        {
            lightSpot_1.isDashing = false;
            lightSpot_2.isDashing = false;
            lightSpot_3.isDashing = false;
        }
        
    }

    public void EvoControl()
    {
        animator.SetInteger("evoLevel", (int)evoLevel);

        if (evoLevel >= 3f) return;

        if (lightNum > 3)
        {
            evoLevel += 1f;
            lightNum = 1;
        }
    }

    public void HurtControl()
    {
        if (touchedTimer > 0f)
        {
            touchedTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    public void OnCollisionEnter2D(Collision2D other)
    {
        // print(1);
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        // print(2);
        if (other.gameObject.tag == "Jelly")
        {
            JellyFish jelly = other.transform.GetComponent<JellyFish>();

            animator.SetTrigger("isTouched");

            if (jelly.isHelped) return;

            lightNum += 1;

            jelly.isHelped = true;
        }

        if (other.gameObject.tag == "Taint")
        {
            if (touchedTimer > 0f) return;

            animator.SetTrigger("isTouched");

            lightNum -= 1;

            // Protection
            if (lightNum < 1)
            {
                lightNum = 1;
                touchedRecoverFactor = 0f;
            }

            touchedTimer = 1f;

            lightSpot_1.tinkleCursor = 0f;
            lightSpot_2.tinkleCursor = 0f;
            lightSpot_3.tinkleCursor = 0f;
        }
    }
}
