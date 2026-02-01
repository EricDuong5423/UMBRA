using UnityEngine;

public class Skeleton : EnemyBase
{
    protected override void PerformAttack(int attackIndex)
    {
        if (anim == null) return;
        
        switch (attackIndex)
        {
            case 1: anim.SetTrigger("Attack1"); break;
            case 2: anim.SetTrigger("Attack2"); break;
        }
    }
}