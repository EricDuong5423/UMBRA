using UnityEngine;
[CreateAssetMenu(fileName = "LifestealEffect", menuName = "Roguelike/Effects/LifestealEffect")]
public class LifestealEffect: ItemEffect
{
    [Header("Settings")] 
    public float healthPerStack = 1f;

    public override void OnHitEnemy(GameObject player, IDamageable enemy, float damage, int stackCount)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            float totalHealth = healthPerStack * stackCount;
            playerHealth.Heal(totalHealth);
        }
    }
}