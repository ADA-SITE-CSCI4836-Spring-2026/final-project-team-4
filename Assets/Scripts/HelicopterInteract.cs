using UnityEngine;

public class HelicopterInteract : MonoBehaviour, IInteractable
{
    public Outline outlineScript;

    void Start()
    {
        if (outlineScript != null) outlineScript.enabled = false;
    }

    public void Interact()
    {
        if (GameManager.Instance.HasAllItems())
            GameManager.Instance.WinGame();
        else
            GameManager.Instance.ShowHelicopterMessage();
    }

    public void OnHover()
    {
        if (outlineScript != null) outlineScript.enabled = true;
    }

    public void OnHoverExit()
    {
        if (outlineScript != null) outlineScript.enabled = false;
    }
}