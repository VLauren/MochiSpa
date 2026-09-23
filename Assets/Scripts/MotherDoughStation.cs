using Spine.Unity;
using UnityEngine;

public class MotherDoughStation : Interactable
{
    enum States
    {
        Empty,
        Full
    }

    States CurrentState = States.Empty;

    public override void Action()
    {
        if(CurrentState == States.Empty)
        {
            if(MainChar.GetPlayerState() == PlayerState.Normal)
            {
                CurrentState = States.Full;

                SkeletonAnimation skel = GetComponent<SkeletonAnimation>();
                // skel.AnimationName = "Station_start/Start_bowl_full";
                skel.AnimationName = "Station_start/Start_bowl_fill_up";
            }
        }

        base.Action();
    }
}
