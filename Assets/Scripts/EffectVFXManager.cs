using UnityEngine;

public class EffectVFXManager : MonoBehaviour
{
    public static EffectVFXManager Instance;

    [SerializeField] private GameObject genericVFXPrefab; 
    [SerializeField] private Transform poolContainer;     

    private ObjectPooling vfxPool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        vfxPool = gameObject.AddComponent<ObjectPooling>();
        vfxPool.SetPrefab(genericVFXPrefab);
    }

    public GameObject SpawnGenericVFX(Vector3 position, Transform parent)
    {
        GameObject vfxObj = vfxPool.Get();
        vfxObj.transform.position = position;
        vfxObj.transform.SetParent(parent, true); 
        return vfxObj;
    }

    public void ReturnGenericVFX(GameObject vfxObj)
    {
        if (vfxObj == null) return;
        vfxObj.GetComponent<SimpleVFXPlayer>()?.StopAnimation();
        vfxObj.transform.SetParent(poolContainer);
        vfxPool.AddToPool(vfxObj);
    }
}
