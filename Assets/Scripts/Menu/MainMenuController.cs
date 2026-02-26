using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static event Action<AudioClip> OnSceneLoaded;

    [SerializeField] private string TutorialMap = "Tutorial-map";
    [SerializeField] private AudioClip tutorialMapClip;
    [SerializeField] private AudioClip mainMenuClip;
    [SerializeField] private List<AudioClip> clickButtonClip;

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene(TutorialMap);
        PlayClickedButtonSound();
        AudioController.Instance.PlayBGMSound(tutorialMapClip);
    }

    public void PlayClickedButtonSound()
    {
        AudioClip randomAudioClip = clickButtonClip[UnityEngine.Random.Range(0, clickButtonClip.Count)];
        AudioController.Instance.PlaySFXSound(randomAudioClip);
    }

    private void Start()
    {
        AudioController.Instance.PlayBGMSound(mainMenuClip);
    }
}
