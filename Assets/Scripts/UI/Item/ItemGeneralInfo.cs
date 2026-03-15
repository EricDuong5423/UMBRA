using DG.Tweening;
using TMPro;
using UnityEngine;

public class ItemGeneralInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemAmount;
    [Header("Animation")]
    [SerializeField] private float punchScale = 0.15f;
    [SerializeField] private float punchDuration = 0.4f;
    [SerializeField] private float countUpDuration = 0.6f;
    private Tween countTween;
    public void Setup(ItemData item, int amount)
    {
        if (item == null)
        {
            Clear();
            return;
        }
        Clear();
        itemName.text = item.Name;
        itemName.transform.DOPunchScale(Vector3.one * punchScale, punchDuration, 4, 0.5f);
        itemAmount.text = "0";
        countTween = DOVirtual.Float(0, amount, countUpDuration, value =>
        {
            itemAmount.text = Mathf.FloorToInt(value).ToString();
        }).SetEase(Ease.OutCubic);
    }
    public void Clear()
    {
        countTween?.Kill();
        itemName.text = "";
        itemAmount.text = "";
        itemName.transform.localScale = Vector3.one;
    }
}
