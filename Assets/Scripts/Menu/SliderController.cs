using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    private AudioController audioController;
    [SerializeField] private Slider slider;
    [SerializeField] private string type;
    void Start()
    {
        audioController = AudioController.Instance;
        if (type.Equals("BGM"))
        {
            slider.value = audioController.BGMVolume;
        }
        else
        {
            slider.value = audioController.SFXVolume;
        }
    }
}
