using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectPlayer : BasicEffectPlayer
{
    [SerializeField]
    List<ParticleSystem> m_particles = new List<ParticleSystem>();

    float originalScale;

    private void Awake()
    {
        ParticleSystem[] childSystems = GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < childSystems.Length; i++)
        {
            m_particles.Add(childSystems[i]);
        }

        originalScale = transform.localScale.x;
    }

    protected override void OnDisable()
    {
        transform.localScale = new Vector3(originalScale, originalScale, 1); // 오리지널 스케일로 마지막에 리셋해주기
        base.OnDisable();
        // --> 모든 이팩트의 기본 scale는 1에 맞추자
        // 여기에 추가 Scale를 넣어서 크기를 키우는 방식으로 구성
    }

    protected override void ResetSize(float sizeMultifly)
    {
        transform.localScale = new Vector3(transform.localScale.x * sizeMultifly, transform.localScale.y * sizeMultifly, 1);
    }

    protected override void ResetLifeTime(float[] lifeTime)
    {
        for (int i = 0; i < m_particles.Count; i++)
        {
            var main = m_particles[i].main;
            main.startLifetime = lifeTime[i];
        }
    }

    public override void RotationEffect(float rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);

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

        if (m_duration == -1) return;
        Invoke("DisableObject", m_duration);
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
