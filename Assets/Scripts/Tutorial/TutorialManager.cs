using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    [SerializeField] private TutorialPopupUI popupUI;
    [SerializeField] private List<TutorialZone> _tutorialZones = new List<TutorialZone>();

    private Coroutine autoDismissCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowStep(TutorialStepData stepData)
    {
        if(stepData == null || popupUI == null) return;
        
        if(stepData.PauseGameWhileShowing) GameManager.Instance.PauseGame();
        
        popupUI.Show(stepData);
        if (autoDismissCoroutine != null) StopCoroutine(autoDismissCoroutine);

        if (stepData.CloseAfterSecond > 0f)
        {
            autoDismissCoroutine = StartCoroutine(AutoDismiss(stepData));
        }
    }

    public void DismissCurrentStep(TutorialStepData stepData)
    {
        if(stepData.PauseGameWhileShowing) GameManager.Instance.ResumeGame();
        popupUI?.Hide();
        if (autoDismissCoroutine != null) 
        {
            StopCoroutine(autoDismissCoroutine);
            autoDismissCoroutine = null;
        }
    }

    private IEnumerator AutoDismiss(TutorialStepData stepData)
    {
        yield return new WaitForSecondsRealtime(stepData.CloseAfterSecond);
        DismissCurrentStep(stepData);
    }
}
