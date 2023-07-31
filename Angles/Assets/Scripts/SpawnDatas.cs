using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnData", menuName = "Scriptable Object/SpawnData", order = int.MaxValue)]
public class SpawnDatas : ScriptableObject
{
    [SerializeField]
    List<SpawnData> spawnDatas;
    public List<SpawnData> SpawnDataCollection { get { return spawnDatas; } }
}
