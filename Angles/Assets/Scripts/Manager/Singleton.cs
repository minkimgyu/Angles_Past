using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting == true) return null;

            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    GameObject obj = Resources.Load("Prefabs/" + typeof(T).Name) as GameObject;
                    instance = Instantiate(obj).GetComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = GetComponent<T>();

            if (transform.parent != null && transform.root != null)
            {
                DontDestroyOnLoad(transform.root.gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}
