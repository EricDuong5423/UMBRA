using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private GameObject objectPrefab;
    private List<GameObject> objectPool = new();

    public void SetPrefab(GameObject prefab)
    {
        objectPrefab = prefab;
    }

    public GameObject Get()
    {
        if (objectPool == null || objectPool.Count <= 0)
        {
            var obj = Instantiate(objectPrefab);
            obj.SetActive(true);
            return obj;
        }
        var result = objectPool[0];
        result.SetActive(true);
        objectPool.RemoveAt(0);
        return result;
    }

    public void AddToPool(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Add(obj);
    }
}
