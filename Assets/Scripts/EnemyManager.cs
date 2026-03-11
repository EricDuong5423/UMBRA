using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [SerializeField] private Transform poolContainer;
    
    private Dictionary<GameObject, ObjectPooling> enemyPools = new Dictionary<GameObject, ObjectPooling>();

    private void Awake()
    {
        if (Instance == null) return;
        else Destroy(gameObject);
    }

    public GameObject SpawnEnemy(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;

        if (!enemyPools.ContainsKey(prefab))
        {
            GameObject newPoolObj = new GameObject($"Pool_${prefab.name}");
            newPoolObj.transform.SetParent(poolContainer);
            ObjectPooling newPool = newPoolObj.AddComponent<ObjectPooling>();
            newPool.SetPrefab(prefab);
            enemyPools.Add(prefab, newPool);
        }

        GameObject enemyObj = enemyPools[prefab].Get();
        enemyObj.transform.position = position;
        enemyObj.transform.rotation = rotation;

        EnemyHealth health = enemyObj.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.orginalPrefab = prefab;
        }

        return enemyObj;
    }

    public void ReturnEnemy(GameObject prefab, GameObject enemyObj)
    {
        if (prefab != null && enemyPools.ContainsKey(prefab))
        {
            enemyObj.transform.SetParent(poolContainer);
            enemyPools[prefab].AddToPool(enemyObj);
        }
        else
        {
            Destroy(enemyObj);
        }
    }
}
