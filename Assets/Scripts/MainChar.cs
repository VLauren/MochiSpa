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
    Fill,
    Beans,
    Circles,
    Triangles
}

public enum MochiColour
{
    Regular,
    Blue,
    Red,
    Yellow
}

public class MainChar : ZSortedObject
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

    Vector2 Movement = Vector2.zero;
    string PreviousDirectionAnim = "ch_3_4_front_right";

    SkeletonAnimation CharacterAnim;
    string CurrentAnimationName;

    void Awake()
    {
        Instance = this;
    }
    protected override void Start()
    {
        base.Start();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");
        interactAction = InputSystem.actions.FindAction("Interact");

        CharacterAnim = Instance.transform.Find("CharacterSpine").GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        Movement = moveAction.ReadValue<Vector2>() * MovementSpeed;
        transform.Translate(Movement * Time.deltaTime);

        if (jumpAction.WasPressedThisFrame() || attackAction.WasPressedThisFrame() || interactAction.WasPressedThisFrame())
        {
            if (CurrentInteractable != null)
                CurrentInteractable.Action();
        }

        Instance.UpdateMochiAnimation();

        string targetAnimName = GetCharacterAnimationName();
        if(!targetAnimName.Equals(CurrentAnimationName))
        {
            CharacterAnim.AnimationState.SetAnimation(0, targetAnimName, true);
            CurrentAnimationName = targetAnimName;
        }

        int order = UpdateSortingOrder();
        Instance.transform.Find("Mochies").GetComponent<Renderer>().sortingOrder = order + 1;

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
        string type = MochiState == MochiState.Beans || MochiState == MochiState.Circles || MochiState == MochiState.Triangles ? "Fills_" : "Mochis_";
        string move = (!_forceIdle && Movement.sqrMagnitude > 0) ? "run/Run" : "idle/Idle";

        string state;
        if (MochiState == MochiState.Beans)
            state = "_fill_beans";
        else if (MochiState == MochiState.Circles)
            state = "_fill_round";
        else if (MochiState == MochiState.Triangles)
            state = "_fill_triangle";
        else if (MochiState == MochiState.Dough)
            state = "_dough";
        else if (MochiState == MochiState.Round)
            state = "_round";
        else if (MochiState == MochiState.Freezed)
            state = "_round_freeze";
        // if (MochiState == MochiState.Grilled)
        else
            state = "_round_grilled";

        string colour;
        if (MochiColour == MochiColour.Regular || MochiState == MochiState.Dough || MochiState == MochiState.Beans)
            colour = "";
        else if (MochiColour == MochiColour.Blue)
            colour = "_blue";
        else if (MochiColour == MochiColour.Red)
            colour = "_red";
        // if (MochiColour == MochiColour.Yellow)
        else
            colour = "_yellow";

        return type + move + state + colour;
    }

    string GetCharacterAnimationName()
    {
        string movement, state, direction;

        if (State == PlayerState.Carry)
            state = "holding/";
        else // if(State == PlayerState.Normal)
            state = "empty/";


        if (Movement.magnitude / MovementSpeed < 0.2f)
        {
            movement = "ch_idle_";
            direction = PreviousDirectionAnim;
        }
        else
        {
            movement = "ch_run_";

            Vector2 normalizedMovement = Movement.normalized;
            if (normalizedMovement.x > 0.25f && normalizedMovement.y > 0.25f) // arriba derecha
                direction = "ch_3_4_back_right";
            else if (normalizedMovement.x > 0.75f) // derecha
                direction = "ch_side_right";
            else if (normalizedMovement.x > 0.25f && normalizedMovement.y < -0.25f) // abajo derecha
                direction = "ch_3_4_front_right";
            else if (normalizedMovement.y > 0.75f) // arriba
                direction = "ch_back";
            else if (normalizedMovement.y < -0.75f) // abajo
                direction = "ch_front";
            else if (normalizedMovement.x < -0.25f && normalizedMovement.y > 0.25f) // arriba izquierda
                direction = "ch_3_4_back_left";
            else if (normalizedMovement.x < -0.75f) // izquierda
                direction = "ch_side_left";
            else // if(movement.x < 0.25f && movement.y < 0.25f) // abajo izquierda
                direction = "ch_3_4_front_left";

            PreviousDirectionAnim = direction;
        }

        return movement + state + direction;
    }
}
