using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class JellyFish : BaseActor
{
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
    public float hurtCDTimer;
    public float hurtTimer;
    public bool isHurting;
    public float touchedRecoverFactor = 1f;

    public List<Enemy> littenEnemies;
    public List<JellyFish> helpedJellies;

    public Transform colliders;

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

        colliders = transform.Find("Colliders");
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();

        if (evoLevel == 1)
        {
            colliders.localScale = Vector3.one * 1f;
        }
        else if (evoLevel == 2)
        {
            colliders.localScale = Vector3.one * 1.5f;
        }
        else if (evoLevel == 3)
        {
            colliders.localScale = Vector3.one * 3.75f;
        }

        LightSphereControl();
        DashControl();
        LightSpotControl();
        EvoControl();
        HurtControl();
        TaintLittingControl();
    }

    public void LightSphereControl()
    {
        size = 0.5f + (lightNum + 3f * (evoLevel - 1f)) * sizePerLightNum * touchedRecoverFactor;

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
            if (isAvatar && evoLevel == 3)
            {
                AudioManager.PlayBGM("Music_Comfort");
                GameObject.Find("GlobalController").GetComponent<GlobalController>().avatarLevel = evoLevel;
            }
            lightNum = 1;

            helpedJellies.Clear();
            isHelped = false;
        }
    }

    public void HurtControl()
    {
        if (hurtCDTimer > 0f)
        {
            hurtCDTimer -= Time.deltaTime;
        }

        if (isHurting && hurtTimer < -1f) hurtTimer = 0.25f;
        if (!isHurting) hurtTimer = -2f;

        if (hurtTimer > 0f)
        {
            hurtTimer -= Time.deltaTime;
        }
        else if (hurtTimer <= 0f && hurtTimer > -1f)
        {
            GetHurt();
            hurtTimer = -2f;
        }

        if (isHurting) state = 1;
    }

    public void GetHurt()
    {
        if (hurtCDTimer > 0f) return;

        animator.SetTrigger("isTouched");

        if (isAvatar)
            AudioManager.PlaySound("Avatar_Hurt");

        lightNum -= 1;

        // Protection
        if (lightNum < 1)
        {
            lightNum = 1;
            touchedRecoverFactor = 0f;
        }

        hurtCDTimer = 1f;

        lightSpot_1.tinkleCursor = 0f;
        lightSpot_2.tinkleCursor = 0f;
        lightSpot_3.tinkleCursor = 0f;
    }

    public void TaintLittingControl()
    {
        foreach (Enemy taint in littenEnemies)
        {
            float dist = Vector3.Distance(transform.position, taint.transform.position);

            if ((dist + taint.size * 0.5f) < this.size * 1.5f + 0.5f)
            {
                taint.isLit = true;
            }
            else
            {
                taint.isLit = false;
            }
        }
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    public void OnCollisionEnter2D(Collision2D other)
    {

    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        Transform theOther = other.transform.parent;

        if (other.gameObject.tag == "Jelly")
        {
            JellyFish jelly = theOther.GetComponent<JellyFish>();

            animator.SetTrigger("isTouched");

            if ( helpedJellies.Contains(jelly)) return;

            if (jelly.evoLevel >= evoLevel && evoLevel != 3)
            {
                if (isAvatar)
                {
                    AudioManager.PlaySound("Guangdian_Merge_03", lightNum);
                    AudioManager.PlaySound("Guangdian_Stinger", lightNum);
                }
                lightNum += 1;
            }

            helpedJellies.Add(jelly);

            if (isAvatar) jelly.isHelped = true;
        }

        if (other.gameObject.tag == "Taint")
        {
            Enemy taint = other.GetComponent<Enemy>();

            if (!littenEnemies.Contains(taint)) littenEnemies.Add(taint);

            if (!taint.jelliesLittingSelf.Contains(this)) taint.jelliesLittingSelf.Add(this);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Transform theOther = other.transform.parent;

        if (other.gameObject.tag == "Taint")
        {
            Enemy taint = other.GetComponent<Enemy>();

            if (littenEnemies.Contains(taint)) littenEnemies.Remove(taint);

            if (taint.jelliesLittingSelf.Contains(this)) taint.jelliesLittingSelf.Remove(this);

            taint.isLit = false;
        }
    }
}
