using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private string type;

    private void Start()
    {
        AudioController audio = AudioController.Instance;
        if (audio == null) return;
        switch (type)
        {
            case "BGM":         slider.value = audio.BGMVolume; break;
            case "SFX":         slider.value = audio.SFXVolume; break;
            case "Enviroment":  slider.value = audio.EnviromentVolume; break;
        }
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        AudioController audio = AudioController.Instance;
        if (audio == null) return;

        switch (type)
        {
            case "BGM":         audio.SetBGMVolume(slider); break;
            case "SFX":         audio.SetSFXVolume(slider); break;
            case "Enviroment":  audio.SetEnviromentVolume(slider); break;
        }
    }
}