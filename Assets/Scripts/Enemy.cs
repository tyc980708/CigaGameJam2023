using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Enemy : BaseActor
{
    public int level;


    // Start is called before the first frame update
    public void Start()
    {
        base.Start();

        level = (int)Random.Range(1, 3);

        size = Random.Range(1, 4) * level;
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();

        SizeControl();
    }

    public void SizeControl()
    {
        transform.localScale = Vector3.one * curSize;
    }
}
