using TMPro;
using UnityEngine;

public class ItemGeneralInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemAmount;

    public void Setup(ItemData item, int amount)
    {
        if (item == null)
        {
            Clear();
            return;
        }
        Clear();
        itemName.text = item.Name;
        itemAmount.text = amount.ToString();
    }

    public void Clear()
    {
        itemName.text = "";
        itemAmount.text = "";
    }
}
