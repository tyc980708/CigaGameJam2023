using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BaseActor : MonoBehaviour
{
    public float evoLevel = 1f;

    public bool isDashing;
    public float dashFactor;
    public float size;
    public float speed;
    public float acceleration;
    public float steerLerpRatio;
    public bool isCalled;
    public Wave wave;

    [HideInInspector]
    public float curSize; 
    [HideInInspector]
    public float curSpeed;
    [HideInInspector]
    public float curAcceleration;

    [HideInInspector]
    public Animator animator;


    public Rigidbody2D rb;

    [Header("AI")]
    public bool isAvatar;
    public bool isEnemy;
    public bool isFriend;

    public float thinkInterval = 2f;
    [HideInInspector]
    public float thinkTimer;

    public int state; // 0 - Idle; 1 - inDanger; 2 - Follow
    public BaseActor focusedTarget;

    // Start is called before the first frame update
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        wave = transform.Find("Wave").GetComponent<Wave>();
    }

    // Update is called once per frame
    public void Update()
    {
        ParametersControl();

        state = 0;
    }

    public void DoMove(Vector2 position)
    {
        // get direction you want to point at
        Vector2 targetDirection = (position - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = Vector2.Lerp(transform.up, targetDirection, steerLerpRatio * Time.fixedDeltaTime);

        // Move Forward
        Vector2 velocity = transform.up * curSpeed;

        rb.MovePosition((Vector2)transform.position + velocity * Time.fixedDeltaTime);

        // Animator
        if (curSpeed > 0)
        {
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }

        if (isDashing)
        {
            animator.SetBool("isDashing", true);
            animator.SetFloat("PlaySpeed", 1.75f);
        }
        else
        {
            animator.SetBool("isDashing", false);
            animator.SetFloat("PlaySpeed", 1f);
        }
    }

    public void ParametersControl()
    {
        curSize = Mathf.Lerp(curSize, size, 2f * Time.deltaTime);
        curAcceleration = Mathf.Lerp(curAcceleration, acceleration, 5f * Time.deltaTime);
        wave.evoLevel = evoLevel;
    }

    public void DoCall()
    {
        wave.isCalled = true;
    }
}
