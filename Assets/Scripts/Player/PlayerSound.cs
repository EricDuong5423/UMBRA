using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> playerStepsInDirtClip;
    [SerializeField] private List<AudioClip> playerStepsInTilesClip;
    [SerializeField] private List<AudioClip> playerAttack;

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
}
