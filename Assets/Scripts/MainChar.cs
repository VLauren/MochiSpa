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

    public float MovementSpeed = 3;

    Interactable CurrentInteractable;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    InputAction interactAction;

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

            SkeletonAnimation skel = GameObject.FindAnyObjectByType<SkeletonAnimation>();
            skel.AnimationName = "Station_color/Stn_color_empty";
        }

        if (attackAction.WasPressedThisFrame())
        {
            SkeletonAnimation skel = GameObject.FindAnyObjectByType<SkeletonAnimation>();
            skel.AnimationName = "Station_start/Start_bowl_lvl_2";
        }

        if (interactAction.WasPressedThisFrame())
        {
            SkeletonAnimation skel = GameObject.FindAnyObjectByType<SkeletonAnimation>();
            skel.AnimationName = "Station_color/Stn_color_working";
        }
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
