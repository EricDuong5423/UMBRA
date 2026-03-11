using UnityEngine;

public class DamageTextManager : BasePoolManager<DamageTextManager>
{
    public void SpawnDamageText(string value, Color color, Transform parentTransform)
    {
        GameObject obj = base.Spawn();
        
        DamageText damageText = obj.GetComponent<DamageText>();

        if (damageText != null)
        {
            damageText.SetData(value, color, parentTransform);
        }
    }
}
