using UnityEngine;

[CreateAssetMenu(fileName = "Boosting speed after take hit", menuName = "Roguelike/Effects/Boosting speed after take hit")]
public class SpeedAffterTakeHitEffect: ItemEffect
{
    [Header("Settings")] 
    public float speedBoost = 0.5f;
    public override void OnPlayerTakeDamage(GameObject player, float damageTaken, int stackCount)
    {
        PlayerEffectManager playerEffectManager = player.GetComponent<PlayerEffectManager>();
        if (playerEffectManager != null)
        {
            playerEffectManager.AddEffect(this, stackCount, baseDuration);
        }
    }

    public override void OnEffectApplied(GameObject target, int stackCount)
    {
        target.GetComponent<PlayerStatsManager>()?.AddMoveSpeedModifier(speedBoost * stackCount );
    }

    public override void OnEffectRemoved(GameObject target, int stackCount)
    {
        target.GetComponent<PlayerStatsManager>()?.AddMoveSpeedModifier(-speedBoost * stackCount);
    }
}
