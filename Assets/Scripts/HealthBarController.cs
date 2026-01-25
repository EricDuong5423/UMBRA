using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image healthBarFrame;
    [SerializeField] private Image healthBarDecoration;
    [SerializeField] private Player player;
    [SerializeField] private Color lowHealthColor;
    [SerializeField] private Color fullHealthColor;
    private float ratio;

    private void Update()
    {
        //Tính toán ratio để biết phần trăm máu
        ratio = player.CurrentEmbers / player.PlayerStats.MaxEmbers;
        ratio = Mathf.Clamp01(ratio);
        
        //Tăng fill tùy thuộc vào ratio
        healthBarFill.fillAmount = ratio;
        
        //Thay đổi màu tùy thuộc vào ratio
        healthBarFill.color = Color.Lerp(lowHealthColor, fullHealthColor, ratio);
        healthBarFrame.color = Color.Lerp(lowHealthColor, fullHealthColor, ratio);
        healthBarDecoration.color = Color.Lerp(lowHealthColor, fullHealthColor, ratio);
    }
}
