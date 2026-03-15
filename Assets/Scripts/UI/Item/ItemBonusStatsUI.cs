using DG.Tweening;
using TMPro;
using UnityEngine;

public class ItemBonusStatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text bonusHealth;
    [SerializeField] private TMP_Text bonusStamina;
    [SerializeField] private TMP_Text bonusAttack;
    [SerializeField] private TMP_Text bonusCritRate;
    [SerializeField] private TMP_Text bonusCritDamage;
    [SerializeField] private TMP_Text bonusMovement;
    [SerializeField] private TMP_Text bonusArmor;
    [Header("Animation")]
    [SerializeField] private float countUpDuration = 0.6f;
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private float staggerDelay = 0.08f;
    private TMP_Text[] statTexts;
    private Tween[] countTweens;
    private void Awake()
    {
        statTexts = new TMP_Text[]
        {
            bonusHealth, bonusStamina, bonusAttack,
            bonusCritRate, bonusCritDamage, bonusMovement, bonusArmor
        };
        countTweens = new Tween[statTexts.Length];
    }
    public void Setup(ItemData item, int amount)
    {
        if (item == null)
        {
            Clear();
            return;
        }
        Clear();
        float[] values = new float[]
        {
            item.bonusMaxEmbers,
            item.bonusStamina,
            item.bonusAttackDmg,
            item.bonusCritRate,
            item.bonusCritDamage,
            item.bonusMoveSpeed,
            item.bonusArmor
        };
        for (int i = 0; i < statTexts.Length; i++)
        {
            int index = i;
            float targetValue = values[i];
            TMP_Text t = statTexts[i];
            t.alpha = 0f;
            t.text = "0";
            t.DOFade(1f, fadeDuration)
             .SetDelay(index * staggerDelay)
             .SetEase(Ease.OutCubic);
            countTweens[index] = DOVirtual.Float(0, targetValue, countUpDuration, value =>
            {
                t.text = value.ToString("0.0");
            })
            .SetDelay(index * staggerDelay)
            .SetEase(Ease.OutCubic);
        }
    }
    public void Clear()
    {
        if (countTweens != null)
        {
            foreach (var t in countTweens)
                t?.Kill();
        }
        if (statTexts != null)
        {
            foreach (var t in statTexts)
            {
                t.text = "";
                t.alpha = 1f;
            }
        }
    }
}
