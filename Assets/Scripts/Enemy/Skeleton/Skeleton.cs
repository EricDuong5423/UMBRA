using UnityEngine;

public class Skeleton : EnemyBase
{
    protected override void PerformAttack(int attackIndex)
    {
        if (currentState == EnemyState.Hurt) return;

        // Sử dụng Anim (viết hoa) từ lớp cha
        if (Anim == null) return;
        
        switch (attackIndex)
        {
            case 1: Anim.SetTrigger("Attack1"); break;
            case 2: Anim.SetTrigger("Attack2"); break;
        }
    }
}