using Spine.Unity;
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

    PlayerState State = PlayerState.Normal;

    public float MovementSpeed = 3;

    Interactable CurrentInteractable;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    InputAction interactAction;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");
        interactAction = InputSystem.actions.FindAction("Interact");
    }
    void Update()
    {
        Vector2 movement = moveAction.ReadValue<Vector2>();
        movement *= Time.deltaTime * MovementSpeed;
        transform.Translate(movement);

        if (jumpAction.WasPressedThisFrame())
        {
            if (CurrentInteractable != null)
                CurrentInteractable.Action();
        }
    }

    public static PlayerState GetPlayerState()
    {
        return Instance.State;
    }

    public static void SetPlayerInteractable(Interactable interactable)
    {
        Instance.CurrentInteractable = interactable;
    }

    public static void ClearPlayerInteractable()
    {
        Instance.CurrentInteractable = null;
    }

    public static void StartMochiCarry()
    {
        Instance.State = PlayerState.Carrying;
        Instance.transform.Find("mochi").gameObject.SetActive(true);
    }
}
