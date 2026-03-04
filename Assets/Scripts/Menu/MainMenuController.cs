using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // public static event Action<AudioClip> OnSceneLoaded;

    [SerializeField] private string TutorialMap = "Tutorial-map";
    [SerializeField] private AudioClip tutorialMapClip;
    [SerializeField] private AudioClip mainMenuClip;
    [SerializeField] private List<AudioClip> clickButtonClip;
    [SerializeField] private AudioClip enviromentClip;

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene(TutorialMap);
        PlayClickedButtonSound();
        AudioController.Instance.PlayBGMSound(tutorialMapClip);
        PlayEnviromentButtonSound();
    }

    public void PlayClickedButtonSound()
    {
        AudioClip randomAudioClip = clickButtonClip[UnityEngine.Random.Range(0, clickButtonClip.Count)];
        AudioController.Instance.PlaySFXSound(randomAudioClip);
    }

    private void PlayEnviromentButtonSound()
    {
        AudioController.Instance.PlayEnviromentSound(enviromentClip);
    }

    private void Start()
    {
        AudioController.Instance.PlayBGMSound(mainMenuClip);
    }
}
