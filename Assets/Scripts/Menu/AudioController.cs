using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [Header("Audio source")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    public static AudioController Instance;
    public float BGMVolume => bgmSource.volume;
    public float SFXVolume => sfxSource.volume;
    
    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Awake()
    {
        if (Instance != null || Instance != this)
        {
            Destroy(Instance);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    public void PlaySFXSound(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayBGMSound(AudioClip clip)
    {
        if (bgmSource == null || clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        bgmSource.loop = true;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void SetBGMVolume(Slider slider)
    {
        SetVolume(bgmSource, slider.value);
        PlayerPrefs.SetFloat(BGMVolumeKey, slider.value);
    }

    public void SetSFXVolume(Slider slider)
    {
        SetVolume(sfxSource, slider.value);
        PlayerPrefs.SetFloat(SFXVolumeKey, slider.value);
    }

    private void SetVolume(AudioSource audioSource, float volume)
    {
        if(audioSource == null) return;
        audioSource.volume = volume;
    }

    private void LoadSettings()
    {
        var bgmVolume = PlayerPrefs.GetFloat(BGMVolumeKey, 1f);
        SetVolume(bgmSource, bgmVolume);
        var sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
        SetVolume(sfxSource, sfxVolume);
    }
}
