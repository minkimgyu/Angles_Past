using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEffect : MonoBehaviour
{
    public ParticleSystem[] particles;

    public virtual void PlayEffect()
    {
        if (particles == null) return;

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }

    public void StopEffect()
    {
        if (particles == null) return;

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
        }
    }
}
