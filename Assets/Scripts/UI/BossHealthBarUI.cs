using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image bossHealthBarFilling;
    [SerializeField] private Image bossHeathBarFrame;
    [SerializeField] private TMP_Text bossNameText;
    [SerializeField] private float moveY = 40f;
    [SerializeField] private float animationTime = 3f;
    [SerializeField] private Ease animationEase = Ease.OutBounce;

    private Sequence appearSequence;
    private EnemyBase boss;
    
    public void Setup(EnemyBase Boss)
    {
        gameObject.SetActive(true);
        boss = Boss;
        boss.HealthSystem.OnHealthChanged += HandleHealthChanged;
        bossHeathBarFrame.fillAmount = 0f;
        bossNameText.alpha = 0f;
        appearSequence = DOTween.Sequence();
        appearSequence.Append(bossHeathBarFrame.rectTransform.DOAnchorPosY(-moveY, animationTime).From(true));
        appearSequence.Join(bossNameText.rectTransform.DOAnchorPosY(-moveY, animationTime).From(true));
        appearSequence.Join(bossHeathBarFrame.DOFillAmount(1f, animationTime));
        appearSequence.Join(bossNameText.DOFade(1f, animationTime));
        appearSequence.SetEase(animationEase).Play();
    }

    private void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        float ratio = currentHealth / maxHealth;
        ratio = Mathf.Clamp01(ratio);
        ratio = 1 - ratio;
        bossHealthBarFilling.DOFillAmount(ratio, 0.2f).SetEase(Ease.OutQuad);
        if(ratio == 0) gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (boss != null && boss.HealthSystem != null)
        {
            boss.HealthSystem.OnHealthChanged -= HandleHealthChanged;
        }
        
        appearSequence?.Kill();
    }
}
