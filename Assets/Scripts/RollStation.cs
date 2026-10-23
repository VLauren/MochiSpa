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
    SkeletonAnimation BellAnim;

    string Colour = "blue";
    string Fill = "beans";

    protected override void Start()
    {
        base.Start();

        Anim = GetComponent<SkeletonAnimation>();
        BellAnim = transform.Find("Bell").GetComponent<SkeletonAnimation>();

        var bellRender = transform.Find("Bell").GetComponent<Renderer>();
        bellRender.sortingOrder = GetComponent<Renderer>().sortingOrder + 1;
    }

    public override void Action()
    {
        if(CurrentState == States.Empty && MainChar.GetPlayerState() == PlayerState.Carry && MainChar.GetMochiState() == MochiState.Round)
        {
            CurrentState = States.Full;
            MainChar.SetPlayerState(PlayerState.Normal);

            Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_full_" + Colour, false);
        }
        else if(CurrentState == States.Full && MainChar.GetPlayerState() == PlayerState.Normal)
        {
            CurrentState = States.Working;

            StartCoroutine(Roll());
        }
        else if(CurrentState == States.Flat && MainChar.GetPlayerState() == PlayerState.Carry 
            && (MainChar.GetMochiState() == MochiState.Beans || MainChar.GetMochiState() == MochiState.Circles || MainChar.GetMochiState() == MochiState.Triangles ))
        {
            CurrentState = States.Filled;
            MainChar.SetPlayerState(PlayerState.Normal);

            Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_"+ Colour + "_filled_" + Fill, false);
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
        Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_working_" + Colour, false);

        yield return new WaitForSeconds(1);

        CurrentState = States.Flat;
        Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_finish_" + Colour, false);
    }
    IEnumerator Close()
    {
        BellAnim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_bell_start", false);
        transform.Find("Bell").gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        BellAnim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_bell_working", true);
        Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_round", false);

        yield return new WaitForSeconds(1);

        BellAnim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_bell_end", false);
        CurrentState = States.Finished;

        // Anim.AnimationState.SetAnimation(0, "Station_roller/Stn_roll_round", false);
    }
}
