using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ItemDescriptionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private float duration = 0.5f;
    
    private Tween descriptionTween;

    public void Setup(ItemData item, int amount)
    {
        if (item == null || string.IsNullOrEmpty(item.Description))
        {
            Clear();
            return;
        }
        
        Clear();
        itemDescription.text = item.Description;
        itemDescription.ForceMeshUpdate();
        int totalChars = itemDescription.textInfo.characterCount;
        itemDescription.maxVisibleCharacters = 0;
        descriptionTween = DOVirtual.Float(0, totalChars, duration, currentValue => 
        {
            itemDescription.maxVisibleCharacters = Mathf.FloorToInt(currentValue);
        }).SetEase(ease);
    }

    public void Clear()
    {
        descriptionTween?.Kill();
        itemDescription.text = "";
    }
}
