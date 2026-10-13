using Spine.Unity;
using System.Collections;
using UnityEngine;

public class FillStation : Interactable
{
    SkeletonAnimation Anim;

    void Start()
    {
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
        Anim.AnimationState.SetAnimation(0, "Station_fill/Stn_fill_beans_empty", false);

        MainChar.SetPlayerState(PlayerState.Carry);
        MainChar.SetMochiState(MochiState.Beans);

        yield return new WaitForSeconds(2);

        Anim.AnimationState.SetAnimation(0, "Station_fill/Stn_fill_beans_full", false);

    }
}
