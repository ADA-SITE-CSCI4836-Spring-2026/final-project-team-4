using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour, IInteractable 
{

    // Drag the QuickOutline component (from the asset you download) here
    public Outline outlineScript; 

    void Start()
    {
        if (outlineScript != null) outlineScript.enabled = false;
    }

    public void Interact() 
    {
        Debug.Log("Coin Picked Up!");
        Destroy(gameObject); 
    }

    // Called when the laser hits the object[cite: 8]
    public void OnHover() 
    {
        if (outlineScript != null) outlineScript.enabled = true;
    }

    // Called when the laser leaves the object[cite: 8]
    public void OnHoverExit() 
    {
        if (outlineScript != null) outlineScript.enabled = false;
    }
}
