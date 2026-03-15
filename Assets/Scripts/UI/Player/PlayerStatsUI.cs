using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private TMP_Text embersText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text armorText;
    [SerializeField] private TMP_Text critRateText;
    [SerializeField] private TMP_Text critDamageText;
    [Header("Mobility")]
    [SerializeField] private TMP_Text moveSpeedText;
    [SerializeField] private TMP_Text rollSpeedText;
    [SerializeField] private TMP_Text rollCooldownText;
    [Header("Stamina")]
    [SerializeField] private TMP_Text maxStaminaText;
    [SerializeField] private TMP_Text staminaRegenText;
    [Header("Animation")]
    [SerializeField] private float countUpDuration = 0.5f;
    [SerializeField] private float staggerDelay = 0.05f;
    private PlayerStatsManager statsManager;
    private Tween[] activeTweens;
    private int tweenCount = 0;
    
    public void Setup()
    {
        statsManager = PlayerManager.Instance.PlayerStatsManager;
        statsManager.OnStatsChange += RefreshUI;
        RefreshUI();
    }
    private void OnDestroy()
    {
        if (statsManager != null)
            statsManager.OnStatsChange -= RefreshUI;
    }
    private void RefreshUI()
    {
        if (statsManager == null) return;
        KillAllTweens();
        activeTweens = new Tween[10];
        tweenCount = 0;
        PlayerStats baseStats = statsManager.BaseStats;
        // Combat
        AnimateStat(embersText,     baseStats.BaseMaxEmbers,    statsManager.bonusMaxEmbers,    "0",    tweenCount++ * staggerDelay);
        AnimateStat(attackText,     baseStats.BaseAtkDamage,    statsManager.bonusAttackDamage, "0",    tweenCount++ * staggerDelay);
        AnimateStat(armorText,      baseStats.BaseArmor,        statsManager.bonusArmor,        "0",    tweenCount++ * staggerDelay);
        AnimateStat(critRateText,   baseStats.BaseCritRate,     statsManager.bonusCritRate,     "0.0",  tweenCount++ * staggerDelay);
        AnimateStat(critDamageText, baseStats.BaseCritDamage,   statsManager.bonusCritDamage,   "0.0",  tweenCount++ * staggerDelay);
        // Mobility
        AnimateStat(moveSpeedText,    baseStats.BaseMoveSpeed, statsManager.bonusMoveSpeed, "0.0", tweenCount++ * staggerDelay);
        AnimateStat(rollSpeedText,    baseStats.RollSpeed,     0f,                          "0.0", tweenCount++ * staggerDelay);
        AnimateStat(rollCooldownText, baseStats.RollCooldown,  0f,                          "0.0", tweenCount++ * staggerDelay);
        // Stamina
        AnimateStat(maxStaminaText,   baseStats.BaseMaxStamina,   statsManager.bonusMaxStamina,   "0",   tweenCount++ * staggerDelay);
        AnimateStat(staminaRegenText, baseStats.BaseStaminaRegen, statsManager.bonusStaminaRegen, "0.0", tweenCount++ * staggerDelay);
    }
    private void AnimateStat(TMP_Text textComp, float baseValue, float bonusValue, string format, float delay)
    {
        if (textComp == null) return;
        textComp.alpha = 0f;
        textComp.text = FormatStat(0f, 0f, format);
        int index = tweenCount - 1;
        if (index >= activeTweens.Length) return;
        textComp.DOFade(1f, 0.2f).SetDelay(delay);
        activeTweens[index] = DOVirtual.Float(0f, baseValue, countUpDuration, value =>
        {
            float bonusProgress = bonusValue * (value / baseValue);
            textComp.text = FormatStat(value, bonusProgress, format);
        })
        .SetDelay(delay)
        .SetEase(Ease.OutCubic);
    }
    private string FormatStat(float baseValue, float bonusValue, string format)
    {
        string baseStr = baseValue.ToString(format);
        if (bonusValue == 0f)
            return baseStr;
        string sign = bonusValue > 0 ? "+" : "";
        string bonusStr = bonusValue.ToString(format);
        return $"{baseStr} ({sign}{bonusStr})";
    }
    private void KillAllTweens()
    {
        if (activeTweens == null) return;
        foreach (var t in activeTweens)
            t?.Kill();
    }
}
