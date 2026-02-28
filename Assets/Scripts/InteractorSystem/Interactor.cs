using UnityEngine;

public class Interactor : MonoBehaviour
{
    private IInteractable currentInteractableObject;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractableObject != null)
        {
            if (currentInteractableObject.CanInteract())
            {
                currentInteractableObject.Interact();
            }
        }
    }

    public void SetInteractable(IInteractable interactable)
    {
        currentInteractableObject = interactable;
    }

    public void RemoveInteractable()
    {
        currentInteractableObject = null;
    }
}
