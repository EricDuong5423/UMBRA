using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopupUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image exampleImage;
    [SerializeField] private Image stepIcon;
    [SerializeField] private GameObject keyCodeContainer;
    [SerializeField] private TMP_Text keyCodeText;
    
    [Header("Effect settings")]
    [SerializeField] private float textDuration = 0.5f;
    [SerializeField] private Ease textEase = Ease.OutBack;

    private TutorialStepData currentStep;
    private Tween textTween;
    private Sequence textSequence;
    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
    
    private void Update()
    {
        if (currentStep == null) return;
        if (currentStep.keyToPress != KeyCode.None && Input.GetKeyDown(currentStep.keyToPress))
            TutorialManager.Instance?.DismissCurrentStep(currentStep);
    }

    public void Show(TutorialStepData stepData)
    {
        currentStep = stepData;
        textSequence?.Kill();
        //Title
        if (titleText == null && descriptionText == null) return;
        titleText.text = stepData.Title;
        titleText.ForceMeshUpdate();
        int totalChars = titleText.textInfo.characterCount;
        titleText.maxVisibleCharacters = 0;
        textSequence = DOTween.Sequence();
        textTween = textSequence.Append(DOVirtual.Float(0, totalChars, textDuration, value =>
        {
            titleText.maxVisibleCharacters = Mathf.FloorToInt(value);
        }));
        //Description
        descriptionText.text = stepData.Description;
        descriptionText.ForceMeshUpdate();
        int totalDescChars = descriptionText.textInfo.characterCount;
        descriptionText.maxVisibleCharacters = 0;
        textTween = textSequence.Append(DOVirtual.Float(0, totalDescChars, textDuration, value =>
        {
            descriptionText.maxVisibleCharacters = Mathf.FloorToInt(value);
        }));
        textSequence.SetEase(textEase).SetUpdate(true).Play();
        //Example Image
        if (exampleImage != null)
        {
            exampleImage.gameObject.SetActive(stepData.ExampleImage != null);
            if (stepData.ExampleImage != null) exampleImage.sprite = stepData.ExampleImage;
        }

        if (stepIcon != null)
        {
            stepIcon.gameObject.SetActive(stepData.Icon != null);
            if (stepData.Icon != null) stepIcon.sprite = stepData.Icon;
        }
        // Press key to continue
        if (keyCodeContainer == null) return;
        bool hasKey = stepData.keyToPress != KeyCode.None;
        keyCodeContainer.SetActive(hasKey);
        if (hasKey && keyCodeText != null)
        {
            keyCodeText.text = $"Press [{stepData.keyToPress}] to continue";
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1f, 0.3f).SetUpdate(true);
    }

    public void Hide()
    {
        currentStep = null;
        textSequence?.Kill();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0f, 0.2f).SetUpdate(true);
    }
}
