using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private EnemyBase enemyBase;

    private void Start()
    {
        if (enemyBase != null)
        {
            enemyBase.HealthSystem.OnHealthChanged += UpdateView;
            UpdateView(enemyBase.HealthSystem.CurrentEmbers, enemyBase.HealthSystem.MaxEmbers);
        }
    }

    private void UpdateView(float currentEmber, float maxEmber)
    {
        float ratio = currentEmber / maxEmber;
        ratio = Mathf.Clamp(ratio, 0, 1);
        
        healthBarFill.fillAmount = ratio;
    }
}
