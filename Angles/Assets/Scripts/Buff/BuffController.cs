using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffFactory
{
    Dictionary<string, IBuff> storedBuffs;

    public void Init() // 이런 식으로 스킬 제작
    {
        // 이런식으로 다른 곳에서 돌려쓸 수 있도록 제작
        storedBuffs.Add("CasterCircleRangeAttack", new HealthEntitySpeedBuff());
    }

    public IBuff OrderBuff(Transform caster, BuffData data)
    {
        IBuff buff = storedBuffs[data.Name].CreateCopy(data);
        skill.Init(caster, data);
        skill.Execute();

        return storedSkills[data.PrefabName];
    }
}

public class BuffController : MonoBehaviour
{
    [SerializeField]
    List<IBuff> m_buffs;

    BuffEffectComponent effectComponent;

    private void Start()
    {
        effectComponent = GetComponentInChildren<BuffEffectComponent>();
    }

    public BaseBuff AddBuff(BuffData data) //--> 같은 효과를 n개 이상 넣으면 무시 --> buffData를 넘겨주자   
    {
        if (CheckBuffList(data.Name) >= data.MaxCount) return null; // maxCount보다 더 많은 버프를 가지고 있다면 Return

        BaseBuff foundBuff = ObjectPooler.SpawnFromPool<BaseBuff>(data.Name);

        foundBuff.Init(data); // 버프 초기화

        m_buffs.Add(foundBuff);
        foundBuff.OnStart(gameObject, effectComponent);

        return foundBuff;
    }  

    public bool RemoveBuff(BuffData data)
    {
        BaseBuff foundBuff = m_buffs.Find(x => x.Data == data.Name);
        if (foundBuff == null) return false;

        m_buffs.Remove(foundBuff);
        foundBuff.OnEnd(effectComponent);

        return true;
    }

    int CheckBuffList(string name)
    {
        int tmpCount = 0;

        for (int i = 0; i < m_buffs.Count; i++)
        {
            if (m_buffs[i].Data.Name == name) tmpCount++;
        }

        return tmpCount;
    }

    void BuffTick()
    {
        for (int i = 0; i < m_buffs.Count; i++)
        {
            m_buffs[i].Tick(Time.deltaTime);
            if(m_buffs[i].IsFinished)
            {
                m_buffs[i].OnEnd(effectComponent);
                m_buffs.Remove(m_buffs[i]);
            }
        }
    }

    private void OnDisable()
    {
        if (m_buffs.Count <= 0) return;

        for (int i = 0; i < m_buffs.Count; i++)
        {
            m_buffs[i].OnEnd(effectComponent);
            m_buffs.Remove(m_buffs[i]);
        }
    }

    private void Update()
    {
        BuffTick();
    }
}
