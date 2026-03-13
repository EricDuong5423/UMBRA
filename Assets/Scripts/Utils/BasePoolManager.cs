using System;
using UnityEngine;

public abstract class BasePoolManager<T> : MonoBehaviour where T: BasePoolManager<T>
{
    public static T Instance { get; private set; }
    
    [Header("Pool Setup")]
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected Transform container;

    protected ObjectPooling pool;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        pool = gameObject.AddComponent<ObjectPooling>();
        pool.SetPrefab(prefab);
    }

    public virtual GameObject Spawn()
    {
        GameObject obj = pool.Get();
        obj.transform.SetParent(container, true);
        return obj;
    }

    public virtual void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;
        obj.transform.SetParent(container);
        pool.AddToPool(obj);
    }
}