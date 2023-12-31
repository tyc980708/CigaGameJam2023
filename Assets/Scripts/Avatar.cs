using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Avatar : JellyFish
{
    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();

        DashControl();
        CallControl();
    }

    public void FixedUpdate()
    {
        DoMoveControl();
    }

    public void DoMoveControl()
    {
        // convert mouse position into world coordinates
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get Keyboard Input for Move Order
        if (Input.GetKey(KeyCode.W)) 
        {
            if (curSpeed < speed) curSpeed += curAcceleration * Time.fixedDeltaTime;
            if (curSpeed > speed) curSpeed = Mathf.Lerp(curSpeed, speed, 1f * Time.fixedDeltaTime);
        }
        else
        {
            if (curSpeed > 0f) curSpeed -= curAcceleration * Time.fixedDeltaTime;
        }

        DoMove(mouseScreenPosition);
    }

    public void DashControl()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            dashEvent.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.Space)) 
        {
            exitDashEvent.Invoke();
        }
    }

    public void CallControl()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            DoCall();
        }
    }
}
