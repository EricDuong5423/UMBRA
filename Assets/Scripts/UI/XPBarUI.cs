using TMPro;
using UnityEngine;

public class XPBarUI : ResourceBarUI
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expAmount;
    [Header("Data Connection")]
    [SerializeField] private StatsManager statsManager;

    private void Start()
    {
        if (statsManager != null)
        {
            statsManager.OnExpChanged += UpdateExpView;
            
            UpdateExpView(statsManager.CurrentLevel, statsManager.CurrentExp, statsManager.ExpToNextLevel);
        }
    }
    
    private void OnDestroy()
    {
        if (statsManager != null)
        {
            statsManager.OnExpChanged -= UpdateExpView;
        }
    }

    private void UpdateExpView(int currentLevel, int currentExp, int expToNextLevel)
    {
        UpdateView(currentExp, expToNextLevel);
        if (levelText != null)
        {
            levelText.text = currentLevel.ToString();
        }

        if (expAmount != null)
        {
            expAmount.text = $"{currentExp} exp/{expToNextLevel} exp";
        }
    }
}
