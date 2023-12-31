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
    public bool isAudioPlayed;

    public bool isEnemy;
    public bool isFriend;
    public bool isAvatar;
    private bool keepYourself;

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

        if (isFriend && owner.transform.GetComponent<JellyFish>().isHelped)
        {
            waveSpeed = 5f;
            maxDistance = 10f;
        }
        else
        {
            waveSpeed = 15f;
            maxDistance = 60f;
        }

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
            if (!isAudioPlayed && !isEnemy)
            {
                AudioManager.PlaySound("Avatar_Call_Normal", 0, owner.gameObject, isFriend);
                isAudioPlayed = true;
            }
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
            if (evoLevel <= 1) 
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

        isAudioPlayed = false;
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
        if (isEnemy) return;

        Wave someone_s_wave = other.transform.GetComponent<Wave>();
        BaseActor someone = someone_s_wave.owner;

        if (!hearedList.Contains(someone))
        {
            hearedList.Add(someone);

            if (someone.gameObject.tag == "Jelly")
            {
                if (isAvatar && someone_s_wave.isFriend && isCalled && !someone_s_wave.isCalled)
                {
                    someone.DoCall();
                    if (Vector3.Distance(someone.transform.position, transform.position) < 20f && someone.state != 1)
                    {
                        someone.focusedTarget = this.transform.parent.GetComponent<BaseActor>();
                    }
                }
            }

            if (someone.gameObject.tag == "Taint")
            {
                // someone.transform.GetComponent<Enemy>().GoCrazy();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (isEnemy) return;

        BaseActor someone = other.transform.GetComponent<Wave>().owner;

        if (hearedList.Contains(someone))
        {
            hearedList.Remove(someone);
        }
    }
}
