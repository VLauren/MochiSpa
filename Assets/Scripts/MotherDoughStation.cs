using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class MotherDoughStation : Interactable
{
    enum States
    {
        Empty,
        Full
    }

    States CurrentState = States.Empty;

    void Start()
    {
        SkeletonAnimation skel = GetComponent<SkeletonAnimation>();
        skel.AnimationState.Event += OnAnimationEvent;
    }
    public override void Action()
    {
        if(CurrentState == States.Empty)
        {
            if(MainChar.GetPlayerState() == PlayerState.Normal)
            {
                CurrentState = States.Full;

                SkeletonAnimation skel = GetComponent<SkeletonAnimation>();
                Spine.TrackEntry entry = skel.AnimationState.SetAnimation(0, "Station_start/Start_bowl_fill_up", false);
            }
        }
        else if(CurrentState == States.Full)
        {
            if(MainChar.GetPlayerState() == PlayerState.Normal)
            {
                CurrentState = States.Empty;

                SkeletonAnimation skel = GetComponent<SkeletonAnimation>();
                Spine.TrackEntry entry = skel.AnimationState.SetAnimation(0, "Station_start/Start_bowl_empty", false);

                MainChar.SetCarryState(PlayerState.Dough);
            }
        }

        base.Action();
    }
    private void OnAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        print(e.Data.Name);
    }
}
