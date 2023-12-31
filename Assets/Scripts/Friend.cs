using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Friend : JellyFish
{

    public Color helpedColor;
    public Color originalColor;

    public Vector3 targetDirection;

    public bool isMove;

    public BaseActor avatar;
    public bool isCompanion;
    private BaseActor evo3;
    
    // Start is called before the first frame update
    public void Start()
    {
        base.Start();

        if (evoLevel < 3) 
        {
            lightNum = Random.Range(1, 3);
        }

        originalColor = lightSphere.GetComponent<SpriteRenderer>().color;

        avatar = GameObject.Find("Avatar").GetComponent<BaseActor>();

        if (focusedTarget && focusedTarget.gameObject.tag == "Jelly" && focusedTarget.evoLevel == 3f)
        {
            isCompanion = true;
            evo3 = focusedTarget;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();

        ColorControl();

        EvoControl2();

        AI();
    }

    public void ColorControl()
    {
        if (isHelped)
        {
            lightSphere.GetComponent<SpriteRenderer>().color = Color.Lerp(lightSphere.GetComponent<SpriteRenderer>().color, helpedColor, Time.deltaTime);
        }
        else
        {
            lightSphere.GetComponent<SpriteRenderer>().color = Color.Lerp(lightSphere.GetComponent<SpriteRenderer>().color, originalColor, Time.deltaTime);
        }
    }

    public void EvoControl2()
    {
        if (isEvoluted)
        {
            if (evoLevel >= avatar.evoLevel) isHelped = false;
            isEvoluted = false;
        }

        if (evoLevel < avatar.evoLevel) isHelped = true;
    }

    public void AI()
    {
        if (isCompanion)
        {
            if (state != 1) 
            {
                state = 2;
                focusedTarget = evo3;
            }
        }

        if (thinkTimer < thinkInterval)
        {
            thinkTimer += Time.deltaTime;
        }
        else
        {
            thinkTimer = 0f;

            if (state == 0)
            {
                float dice = Random.Range(0f, 6f);
                if (dice >= 3f)
                {
                    targetDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
                    isMove = true;
                }
                else
                {
                    // targetDirection = Vector3.up;
                    isMove = false;
                }
            }
            else if (state == 1)
            {
                // targetDirection = (transform.position - focusedTarget.transform.position).normalized;
                // targetDirection = -transform.up.normalized;
                float dice = Random.Range(0f, 6f);
                if (dice >= 3f)
                {
                    targetDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
                    isMove = true;
                }
                else
                {
                    // targetDirection = Vector3.up;
                    isMove = false;
                }

                if (!isDashing) dashEvent.Invoke();
                else exitDashEvent.Invoke();
            }
            else if (state == 2)
            {
                if (focusedTarget == null) state = 0;

                targetDirection = (focusedTarget.transform.position - transform.position).normalized;

                isMove = true;

                if (!isDashing) 
                {
                    float dice = Random.Range(0f, 6f);
                    if (dice > 3) dashEvent.Invoke();
                }
                else exitDashEvent.Invoke();
            }

            if (avatar && Vector3.Distance(transform.position, avatar.transform.position) < 3f && state != 1)
            {
                focusedTarget = avatar.transform.GetComponent<BaseActor>();
            }

            if (focusedTarget && focusedTarget.gameObject.tag == "Jelly")
            {
                float dice = Random.Range(0f, 6f);
                if (dice > 3f)
                {
                    state = 2;
                }
                else
                {
                    state = 0;
                }

                dice = Random.Range(0f, 6f);
                if (dice > 4f)
                {
                    focusedTarget = null;
                    state = 0;
                }
            }

            
        }

        if (isMove) 
        {
            if (curSpeed < speed) curSpeed += curAcceleration * Time.fixedDeltaTime;
            if (curSpeed > speed) curSpeed = Mathf.Lerp(curSpeed, speed, 1f * Time.fixedDeltaTime);
        }
        else
        {
            if (curSpeed > 0f) curSpeed -= curAcceleration * 0.1f * Time.fixedDeltaTime;
        }

        DoMove((Vector2)(transform.position + targetDirection));
    }
}
