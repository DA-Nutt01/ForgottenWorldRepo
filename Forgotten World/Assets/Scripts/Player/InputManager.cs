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
    [SerializeField] private Rigidbody m_RigBody;

    //player input actions (stores player action variables)
    private InputAction m_MoveAction;
    private InputAction m_LookAction;
    private InputAction m_JumpAction;

    private Vector2 m_MoveAmount;
    private Vector2 m_LookAmount;

    [SerializeField] private float m_WalkSpeed = 5f;
    [SerializeField] private float m_JumpHeight = 10f;


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
    }

    private void Update()
    {
        //step 1: read the movement values from the action
        m_MoveAmount = m_MoveAction.ReadValue<Vector2>();
        //step 2: take the input and then apply the input to move the player to the desired place
        Debug.Log(m_MoveAmount);
    }
}
