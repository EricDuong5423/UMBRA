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
        switch (type)
        {
            case "BGM":
            {
                slider.value = audioController.BGMVolume;
                break;
            }
            case "SFX":
            {
                slider.value = audioController.SFXVolume;
                break;
            }
            case "Enviroment":
            {
                slider.value = audioController.EnviromentVolume;
                break;
            }
        }
    }
}
