using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Normal,
    Carry,
    Pushing
}

public enum MochiState
{
    Dough,
    Round,
    Freezed,
    Grilled,
    Fill
}

public enum MochiColour
{
    Regular,
    Blue,
    Red,
    Yellow
}

public class MainChar : MonoBehaviour
{
    public static MainChar Instance { get; private set; }

    PlayerState State = PlayerState.Normal;
    MochiState MochiState = MochiState.Dough;
    MochiColour MochiColour = MochiColour.Regular;

    public float MovementSpeed = 3;

    Interactable CurrentInteractable;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    InputAction interactAction;

    Vector2 movement = Vector2.zero;

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
        movement = moveAction.ReadValue<Vector2>();
        movement *= Time.deltaTime * MovementSpeed;
        transform.Translate(movement);

        if (jumpAction.WasPressedThisFrame())
        {
            if (CurrentInteractable != null)
                CurrentInteractable.Action();
        }

        Instance.UpdateMochiAnimation();
    }

    public static PlayerState GetPlayerState()
    {
        return Instance.State;
    }
    public static MochiState GetMochiState()
    {
        return Instance.MochiState;
    }

    public static void SetPlayerInteractable(Interactable interactable)
    {
        Instance.CurrentInteractable = interactable;
    }

    public static void ClearPlayerInteractable()
    {
        Instance.CurrentInteractable = null;
    }

    public static void SetPlayerState(PlayerState state)
    {
        Instance.State = state;

        if (state == PlayerState.Normal)
        {
            Instance.transform.Find("Mochies").gameObject.SetActive(false);
        }
        else if (state == PlayerState.Carry)
        {
            Instance.transform.Find("Mochies").gameObject.SetActive(true);
        }
        else if (state == PlayerState.Pushing)
        {
            // TODO
        }

        Instance.UpdateMochiAnimation();
    }

    public static void SetMochiState(MochiState state)
    {
        Instance.MochiState = state;
        Instance.UpdateMochiAnimation();
    }

    public static void SetMochiColour(MochiColour state)
    {
        Instance.MochiColour = state;
        Instance.UpdateMochiAnimation();
    }

    void UpdateMochiAnimation()
    {
        SkeletonAnimation skel = Instance.transform.Find("Mochies").GetComponent<SkeletonAnimation>();
        string anim = GetMochiAnimationName();

        if(skel.AnimationName != anim)
            skel.AnimationState.SetAnimation(0, anim, true);
    }

    public static string GetMochiDeliverAnimation()
    {
        return Instance.GetMochiAnimationName(true);
    }

    string GetMochiAnimationName (bool _forceIdle = false)
    {
        string move = (!_forceIdle && movement.sqrMagnitude > 0) ? "Mochis_run/Run" : "Mochis_idle/Idle";

        string state;
        if (MochiState == MochiState.Dough)
            state = "_dough";
        else if (MochiState == MochiState.Round)
            state = "_round";
        else if (MochiState == MochiState.Freezed)
            state = "_round_freeze";
        // if (MochiState == MochiState.Grilled)
        else
            state = "_round_grilled";

        string colour;
        if (MochiColour == MochiColour.Regular || MochiState == MochiState.Dough)
            colour = "";
        else if (MochiColour == MochiColour.Blue)
            colour = "_blue";
        else if (MochiColour == MochiColour.Red)
            colour = "_red";
        // if (MochiColour == MochiColour.Yellow)
        else
            colour = "_yellow";

        return move + state + colour;
    }
}
