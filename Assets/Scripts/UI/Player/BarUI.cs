using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    public enum BarType { Health, Stamina }
    [SerializeField] private BarType barType;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Image fillImage;
    [Header("Animation")]
    [SerializeField] private float fillDuration = 0.4f;
    private Tween barTween;
    private Tween textTween;
    private PlayerHealth playerHealth;
    private StaminaSystem staminaSystem;
    
    public void Setup()
    {
        playerHealth = PlayerManager.Instance.PlayerHealth;
        staminaSystem = PlayerManager.Instance.PlayerStamina;
        if (barType == BarType.Health)
        {
            playerHealth.OnHealthChanged += OnValueChanged;
            fillImage.fillAmount = playerHealth.CurrentEmbers / playerHealth.MaxEmbers;
            amountText.text = $"{playerHealth.CurrentEmbers.ToString()}/{playerHealth.MaxEmbers.ToString()}";
        }
        else
        {
            staminaSystem.OnStaminaChanged += OnValueChanged;
            fillImage.fillAmount = staminaSystem.CurrentStamina / staminaSystem.MaxStamina;
            amountText.text = $"{staminaSystem.CurrentStamina.ToString()}/{staminaSystem.MaxStamina.ToString()}";
        }
    }
    private void OnValueChanged(float current, float max)
    {
        float targetFill = Mathf.Clamp01(current / max);
        barTween?.Kill();
        barTween = fillImage.DOFillAmount(targetFill, fillDuration)
            .SetEase(Ease.OutCubic);
        float fromValue = float.TryParse(amountText.text.Split('/')[0], out float parsed) ? parsed : current;
        textTween?.Kill();
        textTween = DOVirtual.Float(fromValue, current, fillDuration, value =>
        {
            amountText.text = $"{Mathf.RoundToInt(value)}/{Mathf.RoundToInt(max)}";
        }).SetEase(Ease.OutCubic);
    }
    private void OnDestroy()
    {
        if (playerHealth != null) playerHealth.OnHealthChanged -= OnValueChanged;
        if (staminaSystem != null) staminaSystem.OnStaminaChanged -= OnValueChanged;
    }
}
