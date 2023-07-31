using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnAndShootGravityBall", menuName = "Scriptable Object/SpawnMethod/SpawnAndShootGravityBall", order = int.MaxValue)]
public class SpawnAndShootGravityBall : SpawnMethod
{
    [SerializeField]
    float thrust;

    public override void Execute(SpawnSupportData supportData)
    {
        SoundManager.Instance.PlaySFX(supportData.Me.transform.position, "GravityBall", 0.2f);

        GravityBallProjectile projectile = ObjectPooler.SpawnFromPool<GravityBallProjectile>(projectileName);
        projectile.Init(supportData.Caster.transform.position);
        supportData.Me.SpawnedObjects.Add(projectile);
        projectile.ShootBall(supportData.Caster.GetComponent<Rigidbody2D>().velocity.normalized, thrust);
    }
}
