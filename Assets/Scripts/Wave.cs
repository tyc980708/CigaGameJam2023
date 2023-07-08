using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public bool isCalled;
    public float curSize;
    public float evoLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * curSize, 2f * Time.deltaTime);

        if (isCalled)
        {
            curSize += 7.5f * Time.deltaTime;
        }

        if (curSize >= 30f)
        {
            isCalled = false;
            curSize = 1f + 0.3f * evoLevel;
            transform.localScale = Vector3.zero;
        }
    }
}
