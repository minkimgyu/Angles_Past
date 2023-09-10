using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnEntity
{
    public string SpawnEntityName { get; set; }

    public int SpawnCount { get; set; }

    public SpawnCueEventSO SpawnSO { get; set; }

    public void Spawn();
}

public class OrangeSquareEnemy : YellowSquareEnemy, ISpawnEntity
{
    public string SpawnEntityName { get; set; }
    public int SpawnCount { get; set; }
    public SpawnCueEventSO SpawnSO { get; set; }

    public void Initialize(bool immortality, BuffFloat hp, BuffFloat speed, BuffFloat stunTime,
       BuffFloat weight, BuffFloat mass, BuffFloat drag, string dieEffectName, string[] skillNames,
       BuffInt score, BuffInt goldCount, BuffFloat spawnPercentage, BuffFloat skillUseDistance, BuffFloat skillUseOffsetDistance,
       BuffFloat followDistance, BuffFloat followOffsetDistance, string spawnEntityName, int spawnCount, SpawnCueEventSO spawnSO)
    {
        Initialize(immortality, hp, speed, stunTime, weight, mass, drag, dieEffectName, skillNames, score, goldCount, spawnPercentage, skillUseDistance,
            skillUseOffsetDistance, followDistance, followOffsetDistance);

        SpawnEntityName = spawnEntityName;
        SpawnCount = spawnCount;
        SpawnSO = spawnSO;
    }

    public override void Die()
    {
        Spawn(); // 스포너에서 스폰하게끔 만들어주기 --> SO 이용
        base.Die();
    }

    public void Spawn()
    {
        for (int i = 0; i < SpawnCount; i++)
            SpawnSO.OnActionRequested(transform.position, SpawnCount, SpawnEntityName);
    }
}
