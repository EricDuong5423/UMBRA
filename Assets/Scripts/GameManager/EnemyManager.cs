using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    
    [Header("Pool Setup")]
    [SerializeField] protected Transform container;
    
    protected Dictionary<GameObject, ObjectPooling> pools = new Dictionary<GameObject, ObjectPooling>();

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public virtual GameObject SpawnEnemy(GameObject prefab, Vector3 position, Quaternion rotation)
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
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        
        EnemyBase enemyBase = obj.GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            enemyBase.originalPrefab = prefab;
        }
        return obj;
    }
    
    public virtual void ReturnEnemy(GameObject prefab, GameObject obj)
    {
        if (obj == null || prefab == null) return;
        
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
    
    public void ReturnAllEnemies()
    {
        var allEnemies = container.GetComponentsInChildren<EnemyBase>(true);
        foreach (var enemy in allEnemies)
        {
            if (enemy.gameObject.activeSelf)
                ReturnEnemy(enemy.originalPrefab, enemy.gameObject);
        }
    }
}