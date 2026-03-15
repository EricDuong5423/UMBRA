using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject gridParent;
    [SerializeField] private int maxSlots = 100;
    [SerializeField] private ItemDetails itemDetails;
    private ItemManager itemManager;
    public static InventoryUIManager Instance { get; private set; }
    private ItemSlotUI currentSelectedSlot;
    
    private List<ItemSlotUI> itemSlots = new List<ItemSlotUI>();
    [SerializeField] private float slotStaggerDelay = 0.05f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        itemManager = PlayerManager.Instance.PlayerItemManager;
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, gridParent.transform);
            itemSlot.transform.SetParent(gridParent.transform);
            ItemSlotUI itemSlotUI = itemSlot.GetComponent<ItemSlotUI>();
            itemSlotUI.ClearSlot();
            itemSlots.Add(itemSlotUI);
        }
        if (itemManager != null)
        {
            itemManager.OnInvetoryChanged += OnInventoryChangedEvent;
        }
    }

    public void OnClicked(ItemSlotUI selectedSlot)
    {
        if (selectedSlot == null || selectedSlot.currentItem == null) return;

        if (currentSelectedSlot == selectedSlot) return;

        if (currentSelectedSlot != null)
        {
            currentSelectedSlot.SetHighLight(false);
        }
        
        currentSelectedSlot = selectedSlot;
        currentSelectedSlot.SetHighLight(true);

        if (itemDetails != null)
        {
            itemDetails.Setup(currentSelectedSlot.currentItem, currentSelectedSlot.currentItemAmount);
        }
    }

    private void OnDestroy()
    {
        itemManager.OnInvetoryChanged -= OnInventoryChangedEvent;
    }
    
    private void OnInventoryChangedEvent(ItemData item, int count)
    {
        HandleUIChanged();
    }

    private void HandleUIChanged()
    {
        var itemList = itemManager.inventory.ToList();
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (i < itemList.Count)
            {
                float delay = i * slotStaggerDelay;
                itemSlots[i].UpdateSlot(itemList[i].Key, itemList[i].Value, delay);
            }
            else
            {
                itemSlots[i].ClearSlot();
            }
        }
    }
    
    public Color GetRarityColor(ItemRarity rarity)
    {
        switch (rarity)
        {
            case ItemRarity.Common: return Color.white;
            case ItemRarity.Uncommon: return Color.green;
            case ItemRarity.Rare: return Color.blue;
            case ItemRarity.Epic: return Color.magenta;
            case ItemRarity.Legendary: return new Color(1f, 0.5f, 0f);
            default: return Color.white;
        }
    }
}
