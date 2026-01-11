using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance { get; private set; }

    [Header("Component References"), Space(5)]
    [SerializeField] private Rigidbody m_RigidBody;
    [SerializeField] private GameObject m_GroundDetector;
    [SerializeField] private CapsuleCollider m_Collider;

    [Header("Configuration"), Space(5)]
    [SerializeField] float m_GroundDetectionRadius = .25f;
    [SerializeField] private LayerMask m_GroundLayer;
    
    
    [SerializeField] GameObject m_Cam;
    [SerializeField] private float m_WalkSpeed = 5f;
    [SerializeField] private float m_JumpHeight = 10f;
    [SerializeField] private float m_LookSpeed = 1f;
    [SerializeField] private float m_VerticalRotationLimit = 80f; 
    [SerializeField][Tooltip("The num of times that player has jumped without touching the ground. Resets once player touches ground")]
    private int m_JumpCount = 0;
    [SerializeField][Tooltip("The max num of times the player can consecutive")]
    private int m_JumpCap = 2;
    
    private Vector2 m_MoveAmount;
    private Vector2 m_LookAmount;
    private float m_VericalRotation = 0f; 
    private bool m_IsSprinting = false;
    [SerializeField] private bool m_isCrouched = false;
    private float m_StandHeight = 2f;
    private float m_CrouchHeight = 1.2f;
    
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            Debug.Log("Can't Have More Than One Copy Of Player Movement!");
        }

        // Initialize Members
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<CapsuleCollider>();
        m_isCrouched = false;
        m_JumpCount = 0;
    }

    private void Update()
    {
       if (m_isCrouched)
        {
            m_Collider.height = m_CrouchHeight;
        } else
        {
            m_Collider.height = m_StandHeight;
        }
    } 

    public void HandleLooking(Vector2 lookAmount){
        // Rotate the camera based on the look input
        transform.Rotate(Vector3.up * lookAmount.x * m_LookSpeed);

        // Calculate vertical rotation
        m_VericalRotation -= lookAmount.y * m_LookSpeed;
        m_VericalRotation = Mathf.Clamp(m_VericalRotation, -m_VerticalRotationLimit, m_VerticalRotationLimit);

        //Apply vertical rotation to camera only
        m_Cam.transform.localRotation = Quaternion.Euler(m_VericalRotation, 0f, 0f);
    }

    public void HandleMovement(Vector2 moveAmount){
    // Move the player based on the movement input and walk speed
    m_RigidBody.MovePosition(m_RigidBody.position + transform.forward * moveAmount.y * m_WalkSpeed * Time.fixedDeltaTime +
    transform.right * moveAmount.x * m_WalkSpeed * Time.fixedDeltaTime);
    }   


    public void HandleJump(){
        if (m_isCrouched) return; // Skip this function if the player is crouched

        Debug.Log("Trying To Jump");
        // Check if the player is on the ground
        // Step 1: Use Physics.Overlap sphere to see if the player's feet is touching the ground
        Collider[] arry = Physics.OverlapSphere(m_GroundDetector.transform.position, m_GroundDetectionRadius, m_GroundLayer);

        if (IsGrounded()){ // The player is on the ground
            // Make the player jump based on the jump height
            m_JumpCount = 0; // Reset the jump count
            m_RigidBody.AddForce(Vector3.up * m_JumpHeight, ForceMode.Impulse);
            m_JumpCount ++;
            Debug.Log("Succesful Jump");
        } else if (!IsGrounded()){
            // Check if the player is able to double jump
            if (m_JumpCount < m_JumpCap){
                // Double Jump
                m_RigidBody.AddForce(Vector3.up * m_JumpHeight, ForceMode.Impulse);
                m_JumpCount ++;
                Debug.Log("Succesful Double-Jump");
            } 
        } else {
            Debug.Log("Jump Failed");
        }

    
        // Detect when the player is on the ground
        // To double jump, player must be off the ground, has not already double jumped, & the jump button pressed
    }

    private void OnDrawGizmos(){
         // Set the color with custom alpha
        Gizmos.color = new Color(1f, 0f, 0f); // Red with custom alpha

        Gizmos.DrawWireSphere(m_GroundDetector.transform.position, m_GroundDetectionRadius);
    }

    public void SetSprinting(bool isSprinting){
        if (m_isCrouched) return; // Skip this function if the player is crouched
        if (!IsGrounded()) return; // Skip this function if the player is airborn

        m_IsSprinting = isSprinting;
        if (m_IsSprinting){
            m_WalkSpeed = 10f; // Sprint Speed
        } else {
            m_WalkSpeed = 5f; // Normal Speed
        }
    }

    public void ToggleCrouched(){
        m_isCrouched = !m_isCrouched;

         if (m_isCrouched){
            m_WalkSpeed = 3f; // Sprint Speed
        } else {
            m_WalkSpeed = 5f; // Normal Speed
        }
    }

    private bool IsGrounded()
    {
        // Check if the player is on the ground
        // Step 1: Use Physics.Overlap sphere to see if the player's feet is touching the ground
        Collider[] arry = Physics.OverlapSphere(m_GroundDetector.transform.position, m_GroundDetectionRadius, m_GroundLayer);
        if (arry.Length > 0) return true;
        return false;
    }
}
