using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScene : MonoBehaviour
{
    [SerializeField] private Image backgroundCoverImage;
    [SerializeField] private TMP_Text deathText;
    [SerializeField] private RectTransform returnButton;
    [SerializeField] private Ease ease;
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private string MenuScene = "MainMenu";
    
    private PlayerManager _playerManager;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
    


    private void Start()
    {
        returnButton.anchoredPosition = new Vector2(returnButton.anchoredPosition.x, -Screen.height * 1.5f);
        _playerManager = PlayerManager.Instance;
        if (_playerManager == null) return;
        _playerManager.PlayerHealth.OnDeath += HandleDeathUI;
    }

    private void OnDestroy()
    {
        _playerManager.PlayerHealth.OnDeath -= HandleDeathUI;
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         returnButton.anchoredPosition = new Vector2(returnButton.anchoredPosition.x, -Screen.height * 1.5f);
    //         deathText.text = String.Empty;
    //         var color = backgroundCoverImage.color;
    //         color.a = 0;
    //         backgroundCoverImage.color = color;
    //         HandleDeathUI();
    //     }
    // }

    private void HandleDeathUI()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        var sequence1 = DOTween.Sequence();
        var sequence2 = DOTween.Sequence();
        var animationBackground = backgroundCoverImage.DOFade(0.8f, duration).SetEase(ease);
        var animationDeathTextTweener = DOTween.To(() => String.Empty, x => deathText.text = x, "Game Over", duration).SetEase(ease); 
        var animationButton = returnButton.DOAnchorPosY(-170, duration).SetEase(ease);
        sequence1
            .Append(animationBackground)
            .Join(animationDeathTextTweener);
        sequence2
            .Append(sequence1)
            .Append(animationButton).Play();
    }

    public void HandleReturnButton()
    {
        SceneManager.LoadScene(MenuScene);
    }
}
