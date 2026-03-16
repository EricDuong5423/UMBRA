using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image easeHealthBarFill;
    [SerializeField] private EnemyBase enemyBase;
    [SerializeField] private float effectDuration = 0.5f;

    private Tween easeHealthBarEffect;
    private float lastHealth;

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
        var sequence =  DOTween.Sequence();
        easeHealthBarEffect = sequence
            .Append(healthBarFill.DOFillAmount(ratio, 0f))
            .Append(easeHealthBarFill.DOFillAmount(ratio, effectDuration))
            .Play();
    }
}
