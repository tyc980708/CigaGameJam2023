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
    public float touchedRecoverFactor;
    public AnimationCurve tinkleCurve;
    public float tinkleCursor = 1f;

    private Vector3 localPos;

    // Start is called before the first frame update
    public void Start()
    {
        transform.localScale = Vector3.zero;

        particle = transform.Find("BubbleParticle").transform.GetComponent<ParticleSystem>();

        restPercentage = 1f;

        tinkleCursor = 1f;

        localPos = transform.localPosition;
    }

    // Update is called once per frame
    public void Update()
    {
        SizeControl();
        ParticleControl();
        TinkleControl();
    }

    public void SizeControl()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (0.1f + 0.2f * restPercentage) * (evoLevel), 4f * Time.deltaTime);

        if (evoLevel == 1f)
        {
            transform.localPosition = localPos * 1f;
        }
        else if (evoLevel == 2f)
        {
            transform.localPosition = localPos * 2f;
        }
        else if (evoLevel == 3f)
        {
            transform.localPosition = localPos * 3f;
        }
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

    public void TinkleControl()
    {
        if (tinkleCursor < 1f)
        {
            tinkleCursor += Time.deltaTime;
        }

        Color curColor = new Color(transform.GetComponent<SpriteRenderer>().color.r,
                                    transform.GetComponent<SpriteRenderer>().color.g,
                                    transform.GetComponent<SpriteRenderer>().color.b, 
                                    200f/255f * tinkleCurve.Evaluate(tinkleCursor));

        transform.GetComponent<SpriteRenderer>().color = curColor;
    }
}
