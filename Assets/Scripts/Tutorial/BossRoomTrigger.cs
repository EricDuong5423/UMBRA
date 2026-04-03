using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class BossRoomTrigger : MonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPoint;
    
    [Header("Cinematic camera")]
    [SerializeField] private PlayableDirector  cinematicDirector;
    [SerializeField] private CinemachineCamera bossCamera;
    [SerializeField] private float cinematicDuration = 3f;
    
    [Header("Boss health bar")]
    [SerializeField] private BossHealthBarUI bossHealthBarUI;
    private bool hasTriggerd = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggerd)
        {
            hasTriggerd = true;
            GetComponent<Collider2D>().enabled = false;

            StartCoroutine(SpawnAndCinematicRoutine());
        }
    }

    private IEnumerator SpawnAndCinematicRoutine()
    {
        PlayerManager.Instance.PlayerController.DisablePlayerControl();
        PlayerManager.Instance.PlayerMovement.StopMoving();
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);

        if (bossCamera != null)
        {
            bossCamera.Follow =  boss.transform;
        }
        cinematicDirector.Play();
        bossHealthBarUI.Setup(boss.GetComponent<EnemyBase>());
        yield return new WaitForSeconds(cinematicDuration);
        yield return new WaitForSeconds(1.5f);
        PlayerManager.Instance.PlayerController.EnablePlayerControl();
    }
}
