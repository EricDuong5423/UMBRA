using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField] private Image backgroundCoverImage;
    [SerializeField] private TMP_Text endText;
    [SerializeField] private Ease ease;
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform returnButton;
    [SerializeField] private string MenuScene = "MainMenu";
    
    private VampireBoss vampireBoss;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void Start()
    {
        returnButton.anchoredPosition = new Vector2(returnButton.anchoredPosition.x, -Screen.height * 1.5f);
    }

    private void Update()
    {
        if (VampireBoss.Instance != null && vampireBoss == null)
        {
            vampireBoss = VampireBoss.Instance;
            vampireBoss.HealthSystem.OnDeath += HandleBossDeath;
        }
    }

    private void OnDestroy()
    {
        vampireBoss.HealthSystem.OnDeath -= HandleBossDeath;
    }

    private void HandleBossDeath()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        var sequence1 = DOTween.Sequence();
        var sequence2 = DOTween.Sequence();
        var animationBackground = backgroundCoverImage.DOFade(0.8f, duration).SetEase(ease);
        var animationDeathTextTweener = DOTween.To(() => String.Empty, x => endText.text = x, "Game Over", duration).SetEase(ease);
        var animationButton = returnButton.DOAnchorPosY(-170, duration).SetEase(ease);
        sequence1
            .Append(animationBackground)
            .Join(animationDeathTextTweener)
            .SetUpdate(true)
            .Play();
        sequence2
            .Append(sequence1)
            .Append(animationButton)
            .SetUpdate(true)
            .Play();
    }

    public void HandleReturnButton()
    {
        SceneManager.LoadScene(MenuScene);
    }
}
