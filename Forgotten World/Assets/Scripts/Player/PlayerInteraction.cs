using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{

public static PlayerInteraction Instance { get; private set; }

    [Header("Component References"), Space(5)]
    [SerializeField] private Camera m_Cam;
    [SerializeField] private float m_InteractionRange = 2f;
    [SerializeField] private LayerMask m_InteractionLayer;
    [SerializeField] private BaseInteractable m_CurrentLookTarget; // Current interactable the player is looking at
    [SerializeField] private GameObject m_PromptUI;
    [SerializeField] private TextMeshProUGUI  m_PromptText;

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

        m_PromptUI.SetActive(false);
        m_PromptText = null;
    }

    private void Update(){
        UpdateLookTarget();
    }

    private void UpdateLookTarget()
    {
        // Checks what interactable object the player is looking at each frame and upates the current look target field

        // Shoot a ray from the camera forward the interaction range
        Ray interactionRay = new Ray(m_Cam.transform.position, m_Cam.transform.forward);

        // If the interaction ray hits an object within interaction range on the interaction layer...
        if (Physics.Raycast(interactionRay, out RaycastHit hit, m_InteractionRange, m_InteractionLayer))
        {
            // We have hit the interactble
            BaseInteractable interactable = hit.collider.GetComponentInParent<BaseInteractable>();

            if (interactable != m_CurrentLookTarget){
                m_PromptUI.SetActive(true);
                m_CurrentLookTarget = interactable;
                m_PromptText.text =  m_CurrentLookTarget?.GetPromptText();
            } else {
                m_PromptUI.SetActive(false);
                m_CurrentLookTarget = null;
            }
        }
    }

    public void TryInteract()
    {
        m_CurrentLookTarget?.Interact();
    }
}
