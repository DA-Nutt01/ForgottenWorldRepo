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
   // [SerializeField] private Rigidbody m_RigidBody;
    // Create a private vvar of type GameObject named m_GroundDetector, do not assign it a valu 
   // [SerializeField] private GameObject m_GroundDetector;

    // [Header("Configuration")]
    // [SerializeField] float m_GroundDetectionRadius = .25f;
    // [SerializeField] private LayerMask m_GroundLayer;

    [Header("Player Settings"), Space(5)]
    //player input actions (stores player action variables)
    private InputAction m_MoveAction;
    private InputAction m_LookAction;
    private InputAction m_JumpAction;

    // private Vector2 m_MoveAmount;
    // private Vector2 m_LookAmount;
    // private float m_VericalRotation = 0f; 
    
    // [SerializeField] GameObject m_Cam;
    // [SerializeField] private float m_WalkSpeed = 5f;
    // [SerializeField] private float m_JumpHeight = 10f;
    // [SerializeField] private float m_LookSpeed = 1f;
    // [SerializeField] private float m_VerticalRotationLimit = 80f; 


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

        // m_RigidBody = GetComponent<Rigidbody>();
    }

    private void Start() {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Hide the cursor
        //Cursor.visible = false;
    }

    private void Update()
    {
        if (m_JumpAction.WasPressedThisFrame()){
           // Tell PlayerMovement to jump
           PlayerMovement.Instance.HandleJump();
        }
    }

    private void FixedUpdate(){
        // Read the movement and look values from the action
        Vector2 moveAmount = m_MoveAction.ReadValue<Vector2>();
        Vector2 lookAmount = m_LookAction.ReadValue<Vector2>();

        // Using the input to move the camera and player
        PlayerMovement.Instance.HandleMovement(moveAmount);
        PlayerMovement.Instance.HandleLooking(lookAmount);
    }
}
