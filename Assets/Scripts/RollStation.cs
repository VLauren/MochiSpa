using Spine.Unity;
using System.Collections;
using UnityEngine;

public class RollStation : Interactable
{
    enum States
    {
        Empty,
        Full,
        Working,
        Flat,
        Filled,
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
            CurrentState = States.Full;
            MainChar.SetPlayerState(PlayerState.Normal);

            Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_full", false);
        }
        else if(CurrentState == States.Full && MainChar.GetPlayerState() == PlayerState.Normal)
        {
            CurrentState = States.Working;

            StartCoroutine(Roll());
        }
        else if(CurrentState == States.Flat && MainChar.GetPlayerState() == PlayerState.Carry && MainChar.GetMochiState() == MochiState.Beans)
        {
            CurrentState = States.Filled;
            MainChar.SetPlayerState(PlayerState.Normal);

            Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_filled_beans", false);
        }
        else if(CurrentState == States.Filled)
        {
            CurrentState = States.Working;

            StartCoroutine(Close());
        }
        else if(CurrentState == States.Finished && MainChar.GetPlayerState() == PlayerState.Normal)
        {
            CurrentState = States.Empty;
            MainChar.SetPlayerState(PlayerState.Carry);
            MainChar.SetMochiState(MochiState.Round);
            MainChar.SetMochiColour(MochiColour.Yellow);

            Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_empty", false);
        }

        base.Action();
    }

    IEnumerator Roll()
    {
        Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_working", false);

        yield return new WaitForSeconds(1);

        CurrentState = States.Flat;
        Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_finish", false);
    }
    IEnumerator Close()
    {
        Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_round", false);

        yield return new WaitForSeconds(1);

        CurrentState = States.Finished;
        Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_round", false);
    }
}
