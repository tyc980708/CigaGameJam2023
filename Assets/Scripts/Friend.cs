using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Friend : JellyFish
{

    public Color helpedColor;

    public Vector3 targetDirection;
    
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

        AI();
    }

    public void ColorControl()
    {
        if (isHelped)
        {
            lightSphere.GetComponent<SpriteRenderer>().color = Color.Lerp(lightSphere.GetComponent<SpriteRenderer>().color, helpedColor, Time.deltaTime);
        }
    }

    public void AI()
    {
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
                    if (curSpeed < speed) curSpeed += curAcceleration * Time.fixedDeltaTime;
                    if (curSpeed > speed) curSpeed = Mathf.Lerp(curSpeed, speed, 1f * Time.fixedDeltaTime);
                }
                else
                {
                    targetDirection = Vector3.zero;
                    if (curSpeed > 0f) curSpeed -= curAcceleration * Time.fixedDeltaTime;
                }
            }
            else if (state == 1)
            {
                targetDirection = (transform.position - focusedTarget.transform.position).normalized;
                if (!isDashing) dashEvent.Invoke();
                else exitDashEvent.Invoke();
            }
        }

        DoMove((Vector2)(transform.position + targetDirection));
    }
}
