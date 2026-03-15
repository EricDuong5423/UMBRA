using DG.Tweening;
using TMPro;
using UnityEngine;

public class TypewriterTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private float duration = 0.5f;

    private Tween descriptionTween;

    public void Play(string text)
    {
        Clear();

        if (string.IsNullOrEmpty(text)) return;

        textComponent.text = text;
        textComponent.ForceMeshUpdate();
        int totalChars = textComponent.textInfo.characterCount;
        textComponent.maxVisibleCharacters = 0;

        descriptionTween = DOVirtual.Float(0, totalChars, duration, value =>
        {
            textComponent.maxVisibleCharacters = Mathf.FloorToInt(value);
        }).SetEase(ease).Play();
    }

    public void Clear()
    {
        descriptionTween?.Kill();
        if (textComponent != null)
            textComponent.text = "";
    }
}