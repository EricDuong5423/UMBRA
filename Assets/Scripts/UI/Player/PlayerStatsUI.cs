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
        AnimateStat(embersText,     baseStats.BaseMaxEmbers,    statsManager.bonusMaxEmbers,    "0");
        AnimateStat(attackText,     baseStats.BaseAtkDamage,    statsManager.bonusAttackDamage, "0");
        AnimateStat(armorText,      baseStats.BaseArmor,        statsManager.bonusArmor,        "0");
        AnimateStat(critRateText,   baseStats.BaseCritRate,     statsManager.bonusCritRate,     "0.0");
        AnimateStat(critDamageText, baseStats.BaseCritDamage,   statsManager.bonusCritDamage,   "0.0");
        // Mobility
        AnimateStat(moveSpeedText,    baseStats.BaseMoveSpeed, statsManager.bonusMoveSpeed, "0.0");
        AnimateStat(rollSpeedText,    baseStats.RollSpeed,     0f,                          "0.0");
        AnimateStat(rollCooldownText, baseStats.RollCooldown,  0f,                          "0.0");
        // Stamina
        AnimateStat(maxStaminaText,   baseStats.BaseMaxStamina,   statsManager.bonusMaxStamina,   "0");
        AnimateStat(staminaRegenText, baseStats.BaseStaminaRegen, statsManager.bonusStaminaRegen, "0.0");
    }
    private void AnimateStat(TMP_Text textComp, float baseValue, float bonusValue, string format)
    {
        if (textComp == null) return;
        int index = tweenCount;
        tweenCount++; 
        
        if (index >= activeTweens.Length) return;

        textComp.alpha = 0f;
        textComp.text = FormatStat(0f, 0f, format);
        
        textComp.DOFade(1f, 0.2f).SetUpdate(true).Play();
        
        activeTweens[index] = DOVirtual.Float(0f, baseValue, countUpDuration, value =>
            {
                float bonusProgress = baseValue == 0f ? bonusValue : bonusValue * (value / baseValue);
            
                textComp.text = FormatStat(value, bonusProgress, format);
            })
            .SetUpdate(true)
            .SetEase(Ease.OutCubic)
            .Play();
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
