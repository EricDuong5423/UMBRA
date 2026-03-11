using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float startScale;
    [SerializeField] private float startY;
    [SerializeField] private float endY;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;

    [SerializeField, Range(-1f, 0f)] private float xMin;
    [SerializeField, Range(0f, 1f)] private float xMax;

    private void Start()
    {
        var x = UnityEngine.Random.Range(xMin, xMax);
        rectTransform.anchoredPosition = new Vector2(x, startY);
        rectTransform.localScale = startScale * Vector3.one;
    }

    public void SetData(string value, Color color)
    {
        text.text = value;
        text.color = color;

        var moveTween = rectTransform.DOAnchorPosY(endY, duration);
        var fadeTween = text.DOFade(0f, duration);
        var colorTween = text.DOColor(Color.white, duration);
        var scaleTween = rectTransform.DOScale(0f, duration);
        var sequence = DOTween.Sequence();
        sequence
            .Append(moveTween)
            .Join(fadeTween)
            .Join(colorTween)
            .Join(scaleTween)
            .SetEase(ease)
            .OnComplete(SelfDestroy)
            .Play()
            ;
    }

    private void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}