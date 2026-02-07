using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static event Action<AudioClip> OnSceneLoaded;

    [SerializeField] private string TutorialMap = "Tutorial-map";
    [SerializeField] private AudioClip tutorialMapClip;
    [SerializeField] private AudioClip mainMenuClip;
    [SerializeField] private AudioClip clickButtonClip;

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene(TutorialMap);
        PlayClickedButtonSound();
        AudioController.Instance.PlayBGMSound(tutorialMapClip);
    }

    public void PlayClickedButtonSound()
    {
        AudioController.Instance.PlaySFXSound(clickButtonClip);
    }

    private void Start()
    {
        AudioController.Instance.PlayBGMSound(mainMenuClip);
    }
}
