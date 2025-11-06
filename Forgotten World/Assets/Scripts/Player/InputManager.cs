using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //only this script can maage this variable
    public static InputManager Instance {  get; private set; }
    //attribute variable makes private variable public
    [SerializeField]
    private InputActionAsset m_InputActions;

    private void Awake()
    {
        if (Instance == null && Instance != this) //this refers to this specific script
        {
            Instance = this;
            Debug.Log("Instance Variable Assigned!");
        }
        else
        {
            Destroy(this);
            Debug.Log("Can't Have More Than One Copy!");
        }
    }
}
