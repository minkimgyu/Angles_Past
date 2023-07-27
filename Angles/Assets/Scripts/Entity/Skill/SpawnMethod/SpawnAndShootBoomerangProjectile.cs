using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnAndShootBoomerangProjectile", menuName = "Scriptable Object/SpawnMethod/SpawnAndShootBoomerangProjectile", order = int.MaxValue)]
public class SpawnAndShootBoomerangProjectile : SpawnMethod
{
    public override void Execute(SpawnSupportData supportData)
    {
        BoomerangProjectile projectile = ObjectPooler.SpawnFromPool<BoomerangProjectile>(projectileName);
        projectile.Init(supportData.Caster.transform.position);
        supportData.Me.SpawnedObjects.Add(projectile);
        projectile.ShootBoomerang(supportData.Caster);
    }
}
