using System;
using UnityEngine;

public class TutorialZone : MonoBehaviour
{
    [Header("Data")] 
    [SerializeField] private TutorialStepData tutorialStep;

    [SerializeField] private bool triggerOnce = true;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (triggerOnce && hasTriggered) return;

        hasTriggered = true;
        TutorialManager.Instance?.ShowStep(tutorialStep);
    }
}
