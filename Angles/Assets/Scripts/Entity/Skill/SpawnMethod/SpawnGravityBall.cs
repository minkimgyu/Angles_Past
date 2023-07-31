using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnGravityBall", menuName = "Scriptable Object/SpawnMethod/SpawnGravityBall", order = int.MaxValue)]
public class SpawnGravityBall : SpawnMethod
{
    public override void Execute(SpawnSupportData supportData)
    {
        SoundManager.Instance.PlaySFX(supportData.Me.transform.position, "GravityBall", 0.2f);

        BasicProjectile projectile = ObjectPooler.SpawnFromPool<BasicProjectile>(projectileName);
        projectile.Init(supportData.Caster.transform.position);
        supportData.Me.SpawnedObjects.Add(projectile);
    }
}
