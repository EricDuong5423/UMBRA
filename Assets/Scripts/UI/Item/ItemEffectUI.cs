using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ItemEffectUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemEffectDescription;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private float duration = 0.5f;
    
    private Tween descriptionTween;
    private string description;

    public void Setup(ItemData item, int amount)
    {
        if (item == null || string.IsNullOrEmpty(item.Description))
        {
            Clear();
            return;
        }
        
        Clear();
        itemEffectDescription.text = item.Description;
        itemEffectDescription.ForceMeshUpdate();
        int totalChars = itemEffectDescription.textInfo.characterCount;
        itemEffectDescription.maxVisibleCharacters = 0;
        descriptionTween = DOVirtual.Float(0, totalChars, duration, currentValue => 
        {
            itemEffectDescription.maxVisibleCharacters = Mathf.FloorToInt(currentValue);
        }).SetEase(ease);
    }

    public void Clear()
    {
        descriptionTween?.Kill();
        description = "";
        itemEffectDescription.text = "";
    }
}
