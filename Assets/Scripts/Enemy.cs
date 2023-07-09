using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Enemy : BaseActor
{
    public int level;
    public bool isLit;
    public float hurtTimer;

    public Collider2D cd;

    public List<JellyFish> jelliesLittingSelf;
    public List<JellyFish> jelliesBeingHurting;


    // Start is called before the first frame update
    public void Start()
    {
        base.Start();

        level = (int)Random.Range(1, 3);

        size = Random.Range(1, 4) * level;

        cd = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();

        SizeControl();
        LitControl();
        AnimatorControl();
    }

    public void SizeControl()
    {
        transform.localScale = Vector3.one * curSize;
    }

    public void LitControl()
    {
        if (isLit)
        {
            size -= Time.deltaTime;
        }

        if (size <= 0.5f)
        {
            cd.enabled = false;
            isLit = true;

        }

        if (size < 0.001f) Destroy(this.gameObject);

        
    }

    public void AnimatorControl()
    {
        if (jelliesBeingHurting.Count > 0)
        {
            animator.SetBool("isCrazy", true);
        }
        else
        {
            animator.SetBool("isCrazy", false);
        }

        if (isLit)
        {
            animator.SetBool("isLit", true);
        }
        else
        {
            animator.SetBool("isLit", false);
        }
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Jelly")
        {
            JellyFish jelly = other.transform.GetComponent<JellyFish>();

            jelly.isHurting = true;
            jelly.focusedTarget = this;

            if (!jelliesBeingHurting.Contains(jelly))
            {
                jelliesBeingHurting.Add(jelly);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Jelly")
        {
            JellyFish jelly = other.transform.GetComponent<JellyFish>();

            jelly.isHurting = false;

            if (jelliesBeingHurting.Contains(jelly))
            {
                jelliesBeingHurting.Remove(jelly);
            }
        }
    }
}

