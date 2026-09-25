using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class ColourStation : Interactable
{
    enum States
    {
        Empty,
        Select,
        Working,
        Finished
    }

    States CurrentState = States.Empty;

    SkeletonAnimation Anim;

    void Start()
    {
        Anim = GetComponent<SkeletonAnimation>();
    }

    public override void Action()
    {
        if(CurrentState == States.Empty && MainChar.GetPlayerState() == PlayerState.Dough)
        {
            CurrentState = States.Select;
            MainChar.SetCarryState(PlayerState.Normal);

            Anim.AnimationState.SetAnimation(0, "Station_color/Stn_color_select_blue", false);
        }
        else if(CurrentState == States.Select)
        {
            CurrentState = States.Working;

            StartCoroutine(Work());
        }
        else if(CurrentState == States.Finished && MainChar.GetPlayerState() == PlayerState.Normal)
        {
            CurrentState = States.Empty;
            MainChar.SetCarryState(PlayerState.Mochi);

            Anim.AnimationState.SetAnimation(0, "Station_color/Stn_color_empty", false);
        }

        base.Action();
    }

    IEnumerator Work()
    {
        Anim.AnimationState.SetAnimation(0, "Station_color/Stn_color_working", true);

        yield return new WaitForSeconds(5);

        CurrentState = States.Finished;
        Anim.AnimationState.SetAnimation(0, "Station_color/Stn_color_finished_blue", false);
    }
}
