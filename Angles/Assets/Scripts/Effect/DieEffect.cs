using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class 

public class DieEffect : BasicEffect
{
    ParticleSystem attachedParticle;
    public float disableTime = 3f;

    [Header("Red")]
    public Color start;
    public Color end;

    [Header("Red")]
    public Color start;
    public Color end;

    [Header("Red")]
    public Color start;
    public Color end;

    private void Awake()
    {
        attachedParticle = GetComponent<ParticleSystem>();
    }

    public void Init()
    {
        ParticleSystem.MainModule main = attachedParticle.main;
        main.startColor = new ParticleSystem.MinMaxGradient(,);
        PlayEffect();
    }

    private void OnEnable()
    {
        Invoke("DisableObject", disableTime);
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }

    protected void DisableObject() => gameObject.SetActive(false);
}
