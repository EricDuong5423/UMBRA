using System;
using DG.Tweening;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[RequireComponent(typeof(BoxCollider2D))]
public class ItemPickUp : MonoBehaviour
{
    private ItemData itemData;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private BoxCollider2D col;
    
    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true; 
        col.enabled = false; 
    }

    public void Setup(ItemData data)
    {
        itemData = data;
        if (spriteRenderer != null && itemData != null)
        {
            spriteRenderer.sprite = itemData.Icon;
        }
        PlayDropAnimation();
    }
    
    private void PlayDropAnimation()
    {
        Vector3 originalScale = transform.localScale;
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 0.2f; 
        Vector3 targetPosition = transform.position + (Vector3)randomOffset;
        transform.localScale = originalScale * 0.01f; 
        transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack);
        transform.DOJump(targetPosition, jumpPower: 1.5f, numJumps: 1, duration: 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => 
            {
                col.enabled = true;
                transform.DOMoveY(transform.position.y + 0.2f, 1f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
            });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && itemData != null)
        {
            ItemManager playerInventory =  other.GetComponent<ItemManager>();
            if (playerInventory != null)
            {
                playerInventory.AddItem(itemData);
                transform.DOKill();
                Destroy(gameObject);
            }
        }
    }
}
