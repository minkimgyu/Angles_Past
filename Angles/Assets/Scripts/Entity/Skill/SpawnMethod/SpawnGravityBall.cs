using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnGravityBall", menuName = "Scriptable Object/SpawnMethod/SpawnGravityBall", order = int.MaxValue)]
public class SpawnGravityBall : SpawnMethod
{
    public override void Execute(SpawnSupportData supportData)
    {
        BasicProjectile projectile = ObjectPooler.SpawnFromPool<BasicProjectile>(projectileName);
        projectile.Init(supportData.Caster.transform.position);
    }
}
