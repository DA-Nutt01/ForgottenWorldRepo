using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Tooltip("Sensitivity multiplier for moving the camera around")]
    public float LookSensitivity = 1f;

    [Tooltip("Used to flip the vertical input axis")]
        public bool InvertYAxis = false;

    [Tooltip("Used to flip the horizontal input axis")]
    public bool InvertXAxis = false;

    PlayerCharacterController m_PlayerCharacterController;
    bool m_FireInputWasHeld;

    void Start()
    {
        m_PlayerCharacterController = GetComponent<PlayerCharacterController>();

        // Turn the cursor invisible & locks at the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        m_FireInputWasHeld = GetFireInputHeld();
    }

    /// <summary>
    /// Determines whether the application can process player input.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if the cursor is locked; otherwise, <c>false</c>.
    /// </returns>
    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    /// <summary>
    /// Retrieves the player's input as a vector3
    /// </summary>
    /// <returns>
    /// Returns a vector of the player's desired move direction if input can be proccessed
    /// Otherwiese returns Vector.0 if input cannot be processed
    /// </returns>
    public Vector3 GetMoveInput()
    {   
        if (CanProcessInput())
        {
            // Store X & Z keyboard input
            Vector3 moveVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            // Clamp move input to max of 1 to prevent diagonal movement from exceeding max move speed
            moveVector = Vector3.ClampMagnitude(moveVector, 1);

            return moveVector;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// Retrieves the horizontal mouse movement input, used for looking around or rotating the camera.
    /// </summary>
    /// <returns>
    /// A <see cref="float"/> representing the horizontal movement of the mouse:
    /// Positive values indicate movement to the right, negative values indicate movement to the left.
    /// Returns 0 if input cannot be processed.
    /// </returns>
    public float GetLookInputHorizontal()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse X");
        }

        return 0; 
    }

    /// <summary>
    /// Retrieves the vertical mouse movement input, used for looking around or rotating the camera.
    /// </summary>
    /// <returns>
    /// A <see cref="float"/> representing the vertical movement of the mouse:
    /// Positive values indicate movement up, negative values indicate movement down.
    /// Returns 0 if input cannot be processed.
    /// </returns>
    public float GetLookInputVertical()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse Y");
        }

        return 0; 
    }


    /// <summary>
    /// Checks if the jump input is pressed down during the current frame.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if the jump input is pressed down and input can be processed; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method utilizes Unity's legacy input system to detect the jump action, 
    /// mapped to the action name specified by <see cref="GameConstants.k_ButtonNameJump"/>.
    /// Ensure the input action is correctly configured in Unity's Input Manager.
    /// </remarks>
    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown("Jump");
        }

        return false;
    }

    /// <summary>
    /// Checks if the jump input is being held down during the current frame.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if the jump input is held down and input can be processed; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method utilizes Unity's legacy input system to detect the continuous state 
    /// of the jump action, mapped to the action name specified by <see cref="GameConstants.k_ButtonNameJump"/>.
    /// Ensure the input action is correctly configured in Unity's Input Manager.
    /// </remarks>
    public bool GetJumpInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetButton("Jump");
        }

        return false;
    }

    /// <summary>
    /// Checks if the fire input (left mouse button) is being held down during the current frame.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if the left mouse button is held down and input can be processed; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method uses Unity's legacy input system to detect the state of the left mouse button (button index 0).
    /// Ensure that input processing is enabled via the <see cref="CanProcessInput"/> method before calling this function.
    /// </remarks>
    public bool GetFireInputHeld()
    {
        if (CanProcessInput())
        {

            return Input.GetMouseButton(0);
        }

            return false;
    }

    /// <summary>
    /// Checks if the fire input (left mouse button) was pressed down during the current frame.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if the left mouse button was pressed down this frame, based on the current and previous input states; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method combines the current state of the fire input, determined by <see cref="GetFireInputHeld"/>, 
    /// with the previous state of the fire input (<c>m_FireInputWasHeld</c>) to detect a "fire down" event.
    /// Ensure <c>m_FireInputWasHeld</c> is updated correctly in the game's input handling logic.
    /// </remarks>
    public bool GetFireInputDown()
    {
        return GetFireInputHeld() && !m_FireInputWasHeld;
    }
    
    /// <summary>
    /// Checks if the fire input (left mouse button) was released during the current frame.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if the left mouse button was released this frame, based on the current and previous input states; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method detects a "fire release" event by combining the current state of the fire input, 
    /// determined by <see cref="GetFireInputHeld"/>, with the previous state of the fire input (<c>m_FireInputWasHeld</c>).
    /// Ensure <c>m_FireInputWasHeld</c> is updated correctly in the game's input handling logic.
    /// </remarks>
    public bool GetFireInputReleased()
    {
        return !GetFireInputHeld() && m_FireInputWasHeld;
    }


    /// <summary>
    /// Checks if the aim input (right mouse button) is being held down during the current frame.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if the right mouse button is held down and input can be processed; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method uses Unity's legacy input system to detect the state of the right mouse button (button index 1).
    /// Input processing is gated by the <see cref="CanProcessInput"/> method, which must return <c>true</c> for the aim input to be registered.
    /// </remarks>
    public bool GetAimInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButton(1);
        }

        return false;
    }

    /// <summary>
    /// Checks if the sprint input (left shift key) is being held down during the current frame.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if the left shift key is held down and input can be processed; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method uses Unity's legacy input system to detect the state of the left shift key (using <see cref="KeyCode.LeftShift"/>).
    /// Input processing is gated by the <see cref="CanProcessInput"/> method, which must return <c>true</c> for the sprint input to be registered.
    /// </remarks>
    public bool GetSprintInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        return false;
    }

    public bool GetCrouchInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.C);
        }

        return false;
    }

    public bool GetCrouchInputReleased()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyUp(KeyCode.C); // Detects when the C key is released.
        }

        return false;
    }

    public bool GetReloadButtonDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.R);
        }

        return false;
    }

    public int GetSwitchWeaponInput()
    {
        if (CanProcessInput())
        {
            if (Input.GetKeyDown(KeyCode.Q))
                return -1;
            else if (Input.GetKeyDown(KeyCode.E))
                return 1;
        }

        return 0;
    }


    /// <summary>
    /// Checks for key inputs to select a weapon based on the number keys (1-9).
    /// </summary>
    /// <returns>
    /// Returns an integer representing the selected weapon number (1-9) based on the key pressed.
    /// Returns <c>0</c> if no valid input is detected or input cannot be processed.
    /// </returns>
    /// <remarks>
    /// This method listens for key presses on the number keys 1 through 9. When a key is pressed,
    /// it returns the corresponding number (1-9). If no number key is pressed, it returns 0.
    /// Input processing is gated by the <see cref="CanProcessInput"/> method, which must return <c>true</c> 
    /// for the input to be considered valid.
    /// </remarks>
    public int GetSelectWeaponInput()
    {
        if (CanProcessInput())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                return 1;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                return 2;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                return 3;
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                return 4;
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                return 5;
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                return 6;
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                return 7;
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                return 8;
            else if (Input.GetKeyDown(KeyCode.Alpha9))
                return 9;
            else
                return 0;
            }

            return 0;
        }
}
