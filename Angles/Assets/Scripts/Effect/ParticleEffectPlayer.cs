using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectPlayer : BasicEffectPlayer
{
    [SerializeField]
    List<ParticleSystem> m_particles = new List<ParticleSystem>();

    private void Awake()
    {
        ParticleSystem[] childSystems = GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < childSystems.Length; i++)
        {
            m_particles.Add(childSystems[i]);
        }
    }

    public override void RotationEffect(float rotation)
    {
        for (int i = 0; i < m_particles.Count; i++)
        {
            var main = m_particles[i].main;
            main.startRotation = -rotation * Mathf.Deg2Rad;
        }
    }

    public override void PlayEffect()
    {
        if (m_particles == null) return;

        for (int i = 0; i < m_particles.Count; i++)
        {
            m_particles[i].Play();
        }

        Invoke("DisableObject", 5f);
    }

    public override void StopEffect()
    {
        if (m_particles == null) return;

        for (int i = 0; i < m_particles.Count; i++)
        {
            m_particles[i].Stop();
        }

        DisableObject();
    }
}
