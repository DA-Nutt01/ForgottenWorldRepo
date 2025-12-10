using UnityEngine;
using UnityEngine.InputSystem;


/*
 * HOW TO DECLARE C# VARIBALES (SYNTAX)
 * Accessor, DataType, Name, AssingmentOp, Value
 * 
 * Acessors - public, private, protected
 * DataType - int, float, bool, str, UnityClasses*
 * Name - Str*
 * Assignment Operator --> = 
 * Value - Depends on data type of variable
 * 
 * EX. private int points = 0;
 * EX. private Rigidbody rb; 
 * EX. public bool isGameOver = false;
 */
 

[DefaultExecutionOrder(100)]
//collects and manage input from the player
public class InputManager : MonoBehaviour
{
    //only this script can maage this variable
    public static InputManager Instance {  get; private set; }
    //attribute variable makes private variable public
    [Header("Components"), Space(5)]
    [SerializeField] private InputActionAsset m_InputActions;
    [SerializeField] private Rigidbody m_RigidBody;
    // Create a private vvar of type GameObject named m_GroundDetector, do not assign it a valu 
    [SerializeField] private GameObject m_GroundDetector;

    [Header("Configuration")]
    [SerializeField] float m_GroundDetectionRadius = .25f;
    [SerializeField] private LayerMask m_GroundLayer;

    [Header("Player Settings"), Space(5)]
    //player input actions (stores player action variables)
    private InputAction m_MoveAction;
    private InputAction m_LookAction;
    private InputAction m_JumpAction;

    private Vector2 m_MoveAmount;
    private Vector2 m_LookAmount;
    private float m_VericalRotation = 0f; 
    
    [SerializeField] GameObject m_Cam;
    [SerializeField] private float m_WalkSpeed = 5f;
    [SerializeField] private float m_JumpHeight = 10f;
    [SerializeField] private float m_LookSpeed = 1f;
    [SerializeField] private float m_VerticalRotationLimit = 80f; 


    private void OnEnable()
    {
        //when the script is enabled (triggered once)
        m_InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        //when the script is enabled (triggered once)
        m_InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            Debug.Log("Can't Have More Than One Copy!");
        }

        //initilize variables
        m_MoveAction = m_InputActions.FindAction("Move");
        m_LookAction = m_InputActions.FindAction("Look");
        m_JumpAction = m_InputActions.FindAction("Jump");

        m_RigidBody = GetComponent<Rigidbody>();
    }

    private void Start() {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Hide the cursor
        //Cursor.visible = false;
    }

    private void Update()
    {
        // Read the movement and look values from the action
        m_MoveAmount = m_MoveAction.ReadValue<Vector2>();
        
        m_LookAmount = m_LookAction.ReadValue<Vector2>();

        if (m_JumpAction.WasPressedThisFrame()){
            HandleJump();
        }
    }

    private void FixedUpdate(){
        // Using the input to move the camera and player
        HandleMovement();
        HandleLooking();

    }

    private void HandleJump(){
        Debug.Log("Trying To Jump");
        // Check if the player is on the ground
        // Step 1: Use Physics.Overlap sphere to see if the player's feet is touching the ground
        Collider[] arry = Physics.OverlapSphere(m_GroundDetector.transform.position, m_GroundDetectionRadius, m_GroundLayer);

        if (arry.Length > 0){ // The player is on the ground
            // Make the player jump based on the jump height
            m_RigidBody.AddForce(Vector3.up * m_JumpHeight, ForceMode.Impulse);
            Debug.Log("Succesful Jump");
        } else {
            Debug.Log("Jump Failed");
        }

    
        // Detect when the player is on the ground
        // To double jump, player must be off the ground, has not already double jumped, & the jump button pressed
    }

    private void HandleMovement(){
        // Move the player based on the movement input and walk speed
        m_RigidBody.MovePosition(m_RigidBody.position + transform.forward * m_MoveAmount.y * m_WalkSpeed * Time.fixedDeltaTime +
        transform.right * m_MoveAmount.x * m_WalkSpeed * Time.fixedDeltaTime);
    }   

    private void HandleLooking(){
        // Rotate the camera based on the look input
        transform.Rotate(Vector3.up * m_LookAmount.x * m_LookSpeed);

        // Calculate vertical rotation
        m_VericalRotation -= m_LookAmount.y * m_LookSpeed;
        m_VericalRotation = Mathf.Clamp(m_VericalRotation, -m_VerticalRotationLimit, m_VerticalRotationLimit);

        //Apply vertical rotation to camera only
        m_Cam.transform.localRotation = Quaternion.Euler(m_VericalRotation, 0f, 0f);
    }

    private void OnDrawGizmos(){
         // Set the color with custom alpha
        Gizmos.color = new Color(1f, 0f, 0f); // Red with custom alpha

        Gizmos.DrawWireSphere(m_GroundDetector.transform.position, m_GroundDetectionRadius);
    }
}
