using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public float waveSpeed;
    public float maxDistance;

    public bool isCalled;
    public float curSize;
    public float evoLevel;
    public float callCD;
    public bool isInCD;

    public bool isEnemy;
    public bool isFriend;
    public bool isAvatar;

    public BaseActor owner;
    public List<BaseActor> hearedList;

    public AnimationCurve fadeCurve;
    private float a;

    private Collider2D collider;
    
    // Start is called before the first frame update
    void Start()
    {
        a = transform.GetComponent<SpriteRenderer>().color.a;

        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        AlphaControl();

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * curSize, 2f * Time.deltaTime);

        if (isInCD) 
        {
            callCD -= Time.deltaTime;

            isCalled = false;

            if (callCD <= 0f)
            {
                isInCD = false;
            }

            return;
        }

        if (isCalled)
        {
            curSize += waveSpeed * Time.deltaTime;
            // collider.enabled = true;
        }

        if (curSize >= maxDistance)
        {
            ResetWave();

            // collider.enabled = false;
            
            transform.localScale = Vector3.zero;

            isInCD = true;
            callCD = 1f;
        }
    }

    public void ResetWave()
    {
        if (isAvatar)
        {
            if (evoLevel == 1) 
            {
                curSize = 1.3f;
            }
            else if (evoLevel == 2) 
            {
                curSize = 1.7f;
            }
            else if (evoLevel == 3) 
            {
                curSize = 2.3f;
            }
        }

        if (isFriend)
        {
            if (owner.transform.GetComponent<JellyFish>().isHelped)
            {
                waveSpeed = 5f;
                maxDistance = 10f;
            }

            if (evoLevel == 1) 
                {
                    curSize = 1.3f;
                }
                else if (evoLevel == 2) 
                {
                    curSize = 1.7f;
                }
                else if (evoLevel == 3) 
                {
                    curSize = 2.3f;
                }
        }

        if (isEnemy)
        {
            curSize = owner.size * 0.9f;
        }
    }

    public void AlphaControl()
    {
        Color curColor = new Color(transform.GetComponent<SpriteRenderer>().color.r,
                                    transform.GetComponent<SpriteRenderer>().color.g,
                                    transform.GetComponent<SpriteRenderer>().color.b, 
                                    a * fadeCurve.Evaluate(curSize/maxDistance));

        transform.GetComponent<SpriteRenderer>().color = curColor;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Wave someone_s_wave = other.transform.GetComponent<Wave>();
        BaseActor someone = someone_s_wave.owner;

        if (!hearedList.Contains(someone))
        {
            hearedList.Add(someone);

            if (someone.gameObject.tag == "Jelly")
            {
                if (isAvatar && someone_s_wave.isFriend)
                {
                    someone.DoCall();
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        BaseActor someone = other.transform.GetComponent<Wave>().owner;

        if (hearedList.Contains(someone))
        {
            hearedList.Remove(someone);
        }
    }
}
