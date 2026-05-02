using UnityEngine;

public class CoinPickUp : MonoBehaviour, IInteractable
{
    public Outline outlineScript;

    void Start()
    {
        if (outlineScript != null)
            outlineScript.enabled = false;
    }

    public void Interact()
    {
        GameManager.Instance.ShowMessage("Money won't buy you time!");
        Destroy(gameObject);
    }

    public void OnHover()
    {
        if (outlineScript != null)
            outlineScript.enabled = true;
    }

    public void OnHoverExit()
    {
        if (outlineScript != null)
            outlineScript.enabled = false;
    }
}