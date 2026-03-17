using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private GameObject objectPrefab;
    private Queue<GameObject> objectPool = new Queue<GameObject>();
    public void SetPrefab(GameObject prefab) { objectPrefab = prefab; }
    public GameObject Get()
    {
        if (objectPool.Count > 0)
        {
            var result = objectPool.Dequeue();
            result.SetActive(true);
            return result;
        }
        var obj = Instantiate(objectPrefab);
        obj.SetActive(true);
        return obj;
    }
    public void AddToPool(GameObject obj)
    {
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }
}
