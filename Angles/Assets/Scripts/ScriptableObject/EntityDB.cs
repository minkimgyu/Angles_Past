using UnityEngine;

[System.Serializable]
public class EntityData
{
    [SerializeField]
    private string name;
    public string Name { get { return name; } set { name = value; } }

    [SerializeField]
    private float hp;
    public float Hp { get { return Hp; } set { hp = value; } }

    [SerializeField]
    private float damage;
    public float Damage { get { return damage; } set { damage = value; } }

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
}

[CreateAssetMenu(fileName = "EntityDB", menuName = "Scriptable Object/EntityDB")]
public class EntityDB : ScriptableObject
{
    public EntityData[] entityDatas;
}