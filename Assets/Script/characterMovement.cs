using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class characterMovement : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;

    // variable to store the instance of the PlayerInput
    PlayerInput input;

    // variables to store player input values
    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;
    
    // Awake is called when the script instance is being loaded
    void Awake()
    {
        input = new PlayerInput();

        // set the player input values using listeners
        input.CharacterControl.Move.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        input.CharacterControl.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set the animator reference
        animator = GetComponent<Animator>();

        // set the ID references
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleRotation();
    }
    void handleRotation()
    {
        // Current position of our character
        Vector3 currentPosition = transform.position;

        // the change in position our character should point to
        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);

        // combine the positions to give a position to look at
        Vector3 positionToLookAt = currentPosition + newPosition;

        // rotate the character to face the positionToLookAt
        transform.LookAt(positionToLookAt);
    }
    void handleMovement()
    {
        // get parameter values from animator
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        // if player not walking and pressing w key
        if (!isWalking && movementPressed)
        {
            // then set isWalking to be true
            animator.SetBool(isWalkingHash, true);
        }

        // if player is walking and not pressing w key
        if (isWalking && !movementPressed)
        {
            // then set isWalking to be false
            animator.SetBool(isWalkingHash, false);
        }

        // if player not running, pressing w key and left shift
        if (!isRunning && (movementPressed && runPressed))
        {
            // then set isRunning to be true
            animator.SetBool(isRunningHash, true);
        }

        // if player is running and not walking or pressing left shift
        if (isRunning && (!movementPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
            Debug.Log("cc");
        }
    }
    void OnEnable()
    {
        // enable the character controls action map
        input.CharacterControl.Enable();
    }
    void OnDisable()
    {
        // disable the character controls action map
        input.CharacterControl.Disable();
    }
}
