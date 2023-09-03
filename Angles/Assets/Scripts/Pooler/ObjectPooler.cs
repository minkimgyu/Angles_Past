using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ObjectPooler))]
public class ObjectPoolerEditor : Editor
{
	const string INFO = "Ǯ���� ������Ʈ�� ������ �������� \nvoid OnDisable()\n{\n" +
		"    ObjectPooler.ReturnToPool(gameObject);    // �� ��ü�� �ѹ��� \n" +
		"    CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� \n}";

	public override void OnInspectorGUI()
	{
		EditorGUILayout.HelpBox(INFO, MessageType.Info);
		base.OnInspectorGUI();
	}
}
#endif

[Serializable]
public class Pool
{
	public string tag;
	public GameObject prefab;
	public int size;

	public Pool(string _tag, GameObject _prefab, int _size)
	{
		tag = _tag;
		prefab = _prefab;
		size = _size;
	}
}

public class ObjectPooler : MonoBehaviour
{
	static ObjectPooler inst;
	void Awake() 
	{ 
		inst = this;
		tr = transform;
		Init();
	}

	public static Transform tr;

	//Action initAction;
	//public static Action InitAction { get; set; }

    [SerializeField] List<Pool> pools = new List<Pool>();
	List<GameObject> spawnObjects;
	Dictionary<string, Queue<GameObject>> poolDictionary;
	readonly string INFO = " ������Ʈ�� ������ �������� \nvoid OnDisable()\n{\n" +
		"    ObjectPooler.ReturnToPool(gameObject);    // �� ��ü�� �ѹ��� \n" +
		"    CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� \n}";

	public static void AddPool(Pool pool) =>
		inst.AddToPool(pool);

	void AddToPool(Pool pool) => pools.Add(pool);

	public static void ReturnToTransform(Transform tr) => inst._ReturnToTranstorm(tr);

	//public static GameObject SpawnFromPool(string tag) =>
	//	inst._SpawnFromPool(tag, Vector3.zero, Quaternion.identity, inst.transform);

	//public static GameObject SpawnFromPool(string tag, Vector3 position) =>
	//	inst._SpawnFromPool(tag, position, Quaternion.identity, inst.transform);

	//public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) =>
	//	inst._SpawnFromPool(tag, position, rotation, inst.transform);

	//public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform transform) =>
	//	inst._SpawnFromPool(tag, position, rotation, transform);

	public static T SpawnFromPool<T>(string tag)// where T : Component
	{
		GameObject obj = inst._SpawnFromPool(tag, Vector3.zero, Quaternion.identity, inst.transform);
		if (obj.TryGetComponent(out T component))
			return component;
		else
		{
			obj.SetActive(false);
			throw new Exception($"Component not found");
		}
	}

	public static T SpawnFromPool<T>(string tag, Vector3 position)// where T : Component
	{
		GameObject obj = inst._SpawnFromPool(tag, position, Quaternion.identity, inst.transform);
		if (obj.TryGetComponent(out T component))
			return component;
		else
		{
			obj.SetActive(false);
			throw new Exception($"Component not found");
		}
	}

	public static T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation)// where T : Component
	{
		GameObject obj = inst._SpawnFromPool(tag, position, rotation, inst.transform);
		if (obj.TryGetComponent(out T component))
			return component;
		else
		{
			obj.SetActive(false);
			throw new Exception($"Component not found");
		}
	}

	public static T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation, Transform transform)// where T : Component
	{
		GameObject obj = inst._SpawnFromPool(tag, position, rotation, transform);
		if (obj.TryGetComponent(out T component))
			return component;
		else
		{
			obj.SetActive(false);
			throw new Exception($"Component not found");
		}
	}

	public static List<GameObject> GetAllPools(string tag)
	{
		if (!inst.poolDictionary.ContainsKey(tag))
			throw new Exception($"Pool with tag {tag} doesn't exist.");

		return inst.spawnObjects.FindAll(x => x.name == tag);
	}

	public static List<T> GetAllPools<T>(string tag) where T : Component
	{
		List<GameObject> objects = GetAllPools(tag);

		if (!objects[0].TryGetComponent(out T component))
			throw new Exception("Component not found");

		return objects.ConvertAll(x => x.GetComponent<T>());
	}

	public static void ReturnToPool(GameObject obj)
	{
		if (!inst.poolDictionary.ContainsKey(obj.name))
			throw new Exception($"Pool with tag {obj.name} doesn't exist.");

		inst.poolDictionary[obj.name].Enqueue(obj);
	}

	[ContextMenu("GetSpawnObjectsInfo")]
	void GetSpawnObjectsInfo()
	{
		foreach (var pool in pools)
		{
			int count = spawnObjects.FindAll(x => x.name == pool.tag).Count;
			Debug.Log($"{pool.tag} count : {count}");
		}
	}


	void _ReturnToTranstorm(Transform tr)
    {
		tr.SetParent(transform);
	}

	GameObject _SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform transform)
	{
		if (!poolDictionary.ContainsKey(tag))
        {
			//throw new Exception($"Pool with tag {tag} doesn't exist.");
			return null;
		}

		// ť�� ������ ���� �߰�
		Queue<GameObject> poolQueue = poolDictionary[tag];
		if (poolQueue.Count <= 0)
		{
			Pool pool = pools.Find(x => x.tag == tag);
			var obj = CreateNewObject(pool.tag, pool.prefab);
			ArrangePool(obj);
		}

		// ť���� ������ ���
		GameObject objectToSpawn = poolQueue.Dequeue();

		if(transform != null) objectToSpawn.transform.SetParent(transform);
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;
		objectToSpawn.SetActive(true);

		return objectToSpawn;
	}

	void Init()
    {
		//if (InitAction != null) InitAction();

		spawnObjects = new List<GameObject>();
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		// �̸� ����
		foreach (Pool pool in pools)
		{
			poolDictionary.Add(pool.tag, new Queue<GameObject>());
			for (int i = 0; i < pool.size; i++)
			{
				var obj = CreateNewObject(pool.tag, pool.prefab);
				ArrangePool(obj);
			}

			// OnDisable�� ReturnToPool �������ο� �ߺ����� �˻�
			if (poolDictionary[pool.tag].Count <= 0)
				Debug.LogError($"{pool.tag}{INFO}");
			else if (poolDictionary[pool.tag].Count != pool.size)
				Debug.LogError($"{pool.tag}�� ReturnToPool�� �ߺ��˴ϴ�");
		}
	}

	// ������Ʈ Ǯ���� ������ ���ÿ� �ʱ�ȭ�� �Ǿ����
	GameObject CreateNewObject(string tag, GameObject prefab)
	{
		var obj = Instantiate(prefab, transform);
		obj.name = tag;
		obj.SetActive(false); // ��Ȱ��ȭ�� ReturnToPool�� �ϹǷ� Enqueue�� ��
		return obj;
	}

	void ArrangePool(GameObject obj)
	{
		// �߰��� ������Ʈ ��� ����
		bool isFind = false;
		for (int i = 0; i < transform.childCount; i++)
		{
			if (i == transform.childCount - 1)
			{
				obj.transform.SetSiblingIndex(i);
				spawnObjects.Insert(i, obj);
				break;
			}
			else if (transform.GetChild(i).name == obj.name)
				isFind = true;
			else if (isFind)
			{
				obj.transform.SetSiblingIndex(i);
				spawnObjects.Insert(i, obj);
				break;
			}
		}
	}
}