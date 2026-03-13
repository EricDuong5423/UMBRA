using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private ItemManager playerInventory;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private ItemSlotUI itemSlotPrefab;
    [SerializeField] private Button EnableIventoryButton;
    [SerializeField] private Ease AnimationEase;
    [SerializeField] private float AnimationSpeed = 1f;

    private Dictionary<ItemData, ItemSlotUI> spawnedSlot = new Dictionary<ItemData, ItemSlotUI>();

    private void Start()
    {
        if (playerInventory != null) playerInventory.OnInvetoryChanged += HandleItemChanged;
    }

    private void OnDestroy()
    {
        if (playerInventory != null) playerInventory.OnInvetoryChanged -= HandleItemChanged;
    }

    private void HandleItemChanged(ItemData item, int currentStack)
    {
        if (spawnedSlot.ContainsKey(item))
        {
            spawnedSlot[item].Setup(item, currentStack);
        }
        else
        {
            ItemSlotUI newSlot = Instantiate(itemSlotPrefab, itemContainer);
            newSlot.Setup(item, currentStack);
            spawnedSlot.Add(item, newSlot);
        }
    }

    public void TurnOffInventory()
    {
        transform.DOMoveY(transform.position.y + 210f, AnimationSpeed)
                 .SetEase(AnimationEase)
                 .OnComplete(EnableButton)
                 .Play();
    }

    public void TurnOnInventory()
    {
        var sequence = DOTween.Sequence();
        var animation = transform.DOMoveY(transform.position.y - 210f, AnimationSpeed);
        sequence.Append(animation)
                .JoinCallback(DisableButton)
                .SetEase(AnimationEase)
                .Play();
    }

    private void EnableButton()
    {
        EnableIventoryButton.gameObject.SetActive(true);
    }

    private void DisableButton()
    {
        EnableIventoryButton.gameObject.SetActive(false);
    }
}
