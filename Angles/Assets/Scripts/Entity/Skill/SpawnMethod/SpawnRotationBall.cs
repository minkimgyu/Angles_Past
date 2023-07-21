using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --> 이거를 스크립터블 오브젝트로 만들어서 끼워맞춰주자
[System.Serializable]
[CreateAssetMenu(fileName = "SpawnRotationBall", menuName = "Scriptable Object/SpawnMethod/SpawnRotationBall", order = int.MaxValue)]
public class SpawnRotationBall : SpawnMethod
{
    [SerializeField]
    float diatance;

    public override void Execute(SpawnSupportData supportData)
    {
        for (int i = 0; i < supportData.Me.Data.SpawnCount; i++)
        {
            BasicProjectile projectile = ObjectPooler.SpawnFromPool<BasicProjectile>(projectileName);
            projectile.transform.SetParent(supportData.Me.transform);

            supportData.Me.SpawnedObjects.Add(projectile);
        }
        
        for (int j = 0; j < supportData.Me.SpawnedObjects.Count; j++)
        {
            supportData.Me.SpawnedObjects[j].transform.position = Vector3.zero;

            float angle = (360.0f * j) / supportData.Me.SpawnedObjects.Count;

            //Debug.Log(angle);

            Vector3 offset = Vector3.up * diatance;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 rotatedOffset = rotation * offset;

            supportData.Me.SpawnedObjects[j].Init(rotatedOffset);
        }
    }
}
