using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LightSpot : MonoBehaviour
{
    public float evoLevel;
    public bool isDashing;
    public float restPercentage;
    public ParticleSystem particle;

    // Start is called before the first frame update
    public void Start()
    {
        particle = transform.Find("BubbleParticle").transform.GetComponent<ParticleSystem>();

        restPercentage = 1f;
    }

    // Update is called once per frame
    public void Update()
    {
        SizeControl();
        ParticleControl();
    }

    public void SizeControl()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (0.1f + 0.2f * restPercentage) * evoLevel, 1f);
    }

    public void ParticleControl()
    {
        var em = particle.emission;

        if (isDashing)
        {
            em.rateOverTime = Random.Range(5f, 10f);
        }
        else
        {
            em.rateOverTime = Random.Range(0.1f, 0.5f);
        }
    }
}
