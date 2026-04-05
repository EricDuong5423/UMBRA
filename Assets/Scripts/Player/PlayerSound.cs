using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> playerStepsInDirtClip;
    [SerializeField] private List<AudioClip> playerStepsInTilesClip;
    [SerializeField] private List<AudioClip> playerAttack;
    [SerializeField] private List<AudioClip> playerHit;
    [SerializeField] private List<AudioClip> playerHeal;
    [SerializeField] private AudioClip playerDeath;

    public void playDeathSound()
    {
        if(AudioController.Instance == null) return;
        AudioController.Instance.PlaySFXSound(playerDeath);
    }

    public void playDirtStepsSound()
    {
        AudioClip randomAudioClip = playerStepsInDirtClip[Random.Range(0, playerStepsInDirtClip.Count)];
        if(AudioController.Instance == null) return;
        AudioController.Instance.PlaySFXSound(randomAudioClip);
    }

    public void playAttackSound()
    {
        AudioClip randomAudioClip = playerAttack[Random.Range(0, playerAttack.Count)];
        if(AudioController.Instance == null) return;
        AudioController.Instance.PlaySFXSound(randomAudioClip);
    }

    public void playHitSound()
    {
        AudioClip randomAudioClip = playerHit[Random.Range(0, playerHit.Count)];
        if(AudioController.Instance == null) return;
        AudioController.Instance.PlaySFXSound(randomAudioClip);
    }

    public void PlayHealSound()
    {
        AudioClip randomAudioClip = playerHeal[Random.Range(0, playerHeal.Count)];
        if(AudioController.Instance == null) return;
        AudioController.Instance.PlaySFXSound(randomAudioClip);
    }
}
