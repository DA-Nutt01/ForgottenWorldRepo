using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    // A base class every interactable will inherit
    [SerializeField] protected string m_PromptText;

    // Abstact functions MUST be implemented in their children classes
    public abstract void Interact();

    public virtual string GetPromptText(){
        return m_PromptText;
    }
}
