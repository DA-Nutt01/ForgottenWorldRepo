using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

public static PlayerInteraction Instance { get; private set; }

    [Header("Component References"), Space(5)]
    [SerializeField] private Camera m_Cam;
    [SerializeField] private float m_InteractionRange = 2f;
    [SerializeField] private LayerMask m_InteractionLayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            Debug.Log("Can't Have More Than One Copy Of Player Interaction!");
        }
    }
    public void TryInteract()
    {
        Debug.Log("Trying to interact");
        // Shoot a ray from the camera forward the interaction range
        Ray interactionRay = new Ray(m_Cam.transform.position, m_Cam.transform.forward);

        // If the interaction ray hits an object within interaction range on the interaction layer...
        if (Physics.Raycast(interactionRay, out RaycastHit hit, m_InteractionRange, m_InteractionLayer))
        {
            // We have hit the interactble
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable != null){
                interactable.Interact();
            }
        }
    }
}
