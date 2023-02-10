using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEffect : MonoBehaviour
{
    public ParticleSystem[] particles;
    public float time = 3;
    public bool canOffOverTime = false; // true면 일정 시간 후, 이펙트 회수

    protected virtual void OnEnable()
    {
        if(canOffOverTime == true) Invoke("DisableObject", time);
        PlayEffect();
    }

    public void PlayEffect()
    {
        if (particles == null) return;

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }

    protected void DisableObject() { gameObject.SetActive(false); print("DisableObject"); }

    protected virtual void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
    }
}
