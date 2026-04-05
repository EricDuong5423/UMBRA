using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> enemyAttackSFX;
    [SerializeField] private List<AudioClip> enemyHitSFX;
    [SerializeField] private List<AudioClip> enemyDeathSFX;
    
    private AudioController audioController;

    private void Start()
    {
        audioController = AudioController.Instance;
    }

    public void PlaySpecificEnemyAttackSFX(int index)
    {
        if (audioController == null) return;
        audioController.PlaySFXSound(enemyAttackSFX[index]);
    }

    public void PlayEnemyAttackSFX()
    {
        AudioClip randomAudioClip = enemyAttackSFX[Random.Range(0, enemyAttackSFX.Count)];
        if (audioController == null) return;
        audioController.PlaySFXSound(randomAudioClip);
    }

    public void PlayEnemyHitSFX()
    {
        AudioClip randomAudioClip = enemyHitSFX[Random.Range(0, enemyHitSFX.Count)];
        if (audioController == null) return;
        audioController.PlaySFXSound(randomAudioClip);
    }

    public void PlayEnemyDeathSFX()
    {
        AudioClip randomAudioClip = enemyDeathSFX[Random.Range(0, enemyDeathSFX.Count)];
        if (audioController == null) return;
        audioController.PlaySFXSound(randomAudioClip);
    }
}
