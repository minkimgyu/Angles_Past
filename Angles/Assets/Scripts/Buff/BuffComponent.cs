using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffComponent : MonoBehaviour
{
    [SerializeField]
    List<BaseBuff> m_buffs;

    /// <summary>
    /// ��ũ���ͺ� ������Ʈ ���� �ܾ����
    /// </summary>
    public void AddBuff(BaseBuff buff)
    {
        if (CheckOverlap(buff) == false) return;
        
        m_buffs.Add(buff);
        buff.OnStart();
    }

    /// <summary>
    /// ������ ���ĵ� �Ǵ��� üũ
    /// </summary>
    /// <returns></returns>
    bool CheckOverlap(BaseBuff buff)
    {
        if (buff.CanOverlap == true) return true;


        // ������ ����Ʈ�� �����Ѵٸ� false�� �������ش�.
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
