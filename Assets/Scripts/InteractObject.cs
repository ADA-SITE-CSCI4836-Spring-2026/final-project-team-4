using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public interface IInteractable 
{
    void Interact(); 
    void OnHover();    
    void OnHoverExit();
}

public class InteractObject : MonoBehaviour 
{
    public float interactionDistance = 8f; 
    public LayerMask InteractableLayer;
    public GameObject interactPrompt;
    private IInteractable currentTarget;

    void Update()
    {
        if (PauseManager.IsPaused || GameManager.IsGameOver)
        {
            if (currentTarget != null)
            {
                currentTarget.OnHoverExit();
                currentTarget = null;
            }

            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            return;
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);

        if (Physics.Raycast(ray, out hit, interactionDistance, InteractableLayer)) 
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                if (interactable != currentTarget)
                {
                    currentTarget?.OnHoverExit(); 
                    interactable.OnHover();
                    currentTarget = interactable;
                }

                if (interactPrompt != null)
                    interactPrompt.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                    interactable.Interact();
            }
        }
        else if (currentTarget != null)
        {
            currentTarget.OnHoverExit();
            currentTarget = null;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }
}