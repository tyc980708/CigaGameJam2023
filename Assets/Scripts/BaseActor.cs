using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BaseActor : MonoBehaviour
{
    public bool isDashing;
    public float dashFactor;
    public float size;
    public float speed;
    public float acceleration;
    public float steerLerpRatio;
    public bool isCalled;

    [HideInInspector]
    public float curSize; 
    [HideInInspector]
    public float curSpeed;
    [HideInInspector]
    public float curAcceleration;

    [HideInInspector]
    public Animator animator;


    public Rigidbody2D rb;

    // Start is called before the first frame update
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        ParametersControl();
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
    }

    public void ParametersControl()
    {
        curSize = Mathf.Lerp(curSize, size, 2f * Time.deltaTime);
        curAcceleration = Mathf.Lerp(curAcceleration, acceleration, 5f * Time.deltaTime);
    }
}
