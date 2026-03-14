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

    public void Setup(ItemData item, int amount)
    {
        if (item == null)
        {
            Clear();
            return;
        }
        Clear();
        bonusHealth.text = item.bonusMaxEmbers.ToString();
        bonusStamina.text = item.bonusStamina.ToString();
        bonusAttack.text = item.bonusAttackDmg.ToString();
        bonusCritRate.text = item.bonusCritRate.ToString();
        bonusCritDamage.text = item.bonusCritDamage.ToString();
        bonusMovement.text = item.bonusMoveSpeed.ToString();
        bonusArmor.text = item.bonusArmor.ToString();
    }

    public void Clear()
    {
        bonusHealth.text = "";
        bonusStamina.text = "";
        bonusAttack.text = "";
        bonusCritRate.text = "";
        bonusCritDamage.text = "";
        bonusMovement.text = "";
        bonusArmor.text = "";
    }
}
