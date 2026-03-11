using UnityEngine;

public class EffectVFXManager : BasePoolManager<EffectVFXManager>
{
    public GameObject SpawnVFX(Vector3 position, Transform parent)
    {
        GameObject vfxObj = base.Spawn();
        vfxObj.transform.position = position;
        vfxObj.transform.SetParent(parent, true);
        return vfxObj;
    }
    
    public override void ReturnToPool(GameObject vfxObj)
    {
        if (vfxObj == null) return;
        SimpleVFXPlayer player = vfxObj.GetComponent<SimpleVFXPlayer>();
        if (player != null) 
        {
            player.StopAnimation();
        }
        base.ReturnToPool(vfxObj);
    }
}
