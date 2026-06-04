using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Normal,
    Carrying,
    Pushing
}

public class MainChar : MonoBehaviour
{
    public static MainChar Instance { get; private set; }

    public float MovementSpeed = 3;

    Interactable CurrentInteractable;

    InputAction moveAction;
    InputAction jumpAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }
    void Update()
    {
        Vector2 movement = moveAction.ReadValue<Vector2>();
        movement *= Time.deltaTime * MovementSpeed;
        transform.Translate(movement);

        if (jumpAction.WasPressedThisFrame())
            if (CurrentInteractable != null)
                CurrentInteractable.Action();
    }

    public static void SetPlayerInteractable(Interactable interactable)
    {
        Instance.CurrentInteractable = interactable;
    }

    public static void ClearPlayerInteractable()
    {
        Instance.CurrentInteractable = null;
    }
}
