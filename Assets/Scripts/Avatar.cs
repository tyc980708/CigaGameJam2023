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
        }
        else
        {
            if (curSpeed > 0f) curSpeed -= curAcceleration * Time.fixedDeltaTime;
        }

        DoMove(mouseScreenPosition, curSpeed);
    }
}
