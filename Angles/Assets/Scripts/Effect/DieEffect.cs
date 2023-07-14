using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DieEffectColor
{
    [SerializeField]
    string color;

    [SerializeField]
    Color min;

    [SerializeField]
    public Color max;

    public void ResetColor(string color, ParticleSystem particle)
    {
        ParticleSystem.MainModule main = particle.main;
        if (this.color != color) return;

        main.startColor = new ParticleSystem.MinMaxGradient(min, max);
    }
}

public class DieEffect : BasicEffect
{
    ParticleSystem attachedParticle;
    public float disableTime = 3f;

    [SerializeField]
    DieEffectColor[] dieColor;

    private void Awake()
    {
        attachedParticle = GetComponent<ParticleSystem>();
    }

    public void Init(string color)
    {
        for (int i = 0; i < dieColor.Length; i++)
        {
            dieColor[i].ResetColor(color, attachedParticle);
        }
        
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
}
