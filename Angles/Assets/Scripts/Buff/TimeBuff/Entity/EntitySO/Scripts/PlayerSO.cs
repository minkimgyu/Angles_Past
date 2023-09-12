using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerSO", menuName = "Scriptable Object/EntitySO/PlayerSO")]
public class PlayerSO : BaseEntitySO
{
    [SerializeField]
    bool immortality;

    [SerializeField]
    BuffFloat hp;

    [SerializeField]
    BuffFloat speed;

    [SerializeField]
    BuffFloat stunTime;

    [SerializeField]
    BuffFloat weight;

    [SerializeField]
    BuffFloat mass;

    [SerializeField]
    BuffFloat drag;

    [SerializeField]
    string dieEffectName;

    [SerializeField]
    BuffFloat readySpeedDecreaseRatio;

    [SerializeField]
    BuffFloat rushThrust;

    [SerializeField]
    BuffFloat rushRecoverRatio;

    [SerializeField]
    BuffFloat rushDuration;

    [SerializeField]
    float attackCancelOffset;

    [SerializeField]
    BuffInt dashCount;

    [SerializeField]
    BuffFloat dashDuration;

    [SerializeField]
    BuffFloat dashThrust;

    [SerializeField]
    BuffFloat dashRecoverRatio;

    [SerializeField]
    string[] grantedSkillNames;

    public override Entity Create()
    {
        Player player = ObjectPooler.SpawnFromPool<Player>(name);

        player.Initialize(immortality, hp.CopyData(), speed, stunTime, weight, mass, drag, dieEffectName,
        readySpeedDecreaseRatio, rushThrust, rushRecoverRatio, rushDuration, attackCancelOffset,
        dashCount, dashDuration, dashThrust, dashRecoverRatio, grantedSkillNames);

        // 다음과 같이 초기화

        return player;
    }
}
