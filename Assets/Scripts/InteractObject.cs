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
        // 1. Aim from the camera center
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Visual debug laser matching the actual interaction distance
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);

        // 2. ONLY shoot the laser as far as your interaction distance (No more 100f!)
        if (Physics.Raycast(ray, out hit, interactionDistance, InteractableLayer)) 
    {
        IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            // If we look at a NEW object, tell the old one to stop highlighting[cite: 8]
            if (interactable != currentTarget)
            {
                currentTarget?.OnHoverExit(); 
                interactable.OnHover();
                currentTarget = interactable;
            }

            if (interactPrompt != null) interactPrompt.SetActive(true); // Signifier

            if (Input.GetKeyDown(KeyCode.E)) interactable.Interact();
        }
    }
    else if (currentTarget != null)
    {
        // We looked away from everything, turn off the highlight[cite: 8]
        currentTarget.OnHoverExit();
        currentTarget = null;
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }
    }
}