using Spine.Unity;
using System.Collections;
using UnityEngine;

public class GrillStation : Interactable
{
    enum States
    {
        Empty,
        Cooking,
        Finished
    }

    States CurrentState = States.Empty;

    SkeletonAnimation Anim;

    protected override void Start()
    {
        base.Start();

        Anim = GetComponent<SkeletonAnimation>();
    }

    public override void Action()
    {
        if(CurrentState == States.Empty && MainChar.GetPlayerState() == PlayerState.Carry && MainChar.GetMochiState() == MochiState.Round)
        {
            CurrentState = States.Cooking;
            MainChar.SetPlayerState(PlayerState.Normal);

            StopAllCoroutines();
            StartCoroutine(Cook());
        }
        else if(CurrentState == States.Cooking)
        {
            CurrentState = States.Empty;
            MainChar.SetPlayerState(PlayerState.Carry);
            MainChar.SetMochiState(MochiState.Round);

            Anim.AnimationState.SetAnimation(0, "Station_grill/Stn_grill_empty", false);

            StopAllCoroutines();
        }
        else if(CurrentState == States.Finished && MainChar.GetPlayerState() == PlayerState.Normal)
        {
            CurrentState = States.Empty;
            MainChar.SetPlayerState(PlayerState.Carry);
            MainChar.SetMochiState(MochiState.Grilled);

            Anim.AnimationState.SetAnimation(0, "Station_grill/Stn_grill_empty", false);
        }

        base.Action();
    }

    IEnumerator Cook()
    {
        Anim.AnimationState.SetAnimation(0, "Station_grill/Stn_grill_cooking", true);

        yield return new WaitForSeconds(5);

        CurrentState = States.Finished;
        Anim.AnimationState.SetAnimation(0, "Station_grill/Stn_grill_finish", false);
    }
}
