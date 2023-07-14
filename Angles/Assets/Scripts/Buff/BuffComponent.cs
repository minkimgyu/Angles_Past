using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffComponent : MonoBehaviour
{
    [SerializeField]
    List<BaseBuff> m_buffs;

    /// <summary>
    /// 스크립터블 오브젝트 만들어서 긁어오기
    /// </summary>
    public void AddBuff(BaseBuff buff)
    {
        if (CheckOverlap(buff) == false) return;
        
        m_buffs.Add(buff);
        buff.OnStart();
    }

    /// <summary>
    /// 버프가 겹쳐도 되는지 체크
    /// </summary>
    /// <returns></returns>
    bool CheckOverlap(BaseBuff buff)
    {
        if (buff.CanOverlap == true) return true;


        // 버프가 리스트에 존재한다면 false를 리턴해준다.
        return !m_buffs.Exists(x => x == buff);
    }

    public void RemoveBuff(BaseBuff buff)
    {
        BaseBuff foundBuff = m_buffs.Find(x => x == buff);

        if (foundBuff == null) return;

        m_buffs.Remove(foundBuff);
        foundBuff.OnEnd();
    }

    void DoBuffUpdate()
    {
        for (int i = 0; i < m_buffs.Count; i++)
        {
            m_buffs[i].OnUpdate();
        }
    }


    private void Update()
    {
        DoBuffUpdate();
    }
}
