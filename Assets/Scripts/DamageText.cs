using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    
    [Header("Animation Settings")]
    [SerializeField] private float startScale = 1f;
    [SerializeField] private float jumpHeight = 0.5f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease = Ease.OutQuad;
    
    [Header("Random Offset")]
    [SerializeField, Range(-1f, 0f)] private float xMin = -0.5f;
    [SerializeField, Range(0f, 1f)] private float xMax = 0.5f;

    private Transform target;
    private Vector3 randomOffset;
    private Vector3 lastKnownPosition;
    private float animatedY;

    public void SetData(string value, Color color, Transform targetTransform)
    {
        text.text = value;
        text.color = color;
        target = targetTransform;
        float randomX = Random.Range(xMin, xMax);
        randomOffset = new Vector3(randomX, 0f, 0f);
        animatedY = 0f;
        if (target != null) lastKnownPosition = target.position;
        transform.localScale = Vector3.one * startScale;
        var moveTween = DOTween.To(() => animatedY, y => animatedY = y, jumpHeight, duration);
        var fadeTween = text.DOFade(0f, duration);
        var scaleTween = transform.DOScale(0f, duration);
        
        var sequence = DOTween.Sequence();
        sequence
            .Append(moveTween)
            .Join(fadeTween)
            .Join(scaleTween)
            .SetEase(ease)
            .OnComplete(SelfDestroy)
            .Play();
    }

    private void LateUpdate()
    {
        if (target != null && target.gameObject.activeInHierarchy)
        {
            lastKnownPosition = target.position;
        }
        
        transform.position = lastKnownPosition + randomOffset + new Vector3(0, animatedY, 0);
    }

    private void SelfDestroy()
    {
        DamageTextManager.Instance.ReturnToPool(this.gameObject);
    }
}