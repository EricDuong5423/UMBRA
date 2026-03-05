using System;
using Unity.VisualScripting;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private IInteractable currentInteractableObject;
    [SerializeField] private GameObject interactIcon;

    void Start()
    {
        interactIcon.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractableObject != null)
        {
            if (currentInteractableObject.CanInteract())
            {
                currentInteractableObject.Interact();
                TurnOffInteractIcon();
            }
        }
    }

    public void TurnOnInteractIcon()
    {
        interactIcon.SetActive(true);
    }

    public void TurnOffInteractIcon()
    {
        interactIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            currentInteractableObject =  interactable;
            TurnOnInteractIcon();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable) && interactable == currentInteractableObject)
        {
            currentInteractableObject = null;
            TurnOffInteractIcon();
        }
    }
}
