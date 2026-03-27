using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance { get; private set; }
    
    [Header("Pool Setup")]
    [SerializeField] protected Transform container;
    
    private Dictionary<GameObject, ObjectPooling> pools = new Dictionary<GameObject, ObjectPooling>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public GameObject SpawnProjectile(GameObject prefab, Transform targetTransform, Transform ownerTransform, float projectileDamage, float knockBackForce)
    {
        if (prefab == null) return null;
        
        if (!pools.ContainsKey(prefab))
        {
            ObjectPooling newPool = gameObject.AddComponent<ObjectPooling>();
            newPool.SetPrefab(prefab);
            pools.Add(prefab, newPool);
        }
        
        GameObject obj = pools[prefab].Get();
        obj.transform.SetParent(container, true);
        obj.transform.position = ownerTransform.position;
        obj.transform.rotation = ownerTransform.rotation;

        if (!obj.TryGetComponent(out ProjectileBase projectile)) return null;
        projectile.SetupData(targetTransform, ownerTransform, projectileDamage, knockBackForce);

        return obj;
    }

    public void ReturnToPool(GameObject prefab, GameObject obj)
    {
        if(!prefab || !obj) return;
        obj.SetActive(false);
        obj.transform.SetParent(container);
        
        if (pools.ContainsKey(prefab))
        {
            pools[prefab].AddToPool(obj);
        }
        else
        {
            Destroy(obj);
        }
    }

    public void ReturnAll()
    {
        var allProjectiles = container.GetComponentsInChildren<ProjectileBase>();
        foreach (var projectile in allProjectiles)
        {
            if (projectile.gameObject.activeSelf)
                ReturnToPool(projectile.originalPrefab, projectile.gameObject);
        }
    }
}
