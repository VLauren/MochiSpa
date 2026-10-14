using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class DeliverStation : Interactable
{
    enum States
    {
        Empty,
        Delivered
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
        if(CurrentState == States.Empty && MainChar.GetPlayerState() == PlayerState.Carry)
        {
            StartCoroutine(DeliverRoutine());
        }

        base.Action();
    }

    public IEnumerator DeliverRoutine()
    {
        CurrentState = States.Delivered;
        MainChar.SetPlayerState(PlayerState.Normal);

        GameObject mochi = transform.Find("Mochies").gameObject;

        mochi.SetActive(true);
        mochi.GetComponent<Renderer>().sortingOrder = UpdateSortingOrder() + 1;
        SkeletonAnimation skel = transform.Find("Mochies").GetComponent<SkeletonAnimation>();
        skel.AnimationState.SetAnimation(0, MainChar.GetMochiDeliverAnimation(), true);

        yield return new WaitForSeconds(5);

        CurrentState = States.Empty;
        transform.Find("Mochies").gameObject.SetActive(false);
    }

}
