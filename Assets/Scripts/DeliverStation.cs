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

    void Start()
    {
        Anim = GetComponent<SkeletonAnimation>();
    }

    public override void Action()
    {
        if(CurrentState == States.Empty && MainChar.GetPlayerState() == PlayerState.Mochi)
        {
            StartCoroutine(DeliverRoutine());
        }

        base.Action();
    }

    public IEnumerator DeliverRoutine()
    {
        CurrentState = States.Delivered;
        MainChar.SetCarryState(PlayerState.Normal);

        transform.Find("mochi").gameObject.SetActive(true);

        yield return new WaitForSeconds(5);

        CurrentState = States.Empty;
        transform.Find("mochi").gameObject.SetActive(false);
    }

}
