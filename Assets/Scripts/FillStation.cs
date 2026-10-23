using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

public enum FillTypes
{
    Beans,
    Circles,
    Triangles
}

public class FillStation : Interactable
{
    public FillTypes FillType;

    SkeletonAnimation Anim;

    protected override void Start()
    {
        base.Start();

        Anim = GetComponent<SkeletonAnimation>();
    }

    public override void Action()
    {
        if(MainChar.GetPlayerState() == PlayerState.Normal)
        {
            StartCoroutine(Work());
        }
    }

    IEnumerator Work()
    {
        string typeString = FillType == FillTypes.Beans ? "beans" : (FillType == FillTypes.Circles ? "circle" : "triangles");
        Anim.AnimationState.SetAnimation(0, "Station_fill/Stn_fill_" + typeString + "_empty", false);

        MainChar.SetPlayerState(PlayerState.Carry);
        MainChar.SetMochiState((MochiState)Enum.Parse(typeof(MochiState), FillType.ToString()));

        yield return new WaitForSeconds(2);

        Anim.AnimationState.SetAnimation(0, "Station_fill/Stn_fill_" + typeString + "_full", false);

    }
}
