using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : MonoBehaviour
{
    public ParticleSystem[] particleSystems;

    SkillMode skillMode;
    public SkillMode SkillMode
    {
        get
        {
            return skillMode;
        }
        set
        {
            skillMode = value;
        }
    }

    private void OnEnable()
    {
        PlaySkillWhenGet();
    }


    public void PlayEffect()
    {
        Debug.Log("adwadawdawda");

        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
    }

    public virtual void Init()
    {

    }

    public virtual void PlaySkillWhenCollision()
    {

    }

    public virtual void PlaySkillWhenAttackStart()
    {

    }

    public virtual void PlaySkillWhenGet()
    {

    }
}
