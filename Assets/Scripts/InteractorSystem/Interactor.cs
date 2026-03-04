using System;
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
                interactIcon.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            currentInteractableObject =  interactable;
            interactIcon.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable) && interactable == currentInteractableObject)
        {
            currentInteractableObject = null;
            interactIcon.SetActive(false);
        }
    }
}
